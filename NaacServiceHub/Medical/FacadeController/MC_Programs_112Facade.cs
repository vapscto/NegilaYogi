using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Medical.Interface;
using PreadmissionDTOs.NAAC.Medical;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Medical.FacadeController
{
    [Route("api/[controller]")]
    public class MC_Programs_112Facade : Controller
    {
        public MC_Programs_112Interface _Iobj;
        public MC_Programs_112Facade(MC_Programs_112Interface para)
        {
            _Iobj = para;
        }

        [Route("loaddata")]
        public MC_Programs_112_DTO loaddata([FromBody] MC_Programs_112_DTO data)
        {
            return _Iobj.loaddata(data);
        }
        [Route("savedata")]
        public MC_Programs_112_DTO savedata([FromBody] MC_Programs_112_DTO data)
        {
            return _Iobj.savedata(data);
        }
        [Route("editdata")]
        public MC_Programs_112_DTO editdata([FromBody]MC_Programs_112_DTO data)
        {
            return _Iobj.editdata(data);
        }
        [Route("deactive_Y")]
        public MC_Programs_112_DTO deactive_Y([FromBody]MC_Programs_112_DTO data)
        {
            return _Iobj.deactive_Y(data);
        }
        [Route("viewuploadflies")]
        public MC_Programs_112_DTO viewuploadflies([FromBody]MC_Programs_112_DTO data)
        {
            return _Iobj.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public MC_Programs_112_DTO deleteuploadfile([FromBody]MC_Programs_112_DTO data)
        {
            return _Iobj.deleteuploadfile(data);
        }
        [Route("StaffList_Boss")]
        public MC_Programs_112_DTO StaffList_Boss([FromBody]MC_Programs_112_DTO data)
        {
            return _Iobj.StaffList_Boss(data);
        }
        [Route("StaffList_Council")]
        public MC_Programs_112_DTO StaffList_Council([FromBody]MC_Programs_112_DTO data)
        {
            return _Iobj.StaffList_Council(data);
        }
    }
}
