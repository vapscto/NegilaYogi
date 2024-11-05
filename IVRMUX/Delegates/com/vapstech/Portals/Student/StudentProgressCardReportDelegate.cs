using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Portals.Student
{
    public class StudentProgressCardReportDelegate
    {
        CommonDelegate<StudentProgressCardReportDTO, StudentProgressCardReportDTO> _comm = new CommonDelegate<StudentProgressCardReportDTO, StudentProgressCardReportDTO>();

        public StudentProgressCardReportDTO getdetails (StudentProgressCardReportDTO data)
        {
            return _comm.POSTPORTALData(data, "StudentProgressCardReportFacade/getdetails");
        }
        public StudentProgressCardReportDTO onchangeclass(StudentProgressCardReportDTO data)
        {
            return _comm.POSTPORTALData(data, "StudentProgressCardReportFacade/onchangeclass");
        }
        public StudentProgressCardReportDTO getreport(StudentProgressCardReportDTO data)
        {
            return _comm.POSTPORTALData(data, "StudentProgressCardReportFacade/getreport");
        }

        //BGHS 
        public StudentProgressCardReportDTO Bghsgetdetails(StudentProgressCardReportDTO data)
        {
            return _comm.POSTPORTALData(data, "StudentProgressCardReportFacade/Bghsgetdetails");
        }
        public StudentProgressCardReportDTO Bghsonchangeclass(StudentProgressCardReportDTO data)
        {
            return _comm.POSTPORTALData(data, "StudentProgressCardReportFacade/Bghsonchangeclass");
        }
        public StudentProgressCardReportDTO Bghsgetreport(StudentProgressCardReportDTO data)
        {
            return _comm.POSTPORTALData(data, "StudentProgressCardReportFacade/Bghsgetreport");
        }
        
        //STmary 
        public StudentProgressCardReportDTO stmarygetdetails(StudentProgressCardReportDTO data)
        {
            return _comm.POSTPORTALData(data, "StudentProgressCardReportFacade/stmarygetdetails");
        }
        public StudentProgressCardReportDTO stmaryonchangeclass(StudentProgressCardReportDTO data)
        {
            return _comm.POSTPORTALData(data, "StudentProgressCardReportFacade/stmaryonchangeclass");
        }
        public StudentProgressCardReportDTO stmarygetreport(StudentProgressCardReportDTO data)
        {
            return _comm.POSTPORTALData(data, "StudentProgressCardReportFacade/stmarygetreport");
        }

        //HHS
        public StudentProgressCardReportDTO HHSStudentProgressCardReport(StudentProgressCardReportDTO data)
        {
            return _comm.POSTPORTALData(data, "StudentProgressCardReportFacade/HHSStudentProgressCardReport");
        }

        //Stjames
        public StudentProgressCardReportDTO Get_Stjames_Progresscard_Report(StudentProgressCardReportDTO data)
        {
            return _comm.POSTPORTALData(data, "StudentProgressCardReportFacade/Get_Stjames_Progresscard_Report");
        }

        //Notredame
        public StudentProgressCardReportDTO NDS_Get_Progresscard_Report(StudentProgressCardReportDTO data)
        {
            return _comm.POSTPORTALData(data, "StudentProgressCardReportFacade/NDS_Get_Progresscard_Report");
        }

        //BCEHS
        public StudentProgressCardReportDTO Get_BCEHS_Progresscard_Report(StudentProgressCardReportDTO data)
        {
            return _comm.POSTPORTALData(data, "StudentProgressCardReportFacade/Get_BCEHS_Progresscard_Report");
        }

        //BIS
        public StudentProgressCardReportDTO BISStudentProgressCardReport(StudentProgressCardReportDTO data)
        {
            return _comm.POSTPORTALData(data, "StudentProgressCardReportFacade/BISStudentProgressCardReport");
        }
    }
}
