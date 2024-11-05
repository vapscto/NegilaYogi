using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;

namespace WebApplication1.Interfaces
{
    public interface WrittenTestMarksEntryInterface
    {
        WirttenTestSubjectWiseMarksEntryDTO WrittenTestMarksEntryData(WirttenTestSubjectWiseMarksEntryDTO mas);

        WrittenTestMarksBindDataDTO GetWrittenTestMarksEntryData(WrittenTestMarksBindDataDTO WrittenTestMarksBindDataDTO);

        WirttenTestSubjectWiseMarksEntryDTO GetdetailsOnSchedule(WirttenTestSubjectWiseMarksEntryDTO WirttenTestSubjectWiseMarksEntryDTO);

        WrittenTestMarksBindDataDTO GetWrittenTestMarks(WrittenTestMarksBindDataDTO mas);


    }
}
