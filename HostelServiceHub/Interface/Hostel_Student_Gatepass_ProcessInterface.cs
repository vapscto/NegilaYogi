using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostelServiceHub.Interface
{
   public interface Hostel_Student_Gatepass_ProcessInterface
    {
        Hostel_Student_GatePassDTO getBasicData(Hostel_Student_GatePassDTO dto);
        Hostel_Student_GatePassDTO empdetails(Hostel_Student_GatePassDTO dto);


        Hostel_Student_GatePassDTO approvedrecord(Hostel_Student_GatePassDTO dto);

        ////------------------ Approval Report------------------------------
       Hostel_Student_GatePassDTO Onload(Hostel_Student_GatePassDTO dto);
        Hostel_Student_GatePassDTO getapprovalreport(Hostel_Student_GatePassDTO dto);

        //GatePass Admin Apply
        Hostel_Student_GatePassDTO getGatePassAdminApplyOnload(Hostel_Student_GatePassDTO data);
        Hostel_Student_GatePassDTO SaveUpdate(Hostel_Student_GatePassDTO data);
        Hostel_Student_GatePassDTO UpdateStatus(Hostel_Student_GatePassDTO data);
        Hostel_Student_GatePassDTO deactivate(Hostel_Student_GatePassDTO data);
        Hostel_Student_GatePassDTO Edit(Hostel_Student_GatePassDTO data);

    }
}
