using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Reports
{
   public class NaacMOU352ReportDTO
    {
      public long MI_Id { get; set; }
      public Array alldata1 { get; set; }
      public Array alldata12 { get; set; }
      public Array allacademicyear { get; set; }

public long ASMAY_Id { get; set; }

        public long NCAC352MOU_Id { get; set; }
       
        public string NCAC352MOU_OrganisationName { get; set; }
        public string NCAC352MOU_Name { get; set; }
        public long NCAC352MOU_SigningYear { get; set; }
        public string NCAC352MOU_Duration { get; set; }
        public string NCAC352MOU_ActivitiesList { get; set; }
        public long NCAC352MOU_NoOfStudents { get; set; }
        public long NCAC352MOU_NoOfStaff { get; set; }
        public NaacMOU352ReportDTO[] selectedYear { get; set; }
        public bool NCAC352MOU_ActiveFlg { get; set; }
        public string ASMAY_Year { get; set; }
        public long UserId { get; set; }
        public Array getinstitution { get; set; }
        public Array yearlist { get; set; }
        public Array getinstitutioncycle { get; set; }
        public Array reportlist { get; set; }
        public Array reportlist2 { get; set; }
        public long cycleid { get; set; }
        public NaacMOU352ReportDTO[] selected_Inst { get; set; }

    }
}
