# FROM mcr.microsoft.com/dotnet/sdk:6.0

# WORKDIR /app
# COPY ./ /app
# RUN dotnet restore
# RUN dotnet publish -c Release -o out
 
# ENV ASPNETCORE_URLS http://*:5000
# EXPOSE 5000/tcp
 
# ENTRYPOINT ["dotnet", "out/status-api.dll"]

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
    
COPY *.csproj ./
RUN dotnet restore
    
# Copy everything else and build
COPY ./ ./
RUN dotnet publish -c Release -o out
    
# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "status-api.dll"]
