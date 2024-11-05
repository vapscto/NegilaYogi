
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
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Exam;

namespace ExamServiceHub.com.vaps.Services
{
    public class StudentMappingImpl : Interfaces.StudentMappingInterface
    {
        private static ConcurrentDictionary<string, StudentMappingDTO> _login =
         new ConcurrentDictionary<string, StudentMappingDTO>();

        private readonly ExamContext _studentmappingContext;
        public StudentMappingImpl(ExamContext studentmappingContext)
        {
            _studentmappingContext = studentmappingContext;
        }
        public StudentMappingDTO Getdetails(StudentMappingDTO data)
        {
            StudentMappingDTO getdata = new StudentMappingDTO();
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _studentmappingContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                getdata.yearlist = list.ToArray();

                List<Exm_Master_CategoryDMO> clist = new List<Exm_Master_CategoryDMO>();
                clist = _studentmappingContext.Exm_Master_CategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.EMCA_ActiveFlag == true).ToList();
                getdata.ctlist = clist.ToArray();

                List<Exm_Master_GroupDMO> gplist = new List<Exm_Master_GroupDMO>();
                gplist = _studentmappingContext.Exm_Master_GroupDMO.Where(t => t.MI_Id == data.MI_Id && t.EMG_ActiveFlag == true && t.EMG_ElectiveFlg == true).ToList();
                getdata.grouplist = gplist.ToArray();

                List<School_M_Section> seclist = new List<School_M_Section>();
                seclist = _studentmappingContext.School_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).OrderBy(t => t.ASMC_Order).ToList();
                getdata.seclist = seclist.ToArray();

                List<AdmissionClass> admlist = new List<AdmissionClass>();
                admlist = _studentmappingContext.AdmissionClass.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).OrderBy(t => t.ASMCL_Order).ToList();
                getdata.classlist = admlist.ToArray();

                //List<StudentMappingDMO> tablist = new List<StudentMappingDMO>();
                //tablist = _studentmappingContext.StudentMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ESTSU_ElecetiveFlag == true).ToList();
                getdata.studmaplist = (from a in _studentmappingContext.StudentMappingDMO
                                       from b in _studentmappingContext.AdmissionClass
                                       from c in _studentmappingContext.School_M_Section
                                       from d in _studentmappingContext.Adm_M_Student
                                       from e in _studentmappingContext.IVRM_School_Master_SubjectsDMO
                                       where (a.ASMCL_Id == b.ASMCL_Id && a.MI_Id == b.MI_Id && a.ASMS_Id == c.ASMS_Id && a.MI_Id == c.MI_Id && a.AMST_Id == d.AMST_Id && a.MI_Id == d.MI_Id && a.ISMS_Id == e.ISMS_Id && a.MI_Id == e.MI_Id && a.MI_Id == data.MI_Id && a.ESTSU_ElecetiveFlag == true)
                                       select new StudentMappingDTO
                                       {
                                           //ESTSU_Id=  a.ESTSU_Id,
                                           ASMCL_ClassName = b.ASMCL_ClassName,
                                           ASMC_SectionName = c.ASMC_SectionName,
                                           //AMST_FirstName= d.AMST_FirstName,
                                           AMST_FirstName = ((d.AMST_FirstName.Trim() == null || d.AMST_FirstName.Trim() == "" ? "" : d.AMST_FirstName.Trim()) + (d.AMST_MiddleName.Trim() == null || d.AMST_MiddleName.Trim() == "" || d.AMST_MiddleName.Trim() == "0" ? "" : " " + d.AMST_MiddleName.Trim()) + (d.AMST_LastName.Trim() == null || d.AMST_LastName.Trim() == "" || d.AMST_LastName.Trim() == "0" ? "" : " " + d.AMST_LastName.Trim())).Trim(),
                                           AMST_Id = d.AMST_Id,
                                           ESTSU_ActiveFlg = a.ESTSU_ActiveFlg,
                                           ESTSU_ElecetiveFlag = a.ESTSU_ElecetiveFlag,
                                           // ISMS_SubjectName= e.ISMS_SubjectName,
                                       }).Distinct().ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return getdata;

        }
        public StudentMappingDTO getcategory(StudentMappingDTO data)
        {
            StudentMappingDTO getdata = new StudentMappingDTO();
            try
            {
                getdata.ctlist = (from a in _studentmappingContext.Exm_Yearly_CategoryDMO
                                  from b in _studentmappingContext.Exm_Master_CategoryDMO
                                  where (a.EMCA_Id == b.EMCA_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.EYC_ActiveFlg == true)
                                  select new StudentMappingDTO
                                  {
                                      EMCA_Id = b.EMCA_Id,
                                      EMCA_CategoryName = b.EMCA_CategoryName

                                  }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return getdata;

        }
        public StudentMappingDTO getclassid(StudentMappingDTO data)
        {
            StudentMappingDTO getdata = new StudentMappingDTO();
            try
            {
                getdata.classlist = (from c in _studentmappingContext.Exm_Master_CategoryDMO
                                     from a in _studentmappingContext.Exm_Category_ClassDMO
                                     from b in _studentmappingContext.AdmissionClass
                                     where (a.EMCA_Id == c.EMCA_Id && a.EMCA_Id == data.EMCA_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == c.MI_Id
                                     && b.MI_Id == data.MI_Id)
                                     group c by new
                                     {
                                         a.ASMCL_Id,
                                         b.ASMCL_ClassName
                                     } into temp
                                     select new exammastercategoryDTO
                                     {
                                         ASMCL_Id = temp.Key.ASMCL_Id,
                                         ASMCL_ClassName = temp.Key.ASMCL_ClassName
                                     }).ToArray();

                getdata.grouplist = (from a in _studentmappingContext.Exm_Yearly_CategoryDMO
                                     from b in _studentmappingContext.Exm_Yearly_Category_GroupDMO
                                     from c in _studentmappingContext.Exm_Master_GroupDMO
                                     where (a.EYC_Id == b.EYC_Id && a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == data.EMCA_Id && b.EMG_Id == c.EMG_Id && a.MI_Id == c.MI_Id && c.EMG_ElectiveFlg == true)
                                     select c).Distinct().ToArray();

                var getyearlycategorydetails = _studentmappingContext.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.EMCA_Id == data.EMCA_Id && a.EYC_ActiveFlg == true).ToList();

                if (getyearlycategorydetails != null && getyearlycategorydetails.Count > 0 && getyearlycategorydetails.FirstOrDefault().EYC_BasedOnPaperTypeFlg == true)
                {
                    getdata.examlist = _studentmappingContext.masterexam.Where(a => a.MI_Id == data.MI_Id
                    && a.EME_ActiveFlag == true).OrderBy(a => a.EME_ExamOrder).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return getdata;
        }
        public StudentMappingDTO getsubject(StudentMappingDTO data)
        {
            StudentMappingDTO getdata = new StudentMappingDTO();
            try
            {
                getdata.subjlist = (from a in _studentmappingContext.Exm_Master_GroupDMO
                                    from b in _studentmappingContext.Exm_Master_Group_SubjectsDMO
                                    from c in _studentmappingContext.IVRM_School_Master_SubjectsDMO
                                    where (a.EMG_Id == b.EMG_Id && b.ISMS_Id == c.ISMS_Id && a.MI_Id == c.MI_Id && a.EMG_Id == data.EMG_Id && a.MI_Id == data.MI_Id)
                                    select new StudentMappingDTO
                                    {
                                        ISMS_Id = b.ISMS_Id,
                                        ISMS_SubjectName = c.ISMS_SubjectName
                                    }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return getdata;
        }
        public StudentMappingDTO Studentdetails(StudentMappingDTO data)
        {
            StudentMappingDTO getdata = new StudentMappingDTO();
            try
            {
                List<int?> ids = new List<int?>();
                if (data.Left_Flag == true || data.Deactive_Flag == true)
                {
                    ids.Add(0);
                }

                ids.Add(1);

                List<string> sol = new List<string>();
                sol.Add("S");

                if (data.Left_Flag == true)
                {
                    sol.Add("L");
                }
                if (data.Deactive_Flag == true)
                {
                    sol.Add("D");
                }

                getdata.studlist = (from a in _studentmappingContext.School_Adm_Y_Student
                                    from b in _studentmappingContext.AdmissionClass
                                    from c in _studentmappingContext.School_M_Section
                                    from d in _studentmappingContext.AcademicYear
                                    from e in _studentmappingContext.Adm_M_Student
                                    where (a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == c.ASMS_Id && a.ASMAY_Id == d.ASMAY_Id && a.AMST_Id == e.AMST_Id
                                    && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id
                                    && sol.Contains(e.AMST_SOL) && ids.Contains(e.AMST_ActiveFlag) && ids.Contains(a.AMAY_ActiveFlag))
                                    //&& e.AMST_ActiveFlag == 1 && e.AMST_SOL == "S" && a.AMAY_ActiveFlag == 1 && e.AMST_SOL != "WD"
                                    select new StudentMappingDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = (((e.AMST_FirstName == null || e.AMST_FirstName.Trim() == "") ? "" : e.AMST_FirstName.Trim()) + ((e.AMST_MiddleName == null || e.AMST_MiddleName.Trim() == "" || e.AMST_MiddleName.Trim() == "0") ? "" :
                                        " " + e.AMST_MiddleName.Trim()) + ((e.AMST_LastName == null || e.AMST_LastName.Trim() == ""
                                        || e.AMST_LastName.Trim() == "0") ? "" : " " + e.AMST_LastName.Trim())).Trim(),
                                        AMAY_RollNo = a.AMAY_RollNo,
                                        AMST_AdmNo = e.AMST_AdmNo,
                                        AMST_SOL = e.AMST_SOL,
                                    }).Distinct().OrderBy(t => t.AMAY_RollNo).ToArray();

                var allstudent_details = _studentmappingContext.StudentMappingDMO.Where(s => s.MI_Id == data.MI_Id && s.ASMAY_Id == data.ASMAY_Id
                && s.ASMCL_Id == data.ASMCL_Id && s.ASMS_Id == data.ASMS_Id && s.EMG_Id == data.EMG_Id).Distinct().ToList();

                if (data.EME_Id > 0)
                {
                    allstudent_details = allstudent_details.Where(a => a.EME_Id == data.EME_Id).ToList();
                }

                getdata.allstudent_details = allstudent_details.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return getdata;
        }
        public StudentMappingDTO savedetails(StudentMappingDTO data)
        {
            try
            {
                if (data.get_list != null && data.get_list.Length > 0)
                {
                    var Duplicate_Count = 0;

                    for (int i = 0; i < data.get_list.Count(); i++)
                    {
                        var temp_std = data.get_list[i].amsT_Id;

                        var list_delete = _studentmappingContext.StudentMappingDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.EMG_Id == data.EMG_Id
                        && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.AMST_Id == temp_std && t.MI_Id == data.MI_Id).ToList();

                        if (data.EME_Id > 0)
                        {
                            list_delete = list_delete.Where(a => a.EME_Id == data.EME_Id).ToList();
                        }

                        if (list_delete.Any())
                        {
                            for (int x = 0; list_delete.Count > x; x++)
                            {
                                _studentmappingContext.Remove(list_delete.ElementAt(x));
                            }
                        }

                        else if (list_delete.Count == 0)
                        {
                            data.returnval = true;
                        }

                        //if (data.returnval == true)
                        //{
                        for (int j = 0; j < data.get_list[i].sub_list.Count(); j++)
                        {
                            var temp_sub = data.get_list[i].sub_list[j].id;
                            Duplicate_Count = 0;

                            //var result = _studentmappingContext.StudentMappingDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id
                            //&& t.ASMS_Id == data.ASMS_Id && t.AMST_Id == temp_std && t.ISMS_Id == temp_sub && t.MI_Id == data.MI_Id).ToList();
                            //if (result.Count > 0)
                            //{
                            //    Duplicate_Count += 1;
                            //}
                            if (Duplicate_Count == 0)
                            {
                                StudentMappingDMO add = new StudentMappingDMO
                                {
                                    MI_Id = data.MI_Id,
                                    ASMAY_Id = data.ASMAY_Id,
                                    ASMCL_Id = data.ASMCL_Id,
                                    ASMS_Id = data.ASMS_Id,
                                    AMST_Id = temp_std,
                                    ISMS_Id = temp_sub,
                                    EMG_Id = data.EMG_Id,
                                    CreatedDate = DateTime.Now,
                                    UpdatedDate = DateTime.Now,
                                    ESTSU_CreatedBy = data.UserId,
                                    ESTSU_UpdatedBy = data.UserId,
                                    ESTSU_ElecetiveFlag = true,
                                    ESTSU_ActiveFlg = true,
                                    EME_Id = data.EME_Id > 0 ? data.EME_Id : null
                                };
                                _studentmappingContext.Add(add);
                            }
                            else
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                        }
                        // }
                    }
                }

                if (data.get_Removed_list != null && data.get_Removed_list.Length > 0)
                {
                    foreach (var stu_remove in data.get_Removed_list)
                    {
                        var stu_list_delete = _studentmappingContext.StudentMappingDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id
                        && t.EMG_Id == data.EMG_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.AMST_Id == stu_remove.amsT_Id
                        && t.MI_Id == data.MI_Id).ToList();

                        if (stu_list_delete.Any())
                        {
                            for (int x = 0; stu_list_delete.Count > x; x++)
                            {
                                _studentmappingContext.Remove(stu_list_delete.ElementAt(x));
                            }
                        }
                    }
                }

                if ((data.get_list != null && data.get_list.Length > 0) || (data.get_Removed_list != null && data.get_Removed_list.Length > 0))
                {
                    var contactExists = _studentmappingContext.SaveChanges();
                    if (contactExists > 0)
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
                Console.WriteLine(ee);
            }
            return data;
        }
        public StudentMappingDTO editdetails(int ID)
        {
            StudentMappingDTO editlt = new StudentMappingDTO();
            try
            {
                List<StudentMappingDMO> edit = new List<StudentMappingDMO>();
                edit = _studentmappingContext.StudentMappingDMO.Where(t => t.AMST_Id == ID).ToList();
                editlt.editlist = edit.ToArray();

                editlt.edclasslist = (from a in _studentmappingContext.Exm_Category_ClassDMO
                                      from b in _studentmappingContext.AdmissionClass
                                      from c in _studentmappingContext.Exm_Master_CategoryDMO
                                      where (a.EMCA_Id == c.EMCA_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == c.MI_Id && b.MI_Id == edit[0].MI_Id
                                      && a.ASMCL_Id == edit[0].ASMCL_Id && a.ASMAY_Id == edit[0].ASMAY_Id)
                                      select new StudentMappingDTO
                                      {
                                          ASMCL_Id = a.ASMCL_Id,
                                          ASMCL_ClassName = b.ASMCL_ClassName,
                                          EMCA_CategoryName = c.EMCA_CategoryName,
                                          EMCA_Id = c.EMCA_Id
                                      }).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return editlt;
        }
        public StudentMappingDTO getalldetailsviewrecords(int ID)
        {
            StudentMappingDTO detalt = new StudentMappingDTO();
            try
            {
                detalt.gtdetailsview = (from a in _studentmappingContext.StudentMappingDMO
                                        from b in _studentmappingContext.IVRM_School_Master_SubjectsDMO
                                        where (a.ISMS_Id == b.ISMS_Id && a.AMST_Id == ID)
                                        select new StudentMappingDTO
                                        {
                                            ISMS_SubjectName = b.ISMS_SubjectName,
                                            ISMS_SubjectCode = b.ISMS_SubjectCode,

                                        }).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return detalt;
        }
        public StudentMappingDTO deactivate(StudentMappingDTO data)
        {
            StudentMappingDTO deact = new StudentMappingDTO();

            if (data.AMST_Id > 0)
            {
                var result = _studentmappingContext.StudentMappingDMO.Where(t => t.AMST_Id == data.AMST_Id).ToList();
                for (var i = 0; i < result.Count(); i++)
                {
                    var elcflag = result[i].ESTSU_ActiveFlg;
                    if (elcflag == true)
                    {
                        result[i].ESTSU_ActiveFlg = false;
                    }
                    else
                    {
                        result[i].ESTSU_ActiveFlg = true;
                    }

                    _studentmappingContext.Update(result[i]);
                }
                var flag = _studentmappingContext.SaveChanges();
                if (flag >= 1)
                {
                    deact.returnval = true;
                }
                else
                {
                    deact.returnval = false;
                }
            }
            return deact;
        }
        public StudentMappingDTO get_cls_sections(StudentMappingDTO id)
        {
            try
            {
                id.seclist = (from m in _studentmappingContext.Exm_Category_ClassDMO
                              from o in _studentmappingContext.School_M_Section
                              where (m.ASMCL_Id == id.ASMCL_Id && m.ASMS_Id == o.ASMS_Id
                              && o.ASMC_ActiveFlag == 1 && o.MI_Id == id.MI_Id && m.MI_Id == id.MI_Id
                              && m.ASMAY_Id == id.ASMAY_Id && m.ECAC_ActiveFlag == true && m.EMCA_Id == id.EMCA_Id)
                              select new School_M_Section
                              {
                                  ASMS_Id = o.ASMS_Id,
                                  ASMC_SectionName = o.ASMC_SectionName,
                                  ASMC_SectionCode = o.ASMC_SectionCode,
                                  ASMC_Order = o.ASMC_Order,
                                  ASMC_MaxCapacity = o.ASMC_MaxCapacity,
                                  ASMC_ActiveFlag = o.ASMC_ActiveFlag,
                              }).OrderBy(t => t.ASMC_Order).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return id;
        }
        public StudentMappingDTO OnClickRemove(StudentMappingDTO id)
        {
            try
            {
                List<long> ismsids = new List<long>();
                id.returnval = false;
                if (id.Temp_SubjectList != null && id.Temp_SubjectList.Length > 0)
                {
                    foreach (var c in id.Temp_SubjectList)
                    {
                        ismsids.Add(c.id);
                    }

                    var check_student_subject_marks = _studentmappingContext.ExamMarksDMO.Where(a => a.MI_Id == id.MI_Id && a.ASMAY_Id == id.ASMAY_Id
                    && a.ASMCL_Id == id.ASMCL_Id && a.ASMS_Id == id.ASMS_Id && a.AMST_Id == id.AMST_Id && ismsids.Contains(a.ISMS_Id)
                    && a.ESTM_ActiveFlg == true).ToList();

                    if (id.EME_Id > 0)
                    {
                        check_student_subject_marks = check_student_subject_marks.Where(a => a.EME_Id == id.EME_Id).ToList();
                    }

                    if (check_student_subject_marks.Count > 0)
                    {
                        id.returnval = true;
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return id;
        }

        //Student Wise Question Paper Type Mapping
        public StudentMappingDTO BindData_PT(StudentMappingDTO data)
        {
            try
            {
                data.yearlist = _studentmappingContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.classlist = OnChangeYear_GetClass_PT(data).classlist;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentMappingDTO OnChangeYear_GetClass_PT(StudentMappingDTO data)
        {
            try
            {
                data.classlist = (from a in _studentmappingContext.Exm_Category_ClassDMO
                                  from b in _studentmappingContext.AdmissionClass
                                  where (a.ASMCL_Id == b.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ECAC_ActiveFlag == true)
                                  select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentMappingDTO OnChangeClass_GetSection_PT(StudentMappingDTO data)
        {
            try
            {
                data.seclist = (from a in _studentmappingContext.Exm_Category_ClassDMO
                                from b in _studentmappingContext.AdmissionClass
                                from c in _studentmappingContext.School_M_Section
                                where (a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == c.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                && a.ECAC_ActiveFlag == true && a.ASMCL_Id == data.ASMCL_Id)
                                select c).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentMappingDTO OnChangeSection_GetExam_PT(StudentMappingDTO data)
        {
            try
            {
                var catid = _studentmappingContext.Exm_Category_ClassDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id
                && t.ASMS_Id == data.ASMS_Id && t.MI_Id == data.MI_Id && t.ECAC_ActiveFlag == true).Select(t => t.EMCA_Id).ToArray();

                var eycid = _studentmappingContext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id
                && catid.Contains(t.EMCA_Id) && t.EYC_ActiveFlg == true).Select(t => t.EYC_Id).ToArray();

                var emeid = _studentmappingContext.Exm_Yearly_Category_ExamsDMO.Where(t => eycid.Contains(t.EYC_Id)
                && t.EYCE_ActiveFlg == true).Select(t => t.EME_Id).ToArray();

                List<exammasterDMO> examlist = new List<exammasterDMO>();
                examlist = _studentmappingContext.exammasterDMO.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true && emeid.Contains(t.EME_Id)).ToList();
                data.examlist = examlist.Distinct().OrderBy(t => t.EME_ExamOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentMappingDTO OnChangeExam_GetSubject_PT(StudentMappingDTO data)
        {
            try
            {
                var catid = _studentmappingContext.Exm_Category_ClassDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id
                && t.ASMS_Id == data.ASMS_Id && t.MI_Id == data.MI_Id && t.ECAC_ActiveFlag == true).Select(t => t.EMCA_Id).ToArray();

                var eycid = _studentmappingContext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id
                && catid.Contains(t.EMCA_Id) && t.EYC_ActiveFlg == true).Select(t => t.EYC_Id).ToArray();

                data.subjlist = (from a in _studentmappingContext.Exm_Yearly_Category_ExamsDMO
                                 from b in _studentmappingContext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                 from c in _studentmappingContext.IVRM_School_Master_SubjectsDMO
                                 where (a.EYCE_Id == b.EYCE_Id && b.ISMS_Id == c.ISMS_Id && a.EYCE_ActiveFlg == true && b.EYCES_ActiveFlg == true
                                 && a.EME_Id == data.EME_Id && eycid.Contains(a.EYC_Id))
                                 select c).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentMappingDTO OnSearch_PT(StudentMappingDTO data)
        {
            try
            {
                string order = "AMST_FirstName";
                var get_configuration = _studentmappingContext.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
                data.configuration = get_configuration.ToArray();

                if (get_configuration != null && get_configuration.Count > 0)
                {
                    if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "Name")
                    {
                        order = "AMST_FirstName";
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
                }

                List<int?> ids = new List<int?>();
                if (data.Left_Flag == true || data.Deactive_Flag == true)
                {
                    ids.Add(0);
                }

                ids.Add(1);

                List<string> sol = new List<string>();
                sol.Add("S");

                if (data.Left_Flag == true)
                {
                    sol.Add("L");
                }
                if (data.Deactive_Flag == true)
                {
                    sol.Add("D");
                }

                List<StudentMappingDTO> studentList = new List<StudentMappingDTO>();
                studentList = (from a in _studentmappingContext.School_Adm_Y_StudentDMO
                               from b in _studentmappingContext.Adm_M_Student
                               from c in _studentmappingContext.AcademicYear
                               from d in _studentmappingContext.AdmissionClass
                               from e in _studentmappingContext.School_M_Section
                               where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                               && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && b.MI_Id == data.MI_Id
                               && sol.Contains(b.AMST_SOL) && ids.Contains(b.AMST_ActiveFlag) && ids.Contains(a.AMAY_ActiveFlag))
                               select new StudentMappingDTO
                               {
                                   AMST_Id = a.AMST_Id,
                                   AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "" ? "" : b.AMST_FirstName) +
                                   (b.AMST_MiddleName == null || b.AMST_MiddleName == "" ? "" : " " + b.AMST_MiddleName) +
                                   (b.AMST_LastName == null || b.AMST_LastName == "" ? "" : " " + b.AMST_LastName)).Trim(),
                                   AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                                   AMST_RegistrationNo = b.AMST_RegistrationNo,
                                   AMAY_RollNo = a.AMAY_RollNo,
                                   AMST_SOL = b.AMST_SOL
                               }).Distinct().ToList();

                var propertyInfo = typeof(StudentMappingDTO).GetProperty(order);
                studentList = studentList.OrderBy(x => propertyInfo.GetValue(x, null)).ToList();
                data.studlist = studentList.ToArray();

                data.studmaplist = _studentmappingContext.Exm_Student_Examwise_PTDMO.Where(a => a.MI_Id == data.MI_Id
                && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id
                && a.ISMS_Id == data.ISMS_Id).ToArray();

                data.question_papertype_list = _studentmappingContext.Exm_Master_PaperTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.EMPATY_ActiveFlag == true).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentMappingDTO OnSave_PT(StudentMappingDTO data)
        {
            try
            {
                data.returnval = false;

                //Save Or Update
                if (data.Selected_students != null && data.Selected_students.Length > 0)
                {
                    foreach (var c in data.Selected_students)
                    {
                        if (c.ESEWPT_Id > 0)
                        {
                            var result = _studentmappingContext.Exm_Student_Examwise_PTDMO.Single(a => a.ESEWPT_Id == c.ESEWPT_Id);
                            result.ESEWPT_Id = c.ESEWPT_Id;
                            result.ESEWPT_CreatedDate = DateTime.Now;
                            result.ESEWPT_UpdatedBy = data.UserId;
                            _studentmappingContext.Update(result);
                        }
                        else
                        {
                            Exm_Student_Examwise_PTDMO exm_Student_Examwise_PTDMO = new Exm_Student_Examwise_PTDMO
                            {
                                MI_Id = data.MI_Id,
                                ASMAY_Id = data.ASMAY_Id,
                                ASMCL_Id = data.ASMCL_Id,
                                ASMS_Id = data.ASMS_Id,
                                AMST_Id = c.AMST_Id,
                                EME_Id = Convert.ToInt32(data.EME_Id),
                                ISMS_Id = data.ISMS_Id,
                                EMPATY_Id = c.EMPATY_Id,
                                ESEWPT_ActiveFlg = true,
                                ESEWPT_CreatedBy = data.UserId,
                                ESEWPT_CreatedDate = DateTime.Now,
                                ESEWPT_UpdatedBy = data.UserId,
                                ESEWPT_UpdatedDate = DateTime.Now
                            };
                            _studentmappingContext.Add(exm_Student_Examwise_PTDMO);
                        }
                    }
                }

                //Deleting
                if (data.Selected_RemoveRecordsList != null && data.Selected_RemoveRecordsList.Length > 0)
                {
                    foreach (var stu_remove in data.Selected_RemoveRecordsList)
                    {
                        var stu_list_delete = _studentmappingContext.Exm_Student_Examwise_PTDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id
                        && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ISMS_Id == data.ISMS_Id && t.EME_Id == data.EME_Id
                        && t.AMST_Id == stu_remove.AMST_Id && t.MI_Id == data.MI_Id && t.ESEWPT_Id == stu_remove.ESEWPT_Id).ToList();

                        if (stu_list_delete.Any())
                        {
                            for (int x = 0; stu_list_delete.Count > x; x++)
                            {
                                _studentmappingContext.Remove(stu_list_delete.ElementAt(x));
                            }
                        }
                    }
                }

                if ((data.Selected_students != null && data.Selected_students.Length > 0)
                    || (data.Selected_RemoveRecordsList != null && data.Selected_RemoveRecordsList.Length > 0))
                {
                    var contactExists = _studentmappingContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.returnval = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentMappingDTO OnClickRemove_PT(StudentMappingDTO data)
        {
            try
            {
                var check_student_subject_marks = _studentmappingContext.ExamMarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                  && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == data.AMST_Id && a.ISMS_Id == data.ISMS_Id
                  && a.ESTM_ActiveFlg == true && a.EME_Id == data.EME_Id).Count();

                if (check_student_subject_marks > 0)
                {
                    data.returnval = true;
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