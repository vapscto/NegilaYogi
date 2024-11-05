using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class HRMasterPANDelegate
    {
    private readonly object resource;
    private readonly string serviceBaseUrl;
    private const String JsonContentType = "application/json; charset=utf-8";
    CommonDelegate<HRMasterPANDTO, HRMasterPANDTO> COMMM = new CommonDelegate<HRMasterPANDTO, HRMasterPANDTO>();

    public HRMasterPANDTO onloadgetdetails(HRMasterPANDTO dto)
    {
      return COMMM.POSTDataHRMS(dto, "HRMasterPANFacade/onloadgetdetails");
    }

    public HRMasterPANDTO savedetails(HRMasterPANDTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "HRMasterPANFacade/savedetails");
    }
    public HRMasterPANDTO getRecorddetailsById(int id)
    {
      return COMMM.GetDataByIdHRMS(id, "HRMasterPANFacade/getRecordById/");
    }
    public HRMasterPANDTO deleterec(HRMasterPANDTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "HRMasterPANFacade/deactivateRecordById/");
    }

  }
}
