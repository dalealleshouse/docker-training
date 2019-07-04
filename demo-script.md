## Important Points
- The demo system is intended to demonstrate containerization only. Do not
    interpret any of the code as an example of canonical best practices.
- Don't try to memorize every detail of this demo. The intent is to demonstrate
    what is possible. It's not a comprehensive tutorial.

## The Demo System
The system consists of 5 micro-services
1. *Producer* implemented with Ruby - Every second, it produces a random number
   and pushes it as a message to *RabbitMQ*
1. *RabbitMQ* - Standard message broker with no custom configuration
1. *Consumer* implemented in Java - Subscribes to all messages from *RabbitMQ*
   and simulates a workload (for those who are curious, it produces Fibonacci
   numbers).
1. *Rest API* implemented in .NET core - Standard REST API that exposes stats
   from *RabbitMQ*
    - count of queued messaged
    - count of in-process messages (retrieved but not yet acknowledged)
    - count of acknowledged messages
1. *NGINX* - Serves a static HTML page that communicates with the *Rest API* via
   XHR and displays *RabbitMQ* stats.

## Docker
- Each micro service has a Docker file that specifies it's operating
    environment (container). *Open up a Dockerfile and walk through it*
    * The `FROM` instruction indicates a base container that will be downloaded
        from a public or private registry.
    * Dockerfiles are minuscule text files so they can be checked in/versioned
        with the codebase
- There is a single docker-compose.yml file that specifies how the containers
    communicate with one another. *Open up the docker-compose.yml file and walk
    through it*
- This is *Infrastructure as Code*
    * The same containers that run on the dev machine can be pushed to testing
        and production environments
    * Goodbye configuration management problems!!!
    * IT only needs to provide a container environment, devs have complete
        control over the applications.
    * It's now easy for devs to experiment with technologies without having to
        contaminate their machines. Removing the docker images eliminates all
        traces.

## Demo
- As a reminder:
    * None of the technologies used by the micro-services are installed on this
        machine.
    * All of the code required to run this demo is in the repo, consumers do not
        have to install anything other than docker.
- The following command will spin up a container for each of the microservices
    ``` bash
    > docker-compose up -d
    ```
    * Container spin up fast and have a very small footprint
- Navigate to `http://localhost/` and note the number of items in queue vs. the
    number in process.
- The following command will spin up 5 *Consumers* to help with processing the
    items in the queue.
    ``` bash
    > docker-compose scale java-consumer=5
    ```
    * The number of items in *In Progress* should climb to 5
    * Due to the innovation of UnionFS and Images, additional containers
        represent very little overhead



