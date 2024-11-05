using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface CollegemastercasteInterface 
    {
        CollegemastercasteDTO mastercasteData(CollegemastercasteDTO mas);
        CollegemastercasteDTO MasterDeleteModulesData(int ID);
        CollegemastercasteDTO GetSelectedRowDetails(int ID);
        CollegemastercasteDTO GetmastercasteData(CollegemastercasteDTO mastercasteDTO);
    }
}
