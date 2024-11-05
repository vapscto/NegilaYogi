using CommonLibrary;
using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Medical
{
    public class NAAC_MC_EmpTrainedDevelopment244Delegate
    {

        CommonDelegate<NAAC_MC_EmpTrainedDevelopment244_DTO, NAAC_MC_EmpTrainedDevelopment244_DTO> comm = new CommonDelegate<NAAC_MC_EmpTrainedDevelopment244_DTO, NAAC_MC_EmpTrainedDevelopment244_DTO>();
        public NAAC_MC_EmpTrainedDevelopment244_DTO loaddata(NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_MC_EmpTrainedDevelopment244Facade/loaddata");

        }
        public NAAC_MC_EmpTrainedDevelopment244_DTO save(NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_MC_EmpTrainedDevelopment244Facade/save");
        }
        public NAAC_MC_EmpTrainedDevelopment244_DTO deactive(NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_MC_EmpTrainedDevelopment244Facade/deactive");
        }
        public NAAC_MC_EmpTrainedDevelopment244_DTO EditData(NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_MC_EmpTrainedDevelopment244Facade/EditData");
        }
        public NAAC_MC_EmpTrainedDevelopment244_DTO viewuploadflies(NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_MC_EmpTrainedDevelopment244Facade/viewuploadflies");
        }
        public NAAC_MC_EmpTrainedDevelopment244_DTO deleteuploadfile(NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_MC_EmpTrainedDevelopment244Facade/deleteuploadfile");
        }


        public NAAC_MC_EmpTrainedDevelopment244_DTO savemedicaldatawisecomments(NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_MC_EmpTrainedDevelopment244Facade/savemedicaldatawisecomments");
        }
        public NAAC_MC_EmpTrainedDevelopment244_DTO savefilewisecomments(NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_MC_EmpTrainedDevelopment244Facade/savefilewisecomments");
        }
        public NAAC_MC_EmpTrainedDevelopment244_DTO getcomment(NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_MC_EmpTrainedDevelopment244Facade/getcomment");
        }
        public NAAC_MC_EmpTrainedDevelopment244_DTO getfilecomment(NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_MC_EmpTrainedDevelopment244Facade/getfilecomment");
        }

    }
}
