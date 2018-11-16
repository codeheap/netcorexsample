# netcorexsample
Example of a simple dockerized .net core webapp

Work in progress

## Build
if you have installed .NET Core SDK 2.2:
- dotnet build
then
- dotnet run

Or if use docker:
- docker build -t weather-microservice .
then
- docker run -d -p 8081:80 --name hello-docker weather-microservice

#Use:
Call: http://localhost:8081/?lat=-45.12&long=-12.36 (returns a list of generated wheaterforecasts)
