using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using DomainModel.Model;
using DataAccessMsSqlServerProvider;
using MailKit.Net.Smtp;
using MimeKit;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using PortalHub.com.vaps.Student.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Student;
using PreadmissionDTOs.com.vaps.Portals.IVRM;
using PortalHub.com.vaps.IVRM.Interfaces;

namespace PortalHub.com.vaps.IVRM.Controllers
{
    [Route("api/[controller]")]
    public class IVRM_GalleryFacade : Controller
    {
        public IVRM_GalleryInterface _ads;

        public IVRM_GalleryFacade(IVRM_GalleryInterface adstu)
        {
            _ads = adstu;
        }

        [HttpPost]
        [Route("getloaddata")]
        public IVRM_GalleryDTO getloaddata([FromBody]IVRM_GalleryDTO data)
        {
            return _ads.getloaddata(data);
        }
        [Route("get_section")]
        public IVRM_GalleryDTO get_section([FromBody]IVRM_GalleryDTO data)
        {
            return _ads.get_section(data);
        }
        [Route("savedata")]
        public IVRM_GalleryDTO savedata([FromBody]IVRM_GalleryDTO data)
        {
            return _ads.savedata(data);
        }
        [Route("getcovermodel")]
        public IVRM_GalleryDTO getcovermodel([FromBody]IVRM_GalleryDTO data)
        {
            return _ads.getcovermodel(data);
        }
        [Route("savecover")]
        public IVRM_GalleryDTO savecover([FromBody]IVRM_GalleryDTO data)
        {
            return _ads.savecover(data);
        }
        [Route("deactive")]
        public IVRM_GalleryDTO deactive([FromBody]IVRM_GalleryDTO data)
        {
            return _ads.deactive(data);
        }

        //edit

        [Route("Editdetails")]
        public IVRM_GalleryDTO Editdetails([FromBody]IVRM_GalleryDTO data)
        {
            return _ads.Editdetails(data);
        }

        [Route("kioskvideo")]
        public IVRM_GalleryDTO kioskvideo([FromBody]IVRM_GalleryDTO data)
        {
            return _ads.kioskvideo(data);
        }


    }
}
