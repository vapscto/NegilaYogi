using PreadmissionDTOs.NAAC.Admission;
using PreadmissionDTOs.NAAC.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Documents.Interface
{
   public interface NAACGeneralCriteriaInterface
    {
        NAACGeneralCriteriaDTO loaddata(NAACGeneralCriteriaDTO data);
        NAACGeneralCriteriaDTO save(NAACGeneralCriteriaDTO data);
        NAACGeneralCriteriaDTO deactiveStudent(NAACGeneralCriteriaDTO data);
        NAACGeneralCriteriaDTO EditData(NAACGeneralCriteriaDTO obj);
        NAACGeneralCriteriaDTO viewuploadflies(NAACGeneralCriteriaDTO obj);
        NAACGeneralCriteriaDTO deleteuploadfile(NAACGeneralCriteriaDTO obj);
        NAACGeneralCriteriaDTO viewlink(NAACGeneralCriteriaDTO obj);
        NAACGeneralCriteriaDTO deletelink(NAACGeneralCriteriaDTO obj);

    }
}
