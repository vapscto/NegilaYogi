using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class CandidateInterviewListVMSDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_CandidateInterviewScheduleDTO, HR_CandidateInterviewScheduleDTO> COMMM = new CommonDelegate<HR_CandidateInterviewScheduleDTO, HR_CandidateInterviewScheduleDTO>();

        public HR_CandidateInterviewScheduleDTO onloadgetdetails(HR_CandidateInterviewScheduleDTO dto)
        {
            return COMMM.POSTVMS(dto, "CandidateInterviewListVMSFacade/onloadgetdetails");
        }

        public HR_CandidateInterviewScheduleDTO savedetails(HR_CandidateInterviewScheduleDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "AddJobVMSFacade/");
        }
        public HR_CandidateInterviewScheduleDTO getRecorddetailsById(int id)
        {
            return COMMM.GetVMS(id, "CandidateInterviewListVMSFacade/getRecordById/");
        }
        public HR_CandidateInterviewScheduleDTO deleterec(HR_CandidateInterviewScheduleDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "CandidateInterviewListVMSFacade/deactivateRecordById/");
        }

        public HR_CandidateInterviewScheduleDTO getallwithoutcondtn(HR_CandidateInterviewScheduleDTO dto)
        {
            return COMMM.POSTVMS(dto, "CandidateInterviewListVMSFacade/getallwithoutcondtn");
        }        
    }
}
