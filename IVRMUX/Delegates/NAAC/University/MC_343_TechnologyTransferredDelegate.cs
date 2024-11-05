using CommonLibrary;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.University
{
    public class MC_343_TechnologyTransferredDelegate
    {

        CommonDelegate<MC_343_TechnologyTransferredDTO, MC_343_TechnologyTransferredDTO> comm = new CommonDelegate<MC_343_TechnologyTransferredDTO, MC_343_TechnologyTransferredDTO>();

        public MC_343_TechnologyTransferredDTO loaddata(MC_343_TechnologyTransferredDTO data)
        {
            return comm.naacdetailsbypost(data, "MC_343_TechnologyTransferredFacade/loaddata");
        }
        public MC_343_TechnologyTransferredDTO save(MC_343_TechnologyTransferredDTO data)
        {
            return comm.naacdetailsbypost(data, "MC_343_TechnologyTransferredFacade/save");
        }
        public MC_343_TechnologyTransferredDTO deactive(MC_343_TechnologyTransferredDTO data)
        {
            return comm.naacdetailsbypost(data, "MC_343_TechnologyTransferredFacade/deactive");
        }
        public MC_343_TechnologyTransferredDTO EditData(MC_343_TechnologyTransferredDTO data)
        {
            return comm.naacdetailsbypost(data, "MC_343_TechnologyTransferredFacade/EditData");
        }
        public MC_343_TechnologyTransferredDTO viewuploadflies(MC_343_TechnologyTransferredDTO data)
        {
            return comm.naacdetailsbypost(data, "MC_343_TechnologyTransferredFacade/viewuploadflies");
        }
        public MC_343_TechnologyTransferredDTO deleteuploadfile(MC_343_TechnologyTransferredDTO data)
        {
            return comm.naacdetailsbypost(data, "MC_343_TechnologyTransferredFacade/deleteuploadfile");
        }
    }
}
