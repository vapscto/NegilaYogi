using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library
{
    public class MasterAccessoriesDelegate
    {


        CommonDelegate<LIB_Master_Accessories_DTO, LIB_Master_Accessories_DTO> _commbranch = new CommonDelegate<LIB_Master_Accessories_DTO, LIB_Master_Accessories_DTO>();
        public LIB_Master_Accessories_DTO Savedata(LIB_Master_Accessories_DTO data)
        {
            return _commbranch.PostLibrary(data, "MasterAccessoriesFacade/Savedata/");
        }
        public LIB_Master_Accessories_DTO getdetails(int id)
        {
            return _commbranch.GETLibrary(id, "MasterAccessoriesFacade/getdetails/");
        }
        public LIB_Master_Accessories_DTO deactiveY(LIB_Master_Accessories_DTO data)
        {
            return _commbranch.PostLibrary(data, "MasterAccessoriesFacade/deactiveY/");
        }


    }
}
