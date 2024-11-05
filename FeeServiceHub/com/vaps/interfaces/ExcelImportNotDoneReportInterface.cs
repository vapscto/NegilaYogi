using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface ExcelImportNotDoneReportInterface
    {
        ExcelImportNotDoneReportDTO getdata123(ExcelImportNotDoneReportDTO data);

        ExcelImportNotDoneReportDTO getsection(ExcelImportNotDoneReportDTO data);
        ExcelImportNotDoneReportDTO getstudent(ExcelImportNotDoneReportDTO data);
        ExcelImportNotDoneReportDTO getstuddet(ExcelImportNotDoneReportDTO data);
        Task<ExcelImportNotDoneReportDTO> getreport(ExcelImportNotDoneReportDTO data);

        ExcelImportNotDoneReportDTO get_groups(ExcelImportNotDoneReportDTO data);
        ExcelImportNotDoneReportDTO deleterec(int id);
    }
}
