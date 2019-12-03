using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using PharmaceuticalChain.API.Services.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Services.Implementations
{
    public class VerificationService : IVerificationService
    {
        private readonly string bitlyAccessToken;
        public VerificationService(IConfiguration configuration)
        {
            bitlyAccessToken = configuration["BitlyAccessToken"];
        }

        Task<string> IVerificationService.CreateQRCode()
        {
            throw new NotImplementedException();
        }

        async Task<string> IVerificationService.CreateShortenedLink(string longUrl)
        {
            var client = new RestClient("https://api-ssl.bitly.com/v4");
            var request = new RestRequest("shorten", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", bitlyAccessToken);

            request.AddJsonBody(new
            {
                domain = "bit.ly",
                long_url = longUrl
            });

            var response = await client.ExecuteTaskAsync(request);
            var content = response.Content;
            JObject json = JObject.Parse(content);

            var shortenedUrl = json["link"].ToString();

            return shortenedUrl;
        }

        string IVerificationService.CreateVerificatorLink(Guid retailerId, List<Guid> batchIds)
        {
            throw new NotImplementedException();
        }
    }
}
