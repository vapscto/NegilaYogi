using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission.Criteria8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission.Criteria8
{
    public class NaacPGDegrees813Delegate
    {


        CommonDelegate<NAAC_MC_813_PGDegrees_DTO, NAAC_MC_813_PGDegrees_DTO> comm = new CommonDelegate<NAAC_MC_813_PGDegrees_DTO, NAAC_MC_813_PGDegrees_DTO>();

        public NAAC_MC_813_PGDegrees_DTO loaddata(NAAC_MC_813_PGDegrees_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacPGDegrees813Facade/loaddata");
        }
        public NAAC_MC_813_PGDegrees_DTO save(NAAC_MC_813_PGDegrees_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacPGDegrees813Facade/save");
        }
        public NAAC_MC_813_PGDegrees_DTO deactive(NAAC_MC_813_PGDegrees_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacPGDegrees813Facade/deactive");
        }
        public NAAC_MC_813_PGDegrees_DTO EditData(NAAC_MC_813_PGDegrees_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacPGDegrees813Facade/EditData");
        }
        public NAAC_MC_813_PGDegrees_DTO viewuploadflies(NAAC_MC_813_PGDegrees_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacPGDegrees813Facade/viewuploadflies");
        }
        public NAAC_MC_813_PGDegrees_DTO deleteuploadfile(NAAC_MC_813_PGDegrees_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacPGDegrees813Facade/deleteuploadfile");
        }
        public NAAC_MC_813_PGDegrees_DTO getcomment(NAAC_MC_813_PGDegrees_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacPGDegrees813Facade/getcomment");
        }
        public NAAC_MC_813_PGDegrees_DTO getfilecomment(NAAC_MC_813_PGDegrees_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacPGDegrees813Facade/getfilecomment");
        }
        public NAAC_MC_813_PGDegrees_DTO savecomments(NAAC_MC_813_PGDegrees_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacPGDegrees813Facade/savecomments");
        }
        public NAAC_MC_813_PGDegrees_DTO savefilewisecomments(NAAC_MC_813_PGDegrees_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacPGDegrees813Facade/savefilewisecomments");
        }
    }
}
