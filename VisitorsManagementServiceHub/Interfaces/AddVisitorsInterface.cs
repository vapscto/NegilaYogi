using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorsManagementServiceHub.Interfaces
{
    public interface AddVisitorsInterface
    {
        AddVisitorsDTO getDetails(AddVisitorsDTO data);
        Task<AddVisitorsDTO> saveDataAsync(AddVisitorsDTO data);
        AddVisitorsDTO EditDetails(AddVisitorsDTO id);
        AddVisitorsDTO deactivate(AddVisitorsDTO data);
        AddVisitorsDTO GetMultiVisitorDetails(AddVisitorsDTO data);
        AddVisitorsDTO GetVisitorDetails(AddVisitorsDTO data);
        Task<AddVisitorsDTO> UpdateStatus(AddVisitorsDTO data);
        AddVisitorsDTO BlockOrUblockVisitor(AddVisitorsDTO data);
        AddVisitorsDTO GetVisitorMultiDocuments(AddVisitorsDTO data);
        AddVisitorsDTO GetVisitorIdCardDetails(AddVisitorsDTO data);
        AddVisitorsDTO UpdateIDCardDetails(AddVisitorsDTO data);
        AddVisitorsDTO SearchPreviousVisitor(AddVisitorsDTO data);
        AddVisitorsDTO AddPreviousVisitorDetails(AddVisitorsDTO data);
        //Assign Details
        AddVisitorsDTO getAssignDetails(AddVisitorsDTO data);
        AddVisitorsDTO getVisitorAssignDetails(AddVisitorsDTO data);
        Task<AddVisitorsDTO> saveAssignedData(AddVisitorsDTO data);
        AddVisitorsDTO GetVisitorAssginDetails(AddVisitorsDTO data);
        AddVisitorsDTO SearchAppVisitors(AddVisitorsDTO data);
        AddVisitorsDTO GetAppointmentVisitorDetails(AddVisitorsDTO data);
    }
}