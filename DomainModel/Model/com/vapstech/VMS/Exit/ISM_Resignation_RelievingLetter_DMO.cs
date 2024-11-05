using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Exit
{
    [Table("ISM_Resignation_RelievingLetter")]
    public class ISM_Resignation_RelievingLetter_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMRESGRL_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMRESG_Id { get; set; }
        public string ISMRESGRL_RelievingLetterLNo { get; set; }
        public DateTime ISMRESGRL_RLDate { get; set; }
        public bool ISMRESGRL_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ISMRESGRL_CreatedBy { get; set; }
        public long? ISMRESGRL_UpdatedBy { get; set; }
    }
}
