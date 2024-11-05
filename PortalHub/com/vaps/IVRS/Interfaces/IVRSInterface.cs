using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.IVRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.IVRS.Interfaces
{
    public interface IVRSInterface
    {
        JsonResult getdata(IVRSDTO data);
        JsonResult getbranch(IVRSDTO data);
        JsonResult updatecredits(IVRSDTO data);
        JsonResult UpdateMobile(IVRSDTO data);
        IVRM_IVRS_ConfigurationDTO savedetail(IVRM_IVRS_ConfigurationDTO data);
        IVRSDTO getdetails(IVRSDTO data);
        IVRSDTO getpageedit(int id);
      IVRM_IVRS_ConfigurationDTO getdetails_page(int id);
        IVRM_IVRS_ConfigurationDTO deactivate(IVRM_IVRS_ConfigurationDTO data);
        IVRM_IVRS_ConfigurationDTO student_staff_notification(IVRM_IVRS_ConfigurationDTO data);
    }
}
