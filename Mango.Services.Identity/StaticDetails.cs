using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Mango.Services.Identity;

public static class StaticDetails
{
    public const string Admin = nameof(Admin);
    public const string Customer = nameof(Customer);

    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
            new IdentityResources.Profile()
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new("mango", "Mango Server"),
            new("read", "Read your data"),
            new("write", "Write your data"),
            new("delete", "Delete your data")
        };

    public static IEnumerable<Client> Clients => new List<Client>
    {
        new()
        {
            ClientId = "client",
            ClientSecrets = { new Secret("secret1".Sha256()) },
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            AllowedScopes = { "read", "write", "profile" }
        },
        new()
        {
            ClientId = "mango",
            ClientSecrets = { new Secret("secret2".Sha256()) },
            AllowedGrantTypes = GrantTypes.Code,
            RedirectUris = { "https://localhost:7101/signin-oidc" },
            PostLogoutRedirectUris = { "https://localhost:7101/signout-callback-oidc" },
            AllowedScopes = new List<string>
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.StandardScopes.Email,
                "mango"
            }
        }
    };
}