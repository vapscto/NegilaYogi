using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using DomainModel.Model.NAAC.Admission.Criteria7;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.NAAC.Admission;

namespace NaacServiceHub.Admission.Services.Criteria7
{
    public class StatutoryBodiesImpl : Interface.Criteria7.StatutoryBodiesInterface
    {

        public GeneralContext _GeneralContext;
        public StatutoryBodiesImpl(GeneralContext para)
        {
            _GeneralContext = para;
        }

        public async  Task<NAAC_AC_7116_StatutoryBodies_DTO> loaddata(NAAC_AC_7116_StatutoryBodies_DTO data)
        {
            try
            {
                data.institutionlist = (from a in _GeneralContext.Institution
                                        from b in _GeneralContext.UserRoleWithInstituteDMO
                                        where (a.MI_Id == b.MI_Id && a.MI_ActiveFlag == 1 && b.Id == data.UserId)
                                        select new NAAC_AC_7116_StatutoryBodies_DTO
                                        {
                                            MI_Id = a.MI_Id,
                                            MI_Name = a.MI_Name
                                        }).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_AC_7116_StatutoryBodies_DTO savedatatab1(NAAC_AC_7116_StatutoryBodies_DTO data)
        {
            long s = 0;
            try
            {
                if (data.NCAC7116STABOD_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_7116_StatutoryBodiesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC7116STABOD_Year == data.NCAC7116STABOD_Year && t.NCAC7116STABOD_URL == data.NCAC7116STABOD_URL).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_7116_StatutoryBodiesDMO obj1 = new NAAC_AC_7116_StatutoryBodiesDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC7116STABOD_Year = data.NCAC7116STABOD_Year;
                        obj1.NCAC7116STABOD_URL = data.NCAC7116STABOD_URL;
                        obj1.NCAC7116STABOD_ActiveFlg = true;
                        obj1.NCAC7116STABOD_CreatedBy = data.UserId;
                        obj1.NCAC7116STABOD_UpdatedBy = data.UserId;
                        obj1.NCAC7116STABOD_CreatedDate = DateTime.Now;
                        obj1.NCAC7116STABOD_UpdatedDate = DateTime.Now;
                        obj1.NCAC7116STABOD_StatusFlg = "";
                        _GeneralContext.Add(obj1);
                        _GeneralContext.SaveChanges();
                        s = obj1.NCAC7116STABOD_Id;
                        if (data.NAACAC7116DTO.Count() > 0)
                        {
                            foreach (NAAC_AC_7116_StatutoryBodies_DTO DocumentsDTO in data.NAACAC7116DTO)
                            {
                                NAAC_AC_7116_StatutoryBodies_FilesDMO obj2 = new NAAC_AC_7116_StatutoryBodies_FilesDMO();
                                obj2.NCAC7116STABODF_FileName = DocumentsDTO.NCAC7116STABODF_FileName;
                                obj2.NCAC7116STABODF_Filedesc = DocumentsDTO.NCAC7116STABODF_Filedesc;
                                obj2.NCAC7116STABODF_FilePath = DocumentsDTO.NCAC7116STABODF_FilePath;
                                obj2.NCAC7116STABODF_ActiveFlg = true;
                                obj2.NCAC7116STABODF_StatusFlg = "";
                                obj2.NCAC7116STABOD_Id = s;
                                _GeneralContext.Add(obj2);
                               
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
                else if (data.NCAC7116STABOD_Id > 0)
                {
                    var update = _GeneralContext.NAAC_AC_7116_StatutoryBodiesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC7116STABOD_Id == data.NCAC7116STABOD_Id).SingleOrDefault();

                    update.NCAC7116STABOD_Year = data.NCAC7116STABOD_Year;
                    update.NCAC7116STABOD_URL = data.NCAC7116STABOD_URL;
                    update.NCAC7116STABOD_UpdatedBy = data.UserId;
                    update.NCAC7116STABOD_UpdatedDate = DateTime.Now;
                    _GeneralContext.Update(update);
                    _GeneralContext.SaveChanges();
                    s = update.NCAC7116STABOD_Id;
                    if (data.NAACAC7116DTO.Count() > 0)
                    {
                        List<long> Fid = new List<long>();
                        foreach (var item in data.NAACAC7116DTO)
                        {
                            Fid.Add(item.NCAC7116STABODF_Id);
                        }
                        var removefile11 = _GeneralContext.NAAC_AC_7116_StatutoryBodies_FilesDMO.Where(t => t.NCAC7116STABOD_Id == data.NCAC7116STABOD_Id && !Fid.Contains(t.NCAC7116STABODF_Id)).Distinct().ToList();
                        if (removefile11.Count > 0)
                        {
                            foreach (var item2 in removefile11)
                            {
                                var deactfile = _GeneralContext.NAAC_AC_7116_StatutoryBodies_FilesDMO.Single(t => t.NCAC7116STABOD_Id == data.NCAC7116STABOD_Id && t.NCAC7116STABODF_Id == item2.NCAC7116STABODF_Id);
                                deactfile.NCAC7116STABODF_ActiveFlg = false;
                                _GeneralContext.Update(deactfile);
                            }
                        }

                        foreach (NAAC_AC_7116_StatutoryBodies_DTO DocumentsDTO in data.NAACAC7116DTO)
                        {
                            if (DocumentsDTO.NCAC7116STABODF_Id > 0)
                            {
                                var filesdata = _GeneralContext.NAAC_AC_7116_StatutoryBodies_FilesDMO.Where(t => t.NCAC7116STABODF_Id == DocumentsDTO.NCAC7116STABODF_Id).FirstOrDefault();
                                filesdata.NCAC7116STABODF_Filedesc = DocumentsDTO.NCAC7116STABODF_Filedesc;
                                filesdata.NCAC7116STABODF_FileName = DocumentsDTO.NCAC7116STABODF_FileName;
                                filesdata.NCAC7116STABODF_FilePath = DocumentsDTO.NCAC7116STABODF_FilePath;
                                _GeneralContext.Update(filesdata);
                               
                            }
                            else
                            {
                                NAAC_AC_7116_StatutoryBodies_FilesDMO obj2 = new NAAC_AC_7116_StatutoryBodies_FilesDMO();
                                obj2.NCAC7116STABODF_FileName = DocumentsDTO.NCAC7116STABODF_FileName;
                                obj2.NCAC7116STABODF_Filedesc = DocumentsDTO.NCAC7116STABODF_Filedesc;
                                obj2.NCAC7116STABODF_FilePath = DocumentsDTO.NCAC7116STABODF_FilePath;
                                obj2.NCAC7116STABOD_Id = s;
                                obj2.NCAC7116STABODF_StatusFlg = "";
                                obj2.NCAC7116STABODF_ActiveFlg = true;
                                _GeneralContext.Add(obj2);
                               
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


                    //var duplicate = _GeneralContext.NAAC_AC_7116_StatutoryBodiesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC7116STABOD_Year == data.NCAC7116STABOD_Year && t.NCAC7116STABOD_URL == data.NCAC7116STABOD_URL && t.NCAC711GENEQ_FromDate == data.NCAC711GENEQ_FromDate && t.NCAC711GENEQ_ToDate == data.NCAC711GENEQ_ToDate && t.NCAC711GENEQ_NoOfParticipantsMale == data.NCAC711GENEQ_NoOfParticipantsMale && t.NCAC711GENEQ_NoOfParticipantsFeMale == data.NCAC711GENEQ_NoOfParticipantsFeMale).ToList();
                    //if (duplicate.Count > 0)
                    //{
                    //    data.duplicate = true;
                    //}
                    //else
                    //{

                    //}
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_AC_7116_StatutoryBodies_DTO deactivYTab1(NAAC_AC_7116_StatutoryBodies_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_7116_StatutoryBodiesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC7116STABOD_Id == data.NCAC7116STABOD_Id).SingleOrDefault();

                if (result.NCAC7116STABOD_ActiveFlg == true)
                {
                    result.NCAC7116STABOD_ActiveFlg = false;
                }
                else if (result.NCAC7116STABOD_ActiveFlg == false)
                {
                    result.NCAC7116STABOD_ActiveFlg = true;
                }

                result.NCAC7116STABOD_UpdatedDate = DateTime.Now;
                result.NCAC7116STABOD_UpdatedBy = data.UserId;

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

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }

        public NAAC_AC_7116_StatutoryBodies_DTO editTab1(NAAC_AC_7116_StatutoryBodies_DTO data)
        {
            try
            {
                var edit = _GeneralContext.NAAC_AC_7116_StatutoryBodiesDMO.Where(t => t.NCAC7116STABOD_Id == data.NCAC7116STABOD_Id).ToList();
                var editfile = _GeneralContext.NAAC_AC_7116_StatutoryBodies_FilesDMO.Where(t => t.NCAC7116STABOD_Id == data.NCAC7116STABOD_Id&&t.NCAC7116STABODF_ActiveFlg==true).ToList();

                data.editlisttab1 = edit.ToArray();
                data.editfilelist = editfile.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_AC_7116_StatutoryBodies_DTO deleteuploadfile(NAAC_AC_7116_StatutoryBodies_DTO data)
        {
            try
            {
                List<NAAC_AC_7116_StatutoryBodies_FilesDMO> removelist = new List<NAAC_AC_7116_StatutoryBodies_FilesDMO>();
                removelist = _GeneralContext.NAAC_AC_7116_StatutoryBodies_FilesDMO.Where(t => t.NCAC7116STABODF_Id == data.NCAC7116STABODF_Id).ToList();
                foreach (NAAC_AC_7116_StatutoryBodies_FilesDMO obj1 in removelist)
                {

                    obj1.NCAC7116STABODF_ActiveFlg = false;
                    _GeneralContext.Update(obj1);
                    // _GeneralContext.Remove(obj1);
                    _GeneralContext.SaveChanges();
                    data.retrunMsg = "Deleted";
                    data.returnval = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_AC_7116_StatutoryBodies_DTO getData(NAAC_AC_7116_StatutoryBodies_DTO data)
        {
            try
            {
                data.allacademicyear = _GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.alldatalist = (from a in _GeneralContext.NAAC_AC_7116_StatutoryBodiesDMO
                                    from b in _GeneralContext.Academic
                                    where (a.MI_Id == data.MI_Id && a.NCAC7116STABOD_Year == b.ASMAY_Id && b.MI_Id == a.MI_Id)
                                    select new NAAC_AC_7116_StatutoryBodies_DTO
                                    {
                                        NCAC7116STABOD_Id = a.NCAC7116STABOD_Id,
                                        MI_Id = a.MI_Id,
                                        NCAC7116STABOD_Year = a.NCAC7116STABOD_Year,
                                        ASMAY_Year = b.ASMAY_Year,
                                        NCAC7116STABOD_URL = a.NCAC7116STABOD_URL,
                                        NCAC7116STABOD_ActiveFlg = a.NCAC7116STABOD_ActiveFlg,
                                        NCAC7116STABOD_StatusFlg = a.NCAC7116STABOD_StatusFlg
                                    }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public NAAC_AC_7116_StatutoryBodies_DTO getcomment(NAAC_AC_7116_StatutoryBodies_DTO data)
        {
            try
            {
                data.commentlist = (from a in _GeneralContext.NAAC_AC_7116_StatutoryBodies_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCAC7116STABODC_RemarksBy == b.Id && a.NCAC7116STABOD_Id == data.NCAC7116STABOD_Id)
                                    select new NAAC_AC_7116_StatutoryBodies_DTO
                                    {
                                        NCAC7116STABODC_Remarks = a.NCAC7116STABODC_Remarks,
                                        NCAC7116STABODC_Id = a.NCAC7116STABODC_Id,
                                        NCAC7116STABODC_RemarksBy = a.NCAC7116STABODC_RemarksBy,
                                        NCAC7116STABODC_StatusFlg = a.NCAC7116STABODC_StatusFlg,
                                        NCAC7116STABODC_ActiveFlag = a.NCAC7116STABODC_ActiveFlag,
                                        NCAC7116STABODC_CreatedBy = a.NCAC7116STABODC_CreatedBy,
                                        NCAC7116STABODC_CreatedDate = a.NCAC7116STABODC_CreatedDate,
                                        NCAC7116STABODC_UpdatedBy = a.NCAC7116STABODC_UpdatedBy,
                                        NCAC7116STABODC_UpdatedDate = a.NCAC7116STABODC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCAC7116STABODC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        // for file
        public NAAC_AC_7116_StatutoryBodies_DTO getfilecomment(NAAC_AC_7116_StatutoryBodies_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_7116_StatutoryBodies_File_Comments_DMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCAC7116STABODFC_RemarksBy == b.Id && a.NCAC7116STABODF_Id == data.NCAC7116STABODF_Id)
                                     select new NAAC_AC_7116_StatutoryBodies_DTO
                                     {
                                         NCAC7116STABODF_Id = a.NCAC7116STABODF_Id,
                                         NCAC7116STABODFC_Remarks = a.NCAC7116STABODFC_Remarks,
                                         NCAC7116STABODFC_Id = a.NCAC7116STABODFC_Id,
                                         NCAC7116STABODFC_RemarksBy = a.NCAC7116STABODFC_RemarksBy,
                                         NCAC7116STABODFC_StatusFlg = a.NCAC7116STABODFC_StatusFlg,
                                         NCAC7116STABODFC_ActiveFlag = a.NCAC7116STABODFC_ActiveFlag,
                                         NCAC7116STABODFC_CreatedBy = a.NCAC7116STABODFC_CreatedBy,
                                         NCAC7116STABODFC_CreatedDate = a.NCAC7116STABODFC_CreatedDate,
                                         NCAC7116STABODFC_UpdatedBy = a.NCAC7116STABODFC_UpdatedBy,
                                         NCAC7116STABODFC_UpdatedDate = a.NCAC7116STABODFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCAC7116STABODFC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_7116_StatutoryBodies_DTO savemedicaldatawisecomments(NAAC_AC_7116_StatutoryBodies_DTO data)
        {
            try
            {
                NAAC_AC_7116_StatutoryBodies_Comments_DMO obj1 = new NAAC_AC_7116_StatutoryBodies_Comments_DMO();
                obj1.NCAC7116STABODC_Remarks = data.Remarks;
                obj1.NCAC7116STABODC_RemarksBy = data.UserId;
                obj1.NCAC7116STABODC_StatusFlg = "";
                obj1.NCAC7116STABOD_Id = data.filefkid;
                obj1.NCAC7116STABODC_ActiveFlag = true;
                obj1.NCAC7116STABODC_CreatedBy = data.UserId;
                obj1.NCAC7116STABODC_UpdatedBy = data.UserId;
                obj1.NCAC7116STABODC_CreatedDate = DateTime.Now;
                obj1.NCAC7116STABODC_UpdatedDate = DateTime.Now;
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
        public NAAC_AC_7116_StatutoryBodies_DTO savefilewisecomments(NAAC_AC_7116_StatutoryBodies_DTO data)
        {
            try
            {
                NAAC_AC_7116_StatutoryBodies_File_Comments_DMO obj1 = new NAAC_AC_7116_StatutoryBodies_File_Comments_DMO();
                obj1.NCAC7116STABODFC_Remarks = data.Remarks;
                obj1.NCAC7116STABODFC_RemarksBy = data.UserId;
                obj1.NCAC7116STABODFC_StatusFlg = "";
                obj1.NCAC7116STABODF_Id = data.filefkid;
                obj1.NCAC7116STABODFC_ActiveFlag = true;
                obj1.NCAC7116STABODFC_CreatedBy = data.UserId;
                obj1.NCAC7116STABODFC_UpdatedBy = data.UserId;
                obj1.NCAC7116STABODFC_UpdatedDate = DateTime.Now;
                obj1.NCAC7116STABODFC_CreatedDate = DateTime.Now;
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
