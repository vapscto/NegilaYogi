
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
using Microsoft.Extensions.Logging;

namespace ExamServiceHub.com.vaps.Services
{
    public class ProgressCardReportImpl : Interfaces.ProgressCardReportInterface
    {
        private static ConcurrentDictionary<string, ProgressCardReportDTO> _login =
         new ConcurrentDictionary<string, ProgressCardReportDTO>();

        private readonly ExamContext _PCReportContext;
        ILogger<ProgressCardReportImpl> _acdimpl;
        public ProgressCardReportImpl(ExamContext cpContext)
        {
            _PCReportContext = cpContext;
        }


        public ProgressCardReportDTO Getdetails(ProgressCardReportDTO data)//int IVRMM_Id
        {
            ProgressCardReportDTO getdata = new ProgressCardReportDTO();
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _PCReportContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                getdata.yearlist = list.ToArray();

                //List<Exm_Master_CategoryDMO> clist = new List<Exm_Master_CategoryDMO>();
                //clist = _PCReportContext.Exm_Master_CategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.EMCA_ActiveFlag == true).ToList();
                //getdata.ctlist = clist.ToArray();

                //List<Exm_Master_GroupDMO> gplist = new List<Exm_Master_GroupDMO>();
                //gplist = _PCReportContext.Exm_Master_GroupDMO.Where(t => t.MI_Id == data.MI_Id && t.EMG_ActiveFlag == true && t.EMG_ElectiveFlg == true).ToList();
                //getdata.grouplist = gplist.ToArray();

                List<School_M_Section> seclist = new List<School_M_Section>();
                seclist = _PCReportContext.School_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).OrderBy(t=>t.ASMC_Order).ToList();
                getdata.seclist = seclist.ToArray();

                List<AdmissionClass> admlist = new List<AdmissionClass>();
                admlist = _PCReportContext.AdmissionClass.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).OrderBy(t=>t.ASMCL_Order).ToList();
                getdata.classlist = admlist.ToArray();


                List<exammasterDMO> esmp = new List<exammasterDMO>();
                esmp = _PCReportContext.exammasterDMO.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true).OrderBy(t=>t.EME_ExamOrder).ToList();
                getdata.exmstdlist = esmp.ToArray();


                //List<StudentMappingDMO> tablist = new List<StudentMappingDMO>();
                //tablist = _PCReportContext.StudentMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ESTSU_ElecetiveFlag == true).ToList();
                //getdata.studmaplist = (from a in _PCReportContext.StudentMappingDMO
                //                       from b in _PCReportContext.AdmissionClass
                //                       from c in _PCReportContext.School_M_Section
                //                       from d in _PCReportContext.Adm_M_Student
                //                       from e in _PCReportContext.IVRM_School_Master_SubjectsDMO
                //                       where (a.ASMCL_Id == b.ASMCL_Id && a.MI_Id == b.MI_Id && a.ASMS_Id == c.ASMS_Id && a.MI_Id == c.MI_Id && a.AMST_Id == d.AMST_Id && a.MI_Id == d.MI_Id && a.ISMS_Id == e.ISMS_Id && a.MI_Id == e.MI_Id && a.MI_Id == data.MI_Id && a.ESTSU_ElecetiveFlag == true)
                //                       select new ProgressCardReportDTO
                //                       {
                //                           //ESTSU_Id=  a.ESTSU_Id,
                //                           ASMCL_ClassName = b.ASMCL_ClassName,
                //                           ASMC_SectionName = c.ASMC_SectionName,
                //                           AMST_FirstName = d.AMST_FirstName,
                //                           AMST_Id = d.AMST_Id,
                //                           ESTSU_ActiveFlg = a.ESTSU_ActiveFlg,
                //                           ESTSU_ElecetiveFlag = a.ESTSU_ElecetiveFlag,
                //                           // ISMS_SubjectName= e.ISMS_SubjectName,
                //                       }).Distinct().ToArray();



            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return getdata;

        }


        public ProgressCardReportDTO validateordernumber(ProgressCardReportDTO data)
        {
            ProgressCardReportDTO getdata = new ProgressCardReportDTO();
            //try
            //{

            //    getdata.studlist = (from a in _PCReportContext.School_Adm_Y_Student
            //                        from b in _PCReportContext.AdmissionClass
            //                        from c in _PCReportContext.School_M_Section
            //                        from d in _PCReportContext.AcademicYear
            //                        from e in _PCReportContext.Adm_M_Student
            //                        where (a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == c.ASMS_Id && a.ASMAY_Id == d.ASMAY_Id && a.AMST_Id == e.AMST_Id &&
            //                        a.ASMCL_Id ==data.ASMCL_Id && a.ASMS_Id ==data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id)
            //                        select new ProgressCardReportDTO
            //                        {
            //                            AMST_Id = a.AMST_Id,
            //                            AMST_FirstName= e.AMST_FirstName
            //                        }).ToArray();
            //}
            //catch (Exception ex)
            //{
            //     _acdimpl.LogError(ex.Message);
            // _acdimpl.LogDebug(ex.Message);
            //}
            return getdata;
        }

        public ProgressCardReportDTO savedetails(ProgressCardReportDTO data)
        {

            //   ProgressCardReportDTO savedata = new ProgressCardReportDTO();
            try
            {
                data.clstchname = (from a in _PCReportContext.ClassTeacherMappingDMO
                                   from b in _PCReportContext.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                   select new BaldwinAllReportDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = b.HRME_EmployeeFirstName
                                   }).Distinct().ToArray();

                data.savelist = (from a in _PCReportContext.ExmStudentMarksProcessSubjectwiseDMO
                                 from b in _PCReportContext.AdmissionClass
                                 from c in _PCReportContext.exammasterDMO
                                 from d in _PCReportContext.IVRM_School_Master_SubjectsDMO
                                 from e in _PCReportContext.School_M_Section
                                 from f in _PCReportContext.Adm_M_Student
                                     //    from g in _PCReportContext.ExmStudentMarksProcessDMO
                                 from h in _PCReportContext.School_Adm_Y_Student
                                 where (a.ASMCL_Id == b.ASMCL_Id && a.ASMCL_Id == data.ASMCL_Id && a.EME_Id == c.EME_Id && a.ISMS_Id == d.ISMS_Id && a.ASMS_Id == e.ASMS_Id && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == f.AMST_Id && a.MI_Id == data.MI_Id && a.EME_Id == data.EME_Id && h.ASMAY_Id == data.ASMAY_Id && h.ASMCL_Id == data.ASMCL_Id && h.ASMS_Id == data.ASMS_Id && h.AMST_Id == a.AMST_Id)
                                 select new ProgressCardReportDTO
                                 {
                                     ESTMPS_ObtainedMarks = a.ESTMPS_ObtainedMarks,
                                     ESTMPS_ObtainedGrade = a.ESTMPS_ObtainedGrade,
                                     ESTMPS_PassFailFlg = a.ESTMPS_PassFailFlg,
                                     //ESTMP_TotalMaxMarks=g.ESTMP_TotalMaxMarks,
                                     //ESTMP_TotalGrade=g.ESTMP_TotalGrade,
                                     //ESTMP_TotalObtMarks = g.ESTMP_TotalObtMarks, a.EME_Id==g.EME_Id && a.AMST_Id== g.AMST_Id && c.EME_Id==g.EME_Id &&
                                     EME_ExamName = c.EME_ExamName,
                                     ASMCL_ClassName = b.ASMCL_ClassName,
                                     ASMC_SectionName = e.ASMC_SectionName,
                                     AMST_Id = f.AMST_Id,
                                     AMST_FirstName = f.AMST_FirstName,
                                     AMST_DOB = f.AMST_DOB,
                                     AMAY_RollNo = h.AMAY_RollNo,
                                     AMST_AdmNo = f.AMST_AdmNo,
                                     ISMS_Id = d.ISMS_Id,
                                     ISMS_SubjectName = d.ISMS_SubjectName,
                                     ESTMPS_MaxMarks = a.ESTMPS_MaxMarks,
                                     //  ESTMP_Percentage = g.ESTMP_Percentage,
                                 }).Distinct().ToArray();

                data.savelisttot = _PCReportContext.ExmStudentMarksProcessDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.MI_Id == data.MI_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id).Distinct().ToArray();


                data.subjlist = (from a in _PCReportContext.Exm_Category_ClassDMO
                                 from b in _PCReportContext.Exm_Yearly_CategoryDMO
                                 from c in _PCReportContext.Exm_Yearly_Category_ExamsDMO
                                 from d in _PCReportContext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                 from e in _PCReportContext.IVRM_School_Master_SubjectsDMO
                                 where (a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EMCA_Id == b.EMCA_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.EYC_Id == b.EYC_Id && c.EME_Id == data.EME_Id && d.EYCE_Id == c.EYCE_Id && d.EYCES_ActiveFlg == true && e.MI_Id == data.MI_Id && e.ISMS_Id == d.ISMS_Id)
                                 select new ProgressCardReportDTO
                                 {
                                     ISMS_Id = d.ISMS_Id,
                                     ISMS_SubjectName = e.ISMS_SubjectName,
                                     ISMS_SubjectCode = e.ISMS_SubjectCode,
                                     EYCES_AplResultFlg = d.EYCES_AplResultFlg,
                                     EYCES_MaxMarks=d.EYCES_MaxMarks,
                                     EYCES_MinMarks = d.EYCES_MinMarks,
                                     EMGR_Id = d.EMGR_Id,


                                 }
                               ).Distinct().ToArray();
                List<int> grade = new List<int>();
                foreach (ProgressCardReportDTO x in data.subjlist)
                {
                    grade.Add(x.EMGR_Id);
                }

                data.grade_details = (from a in _PCReportContext.Exm_Master_GradeDMO
                                      from b in _PCReportContext.Exm_Master_Grade_DetailsDMO
                                      where (a.MI_Id == data.MI_Id && grade.Contains(a.EMGR_Id) && a.EMGR_Id == b.EMGR_Id)
                                      select b
                                     ).Distinct().ToArray();

                //List<long> subs = new List<long>();
                //foreach(ProgressCardReportDTO x in data.savelist)
                //{
                //    subs.Add(x.ISMS_Id);
                //}
                //data.subjlist = _PCReportContext.IVRM_School_Master_SubjectsDMO.Where(t => subs.Contains(t.ISMS_Id) && t.MI_Id==data.MI_Id).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }

            return data;
        }

        public ProgressCardReportDTO editdetails(int ID)
        {
            ProgressCardReportDTO editlt = new ProgressCardReportDTO();
            //try
            //{
            //    List<StudentMappingDMO> edit = new List<StudentMappingDMO>();
            //    edit = _PCReportContext.StudentMappingDMO.Where(t => t.AMST_Id == ID).ToList();
            //    editlt.editlist = edit.ToArray();
            //    //editlt.editlist = (from a in _PCReportContext.StudentMappingDMO
            //    //                   from b in _PCReportContext.AdmissionClass
            //    //                   from c in _PCReportContext.School_M_Section
            //    //                   from d in _PCReportContext.AcademicYear
            //    //                   from e in _PCReportContext.Adm_M_Student
            //    //                   from f in _PCReportContext.IVRM_School_Master_SubjectsDMO
            //    //                   where(a.ASMCL_Id==b.ASMCL_Id && a.ASMS_Id==c.ASMS_Id && a.ASMAY_Id==d.ASMAY_Id && a.AMST_Id==e.AMST_Id && a.ISMS_Id==f.ISMS_Id && a.AMST_Id==ID)
            //    //                   select new ProgressCardReportDTO
            //    //                   {
            //    //                       ASMCL_Id=b.ASMCL_Id,
            //    //                       ASMAY_Id=d.ASMAY_Id,
            //    //                       MI_Id=a.MI_Id,
            //    //                       ESTSU_Id=a.ESTSU_Id,
            //    //                       ASMAY_Year = d.ASMAY_Year,
            //    //                       ASMC_SectionName = c.ASMC_SectionName,
            //    //                       ASMCL_ClassName = b.ASMCL_ClassName,
            //    //                       AMST_FirstName=e.AMST_FirstName,
            //    //                       ISMS_SubjectName= f.ISMS_SubjectName
            //    //                     }).ToArray();

            //    editlt.edclasslist = (from a in _PCReportContext.Exm_Category_ClassDMO
            //                          from b in _PCReportContext.AdmissionClass
            //                          from c in _PCReportContext.Exm_Master_CategoryDMO
            //                          where (a.EMCA_Id == c.EMCA_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == c.MI_Id && b.MI_Id == edit[0].MI_Id
            //                          && a.ASMCL_Id == edit[0].ASMCL_Id && a.ASMAY_Id == edit[0].ASMAY_Id)
            //                          select new ProgressCardReportDTO
            //                          {
            //                              ASMCL_Id = a.ASMCL_Id,
            //                              ASMCL_ClassName = b.ASMCL_ClassName,
            //                              EMCA_CategoryName = c.EMCA_CategoryName,
            //                              EMCA_Id = c.EMCA_Id
            //                          }).ToArray();

            //}
            //catch (Exception ee)
            //{
            //     _acdimpl.LogError(ex.Message);
            //     _acdimpl.LogDebug(ex.Message);
            //}
            return editlt;
        }



        public ProgressCardReportDTO deactivate(ProgressCardReportDTO data)
        {
            ProgressCardReportDTO deact = new ProgressCardReportDTO();

            // StudentMappingDMO master = Mapper.Map<StudentMappingDMO>(data);
            //if (data.AMST_Id > 0)
            //{
            //    var result = _PCReportContext.StudentMappingDMO.Where(t => t.AMST_Id == data.AMST_Id).ToList();
            //    for (var i = 0; i < result.Count(); i++)
            //    {
            //       var elcflag = result[i].ESTSU_ActiveFlg;
            //        if (elcflag == true)
            //        {
            //            result[i].ESTSU_ActiveFlg = false;
            //        }
            //        else
            //        {
            //            result[i].ESTSU_ActiveFlg = true;
            //        }

            //        _PCReportContext.Update(result[i]);
            //    }
            //    var flag = _PCReportContext.SaveChanges();
            //    if (flag >= 1)
            //    {
            //        deact.returnval = true;
            //    }
            //    else
            //    {
            //        deact.returnval = false;
            //    }
            //}
            return deact;
        }


    }
}
