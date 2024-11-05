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
    public class HostelAllotForCLGStudentFacadeController : Controller
    {
        public HostelAllotForCLGStudentInterface _Interface;
        public HostelAllotForCLGStudentFacadeController(HostelAllotForCLGStudentInterface parameter)
        {
            _Interface = parameter;
        }

        [Route("loaddata")]
        public Task<HostelAllotForCLGStudentDTO> loaddata([FromBody] HostelAllotForCLGStudentDTO data)
        {
            return _Interface.loaddata(data);
        }

        [Route("savedata")]
        public HostelAllotForCLGStudentDTO savedata([FromBody] HostelAllotForCLGStudentDTO data)
        {
            return _Interface.savedata(data);
        }

        [Route("get_studInfo")]
        public Task<HostelAllotForCLGStudentDTO> get_studInfo([FromBody] HostelAllotForCLGStudentDTO data)
        {
            return _Interface.get_studInfo(data);
        }
        [Route("floor")]
        public HostelAllotForCLGStudentDTO floor([FromBody] HostelAllotForCLGStudentDTO data)
        {
            return _Interface.floor(data);
        }
        [Route("room")]
        public HostelAllotForCLGStudentDTO room([FromBody] HostelAllotForCLGStudentDTO data)
        {
            return _Interface.room(data);
        }
        [Route("roomForVacateReport")]
        public HostelAllotForCLGStudentDTO roomForVacateReport([FromBody] HostelAllotForCLGStudentDTO data)
        {
            return _Interface.roomForVacateReport(data);
        }
        [Route("roomdetails")]
        public HostelAllotForCLGStudentDTO roomdetails([FromBody] HostelAllotForCLGStudentDTO data)
        {
            return _Interface.roomdetails(data);
        }
        [Route("get_roomdetails")]
        public HostelAllotForCLGStudentDTO get_roomdetails([FromBody] HostelAllotForCLGStudentDTO data)
        {
            return _Interface.get_roomdetails(data);
        }
        [Route("editdata")]
        public Task<HostelAllotForCLGStudentDTO> editdata([FromBody] HostelAllotForCLGStudentDTO data)
        {
            return _Interface.editdata(data);
        }

        [Route("requestApproved")]
        public HostelAllotForCLGStudentDTO requestApproved([FromBody] HostelAllotForCLGStudentDTO data)
        {
            return _Interface.requestApproved(data);
        }

        [Route("requestRejected")]
        public HostelAllotForCLGStudentDTO requestRejected([FromBody] HostelAllotForCLGStudentDTO data)
        {
            return _Interface.requestRejected(data);
        }
        //HostelT
        [Route("HostelT")]
        public HL_Hostel_Student_Transfer_CollegeDTO HostelT([FromBody] HL_Hostel_Student_Transfer_CollegeDTO data)
        {
            return _Interface.HostelT(data);
        }
        [Route("get_course")]
        public HostelAllotForCLGStudentDTO get_course([FromBody]HostelAllotForCLGStudentDTO data)
        {
            return _Interface.get_course(data);
        }
        [Route("get_branch")]
        public HostelAllotForCLGStudentDTO get_branch([FromBody]HostelAllotForCLGStudentDTO data)
        {
            return _Interface.get_branch(data);
        }
        [Route("get_sem")]
        public HostelAllotForCLGStudentDTO get_sem([FromBody]HostelAllotForCLGStudentDTO data)
        {
            return _Interface.get_sem(data);
        }

        //[Route("get_sec")]
        //public HostelAllotForCLGStudentDTO get_sec([FromBody]HostelAllotForCLGStudentDTO data)
        //{
        //    return _Interface.get_sec(data);
        //}
        [Route("get_student")]
        public HostelAllotForCLGStudentDTO get_student([FromBody]HostelAllotForCLGStudentDTO data)
        {
            return _Interface.get_student(data);
        }
    }
}
