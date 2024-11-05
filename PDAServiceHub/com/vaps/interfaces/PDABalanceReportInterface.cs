using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.PDA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace PDAServiceHub.com.vaps.interfaces
{
   public interface PDABalanceReportInterface
    {
        PDATransactionDTO getalldetails(PDATransactionDTO data);
    }
}
