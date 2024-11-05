using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
    public interface CollegePaymentApprovalInterface
    {
        CollegePaymentApprovalDTO getdetails(CollegePaymentApprovalDTO data);
        CollegePaymentApprovalDTO Getreportdetails(CollegePaymentApprovalDTO data);

        CollegePaymentApprovalDTO savedetails(CollegePaymentApprovalDTO data);
    }
}
