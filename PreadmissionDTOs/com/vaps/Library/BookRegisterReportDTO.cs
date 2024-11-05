using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
   public class BookRegisterReportDTO:CommonParamDTO
    {
        public long LMB_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long LMD_Id { get; set; }
        public string LMD_DepartmentName { get; set; }
        public string LMB_BookTitle { get; set; }
        public string LMB_BookType { get; set; }
        public Array booktype { get; set; }
        public Array deptlist { get; set; }
        public Array clsslist { get; set; }
        public Array reportlist { get; set; }
        public  string Type { get; set; }

        public string LMP_PublisherName { get; set; }
        public string LMBA_AuthorFirstName { get; set; }
        public string LMBA_AuthorMiddleName { get; set; }
        public string LMBA_AuthorLastName { get; set; }
        public string LML_LanguageName { get; set; }
        public string LMS_SubjectName { get; set; }
        public string LMB_ClassNo { get; set; }
        public string LMB_ISBNNo { get; set; }
        public DateTime? LMB_PurchaseDate { get; set; }
        public DateTime LMB_EntryDate { get; set; }
        public string LMB_PurOrDonated { get; set; }
        public string LMB_BillNo { get; set; }
        public Array Master_book { get; set; }
        public Array alldata { get; set; }
        public long LMAL_Id { get; set; }
        public DepartMent_List[] DepartMent_list { get; set; }

        public Book_List[] Book_List { get; set; }

    }
    public class DepartMent_List
    {
        public long LMD_Id { get; set; }
    }
    public class Book_List
    {
        public long LMB_Id { get; set; }
    }
}
