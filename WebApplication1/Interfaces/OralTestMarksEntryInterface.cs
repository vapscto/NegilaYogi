using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;

namespace WebApplication1.Interfaces
{
    public interface OralTestMarksEntryInterface
    {
        OralTestOralByMarksDTO OralTestMarksEntryData(OralTestOralByMarksDTO mas);

        OralTestMarksBindDataDTO GetOralTestMarksEntryData(OralTestOralByMarksDTO masterMDT);
        OralTestOralByMarksDTO GetdetailsOnSchedule(OralTestOralByMarksDTO mas);
        OralTestMarksBindDataDTO[] GetOralTestMarks(OralTestMarksBindDataDTO mas);
    }
}
