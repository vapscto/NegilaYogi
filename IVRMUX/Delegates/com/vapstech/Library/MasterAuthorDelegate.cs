using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library
{
    public class MasterAuthorDelegate
    {
        CommonDelegate<LIB_Master_Author_DTO, LIB_Master_Author_DTO> _commnbranch = new CommonDelegate<LIB_Master_Author_DTO, LIB_Master_Author_DTO>();

        public LIB_Master_Author_DTO Savedata(LIB_Master_Author_DTO data)
        {
            return _commnbranch.PostLibrary(data, "MasterAuthorFacade/Savedata/");
        }
        public LIB_Master_Author_DTO getdetails(LIB_Master_Author_DTO id)
        {
            return _commnbranch.PostLibrary(id, "MasterAuthorFacade/getdetails/");
        }
        public LIB_Master_Author_DTO deactiveY(LIB_Master_Author_DTO data)
        {
            return _commnbranch.PostLibrary(data, "MasterAuthorFacade/deactiveY/");
        }
    }
}
