using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Portals.Student
{
    [Table("Adm_Master_Certificate")]
    public class Adm_Master_Certificate_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMCT_Id { get; set; }
      
        public string AMCT_Certificate_Name { get; set; }
        public string AMCT_Certificate_code { get; set; }
        public string AMCT_Description { get; set; }
        public bool AMCT_ActiceFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
