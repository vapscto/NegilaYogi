using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using InventoryServicesHub.com.vaps.Interface;
using InventoryServicesHub.com.vaps.Purchase.Interface;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryServicesHub.com.vaps.Purchase.Controllers
{
    [Route("api/[controller]")]
    public class INV_T_GRNFacadeController : Controller
    {
        // GET: api/values
        INV_T_GRNInterface _Inv;
        public INV_T_GRNFacadeController(INV_T_GRNInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public INV_T_GRNDTO getloaddata([FromBody] INV_T_GRNDTO data)
        {
            return _Inv.getloaddata(data);
        }

        [Route("getitemDetail")]
        public INV_T_GRNDTO getitemDetail([FromBody] INV_T_GRNDTO data)
        {
            return _Inv.getitemDetail(data);
        }
        [Route("savedetails")]
        public INV_T_GRNDTO savedetails([FromBody] INV_T_GRNDTO data)
        {
            return _Inv.savedetails(data);
        }
        [Route("get_GRNitemDetails")]
        public INV_T_GRNDTO get_GRNitemDetails([FromBody] INV_T_GRNDTO data)
        {
            return _Inv.get_GRNitemDetails(data);
        }
        [Route("get_itemtax")]
        public INV_T_GRNDTO get_itemtax([FromBody] INV_T_GRNDTO data)
        {
            return _Inv.get_itemtax(data);
        }
        [Route("deactiveg")]
        public INV_T_GRNDTO deactiveg([FromBody] INV_T_GRNDTO data)
        {
            return _Inv.deactiveg(data);
        }
        [Route("deactivet")]
        public INV_T_GRNDTO deactivet([FromBody] INV_T_GRNDTO data)
        {
            return _Inv.deactivet(data);
        }
        [Route("deactive")]
        public INV_T_GRNDTO deactive([FromBody] INV_T_GRNDTO data)
        {
            return _Inv.deactive(data);
        }
        [Route("Edit_GRN_details")]
        public INV_T_GRNDTO Edit_GRN_details([FromBody] INV_T_GRNDTO data)
        {
            return _Inv.Edit_GRN_details(data);
        }
        //[Route("SearchByColumn")]
        //public INV_T_GRNDTO SearchByColumn([FromBody] INV_T_GRNDTO data)
        //{
        //    return _Inv.SearchByColumn(data);
        //}


        






    }
}
