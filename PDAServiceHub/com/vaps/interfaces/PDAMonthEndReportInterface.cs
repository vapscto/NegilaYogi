using PreadmissionDTOs.com.vaps.PDA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDAServiceHub.com.vaps.interfaces
{
  public interface PDAMonthEndReportInterface
    {
        PDATransactionDTO getdata123(PDATransactionDTO data);
        Task<PDATransactionDTO> getreport(PDATransactionDTO data);
    }
}
