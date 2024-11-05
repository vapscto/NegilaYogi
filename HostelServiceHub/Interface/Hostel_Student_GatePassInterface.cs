using PreadmissionDTOs.com.vaps.Hostel;

namespace HostelServiceHub.Interface
{
   public  interface Hostel_Student_GatePassInterface
    {
            Hostel_Student_GatePassDTO getBasicData(Hostel_Student_GatePassDTO dto);
            Hostel_Student_GatePassDTO SaveUpdate(Hostel_Student_GatePassDTO dto);
            Hostel_Student_GatePassDTO Edit(Hostel_Student_GatePassDTO dto);
        Hostel_Student_GatePassDTO deactivate(Hostel_Student_GatePassDTO dto);

    }
}

