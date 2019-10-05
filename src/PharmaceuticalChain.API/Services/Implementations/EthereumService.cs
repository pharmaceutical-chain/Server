using Microsoft.Extensions.Options;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Web3.Accounts.Managed;
using PharmaceuticalChain.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Services.Implementations
{
    public class EthereumService : IEthereumService
    {
        private readonly string ethereumAccount;
        private readonly string ethereumPassword;

        private Web3 web3;
        private readonly string abi = "[\r\n {\r\n \"constant\": true,\r\n \"inputs\": [\r\n {\r\n \"name\": \"\",\r\n \"type\": \"bytes32\"\r\n },\r\n {\r\n \"name\": \"\",\r\n \"type\": \"uint256\"\r\n }\r\n ],\r\n \"name\": \"transferCounters\",\r\n \"outputs\": [\r\n {\r\n \"name\": \"\",\r\n \"type\": \"uint256\"\r\n }\r\n ],\r\n \"payable\": false,\r\n \"stateMutability\": \"view\",\r\n \"type\": \"function\",\r\n \"signature\": \"0x1aa357eb\"\r\n },\r\n {\r\n \"constant\": true,\r\n \"inputs\": [\r\n {\r\n \"name\": \"\",\r\n \"type\": \"address\"\r\n }\r\n ],\r\n \"name\": \"admins\",\r\n \"outputs\": [\r\n {\r\n \"name\": \"\",\r\n \"type\": \"bool\"\r\n }\r\n ],\r\n \"payable\": false,\r\n \"stateMutability\": \"view\",\r\n \"type\": \"function\",\r\n \"signature\": \"0x429b62e5\"\r\n },\r\n {\r\n \"constant\": true,\r\n \"inputs\": [\r\n {\r\n \"name\": \"\",\r\n \"type\": \"bytes32\"\r\n }\r\n ],\r\n \"name\": \"chainCounters\",\r\n \"outputs\": [\r\n {\r\n \"name\": \"\",\r\n \"type\": \"uint256\"\r\n }\r\n ],\r\n \"payable\": false,\r\n \"stateMutability\": \"view\",\r\n \"type\": \"function\",\r\n \"signature\": \"0x98f59eb0\"\r\n },\r\n {\r\n \"constant\": true,\r\n \"inputs\": [\r\n {\r\n \"name\": \"\",\r\n \"type\": \"bytes32\"\r\n },\r\n {\r\n \"name\": \"\",\r\n \"type\": \"uint256\"\r\n },\r\n {\r\n \"name\": \"\",\r\n \"type\": \"uint256\"\r\n }\r\n ],\r\n \"name\": \"medicineBatchTransfers\",\r\n \"outputs\": [\r\n {\r\n \"name\": \"\",\r\n \"type\": \"address\"\r\n }\r\n ],\r\n \"payable\": false,\r\n \"stateMutability\": \"view\",\r\n \"type\": \"function\",\r\n \"signature\": \"0xbefee24e\"\r\n },\r\n {\r\n \"constant\": true,\r\n \"inputs\": [],\r\n \"name\": \"globalAdmin\",\r\n \"outputs\": [\r\n {\r\n \"name\": \"\",\r\n \"type\": \"address\"\r\n }\r\n ],\r\n \"payable\": false,\r\n \"stateMutability\": \"view\",\r\n \"type\": \"function\",\r\n \"signature\": \"0xec9a9368\"\r\n },\r\n {\r\n \"constant\": true,\r\n \"inputs\": [\r\n {\r\n \"name\": \"\",\r\n \"type\": \"bytes32\"\r\n }\r\n ],\r\n \"name\": \"contractAddresses\",\r\n \"outputs\": [\r\n {\r\n \"name\": \"\",\r\n \"type\": \"address\"\r\n }\r\n ],\r\n \"payable\": false,\r\n \"stateMutability\": \"view\",\r\n \"type\": \"function\",\r\n \"signature\": \"0xf689e892\"\r\n },\r\n {\r\n \"inputs\": [],\r\n \"payable\": false,\r\n \"stateMutability\": \"nonpayable\",\r\n \"type\": \"constructor\",\r\n \"signature\": \"constructor\"\r\n },\r\n {\r\n \"anonymous\": false,\r\n \"inputs\": [\r\n {\r\n \"indexed\": false,\r\n \"name\": \"_address\",\r\n \"type\": \"address\"\r\n }\r\n ],\r\n \"name\": \"AdminAdded\",\r\n \"type\": \"event\",\r\n \"signature\": \"0x44d6d25963f097ad14f29f06854a01f575648a1ef82f30e562ccd3889717e339\"\r\n },\r\n {\r\n \"anonymous\": false,\r\n \"inputs\": [\r\n {\r\n \"indexed\": false,\r\n \"name\": \"_address\",\r\n \"type\": \"address\"\r\n }\r\n ],\r\n \"name\": \"AdminRemoved\",\r\n \"type\": \"event\",\r\n \"signature\": \"0xa3b62bc36326052d97ea62d63c3d60308ed4c3ea8ac079dd8499f1e9c4f80c0f\"\r\n },\r\n {\r\n \"anonymous\": false,\r\n \"inputs\": [\r\n {\r\n \"indexed\": false,\r\n \"name\": \"_guid\",\r\n \"type\": \"string\"\r\n },\r\n {\r\n \"indexed\": false,\r\n \"name\": \"_address\",\r\n \"type\": \"address\"\r\n }\r\n ],\r\n \"name\": \"MedicineBatchReleased\",\r\n \"type\": \"event\",\r\n \"signature\": \"0x128b0b7c202eb1d81c5ab27d59d4269a96dc187e01e56d59e5bc264832371c7b\"\r\n },\r\n {\r\n \"anonymous\": false,\r\n \"inputs\": [\r\n {\r\n \"indexed\": false,\r\n \"name\": \"_guid\",\r\n \"type\": \"string\"\r\n },\r\n {\r\n \"indexed\": false,\r\n \"name\": \"_address\",\r\n \"type\": \"address\"\r\n }\r\n ],\r\n \"name\": \"MedicineBatchRemoved\",\r\n \"type\": \"event\",\r\n \"signature\": \"0x5833d6fd3cd72993ba1373116d050f01f54bdbabdc0bb3f120c2cfa998fcd18c\"\r\n },\r\n {\r\n \"anonymous\": false,\r\n \"inputs\": [\r\n {\r\n \"indexed\": false,\r\n \"name\": \"_guid\",\r\n \"type\": \"string\"\r\n },\r\n {\r\n \"indexed\": false,\r\n \"name\": \"_address\",\r\n \"type\": \"address\"\r\n }\r\n ],\r\n \"name\": \"ChainPointAdded\",\r\n \"type\": \"event\",\r\n \"signature\": \"0xed9e5a6889251009dc5e45c58be70b315c65a38fbb4ae020ca015678ffd6210b\"\r\n },\r\n {\r\n \"anonymous\": false,\r\n \"inputs\": [\r\n {\r\n \"indexed\": false,\r\n \"name\": \"_guid\",\r\n \"type\": \"string\"\r\n },\r\n {\r\n \"indexed\": false,\r\n \"name\": \"_address\",\r\n \"type\": \"address\"\r\n }\r\n ],\r\n \"name\": \"ChainPointRemoved\",\r\n \"type\": \"event\",\r\n \"signature\": \"0xace5b64b9c6a11ad4bcae4e3fb91a2cf7054feee20ba15ab68b84c15de5229f6\"\r\n },\r\n {\r\n \"anonymous\": false,\r\n \"inputs\": [\r\n {\r\n \"indexed\": false,\r\n \"name\": \"_guid\",\r\n \"type\": \"string\"\r\n },\r\n {\r\n \"indexed\": false,\r\n \"name\": \"_address\",\r\n \"type\": \"address\"\r\n }\r\n ],\r\n \"name\": \"MedicineBatchTransferAdded\",\r\n \"type\": \"event\",\r\n \"signature\": \"0xdcfede9ed004d61438868d95207b969d95dede1c17e2493158ad4a37f1509c21\"\r\n },\r\n {\r\n \"anonymous\": false,\r\n \"inputs\": [\r\n {\r\n \"indexed\": false,\r\n \"name\": \"_guid\",\r\n \"type\": \"string\"\r\n },\r\n {\r\n \"indexed\": false,\r\n \"name\": \"_address\",\r\n \"type\": \"address\"\r\n }\r\n ],\r\n \"name\": \"MedicineBatchTransferRemoved\",\r\n \"type\": \"event\",\r\n \"signature\": \"0xd875f99a948207db897c894067a605ae007c066574b4847d7e51d3668f0e59ff\"\r\n },\r\n {\r\n \"constant\": false,\r\n \"inputs\": [\r\n {\r\n \"name\": \"_guid\",\r\n \"type\": \"string\"\r\n },\r\n {\r\n \"name\": \"_name\",\r\n \"type\": \"string\"\r\n },\r\n {\r\n \"name\": \"_branchName\",\r\n \"type\": \"string\"\r\n },\r\n {\r\n \"name\": \"_batchNumber\",\r\n \"type\": \"string\"\r\n },\r\n {\r\n \"name\": \"_quantity\",\r\n \"type\": \"uint256\"\r\n },\r\n {\r\n \"name\": \"_manufacturingDate\",\r\n \"type\": \"uint256\"\r\n },\r\n {\r\n \"name\": \"_expiryDate\",\r\n \"type\": \"uint256\"\r\n },\r\n {\r\n \"name\": \"_typeOfMedicine\",\r\n \"type\": \"uint8\"\r\n }\r\n ],\r\n \"name\": \"releaseMedicineBatch\",\r\n \"outputs\": [],\r\n \"payable\": false,\r\n \"stateMutability\": \"nonpayable\",\r\n \"type\": \"function\",\r\n \"signature\": \"0xae9a9885\"\r\n },\r\n {\r\n \"constant\": false,\r\n \"inputs\": [\r\n {\r\n \"name\": \"_guid\",\r\n \"type\": \"string\"\r\n }\r\n ],\r\n \"name\": \"removeMedicineBatch\",\r\n \"outputs\": [],\r\n \"payable\": false,\r\n \"stateMutability\": \"nonpayable\",\r\n \"type\": \"function\",\r\n \"signature\": \"0x688e28e4\"\r\n },\r\n {\r\n \"constant\": false,\r\n \"inputs\": [\r\n {\r\n \"name\": \"_guid\",\r\n \"type\": \"string\"\r\n },\r\n {\r\n \"name\": \"_name\",\r\n \"type\": \"string\"\r\n },\r\n {\r\n \"name\": \"_address\",\r\n \"type\": \"string\"\r\n },\r\n {\r\n \"name\": \"_phoneNumber\",\r\n \"type\": \"string\"\r\n },\r\n {\r\n \"name\": \"_taxCode\",\r\n \"type\": \"string\"\r\n },\r\n {\r\n \"name\": \"_BRCLink\",\r\n \"type\": \"string\"\r\n },\r\n {\r\n \"name\": \"_GPCLink\",\r\n \"type\": \"string\"\r\n }\r\n ],\r\n \"name\": \"addChainPoint\",\r\n \"outputs\": [],\r\n \"payable\": false,\r\n \"stateMutability\": \"nonpayable\",\r\n \"type\": \"function\",\r\n \"signature\": \"0x0f5ce804\"\r\n },\r\n {\r\n \"constant\": false,\r\n \"inputs\": [\r\n {\r\n \"name\": \"_guid\",\r\n \"type\": \"string\"\r\n }\r\n ],\r\n \"name\": \"removeChainPoint\",\r\n \"outputs\": [],\r\n \"payable\": false,\r\n \"stateMutability\": \"nonpayable\",\r\n \"type\": \"function\",\r\n \"signature\": \"0xb49d63ec\"\r\n },\r\n {\r\n \"constant\": false,\r\n \"inputs\": [\r\n {\r\n \"name\": \"_guid\",\r\n \"type\": \"string\"\r\n },\r\n {\r\n \"name\": \"_medicineBatchId\",\r\n \"type\": \"string\"\r\n },\r\n {\r\n \"name\": \"_fromPointId\",\r\n \"type\": \"string\"\r\n },\r\n {\r\n \"name\": \"_toPointId\",\r\n \"type\": \"string\"\r\n },\r\n {\r\n \"name\": \"_quantity\",\r\n \"type\": \"uint256\"\r\n },\r\n {\r\n \"name\": \"_chainIndex\",\r\n \"type\": \"uint256\"\r\n }\r\n ],\r\n \"name\": \"transferMedicineBatch\",\r\n \"outputs\": [],\r\n \"payable\": false,\r\n \"stateMutability\": \"nonpayable\",\r\n \"type\": \"function\",\r\n \"signature\": \"0x05657e77\"\r\n },\r\n {\r\n \"constant\": true,\r\n \"inputs\": [\r\n {\r\n \"name\": \"_medicineBatchId\",\r\n \"type\": \"string\"\r\n },\r\n {\r\n \"name\": \"_chainIndex\",\r\n \"type\": \"uint256\"\r\n },\r\n {\r\n \"name\": \"_transferIndex\",\r\n \"type\": \"uint256\"\r\n }\r\n ],\r\n \"name\": \"getMedicineTransfer\",\r\n \"outputs\": [\r\n {\r\n \"name\": \"\",\r\n \"type\": \"string\"\r\n }\r\n ],\r\n \"payable\": false,\r\n \"stateMutability\": \"view\",\r\n \"type\": \"function\",\r\n \"signature\": \"0xfbc1ef48\"\r\n },\r\n {\r\n \"constant\": false,\r\n \"inputs\": [\r\n {\r\n \"name\": \"_guid\",\r\n \"type\": \"string\"\r\n }\r\n ],\r\n \"name\": \"removeMedicineBatchTransfer\",\r\n \"outputs\": [],\r\n \"payable\": false,\r\n \"stateMutability\": \"nonpayable\",\r\n \"type\": \"function\",\r\n \"signature\": \"0xf0d9ea40\"\r\n },\r\n {\r\n \"constant\": false,\r\n \"inputs\": [\r\n {\r\n \"name\": \"_address\",\r\n \"type\": \"address\"\r\n }\r\n ],\r\n \"name\": \"addAdmin\",\r\n \"outputs\": [],\r\n \"payable\": false,\r\n \"stateMutability\": \"nonpayable\",\r\n \"type\": \"function\",\r\n \"signature\": \"0x70480275\"\r\n },\r\n {\r\n \"constant\": false,\r\n \"inputs\": [\r\n {\r\n \"name\": \"_address\",\r\n \"type\": \"address\"\r\n }\r\n ],\r\n \"name\": \"removeAdmin\",\r\n \"outputs\": [],\r\n \"payable\": false,\r\n \"stateMutability\": \"nonpayable\",\r\n \"type\": \"function\",\r\n \"signature\": \"0x1785f53c\"\r\n },\r\n {\r\n \"constant\": true,\r\n \"inputs\": [\r\n {\r\n \"name\": \"_guid\",\r\n \"type\": \"string\"\r\n }\r\n ],\r\n \"name\": \"getAddressByID\",\r\n \"outputs\": [\r\n {\r\n \"name\": \"\",\r\n \"type\": \"address\"\r\n }\r\n ],\r\n \"payable\": false,\r\n \"stateMutability\": \"view\",\r\n \"type\": \"function\",\r\n \"signature\": \"0x1deb47ae\"\r\n },\r\n {\r\n \"constant\": true,\r\n \"inputs\": [\r\n {\r\n \"name\": \"_guid\",\r\n \"type\": \"string\"\r\n }\r\n ],\r\n \"name\": \"getKey\",\r\n \"outputs\": [\r\n {\r\n \"name\": \"\",\r\n \"type\": \"bytes32\"\r\n }\r\n ],\r\n \"payable\": false,\r\n \"stateMutability\": \"pure\",\r\n \"type\": \"function\",\r\n \"signature\": \"0xd37aec92\"\r\n }\r\n ]";

        private readonly string contractAddress = "0x3e18A6DB759fCB7429f1Bd73C9E1C94875450aB8";

        // Local
        //private readonly string contractAddress = "0xafa0e5114e8c8cac9ae6addc93d380d27b790b54";

        public EthereumService(IOptions<EthereumSettings> options)
        {
            ethereumAccount = options.Value.EthereumAccount;
            ethereumPassword = options.Value.EthereumPassword;

            // Currently using testing account provided on Nethereum Docs.
            var privateKey = "0xA32C64EBF23356CE1C6E8968802515DF9AD769162741EFA693E48E1F98FE9EBE";
            var account = new Account(privateKey);
            //web3 = new Web3(account, "https://ropsten.infura.io/v3/ad8ea364154b464eb6c7ff37f66ffc94");
            web3 = new Web3(account, "http://ethecctzc-dns-reg1.southeastasia.cloudapp.azure.com:8540");

        }

        Contract IEthereumService.GetContract()
        {
           return web3.Eth.GetContract(abi, contractAddress);
        }

        async void IEthereumService.Set(uint value)
        {
            var method = (this as IEthereumService).GetFunction("set");
            var estimate = await method.EstimateGasAsync();
            try
            {
                var result = await method.SendTransactionAsync(ethereumAccount,
                    new HexBigInteger(300000),
                    new HexBigInteger(0),
                    value);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        async Task<string> IEthereumService.Get()
        {
            //var isUnlokced = await web3.Personal.UnlockAccount.SendRequestAsync(ethereumAccount, ethereumPassword, 60);
            //if (!isUnlokced) { throw new UnauthorizedAccessException(); }

            var method = (this as IEthereumService).GetFunction("get");
            var estimate = await method.EstimateGasAsync();
            try
            {
                var result = await method.CallAsync<int>(ethereumAccount,
                    new HexBigInteger(estimate.Value / 100 * 140),
                    new HexBigInteger(0));
                return result.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        Function IEthereumService.GetFunction(string name)
        {
            var contract = (this as IEthereumService).GetContract();
            return contract.GetFunction(name);
        }

        async Task<int> IEthereumService.CallFunction(Function function, params object[] functionInput)
        {
            var estimate = await function.EstimateGasAsync();
            try
            {
                var result = await function.CallAsync<int>(
                    ethereumAccount,
                    new HexBigInteger(300000),
                    new HexBigInteger(0),
                    functionInput);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        Task IEthereumService.SendTransaction(Function function, params object[] functionInput)
        {
            throw new NotImplementedException();
        }

        string IEthereumService.GetContractAddress()
        {
            return contractAddress;
        }
    }
}
