using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory.Master
{
    public class INV_StockSummaryDelegate
    {
        CommonDelegate<INV_StockSummaryDTO, INV_StockSummaryDTO> COMINV = new CommonDelegate<INV_StockSummaryDTO, INV_StockSummaryDTO>();
        public INV_StockSummaryDTO getloaddata(INV_StockSummaryDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_StockSummaryFacade/getloaddata/");
        }
        public INV_StockSummaryDTO onreport(INV_StockSummaryDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_StockSummaryFacade/onreport/");
        }
        public INV_StockSummaryDTO onreporttwo(INV_StockSummaryDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_StockSummaryFacade/onreporttwo/");
        }
        public INV_StockSummaryDTO get_load_onchange(INV_StockSummaryDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_StockSummaryFacade/get_load_onchange/");
        }
        public INV_StockSummaryDTO onreportthree(INV_StockSummaryDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_StockSummaryFacade/onreportthree/");
        }
        public INV_StockSummaryDTO getstudent(INV_StockSummaryDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_StockSummaryFacade/getstudent/");
        }
        //getstudent

        public INV_StockSummaryDTO onreportstock(INV_StockSummaryDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_StockSummaryFacade/onreportstock/");
        }
    }
}
