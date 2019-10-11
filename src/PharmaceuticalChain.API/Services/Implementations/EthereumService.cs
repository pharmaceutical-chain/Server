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
        private readonly string abi = "[\r\n\t{\r\n\t\t\"inputs\": [],\r\n\t\t\"payable\": false,\r\n\t\t\"stateMutability\": \"nonpayable\",\r\n\t\t\"type\": \"constructor\"\r\n\t},\r\n\t{\r\n\t\t\"anonymous\": false,\r\n\t\t\"inputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"indexed\": false,\r\n\t\t\t\t\"internalType\": \"address\",\r\n\t\t\t\t\"name\": \"_address\",\r\n\t\t\t\t\"type\": \"address\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"name\": \"AdminAdded\",\r\n\t\t\"type\": \"event\"\r\n\t},\r\n\t{\r\n\t\t\"anonymous\": false,\r\n\t\t\"inputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"indexed\": false,\r\n\t\t\t\t\"internalType\": \"address\",\r\n\t\t\t\t\"name\": \"_address\",\r\n\t\t\t\t\"type\": \"address\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"name\": \"AdminRemoved\",\r\n\t\t\"type\": \"event\"\r\n\t},\r\n\t{\r\n\t\t\"anonymous\": false,\r\n\t\t\"inputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"indexed\": false,\r\n\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\"name\": \"_guid\",\r\n\t\t\t\t\"type\": \"string\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"indexed\": false,\r\n\t\t\t\t\"internalType\": \"address\",\r\n\t\t\t\t\"name\": \"_address\",\r\n\t\t\t\t\"type\": \"address\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"name\": \"ChainPointAdded\",\r\n\t\t\"type\": \"event\"\r\n\t},\r\n\t{\r\n\t\t\"anonymous\": false,\r\n\t\t\"inputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"indexed\": false,\r\n\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\"name\": \"_guid\",\r\n\t\t\t\t\"type\": \"string\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"indexed\": false,\r\n\t\t\t\t\"internalType\": \"address\",\r\n\t\t\t\t\"name\": \"_address\",\r\n\t\t\t\t\"type\": \"address\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"name\": \"ChainPointRemoved\",\r\n\t\t\"type\": \"event\"\r\n\t},\r\n\t{\r\n\t\t\"anonymous\": false,\r\n\t\t\"inputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"indexed\": false,\r\n\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\"name\": \"_guid\",\r\n\t\t\t\t\"type\": \"string\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"indexed\": false,\r\n\t\t\t\t\"internalType\": \"address\",\r\n\t\t\t\t\"name\": \"_address\",\r\n\t\t\t\t\"type\": \"address\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"name\": \"MedicineBatchReleased\",\r\n\t\t\"type\": \"event\"\r\n\t},\r\n\t{\r\n\t\t\"anonymous\": false,\r\n\t\t\"inputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"indexed\": false,\r\n\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\"name\": \"_guid\",\r\n\t\t\t\t\"type\": \"string\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"indexed\": false,\r\n\t\t\t\t\"internalType\": \"address\",\r\n\t\t\t\t\"name\": \"_address\",\r\n\t\t\t\t\"type\": \"address\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"name\": \"MedicineBatchRemoved\",\r\n\t\t\"type\": \"event\"\r\n\t},\r\n\t{\r\n\t\t\"anonymous\": false,\r\n\t\t\"inputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"indexed\": false,\r\n\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\"name\": \"_guid\",\r\n\t\t\t\t\"type\": \"string\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"indexed\": false,\r\n\t\t\t\t\"internalType\": \"address\",\r\n\t\t\t\t\"name\": \"_address\",\r\n\t\t\t\t\"type\": \"address\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"name\": \"MedicineBatchTransferAdded\",\r\n\t\t\"type\": \"event\"\r\n\t},\r\n\t{\r\n\t\t\"anonymous\": false,\r\n\t\t\"inputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"indexed\": false,\r\n\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\"name\": \"_guid\",\r\n\t\t\t\t\"type\": \"string\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"indexed\": false,\r\n\t\t\t\t\"internalType\": \"address\",\r\n\t\t\t\t\"name\": \"_address\",\r\n\t\t\t\t\"type\": \"address\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"name\": \"MedicineBatchTransferRemoved\",\r\n\t\t\"type\": \"event\"\r\n\t},\r\n\t{\r\n\t\t\"constant\": false,\r\n\t\t\"inputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"address\",\r\n\t\t\t\t\"name\": \"_address\",\r\n\t\t\t\t\"type\": \"address\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"name\": \"addAdmin\",\r\n\t\t\"outputs\": [],\r\n\t\t\"payable\": false,\r\n\t\t\"stateMutability\": \"nonpayable\",\r\n\t\t\"type\": \"function\"\r\n\t},\r\n\t{\r\n\t\t\"constant\": false,\r\n\t\t\"inputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\"name\": \"_guid\",\r\n\t\t\t\t\"type\": \"string\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\"name\": \"_name\",\r\n\t\t\t\t\"type\": \"string\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\"name\": \"_address\",\r\n\t\t\t\t\"type\": \"string\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\"name\": \"_phoneNumber\",\r\n\t\t\t\t\"type\": \"string\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\"name\": \"_taxCode\",\r\n\t\t\t\t\"type\": \"string\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\"name\": \"_BRCLink\",\r\n\t\t\t\t\"type\": \"string\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\"name\": \"_GPCLink\",\r\n\t\t\t\t\"type\": \"string\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"name\": \"addChainPoint\",\r\n\t\t\"outputs\": [],\r\n\t\t\"payable\": false,\r\n\t\t\"stateMutability\": \"nonpayable\",\r\n\t\t\"type\": \"function\"\r\n\t},\r\n\t{\r\n\t\t\"constant\": true,\r\n\t\t\"inputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"address\",\r\n\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\"type\": \"address\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"name\": \"admins\",\r\n\t\t\"outputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"bool\",\r\n\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\"type\": \"bool\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"payable\": false,\r\n\t\t\"stateMutability\": \"view\",\r\n\t\t\"type\": \"function\"\r\n\t},\r\n\t{\r\n\t\t\"constant\": true,\r\n\t\t\"inputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"bytes32\",\r\n\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\"type\": \"bytes32\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"name\": \"chainCounters\",\r\n\t\t\"outputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"uint256\",\r\n\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"payable\": false,\r\n\t\t\"stateMutability\": \"view\",\r\n\t\t\"type\": \"function\"\r\n\t},\r\n\t{\r\n\t\t\"constant\": true,\r\n\t\t\"inputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"bytes32\",\r\n\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\"type\": \"bytes32\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"name\": \"contractAddresses\",\r\n\t\t\"outputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"address\",\r\n\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\"type\": \"address\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"payable\": false,\r\n\t\t\"stateMutability\": \"view\",\r\n\t\t\"type\": \"function\"\r\n\t},\r\n\t{\r\n\t\t\"constant\": true,\r\n\t\t\"inputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\"name\": \"_guid\",\r\n\t\t\t\t\"type\": \"string\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"name\": \"getAddressByID\",\r\n\t\t\"outputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"address\",\r\n\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\"type\": \"address\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"payable\": false,\r\n\t\t\"stateMutability\": \"view\",\r\n\t\t\"type\": \"function\"\r\n\t},\r\n\t{\r\n\t\t\"constant\": true,\r\n\t\t\"inputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\"name\": \"_guid\",\r\n\t\t\t\t\"type\": \"string\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"name\": \"getKey\",\r\n\t\t\"outputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"bytes32\",\r\n\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\"type\": \"bytes32\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"payable\": false,\r\n\t\t\"stateMutability\": \"pure\",\r\n\t\t\"type\": \"function\"\r\n\t},\r\n\t{\r\n\t\t\"constant\": true,\r\n\t\t\"inputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\"name\": \"_medicineBatchId\",\r\n\t\t\t\t\"type\": \"string\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"uint256\",\r\n\t\t\t\t\"name\": \"_chainIndex\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"uint256\",\r\n\t\t\t\t\"name\": \"_transferIndex\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"name\": \"getMedicineTransfer\",\r\n\t\t\"outputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\"type\": \"string\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"payable\": false,\r\n\t\t\"stateMutability\": \"view\",\r\n\t\t\"type\": \"function\"\r\n\t},\r\n\t{\r\n\t\t\"constant\": true,\r\n\t\t\"inputs\": [],\r\n\t\t\"name\": \"globalAdmin\",\r\n\t\t\"outputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"address\",\r\n\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\"type\": \"address\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"payable\": false,\r\n\t\t\"stateMutability\": \"view\",\r\n\t\t\"type\": \"function\"\r\n\t},\r\n\t{\r\n\t\t\"constant\": true,\r\n\t\t\"inputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"bytes32\",\r\n\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\"type\": \"bytes32\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"uint256\",\r\n\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"uint256\",\r\n\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"name\": \"medicineBatchTransfers\",\r\n\t\t\"outputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"contract MedicineBatchTransfer\",\r\n\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\"type\": \"address\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"payable\": false,\r\n\t\t\"stateMutability\": \"view\",\r\n\t\t\"type\": \"function\"\r\n\t},\r\n\t{\r\n\t\t\"constant\": false,\r\n\t\t\"inputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\"name\": \"_guid\",\r\n\t\t\t\t\"type\": \"string\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\"name\": \"_name\",\r\n\t\t\t\t\"type\": \"string\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\"name\": \"_branchName\",\r\n\t\t\t\t\"type\": \"string\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\"name\": \"_batchNumber\",\r\n\t\t\t\t\"type\": \"string\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"uint256\",\r\n\t\t\t\t\"name\": \"_quantity\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"uint256\",\r\n\t\t\t\t\"name\": \"_manufacturingDate\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"uint256\",\r\n\t\t\t\t\"name\": \"_expiryDate\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"enum MedicineBatch.TypesOfMedicine\",\r\n\t\t\t\t\"name\": \"_typeOfMedicine\",\r\n\t\t\t\t\"type\": \"uint8\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"name\": \"releaseMedicineBatch\",\r\n\t\t\"outputs\": [],\r\n\t\t\"payable\": false,\r\n\t\t\"stateMutability\": \"nonpayable\",\r\n\t\t\"type\": \"function\"\r\n\t},\r\n\t{\r\n\t\t\"constant\": false,\r\n\t\t\"inputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"address\",\r\n\t\t\t\t\"name\": \"_address\",\r\n\t\t\t\t\"type\": \"address\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"name\": \"removeAdmin\",\r\n\t\t\"outputs\": [],\r\n\t\t\"payable\": false,\r\n\t\t\"stateMutability\": \"nonpayable\",\r\n\t\t\"type\": \"function\"\r\n\t},\r\n\t{\r\n\t\t\"constant\": false,\r\n\t\t\"inputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\"name\": \"_guid\",\r\n\t\t\t\t\"type\": \"string\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"name\": \"removeChainPoint\",\r\n\t\t\"outputs\": [],\r\n\t\t\"payable\": false,\r\n\t\t\"stateMutability\": \"nonpayable\",\r\n\t\t\"type\": \"function\"\r\n\t},\r\n\t{\r\n\t\t\"constant\": false,\r\n\t\t\"inputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\"name\": \"_guid\",\r\n\t\t\t\t\"type\": \"string\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"name\": \"removeMedicineBatch\",\r\n\t\t\"outputs\": [],\r\n\t\t\"payable\": false,\r\n\t\t\"stateMutability\": \"nonpayable\",\r\n\t\t\"type\": \"function\"\r\n\t},\r\n\t{\r\n\t\t\"constant\": false,\r\n\t\t\"inputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\"name\": \"_guid\",\r\n\t\t\t\t\"type\": \"string\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"name\": \"removeMedicineBatchTransfer\",\r\n\t\t\"outputs\": [],\r\n\t\t\"payable\": false,\r\n\t\t\"stateMutability\": \"nonpayable\",\r\n\t\t\"type\": \"function\"\r\n\t},\r\n\t{\r\n\t\t\"constant\": true,\r\n\t\t\"inputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"bytes32\",\r\n\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\"type\": \"bytes32\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"uint256\",\r\n\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"name\": \"transferCounters\",\r\n\t\t\"outputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"uint256\",\r\n\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"payable\": false,\r\n\t\t\"stateMutability\": \"view\",\r\n\t\t\"type\": \"function\"\r\n\t},\r\n\t{\r\n\t\t\"constant\": false,\r\n\t\t\"inputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\"name\": \"_guid\",\r\n\t\t\t\t\"type\": \"string\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\"name\": \"_medicineBatchId\",\r\n\t\t\t\t\"type\": \"string\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\"name\": \"_fromPointId\",\r\n\t\t\t\t\"type\": \"string\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\"name\": \"_toPointId\",\r\n\t\t\t\t\"type\": \"string\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"uint256\",\r\n\t\t\t\t\"name\": \"_quantity\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"internalType\": \"uint256\",\r\n\t\t\t\t\"name\": \"_chainIndex\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"name\": \"transferMedicineBatch\",\r\n\t\t\"outputs\": [],\r\n\t\t\"payable\": false,\r\n\t\t\"stateMutability\": \"nonpayable\",\r\n\t\t\"type\": \"function\"\r\n\t}\r\n]";

        //private readonly string contractAddress = "0x3e18A6DB759fCB7429f1Bd73C9E1C94875450aB8"; // POA Consortium
        private readonly string contractAddress = "0x11211022f9927F9fFb996DAc14D9be85fcD9623f"; // Ropsten

        public EthereumService(IOptions<EthereumSettings> options)
        {
            ethereumAccount = options.Value.EthereumAccount;
            ethereumPassword = options.Value.EthereumPassword;

            var privateKey = "0xA32C64EBF23356CE1C6E8968802515DF9AD769162741EFA693E48E1F98FE9EBE";
            var account = new Account(privateKey);
            web3 = new Web3(account, "https://ropsten.infura.io/v3/ad8ea364154b464eb6c7ff37f66ffc94");
            //web3 = new Web3(account, "http://ethecctzc-dns-reg1.southeastasia.cloudapp.azure.com:8540");

        }

        Contract IEthereumService.GetContract()
        {
           return web3.Eth.GetContract(abi, contractAddress);
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

        string IEthereumService.GetEthereumAccount()
        {
            return ethereumAccount;
        }
    }
}
