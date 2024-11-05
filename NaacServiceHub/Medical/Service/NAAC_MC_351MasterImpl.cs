using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Medical;
using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Medical.Service
{
    public class NAAC_MC_351MasterImpl : Interface.NAAC_MC_351MasterInterface
    {
        public GeneralContext _GeneralContext;
        public NAAC_MC_351MasterImpl(GeneralContext para)
        {
            _GeneralContext = para;
        }


        public NAAC_MC_351_CollaborationActivities_DTO loaddata(NAAC_MC_351_CollaborationActivities_DTO data)
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

                data.yearlist = (from a in _GeneralContext.Academic
                                 where (a.MI_Id == data.MI_Id && a.Is_Active == true)
                                 select new NAAC_MC_351_CollaborationActivities_DTO
                                 {
                                     ASMAY_Id = a.ASMAY_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();




                data.alldata = (from a in _GeneralContext.NAAC_MC_351_CollaborationActivitiesDMO                                
                                where (a.MI_Id == data.MI_Id)
                                select a).Distinct().OrderByDescending(t => t.NCMC351CA_Id).ToArray();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public NAAC_MC_351_CollaborationActivities_DTO savedata(NAAC_MC_351_CollaborationActivities_DTO data)
        {
            try
            {
                if (data.NCMC351CA_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_MC_351_CollaborationActivitiesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC351CA_Year == data.ASMAY_Id && t.NCMC351CA_AgencyName == data.NCMC351CA_AgencyName && t.NCMC351CA_ParticipantsNames == data.NCMC351CA_ParticipantsNames).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_MC_351_CollaborationActivitiesDMO obj = new NAAC_MC_351_CollaborationActivitiesDMO();

                        //obj.NCMC351CA_Id = data.NCMC351CA_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.NCMC351CA_Year = data.ASMAY_Id;
                        obj.NCMC351CA_AgencyName = data.NCMC351CA_AgencyName;
                        obj.NCMC351CA_ActivityTitle = data.NCMC351CA_ActivityTitle;
                        obj.NCMC351CA_AgencyContactDetails = data.NCMC351CA_AgencyContactDetails;
                        obj.NCMC351CA_ParticipantsNames = data.NCMC351CA_ParticipantsNames;
                        obj.NCMC351CA_SourceOfFinacialSupport = data.NCMC351CA_SourceOfFinacialSupport;
                        obj.NCMC351CA_Duration = data.NCMC351CA_Duration;
                        obj.NCMC351CA_NatureOfActivity = data.NCMC351CA_NatureOfActivity;
                        obj.NCMC351CA_LinkDocument = data.NCMC351CA_LinkDocument;
                        obj.NCMC351CA_ActiveFlag = true;
                        obj.NCMC351CA_CreatedBy = data.UserId;
                        obj.NCMC351CA_UpdatedBy = data.UserId;
                        obj.NCMC351CA_CreatedDate = DateTime.Now;
                        obj.NCMC351CA_UpdatedDate = DateTime.Now;

                        _GeneralContext.Add(obj);

                        if (data.filelist.Length > 0)
                        {
                            for (int j = 0; j < data.filelist.Length; j++)
                            {
                                if (data.filelist[0].cfilepath != null)
                                {
                                    NAAC_MC_351_CollaborationActivities_FilesDMO obj2 = new NAAC_MC_351_CollaborationActivities_FilesDMO();

                                    obj2.NCMC351CAF_FileName = data.filelist[j].cfilename;
                                    obj2.NCMC351CAF_Filedesc = data.filelist[j].cfiledesc;
                                    obj2.NCMC351CAF_FilePath = data.filelist[j].cfilepath;
                                    obj2.NCMC351CA_Id = obj.NCMC351CA_Id;

                                    _GeneralContext.Add(obj2);
                                }
                            }
                        }
                        int s = _GeneralContext.SaveChanges();
                        if (s > 0)
                        {
                            data.msg = "saved";
                            data.returnval = true;
                        }
                        else
                        {
                            data.msg = "notsaved";
                            data.returnval = false;
                        }
                    }
                }
                else if (data.NCMC351CA_Id > 0)
                {
                    var duplicate = _GeneralContext.NAAC_MC_351_CollaborationActivitiesDMO.Where(t =>t.NCMC351CA_Id!=data.NCMC351CA_Id && t.MI_Id == data.MI_Id && t.NCMC351CA_Year == data.ASMAY_Id && t.NCMC351CA_AgencyName == data.NCMC351CA_AgencyName && t.NCMC351CA_ParticipantsNames == data.NCMC351CA_ParticipantsNames).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var result = _GeneralContext.NAAC_MC_351_CollaborationActivitiesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC351CA_Year == data.ASMAY_Id && t.NCMC351CA_AgencyName == data.NCMC351CA_AgencyName && t.NCMC351CA_ParticipantsNames == data.NCMC351CA_ParticipantsNames && t.NCMC351CA_Id == data.NCMC351CA_Id).Single();

                        result.NCMC351CA_Year = data.ASMAY_Id;
                        result.NCMC351CA_AgencyName = data.NCMC351CA_AgencyName;
                        result.NCMC351CA_ActivityTitle = data.NCMC351CA_ActivityTitle;
                        result.NCMC351CA_AgencyContactDetails = data.NCMC351CA_AgencyContactDetails;
                        result.NCMC351CA_ParticipantsNames = data.NCMC351CA_ParticipantsNames;
                        result.NCMC351CA_SourceOfFinacialSupport = data.NCMC351CA_SourceOfFinacialSupport;
                        result.NCMC351CA_Duration = data.NCMC351CA_Duration;
                        result.NCMC351CA_NatureOfActivity = data.NCMC351CA_NatureOfActivity;
                        result.NCMC351CA_LinkDocument = data.NCMC351CA_LinkDocument;
                        result.NCMC351CA_UpdatedBy = data.UserId;
                        result.NCMC351CA_UpdatedDate = DateTime.Now;

                        var CountRemoveFiles = _GeneralContext.NAAC_MC_351_CollaborationActivities_FilesDMO.Where(t => t.NCMC351CA_Id == data.NCMC351CA_Id).ToList();
                        if (CountRemoveFiles.Count > 0)
                        {
                            foreach (var RemoveFiles in CountRemoveFiles)
                            {
                                _GeneralContext.Remove(RemoveFiles);
                            }
                        }
                        if (data.filelist.Length > 0)
                        {
                            for (int k = 0; k < data.filelist.Length; k++)
                            {
                                if (data.filelist[0].cfilepath != null)
                                {
                                    NAAC_MC_351_CollaborationActivities_FilesDMO obj2 = new NAAC_MC_351_CollaborationActivities_FilesDMO();

                                    obj2.NCMC351CAF_FileName = data.filelist[k].cfilename;
                                    obj2.NCMC351CAF_Filedesc = data.filelist[k].cfiledesc;
                                    obj2.NCMC351CAF_FilePath = data.filelist[k].cfilepath;
                                    obj2.NCMC351CA_Id = result.NCMC351CA_Id;

                                    _GeneralContext.Add(obj2);
                                }
                            }
                        }

                        int s = _GeneralContext.SaveChanges();
                        if (s > 0)
                        {
                            data.msg = "updated";
                            data.returnval = true;
                        }
                        else
                        {
                            data.msg = "notupdated";
                            data.returnval = false;
                        }
                    }

                       


                    
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public NAAC_MC_351_CollaborationActivities_DTO editdata(NAAC_MC_351_CollaborationActivities_DTO data)
        {
            try
            {
                var edit = (from a in _GeneralContext.NAAC_MC_351_CollaborationActivitiesDMO
                            where (a.NCMC351CA_Id == data.NCMC351CA_Id)
                            select a).Distinct().ToList();

                data.editlist = edit.Distinct().ToArray();  

                data.editFileslist = (from a in _GeneralContext.NAAC_MC_351_CollaborationActivities_FilesDMO
                                      where (a.NCMC351CA_Id == data.NCMC351CA_Id)
                                      select new NAAC_MC_351_CollaborationActivities_DTO
                                      {
                                          cfilename = a.NCMC351CAF_FileName,
                                          cfilepath = a.NCMC351CAF_FilePath,
                                          cfiledesc = a.NCMC351CAF_Filedesc,
                                          NCMC351CAF_Id = a.NCMC351CAF_Id,
                                          NCMC351CA_Id = a.NCMC351CA_Id,
                                      }).Distinct().ToArray();


            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public NAAC_MC_351_CollaborationActivities_DTO deactivY(NAAC_MC_351_CollaborationActivities_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_MC_351_CollaborationActivitiesDMO.Where(t => t.NCMC351CA_Id == data.NCMC351CA_Id).SingleOrDefault();

                if (result.NCMC351CA_ActiveFlag == true)
                {
                    result.NCMC351CA_ActiveFlag = false;
                }
                else if (result.NCMC351CA_ActiveFlag == false)
                {
                    result.NCMC351CA_ActiveFlag = true;
                }

                result.NCMC351CA_UpdatedDate = DateTime.Now;
                result.NCMC351CA_UpdatedBy = data.UserId;

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
        public NAAC_MC_351_CollaborationActivities_DTO viewuploadflies(NAAC_MC_351_CollaborationActivities_DTO data)
        {
            try
            {
                data.viewuploadflies = (from t in _GeneralContext.NAAC_MC_351_CollaborationActivities_FilesDMO
                                        from b in _GeneralContext.NAAC_MC_351_CollaborationActivitiesDMO
                                        where (t.NCMC351CA_Id == data.NCMC351CA_Id && t.NCMC351CA_Id == b.NCMC351CA_Id && b.MI_Id == data.MI_Id)
                                        select new NAAC_MC_351_CollaborationActivities_DTO
                                        {
                                            cfilename = t.NCMC351CAF_FileName,
                                            cfilepath = t.NCMC351CAF_FilePath,
                                            cfiledesc = t.NCMC351CAF_Filedesc,
                                            NCMC351CAF_Id = t.NCMC351CAF_Id,
                                            NCMC351CA_Id = b.NCMC351CA_Id,
                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NAAC_MC_351_CollaborationActivities_DTO deleteuploadfile(NAAC_MC_351_CollaborationActivities_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_MC_351_CollaborationActivities_FilesDMO.Where(t => t.NCMC351CAF_Id == data.NCMC351CAF_Id).ToList();
                if (result.Count > 0)
                {
                    foreach (var resultid in result)
                    {
                        _GeneralContext.Remove(resultid);
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
                data.viewuploadflies = (from t in _GeneralContext.NAAC_MC_351_CollaborationActivities_FilesDMO
                                        from b in _GeneralContext.NAAC_MC_351_CollaborationActivitiesDMO
                                        where (t.NCMC351CA_Id == data.NCMC351CA_Id && t.NCMC351CA_Id == b.NCMC351CA_Id && b.MI_Id == data.MI_Id)
                                        select new NAAC_MC_351_CollaborationActivities_DTO
                                        {
                                            cfilename = t.NCMC351CAF_FileName,
                                            cfilepath = t.NCMC351CAF_FilePath,
                                            cfiledesc = t.NCMC351CAF_Filedesc,
                                            NCMC351CAF_Id = t.NCMC351CAF_Id,
                                            NCMC351CA_Id = b.NCMC351CA_Id,
                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
    }
}
