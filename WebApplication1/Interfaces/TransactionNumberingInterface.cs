using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
    public interface TransactionNumberingInterface
    {
        Master_NumberingDTO saveMaster_Numbering(Master_NumberingDTO enqu);
        Master_NumberingDTO getdetails(MandatoryFieldsDTO id);
        Master_NumberingDTO deleteRollnoconfig(Master_NumberingDTO id);
        
    }
}
