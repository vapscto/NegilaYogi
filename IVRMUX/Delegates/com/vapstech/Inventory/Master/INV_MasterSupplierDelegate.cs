using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_MasterSupplierDelegate
    {
        CommonDelegate<INV_Master_SupplierDTO, INV_Master_SupplierDTO> COMINV = new CommonDelegate<INV_Master_SupplierDTO, INV_Master_SupplierDTO>();
        public INV_Master_SupplierDTO getloaddata(INV_Master_SupplierDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterSupplierFacade/getloaddata/");
        }
        public INV_Master_SupplierDTO savedetails(INV_Master_SupplierDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterSupplierFacade/savedetails/");
        }
        public INV_Master_SupplierDTO deactive(INV_Master_SupplierDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterSupplierFacade/deactive/");
        }

    }
}
