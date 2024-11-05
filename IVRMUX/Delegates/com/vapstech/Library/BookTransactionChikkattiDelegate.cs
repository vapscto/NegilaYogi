using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library
{
    public class BookTransactionChikkattiDelegate
    {
        CommonDelegate<BookTransactionDTO, BookTransactionDTO> _commnbranch = new CommonDelegate<BookTransactionDTO, BookTransactionDTO>();

        public BookTransactionDTO getdetails(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionChikkttiFacade/getdetails/");
        }
        public BookTransactionDTO studentdetails(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionChikkttiFacade/studentdetails/");
        }
        public BookTransactionDTO get_staff1(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionChikkttiFacade/get_staff1/");
        }
        public BookTransactionDTO getdepchange(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionChikkttiFacade/getdepchange/");
        }

        public BookTransactionDTO get_bookdetails(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionChikkttiFacade/get_bookdetails/");
        }
        public BookTransactionDTO searchfilter(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionChikkttiFacade/searchfilter/");
        }
        public BookTransactionDTO searchfilteravail(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionChikkttiFacade/searchfilteravail/");
        }
        public BookTransactionDTO availget_bookdetails(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionChikkttiFacade/availget_bookdetails/");
        }
        public BookTransactionDTO searchfilterbarcode(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionChikkttiFacade/searchfilterbarcode/");
        }
        public BookTransactionDTO stdSearch_Grid(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionChikkttiFacade/stdSearch_Grid/");
        }
        public BookTransactionDTO searchfilterbarcode1(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionChikkttiFacade/searchfilterbarcode1/");
        }
        public BookTransactionDTO Savedata(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionChikkttiFacade/Savedata/");
        }
        public BookTransactionDTO GetStudentDetails1(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionChikkttiFacade/GetStudentDetails1/");
        }

        public BookTransactionDTO renewaldata(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionChikkttiFacade/renewaldata/");
        }

        public BookTransactionDTO Editdata(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionChikkttiFacade/Editdata/");
        }
        public BookTransactionDTO showfine(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionChikkttiFacade/showfine/");
        }

        public BookTransactionDTO returndata(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionChikkttiFacade/returndata/");
        }
        public BookTransactionDTO loadbookavail(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionChikkttiFacade/loadbookavail/");
        }


        public BookTransactionDTO getdetails_smartcard(BookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTransactionChikkttiFacade/getdetails_smartcard/");
        }
    }
}
