using PreadmissionDTOs.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.HRMS.Interface
{
    public interface NAACACCommitteememberInterface
    {
        NAACACCommitteeMembersDTO getBasicData(NAACACCommitteeMembersDTO dto);
        NAACACCommitteeMembersDTO SaveUpdate(NAACACCommitteeMembersDTO dto);
        NAACACCommitteeMembersDTO editData(int id);
        NAACACCommitteeMembersDTO deactivate(NAACACCommitteeMembersDTO dto);
    }
}
