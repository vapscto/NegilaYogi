using CommonLibrary;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Hostel
{
    public class Hostel_Allotment_ReportDelegate
    {
        CommonDelegate<Hostel_Allotment_ReportDTO, Hostel_Allotment_ReportDTO> _comm = new CommonDelegate<Hostel_Allotment_ReportDTO, Hostel_Allotment_ReportDTO>();

        public Hostel_Allotment_ReportDTO getdata(Hostel_Allotment_ReportDTO data)
        {
            return _comm.Post_Hostel(data, "Hostel_Allotment_ReportFacade/getdata/");
        }
        public Hostel_Allotment_ReportDTO getreport(Hostel_Allotment_ReportDTO data)
        {
            return _comm.Post_Hostel(data, "Hostel_Allotment_ReportFacade/getreport/");
        }

        //Hostel Allotment Graphical Presentation Report
        public Hostel_Allotment_ReportDTO Get_GP_OnLoad_Report(Hostel_Allotment_ReportDTO data)
        {
            return _comm.Post_Hostel(data, "Hostel_Allotment_ReportFacade/Get_GP_OnLoad_Report/");
        }
        public Hostel_Allotment_ReportDTO OnChangeHostel(Hostel_Allotment_ReportDTO data)
        {
            return _comm.Post_Hostel(data, "Hostel_Allotment_ReportFacade/OnChangeHostel/");
        }
        public Hostel_Allotment_ReportDTO Get_GP_Report(Hostel_Allotment_ReportDTO data)
        {
            return _comm.Post_Hostel(data, "Hostel_Allotment_ReportFacade/Get_GP_Report/");
        }
        public Hostel_Allotment_ReportDTO Get_GP_RoomWise_StudentAlloted_Details(Hostel_Allotment_ReportDTO data)
        {
            return _comm.Post_Hostel(data, "Hostel_Allotment_ReportFacade/Get_GP_RoomWise_StudentAlloted_Details/");
        }
        
    }
}
