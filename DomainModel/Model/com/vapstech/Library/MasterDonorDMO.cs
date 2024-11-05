using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("Lib_M_Donor", Schema ="LIB")]
   public class MasterDonorDMO:CommonParamDMO
    {
        [Key]
        public long Donor_Id { get; set; }
        public long MI_Id { get; set; }
        public string Donor_Name { get; set; }
        public string Donor_Address { get; set; }
        public bool Donor_ActiveFlag { get; set; }
       
    }
}
