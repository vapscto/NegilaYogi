
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]

    public class subjectmasterFacadeController : Controller
    {
        public subjectmasterInterface _subjectmaster;
        public subjectmasterFacadeController(subjectmasterInterface subjectmaster)
        {
            _subjectmaster = subjectmaster;
        }

        [Route("Getdetails")]
        public subjectmasterDTO Getdetails([FromBody]subjectmasterDTO subjectmasterDTO)//int IVRMM_Id
        {
           
            return _subjectmaster.GetsubjectmasterData(subjectmasterDTO);
           
        }

        [Route("GetSelectedRowDetails/{id:int}")]
        public subjectmasterDTO GetSelectedRowDetails(int ID)
        {
           
            return _subjectmaster.GetSelectedRowDetails(ID);
        }
    
        [HttpPost]
        public subjectmasterDTO Post([FromBody] subjectmasterDTO masterMDT)
        {
          
            return _subjectmaster.subjectmasterData(masterMDT);
        }

        [HttpDelete]
        [Route("MasterDeleteModulesDATA/{id:int}")]
        public subjectmasterDTO MasterDeleteModulesDATA(int ID)
        {           
            return _subjectmaster.MasterDeleteModulesData(ID);
        }     
    }
}
