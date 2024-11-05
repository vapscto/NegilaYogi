using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;

namespace corewebapi18072016.Delegates.com.vapstech.College.Admission
{
    public class clg_CB_SEM_MappingDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<clg_CB_SEM_MappingDTO, clg_CB_SEM_MappingDTO> COMMM = new CommonDelegate<clg_CB_SEM_MappingDTO, clg_CB_SEM_MappingDTO>();
        CommonDelegate<Student_Update_RollNumber, Student_Update_RollNumber> COMMM1 = new CommonDelegate<Student_Update_RollNumber, Student_Update_RollNumber>();
        public clg_CB_SEM_MappingDTO getInstitutiondata(clg_CB_SEM_MappingDTO clg_CB_SEM_MappingDTO)
        {
            return COMMM.clgadmissionbypost(clg_CB_SEM_MappingDTO, "clg_CB_SEM_MappingFacade/getAllDetails/");

        }




        // get list by year
        public clg_CB_SEM_MappingDTO getStudentdataByYear(long id)
        {

            return COMMM.clgadmissionbyid(Convert.ToInt32(id), "clg_CB_SEM_MappingFacade/getStudentdetailsByYear/");


        }

        public clg_CB_SEM_MappingDTO saveSectionAllotmentdetails(clg_CB_SEM_MappingDTO Section)
        {

            return COMMM.clgadmissionbypost(Section, "clg_CB_SEM_MappingFacade/");

        }

        public clg_CB_SEM_MappingDTO SaveData(clg_CB_SEM_MappingDTO ex_dto)
        {
            return COMMM.clgadmissionbypost(ex_dto, "clg_CB_SEM_MappingFacadeController/");

        }


        // Get student details by Year and class

        public clg_CB_SEM_MappingDTO GetstudentdetailsbyYearandclass(clg_CB_SEM_MappingDTO Section)
        {
            return COMMM.clgadmissionbypost(Section, "clg_CB_SEM_MappingFacade/GetstudentdetailsbyYearandclass");
        }

        public clg_CB_SEM_MappingDTO Getbranch(clg_CB_SEM_MappingDTO Section)
        {
            return COMMM.clgadmissionbypost(Section, "clg_CB_SEM_MappingFacade/Getbranch");
        }


        public clg_CB_SEM_MappingDTO savesem(clg_CB_SEM_MappingDTO Section)
        {
            return COMMM.clgadmissionbypost(Section, "clg_CB_SEM_MappingFacade/savesem");
        }
        public clg_CB_SEM_MappingDTO Editrecord(clg_CB_SEM_MappingDTO Section)
        {
            return COMMM.clgadmissionbypost(Section, "clg_CB_SEM_MappingFacade/Editrecord");
        }
        public clg_CB_SEM_MappingDTO deactivate(clg_CB_SEM_MappingDTO Section)
        {
            return COMMM.clgadmissionbypost(Section, "clg_CB_SEM_MappingFacade/deactivate");
        }

        public clg_CB_SEM_MappingDTO sempopup(clg_CB_SEM_MappingDTO Section)
        {
            return COMMM.clgadmissionbypost(Section, "clg_CB_SEM_MappingFacade/sempopup");
        }

        

        //Update roll no
        public clg_CB_SEM_MappingDTO GetStudentListByURN(clg_CB_SEM_MappingDTO data)
        {
            return COMMM.clgadmissionbypost(data, "clg_CB_SEM_MappingFacade/GetStudentListByURN");
        }
        public Student_Update_RollNumber GetStudentListByURNsave(Student_Update_RollNumber data)
        {
            return COMMM1.clgadmissionbypost(data, "clg_CB_SEM_MappingFacade/GetStudentListByURNsave");
        }

    }
}
