using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_MasterTaxDelegate
    {
        CommonDelegate<INV_Master_TaxDTO, INV_Master_TaxDTO> COMINV = new CommonDelegate<INV_Master_TaxDTO, INV_Master_TaxDTO>();
        public INV_Master_TaxDTO getloaddata(INV_Master_TaxDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterTaxFacade/getloaddata/");
        }
        public INV_Master_TaxDTO savedetails(INV_Master_TaxDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterTaxFacade/savedetails/");
        }      
        public INV_Master_TaxDTO deactive(INV_Master_TaxDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterTaxFacade/deactive/");
        }
        
    }
}
