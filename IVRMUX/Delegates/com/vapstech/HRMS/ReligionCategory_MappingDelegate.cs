using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.HRMS
{
    public class ReligionCategory_MappingDelegate
    {
        CommonDelegate<ReligionCategory_MappingDTO, ReligionCategory_MappingDTO> comm = new CommonDelegate<ReligionCategory_MappingDTO, ReligionCategory_MappingDTO>();

        public ReligionCategory_MappingDTO loaddata(ReligionCategory_MappingDTO data)
        {
            return comm.POSTDataHRMS(data, "ReligionCategory_MappingFacade/loaddata");
        }
        public ReligionCategory_MappingDTO savedata(ReligionCategory_MappingDTO data)
        {
            return comm.POSTDataHRMS(data, "ReligionCategory_MappingFacade/savedata");
        }
        public ReligionCategory_MappingDTO Editdata(ReligionCategory_MappingDTO data)
        {
            return comm.POSTDataHRMS(data, "ReligionCategory_MappingFacade/Editdata");
        }
        public ReligionCategory_MappingDTO masterDecative(ReligionCategory_MappingDTO data)
        {
            return comm.POSTDataHRMS(data, "ReligionCategory_MappingFacade/masterDecative");
        }
        //public ReligionCategory_MappingDTO getcast(ReligionCategory_MappingDTO data)
        //{
        //    return comm.POSTDataHRMS(data, "ReligionCategory_MappingFacade/getcast");
        //}
    }
}
