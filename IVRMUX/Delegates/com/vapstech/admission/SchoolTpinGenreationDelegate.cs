using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;

namespace IVRMUX.Delegates.com.vapstech.admission
{
    public class SchoolTpinGenreationDelegate
    {
        CommonDelegate<SchoolTpinGenreationDTO, SchoolTpinGenreationDTO> _comm = new CommonDelegate<SchoolTpinGenreationDTO, SchoolTpinGenreationDTO>();

        public SchoolTpinGenreationDTO loaddata(SchoolTpinGenreationDTO data)
        {
            return _comm.POSTDataADM(data, "SchoolTpinGenreationFacade/loaddata");
        }
        public SchoolTpinGenreationDTO search(SchoolTpinGenreationDTO data)
        {
            return _comm.POSTDataADM(data, "SchoolTpinGenreationFacade/search");
        }
        public SchoolTpinGenreationDTO generatetpin(SchoolTpinGenreationDTO data)
        {
            return _comm.POSTDataADM(data, "SchoolTpinGenreationFacade/generatetpin");
        }
    }
}
