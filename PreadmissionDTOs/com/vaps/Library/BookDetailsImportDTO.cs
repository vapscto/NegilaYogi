using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
   public class BookDetailsImportDTO
    {
        public long LMB_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long LMB_NoOfCopies { get; set; }
        public long ForTheClass { get; set; }


        public long LMBA_Id { get; set; }
        public long LMS_Id { get; set; }
        public long LMD_Id { get; set; }
        public long LMP_Id { get; set; }
        public long LML_Id { get; set; }
        public long Donor_Id { get; set; }
        public long LMV_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long Rack_Id { get; set; }
        public long MB_Pages { get; set; }
        public long LMC_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long LMBANO_Id { get; set; }


        public string LMB_ISBNNo { get; set; }
        public decimal? LMB_NetPrice { get; set; }
        public string LMB_BookTitle { get; set; }
        public string LMB_BillNo { get; set; }
        public string LMB_PublishedYear { get; set; }
        public string LMB_Edition { get; set; }
        public string LMBANO_AccessionNo { get; set; }
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
        public string ASMCL_ClassName { get; set; }
        public bool? With_Accessories { get; set; }
        public bool? MB_ActiveFlag { get; set; }
        public bool? LMBANO_ActiveFlg { get; set; }
        public string LMC_CategoryName { get; set; }
        public BookRegisterDTO[] newlstget1 { get; set; }
        public string returnMsg { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string book_msg_type { get; set; }
        public Array failedlist { get; set; }

    }
}
