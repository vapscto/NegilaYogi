using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface CLGSubjectSchemeTypeInterface
    {
        AdmCollegeSchemeTypeDTO savetype(AdmCollegeSchemeTypeDTO data);
        AdmCollegeSchemeTypeDTO savename(AdmCollegeSchemeTypeDTO data);
        AdmCollegeSchemeTypeDTO getschema(AdmCollegeSchemeTypeDTO data);
        AdmCollegeSchemeTypeDTO getsubject(AdmCollegeSchemeTypeDTO data);
        AdmCollegeSchemeTypeDTO activedeactivebatch(AdmCollegeSchemeTypeDTO data);
        AdmCollegeSchemeTypeDTO activedeactivebatch1(AdmCollegeSchemeTypeDTO data);
        Adm_Prv_Sch_CombinationDTO getcombination(Adm_Prv_Sch_CombinationDTO data);
        Adm_Prv_Sch_CombinationDTO savecombination(Adm_Prv_Sch_CombinationDTO data);
        Adm_Prv_Sch_CombinationDTO activedeactivecomb(Adm_Prv_Sch_CombinationDTO data);
        
    }
}
