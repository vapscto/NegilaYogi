using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_T_GRNDelegate
    {
        CommonDelegate<INV_T_GRNDTO, INV_T_GRNDTO> COMINV = new CommonDelegate<INV_T_GRNDTO, INV_T_GRNDTO>();
        public INV_T_GRNDTO getloaddata(INV_T_GRNDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_T_GRNFacade/getloaddata/");
        }
        public INV_T_GRNDTO getitemDetail(INV_T_GRNDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_T_GRNFacade/getitemDetail/");
        }
        public INV_T_GRNDTO savedetails(INV_T_GRNDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_T_GRNFacade/savedetails/");
        }
        public INV_T_GRNDTO get_GRNitemDetails(INV_T_GRNDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_T_GRNFacade/get_GRNitemDetails/");
        }
        public INV_T_GRNDTO deactiveg(INV_T_GRNDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_T_GRNFacade/deactiveg/");
        }
        public INV_T_GRNDTO get_itemtax(INV_T_GRNDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_T_GRNFacade/get_itemtax/");
        }
        public INV_T_GRNDTO deactivet(INV_T_GRNDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_T_GRNFacade/deactivet/");
        }
        public INV_T_GRNDTO deactive(INV_T_GRNDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_T_GRNFacade/deactive/");
        }
        public INV_T_GRNDTO Edit_GRN_details(INV_T_GRNDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_T_GRNFacade/Edit_GRN_details/");
        }
        //public INV_T_GRNDTO SearchByColumn(INV_T_GRNDTO data)
        //{
        //    return COMINV.POSTDataInventory(data, "INV_T_GRNFacade/SearchByColumn/");
        //}

        

    }
}
