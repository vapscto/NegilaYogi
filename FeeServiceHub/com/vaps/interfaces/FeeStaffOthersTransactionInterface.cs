using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;


namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeeStaffOthersTransactionInterface
    {
        FeeStaffOthersTransactionDTO getdata(FeeStaffOthersTransactionDTO data);
   
      
      
        FeeStaffOthersTransactionDTO duplicaterecept(FeeStaffOthersTransactionDTO data);

        FeeStaffOthersTransactionDTO get_grp_reptno(FeeStaffOthersTransactionDTO data);
        
     
        FeeStaffOthersTransactionDTO edittra(FeeStaffOthersTransactionDTO data);
        //for staff_others
        FeeStaffOthersTransactionDTO select_emp(FeeStaffOthersTransactionDTO id);
        FeeStaffOthersTransactionDTO select_student(FeeStaffOthersTransactionDTO id);
        FeeStaffOthersTransactionDTO getgroupmappedheadsnew_st(FeeStaffOthersTransactionDTO data);
        FeeStaffOthersTransactionDTO savedata_st(FeeStaffOthersTransactionDTO data);
        FeeStaffOthersTransactionDTO searching_s(FeeStaffOthersTransactionDTO data);
        FeeStaffOthersTransactionDTO searching_o(FeeStaffOthersTransactionDTO data);
        FeeStaffOthersTransactionDTO printreceipt_s(FeeStaffOthersTransactionDTO data);
        FeeStaffOthersTransactionDTO printreceipt_o(FeeStaffOthersTransactionDTO data);
        FeeStaffOthersTransactionDTO deletereceipt_s(FeeStaffOthersTransactionDTO data);
        FeeStaffOthersTransactionDTO deletereceipt_o(FeeStaffOthersTransactionDTO data);
        FeeStaffOthersTransactionDTO getacademicyear(FeeStaffOthersTransactionDTO data);

        
    }
}
