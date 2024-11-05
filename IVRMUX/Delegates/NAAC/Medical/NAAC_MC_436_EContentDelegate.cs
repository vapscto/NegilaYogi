using CommonLibrary;
using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Medical
{
    public class NAAC_MC_436_EContentDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<NAAC_MC_436_EContent_DTO, NAAC_MC_436_EContent_DTO> COMMM = new CommonDelegate<NAAC_MC_436_EContent_DTO, NAAC_MC_436_EContent_DTO>();
        public NAAC_MC_436_EContent_DTO loaddata(NAAC_MC_436_EContent_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_MC_436_EContentFacade/loaddata/");
        }
        public NAAC_MC_436_EContent_DTO savedata(NAAC_MC_436_EContent_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_MC_436_EContentFacade/savedata/");
        }
        public NAAC_MC_436_EContent_DTO editdata(NAAC_MC_436_EContent_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_MC_436_EContentFacade/editdata/");
        }
        public NAAC_MC_436_EContent_DTO deactiveStudent(NAAC_MC_436_EContent_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_MC_436_EContentFacade/deactiveStudent/");
        }
        public NAAC_MC_436_EContent_DTO viewuploadflies(NAAC_MC_436_EContent_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_MC_436_EContentFacade/viewuploadflies");
        }
        public NAAC_MC_436_EContent_DTO deleteuploadfile(NAAC_MC_436_EContent_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_MC_436_EContentFacade/deleteuploadfile");
        }

    }
}
