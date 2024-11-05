using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.PettyCash.Interface
{
    public interface PC_Indent_ApprovalInterface
    {
        PC_Indent_ApprovalDTO onloaddata(PC_Indent_ApprovalDTO data);
        PC_Indent_ApprovalDTO onchangedate(PC_Indent_ApprovalDTO data);
        PC_Indent_ApprovalDTO OnChangeInstitution(PC_Indent_ApprovalDTO data);
        PC_Indent_ApprovalDTO getindentdetails(PC_Indent_ApprovalDTO data);
        PC_Indent_ApprovalDTO saverecord(PC_Indent_ApprovalDTO data);
        PC_Indent_ApprovalDTO Viewdata(PC_Indent_ApprovalDTO data);

        // Expenditure
        PC_Indent_ApprovalDTO ExpenditureLoaddata(PC_Indent_ApprovalDTO data);
        PC_Indent_ApprovalDTO OnChangeExpenditureInstitution(PC_Indent_ApprovalDTO data);
        PC_Indent_ApprovalDTO OnChangeExpenditureIndent(PC_Indent_ApprovalDTO data);
        PC_Indent_ApprovalDTO OnChangeExpenditureParticular(PC_Indent_ApprovalDTO data);
        PC_Indent_ApprovalDTO SaveExpenditure(PC_Indent_ApprovalDTO data);
        PC_Indent_ApprovalDTO DeleteExpenditure(PC_Indent_ApprovalDTO data);
    }
}
