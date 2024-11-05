using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
    public interface CollegeStaffAndOtherTransactionInterface
    {
        CollegeStaffAndOtherTransactionDTO getdata(CollegeStaffAndOtherTransactionDTO data);



        CollegeStaffAndOtherTransactionDTO duplicaterecept(CollegeStaffAndOtherTransactionDTO data);

        CollegeStaffAndOtherTransactionDTO get_grp_reptno(CollegeStaffAndOtherTransactionDTO data);


        CollegeStaffAndOtherTransactionDTO edittra(CollegeStaffAndOtherTransactionDTO data);
        //for staff_others
        CollegeStaffAndOtherTransactionDTO select_emp(CollegeStaffAndOtherTransactionDTO id);
        CollegeStaffAndOtherTransactionDTO select_student(CollegeStaffAndOtherTransactionDTO id);
        CollegeStaffAndOtherTransactionDTO getgroupmappedheadsnew_st(CollegeStaffAndOtherTransactionDTO data);
        CollegeStaffAndOtherTransactionDTO savedata_st(CollegeStaffAndOtherTransactionDTO data);
        CollegeStaffAndOtherTransactionDTO searching_s(CollegeStaffAndOtherTransactionDTO data);
        CollegeStaffAndOtherTransactionDTO searching_o(CollegeStaffAndOtherTransactionDTO data);
        CollegeStaffAndOtherTransactionDTO printreceipt_s(CollegeStaffAndOtherTransactionDTO data);
        CollegeStaffAndOtherTransactionDTO printreceipt_o(CollegeStaffAndOtherTransactionDTO data);
        CollegeStaffAndOtherTransactionDTO deletereceipt_s(CollegeStaffAndOtherTransactionDTO data);
        CollegeStaffAndOtherTransactionDTO deletereceipt_o(CollegeStaffAndOtherTransactionDTO data);
        CollegeStaffAndOtherTransactionDTO getacademicyear(CollegeStaffAndOtherTransactionDTO data);

    }
}
