using PharmaceuticalChain.API.Models.Database;
using PharmaceuticalChain.API.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Repositories.Implementations
{
    public class ResourceRepository : BaseRepository, IResourceRepository
    {
        public ResourceRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        void IResourceRepository.Create(Resource resource)
        {
            dbContext.Resources.Add(resource);
            dbContext.SaveChanges();
        }

        string IResourceRepository.CreateAndReturnKey(Resource resource)
        {
            (this as IResourceRepository).Create(resource);
            return resource.Name;
        }

        void IResourceRepository.Delete(string name)
        {
            var recordToBeDeleted = (this as IResourceRepository).Get(name);
            dbContext.Resources.Remove(recordToBeDeleted);
            dbContext.SaveChanges();
        }

        Resource IResourceRepository.Get(string name)
        {
            var result = dbContext.Resources.Where(r => r.Name == name).SingleOrDefault();
            return result;
        }

        List<Resource> IResourceRepository.GetAll()
        {
            var result = dbContext.Resources.ToList();
            return result;
        }

        void IResourceRepository.Update(Resource resource)
        {
            dbContext.Resources.Update(resource);
            dbContext.SaveChanges();
        }
    }
}
