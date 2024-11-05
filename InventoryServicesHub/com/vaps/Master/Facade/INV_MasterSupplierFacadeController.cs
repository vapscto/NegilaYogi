﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using InventoryServicesHub.com.vaps.Interface;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class INV_MasterSupplierFacadeController : Controller
    {
        // GET: api/values
        INV_MasterSupplierInterface _Inv;
        public INV_MasterSupplierFacadeController(INV_MasterSupplierInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public INV_Master_SupplierDTO getloaddata([FromBody] INV_Master_SupplierDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [HttpPost]
        [Route("savedetails")]
        public INV_Master_SupplierDTO savedetails([FromBody] INV_Master_SupplierDTO data)
        {
            return _Inv.savedetails(data);
        }
       
        [Route("deactive")]
        public INV_Master_SupplierDTO deactive([FromBody] INV_Master_SupplierDTO data)
        {
            return _Inv.deactive(data);
        }
        


    }
}