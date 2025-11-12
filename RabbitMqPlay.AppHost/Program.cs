var builder = DistributedApplication.CreateBuilder(args);

var rabbitMq = builder.AddRabbitMQ("rabbitmq")
    .WithManagementPlugin();

builder.AddProject<Projects.RabbitMqPlay_PublisherApi>("rabbitmqplay-publisherapi")
    .WithReference(rabbitMq);

builder.AddProject<Projects.RabbitMqPlay_Consumer>("consumer")
    .WithReference(rabbitMq);

builder.Build().Run();
