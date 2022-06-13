using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace CityDiscoverTourist.Business.Settings;

public class FirestoreCredentialInitializer
{
    public FirestoreCredentialInitializer(IConfiguration configuration)
    {
        Type = configuration["Firebase:type"];
        ProjectId = configuration["Firebase:project_Id"];
        PrivateKeyId = configuration["Firebase:private_key_id"];
        PrivateKey = configuration["Firebase:private_key"];
        ClientEmail = configuration["Firebase:client_email"];
        ClientId = configuration["Firebase:client_id"];
        AuthUri = configuration["Firebase:auth_uri"];
        TokenUri = configuration["Firebase:token_uri"];
        AuthProviderX509CertUrl = configuration["Firebase:auth_provider_x509_cert_url"];
        ClientX509CertUrl = configuration["Firebase:client_x509_cert_url"];
    }

    public FirestoreCredentialInitializer()
    {
    }

    [JsonPropertyName("type")] public string Type { get; set; }

    public string ProjectId { get; set; }
    public string PrivateKeyId { get; set; }
    public string PrivateKey { get; set; }
    public string ClientEmail { get; set; }
    public string ClientId { get; set; }
    public string AuthUri { get; set; }
    public string TokenUri { get; set; }
    public string AuthProviderX509CertUrl { get; set; }
    public string ClientX509CertUrl { get; set; }
}