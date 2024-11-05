using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class Naac_MOUImpl : Interface.Naac_MOUInterface
    {
        public GeneralContext _context;
        public Naac_MOUImpl(GeneralContext w)
        {
            _context = w;
        }
        public Naac_MOU_DTO loaddata(Naac_MOU_DTO data)
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

                data.alldatalist = (from a in _context.Academic
                                    from b in _context.Naac_MOU_DMO
                                    where (a.MI_Id == b.MI_Id && b.NCAC352MOU_SigningYear == a.ASMAY_Id && a.Is_Active == true && a.MI_Id == data.MI_Id)
                                    select new Naac_MOU_DTO
                                    {
                                        NCAC352MOU_Id = b.NCAC352MOU_Id,
                                        NCAC352MOU_Name = b.NCAC352MOU_Name,
                                        NCAC352MOU_SigningYear = b.NCAC352MOU_SigningYear,
                                        NCAC352MOU_NoOfStudents = b.NCAC352MOU_NoOfStudents,
                                        NCAC352MOU_NoOfStaff = b.NCAC352MOU_NoOfStaff,
                                        NCAC352MOU_ActiveFlg = b.NCAC352MOU_ActiveFlg,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCAC352MOU_OrganisationName = b.NCAC352MOU_OrganisationName,
                                        NCAC352MOU_Duration = b.NCAC352MOU_Duration,
                                        NCAC352MOU_ActivitiesList = b.NCAC352MOU_ActivitiesList,
                                        NCAC352MOU_LinkOfDocument = b.NCAC352MOU_LinkOfDocument,
                                        NCAC352MOU_StatusFlg = b.NCAC352MOU_StatusFlg,
                                        MI_Id=b.MI_Id
                                    }).Distinct().OrderByDescending(t => t.NCAC352MOU_Id).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }        
        public Naac_MOU_DTO save(Naac_MOU_DTO data)
        {
            try
            {
                if (data.NCAC352MOU_Id == 0)
                {
                    var duplicate = _context.Naac_MOU_DMO.Where(t =>t.MI_Id == data.MI_Id && t.NCAC352MOU_Name == data.NCAC352MOU_Name && t.NCAC352MOU_SigningYear == data.NCAC352MOU_SigningYear && t.NCAC352MOU_NoOfStaff == data.NCAC352MOU_NoOfStaff && t.NCAC352MOU_NoOfStudents == data.NCAC352MOU_NoOfStudents).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        Naac_MOU_DMO obj1 = new Naac_MOU_DMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC352MOU_SigningYear = data.NCAC352MOU_SigningYear;
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC352MOU_OrganisationName = data.NCAC352MOU_OrganisationName;
                        obj1.NCAC352MOU_Name = data.NCAC352MOU_Name;
                        obj1.NCAC352MOU_SigningYear = data.NCAC352MOU_SigningYear;
                        obj1.NCAC352MOU_Duration = data.NCAC352MOU_Duration;
                        obj1.NCAC352MOU_ActivitiesList = data.NCAC352MOU_ActivitiesList;
                        obj1.NCAC352MOU_NoOfStudents = data.NCAC352MOU_NoOfStudents;
                        obj1.NCAC352MOU_NoOfStaff = data.NCAC352MOU_NoOfStaff;
                        obj1.NCAC352MOU_LinkOfDocument = data.NCAC352MOU_LinkOfDocument;
                        obj1.NCAC352MOU_StatusFlg = "";
                        obj1.NCAC352MOU_CreatedDate = DateTime.Now;
                        obj1.NCAC352MOU_UpdatedDate = DateTime.Now;
                        obj1.NCAC352MOU_ActiveFlg = true;
                        obj1.NCAC352MOU_CreatedBy = data.UserId;
                        obj1.NCAC352MOU_UpdatedBy = data.UserId;
                        _context.Add(obj1);

                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[i].cfilepath!= null)
                                {
                                    NAAC_AC_352_MOU_Files_DMO obj2 = new NAAC_AC_352_MOU_Files_DMO();

                                    obj2.NCAC352MOU_Id = obj1.NCAC352MOU_Id;
                                    obj2.NCAC352MOUF_StatusFlg = "";
                                    obj2.NCAC352MOUF_ActiveFlg = true;
                                    obj2.NCAC352MOUF_FileName = data.filelist[i].cfilename;
                                    obj2.NCAC352MOUF_Filedesc = data.filelist[i].cfiledesc;
                                    obj2.NCAC352MOUF_FilePath = data.filelist[i].cfilepath;
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
                else if (data.NCAC352MOU_Id > 0)
                {
                    var duplicate = _context.Naac_MOU_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC352MOU_Name == data.NCAC352MOU_Name && t.NCAC352MOU_SigningYear == data.NCAC352MOU_SigningYear && t.NCAC352MOU_NoOfStaff == data.NCAC352MOU_NoOfStaff && t.NCAC352MOU_NoOfStudents == data.NCAC352MOU_NoOfStudents && t.NCAC352MOU_Id != data.NCAC352MOU_Id).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _context.Naac_MOU_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC352MOU_Id == data.NCAC352MOU_Id).SingleOrDefault();
                        update.NCAC352MOU_CreatedBy = data.UserId;
                        update.NCAC352MOU_OrganisationName = data.NCAC352MOU_OrganisationName;
                        update.NCAC352MOU_Name = data.NCAC352MOU_Name;
                        update.NCAC352MOU_SigningYear = data.NCAC352MOU_SigningYear;
                        update.NCAC352MOU_Duration = data.NCAC352MOU_Duration;
                        update.NCAC352MOU_ActivitiesList = data.NCAC352MOU_ActivitiesList;
                        update.NCAC352MOU_NoOfStudents = data.NCAC352MOU_NoOfStudents;
                        update.NCAC352MOU_NoOfStaff = data.NCAC352MOU_NoOfStaff;                       
                                          
                        update.NCAC352MOU_UpdatedDate = DateTime.Now;
                        update.MI_Id = data.MI_Id;
                        _context.Update(update);
                       
                        if (data.filelist.Length > 0)
                        {
                            List<long> Fid = new List<long>();
                            foreach (var item in data.filelist)
                            {
                                Fid.Add(item.NCAC352MOUF_Id);
                            }
                            var removefile11 = _context.NAAC_AC_352_MOU_Files_DMO.Where(t => t.NCAC352MOU_Id == data.NCAC352MOU_Id && !Fid.Contains(t.NCAC352MOUF_Id)).Distinct().ToList();

                            if (removefile11.Count > 0)
                            {
                                foreach (var item2 in removefile11)
                                {
                                    var deactfile = _context.NAAC_AC_352_MOU_Files_DMO.Single(t => t.NCAC352MOU_Id == data.NCAC352MOU_Id && t.NCAC352MOUF_Id == item2.NCAC352MOUF_Id);
                                    deactfile.NCAC352MOUF_ActiveFlg = false;
                                    _context.Update(deactfile);

                                }

                            }



                            //var CountRemoveFiles = _context.NAAC_AC_352_MOU_Files_DMO.Where(t => t.NCAC352MOU_Id == data.NCAC352MOU_Id&&t.NCAC352MOUF_StatusFlg!="approved").ToList();
                            //if (CountRemoveFiles.Count > 0)
                            //{
                            //    foreach (var RemoveFiles in CountRemoveFiles)
                            //    {
                            //        _context.Remove(RemoveFiles);
                            //    }
                            //}
                            foreach (Naac_MOU_DTO DocumentsDTO in data.filelist)
                            {

                                if (DocumentsDTO.NCAC352MOUF_Id > 0 && DocumentsDTO.NCAC352MOUF_StatusFlg != "approved")
                                {
                                    if (DocumentsDTO.cfilepath != null)
                                    {

                                        var filesdata = _context.NAAC_AC_352_MOU_Files_DMO.Where(t => t.NCAC352MOUF_Id == DocumentsDTO.NCAC352MOUF_Id).FirstOrDefault();
                                        filesdata.NCAC352MOUF_Filedesc = DocumentsDTO.cfiledesc;
                                        filesdata.NCAC352MOUF_FileName = DocumentsDTO.cfilename;
                                        filesdata.NCAC352MOUF_FilePath = DocumentsDTO.cfilepath;


                                        _context.Update(filesdata);
                                        
                                        
                                    }
                                }
                                else
                                {

                                    if (DocumentsDTO.NCAC352MOUF_Id == 0)
                                    {
                                        if (DocumentsDTO.cfilepath != null)
                                        {
                                            NAAC_AC_352_MOU_Files_DMO obj2 = new NAAC_AC_352_MOU_Files_DMO();
                                            obj2.NCAC352MOUF_FileName = DocumentsDTO.cfilename;
                                            obj2.NCAC352MOUF_Filedesc = DocumentsDTO.cfiledesc;
                                            obj2.NCAC352MOUF_FilePath = DocumentsDTO.cfilepath;
                                            obj2.NCAC352MOUF_StatusFlg = "";
                                            obj2.NCAC352MOUF_ActiveFlg = true;
                                            obj2.NCAC352MOU_Id = data.NCAC352MOU_Id;
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
        public Naac_MOU_DTO deactiveStudent(Naac_MOU_DTO data)
        {
            try
            {
                var u = _context.Naac_MOU_DMO.Where(t => t.NCAC352MOU_Id == data.NCAC352MOU_Id).SingleOrDefault();
                if (data.NCAC352MOU_ActiveFlg == true)
                {
                    u.NCAC352MOU_ActiveFlg = false;
                }
                else if (u.NCAC352MOU_ActiveFlg == false)
                {
                    u.NCAC352MOU_ActiveFlg = true;
                }
                u.NCAC352MOU_UpdatedDate = DateTime.Now;
                u.NCAC352MOU_UpdatedBy = data.UserId;
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
        public Naac_MOU_DTO EditData(Naac_MOU_DTO data)
        {
            try
            {
                data.editlist = (from a in _context.Academic
                                 from b in _context.Naac_MOU_DMO
                                 where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCAC352MOU_SigningYear && b.MI_Id == data.MI_Id && b.NCAC352MOU_Id == data.NCAC352MOU_Id)
                                 select new Naac_MOU_DTO
                                 {
                                     NCAC352MOU_Id = b.NCAC352MOU_Id,
                                     NCAC352MOU_Name = b.NCAC352MOU_Name,
                                     NCAC352MOU_SigningYear = b.NCAC352MOU_SigningYear,
                                     NCAC352MOU_NoOfStudents = b.NCAC352MOU_NoOfStudents,
                                     NCAC352MOU_NoOfStaff = b.NCAC352MOU_NoOfStaff,                                   
                                     NCAC352MOU_ActiveFlg = b.NCAC352MOU_ActiveFlg,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCAC352MOU_OrganisationName = b.NCAC352MOU_OrganisationName,
                                     NCAC352MOU_Duration = b.NCAC352MOU_Duration,
                                     NCAC352MOU_ActivitiesList = b.NCAC352MOU_ActivitiesList,
                                     NCAC352MOU_LinkOfDocument = b.NCAC352MOU_LinkOfDocument,
                                     NCAC352MOU_StatusFlg = b.NCAC352MOU_StatusFlg,
                                     MI_Id=b.MI_Id
                                 }).Distinct().ToArray();

                data.editFileslist = (from a in _context.NAAC_AC_352_MOU_Files_DMO
                                      where (a.NCAC352MOU_Id == data.NCAC352MOU_Id&&a.NCAC352MOUF_ActiveFlg==true)
                                      select new Naac_MOU_DTO
                                      {
                                          cfilename = a.NCAC352MOUF_FileName,
                                          cfilepath = a.NCAC352MOUF_FilePath,
                                          cfiledesc = a.NCAC352MOUF_Filedesc,
                                          NCAC352MOUF_StatusFlg = a.NCAC352MOUF_StatusFlg,
                                          NCAC352MOUF_Id = a.NCAC352MOUF_Id,
                                      }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public Naac_MOU_DTO viewuploadflies(Naac_MOU_DTO data)
        {
            try
            {
                data.viewuploadflies = (from a in _context.NAAC_AC_352_MOU_Files_DMO
                                        where (a.NCAC352MOU_Id == data.NCAC352MOU_Id&&a.NCAC352MOUF_ActiveFlg==true)
                                        select new Naac_MOU_DTO
                                        {
                                            cfilename = a.NCAC352MOUF_FileName,
                                            cfilepath = a.NCAC352MOUF_FilePath,
                                            cfiledesc = a.NCAC352MOUF_Filedesc,
                                            NCAC352MOUF_Id = a.NCAC352MOUF_Id,
                                            NCAC352MOU_Id = a.NCAC352MOU_Id,
                                            NCAC352MOUF_StatusFlg = a.NCAC352MOUF_StatusFlg,
                                        }).Distinct().ToArray();
                  
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;

        }
        public Naac_MOU_DTO deleteuploadfile(Naac_MOU_DTO data)
        {
            try
            {
                var res = _context.NAAC_AC_352_MOU_Files_DMO.Where(t => t.NCAC352MOUF_Id == data.NCAC352MOUF_Id).SingleOrDefault();
                res.NCAC352MOUF_ActiveFlg = false;
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
                data.viewuploadflies = (from a in _context.NAAC_AC_352_MOU_Files_DMO
                                        where (a.NCAC352MOU_Id == data.NCAC352MOU_Id&&a.NCAC352MOUF_ActiveFlg==true)
                                        select new Naac_MOU_DTO
                                        {
                                            cfilename = a.NCAC352MOUF_FileName,
                                            cfilepath = a.NCAC352MOUF_FilePath,
                                            cfiledesc = a.NCAC352MOUF_Filedesc,
                                            NCAC352MOUF_Id = a.NCAC352MOUF_Id,
                                            NCAC352MOU_Id = a.NCAC352MOU_Id,
                                            NCAC352MOUF_StatusFlg = a.NCAC352MOUF_StatusFlg,
                                        }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public Naac_MOU_DTO getcomment(Naac_MOU_DTO data)
        {
            try
            {
                data.commentlist = (from a in _context.NAAC_AC_352_MOU_Comments_DMO
                                    from b in _context.ApplUser
                                    where (a.NCAC352MOUC_RemarksBy == b.Id && a.NCAC352MOU_Id == data.NCAC352MOU_Id)
                                    select new Naac_MOU_DTO
                                    {
                                        NCAC352MOUC_Remarks = a.NCAC352MOUC_Remarks,
                                        NCAC352MOUC_Id = a.NCAC352MOUC_Id,
                                        NCAC352MOUC_RemarksBy = a.NCAC352MOUC_RemarksBy,
                                        NCAC352MOUC_StatusFlg = a.NCAC352MOUC_StatusFlg,
                                        NCAC352MOUC_ActiveFlag = a.NCAC352MOUC_ActiveFlag,
                                        NCAC352MOUC_CreatedBy = a.NCAC352MOUC_CreatedBy,
                                        NCAC352MOUC_CreatedDate = a.NCAC352MOUC_CreatedDate,
                                        NCAC352MOUC_UpdatedBy = a.NCAC352MOUC_UpdatedBy,
                                        NCAC352MOUC_UpdatedDate = a.NCAC352MOUC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCAC352MOUC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        // for file
        public Naac_MOU_DTO getfilecomment(Naac_MOU_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _context.NAAC_AC_352_MOU_File_Comments_DMO
                                     from b in _context.ApplUser
                                     where (a.NCAC352MOUFC_RemarksBy == b.Id && a.NCAC352MOUF_Id == data.NCAC352MOUF_Id)
                                     select new Naac_MOU_DTO
                                     {
                                         NCAC352MOUF_Id = a.NCAC352MOUF_Id,
                                         NCAC352MOUFC_Remarks = a.NCAC352MOUFC_Remarks,
                                         NCAC352MOUFC_Id = a.NCAC352MOUFC_Id,
                                         NCAC352MOUFC_RemarksBy = a.NCAC352MOUFC_RemarksBy,
                                         NCAC352MOUFC_StatusFlg = a.NCAC352MOUFC_StatusFlg,
                                         NCAC352MOUFC_ActiveFlag = a.NCAC352MOUFC_ActiveFlag,
                                         NCAC352MOUFC_CreatedBy = a.NCAC352MOUFC_CreatedBy,
                                         NCAC352MOUFC_CreatedDate = a.NCAC352MOUFC_CreatedDate,
                                         NCAC352MOUFC_UpdatedBy = a.NCAC352MOUFC_UpdatedBy,
                                         NCAC352MOUFC_UpdatedDate = a.NCAC352MOUFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCAC352MOUFC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Naac_MOU_DTO savemedicaldatawisecomments(Naac_MOU_DTO data)
        {
            try
            {
                NAAC_AC_352_MOU_Comments_DMO obj1 = new NAAC_AC_352_MOU_Comments_DMO();
                obj1.NCAC352MOUC_Remarks = data.Remarks;
                obj1.NCAC352MOUC_RemarksBy = data.UserId;
                obj1.NCAC352MOUC_StatusFlg = "";
                obj1.NCAC352MOU_Id = data.filefkid;
                obj1.NCAC352MOUC_ActiveFlag = true;
                obj1.NCAC352MOUC_CreatedBy = data.UserId;
                obj1.NCAC352MOUC_UpdatedBy = data.UserId;
                obj1.NCAC352MOUC_CreatedDate = DateTime.Now;
                obj1.NCAC352MOUC_UpdatedDate = DateTime.Now;
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
        public Naac_MOU_DTO savefilewisecomments(Naac_MOU_DTO data)
        {
            try
            {
                NAAC_AC_352_MOU_File_Comments_DMO obj1 = new NAAC_AC_352_MOU_File_Comments_DMO();
                obj1.NCAC352MOUFC_Remarks = data.Remarks;
                obj1.NCAC352MOUFC_RemarksBy = data.UserId;
                obj1.NCAC352MOUFC_StatusFlg = "";
                obj1.NCAC352MOUF_Id = data.filefkid;
                obj1.NCAC352MOUFC_ActiveFlag = true;
                obj1.NCAC352MOUFC_CreatedBy = data.UserId;
                obj1.NCAC352MOUFC_UpdatedBy = data.UserId;
                obj1.NCAC352MOUFC_UpdatedDate = DateTime.Now;
                obj1.NCAC352MOUFC_CreatedDate = DateTime.Now;
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
