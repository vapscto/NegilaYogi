using PreadmissionDTOs.com.vaps.College.Fee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
  public  interface ClgFeeInstallmentInterface
    {
        Clg_Fee_Installment_DTO SaveGroupData(Clg_Fee_Installment_DTO org);
        Clg_Fee_Installment_DTO EditgroupDetails(int id);
        Clg_Fee_Installment_DTO getdetails(Clg_Fee_Installment_DTO mas);
        Clg_Fee_Installment_DTO GetGroupSearchData(Clg_Fee_Installment_DTO mas);
        Clg_Fee_Installment_DTO getpageedit(int id);
        Clg_Fee_Installment_Due_Date_DTO getpageeditY(int id);
        Clg_Fee_Installment_DTO deleterec(int id);
        Clg_Fee_Installment_Due_Date_DTO deleterecY(int id);
        Clg_Fee_Installment_DTO deactivate(Clg_Fee_Installment_DTO id);
        Clg_Fee_Installments_Yearly_DTO[] GetWrittenTestMarks(Clg_Fee_Installments_Yearly_DTO mas);
        Task<Clg_Fee_Installment_DTO> getIndependentDropDowns(Clg_Fee_Installment_DTO yrs);
        Clg_Fee_Installments_Yearly_DTO[] Getduedates(Clg_Fee_Installments_Yearly_DTO mas);
        Clg_Fee_Installment_DTO savedetailDDD(Clg_Fee_Installment_DTO org);
    }
}
