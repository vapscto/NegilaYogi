using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Admission.Interface;
using NaacServiceHub.Admission.Interface.Criteria7;
using PreadmissionDTOs.NAAC.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Admission.FacadeController.Criteria7
{
    [Route("api/[controller]")]
    public class DifferentlyAbledFacade : Controller
    {
        public DifferentlyAbledInterface _Iobj;
        public DifferentlyAbledFacade(DifferentlyAbledInterface para)
        {
            _Iobj = para;
        }

        [Route("loaddata")]
        public Task<NAAC_AC_719_DifferentlyAbled_DTO> loaddata([FromBody] NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            return _Iobj.loaddata(data);
        }

        [Route("savedatatab1")]
        public NAAC_AC_719_DifferentlyAbled_DTO savedatatab1([FromBody] NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            return _Iobj.savedatatab1(data);
        }

        [Route("editTab1")]
        public NAAC_AC_719_DifferentlyAbled_DTO editTab1([FromBody] NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            return _Iobj.editTab1(data);
        }

        [Route("deactivYTab1")]
        public NAAC_AC_719_DifferentlyAbled_DTO deactivYTab1([FromBody] NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            return _Iobj.deactivYTab1(data);
        }

        [Route("deleteuploadfile")]
        public NAAC_AC_719_DifferentlyAbled_DTO deleteuploadfile([FromBody] NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            return _Iobj.deleteuploadfile(data);
        }

        [Route("getData")]
        public NAAC_AC_719_DifferentlyAbled_DTO getData([FromBody] NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            return _Iobj.getData(data);
        }

        [Route("getDataMC")]
        public NAAC_AC_719_DifferentlyAbled_DTO getDataMC([FromBody] NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            return _Iobj.getDataMC(data);
        }

        [Route("saveMC")]
        public NAAC_AC_719_DifferentlyAbled_DTO saveMC([FromBody] NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            return _Iobj.saveMC(data);
        }

        [Route("EditDataMC")]
        public NAAC_AC_719_DifferentlyAbled_DTO EditDataMC([FromBody] NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            return _Iobj.EditDataMC(data);
        }
    }
}
