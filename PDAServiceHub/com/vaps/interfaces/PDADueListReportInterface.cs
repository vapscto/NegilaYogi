using PreadmissionDTOs.com.vaps.PDA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDAServiceHub.com.vaps.interfaces
{
  public interface PDADueListReportInterface
    {
        PDATransactionDTO getalldetails(PDATransactionDTO data);

        PDATransactionDTO getsection(PDATransactionDTO data);

        PDATransactionDTO getstudent(PDATransactionDTO data);

        Task<PDATransactionDTO> radiobtndata(PDATransactionDTO data);
    }
}
