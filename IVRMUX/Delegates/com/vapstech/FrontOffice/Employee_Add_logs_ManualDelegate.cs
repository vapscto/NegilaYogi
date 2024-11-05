using CommonLibrary;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.FrontOffice
{
    public class Employee_Add_logs_ManualDelegate
    {
        CommonDelegate<FO_Emp_PunchDTO, FO_Emp_PunchDTO> COMFRNT = new CommonDelegate<FO_Emp_PunchDTO, FO_Emp_PunchDTO>();
        public FO_Emp_PunchDTO getdetails(FO_Emp_PunchDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "Employee_Add_logs_Manual_Facade/getalldetails/");
        }

        public FO_Emp_PunchDTO empname(FO_Emp_PunchDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "Employee_Add_logs_Manual_Facade/empname/");
        }
        public FO_Emp_PunchDTO savedetail(FO_Emp_PunchDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "Employee_Add_logs_Manual_Facade/savedetail/");
        }

        public FO_Emp_PunchDTO deleterec(int id)
        {
            return COMFRNT.GetDataByIdFROFF(id, "Employee_Add_logs_Manual_Facade/deletedetails/");
        }



    }
}
