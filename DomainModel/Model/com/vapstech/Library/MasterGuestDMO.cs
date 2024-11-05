using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DomainModel.Model.com.vapstech.Library
{
    [Table("Lib_M_Guest", Schema ="LIB")]
    public class MasterGuestDMO : CommonParamDMO
    {
        [Key]
        public long Guest_Id { get; set; }
        public long MI_Id { get; set; }
        public string Guest_Name { get; set; }
        public string Guest_address { get; set; }
        public string Guest_Email_Id { get; set; }
        public string Guest_Phone_No { get; set; }
        public bool Guest_ActiveFlag { get; set; }
    }
}
