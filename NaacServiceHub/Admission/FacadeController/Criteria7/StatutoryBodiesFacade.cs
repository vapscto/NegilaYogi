﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Admission.Interface;
using NaacServiceHub.Admission.Interface.Criteria7;
using PreadmissionDTOs.NAAC.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Admission.FacadeController.Criteria7
{
    [Route("api/[controller]")]
    public class StatutoryBodiesFacade : Controller
    {
        public StatutoryBodiesInterface _Iobj;
        public StatutoryBodiesFacade(StatutoryBodiesInterface para)
        {
            _Iobj = para;
        }

        [Route("loaddata")]
        public Task<NAAC_AC_7116_StatutoryBodies_DTO> loaddata([FromBody] NAAC_AC_7116_StatutoryBodies_DTO data)
        {
            return _Iobj.loaddata(data);
        }

        [Route("savedatatab1")]
        public NAAC_AC_7116_StatutoryBodies_DTO savedatatab1([FromBody] NAAC_AC_7116_StatutoryBodies_DTO data)
        {
            return _Iobj.savedatatab1(data);
        }
        [Route("editTab1")]
        public NAAC_AC_7116_StatutoryBodies_DTO editTab1([FromBody] NAAC_AC_7116_StatutoryBodies_DTO data)
        {
            return _Iobj.editTab1(data);
        }
        [Route("getfilecomment")]
        public NAAC_AC_7116_StatutoryBodies_DTO getfilecomment([FromBody] NAAC_AC_7116_StatutoryBodies_DTO data)
        {
            return _Iobj.getfilecomment(data);
        }
        [Route("savefilewisecomments")]
        public NAAC_AC_7116_StatutoryBodies_DTO savefilewisecomments([FromBody] NAAC_AC_7116_StatutoryBodies_DTO data)
        {
            return _Iobj.savefilewisecomments(data);
        }
        [Route("savemedicaldatawisecomments")]
        public NAAC_AC_7116_StatutoryBodies_DTO savemedicaldatawisecomments([FromBody] NAAC_AC_7116_StatutoryBodies_DTO data)
        {
            return _Iobj.savemedicaldatawisecomments(data);
        }
        [Route("getcomment")]
        public NAAC_AC_7116_StatutoryBodies_DTO getcomment([FromBody] NAAC_AC_7116_StatutoryBodies_DTO data)
        {
            return _Iobj.getcomment(data);
        }

        [Route("deactivYTab1")]
        public NAAC_AC_7116_StatutoryBodies_DTO deactivYTab1([FromBody] NAAC_AC_7116_StatutoryBodies_DTO data)
        {
            return _Iobj.deactivYTab1(data);
        }

        [Route("deleteuploadfile")]
        public NAAC_AC_7116_StatutoryBodies_DTO deleteuploadfile([FromBody] NAAC_AC_7116_StatutoryBodies_DTO data)
        {
            return _Iobj.deleteuploadfile(data);
        }

        [Route("getData")]
        public NAAC_AC_7116_StatutoryBodies_DTO getData([FromBody] NAAC_AC_7116_StatutoryBodies_DTO data)
        {
            return _Iobj.getData(data);
        }
    }
}
