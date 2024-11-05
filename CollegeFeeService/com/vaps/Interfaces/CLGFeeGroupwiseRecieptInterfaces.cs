using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.College.Fees;

namespace CollegeFeeService.com.vaps.interfaces
{
    public interface CLGFeeGroupwiseRecieptInterfaces
    {
        CLGFeeGroupwiseRecieptDTO Getinitialformload(CLGFeeGroupwiseRecieptDTO data);
        CLGFeeGroupwiseRecieptDTO getbranchdetails(CLGFeeGroupwiseRecieptDTO data);
        CLGFeeGroupwiseRecieptDTO getsemesterdetails(CLGFeeGroupwiseRecieptDTO data);
        CLGFeeGroupwiseRecieptDTO getcourdetails(CLGFeeGroupwiseRecieptDTO data);
        CLGFeeGroupwiseRecieptDTO onsemselection(CLGFeeGroupwiseRecieptDTO data);
        CLGFeeGroupwiseRecieptDTO onselectsec(CLGFeeGroupwiseRecieptDTO data);

        CLGFeeGroupwiseRecieptDTO getreceiptreport(CLGFeeGroupwiseRecieptDTO data);
        CLGFeeGroupwiseRecieptDTO getreceipt(CLGFeeGroupwiseRecieptDTO data);
        

    }
}
