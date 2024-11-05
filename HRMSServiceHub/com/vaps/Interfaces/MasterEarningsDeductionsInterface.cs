using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface MasterEarningsDeductionsInterface
    {
        HR_Master_EarningsDeductionsDTO getBasicData(HR_Master_EarningsDeductionsDTO dto);
        HR_Master_EarningsDeductionsDTO SaveUpdate(HR_Master_EarningsDeductionsDTO dto);
        HR_Master_EarningsDeductionsDTO editData(int id);

        HR_Master_EarningsDeductionsDTO deactivate(HR_Master_EarningsDeductionsDTO dto);

        // Type

        HR_Master_EarningsDeductions_TypeDTO getBasicDatatype(HR_Master_EarningsDeductions_TypeDTO dto);
        HR_Master_EarningsDeductions_TypeDTO SaveUpdatetype(HR_Master_EarningsDeductions_TypeDTO dto);
        HR_Master_EarningsDeductions_TypeDTO editDatatype(int id);

        HR_Master_EarningsDeductions_TypeDTO deactivatetype(HR_Master_EarningsDeductions_TypeDTO dto);


        HR_Master_EarningsDeductionsDTO changeorderData(HR_Master_EarningsDeductionsDTO dto);
    }
}
