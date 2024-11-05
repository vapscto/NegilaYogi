using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;


namespace corewebapi18072016.Delegates.com.vapstech.admission
{
    public class SMSGenaralDelegate
    {
        CommonDelegate<SMSGenaralDTO, SMSGenaralDTO> comm = new CommonDelegate<SMSGenaralDTO, SMSGenaralDTO>();

        public SMSGenaralDTO Getdetails(SMSGenaralDTO data)
        {
            return comm.POSTDataADM(data, "SMSGenaralFacade/Getdetails/");
        }

        public SMSGenaralDTO Getexam(SMSGenaralDTO data)
        {
            return comm.POSTDataADM(data, "SMSGenaralFacade/Getexam/");
        }
        public SMSGenaralDTO GetEmployeeDetailsByLeaveYearAndMonth(SMSGenaralDTO data)
        {
            return comm.POSTDataADM(data, "SMSGenaralFacade/GetEmployeeDetailsByLeaveYearAndMonth/");
        }
        public SMSGenaralDTO Getdepartment(SMSGenaralDTO data)
        {
            return comm.POSTDataADM(data, "SMSGenaralFacade/Getdepartment/");
        }
        public SMSGenaralDTO get_designation(SMSGenaralDTO data)
        {
            return comm.POSTDataADM(data, "SMSGenaralFacade/get_designation/");
        }
        public SMSGenaralDTO get_employee(SMSGenaralDTO data)
        {
            return comm.POSTDataADM(data, "SMSGenaralFacade/get_employee/");
        }
        public SMSGenaralDTO savedetail(SMSGenaralDTO data)
        {
            return comm.POSTDataADM(data, "SMSGenaralFacade/savedetail/");
        }
        public SMSGenaralDTO GetClass(SMSGenaralDTO data)
        {
            return comm.POSTDataADM(data, "SMSGenaralFacade/GetClass/");
        }
        public SMSGenaralDTO GetSection(SMSGenaralDTO data)
        {
            return comm.POSTDataADM(data, "SMSGenaralFacade/GetSection/");
        }
        public SMSGenaralDTO createuser(SMSGenaralDTO data)
        {
            return comm.POSTDataADM(data, "SMSGenaralFacade/createuser/");
        }
        public SMSGenaralDTO GetStudentDetails(SMSGenaralDTO data)
        {
            return comm.POSTDataADM(data, "SMSGenaralFacade/GetStudentDetails/");
        }


    }
}
