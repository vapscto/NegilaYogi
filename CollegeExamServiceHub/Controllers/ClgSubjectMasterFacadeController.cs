using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeExamServiceHub.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeExamServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class ClgSubjectMasterFacadeController : Controller
    {
        public ClgSubjectMasterInterface inter;

        public ClgSubjectMasterFacadeController (ClgSubjectMasterInterface obj)
        {
            inter = obj;
        }

        [Route("getdetails")]

        public MasterSubjectAllMDTO getdetails([FromBody]MasterSubjectAllMDTO mas)
        {
            return inter.GetMasterSubDetails(mas);
        }

        [Route("getalldetails/{id:int}")]

        public MasterSubjectAllMDTO getalldetails(int id)
        {
            return inter.getalldetails(id);
        }
        [Route("Editdetails/{id:int}")]
        public MasterSubjectAllMDTO Getmasterdetails(int id)
        {
            return inter.EditMasterSubDetails(id);
        }

        [Route("savedetail")]
        public MasterSubjectAllMDTO Post([FromBody]MasterSubjectAllMDTO mast)
        {
            return inter.SaveMasterSubDetails(mast);
        }
        [Route("validateordernumber")]
        public MasterSubjectAllMDTO validateordernumber([FromBody]MasterSubjectAllMDTO mast)
        {
            return inter.validateordernumber(mast);
        }

        [Route("Deletedetails/{id:int}")]
        public MasterSubjectAllMDTO Deletedetails(int id)
        {
            return inter.DeleteMasterSubDetails(id);
        }

        [Route("savedata2")]
        public MasterSubjectAllMDTO savedata2([FromBody]MasterSubjectAllMDTO data)
        {
            return inter.savedata2(data);
        }
        
    }
}
