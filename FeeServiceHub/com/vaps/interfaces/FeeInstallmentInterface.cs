using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.interfaces
{
 public   interface FeeInstallmentInterface
    {
        FeeInstallmentDTO SaveGroupData(FeeInstallmentDTO org);
        FeeInstallmentDTO EditgroupDetails(int id);
        FeeInstallmentDTO getdetails(FeeInstallmentDTO mas);
        FeeInstallmentDTO GetGroupSearchData(FeeInstallmentDTO mas);
        FeeInstallmentDTO getpageedit(int id);
        FeeInstalmentDueDateDTO getpageeditY(int id);
        FeeInstallmentDTO deleterec(int id);
        //FeeInstalmentDueDateDTO deleterecY(int id);
        FeeInstalmentDueDateDTO deleterecY(FeeInstalmentDueDateDTO data);
        FeeInstallmentDTO deactivate(FeeInstallmentDTO id);
        FeeInstallmentyeralyDTO[] GetWrittenTestMarks(FeeInstallmentyeralyDTO mas);
        Task<FeeInstallmentDTO> getIndependentDropDowns(FeeInstallmentDTO yrs);
        FeeInstallmentyeralyDTO[] Getduedates(FeeInstallmentyeralyDTO mas);
        FeeInstallmentDTO savedetailDDD(FeeInstallmentDTO org);
      
    }
}
