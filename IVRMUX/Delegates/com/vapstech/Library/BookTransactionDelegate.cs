using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library
{
    public class BookTransactionDelegate
    {
        CommonDelegate<BookTransactionDTO, BookTransactionDTO> _commnbranch = new CommonDelegate<BookTransactionDTO, BookTransactionDTO>();

        public BookTransactionDTO getdetails(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionFacade/getdetails/");
        }
        //getdetailsReturn
        public BookTransactionDTO getdetailsReturn(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionFacade/getdetailsReturn/");
        }
        public BookTransactionDTO studentdetails(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionFacade/studentdetails/");
        }
         public BookTransactionDTO get_staff1(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionFacade/get_staff1/");
        }
        public BookTransactionDTO getdepchange(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionFacade/getdepchange/");
        }

        public BookTransactionDTO get_bookdetails(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionFacade/get_bookdetails/");
        }
        public BookTransactionDTO searchfilter(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionFacade/searchfilter/");
        }
        public BookTransactionDTO searchfilteravail(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionFacade/searchfilteravail/");
        }
        public BookTransactionDTO availget_bookdetails(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionFacade/availget_bookdetails/");
        }
        public BookTransactionDTO searchfilterbarcode(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionFacade/searchfilterbarcode/");
        }
        public BookTransactionDTO stdSearch_Grid(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionFacade/stdSearch_Grid/");
        }
        public BookTransactionDTO searchfilterbarcode1(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionFacade/searchfilterbarcode1/");
        }
        public BookTransactionDTO Savedata(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionFacade/Savedata/");
        }
          public BookTransactionDTO GetStudentDetails1(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionFacade/GetStudentDetails1/");
        }

        public BookTransactionDTO renewaldata(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionFacade/renewaldata/");
        }

        public BookTransactionDTO Editdata(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionFacade/Editdata/");
        }
        public BookTransactionDTO showfine(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionFacade/showfine/");
        }
        //ShowDiffrentDays
        public BookTransactionDTO ShowDiffrentDays(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionFacade/ShowDiffrentDays/");
        }
        public BookTransactionDTO returndata(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionFacade/returndata/");
        }
        //GettransctionDetails
        public BookTransactionDTO GettransctionDetails(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionFacade/GettransctionDetails/");
        }
        public BookTransactionDTO loadbookavail(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionFacade/loadbookavail/");
        }


        public BookTransactionDTO getdetails_smartcard(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionFacade/getdetails_smartcard/");
        }
    }
}
