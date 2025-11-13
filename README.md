# RabbitMQ Example
A very basic example of using the RabbitMQ .Net library. It uses .Net Aspire to orchestrate:
1. A RabbitMQ container.
1. A .Net Web Api with a `POST /publish` endpoint that publishes messages to a Rabbit MQ queue.
1. A .Net Console App that consumes messages from the Rabbit MQ queue.

## Running locally
In Visual Studio 2026 or later you should be able to F5 debug the solution, ensuring that the AppHost project is set as the startup project.
Running the project should automatically open the Aspire dashbaord where you can view the consumer console logs.
To send requests to the publish endpoint of the Publisher Api you can use the .http file in the RabbitMqPlay.PublisherApi project.