using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using WebApplication1.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class DocumentViewFacadeController : Controller
    {
        public DocumentViewInterface _M_doc;
        public DocumentViewFacadeController(DocumentViewInterface _doc)
        {
            _M_doc = _doc;
        }
        [Route("getdetails")]
        public DocumentViewDTO getInitialData([FromBody]DocumentViewDTO miid)
        {
            return _M_doc.getInitailData(miid);
        }
        [Route("getDpData")]
        public DocumentViewDTO getDpData([FromBody]DocumentViewDTO miid)
        {
            return _M_doc.getDpData(miid);
        }
        [Route("getdocksonly")]
        public DocumentViewDTO getdocksonly([FromBody]DocumentViewDTO miid)
        {
            return _M_doc.getdocksonly(miid);
        }

        [Route("StatusGetdetails")]
        public DocumentViewDTO StatusGetdetails([FromBody]DocumentViewDTO masterDTO)//int IVRMM_Id
        {

            return _M_doc.StatusGetdetails(masterDTO);

        }

        [HttpPost]
        public DocumentViewDTO Post([FromBody] DocumentViewDTO masterMDT)
        {

            return _M_doc.mastersaveDTO(masterMDT);
        }


        [Route("GetSelectedRowDetails/{id:int}")]
        public DocumentViewDTO GetSelectedRowDetails(int ID)
        {

            return _M_doc.GetSelectedRowDetails(ID);
        }


        [HttpGet]
        [Route("MasterDeleteModulesDATA/{id:int}")]
        public DocumentViewDTO MasterDeleteModulesDATA(int ID)
        {

            return _M_doc.MasterDeleteModulesData(ID);
        }

    }
}
