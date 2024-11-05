using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NAACAlumniMeetingImpl : Interface.NAACAlumniMeetingInterface
    {
        public GeneralContext _context;
        public NAACAlumniMeetingImpl(GeneralContext w)
        {
            _context = w;
        }
        public NAACAlumniMeetingDTO loaddata(NAACAlumniMeetingDTO data)
        {
            try
            {

                var institutionlist = (from a in _context.Institution
                                       from b in _context.UserRoleWithInstituteDMO
                                       where a.MI_Id == b.MI_Id && a.MI_ActiveFlag == 1 && b.Id == data.UserId && b.Activeflag == 1 && a.MI_ActiveFlag == 1
                                       select a).Distinct().OrderBy(t => t.MI_Name).ToList();
                data.institutionlist = institutionlist.ToArray();
                if (data.MI_Id == 0)
                {
                    if (institutionlist.Count > 0)
                    {
                        data.MI_Id = institutionlist.FirstOrDefault().MI_Id;
                    }
                }
                /////////////////
                data.allacademicyear = _context.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Year).ToArray();

                data.alldatalist = (from a in _context.Academic
                                    from b in _context.NAAC_AC_543_AlumniMeetingsDMO
                                    where (a.MI_Id == b.MI_Id && b.NCAC543ALMMET_MeetingYear == a.ASMAY_Id && a.Is_Active == true && a.MI_Id == data.MI_Id)
                                    select new NAACAlumniMeetingDTO
                                    {
                                        MI_Id = b.MI_Id,
                                        NCAC543ALMMET_Id = b.NCAC543ALMMET_Id,
                                        NCAC543ALMMET_MeetingDate = b.NCAC543ALMMET_MeetingDate,
                                        NCAC543ALMMET_TotalAlumniCount = b.NCAC543ALMMET_TotalAlumniCount,
                                        NCAC543ALMMET_ActiveFlg = b.NCAC543ALMMET_ActiveFlg,
                                        NCAC543ALMMET_NoOfMeetings = b.NCAC543ALMMET_NoOfMeetings,
                                        NCAC543ALMMET_NoOfMemAttnd = b.NCAC543ALMMET_NoOfMemAttnd,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCAC543ALMMET_StatusFlg = b.NCAC543ALMMET_StatusFlg
                                    }).Distinct().OrderByDescending(t => t.NCAC543ALMMET_Id).ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAACAlumniMeetingDTO save(NAACAlumniMeetingDTO data)
        {
            try
            {
                if (data.NCAC543ALMMET_Id == 0)
                {
                   

                    var duplicate = _context.NAAC_AC_543_AlumniMeetingsDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC543ALMMET_MeetingDate == data.NCAC543ALMMET_MeetingDate && t.NCAC543ALMMET_MeetingYear == data.NCAC543ALMMET_MeetingYear ).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {

                      
                        NAAC_AC_543_AlumniMeetingsDMO obj1 = new NAAC_AC_543_AlumniMeetingsDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC543ALMMET_MeetingYear = data.NCAC543ALMMET_MeetingYear;
                         obj1.NCAC543ALMMET_MeetingDate = data.NCAC543ALMMET_MeetingDate;
                         obj1.NCAC543ALMMET_TotalAlumniCount = data.NCAC543ALMMET_TotalAlumniCount;
                         obj1.NCAC543ALMMET_ActiveFlg = data.NCAC543ALMMET_ActiveFlg;
                         obj1.NCAC543ALMMET_NoOfMeetings = data.NCAC543ALMMET_NoOfMeetings;
                        obj1.NCAC543ALMMET_NoOfMemAttnd = data.NCAC543ALMMET_NoOfMemAttnd;
                        obj1.NCAC543ALMMET_ActiveFlg = true;
                        obj1.NCAC543ALMMET_CreatedBy = data.UserId;
                        obj1.NCAC543ALMMET_UpdatedBy = data.UserId;
                        obj1.NCAC543ALMMET_CreatedDate = DateTime.Now;
                        obj1.NCAC543ALMMET_UpdatedDate = DateTime.Now;
                        obj1.NCAC543ALMMET_StatusFlg = "";
                        _context.Add(obj1);


                        if (data.filelist.Length > 0)
                        {
                            foreach (var item in data.filelist)
                            {

                                if (item.cfilepath != null && item.cfilepath != "")
                                {
                                    NAAC_AC_543_AlumniMeetingsFilesDMO obb = new NAAC_AC_543_AlumniMeetingsFilesDMO();


                                    obb.NCAC543ALMMET_Id = obj1.NCAC543ALMMET_Id;
                                    obb.NCAC543ALMMETF_FileName = item.cfilename;
                                    obb.NCAC543ALMMETF_FilePath = item.cfilepath;
                                    obb.NCAC543ALMMETF_Filedesc = item.cfiledesc;
                                    obb.NCAC543ALMMETF_StatusFlg = item.status;
                                    obb.NCAC543ALMMETF_ActiveFlg = true;

                                    _context.Add(obb);
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
                            data.returnval =false;
                        }
                    }
                }
                else if (data.NCAC543ALMMET_Id > 0)
                {
                    var duplicate = _context.NAAC_AC_543_AlumniMeetingsDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC543ALMMET_MeetingDate == data.NCAC543ALMMET_MeetingDate && t.NCAC543ALMMET_MeetingYear == data.NCAC543ALMMET_MeetingYear && t.NCAC543ALMMET_Id !=data.NCAC543ALMMET_Id).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                       
                        var update = _context.NAAC_AC_543_AlumniMeetingsDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC543ALMMET_Id == data.NCAC543ALMMET_Id).SingleOrDefault();
                        update.MI_Id = data.MI_Id;
                        update.NCAC543ALMMET_MeetingYear = data.NCAC543ALMMET_MeetingYear;
                        update.NCAC543ALMMET_MeetingYear = data.NCAC543ALMMET_MeetingYear;
                        update.NCAC543ALMMET_MeetingDate = data.NCAC543ALMMET_MeetingDate;
                        update.NCAC543ALMMET_TotalAlumniCount = data.NCAC543ALMMET_TotalAlumniCount;
                        update.NCAC543ALMMET_ActiveFlg = data.NCAC543ALMMET_ActiveFlg;
                        update.NCAC543ALMMET_NoOfMeetings = data.NCAC543ALMMET_NoOfMeetings;
                        update.NCAC543ALMMET_NoOfMemAttnd = data.NCAC543ALMMET_NoOfMemAttnd;
                        update.NCAC543ALMMET_ActiveFlg = true;
                        update.NCAC543ALMMET_UpdatedBy = data.UserId;
                        update.NCAC543ALMMET_UpdatedDate = DateTime.Now;
                        _context.Update(update);

                        if (data.filelist.Length > 0)
                        {

                            List<long> Fid = new List<long>();
                            foreach (var item in data.filelist)
                            {
                                Fid.Add(item.cfileid);
                            }
                            var removefile1 = _context.NAAC_AC_543_AlumniMeetingsFilesDMO.Where(t => t.NCAC543ALMMET_Id == data.NCAC543ALMMET_Id && !Fid.Contains(t.NCAC543ALMMETF_Id)).Distinct().ToList();

                            if (removefile1.Count > 0)
                            {
                                foreach (var item2 in removefile1)
                                {
                                    var deactfile = _context.NAAC_AC_543_AlumniMeetingsFilesDMO.Single(t => t.NCAC543ALMMET_Id == data.NCAC543ALMMET_Id && t.NCAC543ALMMETF_Id == item2.NCAC543ALMMETF_Id);
                                    deactfile.NCAC543ALMMETF_ActiveFlg = false;
                                    _context.Update(deactfile);

                                }

                            }



                            foreach (var item in data.filelist)
                            {
                                if (item.status == null)
                                {
                                    item.status = "";
                                }

                                if (item.cfileid > 0 && item.status.ToLower() != "approved")
                                {
                                    var filesdata = _context.NAAC_AC_543_AlumniMeetingsFilesDMO.Where(t => t.NCAC543ALMMETF_Id == item.cfileid).FirstOrDefault();
                                    filesdata.NCAC543ALMMET_Id = data.NCAC543ALMMET_Id;
                                    filesdata.NCAC543ALMMETF_FileName = item.cfilename;
                                    filesdata.NCAC543ALMMETF_FilePath = item.cfilepath;
                                    filesdata.NCAC543ALMMETF_Filedesc = item.cfiledesc;
                                    filesdata.NCAC543ALMMETF_ActiveFlg = true;
                                    _context.Update(filesdata);


                                }
                                else
                                {
                                    if (item.cfileid == 0)
                                    {
                                        if (item.cfilepath != null && item.cfilepath != "")
                                        {
                                            NAAC_AC_543_AlumniMeetingsFilesDMO obb = new NAAC_AC_543_AlumniMeetingsFilesDMO();
                                            obb.NCAC543ALMMET_Id = data.NCAC543ALMMET_Id;
                                            obb.NCAC543ALMMETF_FileName = item.cfilename;
                                            obb.NCAC543ALMMETF_FilePath = item.cfilepath;
                                            obb.NCAC543ALMMETF_Filedesc = item.cfiledesc;
                                            obb.NCAC543ALMMETF_ActiveFlg = true;
                                            obb.NCAC543ALMMETF_StatusFlg = "";

                                            _context.Add(obb);

                                        }
                                    }
                                }
                            }



                        }
                        else
                        {

                            var removefile1 = _context.NAAC_AC_543_AlumniMeetingsFilesDMO.Where(t => t.NCAC543ALMMET_Id == data.NCAC543ALMMET_Id).Distinct().ToList();
                            if (removefile1.Count > 0)
                            {
                                foreach (var item in removefile1)
                                {
                                    var deactfile = _context.NAAC_AC_543_AlumniMeetingsFilesDMO.Single(t => t.NCAC543ALMMET_Id == data.NCAC543ALMMET_Id && t.NCAC543ALMMETF_Id == item.NCAC543ALMMETF_Id);
                                    deactfile.NCAC543ALMMETF_ActiveFlg = false;
                                    _context.Update(removefile1);
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

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAACAlumniMeetingDTO deactiveStudent(NAACAlumniMeetingDTO data)
        {
            try
            {
                var u = _context.NAAC_AC_543_AlumniMeetingsDMO.Where(t => t.NCAC543ALMMET_Id == data.NCAC543ALMMET_Id).SingleOrDefault();
                if (data.NCAC543ALMMET_ActiveFlg == true)
                {
                    u.NCAC543ALMMET_ActiveFlg = false;
                }
                else if (u.NCAC543ALMMET_ActiveFlg == false)
                {
                    u.NCAC543ALMMET_ActiveFlg = true;
                }
                u.NCAC543ALMMET_UpdatedDate = DateTime.Now;
                u.NCAC543ALMMET_UpdatedBy = data.UserId;
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
        public NAACAlumniMeetingDTO EditData(NAACAlumniMeetingDTO data)
        {
            try
            {
                data.editlist = (from a in _context.Academic
                                 from b in _context.NAAC_AC_543_AlumniMeetingsDMO
                                 where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCAC543ALMMET_MeetingYear && b.MI_Id == data.MI_Id && b.NCAC543ALMMET_Id == data.NCAC543ALMMET_Id)
                                 select new NAACAlumniMeetingDTO
                                 {
                                     NCAC543ALMMET_Id = b.NCAC543ALMMET_Id,
                                     NCAC543ALMMET_MeetingDate = b.NCAC543ALMMET_MeetingDate,
                                     NCAC543ALMMET_TotalAlumniCount = b.NCAC543ALMMET_TotalAlumniCount,
                                     NCAC543ALMMET_ActiveFlg = b.NCAC543ALMMET_ActiveFlg,
                                     NCAC543ALMMET_NoOfMeetings = b.NCAC543ALMMET_NoOfMeetings,
                                     NCAC543ALMMET_NoOfMemAttnd = b.NCAC543ALMMET_NoOfMemAttnd,
                                     NCAC543ALMMET_MeetingYear = b.NCAC543ALMMET_MeetingYear,
                                     NCAC543ALMMET_StatusFlg = b.NCAC543ALMMET_StatusFlg,
                                 
                                     ASMAY_Year = a.ASMAY_Year
                                 }).Distinct().ToArray();

                data.editfiles = (from a in _context.NAAC_AC_543_AlumniMeetingsFilesDMO

                                  where (a.NCAC543ALMMET_Id == data.NCAC543ALMMET_Id && a.NCAC543ALMMETF_ActiveFlg==true)
                                  select new NAACCriteriaFivefileDTO
                                  {
                                      cfilename = a.NCAC543ALMMETF_FileName,
                                      cfilepath = a.NCAC543ALMMETF_FilePath,
                                      cfiledesc = a.NCAC543ALMMETF_Filedesc,
                                      status = a.NCAC543ALMMETF_StatusFlg,
                                      cfileid=a.NCAC543ALMMETF_Id

                                  }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACAlumniMeetingDTO viewuploadflies(NAACAlumniMeetingDTO data)
        {
            try
            {

                data.editfiles = (from a in _context.NAAC_AC_543_AlumniMeetingsFilesDMO

                                  where (a.NCAC543ALMMET_Id == data.NCAC543ALMMET_Id && a.NCAC543ALMMETF_ActiveFlg == true)
                                  select new NAACCriteriaFivefileDTO
                                  {
                                      gridid = a.NCAC543ALMMET_Id,
                                      cfileid = a.NCAC543ALMMETF_Id,
                                      cfilename = a.NCAC543ALMMETF_FileName,
                                      cfilepath = a.NCAC543ALMMETF_FilePath,
                                      cfiledesc = a.NCAC543ALMMETF_Filedesc,
                                      status = a.NCAC543ALMMETF_StatusFlg,

                                  }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }

        public NAACAlumniMeetingDTO deleteuploadfile(NAACAlumniMeetingDTO data)
        {
            try
            {


                if (data.NCAC543ALMMETF_Id > 0)
                {
                    var deletefile = _context.NAAC_AC_543_AlumniMeetingsFilesDMO.Where(e => e.NCAC543ALMMETF_Id == data.NCAC543ALMMETF_Id).ToList();

                    if (deletefile.Count > 0)
                    {
                        foreach (var item in deletefile)
                        {
                            item.NCAC543ALMMETF_ActiveFlg = true;
                            _context.Update(item);
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



            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }

   
        public NAACAlumniMeetingDTO getcomment(NAACAlumniMeetingDTO data)
        {
            try
            {

                data.commentlist = (from a in _context.NAAC_AC_543_AlumniMeetings_CommentsDMO
                                    from b in _context.ApplUser
                                    where (a.NCAC543ALMMETC_RemarksBy == b.Id && a.NCAC543ALMMET_Id == data.NCAC543ALMMET_Id)
                                    select new commentsdto
                                    {
                                        remarks = a.NCAC543ALMMETC_Remarks,
                                        commentid = a.NCAC543ALMMETC_Id,
                                        status = a.NCAC543ALMMETC_StatusFlg,
                                        createddate = a.NCAC543ALMMETC_CreatedDate,
                                        activeflag = a.NCAC543ALMMETC_ActiveFlag,
                                        username = b.UserName,
                                    }).Distinct().OrderByDescending(t => t.createddate).ToArray();



            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACAlumniMeetingDTO getfilecomment(NAACAlumniMeetingDTO data)
        {
            try
            {

                data.commentlist = (from a in _context.NAAC_AC_543_AlumniMeetings_File_CommentsDMO
                                    from b in _context.ApplUser
                                    where (a.NCAC543ALMMETFC_RemarksBy == b.Id && a.NCAC543ALMMETF_Id == data.NCAC543ALMMETF_Id)
                                    select new commentsdto
                                    {
                                        remarks = a.NCAC543ALMMETFC_Remarks,
                                        commentid = a.NCAC543ALMMETFC_Id,
                                        status = a.NCAC543ALMMETFC_StatusFlg,
                                        createddate = a.NCAC543ALMMETFC_CreatedDate,
                                        activeflag = a.NCAC543ALMMETFC_ActiveFlag,
                                        username = b.UserName,
                                    }).Distinct().OrderByDescending(t => t.createddate).ToArray();



            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACAlumniMeetingDTO savemedicaldatawisecomments(NAACAlumniMeetingDTO data)
        {
            try
            {
                NAAC_AC_543_AlumniMeetings_CommentsDMO cm = new NAAC_AC_543_AlumniMeetings_CommentsDMO();
                cm.NCAC543ALMMETC_Remarks = data.Remarks;
                cm.NCAC543ALMMETC_RemarksBy = data.UserId;
                cm.NCAC543ALMMETC_StatusFlg = "";
                cm.NCAC543ALMMETC_ActiveFlag = true;
                cm.NCAC543ALMMETC_CreatedBy = data.UserId;
                cm.NCAC543ALMMETC_CreatedDate = DateTime.Now;
                cm.NCAC543ALMMETC_UpdatedBy = data.UserId;
                cm.NCAC543ALMMETC_UpdatedDate = DateTime.Now;
                cm.NCAC543ALMMET_Id = data.filefkid;
                _context.Add(cm);
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
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACAlumniMeetingDTO savefilewisecomments(NAACAlumniMeetingDTO data)
        {
            try
            {
                NAAC_AC_543_AlumniMeetings_File_CommentsDMO cm = new NAAC_AC_543_AlumniMeetings_File_CommentsDMO();
                cm.NCAC543ALMMETFC_Remarks = data.Remarks;
                cm.NCAC543ALMMETFC_RemarksBy = data.UserId;
                cm.NCAC543ALMMETFC_StatusFlg = "";
                cm.NCAC543ALMMETFC_ActiveFlag = true;
                cm.NCAC543ALMMETFC_CreatedBy = data.UserId;
                cm.NCAC543ALMMETFC_CreatedDate = DateTime.Now;
                cm.NCAC543ALMMETFC_UpdatedBy = data.UserId;
                cm.NCAC543ALMMETFC_UpdatedDate = DateTime.Now;
                cm.NCAC543ALMMETF_Id = data.filefkid;
                _context.Add(cm);
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
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
    }
}
