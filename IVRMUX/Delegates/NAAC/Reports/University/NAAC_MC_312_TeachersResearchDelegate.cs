using CommonLibrary;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Reports.University
{
    public class NAAC_MC_312_TeachersResearchDelegate
    {
        CommonDelegate<UC_312_TeachersResearchDTO, UC_312_TeachersResearchDTO> _commnbranch = new CommonDelegate<UC_312_TeachersResearchDTO, UC_312_TeachersResearchDTO>();

        public UC_312_TeachersResearchDTO getdata(UC_312_TeachersResearchDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAAC_MC_312_TeachersResearchFacade/getdata/");
        }
        public UC_312_TeachersResearchDTO get_312U_report(UC_312_TeachersResearchDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAAC_MC_312_TeachersResearchFacade/get_312U_report/");
        }
        public UC_312_TeachersResearchDTO get_313U_report(UC_312_TeachersResearchDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAAC_MC_312_TeachersResearchFacade/get_313U_report/");
        }
        public UC_312_TeachersResearchDTO get_314U_report(UC_312_TeachersResearchDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAAC_MC_312_TeachersResearchFacade/get_314U_report/");
        }
        public UC_312_TeachersResearchDTO get_315U_report(UC_312_TeachersResearchDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAAC_MC_312_TeachersResearchFacade/get_315U_report/");
        }
        public UC_312_TeachersResearchDTO get_316U_report(UC_312_TeachersResearchDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAAC_MC_312_TeachersResearchFacade/get_316U_report/");
        }
        public UC_312_TeachersResearchDTO get_342U_report(UC_312_TeachersResearchDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAAC_MC_312_TeachersResearchFacade/get_342U_report/");
        }
        public UC_312_TeachersResearchDTO get_343U_report(UC_312_TeachersResearchDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAAC_MC_312_TeachersResearchFacade/get_343U_report/");
        }
        public UC_312_TeachersResearchDTO get_372U_report(UC_312_TeachersResearchDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAAC_MC_312_TeachersResearchFacade/get_372U_report/");
        }
        public UC_312_TeachersResearchDTO get_362U_report(UC_312_TeachersResearchDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAAC_MC_312_TeachersResearchFacade/get_362U_report/");
        }
        public UC_312_TeachersResearchDTO get_352U_report(UC_312_TeachersResearchDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAAC_MC_312_TeachersResearchFacade/get_352U_report/");
        }
        public UC_312_TeachersResearchDTO get_371U_report(UC_312_TeachersResearchDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAAC_MC_312_TeachersResearchFacade/get_371U_report/");
        }
        public UC_312_TeachersResearchDTO get_341U_report(UC_312_TeachersResearchDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAAC_MC_312_TeachersResearchFacade/get_341U_report/");
        }
        public UC_312_TeachersResearchDTO NAAC_MC_345_TeachersResearchReport(UC_312_TeachersResearchDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAAC_MC_312_TeachersResearchFacade/NAAC_MC_345_TeachersResearchReport/");
        }
        public UC_312_TeachersResearchDTO NAAC_MC_346_TeachersResearchReport(UC_312_TeachersResearchDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAAC_MC_312_TeachersResearchFacade/NAAC_MC_346_TeachersResearchReport/");
        }
    }
}
