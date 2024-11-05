using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostelServiceHub.Interface
{
    public interface HostelAllotForCLGStudentInterface
    {

        Task<HostelAllotForCLGStudentDTO> loaddata(HostelAllotForCLGStudentDTO data);
        HostelAllotForCLGStudentDTO savedata(HostelAllotForCLGStudentDTO data);
        Task<HostelAllotForCLGStudentDTO> get_studInfo(HostelAllotForCLGStudentDTO data);
        HostelAllotForCLGStudentDTO floor(HostelAllotForCLGStudentDTO data);
        HostelAllotForCLGStudentDTO room(HostelAllotForCLGStudentDTO data);
        HostelAllotForCLGStudentDTO roomForVacateReport(HostelAllotForCLGStudentDTO data);
        HostelAllotForCLGStudentDTO roomdetails(HostelAllotForCLGStudentDTO data);
        HostelAllotForCLGStudentDTO get_roomdetails(HostelAllotForCLGStudentDTO data);
        Task<HostelAllotForCLGStudentDTO> editdata(HostelAllotForCLGStudentDTO data);
        HostelAllotForCLGStudentDTO requestApproved(HostelAllotForCLGStudentDTO data);
        HostelAllotForCLGStudentDTO requestRejected(HostelAllotForCLGStudentDTO data);
        //HostelT
        HL_Hostel_Student_Transfer_CollegeDTO HostelT(HL_Hostel_Student_Transfer_CollegeDTO data);

        HostelAllotForCLGStudentDTO get_course(HostelAllotForCLGStudentDTO data);
        HostelAllotForCLGStudentDTO get_branch(HostelAllotForCLGStudentDTO data);
        HostelAllotForCLGStudentDTO get_sem(HostelAllotForCLGStudentDTO data);
        //HostelAllotForCLGStudentDTO get_sec(HostelAllotForCLGStudentDTO data);
        HostelAllotForCLGStudentDTO get_student(HostelAllotForCLGStudentDTO data);

    }
}


