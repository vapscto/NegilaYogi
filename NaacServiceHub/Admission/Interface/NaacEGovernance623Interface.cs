using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
  public interface NaacEGovernance623Interface
    {
        NAAC_AC_623_EGovernance_DTO loaddata(NAAC_AC_623_EGovernance_DTO data);
        NAAC_AC_623_EGovernance_DTO save(NAAC_AC_623_EGovernance_DTO data);
        NAAC_AC_623_EGovernance_DTO deactive(NAAC_AC_623_EGovernance_DTO data);
        NAAC_AC_623_EGovernance_DTO EditData(NAAC_AC_623_EGovernance_DTO data);
        NAAC_AC_623_EGovernance_DTO viewuploadflies(NAAC_AC_623_EGovernance_DTO data);
        NAAC_AC_623_EGovernance_DTO deleteuploadfile(NAAC_AC_623_EGovernance_DTO data);


        NAAC_AC_623_EGovernance_DTO savemedicaldatawisecomments(NAAC_AC_623_EGovernance_DTO data);
        NAAC_AC_623_EGovernance_DTO savefilewisecomments(NAAC_AC_623_EGovernance_DTO data);
        NAAC_AC_623_EGovernance_DTO getcomment(NAAC_AC_623_EGovernance_DTO data);
        NAAC_AC_623_EGovernance_DTO getfilecomment(NAAC_AC_623_EGovernance_DTO data);

    }
}
