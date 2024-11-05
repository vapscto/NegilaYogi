using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CommonLibrary;

namespace corewebapi18072016.Delegates.com.vapstech.admission
{
    public class MasterReligionDelegate
    {
        CommonDelegate<MasterReligionDTO, MasterReligionDTO> COMMM = new CommonDelegate<MasterReligionDTO, MasterReligionDTO>();
        public MasterReligionDTO getdetails(int id)
        {
            MasterReligionDTO dto = null;
            return COMMM.GetDataByIdNoADM(id, dto, "MasterReligionFacade/");

           
        }
        public MasterReligionDTO saveRecord(MasterReligionDTO dto)
        {
            return COMMM.POSTDataADM(dto, "MasterReligionFacade/");

            
        }
        public MasterReligionDTO Edit(int id)
        {
            return COMMM.GetDataByIdADM(id, "MasterReligionFacade/Edit/");

         
        }
        public MasterReligionDTO deleterec(int id)
        {
            return COMMM.DeleteDataByIdADM(id, "MasterReligionFacade/deleterec/");

            
        }
        public MasterReligionDTO deactivate(MasterReligionDTO rel)
        {
            return COMMM.POSTDataADM(rel, "MasterReligionFacade/deactivate/");

        }
        public MasterReligionDTO SearchByColumn(MasterReligionDTO rel)
        {
            return COMMM.POSTDataADM(rel, "MasterReligionFacade/SearchByColumn/");

        }
        

    }
}
