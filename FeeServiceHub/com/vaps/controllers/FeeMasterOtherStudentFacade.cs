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
    public class FeeMasterOtherStudentFacade : Controller
    {
        public FeeMasterOtherStudentInterface _Istud;

        public FeeMasterOtherStudentFacade(FeeMasterOtherStudentInterface Istud)
        {
            _Istud = Istud;
        }

        [Route("getdetails/{id:int}")]
        public FeeMasterOtherStudentDTO getdetails(int id)
        {
            return _Istud.getdetails(id);
        }
        [Route("save")]
        public FeeMasterOtherStudentDTO save([FromBody]FeeMasterOtherStudentDTO data)
        {
            return _Istud.save(data);
        }
        [Route("edit/{id:int}")]
        public FeeMasterOtherStudentDTO edit(int id)
        {
            return _Istud.edit(id);
        }
        [Route("delete/{id:int}")]
        public FeeMasterOtherStudentDTO delete(int id)
        {
            return _Istud.delete(id);
        }
    }
}
