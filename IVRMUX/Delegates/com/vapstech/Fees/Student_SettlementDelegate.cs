using CommonLibrary;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Fees
{
    public class Student_SettlementDelegate
    {
        CommonDelegate<Student_SettlementDTO, Student_SettlementDTO> COMMM = new CommonDelegate<Student_SettlementDTO, Student_SettlementDTO>();
        public Student_SettlementDTO Getdetails(Student_SettlementDTO id)
        {
            return COMMM.POSTDatafee(id, "Student_SettlementFacade/Getdetails/");            
        }
        public Student_SettlementDTO getdates(Student_SettlementDTO id)
        {
            return COMMM.POSTDatafee(id, "Student_SettlementFacade/getdates/");
        }
        public Student_SettlementDTO savedata(Student_SettlementDTO id)
        {
            return COMMM.POSTDatafee(id, "Student_SettlementFacade/savedata/");
        }
        public Student_SettlementDTO viewrecords(Student_SettlementDTO id)
        {
            return COMMM.POSTDatafee(id, "Student_SettlementFacade/viewrecords/");
        }
        public Student_SettlementDTO get_classes(Student_SettlementDTO id)
        {
            return COMMM.POSTDatafee(id, "Student_SettlementFacade/get_classes/");
        }
        public Student_SettlementDTO get_sections(Student_SettlementDTO id)
        {
            return COMMM.POSTDatafee(id, "Student_SettlementFacade/get_sections/");
        }
        public Student_SettlementDTO get_routes(Student_SettlementDTO id)
        {
            return COMMM.POSTDatafee(id, "Student_SettlementFacade/get_routes/");
        }
        public Student_SettlementDTO getreport(Student_SettlementDTO id)
        {
            return COMMM.POSTDatafee(id, "Student_SettlementFacade/getreport/");
        }
        public Student_SettlementDTO getreport1(Student_SettlementDTO id)
        {
            return COMMM.POSTDatafee(id, "Student_SettlementFacade/getreport1/");
        }

        public Student_SettlementDTO fillmerchants(Student_SettlementDTO id)
        {
            return COMMM.POSTDatafee(id, "Student_SettlementFacade/fillmerchants/");
        }
    }
}
