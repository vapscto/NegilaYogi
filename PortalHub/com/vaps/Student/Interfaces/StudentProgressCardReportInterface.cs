using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Student.Interfaces
{
    public interface StudentProgressCardReportInterface
    {
        StudentProgressCardReportDTO getdetails(StudentProgressCardReportDTO data);
        StudentProgressCardReportDTO onchangeclass(StudentProgressCardReportDTO data);
        StudentProgressCardReportDTO getreport(StudentProgressCardReportDTO data);

        //BGHS
        StudentProgressCardReportDTO Bghsgetdetails(StudentProgressCardReportDTO data);
        StudentProgressCardReportDTO Bghsonchangeclass(StudentProgressCardReportDTO data);
        StudentProgressCardReportDTO Bghsgetreport(StudentProgressCardReportDTO data);

        //Stmary
        StudentProgressCardReportDTO stmarygetdetails(StudentProgressCardReportDTO data);
        StudentProgressCardReportDTO stmaryonchangeclass(StudentProgressCardReportDTO data);
        StudentProgressCardReportDTO stmarygetreport(StudentProgressCardReportDTO data);

        //HHS
        StudentProgressCardReportDTO HHSStudentProgressCardReport(StudentProgressCardReportDTO data);

        //Stjames
        StudentProgressCardReportDTO Get_Stjames_Progresscard_Report(StudentProgressCardReportDTO data);

        //Notredame
        StudentProgressCardReportDTO NDS_Get_Progresscard_Report(StudentProgressCardReportDTO data);

        //BCEHS
        StudentProgressCardReportDTO Get_BCEHS_Progresscard_Report(StudentProgressCardReportDTO data);

        //BIS
        StudentProgressCardReportDTO BISStudentProgressCardReport(StudentProgressCardReportDTO data);
    }
}
