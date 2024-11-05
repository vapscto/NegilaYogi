using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NaacIPRImpl : Interface.NaacIPRInterface
    {
        public GeneralContext _context;
        public NaacIPRImpl(GeneralContext w)
        {
            _context = w;
        }

        public NAAC_AC_IPR_322_DTO loaddata(NAAC_AC_IPR_322_DTO data)
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

                data.allacademicyear = _context.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Year).ToArray();

                data.alldata1 = (from a in _context.Academic
                                 from b in _context.NAAC_AC_IPR_322_DMO
                                 where (a.MI_Id == b.MI_Id && b.NCACIPR322_Year == a.ASMAY_Id && a.Is_Active == true && a.MI_Id == data.MI_Id)
                                 select new NAAC_AC_IPR_322_DTO
                                 {
                                     NCACIPR322_Id = b.NCACIPR322_Id,
                                     NCACIPR322_EstablishmentDate = b.NCACIPR322_EstablishmentDate,
                                     NCACIPR322_Year = b.NCACIPR322_Year,
                                     NCACIPR322_WorkshopName = b.NCACIPR322_WorkshopName,
                                     NCACIPR322_FromDate = Convert.ToDateTime(b.NCACIPR322_FromDate),
                                     NCACIPR322_ToDate = Convert.ToDateTime(b.NCACIPR322_ToDate),
                                     NCACIPR322_WebisteLink = b.NCACIPR322_WebisteLink,
                                     MI_Id = b.MI_Id,
                                     NCACIPR322_ActiveFlg = b.NCACIPR322_ActiveFlg,
                                     NCACIPR322_StatusFlg = b.NCACIPR322_StatusFlg,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().OrderByDescending(t => t.NCACIPR322_Id).ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_AC_IPR_322_DTO save(NAAC_AC_IPR_322_DTO data)
        {
            try
            {
                if (data.NCACIPR322_Id == 0)
                {
                    var duplicate = _context.NAAC_AC_IPR_322_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCACIPR322_Id != 0 && t.NCACIPR322_WorkshopName == data.NCACIPR322_WorkshopName && t.NCACIPR322_WebisteLink == data.NCACIPR322_WebisteLink && t.NCACIPR322_EstablishmentDate == data.NCACIPR322_EstablishmentDate).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_IPR_322_DMO rrr = new NAAC_AC_IPR_322_DMO();
                        rrr.MI_Id = data.MI_Id;
                        rrr.NCACIPR322_FromDate = data.NCACIPR322_FromDate;
                        rrr.NCACIPR322_ToDate = data.NCACIPR322_ToDate;
                        rrr.NCACIPR322_EstablishmentDate = data.NCACIPR322_EstablishmentDate;
                        rrr.NCACIPR322_Year = data.ASMAY_Id;
                        rrr.NCACIPR322_WorkshopName = data.NCACIPR322_WorkshopName;
                        rrr.NCACIPR322_WebisteLink = data.NCACIPR322_WebisteLink;
                        rrr.NCACIPR322_CreatedDate = DateTime.Now;
                        rrr.NCACIPR322_UpdatedDate = DateTime.Now;
                        rrr.NCACIPR322_ActiveFlg = true;
                        rrr.NCACIPR322_CreatedBy = data.UserId;
                        rrr.NCACIPR322_UpdatedBy = data.UserId;
                        rrr.NCACIPR322_NoOfParticipants = data.NCACIPR322_NoOfParticipants;
                        rrr.NCACIPR322_StatusFlg ="";
                        rrr.NCACIPR322_NameOfThePrincipalInvst = data.NCACIPR322_NameOfThePrincipalInvst;
                        rrr.NCACIPR322_DeptOfPrincialInvst = data.NCACIPR322_DeptOfPrincialInvst;

                        _context.Add(rrr);

                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[i].cfilepath != null)
                                {
                                    NAAC_AC_IPR_322_Files_DMO obj2 = new NAAC_AC_IPR_322_Files_DMO();

                                    obj2.NCACIPR322_Id = rrr.NCACIPR322_Id;
                                    obj2.NCACIPR322F_StatusFlg = "";
                                    obj2.NCACIPR322F_ActiveFlg = true;
                                    obj2.NCACIPR322F_FileName = data.filelist[i].cfilename;
                                    obj2.NCACIPR322F_Filedesc = data.filelist[i].cfiledesc;
                                    obj2.NCACIPR322F_FilePath = data.filelist[i].cfilepath;

                                    _context.Add(obj2);
                                }
                            }
                        }

                        int y = _context.SaveChanges();
                        if (y > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else if (data.NCACIPR322_Id > 0)
                {
                    var duplicate = _context.NAAC_AC_IPR_322_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCACIPR322_WorkshopName == data.NCACIPR322_WorkshopName && t.NCACIPR322_WebisteLink == data.NCACIPR322_WebisteLink && t.NCACIPR322_Year == data.NCACIPR322_Year && t.NCACIPR322_Id != data.NCACIPR322_Id).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var yy = _context.NAAC_AC_IPR_322_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCACIPR322_Id == data.NCACIPR322_Id).SingleOrDefault();

                        yy.NCACIPR322_UpdatedBy = data.UserId;
                        yy.NCACIPR322_WorkshopName = data.NCACIPR322_WorkshopName;
                        yy.NCACIPR322_WebisteLink = data.NCACIPR322_WebisteLink;
                        yy.NCACIPR322_EstablishmentDate = data.NCACIPR322_EstablishmentDate;
                        yy.NCACIPR322_NoOfParticipants = data.NCACIPR322_NoOfParticipants;
                        yy.NCACIPR322_UpdatedDate = DateTime.Now;
                        yy.NCACIPR322_NameOfThePrincipalInvst = data.NCACIPR322_NameOfThePrincipalInvst;
                        yy.NCACIPR322_DeptOfPrincialInvst = data.NCACIPR322_DeptOfPrincialInvst;
                        yy.MI_Id = data.MI_Id;
                        _context.Update(yy);

                       
                        if (data.filelist.Length > 0)
                        {

                            List<long> Fid = new List<long>();
                            foreach (var item in data.filelist)
                            {
                                Fid.Add(item.NCACIPR322F_Id);
                            }
                            var removefile11 = _context.NAAC_AC_IPR_322_Files_DMO.Where(t => t.NCACIPR322_Id == data.NCACIPR322_Id && !Fid.Contains(t.NCACIPR322F_Id)).Distinct().ToList();

                            if (removefile11.Count > 0)
                            {
                                foreach (var item2 in removefile11)
                                {
                                    var deactfile = _context.NAAC_AC_IPR_322_Files_DMO.Single(t => t.NCACIPR322_Id == data.NCACIPR322_Id && t.NCACIPR322F_Id == item2.NCACIPR322F_Id);
                                    deactfile.NCACIPR322F_ActiveFlg = false;
                                    _context.Update(deactfile);

                                }

                            }
                            //var CountRemoveFiles = _context.NAAC_AC_IPR_322_Files_DMO.Where(t => t.NCACIPR322_Id == data.NCACIPR322_Id&&t.NCACIPR322F_StatusFlg!="approved").ToList();
                            //if (CountRemoveFiles.Count > 0)
                            //{
                            //    foreach (var RemoveFiles in CountRemoveFiles)
                            //    {
                            //        _context.Remove(RemoveFiles);
                            //    }
                            //}


                            foreach (NAAC_AC_IPR_322_DTO DocumentsDTO in data.filelist)
                            {

                                if (DocumentsDTO.NCACIPR322F_Id > 0 && DocumentsDTO.NCACIPR322F_StatusFlg != "approved")
                                {
                                    if (DocumentsDTO.cfilepath != null)
                                    {

                                        var filesdata = _context.NAAC_AC_IPR_322_Files_DMO.Where(t => t.NCACIPR322F_Id == DocumentsDTO.NCACIPR322F_Id).FirstOrDefault();
                                        filesdata.NCACIPR322F_Filedesc = DocumentsDTO.cfiledesc;
                                        filesdata.NCACIPR322F_FileName = DocumentsDTO.cfilename;
                                        filesdata.NCACIPR322F_FilePath = DocumentsDTO.cfilepath;


                                        _context.Update(filesdata);
                                       
                                    }
                                }
                                else
                                {

                                    if (DocumentsDTO.NCACIPR322F_Id == 0)
                                    {
                                        if (DocumentsDTO.cfilepath != null)
                                        {
                                            NAAC_AC_IPR_322_Files_DMO obj2 = new NAAC_AC_IPR_322_Files_DMO();
                                            obj2.NCACIPR322F_FileName = DocumentsDTO.cfilename;
                                            obj2.NCACIPR322F_Filedesc = DocumentsDTO.cfiledesc;
                                            obj2.NCACIPR322F_FilePath = DocumentsDTO.cfilepath;
                                            obj2.NCACIPR322F_StatusFlg = "";
                                            obj2.NCACIPR322F_ActiveFlg = true;
                                            obj2.NCACIPR322_Id = data.NCACIPR322_Id;
                                            _context.Add(obj2);
                                            
                                        }
                                    }
                                }
                            }
                        }
                        int flag = _context.SaveChanges();
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
        public NAAC_AC_IPR_322_DTO deactive(NAAC_AC_IPR_322_DTO data)
        {
            try
            {
                var u = _context.NAAC_AC_IPR_322_DMO.Where(t => t.NCACIPR322_Id == data.NCACIPR322_Id).SingleOrDefault();
                if (u.NCACIPR322_ActiveFlg == true)
                {
                    u.NCACIPR322_ActiveFlg = false;
                }
                else if (u.NCACIPR322_ActiveFlg == false)
                {
                    u.NCACIPR322_ActiveFlg = true;
                }
                u.NCACIPR322_UpdatedDate = DateTime.Now;
                u.NCACIPR322_UpdatedBy = data.UserId;
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
        public NAAC_AC_IPR_322_DTO EditData(NAAC_AC_IPR_322_DTO data)
        {
            try
            {
                data.editlist = (from a in _context.Academic
                                 from b in _context.NAAC_AC_IPR_322_DMO
                                 where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCACIPR322_Year && b.MI_Id == data.MI_Id && b.NCACIPR322_Id == data.NCACIPR322_Id)
                                 select new NAAC_AC_IPR_322_DTO
                                 {
                                     NCACIPR322_Id = b.NCACIPR322_Id,

                                     NCACIPR322_EstablishmentDate = b.NCACIPR322_EstablishmentDate,
                                     NCACIPR322_Year = b.NCACIPR322_Year,
                                     NCACIPR322_WorkshopName = b.NCACIPR322_WorkshopName,
                                     NCACIPR322_FromDate = Convert.ToDateTime(b.NCACIPR322_FromDate),
                                     NCACIPR322_ToDate = Convert.ToDateTime(b.NCACIPR322_ToDate),
                                     NCACIPR322_WebisteLink = b.NCACIPR322_WebisteLink,
                                     NCACIPR322_StatusFlg = b.NCACIPR322_StatusFlg,
                                     MI_Id = b.MI_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCACIPR322_NoOfParticipants = b.NCACIPR322_NoOfParticipants,
                                     NCACIPR322_NameOfThePrincipalInvst = b.NCACIPR322_NameOfThePrincipalInvst,
                                     NCACIPR322_DeptOfPrincialInvst = b.NCACIPR322_DeptOfPrincialInvst,

                                 }).Distinct().ToArray();

                data.editFileslist = (from a in _context.NAAC_AC_IPR_322_Files_DMO
                                      where (a.NCACIPR322_Id == data.NCACIPR322_Id&&a.NCACIPR322F_ActiveFlg==true)
                                      select new NAAC_AC_IPR_322_DTO
                                      {
                                          cfilename = a.NCACIPR322F_FileName,
                                          cfilepath = a.NCACIPR322F_FilePath,
                                          cfiledesc = a.NCACIPR322F_Filedesc,
                                          NCACIPR322F_Id = a.NCACIPR322F_Id,
                                          NCACIPR322_Id = a.NCACIPR322_Id,
                                          NCACIPR322F_StatusFlg = a.NCACIPR322F_StatusFlg,
                                      }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAAC_AC_IPR_322_DTO viewuploadflies(NAAC_AC_IPR_322_DTO data)
        {
            try
            {
                data.viewuploadflies = (from a in _context.NAAC_AC_IPR_322_Files_DMO
                                        where (a.NCACIPR322_Id == data.NCACIPR322_Id&&a.NCACIPR322F_ActiveFlg==true)
                                        select new NAAC_AC_IPR_322_DTO
                                        {
                                            cfilename = a.NCACIPR322F_FileName,
                                            cfilepath = a.NCACIPR322F_FilePath,
                                            cfiledesc = a.NCACIPR322F_Filedesc,
                                            NCACIPR322F_Id = a.NCACIPR322F_Id,
                                            NCACIPR322_Id = a.NCACIPR322_Id,
                                            NCACIPR322F_StatusFlg = a.NCACIPR322F_StatusFlg,

                                        }).Distinct().ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;

        }
        public NAAC_AC_IPR_322_DTO deleteuploadfile(NAAC_AC_IPR_322_DTO data)
        {
            try
            {
                var res = _context.NAAC_AC_IPR_322_Files_DMO.Where(t => t.NCACIPR322F_Id == data.NCACIPR322F_Id).SingleOrDefault();
                res.NCACIPR322F_ActiveFlg = false;
                _context.Update(res);
                int s = _context.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                data.viewuploadflies = (from a in _context.NAAC_AC_IPR_322_Files_DMO
                                        where (a.NCACIPR322_Id == data.NCACIPR322_Id&&a.NCACIPR322F_ActiveFlg==true)
                                        select new NAAC_AC_IPR_322_DTO
                                        {
                                            cfilename = a.NCACIPR322F_FileName,
                                            cfilepath = a.NCACIPR322F_FilePath,
                                            cfiledesc = a.NCACIPR322F_Filedesc,
                                            NCACIPR322F_Id = a.NCACIPR322F_Id,
                                            NCACIPR322_Id = a.NCACIPR322_Id,
                                            NCACIPR322F_StatusFlg = a.NCACIPR322F_StatusFlg,

                                        }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_AC_IPR_322_DTO getcomment(NAAC_AC_IPR_322_DTO data)
        {
            try
            {
                data.commentlist = (from a in _context.NAAC_AC_IPR_322_Comments_DMO
                                    from b in _context.ApplUser
                                    where (a.NCACIPR322C_RemarksBy == b.Id && a.NCACIPR322_Id == data.NCACIPR322_Id)
                                    select new NAAC_AC_IPR_322_DTO
                                    {
                                        NCACIPR322C_Remarks = a.NCACIPR322C_Remarks,
                                        NCACIPR322C_Id = a.NCACIPR322C_Id,
                                        NCACIPR322C_RemarksBy = a.NCACIPR322C_RemarksBy,
                                        NCACIPR322C_StatusFlg = a.NCACIPR322C_StatusFlg,
                                        NCACIPR322C_ActiveFlag = a.NCACIPR322C_ActiveFlag,
                                        NCACIPR322C_CreatedBy = a.NCACIPR322C_CreatedBy,
                                        NCACIPR322C_CreatedDate = a.NCACIPR322C_CreatedDate,
                                        NCACIPR322C_UpdatedBy = a.NCACIPR322C_UpdatedBy,
                                        NCACIPR322C_UpdatedDate = a.NCACIPR322C_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCACIPR322C_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        // for file
        public NAAC_AC_IPR_322_DTO getfilecomment(NAAC_AC_IPR_322_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _context.NAAC_AC_IPR_322_File_Comments_DMO
                                     from b in _context.ApplUser
                                     where (a.NCACIPR322FC_RemarksBy == b.Id && a.NCACIPR322F_Id == data.NCACIPR322F_Id)
                                     select new NAAC_AC_IPR_322_DTO
                                     {
                                         NCACIPR322F_Id = a.NCACIPR322F_Id,
                                         NCACIPR322FC_Remarks = a.NCACIPR322FC_Remarks,
                                         NCACIPR322FC_Id = a.NCACIPR322FC_Id,
                                         NCACIPR322FC_RemarksBy = a.NCACIPR322FC_RemarksBy,
                                         NCACIPR322FC_StatusFlg = a.NCACIPR322FC_StatusFlg,
                                         NCACIPR322FC_ActiveFlag = a.NCACIPR322FC_ActiveFlag,
                                         NCACIPR322FC_CreatedBy = a.NCACIPR322FC_CreatedBy,
                                         NCACIPR322FC_CreatedDate = a.NCACIPR322FC_CreatedDate,
                                         NCACIPR322FC_UpdatedBy = a.NCACIPR322FC_UpdatedBy,
                                         NCACIPR322FC_UpdatedDate = a.NCACIPR322FC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCACIPR322FC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_IPR_322_DTO savemedicaldatawisecomments(NAAC_AC_IPR_322_DTO data)
        {
            try
            {
                NAAC_AC_IPR_322_Comments_DMO obj1 = new NAAC_AC_IPR_322_Comments_DMO();
                obj1.NCACIPR322C_Remarks = data.Remarks;
                obj1.NCACIPR322C_RemarksBy = data.UserId;
                obj1.NCACIPR322C_StatusFlg = "";
                obj1.NCACIPR322_Id = data.filefkid;
                obj1.NCACIPR322C_ActiveFlag = true;
                obj1.NCACIPR322C_CreatedBy = data.UserId;
                obj1.NCACIPR322C_UpdatedBy = data.UserId;
                obj1.NCACIPR322C_CreatedDate = DateTime.Now;
                obj1.NCACIPR322C_UpdatedDate = DateTime.Now;
                _context.Add(obj1);
                int s = _context.SaveChanges();
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
        public NAAC_AC_IPR_322_DTO savefilewisecomments(NAAC_AC_IPR_322_DTO data)
        {
            try
            {
                NAAC_AC_IPR_322_File_Comments_DMO obj1 = new NAAC_AC_IPR_322_File_Comments_DMO();
                obj1.NCACIPR322FC_Remarks = data.Remarks;
                obj1.NCACIPR322FC_RemarksBy = data.UserId;
                obj1.NCACIPR322FC_StatusFlg = "";
                obj1.NCACIPR322F_Id = data.filefkid;
                obj1.NCACIPR322FC_ActiveFlag = true;
                obj1.NCACIPR322FC_CreatedBy = data.UserId;
                obj1.NCACIPR322FC_UpdatedBy = data.UserId;
                obj1.NCACIPR322FC_UpdatedDate = DateTime.Now;
                obj1.NCACIPR322FC_CreatedDate = DateTime.Now;
                _context.Add(obj1);
                int s = _context.SaveChanges();
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
