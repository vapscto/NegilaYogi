using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library
{
    public class CLGBookTransactionDelegate
    {
        CommonDelegate<CLGBookTransactionDTO, CLGBookTransactionDTO> _commnbranch = new CommonDelegate<CLGBookTransactionDTO, CLGBookTransactionDTO>();

        public CLGBookTransactionDTO getdetails(CLGBookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "CLGBookTransactionFacade/getdetails/");
        }
        public CLGBookTransactionDTO studentdetails(CLGBookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "CLGBookTransactionFacade/studentdetails/");
        }
         public CLGBookTransactionDTO get_staff1(CLGBookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "CLGBookTransactionFacade/get_staff1/");
        }
        public CLGBookTransactionDTO getdepchange(CLGBookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "CLGBookTransactionFacade/getdepchange/");
        }

        public CLGBookTransactionDTO get_bookdetails(CLGBookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "CLGBookTransactionFacade/get_bookdetails/");
        }
        public CLGBookTransactionDTO searchfilter(CLGBookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "CLGBookTransactionFacade/searchfilter/");
        }
        public CLGBookTransactionDTO stdSearch_Grid(CLGBookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "CLGBookTransactionFacade/stdSearch_Grid/");
        }
        public CLGBookTransactionDTO searchfilterbarcode(CLGBookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "CLGBookTransactionFacade/searchfilterbarcode/");
        }
        public CLGBookTransactionDTO searchfilterbarcode1(CLGBookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "CLGBookTransactionFacade/searchfilterbarcode1/");
        }
        public CLGBookTransactionDTO Savedata(CLGBookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "CLGBookTransactionFacade/Savedata/");
        }
          public CLGBookTransactionDTO GetStudentDetails1(CLGBookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "CLGBookTransactionFacade/GetStudentDetails1/");
        }

        public CLGBookTransactionDTO renewaldata(CLGBookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "CLGBookTransactionFacade/renewaldata/");
        }

        public CLGBookTransactionDTO Editdata(CLGBookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "CLGBookTransactionFacade/Editdata/");
        }

        public CLGBookTransactionDTO returndata(CLGBookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "CLGBookTransactionFacade/returndata/");
        }
        public CLGBookTransactionDTO showfine(CLGBookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "CLGBookTransactionFacade/showfine/");
        }


        public CLGBookTransactionDTO getdetails_smartcard(CLGBookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "CLGBookTransactionFacade/getdetails_smartcard/");
        }
    }
}
