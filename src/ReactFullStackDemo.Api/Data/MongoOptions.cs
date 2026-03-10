namespace ReactFullStackDemo.Api.Data;

public sealed class MongoOptions
{
    public const string SectionName = "Mongo";

    public string ConnectionString { get; init; } = "mongodb://localhost:27017";
    public string Database { get; init; } = "react_fullstack_demo";
    public string PostsCollection { get; init; } = "posts";
}
