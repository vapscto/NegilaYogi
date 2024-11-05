using CommonLibrary;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.University
{
    public class MC_314_ResearchAssociatesDelegate
    {
        CommonDelegate<MC_314_ResearchAssociatesDTO, MC_314_ResearchAssociatesDTO> comm = new CommonDelegate<MC_314_ResearchAssociatesDTO, MC_314_ResearchAssociatesDTO>();

        public MC_314_ResearchAssociatesDTO loaddata(MC_314_ResearchAssociatesDTO data)
        {
            return comm.naacdetailsbypost(data, "MC_314_ResearchAssociatesFacade/loaddata");
        }
        public MC_314_ResearchAssociatesDTO save(MC_314_ResearchAssociatesDTO data)
        {
            return comm.naacdetailsbypost(data, "MC_314_ResearchAssociatesFacade/save");
        }
        public MC_314_ResearchAssociatesDTO deactive(MC_314_ResearchAssociatesDTO data)
        {
            return comm.naacdetailsbypost(data, "MC_314_ResearchAssociatesFacade/deactive");
        }
        public MC_314_ResearchAssociatesDTO EditData(MC_314_ResearchAssociatesDTO data)
        {
            return comm.naacdetailsbypost(data, "MC_314_ResearchAssociatesFacade/EditData");
        }
        public MC_314_ResearchAssociatesDTO viewuploadflies(MC_314_ResearchAssociatesDTO data)
        {
            return comm.naacdetailsbypost(data, "MC_314_ResearchAssociatesFacade/viewuploadflies");
        }
        public MC_314_ResearchAssociatesDTO deleteuploadfile(MC_314_ResearchAssociatesDTO data)
        {
            return comm.naacdetailsbypost(data, "MC_314_ResearchAssociatesFacade/deleteuploadfile");
        }
    }
}
