using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;

namespace IVRMUX.Delegates.com.vapstech.admission
{
    public class SiblingEmployeeStudentReportDelegate
    {
        CommonDelegate<SiblingEmployeeStudentReportDTO, SiblingEmployeeStudentReportDTO> _comm = new CommonDelegate<SiblingEmployeeStudentReportDTO, SiblingEmployeeStudentReportDTO>();

        public SiblingEmployeeStudentReportDTO getdetails (SiblingEmployeeStudentReportDTO data)
        {
            return _comm.POSTDataADM(data, "SiblingEmployeeStudentReportFacade/getdetails");
        }
        public SiblingEmployeeStudentReportDTO getreport(SiblingEmployeeStudentReportDTO data)
        {
            return _comm.POSTDataADM(data, "SiblingEmployeeStudentReportFacade/getreport");
        }
    }
}
