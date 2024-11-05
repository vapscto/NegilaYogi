using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class ClgyearlycoursemappingDTO
    {
        public long MI_Id { get; set; }
        public Array courselist { get; set; }
        public Array semesterlistget { get; set; }
        public Array branchlist { get; set; }
        public Array semesterlist { get; set; }
        public Array accyearlist { get; set; }
        public Array getcourselist { get; set; }
        public Array getsaveddata { get; set; }
      //  public temp_year_course_DTO[] temp_year_course_DTO { get; set; }
      //  public temp_course_branch_DTO[] temp_course_branch_DTO { get; set; }
        public temp_sem_branch_DTO[] temp_sem_branch_DTO { get; set; }
        public long AMB_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long ACAYC_Id { get; set; }
        public long ACAYCB_Id { get; set; }
        public DateTime ACAYC_From_Date { get; set; }
        public DateTime ACAYC_To_Date { get; set; }
        public int ACAYC_NoOfSEM { get; set; }
        public string AMB_BranchName { get; set; }
        public long AMSE_Id { get; set; }
        public String AMSE_SEMName { get; set; }
        public int noofsem { get; set; }
        public Array branchlistdate { get; set; }
        public DateTime? ACAYCB_PreAdm_FDate { get; set; }
        public DateTime? ACAYCB_PreAdm_TDate { get; set; }
        public DateTime? ACAYCBS_SemStartDate { get; set; }
        public DateTime? ACAYCBS_SemEndDate { get; set; }
        public DateTime? ACAYB_ReferenceDate { get; set; }
        public int ACAYCBS_SemOrder { get; set; }
        public bool returnval { get; set; }
        public string ASMAY_Year { get; set; }
        public string AMCO_CourseName { get; set; }       
        public int ASMAY_Order { get; set; }       
        public int AMCO_Order { get; set; }
        public int AMB_Order { get; set; }
        public int AMSE_SEMOrder { get; set; }
        public Array getviewdetails { get; set; }
    }

    //public class temp_year_course_DTO
    //{
    //    public long ACAYC_Id { get; set; }
    //    public long ASMAY_Id { get; set; }
    //    public long AMCO_Id { get; set; }
    //    public DateTime ACAYC_From_Date { get; set; }
    //    public DateTime ACAYC_To_Date { get; set; }
    //    public int ACAYC_NoOfSEM { get; set; }
    //    public bool ACAYC_ActiveFlag { get; set; }
    //}
    //public class temp_course_branch_DTO
    //{
    //    public long ACAYCB_Id { get; set; }
    //    public long AMB_Id { get; set; }
    //    public DateTime? ACAYCB_PreAdm_FDate { get; set; }
    //    public DateTime? ACAYCB_PreAdm_TDate { get; set; }
    //    public DateTime? ACAYB_ReferenceDate { get; set; }
    //    public bool ACAYCB_ActiveFlag { get; set; }
    //}

    public class temp_sem_branch_DTO
    {
      //  public long ACAYCBS_Id { get; set; }
        public long AMSE_Id { get; set; }
        public DateTime ACAYCBS_SemStartDate { get; set; }
        public DateTime ACAYCBS_SemEndDate { get; set; }
        public int ACAYCBS_SemOrder { get; set; }
      //  public bool ACAYCBS_ActiveFlag { get; set; }
    }
}
