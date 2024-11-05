using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.COE
{
    [Table("COE_Events_Others",Schema ="COE")]
    public class COE_Events_OthersDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]


        public int COEEO_Id { get; set; }
        [ForeignKey("COEE_Id")]
        public int COEE_Id { get; set; }
        public long COEEO_MobileNo { get; set; }
        public string COEEO_Name { get; set; }
        public string COEEO_Emailid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }


    }
}
