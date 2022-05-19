# GraphMailer
A minimal helper project for Microsoft Graph API

[![.NET-Build](https://github.com/JustJordanT/GraphMailer/actions/workflows/dotnet.yml/badge.svg)](https://github.com/JustJordanT/GraphMailer/actions/workflows/dotnet.yml)

[![NuGet Badge](https://buildstats.info/nuget/GraphMailer)](https://www.nuget.org/packages/GraphMailer/)

# Currently Under Development!!!

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


