using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_ProductReportDelegate
    {

        CommonDelegate<INV_Master_ProductDTO, INV_Master_ProductDTO> COMINV = new CommonDelegate<INV_Master_ProductDTO, INV_Master_ProductDTO>();

        public INV_Master_ProductDTO getalldetails(INV_Master_ProductDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_ProductReportFacade/getalldetails/");
        }
        public INV_Master_ProductDTO getdata(INV_Master_ProductDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_ProductReportFacade/getdata/");
        }
        
        public INV_Master_ProductDTO radiobtndata(INV_Master_ProductDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_ProductReportFacade/radiobtndata/");
        }

    }
}
