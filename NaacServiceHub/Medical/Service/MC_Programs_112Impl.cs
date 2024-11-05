using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Medical;
using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Medical.Service
{
    public class MC_Programs_112Impl : Interface.MC_Programs_112Interface
    {
        public GeneralContext _GeneralContext;
        public MC_Programs_112Impl(GeneralContext para)
        {
            _GeneralContext = para;
        }
        public MC_Programs_112_DTO loaddata(MC_Programs_112_DTO data)
        {
            try
            {
                var institutionlist = (from a in _GeneralContext.Institution
                                       from b in _GeneralContext.UserRoleWithInstituteDMO
                                       where (b.Id == data.UserId && b.MI_Id == a.MI_Id && b.Activeflag == 1 
                                       && a.MI_ActiveFlag == 1)
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
                                 select new MC_Programs_112_DTO
                                 {
                                     ASMAY_Id = a.ASMAY_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();

                data.emplylist_1 = (from a in _GeneralContext.HR_Master_Employee_DMO
                                    where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                    select new MC_Programs_112_DTO
                                    {
                                        empname = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                        HRME_Id = a.HRME_Id,
                                        HRME_EmployeeCode = a.HRME_EmployeeCode,
                                        HRME_EmployeeOrder = a.HRME_EmployeeOrder,
                                    }).Distinct().OrderBy(t => t.HRME_EmployeeOrder).ToArray();

                data.emplylist_2 = (from a in _GeneralContext.HR_Master_Employee_DMO
                                    where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                    select new MC_Programs_112_DTO
                                    {
                                        empnamecouncil = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                        empid = a.HRME_Id,
                                        empcode = a.HRME_EmployeeCode,
                                        emporder = a.HRME_EmployeeOrder,
                                    }).Distinct().OrderBy(t => t.emporder).ToArray();

                data.alldata = (from a in _GeneralContext.NAAC_MC_Master_Programs_112_DMO
                                from b in _GeneralContext.NAAC_MC_Master_Programs_112_Details_DMO
                                from y in _GeneralContext.Academic
                                where (a.MI_Id == data.MI_Id && a.NCMCMPR112_Id == b.NCMCMPR112_Id
                                && y.ASMAY_Id == a.NCMCMPR112_Year)
                                select new MC_Programs_112_DTO
                                {
                                    ASMAY_Year = y.ASMAY_Year,
                                    NCMCMPR112_Id = a.NCMCMPR112_Id,
                                    NCMCMPR112_NoOfTeachersAcu = a.NCMCMPR112_NoOfTeachersAcu,
                                    NCMCMPR112_NoOfTeachersPartBos = a.NCMCMPR112_NoOfTeachersPartBos,
                                    NCMCMPR112_ActiveFlag = a.NCMCMPR112_ActiveFlag,
                                    MI_Id = a.MI_Id,

                                }).Distinct().OrderByDescending(t => t.NCMCMPR112_Id).ToArray();

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public MC_Programs_112_DTO savedata(MC_Programs_112_DTO data)
        {
            try
            {

                if (data.NCMCMPR112_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_MC_Master_Programs_112_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCMCMPR112_Year == data.ASMAY_Id && t.NCMCMPR112_NoOfTeachersPartBos == data.countOfBOS && t.NCMCMPR112_NoOfTeachersAcu == data.countOfCouncil && t.NCMCMPR112_NoOfTeachersPartBos == data.NCMCMPR112_NoOfTeachersPartBos).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_MC_Master_Programs_112_DMO obj1 = new NAAC_MC_Master_Programs_112_DMO();

                        //obj1.NCMCMPR112_Id = data.NCMCMPR112_Id;
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCMCMPR112_Year = data.ASMAY_Id;
                        obj1.NCMCMPR112_NoOfTeachersPartBos = data.countOfBOS;
                        obj1.NCMCMPR112_NoOfTeachersAcu = data.countOfCouncil;
                        obj1.NCMCMPR112_CreatedDate = DateTime.Now;
                        obj1.NCMCMPR112_UpdatedDate = DateTime.Now;
                        obj1.NCMCMPR112_CreatedBy = data.UserId;
                        obj1.NCMCMPR112_UpdatedBy = data.UserId;
                        obj1.NCMCMPR112_ActiveFlag = true;

                        _GeneralContext.Add(obj1);

                        if (data.staffBosList.Length > 0)
                        {
                            for (int i = 0; i < data.staffBosList.Length; i++)
                            {
                                NAAC_MC_Master_Programs_112_Details_DMO obj2 = new NAAC_MC_Master_Programs_112_Details_DMO();
                                //obj2.NCMCMPR112D_Id = data.NCMCMPR112D_Id;
                                obj2.NCMCMPR112_Id = obj1.NCMCMPR112_Id;
                                obj2.HRME_Id = data.staffBosList[i].HRME_Id;
                                obj2.NCMCMPR112D_PrgType = data.flag_BOS;
                                obj2.NCMCMPR112D_CreatedBy = data.UserId;
                                obj2.NCMCMPR112D_UpdatedBy = data.UserId;
                                obj2.NCMCMPR112D_CreatedDate = DateTime.Now;
                                obj2.NCMCMPR112D_UpdatedDate = DateTime.Now;

                                _GeneralContext.Add(obj2);
                            }

                        }
                        if (data.staffCouncilList.Length > 0)
                        {
                            for (int i = 0; i < data.staffCouncilList.Length; i++)
                            {
                                NAAC_MC_Master_Programs_112_Details_DMO obj2 = new NAAC_MC_Master_Programs_112_Details_DMO();
                                //obj2.NCMCMPR112D_Id = data.NCMCMPR112D_Id;
                                obj2.NCMCMPR112_Id = obj1.NCMCMPR112_Id;
                                obj2.HRME_Id = data.staffCouncilList[i].empid;
                                obj2.NCMCMPR112D_PrgType = data.flag_COUN;
                                obj2.NCMCMPR112D_CreatedBy = data.UserId;
                                obj2.NCMCMPR112D_UpdatedBy = data.UserId;
                                obj2.NCMCMPR112D_CreatedDate = DateTime.Now;
                                obj2.NCMCMPR112D_UpdatedDate = DateTime.Now;

                                _GeneralContext.Add(obj2);
                            }

                        }
                        if (data.filelist.Length > 0)
                        {
                            for (int j = 0; j < data.filelist.Length; j++)
                            {
                                if (data.filelist[0].cfilepath != null)
                                {
                                    NAAC_MC_Master_Programs_112_Files_DMO obj3 = new NAAC_MC_Master_Programs_112_Files_DMO();

                                    obj3.NCMCMPR112DF_FileName = data.filelist[j].cfilename;
                                    obj3.NCMCMPR112DF_FileDesc = data.filelist[j].cfiledesc;
                                    obj3.NCMCMPR112DF_FilePath = data.filelist[j].cfilepath;
                                    obj3.NCMCMPR112_Id = obj1.NCMCMPR112_Id;
                                    obj3.MI_Id = data.MI_Id;
                                    obj3.NCMCMPR112DF_ActiveFlg = true;
                                    obj3.NCMCMPR112DF_CreatedBy = data.UserId;
                                    obj3.NCMCMPR112DF_UpdatedBy = data.UserId;
                                    obj3.NCMCMPR112DF_CreatedDate = DateTime.Now;
                                    obj3.NCMCMPR112DF_UpdatedDate = DateTime.Now;

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
                else if (data.NCMCMPR112_Id > 0)
                {
                    var duplicate = _GeneralContext.NAAC_MC_Master_Programs_112_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCMCMPR112_Year == data.ASMAY_Id && t.NCMCMPR112_NoOfTeachersPartBos == data.countOfBOS && t.NCMCMPR112_NoOfTeachersAcu == data.countOfCouncil && t.NCMCMPR112_NoOfTeachersPartBos == data.NCMCMPR112_NoOfTeachersPartBos).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _GeneralContext.NAAC_MC_Master_Programs_112_DMO.Where(t => t.NCMCMPR112_Id == data.NCMCMPR112_Id && t.MI_Id == data.MI_Id).Single();

                        update.NCMCMPR112_Year = data.ASMAY_Id;
                        update.NCMCMPR112_NoOfTeachersPartBos = data.countOfBOS;
                        update.NCMCMPR112_NoOfTeachersAcu = data.countOfCouncil;
                        update.NCMCMPR112_UpdatedDate = DateTime.Now;
                        update.NCMCMPR112_UpdatedBy = data.UserId;

                        _GeneralContext.Update(update);

                        var remove = _GeneralContext.NAAC_MC_Master_Programs_112_Details_DMO.Where(t => t.NCMCMPR112_Id == data.NCMCMPR112_Id).ToList();

                        if (remove.Count > 0)
                        {
                            foreach (var rowdata in remove)
                            {
                                _GeneralContext.Remove(rowdata);
                            }
                        }

                        if (data.staffBosList.Length > 0)
                        {
                            for (int i = 0; i < data.staffBosList.Length; i++)
                            {
                                NAAC_MC_Master_Programs_112_Details_DMO obj2 = new NAAC_MC_Master_Programs_112_Details_DMO();
                                //obj2.NCMCMPR112D_Id = data.NCMCMPR112D_Id;
                                obj2.NCMCMPR112_Id = update.NCMCMPR112_Id;
                                obj2.HRME_Id = data.staffBosList[i].HRME_Id;
                                obj2.NCMCMPR112D_PrgType = data.flag_BOS;
                                obj2.NCMCMPR112D_CreatedBy = data.UserId;
                                obj2.NCMCMPR112D_UpdatedBy = data.UserId;
                                obj2.NCMCMPR112D_CreatedDate = DateTime.Now;
                                obj2.NCMCMPR112D_UpdatedDate = DateTime.Now;

                                _GeneralContext.Add(obj2);
                            }

                        }
                        if (data.staffCouncilList.Length > 0)
                        {
                            for (int i = 0; i < data.staffCouncilList.Length; i++)
                            {
                                NAAC_MC_Master_Programs_112_Details_DMO obj2 = new NAAC_MC_Master_Programs_112_Details_DMO();
                                //obj2.NCMCMPR112D_Id = data.NCMCMPR112D_Id;
                                obj2.NCMCMPR112_Id = update.NCMCMPR112_Id;
                                obj2.HRME_Id = data.staffCouncilList[i].empid;
                                obj2.NCMCMPR112D_PrgType = data.flag_COUN;
                                obj2.NCMCMPR112D_CreatedBy = data.UserId;
                                obj2.NCMCMPR112D_UpdatedBy = data.UserId;
                                obj2.NCMCMPR112D_CreatedDate = DateTime.Now;
                                obj2.NCMCMPR112D_UpdatedDate = DateTime.Now;

                                _GeneralContext.Add(obj2);
                            }

                        }

                        var remove2 = _GeneralContext.NAAC_MC_Master_Programs_112_Files_DMO.Where(t => t.NCMCMPR112_Id == data.NCMCMPR112_Id).ToList();

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
                                    NAAC_MC_Master_Programs_112_Files_DMO obj3 = new NAAC_MC_Master_Programs_112_Files_DMO();

                                    obj3.NCMCMPR112DF_FileName = data.filelist[j].cfilename;
                                    obj3.NCMCMPR112DF_FileDesc = data.filelist[j].cfiledesc;
                                    obj3.NCMCMPR112DF_FilePath = data.filelist[j].cfilepath;
                                    obj3.NCMCMPR112_Id = update.NCMCMPR112_Id;
                                    obj3.MI_Id = data.MI_Id;
                                    obj3.NCMCMPR112DF_ActiveFlg = true;
                                    obj3.NCMCMPR112DF_CreatedBy = data.UserId;
                                    obj3.NCMCMPR112DF_UpdatedBy = data.UserId;
                                    obj3.NCMCMPR112DF_CreatedDate = DateTime.Now;
                                    obj3.NCMCMPR112DF_UpdatedDate = DateTime.Now;

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
        public MC_Programs_112_DTO editdata(MC_Programs_112_DTO data)
        {
            try
            {
                data.editdata = (from a in _GeneralContext.NAAC_MC_Master_Programs_112_DMO
                                    //from b in _GeneralContext.NAAC_MC_Master_Programs_112_Details_DMO
                                where (a.NCMCMPR112_Id == data.NCMCMPR112_Id && a.MI_Id == data.MI_Id/* && a.NCMCMPR112_Id == b.NCMCMPR112_Id*/)
                                select new MC_Programs_112_DTO
                                {
                                    NCMCMPR112_Id = a.NCMCMPR112_Id,
                                    MI_Id = a.MI_Id,
                                    NCMCMPR112_Year = a.NCMCMPR112_Year,
                                    NCMCMPR112_NoOfTeachersPartBos = a.NCMCMPR112_NoOfTeachersPartBos,
                                    NCMCMPR112_NoOfTeachersAcu = a.NCMCMPR112_NoOfTeachersAcu,
                                    NCMCMPR112_ActiveFlag=a.NCMCMPR112_ActiveFlag,
                                }).Distinct().ToArray();

                data.editdataBOS = (from b in _GeneralContext.NAAC_MC_Master_Programs_112_Details_DMO
                                   from a in _GeneralContext.HR_Master_Employee_DMO
                                 where (b.NCMCMPR112_Id == data.NCMCMPR112_Id && a.MI_Id == data.MI_Id
                                 && b.NCMCMPR112D_PrgType== "BOS" && a.HRME_Id==b.HRME_Id )
                                   select new MC_Programs_112_DTO {
                                       empname = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeCode = a.HRME_EmployeeCode,
                                       HRME_EmployeeOrder = a.HRME_EmployeeOrder,
                                       NCMCMPR112D_PrgType=b.NCMCMPR112D_PrgType,
                                       MI_Id=a.MI_Id,
                                   }).Distinct().ToArray();
                data.editdatacouncil = (from b in _GeneralContext.NAAC_MC_Master_Programs_112_Details_DMO
                                       from a in _GeneralContext.HR_Master_Employee_DMO
                                       where (b.NCMCMPR112_Id == data.NCMCMPR112_Id && a.MI_Id == data.MI_Id
                                       && b.NCMCMPR112D_PrgType == "Council" && a.HRME_Id == b.HRME_Id)
                                        select new MC_Programs_112_DTO
                                       {
                                           empnamecouncil = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                           empid = a.HRME_Id,
                                           empcode = a.HRME_EmployeeCode,
                                           emporder = a.HRME_EmployeeOrder,
                                           NCMCMPR112D_PrgType = b.NCMCMPR112D_PrgType,
                                           MI_Id = a.MI_Id,
                                       }).Distinct().ToArray();

                data.editFileslist = (from a in _GeneralContext.NAAC_MC_Master_Programs_112_Files_DMO
                                      where (a.NCMCMPR112_Id == data.NCMCMPR112_Id)
                                      select new MC_Programs_112_DTO
                                      {
                                          cfilename = a.NCMCMPR112DF_FileName,
                                          cfilepath = a.NCMCMPR112DF_FilePath,
                                          cfiledesc = a.NCMCMPR112DF_FileDesc,
                                          MI_Id = a.MI_Id,
                                      }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MC_Programs_112_DTO deactive_Y(MC_Programs_112_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_MC_Master_Programs_112_DMO.Where(t => t.NCMCMPR112_Id == data.NCMCMPR112_Id).SingleOrDefault();

                if (result.NCMCMPR112_ActiveFlag == true)
                {
                    result.NCMCMPR112_ActiveFlag = false;
                }
                else if (result.NCMCMPR112_ActiveFlag == false)
                {
                    result.NCMCMPR112_ActiveFlag = true;
                }

                result.NCMCMPR112_UpdatedDate = DateTime.Now;
                result.NCMCMPR112_UpdatedBy = data.UserId;

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
        public MC_Programs_112_DTO viewuploadflies(MC_Programs_112_DTO data)
        {
            try
            {
                data.viewuploadflies = (from t in _GeneralContext.NAAC_MC_Master_Programs_112_Files_DMO
                                        from b in _GeneralContext.NAAC_MC_Master_Programs_112_DMO
                                        where (t.NCMCMPR112_Id == data.NCMCMPR112_Id
                                        && t.NCMCMPR112_Id == b.NCMCMPR112_Id && b.MI_Id == data.MI_Id)
                                        select new MC_Programs_112_DTO
                                        {
                                            cfilename = t.NCMCMPR112DF_FileName,
                                            cfilepath = t.NCMCMPR112DF_FilePath,
                                            cfiledesc = t.NCMCMPR112DF_FileDesc,
                                            NCMCMPR112DF_Id = t.NCMCMPR112DF_Id,
                                            NCMCMPR112_Id = b.NCMCMPR112_Id,
                                            MI_Id = t.MI_Id,
                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public MC_Programs_112_DTO deleteuploadfile(MC_Programs_112_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_MC_Master_Programs_112_Files_DMO.Where(t => t.NCMCMPR112DF_Id == data.NCMCMPR112DF_Id).ToList();
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
                data.viewuploadflies = (from t in _GeneralContext.NAAC_MC_Master_Programs_112_Files_DMO
                                        from b in _GeneralContext.NAAC_MC_Master_Programs_112_DMO
                                        where (t.NCMCMPR112_Id == data.NCMCMPR112_Id
                                        && t.NCMCMPR112_Id == b.NCMCMPR112_Id && b.MI_Id == data.MI_Id)
                                        select new MC_Programs_112_DTO
                                        {
                                            cfilename = t.NCMCMPR112DF_FileName,
                                            cfilepath = t.NCMCMPR112DF_FilePath,
                                            cfiledesc = t.NCMCMPR112DF_FileDesc,
                                            NCMCMPR112DF_Id = t.NCMCMPR112DF_Id,
                                            NCMCMPR112_Id = b.NCMCMPR112_Id,
                                            MI_Id=t.MI_Id,
                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public MC_Programs_112_DTO StaffList_Boss(MC_Programs_112_DTO data)
        {
            try
            {
                data.staflist_boos = (from t in _GeneralContext.NAAC_MC_Master_Programs_112_Details_DMO
                                      from b in _GeneralContext.NAAC_MC_Master_Programs_112_DMO
                                      from a in _GeneralContext.HR_Master_Employee_DMO
                                      where (t.NCMCMPR112_Id == data.NCMCMPR112_Id
                                      && t.NCMCMPR112_Id == b.NCMCMPR112_Id && b.MI_Id == data.MI_Id
                                      && a.HRME_Id == t.HRME_Id && t.NCMCMPR112D_PrgType == "BOS")
                                      select new MC_Programs_112_DTO
                                      {
                                          empname = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                          HRME_Id = a.HRME_Id,
                                          HRME_EmployeeCode = a.HRME_EmployeeCode,
                                          NCMCMPR112D_Id = t.NCMCMPR112D_Id,
                                          NCMCMPR112_Id = b.NCMCMPR112_Id,
                                      }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public MC_Programs_112_DTO StaffList_Council(MC_Programs_112_DTO data)
        {
            try
            {
                data.staflist_council = (from t in _GeneralContext.NAAC_MC_Master_Programs_112_Details_DMO
                                      from b in _GeneralContext.NAAC_MC_Master_Programs_112_DMO
                                      from a in _GeneralContext.HR_Master_Employee_DMO
                                      where (t.NCMCMPR112_Id == data.NCMCMPR112_Id
                                      && t.NCMCMPR112_Id == b.NCMCMPR112_Id && b.MI_Id == data.MI_Id
                                      && a.HRME_Id == t.HRME_Id && t.NCMCMPR112D_PrgType == "Council")
                                      select new MC_Programs_112_DTO
                                      {
                                          empname = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                          HRME_Id = a.HRME_Id,
                                          HRME_EmployeeCode = a.HRME_EmployeeCode,
                                          NCMCMPR112D_Id = t.NCMCMPR112D_Id,
                                          NCMCMPR112_Id = b.NCMCMPR112_Id,
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
