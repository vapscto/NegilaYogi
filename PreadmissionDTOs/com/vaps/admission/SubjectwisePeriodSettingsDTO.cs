using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class SubjectwisePeriodSettingsDTO
    {
        public long ASASMP_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMC_Id { get; set; }
        public long PAMS_Id { get; set; }
        public int ASASMP_MaxPeriod { get; set; }

        public string AcedemicYear { get; set; }
        public string ClassName { get; set; }

        public string InstituteName { get; set; }

        public Array yeardropDown { get; set; }
        public Array classdropDown { get; set; }

        public Array sectiondropDown { get; set; }

        public Array GridviewList { get; set; }

        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
        public int MaxPeriod { get; set; }

        public List<MasterSectionDTO> SelectedSectionDetails { get; set; }

        public List<SubjectwisePeriodSettingsDTO> SelectedSubjectMaxPeriods { get; set; }
        public Array subjectwisePeriodCount { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }

        public Array currentAcdYear { get; set; }
        public int count { get; set; }

        // public AdmissionClassDTO[] SelectedClassDetails { get; set; }

    }
}
