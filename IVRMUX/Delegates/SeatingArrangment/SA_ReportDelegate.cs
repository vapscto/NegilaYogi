using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.SeatingArrangment;

namespace IVRMUX.Delegates.SeatingArrangment
{
    public class SA_ReportDelegate
    {
        CommonDelegate<SA_ReportDTO, SA_ReportDTO> _comm = new CommonDelegate<SA_ReportDTO, SA_ReportDTO>();

        public SA_ReportDTO GetExamDateloaddata(SA_ReportDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "SA_ReportFacade/GetExamDateloaddata");
        }
        public SA_ReportDTO OnChangeyear(SA_ReportDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "SA_ReportFacade/OnChangeyear");
        }
        public SA_ReportDTO GetAbsentStudentReport(SA_ReportDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "SA_ReportFacade/GetAbsentStudentReport");
        }
        public SA_ReportDTO GetMalpracticeStudentReport(SA_ReportDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "SA_ReportFacade/GetMalpracticeStudentReport");
        }
    }
}
