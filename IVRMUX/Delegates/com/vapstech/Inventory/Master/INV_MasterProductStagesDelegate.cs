using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_MasterProductStagesDelegate
    {
        CommonDelegate<INV_Master_ProductDTO, INV_Master_ProductDTO> COMINV = new CommonDelegate<INV_Master_ProductDTO, INV_Master_ProductDTO>();
        public INV_Master_ProductDTO getloaddata(INV_Master_ProductDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterProductStagesFacade/getloaddata/");
        }
        public INV_Master_ProductDTO savedetails(INV_Master_ProductDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterProductStagesFacade/savedetails/");
        }
        public INV_Master_ProductDTO savedetailQty(INV_Master_ProductDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterProductStagesFacade/savedetailQty/");
        }
        

        public INV_Master_ProductDTO savestoreproduct(INV_Master_ProductDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterProductStagesFacade/savestoreproduct/");
        }



        public INV_Master_ProductDTO deactive(INV_Master_ProductDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterProductStagesFacade/deactive/");
        }
        public INV_Master_ProductDTO deactiveQty(INV_Master_ProductDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterProductStagesFacade/deactiveQty/");
        }
        public INV_Master_ProductDTO deactiveptax(INV_Master_ProductDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterProductStagesFacade/deactiveptax/");
        }
        
        public INV_Master_ProductDTO productTax(INV_Master_ProductDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterProductStagesFacade/productTax/");
        }
        public INV_Master_ProductDTO getstages(INV_Master_ProductDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterProductStagesFacade/getstages/");
        }


    }
}
