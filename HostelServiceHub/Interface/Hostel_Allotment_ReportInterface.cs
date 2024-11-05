using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostelServiceHub.Interface
{
   public interface Hostel_Allotment_ReportInterface
    {
        Hostel_Allotment_ReportDTO getdata(Hostel_Allotment_ReportDTO data);
        Task<Hostel_Allotment_ReportDTO> getreport(Hostel_Allotment_ReportDTO data);

        //Hostel Allotment Graphical Presentation Report
        Hostel_Allotment_ReportDTO Get_GP_OnLoad_Report(Hostel_Allotment_ReportDTO data);
        Hostel_Allotment_ReportDTO OnChangeHostel(Hostel_Allotment_ReportDTO data);
        Hostel_Allotment_ReportDTO Get_GP_Report(Hostel_Allotment_ReportDTO data);
        Hostel_Allotment_ReportDTO Get_GP_RoomWise_StudentAlloted_Details(Hostel_Allotment_ReportDTO data);
    }
}
