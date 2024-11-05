using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class SmartCardFreezeImpl : Interfaces.SmartCardFreezeInterface
    {
        public ScheduleReportContext _SReportContext;
        public DomainModelMsSqlServerContext _SSReportContext;
        public SmartCardFreezeImpl(ScheduleReportContext DomainModelContext, DomainModelMsSqlServerContext DomainModelContext1)
        {
            _SReportContext = DomainModelContext; _SSReportContext = DomainModelContext1;
        }

        public SmartCardFreezeDTO getdetails(SmartCardFreezeDTO data)
        {
            try
            {
                var list = _SReportContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).ToList();//AcademicYear
                data.yearlist = list.ToArray();

                var currYear = _SReportContext.AcademicYear.Where(d => d.MI_Id == data.MI_Id).ToList();//AcademicYear
                data.currentYear = currYear.ToArray();

                var classlist = _SReportContext.admissioncls.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true ).ToList();
                data.classlist = classlist.ToArray();

                var sectionlist = _SReportContext.school_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).ToList();
                data.sectionlist = sectionlist.ToArray();
                if (data.ASMAY_Id==0)
                {
                    data.ASMAY_Id = list.FirstOrDefault().ASMAY_Id;
                }


                if (data.ASMCL_Id == 0)
                {
                    data.ASMCL_Id = classlist.FirstOrDefault().ASMCL_Id;
                }
                if (data.ASMS_Id == 0)
                {
                    data.ASMS_Id = sectionlist.FirstOrDefault().ASMS_Id;
                }

                var studentlist = (from m in _SReportContext.AdmissionStudentDMO
                                   from n in _SReportContext.School_Adm_Y_StudentDMO
                                   where m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && m.AMST_SOL.Equals("S") && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1 && n.ASMCL_Id == data.ASMCL_Id && n.ASMS_Id == data.ASMS_Id
                                   select new SmartCardFreezeDTO
                                   {
                                       AMST_Id = m.AMST_Id,
                                       MI_Id = m.MI_Id,
                                       ASMAY_Id = m.ASMAY_Id,
                                       AMST_FirstName = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + " " + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + " " + (m.AMST_LastName == null ? " " : m.AMST_LastName)).Trim(),
                                       AMST_MiddleName = m.AMST_MiddleName,
                                       AMST_LastName = m.AMST_LastName,
                                       AMST_AdmNo = m.AMST_AdmNo

                                   }).Distinct().ToList();

                if (studentlist.Count > 0)
                {
                    data.studentlist = studentlist.OrderBy(t => t.AMST_FirstName).ToArray();
                    data.studentCount = studentlist.Count;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SmartCardFreezeDTO admsearch(SmartCardFreezeDTO data)
        {
            try
            {
                
                var studentlist = (from m in _SReportContext.AdmissionStudentDMO
                                   from n in _SReportContext.School_Adm_Y_StudentDMO
                                   where m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_Id  && m.AMST_SOL.Equals("S") && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1 && m.AMST_AdmNo == data.AMST_AdmNo
                                   select new SmartCardFreezeDTO
                                   {
                                       AMST_Id = m.AMST_Id,
                                       MI_Id = m.MI_Id,
                                       ASMAY_Id = m.ASMAY_Id,
                                       AMST_FirstName = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + " " + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + " " + (m.AMST_LastName == null ? " " : m.AMST_LastName)).Trim(),
                                       AMST_MiddleName = m.AMST_MiddleName,
                                       AMST_LastName = m.AMST_LastName,
                                       AMST_AdmNo = m.AMST_AdmNo

                                   }).Distinct().ToList();

                if (studentlist.Count > 0)
                {
                    data.studentlist = studentlist.OrderBy(t => t.AMST_FirstName).ToArray();
                    data.studentCount = studentlist.Count;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SmartCardFreezeDTO getstddetails(SmartCardFreezeDTO data)
        {
            try
            {
               

                data.studentdetails = (from m in _SReportContext.AdmissionStudentDMO
                                   from n in _SReportContext.School_Adm_Y_StudentDMO
                                   from o in _SReportContext.admissioncls
                                   from p in _SReportContext.school_M_Section
                                   where m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_Id  &&  n.AMST_Id == data.AMST_Id && n.ASMS_Id==p.ASMS_Id && n.ASMCL_Id==o.ASMCL_Id
                                   select new SmartCardFreezeDTO
                                   {
                                       AMST_Id = m.AMST_Id,
                                       MI_Id = m.MI_Id,
                                       ASMAY_Id = n.ASMAY_Id,
                                       AMST_FirstName = m.AMST_AdmNo+":"+((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + " " + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + " " + (m.AMST_LastName == null ? " " : m.AMST_LastName)).Trim(),
                                       AMST_MiddleName = m.AMST_MiddleName,
                                       AMST_LastName = m.AMST_LastName,
                                       AMST_AdmNo = m.AMST_AdmNo,
                                       ASMCL_Id=n.ASMCL_Id,
                                       ASMS_Id=n.ASMS_Id,
                                       ASMCL_ClassName=o.ASMCL_ClassName,
                                       ASMC_SectionName=p.ASMC_SectionName,
                                       AMST_Photoname=m.AMST_Photoname


                                   }).Distinct().ToArray();

               

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public SmartCardFreezeDTO getdetailsstf(SmartCardFreezeDTO data)
        {
            try
            {

                data.SCFLAGLIST = (from a in  _SReportContext.institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1)
                                   select new SmartCardFreezeDTO
                                   {
                                       MI_SchoolCollegeFlag=a.MI_SchoolCollegeFlag 

                                   }).Distinct().OrderBy(t => t.HRME_Id).ToArray();

                data.filldepartment = _SReportContext.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id && a.HRMD_ActiveFlag == true).Distinct().OrderBy(e => e.HRMD_Order).ToArray();
                data.filldesignation = _SReportContext.HR_Master_Designation.Where(a => a.MI_Id == data.MI_Id && a.HRMDES_ActiveFlag == true).Distinct().OrderBy(e => e.HRMDES_Order).ToArray();

                data.stafftlist = (from a in _SReportContext.MasterEmployee.Where(a => a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true)
                                   select new SmartCardFreezeDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       MI_Id = a.MI_Id,
                                       HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                       HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                       HRME_EmployeeLastName = a.HRME_EmployeeLastName
                                   }).Distinct().OrderBy(t => t.HRME_Id).ToArray();


            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }

        public  SmartCardFreezeDTO getstfdetails(SmartCardFreezeDTO data)
        {
            try
            {
                data.selctstaffdata = (from a in _SReportContext.MasterEmployee
                                       from b in _SReportContext.HR_Master_Department
                                       from c in _SReportContext.HR_Master_Designation
                                       where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == b.MI_Id && c.HRMDES_Id == a.HRMDES_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.HRME_Id == data.HRME_Id && a.HRME_ActiveFlag == true && b.HRMD_ActiveFlag == true && c.HRMDES_ActiveFlag == true)
                                       select new SmartCardFreezeDTO
                                       {
                                           HRME_Id = a.HRME_Id,
                                           MI_Id = a.MI_Id,
                                           HRME_EmployeeCode = a.HRME_EmployeeCode,
                                           HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                           HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                           HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                           HRME_MobileNo = a.HRME_MobileNo,
                                           HRMD_DepartmentName = b.HRMD_DepartmentName,
                                           HRMD_Id = b.HRMD_Id,
                                           HRMDES_DesignationName = c.HRMDES_DesignationName,
                                           HRMDES_Id = c.HRMDES_Id,
                                           HRME_Photo = a.HRME_Photo,

                                       }).Distinct().ToArray();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }
        public SmartCardFreezeDTO getdetailsstfdes(SmartCardFreezeDTO data)
        {
            try
            {
                //data.filldepartment = _SReportContext.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id && a.HRMD_ActiveFlag == true).Distinct().OrderBy(e => e.HRMD_Order).ToArray();
                //data.filldesignation = _SReportContext.HR_Master_Designation.Where(a => a.MI_Id == data.MI_Id && a.HRMDES_ActiveFlag == true).Distinct().OrderBy(e => e.HRMDES_Order).ToArray();

                data.stafftlist = (from a in _SReportContext.MasterEmployee.Where(a => a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRMD_Id==data.HRMD_Id && a.HRMDES_Id==data.HRMDES_Id)
                                   select new SmartCardFreezeDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       MI_Id = a.MI_Id,
                                       HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                       HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                       HRME_EmployeeLastName = a.HRME_EmployeeLastName
                                   }).Distinct().OrderBy(t => t.HRME_Id).ToArray();


            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }

        public SmartCardFreezeDTO depchange(SmartCardFreezeDTO data)
        {
            try
            {
                //data.filldepartment = _SReportContext.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id && a.HRMD_ActiveFlag == true).Distinct().OrderBy(e => e.HRMD_Order).ToArray();
                //data.filldesignation = _SReportContext.HR_Master_Designation.Where(a => a.MI_Id == data.MI_Id && a.HRMDES_ActiveFlag == true).Distinct().OrderBy(e => e.HRMDES_Order).ToArray();

                //data.stafftlist = (from a in _SReportContext.MasterEmployee.Where(a => a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRMD_Id==data.HRMD_Id && a.HRMDES_Id==data.HRMDES_Id)
                //                   select new SmartCardFreezeDTO
                //                   {
                //                       HRME_Id = a.HRME_Id,
                //                       MI_Id = a.MI_Id,
                //                       HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                //                       HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                //                       HRME_EmployeeLastName = a.HRME_EmployeeLastName
                //                   }).Distinct().OrderBy(t => t.HRME_Id).ToArray();
                data.filldesignation = (from a in _SReportContext.MasterEmployee
                                        from b in _SReportContext.HR_Master_Designation
                                        where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMDES_Id == b.HRMDES_Id && data.HRMD_Id == a.HRMD_Id && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                                        select b).Distinct().ToArray();


            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }

        public SmartCardFreezeDTO getdetailsCLG(SmartCardFreezeDTO data)
        {
            try
            {


                var list = _SReportContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id
                        // && t.Is_Active == true && t.ASMAY_ActiveFlag == 1 
                        && t.Is_Active==true).OrderByDescending(t => t.ASMAY_Order).ToList();//AcademicYear
                data.yearlist = list.ToArray();

                var currYear = _SReportContext.AcademicYear.Where(d => d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id).ToList();//AcademicYear
                data.currentYear = currYear.ToArray();
                if (data.ASMAY_Id == 0)
                {
                    data.ASMAY_Id = list.FirstOrDefault().ASMAY_Id;
                }

                var courselist = (from a in _SReportContext.MasterCourseDMO
                                  from b in _SReportContext.CLG_Adm_College_AY_CourseDMO
                                  where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                                  select a).Distinct().OrderBy(t => t.AMCO_Order).ToList();
                data.courselist = courselist.ToArray();
                if (data.AMCO_Id == 0)
                {

                    if (courselist.Count>0)
                    {
                        data.AMCO_Id = courselist.FirstOrDefault().AMCO_Id;
                    }
                    
                }

                var branchlist = (from a in _SReportContext.ClgMasterBranchDMO
                                  from b in _SReportContext.CLG_Adm_College_AY_CourseDMO
                                  from c in _SReportContext.CLG_Adm_College_AY_Course_BranchDMO
                                  where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag)
                                  select new SmartCardFreezeDTO
                                  {
                                      AMB_Id = a.AMB_Id,
                                      AMB_BranchName = a.AMB_BranchName,
                                      AMB_BranchCode = a.AMB_BranchCode,
                                      AMB_BranchInfo = a.AMB_BranchInfo,
                                      AMB_BranchType = a.AMB_BranchType,
                                      AMB_Order = a.AMB_Order,
                                  }).Distinct().ToList();
                data.branchlist = branchlist.OrderBy(t => t.AMB_Order).ToArray();

                if (data.AMB_Id == 0)
                {
                    if (branchlist.Count>0)
                    {
                        data.AMB_Id = branchlist.FirstOrDefault().AMB_Id;
                    }
                    
                }

                var semisterlist = (from a in _SReportContext.CLG_Adm_Master_SemesterDMO
                                    from b in _SReportContext.CLG_Adm_College_AY_CourseDMO
                                    from c in _SReportContext.CLG_Adm_College_AY_Course_BranchDMO
                                    from d in _SReportContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
                                    select new SmartCardFreezeDTO
                                    {
                                        AMSE_Id = a.AMSE_Id,
                                        AMSE_SEMName = a.AMSE_SEMName,
                                        AMSE_SEMInfo = a.AMSE_SEMInfo,
                                        AMSE_SEMCode = a.AMSE_SEMCode,
                                        AMSE_SEMOrder = a.AMSE_SEMOrder,
                                      
                                    }).Distinct().ToList();
                data.semisterlist = semisterlist.OrderBy(t => t.AMSE_SEMOrder).ToArray();

                if (data.AMSE_Id == 0)
                {
                    if (semisterlist.Count>0)
                    {
                        data.AMSE_Id = semisterlist.FirstOrDefault().AMSE_Id;
                    }
                    
                }


                var sectionlist = (from a in _SReportContext.Adm_College_Yearly_StudentDMO
                                   from b in _SReportContext.Adm_College_Master_SectionDMO
                                   where a.ASMAY_Id == data.ASMAY_Id && b.ACMS_Id == a.ACMS_Id && b.MI_Id == data.MI_Id && a.AMB_Id == data.AMB_Id && a.AMCO_Id == data.AMCO_Id && a.AMSE_Id == data.AMSE_Id
                                   select new SmartCardFreezeDTO
                                   {
                                       ACMS_Id = b.ACMS_Id,
                                       ACMS_SectionName = b.ACMS_SectionName,
                                       ACMS_Order = b.ACMS_Order
                                   }).Distinct().ToList();

                data.sectionlist = sectionlist.OrderBy(t => t.ACMS_Order).ToArray();

                if (data.ACMS_Id == 0)
                {
                    if (sectionlist.Count>0)
                    {
                        data.ACMS_Id = sectionlist.FirstOrDefault().ACMS_Id;
                    }
                    
                }

                //if (data.ASMCL_Id==0)
                //{
                //    data.ASMCL_Id = classlist.FirstOrDefault().ASMCL_Id;
                //}
                //if (data.ASMS_Id == 0)
                //{
                //    data.ASMS_Id = sectionlist.FirstOrDefault().ASMS_Id;
                //}

                var studentlist = (from m in _SReportContext.Adm_Master_College_StudentDMO
                                   from n in _SReportContext.Adm_College_Yearly_StudentDMO
                                   where m.AMCST_Id == n.AMCST_Id && m.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && m.AMCST_SOL.Equals("S") && m.AMCST_ActiveFlag == true && n.ACYST_ActiveFlag == 1 && n.AMCO_Id == data.AMCO_Id && n.AMB_Id == data.AMB_Id && n.AMSE_Id == data.AMSE_Id && n.ACMS_Id == data.ACMS_Id
                                   select new SmartCardFreezeDTO
                                   {
                                       AMCST_Id = m.AMCST_Id,
                                       MI_Id = m.MI_Id,
                                       ASMAY_Id = m.ASMAY_Id,
                                       AMCST_FirstName = ((m.AMCST_FirstName == null ? " " : m.AMCST_FirstName) + " " + (m.AMCST_MiddleName == null ? " " : m.AMCST_MiddleName) + " " + (m.AMCST_LastName == null ? " " : m.AMCST_LastName)).Trim(),
                                       AMCST_MiddleName = m.AMCST_MiddleName,
                                       AMCST_LastName = m.AMCST_LastName,
                                       AMCST_AdmNo = m.AMCST_AdmNo

                                   }).Distinct().ToList();

                if (studentlist.Count > 0)
                {
                    data.studentlist = studentlist.OrderBy(t => t.AMCST_FirstName).ToArray();
                    data.studentCount = studentlist.Count;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;

        }
        public SmartCardFreezeDTO admsearchclg(SmartCardFreezeDTO data)
        {
            try
            {

                var studentlist = (from m in _SReportContext.Adm_Master_College_StudentDMO
                                   from n in _SReportContext.Adm_College_Yearly_StudentDMO
                                   where m.AMCST_Id == n.AMCST_Id && m.MI_Id == data.MI_Id  && m.AMCST_SOL.Equals("S") && m.AMCST_ActiveFlag == true && n.ACYST_ActiveFlag == 1  && m.AMCST_AdmNo == data.AMST_AdmNo
                                   select new SmartCardFreezeDTO
                                   {
                                       AMCST_Id = m.AMCST_Id,
                                       MI_Id = m.MI_Id,
                                       ASMAY_Id = m.ASMAY_Id,
                                       AMCST_FirstName = ((m.AMCST_FirstName == null ? " " : m.AMCST_FirstName) + " " + (m.AMCST_MiddleName == null ? " " : m.AMCST_MiddleName) + " " + (m.AMCST_LastName == null ? " " : m.AMCST_LastName)).Trim(),
                                       AMCST_MiddleName = m.AMCST_MiddleName,
                                       AMCST_LastName = m.AMCST_LastName,
                                       AMCST_AdmNo = m.AMCST_AdmNo

                                   }).Distinct().ToList();

                if (studentlist.Count > 0)
                {
                    data.studentlist = studentlist.OrderBy(t => t.AMCST_FirstName).ToArray();
                    data.studentCount = studentlist.Count;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;

        }

        public SmartCardFreezeDTO getstddetailscld(SmartCardFreezeDTO data)
        {
            try
            {
                data.studentdetails = (from a in _SReportContext.Adm_Master_College_StudentDMO
                                       from b in _SReportContext.Adm_College_Yearly_StudentDMO
                                       from c in _SReportContext.MasterCourseDMO
                                       from d in _SReportContext.ClgMasterBranchDMO
                                       from e in _SReportContext.CLG_Adm_Master_SemesterDMO
                                       from f in _SReportContext.Adm_College_Master_SectionDMO
                                       where a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && b.AMSE_Id == e.AMSE_Id && b.ACMS_Id == f.ACMS_Id && a.AMCST_Id==data.AMCST_Id
                                       select new SmartCardFreezeDTO
                                       {
                                           AMCST_Id = a.AMCST_Id,
                                           MI_Id = a.MI_Id,
                                           AMCST_FirstName = ((a.AMCST_FirstName == null ? " " : a.AMCST_FirstName) + " " + (a.AMCST_MiddleName == null ? " " : a.AMCST_MiddleName) + " " + (a.AMCST_LastName == null ? " " : a.AMCST_LastName)).Trim(),
                                           AMCST_MiddleName = a.AMCST_MiddleName,
                                           AMCST_LastName = a.AMCST_LastName,
                                           AMCST_AdmNo = a.AMCST_AdmNo,
                                           AMST_Photoname=a.AMCST_StudentPhoto,
                                           AMCO_CourseName=c.AMCO_CourseName,
                                           AMB_BranchName=d.AMB_BranchName,
                                           AMSE_SEMName=e.AMSE_SEMName,
                                           ASMC_SectionName=f.ACMS_SectionName

                                       }).Distinct().ToArray();


            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;

        }
        
    }
}
