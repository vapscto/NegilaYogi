using CommonLibrary;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.University
{
    public class HSU_316_Dept_AwardsDelegate
    {
        CommonDelegate<HSU_316_Dept_AwardsDTO, HSU_316_Dept_AwardsDTO> comm = new CommonDelegate<HSU_316_Dept_AwardsDTO, HSU_316_Dept_AwardsDTO>();

        public HSU_316_Dept_AwardsDTO loaddata(HSU_316_Dept_AwardsDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_316_Dept_AwardsFacade/loaddata");
        }
        public HSU_316_Dept_AwardsDTO save(HSU_316_Dept_AwardsDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_316_Dept_AwardsFacade/save");
        }
        public HSU_316_Dept_AwardsDTO deactive(HSU_316_Dept_AwardsDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_316_Dept_AwardsFacade/deactive");
        }
        public HSU_316_Dept_AwardsDTO EditData(HSU_316_Dept_AwardsDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_316_Dept_AwardsFacade/EditData");
        }
        public HSU_316_Dept_AwardsDTO viewuploadflies(HSU_316_Dept_AwardsDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_316_Dept_AwardsFacade/viewuploadflies");
        }
        public HSU_316_Dept_AwardsDTO deleteuploadfile(HSU_316_Dept_AwardsDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_316_Dept_AwardsFacade/deleteuploadfile");
        }
    }
}
