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
    public class HostelAllotForStudentFacade : Controller
    {
        public HostelAllotForStudentInterface _Interface;
        public HostelAllotForStudentFacade(HostelAllotForStudentInterface parameter)
        {
            _Interface = parameter;
        }

        [Route("loaddata")]
        public Task<HostelAllotForStudent_DTO> loaddata([FromBody] HostelAllotForStudent_DTO data)
        {
            return _Interface.loaddata(data);
        }

        [Route("savedata")]
        public HostelAllotForStudent_DTO savedata([FromBody] HostelAllotForStudent_DTO data)
        {
            return _Interface.savedata(data);
        }

        [Route("get_studInfo")]
        public Task<HostelAllotForStudent_DTO> get_studInfo([FromBody] HostelAllotForStudent_DTO data)
        {
            return _Interface.get_studInfo(data);
        }
        [Route("get_roomdetails")]
        public HostelAllotForStudent_DTO get_roomdetails([FromBody] HostelAllotForStudent_DTO data)
        {
            return _Interface.get_roomdetails(data);
        }
        [Route("editdata")]
        public Task<HostelAllotForStudent_DTO> editdata([FromBody] HostelAllotForStudent_DTO data)
        {
            return _Interface.editdata(data);
        }
    }
}
