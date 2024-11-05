using PreadmissionDTOs.com.vaps.Portals.IVRM;
using PreadmissionDTOs.com.vaps.Portals.IVRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.IVRS.Interfaces
{
    public interface Interaction_Delete_ReportInterface
    {
        Interaction_Delete_Report_DTO getreport(Interaction_Delete_Report_DTO dto);
        Interaction_Delete_Report_DTO loadreportdata(Interaction_Delete_Report_DTO dto);
        Interaction_Delete_Report_DTO getintreport(Interaction_Delete_Report_DTO dto);
        Interaction_Delete_Report_DTO mobload(Interaction_Delete_Report_DTO dto);
        Interaction_Delete_Report_DTO mobreport(Interaction_Delete_Report_DTO dto);
      
    }
}
