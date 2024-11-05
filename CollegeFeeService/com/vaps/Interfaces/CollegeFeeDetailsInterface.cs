using PreadmissionDTOs.com.vaps.College.Fee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
    public interface CollegeFeeDetailsInterface
    {
        CollegeStudentLedgerDTO GetYearList(int id);
        CollegeStudentLedgerDTO get_courses(CollegeStudentLedgerDTO id);
        CollegeStudentLedgerDTO get_branches(CollegeStudentLedgerDTO data);
        CollegeStudentLedgerDTO get_semisters(CollegeStudentLedgerDTO data);
        Task<CollegeStudentLedgerDTO> get_report(CollegeStudentLedgerDTO data);
        Task<CollegeStudentLedgerDTO> get_concession_report(CollegeStudentLedgerDTO data);
        //CollegeStudentLedgerDTO editdata(CollegeStudentLedgerDTO data);
        //CollegeStudentLedgerDTO DeleteRecord(CollegeStudentLedgerDTO data);
    }
}
