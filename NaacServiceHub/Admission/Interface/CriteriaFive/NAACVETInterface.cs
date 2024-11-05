using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
   public interface NAACVETInterface
    {
        NAACVETDTO loaddata(NAACVETDTO data);
        NAACVETDTO save(NAACVETDTO data);
        NAACVETDTO deactiveStudent(NAACVETDTO data);
        NAACVETDTO EditData(NAACVETDTO obj);
        NAACVETDTO deleteuploadfile(NAACVETDTO obj);
        NAACVETDTO viewuploadflies(NAACVETDTO obj);
        NAACVETDTO savemedicaldatawisecomments(NAACVETDTO obj);
        NAACVETDTO getcomment(NAACVETDTO obj);
        NAACVETDTO getfilecomment(NAACVETDTO obj);
        NAACVETDTO savefilewisecomments(NAACVETDTO obj);
    }
}
