using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library
{
    public class BookRegisterDelegate
    {

        CommonDelegate<BookRegisterDTO, BookRegisterDTO> _commnbranch = new CommonDelegate<BookRegisterDTO, BookRegisterDTO>(); 

        public BookRegisterDTO getdetails(BookRegisterDTO id)
        {
            return _commnbranch.PostLibrary(id, "BookRegisterFacade/getdetails/");
        }
        public BookRegisterDTO Savedata(BookRegisterDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookRegisterFacade/Savedata/");
        }
        public BookRegisterDTO Tab1Savedata(BookRegisterDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookRegisterFacade/Tab1Savedata/");
        }
        public BookRegisterDTO Tab2Savedata(BookRegisterDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookRegisterFacade/Tab2Savedata/");
        }

        public BookRegisterDTO Ckeck_ISBNNO(BookRegisterDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookRegisterFacade/Ckeck_ISBNNO/");
        }

        public BookRegisterDTO chekAccno(BookRegisterDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookRegisterFacade/chekAccno/");
        }
        public BookRegisterDTO Addaccnno(BookRegisterDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookRegisterFacade/Addaccnno/");
        }
        public BookRegisterDTO Ckeck_LMBANO_AccessionNo(BookRegisterDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookRegisterFacade/Ckeck_LMBANO_AccessionNo/");
        }
        public BookRegisterDTO deactiveY(BookRegisterDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookRegisterFacade/deactiveY/");
        }

        public BookRegisterDTO Editdata(BookRegisterDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookRegisterFacade/Editdata/");
        }

        public BookRegisterDTO searching(BookRegisterDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookRegisterFacade/searching/");
        }

        public BookRegisterDTO searchfilter(BookRegisterDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookRegisterFacade/searchfilter/");
        }
        public BookRegisterDTO changelibrary(BookRegisterDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookRegisterFacade/changelibrary/");
        }


    }
}
