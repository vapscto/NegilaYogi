using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaBharathiServiceHub.com.vaps.Interfaces
{
    public interface VBSC_Master_SportsCCNameInterface
    {
        VBSC_Master_SportsCCNameDTO getloaddata(VBSC_Master_SportsCCNameDTO data);
        VBSC_Master_SportsCCNameDTO getInstitute(VBSC_Master_SportsCCNameDTO data);
        VBSC_Master_SportsCCNameDTO savedetails(VBSC_Master_SportsCCNameDTO data);
        VBSC_Master_SportsCCNameDTO deactive(VBSC_Master_SportsCCNameDTO data);
    }
}
