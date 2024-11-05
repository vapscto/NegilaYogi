using CommonLibrary;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.University
{
    public class HSU_334_CampusStartUpsDelegate
    {
        CommonDelegate<HSU_334_CampusStartUpsDTO, HSU_334_CampusStartUpsDTO> comm = new CommonDelegate<HSU_334_CampusStartUpsDTO, HSU_334_CampusStartUpsDTO>();

        public HSU_334_CampusStartUpsDTO loaddata(HSU_334_CampusStartUpsDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_334_CampusStartUpsFacade/loaddata");
        }
        public HSU_334_CampusStartUpsDTO save(HSU_334_CampusStartUpsDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_334_CampusStartUpsFacade/save");
        }
        public HSU_334_CampusStartUpsDTO deactive(HSU_334_CampusStartUpsDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_334_CampusStartUpsFacade/deactive");
        }
        public HSU_334_CampusStartUpsDTO EditData(HSU_334_CampusStartUpsDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_334_CampusStartUpsFacade/EditData");
        }
        public HSU_334_CampusStartUpsDTO viewuploadflies(HSU_334_CampusStartUpsDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_334_CampusStartUpsFacade/viewuploadflies");
        }
        public HSU_334_CampusStartUpsDTO deleteuploadfile(HSU_334_CampusStartUpsDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_334_CampusStartUpsFacade/deleteuploadfile");
        }
    }
}
