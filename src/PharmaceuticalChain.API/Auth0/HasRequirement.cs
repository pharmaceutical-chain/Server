using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Auth0
{
    public class HasScopeRequirement : IAuthorizationRequirement
    {
        // 'scope', 'https://www.pharmachain.net/roles'
        public string RequirementName { get; }
        public string Requirement { get; }
        public string Issuer { get; }

        public HasScopeRequirement(string name, string requirement, string issuer)
        {
            RequirementName = name ?? throw new ArgumentNullException(nameof(name));
            Requirement = requirement ?? throw new ArgumentNullException(nameof(requirement));
            Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
        }
    }
}
