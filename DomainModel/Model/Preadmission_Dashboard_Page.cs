using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_Dashboard_Page")]
    public class Preadmission_Dashboard_PageDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PAPG_ID  { get; set; }
        public long PAPG_MIID  { get; set; }
        public string PAPG_TYPE  { get; set; }
        public string PAPG_PAGENAME  { get; set; }

        public DateTime? PAPG_CreatedDate { get; set; }
        public DateTime? PAPG_UpdatedDate { get; set; }
        public long? PAPG_CreatedBy { get; set; }
        public long? PAPG_UpdatedBy { get; set; }

    }
}