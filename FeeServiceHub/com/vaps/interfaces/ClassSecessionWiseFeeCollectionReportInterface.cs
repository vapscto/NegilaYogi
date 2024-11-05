using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.admission;
namespace FeeServiceHub.com.vaps.interfaces
{
  public  interface ClassSecessionWiseFeeCollectionReportInterface
    {
        ClassSecessionWiseFeeCollectionReportDTO getdata123(ClassSecessionWiseFeeCollectionReportDTO data);
        Task<ClassSecessionWiseFeeCollectionReportDTO> getreport(ClassSecessionWiseFeeCollectionReportDTO data);
    
    }
}
