using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class MasterEarningsDeductionsDelegate
    {
    private readonly object resource;
    private readonly string serviceBaseUrl;
    private const String JsonContentType = "application/json; charset=utf-8";
    CommonDelegate<HR_Master_EarningsDeductionsDTO, HR_Master_EarningsDeductionsDTO> COMMM = new CommonDelegate<HR_Master_EarningsDeductionsDTO, HR_Master_EarningsDeductionsDTO>();

    public HR_Master_EarningsDeductionsDTO onloadgetdetails(HR_Master_EarningsDeductionsDTO dto)
    {
      return COMMM.POSTDataHRMS(dto, "MasterEarningsDeductionsFacade/onloadgetdetails");
    }

    public HR_Master_EarningsDeductionsDTO savedetails(HR_Master_EarningsDeductionsDTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "MasterEarningsDeductionsFacade/");
    }
    public HR_Master_EarningsDeductionsDTO getRecorddetailsById(int id)
    {
      return COMMM.GetDataByIdHRMS(id, "MasterEarningsDeductionsFacade/getRecordById/");
    }
    public HR_Master_EarningsDeductionsDTO deleterec(HR_Master_EarningsDeductionsDTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "MasterEarningsDeductionsFacade/deactivateRecordById/");
    }

        public HR_Master_EarningsDeductionsDTO orderchangedata(HR_Master_EarningsDeductionsDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "MasterEarningsDeductionsFacade/orderchangedata");
        }


        CommonDelegate<HR_Master_EarningsDeductions_TypeDTO, HR_Master_EarningsDeductions_TypeDTO> COMMMf = new CommonDelegate<HR_Master_EarningsDeductions_TypeDTO, HR_Master_EarningsDeductions_TypeDTO>();
    //type
    public HR_Master_EarningsDeductions_TypeDTO onloadgetdetailstype(HR_Master_EarningsDeductions_TypeDTO dto)
    {
      return COMMMf.POSTDataHRMS(dto, "MasterEarningsDeductionsFacade/onloadgetdetailstype");
    }

    public HR_Master_EarningsDeductions_TypeDTO savedetails(HR_Master_EarningsDeductions_TypeDTO maspage)
    {
      return COMMMf.POSTDataHRMS(maspage, "MasterEarningsDeductionsFacade/savedetails");
    }

    public HR_Master_EarningsDeductions_TypeDTO getRecorddetailsByIdType(int id)
    {
      return COMMMf.GetDataByIdHRMS(id, "MasterEarningsDeductionsFacade/getRecordByIdType/");
    }
    public HR_Master_EarningsDeductions_TypeDTO deleterec(HR_Master_EarningsDeductions_TypeDTO maspage)
    {
      return COMMMf.POSTDataHRMS(maspage, "MasterEarningsDeductionsFacade/deactivateRecordByIdType/");
    }

  }
}
