using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory.Sales
{
    public class INV_T_Sales_ReturnDelegate
    {
        CommonDelegate<INV_T_SalesReturnDTO, INV_T_SalesReturnDTO> COMINV = new CommonDelegate<INV_T_SalesReturnDTO, INV_T_SalesReturnDTO>();
        public INV_T_SalesReturnDTO getloaddata(INV_T_SalesReturnDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_T_SalesReturnFacade/getloaddata/");
        }
        public INV_T_SalesReturnDTO getStudentClsSec(INV_T_SalesReturnDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_T_SalesReturnFacade/getStudentClsSec/");
        }
        public INV_T_SalesReturnDTO getsectionlist(INV_T_SalesReturnDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_T_SalesReturnFacade/getsectionlist/");
        }
        public INV_T_SalesReturnDTO getStudentlist(INV_T_SalesReturnDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_T_SalesReturnFacade/getStudentlist/");
        }
        public INV_T_SalesReturnDTO getitem(INV_T_SalesReturnDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_T_SalesReturnFacade/getitem/");
        }
        public INV_T_SalesReturnDTO getitemDetail(INV_T_SalesReturnDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_T_SalesReturnFacade/getitemDetail/");
        }
        public INV_T_SalesReturnDTO savedetails(INV_T_SalesReturnDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_T_SalesReturnFacade/savedetails/");
        }
        public INV_T_SalesReturnDTO deactive(INV_T_SalesReturnDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_T_SalesReturnFacade/deactive/");
        }
        public INV_T_SalesReturnDTO viewitem(INV_T_SalesReturnDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_T_SalesReturnFacade/viewitem/");
        }

    }
}
