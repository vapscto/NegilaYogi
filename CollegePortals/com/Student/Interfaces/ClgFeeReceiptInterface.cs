using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.College.Portals;
using PreadmissionDTOs.com.vaps.College.Fees;

namespace CollegePortals.com.vaps.Student.Interfaces
{
    public interface ClgFeeReceiptInterface
    {
        ClgPortalFeeDTO getloaddata(ClgPortalFeeDTO data);
        ClgPortalFeeDTO getrecdetails(ClgPortalFeeDTO data);
        Task<CollegeFeeTransactionDTO> printreceipt([FromBody] CollegeFeeTransactionDTO sddto);
    }
}
