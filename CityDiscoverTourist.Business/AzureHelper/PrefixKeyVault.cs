using Microsoft.Azure.KeyVault.Models;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace CityDiscoverTourist.Business.AzureHelper;

/// <summary>
/// </summary>
public class PrefixKeyVault : IKeyVaultSecretManager
{
    private readonly string _prefix;

    /// <summary>
    /// </summary>
    /// <param name="prefix"></param>
    public PrefixKeyVault(string prefix)
    {
        _prefix = $"{prefix}-";
    }

    /// <summary>
    /// </summary>
    /// <param name="secret"></param>
    /// <returns></returns>
    public bool Load(SecretItem secret)
    {
        return secret.Identifier.Name.StartsWith(_prefix);
    }

    /// <summary>
    /// </summary>
    /// <param name="secret"></param>
    /// <returns></returns>
    public string GetKey(SecretBundle secret)
    {
        return secret.SecretIdentifier.Name.Substring(_prefix.Length).Replace("--", ":");
    }
}