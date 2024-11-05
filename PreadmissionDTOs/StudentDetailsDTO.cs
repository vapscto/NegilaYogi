﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class StudentDetailsDTO : CommonParamDTO
    {
        public long PASR_Id { get; set; }

        public string PASR_FirstName { get; set; }
        public string PASR_MiddleName { get; set; }
        public string PASR_LastName { get; set; }
        public string PASR_RegistrationNo { get; set; }
        public string PASR_Sex { get; set; }
        public long PASR_MobileNo { get; set; }
        public string PASR_emailId { get; set; }

        public long MI_Id { get; set; }
        public long Id { get; set; }
        public long ASMAY_Id { get; set; }
        public Array WrittenTestSchedule { get; set; }

        public Array OralTestSchedule { get; set; }

        public Array VcOralTestSchedule { get; set; }
        public Array OralTestSchedulecount { get; set; }
        public Array overallOralTestSchedulecount  { get; set; }

        public Array stafflist { get; set; }
        public long userid { get; set; }
        public string HRME_EmployeeFirstName { get; set; }

        public long HRME_Id { get; set; }
        public stfiddto[] stfids { get; set; }

        public bool principalflg { get; set; }      
        public bool managerflg { get; set; }
        public bool stafflag { get; set; }
        public bool anybodystart { get; set; }
        public bool pavidc { get; set; }

        public Array studentDetails { get; set; }

        public Array mstConfig { get; set; }

        public int payementcheck { get; set; }

        public Array admissioncatdrp { get; set; }

        public Array admissioncatdrpall { get; set; }

        public Array AcademicList { get; set; }

        public Array classlist { get; set; }

        public Array statuslist { get; set; }

        public Array schedulecount { get; set; }

        public long ASMCL_ID { get; set; }
        public bool vcmeeting  { get; set; }
        public long PAOTS_Id { get; set; }
        public long PAOTSS_Id { get; set; }

    }
    
}
