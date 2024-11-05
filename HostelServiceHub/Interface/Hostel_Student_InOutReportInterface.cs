using PreadmissionDTOs.com.vaps.Hostel;

namespace HostelServiceHub.Interface
{
 public interface Hostel_Student_InOutReportInterface
    {
        Hostel_Student_InOutDTO loaddata(Hostel_Student_InOutDTO data);  
        Hostel_Student_InOutDTO empname(Hostel_Student_InOutDTO data);
        Hostel_Student_InOutDTO savedetail(Hostel_Student_InOutDTO data);
        Hostel_Student_InOutDTO deleterec(Hostel_Student_InOutDTO data);

        // // ///////REPORT

        Hostel_Student_InOutDTO getloaddata(Hostel_Student_InOutDTO data);
        Hostel_Student_InOutDTO report(Hostel_Student_InOutDTO data);
    }
}
