using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.admission
{
    public class StudentCompliantsDelegate
    {
        CommonDelegate<StudentCompliants_DTO, StudentCompliants_DTO> comm = new CommonDelegate<StudentCompliants_DTO, StudentCompliants_DTO>(); 
        public StudentCompliants_DTO loaddata(StudentCompliants_DTO data)
        {
            return comm.POSTDataADM(data, "StudentCompliantsFacade/loaddata");
        }
        public StudentCompliants_DTO getstudents(StudentCompliants_DTO data)
        {
            return comm.POSTDataADM(data, "StudentCompliantsFacade/getstudents");
        }
        public StudentCompliants_DTO edittab1(StudentCompliants_DTO data)
        {
            return comm.POSTDataADM(data, "StudentCompliantsFacade/edittab1");
        }
        public StudentCompliants_DTO getorganizationdata(StudentCompliants_DTO data)
        {
            return comm.POSTDataADM(data, "StudentCompliantsFacade/getorganizationdata");
        }
        public StudentCompliants_DTO getstudentdetails(StudentCompliants_DTO data)
        {
            return comm.POSTDataADM(data, "StudentCompliantsFacade/getstudentdetails");
        }
        public StudentCompliants_DTO save(StudentCompliants_DTO data)
        {
            return comm.POSTDataADM(data, "StudentCompliantsFacade/save");
        }
        public StudentCompliants_DTO deactive(StudentCompliants_DTO data)
        {
            return comm.POSTDataADM(data, "StudentCompliantsFacade/deactive");
        }
        public StudentCompliants_DTO searchfilter(StudentCompliants_DTO data)
        {
            return comm.POSTDataADM(data, "StudentCompliantsFacade/searchfilter");
        }
        public StudentCompliants_DTO report(StudentCompliants_DTO data)
        {
            return comm.POSTDataADM(data, "StudentCompliantsFacade/report");
        }
    }
}
