using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NaacExtnActivitiesImpl : Interface.NaacExtnActivitiesInterface
    {
        public GeneralContext _GeneralContext;
        public NaacExtnActivitiesImpl(GeneralContext r)
        {
            _GeneralContext = r;
        }
        public NAAC_AC_344_ExtnActivities_DTO loaddata(NAAC_AC_344_ExtnActivities_DTO data)
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
                data.yearlist = (from t in _GeneralContext.Academic
                                 where (t.MI_Id == data.MI_Id && t.Is_Active == true)
                                 select new NAAC_AC_344_ExtnActivities_DTO
                                 {
                                     ASMAY_Id = t.ASMAY_Id,
                                     ASMAY_Year = t.ASMAY_Year
                                 }).OrderByDescending(t => t.ASMAY_Year).ToArray();

                data.courselist = (from a in _GeneralContext.MasterCourseDMO
                                   from b in _GeneralContext.CLG_Adm_College_AY_CourseDMO
                                   where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true && b.ACAYC_ActiveFlag == true && a.AMCO_Id == b.AMCO_Id)
                                   select new NAAC_AC_344_ExtnActivities_DTO { AMCO_Id = a.AMCO_Id, AMCO_CourseName = a.AMCO_CourseName, AMCO_Order = a.AMCO_Order }).OrderByDescending(t => t.AMCO_Order).Distinct().ToArray();

                data.filldepartment = _GeneralContext.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id && t.HRMD_ActiveFlag == true).Distinct().OrderBy(t => t.HRMD_DepartmentName).ToArray();

                data.alldata1 = (from a in _GeneralContext.Academic
                                 from b in _GeneralContext.NAAC_AC_344_ExtnActivities_DMO
                                 from c in _GeneralContext.NAAC_AC_344_ExtnActivities_Students_DMO
                                 where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.Is_Active == true
                                 && a.ASMAY_Id == b.NCACET343_Year && c.NCACET343_Id == b.NCACET343_Id)
                                 select new NAAC_AC_344_ExtnActivities_DTO
                                 {
                                     NCACET343_Id = b.NCACET343_Id,
                                     NCACET343_TypeOfActivity = b.NCACET343_TypeOfActivity,
                                     NCACET343_SchemeName = b.NCACET343_SchemeName,
                                     NCACET343_Place = b.NCACET343_Place,
                                     NCACET343_Duration = b.NCACET343_Duration,
                                     NCACET343_NoOfStudents = b.NCACET343_NoOfStudents,
                                     NCACET343_ActiveFlg = b.NCACET343_ActiveFlg,
                                     ASMAY_Year = a.ASMAY_Year,
                                     MI_Id = a.MI_Id,
                                     NCACET343_StatusFlg = b.NCACET343_StatusFlg,
                                 }).Distinct().OrderByDescending(t => t.NCACET343_Id).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_AC_344_ExtnActivities_DTO get_branch(NAAC_AC_344_ExtnActivities_DTO data)
        {
            try
            {
                data.branchlist = (from a in _GeneralContext.ClgMasterBranchDMO
                                   from b in _GeneralContext.CLG_Adm_College_AY_CourseDMO
                                   from c in _GeneralContext.CLG_Adm_College_AY_Course_BranchDMO
                                   where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && a.AMB_ActiveFlag == true && b.ACAYC_ActiveFlag == true && c.ACAYCB_ActiveFlag == true && a.AMB_Id == c.AMB_Id && b.AMCO_Id == data.AMCO_Id && b.ASMAY_Id == data.ASMAY_Id && c.ACAYC_Id == b.ACAYC_Id)
                                   select new NAAC_AC_344_ExtnActivities_DTO
                                   {
                                       AMB_Id = a.AMB_Id,
                                       AMB_BranchName = a.AMB_BranchName,
                                   }).Distinct().OrderByDescending(t => t.AMCO_Id).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_AC_344_ExtnActivities_DTO get_sems(NAAC_AC_344_ExtnActivities_DTO data)
        {
            try
            {
                data.semisterlist = (from a in _GeneralContext.CLG_Adm_Master_SemesterDMO
                                     from b in _GeneralContext.CLG_Adm_College_AY_CourseDMO
                                     from c in _GeneralContext.CLG_Adm_College_AY_Course_BranchDMO
                                     from d in _GeneralContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                     where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && a.AMSE_ActiveFlg == true && b.ACAYC_ActiveFlag == true && c.ACAYCB_ActiveFlag == true && d.ACAYCBS_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == data.AMCO_Id && b.ACAYC_Id == c.ACAYC_Id && c.AMB_Id == data.AMB_Id && c.ACAYCB_Id == d.ACAYCB_Id && a.AMSE_Id == d.AMSE_Id)
                                     select new NAAC_AC_344_ExtnActivities_DTO
                                     {
                                         AMSE_Id = a.AMSE_Id,
                                         AMSE_SEMName = a.AMSE_SEMName,
                                         AMSE_SEMCode = a.AMSE_SEMCode,
                                     }).Distinct().OrderBy(t => t.AMSE_Id).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_AC_344_ExtnActivities_DTO get_section(NAAC_AC_344_ExtnActivities_DTO data)
        {
            try
            {
                data.sectionlist = (from a in _GeneralContext.Adm_College_Yearly_StudentDMO
                                    from b in _GeneralContext.Adm_College_Master_SectionDMO
                                    where (b.MI_Id == data.MI_Id && a.ACMS_Id == b.ACMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id)
                                    select new NAAC_AC_344_ExtnActivities_DTO
                                    {
                                        ACMS_Id = b.ACMS_Id,
                                        ACMS_SectionName = b.ACMS_SectionName,
                                    }).Distinct().OrderByDescending(t => t.ACMS_Id).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_AC_344_ExtnActivities_DTO GetStudentDetails(NAAC_AC_344_ExtnActivities_DTO data)
        {
            try
            {
                data.studentlist = (from m in _GeneralContext.Adm_Master_College_StudentDMO
                                    from n in _GeneralContext.Adm_College_Yearly_StudentDMO
                                    where m.AMCST_Id == n.AMCST_Id && m.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && m.AMCST_SOL.Equals("S") && m.AMCST_ActiveFlag == true && n.ACYST_ActiveFlag == 1 && n.AMCO_Id == data.AMCO_Id && n.AMB_Id == data.AMB_Id && n.AMSE_Id == data.AMSE_Id && n.ACMS_Id == data.ACMS_Id
                                    select new NAAC_AC_344_ExtnActivities_DTO
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
        public NAAC_AC_344_ExtnActivities_DTO saverecord(NAAC_AC_344_ExtnActivities_DTO data)
        {
            try
            {
                if (data.NCACET343_Id == 0)
                {
                    List<long> tempstudentid = new List<long>();
                    if (data.selectdStudentlist.Length > 0)
                    {
                        foreach (var studentid in data.selectdStudentlist)
                        {
                            tempstudentid.Add(studentid.AMCST_Id);
                        }
                    }
                    var duplicate = (from a in _GeneralContext.NAAC_AC_344_ExtnActivities_DMO
                                     from b in _GeneralContext.NAAC_AC_344_ExtnActivities_Students_DMO
                                     where (a.NCACET343_Id == b.NCACET343_Id && a.MI_Id == data.MI_Id && tempstudentid.Contains(b.AMCST_Id) && a.NCACET343_TypeOfActivity == data.NCACET343_TypeOfActivity)
                                     select b).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_344_ExtnActivities_DMO obj1 = new NAAC_AC_344_ExtnActivities_DMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCACET343_ActiveFlg = true;
                        obj1.NCACET343_ActivityDate = data.NCACET343_ActivityDate;
                        obj1.NCACET343_CreatedBy = data.UserId;
                        obj1.NCACET343_CreatedDate = DateTime.Now;
                        obj1.NCACET343_Duration = data.NCACET343_Duration;
                        obj1.NCACET343_NoOfStudents = data.NCACET343_NoOfStudents;
                        obj1.NCACET343_OrgAgency = data.NCACET343_OrgAgency;
                        obj1.NCACET343_Place = data.NCACET343_Place;
                        obj1.NCACET343_SchemeName = data.NCACET343_SchemeName;
                        obj1.NCACET343_TypeOfActivity = data.NCACET343_TypeOfActivity;
                        obj1.NCACET343_UpdatedBy = data.UserId;
                        obj1.NCACET343_UpdatedDate = DateTime.Now;
                        obj1.NCACET343_StatusFlg = "";
                        obj1.NCACET343_NoOEmployee = data.NCACET343_NoOEmployee;
                        obj1.NCACET343_Year = data.ASMAY_Id;
                        
                        _GeneralContext.Add(obj1);
                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[i].cfilepath != null)
                                {



                                    NAAC_AC_344_ExtnActivities_Files_DMO objfiles = new NAAC_AC_344_ExtnActivities_Files_DMO();
                                    objfiles.NCACET343_Id = obj1.NCACET343_Id;
                                    objfiles.NCACET343F_FileName = data.filelist[i].cfilename;
                                    objfiles.NCACET343F_Filedesc = data.filelist[i].cfiledesc;
                                    objfiles.NCACET343F_FilePath = data.filelist[i].cfilepath;
                                    objfiles.NCACET343F_StatusFlg = "";
                                    objfiles.NCACET343F_ActiveFlg = true;
                                    _GeneralContext.Add(objfiles);
                                }
                            }
                        }
                        for (int s = 0; s < data.selectdStudentlist.Length; s++)
                        {
                            NAAC_AC_344_ExtnActivities_Students_DMO obj2 = new NAAC_AC_344_ExtnActivities_Students_DMO();
                            obj2.NCACET343_Id = obj1.NCACET343_Id;
                            obj2.AMCST_Id = data.selectdStudentlist[s].AMCST_Id;
                            obj2.NCACET343S_Role = data.NCACET343S_Role;
                            obj2.NCACET343S_ActiveFlg = true;
                            obj2.NCACET343S_CreatedBy = data.UserId;
                            obj2.NCACET343S_StatusFlg = "";

                            obj2.NCACET343S_CreatedDate = DateTime.Now;
                            obj2.NCACET343S_UpdatedBy = data.UserId;
                            obj2.NCACET343S_UpdatedDate = DateTime.Now;
                            _GeneralContext.Add(obj2);
                            if (data.filelist_student.Length > 0)
                            {
                                for (int s1 = 0; s1 < data.filelist_student.Length; s1++)
                                {
                                    if (data.filelist_student[s1].cfilepath != null)
                                    {
                                        NAAC_AC_344_ExtnActivities_Students_Files_DMO objfiles3 = new NAAC_AC_344_ExtnActivities_Students_Files_DMO();
                                        objfiles3.NCACET343S_Id = obj2.NCACET343S_Id;
                                        objfiles3.NCACET343SF_FileName = data.filelist_student[s1].cfilename;
                                        objfiles3.NCACET343SF_Filedesc = data.filelist_student[s1].cfiledesc;
                                        objfiles3.NCACET343SF_FilePath = data.filelist_student[s1].cfilepath;
                                        objfiles3.NCACET343SF_StatusFlg = "";
                                        objfiles3.NCACET343SF_ActiveFlg = true;
                                        _GeneralContext.Add(objfiles3);
                                    }
                                }
                            }
                        }

                        for (int e = 0; e < data.selectdStafflist.Length; e++)
                        {
                            NAAC_AC_344_ExtnActivities_Staff_DMO obj3 = new NAAC_AC_344_ExtnActivities_Staff_DMO();

                            //obj3.NCACET344STF_Id = data.NCACET344STF_Id;
                            obj3.NCACET343_Id = obj1.NCACET343_Id;
                            obj3.HRME_Id = data.selectdStafflist[e].HRME_Id;
                            obj3.NCACET344STF_Role = data.NCACET344STF_Role;
                            obj3.NCACET344STF_ActiveFlg = true;
                            obj3.NCACET344STF_CreatedBy = data.UserId;
                            obj3.NCACET344STF_UpdatedBy = data.UserId;
                            obj3.NCACET344STF_CreatedDate = DateTime.Now;
                            obj3.NCACET344STF_UpdatedDate = DateTime.Now;
                            obj3.NCACET344STF_StatusFlg = "";



                            _GeneralContext.Add(obj3);
                            if (data.filelist_staff.Length > 0)
                            {
                                for (int s1 = 0; s1 < data.filelist_staff.Length; s1++)
                                {
                                    if (data.filelist_staff[s1].cfilepath != null)
                                    {
                                        NAAC_AC_344_ExtnActivities_Staff_Files_DMO objfiles3 = new NAAC_AC_344_ExtnActivities_Staff_Files_DMO();

                                        objfiles3.NCACET344STFF_FileName = data.filelist_staff[s1].cfilename;
                                        objfiles3.NCACET344STFF_Filedesc = data.filelist_staff[s1].cfiledesc;
                                        objfiles3.NCACET344STFF_FilePath = data.filelist_staff[s1].cfilepath;
                                        objfiles3.NCACET344STF_Id = obj3.NCACET344STF_Id;
                                        objfiles3.NCACET344STFF_ActiveFlg = true;
                                        objfiles3.NCACET344STFF_StatusFlg = "";

                                        _GeneralContext.Add(objfiles3);
                                    }
                                }
                            }
                        }

                        int u = _GeneralContext.SaveChanges();
                        if (u > 0)
                        {
                            data.msg = "saved";
                        }
                        else
                        {
                            data.msg = "failed";
                        }
                    }
                }

                else if (data.NCACET343_Id > 0)
                {
                    var update = _GeneralContext.NAAC_AC_344_ExtnActivities_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCACET343_Id == data.NCACET343_Id).SingleOrDefault();
                    update.NCACET343_UpdatedBy = data.UserId;
                    update.NCACET343_TypeOfActivity = data.NCACET343_TypeOfActivity;
                    update.NCACET343_SchemeName = data.NCACET343_SchemeName;
                    update.NCACET343_ActivityDate = data.NCACET343_ActivityDate;
                    update.NCACET343_OrgAgency = data.NCACET343_OrgAgency;
                   
                    update.NCACET343_UpdatedDate = DateTime.Now;
                    update.NCACET343_Year = data.ASMAY_Id;

                    update.NCACET343_TypeOfActivity = data.NCACET343_TypeOfActivity;
                    update.NCACET343_SchemeName = data.NCACET343_SchemeName;
                   // update.NCACCOMM_Flg = data.NCACCOMM_Flg;
                    _GeneralContext.Update(update);
                    if (data.filelist.Count() > 0)
                    {
                        List<long> Fid = new List<long>();
                        foreach (var item in data.filelist)
                        {
                            Fid.Add(item.NCACET343F_Id);
                        }
                        var removefile11 = _GeneralContext.NAAC_AC_344_ExtnActivities_Files_DMO.Where(t => t.NCACET343_Id == data.NCACET343_Id && !Fid.Contains(t.NCACET343F_Id)).Distinct().ToList();

                        if (removefile11.Count > 0)
                        {
                            foreach (var item2 in removefile11)
                            {
                                var deactfile = _GeneralContext.NAAC_AC_344_ExtnActivities_Files_DMO.Single(t => t.NCACET343_Id == data.NCACET343_Id && t.NCACET343F_Id == item2.NCACET343F_Id);
                                deactfile.NCACET343F_ActiveFlg = false;
                                _GeneralContext.Update(deactfile);
                            }
                        }

                        foreach (NAAC_AC_344_ExtnActivities_DTO DocumentsDTO in data.filelist)
                        {

                            if (DocumentsDTO.NCACET343F_Id > 0 && DocumentsDTO.NCACET343F_StatusFlg != "approved")
                            {
                                if (DocumentsDTO.cfilepath != null)
                                {
                                    var filesdata = _GeneralContext.NAAC_AC_344_ExtnActivities_Files_DMO.Where(t => t.NCACET343F_Id == DocumentsDTO.NCACET343F_Id).SingleOrDefault();
                                    filesdata.NCACET343F_Filedesc = DocumentsDTO.cfiledesc;
                                    filesdata.NCACET343F_FileName = DocumentsDTO.cfilename;
                                    filesdata.NCACET343F_FilePath = DocumentsDTO.cfilepath;
                                    _GeneralContext.Update(filesdata);
                                }
                            }
                            else
                            {

                                if (DocumentsDTO.NCACET343F_Id == 0)
                                {
                                    if (DocumentsDTO.cfilepath != null)
                                    {
                                        NAAC_AC_344_ExtnActivities_Files_DMO obj2 = new NAAC_AC_344_ExtnActivities_Files_DMO();
                                        obj2.NCACET343F_FileName = DocumentsDTO.cfilename;
                                        obj2.NCACET343F_Filedesc = DocumentsDTO.cfiledesc;
                                        obj2.NCACET343F_FilePath = DocumentsDTO.cfilepath;
                                        obj2.NCACET343F_StatusFlg = "";
                                        obj2.NCACET343F_ActiveFlg = true;
                                        obj2.NCACET343_Id = data.NCACET343_Id;
                                        _GeneralContext.Add(obj2);
                                    }
                                }
                            }
                        }
                    }
                    //if (data.all1 == "11")
                    //{
                        for (int e = 0; e < data.selectdStafflist.Length; e++)
                        {
                            var stfflist = _GeneralContext.NAAC_AC_344_ExtnActivities_Staff_DMO.Where(t => t.HRME_Id == data.selectdStafflist[e].HRME_Id && t.NCACET343_Id == data.NCACET343_Id).Single();
                            stfflist.NCACET344STF_Role = data.NCACET344STF_Role;
                            stfflist.NCACET344STF_UpdatedBy = data.UserId;
                            stfflist.NCACET344STF_UpdatedDate = DateTime.Now;
                            _GeneralContext.Update(stfflist);
                            if (data.filelist_staff.Length > 0)
                            {
                                if (data.filelist_staff.Length > 0)
                                {
                                    List<long> Fid = new List<long>();
                                    foreach (var item in data.filelist_staff)
                                    {
                                        if (item.NCACET344STF_Id == stfflist.NCACET344STF_Id)
                                        {
                                            Fid.Add(item.NCACET344STFF_Id);
                                        }
                                    }
                                    var removefile11 = _GeneralContext.NAAC_AC_344_ExtnActivities_Staff_Files_DMO.Where(t => t.NCACET344STF_Id == stfflist.NCACET344STF_Id && !Fid.Contains(t.NCACET344STFF_Id)).Distinct().ToList();
                                    if (removefile11.Count > 0)
                                    {
                                        foreach (var item2 in removefile11)
                                        {
                                            var deactfile = _GeneralContext.NAAC_AC_344_ExtnActivities_Staff_Files_DMO.Single(t => t.NCACET344STF_Id == stfflist.NCACET344STF_Id && t.NCACET344STFF_Id == item2.NCACET344STFF_Id);
                                            deactfile.NCACET344STFF_ActiveFlg = false;
                                            _GeneralContext.Update(deactfile);
                                            _GeneralContext.SaveChanges();
                                        }

                                    }
                                }
                                foreach (NAAC_AC_344_ExtnActivities_DTO DocumentsDTO in data.filelist_staff)
                                {

                                    if (DocumentsDTO.NCACET344STFF_Id > 0 && DocumentsDTO.NCACET343F_StatusFlg != "approved")
                                    {
                                        if (DocumentsDTO.cfilepath != null)
                                        {

                                            var filesdata = _GeneralContext.NAAC_AC_344_ExtnActivities_Staff_Files_DMO.Where(t => t.NCACET344STFF_Id == DocumentsDTO.NCACET344STFF_Id).FirstOrDefault();
                                            filesdata.NCACET344STFF_Filedesc = DocumentsDTO.cfiledesc;
                                            filesdata.NCACET344STFF_FileName = DocumentsDTO.cfilename;
                                            filesdata.NCACET344STFF_FilePath = DocumentsDTO.cfilepath;
                                            _GeneralContext.Update(filesdata);
                                        }
                                    }
                                    else
                                    {

                                        if (DocumentsDTO.NCACET344STFF_Id == 0)
                                        {
                                            if (DocumentsDTO.cfilepath != null)
                                            {
                                            NAAC_AC_344_ExtnActivities_Staff_Files_DMO obj23 = new NAAC_AC_344_ExtnActivities_Staff_Files_DMO();
                                                obj23.NCACET344STFF_FileName = DocumentsDTO.cfilename;
                                                obj23.NCACET344STFF_Filedesc = DocumentsDTO.cfiledesc;
                                                obj23.NCACET344STFF_FilePath = DocumentsDTO.cfilepath;
                                                obj23.NCACET344STFF_StatusFlg = "";
                                                obj23.NCACET344STFF_ActiveFlg = true;
                                                obj23.NCACET344STF_Id = stfflist.NCACET344STF_Id;
                                                _GeneralContext.Add(obj23);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    // }
                    // if (data.all1 == "12")
                    //{
                    for (int e = 0; e < data.selectdStudentlist.Length; e++)
                    {
                        var studentlist1 = _GeneralContext.NAAC_AC_344_ExtnActivities_Students_DMO.Where(t => t.AMCST_Id == data.selectdStudentlist[e].AMCST_Id && t.NCACET343_Id == data.NCACET343_Id).Single();
                        studentlist1.NCACET343S_Role = data.NCACET343S_Role;
                        studentlist1.NCACET343S_UpdatedBy = data.UserId;
                        studentlist1.NCACET343S_UpdatedDate = DateTime.Now;
                        _GeneralContext.Update(studentlist1);
                        if (data.filelist_student.Length > 0)
                        {
                            if (data.filelist_student.Length > 0)
                            {
                                List<long> Fid = new List<long>();
                                foreach (var item in data.filelist_student)
                                {
                                    if (item.NCACET343S_Id == studentlist1.NCACET343S_Id)
                                    {
                                        Fid.Add(item.NCACET343SF_Id);
                                    }
                                }
                                var removefile11 = _GeneralContext.NAAC_AC_344_ExtnActivities_Students_Files_DMO.Where(t => t.NCACET343S_Id == studentlist1.NCACET343S_Id && !Fid.Contains(t.NCACET343SF_Id)).Distinct().ToList();
                                if (removefile11.Count > 0)
                                {
                                    foreach (var item2 in removefile11)
                                    {
                                        var deactfile = _GeneralContext.NAAC_AC_344_ExtnActivities_Students_Files_DMO.Single(t => t.NCACET343S_Id == studentlist1.NCACET343S_Id && t.NCACET343SF_Id == item2.NCACET343SF_Id);
                                        deactfile.NCACET343SF_ActiveFlg = false;
                                        _GeneralContext.Update(deactfile);
                                        _GeneralContext.SaveChanges();
                                    }

                                }
                            }
                            foreach (NAAC_AC_344_ExtnActivities_DTO DocumentsDTO in data.filelist_student)
                            {

                                if (DocumentsDTO.NCACET343SF_Id > 0 && DocumentsDTO.NCACET343F_StatusFlg != "approved")
                                {
                                    if (DocumentsDTO.cfilepath != null)
                                    {

                                        var filesdata = _GeneralContext.NAAC_AC_344_ExtnActivities_Students_Files_DMO.Where(t => t.NCACET343SF_Id == DocumentsDTO.NCACET343SF_Id).FirstOrDefault();
                                        filesdata.NCACET343SF_Filedesc = DocumentsDTO.cfiledesc;
                                        filesdata.NCACET343SF_FileName = DocumentsDTO.cfilename;
                                        filesdata.NCACET343SF_FilePath = DocumentsDTO.cfilepath;
                                        _GeneralContext.Update(filesdata);
                                    }
                                }
                                else
                                {

                                    if (DocumentsDTO.NCACET343SF_Id == 0)
                                    {
                                        if (DocumentsDTO.cfilepath != null)
                                        {
                                            NAAC_AC_344_ExtnActivities_Students_Files_DMO obj23 = new NAAC_AC_344_ExtnActivities_Students_Files_DMO();
                                            obj23.NCACET343SF_FileName = DocumentsDTO.cfilename;
                                            obj23.NCACET343SF_Filedesc = DocumentsDTO.cfiledesc;
                                            obj23.NCACET343SF_FilePath = DocumentsDTO.cfilepath;
                                            obj23.NCACET343SF_StatusFlg = "";
                                            obj23.NCACET343SF_ActiveFlg = true;
                                            obj23.NCACET343S_Id = studentlist1.NCACET343S_Id;
                                            _GeneralContext.Add(obj23);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //}
                    
                }

                int r = _GeneralContext.SaveChanges();
                if (r > 0)
                {
                    data.msg = "update";
                }
                else
                {
                    data.msg = "no";
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_AC_344_ExtnActivities_DTO delete_student_activity(Int64 id)
        {
            NAAC_AC_344_ExtnActivities_DTO data = new NAAC_AC_344_ExtnActivities_DTO();
            try
            {
                var count_student = _GeneralContext.NAAC_AC_344_ExtnActivities_Students_DMO.Where(t => t.NCACET343_Id == id).ToList();
                List<long> count_student_Activity_ids = new List<long>();
                if (count_student.Count > 0)
                {
                    foreach (var item in count_student)
                    {
                        count_student_Activity_ids.Add(item.NCACET343S_Id);
                    }
                }
                var count_student_activity_files = (from a in _GeneralContext.NAAC_AC_344_ExtnActivities_Students_Files_DMO
                                                    where (count_student_Activity_ids.Contains(a.NCACET343S_Id))
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
        public NAAC_AC_344_ExtnActivities_DTO delete_staff_activity(Int64 id)
        {
            NAAC_AC_344_ExtnActivities_DTO data = new NAAC_AC_344_ExtnActivities_DTO();
            try
            {
                var count_staff = _GeneralContext.NAAC_AC_344_ExtnActivities_Staff_DMO.Where(t => t.NCACET343_Id == id).ToList();
                List<long> count_staff_Activity_ids = new List<long>();
                if (count_staff.Count > 0)
                {
                    foreach (var item in count_staff)
                    {
                        count_staff_Activity_ids.Add(item.NCACET344STF_Id);
                    }
                }
                var count_staff_activity_files = (from a in _GeneralContext.NAAC_AC_344_ExtnActivities_Staff_Files_DMO
                                                  where (count_staff_Activity_ids.Contains(a.NCACET344STF_Id))
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
        public NAAC_AC_344_ExtnActivities_DTO deactiveStudent(NAAC_AC_344_ExtnActivities_DTO data)
        {
            try
            {
                var y = _GeneralContext.NAAC_AC_344_ExtnActivities_DMO.Where(t => t.NCACET343_Id == data.NCACET343_Id).SingleOrDefault();
                if (y.NCACET343_ActiveFlg == true)
                {
                    y.NCACET343_ActiveFlg = false;
                }
                else if (y.NCACET343_ActiveFlg == false)
                {
                    y.NCACET343_ActiveFlg = true;
                }
                y.NCACET343_UpdatedBy = data.UserId;
                y.NCACET343_UpdatedDate = DateTime.Now;
                y.MI_Id = data.MI_Id;
                _GeneralContext.Update(y);
                int d = _GeneralContext.SaveChanges();
                if (d > 0)
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
        public NAAC_AC_344_ExtnActivities_DTO EditData(NAAC_AC_344_ExtnActivities_DTO data)
        {
            try
            {
                var editlist = (from a in _GeneralContext.Academic
                                from b in _GeneralContext.NAAC_AC_344_ExtnActivities_DMO
                                from c in _GeneralContext.NAAC_AC_344_ExtnActivities_Students_DMO
                                where (b.NCACET343_Id == data.NCACET343_Id && a.MI_Id == data.MI_Id
                                && a.MI_Id == b.MI_Id && b.NCACET343_Year == a.ASMAY_Id
                                && c.NCACET343_Id == b.NCACET343_Id)
                                select new NAAC_AC_344_ExtnActivities_DTO
                                {
                                    NCACET343_Id = b.NCACET343_Id,
                                    NCACET343_TypeOfActivity = b.NCACET343_TypeOfActivity,
                                    NCACET343_SchemeName = b.NCACET343_SchemeName,
                                    NCACET343_ActivityDate = Convert.ToDateTime(b.NCACET343_ActivityDate),
                                    NCACET343_OrgAgency = b.NCACET343_OrgAgency,
                                    NCACET343_Place = b.NCACET343_Place,
                                    NCACET343_Duration = b.NCACET343_Duration,
                                    NCACET343_NoOfStudents = b.NCACET343_NoOfStudents,
                                    ASMAY_Year = a.ASMAY_Year,
                                    NCACET343S_Role = c.NCACET343S_Role,
                                    AMCST_Id = c.AMCST_Id,
                                    MI_Id = data.MI_Id,
                                    ASMAY_Id = a.ASMAY_Id,
                                    NCACET343_Year = b.NCACET343_Year,
                                    NCACET343_NoOEmployee = b.NCACET343_NoOEmployee,
                                }).Distinct().ToList();
                data.editlist = editlist.ToArray();

                if (editlist[0].AMCST_Id != 0)
                {
                    var ee = (from a in _GeneralContext.Adm_College_Yearly_StudentDMO
                              where (a.AMCST_Id == editlist[0].AMCST_Id && a.ASMAY_Id == editlist[0].ASMAY_Id)
                              select new NAAC_AC_344_ExtnActivities_DTO
                              {
                                  AMCO_Id = a.AMCO_Id,
                                  AMB_Id = a.AMB_Id,
                                  AMSE_Id = a.AMSE_Id,
                                  ACMS_Id = a.ACMS_Id,
                                  ASMAY_Id = a.ASMAY_Id,
                              }).Distinct().ToList();

                    data.AMCO_Id = ee[0].AMCO_Id;
                    data.AMB_Id = ee[0].AMB_Id;
                    data.AMSE_Id = ee[0].AMSE_Id;
                    data.ACMS_Id = ee[0].ACMS_Id;
                    data.ASMAY_Id = ee[0].ASMAY_Id;
                    data.branchlist = (from a in _GeneralContext.ClgMasterBranchDMO
                                       from b in _GeneralContext.CLG_Adm_College_AY_CourseDMO
                                       from c in _GeneralContext.CLG_Adm_College_AY_Course_BranchDMO
                                       where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && a.AMB_ActiveFlag == true && b.ACAYC_ActiveFlag == true && c.ACAYCB_ActiveFlag == true && a.AMB_Id == c.AMB_Id && b.AMCO_Id == data.AMCO_Id && b.ASMAY_Id == data.ASMAY_Id && c.ACAYC_Id == b.ACAYC_Id)
                                       select new NAAC_AC_344_ExtnActivities_DTO
                                       {
                                           AMB_Id = a.AMB_Id,
                                           AMB_BranchName = a.AMB_BranchName,
                                       }).Distinct().OrderByDescending(t => t.AMCO_Id).ToArray();
                    data.semisterlist = (from a in _GeneralContext.CLG_Adm_Master_SemesterDMO
                                         from b in _GeneralContext.CLG_Adm_College_AY_CourseDMO
                                         from c in _GeneralContext.CLG_Adm_College_AY_Course_BranchDMO
                                         from d in _GeneralContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                         where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && a.AMSE_ActiveFlg == true && b.ACAYC_ActiveFlag == true && c.ACAYCB_ActiveFlag == true && d.ACAYCBS_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == data.AMCO_Id && b.ACAYC_Id == c.ACAYC_Id && c.AMB_Id == data.AMB_Id && c.ACAYCB_Id == d.ACAYCB_Id && a.AMSE_Id == d.AMSE_Id)
                                         select new NAAC_AC_344_ExtnActivities_DTO
                                         {
                                             AMSE_Id = a.AMSE_Id,
                                             AMSE_SEMName = a.AMSE_SEMName,
                                             AMSE_SEMCode = a.AMSE_SEMCode,
                                         }).Distinct().OrderByDescending(t => t.AMSE_Id).ToArray();

                    data.sectionlist = (from a in _GeneralContext.Adm_College_Yearly_StudentDMO
                                        from b in _GeneralContext.Adm_College_Master_SectionDMO
                                        where (b.MI_Id == data.MI_Id && a.ACMS_Id == b.ACMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id)
                                        select new NAAC_AC_344_ExtnActivities_DTO
                                        {

                                            ACMS_Id = b.ACMS_Id,
                                            ACMS_SectionName = b.ACMS_SectionName,
                                        }).Distinct().OrderByDescending(t => t.ACMS_Id).ToArray();

                    data.studentlist = (from m in _GeneralContext.Adm_Master_College_StudentDMO
                                        from n in _GeneralContext.Adm_College_Yearly_StudentDMO
                                        where m.AMCST_Id == n.AMCST_Id && m.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && m.AMCST_SOL.Equals("S") && m.AMCST_ActiveFlag == true && n.ACYST_ActiveFlag == 1 && n.AMCO_Id == data.AMCO_Id && n.AMB_Id == data.AMB_Id && n.AMSE_Id == data.AMSE_Id && n.ACMS_Id == data.ACMS_Id
                                        select new NAAC_AC_344_ExtnActivities_DTO
                                        {
                                            AMCST_Id = m.AMCST_Id,
                                            MI_Id = m.MI_Id,
                                            ASMAY_Id = m.ASMAY_Id,
                                            AMCST_FirstName = ((m.AMCST_FirstName == null ? " " : m.AMCST_FirstName) + " " + (m.AMCST_MiddleName == null ? " " : m.AMCST_MiddleName) + " " + (m.AMCST_LastName == null ? " " : m.AMCST_LastName)).Trim(),
                                            AMCST_MiddleName = m.AMCST_MiddleName,
                                            AMCST_LastName = m.AMCST_LastName,
                                            AMCST_AdmNo = m.AMCST_AdmNo
                                        }).Distinct().OrderBy(t => t.AMCST_FirstName).ToArray();

                    data.AMCO_Id = ee[0].AMCO_Id;
                    data.AMB_Id = ee[0].AMB_Id;
                    data.AMSE_Id = ee[0].AMSE_Id;
                    data.ACMS_Id = ee[0].ACMS_Id;

                    data.editMainSActFileslist = (from t in _GeneralContext.NAAC_AC_344_ExtnActivities_Files_DMO
                                                  from b in _GeneralContext.NAAC_AC_344_ExtnActivities_DMO
                                                  where (t.NCACET343_Id == data.NCACET343_Id && t.NCACET343_Id == b.NCACET343_Id
                                                  && b.MI_Id == data.MI_Id&&t.NCACET343F_ActiveFlg==true)
                                                  select new NAAC_AC_344_ExtnActivities_DTO
                                                  {
                                                      NCACET343F_Id = t.NCACET343F_Id,
                                                      NCACET343_Id = t.NCACET343_Id,
                                                      cfilename = t.NCACET343F_FileName,
                                                      cfilepath = t.NCACET343F_FilePath,
                                                      cfiledesc = t.NCACET343F_Filedesc,
                                                      NCACET343F_StatusFlg = t.NCACET343F_StatusFlg,
                                                  }).Distinct().ToArray();

                    List<long> student_tableid = new List<long>();
                    var filter_studenttableid = (from a in _GeneralContext.NAAC_AC_344_ExtnActivities_DMO
                                                 from b in _GeneralContext.NAAC_AC_344_ExtnActivities_Students_DMO
                                                 where (a.NCACET343_Id == data.NCACET343_Id && a.MI_Id == data.MI_Id
                                                 && a.NCACET343_Id == b.NCACET343_Id)
                                                 select b).ToList();
                    if (filter_studenttableid.Count > 0)
                    {
                        foreach (var item in filter_studenttableid)
                        {
                            student_tableid.Add(item.NCACET343S_Id);
                        }
                    }
                    data.editStudentActFileslist = (from t in _GeneralContext.NAAC_AC_344_ExtnActivities_Students_Files_DMO
                                                    from b in _GeneralContext.NAAC_AC_344_ExtnActivities_Students_DMO
                                                    where (student_tableid.Contains(t.NCACET343S_Id) && t.NCACET343S_Id == b.NCACET343S_Id&&t.NCACET343SF_ActiveFlg==true)
                                                    select new NAAC_AC_344_ExtnActivities_DTO
                                                    {
                                                        NCACET343S_Id = t.NCACET343S_Id,
                                                        NCACET343SF_Id = t.NCACET343SF_Id,
                                                        cfilename = t.NCACET343SF_FileName,
                                                        cfilepath = t.NCACET343SF_FilePath,
                                                        cfiledesc = t.NCACET343SF_Filedesc,
                                                        NCACET343SF_StatusFlg = t.NCACET343SF_StatusFlg,
                                                    }).Distinct().ToArray();
                }


                var empdata = (from a in _GeneralContext.NAAC_AC_344_ExtnActivities_DMO
                               from b in _GeneralContext.NAAC_AC_344_ExtnActivities_Staff_DMO
                               where (b.NCACET343_Id == data.NCACET343_Id && a.MI_Id == data.MI_Id
                               && b.NCACET343_Id == a.NCACET343_Id)
                               select new NAAC_AC_344_ExtnActivities_DTO
                               {
                                   HRME_Id = b.HRME_Id,
                                   NCACET344STF_Role = b.NCACET344STF_Role
                               }).Distinct().ToList();

                if (empdata[0].HRME_Id != 0)
                {
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
                                            where (empdept.Contains(a.HRMD_Id) && b.MI_Id.Equals(data.MI_Id)
                                            && a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true
                                            && a.HRMDES_Id == b.HRMDES_Id && b.HRMDES_ActiveFlag == true)
                                            select b).Distinct().ToArray();

                    data.stafftlist = (from a in _GeneralContext.HR_Master_Employee_DMO.Where
                                       (a => a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true
                                         && empdes.Contains(a.HRMDES_Id) && empdept.Contains(a.HRMD_Id))
                                       select new NAAC_AC_344_ExtnActivities_DTO
                                       {
                                           HRME_Id = a.HRME_Id,
                                           MI_Id = a.MI_Id,
                                           empname = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                           HRME_EmployeeOrder = a.HRME_EmployeeOrder
                                       }).Distinct().OrderBy(t => t.HRME_EmployeeOrder).ToArray();

                    //data.editMainSActFileslist = (from t in _GeneralContext.NAAC_AC_344_ExtnActivities_Staff_Files_DMO
                    //                              from b in _GeneralContext.NAAC_AC_344_ExtnActivities_Staff_DMO
                    //                              where (b.NCACET343_Id == data.NCACET343_Id
                    //                              && t.NCACET344STF_Id == b.NCACET344STF_Id)
                    //                              select new NAAC_AC_344_ExtnActivities_DTO
                    //                              {
                    //                                  cfilename = t.NCACET344STFF_FileName,
                    //                                  cfilepath = t.NCACET344STFF_FilePath,
                    //                                  cfiledesc = t.NCACET344STFF_Filedesc,
                    //                              }).Distinct().ToArray();

                    List<long> stafft_tableid = new List<long>();
                    var filter_stafftableid = (from a in _GeneralContext.NAAC_AC_344_ExtnActivities_DMO
                                               from b in _GeneralContext.NAAC_AC_344_ExtnActivities_Staff_DMO
                                               where (a.NCACET343_Id == data.NCACET343_Id && a.MI_Id == data.MI_Id && a.NCACET343_Id == b.NCACET343_Id)
                                               select b).ToList();
                    if (filter_stafftableid.Count > 0)
                    {
                        foreach (var item in filter_stafftableid)
                        {
                            stafft_tableid.Add(item.NCACET344STF_Id);
                        }
                    }
                    // change
                    data.editStaffActFileslist = (from t in _GeneralContext.NAAC_AC_344_ExtnActivities_Staff_Files_DMO
                                                  from b in _GeneralContext.NAAC_AC_344_ExtnActivities_Staff_DMO
                                                  where (stafft_tableid.Contains(t.NCACET344STF_Id)
                                                  && t.NCACET344STF_Id == b.NCACET344STF_Id&&t.NCACET344STFF_ActiveFlg==true)
                                                  select new NAAC_AC_344_ExtnActivities_DTO
                                                  {
                                                      NCACET344STF_Id = t.NCACET344STF_Id,
                                                      NCACET344STFF_Id = t.NCACET344STFF_Id,
                                                      cfilename = t.NCACET344STFF_FileName,
                                                      cfilepath = t.NCACET344STFF_FilePath,
                                                      cfiledesc = t.NCACET344STFF_Filedesc,
                                                      NCACET344STFF_StatusFlg = t.NCACET344STFF_StatusFlg, 
                                                  }).Distinct().ToArray();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public async Task<NAAC_AC_344_ExtnActivities_DTO> get_MappedStudent(NAAC_AC_344_ExtnActivities_DTO data)
        {
            try
            {
                using (var cmd = _GeneralContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_GET_MODALSTUDENTDATA_344";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@NCACET343_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.NCACET343_Id
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
                        data.mappedstudentlist = retObject.ToArray();
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
        public NAAC_AC_344_ExtnActivities_DTO deactive_student(NAAC_AC_344_ExtnActivities_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_344_ExtnActivities_Students_DMO.Where(t => t.NCACET343S_Id == data.NCACET343S_Id).Single();

                if (result.NCACET343S_ActiveFlg == true)
                {
                    result.NCACET343S_ActiveFlg = false;
                }
                else if (result.NCACET343S_ActiveFlg == false)
                {
                    result.NCACET343S_ActiveFlg = true;
                }
                result.NCACET343S_UpdatedDate = DateTime.Now;
                result.NCACET343S_UpdatedBy = data.UserId;

                _GeneralContext.Update(result);

                int s = _GeneralContext.SaveChanges();
                if (s > 0)
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
        public NAAC_AC_344_ExtnActivities_DTO viewdocument_MainActUploadFiles(NAAC_AC_344_ExtnActivities_DTO data)
        {
            try
            {
                data.viewdocument_MainActUploadFiles = (from t in _GeneralContext.NAAC_AC_344_ExtnActivities_Files_DMO
                                                        from b in _GeneralContext.NAAC_AC_344_ExtnActivities_DMO
                                                        where (t.NCACET343_Id == data.NCACET343_Id && t.NCACET343_Id == b.NCACET343_Id
                                                        && b.MI_Id == data.MI_Id&&t.NCACET343F_ActiveFlg==true)
                                                        select new NAAC_AC_344_ExtnActivities_DTO
                                                        {
                                                            cfilename = t.NCACET343F_FileName,
                                                            cfilepath = t.NCACET343F_FilePath,
                                                            cfiledesc = t.NCACET343F_Filedesc,
                                                            NCACET343F_Id = t.NCACET343F_Id,
                                                            NCACET343_Id = b.NCACET343_Id,
                                                            NCACET343F_StatusFlg = t.NCACET343F_StatusFlg,
                                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NAAC_AC_344_ExtnActivities_DTO delete_MainActUploadFiles(NAAC_AC_344_ExtnActivities_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_344_ExtnActivities_Files_DMO.Where(t => t.NCACET343F_Id == data.NCACET343F_Id).SingleOrDefault();

                result.NCACET343F_ActiveFlg = false;
                _GeneralContext.Update(result);
                int row = _GeneralContext.SaveChanges();
                if (row > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                data.viewdocument_MainActUploadFiles = (from t in _GeneralContext.NAAC_AC_344_ExtnActivities_Files_DMO

                                                        where (t.NCACET343_Id == data.NCACET343_Id&&t.NCACET343F_ActiveFlg==true)
                                                        select new NAAC_AC_344_ExtnActivities_DTO
                                                        {
                                                            cfilename = t.NCACET343F_FileName,
                                                            cfilepath = t.NCACET343F_FilePath,
                                                            cfiledesc = t.NCACET343F_Filedesc,
                                                            NCACET343F_Id = t.NCACET343F_Id,
                                                            NCACET343_Id = t.NCACET343_Id,
                                                            NCACET343F_StatusFlg = t.NCACET343F_StatusFlg,
                                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NAAC_AC_344_ExtnActivities_DTO viewdocument_StudentActUploadFiles(NAAC_AC_344_ExtnActivities_DTO data)
        {
            try
            {
                data.viewdocument_StudentActUploadFiles = (from t in _GeneralContext.NAAC_AC_344_ExtnActivities_Students_Files_DMO

                                                           where (t.NCACET343S_Id == data.NCACET343S_Id&&t.NCACET343SF_ActiveFlg==true)
                                                           select new NAAC_AC_344_ExtnActivities_DTO
                                                           {
                                                               cfilename = t.NCACET343SF_FileName,
                                                               cfilepath = t.NCACET343SF_FilePath,
                                                               cfiledesc = t.NCACET343SF_Filedesc,
                                                               NCACET343SF_Id = t.NCACET343SF_Id,
                                                               NCACET343S_Id = t.NCACET343S_Id,
                                                               NCACET343SF_StatusFlg = t.NCACET343SF_StatusFlg,
                                                           }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NAAC_AC_344_ExtnActivities_DTO delete_StudentActUploadFiles(NAAC_AC_344_ExtnActivities_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_344_ExtnActivities_Students_Files_DMO.Where(t => t.NCACET343SF_Id == data.NCACET343SF_Id).SingleOrDefault();
                result.NCACET343SF_ActiveFlg = false;
                _GeneralContext.Update(result);
                _GeneralContext.SaveChanges();


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
                data.viewdocument_StudentActUploadFiles = (from t in _GeneralContext.NAAC_AC_344_ExtnActivities_Students_Files_DMO

                                                           where (t.NCACET343S_Id == data.NCACET343S_Id&&t.NCACET343SF_ActiveFlg==true)
                                                           select new NAAC_AC_344_ExtnActivities_DTO
                                                           {
                                                               cfilename = t.NCACET343SF_FileName,
                                                               cfilepath = t.NCACET343SF_FilePath,
                                                               cfiledesc = t.NCACET343SF_Filedesc,
                                                               NCACET343SF_Id = t.NCACET343SF_Id,
                                                               NCACET343S_Id = t.NCACET343S_Id,
                                                               NCACET343SF_StatusFlg = t.NCACET343SF_StatusFlg,
                                                           }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NAAC_AC_344_ExtnActivities_DTO get_Designation(NAAC_AC_344_ExtnActivities_DTO data)
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
        public NAAC_AC_344_ExtnActivities_DTO get_Employee(NAAC_AC_344_ExtnActivities_DTO data)
        {
            try
            {
                data.stafftlist = (from a in _GeneralContext.HR_Master_Employee_DMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true
                                   && a.HRMDES_Id == data.HRMDES_Id && a.HRMD_Id == data.HRMD_Id)
                                   select new NAAC_AC_344_ExtnActivities_DTO
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
        public NAAC_AC_344_ExtnActivities_DTO viewdocument_StaffActUploadFiles(NAAC_AC_344_ExtnActivities_DTO data)
        {
            try
            {
                data.viewdocument_StaffActUploadFiles =
                    (from t in _GeneralContext.NAAC_AC_344_ExtnActivities_Staff_Files_DMO
                     where (t.NCACET344STF_Id == data.NCACET344STF_Id&&t.NCACET344STFF_ActiveFlg==true)
                     select new NAAC_AC_344_ExtnActivities_DTO
                     {
                         cfilename = t.NCACET344STFF_FileName,
                         cfilepath = t.NCACET344STFF_FilePath,
                         cfiledesc = t.NCACET344STFF_Filedesc,
                         NCACET344STF_Id = t.NCACET344STF_Id,
                         NCACET344STFF_Id = t.NCACET344STFF_Id,
                         NCACET344STFF_StatusFlg = t.NCACET344STFF_StatusFlg,

                     }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NAAC_AC_344_ExtnActivities_DTO delete_StaffActUploadFiles(NAAC_AC_344_ExtnActivities_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_344_ExtnActivities_Staff_Files_DMO.Where(t => t.NCACET344STFF_Id == data.NCACET344STFF_Id).SingleOrDefault();
                result.NCACET344STFF_ActiveFlg = false;
                _GeneralContext.Update(result);

                int row = _GeneralContext.SaveChanges();
                if (row > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                data.viewdocument_StaffActUploadFiles = (from t in _GeneralContext.NAAC_AC_344_ExtnActivities_Staff_Files_DMO
                                                         where (t.NCACET344STF_Id == data.NCACET344STF_Id&&t.NCACET344STFF_ActiveFlg==true)
                                                         select new NAAC_AC_344_ExtnActivities_DTO
                                                         {
                                                             cfilename = t.NCACET344STFF_FileName,
                                                             cfilepath = t.NCACET344STFF_FilePath,
                                                             cfiledesc = t.NCACET344STFF_Filedesc,
                                                             NCACET344STF_Id = t.NCACET344STF_Id,
                                                             NCACET344STFF_Id = t.NCACET344STFF_Id,
                                                             NCACET344STFF_StatusFlg = t.NCACET344STFF_StatusFlg,

                                                         }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public async Task<NAAC_AC_344_ExtnActivities_DTO> get_MappedStaff(NAAC_AC_344_ExtnActivities_DTO data)
        {
            try
            {
                using (var cmd = _GeneralContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_343_Staff_Activity_Modal_Data";// need to write this procedure
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@NCACET343_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.NCACET343_Id
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    // use null instead of {}
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
        public NAAC_AC_344_ExtnActivities_DTO deactive_staff(NAAC_AC_344_ExtnActivities_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_344_ExtnActivities_Staff_DMO.Single(t => t.NCACET344STF_Id == data.NCACET344STF_Id);

                if (result.NCACET344STF_ActiveFlg == true)
                {
                    result.NCACET344STF_ActiveFlg = false;
                }
                else if (result.NCACET344STF_ActiveFlg == false)
                {
                    result.NCACET344STF_ActiveFlg = true;
                }

                result.NCACET344STF_UpdatedBy = data.UserId;
                result.NCACET344STF_UpdatedDate = DateTime.Now;


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





        public NAAC_AC_344_ExtnActivities_DTO getcomment(NAAC_AC_344_ExtnActivities_DTO data)
        {
            try
            {
                data.commentlist = (from a in _GeneralContext.NAAC_AC_344_ExtnActivities_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCACET344C_RemarksBy == b.Id && a.NCACET343_Id == data.NCACET343_Id)
                                    select new NAAC_AC_344_ExtnActivities_DTO
                                    {
                                        NCACET344C_Remarks = a.NCACET344C_Remarks,
                                        NCACET344C_Id = a.NCACET344C_Id,
                                        NCACET344C_RemarksBy = a.NCACET344C_RemarksBy,
                                        NCACET344C_StatusFlg = a.NCACET344C_StatusFlg,
                                        NCACET344C_ActiveFlag = a.NCACET344C_ActiveFlag,
                                        NCACET344C_CreatedBy = a.NCACET344C_CreatedBy,
                                        NCACET344C_CreatedDate = a.NCACET344C_CreatedDate,
                                        NCACET344C_UpdatedBy = a.NCACET344C_UpdatedBy,
                                        NCACET344C_UpdatedDate = a.NCACET344C_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCACET344C_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        // for file
        public NAAC_AC_344_ExtnActivities_DTO getfilecomment(NAAC_AC_344_ExtnActivities_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_344_ExtnActivities_File_Comments_DMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCACET343FC_RemarksBy == b.Id && a.NCACET343F_Id == data.NCACET343F_Id)
                                     select new NAAC_AC_344_ExtnActivities_DTO
                                     {
                                         NCACET343F_Id = a.NCACET343F_Id,
                                         NCACET343FC_Remarks = a.NCACET343FC_Remarks,
                                         NCACET343FC_Id = a.NCACET343FC_Id,
                                         NCACET343FC_RemarksBy = a.NCACET343FC_RemarksBy,
                                         NCACET343FC_StatusFlg = a.NCACET343FC_StatusFlg,
                                         NCACET343FC_ActiveFlag = a.NCACET343FC_ActiveFlag,
                                         NCACET343FC_CreatedBy = a.NCACET343FC_CreatedBy,
                                         NCACET343FC_CreatedDate = a.NCACET343FC_CreatedDate,
                                         NCACET343FC_UpdatedBy = a.NCACET343FC_UpdatedBy,
                                         NCACET343FC_UpdatedDate = a.NCACET343FC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCACET343FC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_344_ExtnActivities_DTO savemedicaldatawisecomments(NAAC_AC_344_ExtnActivities_DTO data)
        {
            try
            {
                NAAC_AC_344_ExtnActivities_Comments_DMO obj1 = new NAAC_AC_344_ExtnActivities_Comments_DMO();
                obj1.NCACET344C_Remarks = data.Remarks;
                obj1.NCACET344C_RemarksBy = data.UserId;
                obj1.NCACET344C_StatusFlg = "";
                obj1.NCACET343_Id = data.filefkid;
                obj1.NCACET344C_ActiveFlag = true;
                obj1.NCACET344C_CreatedBy = data.UserId;
                obj1.NCACET344C_UpdatedBy = data.UserId;
                obj1.NCACET344C_CreatedDate = DateTime.Now;
                obj1.NCACET344C_UpdatedDate = DateTime.Now;
                _GeneralContext.Add(obj1);
                int s = _GeneralContext.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        // for file adding
        public NAAC_AC_344_ExtnActivities_DTO savefilewisecomments(NAAC_AC_344_ExtnActivities_DTO data)
        {
            try
            {
                NAAC_AC_344_ExtnActivities_File_Comments_DMO obj1 = new NAAC_AC_344_ExtnActivities_File_Comments_DMO();
                obj1.NCACET343FC_Remarks = data.Remarks;
                obj1.NCACET343FC_RemarksBy = data.UserId;
                obj1.NCACET343FC_StatusFlg = "";
                obj1.NCACET343F_Id = data.filefkid;
                obj1.NCACET343FC_ActiveFlag = true;
                obj1.NCACET343FC_CreatedBy = data.UserId;
                obj1.NCACET343FC_UpdatedBy = data.UserId;
                obj1.NCACET343FC_UpdatedDate = DateTime.Now;
                obj1.NCACET343FC_CreatedDate = DateTime.Now;
                _GeneralContext.Add(obj1);
                int s = _GeneralContext.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

    }
}
