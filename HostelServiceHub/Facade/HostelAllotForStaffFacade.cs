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
    public class HostelAllotForStaffFacade : Controller
    {
        public HostelAllotForStaffInterface _Interface;
        public HostelAllotForStaffFacade(HostelAllotForStaffInterface parameter)
        {
            _Interface = parameter;
        }

        [Route("loaddata")]
        public Task<HostelAllotForStaff_DTO> loaddata([FromBody] HostelAllotForStaff_DTO data)
        {
            return _Interface.loaddata(data);
        }

        [Route("savedata")]
        public HostelAllotForStaff_DTO savedata([FromBody] HostelAllotForStaff_DTO data)
        {
            return _Interface.savedata(data);
        }

        [Route("get_studInfo")]
        public Task<HostelAllotForStaff_DTO> get_studInfo([FromBody] HostelAllotForStaff_DTO data)
        {
            return _Interface.get_studInfo(data);
        }
        [Route("get_roomdetails")]
        public HostelAllotForStaff_DTO get_roomdetails([FromBody] HostelAllotForStaff_DTO data)
        {
            return _Interface.get_roomdetails(data);
        }
        [Route("editdata")]
        public Task<HostelAllotForStaff_DTO> editdata([FromBody] HostelAllotForStaff_DTO data)
        {
            return _Interface.editdata(data);
        }
        [Route("getdesg")]
        public HostelAllotForStaff_DTO getdesg([FromBody] HostelAllotForStaff_DTO data)
        {
            return _Interface.getdesg(data);
        }
        [Route("deactivYTab1")]
        public HostelAllotForStaff_DTO deactivYTab1([FromBody] HostelAllotForStaff_DTO data)
        {
            return _Interface.deactivYTab1(data);
        }
    }
}
