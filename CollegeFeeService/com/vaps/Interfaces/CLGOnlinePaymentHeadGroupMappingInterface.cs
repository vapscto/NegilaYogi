using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.College.Fees;
using PreadmissionDTOs.com.vaps.College.Fee;

namespace CollegeFeeService.com.vaps.Interfaces
{
  public  interface CLGOnlinePaymentHeadGroupMappingInterface
    {
        Clg_StudentFeeGroupMapping_DTO onlineMappingDetails(int id);
        Clg_StudentFeeGroupMapping_DTO editDetails(int id);
        Clg_StudentFeeGroupMapping_DTO deleteDetails(int id);
        Clg_StudentFeeGroupMapping_DTO groupselection(Clg_StudentFeeGroupMapping_DTO data);
        Clg_StudentFeeGroupMapping_DTO headsel(Clg_StudentFeeGroupMapping_DTO data);
        Clg_StudentFeeGroupMapping_DTO saveDetails(Clg_StudentFeeGroupMapping_DTO data); 
        Clg_StudentFeeGroupMapping_DTO academicsel(Clg_StudentFeeGroupMapping_DTO data);

    }
}
