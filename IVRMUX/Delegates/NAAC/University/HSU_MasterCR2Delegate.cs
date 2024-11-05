using CommonLibrary;
using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.University
{
    public class HSU_MasterCR2Delegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HSU_MasterCR2_DTO, HSU_MasterCR2_DTO> COMMM = new CommonDelegate<HSU_MasterCR2_DTO, HSU_MasterCR2_DTO>();


        public HSU_MasterCR2_DTO loaddata(HSU_MasterCR2_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "HSU_MasterCR2Facade/loaddata/");
        }
        public HSU_MasterCR2_DTO save_HSU_221(HSU_MasterCR2_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "HSU_MasterCR2Facade/save_HSU_221/");
        }
        public HSU_MasterCR2_DTO save_HSU_232(HSU_MasterCR2_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "HSU_MasterCR2Facade/save_HSU_232/");
        }
        public HSU_MasterCR2_DTO save_HSU_255(HSU_MasterCR2_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "HSU_MasterCR2Facade/save_HSU_255/");
        }

    }
}
