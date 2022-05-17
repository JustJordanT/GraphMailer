using Microsoft.Graph;
using Azure.Identity;
using System;

namespace GraphMailer.Email.Sender;

public class AuthProvider : IAuthenticationProvider
{
    public AuthProvider(string _tenentId, string _clientId, string _clientSecret)
    {
        // The client credentials flow requires that you request the
        // /.default scope, and preconfigure your permissions on the
        // app registration in Azure. An administrator must grant consent
        // to those permissions beforehand.
        var _scopes = new[] {"https://graph.microsoft.com/.default"};

        // Multi-tenant apps can use "common",
        // single-tenant apps must use the tenant ID from the Azure portal
        var tenantId = _tenentId;

        // Values from app registration
        var clientId = _clientId;
        var clientSecret = _clientSecret;

        // using Azure.Identity;
        var _options = new TokenCredentialOptions
        {
            AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
        };

        // https://docs.microsoft.com/dotnet/api/azure.identity.clientsecretcredential
        var _clientSecretCredential = new ClientSecretCredential(
            _tenentId, _clientId, _clientSecret, _options);

        var graphClient = new GraphServiceClient(_clientSecretCredential, _scopes);
    }

    public Task AuthenticateRequestAsync(HttpRequestMessage request)
    {
        throw new NotImplementedException();
    }
}