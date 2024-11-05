using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class TTTTGenerationFromPreviousVersionDTO
    {
        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }

        public long TTMSAB_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string TTMSAB_Abbreviation { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool TTMSAB_ActiveFlag { get; set; }
        public Array versionlist { get; set; }
        public Array ttstafflist { get; set; }
        public Array sujectslistedit { get; set; }
        public Array acayear { get; set; }
        public string staffName { get; set; }
        // public string subjectName { get; set; }.
      //  public string staffNamelst { get; set; }
        public string yearName { get; set; }
    }
}
