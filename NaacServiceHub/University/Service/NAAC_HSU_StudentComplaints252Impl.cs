using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.University;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Service
{
    public class NAAC_HSU_StudentComplaints252Impl:Interface.NAAC_HSU_StudentComplaints252Interface
    {

        public GeneralContext _context;
        public NAAC_HSU_StudentComplaints252Impl(GeneralContext w)
        {
            _context = w;
        }
        public NAAC_HSU_StudentComplaints252_DTO loaddata(NAAC_HSU_StudentComplaints252_DTO data)
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
                                 from b in _context.NAAC_HSU_StudentComplaints_252_DMO
                                 where (a.MI_Id == b.MI_Id && a.Is_Active == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.NCHSU252SC_Year)
                                 select new NAAC_HSU_StudentComplaints252_DTO
                                 {
                                     NCHSU252SC_Id = b.NCHSU252SC_Id,
                                     NCHSU252SC_NoOfStudentsComplaints = b.NCHSU252SC_NoOfStudentsComplaints,
                                     NCHSU252SC_TotalNoOfStudentsAppereadExam = b.NCHSU252SC_TotalNoOfStudentsAppereadExam,
                                  
                                     NCHSU252SC_Year = b.NCHSU252SC_Year,
                                    
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCHSU252SC_ActiveFlag = b.NCHSU252SC_ActiveFlag,
                                     MI_Id = b.MI_Id,
                                 }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_HSU_StudentComplaints252_DTO save(NAAC_HSU_StudentComplaints252_DTO data)
        {
            try
            {
                if (data.NCHSU252SC_Id == 0)
                {
                    var duplicate = _context.NAAC_HSU_StudentComplaints_252_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCHSU252SC_NoOfStudentsComplaints == data.NCHSU252SC_NoOfStudentsComplaints && t.NCHSU252SC_TotalNoOfStudentsAppereadExam == data.NCHSU252SC_TotalNoOfStudentsAppereadExam &&  t.NCHSU252SC_Id != 0 ).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        NAAC_HSU_StudentComplaints_252_DMO u = new NAAC_HSU_StudentComplaints_252_DMO();
                        u.MI_Id = data.MI_Id;
                        u.NCHSU252SC_NoOfStudentsComplaints = data.NCHSU252SC_NoOfStudentsComplaints;
                        u.NCHSU252SC_TotalNoOfStudentsAppereadExam = data.NCHSU252SC_TotalNoOfStudentsAppereadExam;
                  
                        u.NCHSU252SC_CreatedBy = data.UserId;
                        u.NCHSU252SC_UpdatedBy = data.UserId;
                        u.NCHSU252SC_CreatedDate = DateTime.Now;
                        u.NCHSU252SC_UpdatedDate = DateTime.Now;
                        u.NCHSU252SC_Year = data.ASMAY_Id;
                        u.NCHSU252SC_ActiveFlag = true;
                        _context.Add(u);
                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[0].cfilepath != null)
                                {
                                    NAAC_HSU_StudentComplaints_252_Files_DMO obj2 = new NAAC_HSU_StudentComplaints_252_Files_DMO();
                                    obj2.NCHSU252SC_Id = u.NCHSU252SC_Id;
                                    obj2.NCHSU252SCF_FileName = data.filelist[i].cfilename;
                                    obj2.NCHSU252SCF_Filedesc = data.filelist[i].cfiledesc;
                                    obj2.NCHSU252SCF_FilePath = data.filelist[i].cfilepath;

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

                else if (data.NCHSU252SC_Id > 0)
                {
                    var duplicate = _context.NAAC_HSU_StudentComplaints_252_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCHSU252SC_Id != data.NCHSU252SC_Id && t.NCHSU252SC_NoOfStudentsComplaints == data.NCHSU252SC_NoOfStudentsComplaints && t.NCHSU252SC_TotalNoOfStudentsAppereadExam == data.NCHSU252SC_TotalNoOfStudentsAppereadExam && t.NCHSU252SC_Year == data.NCHSU252SC_Year  ).Distinct().ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        var j = _context.NAAC_HSU_StudentComplaints_252_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCHSU252SC_Id == data.NCHSU252SC_Id).SingleOrDefault();
                        j.NCHSU252SC_NoOfStudentsComplaints = data.NCHSU252SC_NoOfStudentsComplaints;
                        j.NCHSU252SC_TotalNoOfStudentsAppereadExam = data.NCHSU252SC_TotalNoOfStudentsAppereadExam;
                     
                        j.NCHSU252SC_Year = data.ASMAY_Id;
                        j.MI_Id = data.MI_Id;
                        j.NCHSU252SC_UpdatedDate = DateTime.Now;
                        j.NCHSU252SC_UpdatedBy = data.UserId;
                        _context.Update(j);
                        var CountRemoveFiles = _context.NAAC_HSU_StudentComplaints_252_Files_DMO.Where(t => t.NCHSU252SC_Id == data.NCHSU252SC_Id).ToList();
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

                                        NAAC_HSU_StudentComplaints_252_Files_DMO obj2 = new NAAC_HSU_StudentComplaints_252_Files_DMO();
                                        obj2.NCHSU252SC_Id = j.NCHSU252SC_Id;
                                        obj2.NCHSU252SCF_FileName = data.filelist[i].cfilename;
                                        obj2.NCHSU252SCF_Filedesc = data.filelist[i].cfiledesc;
                                        obj2.NCHSU252SCF_FilePath = data.filelist[i].cfilepath;
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
                                        NAAC_HSU_StudentComplaints_252_Files_DMO obj2 = new NAAC_HSU_StudentComplaints_252_Files_DMO();
                                        obj2.NCHSU252SC_Id = j.NCHSU252SC_Id;
                                        obj2.NCHSU252SCF_FileName = data.filelist[i].cfilename;
                                        obj2.NCHSU252SCF_Filedesc = data.filelist[i].cfiledesc;
                                        obj2.NCHSU252SCF_FilePath = data.filelist[i].cfilepath;
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
        public NAAC_HSU_StudentComplaints252_DTO deactive(NAAC_HSU_StudentComplaints252_DTO data)
        {
            try
            {
                var g = _context.NAAC_HSU_StudentComplaints_252_DMO.Where(t => t.NCHSU252SC_Id == data.NCHSU252SC_Id).SingleOrDefault();
                if (g.NCHSU252SC_ActiveFlag == true)
                {
                    g.NCHSU252SC_ActiveFlag = false;
                }
                else
                {
                    g.NCHSU252SC_ActiveFlag = true;
                }
                g.NCHSU252SC_UpdatedDate = DateTime.Now;
                g.NCHSU252SC_UpdatedBy = data.UserId;
                g.MI_Id = data.MI_Id;
                _context.Update(g);
                int s = _context.SaveChanges();
                if (s > 0)
                {
                    data.ret = true;
                }
                else
                {
                    data.ret = false;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_HSU_StudentComplaints252_DTO EditData(NAAC_HSU_StudentComplaints252_DTO data)
        {
            try
            {

                data.editlist = (from a in _context.Academic
                                 from b in _context.NAAC_HSU_StudentComplaints_252_DMO
                                 where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && b.NCHSU252SC_Id == data.NCHSU252SC_Id && a.ASMAY_Id == b.NCHSU252SC_Year)
                                 select new NAAC_HSU_StudentComplaints252_DTO
                                 {
                                     NCHSU252SC_Id = b.NCHSU252SC_Id,
                                     NCHSU252SC_NoOfStudentsComplaints = b.NCHSU252SC_NoOfStudentsComplaints,
                                     NCHSU252SC_TotalNoOfStudentsAppereadExam = b.NCHSU252SC_TotalNoOfStudentsAppereadExam,
                                   
                                     NCHSU252SC_Year = b.NCHSU252SC_Year,
                                
                                     MI_Id = b.MI_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();

                data.editFileslist = (from a in _context.NAAC_HSU_StudentComplaints_252_Files_DMO
                                      where (a.NCHSU252SC_Id == data.NCHSU252SC_Id)
                                      select new NAAC_HSU_StudentComplaints252_DTO
                                      {
                                          cfilename = a.NCHSU252SCF_FileName,
                                          cfilepath = a.NCHSU252SCF_FilePath,
                                          cfiledesc = a.NCHSU252SCF_Filedesc,
                                      }).Distinct().ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_HSU_StudentComplaints252_DTO viewuploadflies(NAAC_HSU_StudentComplaints252_DTO data)
        {
            try
            {
                data.viewuploadflies = (from a in _context.NAAC_HSU_StudentComplaints_252_Files_DMO
                                        where (a.NCHSU252SC_Id == data.NCHSU252SC_Id)
                                        select new NAAC_HSU_StudentComplaints252_DTO
                                        {
                                            cfilename = a.NCHSU252SCF_FileName,
                                            cfilepath = a.NCHSU252SCF_FilePath,
                                            cfiledesc = a.NCHSU252SCF_Filedesc,
                                            NCHSU252SCF_Id = a.NCHSU252SCF_Id,
                                            NCHSU252SC_Id = a.NCHSU252SC_Id,
                                        }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;

        }
        public NAAC_HSU_StudentComplaints252_DTO deleteuploadfile(NAAC_HSU_StudentComplaints252_DTO data)
        {
            try
            {
                var res = _context.NAAC_HSU_StudentComplaints_252_Files_DMO.Where(t => t.NCHSU252SCF_Id == data.NCHSU252SCF_Id).SingleOrDefault();
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
                data.viewuploadflies = (from a in _context.NAAC_HSU_StudentComplaints_252_Files_DMO
                                        where (a.NCHSU252SC_Id == data.NCHSU252SC_Id)
                                        select new NAAC_HSU_StudentComplaints252_DTO
                                        {
                                            cfilename = a.NCHSU252SCF_FileName,
                                            cfilepath = a.NCHSU252SCF_FilePath,
                                            cfiledesc = a.NCHSU252SCF_Filedesc,
                                            NCHSU252SCF_Id = a.NCHSU252SCF_Id,
                                            NCHSU252SC_Id = a.NCHSU252SC_Id,
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
