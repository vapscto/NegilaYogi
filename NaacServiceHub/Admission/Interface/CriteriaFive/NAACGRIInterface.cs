using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
   public interface NAACGRIInterface
    {
        NAACGRIDTO loaddata(NAACGRIDTO data);
        NAACGRIDTO loaddatamed(NAACGRIDTO data);
        NAACGRIDTO save(NAACGRIDTO data);
        NAACGRIDTO deactiveStudent(NAACGRIDTO data);
        NAACGRIDTO EditData(NAACGRIDTO obj);
        NAACGRIDTO deleteuploadfile(NAACGRIDTO obj);
        NAACGRIDTO viewuploadflies(NAACGRIDTO obj);
        NAACGRIDTO savemedicaldatawisecomments(NAACGRIDTO obj);
        NAACGRIDTO getcomment(NAACGRIDTO obj);
        NAACGRIDTO getfilecomment(NAACGRIDTO obj);
        NAACGRIDTO savefilewisecomments(NAACGRIDTO obj);

    }
}
