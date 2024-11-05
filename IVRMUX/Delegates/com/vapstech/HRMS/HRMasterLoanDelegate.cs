using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class HRMasterLoanDelegate
    {
    private readonly object resource;
    private readonly string serviceBaseUrl;
    private const String JsonContentType = "application/json; charset=utf-8";
    CommonDelegate<HRMasterLoanDTO, HRMasterLoanDTO> COMMM = new CommonDelegate<HRMasterLoanDTO, HRMasterLoanDTO>();

    public HRMasterLoanDTO onloadgetdetails(HRMasterLoanDTO dto)
    {
      return COMMM.POSTDataHRMS(dto, "HRMasterLoanFacade/onloadgetdetails");
    }

    public HRMasterLoanDTO savedetails(HRMasterLoanDTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "HRMasterLoanFacade/");
    }
    public HRMasterLoanDTO getRecorddetailsById(int id)
    {
      return COMMM.GetDataByIdHRMS(id, "HRMasterLoanFacade/getRecordById/");
    }
    public HRMasterLoanDTO deleterec(HRMasterLoanDTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "HRMasterLoanFacade/deactivateRecordById/");
    }

  }
}
