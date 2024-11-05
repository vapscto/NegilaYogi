using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Dynamic;
using System.Text.RegularExpressions;
using CommonLibrary;
using CollegeServiceHub.Interface;
using DomainModel.Model.com.vapstech.College.Admission;
using PreadmissionDTOs.com.vaps.College.Admission;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;

namespace CollegeServiceHub.Impl
{
    public class ClgSectionAllotmentImpl : Interface.ClgSectionAllotmentInterface
    {

        public ClgAdmissionContext _Context;
        public ClgSectionAllotmentImpl(ClgAdmissionContext Admissiondbcontext)
        {
            _Context = Admissiondbcontext;
        }
        public ClgYearWiseStudentDTO GetDropDownList(ClgYearWiseStudentDTO stu)
        {
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _Context.AcademicYear.Where(t => t.MI_Id == stu.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();

                stu.YearList = year.ToArray();

                List<CLG_Adm_Master_SemesterDMO> semlist = new List<CLG_Adm_Master_SemesterDMO>();
                semlist = _Context.CLG_Adm_Master_SemesterDMO.Where(t => t.MI_Id == stu.MI_Id && t.AMSE_ActiveFlg == true).OrderBy(d => d.AMSE_SEMOrder).ToList();
                stu.semlist = semlist.ToArray();


                List<Adm_College_Master_SectionDMO> School_M_SectionList = new List<Adm_College_Master_SectionDMO>();
                School_M_SectionList = _Context.Adm_College_Master_SectionDMO.Where(t => t.MI_Id == stu.MI_Id && t.ACMS_ActiveFlag == true).OrderBy(d => d.ACMS_Order).ToList();
                stu.sectionList = School_M_SectionList.ToArray();

                List<MasterCourseDMO> courselist = new List<MasterCourseDMO>();
                courselist = _Context.MasterCourseDMO.Where(t => t.MI_Id == stu.MI_Id && t.AMCO_ActiveFlag == true).OrderBy(d => d.AMCO_Order).ToList();
                stu.courselist = courselist.ToArray();

                stu.coursebranchlist = _Context.ClgMasterBranchDMO.Where(a => a.MI_Id == stu.MI_Id && a.AMB_ActiveFlag).Distinct().OrderBy(a => a.AMB_Order).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stu;
        }
        public ClgYearWiseStudentDTO Get_academiccourse(ClgYearWiseStudentDTO data)
        {
            try
            {
                data.courselist = (from a in _Context.MasterCourseDMO
                                   from b in _Context.CLG_Adm_College_AY_CourseDMO
                                   where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id 
                                   && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                                   select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }
        public ClgYearWiseStudentDTO Get_semister(ClgYearWiseStudentDTO data)
        {
            try
            {
                data.semlist = (from a in _Context.CLG_Adm_Master_SemesterDMO
                                from b in _Context.CLG_Adm_College_AY_CourseDMO
                                from c in _Context.CLG_Adm_College_AY_Course_BranchDMO
                                from d in _Context.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag 
                                && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag 
                                && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
                                select new CLG_Adm_Master_SemesterDMO
                                {
                                    AMSE_Id = a.AMSE_Id,
                                    AMSE_SEMName = a.AMSE_SEMName,
                                    AMSE_EvenOdd = a.AMSE_EvenOdd
                                }).Distinct().ToArray();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }
        public ClgYearWiseStudentDTO Getbranch(ClgYearWiseStudentDTO data)
        {
            try
            {

                data.coursebranchlist = (from a in _Context.ClgMasterBranchDMO
                                         from b in _Context.CLG_Adm_College_AY_CourseDMO
                                         from c in _Context.CLG_Adm_College_AY_Course_BranchDMO
                                         where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id 
                                         && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id 
                                         && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag)
                                         select new ClgMasterBranchDMO
                                         {
                                             AMB_Id = a.AMB_Id,
                                             AMB_BranchName = a.AMB_BranchName,
                                         }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }
        public ClgYearWiseStudentDTO GetPromocourse(ClgYearWiseStudentDTO data)
        {
            try
            {
                data.courselist = (from a in _Context.MasterCourseDMO
                                   from b in _Context.CLG_Adm_College_AY_CourseDMO
                                   where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.AMCO_Id == b.AMCO_Id && a.AMCO_ActiveFlag == true 
                                   && b.ACAYC_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id)
                                   select new ClgYearWiseStudentDTO
                                   {
                                       AMCO_Id = b.AMCO_Id,
                                       AMCO_CourseName = a.AMCO_CourseName,
                                   }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }
        public ClgYearWiseStudentDTO GetPromobranch(ClgYearWiseStudentDTO data)
        {
            try
            {
                data.coursebranchlist = (from a in _Context.ClgMasterBranchDMO
                                         from b in _Context.CLG_Adm_College_AY_Course_BranchDMO
                                         from c in _Context.CLG_Adm_College_AY_CourseDMO
                                         where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.AMB_Id == b.AMB_Id && a.AMB_ActiveFlag == true 
                                         && b.ACAYCB_ActiveFlag == true && b.ACAYC_Id == c.ACAYC_Id && c.AMCO_Id == data.AMCO_Id && c.ASMAY_Id == data.ASMAY_Id)
                                         select new ClgYearWiseStudentDTO
                                         {
                                             AMB_Id = b.AMB_Id,
                                             AMB_BranchName = a.AMB_BranchName
                                         }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }
        public ClgYearWiseStudentDTO GetPromosem(ClgYearWiseStudentDTO data)
        {
            try
            {
                data.semlist = (from a in _Context.CLG_Adm_Master_SemesterDMO
                                from b in _Context.CLG_Adm_College_AY_Course_BranchDMO
                                from c in _Context.CLG_Adm_College_AY_CourseDMO
                                from d in _Context.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.MI_Id == data.MI_Id && a.AMSE_Id == d.AMSE_Id 
                                && a.AMSE_ActiveFlg == true && b.ACAYCB_ActiveFlag == true && b.ACAYC_Id == c.ACAYC_Id && c.AMCO_Id == data.AMCO_Id
                                && b.ACAYCB_Id == d.ACAYCB_Id && d.ACAYCBS_ActiveFlag == true && c.ASMAY_Id == data.ASMAY_Id && b.AMB_Id == data.AMB_Id)
                                select new ClgYearWiseStudentDTO
                                {
                                    AMSE_Id = a.AMSE_Id,
                                    AMSE_SEMName = a.AMSE_SEMName,
                                }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;

        }
        public ClgYearWiseStudentDTO promsemonchange(ClgYearWiseStudentDTO data)
        {
            try
            {
                var check_order_left = _Context.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_Id == data.AMSE_Id).ToList();
                var semorder_left = check_order_left.FirstOrDefault().AMSE_SEMOrder;
                var semevenorder_left = check_order_left.FirstOrDefault().AMSE_EvenOdd;
                var semyear_left = check_order_left.FirstOrDefault().AMSE_Year;
                int orderright = semorder_left + 1;

                var check_year_left = _Context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id).ToList();
                var yearorder_left = check_year_left.FirstOrDefault().ASMAY_Order;
                int yearorderright = yearorder_left + 1;

                var check_order_rigth = _Context.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_SEMOrder == orderright).ToList();
                var semorder_right = check_order_rigth.FirstOrDefault().AMSE_SEMOrder;
                var semevenorder_right = check_order_rigth.FirstOrDefault().AMSE_EvenOdd;
                var semyear_right = check_order_rigth.FirstOrDefault().AMSE_Year;

                if (semyear_left != semyear_right)
                {
                    var accyear_right = _Context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Order == yearorderright).ToList();
                    data.promoyear = accyear_right.ToArray();

                    var sem_right = _Context.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_SEMOrder == orderright).ToList();
                    data.prosemlist = sem_right.ToArray();

                    data.promotedflag = "nextyear";

                }
                else
                {
                    var accyear_right = _Context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Order == yearorder_left && a.ASMAY_Id == data.ASMAY_Id).ToList();
                    data.promoyear = accyear_right.ToArray();

                    var sem_right = _Context.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_SEMOrder == orderright).ToList();
                    data.prosemlist = sem_right.ToArray();

                    data.promotedflag = "sameyear";

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;

        }
        //student details by year
        public ClgYearWiseStudentDTO GetStudentListByYear(long id)
        {
            ClgYearWiseStudentDTO stu = new ClgYearWiseStudentDTO();
            try
            {
                stu.StudentList = (from s in _Context.Adm_Master_College_StudentDMO
                                   where !(from ys in _Context.Adm_College_Yearly_StudentDMO
                                           where ys.ACYST_ActiveFlag == 0
                                           select ys.AMCST_Id).Contains(s.AMCST_Id) && s.ASMAY_Id == id
                                   select s).ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stu;
        }
        public ClgYearWiseStudentDTO saveSctionAllotment(ClgYearWiseStudentDTO secAllt)
        {
            try
            {
                int MaxCapacity = _Context.Adm_College_Master_SectionDMO.SingleOrDefault(s => s.ACMS_Id == secAllt.ACMS_Id).ACMS_MaxCapacity;

                if (MaxCapacity == 0)
                {
                    secAllt.returnMsg = "Selected Section Contains Zero Maximum Capacity.Please Contact Administrator";
                    return secAllt;
                }
                var count = (from m in _Context.Adm_Master_College_StudentDMO
                             from n in _Context.Adm_College_Yearly_StudentDMO
                             where (m.AMCST_Id == n.AMCST_Id && m.MI_Id == secAllt.MI_Id)
                             select new ClgYearWiseStudentDTO
                             {
                                 AMCST_Id = n.AMCST_Id
                             }).ToList();
                //add Student Section details
                if (secAllt.SectionAllotmentType == "New")
                {
                    try
                    {
                        if (secAllt.resultData1.Count() > 0)
                        {
                            foreach (ClgYearWiseStudentDTO ph in secAllt.resultData1)
                            {
                                var createdcount = _Context.Adm_College_Yearly_StudentDMO.Where(t => t.ACYST_ActiveFlag.Equals(true) && t.AMB_Id == secAllt.AMB_Id && t.ACMS_Id == secAllt.ACMS_Id && t.ASMAY_Id == secAllt.ASMAY_Id && t.AMCO_Id == secAllt.AMCO_Id && t.AMSE_Id == secAllt.AMSE_Id).ToList();

                                if (createdcount.Count < MaxCapacity)
                                {
                                    if (count.Count > 0)
                                    {
                                        var result = (from m in _Context.Adm_Master_College_StudentDMO
                                                      from n in _Context.Adm_College_Yearly_StudentDMO
                                                      where (m.AMCST_Id == n.AMCST_Id && m.MI_Id == secAllt.MI_Id && n.ASMAY_Id == secAllt.ASMAY_Id
                                                      && n.ACMS_Id == secAllt.ACMS_Id && n.AMB_Id == secAllt.AMB_Id && n.AMCO_Id == secAllt.AMCO_Id && n.AMSE_Id == secAllt.AMSE_Id)
                                                      select new ClgYearWiseStudentDTO
                                                      {
                                                          AMCST_Id = n.AMCST_Id,
                                                          ACYST_RollNo = n.ACYST_RollNo
                                                      }).ToList();
                                        if (result.Count > 0)
                                        {
                                            var rollNo = result.OrderByDescending(e => e.ACYST_RollNo).First().ACYST_RollNo;
                                            secAllt.ACYST_RollNo = rollNo + 1;
                                        }
                                        else
                                        {
                                            secAllt.ACYST_RollNo = 1;
                                        }

                                    }
                                    else
                                    {
                                        secAllt.ACYST_RollNo = 1;
                                    }

                                    Adm_College_Yearly_StudentDMO sct = Mapper.Map<Adm_College_Yearly_StudentDMO>(ph);
                                    sct.ASMAY_Id = secAllt.ASMAY_Id;
                                    sct.AMB_Id = secAllt.AMB_Id;
                                    sct.AMSE_Id = secAllt.AMSE_Id;
                                    sct.AMCO_Id = secAllt.AMCO_Id;
                                    sct.ACMS_Id = secAllt.ACMS_Id;

                                    sct.ACYST_RollNo = secAllt.ACYST_RollNo;
                                    sct.ACYST_ActiveFlag = 1;
                                    sct.LoginId = secAllt.LoginId;
                                    sct.ACYST_DateTime = DateTime.Now;
                                    sct.CreatedDate = DateTime.Now;
                                    sct.UpdatedDate = DateTime.Now;
                                    _Context.Add(sct);
                                    var flag = _Context.SaveChanges();
                                    if (flag > 0)
                                    {
                                        secAllt.returnVal = true;
                                    }
                                    else
                                    {
                                        secAllt.returnVal = false;
                                    }
                                }
                                else
                                {
                                    secAllt.returnMsg = "Maximum limit for this section is exceeded.";
                                    return secAllt;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                // Change Section details
                if (secAllt.SectionAllotmentType == "Change")
                {
                    try
                    {
                        if (secAllt.resultData1.Count() > 0)
                        {
                            foreach (ClgYearWiseStudentDTO ph in secAllt.resultData1)
                            {
                                var createdcount = _Context.Adm_College_Yearly_StudentDMO.Where(t => t.ACYST_ActiveFlag.Equals(true) && t.AMB_Id == secAllt.AMB_Id && t.AMCO_Id == secAllt.AMCO_Id && t.AMSE_Id == secAllt.AMSE_Id && t.ACMS_Id == secAllt.ACMS_Id && t.ASMAY_Id == secAllt.ASMAY_Id).ToList();

                                if (createdcount.Count < MaxCapacity)
                                {
                                    if (count.Count > 0)
                                    {
                                        var result11 = (from m in _Context.Adm_Master_College_StudentDMO
                                                        from n in _Context.Adm_College_Yearly_StudentDMO
                                                        where (m.AMCST_Id == n.AMCST_Id && m.MI_Id == secAllt.MI_Id && n.ASMAY_Id == secAllt.ASMAY_Id
                                                        && n.AMB_Id == secAllt.AMB_Id && n.AMCO_Id == secAllt.AMCO_Id
                                                        && n.AMSE_Id == secAllt.AMSE_Id
                                                        && n.ACMS_Id == secAllt.ACMS_Id)
                                                        select new ClgYearWiseStudentDTO
                                                        {
                                                            AMCST_Id = n.AMCST_Id,
                                                            ACYST_RollNo = n.ACYST_RollNo
                                                        }).ToList();
                                        if (result11.Count > 0)
                                        {
                                            var rollNo = result11.OrderByDescending(e => e.ACYST_RollNo).First().ACYST_RollNo;
                                            secAllt.ACYST_RollNo = rollNo + 1;
                                        }
                                        else
                                        {
                                            secAllt.ACYST_RollNo = 1;
                                        }

                                    }
                                    else
                                    {
                                        secAllt.ACYST_RollNo = 1;
                                    }
                                    Adm_College_Yearly_StudentDMO sct = Mapper.Map<Adm_College_Yearly_StudentDMO>(ph);
                                    var result = _Context.Adm_College_Yearly_StudentDMO.SingleOrDefault(t => t.ACYST_Id == sct.ACYST_Id);
                                    result.ACMS_Id = secAllt.ACMS_Id;
                                    result.ACYST_RollNo = secAllt.ACYST_RollNo;
                                    result.CreatedDate = result.CreatedDate;
                                    result.LoginId = secAllt.LoginId;                                   
                                    _Context.Update(result);
                                    var flag = _Context.SaveChanges();
                                    if (flag > 0)
                                    {
                                        secAllt.returnVal = true;                                       
                                    }
                                    else
                                    {
                                        secAllt.returnVal = false;
                                    }
                                }
                                else
                                {
                                    secAllt.returnMsg = "Maximum limit for this section is exceeded.";
                                    return secAllt;
                                }                               
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                // delete  Section details

                if (secAllt.SectionAllotmentType == "Delete")
                {
                    try
                    {
                        if (secAllt.resultData1.Count() > 0)
                        {
                            foreach (ClgYearWiseStudentDTO ph in secAllt.resultData1)
                            {                                 
                                Adm_College_Yearly_StudentDMO sct = Mapper.Map<Adm_College_Yearly_StudentDMO>(ph);

                                var result = _Context.Adm_College_Yearly_StudentDMO.Single(t => t.ACYST_Id == sct.ACYST_Id);                                
                                _Context.Remove(result);
                                var flag = _Context.SaveChanges();
                                if (flag > 0)
                                {
                                    secAllt.returnVal = true;
                                }
                                else
                                {
                                    secAllt.returnVal = false;
                                }                                
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                //add Student Section details
                if (secAllt.SectionAllotmentType == "Promotion")
                {
                    try
                    {
                        if (secAllt.resultData1.Count() > 0)
                        {
                            foreach (ClgYearWiseStudentDTO ph in secAllt.resultData1)
                            {
                                //var createdcount = _Context.SchoolYearWiseStudent.Where(t => t.AMAY_ActiveFlag == 1 && t.ASMCL_Id == secAllt.ASMCL_Id && t.ASMS_Id == secAllt.ASMS_Id && t.ASMAY_Id == secAllt.ASMAY_Id).ToList();

                                var createdcount = _Context.Adm_College_Yearly_StudentDMO.Where(t => t.ACYST_ActiveFlag.Equals(true) && t.AMB_Id == secAllt.AMB_Id && t.AMCO_Id == secAllt.AMCO_Id && t.AMSE_Id == secAllt.AMSE_Id && t.ACMS_Id == secAllt.ACMS_Id && t.ASMAY_Id == secAllt.ASMAY_Id).ToList();


                                if (createdcount.Count < MaxCapacity)
                                {
                                    Adm_College_Yearly_StudentDMO sct = Mapper.Map<Adm_College_Yearly_StudentDMO>(ph);
                                    if (sct.ACYST_Id > 0)
                                    {
                                        var result = _Context.Adm_College_Yearly_StudentDMO.Single(t => t.ACYST_Id == ph.ACYST_Id);
                                        result.ACYST_ActiveFlag = 1;
                                        result.UpdatedDate = DateTime.Now;
                                        result.LoginId = secAllt.LoginId;
                                        _Context.Update(result);
                                        _Context.SaveChanges();
                                    }
                                    if (count.Count > 0)
                                    {
                                        var result = (from m in _Context.Adm_Master_College_StudentDMO
                                                      from n in _Context.Adm_College_Yearly_StudentDMO
                                                      where (m.AMCST_Id == n.AMCST_Id && m.MI_Id == secAllt.MI_Id && n.ASMAY_Id == secAllt.ASMAY_Id
                                                      && n.AMB_Id == secAllt.AMB_Id && n.AMCO_Id == secAllt.AMCO_Id
                                                      && n.AMSE_Id == secAllt.AMSE_Id
                                                      && n.ACMS_Id == secAllt.ACMS_Id)
                                                      select new ClgYearWiseStudentDTO
                                                      {
                                                          AMCST_Id = n.AMCST_Id,
                                                          ACYST_RollNo = n.ACYST_RollNo
                                                      }).ToList();

                                        if (result.Count > 0)
                                        {
                                            var rollNo = result.OrderByDescending(e => e.ACYST_RollNo).First().ACYST_RollNo;
                                            secAllt.ACYST_RollNo = rollNo + 1;
                                        }
                                        else
                                        {
                                            secAllt.ACYST_RollNo = 1;
                                        }

                                    }
                                    else
                                    {
                                        secAllt.ACYST_RollNo = 1;
                                    }

                                    if (secAllt.promotedflag == "sameyear")
                                    {
                                        var checkstd = _Context.Adm_College_Yearly_StudentDMO.Where(a => a.AMCST_Id == sct.AMCST_Id && a.ASMAY_Id == secAllt.ASMAY_Id && a.ACYST_ActiveFlag == 1).ToArray();

                                        if (checkstd.Count() > 0)
                                        {
                                            var checkstd_new = _Context.Adm_College_Yearly_StudentDMO.Single(a => a.AMCST_Id == sct.AMCST_Id && a.ASMAY_Id == secAllt.ASMAY_Id && a.ACYST_ActiveFlag == 1);

                                            checkstd_new.ACYST_ActiveFlag = 0;
                                            checkstd_new.UpdatedDate = DateTime.Now;
                                            _Context.Update(checkstd_new);
                                        }
                                    }

                                    Adm_College_Yearly_StudentDMO sct1 = new Adm_College_Yearly_StudentDMO();                                   
                                    sct1.AMCST_Id = sct.AMCST_Id;
                                    sct1.ASMAY_Id = secAllt.ASMAY_Id;
                                    sct1.AMB_Id = secAllt.AMB_Id;
                                    sct1.AMSE_Id = secAllt.AMSE_Id;
                                    sct1.AMCO_Id = secAllt.AMCO_Id;
                                    sct1.ACMS_Id = secAllt.ACMS_Id;

                                    sct1.ACYST_RollNo = secAllt.ACYST_RollNo;
                                    sct1.ACYST_ActiveFlag = 1;
                                    sct1.LoginId = secAllt.LoginId;
                                    sct1.ACYST_DateTime = DateTime.Now;
                                    sct1.CreatedDate = DateTime.Now;
                                    sct1.UpdatedDate = DateTime.Now;
                                    _Context.Add(sct1);

                                    var flag = _Context.SaveChanges();
                                    if (flag > 0)
                                    {
                                        secAllt.returnVal = true;
                                        if (secAllt.promotedflag != "sameyear")
                                        {
                                            try
                                            {
                                                var confirmstatus = _Context.Database.ExecuteSqlCommand("Batchwise_Student_feeGroup_Mapping @p0,@p1,@p2,@p3",
                                               secAllt.MI_Id, sct.AMCST_Id, secAllt.ASMAY_Id, secAllt.AMSE_Id);
                                            }
                                            catch(Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        secAllt.returnVal = false;
                                    }
                                }
                                else
                                {
                                    secAllt.returnMsg = "Maximum limit for this section is exceeded.";
                                    return secAllt;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                //add Student Section details
                if (secAllt.SectionAllotmentType == "YearLoss")
                {
                    try
                    {
                        if (secAllt.resultData1.Count() > 0)
                        {
                            foreach (ClgYearWiseStudentDTO ph in secAllt.resultData1)
                            {
                                var createdcount = _Context.Adm_College_Yearly_StudentDMO.Where(t => t.ACYST_ActiveFlag.Equals(1) && t.AMCO_Id == secAllt.AMCO_Id
                                && t.ACMS_Id == secAllt.ACMS_Id && t.ASMAY_Id == secAllt.ASMAY_Id && t.AMB_Id == secAllt.AMB_Id && t.AMSE_Id == secAllt.AMSE_Id).ToList();

                                var checkid = _Context.Adm_College_Yearly_StudentDMO.Where(t => t.ACYST_ActiveFlag.Equals(1) && t.AMCO_Id == secAllt.AMCO_Id
                                && t.ACMS_Id == secAllt.ACMS_Id && t.ASMAY_Id == secAllt.ASMAY_Id && t.AMB_Id == secAllt.AMB_Id && t.AMSE_Id == secAllt.AMSE_Id 
                                && t.AMCST_Id == ph.AMCST_Id).ToList();

                                if (checkid.Count() > 0)
                                {
                                    var checkid_list = _Context.Adm_College_Yearly_StudentDMO.Single(t => t.ACYST_ActiveFlag.Equals(1) 
                                    && t.AMCO_Id == secAllt.AMCO_Id && t.ACMS_Id == secAllt.ACMS_Id && t.ASMAY_Id == secAllt.ASMAY_Id && t.AMB_Id == secAllt.AMB_Id 
                                    && t.AMSE_Id == secAllt.AMSE_Id && t.AMCST_Id == ph.AMCST_Id);
                                    checkid_list.ACYST_ActiveFlag = 2;
                                    checkid_list.UpdatedDate = DateTime.Now;
                                    _Context.Update(checkid_list);
                                }
                            }

                            int i = _Context.SaveChanges();
                            if (i > 0)
                            {
                                secAllt.returnVal = true;
                            }
                            else
                            {
                                secAllt.returnVal = false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return secAllt;
        }

        // GetstudentdetailsbyYearandclass
        public ClgYearWiseStudentDTO GetstudentdetailsbyYearandclass(ClgYearWiseStudentDTO stu)
        {
            try
            {
                string pattern = "\\s+";
                string replacement = " ";
                ClgYearWiseStudentDTO dto = new ClgYearWiseStudentDTO();
                if (stu.ACMS_Id > 0)
                {
                    var studentlist = (from m in _Context.Adm_Master_College_StudentDMO
                                       from n in _Context.Adm_College_Yearly_StudentDMO
                                       from o in _Context.CLG_Adm_Master_SemesterDMO
                                       from p in _Context.Adm_College_Master_SectionDMO
                                       from q in _Context.ClgMasterBranchDMO
                                       from r in _Context.MasterCourseDMO
                                       where (m.AMCST_Id == n.AMCST_Id && n.AMB_Id == q.AMB_Id && n.ACMS_Id == p.ACMS_Id
                                       && n.AMSE_Id == o.AMSE_Id && n.AMCO_Id == r.AMCO_Id && m.AMCST_ActiveFlag == true && n.ACYST_ActiveFlag == 1
                                       && n.ASMAY_Id == stu.ASMAY_Id && n.AMB_Id == stu.AMB_Id && n.ACMS_Id == stu.ACMS_Id && n.AMSE_Id == stu.AMSE_Id
                                       && n.AMCO_Id == stu.AMCO_Id && m.MI_Id == o.MI_Id && m.MI_Id == p.MI_Id && m.MI_Id == q.MI_Id
                                       && m.MI_Id == r.MI_Id && m.MI_Id == stu.MI_Id && m.AMCST_SOL == "S")
                                       group new { m, n, o, p, q, r }
                                       by new { m.AMCST_FirstName, m.AMCST_MiddleName, m.AMCST_LastName, n.AMCST_Id, o.AMSE_SEMName, p.ACMS_SectionName, q.AMB_BranchName, r.AMCO_CourseName } into g
                                       select new ClgYearWiseStudentDTO
                                       {
                                           StudentName = ((g.FirstOrDefault().m.AMCST_FirstName == null || g.FirstOrDefault().m.AMCST_FirstName == ""
                                           || g.FirstOrDefault().m.AMCST_FirstName == "0" ? "" : g.FirstOrDefault().m.AMCST_FirstName) +
                                           (g.FirstOrDefault().m.AMCST_MiddleName == null || g.FirstOrDefault().m.AMCST_MiddleName == ""
                                           || g.FirstOrDefault().m.AMCST_MiddleName == "0" ? "" : " " + g.FirstOrDefault().m.AMCST_MiddleName) +
                                           (g.FirstOrDefault().m.AMCST_LastName == null || g.FirstOrDefault().m.AMCST_LastName == ""
                                           || g.FirstOrDefault().m.AMCST_LastName == "0" ? "" : " " + g.FirstOrDefault().m.AMCST_LastName)),
                                           AMSE_SEMName = g.FirstOrDefault().o.AMSE_SEMName,
                                           ACMS_SectionName = g.FirstOrDefault().p.ACMS_SectionName,
                                           AMB_BranchName = g.FirstOrDefault().q.AMB_BranchName,
                                           AMCO_CourseName = g.FirstOrDefault().r.AMCO_CourseName,
                                           ACYST_RollNo = g.FirstOrDefault().n.ACYST_RollNo,
                                           AMCST_AdmNo = g.FirstOrDefault().m.AMCST_AdmNo
                                       }).ToList();
                    for (int i = 0; i < studentlist.Count; i++)
                    {
                        Regex rx = new Regex(pattern);
                        studentlist[i].StudentName = rx.Replace(studentlist[i].StudentName, replacement);
                    }
                    stu.sectionAllotedStudentList = studentlist.ToArray();
                    if (stu.sectionAllotedStudentList.Length > 0)
                    {
                        stu.count = stu.sectionAllotedStudentList.Length;
                    }
                    else
                    {
                        stu.count = 0;
                    }
                }
                else
                {
                    var studentlist = (from m in _Context.Adm_Master_College_StudentDMO
                                       from n in _Context.Adm_College_Yearly_StudentDMO
                                       from o in _Context.CLG_Adm_Master_SemesterDMO
                                       from p in _Context.Adm_College_Master_SectionDMO
                                       from q in _Context.ClgMasterBranchDMO
                                       from r in _Context.MasterCourseDMO
                                       where (m.AMCST_Id == n.AMCST_Id && n.AMB_Id == q.AMB_Id && n.ACMS_Id == p.ACMS_Id && n.AMSE_Id == o.AMSE_Id
                                       && n.AMCO_Id == r.AMCO_Id && m.AMCST_ActiveFlag == true && n.ACYST_ActiveFlag == 1 && n.ASMAY_Id == stu.ASMAY_Id
                                       && n.AMB_Id == stu.AMB_Id && n.AMSE_Id == stu.AMSE_Id && n.AMCO_Id == stu.AMCO_Id && m.MI_Id == o.MI_Id && m.MI_Id == p.MI_Id
                                       && m.MI_Id == q.MI_Id && m.MI_Id == r.MI_Id && m.MI_Id == stu.MI_Id && m.AMCST_SOL == "S")
                                       group new { m, n, o, p, q, r }
                                        by new { m.AMCST_FirstName, m.AMCST_MiddleName, m.AMCST_LastName, n.AMCST_Id, o.AMSE_SEMName, p.ACMS_SectionName } into g
                                       select new ClgYearWiseStudentDTO
                                       {
                                           StudentName = ((g.FirstOrDefault().m.AMCST_FirstName == null || g.FirstOrDefault().m.AMCST_FirstName == ""
                                           || g.FirstOrDefault().m.AMCST_FirstName == "0" ? "" : g.FirstOrDefault().m.AMCST_FirstName) +
                                           (g.FirstOrDefault().m.AMCST_MiddleName == null || g.FirstOrDefault().m.AMCST_MiddleName == ""
                                           || g.FirstOrDefault().m.AMCST_MiddleName == "0" ? "" : " " + g.FirstOrDefault().m.AMCST_MiddleName) +
                                           (g.FirstOrDefault().m.AMCST_LastName == null || g.FirstOrDefault().m.AMCST_LastName == ""
                                           || g.FirstOrDefault().m.AMCST_LastName == "0" ? "" : " " + g.FirstOrDefault().m.AMCST_LastName)),
                                           AMSE_SEMName = g.FirstOrDefault().o.AMSE_SEMName,
                                           ACMS_SectionName = g.FirstOrDefault().p.ACMS_SectionName,
                                           ACYST_RollNo = g.FirstOrDefault().n.ACYST_RollNo,
                                           AMB_BranchName = g.FirstOrDefault().q.AMB_BranchName,
                                           AMCO_CourseName = g.FirstOrDefault().r.AMCO_CourseName,
                                           AMCST_AdmNo = g.FirstOrDefault().m.AMCST_AdmNo
                                       }).ToList();

                    for (int i = 0; i < studentlist.Count; i++)
                    {
                        Regex rx = new Regex(pattern);
                        studentlist[i].StudentName = rx.Replace(studentlist[i].StudentName, replacement);
                    }
                    stu.sectionAllotedStudentList = studentlist.ToArray();

                    if (stu.sectionAllotedStudentList.Length > 0)
                    {
                        stu.count = stu.sectionAllotedStudentList.Length;
                    }
                    else
                    {
                        stu.count = 0;
                    }
                }
                // New Section details
                if (stu.SectionAllotmentType == "New")
                {

                    try
                    {
                        var stdyear = _Context.Adm_College_Yearly_StudentDMO.Where(t => t.ACYST_ActiveFlag == 1).Select(d => d.AMCST_Id).ToArray();

                        if (stdyear.Length > 0)
                        {

                            using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "CLG_adm_sectionallotment_change_promotion_yearloss";
                                cmd.CommandType = CommandType.StoredProcedure;

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.Int) { Value = stu.ASMAY_Id });
                                cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.Int) { Value = stu.AMB_Id });
                                cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.Int) { Value = stu.AMCO_Id });
                                cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.Int) { Value = stu.AMSE_Id });
                                cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.Int) { Value = stu.MI_Id });

                                var retObject = new List<dynamic>();
                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                            {
                                                dataRow.Add(
                                                    dataReader.GetName(iFiled),
                                                    dataReader.IsDBNull(iFiled) ? "" : dataReader[iFiled] // use null instead of {}
                                                );
                                            }
                                            retObject.Add((ExpandoObject)dataRow);
                                        }
                                    }
                                    stu.StudentList = retObject.ToArray();
                                }

                                catch (Exception ex)
                                {
                                    Console.Write(ex.Message);
                                }
                            }
                            if (stu.StudentList.Length > 0)
                            {
                                stu.studentlistCount = stu.StudentList.Length;
                            }
                            else
                            {
                                stu.studentlistCount = 0;
                            }
                        }

                        else
                        {
                            stu.StudentList = _Context.Adm_Master_College_StudentDMO.Where(t =>t.AMCST_ActiveFlag == true && t.MI_Id == stu.MI_Id 
                            && t.ASMAY_Id == stu.ASMAY_Id && t.AMB_Id == stu.AMB_Id && t.AMSE_Id == stu.AMSE_Id && t.AMCO_Id == stu.AMCO_Id).ToArray();
                            if (stu.StudentList.Length > 0)
                            {
                                stu.studentlistCount = stu.StudentList.Length;
                            }
                            else
                            {
                                stu.studentlistCount = 0;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                // Change Section details
                if (stu.SectionAllotmentType == "Change")
                {
                    try
                    {
                        stu.StudentListYear = (from m in _Context.Adm_Master_College_StudentDMO
                                               from n in _Context.Adm_College_Yearly_StudentDMO
                                               from o in _Context.CLG_Adm_Master_SemesterDMO
                                               from p in _Context.Adm_College_Master_SectionDMO
                                               from q in _Context.ClgMasterBranchDMO
                                               from r in _Context.MasterCourseDMO
                                               from s in _Context.AcademicYear
                                               where m.AMCST_Id == n.AMCST_Id && n.AMB_Id == q.AMB_Id && n.ACMS_Id == p.ACMS_Id
                                               && n.AMSE_Id == o.AMSE_Id && n.AMCO_Id == r.AMCO_Id && m.AMCST_ActiveFlag == true
                                               && n.ACYST_ActiveFlag == 1 && n.ASMAY_Id == stu.ASMAY_Id && n.AMB_Id == stu.AMB_Id
                                               && n.ACMS_Id == stu.ACMS_Id && n.AMSE_Id == stu.AMSE_Id && n.AMCO_Id == stu.AMCO_Id
                                               && n.ASMAY_Id == s.ASMAY_Id && m.MI_Id == o.MI_Id && m.MI_Id == p.MI_Id
                                               && m.MI_Id == q.MI_Id && m.MI_Id == r.MI_Id && m.MI_Id == stu.MI_Id
                                               && m.AMCST_SOL == "S"
                                               select new ClgYearWiseStudentDTO
                                               {
                                                   ACYST_Id = n.ACYST_Id,
                                                   AMCST_Id = n.AMCST_Id,
                                                   AMCST_FirstName = m.AMCST_FirstName,
                                                   AMCST_MiddleName = m.AMCST_MiddleName,
                                                   AMCST_LastName = m.AMCST_LastName,
                                                   AMB_Id = n.AMB_Id,
                                                   AMB_BranchName = q.AMB_BranchName,
                                                   ACMS_Id = n.ACMS_Id,
                                                   ACMS_SectionName = p.ACMS_SectionName,
                                                   ACYST_RollNo = n.ACYST_RollNo,
                                                   ASMAY_Id = n.ASMAY_Id,
                                                   ASMAY_Year = s.ASMAY_Year,
                                                   AYST_PassFailFlag = n.AYST_PassFailFlag,
                                                   ACYST_ActiveFlag = n.ACYST_ActiveFlag,
                                                   LoginId = n.LoginId,
                                                   ACYST_DateTime = n.ACYST_DateTime,
                                                   CreatedDate = n.CreatedDate,
                                                   UpdatedDate = n.UpdatedDate,
                                                   AMCST_AdmNo = m.AMCST_AdmNo,
                                                   AMSE_Id = n.AMSE_Id,
                                                   AMSE_SEMName = o.AMSE_SEMName,
                                                   AMCO_Id = n.AMCO_Id,
                                                   AMCO_CourseName = r.AMCO_CourseName

                                               }).ToArray();
                        if (stu.StudentListYear.Length > 0)
                        {
                            stu.studentListYearCount = stu.StudentListYear.Length;
                        }
                        else
                        {
                            stu.studentListYearCount = 0;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                // Delete Section details
                if (stu.SectionAllotmentType == "Delete")
                {
                    try
                    {
                        stu.StudentListYear = (from m in _Context.Adm_Master_College_StudentDMO
                                               from n in _Context.Adm_College_Yearly_StudentDMO
                                               from o in _Context.CLG_Adm_Master_SemesterDMO
                                               from p in _Context.Adm_College_Master_SectionDMO
                                               from q in _Context.ClgMasterBranchDMO
                                               from r in _Context.MasterCourseDMO
                                               from s in _Context.AcademicYear
                                               where m.AMCST_Id == n.AMCST_Id && n.AMB_Id == q.AMB_Id && n.ACMS_Id == p.ACMS_Id
                                               && n.AMSE_Id == o.AMSE_Id && n.AMCO_Id == r.AMCO_Id && m.AMCST_ActiveFlag == true
                                               && n.ACYST_ActiveFlag == 1 && n.ASMAY_Id == stu.ASMAY_Id && n.AMB_Id == stu.AMB_Id
                                               && n.ACMS_Id == stu.ACMS_Id && n.AMSE_Id == stu.AMSE_Id && n.AMCO_Id == stu.AMCO_Id
                                               && n.ASMAY_Id == s.ASMAY_Id && m.MI_Id == o.MI_Id
                                               && m.MI_Id == p.MI_Id && m.MI_Id == q.MI_Id && m.MI_Id == r.MI_Id && m.MI_Id == stu.MI_Id
                                               && m.AMCST_SOL == "S"
                                               select new ClgYearWiseStudentDTO
                                               {
                                                   ACYST_Id = n.ACYST_Id,
                                                   AMCST_Id = n.AMCST_Id,
                                                   AMCST_FirstName = m.AMCST_FirstName,
                                                   AMCST_MiddleName = m.AMCST_MiddleName,
                                                   AMCST_LastName = m.AMCST_LastName,
                                                   AMB_Id = n.AMB_Id,
                                                   AMB_BranchName = q.AMB_BranchName,
                                                   ACMS_Id = n.ACMS_Id,
                                                   ACMS_SectionName = p.ACMS_SectionName,
                                                   ACYST_RollNo = n.ACYST_RollNo,
                                                   ASMAY_Id = n.ASMAY_Id,
                                                   ASMAY_Year = s.ASMAY_Year,
                                                   AYST_PassFailFlag = n.AYST_PassFailFlag,
                                                   ACYST_ActiveFlag = n.ACYST_ActiveFlag,
                                                   LoginId = n.LoginId,
                                                   ACYST_DateTime = n.ACYST_DateTime,
                                                   CreatedDate = n.CreatedDate,
                                                   UpdatedDate = n.UpdatedDate,
                                                   AMCST_AdmNo = m.AMCST_AdmNo,
                                                   AMSE_Id = n.AMSE_Id,
                                                   AMSE_SEMName = o.AMSE_SEMName,
                                                   AMCO_Id = n.AMCO_Id,
                                                   AMCO_CourseName = r.AMCO_CourseName

                                               }).ToArray();
                        if (stu.StudentListYear.Length > 0)
                        {
                            stu.studentListYearCount = stu.StudentListYear.Length;
                        }
                        else
                        {
                            stu.studentListYearCount = 0;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }

                //  Section details
                if (stu.SectionAllotmentType == "Promotion")
                {
                    try
                    {
                        stu.StudentListYear = (from m in _Context.Adm_Master_College_StudentDMO
                                               from n in _Context.Adm_College_Yearly_StudentDMO
                                               from o in _Context.CLG_Adm_Master_SemesterDMO
                                               from p in _Context.Adm_College_Master_SectionDMO
                                               from q in _Context.ClgMasterBranchDMO
                                               from r in _Context.MasterCourseDMO
                                               from s in _Context.AcademicYear
                                               where m.AMCST_Id == n.AMCST_Id && n.AMB_Id == q.AMB_Id && n.ACMS_Id == p.ACMS_Id
                                               && n.AMSE_Id == o.AMSE_Id && n.AMCO_Id == r.AMCO_Id && m.AMCST_ActiveFlag == true
                                               && n.ACYST_ActiveFlag == 1 && n.ASMAY_Id == stu.ASMAY_Id && n.AMB_Id == stu.AMB_Id
                                               && n.ACMS_Id == stu.ACMS_Id && n.AMSE_Id == stu.AMSE_Id && n.AMCO_Id == stu.AMCO_Id
                                               && n.ASMAY_Id == s.ASMAY_Id && m.ASMAY_Id == s.ASMAY_Id && m.MI_Id == o.MI_Id
                                               && m.MI_Id == p.MI_Id && m.MI_Id == q.MI_Id && m.MI_Id == r.MI_Id && m.MI_Id == stu.MI_Id
                                               && m.AMCST_SOL == "S"
                                               select new ClgYearWiseStudentDTO
                                               {
                                                   ACYST_Id = n.ACYST_Id,
                                                   AMCST_Id = n.AMCST_Id,
                                                   AMCST_FirstName = m.AMCST_FirstName,
                                                   AMCST_MiddleName = m.AMCST_MiddleName,
                                                   AMCST_LastName = m.AMCST_LastName,
                                                   AMB_Id = n.AMB_Id,
                                                   AMB_BranchName = q.AMB_BranchName,
                                                   ACMS_Id = n.ACMS_Id,
                                                   ACMS_SectionName = p.ACMS_SectionName,
                                                   ACYST_RollNo = n.ACYST_RollNo,
                                                   ASMAY_Id = n.ASMAY_Id,
                                                   ASMAY_Year = s.ASMAY_Year,
                                                   AYST_PassFailFlag = n.AYST_PassFailFlag,
                                                   ACYST_ActiveFlag = n.ACYST_ActiveFlag,
                                                   LoginId = n.LoginId,
                                                   ACYST_DateTime = n.ACYST_DateTime,
                                                   CreatedDate = n.CreatedDate,
                                                   UpdatedDate = n.UpdatedDate,
                                                   AMCST_AdmNo = m.AMCST_AdmNo,
                                                   AMSE_Id = n.AMSE_Id,
                                                   AMSE_SEMName = o.AMSE_SEMName,
                                                   AMCO_Id = n.AMCO_Id,
                                                   AMCO_CourseName = r.AMCO_CourseName

                                               }).ToArray();
                        if (stu.StudentListYear.Length > 0)
                        {
                            stu.studentListYearCount = stu.StudentListYear.Length;
                        }
                        else
                        {
                            stu.studentListYearCount = 0;
                        }


                        var check_order_left = _Context.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == stu.MI_Id && a.AMSE_Id == stu.AMSE_Id).ToList();
                        var semorder_left = check_order_left.FirstOrDefault().AMSE_SEMOrder;
                        var semevenorder_left = check_order_left.FirstOrDefault().AMSE_EvenOdd;
                        var semyear_left = check_order_left.FirstOrDefault().AMSE_Year;
                        int orderright = semorder_left + 1;

                        var check_year_left = _Context.AcademicYear.Where(a => a.MI_Id == stu.MI_Id && a.ASMAY_Id == stu.ASMAY_Id).ToList();
                        var yearorder_left = check_year_left.FirstOrDefault().ASMAY_Order;
                        int yearorderright = yearorder_left + 1;

                        var check_order_rigth = _Context.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == stu.MI_Id && a.AMSE_SEMOrder == orderright).ToList();
                        var semorder_right = check_order_rigth.FirstOrDefault().AMSE_SEMOrder;
                        var semevenorder_right = check_order_rigth.FirstOrDefault().AMSE_EvenOdd;
                        var semyear_right = check_order_rigth.FirstOrDefault().AMSE_Year;

                        long accyearid = 0;
                        //long presentaccyearid = 0;
                        long semid = 0;

                        if (semyear_left != semyear_right)
                        {
                            var accyear_right = _Context.AcademicYear.Where(a => a.MI_Id == stu.MI_Id && a.ASMAY_Order == yearorderright).ToList();
                            accyearid = accyear_right.FirstOrDefault().ASMAY_Id;

                            var sem_right = _Context.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == stu.MI_Id && a.AMSE_SEMOrder == orderright).ToList();
                            semid = sem_right.FirstOrDefault().AMSE_Id;
                        }
                        else
                        {
                            var accyear_right = _Context.AcademicYear.Where(a => a.MI_Id == stu.MI_Id && a.ASMAY_Order == yearorder_left && a.ASMAY_Id == stu.ASMAY_Id).ToList();
                            accyearid = accyear_right.FirstOrDefault().ASMAY_Id;

                            var sem_right = _Context.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == stu.MI_Id && a.AMSE_SEMOrder == orderright).ToList();
                            semid = sem_right.FirstOrDefault().AMSE_Id;
                        }


                        using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Clg_Adm_Classwisestudentpromoteddetails";
                            cmd.CommandType = CommandType.StoredProcedure;

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();
                            cmd.Parameters.Add(new SqlParameter("@promotedyear", SqlDbType.VarChar) { Value = accyearid });
                            cmd.Parameters.Add(new SqlParameter("@promotedcourse", SqlDbType.VarChar) { Value = stu.AMCO_Id });
                            cmd.Parameters.Add(new SqlParameter("@promotedbranch", SqlDbType.VarChar) { Value = stu.AMB_Id });
                            cmd.Parameters.Add(new SqlParameter("@promotedsem", SqlDbType.VarChar) { Value = semid });

                            cmd.Parameters.Add(new SqlParameter("@presentyear", SqlDbType.VarChar) { Value = stu.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@presentcourse", SqlDbType.VarChar) { Value = stu.AMCO_Id });
                            cmd.Parameters.Add(new SqlParameter("@presentbranch", SqlDbType.VarChar) { Value = stu.AMB_Id });
                            cmd.Parameters.Add(new SqlParameter("@presentsem", SqlDbType.VarChar) { Value = stu.AMSE_Id });
                            cmd.Parameters.Add(new SqlParameter("@presentsec", SqlDbType.VarChar) { Value = stu.ACMS_Id });

                            cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.VarChar) { Value = stu.MI_Id });

                            var retObject = new List<dynamic>();
                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                        {
                                            dataRow.Add(
                                                dataReader.GetName(iFiled),
                                                dataReader.IsDBNull(iFiled) ? "" : dataReader[iFiled] // use null instead of {}
                                            );
                                        }
                                        retObject.Add((ExpandoObject)dataRow);
                                    }
                                }
                                stu.StudentListYear = retObject.ToArray();
                            }

                            catch (Exception ex)
                            {
                                Console.Write(ex.Message);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }

                //year loss details
                if (stu.SectionAllotmentType == "YearLoss")
                {
                    //try
                    //{
                    //    stu.StudentListYear = (from sy in _Context.SchoolYearWiseStudent
                    //                           from s in _Context.Adm_M_Student
                    //                           from c in _Context.School_M_Class
                    //                           from sec in _Context.School_M_Section
                    //                           from y in _Context.AcademicYear
                    //                           where sy.AMST_Id == s.AMST_Id && sy.ASMCL_Id == c.ASMCL_Id &&
                    //                           sy.ASMS_Id == sec.ASMS_Id && sy.ASMAY_Id == y.ASMAY_Id &&
                    //                           s.AMST_ActiveFlag == 1 && s.AMST_SOL.Equals("S") &&
                    //                           sy.ASMAY_Id == stu.ASMAY_Id && sy.ASMCL_Id == stu.ASMCL_Id && sy.ASMS_Id == stu.ASMS_Id
                    //                            && sy.AMAY_ActiveFlag == 1 && s.MI_Id == stu.MI_Id
                    //                           select new ClgYearWiseStudentDTO
                    //                           {
                    //                               ASYST_Id = sy.ASYST_Id,
                    //                               AMST_Id = sy.AMST_Id,
                    //                               AMST_FirstName = s.AMST_FirstName,
                    //                               AMST_MiddleName = s.AMST_MiddleName,
                    //                               AMST_LastName = s.AMST_LastName,
                    //                               ASMCL_Id = sy.ASMCL_Id,
                    //                               ASMCL_ClassName = c.ASMCL_ClassName,
                    //                               ASMS_Id = sy.ASMS_Id,
                    //                               ASMC_SectionName = sec.ASMC_SectionName,
                    //                               AMAY_RollNo = sy.AMAY_RollNo,
                    //                               ASMAY_Id = sy.ASMAY_Id,
                    //                               ASMAY_Year = y.ASMAY_Year,
                    //                               AMST_AdmNo = s.AMST_AdmNo

                    //                           }).ToArray();
                    //    if (stu.StudentListYear.Length > 0)
                    //    {
                    //        stu.studentListYearCount = stu.StudentListYear.Length;
                    //    }
                    //    else
                    //    {
                    //        stu.studentListYearCount = 0;
                    //    }

                    //    int NoOfYears = stu.NoOfYears;

                    //    string Selectedyear = _Context.AcademicYear.Single(y => y.ASMAY_Id == stu.ASMAY_Id).ASMAY_Year;

                    //    string[] SelectedyearArray = Selectedyear.Split('-');

                    //    int firstfield = Convert.ToInt32(SelectedyearArray.ElementAt(0)) + NoOfYears;
                    //    int lastfield = Convert.ToInt32(SelectedyearArray.ElementAt(1)) + NoOfYears;
                    //    string ConvertedYear = firstfield + "-" + lastfield;


                    //    List<MasterAcademic> year = new List<MasterAcademic>();
                    //    year = _Context.AcademicYear.Where(y => y.ASMAY_Year.Equals(ConvertedYear)).OrderBy(t => t.ASMAY_Order).ToList();

                    //    stu.YearList = year.ToArray();
                    //}
                    //catch (Exception e)
                    //{
                    //    Console.WriteLine(e.Message);
                    //}

                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return stu;
        }

        //update roll no get details by class section
        public ClgYearWiseStudentDTO GetStudentListByURN(ClgYearWiseStudentDTO data)
        {
        
            try
            {
                var UpdateRollNo = (from m in _Context.Adm_Master_College_StudentDMO
                                     from n in _Context.Adm_College_Yearly_StudentDMO
                                     from o in _Context.CLG_Adm_Master_SemesterDMO
                                     from p in _Context.Adm_College_Master_SectionDMO
                                     from q in _Context.ClgMasterBranchDMO
                                     from r in _Context.MasterCourseDMO
                                     from s in _Context.AcademicYear
                                     where m.AMCST_Id == n.AMCST_Id && n.AMB_Id == q.AMB_Id && n.ACMS_Id == p.ACMS_Id
                                     && n.AMSE_Id == o.AMSE_Id && n.AMCO_Id == r.AMCO_Id && m.AMCST_ActiveFlag == true
                                     && n.ACYST_ActiveFlag == 1 && n.ASMAY_Id == data.ASMAY_Id && n.AMB_Id == data.AMB_Id
                                     && n.ACMS_Id == data.ACMS_Id && n.AMSE_Id == data.AMSE_Id && n.AMCO_Id == data.AMCO_Id
                                     && n.ASMAY_Id == s.ASMAY_Id && m.MI_Id == o.MI_Id && m.MI_Id == p.MI_Id
                                     && m.MI_Id == q.MI_Id && m.MI_Id == r.MI_Id && m.MI_Id == data.MI_Id
                                     && m.AMCST_SOL == "S"
                                     select new UpdateRollNo
                                     {
                                         ACYST_Id = n.ACYST_Id,
                                         AMCST_Id = n.AMCST_Id,
                                         AMCST_FirstName = m.AMCST_FirstName,
                                         AMCST_MiddleName = m.AMCST_MiddleName,
                                         AMCST_LastName = m.AMCST_LastName,
                                         AMB_Id = n.AMB_Id,
                                         AMB_BranchName = q.AMB_BranchName,
                                         ACMS_Id = n.ACMS_Id,
                                         ACMS_SectionName = p.ACMS_SectionName,
                                         ACYST_RollNo = n.ACYST_RollNo,
                                         ASMAY_Id = n.ASMAY_Id,
                                         ASMAY_Year = s.ASMAY_Year,
                                         AYST_PassFailFlag = n.AYST_PassFailFlag,
                                         ACYST_ActiveFlag = n.ACYST_ActiveFlag,
                                         LoginId = n.LoginId,
                                         ACYST_DateTime = n.ACYST_DateTime,
                                         CreatedDate = n.CreatedDate,
                                         UpdatedDate = n.UpdatedDate,
                                         AMCST_AdmNo = m.AMCST_AdmNo,
                                         AMSE_Id = n.AMSE_Id,
                                         AMSE_SEMName = o.AMSE_SEMName,
                                         AMCO_Id = n.AMCO_Id,
                                         AMCO_CourseName = r.AMCO_CourseName

                                     }).ToList();

                data.UpdateRollNo = UpdateRollNo.ToArray();
                if (data.UpdateRollNo.Length > 0)
                {
                    data.count = data.UpdateRollNo.Length;
                }
                else
                {
                    data.count=0;
                }
                var UpdateRollNo1 = (from m in _Context.Adm_Master_College_StudentDMO
                                    from n in _Context.Adm_College_Yearly_StudentDMO
                                    from o in _Context.CLG_Adm_Master_SemesterDMO
                                    from p in _Context.Adm_College_Master_SectionDMO
                                    from q in _Context.ClgMasterBranchDMO
                                    from r in _Context.MasterCourseDMO
                                    from s in _Context.AcademicYear
                                    where m.AMCST_Id == n.AMCST_Id && n.AMB_Id == q.AMB_Id && n.ACMS_Id == p.ACMS_Id
                                    && n.AMSE_Id == o.AMSE_Id && n.AMCO_Id == r.AMCO_Id && m.AMCST_ActiveFlag == true
                                    && n.ACYST_ActiveFlag == 1 && n.ASMAY_Id == data.ASMAY_Id && n.AMB_Id == data.AMB_Id
                                    && n.ACMS_Id == data.ACMS_Id && n.AMSE_Id == data.AMSE_Id && n.AMCO_Id == data.AMCO_Id
                                    && n.ASMAY_Id == s.ASMAY_Id && m.MI_Id == o.MI_Id && m.MI_Id == p.MI_Id
                                    && m.MI_Id == q.MI_Id && m.MI_Id == r.MI_Id && m.MI_Id == data.MI_Id
                                    && m.AMCST_SOL == "S"
                                    select new UpdateRollNo1
                                    {
                                        ACYST_Id = n.ACYST_Id,
                                        AMCST_Id = n.AMCST_Id,
                                        AMCST_FirstName = m.AMCST_FirstName,
                                        AMCST_MiddleName = m.AMCST_MiddleName,
                                        AMCST_LastName = m.AMCST_LastName,
                                        AMB_Id = n.AMB_Id,
                                        AMB_BranchName = q.AMB_BranchName,
                                        ACMS_Id = n.ACMS_Id,
                                        ACMS_SectionName = p.ACMS_SectionName,
                                        ACYST_RollNo = n.ACYST_RollNo,
                                        ASMAY_Id = n.ASMAY_Id,
                                        ASMAY_Year = s.ASMAY_Year,
                                        AYST_PassFailFlag = n.AYST_PassFailFlag,
                                        ACYST_ActiveFlag = n.ACYST_ActiveFlag,
                                        LoginId = n.LoginId,
                                        ACYST_DateTime = n.ACYST_DateTime,
                                        CreatedDate = n.CreatedDate,
                                        UpdatedDate = n.UpdatedDate,
                                        AMCST_AdmNo = m.AMCST_AdmNo,
                                        AMSE_Id = n.AMSE_Id,
                                        AMSE_SEMName = o.AMSE_SEMName,
                                        AMCO_Id = n.AMCO_Id,
                                        AMCO_CourseName = r.AMCO_CourseName

                                    }).ToList();

                data.UpdateRollNo1 = UpdateRollNo1.ToArray();
                if (data.UpdateRollNo1.Length > 0)
                {
                    data.count = data.UpdateRollNo1.Length;
                }
                else
                {
                    data.count = 0;
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return data;
        }
        public ClgYearWiseStudentDTO GetStudentListByURNsave(ClgYearWiseStudentDTO data)
        {
            try
            {

                for (int i = 0; i < data.UpdateRollNo.Count(); i++)
                {
                    Adm_College_Yearly_StudentDMO std = new Adm_College_Yearly_StudentDMO();
                    var result = _Context.Adm_College_Yearly_StudentDMO.Single(t => t.AMCST_Id == Convert.ToInt64(data.UpdateRollNo[i].AMCST_Id) && t.ACYST_Id == Convert.ToInt64(data.UpdateRollNo[i].ACYST_Id));
                    result.ACYST_RollNo = Convert.ToInt64(data.UpdateRollNo[i].pdays);
                    std.UpdatedDate = DateTime.Now;
                    _Context.Update(result);
                }
                var contactExists = _Context.SaveChanges();
                // _Context.Add(std);
                if (contactExists >= 1)
                {
                    data.returnal = true;
                }
                else
                {
                    data.returnal = false;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // College Year loss  section
        public ClgYearWiseStudentDTO OnChangeyearlossAcademic(ClgYearWiseStudentDTO data)
        {
            try
            {
                data.courselist = (from a in _Context.MasterCourseDMO
                                   from b in _Context.CLG_Adm_College_AY_CourseDMO
                                   where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                                   select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgYearWiseStudentDTO Getyearlossbranch(ClgYearWiseStudentDTO data)
        {
            try
            {
                data.coursebranchlist = (from a in _Context.ClgMasterBranchDMO
                                         from b in _Context.CLG_Adm_College_AY_Course_BranchDMO
                                         from c in _Context.CLG_Adm_College_AY_CourseDMO
                                         where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.AMB_Id == b.AMB_Id && a.AMB_ActiveFlag == true && b.ACAYCB_ActiveFlag == true && b.ACAYC_Id == c.ACAYC_Id && c.AMCO_Id == data.AMCO_Id && c.ASMAY_Id == data.ASMAY_Id)
                                         select new ClgYearWiseStudentDTO
                                         {
                                             AMB_Id = b.AMB_Id,
                                             AMB_BranchName = a.AMB_BranchName
                                         }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgYearWiseStudentDTO Getyearlosssem(ClgYearWiseStudentDTO data)
        {
            try
            {
                data.semlist = (from a in _Context.CLG_Adm_Master_SemesterDMO
                                from b in _Context.CLG_Adm_College_AY_Course_BranchDMO
                                from c in _Context.CLG_Adm_College_AY_CourseDMO
                                from d in _Context.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.MI_Id == data.MI_Id && a.AMSE_Id == d.AMSE_Id && a.AMSE_ActiveFlg == true && b.ACAYCB_ActiveFlag == true && b.ACAYC_Id == c.ACAYC_Id && c.AMCO_Id == data.AMCO_Id
                               && b.ACAYCB_Id == d.ACAYCB_Id && d.ACAYCBS_ActiveFlag == true && c.ASMAY_Id == data.ASMAY_Id && b.AMB_Id == data.AMB_Id)
                                select new ClgYearWiseStudentDTO
                                {
                                    AMSE_Id = a.AMSE_Id,
                                    AMSE_SEMName = a.AMSE_SEMName,
                                }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgYearWiseStudentDTO GetStudentListByYear_yearloss1(ClgYearWiseStudentDTO data)
        {
            try
            {
                data.studentList4ys = (from m in _Context.Adm_Master_College_StudentDMO
                                       from n in _Context.Adm_College_Yearly_StudentDMO
                                       from o in _Context.CLG_Adm_Master_SemesterDMO
                                       from p in _Context.Adm_College_Master_SectionDMO
                                       from q in _Context.ClgMasterBranchDMO
                                       from r in _Context.MasterCourseDMO
                                       from s in _Context.AcademicYear
                                       where m.AMCST_Id == n.AMCST_Id && n.AMB_Id == q.AMB_Id && n.ACMS_Id == p.ACMS_Id
                                       && n.AMSE_Id == o.AMSE_Id && n.AMCO_Id == r.AMCO_Id && m.AMCST_ActiveFlag == true
                                       && n.ACYST_ActiveFlag == 1 && n.ASMAY_Id == data.ASMAY_Id && n.AMB_Id == data.AMB_Id
                                       && n.ACMS_Id == data.ACMS_Id && n.AMSE_Id == data.AMSE_Id && n.AMCO_Id == data.AMCO_Id
                                       && n.ASMAY_Id == s.ASMAY_Id && m.ASMAY_Id == s.ASMAY_Id && m.MI_Id == o.MI_Id
                                       && m.MI_Id == p.MI_Id && m.MI_Id == q.MI_Id && m.MI_Id == r.MI_Id && m.MI_Id == data.MI_Id
                                       && m.AMCST_SOL == "S"
                                       select new ClgYearWiseStudentDTO
                                       {
                                           ACYST_Id = n.ACYST_Id,
                                           AMCST_Id = n.AMCST_Id,
                                           AMCST_FirstName = m.AMCST_FirstName,
                                           AMCST_MiddleName = m.AMCST_MiddleName,
                                           AMCST_LastName = m.AMCST_LastName,
                                           AMB_Id = n.AMB_Id,
                                           AMB_BranchName = q.AMB_BranchName,
                                           ACMS_Id = n.ACMS_Id,
                                           ACMS_SectionName = p.ACMS_SectionName,
                                           ACYST_RollNo = n.ACYST_RollNo,
                                           ASMAY_Id = n.ASMAY_Id,
                                           ASMAY_Year = s.ASMAY_Year,
                                           AYST_PassFailFlag = n.AYST_PassFailFlag,
                                           ACYST_ActiveFlag = n.ACYST_ActiveFlag,
                                           LoginId = n.LoginId,
                                           ACYST_DateTime = n.ACYST_DateTime,
                                           CreatedDate = n.CreatedDate,
                                           UpdatedDate = n.UpdatedDate,
                                           AMCST_AdmNo = m.AMCST_AdmNo,
                                           AMSE_Id = n.AMSE_Id,
                                           AMSE_SEMName = o.AMSE_SEMName,
                                           AMCO_Id = n.AMCO_Id,
                                           AMCO_CourseName = r.AMCO_CourseName

                                       }).ToArray();
                if (data.studentList4ys.Length > 0)
                {
                    data.studentListYearCount = data.studentList4ys.Length;
                }
                else
                {
                    data.studentListYearCount = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
