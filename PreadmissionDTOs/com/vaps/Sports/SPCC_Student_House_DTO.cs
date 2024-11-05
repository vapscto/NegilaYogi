using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Sports
{
   public class SPCC_Student_House_DTO:CommonParamDTO
    {
        #region old code
        //    public long SPCCSH_Id { get; set; }
        //    public long MI_Id { get; set; }
        //    public long ASMAY_Id { get; set; }
        //    public long ASMCL_Id { get; set; }
        //    public DateTime? SPCCSH_AsOnDate { get; set; }
        //    public long ASMS_Id { get; set; }
        //    public long SPCCMH_Id { get; set; }
        //    public long AMST_Id { get; set; }
        //    public string SPCCSH_Age { get; set; }
        //    public decimal? SPCCSH_Height { get; set; }
        //    public decimal? SPCCSH_Weight { get; set; }
        //    public bool SPCCMH_ActiveFlag { get; set; }
        //    public decimal? SPCCSH_BMI { get; set; }
        //    public string SPCCSH_BMIRemarks { get; set; }
        //    public Array yearlist { get; set; }
        //    public Array houseList { get; set; }
        //    public string SPCCMH_HouseName { get; set; }
        //    public Array classList { get; set; }
        //    public Array SectionList { get; set; }
        //    public Array StudentList { get; set; }
        //    public Array editrecord { get; set; }
        //    public string studentname { get; set; }
        //    public bool returnVal { get; set; }
        //    public string msg { get; set; }
        //    public Array masterhousename { get; set; }
        //    public string ASMCL_ClassName { get; set; }
        //    public string ASMC_SectionName { get; set; }
        //    public string AMST_AdmNo { get; set; }
        //    public Array studlist { get; set; }
        //    public Array studAge { get; set; }
        //    public Array alldata { get; set; }
        //    public int count { get; set; }
        //    public int count1 { get; set; }
        //    public studList1[] studList1 { get; set; }       

        //}
        //public class studList1
        //{
        //    public long amsT_Id { get; set; }       
        //    public decimal? height { get; set; }
        //    public decimal? weight { get; set; }
        //    public decimal? spccmhD_BMI { get; set; }
        //    public string spccmhD_BMI_Remark { get; set; }

        //}
        #endregion old code close


        public long SPCCSH_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long SPCCMH_Id { get; set; }
        public long AMST_Id { get; set; }
        public bool SPCCMH_ActiveFlag { get; set; }
        public Array yearlist { get; set; }
        public Array houseList { get; set; }
        public string SPCCMH_HouseName { get; set; }
        public Array classList { get; set; }
        public Array SectionList { get; set; }
        public Array StudentList { get; set; }
        public Array editrecord { get; set; }
        public string studentname { get; set; }
        public bool returnVal { get; set; }
        public string msg { get; set; }
        public Array masterhousename { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string AMST_AdmNo { get; set; }
        public Array studlist { get; set; }
        public Array alldata { get; set; }
        public int count { get; set; }
        public int count1 { get; set; }
        public string SPCCSH_Remarks { get; set; }
        public string ASMAY_Year { get; set; }
        public studList1[] studList1 { get; set; }
        public DateTime? SPCCSH_Date { get; set; }
        public string SPCCSH_Age { get; set; }
        public string SPCCSH_Age_Format { get; set; }
        public Array agelist { get; set; }


    }
    public class studList1
    {
        public long AMST_Id { get; set; }
    }



}
