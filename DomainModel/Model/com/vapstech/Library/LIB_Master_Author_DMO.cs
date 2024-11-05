using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Master_Author", Schema = "LIB")]
    public class LIB_Master_Author_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LMAU_Id { get; set; }
        public long MI_Id { get; set; }
        public string LMAU_AuthorFirstName { get; set; }
        public string LMAU_AuthorMiddleName { get; set; }
        public string LMAU_AuthorLastName { get; set; }
        public long? LMAU_MobileNo { get; set; }
        public string LMAU_PhoneNo { get; set; }
        public string LMAU_EmailId { get; set; }
        public string LMAU_Address { get; set; }
        public bool LMAU_ActiveFlg { get; set; }

        public List<MasterAuthorDMO> MasterAuthorDMO { get; set; }


    }
}
