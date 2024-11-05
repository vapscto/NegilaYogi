using System;
using PreadmissionDTOs.com.vaps.admission;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class SectionAllotmentDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<SchoolYearWiseStudentDTO, SchoolYearWiseStudentDTO> COMMM = new CommonDelegate<SchoolYearWiseStudentDTO, SchoolYearWiseStudentDTO>();
        CommonDelegate<Student_Update_RollNumber, Student_Update_RollNumber> COMMM1 = new CommonDelegate<Student_Update_RollNumber, Student_Update_RollNumber>();
        public SchoolYearWiseStudentDTO getInstitutiondata(SchoolYearWiseStudentDTO SchoolYearWiseStudentDTO)
        {
            return COMMM.POSTDataADM(SchoolYearWiseStudentDTO, "SectionAllotmentFacade/getAllDetails/");
        }
        // get list by year
        public SchoolYearWiseStudentDTO getStudentdataByYear(long id)
        {
            return COMMM.GetDataByIdADM(Convert.ToInt32(id), "SectionAllotmentFacade/getStudentdetailsByYear/");         
        }

        public SchoolYearWiseStudentDTO saveSectionAllotmentdetails(SchoolYearWiseStudentDTO Section)
        {
            return COMMM.POSTDataADM(Section, "SectionAllotmentFacade/");
        }
        public SchoolYearWiseStudentDTO SaveData(SchoolYearWiseStudentDTO ex_dto)
        {
            return COMMM.POSTDataADM(ex_dto, "SectionAllotmentFacadeController/");
        }

        // Get student details by Year and class
        public SchoolYearWiseStudentDTO GetstudentdetailsbyYearandclass(SchoolYearWiseStudentDTO Section)
        {
            return COMMM.POSTDataADM(Section, "SectionAllotmentFacade/GetstudentdetailsbyYearandclass");         
        }

        //Update roll no
        public  SchoolYearWiseStudentDTO GetStudentListByURN(SchoolYearWiseStudentDTO data)
        {
            return COMMM.POSTDataADM(data, "SectionAllotmentFacade/GetStudentListByURN");
        }
        public Student_Update_RollNumber GetStudentListByURNsave(Student_Update_RollNumber data)
        {
            return COMMM1.POSTDataADM(data, "SectionAllotmentFacade/GetStudentListByURNsave");
        }

        // Change Class
        public SchoolYearWiseStudentDTO GetChangeClassDetails(SchoolYearWiseStudentDTO data)
        {
            return COMMM.POSTDataADM(data, "SectionAllotmentFacade/GetChangeClassDetails");
        }
        public SchoolYearWiseStudentDTO GetStudentListByYearCLS(SchoolYearWiseStudentDTO data)
        {
            return COMMM.POSTDataADM(data, "SectionAllotmentFacade/GetStudentListByYearCLS");
        }
        public SchoolYearWiseStudentDTO onstudentnamechange(SchoolYearWiseStudentDTO data)
        {
            return COMMM.POSTDataADM(data, "SectionAllotmentFacade/onstudentnamechange");
        }
        public SchoolYearWiseStudentDTO DeleteFeeMapping(SchoolYearWiseStudentDTO data)
        {
            return COMMM.POSTDataADM(data, "SectionAllotmentFacade/DeleteFeeMapping");
        }
        public SchoolYearWiseStudentDTO SaveClassChange(SchoolYearWiseStudentDTO data)
        {
            return COMMM.POSTDataADM(data, "SectionAllotmentFacade/SaveClassChange");
        }
        public SchoolYearWiseStudentDTO SaveClassFeeChange(SchoolYearWiseStudentDTO data)
        {
            return COMMM.POSTDataADM(data, "SectionAllotmentFacade/SaveClassFeeChange");
        }
    }
}