using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Library
{
    public class MasterLanguageDelegate
    {
        CommonDelegate<MasterLanguageDTO, MasterLanguageDTO> _commbranch = new CommonDelegate<MasterLanguageDTO, MasterLanguageDTO>();
        public MasterLanguageDTO Savedata(MasterLanguageDTO data)
        {
            return _commbranch.PostLibrary(data, "MasterLanguageFacade/Savedata/");
        }
        public MasterLanguageDTO getdetails(int id)
        {
            return _commbranch.GETLibrary(id, "MasterLanguageFacade/getdetails/");
        }
        public MasterLanguageDTO deactiveY(MasterLanguageDTO data)
        {
            return _commbranch.PostLibrary(data, "MasterLanguageFacade/deactiveY/");
        }
    }
}
