using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Client_Project_Master_Docs")]
    public class ISM_Client_Project_Master_Docs_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMCLTPRMDOC_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMCLTPRMDOC_Name { get; set; }
        public string ISMCLTPRMDOC_Description { get; set; }
        public bool ISMCLTPRMDOC_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
