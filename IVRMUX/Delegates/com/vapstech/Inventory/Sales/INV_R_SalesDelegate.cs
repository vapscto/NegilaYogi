using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_R_SalesDelegate
    {
        CommonDelegate<INV_T_SalesDTO, INV_T_SalesDTO> COMINV = new CommonDelegate<INV_T_SalesDTO, INV_T_SalesDTO>();
        public INV_T_SalesDTO getloaddata(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_R_SalesFacade/getloaddata/");
        }

        public INV_T_SalesDTO mainradiochange(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_R_SalesFacade/mainradiochange/");
        }
        public INV_T_SalesDTO radiochange(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_R_SalesFacade/radiochange/");
        }
        public INV_T_SalesDTO getStudentlist(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_R_SalesFacade/getStudentlist/");
        }

        public INV_T_SalesDTO onreport(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_R_SalesFacade/onreport/");
        }


    }
}
