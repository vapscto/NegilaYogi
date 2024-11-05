using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Master_Book", Schema ="LIB")]
    public class BookRegisterDMO : CommonParamDMO
    {
       [Key]
        public long LMB_Id { get; set; }
        public long MI_Id { get; set; }
        public string LMB_BookTitle { get; set; }
        public string LMB_BookSubTitle { get; set; }
        public string LMB_CallNo { get; set; }
        public string LMB_ClassNo { get; set; }
        public string LMB_ISBNNo { get; set; }
        public string LMB_VolNo { get; set; }
        public string LMB_BookType { get; set; }
        public long LMD_Id { get; set; }
        public long? LML_Id { get; set; }
        public long LMS_Id { get; set; }
        public long LMC_Id { get; set; }
        public long? LMP_Id { get; set; }
        public string LMB_BillNo { get; set; }
        public string LMB_VoucherNo { get; set; }
        public decimal LMB_Price { get; set; }
        public decimal LMB_Discount { get; set; }
        public decimal LMB_NetPrice { get; set; }
        public string LMB_BindingType { get; set; }
        public DateTime? LMB_PurchaseDate { get; set; }
        public DateTime LMB_EntryDate { get; set; }
        public string LMB_Biblography { get; set; }
        public string LMB_IndexPage { get; set; }
        public long LMB_NoOfPages { get; set; }
        public string LMB_PublishedYear { get; set; }
        public string LMB_Edition { get; set; }
        public string LMB_PurOrDonated { get; set; }
        public string LMB_DonorName { get; set; }
        public string LMB_DonorAddress { get; set; }
        public string LMB_Remarks { get; set; }
        public string LMB_Keywords { get; set; }
        public string LMB_BookImage { get; set; }
        public long LMB_NoOfCopies { get; set; }
        public bool LMB_WithAccessories { get; set; }
        public string LMB_CurrenceyType { get; set; }
        public bool LMB_ActiveFlg { get; set; }
        public string LMB_BookNo { get; set; }


        public List<Lib_M_Book_Accn_DMO> Lib_M_Book_Accn_DMO { get; set; }
        public List<LIB_Master_Book_VendorDMO> LIB_Master_Book_VendorDMO { get; set; }
        public List<MasterAuthorDMO> MasterAuthorDMO { get; set; }
        public List<LIB_Master_Book_Library_DMO> LIB_Master_Book_Library_DMO { get; set; }
        public List<LIB_Master_Book_Accessories_DMO> LIB_Master_Book_Accessories_DMO { get; set; }
        public List<LIB_Master_Book_KeyFactor_DMO> LIB_Master_Book_KeyFactor_DMO { get; set; }
        public List<Master_Book_FilesDMO> Master_Book_FilesDMO { get; set; }

    }
}
