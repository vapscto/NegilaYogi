using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Sports;
using DomainModel.Model.com.vapstech.MobileApp;
using DomainModel.Model.com.vapstech.Sports;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Sports;
using SportsServiceHub.com.vaps.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Services
{
    public class BMICalculationImpl : BMICalculationInterface
    {
        DomainModelMsSqlServerContext _db;
        SportsContext _context;
        public BMICalculationImpl(DomainModelMsSqlServerContext db, SportsContext context)
        {
            _db = db;
            _context = context;
        }
        public BMICalculationDTO getDetails(BMICalculationDTO data)
        {
            try
            {
                data.academicYear = _context.AcademicYear.Where(d => d.MI_Id == data.MI_Id && d.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();
                data.classList = _db.School_M_Class.Where(d => d.MI_Id == data.MI_Id && d.ASMCL_ActiveFlag == true).ToArray();
                data.sectionList = _db.School_M_Section.Where(d => d.MI_Id == data.MI_Id && d.ASMC_ActiveFlag == 1).ToArray();

                data.ASMAY_Id = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

                var count = _context.Staff_User_Login.Where(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).ToList();
                if (count.Count > 0)
                {
                    data.HRME_Id = _context.Staff_User_Login.Where(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).FirstOrDefault().Emp_Code;
                    if (data.HRME_Id > 0)
                    {
                        var check_classteacher = _context.ClassTeacherMappingDMO.Where(a => a.MI_Id == data.MI_Id
                        && a.ASMAY_Id == data.ASMAY_Id && a.IMCT_ActiveFlag == true
                        && a.HRME_Id == data.HRME_Id).ToList();

                        if (check_classteacher.Count() > 0)
                        {
                            data.studentrecord = (from a in _context.ClassTeacherMappingDMO
                                                  from b in _context.admissionClass
                                                  from c in _context.BMICalculationDMO
                                                  from y in _context.admissionyearstudent
                                                  from d in _context.masterSection
                                                  from s in _context.Adm_M_Student
                                                  from yr in _context.AcademicYear

                                                  where (a.HRME_Id == data.HRME_Id && y.ASMAY_Id == a.ASMAY_Id && y.ASMCL_Id == a.ASMCL_Id && y.ASMS_Id == a.ASMS_Id
                                                  && y.ASMAY_Id == c.ASMAY_Id && y.ASMCL_Id == c.ASMCL_Id && y.ASMS_Id == c.ASMS_Id && y.AMST_Id == c.AMST_Id && y.ASMCL_Id == b.ASMCL_Id && y.ASMS_Id == d.ASMS_Id && y.AMST_Id == s.AMST_Id && y.ASMAY_Id == yr.ASMAY_Id && y.AMAY_ActiveFlag == 1 && a.IMCT_ActiveFlag == true && s.AMST_SOL == "S")
                                                  select new BMICalculationDTO
                                                  {
                                                      SPCCSHW_Id = c.SPCCSHW_Id,
                                                      ASMAY_Id = a.ASMAY_Id,
                                                      ASMCL_Id = a.ASMCL_Id,
                                                      ASMS_Id = a.ASMS_Id,
                                                      AMST_Id = c.AMST_Id,
                                                      SPCCSHW_AsOnDate = c.SPCCSHW_AsOnDate,
                                                      asondate = c.SPCCSHW_AsOnDate.Date.ToString("dd/MM/yyyy"),
                                                      SPCCSHW_Height = c.SPCCSHW_Height,
                                                      SPCCSHW_Weight = c.SPCCSHW_Weight,
                                                      SPCCSHW_BMI = c.SPCCSHW_BMI,
                                                      SPCCSHW_BMIRemark = c.SPCCSHW_BMIRemark,
                                                      SPCCMHW_ActiveFlag = c.SPCCMHW_ActiveFlag,
                                                      ASMCL_ClassName = b.ASMCL_ClassName,
                                                      ASMC_SectionName = d.ASMC_SectionName,
                                                      AMST_AdmNo = s.AMST_AdmNo,
                                                      studentName = ((s.AMST_FirstName == null ? "" : s.AMST_FirstName) +
                                                      (s.AMST_MiddleName == null || s.AMST_MiddleName == "" ? "" : " " + s.AMST_MiddleName) +
                                                      (s.AMST_LastName == null || s.AMST_LastName == "" ? "" : " " + s.AMST_LastName)).Trim(),
                                                      ASMAY_Year = yr.ASMAY_Year,
                                                      CreatedDate = a.CreatedDate
                                                  }).Distinct().OrderByDescending(t => t.CreatedDate).ToArray();
                        }
                    }
                }
                else
                {
                    data.studentrecord = (from a in _context.BMICalculationDMO
                                          from b in _context.admissionyearstudent
                                          from c in _context.admissionClass
                                          from d in _context.masterSection
                                          from s in _context.Adm_M_Student
                                          from yr in _context.AcademicYear
                                          where (a.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == b.ASMS_Id && a.AMST_Id == b.AMST_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && b.AMST_Id == s.AMST_Id && b.AMAY_ActiveFlag == 1 && s.AMST_SOL == "S" && a.MI_Id == s.MI_Id && b.ASMAY_Id == yr.ASMAY_Id && a.MI_Id == data.MI_Id)
                                          select new BMICalculationDTO
                                          {
                                              SPCCSHW_Id = a.SPCCSHW_Id,
                                              ASMAY_Id = a.ASMAY_Id,
                                              ASMCL_Id = a.ASMCL_Id,
                                              ASMS_Id = a.ASMS_Id,
                                              AMST_Id = a.AMST_Id,
                                              SPCCSHW_AsOnDate = a.SPCCSHW_AsOnDate,
                                              asondate = a.SPCCSHW_AsOnDate.Date.ToString("dd/MM/yyyy"),
                                              SPCCSHW_Height = a.SPCCSHW_Height,
                                              SPCCSHW_Weight = a.SPCCSHW_Weight,
                                              SPCCSHW_BMI = a.SPCCSHW_BMI,
                                              SPCCSHW_BMIRemark = a.SPCCSHW_BMIRemark,
                                              SPCCMHW_ActiveFlag = a.SPCCMHW_ActiveFlag,
                                              ASMCL_ClassName = c.ASMCL_ClassName,
                                              ASMC_SectionName = d.ASMC_SectionName,
                                              AMST_AdmNo = s.AMST_AdmNo,
                                              studentName = ((s.AMST_FirstName == null ? "" : s.AMST_FirstName) +
                                                      (s.AMST_MiddleName == null || s.AMST_MiddleName == "" ? "" : " " + s.AMST_MiddleName) +
                                                      (s.AMST_LastName == null || s.AMST_LastName == "" ? "" : " " + s.AMST_LastName)).Trim(),
                                              ASMAY_Year = yr.ASMAY_Year,
                                              CreatedDate = a.CreatedDate
                                          }).Distinct().OrderByDescending(t => t.CreatedDate).ToArray();
                }

                if (data.stringmobileorportal == "Mobile")
                {
                    List<IVRM_User_MobileApp_Login_Privileges> Staffmobileappprivileges = new List<IVRM_User_MobileApp_Login_Privileges>();
                    Staffmobileappprivileges = _db.IVRM_User_MobileApp_Login_Privileges.Where(t => t.IVRMUL_Id == data.UserId && t.MI_Id == data.MI_Id).ToList();

                    if (Staffmobileappprivileges.Count() > 0)
                    {
                        data.Staffmobileappprivileges = (from Mobilepage in _db.IVRM_MobileApp_Page
                                                         from MobileRolePrivileges in _db.IVRM_Role_MobileApp_Privileges
                                                         from UserRolePrivileges in _db.IVRM_User_MobileApp_Login_Privileges
                                                         where (MobileRolePrivileges.MI_ID == UserRolePrivileges.MI_Id
                                                         && Mobilepage.IVRMMAP_Id == MobileRolePrivileges.IVRMMAP_Id
                                                         && Mobilepage.IVRMMAP_Id == UserRolePrivileges.IVRMMAP_Id
                                                         && MobileRolePrivileges.IVRMRT_Id == data.roleid
                                                         && MobileRolePrivileges.MI_ID == data.MI_Id && UserRolePrivileges.IVRMUL_Id == data.UserId)
                                                         select new BMICalculationDTO
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public BMICalculationDTO getStudents(BMICalculationDTO data)
        {
            try
            {
                data.studentList = (from m in _db.School_Adm_Y_StudentDMO
                                    from n in _db.Adm_M_Student
                                    where m.AMST_Id == n.AMST_Id && m.AMAY_ActiveFlag == 1 && n.AMST_ActiveFlag == 1 && m.ASMAY_Id == data.ASMAY_Id && n.MI_Id == data.MI_Id && n.AMST_SOL.Equals("S") && m.ASMCL_Id == data.ASMCL_Id && m.ASMS_Id == data.ASMS_Id
                                    select new BMICalculationDTO
                                    {
                                        AMST_Id = m.AMST_Id,
                                        studentName = n.AMST_FirstName + (string.IsNullOrEmpty(n.AMST_MiddleName) ? "" : ' ' + n.AMST_MiddleName) + (string.IsNullOrEmpty(n.AMST_LastName) ? "" : ' ' + n.AMST_LastName),
                                        AMST_AdmNo = n.AMST_AdmNo,
                                    }).Distinct().OrderBy(n => n.studentName).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public BMICalculationDTO saveRecord(BMICalculationDTO obj)
        {
            try
            {
                if (obj.SPCCSHW_Id == 0)
                {
                    for (int i = 0; i < obj.student.Count(); i++)
                    {
                        var checkduplicate = _context.BMICalculationDMO.Where(d => d.MI_Id == obj.MI_Id && d.ASMAY_Id == obj.ASMAY_Id && d.ASMCL_Id == obj.ASMCL_Id && d.ASMS_Id == obj.ASMS_Id && d.AMST_Id == obj.student[i].AMST_Id && d.SPCCSHW_AsOnDate == obj.SPCCSHW_AsOnDate).ToList();

                        if (checkduplicate.Count > 0)
                        {
                            //obj.returnVal = "duplicate";
                            obj.SPCCSHW_Id = checkduplicate.SingleOrDefault().SPCCSHW_Id;
                            var mapp = _context.BMICalculationDMO.Single(t => t.SPCCSHW_Id == obj.SPCCSHW_Id);
                            // mapp.SPCCSHW_Id = obj.SPCCSHW_Id;
                            mapp.MI_Id = obj.MI_Id;
                            mapp.SPCCSHW_AsOnDate = obj.SPCCSHW_AsOnDate;
                            mapp.ASMAY_Id = obj.ASMAY_Id;
                            mapp.ASMCL_Id = obj.ASMCL_Id;
                            mapp.ASMS_Id = obj.ASMS_Id;
                            mapp.AMST_Id = obj.student[i].AMST_Id;
                            mapp.SPCCSHW_Height = obj.student[i].SPCCSHW_Height;
                            mapp.SPCCSHW_Weight = obj.student[i].SPCCSHW_Weight;
                            mapp.SPCCSHW_BMI = obj.student[i].SPCCSHW_BMI;
                            mapp.SPCCSHW_BMIRemark = obj.student[i].SPCCSHW_BMIRemark;

                            mapp.UpdatedDate = DateTime.Now;
                            _context.Update(mapp);
                            int s = _context.SaveChanges();
                            if (s > 0)
                            {
                                obj.returnVal = "update";
                            }
                            else
                            {
                                obj.returnVal = "update Failed";
                            }
                        }
                        else
                        {

                            BMICalculationDMO mapp = new BMICalculationDMO();
                            // mapp.SPCCSHW_Id = obj.SPCCSHW_Id;
                            mapp.MI_Id = obj.MI_Id;
                            mapp.SPCCSHW_AsOnDate = obj.SPCCSHW_AsOnDate;
                            mapp.ASMAY_Id = obj.ASMAY_Id;
                            mapp.ASMCL_Id = obj.ASMCL_Id;
                            mapp.ASMS_Id = obj.ASMS_Id;
                            mapp.AMST_Id = obj.student[i].AMST_Id;
                            mapp.SPCCSHW_Height = obj.student[i].SPCCSHW_Height;
                            mapp.SPCCSHW_Weight = obj.student[i].SPCCSHW_Weight;
                            mapp.SPCCSHW_BMI = obj.student[i].SPCCSHW_BMI;
                            mapp.SPCCSHW_BMIRemark = obj.student[i].SPCCSHW_BMIRemark;
                            mapp.SPCCMHW_ActiveFlag = true;
                            mapp.CreatedDate = DateTime.Now;
                            mapp.UpdatedDate = DateTime.Now;
                            _context.Add(mapp);
                            int s = _context.SaveChanges();
                            if (s > 0)
                            {
                                obj.returnVal = "saved";
                            }
                            else
                            {
                                obj.returnVal = "savingFailed";
                            }
                        }
                    }
                }
                else if (obj.SPCCSHW_Id > 0)
                {
                    for (int i = 0; i < obj.student.Count(); i++)
                    {
                        //var checkduplicate = _context.BMICalculationDMO.Where(d => d.MI_Id == obj.MI_Id && d.ASMAY_Id == obj.ASMAY_Id && d.SPCCSHW_Id != obj.SPCCSHW_Id && d.ASMCL_Id == obj.ASMCL_Id && d.ASMS_Id == obj.ASMS_Id && d.AMST_Id == obj.student[i].AMST_Id).ToList();

                        //if (checkduplicate.Count > 0)
                        //{
                        //    obj.returnVal = "duplicate";
                        //}
                        //else
                        //{
                        var mapp = _context.BMICalculationDMO.Single(t => t.SPCCSHW_Id == obj.SPCCSHW_Id);
                        // mapp.SPCCSHW_Id = obj.SPCCSHW_Id;
                        mapp.MI_Id = obj.MI_Id;
                        mapp.SPCCSHW_AsOnDate = obj.SPCCSHW_AsOnDate;
                        mapp.ASMAY_Id = obj.ASMAY_Id;
                        mapp.ASMCL_Id = obj.ASMCL_Id;
                        mapp.ASMS_Id = obj.ASMS_Id;
                        mapp.AMST_Id = obj.student[i].AMST_Id;
                        mapp.SPCCSHW_Height = obj.student[i].SPCCSHW_Height;
                        mapp.SPCCSHW_Weight = obj.student[i].SPCCSHW_Weight;
                        mapp.SPCCSHW_BMI = obj.student[i].SPCCSHW_BMI;
                        mapp.SPCCSHW_BMIRemark = obj.student[i].SPCCSHW_BMIRemark;

                        mapp.UpdatedDate = DateTime.Now;
                        _context.Update(mapp);
                        int s = _context.SaveChanges();
                        if (s > 0)
                        {
                            obj.returnVal = "update";
                        }
                        else
                        {
                            obj.returnVal = "update Failed";
                        }
                        //  }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return obj;
        }
        public BMICalculationDTO deactivate(BMICalculationDTO dto)
        {
            try
            {


                var result = _context.BMICalculationDMO.Single(t => t.SPCCSHW_Id == dto.SPCCSHW_Id);
                if (result.SPCCMHW_ActiveFlag == true)
                {
                    result.SPCCMHW_ActiveFlag = false;
                }
                else
                {
                    result.SPCCMHW_ActiveFlag = true;
                }
                result.CreatedDate = result.CreatedDate;
                result.UpdatedDate = DateTime.Now;
                _context.Update(result);
                var flag = _context.SaveChanges();
                if (flag > 0)
                {
                    dto.returnVal2 = true;
                }
                else
                {
                    dto.returnVal2 = false;
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public BMICalculationDTO editdata(BMICalculationDTO data)
        {
            try
            {
                data.editlist = (from a in _context.BMICalculationDMO
                                 from b in _context.Adm_M_Student
                                 where (a.AMST_Id == b.AMST_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.SPCCSHW_Id == data.SPCCSHW_Id)
                                 select new BMICalculationDTO
                                 {
                                     SPCCSHW_Id = a.SPCCSHW_Id,
                                     ASMAY_Id = a.ASMAY_Id,
                                     ASMCL_Id = a.ASMCL_Id,
                                     ASMS_Id = a.ASMS_Id,
                                     AMST_Id = a.AMST_Id,
                                     SPCCSHW_AsOnDate = a.SPCCSHW_AsOnDate,
                                     SPCCSHW_Height = a.SPCCSHW_Height,
                                     SPCCSHW_Weight = a.SPCCSHW_Weight,
                                     SPCCSHW_BMI = a.SPCCSHW_BMI,
                                     SPCCSHW_BMIRemark = a.SPCCSHW_BMIRemark,
                                     SPCCMHW_ActiveFlag = a.SPCCMHW_ActiveFlag,
                                     studentName = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) ? "" : ' ' + b.AMST_LastName),
                                     AMST_AdmNo = b.AMST_AdmNo,
                                 }).ToArray();


                data.studentList = (from m in _db.School_Adm_Y_StudentDMO
                                    from n in _db.Adm_M_Student
                                    where m.AMST_Id == n.AMST_Id && m.AMAY_ActiveFlag == 1 && n.AMST_ActiveFlag == 1 && m.ASMAY_Id == data.ASMAY_Id && n.MI_Id == data.MI_Id && n.AMST_SOL.Equals("S") && m.ASMCL_Id == data.ASMCL_Id && m.ASMS_Id == data.ASMS_Id
                                    select new BMICalculationDTO
                                    {
                                        AMST_Id = m.AMST_Id,
                                        studentName = n.AMST_FirstName + (string.IsNullOrEmpty(n.AMST_MiddleName) ? "" : ' ' + n.AMST_MiddleName) + (string.IsNullOrEmpty(n.AMST_LastName) ? "" : ' ' + n.AMST_LastName),
                                        AMST_AdmNo = n.AMST_AdmNo,
                                    }).Distinct().OrderBy(n => n.studentName).ToArray();

                var count = _context.Staff_User_Login.Where(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).ToList();
                if (count.Count > 0)
                {
                    data.HRME_Id = _context.Staff_User_Login.Where(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).FirstOrDefault().Emp_Code;
                    if (data.HRME_Id > 0)
                    {
                        var check_classteacher = _context.ClassTeacherMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                        && a.IMCT_ActiveFlag == true && a.HRME_Id == data.HRME_Id).ToList();

                        if (check_classteacher.Count() > 0)
                        {
                            data.classlist = (from a in _context.ClassTeacherMappingDMO
                                              from b in _context.admissionClass
                                              from c in _context.AcademicYear
                                              where (a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id
                                              && a.HRME_Id == data.HRME_Id && a.IMCT_ActiveFlag == true && b.ASMCL_ActiveFlag == true)
                                              select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                        }
                    }
                }
                else
                {
                    data.classlist = (from b in _context.admissionClass
                                      from y in _context.admissionyearstudent
                                      where (b.MI_Id == data.MI_Id && y.ASMCL_Id == b.ASMCL_Id && y.ASMAY_Id == data.ASMAY_Id
                                      && b.ASMCL_ActiveFlag == true && y.AMAY_ActiveFlag == 1)
                                      select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                }

                var classid = _context.Masterclasscategory.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id).Select(t => t.ASMCC_Id).ToArray();

                var secid = _context.AdmSchoolMasterClassCatSec.Where(t => classid.Contains(t.ASMCC_Id)).Select(t => t.ASMS_Id).ToArray();
                var sectionexamid = (from e in _context.Staff_User_Login
                                     from f in _context.Exm_Login_PrivilegeDMO
                                     from i in _context.Exm_Login_Privilege_SubjectsDMO
                                     where (/*e.Id == data. &&*/
                                       f.Login_Id == e.IVRMSTAUL_Id && f.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id && i.ASMCL_Id == data.ASMCL_Id && secid.Contains(i.ASMS_Id)
                                       && f.ELP_Id == i.ELP_Id && f.ELP_ActiveFlg == true && i.ELPs_ActiveFlg == true)
                                     select new BMICalculationDTO
                                     {
                                         ASMS_Id = i.ASMS_Id
                                     }).Distinct().Select(t => t.ASMS_Id).ToList();


                data.sectionList = _context.masterSection.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1 && sectionexamid.Contains(t.ASMS_Id)).ToArray();


            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public BMICalculationDTO get_classes(BMICalculationDTO data)
        {
            try
            {
                var count = _context.Staff_User_Login.Where(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).ToList();
                if (count.Count > 0)
                {
                    data.HRME_Id = _context.Staff_User_Login.Where(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).FirstOrDefault().Emp_Code;
                    if (data.HRME_Id > 0)
                    {
                        var check_classteacher = _context.ClassTeacherMappingDMO.Where(a => a.MI_Id == data.MI_Id
                        && a.ASMAY_Id == data.ASMAY_Id && a.IMCT_ActiveFlag == true
                        && a.HRME_Id == data.HRME_Id).ToList();

                        if (check_classteacher.Count() > 0)
                        {
                            data.classlist = (from a in _context.ClassTeacherMappingDMO
                                              from b in _context.admissionClass
                                              where (a.ASMCL_Id == b.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                              && a.IMCT_ActiveFlag == true && a.HRME_Id == data.HRME_Id)
                                              select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                        }
                    }
                }
                else
                {
                    data.classlist = (from a in _context.admissionClass
                                      from b in _context.Masterclasscategory
                                      from c in _context.AcademicYear
                                      where (a.ASMCL_Id == b.ASMCL_Id && b.ASMAY_Id == c.ASMAY_Id && a.ASMCL_ActiveFlag == true && b.Is_Active == true
                                      && c.Is_Active == true && b.ASMAY_Id == data.ASMAY_Id)
                                      select a).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public BMICalculationDTO get_section(BMICalculationDTO data)
        {
            try
            {
                var count = _context.Staff_User_Login.Where(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).ToList();
                if (count.Count > 0)
                {
                    data.HRME_Id = _context.Staff_User_Login.Where(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).FirstOrDefault().Emp_Code;
                    if (data.HRME_Id > 0)
                    {
                        var check_classteacher = _context.ClassTeacherMappingDMO.Where(a => a.MI_Id == data.MI_Id
                        && a.ASMAY_Id == data.ASMAY_Id && a.IMCT_ActiveFlag == true && a.HRME_Id == data.HRME_Id).ToList();

                        if (check_classteacher.Count() > 0)
                        {
                            data.sectionList = (from a in _context.ClassTeacherMappingDMO
                                                from b in _context.admissionClass
                                                from c in _context.masterSection
                                                where (a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == c.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                                && a.ASMCL_Id == data.ASMCL_Id && a.IMCT_ActiveFlag == true && a.HRME_Id == data.HRME_Id
                                                && a.ASMCL_Id == data.ASMCL_Id)
                                                select c).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
                        }
                    }
                }
                else
                {
                    data.sectionList = (from a in _context.AdmSchoolMasterClassCatSec
                                        from b in _context.Masterclasscategory
                                        from c in _context.admissionClass
                                        from d in _context.masterSection
                                        where (a.ASMCC_Id == b.ASMCC_Id && b.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == d.ASMS_Id && a.ASMCCS_ActiveFlg == true
                                        && b.Is_Active == true && c.ASMCL_ActiveFlag == true && d.ASMC_ActiveFlag == 1 && b.MI_Id == data.MI_Id
                                        && b.ASMCL_Id == data.ASMCL_Id && b.ASMAY_Id == data.ASMAY_Id)
                                        select d).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public BMICalculationDTO filterStudeDateWise(BMICalculationDTO data)
        {
            try
            {
                var edit = (from a in _context.BMICalculationDMO
                            where (a.ASMS_Id == data.ASMS_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.SPCCSHW_AsOnDate == data.SPCCSHW_AsOnDate)
                            select new BMICalculationDTO
                            {
                                AMST_Id = a.AMST_Id,
                                ASMAY_Id = a.ASMAY_Id,
                                ASMCL_Id = a.ASMCL_Id,
                                ASMS_Id = a.ASMS_Id,
                                SPCCSHW_Height = a.SPCCSHW_Height,
                                SPCCSHW_Weight = a.SPCCSHW_Weight,
                                SPCCSHW_BMIRemark = a.SPCCSHW_BMIRemark,
                                SPCCSHW_BMI = a.SPCCSHW_BMI,
                            }).Distinct().ToList();
                if (edit.Count > 0)
                {
                    data.editchecklist = edit.Distinct().OrderByDescending(t => t.SPCCSHW_AsOnDate).ToArray();
                }
                data.studentList = (from m in _db.School_Adm_Y_StudentDMO
                                    from n in _db.Adm_M_Student
                                    where m.AMST_Id == n.AMST_Id && m.AMAY_ActiveFlag == 1 && n.AMST_ActiveFlag == 1 && m.ASMAY_Id == data.ASMAY_Id && n.MI_Id == data.MI_Id && n.AMST_SOL.Equals("S") && m.ASMCL_Id == data.ASMCL_Id && m.ASMS_Id == data.ASMS_Id
                                    select new BMICalculationDTO
                                    {
                                        AMST_Id = m.AMST_Id,
                                        studentName = n.AMST_FirstName + (string.IsNullOrEmpty(n.AMST_MiddleName) ? "" : ' ' + n.AMST_MiddleName) + (string.IsNullOrEmpty(n.AMST_LastName) ? "" : ' ' + n.AMST_LastName),
                                        AMST_AdmNo = n.AMST_AdmNo,
                                    }).Distinct().OrderBy(n => n.studentName).ToArray();
            }
            catch (Exception msg)
            {
                Console.WriteLine(msg.Message);
            }
            return data;
        }
    }
}