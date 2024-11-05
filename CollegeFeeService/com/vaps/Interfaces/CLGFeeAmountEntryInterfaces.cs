using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.College.Fees;

namespace CollegeFeeService.com.vaps.interfaces
{
    public interface CLGFeeAmountEntryInterfaces 
    {
        CLGFeeAmountEntryDTO Getinitialformload(CLGFeeAmountEntryDTO data);
        CLGFeeAmountEntryDTO getbranchdetails(CLGFeeAmountEntryDTO data);
        CLGFeeAmountEntryDTO getsemesterdetails(CLGFeeAmountEntryDTO data);
        CLGFeeAmountEntryDTO getcourdetails(CLGFeeAmountEntryDTO data);
        CLGFeeAmountEntryDTO getgroupmapped(CLGFeeAmountEntryDTO data);

        CLGFeeAmountEntryDTO fillslabde(CLGFeeAmountEntryDTO data);

        CLGFeeAmountEntryDTO savedata(CLGFeeAmountEntryDTO data);
    }
}
