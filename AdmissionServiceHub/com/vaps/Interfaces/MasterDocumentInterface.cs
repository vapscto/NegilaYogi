using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;


namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface MasterDocumentInterface
    {
        MasterDocumentDTO Getdetails(MasterDocumentDTO MasterDocumentDTO);

        MasterDocumentDTO SaveData(MasterDocumentDTO mas);

        MasterDocumentDTO DeleteEntry(int ID);

        MasterDocumentDTO GetSelectedRowDetails(int ID);
    }
}
