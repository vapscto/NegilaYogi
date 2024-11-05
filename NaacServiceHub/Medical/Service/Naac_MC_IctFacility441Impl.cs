using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Medical;
using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Medical.Service
{
    public class Naac_MC_IctFacility441Impl:Interface.Naac_MC_IctFacility441Interface
    {
        public GeneralContext _context;
        public Naac_MC_IctFacility441Impl(GeneralContext w)
        {
            _context = w;
        }
        public Naac_MC_IctFacility441_DTO loaddata(Naac_MC_IctFacility441_DTO data)
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
                                 from b in _context.Naac_MC_IctFacility441_DMO
                                 where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCMCCTTF441_Year && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.NCMCCTTF441_Year && a.Is_Active == true)
                                 select new Naac_MC_IctFacility441_DTO
                                 {
                                     NCMCCTTF441_Id = b.NCMCCTTF441_Id,
                                     NCMCCTTF441_NoOfClassroomsSeminarHallsLCD = b.NCMCCTTF441_NoOfClassroomsSeminarHallsLCD,
                                     NCMCCTTF441_TotalNoOfClassSeminarHalls = b.NCMCCTTF441_TotalNoOfClassSeminarHalls,
                                     NCMCCTTF441_NoOfClassroomsSeminarHallsLCDLan = b.NCMCCTTF441_NoOfClassroomsSeminarHallsLCDLan,
                                     NCMCCTTF441_NoOfClassroomsSeminarHallsLCDSmartboardLan = b.NCMCCTTF441_NoOfClassroomsSeminarHallsLCDSmartboardLan,
                                     NCMCCTTF441_NoOfClassroomsSeminarHallsLCDSmartboardLanAuVi = b.NCMCCTTF441_NoOfClassroomsSeminarHallsLCDSmartboardLanAuVi,
                                     NCMCCTTF441_Year = b.NCMCCTTF441_Year,
                                     ASMAY_Year = a.ASMAY_Year,
                                    
                                     MI_Id = b.MI_Id,
                                 }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public Naac_MC_IctFacility441_DTO save(Naac_MC_IctFacility441_DTO data)
        {
            try
            {
                if (data.NCMCCTTF441_Id == 0)
                {
                    var duplicate = _context.Naac_MC_IctFacility441_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCMCCTTF441_NoOfClassroomsSeminarHallsLCD == data.NCMCCTTF441_NoOfClassroomsSeminarHallsLCD && t.NCMCCTTF441_NoOfClassroomsSeminarHallsLCDLan == data.NCMCCTTF441_NoOfClassroomsSeminarHallsLCDLan && t.NCMCCTTF441_NoOfClassroomsSeminarHallsLCDSmartboardLan == data.NCMCCTTF441_NoOfClassroomsSeminarHallsLCDSmartboardLan&&t.NCMCCTTF441_NoOfClassroomsSeminarHallsLCDSmartboardLanAuVi==data.NCMCCTTF441_NoOfClassroomsSeminarHallsLCDSmartboardLanAuVi&&t.NCMCCTTF441_TotalNoOfClassSeminarHalls==data.NCMCCTTF441_TotalNoOfClassSeminarHalls && t.NCMCCTTF441_Id != 0).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        Naac_MC_IctFacility441_DMO u = new Naac_MC_IctFacility441_DMO();
                        u.MI_Id = data.MI_Id;
                        u.NCMCCTTF441_NoOfClassroomsSeminarHallsLCD = data.NCMCCTTF441_NoOfClassroomsSeminarHallsLCD;
                        u.NCMCCTTF441_NoOfClassroomsSeminarHallsLCDLan = data.NCMCCTTF441_NoOfClassroomsSeminarHallsLCDLan;
                        u.NCMCCTTF441_NoOfClassroomsSeminarHallsLCDSmartboardLan = data.NCMCCTTF441_NoOfClassroomsSeminarHallsLCDSmartboardLan;
                        u.NCMCCTTF441_NoOfClassroomsSeminarHallsLCDSmartboardLanAuVi = data.NCMCCTTF441_NoOfClassroomsSeminarHallsLCDSmartboardLanAuVi;
                        u.NCMCCTTF441_TotalNoOfClassSeminarHalls = data.NCMCCTTF441_TotalNoOfClassSeminarHalls;
                        u.NCMCCTTF441_CreatedBy = data.UserId;
                        u.NCMCCTTF441_UpdatedBy = data.UserId;
                        u.NCMCCTTF441_CreateDate = DateTime.Now;
                        u.NCMCCTTF441_UpdatedDate = DateTime.Now;
                        u.NCMCCTTF441_Year = data.ASMAY_Id;
                       
                        _context.Add(u);
                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[0].cfilepath != null)
                                {
                                    Naac_MC_IctFacility441_filesDMO obj2 = new Naac_MC_IctFacility441_filesDMO();
                                    obj2.NCMCCTTF441_Id = u.NCMCCTTF441_Id;
                                    obj2.NCMCCTTF441F_FileName = data.filelist[i].cfilename;
                                    obj2.NCMCCTTF441F_Filedesc = data.filelist[i].cfiledesc;
                                    obj2.NCMCCTTF441F_FilePath = data.filelist[i].cfilepath;

                                    _context.Add(obj2);
                                }
                            }
                        }
                        var w = _context.SaveChanges();
                        if (w > 0)
                        {
                            data.msg = "saved";
                        }
                        else
                        {
                            data.msg = "failed";
                        }
                    }
                }

                else if (data.NCMCCTTF441_Id > 0)
                {
                    var duplicate = _context.Naac_MC_IctFacility441_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCMCCTTF441_Id != data.NCMCCTTF441_Id && t.NCMCCTTF441_NoOfClassroomsSeminarHallsLCD == data.NCMCCTTF441_NoOfClassroomsSeminarHallsLCD &&t.NCMCCTTF441_TotalNoOfClassSeminarHalls ==data.NCMCCTTF441_TotalNoOfClassSeminarHalls && t.NCMCCTTF441_NoOfClassroomsSeminarHallsLCDLan == data.NCMCCTTF441_NoOfClassroomsSeminarHallsLCDLan && t.NCMCCTTF441_NoOfClassroomsSeminarHallsLCDSmartboardLan == data.NCMCCTTF441_NoOfClassroomsSeminarHallsLCDSmartboardLan && t.NCMCCTTF441_NoOfClassroomsSeminarHallsLCDSmartboardLanAuVi==data.NCMCCTTF441_NoOfClassroomsSeminarHallsLCDSmartboardLanAuVi&&t.NCMCCTTF441_Year == data.NCMCCTTF441_Year).Distinct().ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        var j = _context.Naac_MC_IctFacility441_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCMCCTTF441_Id == data.NCMCCTTF441_Id).SingleOrDefault();
                        j.NCMCCTTF441_NoOfClassroomsSeminarHallsLCD = data.NCMCCTTF441_NoOfClassroomsSeminarHallsLCD;
                        j.NCMCCTTF441_TotalNoOfClassSeminarHalls = data.NCMCCTTF441_TotalNoOfClassSeminarHalls;
                        j.NCMCCTTF441_NoOfClassroomsSeminarHallsLCDLan = data.NCMCCTTF441_NoOfClassroomsSeminarHallsLCDLan;
                        j.NCMCCTTF441_NoOfClassroomsSeminarHallsLCDSmartboardLan = data.NCMCCTTF441_NoOfClassroomsSeminarHallsLCDSmartboardLan;
                        j.NCMCCTTF441_NoOfClassroomsSeminarHallsLCDSmartboardLanAuVi = data.NCMCCTTF441_NoOfClassroomsSeminarHallsLCDSmartboardLanAuVi;
                        j.NCMCCTTF441_Year = data.ASMAY_Id;
                        j.MI_Id = data.MI_Id;
                        j.NCMCCTTF441_UpdatedDate = DateTime.Now;
                        j.NCMCCTTF441_UpdatedBy = data.UserId;
                        _context.Update(j);
                        var CountRemoveFiles = _context.Naac_MC_IctFacility441_filesDMO.Where(t => t.NCMCCTTF441_Id == data.NCMCCTTF441_Id).ToList();
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

                                        Naac_MC_IctFacility441_filesDMO obj2 = new Naac_MC_IctFacility441_filesDMO();
                                        obj2.NCMCCTTF441_Id = j.NCMCCTTF441_Id;
                                        obj2.NCMCCTTF441F_FileName = data.filelist[i].cfilename;
                                        obj2.NCMCCTTF441F_Filedesc = data.filelist[i].cfiledesc;
                                        obj2.NCMCCTTF441F_FilePath = data.filelist[i].cfilepath;
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
                                        Naac_MC_IctFacility441_filesDMO obj2 = new Naac_MC_IctFacility441_filesDMO();
                                        obj2.NCMCCTTF441_Id = j.NCMCCTTF441_Id;
                                        obj2.NCMCCTTF441F_FileName = data.filelist[i].cfilename;
                                        obj2.NCMCCTTF441F_Filedesc = data.filelist[i].cfiledesc;
                                        obj2.NCMCCTTF441F_FilePath = data.filelist[i].cfilepath;
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
   
        public Naac_MC_IctFacility441_DTO EditData(Naac_MC_IctFacility441_DTO data)
        {
            try
            {

                data.editlist = (from a in _context.Academic
                                 from b in _context.Naac_MC_IctFacility441_DMO
                                 where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && b.NCMCCTTF441_Id == data.NCMCCTTF441_Id && a.ASMAY_Id == b.NCMCCTTF441_Year)
                                 select new Naac_MC_IctFacility441_DTO
                                 {
                                     NCMCCTTF441_Id = b.NCMCCTTF441_Id,
                                     NCMCCTTF441_NoOfClassroomsSeminarHallsLCD = b.NCMCCTTF441_NoOfClassroomsSeminarHallsLCD,
                                     NCMCCTTF441_TotalNoOfClassSeminarHalls = b.NCMCCTTF441_TotalNoOfClassSeminarHalls,
                                     NCMCCTTF441_NoOfClassroomsSeminarHallsLCDLan = b.NCMCCTTF441_NoOfClassroomsSeminarHallsLCDLan,
                                     NCMCCTTF441_NoOfClassroomsSeminarHallsLCDSmartboardLan = b.NCMCCTTF441_NoOfClassroomsSeminarHallsLCDSmartboardLan,
                                     NCMCCTTF441_NoOfClassroomsSeminarHallsLCDSmartboardLanAuVi = b.NCMCCTTF441_NoOfClassroomsSeminarHallsLCDSmartboardLanAuVi,
                                     NCMCCTTF441_Year = b.NCMCCTTF441_Year,
                                     MI_Id = b.MI_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();

                data.editFileslist = (from a in _context.Naac_MC_IctFacility441_filesDMO
                                      where (a.NCMCCTTF441_Id == data.NCMCCTTF441_Id)
                                      select new Naac_MC_IctFacility441_DTO
                                      {
                                          cfilename = a.NCMCCTTF441F_FileName,
                                          cfilepath = a.NCMCCTTF441F_FilePath,
                                          cfiledesc = a.NCMCCTTF441F_Filedesc,
                                      }).Distinct().ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public Naac_MC_IctFacility441_DTO viewuploadflies(Naac_MC_IctFacility441_DTO data)
        {
            try
            {
                data.viewuploadflies = (from a in _context.Naac_MC_IctFacility441_filesDMO
                                        where (a.NCMCCTTF441_Id == data.NCMCCTTF441_Id)
                                        select new Naac_MC_IctFacility441_DTO
                                        {
                                            cfilename = a.NCMCCTTF441F_FileName,
                                            cfilepath = a.NCMCCTTF441F_FilePath,
                                            cfiledesc = a.NCMCCTTF441F_Filedesc,
                                            NCMCCTTF441F_Id = a.NCMCCTTF441F_Id,
                                            NCMCCTTF441_Id = a.NCMCCTTF441_Id,
                                        }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;

        }
        public Naac_MC_IctFacility441_DTO deleteuploadfile(Naac_MC_IctFacility441_DTO data)
        {
            try
            {
                var res = _context.Naac_MC_IctFacility441_filesDMO.Where(t => t.NCMCCTTF441F_Id == data.NCMCCTTF441F_Id).SingleOrDefault();
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
                data.viewuploadflies = (from a in _context.Naac_MC_IctFacility441_filesDMO
                                        where (a.NCMCCTTF441_Id == data.NCMCCTTF441_Id)
                                        select new Naac_MC_IctFacility441_DTO
                                        {
                                            cfilename = a.NCMCCTTF441F_FileName,
                                            cfilepath = a.NCMCCTTF441F_FilePath,
                                            cfiledesc = a.NCMCCTTF441F_Filedesc,
                                            NCMCCTTF441F_Id = a.NCMCCTTF441F_Id,
                                            NCMCCTTF441_Id = a.NCMCCTTF441_Id,
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
