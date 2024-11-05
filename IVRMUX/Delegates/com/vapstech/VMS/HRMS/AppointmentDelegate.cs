using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class AppointmentDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Candidate_DetailsDTO, HR_Candidate_DetailsDTO> COMMM = new CommonDelegate<HR_Candidate_DetailsDTO, HR_Candidate_DetailsDTO>();

        public HR_Candidate_DetailsDTO onloadgetdetails(HR_Candidate_DetailsDTO dto)
        {
            return COMMM.POSTVMS(dto, "AppointmentFacade/onloadgetdetails");
        }

        public HR_Candidate_DetailsDTO savedetails(HR_Candidate_DetailsDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "AppointmentFacade/");
        }
        public HR_Candidate_DetailsDTO getRecorddetailsById(int id)
        {
            return COMMM.GetVMS(id, "AppointmentFacade/getRecordById/");
        }
        public HR_Candidate_DetailsDTO deleterec(HR_Candidate_DetailsDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "AppointmentFacade/deactivateRecordById/");
        }
        public HR_Candidate_DetailsDTO Get_Desgination(HR_Candidate_DetailsDTO data)
        {
            return COMMM.POSTVMS(data, "AppointmentFacade/Get_Desgination/");
        }
        public HR_Candidate_DetailsDTO saveAppointmentdata(HR_Candidate_DetailsDTO data)
        {
            return COMMM.POSTVMS(data, "AppointmentFacade/saveAppointmentdata/");
        }
        public HR_Candidate_DetailsDTO savesalarydata(HR_Candidate_DetailsDTO data)
        {
            return COMMM.POSTVMS(data, "AppointmentFacade/savesalarydata/");
        }
        public HR_Candidate_DetailsDTO getEmployeeSalaryDetailsByHead(HR_Candidate_DetailsDTO data)
        {
            return COMMM.POSTVMS(data, "AppointmentFacade/getEmployeeSalaryDetailsByHead/");
        }
        public HR_Candidate_DetailsDTO getcandidate(HR_Candidate_DetailsDTO data)
        {
            return COMMM.POSTVMS(data, "AppointmentFacade/getcandidate/");
        }   
        public HR_Candidate_DetailsDTO getCandidateList(HR_Candidate_DetailsDTO data)
        {
            return COMMM.POSTVMS(data, "AppointmentFacade/getCandidateList/");
        }
        public HR_Candidate_DetailsDTO getcandidatename(HR_Candidate_DetailsDTO data)
        {
            return COMMM.POSTVMS(data, "AppointmentFacade/getcandidatename/");
        }
        public HR_Candidate_DetailsDTO getcompanydetails(HR_Candidate_DetailsDTO data)
        {
            return COMMM.POSTVMS(data, "AppointmentFacade/getcompanydetails/");
        }
    }
}
