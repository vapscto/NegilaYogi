using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.Alumni
{
    [Table("Alumni_Gallery", Schema = "ALU")]
    public class Alumni_Gallery_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ALGA_Id { get; set; }
        public long MI_Id { get; set; }
        public string ALGA_GalleryName { get; set; }
        public DateTime ALGA_Date { get; set; }
        public string ALGA_Time { get; set; }
        public string ALGA_CommonGalleryFlg { get; set; }
        public long ALGA_CreatedBy { get; set; }
        public long ALGA_UpdatedBy { get; set; }
        public bool ALGA_ActiveFlag { get; set; }
        public List<Alumni_Gallery_Photos_DMO> Alumni_Gallery_Photos_DMO { get;set;}
        public List<Alumni_Gallery_Videos_DMO> Alumni_Gallery_Videos_DMO { get;set;}
    }
}
