using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface VaccineAgeCriteriaInterface
    {
        VaccineAgeCriteriaDTO OnLoadVaccineAgeCriteriaDetails(VaccineAgeCriteriaDTO data);
        VaccineAgeCriteriaDTO SaveVaccineAgeDetails(VaccineAgeCriteriaDTO data);
        VaccineAgeCriteriaDTO EditVaccineAgeDetails(VaccineAgeCriteriaDTO data);
        VaccineAgeCriteriaDTO ActiveDeactiveVaccineAgeDetails(VaccineAgeCriteriaDTO data);
        VaccineAgeCriteriaDTO OnClickViewDetails(VaccineAgeCriteriaDTO data);
        VaccineAgeCriteriaDTO ActiveDeactiveVaccineDetails(VaccineAgeCriteriaDTO data);

        // Vaccine Student Details
        VaccineAgeCriteriaDTO OnLoadVaccineStudentDetails(VaccineAgeCriteriaDTO data);
        VaccineAgeCriteriaDTO GetStudentDetailsBySearch(VaccineAgeCriteriaDTO data);
        VaccineAgeCriteriaDTO SearchVaccineStudentDetails(VaccineAgeCriteriaDTO data);
        VaccineAgeCriteriaDTO SaveStudentVaccineDetails(VaccineAgeCriteriaDTO data);
        VaccineAgeCriteriaDTO OnClickViewStudentVaccineDetails(VaccineAgeCriteriaDTO data);
        Task<VaccineAgeCriteriaDTO> VaccineDueDateWebJobsApi(VaccineAgeCriteriaDTO data);
        VaccineAgeCriteriaDTO OnLoadIllnessStudentDetails(VaccineAgeCriteriaDTO data);

    }
}
