using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.interfaces
{
 public   interface FeePrevilegeInterface
    {
        FeePrevilegeDTO getdetails(FeePrevilegeDTO data);
        FeePrevilegeDTO getusername(FeePrevilegeDTO data);
        FeePrevilegeDTO delete(int id);
        FeePrevilegeDTO savedetail(FeePrevilegeDTO objcategory);
        FeePrevilegeDTO edit(int id);

        FeePrevilegeDTO fillheadsinterface(FeePrevilegeDTO data);

        //FeeInstallmentDTO SaveGroupData(FeeInstallmentDTO org);
        //FeeInstallmentDTO EditgroupDetails(int id);

        //FeeInstallmentDTO GetGroupSearchData(FeeInstallmentDTO mas);

        //FeeInstalmentDueDateDTO getpageeditY(int id);
        //FeeInstallmentDTO deleterec(int id);
        //FeeInstalmentDueDateDTO deleterecY(int id);
        //FeeInstallmentDTO deactivate(FeeInstallmentDTO id);
        //FeeInstallmentyeralyDTO[] GetWrittenTestMarks(FeeInstallmentyeralyDTO mas);
        //Task<FeeInstallmentDTO> getIndependentDropDowns(FeeInstallmentDTO yrs);
        //FeeInstallmentyeralyDTO[] Getduedates(FeeInstallmentyeralyDTO mas);
        //FeeInstallmentDTO savedetailDDD(FeeInstallmentDTO org);

    }
}
