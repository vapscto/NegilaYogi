using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AlumniHub.Com.Interface;
using DataAccessMsSqlServerProvider;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Alumni;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AlumniHub.Com.Facade
{
    [Route("api/[controller]")]
    public class AlumnilettersFacade : Controller
    {
        private readonly DomainModelMsSqlServerContext _db;
        AlumnilettersInterface _int;
        public AlumnilettersFacade(AlumnilettersInterface stu, DomainModelMsSqlServerContext db)
        {
            _int = stu;
            _db = db;
        }
        [Route("ShowReport")]
        public AlumnilettersDTO ShowReport([FromBody] AlumnilettersDTO stud)
        {
            return _int.ShowReport(stud);
        }
        [Route("BindData")]
        public AlumnilettersDTO BindData([FromBody]AlumnilettersDTO castecategoryDTO)//int IVRMM_Id
        {     
            return _int.BindData(castecategoryDTO);
        }
        [Route("letterReport")]
        public AlumnilettersDTO letterReport([FromBody]AlumnilettersDTO castecategoryDTO)//int IVRMM_Id
        {     
            return _int.letterReport(castecategoryDTO);
        }
    }
}
