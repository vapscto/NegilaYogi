using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
    public interface CLGFeeOpeningBalanceInterface
    {
        CLGFeeOpeningBalanceDTO getalldetails(CLGFeeOpeningBalanceDTO data);
        CLGFeeOpeningBalanceDTO get_courses(CLGFeeOpeningBalanceDTO data);
        CLGFeeOpeningBalanceDTO get_branches(CLGFeeOpeningBalanceDTO data);
        CLGFeeOpeningBalanceDTO get_semisters(CLGFeeOpeningBalanceDTO data);
        CLGFeeOpeningBalanceDTO get_groups(CLGFeeOpeningBalanceDTO data);
        CLGFeeOpeningBalanceDTO get_heads(CLGFeeOpeningBalanceDTO data);
        CLGFeeOpeningBalanceDTO get_installments(CLGFeeOpeningBalanceDTO data);
        CLGFeeOpeningBalanceDTO get_students(CLGFeeOpeningBalanceDTO data);
        CLGFeeOpeningBalanceDTO savedata(CLGFeeOpeningBalanceDTO data);
        CLGFeeOpeningBalanceDTO Deletedetails(CLGFeeOpeningBalanceDTO data);
    }
}
