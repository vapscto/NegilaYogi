using CommonLibrary;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.University
{
    public class HSU_323_ResearchProjectsRatioDelegate
    {
        CommonDelegate<HSU_323_ResearchProjectsRatioDTO, HSU_323_ResearchProjectsRatioDTO> comm = new CommonDelegate<HSU_323_ResearchProjectsRatioDTO, HSU_323_ResearchProjectsRatioDTO>();

        public HSU_323_ResearchProjectsRatioDTO loaddata(HSU_323_ResearchProjectsRatioDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_323_ResearchProjectsRatioFacade/loaddata");
        }
        public HSU_323_ResearchProjectsRatioDTO save(HSU_323_ResearchProjectsRatioDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_323_ResearchProjectsRatioFacade/save");
        }
        public HSU_323_ResearchProjectsRatioDTO deactive(HSU_323_ResearchProjectsRatioDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_323_ResearchProjectsRatioFacade/deactive");
        }
        public HSU_323_ResearchProjectsRatioDTO EditData(HSU_323_ResearchProjectsRatioDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_323_ResearchProjectsRatioFacade/EditData");
        }
        public HSU_323_ResearchProjectsRatioDTO viewuploadflies(HSU_323_ResearchProjectsRatioDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_323_ResearchProjectsRatioFacade/viewuploadflies");
        }
        public HSU_323_ResearchProjectsRatioDTO deleteuploadfile(HSU_323_ResearchProjectsRatioDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_323_ResearchProjectsRatioFacade/deleteuploadfile");
        }
    }
}
