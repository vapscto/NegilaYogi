using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
   public interface Adm_School_Master_StreamInterface
    {
        Adm_School_Master_Stream_DTO getdata(Adm_School_Master_Stream_DTO data);
        Adm_School_Master_Stream_DTO savedata(Adm_School_Master_Stream_DTO data);
        Adm_School_Master_Stream_DTO editdata(Adm_School_Master_Stream_DTO data);
        Adm_School_Master_Stream_DTO activedeactive(Adm_School_Master_Stream_DTO data);
        Adm_School_Master_Stream_DTO savedata2(Adm_School_Master_Stream_DTO data);
        Adm_School_Master_Stream_DTO getdetails(Adm_School_Master_Stream_DTO data);
        Adm_School_Master_Stream_DTO deactive2(Adm_School_Master_Stream_DTO data);
        Adm_School_Master_Stream_DTO edit2(Adm_School_Master_Stream_DTO data);
    }
}
