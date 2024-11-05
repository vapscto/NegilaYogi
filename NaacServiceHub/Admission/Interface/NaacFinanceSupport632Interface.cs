using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
   public interface NaacFinanceSupport632Interface
    {

        NAAC_AC_632_FinanceSupport_DTO loaddata(NAAC_AC_632_FinanceSupport_DTO data);
        NAAC_AC_632_FinanceSupport_DTO save(NAAC_AC_632_FinanceSupport_DTO data);
        NAAC_AC_632_FinanceSupport_DTO deactive(NAAC_AC_632_FinanceSupport_DTO data);
        NAAC_AC_632_FinanceSupport_DTO EditData(NAAC_AC_632_FinanceSupport_DTO data);
        NAAC_AC_632_FinanceSupport_DTO viewuploadflies(NAAC_AC_632_FinanceSupport_DTO data);
        NAAC_AC_632_FinanceSupport_DTO deleteuploadfile(NAAC_AC_632_FinanceSupport_DTO data);


        NAAC_AC_632_FinanceSupport_DTO savemedicaldatawisecomments(NAAC_AC_632_FinanceSupport_DTO data);
        NAAC_AC_632_FinanceSupport_DTO savefilewisecomments(NAAC_AC_632_FinanceSupport_DTO data);
        NAAC_AC_632_FinanceSupport_DTO getcomment(NAAC_AC_632_FinanceSupport_DTO data);
        NAAC_AC_632_FinanceSupport_DTO getfilecomment(NAAC_AC_632_FinanceSupport_DTO data);

    }
}
