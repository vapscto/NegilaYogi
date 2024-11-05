using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.University;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Service
{
    public class NAAC_HSU_EvaluationRelated_253Impl:Interface.NAAC_HSU_EvaluationRelated_253Interface
    {
        public GeneralContext _context;
        public NAAC_HSU_EvaluationRelated_253Impl(GeneralContext y)
        {
            _context = y;
        }
        public NAAC_HSU_EvaluationRelated_253_DTO loaddata(NAAC_HSU_EvaluationRelated_253_DTO data)
        {
            try
            {
                var institutionlist = (from a in _context.Institution
                                       from b in _context.UserRoleWithInstituteDMO
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
                data.allacademicyear = _context.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).ToArray();

                data.alldata1 = (from a in _context.Academic
                                 from b in _context.NAAC_HSU_EvaluationRelated_253_DMO
                                 where (a.MI_Id == data.MI_Id && b.MI_Id == a.MI_Id && a.Is_Active == true && b.NCHSU253ER_Year == a.ASMAY_Id)
                                 select new NAAC_HSU_EvaluationRelated_253_DTO
                                 {
                                     NCHSU253ER_Id = b.NCHSU253ER_Id,
                                     NCHSU253ER_Year = b.NCHSU253ER_Year,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCHSU253ER_TotalNoOfStsAppreadFinalExm = b.NCHSU253ER_TotalNoOfStsAppreadFinalExm,
                                     NCHSU253ER_NoOfCasesSingleValuationAppProcRevaluation = b.NCHSU253ER_NoOfCasesSingleValuationAppProcRevaluation,
                                     NCHSU253ER_NoOfCasesDoubleAppProcRetotallingOnly = b.NCHSU253ER_NoOfCasesDoubleAppProcRetotallingOnly,
                                     NCHSU253ER_NoOfCasesDoubleAppProcRevaluationOnly = b.NCHSU253ER_NoOfCasesDoubleAppProcRevaluationOnly,
                                     NCHSU253ER_NoOfCasesDoubleAppProcRetotalORRevlAccToAnsScript = b.NCHSU253ER_NoOfCasesDoubleAppProcRetotalORRevlAccToAnsScript,
                                     NCHSU253ER_ActiveFlag = b.NCHSU253ER_ActiveFlag,
                                     MI_Id = data.MI_Id
                                 }).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_HSU_EvaluationRelated_253_DTO save(NAAC_HSU_EvaluationRelated_253_DTO data)
        {
            try
            {
                if (data.NCHSU253ER_Id == 0)
                {
                    var duplicate = _context.NAAC_HSU_EvaluationRelated_253_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCHSU253ER_Year == data.ASMAY_Id && t.NCHSU253ER_Id != 0 && t.NCHSU253ER_TotalNoOfStsAppreadFinalExm == data.NCHSU253ER_TotalNoOfStsAppreadFinalExm && t.NCHSU253ER_NoOfCasesSingleValuationAppProcRevaluation == data.NCHSU253ER_NoOfCasesSingleValuationAppProcRevaluation && t.NCHSU253ER_NoOfCasesDoubleAppProcRetotallingOnly == data.NCHSU253ER_NoOfCasesDoubleAppProcRetotallingOnly && t.NCHSU253ER_NoOfCasesDoubleAppProcRevaluationOnly == data.NCHSU253ER_NoOfCasesDoubleAppProcRevaluationOnly && t.NCHSU253ER_NoOfCasesDoubleAppProcRetotalORRevlAccToAnsScript == data.NCHSU253ER_NoOfCasesDoubleAppProcRetotalORRevlAccToAnsScript).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_HSU_EvaluationRelated_253_DMO rrr = new NAAC_HSU_EvaluationRelated_253_DMO();
                        rrr.MI_Id = data.MI_Id;
                        rrr.NCHSU253ER_TotalNoOfStsAppreadFinalExm = data.NCHSU253ER_TotalNoOfStsAppreadFinalExm;
                        rrr.NCHSU253ER_NoOfCasesSingleValuationAppProcRevaluation = data.NCHSU253ER_NoOfCasesSingleValuationAppProcRevaluation;
                        rrr.NCHSU253ER_NoOfCasesDoubleAppProcRetotallingOnly = data.NCHSU253ER_NoOfCasesDoubleAppProcRetotallingOnly;
                        rrr.NCHSU253ER_NoOfCasesDoubleAppProcRevaluationOnly = data.NCHSU253ER_NoOfCasesDoubleAppProcRevaluationOnly;
                        rrr.NCHSU253ER_NoOfCasesDoubleAppProcRetotalORRevlAccToAnsScript = data.NCHSU253ER_NoOfCasesDoubleAppProcRetotalORRevlAccToAnsScript;                       
                        rrr.NCHSU253ER_Year = data.ASMAY_Id;
                        rrr.NCHSU253ER_CreatedDate = DateTime.Now;
                        rrr.NCHSU253ER_UpdatedDate = DateTime.Now;
                        rrr.NCHSU253ER_CreatedBy = data.UserId;
                        rrr.NCHSU253ER_UpdatedBy = data.UserId;
                        rrr.NCHSU253ER_ActiveFlag = true;

                        _context.Add(rrr);
                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[0].cfilepath != null)
                                {
                                    NAAC_HSU_EvaluationRelated_253_Files_DMO obj2 = new NAAC_HSU_EvaluationRelated_253_Files_DMO();
                                    obj2.NCHSU253ER_Id = rrr.NCHSU253ER_Id;
                                    obj2.NCHSU253ERF_FileName = data.filelist[i].cfilename;
                                    obj2.NCHSU253ERF_Filedesc = data.filelist[i].cfiledesc;
                                    obj2.NCHSU253ERF_FilePath = data.filelist[i].cfilepath;
                                    _context.Add(obj2);
                                }
                            }
                        }
                        int y = _context.SaveChanges();
                        if (y > 0)
                        {
                            data.msg = "saved";
                        }
                        else
                        {
                            data.msg = "Failed";
                        }
                    }
                }
                else if (data.NCHSU253ER_Id > 0)
                {
                    var duplicate = _context.NAAC_HSU_EvaluationRelated_253_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCHSU253ER_TotalNoOfStsAppreadFinalExm == data.NCHSU253ER_TotalNoOfStsAppreadFinalExm && t.NCHSU253ER_NoOfCasesSingleValuationAppProcRevaluation == data.NCHSU253ER_NoOfCasesSingleValuationAppProcRevaluation && t.NCHSU253ER_NoOfCasesDoubleAppProcRetotallingOnly == data.NCHSU253ER_NoOfCasesDoubleAppProcRetotallingOnly && t.NCHSU253ER_NoOfCasesDoubleAppProcRevaluationOnly == data.NCHSU253ER_NoOfCasesDoubleAppProcRevaluationOnly && t.NCHSU253ER_NoOfCasesDoubleAppProcRetotalORRevlAccToAnsScript == data.NCHSU253ER_NoOfCasesDoubleAppProcRetotalORRevlAccToAnsScript && t.NCHSU253ER_Id != data.NCHSU253ER_Id && t.NCHSU253ER_Year == data.ASMAY_Id).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var yy = _context.NAAC_HSU_EvaluationRelated_253_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCHSU253ER_Id == data.NCHSU253ER_Id).SingleOrDefault();
                        yy.NCHSU253ER_TotalNoOfStsAppreadFinalExm = data.NCHSU253ER_TotalNoOfStsAppreadFinalExm;
                        yy.NCHSU253ER_NoOfCasesSingleValuationAppProcRevaluation = data.NCHSU253ER_NoOfCasesSingleValuationAppProcRevaluation;
                        yy.NCHSU253ER_NoOfCasesDoubleAppProcRetotallingOnly = data.NCHSU253ER_NoOfCasesDoubleAppProcRetotallingOnly;
                        yy.NCHSU253ER_NoOfCasesDoubleAppProcRevaluationOnly = data.NCHSU253ER_NoOfCasesDoubleAppProcRevaluationOnly;
                        yy.NCHSU253ER_NoOfCasesDoubleAppProcRetotalORRevlAccToAnsScript = data.NCHSU253ER_NoOfCasesDoubleAppProcRetotalORRevlAccToAnsScript;                        
                        yy.NCHSU253ER_Year = data.ASMAY_Id;
                        yy.MI_Id = data.MI_Id;
                        yy.NCHSU253ER_UpdatedDate = DateTime.Now;
                        yy.NCHSU253ER_UpdatedBy = data.UserId;
                        _context.Update(yy);
                        var CountRemoveFiles = _context.NAAC_HSU_EvaluationRelated_253_Files_DMO.Where(t => t.NCHSU253ER_Id == data.NCHSU253ER_Id).ToList();
                        if (CountRemoveFiles.Count > 0)
                        {
                            foreach (var RemoveFiles in CountRemoveFiles)
                            {
                                _context.Remove(RemoveFiles);
                            }
                            if (data.filelist.Length > 0)
                            {
                                for (int i = 0; i < data.filelist.Length; i++)
                                {
                                    if (data.filelist[0].cfilepath != null)
                                    {
                                        NAAC_HSU_EvaluationRelated_253_Files_DMO obj2 = new NAAC_HSU_EvaluationRelated_253_Files_DMO();
                                        obj2.NCHSU253ER_Id = yy.NCHSU253ER_Id;
                                        obj2.NCHSU253ERF_FileName = data.filelist[i].cfilename;
                                        obj2.NCHSU253ERF_Filedesc = data.filelist[i].cfiledesc;
                                        obj2.NCHSU253ERF_FilePath = data.filelist[i].cfilepath;
                                        _context.Add(obj2);
                                    }
                                }
                            }
                        }
                        else if (CountRemoveFiles.Count == 0)
                        {
                            if (data.filelist.Length > 0)
                            {
                                for (int i = 0; i < data.filelist.Length; i++)
                                {
                                    if (data.filelist[0].cfilepath != null)
                                    {
                                        NAAC_HSU_EvaluationRelated_253_Files_DMO obj2 = new NAAC_HSU_EvaluationRelated_253_Files_DMO();
                                        obj2.NCHSU253ER_Id = yy.NCHSU253ER_Id;
                                        obj2.NCHSU253ERF_FileName = data.filelist[i].cfilename;
                                        obj2.NCHSU253ERF_Filedesc = data.filelist[i].cfiledesc;
                                        obj2.NCHSU253ERF_FilePath = data.filelist[i].cfilepath;
                                        _context.Add(obj2);
                                    }
                                }
                            }
                        }
                        var r = _context.SaveChanges();
                        if (r > 0)
                        {
                            data.msg = "updated";
                        }
                        else
                        {
                            data.msg = "failed";
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_HSU_EvaluationRelated_253_DTO deactive(NAAC_HSU_EvaluationRelated_253_DTO data)
        {
            try
            {
                var u = _context.NAAC_HSU_EvaluationRelated_253_DMO.Where(t => t.NCHSU253ER_Id == data.NCHSU253ER_Id).SingleOrDefault();
                if (u.NCHSU253ER_ActiveFlag == true)
                {
                    u.NCHSU253ER_ActiveFlag = false;
                }
                else if (u.NCHSU253ER_ActiveFlag == false)
                {
                    u.NCHSU253ER_ActiveFlag = true;
                }
                u.NCHSU253ER_UpdatedDate = DateTime.Now;
                u.NCHSU253ER_UpdatedBy = data.UserId;
                u.MI_Id = data.MI_Id;
                _context.Update(u);
                int o = _context.SaveChanges();
                if (o > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NAAC_HSU_EvaluationRelated_253_DTO EditData(NAAC_HSU_EvaluationRelated_253_DTO data)
        {
            try
            {
                data.editlist = (from a in _context.Academic
                                 from b in _context.NAAC_HSU_EvaluationRelated_253_DMO
                                 where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCHSU253ER_Year && b.MI_Id == data.MI_Id && b.NCHSU253ER_Id == data.NCHSU253ER_Id)
                                 select new NAAC_HSU_EvaluationRelated_253_DTO
                                 {
                                     NCHSU253ER_Id = b.NCHSU253ER_Id,
                                     NCHSU253ER_TotalNoOfStsAppreadFinalExm = b.NCHSU253ER_TotalNoOfStsAppreadFinalExm,
                                     NCHSU253ER_NoOfCasesSingleValuationAppProcRevaluation = b.NCHSU253ER_NoOfCasesSingleValuationAppProcRevaluation,
                                     NCHSU253ER_NoOfCasesDoubleAppProcRetotallingOnly = b.NCHSU253ER_NoOfCasesDoubleAppProcRetotallingOnly,
                                     NCHSU253ER_NoOfCasesDoubleAppProcRevaluationOnly = b.NCHSU253ER_NoOfCasesDoubleAppProcRevaluationOnly,
                                     NCHSU253ER_NoOfCasesDoubleAppProcRetotalORRevlAccToAnsScript = b.NCHSU253ER_NoOfCasesDoubleAppProcRetotalORRevlAccToAnsScript,
                                     NCHSU253ER_Year = b.NCHSU253ER_Year,                                     
                                     MI_Id = data.MI_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();
                data.editFileslist = (from a in _context.NAAC_HSU_EvaluationRelated_253_Files_DMO
                                      where (a.NCHSU253ER_Id == data.NCHSU253ER_Id)
                                      select new NAAC_HSU_EvaluationRelated_253_DTO
                                      {
                                          cfilename = a.NCHSU253ERF_FileName,
                                          cfiledesc = a.NCHSU253ERF_Filedesc,
                                          cfilepath = a.NCHSU253ERF_FilePath,
                                      }).Distinct().ToArray();
            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAAC_HSU_EvaluationRelated_253_DTO viewuploadflies(NAAC_HSU_EvaluationRelated_253_DTO data)
        {
            try
            {
                data.viewuploadflies = (from a in _context.NAAC_HSU_EvaluationRelated_253_Files_DMO
                                        where (a.NCHSU253ER_Id == data.NCHSU253ER_Id)
                                        select new NAAC_HSU_EvaluationRelated_253_DTO
                                        {
                                            cfilename = a.NCHSU253ERF_FileName,
                                            cfiledesc = a.NCHSU253ERF_Filedesc,
                                            cfilepath = a.NCHSU253ERF_FilePath,
                                            NCHSU253ERF_Id = a.NCHSU253ERF_Id,
                                            NCHSU253ER_Id = a.NCHSU253ER_Id,
                                        }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_HSU_EvaluationRelated_253_DTO deleteuploadfile(NAAC_HSU_EvaluationRelated_253_DTO data)
        {
            try
            {
                var res = _context.NAAC_HSU_EvaluationRelated_253_Files_DMO.Where(t => t.NCHSU253ERF_Id == data.NCHSU253ERF_Id).SingleOrDefault();
                _context.Remove(res);
                int s = _context.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                data.viewuploadflies = (from a in _context.NAAC_HSU_EvaluationRelated_253_Files_DMO
                                        where (a.NCHSU253ER_Id == data.NCHSU253ER_Id)
                                        select new NAAC_HSU_EvaluationRelated_253_DTO
                                        {
                                            NCHSU253ERF_Id = a.NCHSU253ERF_Id,
                                            NCHSU253ER_Id = a.NCHSU253ER_Id,
                                            cfilename = a.NCHSU253ERF_FileName,
                                            cfiledesc = a.NCHSU253ERF_Filedesc,
                                            cfilepath = a.NCHSU253ERF_FilePath,
                                        }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
