using PreadmissionDTOs.com.vaps.PDA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDAServiceHub.com.vaps.interfaces
{
   public interface PDARefundableReportInterface
    {
        PDATransactionDTO getalldetails(PDATransactionDTO data);
        Task<PDATransactionDTO> radiobtndata(PDATransactionDTO data);
    }
}
