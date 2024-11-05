using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class ChangeOfBranchReportDelegate
    {
        CommonDelegate<ChangeOfBranchReportDTO, ChangeOfBranchReportDTO> comm = new CommonDelegate<ChangeOfBranchReportDTO, ChangeOfBranchReportDTO>();

        public ChangeOfBranchReportDTO loaddata(ChangeOfBranchReportDTO data)
        {
            return comm.clgadmissionbypost(data, "ChangeOfBranchReportFacade/loaddata");
        }
        public ChangeOfBranchReportDTO getcourse(ChangeOfBranchReportDTO data)
        {
            return comm.clgadmissionbypost(data, "ChangeOfBranchReportFacade/getcourse");
        }
        public ChangeOfBranchReportDTO getbranch(ChangeOfBranchReportDTO data)
        {
            return comm.clgadmissionbypost(data, "ChangeOfBranchReportFacade/getbranch");
        }
        public ChangeOfBranchReportDTO Report(ChangeOfBranchReportDTO data)
        {
            return comm.clgadmissionbypost(data, "ChangeOfBranchReportFacade/Report");
        }
    }
}
