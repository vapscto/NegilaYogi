using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("HR_Master_Qual_Subject")]
    public class HR_Master_Qual_SubjectDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMQS_Id { get; set; }
        public long HRMQSC_Id { get; set; }
        public string HRMQS_Name { get; set; }
        public bool HRMQS_ActiveFlg { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }

}