using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class AddtoHRMSDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Candidate_DetailsDTO, HR_Candidate_DetailsDTO> COMMM = new CommonDelegate<HR_Candidate_DetailsDTO, HR_Candidate_DetailsDTO>();

        public HR_Candidate_DetailsDTO onloadgetdetails(HR_Candidate_DetailsDTO dto)
        {
            return COMMM.POSTVMS(dto, "AddtoHRMSFacade/onloadgetdetails");
        }

        public HR_Candidate_DetailsDTO savedetails(HR_Candidate_DetailsDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "AddtoHRMSFacade/");
        }
        public HR_Candidate_DetailsDTO getRecorddetailsById(int id)
        {
            return COMMM.GetVMS(id, "AddtoHRMSFacade/getRecordById/");
        }
        public HR_Candidate_DetailsDTO deleterec(HR_Candidate_DetailsDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "AddtoHRMSFacade/deactivateRecordById/");
        }
        public HR_Candidate_DetailsDTO Get_Desgination(HR_Candidate_DetailsDTO data)
        {
            return COMMM.POSTVMS(data, "AddtoHRMSFacade/Get_Desgination/");
        }
        public HR_Candidate_DetailsDTO savedata(HR_Candidate_DetailsDTO data)
        {
            return COMMM.POSTVMS(data, "AddtoHRMSFacade/savetohrms/");
        }
        public HR_Candidate_DetailsDTO savetohrms(HR_Candidate_DetailsDTO data)
        {
            return COMMM.POSTVMS(data, "AddtoHRMSFacade/savetohrms/");
        }
        public HR_Candidate_DetailsDTO getEmployeeSalaryDetailsByHead(HR_Candidate_DetailsDTO data)
        {
            return COMMM.POSTVMS(data, "AddtoHRMSFacade/getEmployeeSalaryDetailsByHead/");
        }
        public HR_Candidate_DetailsDTO getcandidate(HR_Candidate_DetailsDTO data)
        {
            return COMMM.POSTVMS(data, "AddtoHRMSFacade/getcandidate/");
        }   
    }
}
