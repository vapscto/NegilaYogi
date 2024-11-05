using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using DomainModel.Model.NAAC.Admission.Criteria7;
using PreadmissionDTOs.NAAC.Admission.Criteria7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services.Criteria7
{
    public class LocationalAdvtgImpl : Interface.Criteria7.LocationalAdvtgInterface
    {
        public GeneralContext _GeneralContext;
        public LocationalAdvtgImpl(GeneralContext para)
        {
            _GeneralContext = para;
        }
        public async Task<LocationalAdvtgDTO> loaddata(LocationalAdvtgDTO data)
        {
            try
            {
                data.institutionlist = (from a in _GeneralContext.Institution
                                        from b in _GeneralContext.UserRoleWithInstituteDMO
                                        where (a.MI_Id == b.MI_Id && b.Id == data.UserId)
                                        select new LocationalAdvtgDTO
                                        {
                                            MI_Id = a.MI_Id,
                                            MI_Name = a.MI_Name,
                                            UserId = b.Id
                                        }).Distinct().ToArray();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public LocationalAdvtgDTO getdata(LocationalAdvtgDTO data)
        {
            try
            {
                data.allacademicyear = _GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.alldatalist = (from a in _GeneralContext.NAAC_AC_7110_LocationalAdvtgDMO
                                    from b in _GeneralContext.Academic
                                    from c in _GeneralContext.NAAC_AC_7110_LocationalAdvtg_FilesDMO
                                    where (a.MI_Id == data.MI_Id && a.NCAC7110LOCADVTG_Year == b.ASMAY_Id && b.MI_Id == a.MI_Id && a.NCAC7110LOCADVTG_Id == c.NCAC7110LOCADVTG_Id )
                                    select new LocationalAdvtgDTO
                                    {
                                        NCAC7110LOCADVTG_Id = a.NCAC7110LOCADVTG_Id,
                                        NCAC7110LOCADVTG_Year = a.NCAC7110LOCADVTG_Year,
                                        ASMAY_Year = b.ASMAY_Year,
                                        MI_Id = a.MI_Id,
                                        NCAC7110LOCADVTG_NoOfEngage = a.NCAC7110LOCADVTG_NoOfEngage,
                                        NCAC7110LOCADVTG_Duration = a.NCAC7110LOCADVTG_Duration,
                                        NCAC7110LOCADVTG_IssuesAddressed = a.NCAC7110LOCADVTG_IssuesAddressed,
                                        NCAC7110LOCADVTG_NoOfAddress = a.NCAC7110LOCADVTG_NoOfAddress,
                                        NCAC7110LOCADVTG_Date = a.NCAC7110LOCADVTG_Date,
                                        NCAC7110LOCADVTG_InitiativeName = a.NCAC7110LOCADVTG_InitiativeName,
                                        NCAC7110LOCADVTG_NoOfParticipant = a.NCAC7110LOCADVTG_NoOfParticipant,
                                        NCAC7110LOCADVTG_ActiveFlg = a.NCAC7110LOCADVTG_ActiveFlg,
                                        NCAC7110LOCADVTG_StatusFlg = a.NCAC7110LOCADVTG_StatusFlg,
                                        NCAC7110LOCADVTGF_Filedesc = c.NCAC7110LOCADVTGF_Filedesc
                                    }).ToArray();
            }
            catch(Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public LocationalAdvtgDTO savedatatab1(LocationalAdvtgDTO data)
        {
            long s = 0;
            int flag = 0;
            try
            {
                if (data.NCAC7110LOCADVTG_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_7110_LocationalAdvtgDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC7110LOCADVTG_Year == data.NCAC7110LOCADVTG_Year && t.NCAC7110LOCADVTG_NoOfEngage == data.NCAC7110LOCADVTG_NoOfEngage && t.NCAC7110LOCADVTG_Duration == data.NCAC7110LOCADVTG_Duration && t.NCAC7110LOCADVTG_IssuesAddressed == data.NCAC7110LOCADVTG_IssuesAddressed && t.NCAC7110LOCADVTG_NoOfAddress == data.NCAC7110LOCADVTG_NoOfAddress && t.NCAC7110LOCADVTG_Date == data.NCAC7110LOCADVTG_Date && t.NCAC7110LOCADVTG_InitiativeName == data.NCAC7110LOCADVTG_InitiativeName && t.NCAC7110LOCADVTG_NoOfParticipant == data.NCAC7110LOCADVTG_NoOfParticipant).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_7110_LocationalAdvtgDMO obj = new NAAC_AC_7110_LocationalAdvtgDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.NCAC7110LOCADVTG_Year = data.NCAC7110LOCADVTG_Year;
                        obj.NCAC7110LOCADVTG_NoOfEngage = data.NCAC7110LOCADVTG_NoOfEngage;
                        obj.NCAC7110LOCADVTG_Duration = data.NCAC7110LOCADVTG_Duration;
                        obj.NCAC7110LOCADVTG_IssuesAddressed = data.NCAC7110LOCADVTG_IssuesAddressed;
                        obj.NCAC7110LOCADVTG_NoOfAddress = data.NCAC7110LOCADVTG_NoOfAddress;
                        obj.NCAC7110LOCADVTG_Date = data.NCAC7110LOCADVTG_Date;
                        obj.NCAC7110LOCADVTG_InitiativeName = data.NCAC7110LOCADVTG_InitiativeName;
                        obj.NCAC7110LOCADVTG_NoOfParticipant = data.NCAC7110LOCADVTG_NoOfParticipant;
                        obj.NCAC7110LOCADVTG_ActiveFlg = true;
                        obj.NCAC7110LOCADVTG_CreatedBy = data.UserId;
                        obj.NCAC7110LOCADVTG_UpdatedBy = data.UserId;
                        obj.NCAC7110LOCADVTG_CreatedDate = DateTime.Now;
                        obj.NCAC7110LOCADVTG_UpdatedDate = DateTime.Now;

                        _GeneralContext.Add(obj);
                        _GeneralContext.SaveChanges();
                        s = obj.NCAC7110LOCADVTG_Id;
                        if (data.NAACAC711LocationalAdvtgDTO.Count() > 0)
                        {
                            foreach (LocationalAdvtgDTO DocumentsDTO in data.NAACAC711LocationalAdvtgDTO)
                            {
                                NAAC_AC_7110_LocationalAdvtg_FilesDMO obj2 = new NAAC_AC_7110_LocationalAdvtg_FilesDMO();
                                obj2.NCAC7110LOCADVTGF_FileName = DocumentsDTO.NCAC7110LOCADVTGF_FileName;
                                obj2.NCAC7110LOCADVTGF_Filedesc = DocumentsDTO.NCAC7110LOCADVTGF_Filedesc;
                                obj2.NCAC7110LOCADVTGF_FilePath = DocumentsDTO.NCAC7110LOCADVTGF_FilePath;
                                obj2.NCAC7110LOCADVTG_Id = s;
                                _GeneralContext.Add(obj2);
                                flag = _GeneralContext.SaveChanges();
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
                        else if (s > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else if (data.NCAC7110LOCADVTG_Id > 0)
                {
                    var update = _GeneralContext.NAAC_AC_7110_LocationalAdvtgDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC7110LOCADVTG_Id == data.NCAC7110LOCADVTG_Id).SingleOrDefault();
                    update.NCAC7110LOCADVTG_Year = data.NCAC7110LOCADVTG_Year;
                    update.NCAC7110LOCADVTG_NoOfEngage = data.NCAC7110LOCADVTG_NoOfEngage;
                    update.NCAC7110LOCADVTG_Duration = data.NCAC7110LOCADVTG_Duration;
                    update.NCAC7110LOCADVTG_IssuesAddressed = data.NCAC7110LOCADVTG_IssuesAddressed;
                    update.NCAC7110LOCADVTG_NoOfAddress = data.NCAC7110LOCADVTG_NoOfAddress;
                    update.NCAC7110LOCADVTG_Date = data.NCAC7110LOCADVTG_Date;
                    update.NCAC7110LOCADVTG_InitiativeName = data.NCAC7110LOCADVTG_InitiativeName;
                    update.NCAC7110LOCADVTG_NoOfParticipant = data.NCAC7110LOCADVTG_NoOfParticipant;
                    update.NCAC7110LOCADVTG_UpdatedBy = data.UserId;
                    update.NCAC7110LOCADVTG_UpdatedDate = DateTime.Now;
                    _GeneralContext.Update(update);
                    s = update.NCAC7110LOCADVTG_Id;
                    if (data.NAACAC711LocationalAdvtgDTO.Count() > 0)
                    {
                        foreach (LocationalAdvtgDTO DocumentsDTO in data.NAACAC711LocationalAdvtgDTO)
                        {
                            if (DocumentsDTO.NCAC7110LOCADVTGF_Id > 0)
                            {
                                var filesdata = _GeneralContext.NAAC_AC_7110_LocationalAdvtg_FilesDMO.Where(t => t.NCAC7110LOCADVTGF_Id == DocumentsDTO.NCAC7110LOCADVTGF_Id).FirstOrDefault();
                                filesdata.NCAC7110LOCADVTGF_FileName = DocumentsDTO.NCAC7110LOCADVTGF_FileName;
                                filesdata.NCAC7110LOCADVTGF_Filedesc = DocumentsDTO.NCAC7110LOCADVTGF_Filedesc;
                                filesdata.NCAC7110LOCADVTGF_FilePath = DocumentsDTO.NCAC7110LOCADVTGF_FilePath;
                                _GeneralContext.Update(filesdata);
                                flag = _GeneralContext.SaveChanges();
                                if (flag > 0)
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
                                NAAC_AC_7110_LocationalAdvtg_FilesDMO obj2 = new NAAC_AC_7110_LocationalAdvtg_FilesDMO();
                                obj2.NCAC7110LOCADVTGF_FileName = DocumentsDTO.NCAC7110LOCADVTGF_FileName;
                                obj2.NCAC7110LOCADVTGF_Filedesc = DocumentsDTO.NCAC7110LOCADVTGF_Filedesc;
                                obj2.NCAC7110LOCADVTGF_FilePath = DocumentsDTO.NCAC7110LOCADVTGF_FilePath;
                                obj2.NCAC7110LOCADVTG_Id = s;
                                _GeneralContext.Add(obj2);
                                flag = _GeneralContext.SaveChanges();
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
                    else if (s > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }

                    //var duplicate = _GeneralContext.NAAC_AC_7110_LocationalAdvtgDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC7110LOCADVTG_Year == data.NCAC7110LOCADVTG_Year && t.NCAC7110LOCADVTG_NoOfEngage == data.NCAC7110LOCADVTG_NoOfEngage && t.NCAC7110LOCADVTG_Duration == data.NCAC7110LOCADVTG_Duration && t.NCAC7110LOCADVTG_IssuesAddressed == data.NCAC7110LOCADVTG_IssuesAddressed && t.NCAC7110LOCADVTG_NoOfAddress == data.NCAC7110LOCADVTG_NoOfAddress && t.NCAC7110LOCADVTG_Date == data.NCAC7110LOCADVTG_Date && t.NCAC7110LOCADVTG_InitiativeName == data.NCAC7110LOCADVTG_InitiativeName && t.NCAC7110LOCADVTG_NoOfParticipant == data.NCAC7110LOCADVTG_NoOfParticipant).ToList();
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
        public LocationalAdvtgDTO edittab1(LocationalAdvtgDTO data)
        {
            try
            {
                var edit = _GeneralContext.NAAC_AC_7110_LocationalAdvtgDMO.Where(t =>  t.NCAC7110LOCADVTG_Id == data.NCAC7110LOCADVTG_Id).ToList();
                data.editlisttab1 = (from a in _GeneralContext.Academic
                                    from b in _GeneralContext.NAAC_AC_7110_LocationalAdvtgDMO
                                    where (b.NCAC7110LOCADVTG_Id == data.NCAC7110LOCADVTG_Id && a.ASMAY_Id == b.NCAC7110LOCADVTG_Year && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id)
                                    select new LocationalAdvtgDTO
                                    {
                                        NCAC7110LOCADVTG_Id = b.NCAC7110LOCADVTG_Id,
                                        NCAC7110LOCADVTG_Year = b.NCAC7110LOCADVTG_Year,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCAC7110LOCADVTG_NoOfAddress = b.NCAC7110LOCADVTG_NoOfAddress,
                                        NCAC7110LOCADVTG_NoOfEngage = b.NCAC7110LOCADVTG_NoOfEngage,
                                        NCAC7110LOCADVTG_Duration = b.NCAC7110LOCADVTG_Duration,
                                        NCAC7110LOCADVTG_InitiativeName = b.NCAC7110LOCADVTG_InitiativeName,
                                        NCAC7110LOCADVTG_IssuesAddressed = b.NCAC7110LOCADVTG_IssuesAddressed,
                                        NCAC7110LOCADVTG_NoOfParticipant = b.NCAC7110LOCADVTG_NoOfParticipant,
                                        NCAC7110LOCADVTG_StatusFlg = b.NCAC7110LOCADVTG_StatusFlg,
                                        NCAC7110LOCADVTG_Date = b.NCAC7110LOCADVTG_Date
                                    }).Distinct().ToArray();
                
                var editfilelist = _GeneralContext.NAAC_AC_7110_LocationalAdvtg_FilesDMO.Where(t => t.NCAC7110LOCADVTG_Id == data.NCAC7110LOCADVTG_Id).ToList();            
                data.editfilelist = editfilelist.ToArray();
                var testfile = _GeneralContext.NAAC_AC_7110_LocationalAdvtg_FilesDMO.Where(t => t.NCAC7110LOCADVTG_Id == data.NCAC7110LOCADVTG_Id).ToList();
                for (int t = 0; t < testfile.Count; t++)
                {
                    if (testfile[t].NCAC7110LOCADVTGF_StatusFlg == "approved")
                    {

                    }                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LocationalAdvtgDTO deactivYTab1(LocationalAdvtgDTO data)
   {
            try
            {
                var result = _GeneralContext.NAAC_AC_7110_LocationalAdvtgDMO.Where(t =>  t.NCAC7110LOCADVTG_Id == data.NCAC7110LOCADVTG_Id).SingleOrDefault();

                if (result.NCAC7110LOCADVTG_ActiveFlg == true)
                {
                    result.NCAC7110LOCADVTG_ActiveFlg = false;
                }
                else if (result.NCAC7110LOCADVTG_ActiveFlg == false)
                {
                    result.NCAC7110LOCADVTG_ActiveFlg = true;
                }
                result.NCAC7110LOCADVTG_UpdatedDate = DateTime.Now;
                result.NCAC7110LOCADVTG_UpdatedBy = data.UserId;
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
        public LocationalAdvtgDTO deleteuploadfile(LocationalAdvtgDTO data)
        {
            try
            {
                List<NAAC_AC_7110_LocationalAdvtg_FilesDMO> removelist = new List<NAAC_AC_7110_LocationalAdvtg_FilesDMO>();
                removelist = _GeneralContext.NAAC_AC_7110_LocationalAdvtg_FilesDMO.Where(t => t.NCAC7110LOCADVTGF_Id == data.NCAC7110LOCADVTGF_Id).ToList();
                foreach (NAAC_AC_7110_LocationalAdvtg_FilesDMO obj1 in removelist)
                {
                    _GeneralContext.Remove(obj1);
                    _GeneralContext.SaveChanges();
                    data.returnval = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LocationalAdvtgDTO viewuploadflies(LocationalAdvtgDTO data)
        {
            try
            {
                data.view = _GeneralContext.NAAC_AC_7110_LocationalAdvtg_FilesDMO.Where(t => t.NCAC7110LOCADVTG_Id == data.NCAC7110LOCADVTG_Id).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;

        }
        public LocationalAdvtgDTO getcomment(LocationalAdvtgDTO data)
        {
            try
            {

                data.commentlist = (from a in _GeneralContext.NAAC_AC_7110_LocationalAdvtg_CommentsDMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCAC7110LOCADVTGC_RemarksBy == b.Id && a.NCAC7110LOCADVTG_Id == data.NCAC7110LOCADVTG_Id)
                                    select new LocationalAdvtgDTO
                                    {

                                        NCAC7110LOCADVTGC_Remarks = a.NCAC7110LOCADVTGC_Remarks,
                                        NCAC7110LOCADVTGC_Id = a.NCAC7110LOCADVTGC_Id,
                                        NCAC7110LOCADVTGC_RemarksBy = a.NCAC7110LOCADVTGC_RemarksBy,
                                        NCAC7110LOCADVTGC_StatusFlg = a.NCAC7110LOCADVTGC_StatusFlg,
                                        NCAC7110LOCADVTGC_ActiveFlag = a.NCAC7110LOCADVTGC_ActiveFlag,
                                        NCAC7110LOCADVTGC_CreatedBy = a.NCAC7110LOCADVTGC_CreatedBy,
                                        NCAC7110LOCADVTGC_CreatedDate = a.NCAC7110LOCADVTGC_CreatedDate,
                                        NCAC7110LOCADVTGC_UpdatedBy = a.NCAC7110LOCADVTGC_UpdatedBy,
                                        NCAC7110LOCADVTGC_UpdatedDate = a.NCAC7110LOCADVTGC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LocationalAdvtgDTO getfilecomment(LocationalAdvtgDTO data)
        {
            try
            {

                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_7110_LocationalAdvtg_File_CommentsDMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCAC7110LOCADVTGFC_RemarksBy == b.Id && a.NCAC7110LOCADVTGF_Id == data.NCAC7110LOCADVTGF_Id)
                                     select new LocationalAdvtgDTO
                                     {
                                         NCAC7110LOCADVTGF_Id = a.NCAC7110LOCADVTGF_Id,
                                         NCAC7110LOCADVTGFC_Remarks = a.NCAC7110LOCADVTGFC_Remarks,
                                         NCAC7110LOCADVTGFC_Id = a.NCAC7110LOCADVTGFC_Id,
                                         NCAC7110LOCADVTGFC_RemarksBy = a.NCAC7110LOCADVTGFC_RemarksBy,
                                         NCAC7110LOCADVTGFC_StatusFlg = a.NCAC7110LOCADVTGFC_StatusFlg,
                                         NCAC7110LOCADVTGFC_ActiveFlag = a.NCAC7110LOCADVTGFC_ActiveFlag,
                                         NCAC7110LOCADVTGFC_CreatedBy = a.NCAC7110LOCADVTGFC_CreatedBy,
                                         NCAC7110LOCADVTGFC_CreatedDate = a.NCAC7110LOCADVTGFC_CreatedDate,
                                         NCAC7110LOCADVTGFC_UpdatedBy = a.NCAC7110LOCADVTGFC_UpdatedBy,
                                         NCAC7110LOCADVTGFC_UpdatedDate = a.NCAC7110LOCADVTGFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LocationalAdvtgDTO savecomments(LocationalAdvtgDTO data)
        {
            try
            {
                NAAC_AC_7110_LocationalAdvtg_CommentsDMO obj1 = new NAAC_AC_7110_LocationalAdvtg_CommentsDMO();
                obj1.NCAC7110LOCADVTGC_Remarks = data.Remarks;
                obj1.NCAC7110LOCADVTGC_RemarksBy = data.UserId;
                obj1.NCAC7110LOCADVTGC_StatusFlg = "";
                obj1.NCAC7110LOCADVTG_Id = data.filefkid;
                obj1.NCAC7110LOCADVTGC_ActiveFlag = true;
                obj1.NCAC7110LOCADVTGC_CreatedBy = data.UserId;
                obj1.NCAC7110LOCADVTGC_UpdatedBy = data.UserId;
                obj1.NCAC7110LOCADVTGC_CreatedDate = DateTime.Now;
                obj1.NCAC7110LOCADVTGC_UpdatedDate = DateTime.Now;
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
        public LocationalAdvtgDTO savefilewisecomments(LocationalAdvtgDTO data)
        {
            try
            {
                NAAC_AC_7110_LocationalAdvtg_File_CommentsDMO obj1 = new NAAC_AC_7110_LocationalAdvtg_File_CommentsDMO();


                obj1.NCAC7110LOCADVTGFC_Remarks = data.Remarks;
                obj1.NCAC7110LOCADVTGFC_RemarksBy = data.UserId;
                obj1.NCAC7110LOCADVTGFC_StatusFlg = "";
                obj1.NCAC7110LOCADVTGF_Id = data.filefkid;
                obj1.NCAC7110LOCADVTGFC_ActiveFlag = true;
                obj1.NCAC7110LOCADVTGFC_CreatedBy = data.UserId;
                obj1.NCAC7110LOCADVTGFC_UpdatedBy = data.UserId;
                obj1.NCAC7110LOCADVTGFC_CreatedDate = DateTime.Now;
                obj1.NCAC7110LOCADVTGFC_UpdatedDate = DateTime.Now;
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
