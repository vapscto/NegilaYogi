using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.Principal;


namespace PortalHub.com.vaps.Principal.Interfaces
{
  public  interface PrincipalDashboardInterface
    {
        PrincipalDashboardDTO Getdetails(PrincipalDashboardDTO data);
        PrincipalDashboardDTO onclick_notice(PrincipalDashboardDTO data);
        PrincipalDashboardDTO viewnotice(PrincipalDashboardDTO data);
    }
}
