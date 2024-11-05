
using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class CollegeDocumentReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long AMSMD_Id { get; set; }
        public long AMCST_Id { get; set; }
        public string studentname { get; set; }
        public string STDORDOC { get; set; }
        public Array getyear { get; set; }
        public Array getcourse { get; set; }
        public Array getbranch { get; set; }
        public Array getsemester { get; set; }
        public Array getsection { get; set; }
        public Array getdocument { get; set; }
        public Array getreport { get; set; }
        public Array getstudent { get; set; }

        public courselsttwo1[] courselsttwo { get; set; }

        public branchlisttwo1[] branchlisttwo { get; set; }

        public semesterlisttwo1[] semesterlisttwo { get; set; }
        public Array admissioncatdrp { get; set; }

        public Array admissioncatdrpall { get; set; }
        public int configurationsettings1 { get; set; }
        public Array registrationList { get; set; }

        public CollegeDocumentReportDTO[] ddoc { get; set; }

        public long ACSTD_Id { get; set; }
        public string ACSTD_Doc_Path { get; set; }
        public string Document_Path { get; set; }
        public string AMSMD_DocumentName { get; set; }
        public long ACSMD_Id { get; set; }

        public string ACSTD_Doc_Name { get; set; }

    }

    public class courselsttwo1
    {
        public long AMCO_Id { get; set; }


    }
    public class branchlisttwo1
    {
        public long AMB_Id { get; set; }


    }
    public class semesterlisttwo1
    {
        public long AMSE_Id { get; set; }


    }

}
