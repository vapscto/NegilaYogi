using PreadmissionDTOs.com.vaps.Portals.IVRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.IVRS.Interfaces
{
    public interface IVRSMasterLanguagesInterface
    {
        IVRS_Master_LanguagesDTO getdetails(IVRS_Master_LanguagesDTO data);
        IVRS_Master_LanguagesDTO savedetail(IVRS_Master_LanguagesDTO data);
        IVRS_Master_LanguagesDTO getdetails_page(int id);
        IVRS_Master_LanguagesDTO deactivate(IVRS_Master_LanguagesDTO data);
    }
}
