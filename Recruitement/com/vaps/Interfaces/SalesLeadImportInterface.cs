using PreadmissionDTOs.com.vaps.VMS.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface SalesLeadImportInterface
    {
       
        SalesLeadImportDTO saveadvance(SalesLeadImportDTO dto);
    }
}
