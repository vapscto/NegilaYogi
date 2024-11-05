using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Interface
{
    public interface INV_R_GRNInterface
    {
       Task<INV_T_GRNDTO> getloaddata(INV_T_GRNDTO data);
        Task<INV_T_GRNDTO> onreport(INV_T_GRNDTO data);
        INV_OpeningBalanceDTO getdata_ob(INV_OpeningBalanceDTO data);
        INV_OpeningBalanceDTO GetReport_ob(INV_OpeningBalanceDTO data);


    }


}
