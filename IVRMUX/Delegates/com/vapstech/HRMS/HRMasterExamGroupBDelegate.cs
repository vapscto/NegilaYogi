using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class HRMasterExamGroupBDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_MasterExam_GroupBDTO, HR_MasterExam_GroupBDTO> COMMM = new CommonDelegate<HR_MasterExam_GroupBDTO, HR_MasterExam_GroupBDTO>();

        public HR_MasterExam_GroupBDTO onloadgetdetails(HR_MasterExam_GroupBDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "HRMasterExamGroupBFacade/onloadgetdetails");
        }

        public HR_MasterExam_GroupBDTO savedetails(HR_MasterExam_GroupBDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "HRMasterExamGroupBFacade/");
        }
        public HR_MasterExam_GroupBDTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "HRMasterExamGroupBFacade/getRecordById/");
        }
        public HR_MasterExam_GroupBDTO deleterec(HR_MasterExam_GroupBDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "HRMasterExamGroupBFacade/deactivateRecordById/");
        }

    }
}
