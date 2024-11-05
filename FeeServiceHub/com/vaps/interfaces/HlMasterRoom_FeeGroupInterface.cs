using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface HlMasterRoom_FeeGroupInterface
    {
        HlMasterRoom_FeeGroupDTO save(HlMasterRoom_FeeGroupDTO data);
        HlMasterRoom_FeeGroupDTO loaddata(HlMasterRoom_FeeGroupDTO data);
        HlMasterRoom_FeeGroupDTO edittab1(HlMasterRoom_FeeGroupDTO data);
        HlMasterRoom_FeeGroupDTO deactive(HlMasterRoom_FeeGroupDTO data);
    }
}
