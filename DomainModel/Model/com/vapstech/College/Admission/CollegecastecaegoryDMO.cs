using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("IVRM_Master_Caste_Category")]
    public class CollegecastecaegoryDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IMCC_Id { get; set; }
        public string IMCC_CategoryName { get; set; }
        public string IMCC_CategoryDesc { get; set; }
    }
}
