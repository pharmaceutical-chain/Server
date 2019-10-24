using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Auth0.Services.Interfaces
{
    public interface IAuth0Service
    {
        string ObtainAccessToken();

        IRestResponse CreateUser();
    }
}
