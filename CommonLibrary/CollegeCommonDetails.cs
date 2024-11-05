using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Fees;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class CollegeCommonDetails
    {
        public CollFeeGroupContext _db;
        public CollegeCommonDetails(CollFeeGroupContext db)
        {
            _db = db;
        }

        public Array Get_Yearly_Course(long MI_Id, long ASMAY_Id)
        {
            List<MasterCourseDMO> courselist = new List<MasterCourseDMO>();
            try
            {
                courselist = (from a in _db.MasterCourseDMO
                         from b in _db.CLG_Adm_College_AY_CourseDMO
                         where (a.MI_Id.Equals(MI_Id) && a.AMCO_ActiveFlag && b.MI_Id.Equals(MI_Id) && b.ASMAY_Id.Equals(ASMAY_Id) && b.ACAYC_ActiveFlag && b.AMCO_Id.Equals(a.AMCO_Id))
                         select a).Distinct().OrderBy(t => t.AMCO_Order).ToList();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return courselist.ToArray();
        }

        public Array Get_Yearly_Course_Branch(long MI_Id, long ASMAY_Id, long AMCO_Id)
        {
            List<ClgMasterBranchDMO> branchlist = new List<ClgMasterBranchDMO>();
            try
            {
                 branchlist = (from a in _db.ClgMasterBranchDMO
                                  from b in _db.CLG_Adm_College_AY_CourseDMO
                               from c in _db.CLG_Adm_College_AY_Course_BranchDMO
                                  where (a.MI_Id.Equals(MI_Id) && a.AMB_ActiveFlag && b.MI_Id.Equals(MI_Id) && b.ASMAY_Id.Equals(ASMAY_Id) && b.ACAYC_ActiveFlag && b.AMCO_Id.Equals(AMCO_Id) && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id.Equals(MI_Id) && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag)
                                  select new ClgMasterBranchDMO
                                  {
                                      AMB_Id = a.AMB_Id,
                                      AMB_BranchName = a.AMB_BranchName,
                                      AMB_BranchCode = a.AMB_BranchCode,
                                      AMB_BranchInfo = a.AMB_BranchInfo,
                                      AMB_BranchType = a.AMB_BranchType,
                                      AMB_StudentCapacity = a.AMB_StudentCapacity,
                                      AMB_Order = a.AMB_Order,
                                      AMB_AidedUnAided = a.AMB_AidedUnAided
                                  }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return branchlist.ToArray();
        }

        public Array Get_Yearly_Course_Branch_Semesters(long MI_Id, long ASMAY_Id, long AMCO_Id,long AMB_Id)
        {
            List<CLG_Adm_Master_SemesterDMO> semisterlist = new List<CLG_Adm_Master_SemesterDMO>();
            try
            {
                semisterlist = (from a in _db.CLG_Adm_Master_SemesterDMO
                                from b in _db.CLG_Adm_College_AY_CourseDMO
                                from c in _db.CLG_Adm_College_AY_Course_BranchDMO
                                from d in _db.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                where (a.MI_Id.Equals(MI_Id) && a.AMSE_ActiveFlg && b.MI_Id.Equals(MI_Id) && b.ASMAY_Id.Equals(ASMAY_Id) && b.ACAYC_ActiveFlag && b.AMCO_Id.Equals(AMCO_Id) && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id.Equals(MI_Id) && c.AMB_Id.Equals(AMB_Id) && c.ACAYCB_ActiveFlag && d.MI_Id.Equals(MI_Id) && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
                                select new CLG_Adm_Master_SemesterDMO
                                {
                                    AMSE_Id = a.AMSE_Id,
                                    AMSE_SEMName = a.AMSE_SEMName
                                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return semisterlist.ToArray();
        }

        public Array Get_Yearly_groups(long MI_Id, long ASMAY_Id)
        {
            List<FeeGroupClgDMO> grouplist = new List<FeeGroupClgDMO>();
            try
            {
                grouplist = (from a in _db.FeeGroupClgDMO
                             from b in _db.FeeYearGroupDMO
                             where (a.MI_Id.Equals(MI_Id) && b.MI_Id.Equals(MI_Id) && b.ASMAY_Id.Equals(ASMAY_Id) && b.FYG_ActiveFlag.Equals(true) && a.FMG_ActiceFlag.Equals(true))
                              select a).Distinct().OrderBy(t => t.FMG_GroupName).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return grouplist.ToArray();
        }


        public Array Get_All_Academicyear(long MI_Id)
        {
            List<MasterAcademic> academiclist = new List<MasterAcademic>();
            try
            {
                academiclist = (from a in _db.AcademicYear
                             where (a.MI_Id.Equals(MI_Id) && a.Is_Active.Equals(true))
                             select a).Distinct().OrderBy(t => t.ASMAY_Order).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return academiclist.ToArray();
        }

        public Array Get_Current_Academicyear(long MI_Id, long ASMAY_Id)
        {
            List<MasterAcademic> academiclist = new List<MasterAcademic>();
            try
            {
                academiclist = (from a in _db.AcademicYear
                                where (a.MI_Id.Equals(MI_Id) && a.Is_Active.Equals(true) && a.ASMAY_Id.Equals(ASMAY_Id))
                                select a).Distinct().OrderBy(t => t.ASMAY_Order).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return academiclist.ToArray();
        }

    }
}


