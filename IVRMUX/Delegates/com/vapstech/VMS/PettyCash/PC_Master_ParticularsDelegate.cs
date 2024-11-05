using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.PettyCash
{
    public class PC_Master_ParticularsDelegate
    {
        CommonDelegate<PC_Master_ParticularsDTO, PC_Master_ParticularsDTO> _com = new CommonDelegate<PC_Master_ParticularsDTO, PC_Master_ParticularsDTO>();

        public PC_Master_ParticularsDTO onloaddata(PC_Master_ParticularsDTO data)
        {
            return _com.POSTVMS(data, "PC_Master_ParticularsFacade/onloaddata");
        }
        public PC_Master_ParticularsDTO saverecord(PC_Master_ParticularsDTO data)
        {
            return _com.POSTVMS(data, "PC_Master_ParticularsFacade/saverecord");
        }     
        public PC_Master_ParticularsDTO deactiveY(PC_Master_ParticularsDTO data)
        {
            return _com.POSTVMS(data, "PC_Master_ParticularsFacade/deactiveY");
        }
    } 
}
