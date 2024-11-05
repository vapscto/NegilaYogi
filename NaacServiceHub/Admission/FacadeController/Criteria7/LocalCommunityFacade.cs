using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Admission.Interface.Criteria7;
using PreadmissionDTOs.NAAC.Admission.Criteria7;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Admission.FacadeController.Criteria7
{
    [Route("api/[controller]")]
    public class LocalCommunityFacade : Controller
    {
        public LocalCommunityInterface inter;
        public LocalCommunityFacade(LocalCommunityInterface para)
        {
            inter = para;
        }

        [Route("loaddata")]
        public Task<LocalCommunityDTO> loaddata([FromBody] LocalCommunityDTO data)
        {
            return inter.loaddata(data);
        }
        [Route("getdata")]
        public LocalCommunityDTO getdata([FromBody] LocalCommunityDTO data)
        {
            return inter.getdata(data);
        }
        [Route("savedatatab1")]
        public LocalCommunityDTO savedatatab1([FromBody] LocalCommunityDTO data)
        {
            return inter.savedatatab1(data);
        }
        [Route("edittab1")]
        public LocalCommunityDTO edittab1([FromBody] LocalCommunityDTO data)
        {
            return inter.edittab1(data);
        }
        [Route("deactivYTab1")]
        public LocalCommunityDTO deactivYTab1([FromBody] LocalCommunityDTO data)
        {
            return inter.deactivYTab1(data);
        }

        [Route("deleteuploadfile")]
        public LocalCommunityDTO deleteuploadfile([FromBody] LocalCommunityDTO data)
        {
            return inter.deleteuploadfile(data);
        }
        [Route("viewuploadflies")]
        public LocalCommunityDTO viewuploadflies([FromBody] LocalCommunityDTO data)
        {
            return inter.viewuploadflies(data);
        }
        [Route("getcomment")]
        public LocalCommunityDTO getcomment([FromBody] LocalCommunityDTO data)
        {
            return inter.getcomment(data);
        }
        [Route("getfilecomment")]
        public LocalCommunityDTO getfilecomment([FromBody] LocalCommunityDTO data)
        {
            return inter.getfilecomment(data);
        }
        [Route("savecomments")]
        public LocalCommunityDTO savecomments([FromBody] LocalCommunityDTO data)
        {
            return inter.savecomments(data);
        }
        [Route("savefilewisecomments")]
        public LocalCommunityDTO savefilewisecomments([FromBody] LocalCommunityDTO data)
        {
            return inter.savefilewisecomments(data);
        }
    }
}
