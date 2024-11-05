using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Student.Interfaces
{
    public interface HomeworkstaffUploadReportInterface
    {

        HomeworkStaffReportDTO getAllDetail(HomeworkStaffReportDTO dto);

        HomeworkStaffReportDTO getReport(HomeworkStaffReportDTO dto);

        HomeworkStaffReportDTO get_load_onchange(HomeworkStaffReportDTO dto);
        //getOnchange
        HomeworkStaffReportDTO getOnchange(HomeworkStaffReportDTO dto);
        //----Class Wise Report------//
        HomeworkStaffReportDTO getloadDetails(HomeworkStaffReportDTO dto);

        HomeworkStaffReportDTO getLoad_onchange(HomeworkStaffReportDTO dto);

        HomeworkStaffReportDTO getReport_classwise(HomeworkStaffReportDTO dto);
        //smsemail
        HomeworkStaffReportDTO smsemail(HomeworkStaffReportDTO dto);
        //getOnchangeclass
        HomeworkStaffReportDTO getOnchangeclass(HomeworkStaffReportDTO dto);
    }
}
