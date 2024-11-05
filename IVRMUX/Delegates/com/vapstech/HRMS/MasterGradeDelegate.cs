using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class MasterGradeDelegate
    {
    private readonly object resource;
    private readonly string serviceBaseUrl;
    private const String JsonContentType = "application/json; charset=utf-8";
    CommonDelegate<HR_Master_GradeDTO, HR_Master_GradeDTO> COMMM = new CommonDelegate<HR_Master_GradeDTO, HR_Master_GradeDTO>();

    public HR_Master_GradeDTO onloadgetdetails(HR_Master_GradeDTO dto)
    {
      return COMMM.POSTDataHRMS(dto, "MasterGradeFacade/onloadgetdetails");
    }

    public HR_Master_GradeDTO Onchangedetails(HR_Master_GradeDTO dto)
    {
      return COMMM.POSTDataHRMS(dto, "MasterGradeFacade/Onchangedetails");
    }


    public HR_Master_GradeDTO savedetails(HR_Master_GradeDTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "MasterGradeFacade/");
    }
    public HR_Master_GradeDTO getRecorddetailsById(int id)
    {
      return COMMM.GetDataByIdHRMS(id, "MasterGradeFacade/getRecordById/");
    }
    public HR_Master_GradeDTO deleterec(HR_Master_GradeDTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "MasterGradeFacade/deactivateRecordById/");
    }

  }
}
