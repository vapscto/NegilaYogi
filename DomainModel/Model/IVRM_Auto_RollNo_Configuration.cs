using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model
{
    [Table("IVRM_Auto_RollNo_Configuration")]
    public class IVRM_Auto_RollNo_Configuration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMARNC_Id { get; set; }
        public long MI_Id { get; set; }
        public string IVRMARNC_Field { get; set; }
        public string IVRMARNC_AscDscOrder { get; set; }
        public long IVRMARNC_Order { get; set; }
        public bool IVRMARNC_ActiveFlag { get; set; }
        public DateTime? IVRMARNC_CreatedDate { get; set; }
        public DateTime? IVRMARNC_UpdatedDate { get; set; }
        public long IVRMARNC_CreatedBy { get; set; }
        public long IVRMARNC_UpdatedBy { get; set; }

    }
}
