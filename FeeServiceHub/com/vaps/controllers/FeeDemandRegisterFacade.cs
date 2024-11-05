using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeDemandRegisterFacade : Controller
    {
        FeeDemandRegisterInterface _inter;
        public FeeDemandRegisterFacade(FeeDemandRegisterInterface inter)
        {
            _inter = inter;
        }
        [Route("getinitialdata")]
        public async Task<FeeDemandRegisterDTO> getinitialdata([FromBody]FeeDemandRegisterDTO data)
        {
            return await _inter.getinitialdata(data);
        }
        [Route("getStudentByYrClsSec")]
        public async Task<FeeDemandRegisterDTO> getStudentByYrClsSec([FromBody] FeeDemandRegisterDTO data)
        {
            return await _inter.getStudentByYrClsSec(data);
        }
        [Route("getgroupByCG")]
        public async Task<FeeDemandRegisterDTO> getgroupByCG([FromBody] FeeDemandRegisterDTO data)
        {
            return await _inter.getgroupByCG(data);
        }
        [Route("getReport")]
        public async Task<FeeDemandRegisterDTO> getReport([FromBody] FeeDemandRegisterDTO data)
        {
            return await _inter.getReport(data);
        }


    }
}
