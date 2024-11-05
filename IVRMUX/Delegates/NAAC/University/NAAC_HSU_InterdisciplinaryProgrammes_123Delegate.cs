using CommonLibrary;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.University
{
    public class NAAC_HSU_InterdisciplinaryProgrammes_123Delegate
    {
        CommonDelegate<NAAC_HSU_InterdisciplinaryProgrammes_123_DTO, NAAC_HSU_InterdisciplinaryProgrammes_123_DTO> comm = new CommonDelegate<NAAC_HSU_InterdisciplinaryProgrammes_123_DTO, NAAC_HSU_InterdisciplinaryProgrammes_123_DTO>();
        public NAAC_HSU_InterdisciplinaryProgrammes_123_DTO loaddata(NAAC_HSU_InterdisciplinaryProgrammes_123_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_InterdisciplinaryProgrammes_123Facade/loaddata");

        }
        public NAAC_HSU_InterdisciplinaryProgrammes_123_DTO save(NAAC_HSU_InterdisciplinaryProgrammes_123_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_InterdisciplinaryProgrammes_123Facade/save");
        }
        public NAAC_HSU_InterdisciplinaryProgrammes_123_DTO deactive(NAAC_HSU_InterdisciplinaryProgrammes_123_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_InterdisciplinaryProgrammes_123Facade/deactive");
        }
        public NAAC_HSU_InterdisciplinaryProgrammes_123_DTO EditData(NAAC_HSU_InterdisciplinaryProgrammes_123_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_InterdisciplinaryProgrammes_123Facade/EditData");
        }
        public NAAC_HSU_InterdisciplinaryProgrammes_123_DTO viewuploadflies(NAAC_HSU_InterdisciplinaryProgrammes_123_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_InterdisciplinaryProgrammes_123Facade/viewuploadflies");
        }
        public NAAC_HSU_InterdisciplinaryProgrammes_123_DTO deleteuploadfile(NAAC_HSU_InterdisciplinaryProgrammes_123_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_InterdisciplinaryProgrammes_123Facade/deleteuploadfile");
        }
    }
}
