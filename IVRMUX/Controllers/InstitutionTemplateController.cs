using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class InstitutionTemplateController : Controller
    {
        private static FacadeUrl _config;
        private FacadeUrl fdu = new FacadeUrl();
        public InstitutionTemplateDelegate _instTempDel = new InstitutionTemplateDelegate();

        public InstitutionTemplateController(IOptions<FacadeUrl> settings) { _config = settings.Value; new InstitutionTemplateDelegate(_config); fdu = _config; }

        [Route("getbasicdropdowns/{id:int}")]
        public CommonDTO getBasicData(int id)
        {
            return _instTempDel.getBasicData(id);
        }

        [Route("getEditData/{Id:int}")]
        public InstitutionTemplateDTO getEdtDatas(int Id)
        {
            return _instTempDel.getEditData(Id);
        }

        [Route("deactive/{id:int}")]
        public void Deactive_Active(int id)
        {
            _instTempDel.DeatciveActive(id);
        }

        [Route("deletedetails/{id:int}")]
        public void deleteRec(int id)
        {
            _instTempDel.deleteRec(id);
        }

        [HttpPost]
        public InstitutionTemplateDTO SaveInstituteTemplate([FromBody] InstitutionTemplateDTO InstTemp)
        {
           return _instTempDel.SaveInstitueTemplate(InstTemp);
        }
    }
}
