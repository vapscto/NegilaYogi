using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.Exam;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Services
{
    public class ExamPromotionRemarksImpl : Interfaces.ExamPromotionRemarksInterface
    {
        public ExamContext _context;
        ILogger<ExamPromotionRemarksImpl> _log;
        public ExamPromotionRemarksImpl(ExamContext context, ILogger<ExamPromotionRemarksImpl> log)
        {
            _context = context;
            _log = log;
        }

        public ExamPromotionRemarksDTO Getdetails(ExamPromotionRemarksDTO data)
        {
            try
            {
                data.getyear = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _log.LogDebug("Getdetails() in Exam Promotion Remaks form" + ex.Message);
                _log.LogError("Getdetails() in Exam Promotion Remaks form" + ex.Message);
            }
            return data;
        }
        public ExamPromotionRemarksDTO get_class(ExamPromotionRemarksDTO data)
        {
            try
            {
                var getuserid = _context.ApplUser.Where(a => a.UserName.Equals(data.username.Trim())).Select(a => a.Id);

                var check_rolename = (from a in _context.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleId)
                                      select new ExamPromotionRemarksDTO
                                      {
                                          rolename = a.IVRMRT_Role.ToUpper(),
                                      }
                                   ).ToList();

                var empcode_check = (from a in _context.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(data.userId))
                                     select new ExamPromotionRemarksDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                if (check_rolename.FirstOrDefault().rolename.Equals("STAFF"))
                {
                    data.getclass = (from a in _context.ClassTeacherMappingDMO
                                     from c in _context.AdmissionClass
                                     where (c.ASMCL_Id == a.ASMCL_Id
                                     && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                     && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                     && c.ASMCL_ActiveFlag == true && a.IMCT_ActiveFlag == true)
                                     select c).Distinct().OrderBy(c => c.ASMCL_Order).ToArray();
                }
                else
                {
                    data.getclass = (from a in _context.AcademicYear
                                     from b in _context.AdmissionClass
                                     from c in _context.Exm_Category_ClassDMO
                                     where (a.ASMAY_Id == c.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && c.ECAC_ActiveFlag == true && c.MI_Id == data.MI_Id
                                     && c.ASMAY_Id == data.ASMAY_Id)
                                     select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();

                    //data.getclass = _context.AdmissionClass.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true).OrderBy(a => a.ASMCL_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _log.LogDebug("get_class() in Exam Promotion Remaks form" + ex.Message);
                _log.LogError("get_class() in Exam Promotion Remaks form" + ex.Message);
            }
            return data;
        }
        public ExamPromotionRemarksDTO get_section(ExamPromotionRemarksDTO data)
        {
            try
            {
                var getuserid = _context.ApplUser.Where(a => a.UserName.Equals(data.username.Trim())).Select(a => a.Id);

                var check_rolename = (from a in _context.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleId)
                                      select new ExamPromotionRemarksDTO
                                      {
                                          rolename = a.IVRMRT_Role.ToUpper(),
                                      }
                                   ).ToList();

                var empcode_check = (from a in _context.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(data.userId))
                                     select new ExamPromotionRemarksDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                if (check_rolename.FirstOrDefault().rolename.Equals("STAFF"))
                {
                    data.getsection = (from a in _context.ClassTeacherMappingDMO
                                       from c in _context.AdmissionClass
                                       from d in _context.School_M_Section
                                       where (c.ASMCL_Id == a.ASMCL_Id && a.ASMS_Id == d.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                       && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && c.ASMCL_ActiveFlag == true && a.IMCT_ActiveFlag == true
                                       && a.ASMCL_Id == data.ASMCL_Id && d.ASMC_ActiveFlag == 1)
                                       select d
                                      ).Distinct().OrderBy(c => c.ASMC_Order).ToArray();
                }
                else
                {
                    data.getsection = (from a in _context.AcademicYear
                                       from b in _context.AdmissionClass
                                       from c in _context.Exm_Category_ClassDMO
                                       from d in _context.School_M_Section
                                       where (a.ASMAY_Id == c.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && c.ASMS_Id == d.ASMS_Id && c.ECAC_ActiveFlag == true
                                       && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == data.ASMCL_Id)
                                       select d).Distinct().OrderBy(a => a.ASMC_Order).ToArray();

                    //data.getsection = _context.School_M_Section.Where(a => a.MI_Id == data.MI_Id && a.ASMC_ActiveFlag == 1).OrderBy(a => a.ASMC_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _log.LogDebug("get_section() in Exam Promotion Remaks form" + ex.Message);
                _log.LogError("get_section() in Exam Promotion Remaks form" + ex.Message);
            }
            return data;
        }
        public ExamPromotionRemarksDTO get_group(ExamPromotionRemarksDTO data)
        {
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Subject_GroupName";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var retObject = new List<dynamic>();

                try
                {
                    using (var dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                            for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                            {
                                dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                            }
                            retObject.Add((ExpandoObject)dataRow1);
                        }
                    }
                    data.getexamgroupname = retObject.ToArray();
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
            }
            return data;
        }
        public ExamPromotionRemarksDTO get_exam(ExamPromotionRemarksDTO data)
        {
            try
            {
                var get_emcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).ToList();

                var get_eycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.EMCA_Id == get_emcaid[0].EMCA_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id).ToList();

                var get_examid = (from a in _context.Exm_Yearly_Category_ExamsDMO
                                  from b in _context.exammasterDMO
                                  where (a.EME_Id == b.EME_Id && a.EYCE_ActiveFlg == true && a.EYC_Id == get_eycid[0].EYC_Id && b.MI_Id == data.MI_Id && b.EME_ActiveFlag == true)
                                  select b).Distinct().OrderBy(c => c.EME_ExamOrder).ToArray();

                data.getexam = get_examid;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _log.LogDebug("get_exam() in Exam Promotion Remaks form" + ex.Message);
                _log.LogError("get_exam() in Exam Promotion Remaks form" + ex.Message);
            }
            return data;
        }
        public ExamPromotionRemarksDTO search_student(ExamPromotionRemarksDTO data)
        {
            try
            {
                string order = "";
                var get_configuration = _context.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
                order = "studentname";
                if (get_configuration != null && get_configuration.Count > 0)
                {
                    if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "Name")
                    {
                        order = "studentname";
                    }
                    else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "AdmNo")
                    {
                        order = "AMST_Admno";
                    }
                    else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RollNo")
                    {
                        order = "AMAY_RollNo";
                    }
                    else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RegNo")
                    {
                        order = "AMST_RegistrationNo";
                    }
                }
                List<ExamPromotionRemarksDTO> studentList = new List<ExamPromotionRemarksDTO>();

                studentList = (from a in _context.Adm_M_Student
                               from b in _context.School_Adm_Y_StudentDMO
                               from e in _context.AcademicYear
                               from f in _context.AdmissionClass
                               from g in _context.School_M_Section
                               where (a.AMST_Id == b.AMST_Id && b.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == f.ASMCL_Id && b.ASMS_Id == g.ASMS_Id
                               && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id
                               && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1)
                               select new ExamPromotionRemarksDTO
                               {
                                   AMST_Id = a.AMST_Id,
                                   studentname = ((a.AMST_FirstName == null || a.AMST_FirstName == "" ? "" : " " + a.AMST_FirstName) + (a.AMST_MiddleName == null || a.AMST_MiddleName == "" || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) + (a.AMST_LastName == null || a.AMST_LastName == "" || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                   AMST_Admno = a.AMST_AdmNo == null ? "" : a.AMST_AdmNo,
                                   AMAY_RollNo = b.AMAY_RollNo,
                                   AMST_RegistrationNo = a.AMST_RegistrationNo == null ? "" : a.AMST_RegistrationNo
                               }).Distinct().OrderBy(a => order).ToList();

                var propertyInfo = typeof(ExamPromotionRemarksDTO).GetProperty(order);
                data.getstudentdetails = studentList.OrderBy(x => propertyInfo.GetValue(x, null)).ToArray();

                if (data.examtype == 0)
                {
                    data.getsavedetails = _context.ExamPromotionRemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id && a.EPRD_Promotionflag == "IE").ToArray();

                    data.getstudentmarks = _context.ExmStudentMarksProcessDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id).ToArray();
                }
                else
                {
                    data.getsavedetails = _context.ExamPromotionRemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EPRD_Promotionflag == "PE").ToArray();

                    data.getstudentmarks = _context.Exm_Student_MP_PromotionDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _log.LogDebug("search_student() in Exam Promotion Remaks form" + ex.Message);
                _log.LogError("search_student() in Exam Promotion Remaks form" + ex.Message);
            }
            return data;
        }
        public ExamPromotionRemarksDTO save_details(ExamPromotionRemarksDTO data)
        {
            try
            {
                if (data.examtype_temp.Length > 0)
                {
                    for (int i = 0; i < data.examtype_temp.Length; i++)
                    {
                        if (data.examtype_temp[i].EPRD_Id > 0)
                        {
                            if (data.examtype == 0)
                            {
                                var result = _context.ExamPromotionRemarksDMO.Single(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id && a.AMST_Id == data.examtype_temp[i].AMST_Id && a.EPRD_Id == data.examtype_temp[i].EPRD_Id && a.EPRD_Promotionflag == "IE");

                                if (data.examtype_temp[i].EPRD_PromotionName != null)
                                {
                                    result.EPRD_PromotionName = data.examtype_temp[i].EPRD_PromotionName;
                                }
                                else
                                {
                                    result.EPRD_PromotionName = "";
                                }

                                if (data.examtype_temp[i].EPRD_ClassPromoted != null)
                                {
                                    result.EPRD_ClassPromoted = data.examtype_temp[i].EPRD_ClassPromoted;
                                }
                                else
                                {
                                    result.EPRD_ClassPromoted = "";
                                }

                                if (data.examtype_temp[i].EPRD_Remarks != null)
                                {
                                    result.EPRD_Remarks = data.examtype_temp[i].EPRD_Remarks;
                                }
                                else
                                {
                                    result.EPRD_Remarks = "";
                                }
                                result.UpdatedDate = DateTime.Now;
                                _context.Update(result);
                            }
                            else
                            {
                                var result = _context.ExamPromotionRemarksDMO.Single(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == data.examtype_temp[i].AMST_Id && a.EPRD_Id == data.examtype_temp[i].EPRD_Id && a.EPRD_Promotionflag == "PE");

                                if (data.examtype_temp[i].EPRD_PromotionName != null)
                                {
                                    result.EPRD_PromotionName = data.examtype_temp[i].EPRD_PromotionName;
                                }
                                else
                                {
                                    result.EPRD_PromotionName = "";
                                }

                                if (data.examtype_temp[i].EPRD_ClassPromoted != null)
                                {
                                    result.EPRD_ClassPromoted = data.examtype_temp[i].EPRD_ClassPromoted;
                                }
                                else
                                {
                                    result.EPRD_ClassPromoted = "";
                                }

                                if (data.examtype_temp[i].EPRD_Remarks != null)
                                {
                                    result.EPRD_Remarks = data.examtype_temp[i].EPRD_Remarks;
                                }
                                else
                                {
                                    result.EPRD_Remarks = "";
                                }
                                result.UpdatedDate = DateTime.Now;
                                _context.Update(result);
                            }

                        }
                        else
                        {
                            ExamPromotionRemarksDMO dmo = new ExamPromotionRemarksDMO();
                            dmo.MI_Id = data.MI_Id;
                            dmo.ASMAY_Id = data.ASMAY_Id;
                            dmo.ASMCL_Id = data.ASMCL_Id;
                            dmo.ASMS_Id = data.ASMS_Id;
                            dmo.EME_Id = data.EME_Id;
                            dmo.CreatedDate = DateTime.Now;
                            dmo.UpdatedDate = DateTime.Now;
                            if (data.examtype == 0)
                            {
                                dmo.EPRD_Promotionflag = "IE";
                            }
                            else
                            {
                                dmo.EPRD_Promotionflag = "PE";
                            }

                            dmo.AMST_Id = data.examtype_temp[i].AMST_Id;

                            if (data.examtype_temp[i].EPRD_PromotionName != null)
                            {
                                dmo.EPRD_PromotionName = data.examtype_temp[i].EPRD_PromotionName;
                            }
                            else
                            {
                                dmo.EPRD_PromotionName = "";
                            }

                            if (data.examtype_temp[i].EPRD_ClassPromoted != null)
                            {
                                dmo.EPRD_ClassPromoted = data.examtype_temp[i].EPRD_ClassPromoted;
                            }
                            else
                            {
                                dmo.EPRD_ClassPromoted = "";
                            }

                            if (data.examtype_temp[i].EPRD_Remarks != null)
                            {
                                dmo.EPRD_Remarks = data.examtype_temp[i].EPRD_Remarks;
                            }
                            else
                            {
                                dmo.EPRD_Remarks = "";
                            }
                            _context.Add(dmo);
                        }
                    }
                    int iff = _context.SaveChanges();
                    if (iff > 0)
                    {
                        data.message = "Add";
                        data.returnval = true;
                    }
                    else
                    {
                        data.message = "Add";
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _log.LogDebug("save_details() in Exam Promotion Remaks form" + ex.Message);
                _log.LogError("save_details() in Exam Promotion Remaks form" + ex.Message);
                data.message = "Add";
                data.returnval = false;
            }
            return data;
        }





        public ExamPromotionRemarksDTO search_groupwise_student(ExamPromotionRemarksDTO data)    //added by adarsh FOR Calcutta boys school
        {
            try
            {
                string order = "";
                var get_configuration = _context.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
                order = "studentname";
                if (get_configuration != null && get_configuration.Count > 0)
                {
                    if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "Name")
                    {
                        order = "studentname";
                    }
                    else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "AdmNo")
                    {
                        order = "AMST_Admno";
                    }
                    else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RollNo")
                    {
                        order = "AMAY_RollNo";
                    }
                    else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RegNo")
                    {
                        order = "AMST_RegistrationNo";
                    }
                }
                List<ExamPromotionRemarksDTO> studentList = new List<ExamPromotionRemarksDTO>();

                studentList = (from a in _context.Adm_M_Student
                               from b in _context.School_Adm_Y_StudentDMO
                               from e in _context.AcademicYear
                               from f in _context.AdmissionClass
                               from g in _context.School_M_Section
                               where (a.AMST_Id == b.AMST_Id && b.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == f.ASMCL_Id && b.ASMS_Id == g.ASMS_Id
                               && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id
                               && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1)
                               select new ExamPromotionRemarksDTO
                               {
                                   AMST_Id = a.AMST_Id,
                                   studentname = ((a.AMST_FirstName == null || a.AMST_FirstName == "" ? "" : " " + a.AMST_FirstName) + (a.AMST_MiddleName == null || a.AMST_MiddleName == "" || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) + (a.AMST_LastName == null || a.AMST_LastName == "" || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                   AMST_Admno = a.AMST_AdmNo == null ? "" : a.AMST_AdmNo,
                                   AMAY_RollNo = b.AMAY_RollNo,
                                   AMST_RegistrationNo = a.AMST_RegistrationNo == null ? "" : a.AMST_RegistrationNo
                               }).Distinct().OrderBy(a => order).ToList();

                var propertyInfo = typeof(ExamPromotionRemarksDTO).GetProperty(order);
                data.getstudentdetails = studentList.OrderBy(x => propertyInfo.GetValue(x, null)).ToArray();

                if (data.examtype == 0)
                {
                    data.getsavedetails = _context.ExamPromotionGroupwiseRemarksDMO.Where(a => a.MI_ID == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ASMCL_ID == data.ASMCL_Id && a.ASMS_ID == data.ASMS_Id && a.EMPSG_Id == data.EMPSG_Id).ToArray();

                    data.getstudentmarks = _context.ExmStudentMarksProcessDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id).ToArray();
                }
                else
                {
                    data.getsavedetails = _context.ExamPromotionGroupwiseRemarksDMO.Where(a => a.MI_ID == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ASMCL_ID == data.ASMCL_Id && a.ASMS_ID == data.ASMS_Id && a.EMPSG_Id == data.EMPSG_Id).ToArray();

                    data.getstudentmarks = _context.Exm_Student_MP_PromotionDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _log.LogDebug("search_student() in Exam Promotion Remaks form" + ex.Message);
                _log.LogError("search_student() in Exam Promotion Remaks form" + ex.Message);
            }
            return data;
        }

        public ExamPromotionRemarksDTO save_groupwise_details(ExamPromotionRemarksDTO data)
        {
            try
            {
                if (data.examgrpwise_rem.Length > 0)
                {
                    for (int i = 0; i < data.examgrpwise_rem.Length; i++)
                    {
                        if (data.examgrpwise_rem[i].ESGPCR_Id > 0)
                        {
                            if (data.examtype == 0)
                            {
                                //var result = _context.ExamPromotionGroupwiseRemarksDMO.Single(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id && a.AMST_Id == data.examtype_temp[i].AMST_Id && a.EPRD_Id == data.examtype_temp[i].EPRD_Id && a.EPRD_Promotionflag == "IE");
                                var result = _context.ExamPromotionGroupwiseRemarksDMO.Single(a => a.MI_ID == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_ID == data.ASMCL_Id && a.AMST_Id == data.examgrpwise_rem[i].AMST_Id && a.ESGPCR_Id == data.examgrpwise_rem[i].ESGPCR_Id);
                                if (data.examgrpwise_rem[i].ESGPCR_Remarks != null)
                                {
                                    result.ESGPCR_Remarks = data.examgrpwise_rem[i].ESGPCR_Remarks;
                                }
                                else
                                {
                                    result.ESGPCR_Remarks = "";
                                }
                                if (data.examgrpwise_rem[i].ESGPCR_Conduct != null)
                                {
                                    result.ESGPCR_Conduct = data.examgrpwise_rem[i].ESGPCR_Conduct;
                                }
                                else
                                {
                                    result.ESGPCR_Conduct = "";
                                }
                                if (data.examgrpwise_rem[i].EMPSG_Id != null)
                                {
                                    result.EMPSG_Id = data.examgrpwise_rem[i].EMPSG_Id;
                                }
                                else
                                {
                                    result.EMPSG_Id = "";
                                }
                                result.ESGPCR_UpdatedDate = DateTime.Now;
                                result.ESGPCR_UpdatedBy = data.userId;
                                _context.Update(result);
                            }
                            else
                            {
                                //added by adarsh FOR Calcutta boys school
                                var result = _context.ExamPromotionGroupwiseRemarksDMO.Single(x => x.MI_ID == data.MI_Id && x.ASMAY_Id == data.ASMAY_Id && x.ASMCL_ID == data.ASMCL_Id && x.AMST_Id == data.examgrpwise_rem[i].AMST_Id && x.ESGPCR_Id == data.examgrpwise_rem[i].ESGPCR_Id && x.EMPSG_Id == data.EMPSG_Id);
                                if (data.examgrpwise_rem[i].ESGPCR_Remarks != null)
                                {
                                    result.ESGPCR_Remarks = data.examgrpwise_rem[i].ESGPCR_Remarks;
                                }
                                else
                                {
                                    result.ESGPCR_Remarks = "";
                                }
                                if (data.examgrpwise_rem[i].ESGPCR_Conduct != null)
                                {
                                    result.ESGPCR_Conduct = data.examgrpwise_rem[i].ESGPCR_Conduct;
                                }
                                else
                                {
                                    result.ESGPCR_Conduct = "";
                                }
                                result.ESGPCR_UpdatedDate = DateTime.Now;
                                result.ESGPCR_UpdatedBy = data.userId;
                                _context.Update(result);
                            }

                        }
                        else
                        {
                            ExamPromotionGroupwiseRemarksDMO dmo = new ExamPromotionGroupwiseRemarksDMO(); //added by adarsh FOR Calcutta boys school
                            dmo.MI_ID = data.MI_Id;
                            dmo.ASMAY_Id = data.ASMAY_Id;
                            dmo.ASMS_ID = data.ASMS_Id;
                            dmo.ASMCL_ID = data.ASMCL_Id;
                            dmo.EMPSG_Id = data.EMPSG_Id;
                            dmo.AMST_Id = data.examgrpwise_rem[i].AMST_Id;
                            dmo.ESGPCR_Remarks = data.examgrpwise_rem[i].ESGPCR_Remarks;
                            dmo.ESGPCR_Conduct = data.examgrpwise_rem[i].ESGPCR_Conduct;
                            dmo.ESGPCR_CreatedDate = DateTime.Now;
                            dmo.ESGPCR_CreatedBy = data.userId;
                            dmo.ESGPCR_ActiveFlag = true;
                            _context.Add(dmo);
                        }
                    }
                    int iff = _context.SaveChanges();
                    if (iff > 0)
                    {
                        data.message = "Add";
                        data.returnval = true;
                    }
                    else
                    {
                        data.message = "Add";
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _log.LogDebug("save_details() in Exam Promotion Remaks form" + ex.Message);
                _log.LogError("save_details() in Exam Promotion Remaks form" + ex.Message);
                data.message = "Add";
                data.returnval = false;
            }
            return data;
        }

    }
}