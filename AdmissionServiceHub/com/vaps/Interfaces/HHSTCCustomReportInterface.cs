using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
   public interface HHSTCCustomReportInterface
    {
        HHSTCCustomReportDTO getdetails(int id);
        HHSTCCustomReportDTO getnameregno(HHSTCCustomReportDTO data);
        HHSTCCustomReportDTO stdnamechange(HHSTCCustomReportDTO data);
        HHSTCCustomReportDTO onclicktcperortemo(HHSTCCustomReportDTO data);
        HHSTCCustomReportDTO getTcdetails(HHSTCCustomReportDTO data);
        HHSTCCustomReportDTO Vikasha_getTcdetails(HHSTCCustomReportDTO data);

    }
}
