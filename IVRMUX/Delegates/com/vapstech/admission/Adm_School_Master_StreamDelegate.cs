using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.admission
{
    public class Adm_School_Master_StreamDelegate
    {
        CommonDelegate<Adm_School_Master_Stream_DTO, Adm_School_Master_Stream_DTO> comm = new CommonDelegate<Adm_School_Master_Stream_DTO, Adm_School_Master_Stream_DTO>();
        public Adm_School_Master_Stream_DTO getdata(Adm_School_Master_Stream_DTO data)
        {
            return comm.POSTDataADM(data, "Adm_School_Master_StreamFacade/getdata");
        }
        public Adm_School_Master_Stream_DTO savedata(Adm_School_Master_Stream_DTO data)
        {
            return comm.POSTDataADM(data, "Adm_School_Master_StreamFacade/savedata");
        }
        public Adm_School_Master_Stream_DTO editdata(Adm_School_Master_Stream_DTO data)
        {
            return comm.POSTDataADM(data, "Adm_School_Master_StreamFacade/editdata");
        }
        public Adm_School_Master_Stream_DTO activedeactive(Adm_School_Master_Stream_DTO data)
        {
            return comm.POSTDataADM(data, "Adm_School_Master_StreamFacade/activedeactive");
        } public Adm_School_Master_Stream_DTO savedata2(Adm_School_Master_Stream_DTO data)
        {
            return comm.POSTDataADM(data, "Adm_School_Master_StreamFacade/savedata2");
        }
        public Adm_School_Master_Stream_DTO getdetails(Adm_School_Master_Stream_DTO data)
        {
            return comm.POSTDataADM(data, "Adm_School_Master_StreamFacade/getdetails");
        }public Adm_School_Master_Stream_DTO deactive2(Adm_School_Master_Stream_DTO data)
        {
            return comm.POSTDataADM(data, "Adm_School_Master_StreamFacade/deactive2");
        }public Adm_School_Master_Stream_DTO edit2(Adm_School_Master_Stream_DTO data)
        {
            return comm.POSTDataADM(data, "Adm_School_Master_StreamFacade/edit2");
        }
    }
}
