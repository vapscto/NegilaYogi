
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface exammasterCoCurricularInterface
    {
        exammasterCoCurricularDTO savedetails(exammasterCoCurricularDTO data);
        exammasterCoCurricularDTO validateordernumber(exammasterCoCurricularDTO data);
        exammasterCoCurricularDTO deactivate(exammasterCoCurricularDTO data);
        exammasterCoCurricularDTO editdetails(int ID);
        exammasterCoCurricularDTO Getdetails(exammasterCoCurricularDTO data);

        // Student Personlaity Mapping
        exammasterCoCurricularDTO studentdataload(exammasterCoCurricularDTO data);
        exammasterCoCurricularDTO onchangeyear(exammasterCoCurricularDTO data);
        exammasterCoCurricularDTO onchangeclass(exammasterCoCurricularDTO data);
        exammasterCoCurricularDTO onchangesection(exammasterCoCurricularDTO data);
        exammasterCoCurricularDTO searchdata(exammasterCoCurricularDTO data);
        exammasterCoCurricularDTO savemapping(exammasterCoCurricularDTO data);
        exammasterCoCurricularDTO editmappingdetails(exammasterCoCurricularDTO data);

    }
}
