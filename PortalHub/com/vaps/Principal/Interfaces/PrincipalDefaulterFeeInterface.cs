using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.Principal;

namespace PortalHub.com.vaps.Principal.Interfaces
{
   public interface PrincipalDefaulterFeeInterface
    {
        PrincipalDefaulterFeeDTO Getdetails(PrincipalDefaulterFeeDTO data);
        PrincipalDefaulterFeeDTO getclass(PrincipalDefaulterFeeDTO data);
        PrincipalDefaulterFeeDTO Getsection(PrincipalDefaulterFeeDTO data);
        PrincipalDefaulterFeeDTO Getreport(PrincipalDefaulterFeeDTO data);
        PrincipalDefaulterFeeDTO Getstudentdetails(PrincipalDefaulterFeeDTO data);

    }
}
