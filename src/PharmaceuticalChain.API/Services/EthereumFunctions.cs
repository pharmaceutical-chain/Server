using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Services
{
    public class EthereumFunctions
    {
        public static readonly string AddChainPoint = "addTenant";
        public static readonly string RemoveChainPoint = "removeTenant";

        public static readonly string UpdateTenantInformation = "updateTenantInformation";

        public static readonly string GetAddressByID = "getAddressByID";

        public static readonly string AddMedicine = "addMedicine";
        public static readonly string UpdateMedicineInformation = "updateMedicineInformation";
        public static readonly string RemoveMedicine = "removeMedicine";

        public static readonly string AddMedicineBatch = "addMedicineBatch";
        public static readonly string UpdateMedicineBatchInformation = "updateMedicineBatchInformation";
        public static readonly string RemoveMedicineBatch = "removeMedicineBatch";

        public static readonly string AddMedicineBatchTransfer = "addMedicineBatchTransfer";
        public static readonly string UpdateMedicineBatchTransfer = "updateMedicineBatchTransfer";
        public static readonly string RemoveMedicineBatchTransfer = "removeMedicineBatchTransfer";

        public static readonly string SelfDelete = "selfDelete";

    }
}
