﻿using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_PhyStock_UpdationDelegate
    {
        CommonDelegate<INV_PhyStock_UpdationDTO, INV_PhyStock_UpdationDTO> COMINV = new CommonDelegate<INV_PhyStock_UpdationDTO, INV_PhyStock_UpdationDTO>();
        public INV_PhyStock_UpdationDTO getloaddata(INV_PhyStock_UpdationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PhyStock_UpdationFacade/getloaddata/");
        }

      
        public INV_PhyStock_UpdationDTO savedetails(INV_PhyStock_UpdationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PhyStock_UpdationFacade/savedetails/");
        }
        public INV_PhyStock_UpdationDTO deactive(INV_PhyStock_UpdationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PhyStock_UpdationFacade/deactive/");
        }
        public INV_PhyStock_UpdationDTO getobdetails(INV_PhyStock_UpdationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PhyStock_UpdationFacade/getobdetails/");
        }


        
    }
}
