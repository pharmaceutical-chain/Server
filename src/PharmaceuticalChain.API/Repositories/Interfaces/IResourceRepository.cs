using PharmaceuticalChain.API.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Repositories.Interfaces
{
    public interface IResourceRepository
    {
        void Create(Resource resource);
        string CreateAndReturnKey(Resource resource);

        void Update(Resource resource);

        List<Resource> GetAll();
        Resource Get(string id);

        void Delete(string id);
    }
}
