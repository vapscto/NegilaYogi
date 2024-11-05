﻿using PreadmissionDTOs.com.vaps.College.Fee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
   public interface CollegeStudentLedgerInterface
    {
        CollegeStudentLedgerDTO GetYearList(int id);
        CollegeStudentLedgerDTO get_courses(CollegeStudentLedgerDTO id);
        CollegeStudentLedgerDTO get_branches(CollegeStudentLedgerDTO data);
        CollegeStudentLedgerDTO get_semisters(CollegeStudentLedgerDTO data);
        CollegeStudentLedgerDTO get_student(CollegeStudentLedgerDTO data);
         CollegeStudentLedgerDTO get_report(CollegeStudentLedgerDTO data);
        CollegeStudentLedgerDTO savedata(CollegeStudentLedgerDTO data);
        CollegeStudentLedgerDTO editdata(CollegeStudentLedgerDTO data);
        CollegeStudentLedgerDTO DeleteRecord(CollegeStudentLedgerDTO data);

    }
}
