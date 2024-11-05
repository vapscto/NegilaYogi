
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
using DomainModel.Model.com.vapstech.Exam;

namespace ExamServiceHub.com.vaps.Services
{
    public class ExamTermAndExamMappingImpl : Interfaces.ExamTermAndExamMappingInterface
    {
        private static ConcurrentDictionary<string, mastersubsubjectDTO> _login =
         new ConcurrentDictionary<string, mastersubsubjectDTO>();

        private readonly ExamContext _masterexamContext;

        public ExamTermAndExamMappingImpl(ExamContext masterexamContext)
        {
            _masterexamContext = masterexamContext;
        }

        public ExamTermAndExamMappingDTO Getdetails(ExamTermAndExamMappingDTO data)//int IVRMM_Id
        {
            // ExamTermAndExamMappingDTO getdata = new ExamTermAndExamMappingDTO();
            try
            {
                List<CCE_Exam_M_TermsDMO> list = new List<CCE_Exam_M_TermsDMO>();
                list = _masterexamContext.CCE_Exam_M_TermsDMO.Where(t => t.MI_Id == data.MI_Id && t.ECT_ActiveFlag == true).OrderByDescending(t => t.ECT_TermName).ToList();
                data.termlist = list.ToArray();

                List<CCE_Exam_M_TermsDMO> gridlist = new List<CCE_Exam_M_TermsDMO>();
                gridlist = _masterexamContext.CCE_Exam_M_TermsDMO.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.termgridlist = gridlist.ToArray();

                data.categorylist = _masterexamContext.Exm_Master_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.EMCA_ActiveFlag == true).ToArray();

                data.getyear = _masterexamContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.getgradelist = _masterexamContext.Exm_Master_GradeDMO.Where(a => a.MI_Id == data.MI_Id && a.EMGR_ActiveFlag == true).ToArray();


                data.mapgridlist = (from a in _masterexamContext.CCE_Exam_M_TermsDMO
                                    from b in _masterexamContext.Exm_CCE_TERMS_EXAMSDMO
                                    from d in _masterexamContext.Exm_Yearly_CategoryDMO
                                    from e in _masterexamContext.Exm_Master_CategoryDMO
                                    from f in _masterexamContext.AcademicYear
                                    from g in _masterexamContext.Exm_Master_GradeDMO
                                    where (a.ECT_Id == b.ECT_Id && a.EMCA_Id == e.EMCA_Id && a.EMGR_Id == g.EMGR_Id && a.ASMAY_Id == f.ASMAY_Id && a.MI_Id == data.MI_Id)
                                    select new ExamTermAndExamMappingDTO
                                    {
                                        ECT_Id = a.ECT_Id,
                                        ASMAY_Year = f.ASMAY_Year,
                                        ECT_TermName = a.ECT_TermName,
                                        EMCA_CategoryName = e.EMCA_CategoryName,
                                        ECT_Marks = a.ECT_Marks,
                                        ECT_MinMarks = a.ECT_MinMarks,
                                        ECT_ActiveFlag = a.ECT_ActiveFlag,
                                        ECT_TermEndDate = a.ECT_TermEndDate,
                                        ECT_TermStartDate = a.ECT_TermStartDate,
                                        ECT_PublishDate = a.ECT_PublishDate,
                                        ASMAY_Id = a.ASMAY_Id,
                                        EMCA_Id = a.EMCA_Id,
                                        EMGR_GradeName = g.EMGR_GradeName,
                                        EMGR_Id = a.EMGR_Id
                                    }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;

        }
        public ExamTermAndExamMappingDTO onchangeyear(ExamTermAndExamMappingDTO data)
        {
            try
            {
                data.categorylist = (from a in _masterexamContext.Exm_Yearly_CategoryDMO
                                     from b in _masterexamContext.Exm_Master_CategoryDMO
                                     from c in _masterexamContext.AcademicYear
                                     where (a.EMCA_Id == b.EMCA_Id && a.ASMAY_Id == c.ASMAY_Id && a.EYC_ActiveFlg == true && b.EMCA_ActiveFlag == true
                                     && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id)
                                     select b).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamTermAndExamMappingDTO onchangecategory(ExamTermAndExamMappingDTO data)
        {

            try
            {
                data.examlist = (from a in _masterexamContext.Exm_Yearly_CategoryDMO
                                 from b in _masterexamContext.Exm_Yearly_Category_ExamsDMO
                                 from c in _masterexamContext.Exm_Master_CategoryDMO
                                 from d in _masterexamContext.AcademicYear
                                 from e in _masterexamContext.exammasterDMO
                                 where (a.EYC_Id == b.EYC_Id && a.EMCA_Id == c.EMCA_Id && b.EME_Id == e.EME_Id && a.ASMAY_Id == d.ASMAY_Id && a.EYC_ActiveFlg == true
                                 && b.EYCE_ActiveFlg == true && c.EMCA_ActiveFlag == true && e.EME_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id
                                 && a.EMCA_Id == data.EMCA_Id && c.EMCA_Id == data.EMCA_Id)
                                 select e).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamTermAndExamMappingDTO checktermname(ExamTermAndExamMappingDTO data)
        {

            try
            {
                var checktermname = _masterexamContext.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == data.EMCA_Id
                && a.ECT_TermName.Equals(data.ECT_TermName)).ToArray();

                if (checktermname.Length > 0)
                {
                    data.message = "Term Name Already Exists";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamTermAndExamMappingDTO saveddata(ExamTermAndExamMappingDTO data)
        {
            try
            {
                if (data.saveddetails.Count() > 0)
                {
                    if (data.ECT_Id > 0)
                    {
                        var checkresult = _masterexamContext.CCE_Exam_M_TermsDMO.Single(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                        && a.EMCA_Id == data.EMCA_Id && a.ECT_Id == data.ECT_Id);

                        checkresult.UpdatedDate = DateTime.Now;
                        checkresult.EMGR_Id = data.EMGR_Id;
                        checkresult.ECT_PublishDate = data.ECT_PublishDate;
                        _masterexamContext.Update(checkresult);

                        List<long> fid = new List<long>();

                        foreach (var x in data.saveddetails)
                        {
                            fid.Add(x.ECTEX_Id);
                        }

                        var checknotincondition = _masterexamContext.Exm_CCE_TERMS_EXAMSDMO.Where(a => a.ECT_Id == data.ECT_Id && !fid.Contains(a.ECTEX_Id)).ToList();

                        foreach (var y in checknotincondition)
                        {
                            _masterexamContext.Remove(y);
                        }

                        for (int k = 0; k < data.saveddetails.Count(); k++)
                        {
                            var checkresultexistsornot = _masterexamContext.Exm_CCE_TERMS_EXAMSDMO.Where(a => a.ECT_Id == data.ECT_Id
                            && a.ECTEX_Id == data.saveddetails[k].ECTEX_Id && a.EME_Id == data.saveddetails[k].examid).ToList();

                            if (checkresultexistsornot.Count() > 0)
                            {
                                var checkresultmapping = _masterexamContext.Exm_CCE_TERMS_EXAMSDMO.Single(a => a.ECT_Id == data.ECT_Id
                                && a.ECTEX_Id == data.saveddetails[k].ECTEX_Id && a.EME_Id == data.saveddetails[k].examid);
                                checkresultmapping.ECTEX_RoundOffReqFlg = data.saveddetails[k].roundofflag;
                                checkresultmapping.ECTEX_ConversionReqFlg = data.saveddetails[k].converstionreqflag;
                                checkresultmapping.ECTEX_MarksPercentValue = data.saveddetails[k].marksorpercentage;
                                checkresultmapping.ECTEX_MarksPerFlag = data.saveddetails[k].marksorpercentageflag;
                                checkresultmapping.ECTEX_NotApplToTotalFlg = data.saveddetails[k].ECTEX_NotApplToTotalFlg;
                                checkresultmapping.UpdatedDate = DateTime.Now;
                                _masterexamContext.Update(checkresultmapping);
                            }
                            else
                            {
                                Exm_CCE_TERMS_EXAMSDMO dmoexam = new Exm_CCE_TERMS_EXAMSDMO();
                                dmoexam.ECT_Id = data.ECT_Id;
                                dmoexam.EME_Id = data.saveddetails[k].examid;
                                dmoexam.ECTEX_RoundOffReqFlg = data.saveddetails[k].roundofflag;
                                dmoexam.ECTEX_ConversionReqFlg = data.saveddetails[k].converstionreqflag;
                                dmoexam.ECTEX_MarksPercentValue = data.saveddetails[k].marksorpercentage;
                                dmoexam.ECTEX_MarksPerFlag = data.saveddetails[k].marksorpercentageflag;
                                dmoexam.ECTEX_NotApplToTotalFlg = data.saveddetails[k].ECTEX_NotApplToTotalFlg;
                                dmoexam.ECTEX_ActiveFlag = true;
                                dmoexam.CreatedDate = DateTime.Now;
                                dmoexam.UpdatedDate = DateTime.Now;
                                _masterexamContext.Add(dmoexam);
                            }
                        }
                    }
                    else
                    {
                        CCE_Exam_M_TermsDMO dmo = new CCE_Exam_M_TermsDMO();
                        dmo.MI_Id = data.MI_Id;
                        dmo.ASMAY_Id = data.ASMAY_Id;
                        dmo.EMCA_Id = data.EMCA_Id;
                        dmo.ECT_TermName = data.ECT_TermName;
                        dmo.ECT_TermStartDate = data.ECT_TermStartDate;
                        dmo.ECT_TermEndDate = data.ECT_TermEndDate;
                        dmo.ECT_PublishDate = data.ECT_PublishDate;
                        dmo.ECT_ActiveFlag = true;
                        dmo.EMGR_Id = data.EMGR_Id;
                        dmo.CreatedDate = DateTime.Now;
                        dmo.UpdatedDate = DateTime.Now;
                        dmo.ECT_Marks = data.ECT_Marks;
                        dmo.ECT_MinMarks = data.ECT_MinMarks;
                        _masterexamContext.Add(dmo);

                        for (int k = 0; k < data.saveddetails.Count(); k++)
                        {
                            Exm_CCE_TERMS_EXAMSDMO dmoexam = new Exm_CCE_TERMS_EXAMSDMO();
                            dmoexam.ECT_Id = dmo.ECT_Id;
                            dmoexam.EME_Id = data.saveddetails[k].examid;
                            dmoexam.ECTEX_RoundOffReqFlg = data.saveddetails[k].roundofflag;
                            dmoexam.ECTEX_ConversionReqFlg = data.saveddetails[k].converstionreqflag;
                            dmoexam.ECTEX_MarksPercentValue = data.saveddetails[k].marksorpercentage;
                            dmoexam.ECTEX_MarksPerFlag = data.saveddetails[k].marksorpercentageflag;
                            dmoexam.ECTEX_NotApplToTotalFlg = data.saveddetails[k].ECTEX_NotApplToTotalFlg;
                            dmoexam.ECTEX_ActiveFlag = true;
                            dmoexam.CreatedDate = DateTime.Now;
                            dmoexam.UpdatedDate = DateTime.Now;
                            _masterexamContext.Add(dmoexam);
                        }
                    }

                    int l = _masterexamContext.SaveChanges();
                    if (l > 0)
                    {
                        data.message = "Add";
                        data.returnval = true;
                    }
                    else
                    {
                        data.message = "Add";
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                data.message = "Add";
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamTermAndExamMappingDTO editdetailsnew(ExamTermAndExamMappingDTO data)
        {
            try
            {
                data.editlist = _masterexamContext.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                && a.EMCA_Id == data.EMCA_Id && a.ECT_Id == data.ECT_Id).ToArray();

                data.getexamlist = (from a in _masterexamContext.CCE_Exam_M_TermsDMO
                                    from b in _masterexamContext.Exm_CCE_TERMS_EXAMSDMO
                                    from c in _masterexamContext.AcademicYear
                                    from d in _masterexamContext.Exm_Master_CategoryDMO
                                    from e in _masterexamContext.masterexam
                                    where (a.ECT_Id == b.ECT_Id && a.ASMAY_Id == c.ASMAY_Id && a.EMCA_Id == d.EMCA_Id && b.EME_Id == e.EME_Id
                                    && a.ECT_Id == data.ECT_Id && b.ECT_Id == data.ECT_Id && a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == data.EMCA_Id)
                                    select new ExamTermAndExamMappingDTO
                                    {
                                        ECTEX_Id = b.ECTEX_Id,
                                        EME_Id = b.EME_Id,
                                        EME_ExamName = e.EME_ExamName,
                                        ECTEX_MarksPercentValue = b.ECTEX_MarksPercentValue,
                                        ECTEX_MarksPerFlag = b.ECTEX_MarksPerFlag,
                                        ECTEX_NotApplToTotalFlg = b.ECTEX_NotApplToTotalFlg,
                                        ECTEX_ActiveFlag = b.ECTEX_ActiveFlag,
                                        EME_ExamOrder = e.EME_ExamOrder,
                                        ECTEX_RoundOffReqFlg = b.ECTEX_RoundOffReqFlg,
                                        ECTEX_ConversionReqFlg = b.ECTEX_ConversionReqFlg
                                    }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamTermAndExamMappingDTO viewrecordspopup(ExamTermAndExamMappingDTO data)
        {
            try
            {
                var getviewdetails = (from a in _masterexamContext.CCE_Exam_M_TermsDMO
                                      from b in _masterexamContext.Exm_CCE_TERMS_EXAMSDMO
                                      from c in _masterexamContext.AcademicYear
                                      from d in _masterexamContext.Exm_Master_CategoryDMO
                                      from e in _masterexamContext.masterexam
                                      where (a.ECT_Id == b.ECT_Id && a.ASMAY_Id == c.ASMAY_Id && a.EMCA_Id == d.EMCA_Id && b.EME_Id == e.EME_Id
                                      && a.ECT_Id == data.ECT_Id && b.ECT_Id == data.ECT_Id && a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == data.EMCA_Id)
                                      select new ExamTermAndExamMappingDTO
                                      {
                                          ECTEX_Id = b.ECTEX_Id,
                                          EME_Id = b.EME_Id,
                                          EME_ExamName = e.EME_ExamName,
                                          ECTEX_MarksPercentValue = b.ECTEX_MarksPercentValue,
                                          ECTEX_MarksPerFlag = b.ECTEX_MarksPerFlag,
                                          ECTEX_NotApplToTotalFlg = b.ECTEX_NotApplToTotalFlg,
                                          ECTEX_ActiveFlag = b.ECTEX_ActiveFlag,
                                          EME_ExamOrder = e.EME_ExamOrder,
                                          ECT_Id = b.ECT_Id,
                                          ECTEX_RoundOffReqFlg = b.ECTEX_RoundOffReqFlg,
                                          ECTEX_ConversionReqFlg = b.ECTEX_ConversionReqFlg
                                      }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                data.viewdetails = getviewdetails.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamTermAndExamMappingDTO deactivatenew(ExamTermAndExamMappingDTO data)
        {
            try
            {
                int activetrue = 0;
                int deactivetrue = 0;

                var checkresult = _masterexamContext.CCE_Exam_M_TermsDMO.Single(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.EMCA_Id == data.EMCA_Id && a.ECT_Id == data.ECT_Id);
                if (checkresult.ECT_ActiveFlag == true)
                {
                    activetrue = activetrue + 1;
                    checkresult.ECT_ActiveFlag = false;
                }
                else
                {
                    deactivetrue = deactivetrue + 1;
                    checkresult.ECT_ActiveFlag = true;
                }
                checkresult.UpdatedDate = DateTime.Now;
                _masterexamContext.Update(checkresult);

                var checklistcount = _masterexamContext.Exm_CCE_TERMS_EXAMSDMO.Where(a => a.ECT_Id == data.ECT_Id).ToList();

                foreach (var x in checklistcount)
                {
                    var result = _masterexamContext.Exm_CCE_TERMS_EXAMSDMO.Single(a => a.ECT_Id == data.ECT_Id && a.ECTEX_Id == x.ECTEX_Id);
                    if (result.ECTEX_ActiveFlag == true)
                    {
                        if (deactivetrue == 0)
                        {
                            result.ECTEX_ActiveFlag = false;
                        }
                    }
                    else
                    {
                        if (activetrue == 0)
                        {
                            result.ECTEX_ActiveFlag = true;
                        }
                    }
                    result.UpdatedDate = DateTime.Now;
                    _masterexamContext.Update(result);
                }

                var i = _masterexamContext.SaveChanges();
                if (i > 0)
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
                data.message = "Add";
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamTermAndExamMappingDTO deactivesub(ExamTermAndExamMappingDTO data)
        {
            try
            {
                var checklistcount = _masterexamContext.Exm_CCE_TERMS_EXAMSDMO.Where(a => a.ECT_Id == data.ECT_Id && a.ECTEX_Id == data.ECTEX_Id).ToList();

                foreach (var x in checklistcount)
                {
                    var result = _masterexamContext.Exm_CCE_TERMS_EXAMSDMO.Single(a => a.ECT_Id == data.ECT_Id && a.ECTEX_Id == x.ECTEX_Id);
                    if (result.ECTEX_ActiveFlag == true)
                    {
                        result.ECTEX_ActiveFlag = false;
                    }
                    else
                    {
                        result.ECTEX_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;
                    _masterexamContext.Update(result);
                }

                var i = _masterexamContext.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }

                var checktotalcount = _masterexamContext.Exm_CCE_TERMS_EXAMSDMO.Where(a => a.ECT_Id == data.ECT_Id).ToList();

                var checktotaldeactivecount = _masterexamContext.Exm_CCE_TERMS_EXAMSDMO.Where(a => a.ECT_Id == data.ECT_Id && a.ECTEX_ActiveFlag == false).ToList();

                var checktotalactivecount = _masterexamContext.Exm_CCE_TERMS_EXAMSDMO.Where(a => a.ECT_Id == data.ECT_Id && a.ECTEX_ActiveFlag == true).ToList();

                if (checktotalcount.Count == checktotaldeactivecount.Count)
                {
                    var resultmain = _masterexamContext.CCE_Exam_M_TermsDMO.Single(a => a.ECT_Id == data.ECT_Id && a.MI_Id == data.MI_Id);
                    if (resultmain.ECT_ActiveFlag == true)
                    {
                        resultmain.ECT_ActiveFlag = false;
                    }
                    else
                    {
                        resultmain.ECT_ActiveFlag = true;
                    }
                    resultmain.UpdatedDate = DateTime.Now;
                    _masterexamContext.Update(resultmain);
                    var i1 = _masterexamContext.SaveChanges();
                    if (i1 > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = true;
                    }
                }
                if (checktotalcount.Count == checktotalactivecount.Count)
                {
                    var resultmain = _masterexamContext.CCE_Exam_M_TermsDMO.Single(a => a.ECT_Id == data.ECT_Id && a.MI_Id == data.MI_Id);
                    if (resultmain.ECT_ActiveFlag == true)
                    {
                        resultmain.ECT_ActiveFlag = false;
                    }
                    else
                    {
                        resultmain.ECT_ActiveFlag = true;
                    }
                    resultmain.UpdatedDate = DateTime.Now;
                    _masterexamContext.Update(resultmain);
                    var i1 = _masterexamContext.SaveChanges();
                    if (i1 > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = true;
                    }
                }

                var getviewdetails = (from a in _masterexamContext.CCE_Exam_M_TermsDMO
                                      from b in _masterexamContext.Exm_CCE_TERMS_EXAMSDMO
                                      from c in _masterexamContext.AcademicYear
                                      from d in _masterexamContext.Exm_Master_CategoryDMO
                                      from e in _masterexamContext.masterexam
                                      where (a.ECT_Id == b.ECT_Id && a.ASMAY_Id == c.ASMAY_Id && a.EMCA_Id == d.EMCA_Id && b.EME_Id == e.EME_Id
                                      && a.ECT_Id == data.ECT_Id && b.ECT_Id == data.ECT_Id)
                                      select new ExamTermAndExamMappingDTO
                                      {
                                          ECTEX_Id = b.ECTEX_Id,
                                          EME_Id = b.EME_Id,
                                          EME_ExamName = e.EME_ExamName,
                                          ECTEX_MarksPercentValue = b.ECTEX_MarksPercentValue,
                                          ECTEX_MarksPerFlag = b.ECTEX_MarksPerFlag,
                                          ECTEX_NotApplToTotalFlg = b.ECTEX_NotApplToTotalFlg,
                                          ECTEX_ActiveFlag = b.ECTEX_ActiveFlag,
                                          EME_ExamOrder = e.EME_ExamOrder,
                                          ECT_Id = b.ECT_Id
                                      }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                data.viewdetails = getviewdetails.ToArray();

                data.mapgridlist = (from a in _masterexamContext.CCE_Exam_M_TermsDMO
                                    from b in _masterexamContext.Exm_CCE_TERMS_EXAMSDMO
                                    from d in _masterexamContext.Exm_Yearly_CategoryDMO
                                    from e in _masterexamContext.Exm_Master_CategoryDMO
                                    from f in _masterexamContext.AcademicYear
                                    from g in _masterexamContext.Exm_Master_GradeDMO
                                    where (a.ECT_Id == b.ECT_Id && a.EMCA_Id == e.EMCA_Id && a.ASMAY_Id == f.ASMAY_Id && a.EMGR_Id == g.EMGR_Id && a.MI_Id == data.MI_Id)
                                    select new ExamTermAndExamMappingDTO
                                    {
                                        ECT_Id = a.ECT_Id,
                                        ASMAY_Year = f.ASMAY_Year,
                                        ECT_TermName = a.ECT_TermName,
                                        EMCA_CategoryName = e.EMCA_CategoryName,
                                        ECT_Marks = a.ECT_Marks,
                                        ECT_MinMarks = a.ECT_MinMarks,
                                        ECT_ActiveFlag = a.ECT_ActiveFlag,
                                        ECT_TermEndDate = a.ECT_TermEndDate,
                                        ECT_TermStartDate = a.ECT_TermStartDate,
                                        ECT_PublishDate = a.ECT_PublishDate,
                                        ASMAY_Id = a.ASMAY_Id,
                                        EMCA_Id = a.EMCA_Id,
                                        EMGR_Id = a.EMGR_Id,
                                        EMGR_GradeName = g.EMGR_GradeName,
                                    }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                data.message = "Add";
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        //* Dont Refer This Code *//
        public ExamTermAndExamMappingDTO savetermmap(ExamTermAndExamMappingDTO data)
        {
            try
            {

                var result = _masterexamContext.CCE_Exam_Term_MappingDMO.Where(t => t.ECTMP_Id == data.ECTMP_Id).ToList();
                if (result.Count > 0)
                {
                    if (data.EME_Ids.Length > 0)
                    {
                        //CCE_Exam_Term_MappingDMO exmap = new CCE_Exam_Term_MappingDMO();
                        var exmap = _masterexamContext.CCE_Exam_Term_MappingDMO.Single(t => t.ECTMP_Id == data.ECTMP_Id);
                        exmap.ECT_Id = data.ECT_Id;
                        exmap.EMCA_Id = data.EMCA_Id;
                        exmap.ASMAY_Id = data.ASMAY_Id;
                        exmap.ECTMP_Name = data.ECTMP_Name;
                        exmap.ECTMP_MarksPerFlag = data.ECTMP_MarksPerFlag;
                        //   exmap.ECTMP_MarksPercentValue = data.ECTMP_MarksPercentValue;
                        exmap.ECTMP_ActiveFlag = true;
                        //   exmap.ECTMP_TermEndDate = data.ECTMP_TermEndDate;
                        //   exmap.ECTMP_TermStartDate = data.ECTMP_TermStartDate;
                        exmap.CreatedDate = DateTime.Now;
                        exmap.UpdatedDate = DateTime.Now;
                        _masterexamContext.Update(exmap);

                        for (var i = 0; i < data.EME_Ids.Length; i++)
                        {
                            var exmap1 = _masterexamContext.Exm_CCE_TERMS_MP_EXAMSDMO.Single(t => t.ECTMPE_Id == data.ECTMPE_Id);
                            exmap1.EME_ID = Convert.ToInt32(data.EME_Ids[i]);
                            exmap1.ECTMP_Id = exmap.ECTMP_Id;
                            exmap1.ECTMPE_ActiveFlag = true;
                            exmap1.CreatedDate = DateTime.Now;
                            exmap1.UpdatedDate = DateTime.Now;
                            _masterexamContext.Update(exmap1);
                        }
                        var contactExists = _masterexamContext.SaveChanges();
                        if (contactExists > 0)
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
                        data.returnduplicatestatus = "Exam is not Selected";
                    }
                }
                else
                {
                    if (data.EME_Ids.Length > 0)
                    {
                        CCE_Exam_Term_MappingDMO exmap = new CCE_Exam_Term_MappingDMO();
                        exmap.ECT_Id = data.ECT_Id;
                        exmap.EMCA_Id = data.EMCA_Id;
                        exmap.ASMAY_Id = data.ASMAY_Id;
                        exmap.ECTMP_Name = data.ECTMP_Name;
                        exmap.ECTMP_MarksPerFlag = data.ECTMP_MarksPerFlag;
                        //exmap.ECTMP_MarksPercentValue = data.ECTMP_MarksPercentValue;
                        //exmap.ECTMP_TermStartDate = data.ECTMP_TermStartDate;
                        //exmap.ECTMP_TermEndDate = data.ECTMP_TermEndDate;
                        exmap.ECTMP_ActiveFlag = true;
                        exmap.CreatedDate = DateTime.Now;
                        exmap.UpdatedDate = DateTime.Now;
                        _masterexamContext.Add(exmap);

                        for (var i = 0; i < data.EME_Ids.Length; i++)
                        {
                            Exm_CCE_TERMS_MP_EXAMSDMO exmap1 = new Exm_CCE_TERMS_MP_EXAMSDMO();
                            exmap1.EME_ID = Convert.ToInt32(data.EME_Ids[i]);
                            exmap1.ECTMP_Id = exmap.ECTMP_Id;
                            exmap1.ECTMPE_ActiveFlag = true;
                            exmap1.CreatedDate = DateTime.Now;
                            exmap1.UpdatedDate = DateTime.Now;
                            _masterexamContext.Add(exmap1);
                        }
                        var contactExists = _masterexamContext.SaveChanges();
                        if (contactExists > 0)
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
                        data.returnduplicatestatus = "Exam is not Selected";
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
        public ExamTermAndExamMappingDTO savedetail(ExamTermAndExamMappingDTO data)
        {
            ExamTermAndExamMappingDTO savedata = new ExamTermAndExamMappingDTO();
            try
            {


                if (data.ECT_Id != 0)
                {
                    var res = _masterexamContext.CCE_Exam_M_TermsDMO.Where(t => t.ECT_TermName == data.ECT_TermName && t.ECT_Id != data.ECT_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _masterexamContext.CCE_Exam_M_TermsDMO.Single(t => t.ECT_Id == data.ECT_Id);
                        result.ECT_TermName = data.ECT_TermName;
                        _masterexamContext.Update(result);
                        var contactExists = _masterexamContext.SaveChanges();
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
                    //  var res = _masterexamContext.masterexam.Where(t => t.MI_Id == data.MI_Id  && t.EME_ExamName == data.EME_ExamName && t.EME_ExamCode == data.EME_ExamCode).ToList();
                    var res = _masterexamContext.CCE_Exam_M_TermsDMO.Where(t => (t.ECT_TermName == data.ECT_TermName)).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var row_cnt = _masterexamContext.CCE_Exam_M_TermsDMO.Where(t => t.ECT_Id == data.ECT_Id).ToList().Count;
                        CCE_Exam_M_TermsDMO exm = new CCE_Exam_M_TermsDMO();

                        exm.ECT_TermName = data.ECT_TermName;
                        exm.MI_Id = data.MI_Id;
                        exm.ECT_ActiveFlag = true;
                        exm.CreatedDate = DateTime.Now;
                        exm.UpdatedDate = DateTime.Now;

                        _masterexamContext.Add(exm);
                        var contactExists = _masterexamContext.SaveChanges();
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }
        public ExamTermAndExamMappingDTO deleterec(int id)//int IVRMM_Id
        {

            ExamTermAndExamMappingDTO getdata = new ExamTermAndExamMappingDTO();
            return getdata;
        }
        public ExamTermAndExamMappingDTO ontermchange(ExamTermAndExamMappingDTO data)
        {
            try
            {
                data.editlist = _masterexamContext.CCE_Exam_Term_MappingDMO.Where(t => t.ECT_Id == data.ECT_Id).ToArray();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
        public ExamTermAndExamMappingDTO get_exam(ExamTermAndExamMappingDTO data)
        {
            try
            {
                data.editlist = (from a in _masterexamContext.masterexam
                                 from b in _masterexamContext.Exm_Yearly_CategoryDMO
                                 from c in _masterexamContext.Exm_Yearly_Category_ExamsDMO
                                 from d in _masterexamContext.Exm_Master_CategoryDMO
                                 where (a.EME_Id == c.EME_Id && b.MI_Id == a.MI_Id && d.EMCA_Id == b.EMCA_Id && b.EYC_Id == c.EYC_Id && b.EMCA_Id == data.EYC_Id
                                 && b.ASMAY_Id == data.ASMAY_Id && d.EMCA_ActiveFlag == true && c.EYCE_ActiveFlg == true && b.EYC_ActiveFlg == true
                                 && a.EME_ActiveFlag == true)
                                 select new ExamTermAndExamMappingDTO
                                 {
                                     EME_Id = Convert.ToInt32(a.EME_Id),
                                     EME_ExamName = a.EME_ExamName,
                                     EME_ExamOrder = a.EME_ExamOrder

                                 }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                data.examnamelist = (from a in _masterexamContext.masterexam
                                     from b in _masterexamContext.Exm_Yearly_CategoryDMO
                                     from c in _masterexamContext.Exm_Yearly_Category_ExamsDMO
                                     from d in _masterexamContext.Exm_Master_CategoryDMO
                                     where (a.EME_Id == c.EME_Id && b.MI_Id == a.MI_Id && d.EMCA_Id == b.EMCA_Id && b.EYC_Id == c.EYC_Id && b.EMCA_Id == data.EYC_Id
                                     && b.ASMAY_Id == data.ASMAY_Id && d.EMCA_ActiveFlag == true && c.EYCE_ActiveFlg == true && b.EYC_ActiveFlg == true
                                 && a.EME_ActiveFlag == true)
                                     select new ExamTermAndExamMappingDTO
                                     {
                                         EME_Id = Convert.ToInt32(a.EME_Id),
                                         EME_ExamName = a.EME_ExamName,
                                         EME_ExamOrder = a.EME_ExamOrder
                                     }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
        public ExamTermAndExamMappingDTO getexampopup(int ID)
        {
            ExamTermAndExamMappingDTO data = new ExamTermAndExamMappingDTO();
            try
            {
                data.exampopup = (from a in _masterexamContext.Exm_CCE_TERMS_MP_EXAMSDMO
                                  from b in _masterexamContext.CCE_Exam_Term_MappingDMO
                                  from c in _masterexamContext.masterexam
                                  where (a.EME_ID == c.EME_Id && a.ECTMP_Id == b.ECTMP_Id && b.ECTMP_Id == ID)
                                  select new ExamTermAndExamMappingDTO
                                  {
                                      ECTMPE_Id = a.ECTMPE_Id,
                                      EME_ID = a.EME_ID,
                                      EME_ExamName = c.EME_ExamName,
                                      ECTMPE_ActiveFlag = a.ECTMPE_ActiveFlag

                                  }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return data;
        }
        public ExamTermAndExamMappingDTO editdetails(int ID)
        {
            ExamTermAndExamMappingDTO editlt = new ExamTermAndExamMappingDTO();
            try
            {
                List<CCE_Exam_M_TermsDMO> list = new List<CCE_Exam_M_TermsDMO>();
                list = _masterexamContext.CCE_Exam_M_TermsDMO.Where(t => t.ECT_Id == ID).ToList();
                editlt.editlist = list.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return editlt;
        }
        public ExamTermAndExamMappingDTO edittermmap(int ID)
        {
            ExamTermAndExamMappingDTO editlt = new ExamTermAndExamMappingDTO();
            try
            {
                editlt.editlist = (from b in _masterexamContext.CCE_Exam_Term_MappingDMO
                                   where (b.ECTMP_Id == ID)
                                   select new ExamTermAndExamMappingDTO
                                   {
                                       ASMAY_Id = b.ASMAY_Id,
                                       EMCA_Id = b.EMCA_Id,
                                       ECTMP_Name = b.ECTMP_Name,
                                       ECT_Id = b.ECT_Id,
                                       //ECTMP_MarksPercentValue = b.ECTMP_MarksPercentValue,
                                       ECTMP_MarksPerFlag = b.ECTMP_MarksPerFlag,
                                       ECTMP_Id = b.ECTMP_Id,
                                       //ECTMP_TermStartDate = b.ECTMP_TermStartDate,
                                       //ECTMP_TermEndDate = b.ECTMP_TermEndDate,
                                   }).Distinct().ToArray();


                editlt.editexmlist = (from a in _masterexamContext.Exm_CCE_TERMS_MP_EXAMSDMO
                                      from b in _masterexamContext.CCE_Exam_Term_MappingDMO
                                      from c in _masterexamContext.masterexam
                                      where (a.EME_ID == c.EME_Id && a.ECTMP_Id == b.ECTMP_Id && b.ECTMP_Id == ID)
                                      select new ExamTermAndExamMappingDTO
                                      {
                                          ECTMPE_Id = a.ECTMPE_Id,
                                          EME_ID = a.EME_ID,
                                          EME_ExamName = c.EME_ExamName,
                                          ECTMPE_ActiveFlag = a.ECTMPE_ActiveFlag,
                                          ECTMP_Id = b.ECTMP_Id
                                      }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return editlt;
        }
        public ExamTermAndExamMappingDTO deactivate1(ExamTermAndExamMappingDTO data)
        {
            data.already_cnt = false;
            if (data.ECTMP_Id > 0)
            {
                var result = _masterexamContext.CCE_Exam_Term_MappingDMO.Single(t => t.ECTMP_Id == data.ECTMP_Id);
                if (result.ECTMP_ActiveFlag == true)
                {
                    result.ECTMP_ActiveFlag = false;

                }
                else
                {
                    result.ECTMP_ActiveFlag = true;

                }
                _masterexamContext.Update(result);
                var flag = _masterexamContext.SaveChanges();
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
        public ExamTermAndExamMappingDTO deactivate(ExamTermAndExamMappingDTO data)
        {
            data.already_cnt = false;
            //exammasterDTO deact = new exammasterDTO();
            //  CCE_Exam_M_TermsDMO master = Mapper.Map<CCE_Exam_M_TermsDMO>(data);
            if (data.ECT_Id > 0)
            {
                var result = _masterexamContext.CCE_Exam_M_TermsDMO.Single(t => t.ECT_Id == data.ECT_Id && t.MI_Id == data.MI_Id);
                if (result.ECT_ActiveFlag == true)
                {
                    result.ECT_ActiveFlag = false;

                }
                else
                {
                    result.ECT_ActiveFlag = true;

                }
                _masterexamContext.Update(result);
                var flag = _masterexamContext.SaveChanges();
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
        public ExamTermAndExamMappingDTO deactive_sub(ExamTermAndExamMappingDTO data)
        {
            data.already_cnt = false;
            if (data.ECTMPE_Id > 0)
            {
                var result = _masterexamContext.Exm_CCE_TERMS_MP_EXAMSDMO.Single(t => t.ECTMPE_Id == data.ECTMPE_Id);
                if (result.ECTMPE_ActiveFlag == true)
                {
                    result.ECTMPE_ActiveFlag = false;

                }
                else
                {
                    result.ECTMPE_ActiveFlag = true;

                }
                _masterexamContext.Update(result);
                var flag = _masterexamContext.SaveChanges();
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
        //* Dont Refer This Code *//

    }
}
