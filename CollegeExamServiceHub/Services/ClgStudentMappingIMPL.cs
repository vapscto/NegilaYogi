using DataAccessMsSqlServerProvider.com.vapstech.College.Exam;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Exam;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Services
{
    public class ClgStudentMappingIMPL : Interfaces.ClgStudentMappingInterface
    {
        private static ConcurrentDictionary<string, Exm_Col_Studentwise_SubjectsDTO> _login =
                new ConcurrentDictionary<string, Exm_Col_Studentwise_SubjectsDTO>();


        public ClgExamContext _examcontext;
        readonly ILogger<ClgStudentMappingIMPL> _logger;
        public ClgStudentMappingIMPL(ClgExamContext ttcategory, ILogger<ClgStudentMappingIMPL> log)
        {
            _examcontext = ttcategory;
            _logger = log;
        }
        public Exm_Col_Studentwise_SubjectsDTO getdetails(int id)
        {
            Exm_Col_Studentwise_SubjectsDTO TTMC = new Exm_Col_Studentwise_SubjectsDTO();
            try
            {
                TTMC.yearlist = _examcontext.AcademicYear.Where(t => t.MI_Id == id && t.Is_Active == true).ToList().ToArray();
                TTMC.courseslist = _examcontext.MasterCourseDMO.Where(c => c.MI_Id == id && c.AMCO_ActiveFlag == true).ToList().Distinct().ToArray();
                TTMC.branchlist = _examcontext.ClgMasterBranchDMO.Where(c => c.MI_Id == id && c.AMB_ActiveFlag == true).ToList().Distinct().ToArray();
                TTMC.semisters = _examcontext.CLG_Adm_Master_SemesterDMO.Where(c => c.MI_Id == id && c.AMSE_ActiveFlg == true).ToList().Distinct().ToArray();
                TTMC.sections = _examcontext.Adm_College_Master_SectionDMO.Where(t => t.MI_Id == id && t.ACMS_ActiveFlag == true).ToList().ToArray();
                TTMC.subjectgrplist = _examcontext.col_Exm_Master_GroupDMO.Where(c => c.MI_Id == id && c.EMG_ActiveFlag == true).ToList().Distinct().ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }
        public Exm_Col_Studentwise_SubjectsDTO Studentdetails(Exm_Col_Studentwise_SubjectsDTO data)
        {
            Exm_Col_Studentwise_SubjectsDTO getdata = new Exm_Col_Studentwise_SubjectsDTO();
            try
            {
                getdata.studlist = (from a in _examcontext.Adm_Master_College_StudentDMO
                                    from b in _examcontext.Adm_College_Yearly_StudentDMO
                                    where (a.AMCST_Id == b.AMCST_Id &&
                                   b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMSE_Id == data.AMSE_Id
                                   && b.ACMS_Id == data.ACMS_Id && a.AMCST_ActiveFlag == true && a.AMCST_SOL == "S" && b.ACYST_ActiveFlag == 1)
                                    select new Exm_Col_Studentwise_SubjectsDTO
                                    {
                                        AMCST_Id = a.AMCST_Id,
                                        AMCST_Name = (((a.AMCST_FirstName == null || a.AMCST_FirstName.Trim() == "") ? "" : a.AMCST_FirstName.Trim()) + ((a.AMCST_MiddleName == null || a.AMCST_MiddleName.Trim() == "" || a.AMCST_MiddleName.Trim() == "0") ? "" : " " + a.AMCST_MiddleName.Trim()) + ((a.AMCST_LastName == null || a.AMCST_LastName.Trim() == "" || a.AMCST_LastName.Trim() == "0") ? "" : " " + a.AMCST_LastName.Trim())).Trim(),
                                        AMCST_AdmNo = a.AMCST_AdmNo,
                                        AMCST_RegistrationNo = a.AMCST_RegistrationNo

                                    }).Distinct().OrderBy(t => t.AMCST_RegistrationNo).ToArray();

                getdata.allsubject_list = (from a in _examcontext.Exm_Col_Yearly_SchemeDMO
                                           from c in _examcontext.Exm_Col_Yearly_Scheme_GroupDMO
                                           from d in _examcontext.Exm_Col_Yearly_Scheme_Group_SubjectsDMO
                                           from b in _examcontext.IVRM_Master_SubjectsDMO
                                           where (a.ECYS_Id == c.ECYS_Id && c.ECYSG_Id == d.ECYSG_Id && d.ISMS_Id == b.ISMS_Id && a.ECYS_ActiveFlag == true && b.ISMS_ActiveFlag == 1 && c.EMG_Id == data.EMG_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id)
                                           select new Exm_Col_Studentwise_SubjectsDTO
                                           {
                                               ISMS_Id = d.ISMS_Id,
                                               ISMS_SubjectName = b.ISMS_SubjectName

                                           }).Distinct().OrderBy(t => t.ISMS_Id).ToArray();

                getdata.allstudent_details = _examcontext.Exm_Col_Studentwise_SubjectsDMO.Where(s => s.MI_Id == data.MI_Id && s.ASMAY_Id == data.ASMAY_Id && s.AMCO_Id == data.AMCO_Id && s.EMG_Id == data.EMG_Id && s.AMSE_Id == data.AMSE_Id && s.AMB_Id == data.AMB_Id && s.ACMS_Id == data.ACMS_Id).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return getdata;
        }
        public Exm_Col_Studentwise_SubjectsDTO savedetails(Exm_Col_Studentwise_SubjectsDTO data)
        {
            try
            {
                if (data.get_list.Length > 0)
                {
                    var Duplicate_Count = 0;
                    var checkelectiveornot = _examcontext.Exm_Master_GroupDMO.Where(a => a.MI_Id == data.MI_Id && a.EMG_Id == data.EMG_Id).ToList();
                    for (int i = 0; i < data.get_list.Count(); i++)
                    {
                        var temp_std = data.get_list[i].amcsT_Id;

                        var list_delete = _examcontext.Exm_Col_Studentwise_SubjectsDMO.Where(s => s.MI_Id == data.MI_Id && s.ASMAY_Id == data.ASMAY_Id && s.AMCO_Id == data.AMCO_Id && s.EMG_Id == data.EMG_Id && s.AMSE_Id == data.AMSE_Id && s.AMB_Id == data.AMB_Id && s.ACMS_Id == data.ACMS_Id && s.AMCST_Id == temp_std).ToList();
                        if (list_delete.Any())
                        {
                            for (int x = 0; list_delete.Count > x; x++)
                            {
                                _examcontext.Remove(list_delete.ElementAt(x));
                                var contactExists = _examcontext.SaveChanges();
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
                        else if (list_delete.Count == 0)
                        {
                            data.returnval = true;
                        }

                        if (data.returnval == true)
                        {
                            for (int j = 0; j < data.get_list[i].sub_list.Count(); j++)
                            {
                                var temp_sub = data.get_list[i].sub_list[j].id;
                                Duplicate_Count = 0;
                                var result = _examcontext.Exm_Col_Studentwise_SubjectsDMO.Where(s => s.MI_Id == data.MI_Id && s.ASMAY_Id == data.ASMAY_Id
                                && s.AMCO_Id == data.AMCO_Id && s.EMG_Id == data.EMG_Id && s.AMSE_Id == data.AMSE_Id && s.AMB_Id == data.AMB_Id
                                && s.ACMS_Id == data.ACMS_Id && s.AMCST_Id == temp_std && s.ISMS_Id == temp_sub).ToList();
                                if (result.Count > 0)
                                {
                                    Duplicate_Count += 1;
                                }
                                if (Duplicate_Count == 0)
                                {
                                    Exm_Col_Studentwise_SubjectsDMO add = new Exm_Col_Studentwise_SubjectsDMO();
                                    add.MI_Id = data.MI_Id;
                                    add.ASMAY_Id = data.ASMAY_Id;
                                    add.AMCO_Id = data.AMCO_Id;
                                    add.AMSE_Id = data.AMSE_Id;
                                    add.ACMS_Id = data.ACMS_Id;
                                    add.AMCST_Id = temp_std;
                                    add.ISMS_Id = temp_sub;
                                    add.AMB_Id = data.AMB_Id; ;
                                    add.EMG_Id = data.EMG_Id;
                                    add.CreatedDate = DateTime.Now;
                                    add.UpdatedDate = DateTime.Now;
                                    add.ECSTSU_ActiveFlg = true;
                                    add.ECSTSU_ElectiveFlag = checkelectiveornot.FirstOrDefault().EMG_ElectiveFlg;
                                    _examcontext.Add(add);
                                    var contactExists = _examcontext.SaveChanges();
                                    if (contactExists == 1)
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
                                    data.returnduplicatestatus = "Duplicate";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return data;
        }
        public Exm_Col_Studentwise_SubjectsDTO getcourse(Exm_Col_Studentwise_SubjectsDTO data)
        {
            try
            {
                data.courseslist = (from a in _examcontext.MasterCourseDMO
                                    from b in _examcontext.CLG_Adm_College_AY_CourseDMO
                                    where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                                    select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Exm_Col_Studentwise_SubjectsDTO getbranch(Exm_Col_Studentwise_SubjectsDTO data)
        {
            try
            {
                var branchlist = (from a in _examcontext.ClgMasterBranchDMO
                                  from b in _examcontext.CLG_Adm_College_AY_CourseDMO
                                  from c in _examcontext.CLG_Adm_College_AY_Course_BranchDMO
                                  where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag)
                                  select new ClgMasterBranchDMO
                                  {
                                      AMB_Id = a.AMB_Id,
                                      AMB_BranchName = a.AMB_BranchName,
                                      AMB_BranchCode = a.AMB_BranchCode,
                                      AMB_BranchInfo = a.AMB_BranchInfo,
                                      AMB_BranchType = a.AMB_BranchType,
                                      AMB_StudentCapacity = a.AMB_StudentCapacity,
                                      AMB_Order = a.AMB_Order,
                                      AMB_AidedUnAided = a.AMB_AidedUnAided

                                  }).Distinct().ToList();
                data.branchlist = branchlist.OrderBy(t => t.AMB_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Exm_Col_Studentwise_SubjectsDTO getsemester(Exm_Col_Studentwise_SubjectsDTO data)
        {
            try
            {
                var semisterlist = (from a in _examcontext.CLG_Adm_Master_SemesterDMO
                                    from b in _examcontext.CLG_Adm_College_AY_CourseDMO
                                    from c in _examcontext.CLG_Adm_College_AY_Course_BranchDMO
                                    from d in _examcontext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
                                    select new CLG_Adm_Master_SemesterDMO
                                    {
                                        AMSE_Id = a.AMSE_Id,
                                        AMSE_SEMName = a.AMSE_SEMName,
                                        AMSE_SEMInfo = a.AMSE_SEMInfo,
                                        AMSE_SEMCode = a.AMSE_SEMCode,
                                        AMSE_SEMTypeFlag = a.AMSE_SEMTypeFlag,
                                        AMSE_SEMOrder = a.AMSE_SEMOrder,
                                        AMSE_Year = a.AMSE_Year,
                                        AMSE_EvenOdd = a.AMSE_EvenOdd
                                    }).Distinct().ToList();
                data.semisters = semisterlist.OrderBy(t => t.AMSE_SEMOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Exm_Col_Studentwise_SubjectsDTO getsection(Exm_Col_Studentwise_SubjectsDTO data)
        {
            try
            {
                data.sections = _examcontext.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag == true).OrderBy(a => a.ACMS_Order).ToArray();

                data.subjectgrplist = (from a in _examcontext.Exm_Col_Yearly_SchemeDMO
                                       from b in _examcontext.Exm_Col_Yearly_Scheme_GroupDMO
                                       from c in _examcontext.Exm_Master_GroupDMO
                                       where (a.ECYS_Id == b.ECYS_Id && b.EMG_Id == c.EMG_Id && a.AMB_Id == data.AMB_Id && a.AMCO_Id == data.AMCO_Id
                                       && a.AMSE_Id == data.AMSE_Id && a.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && a.ECYS_ActiveFlag == true
                                       && b.ECYSG_ActiveFlag == true && c.EMG_ActiveFlag == true)
                                       select c).Distinct().OrderBy(a => a.EMG_GroupName).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
