using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Portals.Employee
{
    public class EmployeeStudentHomeworkDelegate
    {
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<IVRM_Homework_DTO, IVRM_Homework_DTO> COMMM = new CommonDelegate<IVRM_Homework_DTO, IVRM_Homework_DTO>();
        public IVRM_Homework_DTO savedetail(IVRM_Homework_DTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentHomeworkFacade/savedetail/");
        }

        public IVRM_Homework_DTO Getdetails(IVRM_Homework_DTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentHomeworkFacade/Getdetails/");
        }

        public IVRM_Homework_DTO deactivate(IVRM_Homework_DTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentHomeworkFacade/deactivate/");
        }

        public IVRM_Homework_DTO get_classes(IVRM_Homework_DTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentHomeworkFacade/get_classes/");
        }

        public IVRM_Homework_DTO getsectiondata(IVRM_Homework_DTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentHomeworkFacade/getsectiondata/");
        }

        public IVRM_Homework_DTO getsubject(IVRM_Homework_DTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentHomeworkFacade/getsubject/");
        }

        public IVRM_Homework_DTO editData(IVRM_Homework_DTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentHomeworkFacade/editData/");
        }
        public IVRM_Homework_DTO viewData(IVRM_Homework_DTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentHomeworkFacade/viewData/");
        }
        //==================home work marks enter=======

        public IVRM_Homework_DTO gethomework_student(IVRM_Homework_DTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentHomeworkFacade/gethomework_student/");
        }
        public IVRM_Homework_DTO gethomework_list(IVRM_Homework_DTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentHomeworkFacade/gethomework_list/");
        }
        public IVRM_Homework_DTO getsubjectlist(IVRM_Homework_DTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentHomeworkFacade/getsubjectlist/");
        }
        public IVRM_Homework_DTO homework_marks_update(IVRM_Homework_DTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentHomeworkFacade/homework_marks_update/");
        }

        public IVRM_Homework_DTO edit_homework_mark(IVRM_Homework_DTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentHomeworkFacade/edit_homework_mark/");
        }

         public IVRM_Homework_DTO viewhomework(IVRM_Homework_DTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentHomeworkFacade/viewhomework/");
        }

         public IVRM_Homework_DTO viewstudentupload(IVRM_Homework_DTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentHomeworkFacade/viewstudentupload/");
        }
         public IVRM_Homework_DTO stfupload(IVRM_Homework_DTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentHomeworkFacade/stfupload/");
        }



        public IVRM_Homework_DTO gethomework_listTopic(IVRM_Homework_DTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentHomeworkFacade/gethomework_listTopic/");
        }


        //public IVRM_Homework_DTO callnotification(IVRM_Homework_DTO data)
        //{
        //    return COMMM.POSTPORTALData(data, "EmployeeStudentHomeworkFacade/callnotification/");
        //}
    }
}
