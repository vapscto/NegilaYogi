using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
    public interface SportHouseReportInterface
    {
        Task<House_Report_DTO> showdetails(House_Report_DTO data);
        Task<House_Report_DTO> showdetailsNew(House_Report_DTO data);
        House_Report_DTO Getdetails(House_Report_DTO data);
        House_Report_DTO get_class(House_Report_DTO data);
        House_Report_DTO get_section(House_Report_DTO data);

    }
}
