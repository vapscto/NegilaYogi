using CommonLibrary;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class AddCandidateVMSDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Candidate_DetailsDTO, HR_Candidate_DetailsDTO> COMMM = new CommonDelegate<HR_Candidate_DetailsDTO, HR_Candidate_DetailsDTO>();
        CommonDelegate<PaymentDetails, PaymentDetails> pay = new CommonDelegate<PaymentDetails, PaymentDetails>();

        public HR_Candidate_DetailsDTO onloadgetdetails(HR_Candidate_DetailsDTO dto)
        {
            return COMMM.POSTVMS(dto, "AddCandidateVMSFacade/onloadgetdetails");
        }
        public HR_Candidate_DetailsDTO getallgrade(HR_Candidate_DetailsDTO dto)
        {
            return COMMM.POSTVMS(dto, "AddCandidateVMSFacade/getallgrade");
        }

        public HR_Candidate_DetailsDTO savedetails(HR_Candidate_DetailsDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "AddCandidateVMSFacade/");
        }
        public HR_Candidate_DetailsDTO getRecorddetailsById(int id)
        {
            return COMMM.GetVMS(id, "AddCandidateVMSFacade/getRecordById/");
        }

        public HR_Candidate_DetailsDTO paynow(HR_Candidate_DetailsDTO dt)
        {
            return COMMM.POSTVMS(dt, "AddCandidateVMSFacade/paynow/");
        }

        public PaymentDetails getpaymentresponse(PaymentDetails response)
        {
            return pay.POSTVMS(response, "AddCandidateVMSFacade/getpaymentresponse/");
        }

        public PaymentDetails razorgetpaymentresponse(PaymentDetails response)
        {

            return pay.POSTVMS(response, "AddCandidateVMSFacade/razorgetpaymentresponse/");

        }
        public HR_Candidate_DetailsDTO deleterec(HR_Candidate_DetailsDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "AddCandidateVMSFacade/deactivateRecordById/");
        }
        public HR_Candidate_DetailsDTO Get_Desgination(HR_Candidate_DetailsDTO data)
        {
            return COMMM.POSTVMS(data, "AddCandidateVMSFacade/Get_Desgination/");
        }
        public HR_Candidate_DetailsDTO saveAppointmentdata(HR_Candidate_DetailsDTO data)
        {
            return COMMM.POSTVMS(data, "AddCandidateVMSFacade/saveAppointmentdata/");
        }
        public HR_Candidate_DetailsDTO addQualification(HR_Candidate_DetailsDTO data)
        {
            return COMMM.POSTVMS(data, "AddCandidateVMSFacade/addQualification/");
        }
        public HR_Candidate_DetailsDTO addexperience(HR_Candidate_DetailsDTO data)
        {
            return COMMM.POSTVMS(data, "AddCandidateVMSFacade/addexperience/");
        }
        public HR_Candidate_DetailsDTO addfamily(HR_Candidate_DetailsDTO data)
        {
            return COMMM.POSTVMS(data, "AddCandidateVMSFacade/addfamily/");
        }
        public HR_Candidate_DetailsDTO addlanguage(HR_Candidate_DetailsDTO data)
        {
            return COMMM.POSTVMS(data, "AddCandidateVMSFacade/addlanguage/");
        }
        public HR_Candidate_DetailsDTO sendCallLettermail(HR_Candidate_DetailsDTO data)
        {
            return COMMM.POSTVMS(data, "AddCandidateVMSFacade/sendCallLettermail/");
        }
        public HR_Candidate_DetailsDTO saveAppointmenttab(HR_Candidate_DetailsDTO data)
        {
            return COMMM.POSTVMS(data, "AddCandidateVMSFacade/saveAppointmenttab/");
        }
    }
}
