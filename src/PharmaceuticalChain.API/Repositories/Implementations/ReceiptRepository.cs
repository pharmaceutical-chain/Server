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

        Receipt IReceiptRepository.GetReceipt(Guid receiptId)
        {
            var receipt = dbContext.Receipts.Where(r => r.Id == receiptId).SingleOrDefault();
            return receipt;
        }

        List<Receipt> IReceiptRepository.GetReceipts(uint companyId)
        {
            var receipts = dbContext.Receipts.ToList();
            var result = (from r in receipts
                         where r.CompanyId == companyId
                         select r).ToList();
            foreach(var receipt in result)
            {
                dbContext.Entry(receipt).Collection(r => r.Transactions).Load();

                receipt.ToCompanyName = dbContext.Companies.Where(c => c.Id == receipt.ToCompanyId).Single().Name;
            }
            return result;
        }
    }
}
