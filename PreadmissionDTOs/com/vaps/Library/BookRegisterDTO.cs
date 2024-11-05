using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
    
   public class BookRegisterDTO:CommonParamDTO
    {
        public long LMB_Id { get; set; }
        public long MI_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public long LMBA_Id { get; set; }
        public long LMS_Id { get; set; }
        public long LMD_Id { get; set; }
        public long? LMP_Id { get; set; }
        public long? LML_Id { get; set; }
        public long MB_Pages { get; set; }
        public string Donor_Id { get; set; }
        public long LMV_Id { get; set; }
        public long? LMB_NoOfCopies { get; set; }
        public long LMC_Id { get; set; }
        public long LMBANO_Id { get; set; }
        public string MB_Call_No { get; set; }
        public string LMB_ClassNo { get; set; }
        public string LMB_BookTitle { get; set; }
        public string MB_Subtitle { get; set; }
        public string LMB_BookType { get; set; }
       
        public string LMB_ISBNNo { get; set; }
      
        public decimal LMB_Price { get; set; }
        public decimal? LMB_Discount { get; set; }
        public string MB_Disc_Type { get; set; }
        public decimal LMB_NetPrice { get; set; }
        public string LMB_VolNo { get; set; }
        public string Binding_Type { get; set; }
        public DateTime? Purchase_Date { get; set; }
        public DateTime LMB_EntryDate { get; set; }
        public string Bibliography_Page { get; set; }
        public string Index_Page { get; set; }
        public string LMB_BillNo { get; set; }
        public string Voucher_No { get; set; }
      
        public string LMB_PublishedYear { get; set; }
        public string LMB_Edition { get; set; }
        public string Source_Type { get; set; }
     
        public string MB_Remarks { get; set; }
    
       // public long Rack_Id { get; set; }
        public string LMBANO_AccessionNo { get; set; }
        public string MB_Keywords { get; set; }
        public bool New_Arrival { get; set; }
        public string Book_Image { get; set; }
        public bool With_Accessories { get; set; }
      
        public string CurrencyType { get; set; }
     
        public string Invoice_No { get; set; }
        public bool MB_ActiveFlag { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array subjectlist { get; set; }
        public Array deptlist { get; set; }
        public Array racklist { get; set; }
        public Array langlist { get; set; }
        public Array donorlist { get; set; }
        public Array vendorlist { get; set; }
        public Array publisherlst { get; set; }
        public Array authorlst { get; set; }
        public string LMS_SubjectName { get; set; }
        public string LMD_DepartmentName { get; set; }
        public string Rack_Name { get; set; }
        public string LML_LanguageName { get; set; }
        public string Donor_Name { get; set; }
        public string LMV_VendorName { get; set; }
        public string LMP_PublisherName { get; set; }
        public string LMBA_AuthorFirstName { get; set; }
        public string LMBA_AuthorMiddleName { get; set; }
        public string LMBA_AuthorLastName { get; set; }
        public Array alldata { get; set; }
        public Array editlis { get; set; }
       

        public string ASMCL_ClassName { get; set; }
      //  public long ASMCL_Id { get; set; }
        public Array classlist { get; set; }
       
        public Array categorylist { get; set; }
        public string LMC_CategoryName { get; set; }
        public string Book_Avail_Status { get; set; }
        public DateTime? Delete_Date { get; set; }
        public string Delete_Reason { get; set; }
      //  public long Login_Id { get; set; }
        public decimal? Book_NumericPart { get; set; }
        public string Book_Prefix { get; set; }
        public string Book_Suffix { get; set; }

        public string LMBANO_No { get; set; }

        public BookRegisterDTO[] savetmpdata { get; set; }
        public string LMBANO_AvialableStatus { get; set; }
        public string LMB_PurOrDonated { get; set; }
        public string LMB_DonorAddress { get; set; }
        public string LMB_BookNo { get; set; }

        public long LMBL_Id { get; set; }
        public long? LMAC_Id { get; set; }
        public long LMRA_Id { get; set; }
        public Array accessorieslist { get; set; }
        public long LMAL_Id { get; set; }

        public Array librarylist { get; set; }
        public bool chkaccessionno { get; set; }
        public string LMRA_RackName { get; set; }

        public long UserId { get; set; }
        public long LMBKF_Id { get; set; }
        public string LMBKF_KeyFactor { get; set; }
        public int? LMBKF_PageNo { get; set; }
        public bool LMBKF_ActiveFlg { get; set; }
        public string LMAU_AuthorFirstName { get; set; }
        public long? LMAU_Id { get; set; }
        public BookFiles[] BookFilesPdf { get; set; }
        public Array BookFilesPdfEdit { get; set; }
        public  bool AuthFlag { get; set; }
        public long? LMP_MobileNo { get; set; }
        public string LMP_PhoneNo { get; set; }
        public string LMP_EMailId { get; set; }
        public string LMP_Address { get; set; }

    }
    public class BookFiles
    {
        public long LMBFILE_Id { get; set; }
        public long LMB_Id { get; set; }

        public string LMBFILE_FileName { get; set; }
        public string LMBFILE_FilePath { get; set; }

    }
}
