using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Portals.Chairman
{
   public class Clg_ClassDetails_DTO
    {

        public long UserId { get; set; }
        public long MI_Id { get; set; }
        public long AMCO_Id { get; set; }
        public Array allacademicyear { get; set; }
        public Array courselist { get; set; }
        public long ASMAY_Id { get; set; }
        public Array reportlist { get; set; }
        public Array categorylist { get; set; }
        public string AMCO_CourseName { get; set; }
        public string ASMAY_Year { get; set; }
        public string AMCST_FirstName { get; set; }
         public long count { get; set; }
         public long AMCOC_Id { get; set; }
         public string AMCOC_Name { get; set; }
    }
}
