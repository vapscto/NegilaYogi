using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.PDA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PDAServiceHub.com.vaps.interfaces
{
  public  interface PDARefundInterface
    {
        
        PDATransactionDTO getdetails(PDATransactionDTO data);
        FeeStudentTransactionDTO getsearchfilter(FeeStudentTransactionDTO data);
        PDATransactionDTO getstuddetails(PDATransactionDTO data);
        PDATransactionDTO Savedata(PDATransactionDTO data);
        PDATransactionDTO searching(PDATransactionDTO data);
        PDATransactionDTO Deletedetails(PDATransactionDTO data);
        PDATransactionDTO getdatastuacad(PDATransactionDTO data);
    }
}
