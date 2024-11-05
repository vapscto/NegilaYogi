using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.Sales;

namespace IVRMUX.Controllers.com.vapstech.VMS
{

    [Produces("application/json")]
    [Route("api/ISM_Client_Project")]
    public class ISM_Client_ProjectController : Controller
    {
        ISM_Client_ProjectDelegate ICPD = new ISM_Client_ProjectDelegate();

        [HttpGet]
        [Route("getdate_cmc/{id:int}")]
        public ISM_Client_Project_DTO getdate_cmc(int id)
        {
            ISM_Client_Project_DTO dto = new ISM_Client_Project_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ICPD.getdate_cmc(dto);
        }
        [Route("details_cmc/{id:int}")]
        public ISM_Client_Project_DTO details_cmc(int id)
        {
            ISM_Client_Project_DTO dto = new ISM_Client_Project_DTO();
            dto.ISMCLTC_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ICPD.details_cmc(dto);
        }
         [Route("SaveEdit_cmc")]
        public ISM_Client_Project_DTO SaveEdit_cmc([FromBody] ISM_Client_Project_DTO dto )
        {
           
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.SaveEdit_cmc(dto);
        }
         [Route("deactivate_cmc")]
        public ISM_Client_Project_DTO deactivate_cmc([FromBody] ISM_Client_Project_DTO dto )
        {
           
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.deactivate_cmc(dto);
        }

        //=============BOM

        [Route("getdata_BOM/{id:int}")]
        public ISM_Client_Project_DTO getdata_BOM(int id)
        {
            ISM_Client_Project_DTO dto = new ISM_Client_Project_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.getdata_BOM(dto);
        }
        [Route("details_BOM/{id:int}")]
        public ISM_Client_Project_DTO details_BOM(int id)
        {
            ISM_Client_Project_DTO dto = new ISM_Client_Project_DTO();
            dto.ISMCLTPRBOM_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ICPD.details_BOM(dto);
        }
        [Route("SaveEdit_BOM")]
        public ISM_Client_Project_DTO SaveEdit_BOM([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.SaveEdit_BOM(dto);
        }
        [Route("deactivate_BOM")]
        public ISM_Client_Project_DTO deactivate_BOM([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.deactivate_BOM(dto);
        }

         //=============man power

        [Route("getdata_MP/{id:int}")]
        public ISM_Client_Project_DTO getdata_MP(int id)
        {
            ISM_Client_Project_DTO dto = new ISM_Client_Project_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.getdata_MP(dto);
        }
        [Route("details_MP/{id:int}")]
        public ISM_Client_Project_DTO details_MP(int id)
        {
            ISM_Client_Project_DTO dto = new ISM_Client_Project_DTO();
            dto.ISMCLTPRMP_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ICPD.details_MP(dto);
        }
        [Route("SaveEdit_MP")]
        public ISM_Client_Project_DTO SaveEdit_MP([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.SaveEdit_MP(dto);
        }
        [Route("deactivate_MP")]
        public ISM_Client_Project_DTO deactivate_MP([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.deactivate_MP(dto);
        }

        //=============DOC

        [Route("getdata_DOC/{id:int}")]
        public ISM_Client_Project_DTO getdata_DOC(int id)
        {
            ISM_Client_Project_DTO dto = new ISM_Client_Project_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.getdata_DOC(dto);
        }
        [Route("details_DOC/{id:int}")]
        public ISM_Client_Project_DTO details_DOC(int id)
        {
            ISM_Client_Project_DTO dto = new ISM_Client_Project_DTO();
            dto.ISMCLTPRDOC_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ICPD.details_DOC(dto);
        }
        [Route("SaveEdit_DOC")]
        public ISM_Client_Project_DTO SaveEdit_DOC([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.SaveEdit_DOC(dto);
        }
        [Route("deactivate_DOC")]
        public ISM_Client_Project_DTO deactivate_DOC([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.deactivate_DOC(dto);
        }

        //=============master DOC

        [Route("getdata_MDOC/{id:int}")]
        public ISM_Client_Project_DTO getdata_MDOC(int id)
        {
            ISM_Client_Project_DTO dto = new ISM_Client_Project_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.getdata_MDOC(dto);
        }
        [Route("details_MDOC/{id:int}")]
        public ISM_Client_Project_DTO details_MDOC(int id)
        {
            ISM_Client_Project_DTO dto = new ISM_Client_Project_DTO();
            dto.ISMCLTPRMDOC_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ICPD.details_MDOC(dto);
        }
        [Route("SaveEdit_MDOC")]
        public ISM_Client_Project_DTO SaveEdit_MDOC([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.SaveEdit_MDOC(dto);
        }
        [Route("deactivate_MDOC")]
        public ISM_Client_Project_DTO deactivate_MDOC([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.deactivate_MDOC(dto);
        }



    }
}