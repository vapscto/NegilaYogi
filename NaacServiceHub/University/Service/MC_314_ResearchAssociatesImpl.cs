using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.University;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Service
{
    public class MC_314_ResearchAssociatesImpl:Interface.MC_314_ResearchAssociatesInterface
    {
        public GeneralContext _context;
        public MC_314_ResearchAssociatesImpl(GeneralContext y)
        {
            _context = y;
        }
        public MC_314_ResearchAssociatesDTO loaddata(MC_314_ResearchAssociatesDTO data)
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

                data.allacademicyear = _context.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).ToArray();
                data.alldata1 = (from a in _context.Academic
                                 from b in _context.MC_314_ResearchAssociatesDMO
                                 where (a.MI_Id == data.MI_Id && b.MI_Id == a.MI_Id && a.Is_Active == true && b.NCMCRA314_Year == a.ASMAY_Id)
                                 select new MC_314_ResearchAssociatesDTO
                                 {
                                     NCMCRA314_Id = b.NCMCRA314_Id,
                                     NCMCRA314_Year = b.NCMCRA314_Year,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCMCRA314_NameOfResearch = b.NCMCRA314_NameOfResearch,
                                     NCMCRA314_Type = b.NCMCRA314_Type,
                                     NCMCRA314_GrantingAgency = b.NCMCRA314_GrantingAgency,
                                     NCMCRA314_QualExamName = b.NCMCRA314_QualExamName,
                                     NCMCRA314_Duration = b.NCMCRA314_Duration,
                                     NCMCRA314_ActiveFlag = b.NCMCRA314_ActiveFlag,
                                     MI_Id = data.MI_Id

                                 }).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }

        public MC_314_ResearchAssociatesDTO save(MC_314_ResearchAssociatesDTO data)
        {
            try
            {
                if (data.NCMCRA314_Id == 0)
                {
                    var duplicate = _context.MC_314_ResearchAssociatesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMCRA314_Year == data.asmaY_Id && t.NCMCRA314_Id != 0 && t.NCMCRA314_NameOfResearch == data.NCMCRA314_NameOfResearch && t.NCMCRA314_Type == data.NCMCRA314_Type && t.NCMCRA314_GrantingAgency == data.NCMCRA314_GrantingAgency && t.NCMCRA314_QualExamName == data.NCMCRA314_QualExamName && t.NCMCRA314_Duration==data.NCMCRA314_Duration).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        MC_314_ResearchAssociatesDMO rrr = new MC_314_ResearchAssociatesDMO();
                        rrr.MI_Id = data.MI_Id;
                        rrr.NCMCRA314_NameOfResearch = data.NCMCRA314_NameOfResearch;
                        rrr.NCMCRA314_Type = data.NCMCRA314_Type;
                        rrr.NCMCRA314_GrantingAgency = data.NCMCRA314_GrantingAgency;
                        rrr.NCMCRA314_QualExamName = data.NCMCRA314_QualExamName;
                        rrr.NCMCRA314_Duration = data.NCMCRA314_Duration;
                        rrr.NCMCRA314_Year = data.asmaY_Id;
                        rrr.NCMCRA314_CreatedDate = DateTime.Now;
                        rrr.NCMCRA314_UpdatedDate = DateTime.Now;
                        rrr.NCMCRA314_ActiveFlag = true;
                        rrr.NCMCRA314_CreatedBy = data.UserId;
                        rrr.NCMCRA314_UpdatedBy = data.UserId;
                        _context.Add(rrr);
                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[0].cfilepath != null)
                                {


                                    MC_314_ResearchAssociates_FilesDMO obj2 = new MC_314_ResearchAssociates_FilesDMO();
                                    obj2.NCMCRA314_Id = rrr.NCMCRA314_Id;
                                    //obj2.MI_Id = data.MI_Id;
                                    obj2.NCMCRA314F_FileName = data.filelist[i].cfilename;
                                    obj2.NCMCRA314F_Filedesc = data.filelist[i].cfiledesc;
                                    obj2.NCMCRA314F_FilePath = data.filelist[i].cfilepath;

                                    _context.Add(obj2);
                                }
                            }
                        }

                        int y = _context.SaveChanges();
                        if (y > 0)
                        {
                            data.msg = "saved";
                        }
                        else
                        {
                            data.msg = "Failed";
                        }
                    }
                }
                else if (data.NCMCRA314_Id > 0)
                {
                    var duplicate = _context.MC_314_ResearchAssociatesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMCRA314_NameOfResearch == data.NCMCRA314_NameOfResearch && t.NCMCRA314_Type == data.NCMCRA314_Type && t.NCMCRA314_Year == data.asmaY_Id && t.NCMCRA314_Id != data.NCMCRA314_Id && t.NCMCRA314_GrantingAgency == data.NCMCRA314_GrantingAgency && t.NCMCRA314_QualExamName == data.NCMCRA314_QualExamName && t.NCMCRA314_Duration==data.NCMCRA314_Duration).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var yy = _context.MC_314_ResearchAssociatesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMCRA314_Id == data.NCMCRA314_Id).SingleOrDefault();

                        yy.NCMCRA314_UpdatedBy = data.UserId;
                        yy.NCMCRA314_NameOfResearch = data.NCMCRA314_NameOfResearch;
                        yy.NCMCRA314_Type = data.NCMCRA314_Type;
                        yy.NCMCRA314_GrantingAgency = data.NCMCRA314_GrantingAgency;
                        yy.NCMCRA314_QualExamName = data.NCMCRA314_QualExamName;
                        yy.NCMCRA314_Duration = data.NCMCRA314_Duration;
                        yy.NCMCRA314_Year = data.asmaY_Id;
                        yy.MI_Id = data.MI_Id;
                        yy.NCMCRA314_UpdatedDate = DateTime.Now;
                        _context.Update(yy);

                        var CountRemoveFiles = _context.MC_314_ResearchAssociates_FilesDMO.Where(t => t.NCMCRA314_Id == data.NCMCRA314_Id).ToList();
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


                                        MC_314_ResearchAssociates_FilesDMO obj2 = new MC_314_ResearchAssociates_FilesDMO();
                                        obj2.NCMCRA314_Id = yy.NCMCRA314_Id;
                                        //obj2.MI_Id = data.MI_Id;
                                        obj2.NCMCRA314F_FileName = data.filelist[i].cfilename;
                                        obj2.NCMCRA314F_Filedesc = data.filelist[i].cfiledesc;
                                        obj2.NCMCRA314F_FilePath = data.filelist[i].cfilepath;

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


                                        // UC_312_TeachersResearch_FilesDMO obj2 = new UC_312_TeachersResearch_FilesDMO();
                                        MC_314_ResearchAssociates_FilesDMO obj2 = new MC_314_ResearchAssociates_FilesDMO();
                                        obj2.NCMCRA314_Id = yy.NCMCRA314_Id;
                                        // obj2.MI_Id = data.MI_Id;
                                        obj2.NCMCRA314F_FileName = data.filelist[i].cfilename;
                                        obj2.NCMCRA314F_Filedesc = data.filelist[i].cfiledesc;
                                        obj2.NCMCRA314F_FilePath = data.filelist[i].cfilepath;

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
        public MC_314_ResearchAssociatesDTO deactive(MC_314_ResearchAssociatesDTO data)
        {
            try
            {
                var u = _context.MC_314_ResearchAssociatesDMO.Where(t => t.NCMCRA314_Id == data.NCMCRA314_Id).SingleOrDefault();
                if (u.NCMCRA314_ActiveFlag == true)
                {
                    u.NCMCRA314_ActiveFlag = false;
                }
                else if (u.NCMCRA314_ActiveFlag == false)
                {
                    u.NCMCRA314_ActiveFlag = true;
                }
                u.NCMCRA314_UpdatedDate = DateTime.Now;
                u.NCMCRA314_UpdatedBy = data.UserId;
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
        public MC_314_ResearchAssociatesDTO EditData(MC_314_ResearchAssociatesDTO data)
        {
            try
            {
                data.editlist = (from a in _context.Academic
                                 from b in _context.MC_314_ResearchAssociatesDMO
                                 where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCMCRA314_Year && b.MI_Id == data.MI_Id && b.NCMCRA314_Id == data.NCMCRA314_Id)
                                 select new MC_314_ResearchAssociatesDTO
                                 {
                                     NCMCRA314_Id = b.NCMCRA314_Id,
                                     NCMCRA314_NameOfResearch = b.NCMCRA314_NameOfResearch,
                                     NCMCRA314_Type = b.NCMCRA314_Type,
                                     NCMCRA314_GrantingAgency = b.NCMCRA314_GrantingAgency,
                                     NCMCRA314_QualExamName = b.NCMCRA314_QualExamName,
                                     NCMCRA314_Duration = b.NCMCRA314_Duration,
                                     NCMCRA314_Year = b.NCMCRA314_Year,
                                     MI_Id = data.MI_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();

                data.editFileslist = (from a in _context.MC_314_ResearchAssociates_FilesDMO
                                      where (a.NCMCRA314_Id == data.NCMCRA314_Id)
                                      select new MC_314_ResearchAssociatesDTO
                                      {
                                          cfilename = a.NCMCRA314F_FileName,
                                          cfilepath = a.NCMCRA314F_FilePath,
                                          cfiledesc = a.NCMCRA314F_Filedesc,

                                      }).Distinct().ToArray();
            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }

        public MC_314_ResearchAssociatesDTO viewuploadflies(MC_314_ResearchAssociatesDTO data)
        {
            try
            {
                data.viewuploadflies = (from a in _context.MC_314_ResearchAssociates_FilesDMO
                                        where (a.NCMCRA314_Id == data.NCMCRA314_Id)
                                        select new MC_314_ResearchAssociatesDTO
                                        {
                                            cfilename = a.NCMCRA314F_FileName,
                                            cfilepath = a.NCMCRA314F_FilePath,
                                            cfiledesc = a.NCMCRA314F_Filedesc,
                                            NCMCRA314F_Id = a.NCMCRA314F_Id,
                                            NCMCRA314_Id = a.NCMCRA314_Id,
                                        }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;

        }
        public MC_314_ResearchAssociatesDTO deleteuploadfile(MC_314_ResearchAssociatesDTO data)
        {
            try
            {
                var res = _context.MC_314_ResearchAssociates_FilesDMO.Where(t => t.NCMCRA314F_Id == data.NCMCRA314F_Id).SingleOrDefault();
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
                data.viewuploadflies = (from a in _context.MC_314_ResearchAssociates_FilesDMO
                                        where (a.NCMCRA314_Id == data.NCMCRA314_Id)
                                        select new MC_314_ResearchAssociatesDTO
                                        {
                                            NCMCRA314_Id = a.NCMCRA314_Id,
                                            NCMCRA314F_Id = a.NCMCRA314F_Id,
                                            cfilename = a.NCMCRA314F_FileName,
                                            cfilepath = a.NCMCRA314F_FilePath,
                                            cfiledesc = a.NCMCRA314F_Filedesc,
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
