using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_PI_ToSupplierDelegate
    {
        CommonDelegate<INV_PI_ToSupplierDTO, INV_PI_ToSupplierDTO> COMINV = new CommonDelegate<INV_PI_ToSupplierDTO, INV_PI_ToSupplierDTO>();
        public INV_PI_ToSupplierDTO getloaddata(INV_PI_ToSupplierDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PI_ToSupplierFacade/getloaddata/");
        }
        public INV_PI_ToSupplierDTO getpiDetail(INV_PI_ToSupplierDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PI_ToSupplierFacade/getpiDetail/");
        }
        public INV_PI_ToSupplierDTO savedetails(INV_PI_ToSupplierDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PI_ToSupplierFacade/savedetails/");
        }
   
        public INV_PI_ToSupplierDTO deactive(INV_PI_ToSupplierDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PI_ToSupplierFacade/deactive/");
        }
     

        

    }
}
