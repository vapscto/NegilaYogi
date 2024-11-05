using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory.Sales
{
    public class Sales_Return_Apply_Delegate
    {
        CommonDelegate<Sales_Return_Apply_DTO, Sales_Return_Apply_DTO> COMM = new CommonDelegate<Sales_Return_Apply_DTO, Sales_Return_Apply_DTO>();
        public Sales_Return_Apply_DTO getloaddata(Sales_Return_Apply_DTO dto)
        {
            return COMM.POSTDataInventory(dto, "Sales_Return_ApprovalFacade/getloaddata/");
        }
        public Sales_Return_Apply_DTO getpidetails(Sales_Return_Apply_DTO dto)
        {
            return COMM.POSTDataInventory(dto, "Sales_Return_ApprovalFacade/getpidetails/");
        }
        public Sales_Return_Apply_DTO savedetails(Sales_Return_Apply_DTO dto)
        {
            return COMM.POSTDataInventory(dto, "Sales_Return_ApprovalFacade/savedetails/");
        }

    }
}
