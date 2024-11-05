using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class CandidateListVMSDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Candidate_DetailsDTO, HR_Candidate_DetailsDTO> COMMM = new CommonDelegate<HR_Candidate_DetailsDTO, HR_Candidate_DetailsDTO>();

        public HR_Candidate_DetailsDTO onloadgetdetails(HR_Candidate_DetailsDTO dto)
        {
            return COMMM.POSTVMS(dto, "CandidateListVMSFacade/onloadgetdetails");
        }

        public HR_Candidate_DetailsDTO savedetails(HR_Candidate_DetailsDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "AddJobVMSFacade/");
        }
        public HR_Candidate_DetailsDTO getRecorddetailsById(int id)
        {
            return COMMM.GetVMS(id, "CandidateListVMSFacade/getRecordById/");
        }
        public HR_Candidate_DetailsDTO deleterec(HR_Candidate_DetailsDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "CandidateListVMSFacade/deactivateRecordById/");
        }
    }
}
