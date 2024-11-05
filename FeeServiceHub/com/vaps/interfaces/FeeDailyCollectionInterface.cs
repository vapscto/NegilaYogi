using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
  public  interface FeeDailyCollectionInterface
    {
        DailyCollectionReportDTO getdetails(DailyCollectionReportDTO dt);
        DailyCollectionReportDTO getgroupmappedheads(DailyCollectionReportDTO feedto);

        DailyCollectionReportDTO getgroupheadsid(DailyCollectionReportDTO feedtohead);

      Task<DailyCollectionReportDTO> Getreportdetails(DailyCollectionReportDTO feedtoget); 

         DailyCollectionReportDTO getdata(DailyCollectionReportDTO feedtohead);
        Task<DailyCollectionReportDTO> FeeAccountDetailsReport(DailyCollectionReportDTO feedtohead);
         void ChairmanSMS(DailyCollectionReportDTO data);
        //UserWisereportdetails
        Task<DailyCollectionReportDTO> UserWisereportdetails(DailyCollectionReportDTO feedtoget);

        //Report VVVKS
        Task<DailyCollectionReportDTO> getreport(DailyCollectionReportDTO feedtoget);

    }
}
