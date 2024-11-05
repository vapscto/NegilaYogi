using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.University
{
    [Table("NAAC_HSU_315_Facilites")]
    public class NAAC_HSU_315_FacilitesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NMC315F_Id { get; set; }
        public long MI_Id { get; set; }
        public string NMC315F_FacilitesName { get; set; }
        public long NMC315F_Year { get; set; }
        public string NMC315F_Link { get; set; }
        public DateTime? NMC315F_CreatedDate { get; set; }
        public DateTime? NMC315F_UpdatedDate { get; set; }
        
    }
}
