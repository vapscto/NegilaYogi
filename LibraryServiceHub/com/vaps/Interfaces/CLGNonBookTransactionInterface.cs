using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface CLGNonBookTransactionInterface
    {

        ClgNonBookTransaction_DTO getdetails(ClgNonBookTransaction_DTO data);
        ClgNonBookTransaction_DTO studentdetails(ClgNonBookTransaction_DTO data);
        ClgNonBookTransaction_DTO get_staff1(ClgNonBookTransaction_DTO data);
        ClgNonBookTransaction_DTO getdepchange(ClgNonBookTransaction_DTO data);
        ClgNonBookTransaction_DTO searchfilter(ClgNonBookTransaction_DTO data);
        ClgNonBookTransaction_DTO searchfilterbarcode(ClgNonBookTransaction_DTO data);
        ClgNonBookTransaction_DTO searchfilterbarcode1(ClgNonBookTransaction_DTO data);
        ClgNonBookTransaction_DTO get_bookdetails(ClgNonBookTransaction_DTO data);
        ClgNonBookTransaction_DTO Savedata(ClgNonBookTransaction_DTO data);
        ClgNonBookTransaction_DTO GetStudentDetails1(ClgNonBookTransaction_DTO data);
        ClgNonBookTransaction_DTO renewaldata(ClgNonBookTransaction_DTO data);
        ClgNonBookTransaction_DTO Editdata(ClgNonBookTransaction_DTO data);
        ClgNonBookTransaction_DTO returndata(ClgNonBookTransaction_DTO data);
        ClgNonBookTransaction_DTO getdetails_smartcard(ClgNonBookTransaction_DTO data);

    }
}
