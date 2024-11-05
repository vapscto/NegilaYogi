using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Library
{
    public class MasterDepartmentDelegates
    {
        CommonDelegate<MasterDepartmentDTO, MasterDepartmentDTO> _commbranch = new CommonDelegate<MasterDepartmentDTO, MasterDepartmentDTO>();
        public MasterDepartmentDTO Savedata(MasterDepartmentDTO data)
        {
            return _commbranch.PostLibrary(data, "MasterDepartmentFacade/Savedata/"); 
        }
        public MasterDepartmentDTO getdetails(int id)
        {
            return _commbranch.GETLibrary(id, "MasterDepartmentFacade/getdetails/");
        }
        public MasterDepartmentDTO deactiveY(MasterDepartmentDTO data)
        {
            return _commbranch.PostLibrary(data, "MasterDepartmentFacade/deactiveY/");
        }
    }
}
