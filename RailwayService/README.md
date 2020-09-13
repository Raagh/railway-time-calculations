# Railway Service

The `Railway Service` is an ASP.net core API that calculates the time it takes a person to go from one station to another. It was requested as a coding task during an interview process.
It displays a simple scenario where you can get the time it takes to go from station A to station B including having to do intermediate connections.

API endpoints are versioned as `v1` 

`v1/journeys/{departFrom}/{arrivedAt}` endpoint returns a Journey Model with the calculated time.

```json
{
  "departFrom": "string",
  "arrivedAt": "string",
  "time": "int"
}
```

## Dependencies

- I use [AutoMapper](https://automapper.org/) to map between domain models and API models.
- I use [Moq](https://github.com/moq/moq) and [XUnit](https://xunit.github.io/) for unit testing the `JourneysService`.
- I use [Swagger](https://swagger.io/) for API documentation.

## Extra included features.

- Deployable as a [Docker](https://www.docker.com/) container.
- The default URL shows a [Swagger](https://swagger.io/) page with the API documentation.

## Considerations regarding some decisions
- Although the project is small I decided to use a layered architecture to separate dependencies, this is done to showcase my knowledge due to this being part of an interview process.
- Although there is no need for the repository to be `async` since we are loading information from a json file, I made the common interface `async` so the implementation can be easily switched for a database in the future.
- Although the method that finds the path from one station to another also works when there is a direct connection, I decided to implement the direct connection and the multiple connections separately,
  there might a performance concern of creating a graph when there is a direct connection available.


## Possible Improvements

Due to time constraints there are some "Missing" features that I would normally implement in a "Real World" Project. Some of them are the following:

- Complete separation between Domain Models and Database Models.
- Generic implementation of the `Repository Pattern` when other repositories are introduced to the service.
- Change the implementation to use a Document DB with proper integration testing(using an in memory version) instead of just loading a .json file

## Run on docker

On a terminal with RailwayService as root run the following commands:

```bash
docker build -t railway-service .
docker run -p 8080:80 railway-service

open https://localhost:8080/index.html for the swagger page
```

## Run on dotnet

On a terminal with RailwayService.Api as root run the following commands:

```bash
dotnet restore
dotnet build
dotnet run
```
