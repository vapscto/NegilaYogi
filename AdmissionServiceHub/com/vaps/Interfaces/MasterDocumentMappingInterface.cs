using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;


namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface MasterDocumentMappingInterface
    {
        MasterDocumentMappingDTO Getdetails(MasterDocumentMappingDTO MasterDocumentMappingDTO);

        MasterDocumentMappingDTO SaveData(MasterDocumentMappingDTO mas);

        MasterDocumentMappingDTO DeleteEntry(int ID);

        MasterDocumentMappingDTO GetSelectedRowDetails(int ID);
    }
}
