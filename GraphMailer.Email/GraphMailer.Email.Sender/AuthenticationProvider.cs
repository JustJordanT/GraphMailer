using Microsoft.Graph;
using Azure.Identity;
using System;
using Microsoft.Identity.Client;

namespace GraphMailer.Email.Sender;

public class AuthenticationProvider : IAuthenticationProvider
{
    public readonly string clientId;
    public readonly string clientSecret;
    public readonly string[] appScopes;
    public readonly string tenantId;

    public AuthenticationProvider(string clientId, string clientSecret, string[] appScopes, string tenantId)
    {
        this.clientId = clientId;
        this.clientSecret = clientSecret;
        this.appScopes = appScopes;
        this.tenantId = tenantId;
    }

    public async Task AuthenticateRequestAsync(HttpRequestMessage request)
    {
        var clientApplication = ConfidentialClientApplicationBuilder.Create(this.clientId)
            .WithClientSecret(this.clientSecret)
            .WithClientId(this.clientId)
            .WithTenantId(this.tenantId)
            .Build();

        var result = await clientApplication.AcquireTokenForClient(this.appScopes).ExecuteAsync();

        request.Headers.Add("Authorization", result.CreateAuthorizationHeader());
    }
}