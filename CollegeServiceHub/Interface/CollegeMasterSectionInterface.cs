using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface CollegeMasterSectionInterface
    {
        CollegeMasterSectionDTO getalldetails(int id);
        CollegeMasterSectionDTO saveMasterdata(CollegeMasterSectionDTO id);
        CollegeMasterSectionDTO Editdetails(CollegeMasterSectionDTO id);
        CollegeMasterSectionDTO saveorder(CollegeMasterSectionDTO id);
        CollegeMasterSectionDTO Deletedetails(CollegeMasterSectionDTO id);

    }
}
