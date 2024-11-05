using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HostelServiceHub.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Hostel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HostelServiceHub.Facade
{
    [Route("api/[controller]")]
    public class Hostel_Allotment_ReportFacade : Controller
    {

        public Hostel_Allotment_ReportInterface _Interface;
        public Hostel_Allotment_ReportFacade(Hostel_Allotment_ReportInterface parameter)
        {
            _Interface = parameter;
        }

        [Route("getdata")]
        public Hostel_Allotment_ReportDTO getdata([FromBody] Hostel_Allotment_ReportDTO data)
        {
            return _Interface.getdata(data);
        }

        [Route("getreport")]
        public Task<Hostel_Allotment_ReportDTO> getreport([FromBody] Hostel_Allotment_ReportDTO data)
        {
            return _Interface.getreport(data);
        }

        //Hostel Allotment Graphical Presentation Report
        [Route("Get_GP_OnLoad_Report")]
        public Hostel_Allotment_ReportDTO Get_GP_OnLoad_Report([FromBody] Hostel_Allotment_ReportDTO data)
        {
            return _Interface.Get_GP_OnLoad_Report(data);
        }

        [Route("OnChangeHostel")]
        public Hostel_Allotment_ReportDTO OnChangeHostel([FromBody] Hostel_Allotment_ReportDTO data)
        {
            return _Interface.OnChangeHostel(data);
        }

        [Route("Get_GP_Report")]
        public Hostel_Allotment_ReportDTO Get_GP_Report([FromBody] Hostel_Allotment_ReportDTO data)
        {
            return _Interface.Get_GP_Report(data);
        }

        [Route("Get_GP_RoomWise_StudentAlloted_Details")]
        public Hostel_Allotment_ReportDTO Get_GP_RoomWise_StudentAlloted_Details([FromBody] Hostel_Allotment_ReportDTO data)
        {
            return _Interface.Get_GP_RoomWise_StudentAlloted_Details(data);
        }
    }
}
