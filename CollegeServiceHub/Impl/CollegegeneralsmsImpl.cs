using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.College.Admission;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CollegeServiceHub.Impl
{
    public class CollegegeneralsmsImpl : Interface.CollegegeneralsmsInterface
    {
        private readonly ClgAdmissionContext _smsContext;

        public DomainModelMsSqlServerContext _db;
        public CollegegeneralsmsImpl(ClgAdmissionContext cpContext, DomainModelMsSqlServerContext db)
        {
            _smsContext = cpContext;
            _db = db;
        }

        public async Task<CollegegeneralsmsDTO> Getdetails(CollegegeneralsmsDTO data)//int IVRMM_Id
        {
            {
                try
                {
                    var list = await _smsContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToListAsync();//AcademicYear
                    data.yearlist = list.ToArray();

                    var currYear = await _smsContext.AcademicYear.Where(d => d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id).OrderByDescending(t => t.ASMAY_Order).ToListAsync();
                    data.currentYear = currYear.ToArray();                    

                    var designationdropdown = await _smsContext.HR_Master_Designation.Where(t => t.MI_Id == data.MI_Id && t.HRMDES_ActiveFlag == true).ToListAsync();
                    data.designationdropdown = designationdropdown.ToArray();
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }

                return data;
            }

        }

        // ****** For Student Radio Button Select ********// 
        public CollegegeneralsmsDTO onSelectyear(CollegegeneralsmsDTO data)
        {
            data.courselist = (from a in _smsContext.MasterCourseDMO
                               from b in _smsContext.CLG_Adm_College_AY_CourseDMO
                               where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                               select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();

            return data;
        }
        public CollegegeneralsmsDTO onselectedcourse(CollegegeneralsmsDTO data)
        {
            var branchlist = (from a in _smsContext.ClgMasterBranchDMO
                              from b in _smsContext.CLG_Adm_College_AY_CourseDMO
                              from c in _smsContext.CLG_Adm_College_AY_Course_BranchDMO
                              where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag)
                              select a).Distinct().ToList();
            data.branchlist = branchlist.OrderBy(t => t.AMB_Order).ToArray();

            return data;
        }
        public CollegegeneralsmsDTO onselectbranch(CollegegeneralsmsDTO data)
        {
            if (data.AMB_Id == 0)
            {
                data.semisterlist = _smsContext.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg == true).OrderBy(t => t.AMSE_SEMOrder).ToArray();
            }
            else
            {
                var semisterlist = (from a in _smsContext.CLG_Adm_Master_SemesterDMO
                                    from b in _smsContext.CLG_Adm_College_AY_CourseDMO
                                    from c in _smsContext.CLG_Adm_College_AY_Course_BranchDMO
                                    from d in _smsContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
                                    select a).Distinct().ToList();
                data.semisterlist = semisterlist.OrderBy(t => t.AMSE_SEMOrder).ToArray();
            }



            return data;
        }
        public CollegegeneralsmsDTO onselectsemister(CollegegeneralsmsDTO data)
        {
            data.sectionlist = _smsContext.Adm_College_Master_SectionDMO.Where(t => t.MI_Id == data.MI_Id && t.ACMS_ActiveFlag == true).Distinct().OrderBy(t => t.ACMS_Order).ToArray();
            return data;
        }
        public CollegegeneralsmsDTO Getexam(CollegegeneralsmsDTO data)
        {
            try
            {
                //var Cat_Id1 = _smsContext.Exm_Category_ClassDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMAY_Id == data.ASMAY_Id && t.ECAC_ActiveFlag == true).ToList();
                //if (Cat_Id1.Count > 0)
                //{
                //    var Cat_Id = _smsContext.Exm_Category_ClassDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMAY_Id == data.ASMAY_Id && t.ECAC_ActiveFlag == true).Select(t => t.EMCA_Id).First();
                //    var year_cat_id = _smsContext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.EYC_ActiveFlg == true && t.EMCA_Id == Cat_Id).Select(t => t.EYC_Id).First();

                //    data.exmstdlist = (from a in _smsContext.masterexam
                //                       from b in _smsContext.Exm_Yearly_Category_ExamsDMO
                //                       where (a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true && a.EME_Id == b.EME_Id && b.EYC_Id == year_cat_id)
                //                       select a).Distinct().ToArray();
                //}

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }
        public async Task<CollegegeneralsmsDTO> GetStudentDetails(CollegegeneralsmsDTO data)
        {
            try
            {
                List<long> GrpId = new List<long>();
                List<long> grpidsem = new List<long>();

                if (data.AMB_Id == 0)
                {
                    var getbranchid = (from a in _smsContext.CLG_Adm_College_AY_CourseDMO
                                       from b in _smsContext.CLG_Adm_College_AY_Course_BranchDMO
                                       from c in _smsContext.MasterCourseDMO
                                       from d in _smsContext.ClgMasterBranchDMO
                                       where (a.ACAYC_Id == b.ACAYC_Id && a.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && a.MI_Id == data.MI_Id
                                       && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && a.ACAYC_ActiveFlag == true && b.ACAYCB_ActiveFlag == true)
                                       select new CollegegeneralsmsDTO
                                       {
                                           AMB_Id = b.AMB_Id

                                       }).Distinct().ToList();

                    foreach (var item in getbranchid)
                    {
                        GrpId.Add(item.AMB_Id);
                    }
                }
                else
                {
                    var getbranchid = (from a in _smsContext.CLG_Adm_College_AY_CourseDMO
                                       from b in _smsContext.CLG_Adm_College_AY_Course_BranchDMO
                                       from c in _smsContext.MasterCourseDMO
                                       from d in _smsContext.ClgMasterBranchDMO
                                       where (a.ACAYC_Id == b.ACAYC_Id && a.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && a.MI_Id == data.MI_Id
                                       && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && a.ACAYC_ActiveFlag == true && b.ACAYCB_ActiveFlag == true && b.AMB_Id == data.AMB_Id)
                                       select new CollegegeneralsmsDTO
                                       {
                                           AMB_Id = b.AMB_Id

                                       }).Distinct().ToList();

                    foreach (var item in getbranchid)
                    {
                        GrpId.Add(item.AMB_Id);
                    }
                }

                if (data.AMSE_Id == 0)
                {
                    var getsemid = (from a in _smsContext.CLG_Adm_College_AY_CourseDMO
                                    from b in _smsContext.CLG_Adm_College_AY_Course_BranchDMO
                                    from e in _smsContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    from c in _smsContext.MasterCourseDMO
                                    from d in _smsContext.ClgMasterBranchDMO
                                    from f in _smsContext.CLG_Adm_Master_SemesterDMO
                                    where (a.ACAYC_Id == b.ACAYC_Id && b.ACAYCB_Id == e.ACAYCB_Id && a.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && f.AMSE_Id == e.AMSE_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id 
                                    && GrpId.Contains(b.AMB_Id) && a.ACAYC_ActiveFlag == true && b.ACAYCB_ActiveFlag == true && e.ACAYCBS_ActiveFlag == true)
                                    select new CollegegeneralsmsDTO
                                    {
                                        AMSE_Id = e.AMSE_Id

                                    }).Distinct().ToList();

                    foreach (var item in getsemid)
                    {
                        grpidsem.Add(item.AMSE_Id);
                    }
                }
                else
                {
                    var getsemid = (from a in _smsContext.CLG_Adm_College_AY_CourseDMO
                                    from b in _smsContext.CLG_Adm_College_AY_Course_BranchDMO
                                    from e in _smsContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    from c in _smsContext.MasterCourseDMO
                                    from d in _smsContext.ClgMasterBranchDMO
                                    from f in _smsContext.CLG_Adm_Master_SemesterDMO
                                    where (a.ACAYC_Id == b.ACAYC_Id && b.ACAYCB_Id == e.ACAYCB_Id && a.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && a.MI_Id == data.MI_Id && f.AMSE_Id == e.AMSE_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && a.ACAYC_ActiveFlag == true && b.ACAYCB_ActiveFlag == true && e.ACAYCBS_ActiveFlag==true &&  GrpId.Contains(b.AMB_Id) && e.AMSE_Id==data.AMSE_Id)
                                    select new CollegegeneralsmsDTO
                                    {
                                        AMSE_Id = e.AMSE_Id

                                    }).Distinct().ToList();

                    foreach (var item in getsemid)
                    {
                        grpidsem.Add(item.AMSE_Id);
                    }
                }



                var config = _smsContext.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_Id).ToList();

                if (data.radiotype == "Student")
                {
                    if (data.ASMS_Id == 0)
                    {
                        var studentlist = await (from a in _smsContext.Adm_Master_College_StudentDMO
                                                 from b in _smsContext.Adm_College_Yearly_StudentDMO
                                                 from c in _smsContext.MasterCourseDMO
                                                 from d in _smsContext.ClgMasterBranchDMO
                                                 from e in _smsContext.CLG_Adm_Master_SemesterDMO
                                                 from f in _smsContext.Adm_College_Master_SectionDMO
                                                 from g in _smsContext.AcademicYear
                                                 where a.AMCST_Id == b.AMCST_Id && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && b.AMSE_Id == e.AMSE_Id
                                                 && b.ACMS_Id == f.ACMS_Id && b.ASMAY_Id == g.ASMAY_Id && a.MI_Id == data.MI_Id
                                                 && a.AMCST_SOL.Equals("S") && a.AMCST_ActiveFlag == true && b.ACYST_ActiveFlag == 1
                                                 && b.AMCO_Id == data.AMCO_Id && grpidsem.Contains(b.AMSE_Id) && GrpId.Contains(b.AMB_Id) && b.ASMAY_Id == data.ASMAY_Id
                                                 select new CollegegeneralsmsDTO
                                                 {
                                                     AMCST_Id = a.AMCST_Id,
                                                     studentName = a.AMCST_FirstName + (string.IsNullOrEmpty(a.AMCST_FirstName) || a.AMCST_MiddleName == "0" ? "" : ' ' + a.AMCST_MiddleName) + (string.IsNullOrEmpty(a.AMCST_LastName) || a.AMCST_LastName == "0" ? "" : ' ' + a.AMCST_LastName),
                                                     AMCST_Admno = a.AMCST_AdmNo,
                                                     AMCST_emailId = a.AMCST_emailId,
                                                     AMCST_MobileNo = Convert.ToInt64(a.AMCST_MobileNo),
                                                     AMB_BranchName = d.AMB_BranchName
                                                 }).Distinct().OrderBy(t => t.studentName).ToListAsync();
                        if (studentlist.Count > 0)
                        {
                            data.studentlist = studentlist.ToArray();
                            data.studentCount = studentlist.Count;
                        }
                    }
                    else
                    {
                        var studentlist = await (from a in _smsContext.Adm_Master_College_StudentDMO
                                                 from b in _smsContext.Adm_College_Yearly_StudentDMO
                                                 from c in _smsContext.MasterCourseDMO
                                                 from d in _smsContext.ClgMasterBranchDMO
                                                 from e in _smsContext.CLG_Adm_Master_SemesterDMO
                                                 from f in _smsContext.Adm_College_Master_SectionDMO
                                                 from g in _smsContext.AcademicYear
                                                 where a.AMCST_Id == b.AMCST_Id && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && b.AMSE_Id == e.AMSE_Id
                                                 && b.ACMS_Id == f.ACMS_Id && b.ASMAY_Id == g.ASMAY_Id && a.MI_Id == data.MI_Id
                                                 && a.AMCST_SOL.Equals("S") && a.AMCST_ActiveFlag == true && b.ACYST_ActiveFlag == 1
                                                 && b.AMCO_Id == data.AMCO_Id && grpidsem.Contains(b.AMSE_Id) && GrpId.Contains(b.AMB_Id) && b.ACMS_Id == data.ACMS_Id
                                                 && b.ASMAY_Id == data.ASMAY_Id
                                                 select new CollegegeneralsmsDTO
                                                 {
                                                     AMCST_Id = a.AMCST_Id,
                                                     studentName = a.AMCST_FirstName + (string.IsNullOrEmpty(a.AMCST_FirstName) || a.AMCST_MiddleName == "0" ? "" : ' ' + a.AMCST_MiddleName) + (string.IsNullOrEmpty(a.AMCST_LastName) || a.AMCST_LastName == "0" ? "" : ' ' + a.AMCST_LastName),
                                                     AMCST_Admno = a.AMCST_AdmNo,
                                                     AMCST_emailId = a.AMCST_emailId,
                                                     AMCST_MobileNo = Convert.ToInt64(a.AMCST_MobileNo),
                                                     AMB_BranchName = d.AMB_BranchName
                                                 }).Distinct().OrderBy(t => t.studentName).ToListAsync();
                        if (studentlist.Count > 0)
                        {
                            data.studentlist = studentlist.ToArray();
                            data.studentCount = studentlist.Count;
                        }
                    }
                }
                else if (data.radiotype == "Father")
                {
                    if (data.ASMS_Id == 0)
                    {
                        var studentlist = await (from a in _smsContext.Adm_Master_College_StudentDMO
                                                 from b in _smsContext.Adm_College_Yearly_StudentDMO
                                                 from c in _smsContext.MasterCourseDMO
                                                 from d in _smsContext.ClgMasterBranchDMO
                                                 from e in _smsContext.CLG_Adm_Master_SemesterDMO
                                                 from f in _smsContext.Adm_College_Master_SectionDMO
                                                 from g in _smsContext.AcademicYear
                                                 where a.AMCST_Id == b.AMCST_Id && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && b.AMSE_Id == e.AMSE_Id
                                                 && b.ACMS_Id == f.ACMS_Id && b.ASMAY_Id == g.ASMAY_Id && a.MI_Id == data.MI_Id
                                                 && a.AMCST_SOL.Equals("S") && a.AMCST_ActiveFlag == true && b.ACYST_ActiveFlag == 1
                                                 && b.AMCO_Id == data.AMCO_Id && grpidsem.Contains(b.AMSE_Id) && GrpId.Contains(b.AMB_Id) && b.ASMAY_Id == data.ASMAY_Id
                                                 select new CollegegeneralsmsDTO
                                                 {
                                                     AMCST_Id = a.AMCST_Id,
                                                     studentName = a.AMCST_FirstName + (string.IsNullOrEmpty(a.AMCST_FirstName) || a.AMCST_MiddleName == "0" ? "" : ' ' + a.AMCST_MiddleName) + (string.IsNullOrEmpty(a.AMCST_LastName) || a.AMCST_LastName == "0" ? "" : ' ' + a.AMCST_LastName),
                                                     AMCST_Admno = a.AMCST_AdmNo,
                                                     AMCST_emailId = a.AMCST_FatheremailId,
                                                     AMCST_MobileNo = Convert.ToInt64(a.AMCST_FatherMobleNo),
                                                     AMB_BranchName = d.AMB_BranchName

                                                 }).Distinct().OrderBy(t => t.studentName).ToListAsync();
                        if (studentlist.Count > 0)
                        {
                            data.studentlist = studentlist.ToArray();
                            data.studentCount = studentlist.Count;
                        }
                    }
                    else
                    {
                        var studentlist = await (from a in _smsContext.Adm_Master_College_StudentDMO
                                                 from b in _smsContext.Adm_College_Yearly_StudentDMO
                                                 from c in _smsContext.MasterCourseDMO
                                                 from d in _smsContext.ClgMasterBranchDMO
                                                 from e in _smsContext.CLG_Adm_Master_SemesterDMO
                                                 from f in _smsContext.Adm_College_Master_SectionDMO
                                                 from g in _smsContext.AcademicYear
                                                 where a.AMCST_Id == b.AMCST_Id && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && b.AMSE_Id == e.AMSE_Id
                                                 && b.ACMS_Id == f.ACMS_Id && b.ASMAY_Id == g.ASMAY_Id && a.MI_Id == data.MI_Id
                                                 && a.AMCST_SOL.Equals("S") && a.AMCST_ActiveFlag == true && b.ACYST_ActiveFlag == 1
                                                 && b.AMCO_Id == data.AMCO_Id && grpidsem.Contains(b.AMSE_Id) && GrpId.Contains(b.AMB_Id) && b.ACMS_Id == data.ACMS_Id
                                                 && b.ASMAY_Id == data.ASMAY_Id
                                                 select new CollegegeneralsmsDTO
                                                 {
                                                     AMCST_Id = a.AMCST_Id,
                                                     studentName = a.AMCST_FirstName + (string.IsNullOrEmpty(a.AMCST_FirstName) || a.AMCST_MiddleName == "0" ? "" : ' ' + a.AMCST_MiddleName) + (string.IsNullOrEmpty(a.AMCST_LastName) || a.AMCST_LastName == "0" ? "" : ' ' + a.AMCST_LastName),
                                                     AMCST_Admno = a.AMCST_AdmNo,
                                                     AMCST_emailId = a.AMCST_FatheremailId,
                                                     AMCST_MobileNo = Convert.ToInt64(a.AMCST_FatherMobleNo),
                                                     AMB_BranchName = d.AMB_BranchName

                                                 }).Distinct().OrderBy(t => t.studentName).ToListAsync();
                        if (studentlist.Count > 0)
                        {
                            data.studentlist = studentlist.ToArray();
                            data.studentCount = studentlist.Count;
                        }
                    }
                }
                else if (data.radiotype == "Mother")
                {
                    if (data.ASMS_Id == 0)
                    {
                        var studentlist = await (from a in _smsContext.Adm_Master_College_StudentDMO
                                                 from b in _smsContext.Adm_College_Yearly_StudentDMO
                                                 from c in _smsContext.MasterCourseDMO
                                                 from d in _smsContext.ClgMasterBranchDMO
                                                 from e in _smsContext.CLG_Adm_Master_SemesterDMO
                                                 from f in _smsContext.Adm_College_Master_SectionDMO
                                                 from g in _smsContext.AcademicYear
                                                 where a.AMCST_Id == b.AMCST_Id && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && b.AMSE_Id == e.AMSE_Id
                                                 && b.ACMS_Id == f.ACMS_Id && b.ASMAY_Id == g.ASMAY_Id && a.MI_Id == data.MI_Id
                                                 && a.AMCST_SOL.Equals("S") && a.AMCST_ActiveFlag == true && b.ACYST_ActiveFlag == 1
                                                 && b.AMCO_Id == data.AMCO_Id && grpidsem.Contains(b.AMSE_Id) && GrpId.Contains(b.AMB_Id) && b.ASMAY_Id == data.ASMAY_Id
                                                 select new CollegegeneralsmsDTO
                                                 {
                                                     AMCST_Id = a.AMCST_Id,
                                                     studentName = a.AMCST_FirstName + (string.IsNullOrEmpty(a.AMCST_FirstName) || a.AMCST_MiddleName == "0" ? "" : ' ' + a.AMCST_MiddleName) + (string.IsNullOrEmpty(a.AMCST_LastName) || a.AMCST_LastName == "0" ? "" : ' ' + a.AMCST_LastName),
                                                     AMCST_Admno = a.AMCST_AdmNo,
                                                     AMCST_emailId = a.AMCST_MotheremailId,
                                                     AMCST_MobileNo = Convert.ToInt64(a.AMCST_MotherMobleNo),
                                                     AMB_BranchName = d.AMB_BranchName

                                                 }).Distinct().OrderBy(t => t.studentName).ToListAsync();
                        if (studentlist.Count > 0)
                        {
                            data.studentlist = studentlist.ToArray();
                            data.studentCount = studentlist.Count;
                        }
                    }
                    else
                    {
                        var studentlist = await (from a in _smsContext.Adm_Master_College_StudentDMO
                                                 from b in _smsContext.Adm_College_Yearly_StudentDMO
                                                 from c in _smsContext.MasterCourseDMO
                                                 from d in _smsContext.ClgMasterBranchDMO
                                                 from e in _smsContext.CLG_Adm_Master_SemesterDMO
                                                 from f in _smsContext.Adm_College_Master_SectionDMO
                                                 from g in _smsContext.AcademicYear
                                                 where a.AMCST_Id == b.AMCST_Id && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && b.AMSE_Id == e.AMSE_Id
                                                 && b.ACMS_Id == f.ACMS_Id && b.ASMAY_Id == g.ASMAY_Id && a.MI_Id == data.MI_Id
                                                 && a.AMCST_SOL.Equals("S") && a.AMCST_ActiveFlag == true && b.ACYST_ActiveFlag == 1
                                                 && b.AMCO_Id == data.AMCO_Id && grpidsem.Contains(b.AMSE_Id) && GrpId.Contains(b.AMB_Id) && b.ACMS_Id == data.ACMS_Id
                                                 && b.ASMAY_Id == data.ASMAY_Id
                                                 select new CollegegeneralsmsDTO
                                                 {
                                                     AMCST_Id = a.AMCST_Id,
                                                     studentName = a.AMCST_FirstName + (string.IsNullOrEmpty(a.AMCST_FirstName) || a.AMCST_MiddleName == "0" ? "" : ' ' + a.AMCST_MiddleName) + (string.IsNullOrEmpty(a.AMCST_LastName) || a.AMCST_LastName == "0" ? "" : ' ' + a.AMCST_LastName),
                                                     AMCST_Admno = a.AMCST_AdmNo,
                                                     AMCST_emailId = a.AMCST_MotheremailId,
                                                     AMCST_MobileNo = Convert.ToInt64(a.AMCST_MotherMobleNo),
                                                     AMB_BranchName = d.AMB_BranchName

                                                 }).Distinct().OrderBy(t => t.studentName).ToListAsync();
                        if (studentlist.Count > 0)
                        {
                            data.studentlist = studentlist.ToArray();
                            data.studentCount = studentlist.Count;
                        }
                    }
                }
                else if (data.radiotype == "Guardian")
                {
                    if (data.ASMS_Id == 0)
                    {
                        var studentlist = await (from a in _smsContext.Adm_Master_College_StudentDMO
                                                 from b in _smsContext.Adm_College_Yearly_StudentDMO
                                                 from c in _smsContext.MasterCourseDMO
                                                 from d in _smsContext.ClgMasterBranchDMO
                                                 from e in _smsContext.CLG_Adm_Master_SemesterDMO
                                                 from f in _smsContext.Adm_College_Master_SectionDMO
                                                 from g in _smsContext.AcademicYear
                                                 from h in _smsContext.AdmCollegeStudentGuardianDMO
                                                 where a.AMCST_Id == b.AMCST_Id && h.AMCST_Id == b.AMCST_Id && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id
                                                 && b.AMSE_Id == e.AMSE_Id && b.ACMS_Id == f.ACMS_Id && b.ASMAY_Id == g.ASMAY_Id && a.MI_Id == data.MI_Id
                                                 && a.AMCST_SOL.Equals("S") && a.AMCST_ActiveFlag == true && b.ACYST_ActiveFlag == 1
                                                 && b.AMCO_Id == data.AMCO_Id && grpidsem.Contains(b.AMSE_Id) && GrpId.Contains(b.AMB_Id) && b.ASMAY_Id == data.ASMAY_Id
                                                 select new CollegegeneralsmsDTO
                                                 {
                                                     AMCST_Id = a.AMCST_Id,
                                                     studentName = a.AMCST_FirstName + (string.IsNullOrEmpty(a.AMCST_FirstName) || a.AMCST_MiddleName == "0" ? "" : ' ' + a.AMCST_MiddleName) + (string.IsNullOrEmpty(a.AMCST_LastName) || a.AMCST_LastName == "0" ? "" : ' ' + a.AMCST_LastName),
                                                     AMCST_Admno = a.AMCST_AdmNo,
                                                     AMCST_emailId = h.ACSTG_emailid,
                                                     AMCST_MobileNo = Convert.ToInt64(h.ACSTG_GuardianPhoneNo),
                                                     AMB_BranchName = d.AMB_BranchName

                                                 }).Distinct().OrderBy(t => t.studentName).ToListAsync();

                        if (studentlist.Count > 0)
                        {
                            data.studentlist = studentlist.ToArray();
                            data.studentCount = studentlist.Count;
                        }
                    }
                    else
                    {
                        var studentlist = await (from a in _smsContext.Adm_Master_College_StudentDMO
                                                 from b in _smsContext.Adm_College_Yearly_StudentDMO
                                                 from c in _smsContext.MasterCourseDMO
                                                 from d in _smsContext.ClgMasterBranchDMO
                                                 from e in _smsContext.CLG_Adm_Master_SemesterDMO
                                                 from f in _smsContext.Adm_College_Master_SectionDMO
                                                 from g in _smsContext.AcademicYear
                                                 where a.AMCST_Id == b.AMCST_Id && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && b.AMSE_Id == e.AMSE_Id
                                                 && b.ACMS_Id == f.ACMS_Id && b.ASMAY_Id == g.ASMAY_Id && a.MI_Id == data.MI_Id
                                                 && a.AMCST_SOL.Equals("S") && a.AMCST_ActiveFlag == true && b.ACYST_ActiveFlag == 1
                                                 && b.AMCO_Id == data.AMCO_Id && grpidsem.Contains(b.AMSE_Id) && GrpId.Contains(b.AMB_Id) && b.ACMS_Id == data.ACMS_Id
                                                 && b.ASMAY_Id == data.ASMAY_Id
                                                 select new CollegegeneralsmsDTO
                                                 {
                                                     AMCST_Id = a.AMCST_Id,
                                                     studentName = a.AMCST_FirstName + (string.IsNullOrEmpty(a.AMCST_FirstName) || a.AMCST_MiddleName == "0" ? "" : ' ' + a.AMCST_MiddleName) + (string.IsNullOrEmpty(a.AMCST_LastName) || a.AMCST_LastName == "0" ? "" : ' ' + a.AMCST_LastName),
                                                     AMCST_Admno = a.AMCST_AdmNo,
                                                     AMCST_emailId = a.AMCST_emailId,
                                                     AMCST_MobileNo = Convert.ToInt64(a.AMCST_MobileNo),
                                                     AMB_BranchName = d.AMB_BranchName

                                                 }).Distinct().OrderBy(t => t.studentName).ToListAsync();
                        if (studentlist.Count > 0)
                        {
                            data.studentlist = studentlist.ToArray();
                            data.studentCount = studentlist.Count;
                        }
                    }
                }           
            }
            catch (Exception ex)
            {
                data.studentCount = 0;
                Console.WriteLine(ex.Message);
                throw;
            }

            return data;
        }

        // ****** For Staff Radio Button Select ********// 
        public CollegegeneralsmsDTO Getdepartment(CollegegeneralsmsDTO data)
        {
            var departmentdropdown = _smsContext.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id && t.HRMD_ActiveFlag == true).ToList();
            data.departmentdropdown = departmentdropdown.ToArray();
            return data;
        }
        public CollegegeneralsmsDTO get_designation(CollegegeneralsmsDTO data)
        {
            data.designationdropdown = (from a in _smsContext.HR_Master_Employee_DMO//MasterEmployee
                                        from b in _smsContext.HR_Master_Designation
                                        from c in _smsContext.HR_Master_Department
                                        where (a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == c.HRMD_Id && c.HRMD_ActiveFlag == true
                                        && b.HRMDES_ActiveFlag == true && a.HRME_ActiveFlag == true
                                        && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && data.multipledep.ToString().Contains(Convert.ToString(c.HRMD_Id)))
                                        select new CollegegeneralsmsDTO
                                        {
                                            HRMDES_Id = b.HRMDES_Id,
                                            HRMDES_DesignationName = b.HRMDES_DesignationName,
                                        }).Distinct().ToArray();
            return data;
        }
        public CollegegeneralsmsDTO get_employee(CollegegeneralsmsDTO data)
        {
            data.stafflist = (from a in _smsContext.HR_Master_Employee_DMO
                              from b in _smsContext.Multiple_Mobile_DMO
                              from c in _smsContext.Multiple_Email_DMO//MasterEmployee
                              where (a.HRME_Id == b.HRME_Id && a.HRME_Id == c.HRME_Id && a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true
                              && a.HRME_LeftFlag == false && b.HRMEMNO_DeFaultFlag == "default"
                              && c.HRMEM_DeFaultFlag == "default" && data.multipledes.ToString().Contains(Convert.ToString(a.HRMDES_Id))
                              && data.multipledep.ToString().Contains(Convert.ToString(a.HRMD_Id)))
                              select new CollegegeneralsmsDTO
                              {
                                  HRME_Id = a.HRME_Id,
                                  HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName) +
                                  (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName) + " : " + (a.HRME_EmployeeCode == null || a.HRME_EmployeeCode == "" ? "" : " " + a.HRME_EmployeeCode)),
                                  HRME_MobileNo = b.HRMEMNO_MobileNo,
                                  hrm_email = c.HRMEM_EmailId
                              }).Distinct().OrderBy(t => t.HRME_EmployeeFirstName).ToArray();
            return data;
        }
        public CollegegeneralsmsDTO GetEmployeeDetailsByLeaveYearAndMonth(CollegegeneralsmsDTO data)
        {
            //List<MasterEmployee> employe = new List<MasterEmployee>();
            List<HR_Master_Employee_DMO> employe = new List<HR_Master_Employee_DMO>();

            try
            {
                employe = (from a in _smsContext.HR_Master_Employee_DMO//MasterEmployee

                           where (a.MI_Id.Equals(data.MI_Id)) && a.HRME_ActiveFlag == true
                           select a).Distinct().ToList();

                if (employe.Count > 0)
                {
                    employe = employe.Where(a => a.HRME_LeftFlag == false).ToList();


                    if (data.hrmdeS_IdList.Count() > 0 && data.hrmD_IdList.Count() > 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(data.MI_Id) && data.hrmdeS_IdList.Contains(t.HRMDES_Id) && data.hrmD_IdList.Contains(t.HRMD_Id)).ToList();

                    }
                    else if (data.hrmdeS_IdList.Count() > 0 && data.hrmD_IdList.Count() > 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(data.MI_Id) && data.hrmdeS_IdList.Contains(t.HRMDES_Id) && data.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                    }
                    else if (data.hrmdeS_IdList.Count() > 0 && data.hrmD_IdList.Count() == 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(data.MI_Id) && data.hrmdeS_IdList.Contains(t.HRMDES_Id)).ToList();
                    }
                    else if (data.hrmdeS_IdList.Count() == 0 && data.hrmD_IdList.Count() > 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(data.MI_Id) && data.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                    }
                    else if (data.hrmdeS_IdList.Count() > 0 && data.hrmD_IdList.Count() == 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(data.MI_Id) && data.hrmdeS_IdList.Contains(t.HRMDES_Id)).ToList();
                    }
                    else if (data.hrmdeS_IdList.Count() == 0 && data.hrmD_IdList.Count() > 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(data.MI_Id) && data.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                    }

                    else if (data.hrmdeS_IdList.Count() == 0 && data.hrmD_IdList.Count() == 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(data.MI_Id)).ToList();
                    }
                    data.employeedropdown = employe.ToArray();
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

        //******* Sending The SMS and Email  ******** //
        public async Task<CollegegeneralsmsDTO> savedetail(CollegegeneralsmsDTO data)
        {
            long trnsno = 0;
            SMS sms1 = new SMS(_db);
            trnsno = sms1.getsmsno(data.MI_Id);
            if (data.radiotype == "General")
            {
                //SmsWithoutTemplate sms = new SmsWithoutTemplate(_db);


                //string s = await sms.sendsmsfromPortal(data.MI_Id, Convert.ToInt64(data.Mobno), data.mes);
                string s = await sms1.sendSmsnewwithouttemplete_newtable(data.MI_Id, Convert.ToInt64(data.Mobno), data.template, data.Userid, data.mes, trnsno, "test", data.Userid);
                if (s.Equals("Success"))
                {
                    data.smsStatus = "sent";
                }
                else
                {
                    data.smsStatus = "failed";
                }
            }

            else if (data.radiotype == "Student" || data.radiotype == "Father" || data.radiotype == "Mother" || data.radiotype == "Guardian")
            {
                if (data.studentlistdto.Length > 0)
                {
                    SmsWithoutTemplate sms = new SmsWithoutTemplate(_db);
                    EmailWithoutTemplate email = new EmailWithoutTemplate(_db);
                    string e = string.Empty;
                    for (int i = 0; i < data.studentlistdto.Length; i++)
                    {
                        string emailtext = data.SmsMailText;
                        if (data.snd_sms == true)
                        {
                            if (data.studentlistdto[i].AMCST_MobileNo != 0)
                            {
                                //data.studentlistdto[i].AMCST_MobileNo = 9591081840;
                                e = await sms1.sendSmsnewwithouttemplete_newtable(data.MI_Id, Convert.ToInt64(data.studentlistdto[i].AMCST_MobileNo), data.template, data.Userid, data.SmsMailText, trnsno, "test", data.Userid);
                            }
                        }
                        if (data.snd_email == true)
                        {
                            if (data.studentlistdto[i].AMCST_emailId != null && data.studentlistdto[i].AMCST_emailId != "")
                            {
                                //data.studentlistdto[i].AMCST_emailId = "praveenishwar@vapstech.com";
                                e = Sendgenemail(data.MI_Id, data.studentlistdto[i].studentName, data.studentlistdto[i].AMCST_emailId, emailtext);
                            }
                        }

                    }
                    if (e.Equals("Success"))
                    {
                        data.emailStatus = "sent";
                    }
                    else
                    {
                        data.emailStatus = "failed";
                    }
                }
            }

            else if (data.radiotype == "Staff")
            {

                string s = string.Empty;
                for (int i = 0; i < data.studentlistdto.Length; i++)
                {
                    string stfemltext = string.Empty;
                    stfemltext = data.SmsMailText;
                    if (data.stfsnd_sms == true)
                    {
                        //data.studentlistdto[i].HRME_MobileNo = 9591081840;
                        if (data.studentlistdto[i].HRME_MobileNo != 0)
                        {
                            //data.SmsMailText = data.studentlistdto[i].HRME_EmployeeFirstName + "," + data.SmsMailText;
                            data.SmsMailText = "" + data.SmsMailText;

                            s = await sms1.sendSmsnewwithouttemplete_newtable(data.MI_Id, Convert.ToInt64(data.studentlistdto[i].HRME_MobileNo), data.template, data.Userid, data.SmsMailText, trnsno, "Test", data.Userid);


                        }
                    }
                    if (data.stfsnd_email == true)
                    {
                        if (data.studentlistdto[i].hrm_email != "")
                        {
                            //data.studentlistdto[i].hrm_email = "praveenishwar@vapstech.com";
                            s = Sendgenemail(data.MI_Id, data.studentlistdto[i].HRME_EmployeeFirstName, data.studentlistdto[i].hrm_email, stfemltext);
                        }
                    }

                }
                if (s.Equals("Success"))
                {
                    data.smsStatus = "sent";
                }
                else
                {
                    data.smsStatus = "failed";
                }

            }

            return data;
        }
        public string Sendgenemail(long mi_id, string name, string emailid, string msg)
        {

            try
            {
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID == mi_id).ToList();
                var institutionName = _db.Institution.Where(m => m.MI_Id == mi_id).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    string Subject = "Email From " + institutionName.FirstOrDefault().MI_Name;

                    //Sending mail using SendGrid API.
                    //Date:07-02-2017.

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName.FirstOrDefault().MI_Name);
                    message.Subject = Subject;
                    message.AddTo(emailid);
                    string body = "<div>" + msg + "</div>";
                    string footer = "<br />" + " Thanks and Regards" + "<br />" + "<div>" + institutionName.FirstOrDefault().MI_Name + "</div>";
                    message.HtmlContent = body + footer;
                    var client = new SendGridClient(sengridkey);
                    client.SendEmailAsync(message).Wait();

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = emailid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = msg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = "Admission"
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = mi_id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return e.Message;

            }
            return "Success";
        }
        public async Task<string> sendgeneralsms(long MI_Id, long mobileNo, string message)
        {

            try
            {
                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
                Dictionary<string, string> val = new Dictionary<string, string>();

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                string url = alldetails[0].IVRMSD_URL.ToString();

                string PHNO = mobileNo.ToString();

                url = url.Replace("PHNO", PHNO);

                url = url.Replace("MESSAGE", message);

                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                Stream stream = response.GetResponseStream();

                StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                string responseparameters = readStream.ReadToEnd();

                if (responseparameters != null)
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = message
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = "Admission"
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = responseparameters
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "Success";
        }
    }
}
