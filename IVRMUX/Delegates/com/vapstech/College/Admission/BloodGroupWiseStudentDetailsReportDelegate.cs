using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class BloodGroupWiseStudentDetailsReportDelegate
    {
        CommonDelegate<BloodGroupWiseStudentDetailsReportDTO, BloodGroupWiseStudentDetailsReportDTO> comm = new CommonDelegate<BloodGroupWiseStudentDetailsReportDTO, BloodGroupWiseStudentDetailsReportDTO>();
        public BloodGroupWiseStudentDetailsReportDTO loaddata(BloodGroupWiseStudentDetailsReportDTO data)
        {
            return comm.clgadmissionbypost(data, "BloodGroupWiseStudentDetailsReportFacade/loaddata");
        }
        public BloodGroupWiseStudentDetailsReportDTO getcourse(BloodGroupWiseStudentDetailsReportDTO data)
        {
            return comm.clgadmissionbypost(data, "BloodGroupWiseStudentDetailsReportFacade/getcourse");
        }
        public BloodGroupWiseStudentDetailsReportDTO getbranch(BloodGroupWiseStudentDetailsReportDTO data)
        {
            return comm.clgadmissionbypost(data, "BloodGroupWiseStudentDetailsReportFacade/getbranch");
        }
        public BloodGroupWiseStudentDetailsReportDTO getsemester(BloodGroupWiseStudentDetailsReportDTO data)
        {
            return comm.clgadmissionbypost(data, "BloodGroupWiseStudentDetailsReportFacade/getsemester");
        }
        public BloodGroupWiseStudentDetailsReportDTO Report(BloodGroupWiseStudentDetailsReportDTO data)
        {
            return comm.clgadmissionbypost(data, "BloodGroupWiseStudentDetailsReportFacade/Report");
        }
    }
}
