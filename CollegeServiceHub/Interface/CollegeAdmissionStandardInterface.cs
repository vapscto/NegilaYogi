using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface CollegeAdmissionStandardInterface
    {
        CollegeAdmissionStandardDTO getlisttwo(CollegeAdmissionStandardDTO stu);
        CollegeAdmissionStandardDTO getlistdata(int id);
    }
}
