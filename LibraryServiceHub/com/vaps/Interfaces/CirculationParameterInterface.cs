using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface CirculationParameterInterface
    {
        Task<CirculationParameterDTO> getdetails(CirculationParameterDTO data); 
        CirculationParameterDTO gettype(CirculationParameterDTO data);
        Task<CirculationParameterDTO> getdata(CirculationParameterDTO data);
        Task<CirculationParameterDTO> Savedata(CirculationParameterDTO data);
        CirculationParameterDTO deactiveY(CirculationParameterDTO data);
        CirculationParameterDTO editdata(CirculationParameterDTO data);
    }
}
