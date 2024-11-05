using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class ClgAttendanceSMSDetailsReportDelegate
    {
        CommonDelegate<ClgAttendanceSMSDetailsReport_DTO, ClgAttendanceSMSDetailsReport_DTO> comm = new CommonDelegate<ClgAttendanceSMSDetailsReport_DTO, ClgAttendanceSMSDetailsReport_DTO>();
        public ClgAttendanceSMSDetailsReport_DTO loaddata(ClgAttendanceSMSDetailsReport_DTO data)
        {
            return comm.clgadmissionbypost(data, "ClgAttendanceSMSDetailsReportFacade/loaddata");
        } public ClgAttendanceSMSDetailsReport_DTO getcourse(ClgAttendanceSMSDetailsReport_DTO data)
        {
            return comm.clgadmissionbypost(data, "ClgAttendanceSMSDetailsReportFacade/getcourse");
        } public ClgAttendanceSMSDetailsReport_DTO getbranch(ClgAttendanceSMSDetailsReport_DTO data)
        {
            return comm.clgadmissionbypost(data, "ClgAttendanceSMSDetailsReportFacade/getbranch");
        } public ClgAttendanceSMSDetailsReport_DTO getsemester(ClgAttendanceSMSDetailsReport_DTO data)
        {
            return comm.clgadmissionbypost(data, "ClgAttendanceSMSDetailsReportFacade/getsemester");
        } public ClgAttendanceSMSDetailsReport_DTO showdetails(ClgAttendanceSMSDetailsReport_DTO data)
        {
            return comm.clgadmissionbypost(data, "ClgAttendanceSMSDetailsReportFacade/showdetails");
        }
    }
}
