using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using PharmaceuticalChain.API.Services.Interfaces;
using QRCoder;
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

        string IVerificationService.CreateQRCode(string url)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeAsPngByteArr = qrCode.GetGraphic(20);

            string base64String = Convert.ToBase64String(qrCodeAsPngByteArr);

            return base64String;
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
            string rootUrl = "https://pharmachain-verificator.herokuapp.com";

            string batchIdLinks = "";
            foreach(var id in batchIds)
            {
                batchIdLinks += (id + "/");
            }

            string verificatorLink = rootUrl + "/" + retailerId + "/" + batchIdLinks;

            return verificatorLink;
        }
    }
}
