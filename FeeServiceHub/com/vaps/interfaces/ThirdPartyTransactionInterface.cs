using PreadmissionDTOs.com.vaps.College.Fee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
  public  interface ThirdPartyTransactionInterface
    {
   
        ThirdPartyTransactionDTO getdetails(ThirdPartyTransactionDTO data);
        ThirdPartyTransactionDTO printreceipt(ThirdPartyTransactionDTO data);
        ThirdPartyTransactionDTO getgrpdetails(ThirdPartyTransactionDTO data);  
        ThirdPartyTransactionDTO getStudtdetails(ThirdPartyTransactionDTO data); 
        ThirdPartyTransactionDTO SaveStudentgroupdata(ThirdPartyTransactionDTO data);
        ThirdPartyTransactionDTO Ckeck_Receipt(ThirdPartyTransactionDTO data);
        ThirdPartyTransactionDTO editOthtransaction(ThirdPartyTransactionDTO data); 
        ThirdPartyTransactionDTO DeletOthrRecord(ThirdPartyTransactionDTO data);
    }
}
