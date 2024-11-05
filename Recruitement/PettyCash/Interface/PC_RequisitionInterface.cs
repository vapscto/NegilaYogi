using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.PettyCash.Interface
{
    public interface PC_RequisitionInterface
    {
        PC_RequisitionDTO onloaddata(PC_RequisitionDTO data);
        PC_RequisitionDTO onchangedept(PC_RequisitionDTO data);
        PC_RequisitionDTO saverecord(PC_RequisitionDTO data);
        PC_RequisitionDTO EditData(PC_RequisitionDTO data);
        PC_RequisitionDTO deactiveY(PC_RequisitionDTO data);
        PC_RequisitionDTO Viewdata(PC_RequisitionDTO data);
        PC_RequisitionDTO deactiveparticulars(PC_RequisitionDTO data);
    }
}
