using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
   public interface NAACNonGovShcrshipHsuInterface
    {
        NAACNonGovShcrshipHsuDTO loaddata(NAACNonGovShcrshipHsuDTO data);
        NAACNonGovShcrshipHsuDTO save(NAACNonGovShcrshipHsuDTO data);
        NAACNonGovShcrshipHsuDTO deactiveStudent(NAACNonGovShcrshipHsuDTO data);
        NAACNonGovShcrshipHsuDTO EditData(NAACNonGovShcrshipHsuDTO obj);
        NAACNonGovShcrshipHsuDTO viewuploadflies(NAACNonGovShcrshipHsuDTO obj);
        NAACNonGovShcrshipHsuDTO deleteuploadfile(NAACNonGovShcrshipHsuDTO obj);
        NAACNonGovShcrshipHsuDTO savemedicaldatawisecomments(NAACNonGovShcrshipHsuDTO obj);
        NAACNonGovShcrshipHsuDTO getcomment(NAACNonGovShcrshipHsuDTO obj);
        NAACNonGovShcrshipHsuDTO getfilecomment(NAACNonGovShcrshipHsuDTO obj);
        NAACNonGovShcrshipHsuDTO savefilewisecomments(NAACNonGovShcrshipHsuDTO obj);
    }
}
