using CommonLibrary;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.University
{
    public class UC_312_TeachersResearchDelegate
    {

        CommonDelegate<UC_312_TeachersResearchDTO, UC_312_TeachersResearchDTO> comm = new CommonDelegate<UC_312_TeachersResearchDTO, UC_312_TeachersResearchDTO>();

        public UC_312_TeachersResearchDTO loaddata(UC_312_TeachersResearchDTO data)
        {
            return comm.naacdetailsbypost(data, "UC_312_TeachersResearchFacade/loaddata");
        }
        public UC_312_TeachersResearchDTO save(UC_312_TeachersResearchDTO data)
        {
            return comm.naacdetailsbypost(data, "UC_312_TeachersResearchFacade/save");
        }
        public UC_312_TeachersResearchDTO deactive(UC_312_TeachersResearchDTO data)
        {
            return comm.naacdetailsbypost(data, "UC_312_TeachersResearchFacade/deactive");
        }
        public UC_312_TeachersResearchDTO EditData(UC_312_TeachersResearchDTO data)
        {
            return comm.naacdetailsbypost(data, "UC_312_TeachersResearchFacade/EditData");
        }
        public UC_312_TeachersResearchDTO viewuploadflies(UC_312_TeachersResearchDTO data)
        {
            return comm.naacdetailsbypost(data, "UC_312_TeachersResearchFacade/viewuploadflies");
        }
        public UC_312_TeachersResearchDTO deleteuploadfile(UC_312_TeachersResearchDTO data)
        {
            return comm.naacdetailsbypost(data, "UC_312_TeachersResearchFacade/deleteuploadfile");
        }
        public UC_312_TeachersResearchDTO get_dept(UC_312_TeachersResearchDTO data)
        {
            return comm.naacdetailsbypost(data, "UC_312_TeachersResearchFacade/get_dept");
        }
        public UC_312_TeachersResearchDTO get_emp(UC_312_TeachersResearchDTO data)
        {
            return comm.naacdetailsbypost(data, "UC_312_TeachersResearchFacade/get_emp");
        }
    }
}
