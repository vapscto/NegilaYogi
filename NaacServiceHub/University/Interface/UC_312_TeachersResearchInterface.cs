using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Interface
{
    public interface UC_312_TeachersResearchInterface
    {
        UC_312_TeachersResearchDTO loaddata(UC_312_TeachersResearchDTO data);
        UC_312_TeachersResearchDTO save(UC_312_TeachersResearchDTO data);
        UC_312_TeachersResearchDTO deactive(UC_312_TeachersResearchDTO data);
        UC_312_TeachersResearchDTO EditData(UC_312_TeachersResearchDTO data);
        UC_312_TeachersResearchDTO viewuploadflies(UC_312_TeachersResearchDTO data);
        UC_312_TeachersResearchDTO deleteuploadfile(UC_312_TeachersResearchDTO data);
        UC_312_TeachersResearchDTO get_dept(UC_312_TeachersResearchDTO data);
        UC_312_TeachersResearchDTO get_emp(UC_312_TeachersResearchDTO data);
    }
}
