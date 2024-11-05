using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library
{
    public class LostBookDelegate
    {

        CommonDelegate<LostBook_DTO, LostBook_DTO> _commnbranch = new CommonDelegate<LostBook_DTO, LostBook_DTO>();

        public LostBook_DTO getdetails(LostBook_DTO data)
        {
            return _commnbranch.PostLibrary(data, "LostBookFacade/getdetails/");
        }

        public LostBook_DTO searchfilter(LostBook_DTO data)
        {
            return _commnbranch.PostLibrary(data, "LostBookFacade/searchfilter/");
        }

        public LostBook_DTO get_authorNm(LostBook_DTO data)
        {
            return _commnbranch.PostLibrary(data, "LostBookFacade/get_authorNm/");
        }

        public LostBook_DTO get_radiochange(LostBook_DTO data)
        {
            return _commnbranch.PostLibrary(data, "LostBookFacade/get_radiochange/");
        }

        public LostBook_DTO saverecord(LostBook_DTO data)
        {
            return _commnbranch.PostLibrary(data, "LostBookFacade/saverecord/");
        }


    }
    
}
