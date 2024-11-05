using PreadmissionDTOs.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
    public class Lib_stu_punch_reportDTO
    {

        public FO_Emp_PunchDTO[] temp1 { get; set; }
        public long MI_Id { get; set; }
        public string rolename { get; set; }
        public string UserName { get; set; }
        public long Userid { get; set; }
        public string flag { get; set; }
        public long Roleid { get; set; }
        public long Emp_Code { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMS_Id { get; set; }
        public string studentname { get; set; }
        public bool returnval { get; set; }
        public Array getyearlist { get; set; }
        public Array getclasslist { get; set; }
        public Array getsectionlist { get; set; }
        public Array getstudentlist { get; set; }
        public Array biometricdevice { get; set; }
        public Array getstupunchreport { get; set; }
        public Array columnnames { get; set; }
        public Array clg_academicyear { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array semisterlist { get; set; }
        public Array getsection { get; set; }
        public Array studentlist { get; set; }
        public Array clgstupunchreport { get; set; }
        public string biometricname { get; set; }
        public string classname { get; set; }
        public long ASMCLId { get; set; }
        public int FOBD_Id { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public Temp_AmstIds[] Temp_AmstIds { get; set; }
        public long AMCO_Id { get; set; }
        public string AMCO_CourseName { get; set; }
        public int AMCO_Order { get; set; }
        public long ACMS_Id { get; set; }
        public long AMB_Id { get; set; }
        public int AMB_Order { get; set; }
        public string AMB_BranchName { get; set; }

        public long AMSE_Id { get; set; }
        public string AMSE_SEMName { get; set; }
        public int AMSE_SEMOrder { get; set; }

        public long AMCST_Id { get; set; }
        public string AMCST_Name { get; set; }

        public Temp_AmcstIds[] Temp_AmcstIds { get; set; }

    }
    public class Temp_AmcstIds
    {
        public long AMCST_Id { get; set; }
    }
}
