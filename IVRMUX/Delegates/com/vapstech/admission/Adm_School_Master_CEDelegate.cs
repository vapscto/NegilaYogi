using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.admission
{
    public class Adm_School_Master_CEDelegate
    {

        CommonDelegate<Adm_School_Master_CE_DTO, Adm_School_Master_CE_DTO> comm = new CommonDelegate<Adm_School_Master_CE_DTO, Adm_School_Master_CE_DTO>();
        public Adm_School_Master_CE_DTO getdata(Adm_School_Master_CE_DTO data)
        {
            return comm.POSTDataADM(data, "Adm_School_Master_CEFacade/getdata");
        }
        public Adm_School_Master_CE_DTO savedata(Adm_School_Master_CE_DTO data)
        {
            return comm.POSTDataADM(data, "Adm_School_Master_CEFacade/savedata");
        }
        public Adm_School_Master_CE_DTO editdata(Adm_School_Master_CE_DTO data)
        {
            return comm.POSTDataADM(data, "Adm_School_Master_CEFacade/editdata");
        }
        public Adm_School_Master_CE_DTO activedeactive(Adm_School_Master_CE_DTO data)
        {
            return comm.POSTDataADM(data, "Adm_School_Master_CEFacade/activedeactive");
        }
        public Adm_School_Master_CE_DTO savedata2(Adm_School_Master_CE_DTO data)
        {
            return comm.POSTDataADM(data, "Adm_School_Master_CEFacade/savedata2");
        }
        public Adm_School_Master_CE_DTO deactive2(Adm_School_Master_CE_DTO data)
        {
            return comm.POSTDataADM(data, "Adm_School_Master_CEFacade/deactive2");
        }
        public Adm_School_Master_CE_DTO edit2(Adm_School_Master_CE_DTO data)
        {
            return comm.POSTDataADM(data, "Adm_School_Master_CEFacade/edit2");
        }
    }
}
