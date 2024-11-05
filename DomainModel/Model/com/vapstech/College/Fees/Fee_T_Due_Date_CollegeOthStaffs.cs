using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_College_T_Due_Date_OthStaffs", Schema = "CLG")]
    public class Fee_T_Due_Date_CollegeOthStaffs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FCTDDOST_Id { get; set; }
        public long FMCAOST_Id { get; set; }
        public string FCTDDOST_Day { get; set; }
        public string FCTDDOST_Month { get; set; }

        public int FCTDDOST_Year { get; set; }
        public DateTime? FCTDDOST_CreatedDate  { get; set; }
        public DateTime? FCTDDOST_UpdatedDate  { get; set; }
        public long? FCTDDOST_CreatedBy  { get; set; }
        public long?  FCTDDOST_UpdatedBy { get; set; }
    }
}
