using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.admission;
namespace FeeServiceHub.com.vaps.interfaces
{
  public  interface FeeTrailAuditReportInterface
    {
        FeeTrailAuditDTO getdetails(int id);

        FeeTrailAuditDTO getdata123(FeeTrailAuditDTO data);
        FeeTrailAuditDTO getreport(FeeTrailAuditDTO data);
        FeeTrailAuditDTO viewdetails(FeeTrailAuditDTO data);        
        FeeTrailAuditDTO searchfilter(FeeTrailAuditDTO data);
      // Task<FeeTrailAuditDTO> getreport(FeeTrailAuditDTO data);
    }
}
