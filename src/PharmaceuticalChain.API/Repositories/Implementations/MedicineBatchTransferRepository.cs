using Microsoft.EntityFrameworkCore;
using PharmaceuticalChain.API.Models.Database;
using PharmaceuticalChain.API.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Repositories.Implementations
{
    public class MedicineBatchTransferRepository : BaseRepository, IMedicineBatchTransferRepository
    {
        public MedicineBatchTransferRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        void IMedicineBatchTransferRepository.Create(MedicineBatchTransfer medicineBatchTransfer)
        {
            dbContext.MedicineBatchTransfers.Add(medicineBatchTransfer);
            dbContext.SaveChanges();
        }

        Guid IMedicineBatchTransferRepository.CreateAndReturnId(MedicineBatchTransfer medicineBatchTransfer)
        {
            (this as IMedicineBatchTransferRepository).Create(medicineBatchTransfer);
            return medicineBatchTransfer.Id;
        }

        void IMedicineBatchTransferRepository.Delete(Guid id)
        {
            var recordToBeDeleted = (this as IMedicineBatchTransferRepository).Get(id);
            dbContext.MedicineBatchTransfers.Remove(recordToBeDeleted);
            dbContext.SaveChanges();
        }

        MedicineBatchTransfer IMedicineBatchTransferRepository.Get(Guid id)
        {
            var result = dbContext.MedicineBatchTransfers.Where(c => c.Id == id)
                .Include(t => t.MedicineBatch)
                    .ThenInclude(b => b.Medicine)
                .Include(t => t.From)
                .Include(t => t.To)
                .SingleOrDefault();
            return result;
        }

        List<MedicineBatchTransfer> IMedicineBatchTransferRepository.GetAll()
        {
            return dbContext.MedicineBatchTransfers
                .Include(t => t.MedicineBatch)
                    .ThenInclude(b => b.Medicine)
                .Include(t => t.From)
                .Include(t => t.To)
                .ToList();
        }

        void IMedicineBatchTransferRepository.Update(MedicineBatchTransfer medicineBatchTransfer)
        {
            dbContext.MedicineBatchTransfers.Update(medicineBatchTransfer);
            dbContext.SaveChanges();
        }
    }
}
