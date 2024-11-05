using CommonLibrary;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Reports.University
{
    public class NAAC_HSU_323_ResearchProjectsRatioDelegate
    {
        CommonDelegate<HSU_323_ResearchProjectsRatioDTO, HSU_323_ResearchProjectsRatioDTO> _commnbranch = new CommonDelegate<HSU_323_ResearchProjectsRatioDTO, HSU_323_ResearchProjectsRatioDTO>();

        public HSU_323_ResearchProjectsRatioDTO getdata(HSU_323_ResearchProjectsRatioDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAAC_HSU_323_ResearchProjectsRatioFacade/getdata/");
        }
        public HSU_323_ResearchProjectsRatioDTO get_323U_report(HSU_323_ResearchProjectsRatioDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAAC_HSU_323_ResearchProjectsRatioFacade/get_323U_report/");
        }
        public HSU_323_ResearchProjectsRatioDTO get_334U_report(HSU_323_ResearchProjectsRatioDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAAC_HSU_323_ResearchProjectsRatioFacade/get_334U_report/");
        }
        public HSU_323_ResearchProjectsRatioDTO get_344U_report(HSU_323_ResearchProjectsRatioDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAAC_HSU_323_ResearchProjectsRatioFacade/get_344U_report/");
        }
        public HSU_323_ResearchProjectsRatioDTO get_333U_report(HSU_323_ResearchProjectsRatioDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAAC_HSU_323_ResearchProjectsRatioFacade/get_333U_report/");
        }
        public HSU_323_ResearchProjectsRatioDTO get_349U_report(HSU_323_ResearchProjectsRatioDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAAC_HSU_323_ResearchProjectsRatioFacade/get_349U_report/");
        }
        public HSU_323_ResearchProjectsRatioDTO get_348U_report(HSU_323_ResearchProjectsRatioDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAAC_HSU_323_ResearchProjectsRatioFacade/get_348U_report/");
        }
    }
}
