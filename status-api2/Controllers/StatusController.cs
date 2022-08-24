namespace StatusApi
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly ILogger<StatusController> _logger;
        private readonly RabbitSettings _settings;

        public StatusController(ILogger<StatusController> logger, IOptions<RabbitSettings> options)
        {
            _logger = logger;
            _settings = options.Value;
        }

        [HttpGet(Name = "GetStatus")]
        public async Task<ActionResult<RabbitOverview>> OverviewAsync()
        {
            try
            {
                var overview = await GetRabbitOverviewAsync();
                return new OkObjectResult(overview);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new BadRequestResult();
            }
        }

        private async Task<RabbitOverview> GetRabbitOverviewAsync()
        {
            var completeUri = new Uri(_settings.EndpointUrl);

            var bytes = Encoding.ASCII.GetBytes($"{_settings.Username}:{_settings.Password}");
            var token = Convert.ToBase64String(bytes);
            var auth = new AuthenticationHeaderValue("Basic", token);

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = auth;

            var response = await client.GetAsync(completeUri);

            var stream = await response.Content.ReadAsStreamAsync();

            var serializer = new DataContractJsonSerializer(typeof(RabbitOverview));
            return serializer.ReadObject(stream) as RabbitOverview;
        }
    }

    [DataContract]
    public class RabbitOverview
    {
        [DataMember(Name = "queue_totals")] public MessageCounts QueueStats { get; set; }
        [DataMember(Name = "message_stats")] public TotalCount TotalProcessed { get; set; }
    }

    [DataContract]
    public class MessageCounts
    {
        [DataMember(Name = "messages_ready")] public int Ready { get; set; }

        [DataMember(Name = "messages_unacknowledged")] public int Unacknowledged { get; set; }
    }

    [DataContract]
    public class TotalCount
    {
        [DataMember(Name = "ack")] public int Total { get; set; }
    }
}
