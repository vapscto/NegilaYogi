using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;

//PreadmissionDTOs.com.vaps.admission


namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface castecategoryInterface
    {
        castecategoryDTO castecategoryData(castecategoryDTO mas);

        castecategoryDTO MasterDeleteModulesData(int ID);

        castecategoryDTO GetSelectedRowDetails(int ID);

        castecategoryDTO GetcastecategoryData(castecategoryDTO castecategoryDTO);
    }
}
