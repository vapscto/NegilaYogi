using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_ItemReportDelegate
    {
        CommonDelegate<INV_Master_ItemDTO, INV_Master_ItemDTO> COMINV = new CommonDelegate<INV_Master_ItemDTO, INV_Master_ItemDTO>();
        public INV_Master_ItemDTO getloaddata(INV_Master_ItemDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_ItemReportFacade/getloaddata/");
        }
        public INV_Master_ItemDTO onreport(INV_Master_ItemDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_ItemReportFacade/onreport/");
        }

        
    }
}
