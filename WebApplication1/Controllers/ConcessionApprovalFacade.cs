using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using WebApplication1.Interfaces;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class ConcessionApprovalFacade : Controller
    {
        public ConcessionApprovalInterface _con;

        public ConcessionApprovalFacade(ConcessionApprovalInterface conce)
        {
            _con = conce;
        }

        [HttpPost]
        [Route("loaddata")]
        public Preadmission_School_Registration_CatergoryDTO lodadata([FromBody] Preadmission_School_Registration_CatergoryDTO id)
        {
            Preadmission_School_Registration_CatergoryDTO data = new Preadmission_School_Registration_CatergoryDTO();
          
            return _con.loadta(id);
        }

      
        [Route("catchange")]
        public Preadmission_School_Registration_CatergoryDTO studet([FromBody] Preadmission_School_Registration_CatergoryDTO data)
        {
            return _con.getstudentdetails(data);
        }

        [Route("oncheck")]
        public Preadmission_School_Registration_CatergoryDTO oncheckstudet([FromBody] Preadmission_School_Registration_CatergoryDTO data)
        {
            return _con.oncheckgetstudentdetails(data);
        }


        [Route("saveconfirmdata")]
        public Preadmission_School_Registration_CatergoryDTO saveconfirmdata([FromBody] Preadmission_School_Registration_CatergoryDTO data)
        {
            return _con.confirmdta(data);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
