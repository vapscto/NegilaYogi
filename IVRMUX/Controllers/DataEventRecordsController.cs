using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DomainModel;
using DomainModel.Model;
using Newtonsoft.Json;
using corewebapi18072016.Delegates;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class DataEventRecordsController : Controller
    {
        private readonly IDataAccessProvider _dataAccessProvider;
        //PreadmissionDelegate pre = new PreadmissionDelegate();
        public DataEventRecordsController(IDataAccessProvider dataAccessProvider)
        {
            _dataAccessProvider = dataAccessProvider;
        }

        [HttpGet]
        public IEnumerable<DataEventRecord> Get()
        {
            return _dataAccessProvider.GetDataEventRecords();
        }

        [HttpGet]
        [Route("SourceInfos")]
        public IEnumerable<SourceInfo> GetSourceInfos(bool withChildren)
        {
            return _dataAccessProvider.GetSourceInfos(withChildren);
        }

        [HttpGet("{id}")]
        public string Get(long id)
        {
            //return  pre.getData(id);
            //return _dataAccessProvider.GetDataEventRecord(id);
            //invoke delegate;

            return "";
        }

        [HttpPost]
        public void Post([FromBody]DataEventRecord value)
        {
            DataEventRecord re = new DataEventRecord();
            //return pre.postdata(value);
            _dataAccessProvider.AddDataEventRecord(value);
        }

        [HttpPut("{id}")]
        public void Put(long id, [FromBody]DataEventRecord value)
        {
            _dataAccessProvider.UpdateDataEventRecord(id, value);
        }

        [HttpDelete("{id}")]
        public void Delete(long id)
        {
            _dataAccessProvider.DeleteDataEventRecord(id);
        }
    }
}
