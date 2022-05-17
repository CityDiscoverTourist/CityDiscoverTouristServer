using Microsoft.Azure.KeyVault.Models;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace CityDiscoverTourist.API.AWS;

public class PrefixKeyVault: IKeyVaultSecretManager
{
    private readonly string _prefix;

    public PrefixKeyVault(string prefix)
    {
        _prefix = $"{prefix}-";
    }

    public bool Load(SecretItem secret)
    {
        return secret.Identifier.Name.StartsWith(_prefix);
    }

    public string GetKey(SecretBundle secret)
    {
        return secret.SecretIdentifier.Name.Substring(_prefix.Length)
            .Replace("--", ":");
    }
}