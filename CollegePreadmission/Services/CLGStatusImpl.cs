using AutoMapper;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DataAccessMsSqlServerProvider.com.vapstech.College.Preadmission;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Fees;
using DomainModel.Model.com.vapstech.College.Preadmission;
using DomainModel.Model.com.vapstech.Fee;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Admission;
using PreadmissionDTOs.com.vaps.College.Fees;
using PreadmissionDTOs.com.vaps.College.Preadmission;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using paytm.security;
using paytm.util;
using Org.BouncyCastle.Crypto;

namespace CollegePreadmission.Services
{
    public class CLGStatusImpl : Interfaces.CLGStatusInterface
    {
        ClgAdmissionContext _context;
        CollegepreadmissionContext _precontext;
        private readonly DomainModelMsSqlServerContext _db;
        CollFeeGroupContext _feecontext;
        public CLGStatusImpl(ClgAdmissionContext context, CollegepreadmissionContext precontext, DomainModelMsSqlServerContext db, CollFeeGroupContext feecontext)
        {
            _context = context;
            _precontext = precontext;
            _db = db;
            _feecontext = feecontext;
        }
        public CollegePreadmissionstudnetDto Getdetails(CollegePreadmissionstudnetDto obj)
        {

            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                //allyear = _db.AcademicYear.Where(t => t.MI_Id == mi_id && t.Is_Active == true).ToList();
                //ctdo.AcademicList = allyear.ToArray();

                allyear = (from a in _db.AcademicYear
                           where (a.MI_Id == obj.MI_Id && a.ASMAY_Pre_ActiveFlag == 1 && a.Is_Active == true && a.MI_Id == obj.MI_Id)
                           select new MasterAcademic
                           {
                               ASMAY_Id = a.ASMAY_Id,
                               ASMAY_Year = a.ASMAY_Year
                           }
                      ).ToList();
                obj.AllAcademicYear = allyear.ToArray();

                List<AdmissionStatus> status = new List<AdmissionStatus>();
                status = _db.status.Where(t => t.MI_Id == obj.MI_Id).ToList();
                obj.statuslist = status.ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return obj;
        }
        public CollegePreadmissionstudnetDto getCourse(CollegePreadmissionstudnetDto data)
        {
            try
            {
                var course = (from m in _context.CLG_Adm_College_AY_CourseDMO
                              from n in _context.MasterCourseDMO
                              where m.AMCO_Id == n.AMCO_Id && m.MI_Id == data.MI_Id && m.ASMAY_Id == data.ASMAY_Id && m.ACAYC_ActiveFlag == true && n.AMCO_ActiveFlag == true
                              group new { m, n } by m.AMCO_Id into g
                              select new CollegePreadmissionstudnetDto
                              {
                                  AMCO_Id = g.FirstOrDefault().m.AMCO_Id,
                                  ACAYC_Id = g.FirstOrDefault().m.ACAYC_Id,
                                  courseName = g.FirstOrDefault().n.AMCO_CourseName,
                                  ASMAY_Id = g.FirstOrDefault().m.ASMAY_Id
                              }).Distinct().ToList();
                if (course.Count > 0)
                {
                    data.courses = course.ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegePreadmissionstudnetDto getBranch(CollegePreadmissionstudnetDto data)
        {
            try
            {
                var branch = (from m in _context.CLG_Adm_College_AY_Course_BranchDMO
                              from a in _context.CLG_Adm_College_AY_CourseDMO
                              from n in _context.ClgMasterBranchDMO
                              from b in _context.MasterCourseDMO
                              where a.ACAYC_Id == m.ACAYC_Id && a.AMCO_Id == b.AMCO_Id && m.AMB_Id == n.AMB_Id && m.MI_Id == data.MI_Id && a.AMCO_Id == data.AMCO_Id
                              && m.ACAYCB_ActiveFlag == true && n.AMB_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id
                              select new AdmMasterCollegeStudentDTO
                              {
                                  AMB_Id = m.AMB_Id,
                                  branchName = n.AMB_BranchName,
                                  ACAYCB_Id = m.ACAYCB_Id,
                                  ACAYC_Id = m.ACAYC_Id
                              }).Distinct().ToArray();
                if (branch.Length > 0)
                {
                    data.branches = branch;
                }
                var studentCategory = (from m in _context.ClgMasterCourseCategorycategoryMap
                                       from n in _context.mastercategory
                                       where m.AMCOC_Id == n.AMCOC_Id && m.MI_Id == data.MI_Id && m.AMCO_Id == data.AMCO_Id && m.AMCOCM_ActiveFlg == true
                                       select new AdmMasterCollegeStudentDTO
                                       {
                                           AMCOC_Id = m.AMCOC_Id,
                                           AMCOC_Name = n.AMCOC_Name
                                       }).ToArray();
                if (studentCategory.Length > 0)
                {
                    data.studentCategory = studentCategory;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public CollegePreadmissionstudnetDto SearchData(CollegePreadmissionstudnetDto cdto)
        {
            try
            {
                List<CollegePreadmissionstudnetDto> result = new List<CollegePreadmissionstudnetDto>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    //changed by suryan
                    cmd.CommandText = "Get_Student_Status";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Ids", SqlDbType.Int) { Value = cdto.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMCOC_Id", SqlDbType.Int) { Value = cdto.AMCO_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.Int) { Value = cdto.AMB_Id });
                    cmd.Parameters.Add(new SqlParameter("@PAMST_Ids", SqlDbType.Int) { Value = cdto.PAMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.Int) { Value = cdto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@type_", SqlDbType.VarChar) { Value = cdto.status_type });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new CollegePreadmissionstudnetDto
                                {
                                    //pasR_Id = Convert.ToInt64(dataReader["PASR_Id"]),
                                    //PAMST_Id = Convert.ToInt64(dataReader["PAMS_Id"]),
                                    //remark = (dataReader["Remark"]).ToString(),
                                    //PASR_FirstName = (dataReader["PASR_FirstName"]).ToString(),
                                    //PASR_MiddleName = (dataReader["PASR_MiddleName"]).ToString(),
                                    //PASR_LastName = Convert.ToString(dataReader["PASR_LastName"]),
                                    //ASMCL_Id = Convert.ToInt64(dataReader["ASMCL_Id"]),
                                    //className = dataReader["ASMCL_ClassName"].ToString(),
                                    //statusName = dataReader["PAMST_Status"].ToString(),
                                    //statusFlag = dataReader["PAMST_StatusFlag"].ToString(),
                                    //PASR_Sex = dataReader["PASR_Sex"].ToString(),
                                    //PASR_RegistrationNo = dataReader["PASR_RegistrationNo"].ToString(),
                                    ////PASR_emailId = dataReader["PASR_emailId"].ToString(),
                                    ////PASR_MobileNo = Convert.ToInt64(dataReader["PASR_MobileNo"]),
                                    //Repeat_Class_Id = Convert.ToInt64(dataReader["Repeat_Class_Id"])
                                });
                                cdto.StudentList = result.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return cdto;
        }




        //master competitive exam 
        public Master_Competitive_ExamsClgDTO getexamdetails(Master_Competitive_ExamsClgDTO obj)
        {

            try
            {
                obj.examdetailsarray = (from a in _precontext.Master_Competitive_ExamsClgDMO
                                        where (a.PAMCEXM_ActiveFlg == true && a.MI_Id == obj.MI_Id)
                                        select new Master_Competitive_ExamsClgDTO
                                        {
                                            PAMCEXM_Id = a.PAMCEXM_Id,
                                            PAMCEXM_CompetitiveExams = a.PAMCEXM_CompetitiveExams,
                                            PAMCEXM_CompulsoryFlg = a.PAMCEXM_CompulsoryFlg
                                        }).Distinct().ToArray();
                obj.subdetailsarray = (from a in _precontext.Master_CompetitiveExamsSubjectsClgDMO
                                       from b in _precontext.Master_Competitive_ExamsClgDMO
                                       where (a.PAMCEXM_Id == b.PAMCEXM_Id && b.PAMCEXM_ActiveFlg == true && b.MI_Id == obj.MI_Id && a.PAMCEXMSUB_ActiveFlg == true)
                                       select new Master_Competitive_ExamsClgDTO
                                       {
                                           PAMCEXMSUB_Id = a.PAMCEXMSUB_Id,
                                           PAMCEXMSUB_SubjectName = a.PAMCEXMSUB_SubjectName,
                                           PAMCEXM_CompetitiveExams = b.PAMCEXM_CompetitiveExams,
                                           PAMCEXMSUB_MaxMarks = a.PAMCEXMSUB_MaxMarks
                                       }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return obj;
        }

        public Master_Competitive_ExamsClgDTO saveExamDetails(Master_Competitive_ExamsClgDTO page)
        {
            bool returnresult = false;
            page.returnMsg = "";
            try
            {
                Master_Competitive_ExamsClgDMO maspge = Mapper.Map<Master_Competitive_ExamsClgDMO>(page);


                var resultCount = _precontext.Master_Competitive_ExamsClgDMO.Where(t => t.PAMCEXM_CompetitiveExams == maspge.PAMCEXM_CompetitiveExams && t.MI_Id == page.MI_Id && t.PAMCEXM_ActiveFlg == true).Count();

                if (resultCount == 0)
                {
                    if (page.PAMCEXM_Id > 0)
                    {
                        var result = _precontext.Master_Competitive_ExamsClgDMO.Single(t => t.PAMCEXM_Id == maspge.PAMCEXM_Id);

                        result.PAMCEXM_CompetitiveExams = maspge.PAMCEXM_CompetitiveExams;
                        result.PAMCEXM_CompulsoryFlg = maspge.PAMCEXM_CompulsoryFlg;
                        result.PAMCEXM_ActiveFlg = true;


                        //added by 02/02/2017

                        result.PAMCEXM_UpdatedDate = DateTime.Now;
                        _precontext.Update(result);
                        var contactExists = _precontext.SaveChanges();

                        if (contactExists == 1)
                        {
                            returnresult = true;
                            page.returnval = returnresult;
                            page.returnMsg = "update";
                        }
                        else
                        {
                            returnresult = false;
                            page.returnval = returnresult;
                        }
                    }
                    else
                    {
                       

                        maspge.PAMCEXM_CreatedDate = DateTime.Now;
                        maspge.PAMCEXM_UpdatedDate = DateTime.Now;
                        maspge.PAMCEXM_ActiveFlg = true;
                        _precontext.Add(maspge);
                        var contactExists = _precontext.SaveChanges();

                        if (contactExists == 1)
                        {
                            returnresult = true;
                            page.returnval = returnresult;
                            page.returnMsg = "add";
                        }
                        else
                        {
                            returnresult = false;
                            page.returnval = returnresult;
                        }
                    }
                }
                else
                {
                    page.returnMsg = "duplicate";
                    return page;

                }

                List<Master_Competitive_ExamsClgDMO> allpages = new List<Master_Competitive_ExamsClgDMO>();
                allpages = _precontext.Master_Competitive_ExamsClgDMO.ToList();
                page.pagesdata = allpages.OrderByDescending(c => c.PAMCEXM_CreatedDate).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return page;
        }


        public Master_Competitive_ExamsClgDTO saveExamMapDetails(Master_Competitive_ExamsClgDTO page)
        {
            bool returnresult = false;
            page.returnMsg = "";
            try
            {
                Master_CompetitiveExamsSubjectsClgDMO maspge = Mapper.Map<Master_CompetitiveExamsSubjectsClgDMO>(page);

                if (page.PAMCEXMSUB_Id > 0)
                {
                    var resultCount = _precontext.Master_CompetitiveExamsSubjectsClgDMO.Where(t => t.PAMCEXMSUB_SubjectName == maspge.PAMCEXMSUB_SubjectName && t.PAMCEXM_Id != page.PAMCEXM_Id && t.PAMCEXMSUB_ActiveFlg == true).Count();

                    if (resultCount == 0)
                    {

                        var result = _precontext.Master_CompetitiveExamsSubjectsClgDMO.Single(t => t.PAMCEXMSUB_Id == maspge.PAMCEXMSUB_Id);

                        result.PAMCEXMSUB_SubjectName = maspge.PAMCEXMSUB_SubjectName;
                        result.PAMCEXM_Id = maspge.PAMCEXM_Id;
                        result.PAMCEXMSUB_MaxMarks = maspge.PAMCEXMSUB_MaxMarks;
                        result.PAMCEXMSUB_ActiveFlg = true;
                        result.PAMCEXMSUB_UpdatedDate = DateTime.Now;
                        _precontext.Update(result);
                        var contactExists = _precontext.SaveChanges();

                        if (contactExists == 1)
                        {
                            returnresult = true;
                            page.returnval = returnresult;
                            page.returnMsg = "update";
                        }
                        else
                        {
                            returnresult = false;
                            page.returnval = returnresult;
                        }
                    }
                    else
                    {
                        page.returnMsg = "duplicate";
                        return page;
                    }
                }
                else
                {
                    var resultCount = _precontext.Master_CompetitiveExamsSubjectsClgDMO.Where(t => t.PAMCEXMSUB_SubjectName == maspge.PAMCEXMSUB_SubjectName && t.PAMCEXM_Id == maspge.PAMCEXM_Id && t.PAMCEXMSUB_ActiveFlg == true).Count();

                    if (resultCount > 0)
                    {
                        page.returnMsg = "duplicate";
                        return page;
                    }
                    //added by 02/02/2017
                    maspge.PAMCEXMSUB_CreatedDate = DateTime.Now;
                    maspge.PAMCEXMSUB_UpdatedDate = DateTime.Now;
                    maspge.PAMCEXMSUB_ActiveFlg = true;
                    _precontext.Add(maspge);
                    var contactExists = _precontext.SaveChanges();

                    if (contactExists == 1)
                    {
                        returnresult = true;
                        page.returnval = returnresult;
                        page.returnMsg = "add";
                    }
                    else
                    {
                        returnresult = false;
                        page.returnval = returnresult;
                    }
                }

                List<Master_CompetitiveExamsSubjectsClgDMO> allpages = new List<Master_CompetitiveExamsSubjectsClgDMO>();
                allpages = _precontext.Master_CompetitiveExamsSubjectsClgDMO.ToList();
                page.pagesdatatwo = allpages.OrderByDescending(c => c.PAMCEXMSUB_CreatedDate).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return page;
        }

        public Master_Competitive_ExamsClgDTO getexamedit(int id)
        {
            Master_Competitive_ExamsClgDTO page = new Master_Competitive_ExamsClgDTO();
            try
            {
                List<Master_Competitive_ExamsClgDMO> lorg = new List<Master_Competitive_ExamsClgDMO>();
                lorg = _precontext.Master_Competitive_ExamsClgDMO.AsNoTracking().Where(t => t.PAMCEXM_Id.Equals(id)).ToList();
                page.pagesdata = lorg.OrderByDescending(c => c.PAMCEXM_CreatedDate).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }


        public Master_Competitive_ExamsClgDTO deleterecord(int id)
        {

            List<Master_Competitive_ExamsClgDMO> mastersect = new List<Master_Competitive_ExamsClgDMO>(); // Mapper.Map<Organisation>(org);
            Master_Competitive_ExamsClgDMO maspge = new Master_Competitive_ExamsClgDMO();

            Master_Competitive_ExamsClgDTO mas = new Master_Competitive_ExamsClgDTO();

            try
            {
                var result = _precontext.Master_Competitive_ExamsClgDMO.Single(t => t.PAMCEXM_Id == id);

                if (result.PAMCEXM_ActiveFlg == true)
                {
                    result.PAMCEXM_ActiveFlg = false;
                    result.PAMCEXM_UpdatedDate = DateTime.Now;
                    result.PAMCEXM_CreatedDate = result.PAMCEXM_CreatedDate;
                    _precontext.Update(result);
                    _precontext.SaveChanges();
                    mas.returnval = true;
                }
                else
                {
                    result.PAMCEXM_UpdatedDate = DateTime.Now;
                    result.PAMCEXM_CreatedDate = result.PAMCEXM_CreatedDate;
                    result.PAMCEXM_ActiveFlg = true;
                    _precontext.Update(result);
                    _precontext.SaveChanges();
                    mas.returnval = false;
                }
                List<Master_Competitive_ExamsClgDMO> allmasterexam = new List<Master_Competitive_ExamsClgDMO>();
                allmasterexam = _precontext.Master_Competitive_ExamsClgDMO.Where(d => d.MI_Id == mas.MI_Id).ToList();
                mas.MasterExamData = allmasterexam.OrderByDescending(a => a.PAMCEXM_CreatedDate).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                //master.returnval = ee.Message;
            }

            return mas;
        }


        public Master_Competitive_ExamsClgDTO getsubedit(int id)
        {
            Master_Competitive_ExamsClgDTO page = new Master_Competitive_ExamsClgDTO();
            try
            {
                List<Master_CompetitiveExamsSubjectsClgDMO> lorgsub = new List<Master_CompetitiveExamsSubjectsClgDMO>();
                lorgsub = _precontext.Master_CompetitiveExamsSubjectsClgDMO.AsNoTracking().Where(t => t.PAMCEXMSUB_Id.Equals(id)).ToList();
                page.pagesdata = lorgsub.OrderByDescending(c => c.PAMCEXMSUB_CreatedDate).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }


        public Master_Competitive_ExamsClgDTO deleterecordsub(int id)
        {

            List<Master_CompetitiveExamsSubjectsClgDMO> mastersect = new List<Master_CompetitiveExamsSubjectsClgDMO>(); // Mapper.Map<Organisation>(org);
            Master_CompetitiveExamsSubjectsClgDMO maspge = new Master_CompetitiveExamsSubjectsClgDMO();

            Master_Competitive_ExamsClgDTO mas = new Master_Competitive_ExamsClgDTO();

            try
            {
                var result = _precontext.Master_CompetitiveExamsSubjectsClgDMO.Single(t => t.PAMCEXMSUB_Id == id);

                if (result.PAMCEXMSUB_ActiveFlg == true)
                {
                    result.PAMCEXMSUB_ActiveFlg = false;
                    result.PAMCEXMSUB_UpdatedDate = DateTime.Now;
                    result.PAMCEXMSUB_CreatedDate = result.PAMCEXMSUB_CreatedDate;
                    _precontext.Update(result);
                    _precontext.SaveChanges();
                    mas.returnval = true;
                }
                else
                {
                    result.PAMCEXMSUB_UpdatedDate = DateTime.Now;
                    result.PAMCEXMSUB_CreatedDate = result.PAMCEXMSUB_CreatedDate;
                    result.PAMCEXMSUB_ActiveFlg = true;
                    _precontext.Update(result);
                    _precontext.SaveChanges();
                    mas.returnval = false;
                }
                //List<Master_Competitive_ExamsClgDMO> allmasterexam = new List<Master_Competitive_ExamsClgDMO>();
                //allmasterexam = _precontext.Master_Competitive_ExamsClgDMO.Where(d => d.MI_Id == mas.MI_Id).ToList();
                //mas.MasterExamData = allmasterexam.OrderByDescending(a => a.PAMCEXM_CreatedDate).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                //master.returnval = ee.Message;
            }

            return mas;
        }

    }
}
