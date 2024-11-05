using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_Allowance")]
    public class HR_Master_Allowance
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMAL_Id
        { get; set; }
        public long MI_Id  
        { get; set; }
        public string HRMAL_AllowanceName
        { get; set; }
        public bool HRMAL_AllowanceFlg
        { get; set; }
        public bool HRMAL_MaxLimitAplFlg
        { get; set; }
        public decimal? HRMAL_MaxLimit
        { get; set; }
        public bool HRMAL_ActiveFlg
        { get; set; }
       // public bool HRETDS_ActiveFlg { get; set; }
        public long HRMAL_CreatedBy
        { get; set; }
        public long HRMAL_UpdatedBy { get; set; }
    }
}
