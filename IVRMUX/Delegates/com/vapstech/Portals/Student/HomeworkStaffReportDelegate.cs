using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Portals.Student
{
    public class HomeworkStaffReportDelegate
    {




        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HomeworkStaffReportDTO, HomeworkStaffReportDTO> COMMM = new CommonDelegate<HomeworkStaffReportDTO, HomeworkStaffReportDTO>();


        public HomeworkStaffReportDTO getAllDetail(HomeworkStaffReportDTO data)
        {
            return COMMM.POSTPORTALData(data, "HomeworkStaffUploadFacade/getAllDetail/");
        }

        public HomeworkStaffReportDTO getReport(HomeworkStaffReportDTO data)
        {
            return COMMM.POSTPORTALData(data, "HomeworkStaffUploadFacade/getReport/");
        }
        public HomeworkStaffReportDTO get_load_onchange(HomeworkStaffReportDTO data)
        {
            return COMMM.POSTPORTALData(data, "HomeworkStaffUploadFacade/get_load_onchange/");
        }
        //getOnchange
        public HomeworkStaffReportDTO getOnchange(HomeworkStaffReportDTO data)
        {
            return COMMM.POSTPORTALData(data, "HomeworkStaffUploadFacade/getOnchange/");
        }
        //-----Class Wise Report -----
        public HomeworkStaffReportDTO getloadDetails(HomeworkStaffReportDTO data)
        {
            return COMMM.POSTPORTALData(data, "HomeworkStaffUploadFacade/getloadDetails/");
        }

        public HomeworkStaffReportDTO getLoad_onchange(HomeworkStaffReportDTO data)
        {
            return COMMM.POSTPORTALData(data, "HomeworkStaffUploadFacade/getLoad_onchange/");
        }

        public HomeworkStaffReportDTO getReport_classwise(HomeworkStaffReportDTO data)
        {
            return COMMM.POSTPORTALData(data, "HomeworkStaffUploadFacade/getReport_classwise/");
        }
        //smsemail
        public HomeworkStaffReportDTO smsemail(HomeworkStaffReportDTO data)
        {
            return COMMM.POSTPORTALData(data, "HomeworkStaffUploadFacade/smsemail/");
        }
        //getOnchangeclass
        public HomeworkStaffReportDTO getOnchangeclass(HomeworkStaffReportDTO data)
        {
            return COMMM.POSTPORTALData(data, "HomeworkStaffUploadFacade/getOnchangeclass/");
        }
    }
}
