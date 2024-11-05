using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeFeeService.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class OtherCollegeStudentEntryFacade : Controller
    {
        public OtherCollegeStudentEntryInterface _Istud;

        public OtherCollegeStudentEntryFacade(OtherCollegeStudentEntryInterface Istud)
        {
            _Istud = Istud;
        }

        [Route("getdetails/{id:int}")]
        public Fee_Master_College_OtherStudentsDTO getdetails(int id)
        {
            return _Istud.getdetails(id);
        }
        [Route("save")]
        public Fee_Master_College_OtherStudentsDTO save([FromBody]Fee_Master_College_OtherStudentsDTO data)
        {
            return _Istud.save(data);
        }
        [Route("edit/{id:int}")]
        public Fee_Master_College_OtherStudentsDTO edit(int id)
        {
            return _Istud.edit(id);
        }
        [Route("delete/{id:int}")]
        public Fee_Master_College_OtherStudentsDTO delete(int id)
        {
            return _Istud.delete(id);
        }
    }
}
