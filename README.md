# Singleton Elasticsearch Client with Basic Authentication for .NET

A lightweight, thread-safe singleton implementation of an Elasticsearch client for .NET applications with basic authentication support.

## Features

- **Singleton Pattern**: Ensures only one ElasticClient instance throughout the application lifecycle
- **Basic Authentication**: Built-in support for username/password authentication
- **Configuration-Based**: Uses app.config/web.config for connection settings
- **Thread-Safe**: Safe for use in multi-threaded applications
- **NEST Integration**: Built on top of the official Elasticsearch .NET client (NEST)

## Requirements

- .NET Framework 4.7.2 or higher
- NEST (Elasticsearch .NET client) NuGet package
- System.Configuration assembly

## Installation

1. Add the `ElasticSeachConfiguration.cs` file to your project
2. Install the NEST NuGet package:
   ```
   Install-Package NEST
   ```
3. Add the required configuration settings to your app.config or web.config

## Configuration

Add the following settings to your `app.config` or `web.config` file:

```xml
<configuration>
  <appSettings>
    <add key="ElasticUrl" value="https://your-elasticsearch-server:9200" />
    <add key="ElasticUserName" value="your-username" />
    <add key="ElasticPassword" value="your-password" />
  </appSettings>
</configuration>
```

### Configuration Parameters

| Parameter | Description | Example |
|-----------|-------------|---------|
| `ElasticUrl` | The URL of your Elasticsearch cluster | `https://localhost:9200` |
| `ElasticUserName` | Username for basic authentication | `elastic` |
| `ElasticPassword` | Password for basic authentication | `changeme` |

## Usage

### Basic Usage

```csharp
using ElasticSearch.Connector;

// Get the singleton ElasticClient instance
var client = ElasticSeachConfiguration.ESContext;

// Perform operations with the client
var response = client.Ping();
```

### Search Example

Here's an example of performing a search with pagination and multiple field querying:

```csharp
var responseData = ElasticSeachConfiguration.ESContext.Search<Patient>(s => s
    .From(pageFrom)
    .Size(pageSize)
    .Index(elasticPatientSearchIndex)
    .Query(q => 
          +q.QueryString(mm => mm
            .Query(term)
            .DefaultOperator(Operator.And)
            .Fields(f => f
                        .Field(d => d.Name)
                        .Field(d => d.LastName)
                        .Field(d => d.PhoneNumbers.First().Phone.First())  // for nested fields 
                        .Field(d => d.IdentityNumber))
    )));
```

### Index Operations

```csharp
// Index a document
var indexResponse = ElasticSeachConfiguration.ESContext.Index(document, i => i.Index("my-index"));

// Get a document
var getResponse = ElasticSeachConfiguration.ESContext.Get<MyDocument>("document-id", g => g.Index("my-index"));

// Delete a document
var deleteResponse = ElasticSeachConfiguration.ESContext.Delete<MyDocument>("document-id", d => d.Index("my-index"));
```

## API Reference

### ElasticSeachConfiguration Class

#### Properties

- **`ESContext`** (static): Returns the singleton `IElasticClient` instance

#### Features

- **Thread-Safe**: The singleton implementation is thread-safe using static initialization
- **Lazy Loading**: The ElasticClient is created only when first accessed
- **Connection Pooling**: Built-in connection pooling through NEST
- **Direct Streaming Disabled**: Configured with `.DisableDirectStreaming()` for better debugging

## Implementation Details

The singleton pattern is implemented using:
- Static constructor to ensure thread-safety
- Private constructor to prevent external instantiation
- Static readonly fields for configuration and client instance
- Lazy initialization through static field initialization

## Error Handling

The client will throw exceptions if:
- Configuration values are missing or invalid
- Elasticsearch server is unreachable
- Authentication credentials are incorrect

Make sure to handle these exceptions in your application:

```csharp
try
{
    var response = ElasticSeachConfiguration.ESContext.Search<MyDocument>(/* search parameters */);
    if (response.IsValid)
    {
        // Handle successful response
    }
    else
    {
        // Handle invalid response
        Console.WriteLine($"Error: {response.DebugInformation}");
    }
}
catch (Exception ex)
{
    // Handle connection or configuration errors
    Console.WriteLine($"Elasticsearch error: {ex.Message}");
}
```

## Best Practices

1. **Environment-Specific Configuration**: Use different configuration files for different environments
2. **Secure Credentials**: Store sensitive credentials securely (consider using Azure Key Vault, etc.)
3. **Connection Testing**: Test connectivity during application startup
4. **Error Handling**: Always check `response.IsValid` before processing results
5. **Index Management**: Use meaningful index names and consider index lifecycle management

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## License

This project is provided as-is for educational and development purposes.

## Tested Environment

- .NET Framework 4.7.2
- NEST Elasticsearch client
- Windows/Linux compatible
