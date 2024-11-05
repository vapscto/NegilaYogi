using CommonLibrary;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VisitorsManagement
{
    public class VisitorAppointmentDelegate
    {
        CommonDelegate<Visitor_Management_Appointment_DTO, Visitor_Management_Appointment_DTO> COMVISITOR = new CommonDelegate<Visitor_Management_Appointment_DTO, Visitor_Management_Appointment_DTO>();

        public Visitor_Management_Appointment_DTO saveData(Visitor_Management_Appointment_DTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "VisitorAppointmentFacade/saveData/");
        }

        public Visitor_Management_Appointment_DTO getDetails(Visitor_Management_Appointment_DTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "VisitorAppointmentFacade/getDetails/");
        }

        public Visitor_Management_Appointment_DTO EditDetails(Visitor_Management_Appointment_DTO id)
        {
            return COMVISITOR.POSTDataVisitors(id, "VisitorAppointmentFacade/EditDetails/");
        }

        public Visitor_Management_Appointment_DTO deactivate(Visitor_Management_Appointment_DTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "VisitorAppointmentFacade/deactivate/");
        }
        public Visitor_Management_Appointment_DTO viewuploadflies(Visitor_Management_Appointment_DTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "VisitorAppointmentFacade/viewuploadflies/");
        }
          public Visitor_Management_Appointment_DTO deleteuploadfile(Visitor_Management_Appointment_DTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "VisitorAppointmentFacade/deleteuploadfile/");
        }

    }
}
