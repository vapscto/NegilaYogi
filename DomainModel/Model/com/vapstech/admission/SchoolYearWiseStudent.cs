using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.admission
{
    [Table("Adm_School_Y_Student")]
    public class SchoolYearWiseStudent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASYST_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMAY_Id { get; set; }
        public long AMAY_RollNo { get; set; }
        public int AMAY_PassFailFlag { get; set; }
        public long LoginId { get; set; }
        public DateTime AMAY_DateTime { get; set; }
        public int AMAY_ActiveFlag { get; set; }

    }
}
