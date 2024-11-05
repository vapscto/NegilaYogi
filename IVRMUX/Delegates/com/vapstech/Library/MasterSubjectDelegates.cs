using System;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Library
{
    public class MasterSubjectDelegates
    {
        CommonDelegate<MasterSubject_DTO, MasterSubject_DTO> _commbranch = new CommonDelegate<MasterSubject_DTO, MasterSubject_DTO>();
        public MasterSubject_DTO Savedata(MasterSubject_DTO data)
        {
            return _commbranch.PostLibrary(data, "MasterSubjectFacade/Savedata");
        }
        public MasterSubject_DTO getdetails(int id)
        {
            return _commbranch.GETLibrary(id, "MasterSubjectFacade/getdetails/");
        }
        public MasterSubject_DTO deactiveY(MasterSubject_DTO data)
        {
            return _commbranch.PostLibrary(data, "MasterSubjectFacade/deactiveY/");
        }
    }
}
