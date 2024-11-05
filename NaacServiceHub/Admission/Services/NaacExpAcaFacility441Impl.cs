using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NaacExpAcaFacility441Impl : Interface.NaacExpAcaFacility441Interface
    {
        public GeneralContext _GeneralContext;
        public NaacExpAcaFacility441Impl(GeneralContext parameter)
        {
            _GeneralContext = parameter;
        }

        public NaacExpAcaFacility441DTO loaddata(NaacExpAcaFacility441DTO data)
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

                data.allacademicyear = _GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Year).ToArray();

                data.alldata1 = (from a in _GeneralContext.Academic
                                 from b in _GeneralContext.NAAC_AC_441_ExpAcaFacility_DMO
                                 where (a.MI_Id == b.MI_Id && b.NCAC441EXACFC_Year == a.ASMAY_Id && a.Is_Active == true && a.MI_Id == data.MI_Id)
                                 select new NaacExpAcaFacility441DTO
                                 {
                                     NCAC441ExAcFc_Id = b.NCAC441ExAcFc_Id,
                                     NCAC441EXACFC_ExpAccFacility = b.NCAC441EXACFC_ExpAccFacility,
                                     NCAC441EXACFC_Year = b.NCAC441EXACFC_Year,
                                     NCAC441EXACFC_ExpPhyFacility = b.NCAC441EXACFC_ExpPhyFacility,
                                     //NCAC441EXACFC_FileName = b.NCAC441EXACFC_FileName,
                                     //NCAC441EXACFC_FilePath = b.NCAC441EXACFC_FilePath,
                                     NCAC441EXACFC_ActiveFlg = b.NCAC441EXACFC_ActiveFlg,
                                     MI_Id = data.MI_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCAC441ExAcFc_StatusFlg = b.NCAC441ExAcFc_StatusFlg,
                                 }).Distinct().OrderByDescending(t => t.NCAC441ExAcFc_Id).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NaacExpAcaFacility441DTO save(NaacExpAcaFacility441DTO data)
        {
            try
            {
                if (data.NCAC441ExAcFc_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_441_ExpAcaFacility_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC441ExAcFc_Id != 0 && t.NCAC441EXACFC_ExpAccFacility == data.NCAC441EXACFC_ExpAccFacility && t.NCAC441EXACFC_ExpPhyFacility == data.NCAC441EXACFC_ExpPhyFacility).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_441_ExpAcaFacility_DMO obj = new NAAC_AC_441_ExpAcaFacility_DMO();
                        obj.MI_Id = data.MI_Id;
                        obj.NCAC441EXACFC_ExpAccFacility = data.NCAC441EXACFC_ExpAccFacility;
                        obj.NCAC441EXACFC_ExpPhyFacility = data.NCAC441EXACFC_ExpPhyFacility;
                        //obj.NCAC441EXACFC_FilePath = data.NCAC441EXACFC_FilePath;
                        //obj.NCAC441EXACFC_FileName = data.NCAC441EXACFC_FileName;
                        obj.NCAC441EXACFC_Year = data.ASMAY_Id;
                        obj.NCAC441EXACFC_CreatedDate = DateTime.Now;
                        obj.NCAC441EXACFC_UpdatedDate = DateTime.Now;
                        obj.NCAC441EXACFC_ActiveFlg = true;
                        obj.NCAC441EXACFC_CreatedBy = data.UserId;
                        obj.NCAC441EXACFC_UpdatedBy = data.UserId;
                        obj.NCAC441ExAcFc_StatusFlg = "";
                        obj.MI_Id = data.MI_Id;
                        _GeneralContext.Add(obj);

                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[i].cfilepath != null)
                                {
                                    NAAC_AC_441_ExpAcaFacility_Files_DMO obj2 = new NAAC_AC_441_ExpAcaFacility_Files_DMO();

                                    obj2.NCAC441ExAcFcF_FileName = data.filelist[i].cfilename;
                                    obj2.NCAC441ExAcFcF_Filedesc = data.filelist[i].cfiledesc;
                                    obj2.NCAC441ExAcFcF_FilePath = data.filelist[i].cfilepath;
                                    obj2.NCAC441ExAcFc_Id = obj.NCAC441ExAcFc_Id;
                                    obj2.NCAC441ExAcFcF_StatusFlg = "";
                                    obj2.NCAC441ExAcFcF_ActiveFlg = true;

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
                else if (data.NCAC441ExAcFc_Id > 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_441_ExpAcaFacility_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC441EXACFC_ExpAccFacility == data.NCAC441EXACFC_ExpAccFacility   && t.NCAC441ExAcFc_Id != data.NCAC441ExAcFc_Id).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _GeneralContext.NAAC_AC_441_ExpAcaFacility_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC441ExAcFc_Id == data.NCAC441ExAcFc_Id).SingleOrDefault();

                        update.NCAC441EXACFC_UpdatedBy = data.UserId;
                        update.NCAC441EXACFC_ExpAccFacility = data.NCAC441EXACFC_ExpAccFacility;
                        update.NCAC441EXACFC_ExpPhyFacility = data.NCAC441EXACFC_ExpPhyFacility;
                        update.NCAC441EXACFC_UpdatedDate = DateTime.Now;
                        update.MI_Id = data.MI_Id;
                        _GeneralContext.Update(update);

                        
                        if (data.filelist.Length > 0)
                        {


                            List<long> Fid = new List<long>();
                            foreach (var item in data.filelist)
                            {
                                Fid.Add(item.NCAC441ExAcFcF_Id);
                            }
                            var removefile11 = _GeneralContext.NAAC_AC_441_ExpAcaFacility_Files_DMO.Where(t => t.NCAC441ExAcFc_Id == data.NCAC441ExAcFc_Id && !Fid.Contains(t.NCAC441ExAcFcF_Id)).Distinct().ToList();

                            if (removefile11.Count > 0)
                            {
                                foreach (var item2 in removefile11)
                                {
                                    var deactfile = _GeneralContext.NAAC_AC_441_ExpAcaFacility_Files_DMO.Single(t => t.NCAC441ExAcFc_Id == data.NCAC441ExAcFc_Id && t.NCAC441ExAcFcF_Id == item2.NCAC441ExAcFcF_Id);
                                    deactfile.NCAC441ExAcFcF_ActiveFlg = false;
                                    _GeneralContext.Update(deactfile);

                                }

                            }

                            //var CountRemoveFiles = _GeneralContext.NAAC_AC_441_ExpAcaFacility_Files_DMO.Where(t => t.NCAC441ExAcFc_Id == data.NCAC441ExAcFc_Id && t.NCAC441ExAcFcF_StatusFlg != "approved").ToList();
                            //if (CountRemoveFiles.Count > 0)
                            //{
                            //    foreach (var RemoveFiles in CountRemoveFiles)
                            //    {
                            //        _GeneralContext.Remove(RemoveFiles);
                            //    }
                            //}
                            foreach (NaacExpAcaFacility441DTO DocumentsDTO in data.filelist)
                            {

                                if (DocumentsDTO.NCAC441ExAcFcF_Id > 0 && DocumentsDTO.NCAC441ExAcFcF_StatusFlg != "approved")
                                {
                                    if (DocumentsDTO.cfilepath != null)
                                    {

                                        var filesdata = _GeneralContext.NAAC_AC_441_ExpAcaFacility_Files_DMO.Where(t => t.NCAC441ExAcFcF_Id == DocumentsDTO.NCAC441ExAcFcF_Id).FirstOrDefault();
                                        filesdata.NCAC441ExAcFcF_Filedesc = DocumentsDTO.cfiledesc;
                                        filesdata.NCAC441ExAcFcF_FileName = DocumentsDTO.cfilename;
                                        filesdata.NCAC441ExAcFcF_FilePath = DocumentsDTO.cfilepath;


                                        _GeneralContext.Update(filesdata);
                                       
                                    }
                                }
                                else
                                {

                                    if (DocumentsDTO.NCAC441ExAcFcF_Id == 0)
                                    {
                                        if (DocumentsDTO.cfilepath != null)
                                        {
                                            NAAC_AC_441_ExpAcaFacility_Files_DMO obj2 = new NAAC_AC_441_ExpAcaFacility_Files_DMO();
                                            obj2.NCAC441ExAcFcF_FileName = DocumentsDTO.cfilename;
                                            obj2.NCAC441ExAcFcF_Filedesc = DocumentsDTO.cfiledesc;
                                            obj2.NCAC441ExAcFcF_FilePath = DocumentsDTO.cfilepath;
                                            obj2.NCAC441ExAcFcF_StatusFlg = "";
                                            obj2.NCAC441ExAcFcF_ActiveFlg = true;
                                            obj2.NCAC441ExAcFc_Id = data.NCAC441ExAcFc_Id;
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
        public NaacExpAcaFacility441DTO EditData(NaacExpAcaFacility441DTO data)
        {
            try
            {
                data.editlist = (from a in _GeneralContext.Academic
                                 from b in _GeneralContext.NAAC_AC_441_ExpAcaFacility_DMO
                                 where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCAC441EXACFC_Year && b.MI_Id == data.MI_Id && b.NCAC441ExAcFc_Id == data.NCAC441ExAcFc_Id)
                                 select new NaacExpAcaFacility441DTO
                                 {
                                     NCAC441ExAcFc_Id = b.NCAC441ExAcFc_Id,
                                     NCAC441EXACFC_ExpAccFacility = b.NCAC441EXACFC_ExpAccFacility,
                                     NCAC441EXACFC_Year = b.NCAC441EXACFC_Year,
                                     NCAC441EXACFC_ExpPhyFacility = b.NCAC441EXACFC_ExpPhyFacility,
                                     MI_Id = data.MI_Id,
                                     //NCAC441EXACFC_FileName = b.NCAC441EXACFC_FileName,
                                     //NCAC441EXACFC_FilePath = b.NCAC441EXACFC_FilePath,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();
                data.editFileslist = (from t in _GeneralContext.NAAC_AC_441_ExpAcaFacility_Files_DMO
                                      from b in _GeneralContext.NAAC_AC_441_ExpAcaFacility_DMO
                                      where (t.NCAC441ExAcFc_Id == data.NCAC441ExAcFc_Id && t.NCAC441ExAcFc_Id == b.NCAC441ExAcFc_Id && b.MI_Id == data.MI_Id&&t.NCAC441ExAcFcF_ActiveFlg==true)
                                      select new NaacExpAcaFacility441DTO
                                      {
                                          cfilename = t.NCAC441ExAcFcF_FileName,
                                          cfilepath = t.NCAC441ExAcFcF_FilePath,
                                          cfiledesc = t.NCAC441ExAcFcF_Filedesc,
                                          NCAC441ExAcFcF_StatusFlg = t.NCAC441ExAcFcF_StatusFlg,
                                          NCAC441ExAcFcF_Id = t.NCAC441ExAcFcF_Id,
                                          NCAC441ExAcFc_Id = t.NCAC441ExAcFc_Id,

                                      }).Distinct().ToArray();
            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NaacExpAcaFacility441DTO deactiveStudent(NaacExpAcaFacility441DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_441_ExpAcaFacility_DMO.Where(t => t.NCAC441ExAcFc_Id == data.NCAC441ExAcFc_Id).SingleOrDefault();
                if (result.NCAC441EXACFC_ActiveFlg == true)
                {
                    result.NCAC441EXACFC_ActiveFlg = false;
                }
                else if (result.NCAC441EXACFC_ActiveFlg == false)
                {
                    result.NCAC441EXACFC_ActiveFlg = true;
                }
                result.NCAC441EXACFC_UpdatedDate = DateTime.Now;
                result.NCAC441EXACFC_UpdatedBy = data.UserId;
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
        public NaacExpAcaFacility441DTO viewuploadflies(NaacExpAcaFacility441DTO data)
        {
            try
            {
                data.viewuploadflies = (from t in _GeneralContext.NAAC_AC_441_ExpAcaFacility_Files_DMO
                                       
                                        where (t.NCAC441ExAcFc_Id == data.NCAC441ExAcFc_Id&&t.NCAC441ExAcFcF_ActiveFlg==true)
                                        select new NaacExpAcaFacility441DTO
                                        {
                                            cfilename = t.NCAC441ExAcFcF_FileName,
                                            cfilepath = t.NCAC441ExAcFcF_FilePath,
                                            cfiledesc = t.NCAC441ExAcFcF_Filedesc,
                                            NCAC441ExAcFcF_Id = t.NCAC441ExAcFcF_Id,
                                            NCAC441ExAcFc_Id = t.NCAC441ExAcFc_Id,
                                            NCAC441ExAcFcF_StatusFlg = t.NCAC441ExAcFcF_StatusFlg,

                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NaacExpAcaFacility441DTO deleteuploadfile(NaacExpAcaFacility441DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_441_ExpAcaFacility_Files_DMO.Where(t => t.NCAC441ExAcFcF_Id == data.NCAC441ExAcFcF_Id).SingleOrDefault();

                result.NCAC441ExAcFcF_ActiveFlg = false;
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

               
              
                data.viewuploadflies = (from t in _GeneralContext.NAAC_AC_441_ExpAcaFacility_Files_DMO
                                   
                                        where (t.NCAC441ExAcFc_Id == data.NCAC441ExAcFc_Id &&t.NCAC441ExAcFcF_ActiveFlg==true)
                                        select new NaacExpAcaFacility441DTO
                                        {
                                            cfilename = t.NCAC441ExAcFcF_FileName,
                                            cfilepath = t.NCAC441ExAcFcF_FilePath,
                                            cfiledesc = t.NCAC441ExAcFcF_Filedesc,
                                            NCAC441ExAcFcF_Id = t.NCAC441ExAcFcF_Id,
                                            NCAC441ExAcFc_Id = t.NCAC441ExAcFc_Id,
                                            NCAC441ExAcFcF_StatusFlg = t.NCAC441ExAcFcF_StatusFlg,

                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }

        public NaacExpAcaFacility441DTO getcomment(NaacExpAcaFacility441DTO data)
        {
            try
            {
                data.commentlist = (from a in _GeneralContext.NAAC_AC_441_ExpAcaFacility_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCAC441EXACFCC_RemarksBy == b.Id && a.NCAC441EXACFC_Id == data.NCAC441ExAcFc_Id)
                                    select new NaacExpAcaFacility441DTO
                                    {
                                        NCAC441EXACFCC_Remarks = a.NCAC441EXACFCC_Remarks,
                                        NCAC441EXACFCC_Id = a.NCAC441EXACFCC_Id,
                                        NCAC441EXACFCC_RemarksBy = a.NCAC441EXACFCC_RemarksBy,
                                        NCAC441EXACFCC_StatusFlg = a.NCAC441EXACFCC_StatusFlg,
                                        NCAC441EXACFCC_ActiveFlag = a.NCAC441EXACFCC_ActiveFlag,
                                        NCAC441EXACFCC_CreatedBy = a.NCAC441EXACFCC_CreatedBy,
                                        NCAC441EXACFCC_CreatedDate = a.NCAC441EXACFCC_CreatedDate,
                                        NCAC441EXACFCC_UpdatedBy = a.NCAC441EXACFCC_UpdatedBy,
                                        NCAC441EXACFCC_UpdatedDate = a.NCAC441EXACFCC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCAC441EXACFCC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        // for file
        public NaacExpAcaFacility441DTO getfilecomment(NaacExpAcaFacility441DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_441_ExpAcaFacility_File_Comments_DMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCAC441EXACFCFC_RemarksBy == b.Id && a.NCAC441EXACFCF_Id == data.NCAC441ExAcFcF_Id)
                                     select new NaacExpAcaFacility441DTO
                                     {
                                         NCAC441ExAcFcF_Id = a.NCAC441EXACFCF_Id,
                                         NCAC441EXACFCFC_Remarks = a.NCAC441EXACFCFC_Remarks,
                                         NCAC441EXACFCFC_Id = a.NCAC441EXACFCFC_Id,
                                         NCAC441EXACFCFC_RemarksBy = a.NCAC441EXACFCFC_RemarksBy,
                                         NCAC441EXACFCFC_StatusFlg = a.NCAC441EXACFCFC_StatusFlg,
                                         NCAC441EXACFCFC_ActiveFlag = a.NCAC441EXACFCFC_ActiveFlag,
                                         NCAC441EXACFCFC_CreatedBy = a.NCAC441EXACFCFC_CreatedBy,
                                         NCAC441EXACFCFC_CreatedDate = a.NCAC441EXACFCFC_CreatedDate,
                                         NCAC441EXACFCFC_UpdatedBy = a.NCAC441EXACFCFC_UpdatedBy,
                                         NCAC441EXACFCFC_UpdatedDate = a.NCAC441EXACFCFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCAC441EXACFCFC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NaacExpAcaFacility441DTO savemedicaldatawisecomments(NaacExpAcaFacility441DTO data)
        {
            try
            {
                NAAC_AC_441_ExpAcaFacility_Comments_DMO obj1 = new NAAC_AC_441_ExpAcaFacility_Comments_DMO();
                obj1.NCAC441EXACFCC_Remarks = data.Remarks;
                obj1.NCAC441EXACFCC_RemarksBy = data.UserId;
                obj1.NCAC441EXACFCC_StatusFlg = "";
                obj1.NCAC441EXACFC_Id = data.filefkid;
                obj1.NCAC441EXACFCC_ActiveFlag = true;
                obj1.NCAC441EXACFCC_CreatedBy = data.UserId;
                obj1.NCAC441EXACFCC_UpdatedBy = data.UserId;
                obj1.NCAC441EXACFCC_CreatedDate = DateTime.Now;
                obj1.NCAC441EXACFCC_UpdatedDate = DateTime.Now;
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
        public NaacExpAcaFacility441DTO savefilewisecomments(NaacExpAcaFacility441DTO data)
        {
            try
            {
                NAAC_AC_441_ExpAcaFacility_File_Comments_DMO obj1 = new NAAC_AC_441_ExpAcaFacility_File_Comments_DMO();
                obj1.NCAC441EXACFCFC_Remarks = data.Remarks;
                obj1.NCAC441EXACFCFC_RemarksBy = data.UserId;
                obj1.NCAC441EXACFCFC_StatusFlg = "";
                obj1.NCAC441EXACFCF_Id = data.filefkid;
                obj1.NCAC441EXACFCFC_ActiveFlag = true;
                obj1.NCAC441EXACFCFC_CreatedBy = data.UserId;
                obj1.NCAC441EXACFCFC_UpdatedBy = data.UserId;
                obj1.NCAC441EXACFCFC_UpdatedDate = DateTime.Now;
                obj1.NCAC441EXACFCFC_CreatedDate = DateTime.Now;
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
