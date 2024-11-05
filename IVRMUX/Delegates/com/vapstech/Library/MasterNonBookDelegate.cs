using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library
{
    public class MasterNonBookDelegate
    {
        CommonDelegate<MatserNonBook_DTO, MatserNonBook_DTO> _commnbranch = new CommonDelegate<MatserNonBook_DTO, MatserNonBook_DTO>();

        public MatserNonBook_DTO getdetails(MatserNonBook_DTO id)
        {
            return _commnbranch.PostLibrary(id, "MasterNonBookFacade/getdetails/");
        }
        public MatserNonBook_DTO Savedata(MatserNonBook_DTO data)
        {
            return _commnbranch.PostLibrary(data, "MasterNonBookFacade/Savedata/");
        }
        public MatserNonBook_DTO deactiveY(MatserNonBook_DTO data)
        {
            return _commnbranch.PostLibrary(data, "MasterNonBookFacade/deactiveY/");
        }

        public MatserNonBook_DTO Editdata(MatserNonBook_DTO data)
        {
            return _commnbranch.PostLibrary(data, "MasterNonBookFacade/Editdata/");
        }

        public MatserNonBook_DTO searching(MatserNonBook_DTO data)
        {
            return _commnbranch.PostLibrary(data, "MasterNonBookFacade/searching/");
        }
        public MatserNonBook_DTO changelibrary(MatserNonBook_DTO data)
        {
            return _commnbranch.PostLibrary(data, "MasterNonBookFacade/changelibrary/");
        }

        
    }
}
