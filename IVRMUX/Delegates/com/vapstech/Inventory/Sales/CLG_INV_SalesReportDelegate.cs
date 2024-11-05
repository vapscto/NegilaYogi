using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class CLG_INV_SalesReportDelegate
    {
        CommonDelegate<INV_T_SalesDTO, INV_T_SalesDTO> COMINV = new CommonDelegate<INV_T_SalesDTO, INV_T_SalesDTO>();
        public INV_T_SalesDTO getloaddata(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "CLG_INV_SalesReportFacade/getloaddata/");
        }

        public INV_T_SalesDTO mainradiochange(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "CLG_INV_SalesReportFacade/mainradiochange/");
        }
        public INV_T_SalesDTO getbranchlist(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "CLG_INV_SalesReportFacade/getbranchlist/");
        }
        public INV_T_SalesDTO getsemesterlist(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "CLG_INV_SalesReportFacade/getsemesterlist/");
        }
        public INV_T_SalesDTO getStudentlist(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "CLG_INV_SalesReportFacade/getStudentlist/");
        }

        public INV_T_SalesDTO onreport(INV_T_SalesDTO data)
        {
            return COMINV.POSTDataInventory(data, "CLG_INV_SalesReportFacade/onreport/");
        }


    }
}
