using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaceuticalChain.API.Models.Database;


namespace PharmaceuticalChain.API.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        void Create(Transaction transaction);

        List<Transaction> Get();
    }
}
