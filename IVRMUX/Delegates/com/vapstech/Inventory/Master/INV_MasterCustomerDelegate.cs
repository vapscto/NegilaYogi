using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_MasterCustomerDelegate
    {
        CommonDelegate<INV_Master_CustomerDTO, INV_Master_CustomerDTO> COMINV = new CommonDelegate<INV_Master_CustomerDTO, INV_Master_CustomerDTO>();
        public INV_Master_CustomerDTO getloaddata(INV_Master_CustomerDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterCustomerFacade/getloaddata/");
        }
        public INV_Master_CustomerDTO savedetails(INV_Master_CustomerDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterCustomerFacade/savedetails/");
        }
        public INV_Master_CustomerDTO deactive(INV_Master_CustomerDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterCustomerFacade/deactive/");
        }

    }
}
