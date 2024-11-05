using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.Alumni
{
    [Table("Alumni_Gallery_Photos", Schema = "ALU")]
    public class Alumni_Gallery_Photos_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ALGAP_Id { get; set; }
        public long ALGA_Id { get; set; }
        public string ALGAP_Photos { get; set; }
        public string ALGAP_CoverPhotoFlag { get; set; }
        public bool ALGAP_ActiveFlag { get; set; }
    }
}
