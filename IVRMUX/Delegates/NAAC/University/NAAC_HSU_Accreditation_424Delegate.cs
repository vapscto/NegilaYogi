using CommonLibrary;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.University
{
    public class NAAC_HSU_Accreditation_424Delegate
    {

        CommonDelegate<NAAC_HSU_Accreditation_424_DTO, NAAC_HSU_Accreditation_424_DTO> comm = new CommonDelegate<NAAC_HSU_Accreditation_424_DTO, NAAC_HSU_Accreditation_424_DTO>();
        public NAAC_HSU_Accreditation_424_DTO loaddata(NAAC_HSU_Accreditation_424_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_Accreditation_424Facade/loaddata");
        }
        public NAAC_HSU_Accreditation_424_DTO save(NAAC_HSU_Accreditation_424_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_Accreditation_424Facade/save");
        }
        public NAAC_HSU_Accreditation_424_DTO EditData(NAAC_HSU_Accreditation_424_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_Accreditation_424Facade/EditData");
        }
    }
}
