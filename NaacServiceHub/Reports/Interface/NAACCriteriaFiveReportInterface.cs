using PreadmissionDTOs.NAAC.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Reports.Interface
{
   public interface NAACCriteriaFiveReportInterface
    {
        NAACCriteriaFiveReportDTO getdata(NAACCriteriaFiveReportDTO data);
        Task<NAACCriteriaFiveReportDTO> get_report(NAACCriteriaFiveReportDTO data);
        Task<NAACCriteriaFiveReportDTO> HSU511(NAACCriteriaFiveReportDTO data);
        Task<NAACCriteriaFiveReportDTO> get_report513(NAACCriteriaFiveReportDTO data);
        Task<NAACCriteriaFiveReportDTO> get_report513med(NAACCriteriaFiveReportDTO data);
        Task<NAACCriteriaFiveReportDTO> get_report514(NAACCriteriaFiveReportDTO data);
        Task<NAACCriteriaFiveReportDTO> get_report516(NAACCriteriaFiveReportDTO data);
        Task<NAACCriteriaFiveReportDTO> get_report515med(NAACCriteriaFiveReportDTO data);
        Task<NAACCriteriaFiveReportDTO> get_report521(NAACCriteriaFiveReportDTO data);
        Task<NAACCriteriaFiveReportDTO> get_report522(NAACCriteriaFiveReportDTO data);
        Task<NAACCriteriaFiveReportDTO> get_report531(NAACCriteriaFiveReportDTO data);
        Task<NAACCriteriaFiveReportDTO> get_report533(NAACCriteriaFiveReportDTO data);
        Task<NAACCriteriaFiveReportDTO> get_report542(NAACCriteriaFiveReportDTO data);
        Task<NAACCriteriaFiveReportDTO> get_report542HSU(NAACCriteriaFiveReportDTO data);
        Task<NAACCriteriaFiveReportDTO> get_report543(NAACCriteriaFiveReportDTO data);
        Task<NAACCriteriaFiveReportDTO> get_report523(NAACCriteriaFiveReportDTO data);
        Task<NAACCriteriaFiveReportDTO> get_report515(NAACCriteriaFiveReportDTO data);


    }
}
