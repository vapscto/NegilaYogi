using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;

namespace MobileApp.Delegates
{
    public class SmartcardattendanceDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<StudentAttendanceEntryDTO, StudentAttendanceEntryDTO> COMMM = new CommonDelegate<StudentAttendanceEntryDTO, StudentAttendanceEntryDTO>();

        CommonDelegate<class_section_list, class_section_list> COMMM1 = new CommonDelegate<class_section_list, class_section_list>();
        CommonDelegate<CollegeMultiHoursAttendanceEntryDTO, CollegeMultiHoursAttendanceEntryDTO> COMMM2 = new CommonDelegate<CollegeMultiHoursAttendanceEntryDTO, CollegeMultiHoursAttendanceEntryDTO>();
        public class_section_list getSmartCardData(class_section_list data)
        {
            return COMMM1.POSTDataADM(data, "StudentAttendanceEntryFacade/getSmartCardData/");
        }
        public class_section_list SaveSmartCardData(class_section_list data)
        {
            return COMMM1.POSTDataADM(data, "StudentAttendanceEntryFacade/SaveSmartCardData/");
        }

        public StudentAttendanceEntryDTO sendsmsabsent(StudentAttendanceEntryDTO data)
        {
            return COMMM.POSTDataADM(data, "StudentAttendanceEntryFacade/sendsmsabsent/");
        }

        public StudentAttendanceEntryDTO saveattendancesmartcard(StudentAttendanceEntryDTO data)
        {
            return COMMM.POSTDataADM(data, "StudentAttendanceEntryFacade/saveattendancesmartcard/");
        }
        public class_section_list getstudentdetailssmart(class_section_list data)
        {
            return COMMM1.POSTDataADM(data, "StudentAttendanceEntryFacade/getstudentdetailssmart/");
        }

        //College Smart card attendance

        public CollegeMultiHoursAttendanceEntryDTO getalldetails(CollegeMultiHoursAttendanceEntryDTO data)
        {
            return COMMM2.clgadmissionbypost(data, "CollegeMultiHoursAttendanceEntryFacade/getalldetails/");
        }
        public CollegeMultiHoursAttendanceEntryDTO getBranchdata(CollegeMultiHoursAttendanceEntryDTO data)
        {
            return COMMM2.clgadmissionbypost(data, "CollegeMultiHoursAttendanceEntryFacade/getBranchdata/");
        }
        public CollegeMultiHoursAttendanceEntryDTO getSemesterdata(CollegeMultiHoursAttendanceEntryDTO data)
        {
            return COMMM2.clgadmissionbypost(data, "CollegeMultiHoursAttendanceEntryFacade/getSemesterdata/");
        }
        public CollegeMultiHoursAttendanceEntryDTO getSectiondata(CollegeMultiHoursAttendanceEntryDTO data)
        {
            return COMMM2.clgadmissionbypost(data, "CollegeMultiHoursAttendanceEntryFacade/getSectiondata/");
        }
        public CollegeMultiHoursAttendanceEntryDTO getSubjdata(CollegeMultiHoursAttendanceEntryDTO data)
        {
            return COMMM2.clgadmissionbypost(data, "CollegeMultiHoursAttendanceEntryFacade/getSubjdata/");
        }
        public CollegeMultiHoursAttendanceEntryDTO getBatchdata(CollegeMultiHoursAttendanceEntryDTO data)
        {
            return COMMM2.clgadmissionbypost(data, "CollegeMultiHoursAttendanceEntryFacade/getBatchdata/");
        }
    }
}
