using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface ECSBulkImportInterface
    {
         ECSBulkImportDTO getdata123(ECSBulkImportDTO data);
        Task<ECSBulkImportDTO> checkvalidation(ECSBulkImportDTO data);

    }
}
