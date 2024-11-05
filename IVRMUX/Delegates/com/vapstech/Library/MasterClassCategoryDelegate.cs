using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library
{
    public class MasterClassCategoryDelegate
    {

        CommonDelegate<LIB_Master_ClassCategory_DTO, LIB_Master_ClassCategory_DTO> _commbranch = new CommonDelegate<LIB_Master_ClassCategory_DTO, LIB_Master_ClassCategory_DTO>();
        public LIB_Master_ClassCategory_DTO Savedata(LIB_Master_ClassCategory_DTO data)
        {
            return _commbranch.PostLibrary(data, "MasterClassCategoryFacade/Savedata/");
        }
        public LIB_Master_ClassCategory_DTO getdetails(LIB_Master_ClassCategory_DTO id)
        {
            return _commbranch.PostLibrary(id, "MasterClassCategoryFacade/getdetails/");
        }
        public LIB_Master_ClassCategory_DTO deactiveY(LIB_Master_ClassCategory_DTO data)
        {
            return _commbranch.PostLibrary(data, "MasterClassCategoryFacade/deactiveY/");
        }

    }
}
