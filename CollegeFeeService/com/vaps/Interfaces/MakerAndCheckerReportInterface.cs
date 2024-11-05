using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
    public interface MakerAndCheckerReportInterface
    {

        CollegePaymentApprovalDTO getdetails(CollegePaymentApprovalDTO dt);


        CollegePaymentApprovalDTO get_courses(CollegePaymentApprovalDTO data);
        CollegePaymentApprovalDTO get_branches(CollegePaymentApprovalDTO data);
        CollegePaymentApprovalDTO get_semisters(CollegePaymentApprovalDTO data);
        CollegePaymentApprovalDTO get_semisters_new(CollegePaymentApprovalDTO data);
        CollegePaymentApprovalDTO getgroupmappedheads(CollegePaymentApprovalDTO feedto);

        CollegePaymentApprovalDTO getgroupheadsid(CollegePaymentApprovalDTO feedtohead);

        Task<CollegePaymentApprovalDTO> Getreportdetails(CollegePaymentApprovalDTO feedtoget);

        CollegePaymentApprovalDTO getdata(CollegePaymentApprovalDTO feedtohead);
    }
}
