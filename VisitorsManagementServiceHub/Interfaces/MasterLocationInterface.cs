using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorsManagementServiceHub.Interfaces
{
   public interface MasterLocationInterface
    {

        Visitor_Management_Master_Location_DTO getdetails(Visitor_Management_Master_Location_DTO data);
        Visitor_Management_Master_Location_DTO saveRecorddata(Visitor_Management_Master_Location_DTO data);
        Visitor_Management_Master_Location_DTO editrecord(Visitor_Management_Master_Location_DTO data);
        Visitor_Management_Master_Location_DTO deactiveY(Visitor_Management_Master_Location_DTO data);
        
    }
}
