using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.Exam;
using ExamServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Services
{
    public class ExamTTTransSettingsImpl : ExamTTTransSettingsInterface
    {
        public ExamContext _examctxt;
        private readonly subjectmasterContext _subctxt;
        private readonly ExamTimeTableContext _examttctxt;
        public ExamTTTransSettingsImpl(ExamContext obj, subjectmasterContext obj1, ExamTimeTableContext obj2)
        {
            _examctxt = obj;
            _subctxt = obj1;
            _examttctxt = obj2;
        }
        public ExamTTTransSettingsDTO getdetails(ExamTTTransSettingsDTO data)
        {
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _examctxt.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.Acdlist = list.ToArray();

                data.examlist = _examctxt.masterexam.Where(z => z.MI_Id == data.MI_Id && z.EME_ActiveFlag == true).ToArray();

                data.detailslist = (from a in _examctxt.AcademicYear
                                    from b in _examctxt.AdmissionClass
                                    from c in _examctxt.School_M_Section
                                    from d in _examctxt.masterexam
                                    from e in _examctxt.Exm_TimeTableDMO
                                    from f in _examctxt.Exm_Master_GroupDMO
                                    where (a.MI_Id == data.MI_Id && e.ASMAY_Id == a.ASMAY_Id && e.ASMCL_Id == b.ASMCL_Id && e.ASMS_Id == c.ASMS_Id && e.EME_Id == d.EME_Id && e.EXTT_ActiveFlag == true && f.EMG_Id == e.EMG_Id)
                                    select new ExamTTTransSettingsDTO
                                    {
                                        EXTT_Id = e.EXTT_Id,
                                        academicyear = a.ASMAY_Year,
                                        classname = b.ASMCL_ClassName,
                                        sectionname = c.ASMC_SectionName,
                                        examname = d.EME_ExamName,
                                        EXTT_FromDate = e.EXTT_FromDate,
                                        EXTT_EndDate = e.EXTT_EndDate,
                                        EMG_GroupName = f.EMG_GroupName,
                                        ASMAY_Order = a.ASMAY_Order
                                    }).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.subject_group = (from a in _examctxt.Exm_Category_ClassDMO
                                      from b in _examctxt.Exm_Yearly_CategoryDMO
                                      from c in _examctxt.Exm_Yearly_Category_GroupDMO
                                      from d in _examctxt.Exm_Master_GroupDMO
                                      where (a.MI_Id == data.MI_Id && a.ECAC_ActiveFlag == true && b.MI_Id == a.MI_Id && b.ASMAY_Id == a.ASMAY_Id && b.EYC_ActiveFlg == true && b.EMCA_Id == a.EMCA_Id && c.EYC_Id == b.EYC_Id && c.EYCG_ActiveFlg == true && d.MI_Id == a.MI_Id && d.EMG_ActiveFlag == true && d.EMG_Id == c.EMG_Id)
                                      select new ExamTTTransSettingsDTO
                                      {
                                          EMG_Id = d.EMG_Id,
                                          EMG_GroupName = d.EMG_GroupName

                                      }).Distinct().ToArray();

                data.subject_name = (from a in _examctxt.Exm_Category_ClassDMO
                                     from b in _examctxt.Exm_Yearly_CategoryDMO
                                     from c in _examctxt.Exm_Yearly_Category_GroupDMO
                                     from d in _examctxt.Exm_Master_GroupDMO
                                     from e in _examctxt.Exm_Yearly_Category_Group_SubjectsDMO
                                     from f in _examctxt.Exm_Master_Group_SubjectsDMO
                                     from g in _subctxt.subjectmasterDMO
                                     where (a.MI_Id == data.MI_Id && a.ECAC_ActiveFlag == true && b.MI_Id == a.MI_Id && b.ASMAY_Id == a.ASMAY_Id && b.EYC_ActiveFlg == true && b.EMCA_Id == a.EMCA_Id && c.EYC_Id == b.EYC_Id && c.EYCG_ActiveFlg == true && d.MI_Id == a.MI_Id && d.EMG_ActiveFlag == true && d.EMG_Id == c.EMG_Id && e.EYCGS_ActiveFlg == true && e.EYCG_Id == c.EYCG_Id && f.EMG_Id == d.EMG_Id && f.EMGS_ActiveFlag == true && f.ISMS_Id == e.ISMS_Id && g.MI_Id == a.MI_Id && g.ISMS_ActiveFlag == 1 && g.ISMS_ExamFlag == 1 && g.ISMS_Id == f.ISMS_Id)
                                     select new ExamTTTransSettingsDTO
                                     {
                                         EMG_Id = d.EMG_Id,
                                         EMG_GroupName = d.EMG_GroupName,
                                         ISMS_Id = g.ISMS_Id,
                                         ISMS_SubjectName = g.ISMS_SubjectName

                                     }).ToArray();

                data.time_slot = _examttctxt.Exm_TT_M_SessionDMO.Where(t => t.MI_Id == data.MI_Id && t.ETTS_Activeflag == true).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public ExamTTTransSettingsDTO getalldetailsviewrecords(ExamTTTransSettingsDTO acdmc)
        {

            try
            {
                acdmc.viewdata = (from a in _examctxt.Exm_TimeTableDMO
                                  from b in _examctxt.Exm_TimeTable_SubjectsDMO
                                  from c in _examctxt.masterexam
                                  from d in _subctxt.subjectmasterDMO
                                  from e in _examctxt.Exm_TT_M_SessionDMO
                                  where (a.MI_Id == acdmc.MI_Id && a.EXTT_Id == b.EXTT_Id && c.EME_Id == a.EME_Id && d.ISMS_Id == b.ISMS_Id
                                  && a.EXTT_Id == acdmc.EXTT_Id && e.ETTS_Id == b.ETTS_Id)
                                  select new ExamTTTransSettingsDTO
                                  {
                                      EXTTS_Id = b.EXTTS_Id,
                                      EXTT_Id = a.EXTT_Id,
                                      examname = c.EME_ExamName,
                                      subjectName = d.ISMS_SubjectName,
                                      slotname = e.ETTS_SessionName,
                                      slotdate = b.EXTTS_Date,
                                      EXTTS_ActiveFlag = b.EXTTS_ActiveFlag

                                  }).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return acdmc;
        }
        public ExamTTTransSettingsDTO editgetdetails(ExamTTTransSettingsDTO acdmc)
        {

            try
            {
                List<ExamTTTransSettingsDTO> hh = new List<ExamTTTransSettingsDTO>();
                hh = (from j in _examctxt.Exm_TimeTableDMO
                      from k in _examctxt.Exm_TimeTable_SubjectsDMO
                      from l in _subctxt.subjectmasterDMO
                      where (j.MI_Id == acdmc.MI_Id && j.EXTT_Id == k.EXTT_Id && j.EXTT_Id == acdmc.EXTT_Id && l.ISMS_Id == k.ISMS_Id)
                      select new ExamTTTransSettingsDTO
                      {
                          EXTT_Id = j.EXTT_Id,
                          ASMAY_Id = j.ASMAY_Id,
                          ASMCL_Id = j.ASMCL_Id,
                          ASMS_Id = j.ASMS_Id,
                          EME_Id = j.EME_Id,
                          EMG_Id = j.EMG_Id,
                          EXTT_FromDate = j.EXTT_FromDate,
                          EXTT_EndDate = j.EXTT_EndDate,
                          ISMS_Id = k.ISMS_Id,
                          EXTTS_Date = k.EXTTS_Date,
                          ETTS_Id = k.ETTS_Id,
                          ISMS_SubjectName = l.ISMS_SubjectName,

                      }).ToList();
                acdmc.listedit = (from j in _examctxt.Exm_TimeTableDMO
                                  from k in _examctxt.Exm_TimeTable_SubjectsDMO
                                  from l in _subctxt.subjectmasterDMO
                                  where (j.MI_Id == acdmc.MI_Id && j.EXTT_Id == k.EXTT_Id && j.EXTT_Id == acdmc.EXTT_Id && l.ISMS_Id == k.ISMS_Id)
                                  select new ExamTTTransSettingsDTO
                                  {
                                      ASMAY_Id = j.ASMAY_Id,
                                      ASMCL_Id = j.ASMCL_Id,
                                      ASMS_Id = j.ASMS_Id,
                                      EME_Id = j.EME_Id,
                                      EMG_Id = j.EMG_Id,
                                      EXTT_FromDate = j.EXTT_FromDate,
                                      EXTT_EndDate = j.EXTT_EndDate,
                                      ISMS_Id = k.ISMS_Id,
                                      EXTTS_Date = k.EXTTS_Date,
                                      ETTS_Id = k.ETTS_Id,
                                      ISMS_SubjectName = l.ISMS_SubjectName,

                                  }).ToArray();


                acdmc.subject_name = (from a in _examctxt.Exm_Category_ClassDMO
                                      from b in _examctxt.Exm_Yearly_CategoryDMO
                                      from c in _examctxt.Exm_Yearly_Category_GroupDMO
                                      from d in _examctxt.Exm_Master_GroupDMO
                                      from e in _examctxt.Exm_Yearly_Category_Group_SubjectsDMO
                                      from f in _examctxt.Exm_Master_Group_SubjectsDMO
                                      from g in _subctxt.subjectmasterDMO
                                      from h in _examctxt.Exm_Yearly_Category_ExamsDMO
                                      from i in _examctxt.Exm_Yrly_Cat_Exams_SubwiseDMO
                                      where (a.ECAC_ActiveFlag == true && b.EYC_ActiveFlg == true && b.EMCA_Id == a.EMCA_Id && c.EYC_Id == b.EYC_Id
                                      && c.EYCG_ActiveFlg == true && d.EMG_ActiveFlag == true && d.EMG_Id == c.EMG_Id && e.EYCGS_ActiveFlg == true
                                      && e.EYCG_Id == c.EYCG_Id && f.EMG_Id == d.EMG_Id && f.EMGS_ActiveFlag == true && f.ISMS_Id == e.ISMS_Id
                                      && g.ISMS_ActiveFlag == 1 && g.ISMS_ExamFlag == 1 && g.ISMS_Id == f.ISMS_Id && c.EMG_Id == hh[0].EMG_Id && h.EYC_Id == b.EYC_Id
                                      && h.EYCE_ActiveFlg == true && h.EME_Id == hh[0].EME_Id && i.EYCE_Id == h.EYCE_Id && i.EYCES_ActiveFlg == true
                                      && i.ISMS_Id == f.ISMS_Id && a.MI_Id == acdmc.MI_Id && a.ASMAY_Id == hh[0].ASMAY_Id && a.ASMCL_Id == hh[0].ASMCL_Id
                                      && a.ASMS_Id == hh[0].ASMS_Id && b.ASMAY_Id == hh[0].ASMAY_Id)
                                      select new ExamTTTransSettingsDTO
                                      {
                                          EMG_Id = d.EMG_Id,
                                          EMG_GroupName = d.EMG_GroupName,
                                          ISMS_Id = g.ISMS_Id,
                                          ISMS_SubjectName = g.ISMS_SubjectName
                                      }).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return acdmc;
        }
        public ExamTTTransSettingsDTO onselectAcdYear(ExamTTTransSettingsDTO data)
        {
            try
            {
                data.ctlist = (from c in _examctxt.AdmissionClass
                               from d in _examctxt.Exm_Category_ClassDMO
                               where (c.MI_Id == data.MI_Id && c.ASMCL_ActiveFlag == true && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && d.ASMCL_Id == c.ASMCL_Id && d.ECAC_ActiveFlag == true)
                               select c).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ExamTTTransSettingsDTO onselectclass(ExamTTTransSettingsDTO data)
        {
            try
            {
                data.seclist = (from c in _examctxt.School_M_Section
                                from d in _examctxt.Exm_Category_ClassDMO
                                where (c.MI_Id == data.MI_Id && c.ASMC_ActiveFlag == 1 && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && d.ASMCL_Id == data.ASMCL_Id && d.ECAC_ActiveFlag == true && c.ASMS_Id == d.ASMS_Id)
                                select c).Distinct().OrderBy(t => t.ASMC_Order).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ExamTTTransSettingsDTO onselectSection(ExamTTTransSettingsDTO data)
        {
            try
            {
                List<int> MI_ID_DTO_list = new List<int>();
                if (data.ASMS_Id == 0)
                {
                    var EMCA_Id = _examctxt.Exm_Category_ClassDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).Distinct().ToList();

                    for (int l = 0; l < EMCA_Id.Count; l++)
                    {
                        MI_ID_DTO_list.Add(EMCA_Id[l]);
                    }
                }
                else
                {
                    var EMCA_Id = _examctxt.Exm_Category_ClassDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).ToList();

                    for (int l = 0; l < EMCA_Id.Count; l++)
                    {
                        MI_ID_DTO_list.Add(EMCA_Id[l]);
                    }
                }


                List<long> eyc = new List<long>();

                var EYC_Id = _examctxt.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && MI_ID_DTO_list.Contains(t.EMCA_Id) && t.EYC_ActiveFlg == true).Select(a => a.EYC_Id).Distinct().ToList();

                for (int k = 0; k < EYC_Id.Count; k++)
                {
                    eyc.Add(EYC_Id[k]);
                }

                var examlist = (from a in _examctxt.masterexam
                                from b in _examctxt.Exm_Yearly_Category_ExamsDMO
                                where (a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true && a.EME_Id == b.EME_Id && eyc.Contains(b.EYC_Id) && b.EYCE_ActiveFlg == true)
                                select a).Distinct().OrderBy(t => t.EME_ExamOrder).ToList();

                data.examlist = examlist.Distinct().OrderBy(t => t.EME_ExamOrder).ToArray();


                if (data.ASMS_Id == 0)
                {
                    data.subject_group = (from a in _examctxt.Exm_Category_ClassDMO
                                          from b in _examctxt.Exm_Yearly_CategoryDMO
                                          from c in _examctxt.Exm_Yearly_Category_GroupDMO
                                          from d in _examctxt.Exm_Master_GroupDMO
                                          where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                                          //&& a.ASMS_Id == data.ASMS_Id
                                          && a.ECAC_ActiveFlag == true && b.MI_Id == a.MI_Id && b.ASMAY_Id == a.ASMAY_Id && b.EYC_ActiveFlg == true
                                          && b.EMCA_Id == a.EMCA_Id && c.EYC_Id == b.EYC_Id && c.EYCG_ActiveFlg == true && d.MI_Id == a.MI_Id && d.EMG_ActiveFlag == true
                                          && d.EMG_Id == c.EMG_Id)
                                          select new ExamTTTransSettingsDTO
                                          {
                                              EMG_Id = d.EMG_Id,
                                              EMG_GroupName = d.EMG_GroupName
                                          }).Distinct().ToArray();


                    data.subject_name = (from a in _examctxt.Exm_Category_ClassDMO
                                         from b in _examctxt.Exm_Yearly_CategoryDMO
                                         from c in _examctxt.Exm_Yearly_Category_GroupDMO
                                         from d in _examctxt.Exm_Master_GroupDMO
                                         from e in _examctxt.Exm_Yearly_Category_Group_SubjectsDMO
                                         from f in _examctxt.Exm_Master_Group_SubjectsDMO
                                         from g in _subctxt.subjectmasterDMO
                                         where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                                         //&& a.ASMS_Id == data.ASMS_Id 
                                         && a.ECAC_ActiveFlag == true && b.MI_Id == a.MI_Id && b.ASMAY_Id == a.ASMAY_Id && b.EYC_ActiveFlg == true && b.EMCA_Id == a.EMCA_Id && c.EYC_Id == b.EYC_Id && c.EYCG_ActiveFlg == true && d.MI_Id == a.MI_Id && d.EMG_ActiveFlag == true && d.EMG_Id == c.EMG_Id && e.EYCGS_ActiveFlg == true && e.EYCG_Id == c.EYCG_Id && f.EMG_Id == d.EMG_Id && f.EMGS_ActiveFlag == true && f.ISMS_Id == e.ISMS_Id && g.MI_Id == a.MI_Id && g.ISMS_ActiveFlag == 1 && g.ISMS_ExamFlag == 1 && g.ISMS_Id == f.ISMS_Id && c.EMG_Id == data.EMG_Id)
                                         select new ExamTTTransSettingsDTO
                                         {
                                             EMG_Id = d.EMG_Id,
                                             EMG_GroupName = d.EMG_GroupName,
                                             ISMS_Id = g.ISMS_Id,
                                             ISMS_SubjectName = g.ISMS_SubjectName

                                         }).Distinct().ToArray();

                }
                else
                {
                    data.subject_group = (from a in _examctxt.Exm_Category_ClassDMO
                                          from b in _examctxt.Exm_Yearly_CategoryDMO
                                          from c in _examctxt.Exm_Yearly_Category_GroupDMO
                                          from d in _examctxt.Exm_Master_GroupDMO
                                          where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id
                                          && a.ECAC_ActiveFlag == true && b.MI_Id == a.MI_Id && b.ASMAY_Id == a.ASMAY_Id && b.EYC_ActiveFlg == true
                                          && b.EMCA_Id == a.EMCA_Id && c.EYC_Id == b.EYC_Id && c.EYCG_ActiveFlg == true && d.MI_Id == a.MI_Id && d.EMG_ActiveFlag == true
                                          && d.EMG_Id == c.EMG_Id)
                                          select new ExamTTTransSettingsDTO
                                          {
                                              EMG_Id = d.EMG_Id,
                                              EMG_GroupName = d.EMG_GroupName

                                          }).Distinct().ToArray();

                    data.subject_name = (from a in _examctxt.Exm_Category_ClassDMO
                                         from b in _examctxt.Exm_Yearly_CategoryDMO
                                         from c in _examctxt.Exm_Yearly_Category_GroupDMO
                                         from d in _examctxt.Exm_Master_GroupDMO
                                         from e in _examctxt.Exm_Yearly_Category_Group_SubjectsDMO
                                         from f in _examctxt.Exm_Master_Group_SubjectsDMO
                                         from g in _subctxt.subjectmasterDMO
                                         where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true && b.MI_Id == a.MI_Id && b.ASMAY_Id == a.ASMAY_Id && b.EYC_ActiveFlg == true && b.EMCA_Id == a.EMCA_Id && c.EYC_Id == b.EYC_Id && c.EYCG_ActiveFlg == true && d.MI_Id == a.MI_Id && d.EMG_ActiveFlag == true && d.EMG_Id == c.EMG_Id && e.EYCGS_ActiveFlg == true && e.EYCG_Id == c.EYCG_Id && f.EMG_Id == d.EMG_Id && f.EMGS_ActiveFlag == true && f.ISMS_Id == e.ISMS_Id && g.MI_Id == a.MI_Id && g.ISMS_ActiveFlag == 1 && g.ISMS_ExamFlag == 1 && g.ISMS_Id == f.ISMS_Id && c.EMG_Id == data.EMG_Id)
                                         select new ExamTTTransSettingsDTO
                                         {
                                             EMG_Id = d.EMG_Id,
                                             EMG_GroupName = d.EMG_GroupName,
                                             ISMS_Id = g.ISMS_Id,
                                             ISMS_SubjectName = g.ISMS_SubjectName

                                         }).Distinct().ToArray();

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ExamTTTransSettingsDTO onselectSubject(ExamTTTransSettingsDTO data)
        {
            try
            {
                if (data.ASMS_Id == 0)
                {
                    data.subject_name = (from a in _examctxt.Exm_Category_ClassDMO
                                         from b in _examctxt.Exm_Yearly_CategoryDMO
                                         from c in _examctxt.Exm_Yearly_Category_GroupDMO
                                         from d in _examctxt.Exm_Master_GroupDMO
                                         from e in _examctxt.Exm_Yearly_Category_Group_SubjectsDMO
                                         from f in _examctxt.Exm_Master_Group_SubjectsDMO
                                         from g in _subctxt.subjectmasterDMO
                                         from h in _examctxt.Exm_Yearly_Category_ExamsDMO
                                         from i in _examctxt.Exm_Yrly_Cat_Exams_SubwiseDMO
                                         where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                                         //&& a.ASMS_Id == data.ASMS_Id 
                                         && a.ECAC_ActiveFlag == true && b.EYC_ActiveFlg == true && b.EMCA_Id == a.EMCA_Id && c.EYC_Id == b.EYC_Id && c.EYCG_ActiveFlg == true && d.EMG_ActiveFlag == true && d.EMG_Id == c.EMG_Id && e.EYCGS_ActiveFlg == true && e.EYCG_Id == c.EYCG_Id && f.EMG_Id == d.EMG_Id && f.EMGS_ActiveFlag == true && f.ISMS_Id == e.ISMS_Id && g.ISMS_ActiveFlag == 1 && g.ISMS_ExamFlag == 1 && g.ISMS_Id == f.ISMS_Id && c.EMG_Id == data.EMG_Id && h.EYC_Id == b.EYC_Id && h.EYCE_ActiveFlg == true && h.EME_Id == data.EME_Id && i.EYCE_Id == h.EYCE_Id && i.EYCES_ActiveFlg == true && i.ISMS_Id == f.ISMS_Id && b.ASMAY_Id == data.ASMAY_Id)
                                         select new ExamTTTransSettingsDTO
                                         {
                                             EMG_Id = d.EMG_Id,
                                             EMG_GroupName = d.EMG_GroupName,
                                             ISMS_Id = g.ISMS_Id,
                                             ISMS_SubjectName = g.ISMS_SubjectName
                                         }).Distinct().ToArray();
                }
                else
                {
                    data.subject_name = (from a in _examctxt.Exm_Category_ClassDMO
                                         from b in _examctxt.Exm_Yearly_CategoryDMO
                                         from c in _examctxt.Exm_Yearly_Category_GroupDMO
                                         from d in _examctxt.Exm_Master_GroupDMO
                                         from e in _examctxt.Exm_Yearly_Category_Group_SubjectsDMO
                                         from f in _examctxt.Exm_Master_Group_SubjectsDMO
                                         from g in _subctxt.subjectmasterDMO
                                         from h in _examctxt.Exm_Yearly_Category_ExamsDMO
                                         from i in _examctxt.Exm_Yrly_Cat_Exams_SubwiseDMO
                                         where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true && b.EYC_ActiveFlg == true && b.EMCA_Id == a.EMCA_Id && c.EYC_Id == b.EYC_Id && c.EYCG_ActiveFlg == true && d.EMG_ActiveFlag == true && d.EMG_Id == c.EMG_Id && e.EYCGS_ActiveFlg == true && e.EYCG_Id == c.EYCG_Id && f.EMG_Id == d.EMG_Id && f.EMGS_ActiveFlag == true && f.ISMS_Id == e.ISMS_Id && g.ISMS_ActiveFlag == 1 && g.ISMS_ExamFlag == 1 && g.ISMS_Id == f.ISMS_Id && c.EMG_Id == data.EMG_Id && h.EYC_Id == b.EYC_Id && h.EYCE_ActiveFlg == true && h.EME_Id == data.EME_Id && i.EYCE_Id == h.EYCE_Id && i.EYCES_ActiveFlg == true && i.ISMS_Id == f.ISMS_Id && b.ASMAY_Id == data.ASMAY_Id)
                                         select new ExamTTTransSettingsDTO
                                         {
                                             EMG_Id = d.EMG_Id,
                                             EMG_GroupName = d.EMG_GroupName,
                                             ISMS_Id = g.ISMS_Id,
                                             ISMS_SubjectName = g.ISMS_SubjectName
                                         }).Distinct().ToArray();
                }

                data.time_slot = _examttctxt.Exm_TT_M_SessionDMO.Where(t => t.MI_Id == data.MI_Id && t.ETTS_Activeflag == true).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ExamTTTransSettingsDTO onselectSubSubject(ExamTTTransSettingsDTO data)
        {
            try
            {

                data.SubSubject = (from a in _examctxt.Exm_Category_ClassDMO
                                   from b in _examctxt.Exm_Yearly_CategoryDMO
                                   from h in _examctxt.Exm_Yearly_Category_ExamsDMO
                                   from i in _examctxt.Exm_Yrly_Cat_Exams_SubwiseDMO
                                   from j in _examctxt.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO
                                   from k in _examctxt.mastersubsubject
                                   where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true && b.MI_Id == a.MI_Id && b.ASMAY_Id == a.ASMAY_Id && b.EYC_ActiveFlg == true && b.EMCA_Id == a.EMCA_Id && h.EYC_Id == b.EYC_Id && h.EYCE_ActiveFlg == true && h.EME_Id == data.EME_Id && i.EYCE_Id == h.EYCE_Id && i.EYCES_ActiveFlg == true && j.EYCES_Id == i.EYCES_Id && j.EYCESSS_ActiveFlg == true && k.MI_Id == a.MI_Id && k.EMSS_ActiveFlag == true && k.EMSS_Id == j.EMSS_Id && i.ISMS_Id == data.ISMS_Id)
                                   select new ExamTTTransSettingsDTO
                                   {
                                       ISMS_Id = i.ISMS_Id,
                                       EMSS_Id = k.EMSS_Id,
                                       EMSS_SubSubjectName = k.EMSS_SubSubjectName
                                   }).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ExamTTTransSettingsDTO savedetail(ExamTTTransSettingsDTO data)
        {


            try
            {
                if (data.EXTT_Id > 0)
                {
                    var res = _examctxt.Exm_TimeTableDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.EMG_Id == data.EMG_Id && t.EXTT_Id != data.EXTT_Id).ToList();

                    if (res.Count() > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var objpge1 = _examctxt.Exm_TimeTableDMO.Single(t => t.EXTT_Id == data.EXTT_Id);
                        objpge1.MI_Id = data.MI_Id;
                        objpge1.ASMAY_Id = data.ASMAY_Id;
                        objpge1.ASMCL_Id = data.ASMCL_Id;
                        objpge1.ASMS_Id = data.ASMS_Id;
                        objpge1.EME_Id = data.EME_Id;
                        objpge1.EXTT_ActiveFlag = true;
                        objpge1.EXTT_FromDate = data.EXTT_FromDate;
                        objpge1.EXTT_EndDate = data.EXTT_EndDate;
                        objpge1.EMG_Id = data.EMG_Id;

                        objpge1.UpdatedDate = DateTime.Now;
                        _examctxt.Update(objpge1);
                        var contactExists = _examctxt.SaveChanges();
                        var result = data.EXTT_Id;
                        if (contactExists == 1)
                        {
                            for (int i = 0; i < data.TempararyArrayList.Count(); i++)
                            {
                                var a = 0;
                                List<Exm_TimeTable_SubjectsDMO> objpge3 = new List<Exm_TimeTable_SubjectsDMO>();
                                objpge3 = _examctxt.Exm_TimeTable_SubjectsDMO.Where(t => t.EXTT_Id == data.EXTT_Id).ToList();
                                for (var j = 0; j < objpge3.Count(); j++)
                                {
                                    if (data.TempararyArrayList[i].ISMS_Id == objpge3[j].ISMS_Id)
                                    {
                                        if (data.TempararyArrayList[i].check_save == "1")
                                        {
                                            var objpge2 = _examctxt.Exm_TimeTable_SubjectsDMO.Single(t => t.EXTTS_Id == objpge3[j].EXTTS_Id);
                                            objpge2.EXTT_Id = result;
                                            objpge2.ISMS_Id = data.TempararyArrayList[i].ISMS_Id;
                                            objpge2.ETTS_Id = data.TempararyArrayList[i].ETTS_Id;
                                            objpge2.EXTTS_Date = data.TempararyArrayList[i].EXTTS_Date;
                                            // objpge2.CreatedDate = DateTime.Now;
                                            objpge2.UpdatedDate = DateTime.Now;
                                            objpge2.EXTTS_ExamDuration = data.EXTTS_ExamDuration;
                                            objpge2.EXTTS_FromTime = data.EXTTS_FromTime;
                                            objpge2.EXTTS_EndTime = data.EXTTS_EndTime;
                                            objpge2.EXTTS_ActiveFlag = true;
                                            _examctxt.Update(objpge2);
                                            var contactExists1 = _examctxt.SaveChanges();
                                            if (contactExists1 == 1)
                                            {
                                                data.returnval = true;
                                                a = 1;
                                            }
                                            else
                                            {
                                                data.returnval = false;
                                            }
                                        }
                                        else
                                        {
                                            List<Exm_TimeTable_SubjectsDMO> lorg2 = new List<Exm_TimeTable_SubjectsDMO>();
                                            lorg2 = _examctxt.Exm_TimeTable_SubjectsDMO.Where(t => t.EXTTS_Id.Equals(objpge3[j].EXTTS_Id)).ToList();
                                            if (lorg2.Any())
                                            {
                                                for (int z = 0; z < lorg2.Count; z++)
                                                {
                                                    _examctxt.Remove(lorg2.ElementAt(z));
                                                    var contactExists2 = _examctxt.SaveChanges();
                                                    if (contactExists2.Equals(1))
                                                    {
                                                        data.returnval = true;
                                                    }
                                                    else
                                                    {
                                                        data.returnval = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                if (a == 0)
                                {
                                    if (data.TempararyArrayList[i].check_save == "1")
                                    {
                                        Exm_TimeTable_SubjectsDMO objpge2 = new Exm_TimeTable_SubjectsDMO();
                                        objpge2.EXTT_Id = result;
                                        objpge2.ISMS_Id = data.TempararyArrayList[i].ISMS_Id;
                                        objpge2.ETTS_Id = data.TempararyArrayList[i].ETTS_Id;
                                        objpge2.EXTTS_Date = data.TempararyArrayList[i].EXTTS_Date;
                                        objpge2.CreatedDate = DateTime.Now;
                                        objpge2.UpdatedDate = DateTime.Now;
                                        objpge2.EXTTS_ExamDuration = data.EXTTS_ExamDuration;
                                        objpge2.EXTTS_FromTime = data.EXTTS_FromTime;
                                        objpge2.EXTTS_EndTime = data.EXTTS_EndTime;
                                        objpge2.EXTTS_ActiveFlag = true;
                                        _examctxt.Add(objpge2);
                                        var contactExists1 = _examctxt.SaveChanges();
                                        if (contactExists1 == 1)
                                        {
                                            data.returnval = true;
                                        }
                                        else
                                        {
                                            data.returnval = false;
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
                else
                {
                    List<long> sec = new List<long>();
                    if (data.ASMS_Id == 0)
                    {
                        var sectionlist = (from c in _examctxt.School_M_Section
                                           from d in _examctxt.Exm_Category_ClassDMO
                                           where (c.ASMS_Id == d.ASMS_Id && c.MI_Id == data.MI_Id && c.ASMC_ActiveFlag == 1 && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && d.ASMCL_Id == data.ASMCL_Id && d.ECAC_ActiveFlag == true)
                                           select c).Distinct().ToList();

                        for (int i = 0; i < sectionlist.Count; i++)
                        {
                            sec.Add(sectionlist[i].ASMS_Id);
                        }
                    }
                    else
                    {
                        var sectionlist = (from c in _examctxt.School_M_Section
                                           from d in _examctxt.Exm_Category_ClassDMO
                                           where (c.ASMS_Id == d.ASMS_Id && c.MI_Id == data.MI_Id && c.ASMC_ActiveFlag == 1 && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && d.ASMCL_Id == data.ASMCL_Id && d.ECAC_ActiveFlag == true && d.ASMS_Id == data.ASMS_Id)
                                           select c).Distinct().ToList();
                        for (int i = 0; i < sectionlist.Count; i++)
                        {
                            sec.Add(sectionlist[i].ASMS_Id);
                        }
                    }

                    for (int k = 0; k < sec.Count; k++)
                    {
                        data.ASMS_Id = sec[k];
                        var res = _examctxt.Exm_TimeTableDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.EMG_Id == data.EMG_Id).ToList();

                        if (res.Count() > 0)
                        {
                            data.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            Exm_TimeTableDMO objpge = new Exm_TimeTableDMO();

                            objpge.MI_Id = data.MI_Id;
                            objpge.ASMAY_Id = data.ASMAY_Id;
                            objpge.ASMCL_Id = data.ASMCL_Id;
                            objpge.ASMS_Id = data.ASMS_Id;
                            objpge.EME_Id = data.EME_Id;
                            objpge.EXTT_ActiveFlag = true;
                            objpge.EXTT_FromDate = data.EXTT_FromDate;
                            objpge.EXTT_EndDate = data.EXTT_EndDate;
                            objpge.EMG_Id = data.EMG_Id;
                            objpge.CreatedDate = DateTime.Now;
                            objpge.UpdatedDate = DateTime.Now;
                            _examctxt.Add(objpge);
                            //    var contactExists = _examctxt.SaveChanges();
                            //var result = _examctxt.Exm_TimeTableDMO.Max(t => t.EXTT_Id);
                            //if (contactExists == 1)
                            //{
                            for (int i = 0; i < data.TempararyArrayList.Count(); i++)
                            {
                                if (data.TempararyArrayList[i].check_save == "1")
                                {
                                    Exm_TimeTable_SubjectsDMO objpge1 = new Exm_TimeTable_SubjectsDMO();
                                    objpge1.EXTT_Id = objpge.EXTT_Id;
                                    //objpge1.EXTT_Id = result;
                                    objpge1.ISMS_Id = data.TempararyArrayList[i].ISMS_Id;
                                    objpge1.ETTS_Id = data.TempararyArrayList[i].ETTS_Id;
                                    objpge1.EXTTS_Date = data.TempararyArrayList[i].EXTTS_Date;
                                    objpge1.CreatedDate = DateTime.Now;
                                    objpge1.UpdatedDate = DateTime.Now;
                                    objpge1.EXTTS_ExamDuration = data.EXTTS_ExamDuration;
                                    objpge1.EXTTS_FromTime = data.EXTTS_FromTime;
                                    objpge1.EXTTS_EndTime = data.EXTTS_EndTime;
                                    objpge1.EXTTS_ActiveFlag = true;
                                    _examctxt.Add(objpge1);
                                }
                            }

                            var contactExists1 = _examctxt.SaveChanges();
                            if (contactExists1 > 0)
                            {
                                data.returnval = true;
                            }
                            else
                            {
                                data.returnval = false;
                            }
                        }
                        //}
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ExamTTTransSettingsDTO deactivate(ExamTTTransSettingsDTO acd)
        {
            try
            {
                if (acd.EXTTS_Id > 0)
                {
                    var result = _examctxt.Exm_TimeTable_SubjectsDMO.Single(t => t.EXTTS_Id == acd.EXTTS_Id);
                    result.UpdatedDate = DateTime.Now;

                    if (result.EXTTS_ActiveFlag == true)
                    {
                        result.EXTTS_ActiveFlag = false;
                    }
                    else
                    {
                        result.EXTTS_ActiveFlag = true;
                    }
                    _examctxt.Update(result);
                    var flag = _examctxt.SaveChanges();
                    if (flag == 1)
                    {
                        acd.returnval = true;
                    }
                    else
                    {
                        acd.returnval = false;
                    }


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }

    }
}
