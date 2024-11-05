using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
   
    public class MasterChapterVIDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterChapterVIDTO, MasterChapterVIDTO> COMMM = new CommonDelegate<MasterChapterVIDTO, MasterChapterVIDTO>();

        public MasterChapterVIDTO onloadgetdetails(MasterChapterVIDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "HRMasterChapterVIFacade/onloadgetdetails");
        }

        public MasterChapterVIDTO savedetails(MasterChapterVIDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "HRMasterChapterVIFacade/");
        }
        public MasterChapterVIDTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "HRMasterChapterVIFacade/getRecordById/");
        }
        public MasterChapterVIDTO deleterec(MasterChapterVIDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "HRMasterChapterVIFacade/deactivateRecordById/");
        }
        public MasterChapterVIDTO validateordernumber(MasterChapterVIDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "HRMasterChapterVIFacade/validateordernumber/");

        }
    }
}
