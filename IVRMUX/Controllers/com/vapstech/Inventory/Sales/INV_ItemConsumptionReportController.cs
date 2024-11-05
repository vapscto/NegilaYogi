﻿using IVRMUX.Delegates.com.vapstech.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Controllers.com.vapstech.Inventory
{
    [Route("api/[controller]")]
    public class INV_ItemConsumptionReportController : Controller
    {
        INV_ItemConsumptionReportDelegate _delegate = new INV_ItemConsumptionReportDelegate();


        [Route("getloaddata")]
        public INV_ItemConsumptionDTO getloaddata([FromBody] INV_ItemConsumptionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("onreport")]
        public INV_ItemConsumptionDTO onreport([FromBody] INV_ItemConsumptionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.onreport(data);
        }



    }
}