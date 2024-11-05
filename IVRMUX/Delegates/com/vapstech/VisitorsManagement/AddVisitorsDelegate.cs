using CommonLibrary;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VisitorsManagement
{
    public class AddVisitorsDelegate
    {
        CommonDelegate<AddVisitorsDTO, AddVisitorsDTO> COMVISITOR = new CommonDelegate<AddVisitorsDTO, AddVisitorsDTO>();

        public AddVisitorsDTO saveData(AddVisitorsDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AddVisitorsFacade/saveData/");
        }

        public AddVisitorsDTO getDetails(AddVisitorsDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AddVisitorsFacade/getDetails/");
        }

        public AddVisitorsDTO EditDetails(AddVisitorsDTO id)
        {
            return COMVISITOR.POSTDataVisitors(id, "AddVisitorsFacade/EditDetails/");
        }

        public AddVisitorsDTO deactivate(AddVisitorsDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AddVisitorsFacade/deactivate/");
        }

        public AddVisitorsDTO GetMultiVisitorDetails(AddVisitorsDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AddVisitorsFacade/GetMultiVisitorDetails/");
        }

        public AddVisitorsDTO GetVisitorDetails(AddVisitorsDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AddVisitorsFacade/GetVisitorDetails/");
        }

        public AddVisitorsDTO UpdateStatus(AddVisitorsDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AddVisitorsFacade/UpdateStatus/");
        }
        public AddVisitorsDTO BlockOrUblockVisitor(AddVisitorsDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AddVisitorsFacade/BlockOrUblockVisitor/");
        }
        public AddVisitorsDTO GetVisitorMultiDocuments(AddVisitorsDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AddVisitorsFacade/GetVisitorMultiDocuments/");
        }
        public AddVisitorsDTO GetVisitorIdCardDetails(AddVisitorsDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AddVisitorsFacade/GetVisitorIdCardDetails/");
        }
        public AddVisitorsDTO UpdateIDCardDetails(AddVisitorsDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AddVisitorsFacade/UpdateIDCardDetails/");
        }
        public AddVisitorsDTO SearchPreviousVisitor(AddVisitorsDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AddVisitorsFacade/SearchPreviousVisitor/");
        }
        public AddVisitorsDTO AddPreviousVisitorDetails(AddVisitorsDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AddVisitorsFacade/AddPreviousVisitorDetails/");
        }

        // Assign Details

        public AddVisitorsDTO getAssignDetails(AddVisitorsDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AddVisitorsFacade/getAssignDetails/");
        }
        public AddVisitorsDTO getVisitorAssignDetails(AddVisitorsDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AddVisitorsFacade/getVisitorAssignDetails/");
        }
        public AddVisitorsDTO saveAssignedData(AddVisitorsDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AddVisitorsFacade/saveAssignedData/");
        }
        public AddVisitorsDTO GetVisitorAssginDetails(AddVisitorsDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AddVisitorsFacade/GetVisitorAssginDetails/");
        }

        // Appointment Visitor

        public AddVisitorsDTO SearchAppVisitors(AddVisitorsDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AddVisitorsFacade/SearchAppVisitors/");
        }
        public AddVisitorsDTO GetAppointmentVisitorDetails(AddVisitorsDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AddVisitorsFacade/GetAppointmentVisitorDetails/");
        }        
    }
}
