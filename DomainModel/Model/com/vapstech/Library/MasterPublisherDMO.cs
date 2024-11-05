using System;
using System.Collections.Generic;
using System.Text;
using DomainModel.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Master_Publisher", Schema = "LIB")]
   public class MasterPublisherDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LMP_Id { get; set; }
        public long MI_Id { get; set; }
        public string LMP_PublisherName { get; set; }
        public long? LMP_MobileNo { get; set; }
        public string LMP_PhoneNo { get; set; }
        public string LMP_EMailId { get; set; }
        public string LMP_Address { get; set; }
        public bool LMP_ActiveFlg { get; set; }
        public List <BookRegisterDMO> BookRegisterDMO { get; set; }
    }
}
