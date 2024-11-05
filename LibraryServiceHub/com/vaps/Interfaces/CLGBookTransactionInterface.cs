using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface CLGBookTransactionInterface
    {
        CLGBookTransactionDTO getdetails(CLGBookTransactionDTO data);
        CLGBookTransactionDTO studentdetails(CLGBookTransactionDTO data);
        CLGBookTransactionDTO get_staff1(CLGBookTransactionDTO data);
        CLGBookTransactionDTO getdepchange(CLGBookTransactionDTO data);
        CLGBookTransactionDTO searchfilter(CLGBookTransactionDTO data);
        CLGBookTransactionDTO stdSearch_Grid(CLGBookTransactionDTO data);
        CLGBookTransactionDTO searchfilterbarcode(CLGBookTransactionDTO data);
        CLGBookTransactionDTO searchfilterbarcode1(CLGBookTransactionDTO data);
        CLGBookTransactionDTO get_bookdetails(CLGBookTransactionDTO data);
        CLGBookTransactionDTO Savedata(CLGBookTransactionDTO data);
        CLGBookTransactionDTO GetStudentDetails1(CLGBookTransactionDTO data);
        CLGBookTransactionDTO renewaldata(CLGBookTransactionDTO data);
        CLGBookTransactionDTO Editdata(CLGBookTransactionDTO data);
        CLGBookTransactionDTO returndata(CLGBookTransactionDTO data);
        CLGBookTransactionDTO showfine(CLGBookTransactionDTO data);
        CLGBookTransactionDTO getdetails_smartcard(CLGBookTransactionDTO data);
    }
}
