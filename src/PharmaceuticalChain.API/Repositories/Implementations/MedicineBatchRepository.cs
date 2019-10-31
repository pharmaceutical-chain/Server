using PharmaceuticalChain.API.Models.Database;
using PharmaceuticalChain.API.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Repositories.Implementations
{
    public class MedicineBatchRepository : BaseRepository, IMedicineBatchRepository
    {
        public MedicineBatchRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        void IMedicineBatchRepository.Create(MedicineBatch medicineBatch)
        {
            dbContext.MedicineBatches.Add(medicineBatch);
            dbContext.SaveChanges();
        }

        Guid IMedicineBatchRepository.CreateAndReturnId(MedicineBatch medicineBatch)
        {
            (this as IMedicineBatchRepository).Create(medicineBatch);
            return medicineBatch.Id;
        }

        void IMedicineBatchRepository.Delete(Guid id)
        {
            var recordToBeDeleted = (this as IMedicineBatchRepository).Get(id);
            dbContext.MedicineBatches.Remove(recordToBeDeleted);
            dbContext.SaveChanges();
        }

        MedicineBatch IMedicineBatchRepository.Get(Guid id)
        {
            var result = dbContext.MedicineBatches.Where(c => c.Id == id).SingleOrDefault();
            return result;
        }

        List<MedicineBatch> IMedicineBatchRepository.GetAll()
        {
            return dbContext.MedicineBatches.ToList();
        }

        void IMedicineBatchRepository.Update(MedicineBatch medicineBatch)
        {
            dbContext.MedicineBatches.Update(medicineBatch);
            dbContext.SaveChanges();
        }
    }
}
