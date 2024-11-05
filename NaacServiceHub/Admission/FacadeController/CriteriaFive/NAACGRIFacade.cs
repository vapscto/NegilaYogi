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
    public class NAACGRIFacade : Controller
    {
        public NAACGRIInterface _Interface;

        public NAACGRIFacade(NAACGRIInterface q)
        {
            _Interface = q;
        }

        [Route("loaddata")]
        public NAACGRIDTO loaddata([FromBody] NAACGRIDTO data)
        {
            return _Interface.loaddata(data);
        }
        [Route("loaddatamed")]
        public NAACGRIDTO loaddatamed([FromBody] NAACGRIDTO data)
        {
            return _Interface.loaddatamed(data);
        }
        [Route("save")]
        public NAACGRIDTO save([FromBody] NAACGRIDTO data)
        {
            return _Interface.save(data);
        }

        [Route("deactiveStudent")]
        public NAACGRIDTO deactiveStudent([FromBody] NAACGRIDTO data)
        {
            return _Interface.deactiveStudent(data);
        }

        [Route("EditData")]
        public NAACGRIDTO EditData([FromBody]NAACGRIDTO category)
        {
            return _Interface.EditData(category);
        }
        [Route("viewuploadflies")]
        public NAACGRIDTO viewuploadflies([FromBody]NAACGRIDTO category)
        {
            return _Interface.viewuploadflies(category);
        }
        [Route("deleteuploadfile")]
        public NAACGRIDTO deleteuploadfile([FromBody]NAACGRIDTO category)
        {
            return _Interface.deleteuploadfile(category);
        }
        [Route("savemedicaldatawisecomments")]
        public NAACGRIDTO savemedicaldatawisecomments([FromBody]NAACGRIDTO category)
        {
            return _Interface.savemedicaldatawisecomments(category);
        }
        [Route("getcomment")]
        public NAACGRIDTO getcomment([FromBody]NAACGRIDTO category)
        {
            return _Interface.getcomment(category);
        }
        [Route("getfilecomment")]
        public NAACGRIDTO getfilecomment([FromBody]NAACGRIDTO category)
        {
            return _Interface.getfilecomment(category);
        }
        [Route("savefilewisecomments")]
        public NAACGRIDTO savefilewisecomments([FromBody]NAACGRIDTO category)
        {
            return _Interface.savefilewisecomments(category);
        }
    }
}
