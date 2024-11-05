using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_R_GRNDelegate
    {
        CommonDelegate<INV_T_GRNDTO, INV_T_GRNDTO> COMINV = new CommonDelegate<INV_T_GRNDTO, INV_T_GRNDTO>();
        CommonDelegate<INV_OpeningBalanceDTO, INV_OpeningBalanceDTO> COMINOB = new CommonDelegate<INV_OpeningBalanceDTO, INV_OpeningBalanceDTO>();
      
        public INV_T_GRNDTO getloaddata(INV_T_GRNDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_R_GRNFacade/getloaddata/");
        }
        public INV_T_GRNDTO onreport(INV_T_GRNDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_R_GRNFacade/onreport/");
        }
        public INV_OpeningBalanceDTO getdata_ob(INV_OpeningBalanceDTO data)
        {
            return COMINOB.POSTDataInventory(data, "INV_R_GRNFacade/getdata_ob/");
        }
         public INV_OpeningBalanceDTO GetReport_ob(INV_OpeningBalanceDTO data)
        {
            return COMINOB.POSTDataInventory(data, "INV_R_GRNFacade/GetReport_ob/");
        }
        public INV_T_GRNDTO getdata_grn(INV_T_GRNDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_R_GRNFacade/getdata_ob/");
        }
         public INV_T_GRNDTO GetReport_grn(INV_T_GRNDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_R_GRNFacade/GetReport_grn/");
        }


    }
}
