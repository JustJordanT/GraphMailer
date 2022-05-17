using Microsoft.Graph;

namespace GraphMailer.Email.Sender;

public class EmailSender
{
    public EmailSender(string toEmail, string subject,
        string bodyContent, string _tenentId, string _clientId, string _clientSecret, string[] scope)
    {
        
    }
    
    public EmailSender(string toEmail, string ccRecipients, string subject,
        string bodyContent, string _tenentId, string _clientId, string _clientSecret, string[] scope)
    {
        var authProvider = new AuthenticationProvider(_clientId, _clientSecret, scope, _tenentId);
        GraphServiceClient graphClient = new GraphServiceClient(authProvider);

        var message = new Message
        {
            Subject = subject,
            Body = new ItemBody
            {
                ContentType = BodyType.Text,
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
            CcRecipients = new List<Recipient>()
            {
                new Recipient
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = ccRecipients
                    }
                }
            }
        };

        var saveToSentItems = false;

        graphClient.Me
            .SendMail(message, saveToSentItems)
            .Request()
            .PostAsync();
    }
}