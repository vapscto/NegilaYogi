using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.PettyCash.Interface
{
    public interface PC_Master_ParticularsInterface
    {
        PC_Master_ParticularsDTO onloaddata(PC_Master_ParticularsDTO data);
        PC_Master_ParticularsDTO saverecord(PC_Master_ParticularsDTO data);
        PC_Master_ParticularsDTO deactiveY(PC_Master_ParticularsDTO data);
    }
}
