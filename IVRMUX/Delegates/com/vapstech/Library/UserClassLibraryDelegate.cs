using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library
{
    public class UserClassLibraryDelegate
    {
        CommonDelegate<LIB_Library_Class_DTO, LIB_Library_Class_DTO> _commbranch = new CommonDelegate<LIB_Library_Class_DTO, LIB_Library_Class_DTO>();

        public LIB_Library_Class_DTO getdetails(int id)
        {
            return _commbranch.GETLibrary(id, "UserClassLibraryFacade/getdetails/");
        }

        public LIB_Library_Class_DTO Savedata(LIB_Library_Class_DTO data)
        {
            return _commbranch.PostLibrary(data, "UserClassLibraryFacade/Savedata/");
        }
        

    }
}
