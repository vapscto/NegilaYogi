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
    public class NAACAlumniContributionFacade : Controller
    {
        public NAACAlumniContributionInterface _Interface;

        public NAACAlumniContributionFacade(NAACAlumniContributionInterface q)
        {
            _Interface = q;
        }

        [Route("loaddatahsu")]
        public NAACAlumniContributionDTO loaddatahsu([FromBody] NAACAlumniContributionDTO data)
        {
            return _Interface.loaddatahsu(data);
        }
        [Route("loaddata")]
        public NAACAlumniContributionDTO loaddata([FromBody] NAACAlumniContributionDTO data)
        {
            return _Interface.loaddata(data);
        }
        [Route("save")]
        public NAACAlumniContributionDTO save([FromBody] NAACAlumniContributionDTO data)
        {
            return _Interface.save(data);
        }
        [Route("savehsu")]
        public NAACAlumniContributionDTO savehsu([FromBody] NAACAlumniContributionDTO data)
        {
            return _Interface.savehsu(data);
        }

        [Route("deactiveStudent")]
        public NAACAlumniContributionDTO deactiveStudent([FromBody] NAACAlumniContributionDTO data)
        {
            return _Interface.deactiveStudent(data);
        }

        [Route("EditData")]
        public NAACAlumniContributionDTO EditData([FromBody]NAACAlumniContributionDTO category)
        {
            return _Interface.EditData(category);
        }
            [Route("viewuploadflies")]
        public NAACAlumniContributionDTO viewuploadflies([FromBody]NAACAlumniContributionDTO category)
        {
            return _Interface.viewuploadflies(category);
        }
            [Route("deleteuploadfile")]
        public NAACAlumniContributionDTO deleteuploadfile([FromBody]NAACAlumniContributionDTO category)
        {
            return _Interface.deleteuploadfile(category);
        }
        [Route("savemedicaldatawisecomments")]
        public NAACAlumniContributionDTO savemedicaldatawisecomments([FromBody]NAACAlumniContributionDTO category)
        {
            return _Interface.savemedicaldatawisecomments(category);
        }
        [Route("getcomment")]
        public NAACAlumniContributionDTO getcomment([FromBody]NAACAlumniContributionDTO category)
        {
            return _Interface.getcomment(category);
        }
        [Route("getfilecomment")]
        public NAACAlumniContributionDTO getfilecomment([FromBody]NAACAlumniContributionDTO category)
        {
            return _Interface.getfilecomment(category);
        }
        [Route("savefilewisecomments")]
        public NAACAlumniContributionDTO savefilewisecomments([FromBody]NAACAlumniContributionDTO category)
        {
            return _Interface.savefilewisecomments(category);
        }
    }
}
