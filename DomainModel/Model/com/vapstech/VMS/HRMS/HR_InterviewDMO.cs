using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("HR_Interview")]
    public class HR_InterviewDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRI_Id { get; set; }
        public string HRI_Title { get; set; }
        public string HRI_AssignTo { get; set; }
        public string HRI_Type { get; set; }
        public DateTime HRI_Datetime { get; set; }
        public string HRI_Status { get; set; }
        public bool HRI_ActiveFlg { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }

}