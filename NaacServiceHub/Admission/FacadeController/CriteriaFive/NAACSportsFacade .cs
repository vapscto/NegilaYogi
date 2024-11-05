using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Admission.Interface;
using PreadmissionDTOs.NAAC.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Admission.FacadeController
{
    [Route("api/[controller]")]
    public class NAACSportsFacade : Controller
    {
        public NAACSportsInterface _Interface;

        public NAACSportsFacade(NAACSportsInterface q)
        {
            _Interface = q;
        }

        [Route("loaddata")]
        public NAACSportsDTO loaddata([FromBody] NAACSportsDTO data)
        {
            return _Interface.loaddata(data);
        }
    

        [Route("save")]
        public NAACSportsDTO save([FromBody] NAACSportsDTO data)
        {
            return _Interface.save(data);
        }

        [Route("deactiveStudent")]
        public NAACSportsDTO deactiveStudent([FromBody] NAACSportsDTO data)
        {
            return _Interface.deactiveStudent(data);
        }

        [Route("EditData")]
        public NAACSportsDTO EditData([FromBody]NAACSportsDTO category)
        {
            return _Interface.EditData(category);
        }


        [Route("viewuploadflies")]
        public NAACSportsDTO viewuploadflies([FromBody]NAACSportsDTO category)
        {
            return _Interface.viewuploadflies(category);
        }

        [Route("deleteuploadfile")]
        public NAACSportsDTO deleteuploadfile([FromBody]NAACSportsDTO category)
        {
            return _Interface.deleteuploadfile(category);
        }
        
         [Route("get_course")]
        public NAACSportsDTO get_course([FromBody]NAACSportsDTO category)
        {
            return _Interface.get_course(category);
        }
         [Route("get_branch")]
        public NAACSportsDTO get_branch([FromBody]NAACSportsDTO category)
        {
            return _Interface.get_branch(category);
        }
         [Route("get_sems")]
        public NAACSportsDTO get_sems([FromBody]NAACSportsDTO category)
        {
            return _Interface.get_sems(category);
        }
         [Route("get_section")]
        public NAACSportsDTO get_section([FromBody]NAACSportsDTO category)
        {
            return _Interface.get_section(category);
        }
         [Route("GetStudentDetails")]
        public NAACSportsDTO GetStudentDetails([FromBody]NAACSportsDTO category)
        {
            return _Interface.GetStudentDetails(category);
        }


        [Route("savemedicaldatawisecomments")]
        public NAACSportsDTO savemedicaldatawisecomments([FromBody]NAACSportsDTO category)
        {
            return _Interface.savemedicaldatawisecomments(category);
        }
        [Route("getcomment")]
        public NAACSportsDTO getcomment([FromBody]NAACSportsDTO category)
        {
            return _Interface.getcomment(category);
        }
        [Route("getfilecomment")]
        public NAACSportsDTO getfilecomment([FromBody]NAACSportsDTO category)
        {
            return _Interface.getfilecomment(category);
        }
        [Route("savefilewisecomments")]
        public NAACSportsDTO savefilewisecomments([FromBody]NAACSportsDTO category)
        {
            return _Interface.savefilewisecomments(category);
        }

    }
}
