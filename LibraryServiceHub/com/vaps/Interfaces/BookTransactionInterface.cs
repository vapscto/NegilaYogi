using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface BookTransactionInterface
    {
        BookTransactionDTO getdetails(BookTransactionDTO data);
        //getdetailsReturn
        BookTransactionDTO getdetailsReturn(BookTransactionDTO data);
        BookTransactionDTO studentdetails(BookTransactionDTO data);
        BookTransactionDTO get_staff1(BookTransactionDTO data);
        BookTransactionDTO getdepchange(BookTransactionDTO data);
        BookTransactionDTO searchfilter(BookTransactionDTO data);
        BookTransactionDTO searchfilteravail(BookTransactionDTO data);
        BookTransactionDTO availget_bookdetails(BookTransactionDTO data);
        BookTransactionDTO searchfilterbarcode(BookTransactionDTO data);
        BookTransactionDTO searchfilterbarcode1(BookTransactionDTO data);
        BookTransactionDTO stdSearch_Grid(BookTransactionDTO data);
        BookTransactionDTO get_bookdetails(BookTransactionDTO data);
        BookTransactionDTO Savedata(BookTransactionDTO data);
        BookTransactionDTO GetStudentDetails1(BookTransactionDTO data);
        BookTransactionDTO renewaldata(BookTransactionDTO data);
        BookTransactionDTO Editdata(BookTransactionDTO data);
        BookTransactionDTO returndata(BookTransactionDTO data);
        BookTransactionDTO loadbookavail(BookTransactionDTO data);
        BookTransactionDTO getdetails_smartcard(BookTransactionDTO data);
        BookTransactionDTO smsdue(BookTransactionDTO data);
        BookTransactionDTO showfine(BookTransactionDTO data);
        BookTransactionDTO aftersmsdue(BookTransactionDTO data);
        BookTransactionDTO ShowDiffrentDays(BookTransactionDTO data);
        //GettransctionDetails
        BookTransactionDTO GettransctionDetails(BookTransactionDTO data);
        //SaveSmartCard
        BookTransactionDTO SaveSmartCard(BookTransactionDTO data);
    }
}
