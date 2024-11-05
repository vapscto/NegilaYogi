using CommonLibrary;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.University
{
    public class HSU_352_RevenueGeneratedDelegate
    {
        CommonDelegate<HSU_352_RevenueGeneratedDTO, HSU_352_RevenueGeneratedDTO> comm = new CommonDelegate<HSU_352_RevenueGeneratedDTO, HSU_352_RevenueGeneratedDTO>();

        public HSU_352_RevenueGeneratedDTO loaddata(HSU_352_RevenueGeneratedDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_352_RevenueGeneratedFacade/loaddata");
        }
        public HSU_352_RevenueGeneratedDTO save(HSU_352_RevenueGeneratedDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_352_RevenueGeneratedFacade/save");
        }
        public HSU_352_RevenueGeneratedDTO deactive(HSU_352_RevenueGeneratedDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_352_RevenueGeneratedFacade/deactive");
        }
        public HSU_352_RevenueGeneratedDTO EditData(HSU_352_RevenueGeneratedDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_352_RevenueGeneratedFacade/EditData");
        }
        public HSU_352_RevenueGeneratedDTO viewuploadflies(HSU_352_RevenueGeneratedDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_352_RevenueGeneratedFacade/viewuploadflies");
        }
        public HSU_352_RevenueGeneratedDTO deleteuploadfile(HSU_352_RevenueGeneratedDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_352_RevenueGeneratedFacade/deleteuploadfile");
        }
    }
}
