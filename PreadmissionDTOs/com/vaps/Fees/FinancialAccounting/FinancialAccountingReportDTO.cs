using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees.FinancialAccounting
{
    public class FinancialAccountingReportDTO
    {
        public long MI_Id { get; set; }
        public long user_id { get; set; }
        public long IMFY_Id { get; set; }
        public long FAMCOMP_Id { get; set; }
        public long FAMVOU_Id { get; set; }
        public long FAMGRP_Id { get; set; }
        public long FAMLED_Id { get; set; }
        public long Monthid { get; set; }
        public DateTime Fromdate { get; set; }
        public DateTime Todate { get; set; }
        public string type { get; set; }
        public string reporttype { get; set; }
        public Array fillcompany { get; set; }
        public Array fillfinacialyear { get; set; }
        public Array reportdetails { get; set; }
        public Array subreportdetails { get; set; }
    }
}
