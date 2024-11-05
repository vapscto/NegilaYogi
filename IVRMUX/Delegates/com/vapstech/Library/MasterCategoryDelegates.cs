using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;

namespace corewebapi18072016.Delegates.com.vapstech.Library
{
    public class MasterCategoryDelegates
    {
        CommonDelegate<MasterCategory_DTO, MasterCategory_DTO> _commbranch = new CommonDelegate<MasterCategory_DTO, MasterCategory_DTO>();
        public MasterCategory_DTO Savedata(MasterCategory_DTO data)
        {
            return _commbranch.PostLibrary(data,"MasterCategoryFacade/Savedata/");
        }
        public MasterCategory_DTO deactiveY(MasterCategory_DTO data)
        {
            return _commbranch.PostLibrary(data, "MasterCategoryFacade/deactiveY/");
        }
        public MasterCategory_DTO getdetails(int id)
        {
            return _commbranch.GETLibrary(id,"MasterCategoryFacade/getdetails/");
        }
    }
}
