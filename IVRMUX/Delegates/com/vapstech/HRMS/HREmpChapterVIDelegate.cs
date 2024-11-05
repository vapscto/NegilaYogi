using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class HREmpChapterVIDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Emp_ChapterVIDTO, HR_Emp_ChapterVIDTO> COMMM = new CommonDelegate<HR_Emp_ChapterVIDTO, HR_Emp_ChapterVIDTO>();

        public HR_Emp_ChapterVIDTO onloadgetdetails(HR_Emp_ChapterVIDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "HREmpChapterVIFacade/onloadgetdetails");
        }

        public HR_Emp_ChapterVIDTO savedetails(HR_Emp_ChapterVIDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "HREmpChapterVIFacade/");
        }
        public HR_Emp_ChapterVIDTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "HREmpChapterVIFacade/getRecordById/");
        }
        public HR_Emp_ChapterVIDTO deleterec(HR_Emp_ChapterVIDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "HREmpChapterVIFacade/deactivateRecordById/");
        }
        public HR_Emp_ChapterVIDTO getDetailsByEmployee(HR_Emp_ChapterVIDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "HREmpChapterVIFacade/validateordernumber/");
        }



      
    }
}
