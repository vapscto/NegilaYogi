using PreadmissionDTOs.com.vaps.Scholorship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
    public interface  IVRM_Master_ViddyBharthiInterface
    {
        ScholorshipMasterDTO getallDetails(ScholorshipMasterDTO acdto);
        ScholorshipMasterDTO savecountry(ScholorshipMasterDTO acdto);
        ScholorshipStateDTO savestate(ScholorshipStateDTO acdto);
        ScholorshipDitictDTO onchnagestate(ScholorshipDitictDTO acdto);
        ScholorshipDitictDTO saveDistrict(ScholorshipDitictDTO acdto);
        ScholorshipTalukaDTO savetaluka(ScholorshipTalukaDTO acdto);
        ScholorshipMasterDTO deactivateCountry(ScholorshipMasterDTO acdto);
        ScholorshipStateDTO deactivestate(ScholorshipStateDTO acdto);
        ScholorshipDitictDTO deactivedistict(ScholorshipDitictDTO acdto);
        ScholorshipTalukaDTO deactivetaluka(ScholorshipTalukaDTO acdto);

    }


}
