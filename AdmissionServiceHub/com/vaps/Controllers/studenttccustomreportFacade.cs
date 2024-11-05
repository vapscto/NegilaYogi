using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdmissionServiceHub.com.vaps.Interfaces;
using DataAccessMsSqlServerProvider;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class studenttccustomreportFacade : Controller
    {
        public studenttccustomreportInterface _report;
        private readonly DomainModelMsSqlServerContext _db;
        public studenttccustomreportFacade(studenttccustomreportInterface studTC, DomainModelMsSqlServerContext db)
        {
            _report = studTC;
            _db = db;
        }
        [Route("getinitialdata/{id:int}")]
        public studenttccustomreportDTO getinitialdata(int id)
        {
            return _report.getinitialdata(id);
        }
        [Route("changeyear")]
        public studenttccustomreportDTO changeyear([FromBody]studenttccustomreportDTO data)
        {
            return _report.changeyear(data);
        }
        [Route("changeclass")]
        public studenttccustomreportDTO changeclass([FromBody]studenttccustomreportDTO data)
        {
            return _report.changeclass(data);
        }
        [Route("changesection")]
        public studenttccustomreportDTO changesection([FromBody]studenttccustomreportDTO data)
        {
            return _report.changesection(data);
        }

        [Route("getTCdata")]
        public studenttccustomreportDTO getTCdata([FromBody] studenttccustomreportDTO data)
        {
            return _report.getTCdata(data);
        }
    }
}
