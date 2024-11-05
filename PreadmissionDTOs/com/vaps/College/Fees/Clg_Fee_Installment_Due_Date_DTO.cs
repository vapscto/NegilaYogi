using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Fee
{
    public class Clg_Fee_Installment_Due_Date_DTO
    {
        public long FTIDD_Id { get; set; }
        public long MI_Id { get; set; }
        public long FTI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime FTIDD_FromDate { get; set; }
        public DateTime FTIDD_ToDate { get; set; }
        public DateTime FTIDD_ApplicableDate { get; set; }
        public DateTime FTIDD_DueDate { get; set; }
        public Array retrivedata { get; set; }

        public string returnvalexist { get; set; }
        public Array InstallmentDatad { get; set; }
        public bool returnval { get; set; }

        public string returnvalstring { get; set; }
        //01/12/2016
        public DateTime fdate1 { get; set; }
        public DateTime tdate1 { get; set; }
        public DateTime Aplc1 { get; set; }
        public DateTime ddate1 { get; set; }
        public string fname { get; set; }
        public string ftI_Name { get; set; }


        public long fmiid { set; get; }
        public string ftiname { set; get; }
        public DateTime fromdate2 { set; get; }
        public DateTime todate2 { set; get; }
        public DateTime apldate2 { set; get; }
        public DateTime ddudate2 { set; get; }
        public long yrid { set; get; }
    }
}
