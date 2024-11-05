using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
   public interface SmartCardFreezeInterface
    {
        SmartCardFreezeDTO getdetails(SmartCardFreezeDTO data);
        SmartCardFreezeDTO getstddetails(SmartCardFreezeDTO data);
        SmartCardFreezeDTO getdetailsstf(SmartCardFreezeDTO data);
        SmartCardFreezeDTO getdetailsstfdes(SmartCardFreezeDTO data);
        SmartCardFreezeDTO depchange(SmartCardFreezeDTO data);
        SmartCardFreezeDTO getstfdetails(SmartCardFreezeDTO data);
        SmartCardFreezeDTO getdetailsCLG(SmartCardFreezeDTO data);
        SmartCardFreezeDTO getstddetailscld(SmartCardFreezeDTO data);
        SmartCardFreezeDTO admsearch(SmartCardFreezeDTO data);
        SmartCardFreezeDTO admsearchclg(SmartCardFreezeDTO data);
       

    }
}
