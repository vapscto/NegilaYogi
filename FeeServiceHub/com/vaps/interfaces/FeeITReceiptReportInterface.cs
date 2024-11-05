using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.admission;
namespace FeeServiceHub.com.vaps.interfaces
{
  public  interface FeeITReceiptReportInterface
    {
        FeeITReceiptDTO getdata123(FeeITReceiptDTO data);
        FeeITReceiptDTO getstuddet(FeeITReceiptDTO data);
        Task<FeeITReceiptDTO> getreport(FeeITReceiptDTO data);
        //selectacademicyear
        FeeITReceiptDTO selectacademicyear(FeeITReceiptDTO data);
    }
}
