using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class SiblingEmployeeMappingFacade : Controller
    {
        public SiblingEmployeeMappingInterface _org;

        public SiblingEmployeeMappingFacade(SiblingEmployeeMappingInterface orga)
        {
            _org = orga;
        }
        [Route("getalldetails")]
        public Adm_M_Sibling formload([FromBody] Adm_M_Sibling data)
        {
            return _org.initialdata(data);
        }

        [Route("selectacademic")]
        public Adm_M_Sibling selaca([FromBody] Adm_M_Sibling data)
        {
            return _org.selectacade(data);
        }

        [Route("onstudentnamechange")]
        public Adm_M_Sibling onstudentnamechange([FromBody] Adm_M_Sibling data)
        {
            return _org.onstudentnamechange(data);
        }
        [Route("onstudentnamechangerte")]
        public Adm_M_Sibling onstudentnamechangerte([FromBody] Adm_M_Sibling data)
        {
            return _org.onstudentnamechangerte(data);
        }
        [Route("onselectstaff")]
        public Adm_M_Sibling onselectstaff([FromBody] Adm_M_Sibling data)
        {
            return _org.onselectstaff(data);
        }

        [Route("savedata")]
        public Adm_M_Sibling svedata([FromBody] Adm_M_Sibling data)
        {
            return _org.sved(data);
        }

        [Route("Deletedetails")]
        public Adm_M_Sibling deldta([FromBody] Adm_M_Sibling data)
        {
            return _org.deletedta(data);
        }
        [Route("DeletRecordemployee")]
        public Adm_M_Sibling DeletRecordemployee([FromBody] Adm_M_Sibling data)
        {
            return _org.DeletRecordemployee(data);
        }

        [Route("viewsiblingdetails")]
        public Adm_M_Sibling viewsiblingdetails([FromBody] Adm_M_Sibling data)
        {
            return _org.viewsiblingdetails(data);
        }
        [Route("viewsiblingdetailsemployee")]
        public Adm_M_Sibling viewsiblingdetailsemployee([FromBody] Adm_M_Sibling data)
        {
            return _org.viewsiblingdetailsemployee(data);
        }
        [Route("checkfeegroup")]
        public Adm_M_Sibling checkfeegroup([FromBody] Adm_M_Sibling data)
        {
            return _org.checkfeegroup(data);
        }
        
    }
}
