using PharmaceuticalChain.API.Models.Database;
using PharmaceuticalChain.API.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Repositories.Implementations
{
    public class ReceiptRepository : BaseRepository, IReceiptRepository
    {
        public ReceiptRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        Guid IReceiptRepository.CreateAndReturnId(Receipt receipt)
        {
            dbContext.Receipts.Add(receipt);
            dbContext.SaveChanges();

            return receipt.Id;
        }

        List<Receipt> IReceiptRepository.GetReceipts(uint companyId)
        {
            var receipts = dbContext.Receipts.ToList();
            var result = from r in receipts
                         where r.CompanyId == companyId
                         select r;
            return result.ToList();
        }
    }
}
