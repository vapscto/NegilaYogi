using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
    public interface SRKVSSportsReportInterface
    {
        SRKVSSportsReportDTO showdetails(SRKVSSportsReportDTO data);
        SRKVSSportsReportDTO Getdetails(SRKVSSportsReportDTO data);
        SRKVSSportsReportDTO get_class(SRKVSSportsReportDTO data);
        SRKVSSportsReportDTO get_classs(SRKVSSportsReportDTO data);
        SRKVSSportsReportDTO get_section(SRKVSSportsReportDTO data);
    }
}
