using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
   public interface MasterReligionInterface
    {
        MasterReligionDTO getdetails();
        MasterReligionDTO saveData(MasterReligionDTO data);
        MasterReligionDTO Edit(int id);
        MasterReligionDTO deleterec(int id);
        MasterReligionDTO deactivate(MasterReligionDTO dto);
        MasterReligionDTO searchByColumn(MasterReligionDTO dto);
        


    }
}
