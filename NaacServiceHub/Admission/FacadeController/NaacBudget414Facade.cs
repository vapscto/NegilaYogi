using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Admission.Interface;
using PreadmissionDTOs.NAAC.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Admission.FacadeController
{
    [Route("api/[controller]")]
    public class NaacBudget414Facade : Controller
    {
        public NaacBudget_414_Interface _inter;

        public NaacBudget414Facade(NaacBudget_414_Interface r)
        {
            _inter = r;
        }

        [Route("loaddata")]
        public NaacBudget_414_DTO loaddata([FromBody] NaacBudget_414_DTO data)
        {
            return _inter.loaddata(data);
        }
        [Route("save")]
        public NaacBudget_414_DTO save([FromBody] NaacBudget_414_DTO data)
        {
            return _inter.save(data);
        }
        [Route("getcomment")]
        public NaacBudget_414_DTO getcomment([FromBody] NaacBudget_414_DTO data)
        {
            return _inter.getcomment(data);
        }
        [Route("savemedicaldatawisecomments")]
        public NaacBudget_414_DTO savemedicaldatawisecomments([FromBody] NaacBudget_414_DTO data)
        {
            return _inter.savemedicaldatawisecomments(data);
        }
        [Route("getfilecomment")]
        public NaacBudget_414_DTO getfilecomment([FromBody] NaacBudget_414_DTO data)
        {
            return _inter.getfilecomment(data);
        }
        [Route("savefilewisecomments")]
        public NaacBudget_414_DTO savefilewisecomments([FromBody] NaacBudget_414_DTO data)
        {
            return _inter.savefilewisecomments(data);
        }
        [Route("EditData")]
        public NaacBudget_414_DTO EditData([FromBody] NaacBudget_414_DTO data)
        {
            return _inter.EditData(data);
        }
        [Route("deactiveStudent")]
        public NaacBudget_414_DTO deactiveStudent([FromBody] NaacBudget_414_DTO data)
        {
            return _inter.deactiveStudent(data);
        }
        [Route("viewuploadflies")]
        public NaacBudget_414_DTO viewuploadflies([FromBody] NaacBudget_414_DTO data)
        {
            return _inter.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public NaacBudget_414_DTO deleteuploadfile([FromBody] NaacBudget_414_DTO data)
        {
            return _inter.deleteuploadfile(data);
        }
        
    }
}
