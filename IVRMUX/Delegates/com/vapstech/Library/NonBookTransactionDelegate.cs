using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library
{
    public class NonBookTransactionDelegate
    {

        CommonDelegate<NonBookTransaction_DTO, NonBookTransaction_DTO> _commnbranch = new CommonDelegate<NonBookTransaction_DTO, NonBookTransaction_DTO>();

        public NonBookTransaction_DTO getdetails(NonBookTransaction_DTO data)
        {
            return _commnbranch.PostLibrary(data, "NonBookTransactionFacade/getdetails/");
        }
        public NonBookTransaction_DTO studentdetails(NonBookTransaction_DTO data)
        {
            return _commnbranch.PostLibrary(data, "NonBookTransactionFacade/studentdetails/");
        }
        public NonBookTransaction_DTO get_staff1(NonBookTransaction_DTO data)
        {
            return _commnbranch.PostLibrary(data, "NonBookTransactionFacade/get_staff1/");
        }
        public NonBookTransaction_DTO getdepchange(NonBookTransaction_DTO data)
        {
            return _commnbranch.PostLibrary(data, "NonBookTransactionFacade/getdepchange/");
        }

        public NonBookTransaction_DTO get_bookdetails(NonBookTransaction_DTO data)
        {
            return _commnbranch.PostLibrary(data, "NonBookTransactionFacade/get_bookdetails/");
        }
        public NonBookTransaction_DTO searchfilter(NonBookTransaction_DTO data)
        {
            return _commnbranch.PostLibrary(data, "NonBookTransactionFacade/searchfilter/");
        }
        public NonBookTransaction_DTO searchfilterbarcode(NonBookTransaction_DTO data)
        {
            return _commnbranch.PostLibrary(data, "NonBookTransactionFacade/searchfilterbarcode/");
        }
        public NonBookTransaction_DTO searchfilterbarcode1(NonBookTransaction_DTO data)
        {
            return _commnbranch.PostLibrary(data, "NonBookTransactionFacade/searchfilterbarcode1/");
        }
        public NonBookTransaction_DTO Savedata(NonBookTransaction_DTO data)
        {
            return _commnbranch.PostLibrary(data, "NonBookTransactionFacade/Savedata/");
        }
        public NonBookTransaction_DTO GetStudentDetails1(NonBookTransaction_DTO data)
        {
            return _commnbranch.PostLibrary(data, "NonBookTransactionFacade/GetStudentDetails1/");
        }

        public NonBookTransaction_DTO renewaldata(NonBookTransaction_DTO data)
        {
            return _commnbranch.PostLibrary(data, "NonBookTransactionFacade/renewaldata/");
        }

        public NonBookTransaction_DTO Editdata(NonBookTransaction_DTO data)
        {
            return _commnbranch.PostLibrary(data, "NonBookTransactionFacade/Editdata/");
        }

        public NonBookTransaction_DTO returndata(NonBookTransaction_DTO data)
        {
            return _commnbranch.PostLibrary(data, "NonBookTransactionFacade/returndata/");
        }


        public NonBookTransaction_DTO getdetails_smartcard(NonBookTransaction_DTO data)
        {
            return _commnbranch.PostLibrary(data, "NonBookTransactionFacade/getdetails_smartcard/");
        }

    }
}
