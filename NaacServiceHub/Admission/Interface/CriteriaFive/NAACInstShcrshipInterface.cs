using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
   public interface NAACInstShcrshipInterface
    {
        NAACInstShcrshipDTO loaddata(NAACInstShcrshipDTO data);
        NAACInstShcrshipDTO save(NAACInstShcrshipDTO data);
        NAACInstShcrshipDTO deactiveStudent(NAACInstShcrshipDTO data);
        NAACInstShcrshipDTO EditData(NAACInstShcrshipDTO obj);
        NAACInstShcrshipDTO viewuploadflies(NAACInstShcrshipDTO obj);
        NAACInstShcrshipDTO deleteuploadfile(NAACInstShcrshipDTO obj);
        NAACInstShcrshipDTO savemedicaldatawisecomments(NAACInstShcrshipDTO obj);
        NAACInstShcrshipDTO getcomment(NAACInstShcrshipDTO obj);
        NAACInstShcrshipDTO getfilecomment(NAACInstShcrshipDTO obj);
        NAACInstShcrshipDTO savefilewisecomments(NAACInstShcrshipDTO obj);

    }
}
