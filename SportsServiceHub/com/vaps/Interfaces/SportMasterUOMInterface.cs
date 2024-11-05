using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
    public interface SportMasterUOMInterface
    {
        SportMasterUOMDTO mastercasteData(SportMasterUOMDTO mas);

        SportMasterUOMDTO deactivate(SportMasterUOMDTO dto);

        SportMasterUOMDTO GetSelectedRowDetails(int ID);

        SportMasterUOMDTO GetmastercasteData(SportMasterUOMDTO SportMasterUOMDTO);
    }
}
