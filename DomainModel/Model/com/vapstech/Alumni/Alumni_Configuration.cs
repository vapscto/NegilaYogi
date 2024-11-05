using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Alumni
{
    [Table("Alumni_Configuration", Schema = "ALU")]
    public class Alumni_Configuration 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ALCON_Id { get; set; }
        public long MI_Id { get; set; }
        public bool ALCON_AlumniAutoTransferFlg { get; set; }
       
    }
}


