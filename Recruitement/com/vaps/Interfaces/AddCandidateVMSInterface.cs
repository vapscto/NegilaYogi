using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.VMS.HRMS;

namespace Recruitment.com.vaps.Interfaces
{
    public interface AddCandidateVMSInterface
    {
        HR_Candidate_DetailsDTO getBasicData(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO paynow(HR_Candidate_DetailsDTO dt);
        PaymentDetails payuresponse(PaymentDetails stu);
        PaymentDetails razorgetpaymentresponse(PaymentDetails stu);
        HR_Candidate_DetailsDTO SaveUpdate(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO editData(int id);
        HR_Candidate_DetailsDTO deactivate(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO Get_Desgination(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO saveAppointmentdata(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO addQualification(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO addexperience(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO addfamily(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO addlanguage(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO sendCallLettermail(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO saveAppointmenttab(HR_Candidate_DetailsDTO dto);
    }
}
