using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NaacExpenditure424Impl : Interface.NaacExpenditure424Interface
    {
        public GeneralContext _GeneralContext;
        public NaacExpenditure424Impl(GeneralContext parameter)
        {
            _GeneralContext = parameter;
        }

        public NaacExpenditure424DTO save(NaacExpenditure424DTO data)
        {
            try
            {
                if (data.NCAC424EXP_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_424_Expenditure_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC424EXP_Id != 0 && t.NCAC424EXP_BooksExp == data.NCAC424EXP_BooksExp && t.NCAC424EXP_JournalExp == data.NCAC424EXP_JournalExp && t.NCAC424EXP_EJournalExp == data.NCAC424EXP_EJournalExp && t.NCAC424EXP_ExpYear == data.NCAC424EXP_ExpYear).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_424_Expenditure_DMO obj = new NAAC_AC_424_Expenditure_DMO();
                        obj.MI_Id = data.MI_Id;
                        obj.NCAC424EXP_BooksExp = data.NCAC424EXP_BooksExp;
                        obj.NCAC424EXP_ExpYear = data.NCAC424EXP_ExpYear;
                        obj.NCAC424EXP_EJournalExp = data.NCAC424EXP_EJournalExp;
                        obj.NCAC424EXP_JournalExp = data.NCAC424EXP_JournalExp;
                        //obj.NCAC424EXP_FileName = data.NCAC424EXP_FileName;
                        //obj.NCAC424EXP_FilePath = data.NCAC424EXP_FilePath;
                        obj.NCAC424EXP_CreatedDate = DateTime.Now;
                        obj.NCAC424EXP_UpdatedDate = DateTime.Now;
                        obj.NCAC424EXP_ActiveFlg = true;
                        obj.NCAC424EXP_CreatedBy = data.UserId;
                        obj.NCAC424EXP_UpdatedBy = data.UserId;
                        obj.NCAC424EXP_StatusFlg = "";
                        obj.MI_Id = data.MI_Id;
                        _GeneralContext.Add(obj);
                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[i].cfilepath != null)
                                {
                                    NAAC_AC_424_Expenditure_Files_DMO obj2 = new NAAC_AC_424_Expenditure_Files_DMO();

                                    obj2.NCAC424EXPF_FileName = data.filelist[i].cfilename;
                                    obj2.NCAC424EXPF_Filedesc = data.filelist[i].cfiledesc;
                                    obj2.NCAC424EXPF_FilePath = data.filelist[i].cfilepath;
                                    obj2.NCAC424EXP_Id = obj.NCAC424EXP_Id;
                                    obj2.NCAC424EXPF_StatusFlg = "";
                                    obj2.NCAC424EXPF_ActiveFlg = true;

                                    _GeneralContext.Add(obj2);
                                }
                            }
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
                }
                else if (data.NCAC424EXP_Id > 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_424_Expenditure_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC424EXP_BooksExp == data.NCAC424EXP_BooksExp && t.NCAC424EXP_EJournalExp == data.NCAC424EXP_EJournalExp && t.NCAC424EXP_ExpYear == data.NCAC424EXP_ExpYear && t.NCAC424EXP_JournalExp == data.NCAC424EXP_JournalExp && t.NCAC424EXP_Id != data.NCAC424EXP_Id).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _GeneralContext.NAAC_AC_424_Expenditure_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC424EXP_Id == data.NCAC424EXP_Id).SingleOrDefault();
                        update.NCAC424EXP_UpdatedBy = data.UserId;
                        update.NCAC424EXP_BooksExp = data.NCAC424EXP_BooksExp;
                        update.NCAC424EXP_ExpYear = data.NCAC424EXP_ExpYear;
                        update.NCAC424EXP_JournalExp = data.NCAC424EXP_JournalExp;
                        update.NCAC424EXP_EJournalExp = data.NCAC424EXP_EJournalExp;
                        update.NCAC424EXP_UpdatedDate = DateTime.Now;
                        update.MI_Id = data.MI_Id;
                        _GeneralContext.Update(update);
                        
                        if (data.filelist.Length > 0)
                        {


                            List<long> Fid = new List<long>();
                            foreach (var item in data.filelist)
                            {
                                Fid.Add(item.NCAC424EXPF_Id);
                            }
                            var removefile11 = _GeneralContext.NAAC_AC_424_Expenditure_Files_DMO.Where(t => t.NCAC424EXP_Id == data.NCAC424EXP_Id && !Fid.Contains(t.NCAC424EXPF_Id)).Distinct().ToList();

                            if (removefile11.Count > 0)
                            {
                                foreach (var item2 in removefile11)
                                {
                                    var deactfile = _GeneralContext.NAAC_AC_424_Expenditure_Files_DMO.Single(t => t.NCAC424EXP_Id == data.NCAC424EXP_Id && t.NCAC424EXPF_Id == item2.NCAC424EXPF_Id);
                                    deactfile.NCAC424EXPF_ActiveFlg = false;
                                    _GeneralContext.Update(deactfile);

                                }

                            }

                            //var CountRemoveFiles = _GeneralContext.NAAC_AC_424_Expenditure_Files_DMO.Where(t => t.NCAC424EXP_Id == data.NCAC424EXP_Id && t.NCAC424EXPF_StatusFlg != "approved").ToList();
                            //if (CountRemoveFiles.Count > 0)
                            //{
                            //    foreach (var RemoveFiles in CountRemoveFiles)
                            //    {
                            //        _GeneralContext.Remove(RemoveFiles);
                            //    }
                            //}

                            foreach (NaacExpenditure424DTO DocumentsDTO in data.filelist)
                            {

                                if (DocumentsDTO.NCAC424EXPF_Id > 0 && DocumentsDTO.NCAC424EXPF_StatusFlg != "approved")
                                {
                                    if (DocumentsDTO.cfilepath != null)
                                    {

                                        var filesdata = _GeneralContext.NAAC_AC_424_Expenditure_Files_DMO.Where(t => t.NCAC424EXPF_Id == DocumentsDTO.NCAC424EXPF_Id).FirstOrDefault();
                                        filesdata.NCAC424EXPF_Filedesc = DocumentsDTO.cfiledesc;
                                        filesdata.NCAC424EXPF_FileName = DocumentsDTO.cfilename;
                                        filesdata.NCAC424EXPF_FilePath = DocumentsDTO.cfilepath;


                                        _GeneralContext.Update(filesdata);
                                       
                                    }
                                }
                                else
                                {

                                    if (DocumentsDTO.NCAC424EXPF_Id == 0)
                                    {
                                        if (DocumentsDTO.cfilepath != null)
                                        {
                                            NAAC_AC_424_Expenditure_Files_DMO obj2 = new NAAC_AC_424_Expenditure_Files_DMO();
                                            obj2.NCAC424EXPF_FileName = DocumentsDTO.cfilename;
                                            obj2.NCAC424EXPF_Filedesc = DocumentsDTO.cfiledesc;
                                            obj2.NCAC424EXPF_FilePath = DocumentsDTO.cfilepath;
                                            obj2.NCAC424EXPF_StatusFlg = "";
                                            obj2.NCAC424EXPF_ActiveFlg = true;
                                            obj2.NCAC424EXP_Id = data.NCAC424EXP_Id;
                                            _GeneralContext.Add(obj2);
                                            
                                        }
                                    }
                                }
                            }
                        }
                        int flag = _GeneralContext.SaveChanges();
                        if (flag > 0)
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NaacExpenditure424DTO loaddata(NaacExpenditure424DTO data)
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

                data.alldata1 = (from b in _GeneralContext.NAAC_AC_424_Expenditure_DMO
                                 from a in _GeneralContext.Academic
                                 where (b.NCAC424EXP_ExpYear == a.ASMAY_Id && b.MI_Id == data.MI_Id)
                                 select new NaacExpenditure424DTO {
                                     NCAC424EXP_EJournalExp = b.NCAC424EXP_EJournalExp,
                                     NCAC424EXP_BooksExp = b.NCAC424EXP_BooksExp,
                                     NCAC424EXP_ExpYear = b.NCAC424EXP_ExpYear,
                                     NCAC424EXP_JournalExp = b.NCAC424EXP_JournalExp,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCAC424EXP_Id = b.NCAC424EXP_Id,
                                     MI_Id = b.MI_Id,
                                     NCAC424EXP_StatusFlg = b.NCAC424EXP_StatusFlg,
                                     NCAC424EXP_ActiveFlg = b.NCAC424EXP_ActiveFlg,
                                 }).Distinct().ToArray();
                data.allacademicyear = _GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id&&t.Is_Active==true).Distinct().OrderByDescending(t => t.ASMAY_Year).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NaacExpenditure424DTO EditData(NaacExpenditure424DTO data)
        {
            try
            {
                data.editlist = (from a in _GeneralContext.Academic
                                 from b in _GeneralContext.NAAC_AC_424_Expenditure_DMO
                                 where (b.NCAC424EXP_ExpYear == a.ASMAY_Id && b.MI_Id == data.MI_Id && b.NCAC424EXP_Id == data.NCAC424EXP_Id)
                                 select new NaacExpenditure424DTO
                                 {
                                     NCAC424EXP_Id = b.NCAC424EXP_Id,
                                     NCAC424EXP_ExpYear = b.NCAC424EXP_ExpYear,
                                     NCAC424EXP_BooksExp = b.NCAC424EXP_BooksExp,
                                     NCAC424EXP_EJournalExp = b.NCAC424EXP_EJournalExp,
                                     NCAC424EXP_JournalExp = b.NCAC424EXP_JournalExp,
                                     MI_Id = b.MI_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();
                data.editFileslist = (from t in _GeneralContext.NAAC_AC_424_Expenditure_Files_DMO
                                      
                                      where (t.NCAC424EXP_Id == data.NCAC424EXP_Id&&t.NCAC424EXPF_ActiveFlg==true )
                                      select new NaacExpenditure424DTO
                                      {
                                          cfilename = t.NCAC424EXPF_FileName,
                                          cfilepath = t.NCAC424EXPF_FilePath,
                                          cfiledesc = t.NCAC424EXPF_Filedesc,
                                          NCAC424EXPF_StatusFlg = t.NCAC424EXPF_StatusFlg,
                                          NCAC424EXPF_Id = t.NCAC424EXPF_Id,
                                     
                                      }).Distinct().ToArray();
            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NaacExpenditure424DTO deactiveStudent(NaacExpenditure424DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_424_Expenditure_DMO.Where(t => t.NCAC424EXP_Id == data.NCAC424EXP_Id).SingleOrDefault();
                if (result.NCAC424EXP_ActiveFlg == true)
                {
                    result.NCAC424EXP_ActiveFlg = false;
                }
                else if (result.NCAC424EXP_ActiveFlg == false)
                {
                    result.NCAC424EXP_ActiveFlg = true;
                }
                result.NCAC424EXP_UpdatedDate = DateTime.Now;
                result.NCAC424EXP_UpdatedBy = data.UserId;
                result.MI_Id = data.MI_Id;
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
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NaacExpenditure424DTO viewuploadflies(NaacExpenditure424DTO data)
        {
            try
            {
                data.viewuploadflies = (from t in _GeneralContext.NAAC_AC_424_Expenditure_Files_DMO
                                       
                                        where (t.NCAC424EXP_Id == data.NCAC424EXP_Id &&t.NCAC424EXPF_ActiveFlg==true)
                                        select new NaacExpenditure424DTO
                                        {
                                            cfilename = t.NCAC424EXPF_FileName,
                                            cfilepath = t.NCAC424EXPF_FilePath,
                                            cfiledesc = t.NCAC424EXPF_Filedesc,
                                            NCAC424EXPF_Id = t.NCAC424EXPF_Id,
                                            NCAC424EXP_Id = t.NCAC424EXP_Id,
                                            NCAC424EXPF_StatusFlg = t.NCAC424EXPF_StatusFlg,

                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NaacExpenditure424DTO deleteuploadfile(NaacExpenditure424DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_424_Expenditure_Files_DMO.Where(t => t.NCAC424EXPF_Id == data.NCAC424EXPF_Id).SingleOrDefault();
                result.NCAC424EXPF_ActiveFlg = false;
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

                data.viewuploadflies = (from t in _GeneralContext.NAAC_AC_424_Expenditure_Files_DMO
                                     
                                        where (t.NCAC424EXP_Id == data.NCAC424EXP_Id&&t.NCAC424EXPF_ActiveFlg==true)
                                        select new NaacExpenditure424DTO
                                        {
                                            cfilename = t.NCAC424EXPF_FileName,
                                            cfilepath = t.NCAC424EXPF_FilePath,
                                            cfiledesc = t.NCAC424EXPF_Filedesc,
                                            NCAC424EXPF_Id = t.NCAC424EXPF_Id,
                                            NCAC424EXP_Id = t.NCAC424EXP_Id,
                                            NCAC424EXPF_StatusFlg = t.NCAC424EXPF_StatusFlg,

                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }

        public NaacExpenditure424DTO getcomment(NaacExpenditure424DTO data)
        {
            try
            {
                data.commentlist = (from a in _GeneralContext.NAAC_AC_424_Expenditure_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCAC424EXPC_RemarksBy == b.Id && a.NCAC424EXP_Id == data.NCAC424EXP_Id)
                                    select new NaacExpenditure424DTO
                                    {
                                        NCAC424EXPC_Remarks = a.NCAC424EXPC_Remarks,
                                        NCAC424EXPC_Id = a.NCAC424EXPC_Id,
                                        NCAC424EXP_Id = a.NCAC424EXP_Id,
                                        NCAC424EXPC_RemarksBy = a.NCAC424EXPC_RemarksBy,
                                        NCAC424EXPC_StatusFlg = a.NCAC424EXPC_StatusFlg,
                                        NCAC424EXPC_ActiveFlag = a.NCAC424EXPC_ActiveFlag,
                                        NCAC424EXPC_CreatedBy = a.NCAC424EXPC_CreatedBy,
                                        NCAC424EXPC_CreatedDate = a.NCAC424EXPC_CreatedDate,
                                        NCAC424EXPC_UpdatedBy = a.NCAC424EXPC_UpdatedBy,
                                        NCAC424EXPC_UpdatedDate = a.NCAC424EXPC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCAC424EXPC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        // for file
        public NaacExpenditure424DTO getfilecomment(NaacExpenditure424DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_424_Expenditure_File_Comments_DMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCAC424EXPFC_RemarksBy == b.Id && a.NCAC424EXPF_Id == data.NCAC424EXPF_Id)
                                     select new NaacExpenditure424DTO
                                     {
                                         NCAC424EXPF_Id = a.NCAC424EXPF_Id,
                                         NCAC424EXPFC_Remarks = a.NCAC424EXPFC_Remarks,
                                         NCAC424EXPFC_Id = a.NCAC424EXPFC_Id,
                                         NCAC424EXPFC_RemarksBy = a.NCAC424EXPFC_RemarksBy,
                                         NCAC424EXPFC_StatusFlg = a.NCAC424EXPFC_StatusFlg,
                                         NCAC424EXPFC_ActiveFlag = a.NCAC424EXPFC_ActiveFlag,
                                         NCAC424EXPFC_CreatedBy = a.NCAC424EXPFC_CreatedBy,
                                         NCAC424EXPFC_CreatedDate = a.NCAC424EXPFC_CreatedDate,
                                         NCAC424EXPFC_UpdatedBy = a.NCAC424EXPFC_UpdatedBy,
                                         NCAC424EXPFC_UpdatedDate = a.NCAC424EXPFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCAC424EXPFC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NaacExpenditure424DTO savemedicaldatawisecomments(NaacExpenditure424DTO data)
        {
            try
            {
                NAAC_AC_424_Expenditure_Comments_DMO obj1 = new NAAC_AC_424_Expenditure_Comments_DMO();
                obj1.NCAC424EXPC_Remarks = data.Remarks;
                obj1.NCAC424EXPC_RemarksBy = data.UserId;
                obj1.NCAC424EXPC_StatusFlg = "";
                obj1.NCAC424EXP_Id = data.filefkid;
                obj1.NCAC424EXPC_ActiveFlag = true;
                obj1.NCAC424EXPC_CreatedBy = data.UserId;
                obj1.NCAC424EXPC_UpdatedBy = data.UserId;
                obj1.NCAC424EXPC_CreatedDate = DateTime.Now;
                obj1.NCAC424EXPC_UpdatedDate = DateTime.Now;
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
        public NaacExpenditure424DTO savefilewisecomments(NaacExpenditure424DTO data)
        {
            try
            {
                NAAC_AC_424_Expenditure_File_Comments_DMO obj1 = new NAAC_AC_424_Expenditure_File_Comments_DMO();
                obj1.NCAC424EXPFC_Remarks = data.Remarks;
                obj1.NCAC424EXPFC_RemarksBy = data.UserId;
                obj1.NCAC424EXPFC_StatusFlg = "";
                obj1.NCAC424EXPF_Id = data.filefkid;
                obj1.NCAC424EXPFC_ActiveFlag = true;
                obj1.NCAC424EXPFC_CreatedBy = data.UserId;
                obj1.NCAC424EXPFC_UpdatedBy = data.UserId;
                obj1.NCAC424EXPFC_UpdatedDate = DateTime.Now;
                obj1.NCAC424EXPFC_CreatedDate = DateTime.Now;
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
