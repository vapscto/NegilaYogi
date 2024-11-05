using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdmissionServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class Adm_School_Master_StreamFacade : Controller
    {
       public Adm_School_Master_StreamInterface _inter;
        public Adm_School_Master_StreamFacade(Adm_School_Master_StreamInterface r)
        {
            _inter = r;
        }
        [Route("getdata")]
        public Adm_School_Master_Stream_DTO getdata([FromBody] Adm_School_Master_Stream_DTO data)
        {
            return _inter.getdata(data);
        }
        [Route("savedata")]
        public Adm_School_Master_Stream_DTO savedata([FromBody] Adm_School_Master_Stream_DTO data)
        {
            return _inter.savedata(data);
        }
        [Route("editdata")]
        public Adm_School_Master_Stream_DTO editdata([FromBody] Adm_School_Master_Stream_DTO data)
        {
            return _inter.editdata(data);
        }
        [Route("activedeactive")]
        public Adm_School_Master_Stream_DTO activedeactive([FromBody] Adm_School_Master_Stream_DTO data)
        {
            return _inter.activedeactive(data);
        }
        [Route("savedata2")]
        public Adm_School_Master_Stream_DTO savedata2([FromBody] Adm_School_Master_Stream_DTO data)
        {
            return _inter.savedata2(data);
        }
        [Route("getdetails")]
        public Adm_School_Master_Stream_DTO getdetails([FromBody] Adm_School_Master_Stream_DTO data)
        {
            return _inter.getdetails(data);
        } [Route("deactive2")]
        public Adm_School_Master_Stream_DTO deactive2([FromBody] Adm_School_Master_Stream_DTO data)
        {
            return _inter.deactive2(data);
        } [Route("edit2")]
        public Adm_School_Master_Stream_DTO edit2([FromBody] Adm_School_Master_Stream_DTO data)
        {
            return _inter.edit2(data);
        }

    }
}
