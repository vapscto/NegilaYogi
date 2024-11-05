using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_MasterProductDelegate
    {
        CommonDelegate<INV_Master_ProductDTO, INV_Master_ProductDTO> COMINV = new CommonDelegate<INV_Master_ProductDTO, INV_Master_ProductDTO>();
        public INV_Master_ProductDTO getloaddata(INV_Master_ProductDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterProductFacade/getloaddata/");
        }
        public INV_Master_ProductDTO savedetails(INV_Master_ProductDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterProductFacade/savedetails/");
        }
        public INV_Master_ProductDTO savedetailQty(INV_Master_ProductDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterProductFacade/savedetailQty/");
        }
        
        public INV_Master_ProductDTO deactive(INV_Master_ProductDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterProductFacade/deactive/");
        }
        public INV_Master_ProductDTO deactiveQty(INV_Master_ProductDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterProductFacade/deactiveQty/");
        }
        public INV_Master_ProductDTO deactiveptax(INV_Master_ProductDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterProductFacade/deactiveptax/");
        }
        
        public INV_Master_ProductDTO productTax(INV_Master_ProductDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterProductFacade/productTax/");
        }
        
    }
}
