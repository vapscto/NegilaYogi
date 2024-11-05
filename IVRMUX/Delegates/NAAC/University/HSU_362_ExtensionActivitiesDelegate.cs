using CommonLibrary;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.University
{
    public class HSU_362_ExtensionActivitiesDelegate
    {
        CommonDelegate<HSU_362_ExtensionActivitiesDTO, HSU_362_ExtensionActivitiesDTO> comm = new CommonDelegate<HSU_362_ExtensionActivitiesDTO, HSU_362_ExtensionActivitiesDTO>();

        public HSU_362_ExtensionActivitiesDTO loaddata(HSU_362_ExtensionActivitiesDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_362_ExtensionActivitiesFacade/loaddata");
        }
        public HSU_362_ExtensionActivitiesDTO save(HSU_362_ExtensionActivitiesDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_362_ExtensionActivitiesFacade/save");
        }
        public HSU_362_ExtensionActivitiesDTO deactive(HSU_362_ExtensionActivitiesDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_362_ExtensionActivitiesFacade/deactive");
        }
        public HSU_362_ExtensionActivitiesDTO EditData(HSU_362_ExtensionActivitiesDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_362_ExtensionActivitiesFacade/EditData");
        }
        public HSU_362_ExtensionActivitiesDTO viewuploadflies(HSU_362_ExtensionActivitiesDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_362_ExtensionActivitiesFacade/viewuploadflies");
        }
        public HSU_362_ExtensionActivitiesDTO deleteuploadfile(HSU_362_ExtensionActivitiesDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_362_ExtensionActivitiesFacade/deleteuploadfile");
        }
    }
}
