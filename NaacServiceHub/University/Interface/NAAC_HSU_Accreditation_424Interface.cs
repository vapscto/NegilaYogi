using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Interface
{
   public interface NAAC_HSU_Accreditation_424Interface
    {

        NAAC_HSU_Accreditation_424_DTO loaddata(NAAC_HSU_Accreditation_424_DTO data);
        NAAC_HSU_Accreditation_424_DTO save(NAAC_HSU_Accreditation_424_DTO data);

        NAAC_HSU_Accreditation_424_DTO EditData(NAAC_HSU_Accreditation_424_DTO data);

    }
}
