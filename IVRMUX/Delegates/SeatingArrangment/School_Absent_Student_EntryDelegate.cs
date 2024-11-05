using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.SeatingArrangment;

namespace IVRMUX.Delegates.SeatingArrangment
{
    public class School_Absent_Student_EntryDelegate
    {
        CommonDelegate<School_Absent_Student_EntryDTO, School_Absent_Student_EntryDTO> _comm = new CommonDelegate<School_Absent_Student_EntryDTO, School_Absent_Student_EntryDTO>();

        public School_Absent_Student_EntryDTO GetAbsentStudentLoadData(School_Absent_Student_EntryDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "School_Absent_Student_EntryFacade/GetAbsentStudentLoadData");
        }
        public School_Absent_Student_EntryDTO OnChangeYear(School_Absent_Student_EntryDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "School_Absent_Student_EntryFacade/OnChangeYear");
        }
        public School_Absent_Student_EntryDTO OnChangeClass(School_Absent_Student_EntryDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "School_Absent_Student_EntryFacade/OnChangeClass");
        }
        public School_Absent_Student_EntryDTO OnChangeSection(School_Absent_Student_EntryDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "School_Absent_Student_EntryFacade/OnChangeSection");
        }
        public School_Absent_Student_EntryDTO SearchData(School_Absent_Student_EntryDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "School_Absent_Student_EntryFacade/SearchData");
        }
        public School_Absent_Student_EntryDTO SaveData(School_Absent_Student_EntryDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "School_Absent_Student_EntryFacade/SaveData");
        }

        //Absent Report
        public School_Absent_Student_EntryDTO GetAbsentStudentReportLoadData(School_Absent_Student_EntryDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "School_Absent_Student_EntryFacade/GetAbsentStudentReportLoadData");
        }
        public School_Absent_Student_EntryDTO OnChangeYearAbsentReport(School_Absent_Student_EntryDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "School_Absent_Student_EntryFacade/OnChangeYearAbsentReport");
        }
        public School_Absent_Student_EntryDTO OnChangeClassAbsentReport(School_Absent_Student_EntryDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "School_Absent_Student_EntryFacade/OnChangeClassAbsentReport");
        }
        public School_Absent_Student_EntryDTO OnChangeSectionAbsentReport(School_Absent_Student_EntryDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "School_Absent_Student_EntryFacade/OnChangeSectionAbsentReport");
        }
        public School_Absent_Student_EntryDTO GetSchoolAbsentStudentReport(School_Absent_Student_EntryDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "School_Absent_Student_EntryFacade/GetSchoolAbsentStudentReport");
        }
    }
}
