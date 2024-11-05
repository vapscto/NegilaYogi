using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Interface
{
   public interface HSU_341_EthicsInterface
    {
        HSU_341_EthicsDTO loaddata(HSU_341_EthicsDTO data);
        HSU_341_EthicsDTO savedata(HSU_341_EthicsDTO data);
        HSU_341_EthicsDTO deactive(HSU_341_EthicsDTO data);
        HSU_341_EthicsDTO editdata(HSU_341_EthicsDTO data);
    }
}
