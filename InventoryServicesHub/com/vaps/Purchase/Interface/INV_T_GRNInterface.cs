using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Interface
{
    public interface INV_T_GRNInterface
    {
        INV_T_GRNDTO getloaddata(INV_T_GRNDTO data);
        INV_T_GRNDTO getitemDetail(INV_T_GRNDTO data);
        INV_T_GRNDTO savedetails(INV_T_GRNDTO data);
        INV_T_GRNDTO get_GRNitemDetails(INV_T_GRNDTO data);
        INV_T_GRNDTO get_itemtax(INV_T_GRNDTO data);
        INV_T_GRNDTO deactiveg(INV_T_GRNDTO data);
        INV_T_GRNDTO deactive(INV_T_GRNDTO data);
        INV_T_GRNDTO deactivet(INV_T_GRNDTO data);
        INV_T_GRNDTO Edit_GRN_details(INV_T_GRNDTO data);
    //    INV_T_GRNDTO SearchByColumn(INV_T_GRNDTO data);
        

    }


}
