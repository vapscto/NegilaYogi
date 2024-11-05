using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class DCS_T_SalesDelegate
    {
        CommonDelegate<INV_T_SalesDTO, INV_T_SalesDTO> COMINV = new CommonDelegate<INV_T_SalesDTO, INV_T_SalesDTO>();
        public INV_T_SalesDTO getloaddata(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "DCS_T_SalesFacade/getloaddata/");
        }
 
        public INV_T_SalesDTO getitem(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "DCS_T_SalesFacade/getitem/");
        }
        public INV_T_SalesDTO getitemDetail(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "DCS_T_SalesFacade/getitemDetail/");
        }


        public INV_T_SalesDTO savedetails(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "DCS_T_SalesFacade/savedetails/");
        }
        public INV_T_SalesDTO getSaletypes(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "DCS_T_SalesFacade/getSaletypes/");
        }
        public INV_T_SalesDTO getSaleItemDetails(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "DCS_T_SalesFacade/getSaleItemDetails/");
        }
        public INV_T_SalesDTO getSaleItemTax(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "DCS_T_SalesFacade/getSaleItemTax/");
        }
        public INV_T_SalesDTO deactive(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "DCS_T_SalesFacade/deactive/");
        }
        public INV_T_SalesDTO deactiveS(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "DCS_T_SalesFacade/deactiveS/");
        }
        public INV_T_SalesDTO deactivetax(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "DCS_T_SalesFacade/deactivetax/");
        }


    }
}
