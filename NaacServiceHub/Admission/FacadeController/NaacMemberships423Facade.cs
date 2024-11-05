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
    public class NaacMemberships423Facade : Controller
    {
        public Naac_Memberships_423_Interface _InterFace;

        public NaacMemberships423Facade(Naac_Memberships_423_Interface r)
        {
            _InterFace = r;
        }

        [Route("deactiveStudent")]
        public Naac_Memberships_423_DTO deactiveStudent([FromBody] Naac_Memberships_423_DTO data)
        {
            return _InterFace.deactiveStudent(data);
        }

        [Route("save")]
        public Naac_Memberships_423_DTO save([FromBody] Naac_Memberships_423_DTO data)
        {
            return _InterFace.save(data);
        }

        [Route("getcomment")]
        public Naac_Memberships_423_DTO getcomment([FromBody] Naac_Memberships_423_DTO data)
        {
            return _InterFace.getcomment(data);
        }

        [Route("getfilecomment")]
        public Naac_Memberships_423_DTO getfilecomment([FromBody] Naac_Memberships_423_DTO data)
        {
            return _InterFace.getfilecomment(data);
        }

        [Route("savefilewisecomments")]
        public Naac_Memberships_423_DTO savefilewisecomments([FromBody] Naac_Memberships_423_DTO data)
        {
            return _InterFace.savefilewisecomments(data);
        }

        [Route("savemedicaldatawisecomments")]
        public Naac_Memberships_423_DTO savemedicaldatawisecomments([FromBody] Naac_Memberships_423_DTO data)
        {
            return _InterFace.savemedicaldatawisecomments(data);
        }

        [Route("loaddata")]
        public Naac_Memberships_423_DTO loaddata([FromBody] Naac_Memberships_423_DTO data)
        {
            return _InterFace.loaddata(data);
        }

        [Route("EditData")]
        public Naac_Memberships_423_DTO EditData([FromBody] Naac_Memberships_423_DTO data)
        {
            return _InterFace.EditData(data);
        }

        [Route("viewuploadflies")]
        public Naac_Memberships_423_DTO viewuploadflies([FromBody] Naac_Memberships_423_DTO data)
        {
            return _InterFace.viewuploadflies(data);
        }

        [Route("deleteuploadfile")]
        public Naac_Memberships_423_DTO deleteuploadfile([FromBody] Naac_Memberships_423_DTO data)
        {
            return _InterFace.deleteuploadfile(data);
        }
    }
}
