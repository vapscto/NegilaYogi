using PreadmissionDTOs.com.vaps.College.Fee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
   public interface Clg_StudentFeeGroupMappingInterface
    {
        Clg_StudentFeeGroupMapping_DTO GetYearList(int id);
        Clg_StudentFeeGroupMapping_DTO get_courses(Clg_StudentFeeGroupMapping_DTO id);
        Clg_StudentFeeGroupMapping_DTO get_branches(Clg_StudentFeeGroupMapping_DTO data);
        Clg_StudentFeeGroupMapping_DTO get_semisters(Clg_StudentFeeGroupMapping_DTO data);

         Clg_StudentFeeGroupMapping_DTO get_report(Clg_StudentFeeGroupMapping_DTO data);
        Clg_StudentFeeGroupMapping_DTO savedata(Clg_StudentFeeGroupMapping_DTO data);
        Clg_StudentFeeGroupMapping_DTO editdata(Clg_StudentFeeGroupMapping_DTO data);
        Clg_StudentFeeGroupMapping_DTO DeleteRecord(Clg_StudentFeeGroupMapping_DTO data);
        //saveeditdata
        Clg_StudentFeeGroupMapping_DTO saveeditdata(Clg_StudentFeeGroupMapping_DTO data);
    }
}
