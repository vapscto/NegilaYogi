using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.PettyCash
{
    public class PC_Indent_ApprovalDelegate
    {
        CommonDelegate<PC_Indent_ApprovalDTO, PC_Indent_ApprovalDTO> _comm = new CommonDelegate<PC_Indent_ApprovalDTO, PC_Indent_ApprovalDTO>();

        public PC_Indent_ApprovalDTO onloaddata(PC_Indent_ApprovalDTO data)
        {
            return _comm.POSTVMS(data, "PC_Indent_ApprovalFacade/onloaddata");
        }
        public PC_Indent_ApprovalDTO OnChangeInstitution(PC_Indent_ApprovalDTO data)
        {
            return _comm.POSTVMS(data, "PC_Indent_ApprovalFacade/OnChangeInstitution");
        }
        public PC_Indent_ApprovalDTO onchangedate(PC_Indent_ApprovalDTO data)
        {
            return _comm.POSTVMS(data, "PC_Indent_ApprovalFacade/onchangedate");
        }
        public PC_Indent_ApprovalDTO getindentdetails(PC_Indent_ApprovalDTO data)
        {
            return _comm.POSTVMS(data, "PC_Indent_ApprovalFacade/getindentdetails");
        }
        public PC_Indent_ApprovalDTO saverecord(PC_Indent_ApprovalDTO data)
        {
            return _comm.POSTVMS(data, "PC_Indent_ApprovalFacade/saverecord");
        }
        public PC_Indent_ApprovalDTO Viewdata(PC_Indent_ApprovalDTO data)
        {
            return _comm.POSTVMS(data, "PC_Indent_ApprovalFacade/Viewdata");
        }

        // Expenditure
        public PC_Indent_ApprovalDTO ExpenditureLoaddata(PC_Indent_ApprovalDTO data)
        {
            return _comm.POSTVMS(data, "PC_Indent_ApprovalFacade/ExpenditureLoaddata");
        }
        public PC_Indent_ApprovalDTO OnChangeExpenditureInstitution(PC_Indent_ApprovalDTO data)
        {
            return _comm.POSTVMS(data, "PC_Indent_ApprovalFacade/OnChangeExpenditureInstitution");
        }
        public PC_Indent_ApprovalDTO OnChangeExpenditureIndent(PC_Indent_ApprovalDTO data)
        {
            return _comm.POSTVMS(data, "PC_Indent_ApprovalFacade/OnChangeExpenditureIndent");
        }
        public PC_Indent_ApprovalDTO OnChangeExpenditureParticular(PC_Indent_ApprovalDTO data)
        {
            return _comm.POSTVMS(data, "PC_Indent_ApprovalFacade/OnChangeExpenditureParticular");
        }
        public PC_Indent_ApprovalDTO SaveExpenditure(PC_Indent_ApprovalDTO data)
        {
            return _comm.POSTVMS(data, "PC_Indent_ApprovalFacade/SaveExpenditure");
        }
        public PC_Indent_ApprovalDTO DeleteExpenditure(PC_Indent_ApprovalDTO data)
        {
            return _comm.POSTVMS(data, "PC_Indent_ApprovalFacade/DeleteExpenditure");
        }
    }
}
