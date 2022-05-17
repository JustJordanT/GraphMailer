# GraphMailer
A minimal helper project for Microsoft Graph API

## Current Functionality

### EmailSender

**Constructors**:

```csharp
EmailSender(string toEmail, string ccRecipients, string subject,
        string bodyContent, string _tenentId, string _clientId, string _clientSecret, string[] scope)
```

```csharp
EmailSender(string toEmail, string subject,
        string bodyContent, string _tenentId, string _clientId, string _clientSecret, string[] scope)
```
