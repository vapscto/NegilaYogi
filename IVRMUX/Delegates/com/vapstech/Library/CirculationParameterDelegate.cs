using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library
{
    public class CirculationParameterDelegate
    {
     
        CommonDelegate<CirculationParameterDTO, CirculationParameterDTO> _commbranch = new CommonDelegate<CirculationParameterDTO, CirculationParameterDTO>();

        public CirculationParameterDTO getdetails(CirculationParameterDTO data)
        {
            return _commbranch.PostLibrary(data, "CirculationParameterFacade/getdetails/");
        }
        public CirculationParameterDTO getdata(CirculationParameterDTO data)
        {
            return _commbranch.PostLibrary(data, "CirculationParameterFacade/getdata/");
        }
        public CirculationParameterDTO gettype(CirculationParameterDTO data)
        {
            return _commbranch.PostLibrary(data, "CirculationParameterFacade/gettype/");
        }
        public CirculationParameterDTO Savedata(CirculationParameterDTO data)
        {
            return _commbranch.PostLibrary(data, "CirculationParameterFacade/Savedata/");
        }
        public CirculationParameterDTO deactiveY(CirculationParameterDTO data)
        {
            return _commbranch.PostLibrary(data, "CirculationParameterFacade/deactiveY/");
        }
        public CirculationParameterDTO editdata(CirculationParameterDTO data)
        {
            return _commbranch.PostLibrary(data, "CirculationParameterFacade/editdata/");
        }
    }
}
