using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface NonBookTransactionInterface
    {
        NonBookTransaction_DTO getdetails(NonBookTransaction_DTO data);
        NonBookTransaction_DTO studentdetails(NonBookTransaction_DTO data);
        NonBookTransaction_DTO get_staff1(NonBookTransaction_DTO data);
        NonBookTransaction_DTO getdepchange(NonBookTransaction_DTO data);
        NonBookTransaction_DTO searchfilter(NonBookTransaction_DTO data);
        NonBookTransaction_DTO searchfilterbarcode(NonBookTransaction_DTO data);
        NonBookTransaction_DTO searchfilterbarcode1(NonBookTransaction_DTO data);
        NonBookTransaction_DTO get_bookdetails(NonBookTransaction_DTO data);
        NonBookTransaction_DTO Savedata(NonBookTransaction_DTO data);
        NonBookTransaction_DTO GetStudentDetails1(NonBookTransaction_DTO data);
        NonBookTransaction_DTO renewaldata(NonBookTransaction_DTO data);
        NonBookTransaction_DTO Editdata(NonBookTransaction_DTO data);
        NonBookTransaction_DTO returndata(NonBookTransaction_DTO data);
        NonBookTransaction_DTO getdetails_smartcard(NonBookTransaction_DTO data);

    }
}
