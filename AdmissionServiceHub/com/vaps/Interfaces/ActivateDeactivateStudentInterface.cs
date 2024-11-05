using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface ActivateDeactivateStudentInterface
    {
        ActivateDeactivateStudentDTO getdetails(int id);
        ActivateDeactivateStudentDTO getlistone(ActivateDeactivateStudentDTO id);
        Task<ActivateDeactivateStudentDTO> getlisttwo(ActivateDeactivateStudentDTO stu);
        ActivateDeactivateStudentDTO getlistthree(ActivateDeactivateStudentDTO stu);
    }
}
