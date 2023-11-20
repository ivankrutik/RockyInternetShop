using Microsoft.AspNetCore.Identity.UI.Services;
using RestSharp;
using RestSharp.Authenticators;

namespace RockyInternetShop.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly MailgunSettings _mailgunSettings;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
            _mailgunSettings = _configuration.GetSection("Mailgun").Get<MailgunSettings>();
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async Task Execute(string email, string subject, string body)
        {
            var options = new RestClientOptions("https://api.mailgun.net/v3")
            {
                Authenticator = new HttpBasicAuthenticator("api", _mailgunSettings.ApiKey)
            };
            var client = new RestClient(options);

            RestRequest request = new RestRequest();
            request.AddParameter("domain", _mailgunSettings.Domain, ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "Ecommerce sandbox Mail<mailgun@sandbox0e3318a344384c3d8eb0aefba37b20de.mailgun.org>");
            request.AddParameter("to", "ivan.krutik@gmail.com");
            request.AddParameter("to", "ivan.krutik@gmail.com@" + _mailgunSettings.Domain);
            request.AddParameter("subject", "Hello");
            request.AddParameter("text", "Testing some Mailgun awesomness!");
            request.Method = Method.Post;

            var res = client.Execute(request);
            //await Task.Run(() => client.Execute(request));
        }
    }
}
