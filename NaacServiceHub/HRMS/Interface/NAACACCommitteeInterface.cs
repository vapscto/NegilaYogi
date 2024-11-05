using PreadmissionDTOs.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.HRMS.Interface
{
    public interface NAACACCommitteeInterface
    {
        NAACACCommitteeDTO getBasicData(NAACACCommitteeDTO dto);
        NAACACCommitteeDTO SaveUpdate(NAACACCommitteeDTO dto);
        NAACACCommitteeDTO editData(int id);
        NAACACCommitteeDTO deactivate(NAACACCommitteeDTO dto);
    }
}
