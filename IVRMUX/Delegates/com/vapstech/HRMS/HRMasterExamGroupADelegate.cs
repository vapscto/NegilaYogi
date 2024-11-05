using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class HRMasterExamGroupADelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_MasterExam_GroupADTO, HR_MasterExam_GroupADTO> COMMM = new CommonDelegate<HR_MasterExam_GroupADTO, HR_MasterExam_GroupADTO>();

        public HR_MasterExam_GroupADTO onloadgetdetails(HR_MasterExam_GroupADTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "HRMasterExamGroupAFacade/onloadgetdetails");
        }

        public HR_MasterExam_GroupADTO savedetails(HR_MasterExam_GroupADTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "HRMasterExamGroupAFacade/");
        }
        public HR_MasterExam_GroupADTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "HRMasterExamGroupAFacade/getRecordById/");
        }
        public HR_MasterExam_GroupADTO deleterec(HR_MasterExam_GroupADTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "HRMasterExamGroupAFacade/deactivateRecordById/");
        }

    }
}
