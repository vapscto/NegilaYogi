using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Student.Interfaces
{
    public interface StudentHallticketInterface
    {
        StudentHallticketDTO GetLoadData(StudentHallticketDTO data);
        StudentHallticketDTO GetExamDetails(StudentHallticketDTO data);
        StudentHallticketDTO GetReport(StudentHallticketDTO data);
    }
}
