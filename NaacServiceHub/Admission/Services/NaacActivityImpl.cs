using DataAccessMsSqlServerProvider.NAAC;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model.NAAC.Admission;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace NaacServiceHub.Admission.Services
{
    public class NaacActivityImpl : Interface.NaacActivityInterface
    {

        public GeneralContext _GeneralContext;

        public NaacActivityImpl(GeneralContext a)
        {
            _GeneralContext = a;
        }
        public NaacActivity_DTO loaddata(NaacActivity_DTO data)
        {
            try
            {
                var institutionlist = (from a in _GeneralContext.Institution
                                       from b in _GeneralContext.UserRoleWithInstituteDMO
                                       where (b.Id == data.UserId && b.MI_Id == a.MI_Id && b.Activeflag == 1 && a.MI_ActiveFlag == 1)
                                       select a).Distinct().OrderBy(t => t.MI_Name).ToList();
                data.institutionlist = institutionlist.ToArray();
                if (data.MI_Id == 0)
                {
                    if (institutionlist.Count > 0)
                    {
                        data.MI_Id = institutionlist.FirstOrDefault().MI_Id;
                    }
                }
                data.yearlist = _GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.filldepartment = _GeneralContext.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id && t.HRMD_ActiveFlag == true).Distinct().OrderBy(t => t.HRMD_DepartmentName).ToArray();


                data.alldata1 = (from a in _GeneralContext.Academic
                                 from b in _GeneralContext.NAAC_AC_343_StudentActivities_DMO
                                     //from c in _GeneralContext.NAAC_AC_343_SA_Students_DMO
                                     //from d in _GeneralContext.NAAC_AC_343_SA_Employee_DMO
                                 where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.Is_Active == true && a.ASMAY_Id == b.NCACSA343_Year)
                                 select new NaacActivity_DTO
                                 {
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCACSA343_Id = b.NCACSA343_Id,
                                     NCACSA343_TypeOfActivity = b.NCACSA343_TypeOfActivity,
                                     NCACSA343_OrgAgency = b.NCACSA343_OrgAgency,
                                     NCACSA343_Place = b.NCACSA343_Place,
                                     NCACSA343_Duration = b.NCACSA343_Duration,
                                     NCACSA343_NoOfStudents = b.NCACSA343_NoOfStudents,
                                     NCACSA343_NoOfTeachers = b.NCACSA343_NoOfTeachers,
                                     NCACSA343_ActiveFlg = b.NCACSA343_ActiveFlg,
                                     MI_Id = data.MI_Id

                                 }).Distinct().ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NaacActivity_DTO get_course(NaacActivity_DTO data)
        {
            try
            {
                data.courselist = (from a in _GeneralContext.MasterCourseDMO
                                   from b in _GeneralContext.CLG_Adm_College_AY_CourseDMO
                                   where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                                   select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NaacActivity_DTO get_branch(NaacActivity_DTO data)
        {
            try
            {
                data.branchlist = (from a in _GeneralContext.ClgMasterBranchDMO
                                   from b in _GeneralContext.CLG_Adm_College_AY_CourseDMO
                                   from c in _GeneralContext.CLG_Adm_College_AY_Course_BranchDMO
                                   where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag)
                                   select new NaacActivity_DTO
                                   {
                                       AMB_Id = a.AMB_Id,
                                       AMB_BranchName = a.AMB_BranchName,
                                       AMB_BranchCode = a.AMB_BranchCode,
                                       AMB_Order = a.AMB_Order,
                                   }).Distinct().OrderBy(t => t.AMB_Order).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NaacActivity_DTO get_sems(NaacActivity_DTO data)
        {
            try
            {
                data.semisterlist = (from a in _GeneralContext.CLG_Adm_Master_SemesterDMO
                                     from b in _GeneralContext.CLG_Adm_College_AY_CourseDMO
                                     from c in _GeneralContext.CLG_Adm_College_AY_Course_BranchDMO
                                     from d in _GeneralContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                     where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
                                     select new NaacActivity_DTO
                                     {
                                         AMSE_Id = a.AMSE_Id,
                                         AMSE_SEMName = a.AMSE_SEMName,
                                         AMSE_SEMCode = a.AMSE_SEMCode,
                                         AMSE_SEMOrder = a.AMSE_SEMOrder,
                                     }).Distinct().OrderBy(t => t.AMSE_SEMOrder).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NaacActivity_DTO get_section(NaacActivity_DTO data)
        {
            try
            {
                data.sectionlist = (from a in _GeneralContext.Adm_College_Yearly_StudentDMO
                                    from b in _GeneralContext.Adm_College_Master_SectionDMO
                                    where a.ASMAY_Id == data.ASMAY_Id && b.ACMS_Id == a.ACMS_Id && b.MI_Id == data.MI_Id && a.AMB_Id == data.AMB_Id && a.AMCO_Id == data.AMCO_Id && a.AMSE_Id == data.AMSE_Id
                                    select new NaacActivity_DTO
                                    {
                                        ACMS_Id = b.ACMS_Id,
                                        ACMS_SectionName = b.ACMS_SectionName,
                                        ACMS_Order = b.ACMS_Order
                                    }).Distinct().OrderBy(t => t.ACMS_Order).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NaacActivity_DTO GetStudentDetails(NaacActivity_DTO data)
        {
            try
            {
                data.studentlist = (from m in _GeneralContext.Adm_Master_College_StudentDMO
                                    from n in _GeneralContext.Adm_College_Yearly_StudentDMO
                                    where m.AMCST_Id == n.AMCST_Id && m.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && m.AMCST_SOL.Equals("S") && m.AMCST_ActiveFlag == true && n.ACYST_ActiveFlag == 1 && n.AMCO_Id == data.AMCO_Id && n.AMB_Id == data.AMB_Id && n.AMSE_Id == data.AMSE_Id && n.ACMS_Id == data.ACMS_Id
                                    select new NaacActivity_DTO
                                    {
                                        AMCST_Id = m.AMCST_Id,
                                        MI_Id = m.MI_Id,
                                        ASMAY_Id = m.ASMAY_Id,
                                        AMCST_FirstName = ((m.AMCST_FirstName == null ? " " : m.AMCST_FirstName) + " " + (m.AMCST_MiddleName == null ? " " : m.AMCST_MiddleName) + " " + (m.AMCST_LastName == null ? " " : m.AMCST_LastName)).Trim(),
                                        AMCST_MiddleName = m.AMCST_MiddleName,
                                        AMCST_LastName = m.AMCST_LastName,
                                        AMCST_AdmNo = m.AMCST_AdmNo

                                    }).Distinct().OrderBy(t => t.AMCST_FirstName).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NaacActivity_DTO get_Designation(NaacActivity_DTO data)
        {
            try
            {
                data.filldesignation = (from a in _GeneralContext.HR_Master_Employee_DMO
                                        from b in _GeneralContext.HR_Master_Designation
                                        where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == data.HRMD_Id && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                                        select b).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NaacActivity_DTO get_Employee(NaacActivity_DTO data)
        {
            try
            {
                data.stafftlist = (from a in _GeneralContext.HR_Master_Employee_DMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true
                                   && a.HRMDES_Id == data.HRMDES_Id && a.HRMD_Id == data.HRMD_Id)
                                   select new NaacActivity_DTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       MI_Id = a.MI_Id,
                                       empname = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),

                                   }).Distinct().OrderBy(t => t.empname).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NaacActivity_DTO saverecord(NaacActivity_DTO data)
        {
            try
            {
                if (data.NCACSA343_Id == 0)
                {
                    NAAC_AC_343_StudentActivities_DMO obj1 = new NAAC_AC_343_StudentActivities_DMO();

                    //obj1.NCACSA343_Id = data.NCACSA343_Id;
                    obj1.MI_Id = data.MI_Id;
                    obj1.NCACSA343_ActivityDate = data.NCACSA343_ActivityDate;
                    obj1.NCACSA343_NoOfTeachers = data.NCACSA343_NoOfTeachers;
                    obj1.NCACSA343_TypeOfActivity = data.NCACSA343_TypeOfActivity;
                    obj1.NCACSA343_OrgAgency = data.NCACSA343_OrgAgency;
                    obj1.NCACSA343_Place = data.NCACSA343_Place;
                    obj1.NCACSA343_Duration = data.NCACSA343_Duration;
                    obj1.NCACSA343_Year = data.ASMAY_Id;
                    obj1.NCACSA343_NoOfStudents = data.NCACSA343_NoOfStudents;
                    obj1.NCACSA343_ActiveFlg = true;
                    obj1.NCACSA343_CreatedBy = data.UserId;
                    obj1.NCACSA343_UpdatedBy = data.UserId;
                    obj1.NCACSA343_CreatedDate = DateTime.Now;
                    obj1.NCACSA343_UpdatedDate = DateTime.Now;
                    _GeneralContext.Add(obj1);

                    if (data.filelist.Length > 0)
                    {
                        for (int i = 0; i < data.filelist.Length; i++)
                        {
                            if (data.filelist[i].cfilepath != null)
                            {


                                NAAC_AC_343_StudentActivities_Files_DMO objfiles = new NAAC_AC_343_StudentActivities_Files_DMO();

                                objfiles.NCACSA343F_FileName = data.filelist[i].cfilename;
                                objfiles.NCACSA343F_Filedesc = data.filelist[i].cfiledesc;
                                objfiles.NCACSA343F_FilePath = data.filelist[i].cfilepath;
                                objfiles.NCACSA343_Id = obj1.NCACSA343_Id;

                                _GeneralContext.Add(objfiles);
                            }
                        }
                    }

                    for (int s = 0; s < data.selectdStudentlist.Length; s++)
                    {
                        NAAC_AC_343_SA_Students_DMO obj2 = new NAAC_AC_343_SA_Students_DMO();

                        obj2.NCACSA343_Id = obj1.NCACSA343_Id;
                        obj2.AMCST_Id = data.selectdStudentlist[s].AMCST_Id;
                        obj2.NCACSA343S_Role = data.NCACSA343S_Role;
                        obj2.NCACSA343S_ActiveFlg = true;
                        obj2.NCACSA343S_CreatedBy = data.UserId;
                        obj2.NCACSA343S_UpdatedBy = data.UserId;
                        obj2.NCACSA343S_CreatedDate = DateTime.Now;
                        obj2.NCACSA343S_UpdatedDate = DateTime.Now;

                        _GeneralContext.Add(obj2);

                        if (data.filelist_student.Length > 0)
                        {
                            for (int s1 = 0; s1 < data.filelist_student.Length; s1++)
                            {
                                if (data.filelist_student[s1].cfilepath != null)
                                {

                                    NAAC_AC_343_SA_Students_Files_DMO objfiles2 = new NAAC_AC_343_SA_Students_Files_DMO();

                                    objfiles2.NCACSA343SF_FileName = data.filelist_student[s1].cfilename;
                                    objfiles2.NCACSA343SF_Filedesc = data.filelist_student[s1].cfiledesc;
                                    objfiles2.NCACSA343SF_FilePath = data.filelist_student[s1].cfilepath;
                                    objfiles2.NCACSA343S_Id = obj2.NCACSA343S_Id;

                                    _GeneralContext.Add(objfiles2);
                                }
                            }
                        }
                    }

                    for (int e = 0; e < data.selectdStafflist.Length; e++)
                    {
                        NAAC_AC_343_SA_Employee_DMO obj3 = new NAAC_AC_343_SA_Employee_DMO();

                        //obj3.NCACSA343E_Id = data.NCACSA343E_Id;
                        obj3.NCACSA343_Id = obj1.NCACSA343_Id;
                        obj3.HRME_Id = data.selectdStafflist[e].HRME_Id;
                        obj3.NCACSA343E_Role = data.NCACSA343E_Role;
                        obj3.NCACSA343E_ActiveFlg = true;
                        obj3.NCACSA343E_CreatedBy = data.UserId;
                        obj3.NCACSA343E_UpdatedBy = data.UserId;
                        obj3.NCACSA343E_CreatedDate = DateTime.Now;
                        obj3.NCACSA343E_UpdatedDate = DateTime.Now;

                        _GeneralContext.Add(obj3);
                        if (data.filelist_staff.Length > 0)
                        {
                            for (int s1 = 0; s1 < data.filelist_staff.Length; s1++)
                            {
                                if (data.filelist_staff[s1].cfilepath != null)
                                {
                                    NAAC_AC_343_SA_Employee_Files_DMO objfiles3 = new NAAC_AC_343_SA_Employee_Files_DMO();

                                    objfiles3.NCACSA343EF_FileName = data.filelist_staff[s1].cfilename;
                                    objfiles3.NCACSA343EF_Filedesc = data.filelist_staff[s1].cfiledesc;
                                    objfiles3.NCACSA343EF_FilePath = data.filelist_staff[s1].cfilepath;
                                    objfiles3.NCACSA343E_Id = obj3.NCACSA343E_Id;

                                    _GeneralContext.Add(objfiles3);
                                }
                            }
                        }
                    }

                    int rowAffected = _GeneralContext.SaveChanges();
                    if (rowAffected > 0)
                    {
                        data.msg = "saved";
                    }
                    else
                    {
                        data.msg = "notsaved";
                    }
                }
                else if (data.NCACSA343_Id >= 0)
                {
                    var update = _GeneralContext.NAAC_AC_343_StudentActivities_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCACSA343_Id == data.NCACSA343_Id).SingleOrDefault();

                    //obj1.NCACSA343_Id = data.NCACSA343_Id;                
                    update.NCACSA343_NoOfTeachers = data.NCACSA343_NoOfTeachers;
                    update.NCACSA343_TypeOfActivity = data.NCACSA343_TypeOfActivity;
                    update.NCACSA343_ActivityDate = data.NCACSA343_ActivityDate;
                    update.NCACSA343_OrgAgency = data.NCACSA343_OrgAgency;
                    update.NCACSA343_Place = data.NCACSA343_Place;
                    update.NCACSA343_Duration = data.NCACSA343_Duration;
                    update.NCACSA343_Year = data.ASMAY_Id;
                    update.MI_Id = data.MI_Id;

                    update.NCACSA343_NoOfStudents = data.NCACSA343_NoOfStudents;
                    update.NCACSA343_UpdatedBy = data.UserId;
                    update.NCACSA343_UpdatedDate = DateTime.Now;

                    _GeneralContext.Update(update);

                    var removeMainStu_activity_files = _GeneralContext.NAAC_AC_343_StudentActivities_Files_DMO.Where(t => t.NCACSA343_Id == data.NCACSA343_Id).ToList();
                    if (removeMainStu_activity_files.Count > 0)
                    {
                        foreach (var main_activity in removeMainStu_activity_files)
                        {
                            _GeneralContext.Remove(main_activity);
                        }
                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[i].cfilepath != null)
                                {
                                    NAAC_AC_343_StudentActivities_Files_DMO objfiles = new NAAC_AC_343_StudentActivities_Files_DMO();

                                    objfiles.NCACSA343F_FileName = data.filelist[i].cfilename;
                                    objfiles.NCACSA343F_Filedesc = data.filelist[i].cfiledesc;
                                    objfiles.NCACSA343F_FilePath = data.filelist[i].cfilepath;
                                    objfiles.NCACSA343_Id = update.NCACSA343_Id;

                                    _GeneralContext.Add(objfiles);
                                }
                            }
                        }
                    }
                    else if (removeMainStu_activity_files.Count == 0)
                    {
                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[i].cfilepath != null)
                                {
                                    NAAC_AC_343_StudentActivities_Files_DMO objfiles = new NAAC_AC_343_StudentActivities_Files_DMO();

                                    objfiles.NCACSA343F_FileName = data.filelist[i].cfilename;
                                    objfiles.NCACSA343F_Filedesc = data.filelist[i].cfiledesc;
                                    objfiles.NCACSA343F_FilePath = data.filelist[i].cfilepath;
                                    objfiles.NCACSA343_Id = update.NCACSA343_Id;

                                    _GeneralContext.Add(objfiles);
                                }
                            }
                        }
                    }

                    delete_student_activity(data.NCACSA343_Id);

                    for (int s = 0; s < data.selectdStudentlist.Length; s++)
                    {
                        NAAC_AC_343_SA_Students_DMO obj2 = new NAAC_AC_343_SA_Students_DMO();

                        obj2.NCACSA343_Id = update.NCACSA343_Id;
                        obj2.AMCST_Id = data.selectdStudentlist[s].AMCST_Id;
                        obj2.NCACSA343S_Role = data.NCACSA343S_Role;
                        obj2.NCACSA343S_ActiveFlg = true;
                        obj2.NCACSA343S_CreatedBy = data.UserId;
                        obj2.NCACSA343S_UpdatedBy = data.UserId;
                        obj2.NCACSA343S_CreatedDate = DateTime.Now;
                        obj2.NCACSA343S_UpdatedDate = DateTime.Now;

                        _GeneralContext.Add(obj2);

                        if (data.filelist_student.Length > 0)
                        {
                            for (int s1 = 0; s1 < data.filelist_student.Length; s1++)
                            {
                                if (data.filelist_student[s1].cfilepath != null)
                                {

                                    NAAC_AC_343_SA_Students_Files_DMO objfiles2 = new NAAC_AC_343_SA_Students_Files_DMO();

                                    objfiles2.NCACSA343SF_FileName = data.filelist_student[s1].cfilename;
                                    objfiles2.NCACSA343SF_Filedesc = data.filelist_student[s1].cfiledesc;
                                    objfiles2.NCACSA343SF_FilePath = data.filelist_student[s1].cfilepath;
                                    objfiles2.NCACSA343S_Id = obj2.NCACSA343S_Id;

                                    _GeneralContext.Add(objfiles2);
                                }
                            }
                        }
                    }

                    delete_satff_activity(data.NCACSA343_Id);

                    for (int e = 0; e < data.selectdStafflist.Length; e++)
                    {
                        NAAC_AC_343_SA_Employee_DMO obj3 = new NAAC_AC_343_SA_Employee_DMO();

                        //obj3.NCACSA343E_Id = data.NCACSA343E_Id;
                        obj3.NCACSA343_Id = update.NCACSA343_Id;
                        obj3.HRME_Id = data.selectdStafflist[e].HRME_Id;
                        obj3.NCACSA343E_Role = data.NCACSA343E_Role;
                        obj3.NCACSA343E_ActiveFlg = true;
                        obj3.NCACSA343E_CreatedBy = data.UserId;
                        obj3.NCACSA343E_UpdatedBy = data.UserId;
                        obj3.NCACSA343E_CreatedDate = DateTime.Now;
                        obj3.NCACSA343E_UpdatedDate = DateTime.Now;

                        _GeneralContext.Add(obj3);
                        if (data.filelist_staff.Length > 0)
                        {
                            for (int s1 = 0; s1 < data.filelist_staff.Length; s1++)
                            {
                                if (data.filelist_staff[s1].cfilepath != null)
                                {
                                    NAAC_AC_343_SA_Employee_Files_DMO objfiles3 = new NAAC_AC_343_SA_Employee_Files_DMO();

                                    objfiles3.NCACSA343EF_FileName = data.filelist_staff[s1].cfilename;
                                    objfiles3.NCACSA343EF_Filedesc = data.filelist_staff[s1].cfiledesc;
                                    objfiles3.NCACSA343EF_FilePath = data.filelist_staff[s1].cfilepath;
                                    objfiles3.NCACSA343E_Id = obj3.NCACSA343E_Id;

                                    _GeneralContext.Add(objfiles3);
                                }
                            }
                        }
                    }


                    int p = _GeneralContext.SaveChanges();
                    if (p > 0)
                    {
                        data.msg = "saved1";
                    }
                    else
                    {
                        data.msg = "notsaved1";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NaacActivity_DTO deactiveStudent(NaacActivity_DTO data)
        {
            try
            {
                var w = _GeneralContext.NAAC_AC_343_StudentActivities_DMO.Where(t => t.NCACSA343_Id == data.NCACSA343_Id).SingleOrDefault();
                if (w.NCACSA343_ActiveFlg == false)
                {
                    w.NCACSA343_ActiveFlg = true;
                }
                else
                {
                    w.NCACSA343_ActiveFlg = false;
                }
                w.NCACSA343_UpdatedBy = data.UserId;
                w.NCACSA343_UpdatedDate = DateTime.Now;
                w.MI_Id = data.MI_Id;
                _GeneralContext.Update(w);
                int y = _GeneralContext.SaveChanges();
                if (y > 0)
                {
                    data.ret = true;
                }
                else
                {
                    data.ret = false;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NaacActivity_DTO EditData(NaacActivity_DTO data)
        {
            try
            {
                var editdata = (from a in _GeneralContext.Academic
                                from b in _GeneralContext.NAAC_AC_343_StudentActivities_DMO
                                    //from c in _GeneralContext.NAAC_AC_343_SA_Students_DMO
                                    //from d in _GeneralContext.NAAC_AC_343_SA_Employee_DMO
                                where (a.MI_Id == b.MI_Id && b.NCACSA343_Year == a.ASMAY_Id && a.MI_Id == data.MI_Id && b.NCACSA343_Id == data.NCACSA343_Id)
                                select new NaacActivity_DTO
                                {
                                    NCACSA343_Id = b.NCACSA343_Id,
                                    NCACSA343_OrgAgency = b.NCACSA343_OrgAgency,
                                    NCACSA343_Place = b.NCACSA343_Place,
                                    NCACSA343_Duration = b.NCACSA343_Duration,
                                    NCACSA343_ActivityDate = Convert.ToDateTime(b.NCACSA343_ActivityDate),
                                    NCACSA343_NoOfStudents = b.NCACSA343_NoOfStudents,
                                    NCACSA343_NoOfTeachers = b.NCACSA343_NoOfTeachers,
                                    ASMAY_Year = a.ASMAY_Year,
                                    NCACSA343_Year = b.NCACSA343_Year,
                                    NCACSA343_TypeOfActivity = b.NCACSA343_TypeOfActivity,
                                    MI_Id = data.MI_Id
                                    //NCACSA343_FileName = b.NCACSA343_FileName,
                                    //NCACSA343_FilePath = b.NCACSA343_FilePath,                                                                     

                                }).Distinct().ToList();

                data.editlist = editdata.ToArray();

                var stud_id = (from a in _GeneralContext.NAAC_AC_343_SA_Students_DMO
                               from b in _GeneralContext.NAAC_AC_343_StudentActivities_DMO
                               where (b.NCACSA343_Id == a.NCACSA343_Id && b.MI_Id == data.MI_Id && b.NCACSA343_Id == data.NCACSA343_Id)
                               select new NaacActivity_DTO
                               {
                                   AMCST_Id = a.AMCST_Id,
                                   //NCACSA343S_FileName = a.NCACSA343S_FileName,
                                   //NCACSA343S_FilePath = a.NCACSA343S_FilePath,
                                   NCACSA343S_Role = a.NCACSA343S_Role,
                               }).Distinct().ToList();

                data.editstudetdata = stud_id.ToArray();

                List<long> stud_ids = new List<long>();

                if (stud_id.Count > 0)
                {
                    foreach (var item in stud_id)
                    {
                        stud_ids.Add(item.AMCST_Id);
                    }
                }

                var studendata = (from t in _GeneralContext.Adm_College_Yearly_StudentDMO
                                  from b in _GeneralContext.NAAC_AC_343_SA_Students_DMO
                                  from c in _GeneralContext.NAAC_AC_343_StudentActivities_DMO
                                  where (c.MI_Id == data.MI_Id && c.NCACSA343_Id == b.NCACSA343_Id && t.AMCST_Id == b.AMCST_Id && stud_ids.Contains(t.AMCST_Id))
                                  select new NaacActivity_DTO
                                  {
                                      ASMAY_Id = t.ASMAY_Id,
                                      AMCO_Id = t.AMCO_Id,
                                      AMB_Id = t.AMB_Id,
                                      AMSE_Id = t.AMSE_Id,
                                      ACMS_Id = t.ACMS_Id,
                                  }).Distinct().ToList();
                List<long> yearid = new List<long>();
                List<long> courseid = new List<long>();
                List<long> branchid = new List<long>();
                List<long> semsid = new List<long>();
                List<long> sectionid = new List<long>();
                foreach (var year in studendata)
                {
                    yearid.Add(year.ASMAY_Id);
                }
                foreach (var course in studendata)
                {
                    courseid.Add(course.AMCO_Id);
                }
                foreach (var branch in studendata)
                {
                    branchid.Add(branch.AMB_Id);
                }
                foreach (var semester in studendata)
                {
                    semsid.Add(semester.AMSE_Id);
                }
                foreach (var sections in studendata)
                {
                    sectionid.Add(sections.ACMS_Id);
                }

                var cours = (from a in _GeneralContext.MasterCourseDMO
                             from b in _GeneralContext.CLG_Adm_College_AY_CourseDMO
                             where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag && b.MI_Id == data.MI_Id && yearid.Contains(b.ASMAY_Id) && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                             select a).Distinct().OrderBy(t => t.AMCO_Order).ToList();
              
                data.courselist = cours.ToArray();

                var selectedcourse = (from a in _GeneralContext.MasterCourseDMO
                                      where (a.MI_Id == data.MI_Id && courseid.Contains(a.AMCO_Id))
                                      select new NaacActivity_DTO
                                      {
                                          AMCO_Id = a.AMCO_Id,
                                          AMCO_CourseName = a.AMCO_CourseName,
                                      }).Distinct().ToList();

                data.editcourse = selectedcourse.ToArray();

                var brnch = (from a in _GeneralContext.ClgMasterBranchDMO
                             from b in _GeneralContext.CLG_Adm_College_AY_CourseDMO
                             from c in _GeneralContext.CLG_Adm_College_AY_Course_BranchDMO
                             where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && yearid.Contains(b.ASMAY_Id) && b.ACAYC_ActiveFlag && courseid.Contains(b.AMCO_Id) && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag)
                             select new NaacActivity_DTO
                             {
                                 AMB_Id = a.AMB_Id,
                                 AMB_BranchName = a.AMB_BranchName,
                                 //AMB_Order = a.AMB_Order,
                             }).Distinct().OrderBy(t => t.AMB_Order).ToList();

                data.branchlist = brnch.ToArray();

                var selectedbranch = _GeneralContext.ClgMasterBranchDMO.Where(t => t.MI_Id == data.MI_Id && branchid.Contains(t.AMB_Id)).Distinct().ToList();

                data.editbranch = selectedbranch.ToArray();

                var sems = (from a in _GeneralContext.CLG_Adm_Master_SemesterDMO
                            from b in _GeneralContext.CLG_Adm_College_AY_CourseDMO
                            from c in _GeneralContext.CLG_Adm_College_AY_Course_BranchDMO
                            from d in _GeneralContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                            where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id
                            && yearid.Contains(b.ASMAY_Id) && b.ACAYC_ActiveFlag && courseid.Contains(b.AMCO_Id)
                            && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && branchid.Contains(c.AMB_Id)
                            && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id
                            && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
                            select new NaacActivity_DTO
                            {
                                AMSE_Id = a.AMSE_Id,
                                AMSE_SEMName = a.AMSE_SEMName,
                            }).Distinct().OrderBy(t => t.AMSE_SEMOrder).ToList();

                data.semisterlist = sems.ToArray();

                var selectedsemester = _GeneralContext.CLG_Adm_Master_SemesterDMO.Where(t => t.MI_Id == data.MI_Id
                && semsid.Contains(t.AMSE_Id)).Distinct().ToList();

                data.editsemeste = selectedsemester.ToArray();

                var section = (from a in _GeneralContext.Adm_College_Yearly_StudentDMO
                               from b in _GeneralContext.Adm_College_Master_SectionDMO
                               where (yearid.Contains(a.ASMAY_Id) && b.ACMS_Id == a.ACMS_Id && b.MI_Id == data.MI_Id
                               && branchid.Contains(a.AMB_Id) && courseid.Contains(a.AMCO_Id) && semsid.Contains(a.AMSE_Id))
                               select new NaacActivity_DTO
                               {
                                   ACMS_Id = b.ACMS_Id,
                                   ACMS_SectionName = b.ACMS_SectionName,
                                   ACMS_Order = b.ACMS_Order
                               }).Distinct().OrderBy(t => t.ACMS_Order).ToList();

                data.sectionlist = section.ToArray();

                var selectedsection = _GeneralContext.Adm_College_Master_SectionDMO.Where(t => t.MI_Id == data.MI_Id
                && sectionid.Contains(t.ACMS_Id)).Distinct().ToList();

                data.editsection = selectedsection.ToArray();


                data.studentlist = (from m in _GeneralContext.Adm_Master_College_StudentDMO
                                    from n in _GeneralContext.Adm_College_Yearly_StudentDMO
                                    where (m.AMCST_Id == n.AMCST_Id && m.MI_Id == data.MI_Id && yearid.Contains(n.ASMAY_Id) && m.AMCST_SOL.Equals("S") && m.AMCST_ActiveFlag == true && n.ACYST_ActiveFlag == 1 && courseid.Contains(n.AMCO_Id) && branchid.Contains(n.AMB_Id)
                                    && semsid.Contains(n.AMSE_Id) && sectionid.Contains(n.ACMS_Id))
                                    select new NaacActivity_DTO
                                    {
                                        AMCST_Id = m.AMCST_Id,
                                        MI_Id = m.MI_Id,
                                        ASMAY_Id = m.ASMAY_Id,
                                        AMCST_FirstName = ((m.AMCST_FirstName == null ? " " : m.AMCST_FirstName) + " " + (m.AMCST_MiddleName == null ? " " : m.AMCST_MiddleName) + " " + (m.AMCST_LastName == null ? " " : m.AMCST_LastName)).Trim(),
                                        AMCST_MiddleName = m.AMCST_MiddleName,
                                        AMCST_LastName = m.AMCST_LastName,
                                        AMCST_AdmNo = m.AMCST_AdmNo

                                    }).Distinct().OrderBy(t => t.AMCST_FirstName).ToArray();

                data.studentRoledata = (from a in _GeneralContext.NAAC_AC_343_SA_Students_DMO
                                        from b in _GeneralContext.NAAC_AC_343_StudentActivities_DMO
                                        where (b.NCACSA343_Id == data.NCACSA343_Id && b.MI_Id == data.MI_Id
                                        && b.NCACSA343_Id == a.NCACSA343_Id)
                                        select new NaacActivity_DTO
                                        {
                                            NCACSA343S_Role = a.NCACSA343S_Role,
                                        }).Distinct().ToArray();



                var empdata = (from a in _GeneralContext.NAAC_AC_343_SA_Employee_DMO
                               from b in _GeneralContext.NAAC_AC_343_StudentActivities_DMO
                               where (b.NCACSA343_Id == data.NCACSA343_Id && b.MI_Id == data.MI_Id
                               && b.NCACSA343_Id == a.NCACSA343_Id)
                               select new NaacActivity_DTO
                               {
                                   HRME_Id = a.HRME_Id,
                                   NCACSA343E_Role = a.NCACSA343E_Role
                               }).Distinct().ToList();
                List<long> hrmeids = new List<long>();
                if (empdata.Count > 0)
                {
                    foreach (var hrme in empdata)
                    {
                        hrmeids.Add(hrme.HRME_Id);
                    }
                }

                data.empdatarole = empdata.ToArray();

                var empdept = (from a in _GeneralContext.HR_Master_Employee_DMO
                               where (hrmeids.Contains(a.HRME_Id) && a.MI_Id == data.MI_Id)
                               select (a.HRMD_Id)).Distinct().ToList();

                data.empdeptSelectedId = empdept.ToArray();
                var empdes = (from a in _GeneralContext.HR_Master_Employee_DMO
                              where (hrmeids.Contains(a.HRME_Id) && a.MI_Id == data.MI_Id)
                              select (a.HRMDES_Id)).Distinct().ToList();

                data.empDesSelectedId = empdes.ToArray();

                data.filldesignation = (from a in _GeneralContext.HR_Master_Employee_DMO
                                        from b in _GeneralContext.HR_Master_Designation
                                        where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true
                                        && a.HRMDES_Id == b.HRMDES_Id && empdept.Contains(a.HRMD_Id)
                                        && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                                        select b).Distinct().ToArray();

                data.stafftlist = (from a in _GeneralContext.HR_Master_Employee_DMO.Where(a => a.MI_Id == data.MI_Id
                                   && a.HRME_ActiveFlag == true && empdes.Contains(a.HRMDES_Id)
                                   && empdept.Contains(a.HRMD_Id))
                                   select new NaacActivity_DTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       MI_Id = a.MI_Id,
                                       empname = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                       HRME_EmployeeOrder=a.HRME_EmployeeOrder,
                                   }).Distinct().OrderBy(t => t.HRME_EmployeeOrder).ToArray();

                data.editMainSActFileslist = (from t in _GeneralContext.NAAC_AC_343_StudentActivities_Files_DMO
                                              from b in _GeneralContext.NAAC_AC_343_StudentActivities_DMO
                                              where (t.NCACSA343_Id == data.NCACSA343_Id
                                              && t.NCACSA343_Id == b.NCACSA343_Id
                                              && b.MI_Id == data.MI_Id)
                                              select new NaacActivity_DTO
                                              {
                                                  cfilename = t.NCACSA343F_FileName,
                                                  cfilepath = t.NCACSA343F_FilePath,
                                                  cfiledesc = t.NCACSA343F_Filedesc,
                                              }).Distinct().ToArray();

                List<long> student_tableid = new List<long>();
                var filter_studenttableid = (from a in _GeneralContext.NAAC_AC_343_StudentActivities_DMO
                                             from b in _GeneralContext.NAAC_AC_343_SA_Students_DMO
                                             where (a.NCACSA343_Id == data.NCACSA343_Id && a.MI_Id == data.MI_Id
                                             && a.NCACSA343_Id == b.NCACSA343_Id)
                                             select b).ToList();
                if (filter_studenttableid.Count > 0)
                {
                    foreach (var item in filter_studenttableid)
                    {
                        student_tableid.Add(item.NCACSA343S_Id);
                    }
                }

                data.editStudentActFileslist = (from t in _GeneralContext.NAAC_AC_343_SA_Students_Files_DMO
                                                from b in _GeneralContext.NAAC_AC_343_SA_Students_DMO
                                                where (student_tableid.Contains(t.NCACSA343S_Id)
                                                && t.NCACSA343S_Id == b.NCACSA343S_Id)
                                                select new NaacActivity_DTO
                                                {
                                                    cfilename = t.NCACSA343SF_FileName,
                                                    cfilepath = t.NCACSA343SF_FilePath,
                                                    cfiledesc = t.NCACSA343SF_Filedesc,
                                                }).Distinct().ToArray();

                List<long> stafft_tableid = new List<long>();
                var filter_stafftableid = (from a in _GeneralContext.NAAC_AC_343_StudentActivities_DMO
                                           from b in _GeneralContext.NAAC_AC_343_SA_Employee_DMO
                                           where (a.NCACSA343_Id == data.NCACSA343_Id && a.MI_Id == data.MI_Id
                                           && a.NCACSA343_Id == b.NCACSA343_Id)
                                           select b).ToList();
                if (filter_stafftableid.Count > 0)
                {
                    foreach (var item in filter_stafftableid)
                    {
                        student_tableid.Add(item.NCACSA343E_Id);
                    }
                }

                data.editStaffActFileslist = (from t in _GeneralContext.NAAC_AC_343_SA_Employee_Files_DMO
                                              from b in _GeneralContext.NAAC_AC_343_SA_Employee_DMO
                                              where (student_tableid.Contains(t.NCACSA343E_Id)
                                              && t.NCACSA343E_Id == b.NCACSA343E_Id)
                                              select new NaacActivity_DTO
                                              {
                                                  cfilename = t.NCACSA343EF_FileName,
                                                  cfilepath = t.NCACSA343EF_FilePath,
                                                  cfiledesc = t.NCACSA343EF_Filedesc,
                                              }).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public async Task<NaacActivity_DTO> get_MappedStudent(NaacActivity_DTO data)
        {
            try
            {

                using (var cmd = _GeneralContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_343_Student_Modal_Data";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@NCACSA343_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.NCACSA343_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.studentmodaldata = retObject.ToArray();

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
        public async Task<NaacActivity_DTO> get_MappedStaff(NaacActivity_DTO data)
        {
            try
            {
                using (var cmd = _GeneralContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_343_Staff_Modal_Data";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@NCACSA343_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.NCACSA343_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.staffmodaldata = retObject.ToArray();

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
        public NaacActivity_DTO deactive_student(NaacActivity_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_343_SA_Students_DMO.Single(t => t.NCACSA343S_Id == data.NCACSA343S_Id);

                if (result.NCACSA343S_ActiveFlg == true)
                {
                    result.NCACSA343S_ActiveFlg = false;
                }
                else if (result.NCACSA343S_ActiveFlg == false)
                {
                    result.NCACSA343S_ActiveFlg = true;
                }

                result.NCACSA343S_UpdatedBy = data.UserId;
                result.NCACSA343S_UpdatedDate = DateTime.Now;


                _GeneralContext.Update(result);
                int s = _GeneralContext.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.ret = false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NaacActivity_DTO deactive_staff(NaacActivity_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_343_SA_Employee_DMO.Single(t => t.NCACSA343E_Id == data.NCACSA343E_Id);

                if (result.NCACSA343E_ActiveFlg == true)
                {
                    result.NCACSA343E_ActiveFlg = false;
                }
                else if (result.NCACSA343E_ActiveFlg == false)
                {
                    result.NCACSA343E_ActiveFlg = true;
                }

                result.NCACSA343E_UpdatedBy = data.UserId;
                result.NCACSA343E_UpdatedDate = DateTime.Now;


                _GeneralContext.Update(result);
                int s = _GeneralContext.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.ret = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NaacActivity_DTO delete_student_activity(Int64 id)
        {
            NaacActivity_DTO data = new NaacActivity_DTO();
            try
            {
                var count_student = _GeneralContext.NAAC_AC_343_SA_Students_DMO.Where(t => t.NCACSA343_Id == id).ToList();

                List<long> count_student_Activity_ids = new List<long>();
                if (count_student.Count > 0)
                {
                    foreach (var item in count_student)
                    {
                        count_student_Activity_ids.Add(item.NCACSA343S_Id);
                    }
                }
                var count_student_activity_files = (from a in _GeneralContext.NAAC_AC_343_SA_Students_Files_DMO
                                                    where (count_student_Activity_ids.Contains(a.NCACSA343S_Id))
                                                    select a).ToList();
                if (count_student_activity_files.Count > 0)
                {
                    foreach (var item in count_student_activity_files)
                    {
                        _GeneralContext.Remove(item);
                    }
                }
                foreach (var item2 in count_student)
                {
                    _GeneralContext.Remove(item2);
                }
                int row = _GeneralContext.SaveChanges();
                if (row > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public NaacActivity_DTO delete_satff_activity(Int64 id)
        {
            NaacActivity_DTO data = new NaacActivity_DTO();
            try
            {
                var count_staff = _GeneralContext.NAAC_AC_343_SA_Employee_DMO.Where(t => t.NCACSA343_Id == id).ToList();

                List<long> count_staff_Activity_ids = new List<long>();
                if (count_staff.Count > 0)
                {
                    foreach (var item in count_staff)
                    {
                        count_staff_Activity_ids.Add(item.NCACSA343E_Id);
                    }
                }
                var count_staff_activity_files = (from a in _GeneralContext.NAAC_AC_343_SA_Employee_Files_DMO
                                                  where (count_staff_Activity_ids.Contains(a.NCACSA343E_Id))
                                                  select a).ToList();
                if (count_staff_activity_files.Count > 0)
                {
                    foreach (var item in count_staff_activity_files)
                    {
                        _GeneralContext.Remove(item);
                    }
                }
                foreach (var item2 in count_staff)
                {
                    _GeneralContext.Remove(item2);
                }
                int row = _GeneralContext.SaveChanges();
                if (row > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public NaacActivity_DTO viewdocument_MainActUploadFiles(NaacActivity_DTO data)
        {
            try
            {
                data.viewdocument_MainActUploadFiles = (from t in _GeneralContext.NAAC_AC_343_StudentActivities_Files_DMO

                                                        where (t.NCACSA343_Id == data.NCACSA343_Id)
                                                        select new NaacActivity_DTO
                                                        {
                                                            cfilename = t.NCACSA343F_FileName,
                                                            cfilepath = t.NCACSA343F_FilePath,
                                                            cfiledesc = t.NCACSA343F_Filedesc,
                                                            NCACSA343F_Id = t.NCACSA343F_Id,
                                                            NCACSA343_Id = t.NCACSA343_Id,

                                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NaacActivity_DTO delete_MainActUploadFiles(NaacActivity_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_343_StudentActivities_Files_DMO.Where(t => t.NCACSA343F_Id == data.NCACSA343F_Id).SingleOrDefault();
                _GeneralContext.Remove(result);
                //if (result.Count > 0)
                //{
                //    foreach (var resultid in result)
                //    {
                //        _GeneralContext.Remove(resultid);
                //    }
                //}
                int row = _GeneralContext.SaveChanges();
                if (row > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                data.viewdocument_MainActUploadFiles = (from t in _GeneralContext.NAAC_AC_343_StudentActivities_Files_DMO

                                                        where (t.NCACSA343_Id == data.NCACSA343_Id)
                                                        select new NaacActivity_DTO
                                                        {
                                                            cfilename = t.NCACSA343F_FileName,
                                                            cfilepath = t.NCACSA343F_FilePath,
                                                            cfiledesc = t.NCACSA343F_Filedesc,
                                                            NCACSA343F_Id = t.NCACSA343F_Id,
                                                            NCACSA343_Id = t.NCACSA343_Id,

                                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NaacActivity_DTO viewdocument_StudentActUploadFiles(NaacActivity_DTO data)
        {
            try
            {
                data.viewdocument_StudentActUploadFiles = (from t in _GeneralContext.NAAC_AC_343_SA_Students_Files_DMO

                                                           where (t.NCACSA343S_Id == data.NCACSA343S_Id)
                                                           select new NaacActivity_DTO
                                                           {
                                                               cfilename = t.NCACSA343SF_FileName,
                                                               cfilepath = t.NCACSA343SF_FilePath,
                                                               cfiledesc = t.NCACSA343SF_Filedesc,
                                                               NCACSA343SF_Id = t.NCACSA343SF_Id,
                                                               NCACSA343S_Id = t.NCACSA343S_Id,

                                                           }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NaacActivity_DTO delete_StudentActUploadFiles(NaacActivity_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_343_SA_Students_Files_DMO.Where(t => t.NCACSA343SF_Id == data.NCACSA343SF_Id).SingleOrDefault();
                _GeneralContext.Remove(result);
                //if (result.Count > 0)
                //{
                //    foreach (var resultid in result)
                //    {
                //        _GeneralContext.Remove(resultid);
                //    }
                //}
                int row = _GeneralContext.SaveChanges();
                if (row > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                data.viewdocument_StudentActUploadFiles = (from t in _GeneralContext.NAAC_AC_343_SA_Students_Files_DMO

                                                           where (t.NCACSA343S_Id == data.NCACSA343S_Id)
                                                           select new NaacActivity_DTO
                                                           {
                                                               cfilename = t.NCACSA343SF_FileName,
                                                               cfilepath = t.NCACSA343SF_FilePath,
                                                               cfiledesc = t.NCACSA343SF_Filedesc,
                                                               NCACSA343SF_Id = t.NCACSA343SF_Id,
                                                               NCACSA343S_Id = t.NCACSA343S_Id,

                                                           }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NaacActivity_DTO viewdocument_StaffActUploadFiles(NaacActivity_DTO data)
        {
            try
            {
                data.viewdocument_StaffActUploadFiles = (from t in _GeneralContext.NAAC_AC_343_SA_Employee_Files_DMO

                                                         where (t.NCACSA343E_Id == data.NCACSA343E_Id)
                                                         select new NaacActivity_DTO
                                                         {
                                                             cfilename = t.NCACSA343EF_FileName,
                                                             cfilepath = t.NCACSA343EF_FilePath,
                                                             cfiledesc = t.NCACSA343EF_Filedesc,
                                                             NCACSA343EF_Id = t.NCACSA343EF_Id,
                                                             NCACSA343E_Id = t.NCACSA343E_Id,

                                                         }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NaacActivity_DTO delete_StaffActUploadFiles(NaacActivity_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_343_SA_Employee_Files_DMO.Where(t => t.NCACSA343EF_Id == data.NCACSA343EF_Id).SingleOrDefault();
                _GeneralContext.Remove(result);
                //if (result.Count > 0)
                //{
                //    foreach (var resultid in result)
                //    {
                //        _GeneralContext.Remove(resultid);
                //    }
                //}
                int row = _GeneralContext.SaveChanges();
                if (row > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                data.viewdocument_StaffActUploadFiles = (from t in _GeneralContext.NAAC_AC_343_SA_Employee_Files_DMO

                                                         where (t.NCACSA343E_Id == data.NCACSA343E_Id)
                                                         select new NaacActivity_DTO
                                                         {
                                                             cfilename = t.NCACSA343EF_FileName,
                                                             cfilepath = t.NCACSA343EF_FilePath,
                                                             cfiledesc = t.NCACSA343EF_Filedesc,
                                                             NCACSA343EF_Id = t.NCACSA343EF_Id,
                                                             NCACSA343E_Id = t.NCACSA343E_Id,

                                                         }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }

    }
}
