var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username", secret: true);
var password = builder.AddParameter("password", secret: true);

var rabbitMq = builder.AddRabbitMQ("rabbitmq", username, password)
    .WithManagementPlugin();

builder.AddProject<Projects.RabbitMqPlay_PublisherApi>("rabbitmqplay-publisherapi")
    .WithReference(rabbitMq);

builder.Build().Run();
