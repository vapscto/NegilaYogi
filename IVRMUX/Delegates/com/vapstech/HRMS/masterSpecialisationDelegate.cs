using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.HRMS
{
    public class masterSpecialisationDelegate
    {
        CommonDelegate<masterSpecialisationDTO, masterSpecialisationDTO> comm = new CommonDelegate<masterSpecialisationDTO, masterSpecialisationDTO>();
        public masterSpecialisationDTO loaddata(masterSpecialisationDTO data)
        {
            return comm.POSTDataHRMS(data, "masterSpecialisationFacade/loaddata");
        }
        public masterSpecialisationDTO savedata(masterSpecialisationDTO data)
        {
            return comm.POSTDataHRMS(data, "masterSpecialisationFacade/savedata");
        }
        public masterSpecialisationDTO EditData(masterSpecialisationDTO data)
        {
            return comm.POSTDataHRMS(data, "masterSpecialisationFacade/EditData");
        }
        public masterSpecialisationDTO masterDecative(masterSpecialisationDTO data)
        {
            return comm.POSTDataHRMS(data, "masterSpecialisationFacade/masterDecative");
        }
    }
}
