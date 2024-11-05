using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_College_T_Fine_Slabs_OthStaffs", Schema = "CLG")]
    public class Fee_T_Fine_Slabs_CollegeOthStaffs
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
   
        public long FCTFSOST_Id { get; set; }
        public long FMFS_Id { get; set; }
        public long FMCAOST_Id { get; set; }
        public string FCTFSOST_FineType { get; set; }
        public decimal FCTFSOST_Amount { get; set; }

         public DateTime? FCTFSOST_CreatedDate { get; set; }
        public DateTime? FCTFSOST_UpdatedDate { get; set; }
        public long?  FCTFSOST_CreatedBy { get; set; }
        public long?  FCTFSOST_UpdatedBy { get; set; }




    }
}
