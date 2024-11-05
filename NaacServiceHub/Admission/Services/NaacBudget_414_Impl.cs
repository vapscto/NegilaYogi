using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NaacBudget_414_Impl : Interface.NaacBudget_414_Interface 
    {
        public GeneralContext _GeneralContext;
        public NaacBudget_414_Impl(GeneralContext w)
        {
            _GeneralContext = w;
        }
        public NaacBudget_414_DTO loaddata(NaacBudget_414_DTO data)
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

                data.allacademicyear = _GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id&&t.Is_Active==true).Distinct().OrderByDescending(t => t.ASMAY_Year).ToArray();

                data.alldata1 = (from a in _GeneralContext.Academic
                                 from b in _GeneralContext.NAAC_AC_414_Budget_DMO
                                 where (b.NCAC414BD_AllotYear == a.ASMAY_Id && b.MI_Id == data.MI_Id)
                                 select new NaacBudget_414_DTO
                                 {
                                     NCAC414BD_Id = b.NCAC414BD_Id,
                                     NCAC414BD_AllotYear = b.NCAC414BD_AllotYear,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCAC414BD_Budget = b.NCAC414BD_Budget,
                                     //NCAC414BD_FileName = b.NCAC414BD_FileName,
                                     //NCAC414BD_FilePath = b.NCAC414BD_FilePath,
                                     NCAC414BD_ActiveFlg = b.NCAC414BD_ActiveFlg,
                                     NCAC414BD_StatusFlg = b.NCAC414BD_StatusFlg,
                                     MI_Id = b.MI_Id,
                                   
                                 }).Distinct().OrderByDescending(t => t.NCAC414BD_Id).ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NaacBudget_414_DTO save(NaacBudget_414_DTO data)
        {
            try
            {
                if (data.NCAC414BD_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_414_Budget_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC414BD_Id != 0 && t.NCAC414BD_Budget == data.NCAC414BD_Budget && t.NCAC414BD_AllotYear == data.NCAC414BD_AllotYear).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_414_Budget_DMO obj1 = new NAAC_AC_414_Budget_DMO();
                        obj1.MI_Id = data.MI_Id;                   
                        obj1.NCAC414BD_AllotYear = data.NCAC414BD_AllotYear;
                        obj1.NCAC414BD_Budget = data.NCAC414BD_Budget;
                        obj1.CreatedDate = DateTime.Now;
                        obj1.UpdatedDate = DateTime.Now;
                        obj1.NCAC414BD_ActiveFlg = true;
                        obj1.NCAC414BD_BudgetINfDev = data.NCAC414BD_BudgetINfDev;
                        obj1.NCAC414BD_TotalExpExcludeSal = data.NCAC414BD_TotalExpExcludeSal;
                        obj1.NCAC414BD_BudgetINfAugn = data.NCAC414BD_BudgetINfAugn;
                        obj1.NCAC414BD_CreatedBy = data.UserId;
                        obj1.NCAC414BD_UpdatedBy = data.UserId;
                        obj1.NCAC414BD_BudgetAllotDev = data.NCAC414BD_BudgetAllotDev;
                        obj1.MI_Id = data.MI_Id;
                        _GeneralContext.Add(obj1);

                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if(data.filelist[i].cfilepath!=null)
                                    {

                                    NAAC_AC_414_Budget_Files_DMO obj2 = new NAAC_AC_414_Budget_Files_DMO();

                                    //obj2.NCAC414BDF_Id = data.NCAC414BDF_Id;
                                    obj2.NCAC414BDF_FileName = data.filelist[i].cfilename;
                                    obj2.NCAC414BDF_Filedesc = data.filelist[i].cfiledesc;
                                    obj2.NCAC414BDF_FilePath = data.filelist[i].cfilepath;
                                    obj2.NCAC414BD_Id = obj1.NCAC414BD_Id;
                                    obj2.NCAC414BDF_ActiveFlg = true ;

                                    _GeneralContext.Add(obj2);
                                }
                            }
                        }

                        int flagw = _GeneralContext.SaveChanges();
                        if (flagw > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                        
                    }
                }
                else if (data.NCAC414BD_Id > 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_414_Budget_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC414BD_Budget == data.NCAC414BD_Budget && t.NCAC414BD_AllotYear == data.NCAC414BD_AllotYear && t.NCAC414BD_Id != data.NCAC414BD_Id).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _GeneralContext.NAAC_AC_414_Budget_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC414BD_Id == data.NCAC414BD_Id).SingleOrDefault();
                        update.NCAC414BD_UpdatedBy = data.UserId;
                        update.NCAC414BD_Budget = data.NCAC414BD_Budget;
                        update.NCAC414BD_BudgetAllotDev = data.NCAC414BD_BudgetAllotDev;
                        update.NCAC414BD_TotalExpExcludeSal = data.NCAC414BD_TotalExpExcludeSal;
                        update.NCAC414BD_BudgetINfDev = data.NCAC414BD_BudgetINfDev;
                        update.NCAC414BD_BudgetINfAugn = data.NCAC414BD_BudgetINfAugn;
                        update.UpdatedDate = DateTime.Now;
                        update.MI_Id = data.MI_Id;
                        _GeneralContext.Update(update);

                        
                        if (data.filelist.Length > 0)
                        {


                            List<long> Fid = new List<long>();
                            foreach (var item in data.filelist)
                            {
                                Fid.Add(item.NCAC414BDF_Id);
                            }
                            var removefile11 = _GeneralContext.NAAC_AC_414_Budget_Files_DMO.Where(t => t.NCAC414BD_Id == data.NCAC414BD_Id && !Fid.Contains(t.NCAC414BDF_Id)).Distinct().ToList();

                            if (removefile11.Count > 0)
                            {
                                foreach (var item2 in removefile11)
                                {
                                    var deactfile = _GeneralContext.NAAC_AC_414_Budget_Files_DMO.Single(t => t.NCAC414BD_Id == data.NCAC414BD_Id && t.NCAC414BDF_Id == item2.NCAC414BDF_Id);
                                    deactfile.NCAC414BDF_ActiveFlg = false;
                                    _GeneralContext.Update(deactfile);

                                }

                            }
                            //var CountRemoveFiles = _GeneralContext.NAAC_AC_414_Budget_Files_DMO.Where(t => t.NCAC414BD_Id == data.NCAC414BD_Id && t.NCAC414BDF_StatusFlg != "approved").ToList();
                            //if (CountRemoveFiles.Count > 0)
                            //{
                            //    foreach (var RemoveFiles in CountRemoveFiles)
                            //    {
                            //        _GeneralContext.Remove(RemoveFiles);
                            //    }
                            //}





                            foreach (NaacBudget_414_DTO DocumentsDTO in data.filelist)
                            {

                                if (DocumentsDTO.NCAC414BDF_Id > 0 && DocumentsDTO.NCAC414BDF_StatusFlg != "approved")
                                {
                                    if (DocumentsDTO.cfilepath != null)
                                    {

                                        var filesdata = _GeneralContext.NAAC_AC_414_Budget_Files_DMO.Where(t => t.NCAC414BDF_Id == DocumentsDTO.NCAC414BDF_Id).FirstOrDefault();
                                        filesdata.NCAC414BDF_Filedesc = DocumentsDTO.cfiledesc;
                                        filesdata.NCAC414BDF_FileName = DocumentsDTO.cfilename;
                                        filesdata.NCAC414BDF_FilePath = DocumentsDTO.cfilepath;


                                        _GeneralContext.Update(filesdata);
                                       
                                       
                                    }
                                }
                                else
                                {

                                    if (DocumentsDTO.NCAC414BDF_Id == 0)
                                    {
                                        if (DocumentsDTO.cfilepath != null)
                                        {
                                            NAAC_AC_414_Budget_Files_DMO obj2 = new NAAC_AC_414_Budget_Files_DMO();
                                            obj2.NCAC414BDF_FileName = DocumentsDTO.cfilename;
                                            obj2.NCAC414BDF_Filedesc = DocumentsDTO.cfiledesc;
                                            obj2.NCAC414BDF_FilePath = DocumentsDTO.cfilepath;
                                            obj2.NCAC414BDF_StatusFlg = "";
                                            obj2.NCAC414BDF_ActiveFlg = true;
                                            obj2.NCAC414BD_Id = data.NCAC414BD_Id;
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
        public NaacBudget_414_DTO EditData(NaacBudget_414_DTO data)
        {
            try
            {
                data.editlist = (from a in _GeneralContext.Academic
                                 from b in _GeneralContext.NAAC_AC_414_Budget_DMO
                                 where (b.NCAC414BD_AllotYear == a.ASMAY_Id && b.MI_Id == data.MI_Id && b.NCAC414BD_Id == data.NCAC414BD_Id)
                                 select new NaacBudget_414_DTO
                                 {
                                     NCAC414BD_Id = b.NCAC414BD_Id,
                                     NCAC414BD_AllotYear = b.NCAC414BD_AllotYear,
                                     NCAC414BD_Budget = b.NCAC414BD_Budget,
                                     MI_Id=b.MI_Id,
                                     NCAC414BD_BudgetAllotDev = b.NCAC414BD_BudgetAllotDev,
                                     NCAC414BD_TotalExpExcludeSal = b.NCAC414BD_TotalExpExcludeSal,
                                     NCAC414BD_BudgetINfDev = b.NCAC414BD_BudgetINfDev,
                                     NCAC414BD_BudgetINfAugn = b.NCAC414BD_BudgetINfAugn,
                                     NCAC414BD_StatusFlg = b.NCAC414BD_StatusFlg,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();

                data.editFileslist = (from a in _GeneralContext.NAAC_AC_414_Budget_Files_DMO
                                      where (a.NCAC414BD_Id == data.NCAC414BD_Id&&a.NCAC414BDF_ActiveFlg==true)
                                      select new NaacBudget_414_DTO
                                      {
                                          cfilename = a.NCAC414BDF_FileName,
                                          cfilepath = a.NCAC414BDF_FilePath,
                                          cfiledesc = a.NCAC414BDF_Filedesc,
                                          NCAC414BDF_StatusFlg = a.NCAC414BDF_StatusFlg,
                                          NCAC414BDF_Id = a.NCAC414BDF_Id,
                                          

                                      }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NaacBudget_414_DTO deactiveStudent(NaacBudget_414_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_414_Budget_DMO.Where(t => t.NCAC414BD_Id == data.NCAC414BD_Id).SingleOrDefault();
                if (result.NCAC414BD_ActiveFlg == true)
                {
                    result.NCAC414BD_ActiveFlg = false;
                }
                else if (result.NCAC414BD_ActiveFlg == false)
                {
                    result.NCAC414BD_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                result.NCAC414BD_UpdatedBy = data.UserId;
                result.MI_Id = data.MI_Id;
                _GeneralContext.Update(result);
                int o = _GeneralContext.SaveChanges();
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
        public NaacBudget_414_DTO viewuploadflies(NaacBudget_414_DTO data)
        {
            try
            {                
               


                data.viewuploadflies = (from t in _GeneralContext.NAAC_AC_414_Budget_Files_DMO
                                       
                                        where (t.NCAC414BD_Id == data.NCAC414BD_Id&&t.NCAC414BDF_ActiveFlg==true)
                                        select new NaacBudget_414_DTO
                                        {
                                            cfilename = t.NCAC414BDF_FileName,
                                            cfilepath = t.NCAC414BDF_FilePath,
                                            cfiledesc = t.NCAC414BDF_Filedesc,
                                            NCAC414BDF_Id = t.NCAC414BDF_Id,
                                            NCAC414BD_Id = t.NCAC414BD_Id,
                                            NCAC414BDF_StatusFlg = t.NCAC414BDF_StatusFlg,
                                        }).Distinct().ToArray();


            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NaacBudget_414_DTO deleteuploadfile(NaacBudget_414_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_414_Budget_Files_DMO.Where(t => t.NCAC414BDF_Id == data.NCAC414BDF_Id).SingleOrDefault();
                result.NCAC414BDF_ActiveFlg = false;
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


              
                data.viewuploadflies = (from t in _GeneralContext.NAAC_AC_414_Budget_Files_DMO
                                       
                                        where (t.NCAC414BD_Id == data.NCAC414BD_Id&&t.NCAC414BDF_ActiveFlg==true )
                                        select new NaacBudget_414_DTO
                                        {
                                            cfilename = t.NCAC414BDF_FileName,
                                            cfilepath = t.NCAC414BDF_FilePath,
                                            cfiledesc = t.NCAC414BDF_Filedesc,
                                            NCAC414BDF_Id = t.NCAC414BDF_Id,
                                            NCAC414BD_Id = t.NCAC414BD_Id,
                                            NCAC414BDF_StatusFlg = t.NCAC414BDF_StatusFlg,
                                       
                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }

        public NaacBudget_414_DTO getcomment(NaacBudget_414_DTO data)
        {
            try
            {
                data.commentlist = (from a in _GeneralContext.NAAC_AC_414_Budget_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCAC414BDC_RemarksBy == b.Id && a.NCAC414BD_Id == data.NCAC414BD_Id)
                                    select new NaacBudget_414_DTO
                                    {
                                        NCAC414BDC_Remarks = a.NCAC414BDC_Remarks,
                                        NCAC414BDC_Id = a.NCAC414BDC_Id,
                                        NCAC414BDC_RemarksBy = a.NCAC414BDC_RemarksBy,
                                        NCAC414BDC_StatusFlg = a.NCAC414BDC_StatusFlg,
                                        NCAC414BDC_ActiveFlag = a.NCAC414BDC_ActiveFlag,
                                        NCAC414BDC_CreatedBy = a.NCAC414BDC_CreatedBy,
                                        NCAC414BDC_CreatedDate = a.NCAC414BDC_CreatedDate,
                                        NCAC414BDC_UpdatedBy = a.NCAC414BDC_UpdatedBy,
                                        NCAC414BDC_UpdatedDate = a.NCAC414BDC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCAC414BDC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        // for file
        public NaacBudget_414_DTO getfilecomment(NaacBudget_414_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_414_Budget_File_Comments_DMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCAC414BDFC_RemarksBy == b.Id && a.NCAC414BDF_Id == data.NCAC414BDF_Id)
                                     select new NaacBudget_414_DTO
                                     {
                                         NCAC414BDF_Id = a.NCAC414BDF_Id,
                                         NCAC414BDFC_Remarks = a.NCAC414BDFC_Remarks,
                                         NCAC414BDFC_Id = a.NCAC414BDFC_Id,
                                         NCAC414BDFC_RemarksBy = a.NCAC414BDFC_RemarksBy,
                                         NCAC414BDFC_StatusFlg = a.NCAC414BDFC_StatusFlg,
                                         NCAC414BDFC_ActiveFlag = a.NCAC414BDFC_ActiveFlag,
                                         NCAC414BDFC_CreatedBy = a.NCAC414BDFC_CreatedBy,
                                         NCAC414BDFC_CreatedDate = a.NCAC414BDFC_CreatedDate,
                                         NCAC414BDFC_UpdatedBy = a.NCAC414BDFC_UpdatedBy,
                                         NCAC414BDFC_UpdatedDate = a.NCAC414BDFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCAC414BDFC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NaacBudget_414_DTO savemedicaldatawisecomments(NaacBudget_414_DTO data)
        {
            try
            {
                NAAC_AC_414_Budget_Comments_DMO obj1 = new NAAC_AC_414_Budget_Comments_DMO();
                obj1.NCAC414BDC_Remarks = data.Remarks;
                obj1.NCAC414BDC_RemarksBy = data.UserId;
                obj1.NCAC414BDC_StatusFlg = "";
                obj1.NCAC414BD_Id = data.filefkid;
                obj1.NCAC414BDC_ActiveFlag = true;
                obj1.NCAC414BDC_CreatedBy = data.UserId;
                obj1.NCAC414BDC_UpdatedBy = data.UserId;
                obj1.NCAC414BDC_CreatedDate = DateTime.Now;
                obj1.NCAC414BDC_UpdatedDate = DateTime.Now;
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
        public NaacBudget_414_DTO savefilewisecomments(NaacBudget_414_DTO data)
        {
            try
            {
                NAAC_AC_414_Budget_File_Comments_DMO obj1 = new NAAC_AC_414_Budget_File_Comments_DMO();
                obj1.NCAC414BDFC_Remarks = data.Remarks;
                obj1.NCAC414BDFC_RemarksBy = data.UserId;
                obj1.NCAC414BDFC_StatusFlg = "";
                obj1.NCAC414BDF_Id = data.filefkid;
                obj1.NCAC414BDFC_ActiveFlag = true;
                obj1.NCAC414BDFC_CreatedBy = data.UserId;
                obj1.NCAC414BDFC_UpdatedBy = data.UserId;
                obj1.NCAC414BDFC_UpdatedDate = DateTime.Now;
                obj1.NCAC414BDFC_CreatedDate = DateTime.Now;
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
