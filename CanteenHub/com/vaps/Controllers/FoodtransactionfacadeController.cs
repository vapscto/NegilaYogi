using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CanteenHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Canteen;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CanteenHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class FoodtransactionfacadeController : Controller
    {
        public FoodtransactionInterface _fmt;

        public FoodtransactionfacadeController(FoodtransactionInterface fmtdept)
        {
            _fmt = fmtdept;
        }

        [Route("loaddata")]
        public FoodtransactionDTO loaddata([FromBody]FoodtransactionDTO data)
        {
            return _fmt.loaddata(data);
        }

        [HttpPost]
        [Route("FoodItem")]
        public FoodtransactionDTO FoodItem([FromBody] FoodtransactionDTO data)
        {
            return _fmt.FoodItem(data);
        }

        [HttpPost]
        [Route("FoodItemtax")]
        public FoodtransactionDTO FoodItemtax([FromBody] FoodtransactionDTO data)
        {
            return _fmt.FoodItemtax(data);
        }

        [HttpPost]
        [Route("savedata")]
        public FoodtransactionDTO savedata([FromBody] FoodtransactionDTO data)
        {
            return _fmt.savedata(data);
        }
        [HttpPost]
        [Route("paymenthistory")]
        public FoodtransactionDTO paymenthistory([FromBody] FoodtransactionDTO data)
        {
            return _fmt.paymenthistory(data);
        }
        [HttpPost]
        [Route("deactivate")]
        public FoodtransactionDTO deactivate([FromBody] FoodtransactionDTO data)
        {

            return _fmt.deactivate(data);
        }


        [HttpPost]
        [Route("getstudent")]
        public FoodtransactionDTO getstudent([FromBody] FoodtransactionDTO data)
        {

            return _fmt.getstudent(data);
        }

        [HttpPost]
        [Route("savesmartData")]
        public FoodtransactionDTO savesmartData([FromBody] FoodtransactionDTO data)
        {

            return _fmt.savesmartData(data);
        }

        [HttpPost]
        [Route("trns_cancel")]
        public FoodtransactionDTO trns_cancel([FromBody] FoodtransactionDTO data)
        {

            return _fmt.trns_cancel(data);
        }


        [Route("orderdeatils")]
        public FoodtransactionDTO orderdeatils([FromBody]FoodtransactionDTO data)
        {
            return _fmt.orderdeatils(data);
        }

        [Route("foodreport")]
        public FoodtransactionDTO foodreport([FromBody]FoodtransactionDTO data)
        {
            return _fmt.foodreport(data);
        }
        [Route("Month_Daywise_graph")]
        public FoodtransactionDTO Month_Daywise_graph([FromBody]FoodtransactionDTO data)
        {
            return _fmt.Month_Daywise_graph(data);
        }

        [Route("YearWise_graph")]
        public FoodtransactionDTO YearWise_graph([FromBody]FoodtransactionDTO data)
        {
            return _fmt.YearWise_graph(data);
        }

        [Route("paymenthistory_print")]
        public FoodtransactionDTO paymenthistory_print([FromBody]FoodtransactionDTO data)
        {
            return _fmt.paymenthistory_print(data);
        }

        [Route("paymenthistory_print_onetime")]
        public FoodtransactionDTO paymenthistory_print_onetime([FromBody]FoodtransactionDTO data)
        {
            return _fmt.paymenthistory_print_onetime(data);
        }


    }
}
