
using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.Exam;
using AutoMapper;
using PreadmissionDTOs.com.vaps.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.MobileApp;
using DomainModel.Model.com.vapstech.Exam;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;

namespace ExamServiceHub.com.vaps.Services
{
    public class exammasterRemarkImpl : Interfaces.exammasterRemarkInterface
    {
        private static ConcurrentDictionary<string, exammasterRemarkDTO> _login =
         new ConcurrentDictionary<string, exammasterRemarkDTO>();

        private readonly ExamContext _examContext;
        DomainModelMsSqlServerContext _dbd;
        public exammasterRemarkImpl(ExamContext examContext, DomainModelMsSqlServerContext _db)
        {
            _examContext = examContext;
            _dbd = _db;
        }

        public exammasterRemarkDTO Getdetails(exammasterRemarkDTO data)
        {
            exammasterRemarkDTO getdata = new exammasterRemarkDTO();
            try
            {
                List<exammasterRemarkDMO> list = new List<exammasterRemarkDMO>();
                list = _examContext.exammasterRemarkDMO.Where(t => t.MI_Id == data.MI_Id).OrderBy(t => t.EPCR_RemarksOrder).ToList();
                getdata.exammasterRemaksname = list.ToArray();

                if (data.stringmobileorportal == "Mobile")
                {
                    List<IVRM_User_MobileApp_Login_Privileges> Staffmobileappprivileges = new List<IVRM_User_MobileApp_Login_Privileges>();
                    Staffmobileappprivileges = _dbd.IVRM_User_MobileApp_Login_Privileges.Where(t => t.IVRMUL_Id == data.User_Id && t.MI_Id == data.MI_Id).ToList();

                    if (Staffmobileappprivileges.Count() > 0)
                    {
                        data.Staffmobileappprivileges = (from Mobilepage in _dbd.IVRM_MobileApp_Page
                                                         from MobileRolePrivileges in _dbd.IVRM_Role_MobileApp_Privileges
                                                         from UserRolePrivileges in _dbd.IVRM_User_MobileApp_Login_Privileges
                                                         where (MobileRolePrivileges.MI_ID == UserRolePrivileges.MI_Id
                                                         && Mobilepage.IVRMMAP_Id == MobileRolePrivileges.IVRMMAP_Id
                                                         && Mobilepage.IVRMMAP_Id == UserRolePrivileges.IVRMMAP_Id
                                                         && MobileRolePrivileges.IVRMRT_Id == data.roleid
                                                         && MobileRolePrivileges.MI_ID == data.MI_Id && UserRolePrivileges.IVRMUL_Id == data.User_Id)
                                                         select new StudentTransactionDTO
                                                         {
                                                             Pagename = Mobilepage.IVRMMAP_AppPageName,
                                                             Pageicon = Mobilepage.IVRMMAP_AppPageDesc,
                                                             Pageurl = Mobilepage.IVRMMAP_AppPageURL,
                                                             IVRMRMAP_Id = MobileRolePrivileges.IVRMRMAP_Id,
                                                             IVRMMAP_AddFlg = UserRolePrivileges.IVRMUMALP_AddFlg,
                                                             IVRMMAP_UpdateFlg = UserRolePrivileges.IVRMUMALP_UpdateFlg,
                                                             IVRMMAP_DeleteFlg = UserRolePrivileges.IVRMUMALP_DeleteFlg
                                                         }).OrderBy(d => d.IVRMRMAP_Id).ToArray();

                        data.mobileprivileges = "true";
                    }
                    else
                    {
                        data.mobileprivileges = "false";
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return getdata;

        }

        //Onchange       
        public exammasterRemarkDTO validateordernumber(exammasterRemarkDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.examRemarkDTO.Count() > 0)
                {
                    foreach (exammasterRemarkDTO mob in dto.examRemarkDTO)
                    {
                        if (mob.EPCR_Id > 0)
                        {
                            var result = _examContext.exammasterRemarkDMO.Single(t => t.EPCR_Id.Equals(mob.EPCR_Id));
                            Mapper.Map(mob, result);
                            result.UpdatedDate = DateTime.Now;
                            _examContext.Update(result);
                            _examContext.SaveChanges();
                        }
                    }
                    dto.retrunMsg = "Order Updated Successfully";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured"; ;
            }
            return dto;
        }
        public exammasterRemarkDTO savedetails(exammasterRemarkDTO data)
        {

            if (data.EPCR_Id != 0)
            {
                var res = _examContext.exammasterRemarkDMO.Where(t => t.MI_Id == data.MI_Id && (t.EPCR_RemarksName == data.EPCR_RemarksName) && t.EPCR_Id != data.EPCR_Id).ToList();
                if (res.Count > 0)
                {
                    data.returnduplicatestatus = "Duplicate";
                }
                else
                {
                    var result = _examContext.exammasterRemarkDMO.Single(t => t.MI_Id == data.MI_Id && t.EPCR_Id == data.EPCR_Id);
                    result.EPCR_RemarksName = data.EPCR_RemarksName;
                    result.UpdatedDate = DateTime.Now;
                    _examContext.Update(result);
                    var contactExists = _examContext.SaveChanges();
                    if (contactExists == 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }

                }
            }
            else
            {
                var res = _examContext.exammasterRemarkDMO.Where(t => t.MI_Id == data.MI_Id && t.EPCR_RemarksName == data.EPCR_RemarksName).ToList();
                if (res.Count > 0)
                {
                    data.returnduplicatestatus = "Duplicate";
                }
                else
                {
                    var row_cnt = _examContext.exammasterRemarkDMO.Where(t => t.MI_Id == data.MI_Id).ToList().Count;
                    exammasterRemarkDMO exm = new exammasterRemarkDMO();
                    exm.EPCR_RemarksName = data.EPCR_RemarksName;
                    exm.EPCR_RemarksOrder = row_cnt + 1;
                    exm.MI_Id = data.MI_Id;
                    exm.EPCR_ActiveFlag = true;

                    exm.CreatedDate = DateTime.Now;
                    exm.UpdatedDate = DateTime.Now;
                    _examContext.Add(exm);
                    var contactExists = _examContext.SaveChanges();
                    if (contactExists == 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }

            }

            return data;
        }
        public exammasterRemarkDTO editdetails(int ID)
        {
            exammasterRemarkDTO editlt = new exammasterRemarkDTO();
            try
            {
                List<exammasterRemarkDMO> list = new List<exammasterRemarkDMO>();
                list = _examContext.exammasterRemarkDMO.Where(t => t.EPCR_Id == ID).ToList();
                editlt.exammRemakname = list.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return editlt;
        }
        public exammasterRemarkDTO deactivate(exammasterRemarkDTO data)
        {
            data.already_cnt = false;
            exammasterRemarkDMO master = Mapper.Map<exammasterRemarkDMO>(data);
            if (master.EPCR_Id > 0)
            {
                var result = _examContext.exammasterRemarkDMO.Single(t => t.EPCR_Id == master.EPCR_Id);
                if (result.EPCR_ActiveFlag == true)
                {
                    result.EPCR_ActiveFlag = false;
                    result.UpdatedDate = DateTime.Now;
                    _examContext.Update(result);
                }
                else
                {
                    result.EPCR_ActiveFlag = true;
                    result.UpdatedDate = DateTime.Now;
                    _examContext.Update(result);
                }

                var flag = _examContext.SaveChanges();
                if (flag == 1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            return data;
        }

        // Mappping
        public exammasterRemarkDTO studentdataload(exammasterRemarkDTO data)
        {
            try
            {

                List<long> classid = new List<long>();
                List<long> sectionid = new List<long>();

                data.yearlist = _examContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                var check_rolename = (from a in _examContext.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleid)
                                      select new exammasterCoCurricularDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();

                var empcode_check = (from a in _examContext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(data.User_Id))
                                     select new exammasterCoCurricularDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                if (empcode_check.Count() > 0)
                {
                    var check_classteacher = _examContext.ClassTeacherMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.IMCT_ActiveFlag == true && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code).ToList();

                    if (check_classteacher.Count() > 0)
                    {
                        for (int i = 0; i < check_classteacher.Count; i++)
                        {
                            classid.Add(check_classteacher[i].ASMCL_Id);
                            sectionid.Add(check_classteacher[i].ASMS_Id);

                            data.loaddata = (from a in _examContext.Exm_ProgressCard_RemarksDMO
                                             from c in _examContext.exammasterDMO
                                             from d in _examContext.AcademicYear
                                             from e in _examContext.AdmissionClass
                                             from f in _examContext.School_M_Section
                                             where (a.ASMAY_Id == d.ASMAY_Id && a.ASMCL_Id == e.ASMCL_Id && a.ASMS_Id == f.ASMS_Id
                                             && a.EME_ID == c.EME_Id && a.MI_Id == data.MI_Id && classid.Contains(a.ASMCL_Id) && sectionid.Contains(a.ASMS_Id))
                                             select new exammasterRemarkDTO
                                             {
                                                 ASMCL_ClassName = e.ASMCL_ClassName,
                                                 ASMC_SectionName = f.ASMC_SectionName,
                                                 ASMAY_Year = d.ASMAY_Year,
                                                 ASMCL_Id = a.ASMCL_Id,
                                                 ASMS_Id = a.ASMS_Id,
                                                 ASMAY_Id = a.ASMAY_Id,
                                                 EME_Id = a.EME_ID,
                                                 EME_ExamName = c.EME_ExamName,
                                                 EME_ExamOrder = c.EME_ExamOrder,
                                                 ASMAY_Order = d.ASMAY_Order
                                             }).Distinct().OrderByDescending(a => a.ASMAY_Order).ThenBy(a => a.EME_ExamOrder).ToArray();
                        }
                    }
                }
                else
                {
                    data.loaddata = (from a in _examContext.Exm_ProgressCard_RemarksDMO
                                     from c in _examContext.exammasterDMO
                                     from d in _examContext.AcademicYear
                                     from e in _examContext.AdmissionClass
                                     from f in _examContext.School_M_Section
                                     where (a.ASMAY_Id == d.ASMAY_Id && a.ASMCL_Id == e.ASMCL_Id && a.ASMS_Id == f.ASMS_Id
                                     && a.EME_ID == c.EME_Id && a.MI_Id == data.MI_Id)
                                     select new exammasterRemarkDTO
                                     {
                                         ASMCL_ClassName = e.ASMCL_ClassName,
                                         ASMC_SectionName = f.ASMC_SectionName,
                                         ASMAY_Year = d.ASMAY_Year,
                                         ASMCL_Id = a.ASMCL_Id,
                                         ASMS_Id = a.ASMS_Id,
                                         ASMAY_Id = a.ASMAY_Id,
                                         EME_Id = a.EME_ID,
                                         EME_ExamName = c.EME_ExamName,
                                         EME_ExamOrder = c.EME_ExamOrder,
                                         ASMAY_Order = d.ASMAY_Order
                                     }).Distinct().OrderByDescending(a => a.ASMAY_Order).ThenBy(a => a.EME_ExamOrder).ToArray();
                }


                if (data.stringmobileorportal == "Mobile")
                {
                    List<IVRM_User_MobileApp_Login_Privileges> Staffmobileappprivileges = new List<IVRM_User_MobileApp_Login_Privileges>();
                    Staffmobileappprivileges = _dbd.IVRM_User_MobileApp_Login_Privileges.Where(t => t.IVRMUL_Id == data.User_Id && t.MI_Id == data.MI_Id).ToList();

                    if (Staffmobileappprivileges.Count() > 0)
                    {
                        data.Staffmobileappprivileges = (from Mobilepage in _dbd.IVRM_MobileApp_Page
                                                         from MobileRolePrivileges in _dbd.IVRM_Role_MobileApp_Privileges
                                                         from UserRolePrivileges in _dbd.IVRM_User_MobileApp_Login_Privileges
                                                         where (MobileRolePrivileges.MI_ID == UserRolePrivileges.MI_Id
                                                         && Mobilepage.IVRMMAP_Id == MobileRolePrivileges.IVRMMAP_Id
                                                         && Mobilepage.IVRMMAP_Id == UserRolePrivileges.IVRMMAP_Id
                                                         && MobileRolePrivileges.IVRMRT_Id == data.roleid
                                                         && MobileRolePrivileges.MI_ID == data.MI_Id && UserRolePrivileges.IVRMUL_Id == data.User_Id)
                                                         select new StudentTransactionDTO
                                                         {
                                                             Pagename = Mobilepage.IVRMMAP_AppPageName,
                                                             Pageicon = Mobilepage.IVRMMAP_AppPageDesc,
                                                             Pageurl = Mobilepage.IVRMMAP_AppPageURL,
                                                             IVRMRMAP_Id = MobileRolePrivileges.IVRMRMAP_Id,
                                                             IVRMMAP_AddFlg = UserRolePrivileges.IVRMUMALP_AddFlg,
                                                             IVRMMAP_UpdateFlg = UserRolePrivileges.IVRMUMALP_UpdateFlg,
                                                             IVRMMAP_DeleteFlg = UserRolePrivileges.IVRMUMALP_DeleteFlg
                                                         }).OrderBy(d => d.IVRMRMAP_Id).ToArray();

                        data.mobileprivileges = "true";
                    }
                    else
                    {
                        data.mobileprivileges = "false";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;

        }
        public exammasterRemarkDTO onchangeyear(exammasterRemarkDTO data)
        {
            try
            {
                var check_rolename = (from a in _examContext.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleid)
                                      select new exammasterRemarkDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }
                                 ).ToList();

                var empcode_check = (from a in _examContext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(data.User_Id))
                                     select new exammasterRemarkDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();
                if (empcode_check.Count() > 0)
                {
                    var check_classteacher = _examContext.ClassTeacherMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.IMCT_ActiveFlag == true && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code).ToList();

                    if (check_classteacher.Count() > 0)
                    {
                        data.classlist = (from a in _examContext.ClassTeacherMappingDMO
                                          from b in _examContext.AdmissionClass
                                          from c in _examContext.AcademicYear
                                          where (a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id
                                          && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && a.IMCT_ActiveFlag == true && b.ASMCL_ActiveFlag == true)
                                          select new exammasterRemarkDTO
                                          {
                                              ASMCL_Id = b.ASMCL_Id,
                                              ASMCL_ClassName = b.ASMCL_ClassName,
                                              ASMCL_ClassOrder = b.ASMCL_Order
                                          }).Distinct().OrderBy(a => a.ASMCL_ClassOrder).ToArray();
                    }
                }
                else
                {
                    data.classlist = _examContext.AdmissionClass.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true).OrderBy(a => a.ASMCL_Order).ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;

        }
        public exammasterRemarkDTO onchangeclass(exammasterRemarkDTO data)
        {
            try
            {
                var check_rolename = (from a in _examContext.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleid)
                                      select new exammasterRemarkDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();

                var empcode_check = (from a in _examContext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(data.User_Id))
                                     select new exammasterRemarkDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                if (empcode_check.Count() > 0)
                {
                    var check_classteacher = _examContext.ClassTeacherMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.IMCT_ActiveFlag == true && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code).ToList();

                    if (check_classteacher.Count() > 0)
                    {
                        data.sectionlist = (from a in _examContext.ClassTeacherMappingDMO
                                            from b in _examContext.AdmissionClass
                                            from d in _examContext.School_M_Section
                                            from c in _examContext.AcademicYear
                                            where (a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == d.ASMS_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id
                                            && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && a.IMCT_ActiveFlag == true && b.ASMCL_ActiveFlag == true)
                                            select new exammasterRemarkDTO
                                            {
                                                ASMS_Id = d.ASMS_Id,
                                                ASMC_SectionName = d.ASMC_SectionName,
                                                ASMC_Order = d.ASMC_Order
                                            }).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
                    }
                }
                else
                {
                    data.sectionlist = _examContext.School_M_Section.Where(a => a.MI_Id == data.MI_Id && a.ASMC_ActiveFlag == 1).OrderBy(a => a.ASMC_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;

        }
        public exammasterRemarkDTO onchangesection(exammasterRemarkDTO data)
        {
            try
            {
                var getemcaid = _examContext.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Distinct().ToArray();

                var geteycid = _examContext.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.EYC_ActiveFlg == true
                && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id).Distinct().ToList();

                data.examlist = (from a in _examContext.Exm_Yearly_Category_ExamsDMO
                                 from b in _examContext.exammasterDMO
                                 where (a.EME_Id == b.EME_Id && a.EYCE_ActiveFlg == true && a.EYC_Id == geteycid.FirstOrDefault().EYC_Id)
                                 select b).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }
        public exammasterRemarkDTO searchdata(exammasterRemarkDTO data)
        {
            try
            {
                string order = "";
                var get_configuration = _examContext.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
                data.configuration = get_configuration.ToArray();

                if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "Name")
                {
                    order = "studentname";
                }
                else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "AdmNo")
                {
                    order = "AMST_AdmNo";
                }
                else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RollNo")
                {
                    order = "AMAY_RollNo";
                }
                else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RegNo")
                {
                    order = "AMST_RegistrationNo";
                }
                else
                {
                    order = "studentname";
                }

                List<exammasterRemarkDTO> studentList = new List<exammasterRemarkDTO>();

                studentList = (from a in _examContext.School_Adm_Y_StudentDMO
                               from b in _examContext.Adm_M_Student
                               from c in _examContext.AcademicYear
                               from d in _examContext.AdmissionClass
                               from e in _examContext.School_M_Section
                               where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                               && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && b.MI_Id == data.MI_Id
                               && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1)
                               select new exammasterRemarkDTO
                               {
                                   AMST_Id = a.AMST_Id,
                                   studentname = ((b.AMST_FirstName == null || b.AMST_FirstName == "" ? "" : b.AMST_FirstName) +
                                   (b.AMST_MiddleName == null || b.AMST_MiddleName == "" ? "" : " " + b.AMST_MiddleName) +
                                   (b.AMST_LastName == null || b.AMST_LastName == "" ? "" : " " + b.AMST_LastName)).Trim(),
                                   AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                                   AMST_RegistrationNo = b.AMST_RegistrationNo,
                                   AMAY_RollNo = a.AMAY_RollNo

                               }).Distinct().ToList();

                var propertyInfo = typeof(exammasterRemarkDTO).GetProperty(order);
                studentList = studentList.OrderBy(x => propertyInfo.GetValue(x, null)).ToList();
                data.studentList = studentList.ToArray();

                data.savedata = _examContext.Exm_ProgressCard_RemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                  && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_ID == data.EME_Id).ToArray();

                data.getstudentmarks = _examContext.ExmStudentMarksProcessDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id).ToArray();

                data.get_subjectwiseremarks = (from a in _examContext.Exm_Student_ProgressCard_SubjectWise_RemarksDMO
                                               where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id
                                               && a.EME_ID == data.EME_Id)
                                               select new exammasterRemarkDTO
                                               {
                                                   AMST_Id = a.AMST_Id
                                               }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }
        public exammasterRemarkDTO savemapping(exammasterRemarkDTO data)
        {
            try
            {
                data.returnval = false;
                data.returnval = false;

                if (data.Temp_studentdata_Remarks != null && data.Temp_studentdata_Remarks.Length > 0)
                {
                    for (int i = 0; i < data.Temp_studentdata_Remarks.Length; i++)
                    {
                        var check_stdid = _examContext.Exm_ProgressCard_RemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_ID == data.EME_Id && a.AMST_Id == data.Temp_studentdata_Remarks[i].AMST_Id).ToList();

                        if (check_stdid.Count() > 0)
                        {
                            var result = _examContext.Exm_ProgressCard_RemarksDMO.Single(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_ID == data.EME_Id && a.AMST_Id == data.Temp_studentdata_Remarks[i].AMST_Id);
                            result.EMER_Remarks = data.Temp_studentdata_Remarks[i].EMER_Remarks;
                            result.UpdatedDate = DateTime.Now;
                            _examContext.Update(result);
                        }
                        else
                        {
                            Exm_ProgressCard_RemarksDMO map = new Exm_ProgressCard_RemarksDMO();
                            map.AMST_Id = data.Temp_studentdata_Remarks[i].AMST_Id;
                            map.ASMAY_Id = data.ASMAY_Id;
                            map.ASMCL_Id = data.ASMCL_Id;
                            map.ASMS_Id = data.ASMS_Id;
                            map.MI_Id = data.MI_Id;
                            map.EMER_Remarks = data.Temp_studentdata_Remarks[i].EMER_Remarks;
                            map.EMER_ActiveFlag = true;
                            map.CreatedDate = DateTime.Now;
                            map.UpdatedDate = DateTime.Now;
                            map.EME_ID = data.EME_Id;
                            _examContext.Add(map);
                        }
                    }
                    var d = _examContext.SaveChanges();
                    if (d > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = false;
            }
            return data;
        }
        public exammasterRemarkDTO editmappingdetails(exammasterRemarkDTO data)
        {
            try
            {
                string order = "";
                var get_configuration = _examContext.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
                data.configuration = get_configuration.ToArray();

                if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "Name")
                {
                    order = "studentname";
                }
                else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "AdmNo")
                {
                    order = "AMST_AdmNo";
                }
                else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RollNo")
                {
                    order = "AMAY_RollNo";
                }
                else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RegNo")
                {
                    order = "AMST_RegistrationNo";
                }
                else
                {
                    order = "studentname";
                }

                List<exammasterRemarkDTO> studentList = new List<exammasterRemarkDTO>();

                studentList = (from a in _examContext.School_Adm_Y_StudentDMO
                               from b in _examContext.Adm_M_Student
                               from c in _examContext.AcademicYear
                               from d in _examContext.AdmissionClass
                               from e in _examContext.School_M_Section
                               where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                               && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && b.MI_Id == data.MI_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1)
                               select new exammasterRemarkDTO
                               {
                                   AMST_Id = a.AMST_Id,
                                   studentname = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                                   AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                                   AMST_RegistrationNo = b.AMST_RegistrationNo,
                                   AMAY_RollNo = a.AMAY_RollNo

                               }).Distinct().ToList();

                var propertyInfo = typeof(exammasterRemarkDTO).GetProperty(order);
                studentList = studentList.OrderBy(x => propertyInfo.GetValue(x, null)).ToList();
                data.studentList = studentList.ToArray();

                data.savedata = _examContext.Exm_ProgressCard_RemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_ID == data.EME_Id).ToArray();

                data.get_subjectwiseremarks = (from a in _examContext.Exm_Student_ProgressCard_SubjectWise_RemarksDMO
                                               where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id
                                               && a.EME_ID == data.EME_Id)
                                               select new exammasterRemarkDTO
                                               {
                                                   AMST_Id = a.AMST_Id
                                               }).Distinct().ToArray();

                data.yearlist = _examContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id).ToArray();
                data.classlist = _examContext.AdmissionClass.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id).ToArray();
                data.sectionlist = _examContext.School_M_Section.Where(a => a.MI_Id == data.MI_Id && a.ASMS_Id == data.ASMS_Id).ToArray();
                data.examlist = _examContext.exammasterDMO.Where(a => a.EME_Id == data.EME_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }
        public exammasterRemarkDTO ViewSubjectWiseRemarks(exammasterRemarkDTO data)
        {
            try
            {
                data.get_viewsubjectwiseremarks = (from a in _examContext.Exm_Student_ProgressCard_SubjectWise_RemarksDMO
                                                   from b in _examContext.IVRM_School_Master_SubjectsDMO
                                                   where (a.ISMS_Id == b.ISMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                                                   && a.ASMS_Id == data.ASMS_Id && a.EME_ID == data.EME_Id && a.AMST_Id == data.AMST_Id)
                                                   select new exammasterRemarkDTO
                                                   {
                                                       ISMS_Id = a.ISMS_Id,
                                                       ISMS_SubjectName = b.ISMS_SubjectName,
                                                       ESPCSR_Remarks = a.ESSEPCR_Remarks,
                                                       ESPCSR_Id = a.ESSEPCR_Id,
                                                       ISMS_OrderFlag = b.ISMS_OrderFlag
                                                   }).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }

        //Subject Wise Remarks
        public exammasterRemarkDTO Subjectwise_studentdataload(exammasterRemarkDTO data)
        {
            try
            {
                data.yearlist = _examContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.classlist = Subjectwise_onchangeyear(data).classlist;

                using (var cmd = _examContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Get_Exam_SubjectWise_Student_Remarks_LoadData_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = data.User_Id });                  
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
                        data.loaddata = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }
        public exammasterRemarkDTO Subjectwise_onchangeyear(exammasterRemarkDTO data)
        {
            try
            {
                var getuserdetails = _examContext.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Id == data.User_Id).ToList();

                List<exammasterRemarkDTO> getclass = new List<exammasterRemarkDTO>();
                List<long> classid = new List<long>();

                if (getuserdetails.Count > 0)
                {
                    var getempcode = getuserdetails.FirstOrDefault().Emp_Code;
                    var loginid = getuserdetails.FirstOrDefault().IVRMSTAUL_Id;

                    getclass = (from a in _examContext.Exm_Login_PrivilegeDMO
                                from c in _examContext.Exm_Login_Privilege_SubjectsDMO
                                from d in _examContext.AdmissionClass
                                from e in _examContext.Staff_User_Login
                                where (a.ELP_Id == c.ELP_Id && e.IVRMSTAUL_Id == a.Login_Id && c.ASMCL_Id == d.ASMCL_Id && a.ELP_ActiveFlg == true
                                && a.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && c.ELPs_ActiveFlg == true && a.Login_Id == loginid
                                && a.ASMAY_Id == data.ASMAY_Id)
                                select new exammasterRemarkDTO
                                {
                                    ASMCL_Id = c.ASMCL_Id
                                }).Distinct().ToList();


                    if (getclass.Count > 0)
                    {
                        foreach (var c in getclass)
                        {
                            classid.Add(c.ASMCL_Id);
                        }

                        data.classlist = _examContext.AdmissionClass.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true
                        && classid.Contains(a.ASMCL_Id)).OrderBy(a => a.ASMCL_Order).ToArray();
                    }
                }
                else
                {
                    data.classlist = (from a in _examContext.Masterclasscategory
                                      from b in _examContext.AcademicYear
                                      from c in _examContext.AdmissionClass
                                      where (a.ASMCL_Id == c.ASMCL_Id && a.ASMAY_Id == b.ASMAY_Id && a.Is_Active == true && a.ASMAY_Id == data.ASMAY_Id)
                                      select c).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }
        public exammasterRemarkDTO Subjectwise_onchangeclass(exammasterRemarkDTO data)
        {
            try
            {
                var getuserdetails = _examContext.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Id == data.User_Id).ToList();

                List<exammasterRemarkDTO> getsection = new List<exammasterRemarkDTO>();
                List<long> classsecid = new List<long>();

                if (getuserdetails.Count > 0)
                {
                    var getempcode = getuserdetails.FirstOrDefault().Emp_Code;
                    var loginid = getuserdetails.FirstOrDefault().IVRMSTAUL_Id;

                    getsection = (from a in _examContext.Exm_Login_PrivilegeDMO
                                  from c in _examContext.Exm_Login_Privilege_SubjectsDMO
                                  from d in _examContext.AdmissionClass
                                  from e in _examContext.Staff_User_Login
                                  from f in _examContext.School_M_Section
                                  where (a.ELP_Id == c.ELP_Id && e.IVRMSTAUL_Id == a.Login_Id && c.ASMCL_Id == d.ASMCL_Id && f.ASMS_Id == c.ASMS_Id
                                  && a.ELP_ActiveFlg == true && a.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && c.ELPs_ActiveFlg == true && a.Login_Id == loginid
                                  && a.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == data.ASMCL_Id)
                                  select new exammasterRemarkDTO
                                  {
                                      ASMS_Id = c.ASMS_Id
                                  }).Distinct().ToList();


                    if (getsection.Count > 0)
                    {
                        foreach (var c in getsection)
                        {
                            classsecid.Add(c.ASMS_Id);
                        }

                        data.sectionlist = _examContext.School_M_Section.Where(a => a.MI_Id == data.MI_Id && a.ASMC_ActiveFlag == 1
                        && classsecid.Contains(a.ASMS_Id)).OrderBy(a => a.ASMC_Order).ToArray();
                    }
                }
                else
                {
                    data.sectionlist = (from a in _examContext.Masterclasscategory
                                        from b in _examContext.AcademicYear
                                        from c in _examContext.AdmissionClass
                                        from d in _examContext.School_M_Section
                                        from f in _examContext.AdmSchoolMasterClassCatSec
                                        where (a.ASMCL_Id == c.ASMCL_Id && a.ASMCC_Id == f.ASMCC_Id && d.ASMS_Id == f.ASMS_Id && a.ASMAY_Id == b.ASMAY_Id
                                        && f.ASMCCS_ActiveFlg == true && a.Is_Active == true && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id)
                                        select d).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }
        public exammasterRemarkDTO Subjectwise_onchangesection(exammasterRemarkDTO data)
        {
            try
            {
                var getemcaid = _examContext.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Distinct().ToArray();

                var geteycid = _examContext.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.EYC_ActiveFlg == true
                && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id).Distinct().ToList();

                data.examlist = (from a in _examContext.Exm_Yearly_Category_ExamsDMO
                                 from b in _examContext.exammasterDMO
                                 where (a.EME_Id == b.EME_Id && a.EYCE_ActiveFlg == true && a.EYC_Id == geteycid.FirstOrDefault().EYC_Id)
                                 select b).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }
        public exammasterRemarkDTO Subjectwise_onchangeexam(exammasterRemarkDTO data)
        {
            try
            {
                var getemcaid = _examContext.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Distinct().ToArray();

                var geteycid = _examContext.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.EYC_ActiveFlg == true
                && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id).Distinct().ToList();

                var subjectlist = (from a in _examContext.Exm_Yearly_Category_ExamsDMO
                                   from b in _examContext.exammasterDMO
                                   from c in _examContext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                   where (a.EME_Id == b.EME_Id && a.EYCE_Id == c.EYCE_Id && a.EYCE_ActiveFlg == true && c.EYCES_ActiveFlg == true
                                   && a.EYC_Id == geteycid.FirstOrDefault().EYC_Id && a.EME_Id == data.EME_Id)
                                   select new exammasterRemarkDTO
                                   {
                                       ISMS_Id = c.ISMS_Id
                                   }).Distinct().ToList();

                List<long> ismsid = new List<long>();

                foreach (var c in subjectlist)
                {
                    ismsid.Add(c.ISMS_Id);
                }
                var getuserdetails = _examContext.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Id == data.User_Id).ToList();

                List<exammasterRemarkDTO> getclass = new List<exammasterRemarkDTO>();
                List<long> classid = new List<long>();

                if (getuserdetails.Count > 0)
                {
                    var getempcode = getuserdetails.FirstOrDefault().Emp_Code;
                    var loginid = getuserdetails.FirstOrDefault().IVRMSTAUL_Id;

                    data.subjectlist = (from a in _examContext.Exm_Login_PrivilegeDMO
                                        from c in _examContext.Exm_Login_Privilege_SubjectsDMO
                                        from d in _examContext.IVRM_School_Master_SubjectsDMO
                                        from e in _examContext.Staff_User_Login
                                        where (a.ELP_Id == c.ELP_Id && e.IVRMSTAUL_Id == a.Login_Id && c.ISMS_Id == d.ISMS_Id && a.ELP_ActiveFlg == true
                                        && a.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && c.ELPs_ActiveFlg == true && a.Login_Id == loginid
                                        && e.Emp_Code == getempcode && c.ASMCL_Id == data.ASMCL_Id && a.ASMAY_Id == data.ASMAY_Id && c.ASMS_Id == data.ASMS_Id
                                        && ismsid.Contains(c.ISMS_Id))
                                        select new exammasterRemarkDTO
                                        {
                                            ISMS_Id = c.ISMS_Id,
                                            ISMS_SubjectName = d.ISMS_SubjectName,
                                            ISMS_OrderFlag = d.ISMS_OrderFlag
                                        }).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
                }
                else
                {
                    data.subjectlist = _examContext.IVRM_School_Master_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && ismsid.Contains(a.ISMS_Id)).OrderBy(a => a.ISMS_OrderFlag).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }
        public exammasterRemarkDTO Subjectwise_searchdata(exammasterRemarkDTO data)
        {
            try
            {
                var getemcaid = _examContext.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Distinct().ToArray();

                var geteycid = _examContext.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.EYC_ActiveFlg == true
                && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id).Distinct().ToList();

                string order = "";
                var get_configuration = _examContext.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
                data.configuration = get_configuration.ToArray();

                if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "Name")
                {
                    order = "studentname";
                }
                else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "AdmNo")
                {
                    order = "AMST_AdmNo";
                }
                else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RollNo")
                {
                    order = "AMAY_RollNo";
                }
                else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RegNo")
                {
                    order = "AMST_RegistrationNo";
                }
                else
                {
                    order = "studentname";
                }

                List<exammasterRemarkDTO> studentList = new List<exammasterRemarkDTO>();

                studentList = (from a in _examContext.School_Adm_Y_StudentDMO
                               from b in _examContext.Adm_M_Student
                               from c in _examContext.AcademicYear
                               from d in _examContext.AdmissionClass
                               from e in _examContext.School_M_Section
                               from f in _examContext.StudentMappingDMO
                               from g in _examContext.IVRM_School_Master_SubjectsDMO
                               where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                               && a.AMST_Id == f.AMST_Id && f.ASMAY_Id == c.ASMAY_Id && f.ASMCL_Id == d.ASMCL_Id && f.ASMS_Id == e.ASMS_Id && f.ISMS_Id == g.ISMS_Id
                               && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && b.MI_Id == data.MI_Id
                               && f.ASMAY_Id == data.ASMAY_Id && f.ASMCL_Id == data.ASMCL_Id && f.ASMS_Id == data.ASMS_Id && f.MI_Id == data.MI_Id
                               && f.ISMS_Id == data.ISMS_Id && f.ESTSU_ActiveFlg == true && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1)
                               select new exammasterRemarkDTO
                               {
                                   AMST_Id = a.AMST_Id,
                                   studentname = ((b.AMST_FirstName == null || b.AMST_FirstName == "" ? "" : b.AMST_FirstName) +
                                   (b.AMST_MiddleName == null || b.AMST_MiddleName == "" ? "" : " " + b.AMST_MiddleName) +
                                   (b.AMST_LastName == null || b.AMST_LastName == "" ? "" : " " + b.AMST_LastName)).Trim(),
                                   AMST_AdmNo = b.AMST_AdmNo == null || b.AMST_AdmNo == "" ? "" : b.AMST_AdmNo,
                                   AMST_RegistrationNo = b.AMST_RegistrationNo == null || b.AMST_RegistrationNo == "" ? "" : b.AMST_RegistrationNo,
                                   AMAY_RollNo = a.AMAY_RollNo,
                               }).Distinct().ToList();

                var propertyInfo = typeof(exammasterRemarkDTO).GetProperty(order);
                studentList = studentList.OrderBy(x => propertyInfo.GetValue(x, null)).ToList();
                data.studentList = studentList.ToArray();

                data.savedata = _examContext.Exm_Student_ProgressCard_SubjectWise_RemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                  && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_ID == data.EME_Id && a.ISMS_Id == data.ISMS_Id).ToArray();

                data.getstudentmarks = _examContext.ExmStudentMarksProcessSubjectwiseDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id && a.ISMS_Id == data.ISMS_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }
        public exammasterRemarkDTO SubjectWise_savemapping(exammasterRemarkDTO data)
        {
            try
            {
                data.returnval = false;

                if (data.Temp_studentdata_Remarks != null && data.Temp_studentdata_Remarks.Length > 0)
                {
                    for (int i = 0; i < data.Temp_studentdata_Remarks.Length; i++)
                    {
                        long AMST_Id = data.Temp_studentdata_Remarks[i].AMST_Id;
                        string EMER_Remarks = data.Temp_studentdata_Remarks[i].EMER_Remarks;

                        var check_stdid = _examContext.Exm_Student_ProgressCard_SubjectWise_RemarksDMO.Where(a => a.MI_Id == data.MI_Id
                        && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_ID == data.EME_Id
                        && a.AMST_Id == AMST_Id && a.ISMS_Id == data.ISMS_Id).ToList();

                        if (check_stdid.Count() > 0)
                        {
                            var result = _examContext.Exm_Student_ProgressCard_SubjectWise_RemarksDMO.Single(a => a.MI_Id == data.MI_Id
                            && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_ID == data.EME_Id
                            && a.AMST_Id == AMST_Id && a.ISMS_Id == data.ISMS_Id);
                            result.ESSEPCR_Remarks = EMER_Remarks;
                            result.ESSEPCR_UpdatedDate = DateTime.Now;
                            result.ESSEPCR_UpdatedBy = data.User_Id;
                            _examContext.Update(result);
                        }
                        else
                        {
                            Exm_Student_ProgressCard_SubjectWise_RemarksDMO map = new Exm_Student_ProgressCard_SubjectWise_RemarksDMO
                            {
                                AMST_Id = AMST_Id,
                                ASMAY_Id = data.ASMAY_Id,
                                ASMCL_Id = data.ASMCL_Id,
                                ASMS_Id = data.ASMS_Id,
                                MI_Id = data.MI_Id,
                                ESSEPCR_Remarks = EMER_Remarks,
                                ESSEPCR_ActiveFlag = true,
                                ESSEPCR_CreatedDate = DateTime.Now,
                                ESSEPCR_UpdatedDate = DateTime.Now,
                                ESSEPCR_CreatedBy = data.User_Id,
                                ESSEPCR_UpdatedBy = data.User_Id,
                                EME_ID = data.EME_Id,
                                ISMS_Id = data.ISMS_Id
                            };
                            _examContext.Add(map);
                        }
                    }

                    var d = _examContext.SaveChanges();
                    if (d > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }
        public exammasterRemarkDTO SubjectWise_editmappingdetails(exammasterRemarkDTO data)
        {
            try
            {
                exammasterRemarkDTO new_dto = new exammasterRemarkDTO();

                new_dto = Subjectwise_searchdata(data);

                data.studentList = new_dto.studentList;
                data.savedata = new_dto.savedata;
                data.getstudentmarks = new_dto.getstudentmarks;
                data.configuration = new_dto.configuration;

                data.yearlist = _examContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id).ToArray();
                data.classlist = _examContext.AdmissionClass.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id).ToArray();
                data.sectionlist = _examContext.School_M_Section.Where(a => a.MI_Id == data.MI_Id && a.ASMS_Id == data.ASMS_Id).ToArray();
                data.examlist = _examContext.exammasterDMO.Where(a => a.EME_Id == data.EME_Id).ToArray();
                data.subjectlist = _examContext.IVRM_School_Master_SubjectsDMO.Where(a => a.ISMS_Id == data.ISMS_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }
    }
}