using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;
using Microsoft.AspNetCore.Http;
using CollegeServiceHub.Interface;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class CLGSubjectSchemeTypeFacadeController : Controller
    {
       public CLGSubjectSchemeTypeInterface _schemeType;
        public CLGSubjectSchemeTypeFacadeController(CLGSubjectSchemeTypeInterface scheme)
        {
            _schemeType = scheme;
        }
        // GET: api/values
        [Route("getschema")]
        public AdmCollegeSchemeTypeDTO getschema([FromBody] AdmCollegeSchemeTypeDTO data)
        {
            return _schemeType.getschema(data);
        }
        [Route("getsubject")]
        public AdmCollegeSchemeTypeDTO getsubject([FromBody] AdmCollegeSchemeTypeDTO data)
        {
            return _schemeType.getsubject(data);
        }



        [Route("savetype")]
        public AdmCollegeSchemeTypeDTO savetype([FromBody] AdmCollegeSchemeTypeDTO data)
        {
             return _schemeType.savetype(data);
        }

        [Route("savename")]
        public AdmCollegeSchemeTypeDTO savename([FromBody] AdmCollegeSchemeTypeDTO data)
        {
            return _schemeType.savename(data);
        }
        [Route("activedeactivebatch")]
        public AdmCollegeSchemeTypeDTO activedeactivebatch([FromBody] AdmCollegeSchemeTypeDTO data)
        {
            return _schemeType.activedeactivebatch(data);
        }
        [Route("activedeactivebatch1")]
        public AdmCollegeSchemeTypeDTO activedeactivebatch1([FromBody] AdmCollegeSchemeTypeDTO data)
        {
            return _schemeType.activedeactivebatch1(data);
        }

        [Route("getcombination")]
        public Adm_Prv_Sch_CombinationDTO getcombination([FromBody] Adm_Prv_Sch_CombinationDTO data)
        {
            return _schemeType.getcombination(data);
        }
        [Route("savecombination")]
        public Adm_Prv_Sch_CombinationDTO savecombination([FromBody] Adm_Prv_Sch_CombinationDTO data)
        {
            return _schemeType.savecombination(data);
        }
        [Route("activedeactivecomb")]
        public Adm_Prv_Sch_CombinationDTO activedeactivecomb([FromBody] Adm_Prv_Sch_CombinationDTO data)
        {
            return _schemeType.activedeactivecomb(data);
        }
        
    }
}
