using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Library
{
    public class AddELibraryLinksDelegate
    {
        CommonDelegate< AddELibraryLinksDTO,  AddELibraryLinksDTO> _commbranch = new CommonDelegate< AddELibraryLinksDTO,  AddELibraryLinksDTO>();
        public  AddELibraryLinksDTO Savedata( AddELibraryLinksDTO data)
        {
            return _commbranch.PostLibrary(data, "AddELibraryLinksFacade/Savedata/");
        }
        public  AddELibraryLinksDTO getdetails(int id)
        {
            return _commbranch.GETLibrary(id, "AddELibraryLinksFacade/getdetails/");
        }
        public  AddELibraryLinksDTO GetELibrary(int id)
        {
            return _commbranch.GETLibrary(id, "AddELibraryLinksFacade/GetELibrary/");
        }
        public  AddELibraryLinksDTO deactiveY( AddELibraryLinksDTO data)
        {
            return _commbranch.PostLibrary(data, "AddELibraryLinksFacade/deactiveY/");
        }
        public  AddELibraryLinksDTO geteditdata( AddELibraryLinksDTO data)
        {
            return _commbranch.PostLibrary(data, "AddELibraryLinksFacade/geteditdata/");
        }
    }
}
