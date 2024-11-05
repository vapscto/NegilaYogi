using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library
{
    public class CLGNonBookTransactionDelegate
    {


        CommonDelegate<ClgNonBookTransaction_DTO, ClgNonBookTransaction_DTO> _commnbranch = new CommonDelegate<ClgNonBookTransaction_DTO, ClgNonBookTransaction_DTO>();

        public ClgNonBookTransaction_DTO getdetails(ClgNonBookTransaction_DTO data)
        {
            return _commnbranch.PostLibrary(data, "CLGNonBookTransactionFacade/getdetails/");
        }

        public ClgNonBookTransaction_DTO studentdetails(ClgNonBookTransaction_DTO data)
        {
            return _commnbranch.PostLibrary(data, "CLGNonBookTransactionFacade/studentdetails/");
        }

        public ClgNonBookTransaction_DTO get_staff1(ClgNonBookTransaction_DTO data)
        {
            return _commnbranch.PostLibrary(data, "CLGNonBookTransactionFacade/get_staff1/");
        }

        public ClgNonBookTransaction_DTO getdepchange(ClgNonBookTransaction_DTO data)
        {
            return _commnbranch.PostLibrary(data, "CLGNonBookTransactionFacade/getdepchange/");
        }

        public ClgNonBookTransaction_DTO get_bookdetails(ClgNonBookTransaction_DTO data)
        {
            return _commnbranch.PostLibrary(data, "CLGNonBookTransactionFacade/get_bookdetails/");
        }

        public ClgNonBookTransaction_DTO searchfilter(ClgNonBookTransaction_DTO data)
        {
            return _commnbranch.PostLibrary(data, "CLGNonBookTransactionFacade/searchfilter/");
        }

        public ClgNonBookTransaction_DTO searchfilterbarcode(ClgNonBookTransaction_DTO data)
        {
            return _commnbranch.PostLibrary(data, "CLGNonBookTransactionFacade/searchfilterbarcode/");
        }

        public ClgNonBookTransaction_DTO searchfilterbarcode1(ClgNonBookTransaction_DTO data)
        {
            return _commnbranch.PostLibrary(data, "CLGNonBookTransactionFacade/searchfilterbarcode1/");
        }

        public ClgNonBookTransaction_DTO Savedata(ClgNonBookTransaction_DTO data)
        {
            return _commnbranch.PostLibrary(data, "CLGNonBookTransactionFacade/Savedata/");
        }

        public ClgNonBookTransaction_DTO GetStudentDetails1(ClgNonBookTransaction_DTO data)
        {
            return _commnbranch.PostLibrary(data, "CLGNonBookTransactionFacade/GetStudentDetails1/");
        }

        public ClgNonBookTransaction_DTO renewaldata(ClgNonBookTransaction_DTO data)
        {
            return _commnbranch.PostLibrary(data, "CLGNonBookTransactionFacade/renewaldata/");
        }

        public ClgNonBookTransaction_DTO Editdata(ClgNonBookTransaction_DTO data)
        {
            return _commnbranch.PostLibrary(data, "CLGNonBookTransactionFacade/Editdata/");
        }

        public ClgNonBookTransaction_DTO returndata(ClgNonBookTransaction_DTO data)
        {
            return _commnbranch.PostLibrary(data, "CLGNonBookTransactionFacade/returndata/");
        }


        public ClgNonBookTransaction_DTO getdetails_smartcard(ClgNonBookTransaction_DTO data)
        {
            return _commnbranch.PostLibrary(data, "CLGNonBookTransactionFacade/getdetails_smartcard/");
        }

    }
}
