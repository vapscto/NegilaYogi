using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
   public interface DocumentViewInterface
    {
        DocumentViewDTO getInitailData(DocumentViewDTO id);
        DocumentViewDTO getDpData(DocumentViewDTO id);
        DocumentViewDTO getdocksonly(DocumentViewDTO id);
        DocumentViewDTO StatusGetdetails(DocumentViewDTO id);
        DocumentViewDTO mastersaveDTO(DocumentViewDTO id);
        DocumentViewDTO GetSelectedRowDetails(int id);
        DocumentViewDTO MasterDeleteModulesData(int id);
        


    }
}
