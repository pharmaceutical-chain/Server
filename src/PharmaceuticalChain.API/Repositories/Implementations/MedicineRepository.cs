using PharmaceuticalChain.API.Models.Database;
using PharmaceuticalChain.API.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Repositories.Implementations
{
    public class MedicineRepository : BaseRepository, IMedicineRepository
    {
        public MedicineRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        void IMedicineRepository.Create(Medicine medicine)
        {
            dbContext.Medicines.Add(medicine);
            dbContext.SaveChanges();
        }

        Guid IMedicineRepository.CreateAndReturnId(Medicine medicine)
        {
            (this as IMedicineRepository).Create(medicine);
            return medicine.Id;
        }

        void IMedicineRepository.Delete(Guid id)
        {
            var recordToBeDeleted = (this as IMedicineRepository).Get(id);
            dbContext.Medicines.Remove(recordToBeDeleted);
            dbContext.SaveChanges();
        }

        Medicine IMedicineRepository.Get(Guid id)
        {
            var result = dbContext.Medicines.Where(c => c.Id == id).SingleOrDefault();
            return result;
        }

        List<Medicine> IMedicineRepository.GetAll()
        {
            return dbContext.Medicines.ToList();
        }

        void IMedicineRepository.Update(Medicine medicine)
        {
            dbContext.Medicines.Update(medicine);
            dbContext.SaveChanges();
        }
    }
}
