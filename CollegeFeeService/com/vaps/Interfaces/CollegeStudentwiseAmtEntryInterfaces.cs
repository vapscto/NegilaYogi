using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.College.Fees;

namespace CollegeFeeService.com.vaps.interfaces
{
    public interface CollegeStudentwiseAmtEntryInterfaces
    {
        CLGFeeAmountEntryDTO Getinitialformload(CLGFeeAmountEntryDTO data);
        CLGFeeAmountEntryDTO getbranchdetails(CLGFeeAmountEntryDTO data);
        CLGFeeAmountEntryDTO getsemesterdetails(CLGFeeAmountEntryDTO data); 
         CLGFeeAmountEntryDTO selectsem(CLGFeeAmountEntryDTO data);
        CLGFeeAmountEntryDTO selectacademicyear(CLGFeeAmountEntryDTO data);
        CLGFeeAmountEntryDTO getgroupmapped(CLGFeeAmountEntryDTO data);

        CLGFeeAmountEntryDTO fillslabde(CLGFeeAmountEntryDTO data);

        CLGFeeAmountEntryDTO savedata(CLGFeeAmountEntryDTO data);
        CLGFeeAmountEntryDTO getalldetailsOnselectiontype(CLGFeeAmountEntryDTO data);
        CLGFeeAmountEntryDTO deleterec(CLGFeeAmountEntryDTO data);

        // holy-cross  14-03-2024

        CLGFeeAmountEntryDTO getmappedconcessionheads(CLGFeeAmountEntryDTO data);
        CLGFeeAmountEntryDTO savescholershpheaddata(CLGFeeAmountEntryDTO data);
    }
}
