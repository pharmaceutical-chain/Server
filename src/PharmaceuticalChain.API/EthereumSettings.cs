namespace PharmaceuticalChain.API
{
    public class EthereumSettings
    {
        public EthereumSettings()
        {
            // Empty
        }
        public string EthereumAccount { get; set; }
        public string EthereumPassword { get; set; }


        /// <summary>
        /// Abi of the master contract.
        /// </summary>
        public string Abi { get; set; }
        /// <summary>
        /// Address of the master contract on Ethereum blockchain.
        /// </summary>
        public string ContractAddress { get; set; }

        
        public string MedicineAbi { get; set; }
        public string MedicineBatchAbi { get; set; }
        public string MedicineBatchTransferAbi { get; set; }

        public string TenantAbi { get; set; }



    }
}