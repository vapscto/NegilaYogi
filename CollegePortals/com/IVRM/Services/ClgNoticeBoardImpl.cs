using PreadmissionDTOs.com.vaps.College.Portals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using DataAccessMsSqlServerProvider.com.vapstech.College.Portal;
using PreadmissionDTOs.com.vaps.College.Portals.Student;
using PreadmissionDTOs.com.vaps.College.Portals.IVRM;
using DomainModel.Model.com.vapstech.College.Portals.IVRM;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Portals.Employee;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Employee;

namespace CollegePortals.com.Student.Services
{
    public class ClgNoticeBoardImpl : Interfaces.ClgNoticeBoardInterface
    {
        private static ConcurrentDictionary<string, ClgNoticeBoardDTO> _login =
           new ConcurrentDictionary<string, ClgNoticeBoardDTO>();
        private CollegeportalContext _ClgPortalContext;
        public ClgNoticeBoardImpl(CollegeportalContext ClgPortalContext)
        {
            _ClgPortalContext = ClgPortalContext;
        }

        public ClgNoticeBoardDTO getloaddata(ClgNoticeBoardDTO data)
        {
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _ClgPortalContext.AcademicYear.Where(y => y.MI_Id == data.MI_Id && y.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.fillyear = year.ToArray();


                data.noticelist = _ClgPortalContext.IVRM_NoticeBoardDMO.Where(n => n.MI_Id == data.MI_Id).Distinct().OrderBy(i => i.INTB_Id).ToArray();

                data.fee_group = (from a in _ClgPortalContext.FeeGroupDMO
                                  where (a.MI_Id == data.MI_Id)
                                  select new ClgNoticeBoardDTO
                                  {
                                      FMG_Id = a.FMG_Id,
                                      FMG_Name = a.FMG_GroupName
                                  }).Distinct().ToArray();


                data.fee_heads = (from a in _ClgPortalContext.FeeHeadsDMO
                                  where (a.MI_Id == data.MI_Id)
                                  select new ClgNoticeBoardDTO
                                  {
                                      FMH_Id = a.FMH_Id,
                                      FMH_FeeName = a.FMH_FeeName
                                  }).Distinct().ToArray();


                using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_DepartmentList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@role",
      SqlDbType.VarChar)
                    {
                        Value = "a"
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
SqlDbType.BigInt)
                    {
                        Value = 1
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.departmentList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgNoticeBoardDTO getcoursedata(ClgNoticeBoardDTO data)
        {
            try
            {
                data.course_list = (from a in _ClgPortalContext.CLG_Adm_College_AY_CourseDMO
                                    from c in _ClgPortalContext.academicYearDMO
                                    from d in _ClgPortalContext.MasterCourseDMO
                                    where (a.MI_Id == c.MI_Id && a.AMCO_Id == d.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && d.AMCO_ActiveFlag == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                    select new ClgNoticeBoardDTO
                                    {
                                        AMCO_Id = d.AMCO_Id,
                                        AMCO_CourseName = d.AMCO_CourseName,
                                        AMCO_CourseCode = d.AMCO_CourseCode,
                                        AMCO_CourseFlag = d.AMCO_CourseFlag,
                                        AMCO_ActiveFlag = d.AMCO_ActiveFlag,
                                        AMCO_Order = d.AMCO_Order
                                    }
                           ).Distinct().OrderBy(c => c.AMCO_Order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ClgNoticeBoardDTO getbranchdata(ClgNoticeBoardDTO data)
        {
            try
            {
                data.branch_list = (from a in _ClgPortalContext.CLG_Adm_College_AY_CourseDMO
                                    from b in _ClgPortalContext.CLG_Adm_College_AY_Course_BranchDMO
                                    from c in _ClgPortalContext.academicYearDMO
                                    from d in _ClgPortalContext.ClgMasterBranchDMO
                                    where (a.ACAYC_Id == b.ACAYC_Id && a.MI_Id == c.MI_Id && a.ASMAY_Id == c.ASMAY_Id && b.AMB_Id == d.AMB_Id && d.AMB_ActiveFlag == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                    select new ClgNoticeBoardDTO
                                    {
                                        AMB_Id = d.AMB_Id,
                                        AMB_BranchName = d.AMB_BranchName,
                                        AMB_BranchCode = d.AMB_BranchCode,
                                        AMB_ActiveFlag = d.AMB_ActiveFlag,
                                        AMB_Order = d.AMB_Order
                                    }
                        ).Distinct().OrderBy(c => c.AMB_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgNoticeBoardDTO getsemdata(ClgNoticeBoardDTO data)
        {
            try
            {
                List<long> ids = new List<long>();
                if (data.branchArray != null)
                {
                    foreach (var c in data.branchArray)
                    {
                        ids.Add(c.AMB_Id);
                    }
                }
                data.sem_list = (from a in _ClgPortalContext.CLG_Adm_College_AY_CourseDMO
                                 from b in _ClgPortalContext.CLG_Adm_College_AY_Course_BranchDMO
                                 from c in _ClgPortalContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                 from d in _ClgPortalContext.academicYearDMO
                                 from e in _ClgPortalContext.CLG_Adm_Master_SemesterDMO
                                 where (a.ACAYC_Id == b.ACAYC_Id && b.ACAYCB_Id == c.ACAYCB_Id && a.MI_Id == c.MI_Id && a.ASMAY_Id == d.ASMAY_Id && c.AMSE_Id == e.AMSE_Id && e.AMSE_ActiveFlg == true && c.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && ids.Contains(b.AMB_Id))
                                 select new ClgNoticeBoardDTO
                                 {
                                     AMSE_Id = e.AMSE_Id,
                                     AMSE_SEMName = e.AMSE_SEMName,
                                     AMSE_SEMCode = e.AMSE_SEMCode,
                                     AMSE_SEMOrder = e.AMSE_SEMOrder
                                 }
                       ).Distinct().OrderBy(c => c.AMSE_SEMOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //public ClgNoticeBoardDTO savedata(ClgNoticeBoardDTO data)
        //{
        //    try
        //    {
        //        int returnval = 0;
        //        if (data.INTB_Id > 0)
        //        {
        //            var ChkDuplicate = _ClgPortalContext.IVRM_NoticeBoardDMO.Where(t => t.INTB_Title.Equals(data.INTB_Title) && t.INTB_Attachment.Equals(data.INTB_Attachment) && t.MI_Id.Equals(data.MI_Id) && t.INTB_StartDate.Equals(data.INTB_StartDate) && t.INTB_EndDate.Equals(data.INTB_EndDate) && t.INTB_Id != data.INTB_Id).ToList();
        //            if (ChkDuplicate.Count() > 0)
        //            {
        //                data.already_cnt = true;
        //            }
        //            else
        //            {
        //                var resultobj = _ClgPortalContext.IVRM_NoticeBoardDMO.Single(t => t.INTB_Id.Equals(data.INTB_Id) && t.MI_Id.Equals(data.MI_Id));

        //                resultobj.MI_Id = data.MI_Id;
        //                resultobj.INTB_Title = data.INTB_Title;
        //                resultobj.INTB_Description = data.INTB_Description;
        //                resultobj.INTB_StartDate = data.INTB_StartDate;
        //                resultobj.INTB_EndDate = data.INTB_EndDate;
        //                resultobj.INTB_DisplayDate = data.INTB_DisplayDate;
        //                resultobj.INTB_FilePath = data.INTB_FilePath;
        //                resultobj.INTB_Attachment = data.INTB_Attachment;
        //                resultobj.INTB_DispalyDisableFlg = data.INTB_DispalyDisableFlg;
        //                resultobj.INTB_ToStudentFlg = data.INTB_ToStudentFlg;
        //                resultobj.INTB_ToStaffFlg = data.INTB_ToStaffFlg;
        //                resultobj.UpdatedDate = DateTime.Now;
        //                _ClgPortalContext.Add(resultobj);

        //                foreach (var c in data.courseArray)
        //                {
        //                    foreach (var b in data.branchArray)
        //                    {
        //                        foreach (var s in data.semesterArray)
        //                        {

        //                            var res1 = _ClgPortalContext.IVRM_NoticeBoard_CoBranchDMO.Where(a => a.INTBCB_Id == c.INTBCB_Id).ToList();
        //                            if (res1.Count > 0)
        //                            {
        //                                var res11 = _ClgPortalContext.IVRM_NoticeBoard_CoBranchDMO.Single(a => a.INTBCB_Id == c.INTBCB_Id);
        //                                res11.INTB_Id = resultobj.INTB_Id;
        //                                res11.AMCO_Id = c.AMCO_Id;
        //                                res11.AMB_Id = b.AMB_Id;
        //                                res11.AMSE_Id = s.AMSE_Id;
        //                                res11.UpdatedDate = DateTime.Now;
        //                                _ClgPortalContext.Update(res11);
        //                            }
        //                            else
        //                            {
        //                                IVRM_NoticeBoard_CoBranchDMO clg = new IVRM_NoticeBoard_CoBranchDMO();
        //                                clg.INTB_Id = resultobj.INTB_Id;
        //                                clg.AMCO_Id = c.AMCO_Id;
        //                                clg.AMB_Id = b.AMB_Id;
        //                                clg.AMSE_Id = s.AMSE_Id;
        //                                clg.INTBCB_ActiveFlag = true;
        //                                clg.CreatedDate = DateTime.Now;
        //                                clg.UpdatedDate = DateTime.Now;
        //                                _ClgPortalContext.Add(clg);
        //                            }


        //                        }
        //                    }
        //                }


        //                foreach (var stu in data.studentarray)
        //                {
        //                    IVRM_NoticeBoard_Student_CollegeDMO st = new IVRM_NoticeBoard_Student_CollegeDMO();
        //                    st.AMCST_Id = stu.AMCST_Id;
        //                    st.INTB_Id = resultobj.INTB_Id;
        //                    st.INTBCSTDC_ActiveFlag = true;
        //                    st.INTBCSTDC_CreatedDate = DateTime.Today;
        //                    st.INTBCSTDC_UpdatedDate = DateTime.Today;
        //                    st.INTBCSTDC_CreatedBy = data.UserId;
        //                    st.INTBCSTDC_UpdatedBy = data.UserId;
        //                    _ClgPortalContext.Add(st);
        //                }

        //                _ClgPortalContext.Update(resultobj);
        //                returnval = _ClgPortalContext.SaveChanges();
        //                if (returnval > 0)
        //                {
        //                    data.returnval = true;
        //                }
        //                else
        //                {
        //                    data.returnval = false;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            var result = _ClgPortalContext.IVRM_NoticeBoardDMO.Where(t => t.INTB_Title.Equals(data.INTB_Title) && t.INTB_StartDate.Equals(data.INTB_StartDate) && t.INTB_EndDate.Equals(data.INTB_EndDate) && t.INTB_Description.Equals(data.INTB_Description) && t.INTB_Attachment.Equals(data.INTB_Attachment) && t.MI_Id.Equals(data.MI_Id)).ToList();
        //            if (result.Count() > 0)
        //            {
        //                data.already_cnt = true;
        //            }
        //            else
        //            {
        //                IVRM_NoticeBoardDMO obj = new IVRM_NoticeBoardDMO();

        //                obj.MI_Id = data.MI_Id;
        //                obj.INTB_Title = data.INTB_Title;
        //                obj.INTB_Description = data.INTB_Description;
        //                obj.INTB_StartDate = data.INTB_StartDate;
        //                obj.INTB_EndDate = data.INTB_EndDate;
        //                obj.INTB_DisplayDate = data.INTB_DisplayDate;
        //                obj.INTB_Attachment = data.INTB_Attachment;
        //                obj.INTB_FilePath = data.INTB_FilePath;
        //                obj.NTB_TTSylabusFlg = data.NTB_TTSylabusFlg;
        //                obj.INTB_DispalyDisableFlg = data.INTB_DispalyDisableFlg;
        //                obj.INTB_ActiveFlag = true;
        //                obj.INTB_ToStudentFlg = data.INTB_ToStudentFlg;
        //                obj.INTB_ToStaffFlg = data.INTB_ToStaffFlg;
        //                obj.UpdatedDate = DateTime.Now;
        //                obj.CreatedDate = DateTime.Now;
        //                _ClgPortalContext.Add(obj);
        //                returnval = _ClgPortalContext.SaveChanges();
        //                data.INTB_Id = obj.INTB_Id;
        //                foreach (var c in data.courseArray)
        //                {
        //                    foreach (var b in data.branchArray)
        //                    {
        //                        foreach (var s in data.semesterArray)
        //                        {
        //                            IVRM_NoticeBoard_CoBranchDMO clg = new IVRM_NoticeBoard_CoBranchDMO();
        //                            clg.INTB_Id = data.INTB_Id;
        //                            clg.AMCO_Id = c.AMCO_Id;
        //                            clg.AMB_Id = b.AMB_Id;
        //                            clg.AMSE_Id = s.AMSE_Id;
        //                            clg.INTBCB_ActiveFlag = true;
        //                            clg.CreatedDate = DateTime.Now;
        //                            clg.UpdatedDate = DateTime.Now;
        //                            _ClgPortalContext.Add(clg);
        //                        }
        //                    }
        //                }
        //                if (data.studentarray != null && data.studentarray.Length > 0)
        //                {
        //                    foreach (var stu in data.studentarray)
        //                    {
        //                        IVRM_NoticeBoard_Student_CollegeDMO st = new IVRM_NoticeBoard_Student_CollegeDMO();
        //                        st.AMCST_Id = stu.AMCST_Id;
        //                        st.INTB_Id = data.INTB_Id;
        //                        st.INTBCSTDC_ActiveFlag = true;
        //                        st.INTBCSTDC_CreatedDate = DateTime.Today;
        //                        st.INTBCSTDC_UpdatedDate = DateTime.Today;
        //                        st.INTBCSTDC_CreatedBy = data.UserId;
        //                        st.INTBCSTDC_UpdatedBy = data.UserId;
        //                        _ClgPortalContext.Add(st);
        //                    }
        //                }

        //                if (data.INTB_ToStaffFlg == true)
        //                {
        //                    foreach (var emp in data.employeearraylist)
        //                    {
        //                        IVRM_NoticeBoard_Staff_DMO em = new IVRM_NoticeBoard_Staff_DMO();
        //                        em.INTB_Id = data.INTB_Id;
        //                        em.HRME_Id = emp.HRME_Id;
        //                        em.INTBCSTF_ActiveFlag = true;
        //                        em.CreatedDate = DateTime.Today;
        //                        em.INTBCSTF_CreatedBy = data.UserId;
        //                        _ClgPortalContext.Add(em);
        //                    }
        //                }

        //                //if (data.FilePath_Array.Length > 0)
        //                //{
        //                //    foreach (var fl in data.FilePath_Array)
        //                //    {
        //                //        IVRM_NoticeBoard_FilesDMO em = new IVRM_NoticeBoard_FilesDMO();
        //                //        em.INTB_Id = data.INTB_Id;
        //                //        em.MI_Id = data.MI_Id;
        //                //        em.INTBFL_FileName = fl.FileName;
        //                //        em.INTBFL_FilePath = fl.INTBFL_FilePath;
        //                //        em.INTBFL_ActiveFlag = true;
        //                //        em.INTBFL_CreatedDate = DateTime.Today;
        //                //        em.INTBFL_CreatedBy = data.UserId;
        //                //        _ClgPortalContext.Add(em);
        //                //    }
        //                //}
        //                if (data.INTB_Attachment != null && data.INTB_Attachment != "")
        //                {
        //                    IVRM_NoticeBoard_FilesDMO em = new IVRM_NoticeBoard_FilesDMO();
        //                    em.INTB_Id = data.INTB_Id;
        //                    em.MI_Id = data.MI_Id;
        //                    em.INTBFL_FileName = data.INTB_Attachment;
        //                    em.INTBFL_FilePath = data.INTB_FilePath;
        //                    em.INTBFL_ActiveFlag = true;
        //                    em.INTBFL_CreatedDate = DateTime.Today;
        //                    em.INTBFL_CreatedBy = data.UserId;
        //                    _ClgPortalContext.Add(em);
        //                }



        //                returnval = _ClgPortalContext.SaveChanges();
        //                if (returnval > 0)
        //                {
        //                    data.returnval = true;
        //                }
        //                else
        //                {
        //                    data.returnval = false;
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}


        public ClgNoticeBoardDTO savedata(ClgNoticeBoardDTO data)
        {
            try
            {
                int returnval = 0;
                if (data.INTB_Id > 0)
                {
                    var ChkDuplicate = _ClgPortalContext.IVRM_NoticeBoardDMO.Where(t => t.INTB_Title.Equals(data.INTB_Title) && t.INTB_Attachment.Equals(data.INTB_Attachment) && t.MI_Id.Equals(data.MI_Id) && t.INTB_StartDate.Equals(data.INTB_StartDate) && t.INTB_EndDate.Equals(data.INTB_EndDate) && t.INTB_Id != data.INTB_Id).ToList();
                    if (ChkDuplicate.Count() > 0)
                    {
                        data.already_cnt = true;
                    }
                    else
                    {
                        var resultobj = _ClgPortalContext.IVRM_NoticeBoardDMO.Single(t => t.INTB_Id.Equals(data.INTB_Id) && t.MI_Id.Equals(data.MI_Id));

                        resultobj.MI_Id = data.MI_Id;
                        resultobj.INTB_Title = data.INTB_Title;
                        resultobj.INTB_Description = data.INTB_Description;
                        resultobj.INTB_StartDate = data.INTB_StartDate;
                        resultobj.INTB_EndDate = data.INTB_EndDate;
                        resultobj.INTB_DisplayDate = data.INTB_DisplayDate;
                        resultobj.INTB_FilePath = data.INTB_FilePath;
                        resultobj.INTB_Attachment = data.INTB_Attachment;
                        resultobj.INTB_DispalyDisableFlg = data.INTB_DispalyDisableFlg;
                        resultobj.INTB_ToStudentFlg = data.INTB_ToStudentFlg;
                        resultobj.INTB_ToStaffFlg = data.INTB_ToStaffFlg;
                        resultobj.UpdatedDate = DateTime.Now;
                        _ClgPortalContext.Add(resultobj);

                        foreach (var c in data.courseArray)
                        {
                            foreach (var b in data.branchArray)
                            {
                                foreach (var s in data.semesterArray)
                                {

                                    var res1 = _ClgPortalContext.IVRM_NoticeBoard_CoBranchDMO.Where(a => a.INTBCB_Id == c.INTBCB_Id).ToList();
                                    if (res1.Count > 0)
                                    {
                                        var res11 = _ClgPortalContext.IVRM_NoticeBoard_CoBranchDMO.Single(a => a.INTBCB_Id == c.INTBCB_Id);
                                        res11.INTB_Id = resultobj.INTB_Id;
                                        res11.AMCO_Id = c.AMCO_Id;
                                        res11.AMB_Id = b.AMB_Id;
                                        res11.AMSE_Id = s.AMSE_Id;
                                        res11.UpdatedDate = DateTime.Now;
                                        _ClgPortalContext.Update(res11);
                                    }
                                    else
                                    {
                                        IVRM_NoticeBoard_CoBranchDMO clg = new IVRM_NoticeBoard_CoBranchDMO();
                                        clg.INTB_Id = resultobj.INTB_Id;
                                        clg.AMCO_Id = c.AMCO_Id;
                                        clg.AMB_Id = b.AMB_Id;
                                        clg.AMSE_Id = s.AMSE_Id;
                                        clg.INTBCB_ActiveFlag = true;
                                        clg.CreatedDate = DateTime.Now;
                                        clg.UpdatedDate = DateTime.Now;
                                        _ClgPortalContext.Add(clg);
                                    }


                                }
                            }
                        }


                        foreach (var stu in data.studentarray)
                        {
                            IVRM_NoticeBoard_Student_CollegeDMO st = new IVRM_NoticeBoard_Student_CollegeDMO();
                            st.AMCST_Id = stu.AMCST_Id;
                            st.INTB_Id = resultobj.INTB_Id;
                            st.INTBCSTDC_ActiveFlag = true;
                            st.INTBCSTDC_CreatedDate = DateTime.Today;
                            st.INTBCSTDC_UpdatedDate = DateTime.Today;
                            st.INTBCSTDC_CreatedBy = data.UserId;
                            st.INTBCSTDC_UpdatedBy = data.UserId;
                            _ClgPortalContext.Add(st);
                        }



                        _ClgPortalContext.Update(resultobj);
                        returnval = _ClgPortalContext.SaveChanges();
                        if (returnval > 0)
                        {
                            if (returnval > 0)
                            {
                                data.returnval = true;
                                if (data.INTB_ToStudentFlg == true)
                                {
                                    //=========================================== Notification
                                    // if (data.getcourse == true)
                                    // {
                                    List<long> course_ids = new List<long>();
                                    foreach (var item in data.courseArray)
                                    {
                                        course_ids.Add(item.AMCO_Id);
                                    }
                                    List<long> branch_ids = new List<long>();
                                    foreach (var item in data.branchArray)
                                    {
                                        branch_ids.Add(item.AMB_Id);
                                    }
                                    List<long> sem_ids = new List<long>();
                                    foreach (var item in data.semesterArray)
                                    {
                                        sem_ids.Add(item.AMSE_Id);
                                    }
                                    var devicelist = (from a in _ClgPortalContext.Adm_Master_College_StudentDMO
                                                      from b in _ClgPortalContext.Adm_College_Yearly_StudentDMO
                                                      where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && a.AMCST_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id && 
                                                      course_ids.Contains(b.AMCO_Id) && branch_ids.Contains(b.AMB_Id) && sem_ids.Contains(b.AMSE_Id))
                                                      select new IVRM_Homework_DTO
                                                      {
                                                          AMST_MobileNo = a.AMCST_MobileNo,
                                                          AMST_Id = a.AMCST_Id,
                                                          AMST_AppDownloadedDeviceId = a.AMCST_AppDownloadedDeviceId
                                                      }).Distinct().ToList();


                                    IVRM_Homework_DTO dto = new IVRM_Homework_DTO();
                                    dto.devicelist12 = devicelist;
                                    data.deviceArray = devicelist.ToArray();


                                    var deviceidsnew = "";
                                    var devicenew = "";
                                    var redirecturl = "";
                                    long revieveduserid = 0;

                                    if (devicelist.Count > 0)
                                    {
                                        foreach (var device_id in devicelist)
                                        {
                                            if (device_id.AMST_AppDownloadedDeviceId != null && device_id.AMST_AppDownloadedDeviceId != "")
                                            {


                                                revieveduserid = (from a in _ClgPortalContext.CollegeStudentlogin
                                                                  from b in _ClgPortalContext.ApplicationUser
                                                                  where (a.IVRMUL_Id == b.Id && a.AMCST_Id == device_id.AMST_Id)
                                                                  select b).Select(t => t.Id).FirstOrDefault();



                                                PushNotification push_noti = new PushNotification(_ClgPortalContext);
                                                push_noti.Clg_Call_PushNotificationGeneral(device_id.AMST_AppDownloadedDeviceId, data.MI_Id, data.UserId, revieveduserid, data.INTB_Id, data.INTB_Title, "NoticeBoard", "NoticeBoard");

                                            }
                                        }
                                    }
                                }

                                if (data.INTB_ToStaffFlg == true)
                                {
                                    List<long> hrmeid = new List<long>();
                                    foreach (var item in data.employeearraylist)
                                    {
                                        hrmeid.Add(item.HRME_Id);
                                    }
                                    //=========================================== Notification
                                    var devicelist = (from a in _ClgPortalContext.MasterEmployee
                                                      where (a.MI_Id == data.MI_Id && hrmeid.Contains(a.HRME_Id) && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                                      select new IVRM_Homework_DTO
                                                      {
                                                          AMST_MobileNo = Convert.ToInt64(a.HRME_MobileNo),
                                                          HRME_Id = a.HRME_Id,
                                                          AMST_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                                                      }).Distinct().ToList();


                                    IVRM_Homework_DTO dto = new IVRM_Homework_DTO();
                                    dto.devicelist12 = devicelist;
                                    data.deviceArray = devicelist.ToArray();

                                    var deviceidsnew = "";
                                    var devicenew = "";
                                    var redirecturl = "";
                                    long revieveduserid = 0;

                                    if (devicelist.Count > 0)
                                    {
                                        foreach (var device_id in devicelist)
                                        {
                                            if (device_id.AMST_AppDownloadedDeviceId != null && device_id.AMST_AppDownloadedDeviceId != "")
                                            {


                                                revieveduserid = (from a in _ClgPortalContext.Staff_User_Login
                                                                  from b in _ClgPortalContext.ApplicationUser
                                                                  where (a.IVRMSTAUL_UserName == b.UserName && a.Emp_Code == device_id.HRME_Id)
                                                                  select b).Select(t => t.Id).FirstOrDefault();



                                                PushNotification push_noti = new PushNotification(_ClgPortalContext);
                                                push_noti.Clg_Call_PushNotificationGeneral(device_id.AMST_AppDownloadedDeviceId, data.MI_Id, data.UserId, revieveduserid, data.INTB_Id, data.INTB_Title, "NoticeBoard", "NoticeBoard");

                                            }
                                        }
                                    }

                                }
                            }
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    var result = _ClgPortalContext.IVRM_NoticeBoardDMO.Where(t => t.INTB_Title.Equals(data.INTB_Title) && t.INTB_StartDate.Equals(data.INTB_StartDate) && t.INTB_EndDate.Equals(data.INTB_EndDate) && t.INTB_Description.Equals(data.INTB_Description) && t.INTB_Attachment.Equals(data.INTB_Attachment) && t.MI_Id.Equals(data.MI_Id)).ToList();
                    if (result.Count() > 0)
                    {
                        data.already_cnt = true;
                    }
                    else
                    {
                        IVRM_NoticeBoardDMO obj = new IVRM_NoticeBoardDMO();

                        obj.MI_Id = data.MI_Id;
                        obj.INTB_Title = data.INTB_Title;
                        obj.INTB_Description = data.INTB_Description;
                        obj.INTB_StartDate = data.INTB_StartDate;
                        obj.INTB_EndDate = data.INTB_EndDate;
                        obj.INTB_DisplayDate = data.INTB_DisplayDate;
                        obj.INTB_Attachment = data.INTB_Attachment;
                        obj.INTB_FilePath = data.INTB_FilePath;
                        obj.NTB_TTSylabusFlg = data.NTB_TTSylabusFlg;
                        obj.INTB_DispalyDisableFlg = data.INTB_DispalyDisableFlg;
                        obj.INTB_ActiveFlag = true;
                        obj.INTB_ToStudentFlg = data.INTB_ToStudentFlg;
                        obj.INTB_ToStaffFlg = data.INTB_ToStaffFlg;
                        obj.UpdatedDate = DateTime.Now;
                        obj.CreatedDate = DateTime.Now;
                        _ClgPortalContext.Add(obj);
                        returnval = _ClgPortalContext.SaveChanges();
                        data.INTB_Id = obj.INTB_Id;
                        foreach (var c in data.courseArray)
                        {
                            foreach (var b in data.branchArray)
                            {
                                foreach (var s in data.semesterArray)
                                {
                                    IVRM_NoticeBoard_CoBranchDMO clg = new IVRM_NoticeBoard_CoBranchDMO();
                                    clg.INTB_Id = data.INTB_Id;
                                    clg.AMCO_Id = c.AMCO_Id;
                                    clg.AMB_Id = b.AMB_Id;
                                    clg.AMSE_Id = s.AMSE_Id;
                                    clg.INTBCB_ActiveFlag = true;
                                    clg.CreatedDate = DateTime.Now;
                                    clg.UpdatedDate = DateTime.Now;
                                    _ClgPortalContext.Add(clg);
                                }
                            }
                        }
                        if (data.studentarray != null && data.studentarray.Length > 0)
                        {
                            foreach (var stu in data.studentarray)
                            {
                                IVRM_NoticeBoard_Student_CollegeDMO st = new IVRM_NoticeBoard_Student_CollegeDMO();
                                st.AMCST_Id = stu.AMCST_Id;
                                st.INTB_Id = data.INTB_Id;
                                st.INTBCSTDC_ActiveFlag = true;
                                st.INTBCSTDC_CreatedDate = DateTime.Today;
                                st.INTBCSTDC_UpdatedDate = DateTime.Today;
                                st.INTBCSTDC_CreatedBy = data.UserId;
                                st.INTBCSTDC_UpdatedBy = data.UserId;
                                _ClgPortalContext.Add(st);
                            }
                        }

                        if (data.INTB_ToStaffFlg == true)
                        {
                            foreach (var emp in data.employeearraylist)
                            {
                                IVRM_NoticeBoard_Staff_DMO em = new IVRM_NoticeBoard_Staff_DMO();
                                em.INTB_Id = data.INTB_Id;
                                em.HRME_Id = emp.HRME_Id;
                                em.INTBCSTF_ActiveFlag = true;
                                em.CreatedDate = DateTime.Today;
                                em.INTBCSTF_CreatedBy = data.UserId;
                                _ClgPortalContext.Add(em);
                            }
                        }

                        //if (data.FilePath_Array.Length > 0)
                        //{
                        //    foreach (var fl in data.FilePath_Array)
                        //    {
                        //        IVRM_NoticeBoard_FilesDMO em = new IVRM_NoticeBoard_FilesDMO();
                        //        em.INTB_Id = data.INTB_Id;
                        //        em.MI_Id = data.MI_Id;
                        //        em.INTBFL_FileName = fl.FileName;
                        //        em.INTBFL_FilePath = fl.INTBFL_FilePath;
                        //        em.INTBFL_ActiveFlag = true;
                        //        em.INTBFL_CreatedDate = DateTime.Today;
                        //        em.INTBFL_CreatedBy = data.UserId;
                        //        _ClgPortalContext.Add(em);
                        //    }
                        //}
                        if (data.INTB_Attachment != null && data.INTB_Attachment != "")
                        {
                            IVRM_NoticeBoard_FilesDMO em = new IVRM_NoticeBoard_FilesDMO();
                            em.INTB_Id = data.INTB_Id;
                            em.MI_Id = data.MI_Id;
                            em.INTBFL_FileName = data.INTB_Attachment;
                            em.INTBFL_FilePath = data.INTB_FilePath;
                            em.INTBFL_ActiveFlag = true;
                            em.INTBFL_CreatedDate = DateTime.Today;
                            em.INTBFL_CreatedBy = data.UserId;
                            _ClgPortalContext.Add(em);
                        }

                        returnval = _ClgPortalContext.SaveChanges();


                        if (returnval > 0)
                        {
                            data.returnval = true;
                            if (data.INTB_ToStudentFlg == true)
                            {
                                //=========================================== Notification
                                // if (data.getcourse == true)
                                // {
                                //List<long> course_ids = new List<long>();
                                //foreach (var item in data.courseArray)
                                //{
                                //    course_ids.Add(item.AMCO_Id);
                                //}
                                //List<long> branch_ids = new List<long>();
                                //foreach (var item in data.branchArray)
                                //{
                                //    branch_ids.Add(item.AMB_Id);
                                //}
                                //List<long> sem_ids = new List<long>();
                                //foreach (var item in data.semesterArray)
                                //{
                                //    sem_ids.Add(item.AMSE_Id);
                                //}
                                List<long> stud_ids = new List<long>();
                                foreach (var item in data.studentarray)
                                {
                                    stud_ids.Add(item.AMCST_Id);
                                }
                                var devicelist = (from a in _ClgPortalContext.Adm_Master_College_StudentDMO
                                                  from b in _ClgPortalContext.Adm_College_Yearly_StudentDMO
                                                  where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && a.AMCST_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id && stud_ids.Contains(b.AMCST_Id))
                                                  select new IVRM_Homework_DTO
                                                  {
                                                      AMST_MobileNo = a.AMCST_MobileNo,
                                                      AMST_Id = a.AMCST_Id,
                                                      AMST_AppDownloadedDeviceId = a.AMCST_AppDownloadedDeviceId
                                                  }).Distinct().ToList();

                                IVRM_Homework_DTO dto = new IVRM_Homework_DTO();
                                    dto.devicelist12 = devicelist;
                                    data.deviceArray = devicelist.ToArray();


                                    var deviceidsnew = "";
                                    var devicenew = "";
                                    var redirecturl = "";
                                    long revieveduserid = 0;

                                    if (devicelist.Count > 0)
                                    {
                                        foreach (var device_id in devicelist)
                                        {
                                            if (device_id.AMST_AppDownloadedDeviceId != null && device_id.AMST_AppDownloadedDeviceId != "")
                                            {


                                                revieveduserid = (from a in _ClgPortalContext.CollegeStudentlogin
                                                                  from b in _ClgPortalContext.ApplicationUser
                                                                  where ( a.IVRMUL_Id == b.Id && a.AMCST_Id == device_id.AMST_Id)
                                                                  select b).Select(t => t.Id).FirstOrDefault();



                                                PushNotification push_noti = new PushNotification(_ClgPortalContext);
                                                push_noti.Clg_Call_PushNotificationGeneral(device_id.AMST_AppDownloadedDeviceId, data.MI_Id, data.UserId, revieveduserid, data.INTB_Id, data.INTB_Title, "NoticeBoard", "NoticeBoard");

                                            }
                                        }
                                    }
                            }

                            if (data.INTB_ToStaffFlg == true)
                            {
                                List<long> hrmeid = new List<long>();
                                foreach (var item in data.employeearraylist)
                                {
                                    hrmeid.Add(item.HRME_Id);
                                }
                                //=========================================== Notification
                                var devicelist = (from a in _ClgPortalContext.MasterEmployee
                                                  where (a.MI_Id == data.MI_Id && hrmeid.Contains(a.HRME_Id) && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                                  select new IVRM_Homework_DTO
                                                  {
                                                      AMST_MobileNo = Convert.ToInt64(a.HRME_MobileNo),
                                                      HRME_Id = a.HRME_Id,
                                                      AMST_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                                                  }).Distinct().ToList();


                                IVRM_Homework_DTO dto = new IVRM_Homework_DTO();
                                dto.devicelist12 = devicelist;
                                data.deviceArray = devicelist.ToArray();

                                var deviceidsnew = "";
                                var devicenew = "";
                                var redirecturl = "";
                                long revieveduserid = 0;

                                if (devicelist.Count > 0)
                                {
                                    foreach (var device_id in devicelist)
                                    {
                                        if (device_id.AMST_AppDownloadedDeviceId != null && device_id.AMST_AppDownloadedDeviceId != "")
                                        {


                                            revieveduserid = (from a in _ClgPortalContext.Staff_User_Login
                                                              from b in _ClgPortalContext.ApplicationUser
                                                              where (a.IVRMSTAUL_UserName == b.UserName && a.Emp_Code == device_id.HRME_Id)
                                                              select b).Select(t => t.Id).FirstOrDefault();



                                            PushNotification push_noti = new PushNotification(_ClgPortalContext);
                                            push_noti.Clg_Call_PushNotificationGeneral(device_id.AMST_AppDownloadedDeviceId, data.MI_Id, data.UserId, revieveduserid, data.INTB_Id, data.INTB_Title, "NoticeBoard", "NoticeBoard");

                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public ClgNoticeBoardDTO getNoticedata(ClgNoticeBoardDTO data)
        {
            try
            {
                data.noticedetails = (from a in _ClgPortalContext.IVRM_NoticeBoardDMO
                                      from b in _ClgPortalContext.IVRM_NoticeBoard_CoBranchDMO
                                      from c in _ClgPortalContext.MasterCourseDMO
                                      from d in _ClgPortalContext.ClgMasterBranchDMO
                                      from e in _ClgPortalContext.CLG_Adm_Master_SemesterDMO
                                      where (b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && b.AMSE_Id == e.AMSE_Id && a.INTB_Id == b.INTB_Id && a.MI_Id == data.MI_Id && a.INTB_Id == data.INTB_Id)
                                      select new ClgNoticeBoardDTO
                                      {
                                          INTBCB_Id = b.INTBCB_Id,
                                          AMCO_Id = b.AMCO_Id,
                                          AMB_Id = b.AMB_Id,
                                          AMSE_Id = b.AMSE_Id,
                                          INTB_Title = a.INTB_Title,
                                          AMCO_CourseName = c.AMCO_CourseName,
                                          AMB_BranchName = d.AMB_BranchName,
                                          AMSE_SEMName = e.AMSE_SEMName,
                                          INTBCB_ActiveFlag = b.INTBCB_ActiveFlag
                                      }
                            ).Distinct().OrderBy(c => c.INTBCB_Id).ToArray();

                data.staffnoticedetails = (from a in _ClgPortalContext.IVRM_NoticeBoardDMO
                                           from b in _ClgPortalContext.IVRM_NoticeBoard_Staff_DMO_con
                                           from c in _ClgPortalContext.MasterEmployee
                                           from d in _ClgPortalContext.HR_Master_Department
                                           from e in _ClgPortalContext.HR_Master_DesignationDMO
                                           where (b.HRME_Id == c.HRME_Id && c.HRMD_Id == d.HRMD_Id && c.HRMDES_Id == e.HRMDES_Id && a.INTB_Id == b.INTB_Id && a.MI_Id == data.MI_Id && a.INTB_Id == data.INTB_Id)
                                      select new ClgNoticeBoardDTO
                                      {
                                          INTBCSTF_Id = b.INTBCSTF_Id,
                                          INTB_Title = a.INTB_Title,
                                          HRME_EmployeeFirstName = c.HRME_EmployeeFirstName,
                                          HRMD_DepartmentName = d.HRMD_DepartmentName,
                                          HRMDES_DesignationName = e.HRMDES_DesignationName,
                                          INTBCSTF_ActiveFlag = b.INTBCSTF_ActiveFlag
                                      }
            ).Distinct().OrderBy(c => c.INTBCB_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ClgNoticeBoardDTO editdetails(ClgNoticeBoardDTO data)
        {
            try
            {
                data.editdetails = (from a in _ClgPortalContext.IVRM_NoticeBoardDMO
                                    from b in _ClgPortalContext.IVRM_NoticeBoard_CoBranchDMO
                                    from c in _ClgPortalContext.MasterCourseDMO
                                    from d in _ClgPortalContext.ClgMasterBranchDMO
                                    from e in _ClgPortalContext.CLG_Adm_Master_SemesterDMO
                                    where (b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && b.AMSE_Id == e.AMSE_Id && a.INTB_Id == b.INTB_Id && a.MI_Id == data.MI_Id && a.INTB_Id == data.INTB_Id)
                                    select new ClgNoticeBoardDTO
                                    {
                                        INTBCB_Id = b.INTBCB_Id,
                                        INTB_Id = a.INTB_Id,
                                        AMCO_Id = b.AMCO_Id,
                                        AMB_Id = b.AMB_Id,
                                        AMSE_Id = b.AMSE_Id,
                                        INTB_Title = a.INTB_Title,
                                        INTB_Description = a.INTB_Description,
                                        AMCO_CourseName = c.AMCO_CourseName,
                                        AMB_BranchName = d.AMB_BranchName,
                                        AMSE_SEMName = e.AMSE_SEMName,
                                        INTB_StartDate = a.INTB_StartDate,
                                        INTB_EndDate = a.INTB_EndDate,
                                        INTB_DispalyDisableFlg = a.INTB_DispalyDisableFlg,
                                        INTB_DisplayDate = a.INTB_DisplayDate,
                                        INTB_FilePath = a.INTB_FilePath,
                                        INTB_Attachment = a.INTB_Attachment,
                                        NTB_TTSylabusFlg = a.NTB_TTSylabusFlg,
                                        INTB_ActiveFlag = a.INTB_ActiveFlag,
                                        INTBCB_ActiveFlag = b.INTBCB_ActiveFlag
                                    }
                            ).Distinct().OrderBy(c => c.INTBCB_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ClgNoticeBoardDTO deactive(ClgNoticeBoardDTO data)
        {
            try
            {
                var result = _ClgPortalContext.IVRM_NoticeBoardDMO.Single(t => t.INTB_Id == data.INTB_Id);

                if (result.INTB_ActiveFlag == true)
                {
                    result.INTB_ActiveFlag = false;
                }
                else if (result.INTB_ActiveFlag == false)
                {
                    result.INTB_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                _ClgPortalContext.Update(result);

                var resultt = _ClgPortalContext.IVRM_NoticeBoard_CoBranchDMO.Where(t => t.INTB_Id == data.INTB_Id).ToList();
                foreach (var r in resultt)
                {
                    if (result.INTB_ActiveFlag == false)
                    {
                        r.INTBCB_ActiveFlag = false;
                    }
                    else if (result.INTB_ActiveFlag == true)
                    {
                        r.INTBCB_ActiveFlag = true;
                    }
                    r.UpdatedDate = DateTime.Now;
                    _ClgPortalContext.Update(r);
                }

                int returnval = _ClgPortalContext.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

        public ClgNoticeBoardDTO deactivedetails(ClgNoticeBoardDTO data)
        {
            try
            {
                if(data.INTBCB_Id > 0)
                {
                    var result = _ClgPortalContext.IVRM_NoticeBoard_CoBranchDMO.Single(t => t.INTBCB_Id == data.INTBCB_Id);

                    if (result.INTBCB_ActiveFlag == true)
                    {
                        result.INTBCB_ActiveFlag = false;
                    }
                    else if (result.INTBCB_ActiveFlag == false)
                    {
                        result.INTBCB_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _ClgPortalContext.Update(result);
                    int returnval = _ClgPortalContext.SaveChanges();
                    if (returnval > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                else
                {
                    var result = _ClgPortalContext.IVRM_NoticeBoard_Staff_DMO_con.Single(t => t.INTBCSTF_Id == data.INTBCSTF_Id);

                    if (result.INTBCSTF_ActiveFlag == true)
                    {
                        result.INTBCSTF_ActiveFlag = false;
                    }
                    else if (result.INTBCSTF_ActiveFlag == false)
                    {
                        result.INTBCSTF_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _ClgPortalContext.Update(result);
                    int returnval = _ClgPortalContext.SaveChanges();
                    if (returnval > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }


        //added for noticeboard consolidated report
        public ClgNoticeBoardDTO Getdata_class(ClgNoticeBoardDTO dto)
        {
            try
            {
                var rolet = _ClgPortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == dto.IVRMRT_Id).ToList();

                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {

                    dto.HRME_Id = _ClgPortalContext.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;
                }
                else
                {
                    dto.HRME_Id = 0;
                }

                //var rolet = _Context.IVRM_Role_Type.Where(t => t.IVRMRT_Id == dto.IVRMRT_Id).ToList();

                //dto.HRME_Id = _Context.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;

                using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_ClassList_Clg";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Role",
                SqlDbType.VarChar)
                    {
                        Value = rolet[0].IVRMRT_Role
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.course_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public ClgNoticeBoardDTO getreportnotice(ClgNoticeBoardDTO dto)
        {
            try
            {
                //var rolet = _ClgPortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == dto.IVRMRT_Id).ToList();

                //if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                //{

                //    dto.HRME_Id = _ClgPortalContext.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;
                //}
                //else
                //{
                //    dto.HRME_Id = 0;
                //}
                string amcoid = "0";
                if (dto.coursearray.Length > 0)
                {
                    foreach (var item in dto.coursearray)
                    {
                        amcoid = amcoid + "," + item.AMCO_Id;
                    }
                }
                //var rolet = _Context.IVRM_Role_Type.Where(t => t.IVRMRT_Id == dto.IVRMRT_Id).ToList();

                //dto.HRME_Id = _Context.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;


                using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_NoticeboardConsolidated_Clg";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
               SqlDbType.VarChar)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                 SqlDbType.VarChar)
                    {
                        Value = amcoid
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                    SqlDbType.VarChar)
                    {
                        Value = dto.fromdate
                    });

                    cmd.Parameters.Add(new SqlParameter("@todate",
                    SqlDbType.VarChar)
                    {
                        Value = dto.todate
                    });


                    cmd.Parameters.Add(new SqlParameter("@flag",
                SqlDbType.VarChar)
                    {
                        Value = dto.flag
                    });
                    cmd.Parameters.Add(new SqlParameter("@type",
         SqlDbType.VarChar)
                    {
                        Value = dto.type
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.reportlist = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public ClgNoticeBoardDTO Getdataview(ClgNoticeBoardDTO dto)
        {
            try

            {
                var rolet = _ClgPortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == dto.IVRMRT_Id).ToList();
                using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_NoticeboardConsolidated_Clg";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
               SqlDbType.VarChar)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                 SqlDbType.VarChar)
                    {
                        Value = dto.AMCO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                    SqlDbType.VarChar)
                    {
                        Value = dto.fromdate
                    });

                    cmd.Parameters.Add(new SqlParameter("@todate",
                    SqlDbType.VarChar)
                    {
                        Value = dto.todate
                    });


                    cmd.Parameters.Add(new SqlParameter("@flag",
                SqlDbType.VarChar)
                    {
                        Value = "Detailed"
                    });
                    cmd.Parameters.Add(new SqlParameter("@type",
         SqlDbType.VarChar)
                    {
                        Value = dto.type
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.view_array = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        //getstudent

        public ClgNoticeBoardDTO getstudent(ClgNoticeBoardDTO data)
        {
            try
            {

                //added by roopa//
                string amb_ids = "0", amco_ids = "0", asmeid = "0", fmg_ids = "0", fmh_ids = "0", flag = "";
                if (data.courseArray != null && data.courseArray.Length > 0)
                {
                    if (data.courseArray.Length > 0)
                    {
                        foreach (var ue in data.courseArray)
                        {
                            amco_ids = amco_ids + "," + ue.AMCO_Id;

                        }

                    }
                }

                if (data.branchArray != null && data.branchArray.Length > 0)
                {
                    if (data.branchArray.Length > 0)
                    {
                        foreach (var ue in data.branchArray)
                        {
                            amb_ids = amb_ids + "," + ue.AMB_Id;
                            // asmsid = asmsid + "," + ue.ASMS_Id;
                        }

                    }
                }

                if (data.semesterArray != null && data.semesterArray.Length > 0)
                {
                    foreach (var item in data.semesterArray)
                    {
                        asmeid = asmeid + "," + item.AMSE_Id;
                    }
                }
                if (data.fee_def == true)
                {
                    flag = "F";
                }
                else
                {
                    flag = "S";
                }
                if (data.fee_def == true)
                {

                    if (data.defarray != null && data.defarray.Length > 0)
                    {
                        foreach (var item in data.defarray)
                        {
                            fmg_ids = fmg_ids + "," + item.FMG_Id;
                        }
                    }

                    if (data.defheadarray != null && data.defheadarray.Length > 0)
                    {
                        foreach (var item in data.defheadarray)
                        {
                            fmh_ids = fmh_ids + "," + item.FMH_Id;
                        }
                    }
                }
                using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FEE_STUDENTS_list_clg";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                 SqlDbType.VarChar)
                    {
                        Value = amco_ids
                        //Value = amb_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                  SqlDbType.VarChar)
                    {
                        // Value = dto.ASMCL_Id
                        Value = amb_ids
                    });


                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                    SqlDbType.VarChar)
                    {
                        Value = asmeid
                    });
                    cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                 SqlDbType.VarChar)
                    {
                        Value = fmg_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@FMH_Id",
                 SqlDbType.VarChar)
                    {
                        Value = fmh_ids
                    });

                    cmd.Parameters.Add(new SqlParameter("@flag",
                   SqlDbType.VarChar)
                    {
                        Value = flag
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.studentlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        //Akash

        //department and designation

        public ClgNoticeBoardDTO Deptselectiondetails(ClgNoticeBoardDTO dto)
        {
            try
            {
                string departments = "0";
                if (dto.departmentlist.Length > 0)
                {
                    foreach (var ue in dto.departmentlist)
                    {
                        departments = departments + "," + ue.HRMDC_ID;
                    }
                }
                //******************************Deviation Remarks & Deviation Calculation Report ************************************//         
                using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Clg_IVRM_DepartmentChange";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@departments",
                    SqlDbType.VarChar)
                    {
                        Value = departments
                    });

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                    {
                        Value = dto.MI_Id
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.designation = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public ClgNoticeBoardDTO Desgselectiondetails(ClgNoticeBoardDTO dto)
        {
            try
            {
                string departments = "0";
                string designations = "0";
                if (dto.designationlist.Length > 0)
                {
                    foreach (var ue in dto.designationlist)
                    {
                        designations = designations + "," + ue.HRMDES_Id;
                    }

                }

                if (dto.departmentlist.Length > 0)
                {
                    foreach (var ue in dto.departmentlist)
                    {
                        departments = departments + "," + ue.HRMDC_ID;
                    }

                }
                //******************************Deviation Remarks & Deviation Calculation Report ************************************//         
                using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Clg_ISM_DesignationChange";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@departments",
                    SqlDbType.VarChar)
                    {
                        Value = departments
                    });
                    cmd.Parameters.Add(new SqlParameter("@designations",
                   SqlDbType.VarChar)
                    {
                        Value = designations
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.get_userEmplist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }


        public ClgNoticeBoardDTO getmultiuploadedfile(ClgNoticeBoardDTO data)
        {
            try
            {
                data.view_array = _ClgPortalContext.IVRM_NoticeBoard_FilesDMO.Where(t => t.INTB_Id == data.INTB_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        //
    }
}
