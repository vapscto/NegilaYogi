using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library
{
    public class ImportLibrarydataDelegate
    {
        CommonDelegate<ImportLibrarydataDTO, ImportLibrarydataDTO> _commnbranch = new CommonDelegate<ImportLibrarydataDTO, ImportLibrarydataDTO>();

        public ImportLibrarydataDTO Savedata(ImportLibrarydataDTO data)
        {
            return _commnbranch.PostLibrary(data, "ImportLibrarydataFacade/Savedata/");
        }
        public ImportLibrarydataDTO getdetails(int id)
        {
            return _commnbranch.GETLibrary(id, "ImportLibrarydataFacade/getdetails/");
        }
        public ImportLibrarydataDTO deactiveY(ImportLibrarydataDTO data)
        {
            return _commnbranch.PostLibrary(data, "ImportLibrarydataFacade/deactiveY/");
        }
    }
}
