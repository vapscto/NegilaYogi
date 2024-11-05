using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library
{
    public class MasterLibraryDelegates
    {

        CommonDelegate<LIB_Master_Library_DTO, LIB_Master_Library_DTO> _commbranch = new CommonDelegate<LIB_Master_Library_DTO, LIB_Master_Library_DTO>();
        public LIB_Master_Library_DTO Savedata(LIB_Master_Library_DTO data)
        {
            return _commbranch.PostLibrary(data, "MasterLibraryFacade/Savedata/");
        }
        public LIB_Master_Library_DTO getdetails(LIB_Master_Library_DTO id)
        {
            return _commbranch.PostLibrary(id, "MasterLibraryFacade/getdetails/");
        }
        public LIB_Master_Library_DTO deactiveY(LIB_Master_Library_DTO data)
        {
            return _commbranch.PostLibrary(data, "MasterLibraryFacade/deactiveY/");
        }

        public LIB_Master_Library_DTO saveclassdata(LIB_Master_Library_DTO data)
        {
            return _commbranch.PostLibrary(data, "MasterLibraryFacade/saveclassdata/");
        }
        public LIB_Master_Library_DTO deactiveYstf(LIB_Master_Library_DTO data)
        {
            return _commbranch.PostLibrary(data, "MasterLibraryFacade/deactiveYstf/");
        }
        public LIB_Master_Library_DTO EditstaffData(LIB_Master_Library_DTO data)
        {
            return _commbranch.PostLibrary(data, "MasterLibraryFacade/EditstaffData/");
        }

        public LIB_Master_Library_DTO modalclsslst(LIB_Master_Library_DTO data)
        {
            return _commbranch.PostLibrary(data, "MasterLibraryFacade/modalclsslst/");
        }

        public LIB_Master_Library_DTO deactivclsdata(LIB_Master_Library_DTO data)
        {
            return _commbranch.PostLibrary(data, "MasterLibraryFacade/deactivclsdata/");
        }
        public LIB_Master_Library_DTO getusername(LIB_Master_Library_DTO data)
        {
            return _commbranch.PostLibrary(data, "MasterLibraryFacade/getusername/");
        }
        public LIB_Master_Library_DTO check_userclass(LIB_Master_Library_DTO data)
        {
            return _commbranch.PostLibrary(data, "MasterLibraryFacade/check_userclass/");
        }

        public LIB_Master_Library_DTO EditclassData(LIB_Master_Library_DTO data)
        {
            return _commbranch.PostLibrary(data, "MasterLibraryFacade/EditclassData/");
        }
        public LIB_Master_Library_DTO get_MappedClasslist(LIB_Master_Library_DTO data)
        {
            return _commbranch.PostLibrary(data, "MasterLibraryFacade/get_MappedClasslist/");
        }

        public LIB_Master_Library_DTO savestaffdata(LIB_Master_Library_DTO data)
        {
            return _commbranch.PostLibrary(data, "MasterLibraryFacade/savestaffdata/");
        }
        
    }
}
