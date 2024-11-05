using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface HostelFoodConveyanceReportInterface
    {
        Adm_M_StudentDTO getdetails(Adm_M_StudentDTO stu);
        //Adm_M_StudentDTO getStudDetails(Adm_M_StudentDTO studData);
        Task<Adm_M_StudentDTO> getStudDetails(Adm_M_StudentDTO studData);
        //ActivateDeactivateStudentDTO getlistone(int id);
        //ActivateDeactivateStudentDTO getlisttwo(ActivateDeactivateStudentDTO stu);
        //ActivateDeactivateStudentDTO getlistthree(ActivateDeactivateStudentDTO stu);
    }
}
