using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
   public interface SportsStudentHouseMappingInterface
    {

        SPCC_Student_House_DTO getdetails(SPCC_Student_House_DTO dTO);        
        SPCC_Student_House_DTO get_class(SPCC_Student_House_DTO mas);
        SPCC_Student_House_DTO get_section(SPCC_Student_House_DTO mas);
        SPCC_Student_House_DTO get_student(SPCC_Student_House_DTO mas);
        SPCC_Student_House_DTO get_student_info(SPCC_Student_House_DTO mas);
        SPCC_Student_House_DTO saveRecord(SPCC_Student_House_DTO mas);
        SPCC_Student_House_DTO deactivate(SPCC_Student_House_DTO dto);
        SPCC_Student_House_DTO EditRecord(SPCC_Student_House_DTO dto);


        
    }
}
