using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Medical;
using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Medical.Service
{
    public class NAAC_422_Clinical_LaboratoryImpl:Interface.NAAC_422_Clinical_LaboratoryInterface
    {

        public GeneralContext _GeneralContext;
        public NAAC_422_Clinical_LaboratoryImpl(GeneralContext para)
        {
            _GeneralContext = para;
        }
        public NAAC_MC_422_Clinical_Laboratory_DTO loaddata(NAAC_MC_422_Clinical_Laboratory_DTO data)
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
                                 select new NAAC_MC_422_Clinical_Laboratory_DTO
                                 {
                                     ASMAY_Id = a.ASMAY_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();



                data.alldata = (from a in _GeneralContext.NAAC_MC_422_Clinical_Laboratory_DMO
                                from b in _GeneralContext.NAAC_MC_422_Clinical_Laboratory_files_DMO
                                from y in _GeneralContext.Academic
                                where (a.MI_Id == data.MI_Id && a.NCMC422CL_Id == b.NCMC422CL_Id
                                && y.ASMAY_Id == a.NCMC422CL_Year)
                                select new NAAC_MC_422_Clinical_Laboratory_DTO
                                {
                                    ASMAY_Year = y.ASMAY_Year,
                                    NCMC422CL_Id = a.NCMC422CL_Id,
                                    NCMC422CL_NoOfOutpatientsTreated = a.NCMC422CL_NoOfOutpatientsTreated,
                                    NCMC422CL_OutStuPatientRatio = a.NCMC422CL_OutStuPatientRatio,
                                    NCMC422CL_NoofInPatientsTreated = a.NCMC422CL_NoofInPatientsTreated,
                                    NCMC422CL_InStuPatientRatio = a.NCMC422CL_InStuPatientRatio,
                                    NCMC422CL_ActiveFlag = a.NCMC422CL_ActiveFlag,
                                    MI_Id = a.MI_Id,
                                }).Distinct().OrderByDescending(t => t.NCMC422CL_Id).ToArray();

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public NAAC_MC_422_Clinical_Laboratory_DTO savedata(NAAC_MC_422_Clinical_Laboratory_DTO data)
        {
            try
            {

                if (data.NCMC422CL_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_MC_422_Clinical_Laboratory_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC422CL_Year == data.ASMAY_Id && t.NCMC422CL_NoOfOutpatientsTreated == data.NCMC422CL_NoOfOutpatientsTreated && t.NCMC422CL_OutStuPatientRatio == data.NCMC422CL_OutStuPatientRatio && t.NCMC422CL_NoofInPatientsTreated == data.NCMC422CL_NoofInPatientsTreated && t.NCMC422CL_InStuPatientRatio == data.NCMC422CL_InStuPatientRatio).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_MC_422_Clinical_Laboratory_DMO obj1 = new NAAC_MC_422_Clinical_Laboratory_DMO();


                        //obj1.NCMC422CL_Id = data.NCMC422CL_Id;
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCMC422CL_Year = data.ASMAY_Id;
                        obj1.NCMC422CL_NoOfOutpatientsTreated = data.NCMC422CL_NoOfOutpatientsTreated;
                        obj1.NCMC422CL_OutStuPatientRatio = data.NCMC422CL_OutStuPatientRatio;
                        obj1.NCMC422CL_NoofInPatientsTreated = data.NCMC422CL_NoofInPatientsTreated;
                        obj1.NCMC422CL_InStuPatientRatio = data.NCMC422CL_InStuPatientRatio;
                        obj1.NCMC422CL_CreatedDate = DateTime.Now;
                        obj1.NCMC422CL_UpdatedDate = DateTime.Now;
                        obj1.NCMC422CL_CreatedBy = data.UserId;
                        obj1.NCMC422CL_UpdatedBy = data.UserId;
                        obj1.NCMC422CL_ActiveFlag = true;

                        _GeneralContext.Add(obj1);
                      
                   
                        if (data.filelist.Length > 0)
                        {
                            for (int j = 0; j < data.filelist.Length; j++)
                            {
                                if (data.filelist[0].cfilepath != null)
                                {
                                    NAAC_MC_422_Clinical_Laboratory_files_DMO obj3 = new NAAC_MC_422_Clinical_Laboratory_files_DMO();

                                    obj3.NCMC422CLF_FileName = data.filelist[j].cfilename;
                                    obj3.NCMC422CLF_Filedesc = data.filelist[j].cfiledesc;
                                    obj3.NCMC422CLF_FilePath = data.filelist[j].cfilepath;
                                    obj3.NCMC422CL_Id = obj1.NCMC422CL_Id; 

                                    _GeneralContext.Add(obj3);
                                }
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
                    }
                }
                else if (data.NCMC422CL_Id > 0)
                {
                    var duplicate = _GeneralContext.NAAC_MC_422_Clinical_Laboratory_DMO.Where(t => t.MI_Id == data.MI_Id
                    && t.NCMC422CL_Id!=data.NCMC422CL_Id && t.NCMC422CL_Year == data.ASMAY_Id
                    && t.NCMC422CL_NoOfOutpatientsTreated == data.NCMC422CL_NoOfOutpatientsTreated 
                    && t.NCMC422CL_OutStuPatientRatio == data.NCMC422CL_OutStuPatientRatio
                    && t.NCMC422CL_NoOfOutpatientsTreated == data.NCMC422CL_NoOfOutpatientsTreated 
                    && t.NCMC422CL_InStuPatientRatio== data.NCMC422CL_InStuPatientRatio).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _GeneralContext.NAAC_MC_422_Clinical_Laboratory_DMO.Where(t => t.NCMC422CL_Id == data.NCMC422CL_Id && t.MI_Id == data.MI_Id).Single();
                       
                        update.NCMC422CL_Year = data.ASMAY_Id;
                        update.NCMC422CL_NoOfOutpatientsTreated = data.NCMC422CL_NoOfOutpatientsTreated;
                        update.NCMC422CL_OutStuPatientRatio = data.NCMC422CL_OutStuPatientRatio;
                        update.NCMC422CL_NoofInPatientsTreated = data.NCMC422CL_NoofInPatientsTreated;
                        update.NCMC422CL_InStuPatientRatio = data.NCMC422CL_InStuPatientRatio;
                        update.NCMC422CL_UpdatedDate = DateTime.Now;
                        update.NCMC422CL_UpdatedBy = data.UserId;

                        _GeneralContext.Update(update);                       

                        var remove2 = _GeneralContext.NAAC_MC_422_Clinical_Laboratory_files_DMO.Where(t => t.NCMC422CL_Id == data.NCMC422CL_Id).ToList();

                        if (remove2.Count > 0)
                        {
                            foreach (var rowdata2 in remove2)
                            {
                                _GeneralContext.Remove(rowdata2);
                            }
                        }
                        if (data.filelist.Length > 0)
                        {
                            for (int j = 0; j < data.filelist.Length; j++)
                            {
                                if (data.filelist[0].cfilepath != null)
                                {
                                    NAAC_MC_422_Clinical_Laboratory_files_DMO obj3 = new NAAC_MC_422_Clinical_Laboratory_files_DMO();

                                    obj3.NCMC422CLF_FileName = data.filelist[j].cfilename;
                                    obj3.NCMC422CLF_Filedesc = data.filelist[j].cfiledesc;
                                    obj3.NCMC422CLF_FilePath = data.filelist[j].cfilepath;
                                    obj3.NCMC422CL_Id = update.NCMC422CL_Id;

                                    _GeneralContext.Add(obj3);
                                }
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
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_MC_422_Clinical_Laboratory_DTO editdata(NAAC_MC_422_Clinical_Laboratory_DTO data)
        {
            try
            {
                data.editdata = (from a in _GeneralContext.NAAC_MC_422_Clinical_Laboratory_DMO
                                 where (a.NCMC422CL_Id == data.NCMC422CL_Id && a.MI_Id == data.MI_Id)
                                 select new NAAC_MC_422_Clinical_Laboratory_DTO
                                 {
                                     NCMC422CL_Id = a.NCMC422CL_Id,
                                     MI_Id = a.MI_Id,
                                     NCMC422CL_Year = a.NCMC422CL_Year,
                                     NCMC422CL_NoOfOutpatientsTreated = a.NCMC422CL_NoOfOutpatientsTreated,
                                     NCMC422CL_OutStuPatientRatio = a.NCMC422CL_OutStuPatientRatio,
                                     NCMC422CL_NoofInPatientsTreated = a.NCMC422CL_NoofInPatientsTreated,
                                     NCMC422CL_InStuPatientRatio = a.NCMC422CL_InStuPatientRatio,
                                     NCMC422CL_ActiveFlag = a.NCMC422CL_ActiveFlag,
                                 }).Distinct().ToArray();
              

                data.editFileslist = (from t in _GeneralContext.NAAC_MC_422_Clinical_Laboratory_files_DMO
                                      from b in _GeneralContext.NAAC_MC_422_Clinical_Laboratory_DMO
                                      where (t.NCMC422CL_Id == data.NCMC422CL_Id
                                      && t.NCMC422CL_Id == b.NCMC422CL_Id && b.MI_Id == data.MI_Id)
                                      select new NAAC_MC_422_Clinical_Laboratory_DTO
                                      {
                                          cfilename = t.NCMC422CLF_FileName,
                                          cfilepath = t.NCMC422CLF_FilePath,
                                          cfiledesc = t.NCMC422CLF_Filedesc,
                                          NCMC422CL_Id = t.NCMC422CL_Id,
                                          MI_Id = b.MI_Id,
                                      }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_MC_422_Clinical_Laboratory_DTO deactive_Y(NAAC_MC_422_Clinical_Laboratory_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_MC_422_Clinical_Laboratory_DMO.Where(t => t.NCMC422CL_Id == data.NCMC422CL_Id).SingleOrDefault();

                if (result.NCMC422CL_ActiveFlag == true)
                {
                    result.NCMC422CL_ActiveFlag = false;
                }
                else if (result.NCMC422CL_ActiveFlag == false)
                {
                    result.NCMC422CL_ActiveFlag = true;
                }

                result.NCMC422CL_UpdatedDate = DateTime.Now;
                result.NCMC422CL_UpdatedBy = data.UserId;

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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_MC_422_Clinical_Laboratory_DTO viewuploadflies(NAAC_MC_422_Clinical_Laboratory_DTO data)
        {
            try
            {
                data.viewuploadflies = (from t in _GeneralContext.NAAC_MC_422_Clinical_Laboratory_files_DMO
                                        from b in _GeneralContext.NAAC_MC_422_Clinical_Laboratory_DMO
                                        where (t.NCMC422CL_Id == data.NCMC422CL_Id
                                        && t.NCMC422CL_Id == b.NCMC422CL_Id && b.MI_Id == data.MI_Id)
                                        select new NAAC_MC_422_Clinical_Laboratory_DTO
                                        {
                                            cfilename = t.NCMC422CLF_FileName,
                                            cfilepath = t.NCMC422CLF_FilePath,
                                            cfiledesc = t.NCMC422CLF_Filedesc,
                                            NCMC422CL_Id = t.NCMC422CL_Id,
                                            NCMC422CLF_Id = t.NCMC422CLF_Id,
                                            MI_Id = b.MI_Id,
                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NAAC_MC_422_Clinical_Laboratory_DTO deleteuploadfile(NAAC_MC_422_Clinical_Laboratory_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_MC_422_Clinical_Laboratory_files_DMO.Where(t => t.NCMC422CLF_Id == data.NCMC422CLF_Id).ToList();
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
                data.viewuploadflies = (from t in _GeneralContext.NAAC_MC_422_Clinical_Laboratory_files_DMO
                                        from b in _GeneralContext.NAAC_MC_422_Clinical_Laboratory_DMO
                                        where (t.NCMC422CL_Id == data.NCMC422CL_Id
                                        && t.NCMC422CL_Id == b.NCMC422CL_Id && b.MI_Id == data.MI_Id)
                                        select new NAAC_MC_422_Clinical_Laboratory_DTO
                                        {
                                            cfilename = t.NCMC422CLF_FileName,
                                            cfilepath = t.NCMC422CLF_FilePath,
                                            cfiledesc = t.NCMC422CLF_Filedesc,
                                            NCMC422CL_Id = t.NCMC422CL_Id,                                          
                                            MI_Id = b.MI_Id,
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
