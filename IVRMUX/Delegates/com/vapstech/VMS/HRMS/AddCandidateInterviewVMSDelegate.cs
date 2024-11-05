using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class AddCandidateInterviewVMSDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_CandidateInterviewScheduleDTO, HR_CandidateInterviewScheduleDTO> COMMM = new CommonDelegate<HR_CandidateInterviewScheduleDTO, HR_CandidateInterviewScheduleDTO>();

        public HR_CandidateInterviewScheduleDTO onloadgetdetails(HR_CandidateInterviewScheduleDTO dto)
        {
            return COMMM.POSTVMS(dto, "AddCandidateInterviewVMSFacade/onloadgetdetails");
        }
        public HR_CandidateInterviewScheduleDTO getallgrade(HR_CandidateInterviewScheduleDTO dto)
        {
            return COMMM.POSTVMS(dto, "AddCandidateInterviewVMSFacade/getallgrade");
        }

        public HR_CandidateInterviewScheduleDTO savedetails(HR_CandidateInterviewScheduleDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "AddCandidateInterviewVMSFacade/");
        }
        public HR_CandidateInterviewScheduleDTO getRecorddetailsById(int id)
        {
            return COMMM.GetVMS(id, "AddCandidateInterviewVMSFacade/getRecordById/");
        }
        public HR_CandidateInterviewScheduleDTO deleterec(HR_CandidateInterviewScheduleDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "AddCandidateInterviewVMSFacade/deactivateRecordById/");
        }
        public HR_CandidateInterviewScheduleDTO getrpt(HR_CandidateInterviewScheduleDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "AddCandidateInterviewVMSFacade/getrpt/");
        }
        public HR_CandidateInterviewScheduleDTO savefeedback(HR_CandidateInterviewScheduleDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "AddCandidateInterviewVMSFacade/savefeedback/");
        }
        
    }
}
