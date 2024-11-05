using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model
{
    [Table("PN_Sent_Details_Student")]
    public class PN_Sent_Details_Student_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PNSDS_Id { get; set; }
        public long PNSD_Id { get; set; }
        public long AMST_Id { get; set; }
    }
}
