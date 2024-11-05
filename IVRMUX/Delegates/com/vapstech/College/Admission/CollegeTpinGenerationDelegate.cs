using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class CollegeTpinGenerationDelegate
    {
        CommonDelegate<CollegeTpinGenerationDTO, CollegeTpinGenerationDTO> _comm = new CommonDelegate<CollegeTpinGenerationDTO, CollegeTpinGenerationDTO>();

        public CollegeTpinGenerationDTO loaddata(CollegeTpinGenerationDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeTpinGenerationFacade/loaddata");
        }
        public CollegeTpinGenerationDTO search(CollegeTpinGenerationDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeTpinGenerationFacade/search");
        }
        public CollegeTpinGenerationDTO generatetpin(CollegeTpinGenerationDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeTpinGenerationFacade/generatetpin");
        }
    }
}
