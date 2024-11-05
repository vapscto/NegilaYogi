using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class DCS_Vendor_PaymentDelegate
    {
        CommonDelegate<INV_T_SalesDTO, INV_T_SalesDTO> COMINV = new CommonDelegate<INV_T_SalesDTO, INV_T_SalesDTO>();
        public INV_T_SalesDTO getloaddata(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "DCS_Vendor_PaymentFacade/getloaddata/");
        }
 
        public INV_T_SalesDTO getitem(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "DCS_Vendor_PaymentFacade/getitem/");
        }
        public INV_T_SalesDTO getitemDetail(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "DCS_Vendor_PaymentFacade/getitemDetail/");
        }


        public INV_T_SalesDTO savedetails(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "DCS_Vendor_PaymentFacade/savedetails/");
        }
        public INV_T_SalesDTO getSaletypes(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "DCS_Vendor_PaymentFacade/getSaletypes/");
        }
        public INV_T_SalesDTO getbilldetails(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "DCS_Vendor_PaymentFacade/getbilldetails/");
        }
        public INV_T_SalesDTO getSaleItemTax(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "DCS_Vendor_PaymentFacade/getSaleItemTax/");
        }
        public INV_T_SalesDTO deactive(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "DCS_Vendor_PaymentFacade/deactive/");
        }
        public INV_T_SalesDTO deactiveS(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "DCS_Vendor_PaymentFacade/deactiveS/");
        }
        public INV_T_SalesDTO deactivetax(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "DCS_Vendor_PaymentFacade/deactivetax/");
        }


    }
}
