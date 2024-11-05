using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface SchoolTpinGenreationInterface
    {
        SchoolTpinGenreationDTO loaddata(SchoolTpinGenreationDTO data);
        SchoolTpinGenreationDTO search(SchoolTpinGenreationDTO data);
        SchoolTpinGenreationDTO generatetpin(SchoolTpinGenreationDTO data);
    }
}
