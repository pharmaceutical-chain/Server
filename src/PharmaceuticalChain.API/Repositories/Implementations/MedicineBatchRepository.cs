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

        void IMedicineBatchRepository.Create(Medicine medicineBatch)
        {
            dbContext.Medicines.Add(medicineBatch);
            dbContext.SaveChanges();
        }

        Guid IMedicineBatchRepository.CreateAndReturnId(Medicine medicineBatch)
        {
            (this as IMedicineBatchRepository).Create(medicineBatch);
            return medicineBatch.Id;
        }

        void IMedicineBatchRepository.Delete(Guid id)
        {
            var recordToBeDeleted = (this as IMedicineBatchRepository).Get(id);
            dbContext.Medicines.Remove(recordToBeDeleted);
            dbContext.SaveChanges();
        }

        Medicine IMedicineBatchRepository.Get(Guid id)
        {
            var result = dbContext.Medicines.Where(c => c.Id == id).SingleOrDefault();
            return result;
        }

        List<Medicine> IMedicineBatchRepository.GetAll()
        {
            return dbContext.Medicines.ToList();
        }

        void IMedicineBatchRepository.Update(Medicine medicineBatch)
        {
            dbContext.Medicines.Update(medicineBatch);
            dbContext.SaveChanges();
        }
    }
}
