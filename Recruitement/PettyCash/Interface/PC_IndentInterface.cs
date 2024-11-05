using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.PettyCash.Interface
{
    public interface PC_IndentInterface
    {
        PC_IndentDTO onloaddata(PC_IndentDTO data);
        PC_IndentDTO OnChangeInstitution(PC_IndentDTO data);
        PC_IndentDTO onchangedate(PC_IndentDTO data);
        PC_IndentDTO getrequisitiondetails(PC_IndentDTO data);
        PC_IndentDTO saverecord(PC_IndentDTO data);
        PC_IndentDTO EditData(PC_IndentDTO data);
        PC_IndentDTO deactiveY(PC_IndentDTO data);
        PC_IndentDTO Viewdata(PC_IndentDTO data);
        PC_IndentDTO deactiveparticulars(PC_IndentDTO data);
    }
}
