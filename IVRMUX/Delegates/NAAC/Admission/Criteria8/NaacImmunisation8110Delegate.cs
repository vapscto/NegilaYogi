using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission.Criteria8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission.Criteria8
{
    public class NaacImmunisation8110Delegate
    {



        CommonDelegate<NAAC_MC_8110_Immunisation_DTO, NAAC_MC_8110_Immunisation_DTO> comm = new CommonDelegate<NAAC_MC_8110_Immunisation_DTO, NAAC_MC_8110_Immunisation_DTO>();

        public NAAC_MC_8110_Immunisation_DTO loaddata(NAAC_MC_8110_Immunisation_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacImmunisation8110Facade/loaddata");
        }
        public NAAC_MC_8110_Immunisation_DTO save(NAAC_MC_8110_Immunisation_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacImmunisation8110Facade/save");
        }
        public NAAC_MC_8110_Immunisation_DTO deactive(NAAC_MC_8110_Immunisation_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacImmunisation8110Facade/deactive");
        }
        public NAAC_MC_8110_Immunisation_DTO EditData(NAAC_MC_8110_Immunisation_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacImmunisation8110Facade/EditData");
        }
        public NAAC_MC_8110_Immunisation_DTO viewuploadflies(NAAC_MC_8110_Immunisation_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacImmunisation8110Facade/viewuploadflies");
        }
        public NAAC_MC_8110_Immunisation_DTO deleteuploadfile(NAAC_MC_8110_Immunisation_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacImmunisation8110Facade/deleteuploadfile");
        }
        public NAAC_MC_8110_Immunisation_DTO getcomment(NAAC_MC_8110_Immunisation_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacImmunisation8110Facade/getcomment");
        }
         public NAAC_MC_8110_Immunisation_DTO getfilecomment(NAAC_MC_8110_Immunisation_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacImmunisation8110Facade/getfilecomment");
        }
        public NAAC_MC_8110_Immunisation_DTO savecomments(NAAC_MC_8110_Immunisation_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacImmunisation8110Facade/savecomments");
        }
        public NAAC_MC_8110_Immunisation_DTO savefilewisecomments(NAAC_MC_8110_Immunisation_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacImmunisation8110Facade/savefilewisecomments");
        }

    }
}
