using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace PortalHub.com.vaps.Student.Interfaces
{
    public interface FeeReceiptInterface
    {
        StudentDashboardDTO getloaddata(StudentDashboardDTO data);
        StudentDashboardDTO getrecdetails(StudentDashboardDTO data);
        StudentDashboardDTO preadmissiongetrecdetails(StudentDashboardDTO data);
        Task<FeeStudentTransactionDTO> printreceipt([FromBody] FeeStudentTransactionDTO sddto);

        StudentDashboardDTO preadmissiongetdetails(StudentDashboardDTO data);
        StudentDashboardDTO getstudetails(StudentDashboardDTO data);
    }
}
