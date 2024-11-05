using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.PDA;

namespace PDAServiceHub.com.vaps.interfaces
{
    public interface PDAReportInterface
    {
        PDATransactionDTO getalldetails(PDATransactionDTO data);

        Task<PDATransactionDTO> radiobtndata(PDATransactionDTO data);
        PDATransactionDTO getsection(PDATransactionDTO data);
        PDATransactionDTO getstudent(PDATransactionDTO data);
    }
}

