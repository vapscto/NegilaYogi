using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.PDA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDAServiceHub.com.vaps.interfaces
{
    public interface PDATransactionInterface
    {
        PDATransactionDTO getdetails(PDATransactionDTO data);

        FeeStudentTransactionDTO getsearchfilter(FeeStudentTransactionDTO data);
        PDATransactionDTO Savedata(PDATransactionDTO data);

        PDATransactionDTO searching(PDATransactionDTO data);

        PDATransactionDTO Deletedetails(PDATransactionDTO data);

        PDATransactionDTO getsection(PDATransactionDTO data);
        PDATransactionDTO getstudent(PDATransactionDTO data);

    }
}
