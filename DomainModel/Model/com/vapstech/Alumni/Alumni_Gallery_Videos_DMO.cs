using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.Alumni
{
    [Table("Alumni_Gallery_Videos", Schema = "ALU")]
    public class Alumni_Gallery_Videos_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ALGAV_Id { get; set; }
        public long ALGA_Id { get; set; }
        public string ALGAV_Videos { get; set; }
        public bool ALGAV_ActiveFlag { get; set; }
    }
}
