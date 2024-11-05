using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace IVRMUX.Delegates.com.vapstech.Portals.Student
{
    public class StudentHallticketDelegate
    {
        CommonDelegate<StudentHallticketDTO, StudentHallticketDTO> _comm = new CommonDelegate<StudentHallticketDTO, StudentHallticketDTO>();

        public StudentHallticketDTO GetLoadData(StudentHallticketDTO data)
        {
            return _comm.POSTPORTALData(data, "StudentHallticketFacade/GetLoadData");
        }
        public StudentHallticketDTO GetExamDetails(StudentHallticketDTO data)
        {
            return _comm.POSTPORTALData(data, "StudentHallticketFacade/GetExamDetails");
        }
        public StudentHallticketDTO GetReport(StudentHallticketDTO data)
        {
            return _comm.POSTPORTALData(data, "StudentHallticketFacade/GetReport");
        }
    }
}
