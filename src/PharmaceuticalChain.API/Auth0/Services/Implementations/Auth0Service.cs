using Newtonsoft.Json.Linq;
using PharmaceuticalChain.API.Auth0.Services.Interfaces;
using PharmaceuticalChain.API.Auth0.Services.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Auth0.Services.Implementations
{
    public class Auth0Service : IAuth0Service
    {
        public IRestResponse CreateUser()
        {
            var accessToken = ObtainAccessToken();
            if (accessToken != null)
            {
                var clientUser = new RestClient("https://pharmachain.au.auth0.com/api/v2/users");
                var requestCreateUser = new RestRequest(Method.POST);
                requestCreateUser.AddHeader("content-type", "application/json");
                requestCreateUser.AddHeader("authorization", $"Bearer {accessToken}");
                requestCreateUser.AddJsonBody(new User("123", "user@gmail.com", "123456789?a", "user"));
                IRestResponse response = clientUser.Execute(requestCreateUser);

                return response;
            }

            return null;
        }

        public string ObtainAccessToken()
        {
            var client_id = "JfxM3fcapSHTT7mLs6UWubcAZ1FROjPj";
            var client_secret = "-PieTAefOL8wFiWT20-uyWGD-4nlyGfyGpYX0ZleIyKdol2B07dvdaBANhixCxQE";
            var audience = "https://pharmachain.au.auth0.com/api/v2/";

            var clientToken = new RestClient("https://pharmachain.au.auth0.com/oauth/token");
            var requestGetToken = new RestRequest(Method.POST);
            requestGetToken.AddHeader("content-type", "application/x-www-form-urlencoded");
            requestGetToken.AddParameter("grant_type", "client_credentials");
            requestGetToken.AddParameter("client_id", client_id);
            requestGetToken.AddParameter("client_secret", client_secret);
            requestGetToken.AddParameter("audience", audience);
            IRestResponse responseToken = clientToken.Execute(requestGetToken);

            if (responseToken.StatusCode == HttpStatusCode.OK)
            {
                var responseJObject = JObject.Parse(responseToken.Content);
                return responseJObject.GetValue("access_token").ToString();
            }

            return null;
        }
    }
}
