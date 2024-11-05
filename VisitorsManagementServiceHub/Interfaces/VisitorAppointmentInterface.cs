using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorsManagementServiceHub.Interfaces
{
   public interface VisitorAppointmentInterface
    {
        Visitor_Management_Appointment_DTO getDetails(Visitor_Management_Appointment_DTO data);
        Visitor_Management_Appointment_DTO saveData(Visitor_Management_Appointment_DTO data);
        Visitor_Management_Appointment_DTO EditDetails(Visitor_Management_Appointment_DTO id);
        Visitor_Management_Appointment_DTO deactivate(Visitor_Management_Appointment_DTO data);
        Visitor_Management_Appointment_DTO deleteuploadfile(Visitor_Management_Appointment_DTO data);
        Visitor_Management_Appointment_DTO viewuploadflies(Visitor_Management_Appointment_DTO data);
        Visitor_Management_Appointment_DTO visitormailtrigger(Visitor_Management_Appointment_DTO data);
        Visitor_Management_Appointment_DTO visitorappoinmentstatus(Visitor_Management_Appointment_DTO data);
        Visitor_Management_Appointment_DTO todayappointments(Visitor_Management_Appointment_DTO data);

    }
}
