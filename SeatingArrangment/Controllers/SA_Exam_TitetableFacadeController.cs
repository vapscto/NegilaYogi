using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.SeatingArrangment;
using SeatingArrangment.Interface;

namespace SeatingArrangment.Controllers
{
   
    [Route("api/[controller]")]
    public class SA_Exam_TitetableFacadeController : Controller
    {
        SA_Exam_TitetableInterface ETT;
        public SA_Exam_TitetableFacadeController (SA_Exam_TitetableInterface et)
        {
            ETT = et;
        }
        [Route("load_TT")]
        public SA_Exam_TitetableDTO load_TT ([FromBody] SA_Exam_TitetableDTO dto)
        {
            return ETT.load_TT(dto);
        }
        [Route("Save_TT")]
        public SA_Exam_TitetableDTO Save_TT([FromBody] SA_Exam_TitetableDTO dto)
        {
            return ETT.Save_TT(dto);
        }
        [Route("Edit_TT")]
        public SA_Exam_TitetableDTO Edit_TT([FromBody] SA_Exam_TitetableDTO dto)
        {
            return ETT.Edit_TT(dto);
        }
        [Route("Deactive_TT")]
        public SA_Exam_TitetableDTO Deactive_TT([FromBody] SA_Exam_TitetableDTO dto)
        {
            return ETT.Deactive_TT(dto);
        }

        [Route("viewTTdetails")]
        public SA_Exam_TitetableDTO viewTTdetails([FromBody] SA_Exam_TitetableDTO dto)
        {
            return ETT.viewTTdetails(dto);
        }

    }
}