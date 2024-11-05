using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Master_Book_Author", Schema ="LIB")]
   public class MasterAuthorDMO:CommonParamDMO
    {
        [Key]
        public long LMBA_Id { get; set; }
        public long LMB_Id { get; set; }
        public string LMBA_AuthorFirstName { get; set; }
        public string LMBA_AuthorMiddleName { get; set; }
        public string LMBA_AuthorLastName { get; set; }
        public bool LMBA_MainAuthorFlg { get; set; }
        public bool LMBA_ActiveFlg { get; set; }
        public long? LMAU_Id { get; set; }



    }
}
