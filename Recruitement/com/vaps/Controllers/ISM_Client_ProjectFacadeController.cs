using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.Sales;
using Recruitment.com.vaps.Interfaces;

namespace Recruitment.com.vaps.Controllers
{
    [Produces("application/json")]
    [Route("api/ISM_Client_ProjectFacade")]
    public class ISM_Client_ProjectFacadeController : Controller
    {
        public ISM_Client_ProjectInterface _CPI;

        public ISM_Client_ProjectFacadeController(ISM_Client_ProjectInterface cpi)
        {
            _CPI = cpi;
        }
        [Route("getdate_cmc")]
        public ISM_Client_Project_DTO getdate_cmc([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.getdate_cmc(dto);
        }
        [Route("details_cmc")]
        public ISM_Client_Project_DTO details_cmc([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.details_cmc(dto);
        }
        [Route("SaveEdit_cmc")]
        public ISM_Client_Project_DTO SaveEdit_cmc([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.SaveEdit_cmc(dto);
        }
        [Route("deactivate_cmc")]
        public ISM_Client_Project_DTO deactivate_cmc([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.deactivate_cmc(dto);
        }

        //======================== BOM
        [Route("getdata_BOM")]
        public ISM_Client_Project_DTO getdata_BOM([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.getdata_BOM(dto);
        }

        [Route("details_BOM")]
        public ISM_Client_Project_DTO details_BOM([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.details_BOM(dto);
        }
        [Route("SaveEdit_BOM")]
        public ISM_Client_Project_DTO SaveEdit_BOM([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.SaveEdit_BOM(dto);
        }
        [Route("deactivate_BOM")]
        public ISM_Client_Project_DTO deactivate_BOM([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.deactivate_BOM(dto);
        }

        
        //======================== Man power
        [Route("getdata_MP")]
        public ISM_Client_Project_DTO getdata_MP([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.getdata_MP(dto);
        }

        [Route("details_MP")]
        public ISM_Client_Project_DTO details_MP([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.details_MP(dto);
        }
        [Route("SaveEdit_MP")]
        public ISM_Client_Project_DTO SaveEdit_MP([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.SaveEdit_MP(dto);
        }
        [Route("deactivate_MP")]
        public ISM_Client_Project_DTO deactivate_MP([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.deactivate_MP(dto);
        }
        //======================== DOC
        [Route("getdata_DOC")]
        public ISM_Client_Project_DTO getdata_DOC([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.getdata_DOC(dto);
        }

        [Route("details_DOC")]
        public ISM_Client_Project_DTO details_DOC([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.details_DOC(dto);
        }
        [Route("SaveEdit_DOC")]
        public ISM_Client_Project_DTO SaveEdit_DOC([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.SaveEdit_DOC(dto);
        }
        [Route("deactivate_DOC")]
        public ISM_Client_Project_DTO deactivate_DOC([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.deactivate_DOC(dto);
        }
         //========================MASTER DOC
        [Route("getdata_MDOC")]
        public ISM_Client_Project_DTO getdata_MDOC([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.getdata_MDOC(dto);
        }

        [Route("details_MDOC")]
        public ISM_Client_Project_DTO details_MDOC([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.details_MDOC(dto);
        }
        [Route("SaveEdit_MDOC")]
        public ISM_Client_Project_DTO SaveEdit_MDOC([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.SaveEdit_MDOC(dto);
        }
        [Route("deactivate_MDOC")]
        public ISM_Client_Project_DTO deactivate_MDOC([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.deactivate_MDOC(dto);
        }


    }
}