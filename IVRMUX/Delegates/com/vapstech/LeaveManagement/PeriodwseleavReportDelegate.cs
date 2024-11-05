using CommonLibrary;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.LeaveManagement
{
    
    public class PeriodwseleavReportDelegate
    {
        CommonDelegate<LeaveCreditDTO, LeaveCreditDTO> COMFRNT = new CommonDelegate<LeaveCreditDTO, LeaveCreditDTO>();
        public LeaveCreditDTO getdata(LeaveCreditDTO data)
        {
            return COMFRNT.POSTDataOnlineLeave(data, "PeriodwseleavReportFacade/getalldetails/");
        }
        public LeaveCreditDTO get_departments(LeaveCreditDTO student)
        {
            return COMFRNT.POSTDataOnlineLeave(student, "PeriodwseleavReportFacade/get_departments/");
        }
        public LeaveCreditDTO get_designation(LeaveCreditDTO student)
        {
            return COMFRNT.POSTDataOnlineLeave(student, "PeriodwseleavReportFacade/get_designation/");
        }
        public LeaveCreditDTO get_employee(LeaveCreditDTO student)
        {
            return COMFRNT.POSTDataOnlineLeave(student, "PeriodwseleavReportFacade/get_employee/");
        }
        public LeaveCreditDTO getrpt(LeaveCreditDTO student)
        {
            return COMFRNT.POSTDataOnlineLeave(student, "PeriodwseleavReportFacade/getrpt/");
        }
        public LeaveCreditDTO getsiglerpt(LeaveCreditDTO student)
        {
            return COMFRNT.POSTDataOnlineLeave(student, "PeriodwseleavReportFacade/getsiglerpt/");
        }
    }
}
