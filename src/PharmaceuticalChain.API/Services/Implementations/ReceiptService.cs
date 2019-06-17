using Nethereum.Hex.HexTypes;
using PharmaceuticalChain.API.Models;
using PharmaceuticalChain.API.Models.Database;
using PharmaceuticalChain.API.Repositories.Interfaces;
using PharmaceuticalChain.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Services.Implementations
{
    public class ReceiptService : IReceiptService
    {
        private readonly IEthereumService ethereumService;
        private readonly IReceiptRepository receiptRepository;
        private readonly ICompanyRepository companyRepository;
        public ReceiptService(
            IEthereumService ethereumService,
            IReceiptRepository receiptRepository,
            ICompanyRepository companyRepository)
        {
            this.ethereumService = ethereumService;
            this.receiptRepository = receiptRepository;
            this.companyRepository = companyRepository;
        }

        Guid IReceiptService.CreateAndReturnId(CreateReceiptCommand command)
        {
            var newReceiptId = receiptRepository.CreateAndReturnId(new Receipt()
            {
                CompanyId = command.companyId,
                ToCompanyId = command.toCompanyId,
                ToCompanyName = companyRepository.Get(command.toCompanyId).Name
            });

            return newReceiptId;
        }

        async Task<ReceiptQuery> IReceiptService.GetReceipts(int companyId)
        {
            var getCompanyFunction = ethereumService.GetFunction("getCompany");
            var getTotalFunction = ethereumService.GetFunction("getTotalCompanies");

            var total = await ethereumService.CallFunction(getTotalFunction);
            CompanyInformation companyInfo = null;
            ReceiptQuery result = new ReceiptQuery();

            //companyInfo = await getCompanyFunction.CallDeserializingToObjectAsync<CompanyInformation>(
            //    "0xa5eE58Df60d9f6c2FE211D287926948292DffbD3",
            //    new HexBigInteger(300000),
            //    new HexBigInteger(0),
            //    functionInput: new object[] { companyId }
            //    );

            result.Receipts = receiptRepository.GetReceipts((uint)companyId);

            

            return result;
        }
    }
}
