using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.PettyCash
{
    public class PC_IndentDelegate
    {
        CommonDelegate<PC_IndentDTO, PC_IndentDTO> _comm = new CommonDelegate<PC_IndentDTO, PC_IndentDTO>();

        public PC_IndentDTO onloaddata(PC_IndentDTO data)
        {
            return _comm.POSTVMS(data, "PC_IndentFacade/onloaddata");
        }
        public PC_IndentDTO onchangedate(PC_IndentDTO data)
        {
            return _comm.POSTVMS(data, "PC_IndentFacade/onchangedate");
        }
        public PC_IndentDTO OnChangeInstitution(PC_IndentDTO data)
        {
            return _comm.POSTVMS(data, "PC_IndentFacade/OnChangeInstitution");
        }
        public PC_IndentDTO getrequisitiondetails(PC_IndentDTO data)
        {
            return _comm.POSTVMS(data, "PC_IndentFacade/getrequisitiondetails");
        }
        public PC_IndentDTO saverecord(PC_IndentDTO data)
        {
            return _comm.POSTVMS(data, "PC_IndentFacade/saverecord");
        }
        public PC_IndentDTO EditData(PC_IndentDTO data)
        {
            return _comm.POSTVMS(data, "PC_IndentFacade/EditData");
        }
        public PC_IndentDTO deactiveY(PC_IndentDTO data)
        {
            return _comm.POSTVMS(data, "PC_IndentFacade/deactiveY");
        }
        public PC_IndentDTO Viewdata(PC_IndentDTO data)
        {
            return _comm.POSTVMS(data, "PC_IndentFacade/Viewdata");
        }
        public PC_IndentDTO deactiveparticulars(PC_IndentDTO data)
        {
            return _comm.POSTVMS(data, "PC_IndentFacade/deactiveparticulars");
        }
    }
}
