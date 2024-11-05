using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaBharathiServiceHub.com.vaps.Interfaces
{
    public interface VBSC_Master_SportsCCGroupNameInterface
    {
        VBSC_Master_SportsCCGroupNameDTO getloaddata(VBSC_Master_SportsCCGroupNameDTO data);
        VBSC_Master_SportsCCGroupNameDTO savedetails(VBSC_Master_SportsCCGroupNameDTO data);
        VBSC_Master_SportsCCGroupNameDTO deactive(VBSC_Master_SportsCCGroupNameDTO data);

    }
}
