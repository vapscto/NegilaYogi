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
    public class LocationalAdvtgFacade : Controller
    {
        public LocationalAdvtgInterface inter;
        public LocationalAdvtgFacade(LocationalAdvtgInterface para)
        {
            inter = para;
        }
       
        [Route("loaddata")]
        public Task<LocationalAdvtgDTO> loaddata([FromBody] LocationalAdvtgDTO data)
        {
            return inter.loaddata(data);
        }
        [Route("getdata")]
        public LocationalAdvtgDTO getdata([FromBody] LocationalAdvtgDTO data)
        {
            return inter.getdata(data);
        }
        [Route("savedatatab1")]
        public LocationalAdvtgDTO savedatatab1([FromBody] LocationalAdvtgDTO data)
        {
            return inter.savedatatab1(data);
        }
        [Route("edittab1")]
        public LocationalAdvtgDTO edittab1([FromBody] LocationalAdvtgDTO data)
        {
            return inter.edittab1(data);
        }
        [Route("deactivYTab1")]
        public LocationalAdvtgDTO deactivYTab1([FromBody] LocationalAdvtgDTO data)
        {
            return inter.deactivYTab1(data);
        }

        [Route("deleteuploadfile")]
        public LocationalAdvtgDTO deleteuploadfile([FromBody] LocationalAdvtgDTO data)
        {
            return inter.deleteuploadfile(data);
        }
        [Route("viewuploadflies")]
        public LocationalAdvtgDTO viewuploadflies([FromBody] LocationalAdvtgDTO data)
        {
            return inter.viewuploadflies(data);
        }
        [Route("getcomment")]
        public LocationalAdvtgDTO getcomment([FromBody] LocationalAdvtgDTO data)
        {
            return inter.getcomment(data);
        }
        [Route("getfilecomment")]
        public LocationalAdvtgDTO getfilecomment([FromBody] LocationalAdvtgDTO data)
        {
            return inter.getfilecomment(data);
        }
        [Route("savecomments")]
        public LocationalAdvtgDTO savecomments([FromBody] LocationalAdvtgDTO data)
        {
            return inter.savecomments(data);
        }
        [Route("savefilewisecomments")]
        public LocationalAdvtgDTO savefilewisecomments([FromBody] LocationalAdvtgDTO data)
        {
            return inter.savefilewisecomments(data);
        }
    }
}

