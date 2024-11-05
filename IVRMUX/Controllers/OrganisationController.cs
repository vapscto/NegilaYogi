using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class OrganisationController : Controller
    {
        OrganisationDelegate od = new OrganisationDelegate();
        // GET: api/values
       
        [Route("getalldetails")]
        public OrganisationDTO Get([FromBody] OrganisationDTO org)
        {
            org.trustPagination.PageSize = 5;
            org.MO_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MO_Id"));
            org.sessionMI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            org.RoleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            org.trustPagination.CurrentPageIndex = 1;
            return od.getcountrydata(org);
        }

        //[Route("getfiltereddetails/{id:int}")]
        //public OrganisationDTO getfiltereddetails(int filtype)
        //{
        //    filtype = 1;
        //    return od.getfilterdet(filtype);
        //}

        [Route("getorganisationcontroller/{id:int}")]
        public StateDTO Getstates(int id)
        {
            return od.enqdatacountrydrp(id);
        }

        [Route("getorganisationstatecontroller/{id:int}")]
        public CountryDTO getcity(int id)
        {
            return od.cityfill(id);
        }

        [Route("getdetails/{id:int}")]
        public OrganisationDTO getdetail(int id)
        {
            //HttpContext.Session.SetString("institutionid", id.ToString()); //Set
            // id = 12;
            return od.orgdet(id);
            //HttpContext.Session.SetString("institutionid","0"); //Set
        }

        [Route("getcurrencydetails/{id:int}")]
        public OrganisationDTO getcurrencydet(int id)
        {
            return od.getcurrency(id);
        }

        // POST api/values
        [HttpPost]
        public OrganisationDTO savedetail([FromBody] OrganisationDTO org)
        {
            //int trustid = 0;
            //if (HttpContext.Session.GetString("institutionid") != null){
            //    trustid = Convert.ToInt32(HttpContext.Session.GetString("institutionid"));//Get
            //}
           
            //org.MO_Id = trustid;
            //HttpContext.Session.Remove("institutionid");
            return od.savedetails(org);
        }

        // PUT api/values/5
        [HttpPost("{id}")]
        public OrganisationDTO Put(int id, [FromBody] OrganisationDTO value)
        {
            return od.getfilterde(id, value);
        }


        [Route("getOrganisationSearchedDetails")]
        public OrganisationDTO SearchedDetails([FromBody] SortingPagingInfoDTO Ins)
        {
            Ins.PageSize = 5;
            return od.getorgSearchedDetails(Ins);
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public OrganisationDTO Delete(int id)
        {
            return od.deleterec(id);
        }
    }
}
