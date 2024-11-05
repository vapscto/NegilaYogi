using CommonLibrary;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Fees
{
    public class HlMasterRoom_FeeGroupDelegate
    {
        CommonDelegate<HlMasterRoom_FeeGroupDTO, HlMasterRoom_FeeGroupDTO> comm = new CommonDelegate<HlMasterRoom_FeeGroupDTO, HlMasterRoom_FeeGroupDTO>();
        public HlMasterRoom_FeeGroupDTO save(HlMasterRoom_FeeGroupDTO data)
        {
            return comm.POSTDatafee(data, "HlMasterRoom_FeeGroupFacade/save");
        }
        public HlMasterRoom_FeeGroupDTO loaddata(HlMasterRoom_FeeGroupDTO data)
        {
            return comm.POSTDatafee(data, "HlMasterRoom_FeeGroupFacade/loaddata/");
        }
        public HlMasterRoom_FeeGroupDTO edittab1(HlMasterRoom_FeeGroupDTO data)
        {
            return comm.POSTDatafee(data, "HlMasterRoom_FeeGroupFacade/edittab1");
        }
        public HlMasterRoom_FeeGroupDTO deactive(HlMasterRoom_FeeGroupDTO data)
        {
            return comm.POSTDatafee(data, "HlMasterRoom_FeeGroupFacade/deactive");
        }
    }
}
