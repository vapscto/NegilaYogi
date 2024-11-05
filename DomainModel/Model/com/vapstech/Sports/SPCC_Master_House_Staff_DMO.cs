using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Sports
{
    [Table("SPCC_Master_House_Staff", Schema = "SPC")]
    public class SPCC_Master_House_Staff_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long SPCCMHS_Id { get; set; }
        public long MI_Id { get; set; }
        public long SPCCMH_Id { get; set; }
        public long HRME_Id { get; set; }
        public string SPCCMHS_Description { get; set; }
        public bool SPCCMHS_ActiveFlag { get; set; }
        public long ASMAY_Id { get; set; }
       

    }
}
