using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using WebApplication1.Interfaces;
using DomainModel.Model;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class MasterClassCategoryFacade : Controller
    {
        // GET: api/values
        MasterClassCategoryInterface _interface;
        public MasterClassCategoryFacade(MasterClassCategoryInterface inter)
        {
            _interface = inter;
        }

        // GET api/values/5
        
        [Route("getInitialdata")]
        public MasterClassCategoryDTO getData([FromBody]MasterClassCategoryDTO data)
        {
            return _interface.getDat(data);
        }

       // POST api/values
       [HttpPost]
        public MasterClassCategoryDTO save([FromBody]MasterClassCategoryDTO data)
        {
            return _interface.saveData(data);
        }
        [Route("getdetails")]
        public MasterClassCategoryDTO EditRecord([FromBody]MasterClassCategoryDTO ids)
        {
            return _interface.Edit(ids);
        }
        
        [Route("deletedetails/{id:int}")]
        public MasterClassCategoryDTO deletedetails(int id)
        {
            return _interface.deleterec(id);
        }
        [Route("deactivate")]
        public MasterClassCategoryDTO deactivate([FromBody]MasterClassCategoryDTO dto)
        {
            return _interface.deactivate(dto);
        }
        [Route("searchByColumn")]
        public MasterClassCategoryDTO SearchByColumn([FromBody]MasterClassCategoryDTO dto)
        {
            return _interface.searchByColumn(dto);
        }
        [Route("viewrecordspopup")]
        public MasterClassCategoryDTO viewrecordspopup([FromBody]MasterClassCategoryDTO dto)
        {
            return _interface.viewrecordspopup(dto);
        }
        [Route("deactivesection")]
        public MasterClassCategoryDTO deactivesection([FromBody]MasterClassCategoryDTO dto)
        {
            return _interface.deactivesection(dto);
        }
    }
}
