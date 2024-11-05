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
    public class NAACVETFacade : Controller
    {
        public NAACVETInterface _Interface;

        public NAACVETFacade(NAACVETInterface q)
        {
            _Interface = q;
        }

        [Route("loaddata")]
        public NAACVETDTO loaddata([FromBody] NAACVETDTO data)
        {
            return _Interface.loaddata(data);
        }
        [Route("save")]
        public NAACVETDTO save([FromBody] NAACVETDTO data)
        {
            return _Interface.save(data);
        }

        [Route("deactiveStudent")]
        public NAACVETDTO deactiveStudent([FromBody] NAACVETDTO data)
        {
            return _Interface.deactiveStudent(data);
        }

        [Route("EditData")]
        public NAACVETDTO EditData([FromBody]NAACVETDTO category)
        {
            return _Interface.EditData(category);
        }
        [Route("viewuploadflies")]
        public NAACVETDTO viewuploadflies([FromBody]NAACVETDTO category)
        {
            return _Interface.viewuploadflies(category);
        }
        [Route("deleteuploadfile")]
        public NAACVETDTO deleteuploadfile([FromBody]NAACVETDTO category)
        {
            return _Interface.deleteuploadfile(category);
        }
        [Route("savemedicaldatawisecomments")]
        public NAACVETDTO savemedicaldatawisecomments([FromBody]NAACVETDTO category)
        {
            return _Interface.savemedicaldatawisecomments(category);
        }
        [Route("getcomment")]
        public NAACVETDTO getcomment([FromBody]NAACVETDTO category)
        {
            return _Interface.getcomment(category);
        }
        [Route("getfilecomment")]
        public NAACVETDTO getfilecomment([FromBody]NAACVETDTO category)
        {
            return _Interface.getfilecomment(category);
        }
        [Route("savefilewisecomments")]
        public NAACVETDTO savefilewisecomments([FromBody]NAACVETDTO category)
        {
            return _Interface.savefilewisecomments(category);
        }
    }
}
