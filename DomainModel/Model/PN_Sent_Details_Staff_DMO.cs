using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model
{
    [Table("PN_Sent_Details_Staff")]
    public class PN_Sent_Details_Staff_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PNSDST_Id { get; set; }
        public long PNSD_Id { get; set; }
        public long HRME_Id { get; set; }
    }
}
