using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using PreadmissionDTOs.com.vaps.College.Admission;

namespace corewebapi18072016.Delegates.com.vapstech.College.Admission
{
    public class ClgSectionAllotmentDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ClgYearWiseStudentDTO, ClgYearWiseStudentDTO> COMMM = new CommonDelegate<ClgYearWiseStudentDTO, ClgYearWiseStudentDTO>();
        CommonDelegate<Student_Update_RollNumber, Student_Update_RollNumber> COMMM1 = new CommonDelegate<Student_Update_RollNumber, Student_Update_RollNumber>();
        public ClgYearWiseStudentDTO getInstitutiondata(ClgYearWiseStudentDTO ClgYearWiseStudentDTO)
        {
            return COMMM.clgadmissionbypost(ClgYearWiseStudentDTO, "ClgSectionAllotmentFacade/getAllDetails/");

        }




        // get list by year
        public ClgYearWiseStudentDTO getStudentdataByYear(long id)
        {

            return COMMM.clgadmissionbyid(Convert.ToInt32(id), "ClgSectionAllotmentFacade/getStudentdetailsByYear/");


        }

        public ClgYearWiseStudentDTO saveSectionAllotmentdetails(ClgYearWiseStudentDTO Section)
        {

            return COMMM.clgadmissionbypost(Section, "ClgSectionAllotmentFacade/");

        }

        public ClgYearWiseStudentDTO SaveData(ClgYearWiseStudentDTO ex_dto)
        {
            return COMMM.clgadmissionbypost(ex_dto, "ClgSectionAllotmentFacadeController/");

        }


        // Get student details by Year and class

        public ClgYearWiseStudentDTO GetstudentdetailsbyYearandclass(ClgYearWiseStudentDTO Section)
        {
            return COMMM.clgadmissionbypost(Section, "ClgSectionAllotmentFacade/GetstudentdetailsbyYearandclass");
        }

        public ClgYearWiseStudentDTO Getbranch(ClgYearWiseStudentDTO Section)
        {
            return COMMM.clgadmissionbypost(Section, "ClgSectionAllotmentFacade/Getbranch");
        }

        public ClgYearWiseStudentDTO Get_academiccourse(ClgYearWiseStudentDTO Section)
        {
            return COMMM.clgadmissionbypost(Section, "ClgSectionAllotmentFacade/Get_academiccourse");
        }
        public ClgYearWiseStudentDTO Get_semister(ClgYearWiseStudentDTO Section)
        {
            return COMMM.clgadmissionbypost(Section, "ClgSectionAllotmentFacade/Get_semister");
        }


        public ClgYearWiseStudentDTO GetPromocourse(ClgYearWiseStudentDTO Section)
        {
            return COMMM.clgadmissionbypost(Section, "ClgSectionAllotmentFacade/GetPromocourse");
        }
        public ClgYearWiseStudentDTO GetPromobranch(ClgYearWiseStudentDTO Section)
        {
            return COMMM.clgadmissionbypost(Section, "ClgSectionAllotmentFacade/GetPromobranch");
        }
        public ClgYearWiseStudentDTO GetPromosem(ClgYearWiseStudentDTO Section)
        {
            return COMMM.clgadmissionbypost(Section, "ClgSectionAllotmentFacade/GetPromosem");
        }

        public ClgYearWiseStudentDTO promsemonchange(ClgYearWiseStudentDTO Section)
        {
            return COMMM.clgadmissionbypost(Section, "ClgSectionAllotmentFacade/promsemonchange");
        }



        //Update roll no
        public ClgYearWiseStudentDTO GetStudentListByURN(ClgYearWiseStudentDTO data)
        {
            return COMMM.clgadmissionbypost(data, "ClgSectionAllotmentFacade/GetStudentListByURN");
        }
        public ClgYearWiseStudentDTO GetStudentListByURNsave(ClgYearWiseStudentDTO data)
        {
            return COMMM.clgadmissionbypost(data, "ClgSectionAllotmentFacade/GetStudentListByURNsave");
        }

        // Year Loss Section 

        public ClgYearWiseStudentDTO OnChangeyearlossAcademic(ClgYearWiseStudentDTO data)
        {
            return COMMM.clgadmissionbypost(data, "ClgSectionAllotmentFacade/OnChangeyearlossAcademic");
        }
        public ClgYearWiseStudentDTO Getyearlossbranch(ClgYearWiseStudentDTO data)
        {
            return COMMM.clgadmissionbypost(data, "ClgSectionAllotmentFacade/Getyearlossbranch");
        }
        public ClgYearWiseStudentDTO Getyearlosssem(ClgYearWiseStudentDTO data)
        {
            return COMMM.clgadmissionbypost(data, "ClgSectionAllotmentFacade/Getyearlosssem");
        }
        public ClgYearWiseStudentDTO GetStudentListByYear_yearloss1(ClgYearWiseStudentDTO data)
        {
            return COMMM.clgadmissionbypost(data, "ClgSectionAllotmentFacade/GetStudentListByYear_yearloss1");
        }
        

    }
}
