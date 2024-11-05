using System;
using PreadmissionDTOs.com.vaps.admission;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class readmitstudentDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<SchoolYearWiseStudentDTO, SchoolYearWiseStudentDTO> COMMM = new CommonDelegate<SchoolYearWiseStudentDTO, SchoolYearWiseStudentDTO>();

        CommonDelegate<readmitstudentDTO, readmitstudentDTO> COMMM1 = new CommonDelegate<readmitstudentDTO, readmitstudentDTO>();
        public SchoolYearWiseStudentDTO getInstitutiondata(SchoolYearWiseStudentDTO SchoolYearWiseStudentDTO)
        {
            return COMMM.POSTDataADM(SchoolYearWiseStudentDTO, "readmitstudentFacade/getAllDetails/");

        }




        // get list by year
        public SchoolYearWiseStudentDTO getStudentdataByYear(long id)
        {

            return COMMM.GetDataByIdADM(Convert.ToInt32(id), "readmitstudentFacade/getStudentdetailsByYear/");


        }

        public readmitstudentDTO savereadmit_student(readmitstudentDTO Section)
        {

            return COMMM1.POSTDataADM(Section, "readmitstudentFacade/");

        }

        public SchoolYearWiseStudentDTO SaveData(SchoolYearWiseStudentDTO ex_dto)
        {
            return COMMM.POSTDataADM(ex_dto, "SectionAllotmentFacadeController/");

        }


        // Get student details by Year and class

        public SchoolYearWiseStudentDTO GetstudentdetailsbyYearandclass(SchoolYearWiseStudentDTO Section)
        {

            return COMMM.POSTDataADM(Section, "readmitstudentFacade/GetstudentdetailsbyYearandclass");


        }
        public SchoolYearWiseStudentDTO getnewjoinlist(SchoolYearWiseStudentDTO data)
        {
            return COMMM.POSTDataADM(data, "readmitstudentFacade/getnewjoinlist");

        }


    }
}