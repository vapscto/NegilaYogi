using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
   public class MatserNonBook_DTO:CommonParamDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long LMNBK_Id { get; set; }
        public long LMNBKANO_Id { get; set; }
        public long LMPE_Id { get; set; }
        public long LMP_Id { get; set; }
        public long LMD_Id { get; set; }
        public long LMV_Id { get; set; }
        public long LMC_Id { get; set; }
        public long LML_Id { get; set; }
        public long? LMSU_Id { get; set; }
        public string LMNBK_NonBookTitle { get; set; }
        public string LMNBK_VolumeNo { get; set; }
        public string LMNBK_IssueNo { get; set; }
        public string LMNBK_PeriodicalTypeFlg { get; set; }
        public string LMNBK_ISBN { get; set; }
        public DateTime LMNBK_PublishDate { get; set; }
        public DateTime LMNBK_PurchaseDate { get; set; }
        public string LMNBK_BindStatus { get; set; }
        public decimal LMNBK_Price { get; set; }
        public bool LMNBK_WithAccessoriesFlg { get; set; }
        public decimal LMNBK_Discount { get; set; }
        public string LMNBK_DiscountTypeFlg { get; set; }
        public decimal LMNBK_NetPrice { get; set; }
        public string LMNBK_BindingType { get; set; }
        public string LMNBK_BillNo { get; set; }
        public string LMNBK_VoucherNo { get; set; }
        public string LMNBK_NoOfPages { get; set; }
        public string LMNBK_SourceType { get; set; }
        public string LMNBK_DonarName { get; set; }
        public string LMNBK_DonarAddress { get; set; }
        public string LMNBK_Keywords { get; set; }
        public DateTime LMNBK_BillDate { get; set; }
        public string LMNBK_NoOfCopies { get; set; }
        public string LMNBK_ReferenceNo { get; set; }
        public string LMNBK_Description { get; set; }
        public string LMNBK_CurrencyType { get; set; }
        public long? LMAC_Id { get; set; }
        public long LMRA_Id { get; set; }
        public long LMAL_Id { get; set; }
        public long LMNBKF_Id { get; set; }
        public string LMNBKF_KeyFactor { get; set; }
        public int LMNBKF_PageNo { get; set; }
        public string LMNBKANO_AccnNo { get; set; }
        public string LMNBKANO_AvailableStatus { get; set; }
        public DateTime? LMNBKANO_DeletedLostDate { get; set; }
        public bool chkaccessionno { get; set; }
        public MatserNonBook_DTO[] savetmpdata { get; set; }



        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array subjectlist { get; set; }
        public Array deptlist { get; set; }
        public Array racklist { get; set; }
        public Array langlist { get; set; }
        public Array vendorlist { get; set; }
        public Array publisherlst { get; set; }
        public Array accessorieslist { get; set; }
        public Array librarylist { get; set; }
        public Array categorylist { get; set; }
        public Array editlis { get; set; }
        public Array alldata { get; set; }
        public Array subscriptionist { get; set; }
        public Array periodicitylist { get; set; }
       
        public string Delete_Reason { get; set; }
        public string Book_Prefix { get; set; }
        public string Book_Suffix { get; set; }
        public string LMD_DepartmentName { get; set; }
        public string LML_LanguageName { get; set; }
        public string LMP_PublisherName { get; set; }
        public bool LMNBKANO_ActiveFlg { get; set; }
        public string LMRA_RackName { get; set; }
        public string LMV_VendorName { get; set; }
        public string LMC_CategoryName { get; set; }
        public long LMNBKL_Id { get; set; }



    }
}
