using CommonLibrary;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VisitorsManagement
{
    public class MasterLocationDelegate
    {
        CommonDelegate<Visitor_Management_Master_Location_DTO, Visitor_Management_Master_Location_DTO> COMVISITOR = new CommonDelegate<Visitor_Management_Master_Location_DTO, Visitor_Management_Master_Location_DTO>();

        public Visitor_Management_Master_Location_DTO getdetails(Visitor_Management_Master_Location_DTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "MasterLocationFacade/getdetails/");
        }

        public Visitor_Management_Master_Location_DTO saveRecorddata(Visitor_Management_Master_Location_DTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "MasterLocationFacade/saveRecorddata/");
        }
        
        public Visitor_Management_Master_Location_DTO editrecord(Visitor_Management_Master_Location_DTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "MasterLocationFacade/editrecord/");
        }
        public Visitor_Management_Master_Location_DTO deactiveY(Visitor_Management_Master_Location_DTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "MasterLocationFacade/deactiveY/");
        }
        
    }
}
