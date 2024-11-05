using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface CollegecastecategoryInterface
    {
        CollegecastecategoryDTO castecategoryData(CollegecastecategoryDTO mas);
        CollegecastecategoryDTO MasterDeleteModulesData(int ID);
        CollegecastecategoryDTO GetSelectedRowDetails(int ID);
        CollegecastecategoryDTO GetcastecategoryData(CollegecastecategoryDTO castecategoryDTO);
    }
}
