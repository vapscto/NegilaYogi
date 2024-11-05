using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeeReceiptImportStthomasInterface
    {
       // FeeReceiptImportStthomasDTO Savedata(FeeReceiptImportStthomasDTO data);
        Task<FeeReceiptImportStthomasDTO> Savedata(FeeReceiptImportStthomasDTO data);
        FeeReceiptImportStthomasDTO getdetails(int id);
        FeeReceiptImportStthomasDTO deactiveY(FeeReceiptImportStthomasDTO data);
        //Added By PraveenGouda
        FeeReceiptImportStthomasDTO getreportdetails(FeeReceiptImportStthomasDTO data);
        FeeReceiptImportStthomasDTO deletereceipt(FeeReceiptImportStthomasDTO data);

    }
}
