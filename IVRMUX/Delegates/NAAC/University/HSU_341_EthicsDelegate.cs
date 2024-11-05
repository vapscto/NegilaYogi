using CommonLibrary;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.University
{
    public class HSU_341_EthicsDelegate
    {
        CommonDelegate<HSU_341_EthicsDTO, HSU_341_EthicsDTO> comm = new CommonDelegate<HSU_341_EthicsDTO, HSU_341_EthicsDTO>();
        public HSU_341_EthicsDTO loaddata(HSU_341_EthicsDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_341_EthicsFacade/getdata");
        }
        public HSU_341_EthicsDTO savedata(HSU_341_EthicsDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_341_EthicsFacade/savedata");
        }
        public HSU_341_EthicsDTO deactive(HSU_341_EthicsDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_341_EthicsFacade/deactive");
        }
        public HSU_341_EthicsDTO editdata(HSU_341_EthicsDTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_341_EthicsFacade/editdata");
        }
    }
}
