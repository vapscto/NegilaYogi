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
using PreadmissionDTOs.com.vaps.Portals.Student;
using PreadmissionDTOs.com.vaps.Portals.IVRM;
using CollegePortals.com.vaps.IVRM.Interfaces;
using PreadmissionDTOs.com.vaps.College.Portals.IVRM;

namespace CollegePortals.com.vaps.IVRM.Controllers
{
    [Route("api/[controller]")]
    public class Clg_IVRM_GalleryFacade : Controller
    {
        public Clg_IVRM_GalleryInterface _ads;

        public Clg_IVRM_GalleryFacade(Clg_IVRM_GalleryInterface adstu)
        {
            _ads = adstu;
        }

        [HttpPost]
        [Route("getloaddata")]
        public ClgIVRMGalleryDTO getloaddata([FromBody]ClgIVRMGalleryDTO data)
        {
            return _ads.getloaddata(data);
        }
        [Route("get_branch")]
        public ClgIVRMGalleryDTO get_branch([FromBody]ClgIVRMGalleryDTO data)
        {
            return _ads.get_branch(data);
        }
        [Route("get_semester")]
        public ClgIVRMGalleryDTO get_semester([FromBody]ClgIVRMGalleryDTO data)
        {
            return _ads.get_semester(data);
        }
        [Route("get_Section")]
        public ClgIVRMGalleryDTO get_Section([FromBody]ClgIVRMGalleryDTO data)
        {
            return _ads.get_Section(data);
        }
        [Route("savedata")]
        public ClgIVRMGalleryDTO savedata([FromBody]ClgIVRMGalleryDTO data)
        {
            return _ads.savedata(data);
        }
        [Route("getcovermodel")]
        public ClgIVRMGalleryDTO getcovermodel([FromBody]ClgIVRMGalleryDTO data)
        {
            return _ads.getcovermodel(data);
        }
        [Route("savecover")]
        public ClgIVRMGalleryDTO savecover([FromBody]ClgIVRMGalleryDTO data)
        {
            return _ads.savecover(data);
        }
        [Route("deactive")]
        public ClgIVRMGalleryDTO deactive([FromBody]ClgIVRMGalleryDTO data)
        {
            return _ads.deactive(data);
        }



    }
}
