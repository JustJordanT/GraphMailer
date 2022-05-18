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

namespace GraphMailer.Email.Sender;

public class EmailTester
{
	public static async Task SendGraphMail(string toEmail, string subject,
		string bodyContent, string tenantId, string clientId, string clientSecret, string fromUserId , string[] scope)
	{
		try
		{
			var tenentID = tenantId;
			
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
				}
			};


			IConfidentialClientApplication confidentialClient = ConfidentialClientApplicationBuilder
				.Create(clientId)
				.WithClientSecret(clientSecret)
				.WithAuthority(new Uri($"https://login.microsoftonline.com/{tenentID}/v2.0"))
				.Build();

			// Retrieve an access token for Microsoft Graph (gets a fresh token if needed).
			var authResult = await confidentialClient
				.AcquireTokenForClient(scope)
				.ExecuteAsync().ConfigureAwait(false);

			var token = authResult.AccessToken;
			
			var cca = ConfidentialClientApplicationBuilder
				.Create(clientId)
				.WithTenantId(tenantId)
				.WithClientSecret(clientSecret)
				.Build();
			
			// // Build the Microsoft Graph client. As the authentication provider, set an async lambda
			// // which uses the MSAL client to obtain an app-only access token to Microsoft Graph,
			// // and inserts this access token in the Authorization header of each API request. 
			// var graphServiceClient = new GraphServiceClient(new DelegateAuthenticationProvider((requestMessage) => {
			// 	requestMessage
			// 		.Headers
			// 		.Authorization = new AuthenticationHeaderValue("Bearer", token);
			//
			// 	return Task.CompletedTask;
			// }));
			
			var authProvider = new DelegateAuthenticationProvider(async (request) => {
				// Use Microsoft.Identity.Client to retrieve token
				var assertion = new UserAssertion(token);
				var result = await cca.AcquireTokenOnBehalfOf(scope, assertion).ExecuteAsync();

				request.Headers.Authorization =
					new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.AccessToken);
			});

			var graphClient = new GraphServiceClient(authProvider);
			
			await graphClient.Users[fromUserId]
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