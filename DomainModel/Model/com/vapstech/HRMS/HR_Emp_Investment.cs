using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Employee_Investment")]
    public class HR_Employee_Investment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HREID_Id

        { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long IMFY_Id { get; set; }
        public long HRMCVIA_Id { get; set; }
        public decimal? HREID_Amount { get; set; }
        // public string HRETDS_ChallanNo { get; set; }
        public bool HREID_ActiveFlg

        { get; set; }
        //public long HREID_CreatedBy

        //  { get; set; }
        //public long HREID_UpdatedBy

        // { get; set; }

    }
}
