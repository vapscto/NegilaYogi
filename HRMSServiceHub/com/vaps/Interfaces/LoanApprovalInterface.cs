﻿using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface LoanApprovalInterface
    {
        HR_Emp_Loan_ApprovalDTO getBasicData(HR_Emp_Loan_ApprovalDTO dto);

    }
}