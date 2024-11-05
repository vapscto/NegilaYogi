using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeeReceiptImportInterface
    {
        FeeReceiptImportDTO Savedata(FeeReceiptImportDTO data);
        FeeReceiptImportDTO getdetails(int id);
        FeeReceiptImportDTO deactiveY(FeeReceiptImportDTO data);
    }
}
