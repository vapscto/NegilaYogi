using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
    public class ImportLibrarydataDTO : CommonParamDTO
    {
        public long LMBA_Id { get; set; }
        public long LMB_Id { get; set; }
        public long MI_Id { get; set; }
        public long LMV_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string LMBA_AuthorFirstName { get; set; }
        public string LMBA_AuthorMiddleName { get; set; }
        public string LMBA_AuthorLastName { get; set; }
        public string Author_Address { get; set; }
        public bool LMBA_ActiveFlg { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array authorlist { get; set; }
        public int dupcnt { get; set; }
        public int failcnt { get; set; }
        public int suscnt { get; set; }

        public long LMAU_Id { get; set; }
        public LIB_Master_SubjectIMP[] mastersub { get; set; }
        public LIB_Master_DepartmentIMP[] masterdep { get; set; }
        public LIB_Master_LanguageIMP[] masterlang { get; set; }
        public LIB_Master_Publisher[] masterpubl { get; set; }
        public LIB_Master_BookIMP[] masterbook { get; set; }
        public LIB_Master_RackIMP[] masterrack { get; set; }
        public LIB_Master_Book_AuthorIMP[] masterauthor  { get; set; }
        public LIB_Master_Book_AccnNoIMP[] masteracces  { get; set; }
        public booktransIMP[] booktransIMP { get; set; }
        public LIB_Master_Book_vendorIMP[] mastervender { get; set; }
        public long  LMAL_Id { get; set; }
        public Array masterlibarary { get; set; }
    }


    public class LIB_Master_SubjectIMP
    {
        public long LMS_Id { get; set; }
        public long MI_Id { get; set; }
        public string LMS_SubjectName { get; set; }
        public string LMS_ParentId { get; set; }
        public string LMS_ActiveFlg { get; set; }
        public string LMS_SubjectNo { get; set; }
        public string LMS_Level { get; set; }


    }

    public class LIB_Master_DepartmentIMP
    {
        public long LMD_Id { get; set; }
        public long MI_Id { get; set; }
        public string LMD_DepartmentName { get; set; }
        public string LMD_ActiveFlg { get; set; }

    }


    public class LIB_Master_LanguageIMP
    {
        public long LML_Id { get; set; }
        public long MI_Id { get; set; }
        public string LML_LanguageName { get; set; }
        public string LML_ActiveFlg { get; set; }

    }

    public class LIB_Master_Publisher
    {
        public long LMP_Id { get; set; }
        public long MI_Id { get; set; }
        public string LMP_PublisherName { get; set; }
        public string LMP_MobileNo { get; set; }
        public string LMP_PhoneNo { get; set; }
        public string LMP_EMailId { get; set; }
        public string LMP_Address { get; set; }
        public string LMP_ActiveFlg { get; set; }

    }


    public class LIB_Master_BookIMP
    {
        public long LMB_Id { get; set; }
        public long MI_Id { get; set; }
        public string LMB_BookTitle { get; set; }
        public string LMB_BookSubTitle { get; set; }
        public string LMB_CallNo { get; set; }
        public string LMB_ClassNo { get; set; }
        public string LMB_ISBNNo { get; set; }
        public string LMB_VolNo { get; set; }
        public string LMB_BookType { get; set; }
        public string LMD_Id { get; set; }
        public string LMD_DepartmentName { get; set; }
        public string LML_Id { get; set; }
        public string LML_LanguageName { get; set; }
        public string LMS_Id { get; set; }
        public string LMS_SubjectName { get; set; }
        public string LMC_Id { get; set; }
        public string LMB_BookNo { get; set; }
        public string LMP_PublisherName { get; set; }
        public string LMP_Id { get; set; }
        public string LMB_BillNo { get; set; }
        public string LMB_VoucherNo { get; set; }
        public string LMB_Price { get; set; }
        public string LMB_Discount { get; set; }
        public string LMB_NetPrice { get; set; }
        public string LMB_BindingType { get; set; }
        public string LMB_PurchaseDate { get; set; }
        public string LMB_EntryDate { get; set; }
        public string LMB_Biblography { get; set; }
        public string LMB_IndexPage { get; set; }
        public string LMB_NoOfPages { get; set; }
        public string LMB_PublishedYear { get; set; }
        public string LMB_Edition { get; set; }
        public string LMB_PurOrDonated { get; set; }
        public string LMB_DonorName { get; set; }
        public string LMB_DonorAddress { get; set; }
        public string LMB_Remarks { get; set; }
        public string LMB_Keywords { get; set; }
        public string LMB_NoOfCopies { get; set; }
        public string LMB_WithAccessories { get; set; }
        public string LMB_CurrenceyType { get; set; }
        public string LMB_BookImage { get; set; }
        public string LMB_ActiveFlg { get; set; }
        
    }


    public class LIB_Master_RackIMP
    {
        public long LMRA_Id { get; set; }
        public long MI_Id { get; set; }
        public string LMRA_RackName { get; set; }
        public string LMRA_DisplayColour { get; set; }
        public string LMRA_BuildingName { get; set; }
        public string LMRA_FloorName { get; set; }
        public string LMRA_Location { get; set; }
      

    }

    public class LIB_Master_Book_AuthorIMP
    {
        public long LMBA_Id { get; set; }
        public long LMB_Id { get; set; }
        public string LMB_BookSubTitle   { get; set; }
        public string LMBA_AuthorFirstName { get; set; }
        public string LMBA_AuthorMiddleName { get; set; }
        public string LMBA_AuthorLastName { get; set; }
        public string LMBA_MainAuthorFlg { get; set; }
        public string LMBA_ActiveFlg { get; set; }
        public string LMP_PublisherName { get; set; }
        public string LMB_CallNo { get; set; }

    }
      public class LIB_Master_Book_vendorIMP
    {

        public long LMV_Id { get; set; }

        public string LMV_VendorName { get; set; }
        public string LMV_MobileNo { get; set; }
        public string LMV_PhoneNo { get; set; }
        public string LMV_EMailId { get; set; }
        public string LMV_Address { get; set; } 
        public string LMV_ActiveFlg { get; set; } 
        public string LMB_BookSubTitle { get; set; } 
    

    }

    public class LIB_Master_Book_AccnNoIMP
    {
        public long LMBANO_Id { get; set; }
        public long LMB_Id { get; set; }
        public string LMB_BookSubTitle { get; set; }
        public string LMBANO_AvialableStatus { get; set; }

        public string LMBANO_DeletedDate { get; set; }
        public string LMBANO_DeleteReason { get; set; }

        public string LMB_CallNo { get; set; }
        public string LMP_PublisherName { get; set; }

        public string LMBANO_ActiveFlg { get; set; }
        public string LMRA_Id { get; set; }
        public string LMBANO_AccessionNo { get; set; }
        public string Rack_Name { get; set; }

    }

    public class booktransIMP
    {
        public string adm_no { get; set; }
        public string Accession_No { get; set; }
        public string Booktittle { get; set; }
        public string LBTR_IssuedDate { get; set; }
        public string LBTR_Status { get; set; }
        public string LBTR_ReturnedDate { get; set; }
        

    }
}
