using System.Net.Http.Headers;
using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace GraphMailer.Email.Sender;

public class mailer
{
    public async Task mailSender(string toEmail, string subject,
        string bodyContent, string tenantId, string clientId, string clientSecret, string fromUserId , string[] scope)
    {

        var message = new Message
        {
            Subject = subject,
            Body = new ItemBody
            {
                ContentType = BodyType.Html,
                Content = bodyContent
            },
            ToRecipients = new List<Recipient>()
            {
                new Recipient
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = toEmail
                    }
                }
            },
        };

        IConfidentialClientApplication confidentialClient = ConfidentialClientApplicationBuilder
            .Create(clientId)
            .WithClientSecret(clientSecret)
            .WithAuthority(new Uri($"https://login.microsoftonline.com/{tenantId}/v2.0"))
            .Build();

        // Retrieve an access token for Microsoft Graph (gets a fresh token if needed).
        var authResult = await confidentialClient
            .AcquireTokenForClient(scope)
            .ExecuteAsync().ConfigureAwait(false);

        var token = authResult.AccessToken;
        // Build the Microsoft Graph client. As the authentication provider, set an async lambda
        // which uses the MSAL client to obtain an app-only access token to Microsoft Graph,
        // and inserts this access token in the Authorization header of each API request. 
        GraphServiceClient graphServiceClient =
            new GraphServiceClient(new DelegateAuthenticationProvider(async (requestMessage) =>
                {
                    // Add the access token in the Authorization header of the API request.
                    requestMessage.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                })
            );

        await graphServiceClient.Users[fromUserId]
            .SendMail(message, false)
            .Request()
            .PostAsync();
    }
}