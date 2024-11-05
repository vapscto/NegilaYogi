using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using WebApplication1.Interfaces;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class InstitutionTemplateFacadeController : Controller
    {
        public InstitutionTemplateInterface _Iinstitutetemplate;

        public InstitutionTemplateFacadeController(InstitutionTemplateInterface Iobj)
        {
            _Iinstitutetemplate = Iobj;
        }

        [Route("getdrpdwns/{id:int}")]
        public Task<CommonDTO> getBasicData(int id)
        {
            return _Iinstitutetemplate.getBasicData(id);
        }

        [Route("geteditdata/{id:int}")]
        public InstitutionTemplateDTO getEditData(int Id)
        {
            return _Iinstitutetemplate.getEditData(Id);
        }

        [Route("deleteRec/{id:int}")]
        public void deleteData(int id)
        {
            _Iinstitutetemplate.deleteRec(id);
        }

        [Route("deactiveactive/{id:int}")]
        public void DeactiveActive(int id)
        {
            _Iinstitutetemplate.DeactiveActive(id);
        }

        [HttpPost]
        public InstitutionTemplateDTO SaveInstituteTemp([FromBody] InstitutionTemplateDTO InstTemp)
        {
            return _Iinstitutetemplate.SaveInstituteTemp(InstTemp);
        }
    }
}
