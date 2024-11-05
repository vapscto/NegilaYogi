using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class HREmpOtherIncomeDelegate
    {

    private readonly object resource;
    private readonly string serviceBaseUrl;
    private const String JsonContentType = "application/json; charset=utf-8";
    CommonDelegate<HR_Emp_otherIncomeDTO, HR_Emp_otherIncomeDTO> COMMM = new CommonDelegate<HR_Emp_otherIncomeDTO, HR_Emp_otherIncomeDTO>();

    public HR_Emp_otherIncomeDTO onloadgetdetails(HR_Emp_otherIncomeDTO dto)
    {
      return COMMM.POSTDataHRMS(dto, "HREmpOtherIncomeFacade/onloadgetdetails");
    }

    public HR_Emp_otherIncomeDTO savedetails(HR_Emp_otherIncomeDTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "HREmpOtherIncomeFacade/");
    }
    public HR_Emp_otherIncomeDTO getRecorddetailsById(int id)
    {
      return COMMM.GetDataByIdHRMS(id, "HREmpOtherIncomeFacade/getRecordById/");
    }
    public HR_Emp_otherIncomeDTO deleterec(HR_Emp_otherIncomeDTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "HREmpOtherIncomeFacade/deactivateRecordById/");
    }
        public HR_Emp_otherIncomeDTO getDetailsByEmployee(HR_Emp_otherIncomeDTO maspage)
            {
            return COMMM.POSTDataHRMS(maspage, "HREmpOtherIncomeFacade/getDetailsByEmployee/");
            }

        }
}
