using CommonLibrary;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VisitorsManagement
{
    public class LateInStudentDelegate
    {

        CommonDelegate<LateInStudent_DTO, LateInStudent_DTO> COMVISITOR = new CommonDelegate<LateInStudent_DTO, LateInStudent_DTO>();
        public LateInStudent_DTO loaddata(LateInStudent_DTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "LateInStudentFacade/loaddata/");
        }
        public LateInStudent_DTO get_class(LateInStudent_DTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "LateInStudentFacade/get_class/");
        }
        public LateInStudent_DTO get_section(LateInStudent_DTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "LateInStudentFacade/get_section/");
        }
        public LateInStudent_DTO get_student(LateInStudent_DTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "LateInStudentFacade/get_student/");
        }
        public LateInStudent_DTO savedata(LateInStudent_DTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "LateInStudentFacade/savedata/");
        }
        public LateInStudent_DTO editdata(LateInStudent_DTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "LateInStudentFacade/editdata/");
        }
        public LateInStudent_DTO deactive(LateInStudent_DTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "LateInStudentFacade/deactive/");
        }

        
    }
}
