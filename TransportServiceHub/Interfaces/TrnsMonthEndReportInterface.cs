using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
   public  interface TrnsMonthEndReportInterface
    {
        TrnsMonthEndReportDTO getdata(int id);
        TrnsMonthEndReportDTO savedata(TrnsMonthEndReportDTO data);
        TrnsMonthEndReportDTO getdata1(int id);
        TrnsMonthEndReportDTO savedata1(TrnsMonthEndReportDTO data);
        TrnsMonthEndReportDTO geteditdata(TrnsMonthEndReportDTO data);
        TrnsMonthEndReportDTO activedeactive(TrnsMonthEndReportDTO data);

    }
}
