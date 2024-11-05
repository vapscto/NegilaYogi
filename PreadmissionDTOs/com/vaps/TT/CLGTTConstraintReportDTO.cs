using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.TT
{
   public class CLGTTConstraintReportDTO
    {


        public long MI_ID { get; set; }
        //public long MI_Id { get; set; }
        //public Array acayear { get; set; }
        public Array Acayear { get; set; }
        public Array fix_day_period_list { get; set; }
        public Array restrict_day_period_list { get; set; }
        public Array consecutivelst { get; set; }
        public Array bif_detailslist { get; set; }
        public Array labdetailsarray { get; set; }
        public string constype { get; set; }
        public string periodtype { get; set; }
        public long ASMAY_Id { get; set; }
        public string TTMC_CategoryName { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string staffName { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string TTMP_PeriodName { get; set; }
        public string TTMD_DayName { get; set; }
        public string CategoryName { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public string SubjectName { get; set; }
        public decimal NoOfPeriods { get; set; }
        public string categoryName1 { get; set; }
        public string bifricationName { get; set; }
        public string periodname { get; set; }
        public string ASMAYYear { get; set; }
        public string CategoryName2 { get; set; }
        public string TTLAB_LABLIBName { get; set; }
        public Array getreportdata { get; set; }
  

    }
}
