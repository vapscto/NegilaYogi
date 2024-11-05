using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface SiblingEmployeeMappingInterface
    {
        Adm_M_Sibling initialdata(Adm_M_Sibling data);
        Adm_M_Sibling selectacade(Adm_M_Sibling data);
        Adm_M_Sibling onstudentnamechange(Adm_M_Sibling data);
        Adm_M_Sibling onselectstaff(Adm_M_Sibling data);
        Adm_M_Sibling onstudentnamechangerte(Adm_M_Sibling data);
        Adm_M_Sibling sved(Adm_M_Sibling data);
        Adm_M_Sibling deletedta(Adm_M_Sibling data);
        Adm_M_Sibling DeletRecordemployee(Adm_M_Sibling data);
        Adm_M_Sibling viewsiblingdetails(Adm_M_Sibling data);
        Adm_M_Sibling viewsiblingdetailsemployee(Adm_M_Sibling data);
        Adm_M_Sibling checkfeegroup(Adm_M_Sibling data);
        
    }
}
