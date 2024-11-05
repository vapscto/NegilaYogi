using CommonLibrary;
using PreadmissionDTOs.com.vaps.Scholorship;
using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VidyaBharathi
{
    public class VBadminDelegate
    {
        //private const String JsonContentType = "application/json; charset=utf-8";
      
        CommonDelegate<VBadminDTO, VBadminDTO> COMMC = new CommonDelegate<VBadminDTO, VBadminDTO>();
        public VBadminDTO LoadData(VBadminDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "VBadminFacade/loaddata/");
        }

        //POSTDataClubManagement
        public VBadminDTO ViewCOEDetails(VBadminDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "VBadminFacade/ViewCOEDetails/");
        }
       
    }
}
