using PreadmissionDTOs.com.vaps.VMS.HRMS;

namespace Recruitment.com.vaps.Interfaces
{
    public interface AppointmentInterface
    {
        HR_Candidate_DetailsDTO getBasicData(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO SaveUpdate(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO editData(int id);
        HR_Candidate_DetailsDTO deactivate(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO Get_Desgination(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO saveAppointmentdata(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO savesalarydata(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO getEmployeeSalaryDetailsByHead(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO getcandidate(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO getCandidateList(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO getcandidatename(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO getcompanydetails(HR_Candidate_DetailsDTO dto);
    }
}
