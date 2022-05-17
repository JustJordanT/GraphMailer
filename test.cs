using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Configuration;
using Microsoft.Office.Interop;
using Microsoft.Identity.Client;
using Microsoft.Graph;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace TimeKeeper
{
    public class Email
    {
		public static async Task SendGraphMail(string EmailSubject, string EmailContent, string SupervisorEmail)
		{
			try
			{
				var tenantId = System.Configuration.ConfigurationManager.AppSettings["Tenant"];
				var clientId = System.Configuration.ConfigurationManager.AppSettings["ClientId"];
				var clientSecret = System.Configuration.ConfigurationManager.AppSettings["ClientSecret"];
				var fromUserId = System.Configuration.ConfigurationManager.AppSettings["FromUserId"];
				string[] scopes = new string[] { "https://graph.microsoft.com/.default" };

				var message = new Message
				{
					Subject = EmailSubject,
					Body = new ItemBody
					{
						ContentType = BodyType.Html,
						Content = EmailContent                   // "Email Content"
					},
					ToRecipients = new List<Recipient>()
				{
					new Recipient
					{
						EmailAddress = new EmailAddress
						{
							//          Address = "jeff.forsyth@gmail.com"
							Address = SupervisorEmail                          //  "kevin.dinh@ies-co.com"
						}
					}
				}
				};


				IConfidentialClientApplication confidentialClient = ConfidentialClientApplicationBuilder
					.Create(clientId)
					.WithClientSecret(clientSecret)
					.WithAuthority(new Uri($"https://login.microsoftonline.com/{tenantId}/v2.0"))
					.Build();

				// Retrieve an access token for Microsoft Graph (gets a fresh token if needed).
				var authResult = await confidentialClient
						.AcquireTokenForClient(scopes)
						.ExecuteAsync().ConfigureAwait(false);

				var token = authResult.AccessToken;
				// Build the Microsoft Graph client. As the authentication provider, set an async lambda
				// which uses the MSAL client to obtain an app-only access token to Microsoft Graph,
				// and inserts this access token in the Authorization header of each API request. 
				var graphServiceClient = new GraphServiceClient(new DelegateAuthenticationProvider((requestMessage) => {
					requestMessage
						.Headers
						.Authorization = new AuthenticationHeaderValue("Bearer", token);

					return Task.CompletedTask;
				}));

				await graphServiceClient.Users[fromUserId]
					  .SendMail(message, false)
					  .Request()
					  .PostAsync();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
