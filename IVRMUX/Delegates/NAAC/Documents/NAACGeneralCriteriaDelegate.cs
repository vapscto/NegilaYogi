using CommonLibrary;
using PreadmissionDTOs.NAAC.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Documents
{
    public class NAACGeneralCriteriaDelegate
    {
        CommonDelegate<NAACGeneralCriteriaDTO, NAACGeneralCriteriaDTO> comm = new CommonDelegate<NAACGeneralCriteriaDTO, NAACGeneralCriteriaDTO>();
        public NAACGeneralCriteriaDTO loaddata(NAACGeneralCriteriaDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACGeneralCriteriaFacade/loaddata");
        }
        public NAACGeneralCriteriaDTO save(NAACGeneralCriteriaDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACGeneralCriteriaFacade/save");
        }
        public NAACGeneralCriteriaDTO deactiveStudent(NAACGeneralCriteriaDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACGeneralCriteriaFacade/deactiveStudent");
        }

        public NAACGeneralCriteriaDTO EditData(NAACGeneralCriteriaDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACGeneralCriteriaFacade/EditData");
        }
        public NAACGeneralCriteriaDTO viewuploadflies(NAACGeneralCriteriaDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACGeneralCriteriaFacade/viewuploadflies");
        }
          public NAACGeneralCriteriaDTO deleteuploadfile(NAACGeneralCriteriaDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACGeneralCriteriaFacade/deleteuploadfile");
        }
          public NAACGeneralCriteriaDTO deletelink(NAACGeneralCriteriaDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACGeneralCriteriaFacade/deletelink");
        }
        public NAACGeneralCriteriaDTO viewlink(NAACGeneralCriteriaDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACGeneralCriteriaFacade/viewlink");
        }


    }
}
