﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.University.Interface;
using PreadmissionDTOs.NAAC.University;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.University.FacadeController
{
    [Route("api/[controller]")]
    public class NAAC_HSU_345_TeacherResearchPapersFacade : Controller
    {
        public NAAC_HSU_345_TeacherResearchPapersInterface inter;
        public NAAC_HSU_345_TeacherResearchPapersFacade(NAAC_HSU_345_TeacherResearchPapersInterface q)
        {
            inter = q;
        }

        [Route("loaddata")]
        public NAAC_HSU_345_TeacherResearchPapers_DTO loaddata([FromBody] NAAC_HSU_345_TeacherResearchPapers_DTO data)
        {
            return inter.loaddata(data);
        }
        [Route("save")]
        public NAAC_HSU_345_TeacherResearchPapers_DTO save([FromBody] NAAC_HSU_345_TeacherResearchPapers_DTO data)
        {
            return inter.save(data);
        }
        [Route("deactive")]
        public NAAC_HSU_345_TeacherResearchPapers_DTO deactive([FromBody] NAAC_HSU_345_TeacherResearchPapers_DTO data)
        {
            return inter.deactive(data);
        }
        [Route("EditData")]

        public NAAC_HSU_345_TeacherResearchPapers_DTO EditData([FromBody] NAAC_HSU_345_TeacherResearchPapers_DTO data)
        {
            return inter.EditData(data);
        }

        [Route("viewuploadflies")]
        public NAAC_HSU_345_TeacherResearchPapers_DTO viewuploadflies([FromBody] NAAC_HSU_345_TeacherResearchPapers_DTO data)
        {
            return inter.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public NAAC_HSU_345_TeacherResearchPapers_DTO deleteuploadfile([FromBody] NAAC_HSU_345_TeacherResearchPapers_DTO data)
        {
            return inter.deleteuploadfile(data);
        }
    }
}
