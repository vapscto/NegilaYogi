using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.PettyCash
{
    public class PC_RequisitionDelegate
    {
        CommonDelegate<PC_RequisitionDTO, PC_RequisitionDTO> _comm = new CommonDelegate<PC_RequisitionDTO, PC_RequisitionDTO>();

        public PC_RequisitionDTO onloaddata(PC_RequisitionDTO data)
        {
            return _comm.POSTVMS(data, "PC_RequisitionFacade/onloaddata");
        }
        public PC_RequisitionDTO onchangedept(PC_RequisitionDTO data)
        {
            return _comm.POSTVMS(data, "PC_RequisitionFacade/onchangedept");
        }
        public PC_RequisitionDTO saverecord(PC_RequisitionDTO data)
        {
            return _comm.POSTVMS(data, "PC_RequisitionFacade/saverecord");
        }
        public PC_RequisitionDTO EditData(PC_RequisitionDTO data)
        {
            return _comm.POSTVMS(data, "PC_RequisitionFacade/EditData");
        }
        public PC_RequisitionDTO deactiveY(PC_RequisitionDTO data)
        {
            return _comm.POSTVMS(data, "PC_RequisitionFacade/deactiveY");
        }
        public PC_RequisitionDTO Viewdata(PC_RequisitionDTO data)
        {
            return _comm.POSTVMS(data, "PC_RequisitionFacade/Viewdata");
        }
        public PC_RequisitionDTO deactiveparticulars(PC_RequisitionDTO data)
        {
            return _comm.POSTVMS(data, "PC_RequisitionFacade/deactiveparticulars");
        }
    }
}
