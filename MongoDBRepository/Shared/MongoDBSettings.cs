﻿
namespace MongoDBRepository.Shared;

public class MongoDBSettings : IMongoDBSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}
