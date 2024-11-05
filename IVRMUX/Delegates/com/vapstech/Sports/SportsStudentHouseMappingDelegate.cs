using CommonLibrary;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Sports
{
    public class SportsStudentHouseMappingDelegate
    {

        CommonDelegate<SPCC_Student_House_DTO, SPCC_Student_House_DTO> COMSPRT = new CommonDelegate<SPCC_Student_House_DTO, SPCC_Student_House_DTO>();

        public SPCC_Student_House_DTO getdetails(SPCC_Student_House_DTO data)
        {
            return COMSPRT.POSTDataSports(data, "SportsStudentHouseMappingFacade/getdetails/");
        }

        public SPCC_Student_House_DTO get_class(SPCC_Student_House_DTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "SportsStudentHouseMappingFacade/get_class/");
        }

        public SPCC_Student_House_DTO get_section(SPCC_Student_House_DTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "SportsStudentHouseMappingFacade/get_section/");
        }

        public SPCC_Student_House_DTO get_student(SPCC_Student_House_DTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "SportsStudentHouseMappingFacade/get_student/");
        }

        public SPCC_Student_House_DTO get_student_info(SPCC_Student_House_DTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "SportsStudentHouseMappingFacade/get_student_info/");
        }

        public SPCC_Student_House_DTO saveRecord(SPCC_Student_House_DTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "SportsStudentHouseMappingFacade/saveRecord/");
        }
        public SPCC_Student_House_DTO EditRecord(SPCC_Student_House_DTO dTO)
        {
            return COMSPRT.POSTDataSports(dTO, "SportsStudentHouseMappingFacade/EditRecord/");
        }
        public SPCC_Student_House_DTO deactivate(SPCC_Student_House_DTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "SportsStudentHouseMappingFacade/deactivate/");
        }


        
    }
}
