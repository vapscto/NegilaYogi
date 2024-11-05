using CommonLibrary;
using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Medical
{
    public class Naac_MC_IctFacility441Delegate
    {

        CommonDelegate<Naac_MC_IctFacility441_DTO, Naac_MC_IctFacility441_DTO> comm = new CommonDelegate<Naac_MC_IctFacility441_DTO, Naac_MC_IctFacility441_DTO>();
        public Naac_MC_IctFacility441_DTO loaddata(Naac_MC_IctFacility441_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MC_IctFacility441Facade/loaddata");
        }
        public Naac_MC_IctFacility441_DTO save(Naac_MC_IctFacility441_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MC_IctFacility441Facade/save");
        }
        public Naac_MC_IctFacility441_DTO EditData(Naac_MC_IctFacility441_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MC_IctFacility441Facade/EditData");
        }
        public Naac_MC_IctFacility441_DTO viewuploadflies(Naac_MC_IctFacility441_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MC_IctFacility441Facade/viewuploadflies");
        }
        public Naac_MC_IctFacility441_DTO deleteuploadfile(Naac_MC_IctFacility441_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MC_IctFacility441Facade/deleteuploadfile");
        }
    }
}
