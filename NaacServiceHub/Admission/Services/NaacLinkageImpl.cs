using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NaacLinkageImpl : Interface.NaacLinkageInterface
    {
        public GeneralContext _context;
        public NaacLinkageImpl(GeneralContext i)
        {
            _context = i;
        }
        public NAAC_AC_351_Linkage_DTO loaddata(NAAC_AC_351_Linkage_DTO data)
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

                data.yearlist = _context.Academic.Where(t => t.MI_Id == data.MI_Id).Distinct().ToArray();
                data.alldata1 = (from a in _context.Academic
                                 from b in _context.NAAC_AC_351_Linkage_DMO
                                 where (a.MI_Id == b.MI_Id && b.NCAC351LIN_CommYear == a.ASMAY_Id && a.Is_Active == true && a.MI_Id == data.MI_Id)
                                 select new NAAC_AC_351_Linkage_DTO
                                 {
                                     NCAC351LIN_From = Convert.ToDateTime(b.NCAC351LIN_From),
                                     NCAC351LIN_To = Convert.ToDateTime(b.NCAC351LIN_To),
                                     NCAC351LIN_CommYear = data.NCAC351LIN_CommYear,
                                     NCAC351LIN_LinkageTitle = b.NCAC351LIN_LinkageTitle,
                                     NCAC351LIN_PartnerName = b.NCAC351LIN_PartnerName,
                                     NCAC351LIN_LinkageNature = b.NCAC351LIN_LinkageNature,
                                     NCAC351LIN_LinkOfDocument = b.NCAC351LIN_LinkOfDocument,
                                     NCAC351LIN_Id = b.NCAC351LIN_Id,
                                     NCAC351LIN_ActiveFlg = b.NCAC351LIN_ActiveFlg,
                                     NCAC351LIN_StatusFlg = b.NCAC351LIN_StatusFlg,
                                     ASMAY_Year = a.ASMAY_Year,
                                     MI_Id=b.MI_Id
                                 }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_AC_351_Linkage_DTO save(NAAC_AC_351_Linkage_DTO data)
        {
            try
            {
                if (data.NCAC351LIN_Id == 0)
                {
                    var duplicate = _context.NAAC_AC_351_Linkage_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC351LIN_Id != 0 && t.NCAC351LIN_LinkageTitle == data.NCAC351LIN_LinkageTitle && t.NCAC351LIN_PartnerName == data.NCAC351LIN_PartnerName && t.NCAC351LIN_LinkageNature == data.NCAC351LIN_LinkageNature && t.NCAC351LIN_LinkOfDocument == data.NCAC351LIN_LinkOfDocument).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_351_Linkage_DMO rrr = new NAAC_AC_351_Linkage_DMO();
                        rrr.MI_Id = data.MI_Id;
                        rrr.NCAC351LIN_From = data.NCAC351LIN_From;
                        rrr.NCAC351LIN_To = data.NCAC351LIN_To;
                        rrr.NCAC351LIN_CommYear = data.NCAC351LIN_CommYear;
                        rrr.NCAC351LIN_LinkageTitle = data.NCAC351LIN_LinkageTitle;
                        rrr.NCAC351LIN_PartnerName = data.NCAC351LIN_PartnerName;
                        rrr.NCAC351LIN_LinkageNature = data.NCAC351LIN_LinkageNature;
                        rrr.NCAC351LIN_LinkOfDocument = data.NCAC351LIN_LinkOfDocument;
                        rrr.NCAC351LIN_CreatedDate = DateTime.Now;
                        rrr.NCAC351LIN_UpdatedDate = DateTime.Now;
                        rrr.NCAC351LIN_ActiveFlg = true;
                        rrr.NCAC351LIN_StatusFlg = "";
                        rrr.NCAC351LIN_CreatedBy = data.UserId;
                        rrr.NCAC351LIN_UpdatedBy = data.UserId;
                        _context.Add(rrr);
                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[i].cfilepath != null)
                                {

                              
                                NAAC_AC_351_Linkage_Files_DMO obj2 = new NAAC_AC_351_Linkage_Files_DMO();
                                obj2.NCAC351LIN_Id = rrr.NCAC351LIN_Id;
                                obj2.NCAC351LINF_FileName = data.filelist[i].cfilename;
                                obj2.NCAC351LINF_Filedesc = data.filelist[i].cfiledesc;
                                obj2.NCAC351LINF_FilePath = data.filelist[i].cfilepath;

                                    obj2.NCAC351LINF_StatusFlg = "";
                                    obj2.NCAC351LINF_ActiveFlg = true;
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
                else if (data.NCAC351LIN_Id > 0)
                {
                    var duplicate = _context.NAAC_AC_351_Linkage_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC351LIN_LinkageTitle == data.NCAC351LIN_LinkageTitle && t.NCAC351LIN_PartnerName == data.NCAC351LIN_PartnerName && t.NCAC351LIN_LinkageNature == data.NCAC351LIN_LinkageNature && t.NCAC351LIN_LinkOfDocument == data.NCAC351LIN_LinkOfDocument && t.NCAC351LIN_CommYear == data.NCAC351LIN_CommYear && t.NCAC351LIN_Id != data.NCAC351LIN_Id).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var yy = _context.NAAC_AC_351_Linkage_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC351LIN_Id == data.NCAC351LIN_Id).SingleOrDefault();
                        yy.NCAC351LIN_From = data.NCAC351LIN_From;
                        yy.NCAC351LIN_To = data.NCAC351LIN_To;
                        yy.NCAC351LIN_CommYear = data.NCAC351LIN_CommYear;
                        yy.NCAC351LIN_LinkageTitle = data.NCAC351LIN_LinkageTitle;
                        yy.NCAC351LIN_PartnerName = data.NCAC351LIN_PartnerName;
                        yy.NCAC351LIN_LinkageNature = data.NCAC351LIN_LinkageNature;
                        yy.NCAC351LIN_LinkOfDocument = data.NCAC351LIN_LinkOfDocument;
                        yy.NCAC351LIN_UpdatedDate = DateTime.Now;
                        yy.NCAC351LIN_UpdatedBy = data.UserId;
                        yy.MI_Id = data.MI_Id;
                        _context.Update(yy);
                       
                        if (data.filelist.Length > 0)
                        {


                            List<long> Fid = new List<long>();
                            foreach (var item in data.filelist)
                            {
                                Fid.Add(item.NCAC351LINF_Id);
                            }
                            var removefile11 = _context.NAAC_AC_351_Linkage_Files_DMO.Where(t => t.NCAC351LIN_Id == data.NCAC351LIN_Id && !Fid.Contains(t.NCAC351LINF_Id)).Distinct().ToList();
                            if (removefile11.Count > 0)
                            {
                                foreach (var item2 in removefile11)
                                {
                                    var deactfile = _context.NAAC_AC_351_Linkage_Files_DMO.Single(t => t.NCAC351LIN_Id == data.NCAC351LIN_Id && t.NCAC351LINF_Id == item2.NCAC351LINF_Id);
                                    deactfile.NCAC351LINF_ActiveFlg = false;
                                    _context.Update(deactfile);
                                }
                            }

                            //var CountRemoveFiles = _context.NAAC_AC_351_Linkage_Files_DMO.Where(t => t.NCAC351LIN_Id == data.NCAC351LIN_Id&&t.NCAC351LINF_StatusFlg!="approved").ToList();
                            //if (CountRemoveFiles.Count > 0)
                            //{
                            //    foreach (var RemoveFiles in CountRemoveFiles)
                            //    {
                            //        _context.Remove(RemoveFiles);
                            //    }
                            //}


                            //for(int ii = 0; ii < data.remv2.Length; ii++)
                            //{
                            //  var  count = 0;
                            //    var fileli = _context.NAAC_AC_351_Linkage_Files_DMO.Where(t => t.NCAC351LINF_Id == data.remv2[ii].NCAC351LINF_Id).SingleOrDefault();

                            //    fileli.NCAC351LINF_ActiveFlg = false;
                            //    _context.Update(fileli);
                            //  int gg=  _context.SaveChanges();
                            //    if (gg > 0)
                            //    {
                            //        count =count +1;
                            //    }
                            //    else
                            //    {

                            //    }
                            //}



                            //for(int tt=0;tt<data.filelist.Length;tt++)
                            //{
                            //    var fileli = _context.NAAC_AC_351_Linkage_Files_DMO.Where(t => t.NCAC351LIN_Id == data.NCAC351LIN_Id).ToList();                               
                            //    for(int hjb = 0; hjb < fileli.Count; hjb++)
                            //    {
                            //        var hb = _context.NAAC_AC_351_Linkage_Files_DMO.Where(t => t.NCAC351LINF_Id != fileli[hjb].NCAC351LINF_Id);
                            //    }
                            //    var remvfileww = _context.NAAC_AC_351_Linkage_Files_DMO.Where(t => t.NCAC351LINF_Id == data.filelist[tt].NCAC351LINF_Id).SingleOrDefault();
                            //    if (remvfileww != null)
                            //    {
                            //        if (remvfileww.NCAC351LINF_Id != 0 && remvfileww.NCAC351LINF_Id > 0 && remvfileww.NCAC351LINF_Id != data.filelist[tt].NCAC351LINF_Id)
                            //        {
                            //            remvfileww.NCAC351LINF_ActiveFlg = false;
                            //            _context.Update(remvfileww);
                            //            int jhj = _context.SaveChanges();
                            //            if (jhj > 0)
                            //            {

                            //            }
                            //            else
                            //            {

                            //            }
                            //        }
                            //    }


                            //}











                            //foreach (NAAC_AC_351_Linkage_DTO DocumentsDTO1 in data.filelist)
                            //{


                            //    if(DocumentsDTO1.NCAC351LINF_Id==)


                            //}
                            //foreach (NAAC_AC_351_Linkage_DTO DocumentsDTO1 in data.filelist)
                            //{
                            //    var remvfile = _context.NAAC_AC_351_Linkage_Files_DMO.Where(t => t.NCAC351LINF_Id != DocumentsDTO1.NCAC351LINF_Id).SingleOrDefault();
                            //    remvfile.NCAC351LINF_ActiveFlg = false;
                            //    _context.Update(remvfile);
                            //    int jhj = _context.SaveChanges();
                            //    if (jhj > 0)
                            //    {

                            //    }
                            //    else
                            //    {

                            //    }
                            //}





                            foreach (NAAC_AC_351_Linkage_DTO DocumentsDTO in data.filelist)
                            {
                                if (DocumentsDTO.NCAC351LINF_Id > 0 && DocumentsDTO.NCAC351LINF_StatusFlg != "approved")
                                {
                                    if (DocumentsDTO.cfilepath != null)
                                    {

                                        var filesdata = _context.NAAC_AC_351_Linkage_Files_DMO.Where(t => t.NCAC351LINF_Id == DocumentsDTO.NCAC351LINF_Id).FirstOrDefault();
                                        filesdata.NCAC351LINF_Filedesc = DocumentsDTO.cfiledesc;
                                        filesdata.NCAC351LINF_FileName = DocumentsDTO.cfilename;
                                        filesdata.NCAC351LINF_FilePath = DocumentsDTO.cfilepath;


                                        _context.Update(filesdata);
                                       
                                    }
                                }
                                else
                                {

                                    if (DocumentsDTO.NCAC351LINF_Id == 0)
                                    {
                                        if (DocumentsDTO.cfilepath != null)
                                        {
                                            NAAC_AC_351_Linkage_Files_DMO obj2 = new NAAC_AC_351_Linkage_Files_DMO();
                                            obj2.NCAC351LINF_FileName = DocumentsDTO.cfilename;
                                            obj2.NCAC351LINF_Filedesc = DocumentsDTO.cfiledesc;
                                            obj2.NCAC351LINF_FilePath = DocumentsDTO.cfilepath;
                                            obj2.NCAC351LINF_StatusFlg = "";
                                            obj2.NCAC351LINF_ActiveFlg = true;
                                            obj2.NCAC351LIN_Id = data.NCAC351LIN_Id;
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
        public NAAC_AC_351_Linkage_DTO EditData(NAAC_AC_351_Linkage_DTO data)
        {
            try
            {
                data.editlist = (from a in _context.Academic
                                 from b in _context.NAAC_AC_351_Linkage_DMO
                                 where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCAC351LIN_CommYear && b.MI_Id == data.MI_Id && b.NCAC351LIN_Id == data.NCAC351LIN_Id)
                                 select new NAAC_AC_351_Linkage_DTO
                                 {
                                     NCAC351LIN_Id = b.NCAC351LIN_Id,
                                     NCAC351LIN_CommYear = b.NCAC351LIN_CommYear,
                                     NCAC351LIN_From = Convert.ToDateTime(b.NCAC351LIN_From),
                                     NCAC351LIN_To = Convert.ToDateTime(b.NCAC351LIN_To),
                                     NCAC351LIN_LinkageTitle = b.NCAC351LIN_LinkageTitle,
                                     NCAC351LIN_PartnerName = b.NCAC351LIN_PartnerName,
                                     NCAC351LIN_LinkageNature = b.NCAC351LIN_LinkageNature,
                                     NCAC351LIN_StatusFlg = b.NCAC351LIN_StatusFlg,
                                     NCAC351LIN_LinkOfDocument = b.NCAC351LIN_LinkOfDocument,
                                     ASMAY_Year = a.ASMAY_Year,
                                     MI_Id=b.MI_Id
                                 }).Distinct().ToArray();
                data.editFileslist = (from b in _context.NAAC_AC_351_Linkage_DMO
                    from a in _context.NAAC_AC_351_Linkage_Files_DMO
                                      where (b.NCAC351LIN_Id==a.NCAC351LIN_Id&&a.NCAC351LIN_Id == data.NCAC351LIN_Id&&a.NCAC351LINF_ActiveFlg==true)
                                      select new NAAC_AC_351_Linkage_DTO
                                      {
                                          cfilename = a.NCAC351LINF_FileName,
                                          cfilepath = a.NCAC351LINF_FilePath,
                                          cfiledesc = a.NCAC351LINF_Filedesc,
                                          NCAC351LINF_StatusFlg = a.NCAC351LINF_StatusFlg,
                                          NCAC351LINF_Id = a.NCAC351LINF_Id,
                                          NCAC351LIN_Id = a.NCAC351LIN_Id,
                                      }).Distinct().ToArray();

                data.orig = data.editFileslist;
            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAAC_AC_351_Linkage_DTO deactive(NAAC_AC_351_Linkage_DTO data)
        {
            try
            {
                var u = _context.NAAC_AC_351_Linkage_DMO.Where(t => t.NCAC351LIN_Id == data.NCAC351LIN_Id).SingleOrDefault();
                if (u.NCAC351LIN_ActiveFlg == true)
                {
                    u.NCAC351LIN_ActiveFlg = false;
                }
                else if (u.NCAC351LIN_ActiveFlg == false)
                {
                    u.NCAC351LIN_ActiveFlg = true;
                }
                u.NCAC351LIN_UpdatedDate = DateTime.Now;
                u.NCAC351LIN_UpdatedBy = data.UserId;
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
        public NAAC_AC_351_Linkage_DTO viewuploadflies(NAAC_AC_351_Linkage_DTO data)
        {
            try
            {
                data.viewuploadflies = (from a in _context.NAAC_AC_351_Linkage_Files_DMO
                                        where (a.NCAC351LIN_Id == data.NCAC351LIN_Id&&a.NCAC351LINF_ActiveFlg==true)
                                        select new NAAC_AC_351_Linkage_DTO
                                        {
                                            cfilename = a.NCAC351LINF_FileName,
                                            cfilepath = a.NCAC351LINF_FilePath,
                                            cfiledesc = a.NCAC351LINF_Filedesc,
                                            NCAC351LINF_Id = a.NCAC351LINF_Id,
                                            NCAC351LIN_Id = a.NCAC351LIN_Id,
                                            NCAC351LINF_StatusFlg = a.NCAC351LINF_StatusFlg,
                                        }).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_AC_351_Linkage_DTO deleteuploadfile(NAAC_AC_351_Linkage_DTO data)
        {
            try
            {
                var res = _context.NAAC_AC_351_Linkage_Files_DMO.Where(t => t.NCAC351LINF_Id == data.NCAC351LINF_Id).SingleOrDefault();
                res.NCAC351LINF_ActiveFlg = false;  
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
                data.viewuploadflies = (from a in _context.NAAC_AC_351_Linkage_Files_DMO
                                        where (a.NCAC351LIN_Id == data.NCAC351LIN_Id&&a.NCAC351LINF_ActiveFlg==true)
                                        select new NAAC_AC_351_Linkage_DTO
                                        {
                                            cfilename = a.NCAC351LINF_FileName,
                                            cfilepath = a.NCAC351LINF_FilePath,
                                            cfiledesc = a.NCAC351LINF_Filedesc,
                                            NCAC351LINF_Id = a.NCAC351LINF_Id,
                                            NCAC351LIN_Id = a.NCAC351LIN_Id,
                                            NCAC351LINF_StatusFlg = a.NCAC351LINF_StatusFlg,
                                        }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public NAAC_AC_351_Linkage_DTO getcomment(NAAC_AC_351_Linkage_DTO data)
        {
            try
            {
                data.commentlist = (from a in _context.NAAC_AC_351_Linkage_Comments_DMO
                                    from b in _context.ApplUser
                                    where (a.NCAC351LINC_RemarksBy == b.Id && a.NCAC351LIN_Id == data.NCAC351LIN_Id)
                                    select new NAAC_AC_351_Linkage_DTO
                                    {
                                        NCAC351LINC_Remarks = a.NCAC351LINC_Remarks,
                                        NCAC351LINC_Id = a.NCAC351LINC_Id,
                                        NCAC351LINC_RemarksBy = a.NCAC351LINC_RemarksBy,
                                        NCAC351LINC_StatusFlg = a.NCAC351LINC_StatusFlg,
                                        NCAC351LINC_ActiveFlag = a.NCAC351LINC_ActiveFlag,
                                        NCAC351LINC_CreatedBy = a.NCAC351LINC_CreatedBy,
                                        NCAC351LINC_CreatedDate = a.NCAC351LINC_CreatedDate,
                                        NCAC351LINC_UpdatedBy = a.NCAC351LINC_UpdatedBy,
                                        NCAC351LINC_UpdatedDate = a.NCAC351LINC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCAC351LINC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        // for file
        public NAAC_AC_351_Linkage_DTO getfilecomment(NAAC_AC_351_Linkage_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _context.NAAC_AC_351_Linkage_File_Comments_DMO
                                     from b in _context.ApplUser
                                     where (a.NCAC351LINFC_RemarksBy == b.Id && a.NCAC351LINF_Id == data.NCAC351LINF_Id)
                                     select new NAAC_AC_351_Linkage_DTO
                                     {
                                         NCAC351LINF_Id = a.NCAC351LINF_Id,
                                         NCAC351LINFC_Remarks = a.NCAC351LINFC_Remarks,
                                         NCAC351LINFC_Id = a.NCAC351LINFC_Id,
                                         NCAC351LINFC_RemarksBy = a.NCAC351LINFC_RemarksBy,
                                         NCAC351LINFC_StatusFlg = a.NCAC351LINFC_StatusFlg,
                                         NCAC351LINFC_ActiveFlag = a.NCAC351LINFC_ActiveFlag,
                                         NCAC351LINFC_CreatedBy = a.NCAC351LINFC_CreatedBy,
                                         NCAC351LINFC_CreatedDate = a.NCAC351LINFC_CreatedDate,
                                         NCAC351LINFC_UpdatedBy = a.NCAC351LINFC_UpdatedBy,
                                         NCAC351LINFC_UpdatedDate = a.NCAC351LINFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCAC351LINFC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_351_Linkage_DTO savemedicaldatawisecomments(NAAC_AC_351_Linkage_DTO data)
        {
            try
            {
                NAAC_AC_351_Linkage_Comments_DMO obj1 = new NAAC_AC_351_Linkage_Comments_DMO();
                obj1.NCAC351LINC_Remarks = data.Remarks;
                obj1.NCAC351LINC_RemarksBy = data.UserId;
                obj1.NCAC351LINC_StatusFlg = "";
                obj1.NCAC351LIN_Id = data.filefkid;
                obj1.NCAC351LINC_ActiveFlag = true;
                obj1.NCAC351LINC_CreatedBy = data.UserId;
                obj1.NCAC351LINC_UpdatedBy = data.UserId;
                obj1.NCAC351LINC_CreatedDate = DateTime.Now;
                obj1.NCAC351LINC_UpdatedDate = DateTime.Now;
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
        public NAAC_AC_351_Linkage_DTO savefilewisecomments(NAAC_AC_351_Linkage_DTO data)
        {
            try
            {
                NAAC_AC_351_Linkage_File_Comments_DMO obj1 = new NAAC_AC_351_Linkage_File_Comments_DMO();
                obj1.NCAC351LINFC_Remarks = data.Remarks;
                obj1.NCAC351LINFC_RemarksBy = data.UserId;
                obj1.NCAC351LINFC_StatusFlg = "";
                obj1.NCAC351LINF_Id = data.filefkid;
                obj1.NCAC351LINFC_ActiveFlag = true;
                obj1.NCAC351LINFC_CreatedBy = data.UserId;
                obj1.NCAC351LINFC_UpdatedBy = data.UserId;
                obj1.NCAC351LINFC_UpdatedDate = DateTime.Now;
                obj1.NCAC351LINFC_CreatedDate = DateTime.Now;
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
