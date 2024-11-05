using CommonLibrary;
using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Medical
{
    public class NAAC_MC_423_StuLearningResourceDelegate
    {

        CommonDelegate<NAAC_MC_423_StuLearningResource_DTO, NAAC_MC_423_StuLearningResource_DTO> comm = new CommonDelegate<NAAC_MC_423_StuLearningResource_DTO, NAAC_MC_423_StuLearningResource_DTO>();
        public NAAC_MC_423_StuLearningResource_DTO loaddata(NAAC_MC_423_StuLearningResource_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_MC_423_StuLearningResourceFacade/loaddata");
        }
        public NAAC_MC_423_StuLearningResource_DTO save(NAAC_MC_423_StuLearningResource_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_MC_423_StuLearningResourceFacade/save");
        }
        public NAAC_MC_423_StuLearningResource_DTO EditData(NAAC_MC_423_StuLearningResource_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_MC_423_StuLearningResourceFacade/EditData");
        }
        public NAAC_MC_423_StuLearningResource_DTO viewuploadflies(NAAC_MC_423_StuLearningResource_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_MC_423_StuLearningResourceFacade/viewuploadflies");
        }
        public NAAC_MC_423_StuLearningResource_DTO deleteuploadfile(NAAC_MC_423_StuLearningResource_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_MC_423_StuLearningResourceFacade/deleteuploadfile");
        }
        public NAAC_MC_423_StuLearningResource_DTO loaddatainfra(NAAC_MC_423_StuLearningResource_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_MC_423_StuLearningResourceFacade/loaddatainfra");
        }
        public NAAC_MC_423_StuLearningResource_DTO saveinfra(NAAC_MC_423_StuLearningResource_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_MC_423_StuLearningResourceFacade/saveinfra");
        }
        public NAAC_MC_423_StuLearningResource_DTO EditDatainfra(NAAC_MC_423_StuLearningResource_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_MC_423_StuLearningResourceFacade/EditDatainfra");
        }
    }
}
