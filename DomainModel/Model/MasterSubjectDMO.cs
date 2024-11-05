using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_Master_Subjects")]

    public class MasterSubjectDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PAMSU_Id { get; set; }
        public long MI_Id { get; set; }
        public string PAMSU_SubjectName { get; set; }
        public string PAMSU_SubjectCode { get; set; }
        public decimal PAMSU_MaxMarks { get; set; }
        public decimal PAMSU_MinMarks { get; set; }
        public string PAMSU_SubjectFlag { get; set; }
        public int PAMSU_ActiveFlag { get; set; }
    }
}
