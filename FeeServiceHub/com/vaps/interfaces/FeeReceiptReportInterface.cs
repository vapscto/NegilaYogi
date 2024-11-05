using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.admission;
namespace FeeServiceHub.com.vaps.interfaces
{
  public  interface FeeReceiptReportInterface
    {
        FeeReceiptDTO getdata123(FeeReceiptDTO data);
        FeeReceiptDTO getinsdetils(FeeReceiptDTO data);
        Task<FeeReceiptDTO> getreport(FeeReceiptDTO data);
      
    }
}
