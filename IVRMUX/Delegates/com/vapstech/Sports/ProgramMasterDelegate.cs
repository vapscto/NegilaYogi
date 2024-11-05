using CommonLibrary;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Sports
{
    public class ProgramMasterDelegate
    {
        CommonDelegate<ProgramMasterDTO, ProgramMasterDTO> COMSPRT = new CommonDelegate<ProgramMasterDTO, ProgramMasterDTO>();
        public ProgramMasterDTO getDetails(ProgramMasterDTO data)
        {
            return COMSPRT.POSTDataSports(data, "ProgramMasterFacade/getDetails/");
        }
        public ProgramMasterDTO save(ProgramMasterDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "ProgramMasterFacade/save/");
        }
        public ProgramMasterDTO EditDetails(int id)
        {
            return COMSPRT.GetDataByIdSports(id, "ProgramMasterFacade/EditDetails/");
        }
        public ProgramMasterDTO deactivate(ProgramMasterDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "ProgramMasterFacade/deactivate/");
        }

    }
}
