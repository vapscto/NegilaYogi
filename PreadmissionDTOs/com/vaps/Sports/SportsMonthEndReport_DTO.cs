using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Sports
{
   public class SportsMonthEndReport_DTO:CommonParamDTO
    {
        public long MI_Id { get; set; }
        public Array fillmonth { get; set; }
        public int month { get; set; }
        public string monthname { get; set; }
        public Array fillyear { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public Array sportsreports { get; set; }
        public int count { get; set; }
        public string admNo { get; set; }
        public string studentName { get; set; }
        public string className { get; set; }
        public string sectionName { get; set; }
        public string houseName { get; set; }
        public string SPCCME_EventName { get; set; }
        public string SPCCMEV_EventVenue { get; set; }
        public DateTime? SPCCE_StartDate { get; set; }
        public string SPCCESTR_Rank { get; set; }
        public double points { get; set; }
        public int total_count_student { get; set; }
        public int total_partic_student { get; set; }
        public int total_winner_student { get; set; }
        public int not_patr_std { get; set; }
        public int ASMAY_Order { get; set; }
    }
}
