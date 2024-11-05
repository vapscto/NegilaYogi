using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_M_Category")]
    public class exammastercategoryDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]     


        public long EMCA_Id { get; set; }
        public string EMCA_Name { get; set; }
        public string EMCA_Detail { get; set; }
       
    }
}
