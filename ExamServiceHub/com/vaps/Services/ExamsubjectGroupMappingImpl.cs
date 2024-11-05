using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.Exam;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Services
{
    public class ExamsubjectGroupMappingImpl : Interfaces.ExamsubjectGroupMappingInterface
    {
        public ExamContext _context;
        ILogger<ExamsubjectGroupMappingImpl> _acdimpl;

        public ExamsubjectGroupMappingImpl(ExamContext context, ILogger<ExamsubjectGroupMappingImpl> acdimpl)
        {
            _acdimpl = acdimpl;
            _context = context;
        }
        public ExamsubjectGroupMappingDTo Getdetails(ExamsubjectGroupMappingDTo data)
        {
            try
            {
                data.getyear = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.getdetails = (from a in _context.ExamsubjectGroupMappingDMO
                                   from b in _context.ExamSubjectGroupMappingExamsDMO
                                   from c in _context.ExamSubjectGroupMappingSubjectsDMO
                                   from d in _context.Exm_Master_CategoryDMO
                                   from e in _context.Exm_Yearly_CategoryDMO
                                   from f in _context.exammasterDMO
                                   from g in _context.AcademicYear
                                   where (a.ESG_Id == b.ESG_Id && a.ESG_Id == c.ESG_Id && a.EMCA_Id == e.EMCA_Id && d.EMCA_Id == e.EMCA_Id && b.EME_Id == f.EME_Id && a.MI_Id == data.MI_Id && a.ESG_ExamPromotionFlag == "IE" && g.ASMAY_Id == a.ASMAY_Id)
                                   select new ExamsubjectGroupMappingDTo
                                   {
                                       ASMAY_Id = a.ASMAY_Id,
                                       ASMAY_Year = g.ASMAY_Year,
                                       EMCA_CategoryName = d.EMCA_CategoryName,
                                       grpname = a.ESG_SubjectGroupName,
                                       percentage = Convert.ToString(a.ESG_GroupMinMarks),
                                       ESG_GroupMaxMarks = a.ESG_GroupMaxMarks,
                                       EME_ExamName = f.EME_ExamName,
                                       ESG_Id = a.ESG_Id,
                                       ESGE_Id = b.ESGE_Id,
                                       Compulsory1 = a.ESG_CompulsoryFlag,
                                       ASMAY_Order = g.ASMAY_Order
                                   }).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogInformation("Exam Subject Group Mapping load :" + ex.Message);
            }
            return data;
        }
        public ExamsubjectGroupMappingDTo getcategory(ExamsubjectGroupMappingDTo data)
        {
            try
            {
                data.getcategory = (from b in _context.Exm_Master_CategoryDMO
                                    from c in _context.Exm_Yearly_CategoryDMO
                                    where (c.EMCA_Id == b.EMCA_Id && b.EMCA_ActiveFlag == true && c.EYC_ActiveFlg == true
                                    && b.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id)
                                    select new ExamsubjectGroupMappingDTo
                                    {
                                        EMCA_Id = b.EMCA_Id,
                                        EMCA_CategoryName = b.EMCA_CategoryName
                                    }).ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogInformation("Exam Subject Group Mapping category :" + ex.Message);
            }
            return data;
        }
        public ExamsubjectGroupMappingDTo getexam(ExamsubjectGroupMappingDTo data)
        {
            try
            {
                if (data.Flag == "0")
                {
                    data.getexam = (from c in _context.Exm_Yearly_CategoryDMO
                                    from d in _context.Exm_Yearly_Category_ExamsDMO
                                    from e in _context.exammasterDMO
                                    where (e.EME_Id == d.EME_Id && d.EYC_Id == c.EYC_Id && c.EYC_ActiveFlg == true
                                    && d.EYCE_ActiveFlg == true && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.EMCA_Id == data.EMCA_Id)
                                    select new ExamsubjectGroupMappingDTo
                                    {
                                        EME_Id = e.EME_Id,
                                        EME_ExamName = e.EME_ExamName,
                                        EME_ExamOrder = Convert.ToInt64(e.EME_ExamOrder)
                                    }).Distinct().OrderBy(f => f.EME_ExamOrder).ToArray();
                }
                else
                {
                    if (data.Flag1 == "E")
                    {
                        data.getsubject = (from a in _context.Exm_Yrly_Cat_Exams_SubwiseDMO
                                           from b in _context.IVRM_School_Master_SubjectsDMO
                                           from c in _context.Exm_Yearly_CategoryDMO
                                           from d in _context.Exm_Yearly_Category_ExamsDMO
                                           where (a.ISMS_Id == b.ISMS_Id && a.EYCE_Id == d.EYCE_Id && c.EYC_Id == d.EYC_Id && a.EYCES_ActiveFlg == true && b.ISMS_ActiveFlag == 1 && d.EYCE_ActiveFlg == true && c.EYC_ActiveFlg == true && c.EMCA_Id == data.EMCA_Id)
                                           select new ExamsubjectGroupMappingDTo
                                           {
                                               ISMS_Id = b.ISMS_Id,
                                               ISMS_SubjectName = b.ISMS_SubjectName,
                                               ISMS_OrderFlag = b.ISMS_OrderFlag
                                               

                                           }).Distinct().OrderBy(dd => dd.ISMS_OrderFlag).ToArray();
                    }
                    else
                    {
                        var get_mappedids = (from a in _context.ExamsubjectGroupMappingDMO
                                             from c in _context.ExamSubjectGroupMappingSubjectsDMO
                                             where (a.ESG_Id == c.ESG_Id && a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == data.EMCA_Id
                                             && a.ESG_ActiveFlag == true && c.ESGS_ActiveFlag == true && a.ESG_ExamPromotionFlag == "PE")
                                             select new get_subjectlist
                                             {
                                                 ISMS_Id = c.ISMS_Id,
                                             }).Distinct().ToList();

                        List<long> GrpId = new List<long>();
                        foreach (var item in get_mappedids)
                        {
                            GrpId.Add(item.ISMS_Id);
                        }

                        data.getsubject = (from a in _context.Exm_Yrly_Cat_Exams_SubwiseDMO
                                           from b in _context.IVRM_School_Master_SubjectsDMO
                                           from c in _context.Exm_Yearly_CategoryDMO
                                           from d in _context.Exm_Yearly_Category_ExamsDMO
                                           where (a.ISMS_Id == b.ISMS_Id && a.EYCE_Id == d.EYCE_Id && c.EYC_Id == d.EYC_Id && a.EYCES_ActiveFlg == true && b.ISMS_ActiveFlag == 1 && d.EYCE_ActiveFlg == true && c.EYC_ActiveFlg == true && c.EMCA_Id == data.EMCA_Id 
                                           && !GrpId.Contains(a.ISMS_Id))
                                           select new ExamsubjectGroupMappingDTo
                                           {
                                               ISMS_Id = b.ISMS_Id,
                                               ISMS_SubjectName = b.ISMS_SubjectName,
                                               ISMS_OrderFlag = b.ISMS_OrderFlag                                              

                                           }).Distinct().OrderBy(dd => dd.ISMS_OrderFlag).ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                _acdimpl.LogInformation("Exam Subject Group Mapping getexam  :" + ex.Message);
            }
            return data;
        }
        public ExamsubjectGroupMappingDTo getsubject(ExamsubjectGroupMappingDTo data)
        {
            try
            {

                if (data.Flag1 == "E")
                {
                    data.getsubject = (from a in _context.Exm_Yrly_Cat_Exams_SubwiseDMO
                                       from b in _context.IVRM_School_Master_SubjectsDMO
                                       from c in _context.Exm_Yearly_CategoryDMO
                                       from d in _context.Exm_Yearly_Category_ExamsDMO
                                       where (a.ISMS_Id == b.ISMS_Id && a.EYCE_Id == d.EYCE_Id && c.EYC_Id == d.EYC_Id && a.EYCES_ActiveFlg == true && b.ISMS_ActiveFlag == 1 && d.EYCE_ActiveFlg == true && c.EYC_ActiveFlg == true && c.EMCA_Id == data.EMCA_Id && d.EME_Id == data.EME_Id)
                                       select new ExamsubjectGroupMappingDTo
                                       {
                                           ISMS_Id = b.ISMS_Id,
                                           ISMS_SubjectName = b.ISMS_SubjectName,
                                           ISMS_OrderFlag = b.ISMS_OrderFlag,
                                           appornonresult = a.EYCES_AplResultFlg

                                       }).Distinct().OrderBy(dd => dd.ISMS_OrderFlag).ToArray();
                }
                else
                {
                    var get_mappedids = (from a in _context.ExamsubjectGroupMappingDMO
                                         from b in _context.ExamSubjectGroupMappingExamsDMO
                                         from c in _context.ExamSubjectGroupMappingSubjectsDMO
                                         where (a.ESG_Id == b.ESG_Id && a.ESG_Id == c.ESG_Id && a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == data.EMCA_Id && b.EME_Id == data.EME_Id
                                         && a.ESG_ActiveFlag == true && b.ESGE_ActiveFlag == true && c.ESGS_ActiveFlag == true && a.ESG_ExamPromotionFlag == "IE")
                                         select new get_subjectlist
                                         {
                                             ISMS_Id = c.ISMS_Id,
                                         }).Distinct().ToList();

                    List<long> GrpId = new List<long>();
                    foreach (var item in get_mappedids)
                    {
                        GrpId.Add(item.ISMS_Id);
                    }
                    data.getsubject = (from a in _context.Exm_Yrly_Cat_Exams_SubwiseDMO
                                       from b in _context.IVRM_School_Master_SubjectsDMO
                                       from c in _context.Exm_Yearly_CategoryDMO
                                       from d in _context.Exm_Yearly_Category_ExamsDMO
                                       where (a.ISMS_Id == b.ISMS_Id && a.EYCE_Id == d.EYCE_Id && c.EYC_Id == d.EYC_Id && a.EYCES_ActiveFlg == true && b.ISMS_ActiveFlag == 1 && d.EYCE_ActiveFlg == true && c.EYC_ActiveFlg == true && d.EME_Id == data.EME_Id && c.EMCA_Id == data.EMCA_Id && !GrpId.Contains(a.ISMS_Id))
                                       select new ExamsubjectGroupMappingDTo
                                       {
                                           ISMS_Id = b.ISMS_Id,
                                           ISMS_SubjectName = b.ISMS_SubjectName,
                                           ISMS_OrderFlag = b.ISMS_OrderFlag,
                                           appornonresult = a.EYCES_AplResultFlg

                                       }).Distinct().OrderBy(dd => dd.ISMS_OrderFlag).ToArray();
                }
            }
            catch (Exception ex)
            {
                _acdimpl.LogInformation("Exam Subject Group Mapping getsubject :" + ex.Message);
            }
            return data;
        }
        public ExamsubjectGroupMappingDTo savedetails(ExamsubjectGroupMappingDTo data)
        {
            try
            {
                if (data.ESG_Id > 0)
                {
                    if (data.Flag == "0")
                    {
                        List<long> temparr = new List<long>();
                        List<long> temparr1 = new List<long>();
                        //getting all mobilenumbers
                        foreach (get_subjectlist ph in data.get_subjectlist)
                        {
                            temparr.Add(ph.ISMS_Id);
                        }

                        //removing mobile number 
                        Array remove_subject = _context.ExamSubjectGroupMappingSubjectsDMO.Where(t => !temparr.Contains(t.ISMS_Id) && t.ESG_Id == data.ESG_Id).ToArray();
                        foreach (ExamSubjectGroupMappingSubjectsDMO ph1 in remove_subject)
                        {
                            _context.Remove(ph1);
                        }

                    }
                    else
                    {

                    }
                }
                else
                {
                    //-------------------Individual exam Wise Saving Record--------------------//
                    if (data.Flag == "0")
                    {
                        var check_groupname = (from a in _context.ExamsubjectGroupMappingDMO
                                               from b in _context.ExamSubjectGroupMappingExamsDMO
                                               where (a.ESG_Id == b.ESG_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == data.EMCA_Id && b.EME_Id == data.EME_Id && a.ESG_SubjectGroupName.Equals(data.grpname) && a.ESG_ActiveFlag == true && b.ESGE_ActiveFlag == true && a.ESG_ExamPromotionFlag == "IE")
                                               select new ExamsubjectGroupMappingDTo
                                               {
                                                   grpname = a.ESG_SubjectGroupName
                                               }).ToList();

                        if (check_groupname.Count > 0)
                        {
                            data.message = "Group Name Already Exist For This Combination";
                        }
                        else
                        {
                            try
                            {
                                ExamsubjectGroupMappingDMO exmsub = new ExamsubjectGroupMappingDMO();
                                ExamSubjectGroupMappingExamsDMO exmsubexam = new ExamSubjectGroupMappingExamsDMO();


                                exmsub.ASMAY_Id = data.ASMAY_Id;
                                exmsub.EMCA_Id = data.EMCA_Id;
                                exmsub.MI_Id = data.MI_Id;
                                exmsub.ESG_ActiveFlag = true;
                                exmsub.ESG_CompulsoryFlag = data.Compulsory1;
                                exmsub.ESG_ExamPromotionFlag = data.Promotion1;
                                exmsub.ESG_SubjectGroupName = data.grpname;
                                exmsub.ESG_GroupMinMarks = Convert.ToDecimal(data.percentage);
                                exmsub.ESG_GroupMaxMarks = data.ESG_GroupMaxMarks;
                                exmsub.CreatedDate = DateTime.Now;
                                exmsub.UpdatedDate = DateTime.Now;
                                _context.Add(exmsub);

                                exmsubexam.ESG_Id = exmsub.ESG_Id;
                                exmsubexam.EME_Id = data.EME_Id;
                                exmsubexam.ESGE_ActiveFlag = true;
                                exmsubexam.CreatedDate = DateTime.Now;
                                exmsubexam.UpdatedDate = DateTime.Now;
                                _context.Add(exmsubexam);

                                for (int ik = 0; ik < data.get_subjectlist.Length; ik++)
                                {
                                    ExamSubjectGroupMappingSubjectsDMO exmsubexamsubject = new ExamSubjectGroupMappingSubjectsDMO();

                                    exmsubexamsubject.ESG_Id = exmsub.ESG_Id;
                                    exmsubexamsubject.ISMS_Id = data.get_subjectlist[ik].ISMS_Id;
                                    exmsubexamsubject.ESGS_ActiveFlag = true;
                                    exmsubexamsubject.CreatedDate = DateTime.Now;
                                    exmsubexamsubject.UpdatedDate = DateTime.Now;
                                    _context.Add(exmsubexamsubject);
                                }

                                var ii = _context.SaveChanges();
                                if (ii > 0)
                                {
                                    data.message = "Record Saved Successfully";
                                }
                                else
                                {
                                    data.message = "Failed To Save Record";
                                }
                            }
                            catch (Exception ex)
                            {
                                _acdimpl.LogError(ex.Message);
                                _acdimpl.LogInformation("Exam Subject Group Mapping Individual examwise savedetails :" + ex.Message);
                            }
                        }
                    }
                    //-------------------Promotion Wise Saving Record--------------------//
                    else
                    {
                        try
                        {
                            var check_groupname = (from a in _context.ExamsubjectGroupMappingDMO
                                                   where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == data.EMCA_Id && a.ESG_SubjectGroupName.Equals(data.grpname) && a.ESG_ActiveFlag == true && a.ESG_ExamPromotionFlag == "PE")
                                                   select new ExamsubjectGroupMappingDTo
                                                   {
                                                       grpname = a.ESG_SubjectGroupName
                                                   }).ToList();

                            if (check_groupname.Count > 0)
                            {
                                data.message = "Group Name Already Exist For This Combination";
                            }
                            else
                            {
                                try
                                {
                                    ExamsubjectGroupMappingDMO exmsub = new ExamsubjectGroupMappingDMO();
                                    //   ExamSubjectGroupMappingSubjectsDMO exmsubexamsubject = new ExamSubjectGroupMappingSubjectsDMO();

                                    exmsub.ASMAY_Id = data.ASMAY_Id;
                                    exmsub.EMCA_Id = data.EMCA_Id;
                                    exmsub.MI_Id = data.MI_Id;
                                    exmsub.ESG_ActiveFlag = true;
                                    exmsub.ESG_CompulsoryFlag = data.Compulsory1;
                                    exmsub.ESG_ExamPromotionFlag = data.Promotion1;
                                    exmsub.ESG_SubjectGroupName = data.grpname;
                                    exmsub.ESG_GroupMinMarks = Convert.ToDecimal(data.percentage);
                                    exmsub.ESG_GroupMaxMarks = data.ESG_GroupMaxMarks;
                                    exmsub.CreatedDate = DateTime.Now;
                                    exmsub.UpdatedDate = DateTime.Now;
                                    _context.Add(exmsub);

                                    for (int ik = 0; ik < data.get_subjectlist.Length; ik++)
                                    {
                                        ExamSubjectGroupMappingSubjectsDMO exmsubexamsubject = new ExamSubjectGroupMappingSubjectsDMO();
                                        exmsubexamsubject.ESG_Id = exmsub.ESG_Id;
                                        exmsubexamsubject.ISMS_Id = data.get_subjectlist[ik].ISMS_Id;
                                        exmsubexamsubject.ESGS_ActiveFlag = true;
                                        exmsubexamsubject.CreatedDate = DateTime.Now;
                                        exmsubexamsubject.UpdatedDate = DateTime.Now;
                                        _context.Add(exmsubexamsubject);
                                    }

                                    var ii = _context.SaveChanges();
                                    if (ii > 0)
                                    {
                                        data.message = "Record Saved Successfully";
                                    }
                                    else
                                    {
                                        data.message = "Failed To Save Record";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _acdimpl.LogError(ex.Message);
                                    _acdimpl.LogInformation("Exam Subject Group Mapping Individual examwise savedetails :" + ex.Message);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            data.message = "Failed To Save /update  Record";
                            _acdimpl.LogError(ex.Message);
                            _acdimpl.LogInformation("Exam Subject Group Mapping Individual Promotion wise savedetails :" + ex.Message);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogInformation("Exam Subject Group Mapping savedetails :" + ex.Message);
            }
            return data;
        }
        public ExamsubjectGroupMappingDTo getlist(ExamsubjectGroupMappingDTo data)
        {
            try
            {
                if (data.Flag == "0")
                {
                    data.getdetails = (from a in _context.ExamsubjectGroupMappingDMO
                                       from b in _context.ExamSubjectGroupMappingExamsDMO
                                       from c in _context.ExamSubjectGroupMappingSubjectsDMO
                                       from d in _context.Exm_Master_CategoryDMO
                                       from e in _context.Exm_Yearly_CategoryDMO
                                       from f in _context.exammasterDMO
                                       from g in _context.AcademicYear
                                       where (a.ESG_Id == b.ESG_Id && a.ESG_Id == c.ESG_Id && a.EMCA_Id == e.EMCA_Id && d.EMCA_Id == e.EMCA_Id && b.EME_Id == f.EME_Id && a.MI_Id == data.MI_Id && a.ESG_ExamPromotionFlag == "IE" && g.ASMAY_Id == a.ASMAY_Id)
                                       select new ExamsubjectGroupMappingDTo
                                       {
                                           ASMAY_Id = a.ASMAY_Id,
                                           ASMAY_Year = g.ASMAY_Year,
                                           EMCA_CategoryName = d.EMCA_CategoryName,
                                           grpname = a.ESG_SubjectGroupName,
                                           percentage = Convert.ToString(a.ESG_GroupMinMarks),
                                           ESG_GroupMaxMarks = a.ESG_GroupMaxMarks,
                                           EME_ExamName = f.EME_ExamName,
                                           ESG_Id = a.ESG_Id,
                                           ESGE_Id = b.ESGE_Id,
                                           Compulsory1 = a.ESG_CompulsoryFlag,
                                           ASMAY_Order = g.ASMAY_Order
                                       }).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();
                }
                else
                {
                    data.getdetails = (from a in _context.ExamsubjectGroupMappingDMO
                                       from c in _context.ExamSubjectGroupMappingSubjectsDMO
                                       from d in _context.Exm_Master_CategoryDMO
                                       from e in _context.Exm_Yearly_CategoryDMO
                                       from f in _context.exammasterDMO
                                       from g in _context.AcademicYear
                                       where (a.ESG_Id == c.ESG_Id && a.EMCA_Id == e.EMCA_Id && d.EMCA_Id == e.EMCA_Id && a.MI_Id == data.MI_Id
                                       && a.ESG_ExamPromotionFlag == "PE" && g.ASMAY_Id == a.ASMAY_Id)
                                       select new ExamsubjectGroupMappingDTo
                                       {
                                           ASMAY_Id = a.ASMAY_Id,
                                           ASMAY_Year = g.ASMAY_Year,
                                           EMCA_CategoryName = d.EMCA_CategoryName,
                                           grpname = a.ESG_SubjectGroupName,
                                           percentage = Convert.ToString(a.ESG_GroupMinMarks),
                                           ESG_GroupMaxMarks = a.ESG_GroupMaxMarks,
                                           ESG_Id = a.ESG_Id,
                                           Compulsory1 = a.ESG_CompulsoryFlag,
                                           ASMAY_Order = g.ASMAY_Order
                                       }).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();
                }

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogInformation("Exam Subject Group Mapping getlist :" + ex.Message);
            }
            return data;
        }
        public ExamsubjectGroupMappingDTo Editexammasterdata1(ExamsubjectGroupMappingDTo data)
        {
            try
            {
                if (data.Flag2 == "N")
                {
                    data.editdata = (from a in _context.ExamsubjectGroupMappingDMO
                                     from b in _context.ExamSubjectGroupMappingExamsDMO
                                     from c in _context.ExamSubjectGroupMappingSubjectsDMO
                                     from d in _context.Exm_Master_CategoryDMO
                                     from e in _context.Exm_Yearly_CategoryDMO
                                     from f in _context.exammasterDMO
                                     from g in _context.IVRM_School_Master_SubjectsDMO
                                     where (a.ESG_Id == b.ESG_Id && a.ESG_Id == c.ESG_Id && a.EMCA_Id == e.EMCA_Id && d.EMCA_Id == e.EMCA_Id && b.EME_Id == f.EME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ESG_ExamPromotionFlag == "IE" && a.ESG_Id == data.ESG_Id && g.ISMS_Id == c.ISMS_Id)
                                     select new ExamsubjectGroupMappingDTo
                                     {
                                         EMCA_CategoryName = d.EMCA_CategoryName,
                                         grpname = a.ESG_SubjectGroupName,
                                         percentage = Convert.ToString(a.ESG_GroupMinMarks),
                                         ESG_GroupMaxMarks = a.ESG_GroupMaxMarks,
                                         EME_ExamName = f.EME_ExamName,
                                         ESG_Id = a.ESG_Id,
                                         ESGE_Id = b.ESGE_Id,
                                         Compulsory1 = a.ESG_CompulsoryFlag,
                                         ISMS_SubjectName = g.ISMS_SubjectName,
                                         ESGS_Id = c.ESGS_Id,
                                         ASMAY_Id = a.ASMAY_Id,
                                         EMCA_Id = a.EMCA_Id,
                                         EME_Id = b.EME_Id,
                                         ISMS_Id = c.ISMS_Id,
                                     }).Distinct().ToArray();
                }
                else
                {
                    data.editdata = (from a in _context.ExamsubjectGroupMappingDMO
                                     from c in _context.ExamSubjectGroupMappingSubjectsDMO
                                     from d in _context.Exm_Master_CategoryDMO
                                     from e in _context.Exm_Yearly_CategoryDMO

                                     from g in _context.IVRM_School_Master_SubjectsDMO
                                     where (a.ESG_Id == c.ESG_Id && a.EMCA_Id == e.EMCA_Id && d.EMCA_Id == e.EMCA_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ESG_ExamPromotionFlag == "PE" && a.ESG_Id == data.ESG_Id && g.ISMS_Id == c.ISMS_Id)
                                     select new ExamsubjectGroupMappingDTo
                                     {
                                         EMCA_CategoryName = d.EMCA_CategoryName,
                                         grpname = a.ESG_SubjectGroupName,
                                         percentage = Convert.ToString(a.ESG_GroupMinMarks),
                                         ESG_GroupMaxMarks = a.ESG_GroupMaxMarks,
                                         ESG_Id = a.ESG_Id,
                                         Compulsory1 = a.ESG_CompulsoryFlag,
                                         ISMS_SubjectName = g.ISMS_SubjectName,
                                         ESGS_Id = c.ESGS_Id,
                                         ASMAY_Id = a.ASMAY_Id,
                                         EMCA_Id = a.EMCA_Id,
                                         ISMS_Id = c.ISMS_Id,
                                     }).Distinct().ToArray();
                }

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogInformation("Exam Subject Group Mapping Editexammasterdata1 :" + ex.Message);
            }
            return data;
        }
        public ExamsubjectGroupMappingDTo viewrecordspopup(ExamsubjectGroupMappingDTo data)
        {
            try
            {
                if (data.Flag2 == "N")
                {
                    data.viewdata = (from a in _context.ExamsubjectGroupMappingDMO
                                     from b in _context.ExamSubjectGroupMappingExamsDMO
                                     from c in _context.ExamSubjectGroupMappingSubjectsDMO
                                     from d in _context.Exm_Master_CategoryDMO
                                     from e in _context.Exm_Yearly_CategoryDMO
                                     from f in _context.exammasterDMO
                                     from g in _context.IVRM_School_Master_SubjectsDMO
                                     where (a.ESG_Id == b.ESG_Id && a.ESG_Id == c.ESG_Id && a.EMCA_Id == e.EMCA_Id && d.EMCA_Id == e.EMCA_Id && b.EME_Id == f.EME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ESG_ExamPromotionFlag == "IE" && a.ESG_Id == data.ESG_Id && g.ISMS_Id == c.ISMS_Id)
                                     select new ExamsubjectGroupMappingDTo
                                     {
                                         EMCA_CategoryName = d.EMCA_CategoryName,
                                         grpname = a.ESG_SubjectGroupName,
                                         percentage = Convert.ToString(a.ESG_GroupMinMarks),
                                         ESG_GroupMaxMarks = a.ESG_GroupMaxMarks,
                                         EME_ExamName = f.EME_ExamName,
                                         ESG_Id = a.ESG_Id,
                                         ESGE_Id = b.ESGE_Id,
                                         Compulsory1 = a.ESG_CompulsoryFlag,
                                         ISMS_SubjectName = g.ISMS_SubjectName,
                                         ESGS_Id = c.ESGS_Id,
                                         ASMAY_Id = a.ASMAY_Id,
                                         EMCA_Id = a.EMCA_Id,
                                         EME_Id = b.EME_Id,
                                         ISMS_Id = c.ISMS_Id,
                                         ESGS_ActiveFlag = c.ESGS_ActiveFlag,
                                     }).Distinct().ToArray();
                }
                else
                {
                    data.viewdata = (from a in _context.ExamsubjectGroupMappingDMO
                                     from c in _context.ExamSubjectGroupMappingSubjectsDMO
                                     from d in _context.Exm_Master_CategoryDMO
                                     from e in _context.Exm_Yearly_CategoryDMO
                                     from g in _context.IVRM_School_Master_SubjectsDMO
                                     where (a.ESG_Id == c.ESG_Id && a.EMCA_Id == e.EMCA_Id && d.EMCA_Id == e.EMCA_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ESG_ExamPromotionFlag == "PE" && a.ESG_Id == data.ESG_Id && g.ISMS_Id == c.ISMS_Id)
                                     select new ExamsubjectGroupMappingDTo
                                     {
                                         EMCA_CategoryName = d.EMCA_CategoryName,
                                         grpname = a.ESG_SubjectGroupName,
                                         percentage = Convert.ToString(a.ESG_GroupMinMarks),
                                         ESG_GroupMaxMarks = a.ESG_GroupMaxMarks,
                                         ESG_Id = a.ESG_Id,
                                         Compulsory1 = a.ESG_CompulsoryFlag,
                                         ISMS_SubjectName = g.ISMS_SubjectName,
                                         ESGS_Id = c.ESGS_Id,
                                         ASMAY_Id = a.ASMAY_Id,
                                         EMCA_Id = a.EMCA_Id,
                                         ISMS_Id = c.ISMS_Id,
                                         ESGS_ActiveFlag = c.ESGS_ActiveFlag,
                                     }).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogInformation("Exam Subject Group Mapping viewrecordspopup :" + ex.Message);
            }
            return data;
        }
        public ExamsubjectGroupMappingDTo deactivate(ExamsubjectGroupMappingDTo data)
        {
            try
            {

                var result = _context.ExamSubjectGroupMappingSubjectsDMO.Single(a => a.ESGS_Id == data.ESGS_Id);
                if (result.ESGS_ActiveFlag == true)
                {
                    result.ESGS_ActiveFlag = false;
                }
                else
                {
                    result.ESGS_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                _context.Update(result);
                int ii = _context.SaveChanges();
                if (ii > 0)
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
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogInformation("Exam Subject Group Mapping deactivate :" + ex.Message);
            }
            return data;
        }
    }
}
