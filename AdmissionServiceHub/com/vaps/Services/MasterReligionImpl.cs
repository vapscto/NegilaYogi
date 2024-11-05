using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.admission;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.Extensions.Logging;


namespace AdmissionServiceHub.com.vaps.Services
{
    public class MasterReligionImpl : Interfaces.MasterReligionInterface
    {
        private static ConcurrentDictionary<string, MasterReligionDTO> _login =
           new ConcurrentDictionary<string, MasterReligionDTO>();

        public DomainModelMsSqlServerContext _db;
        ILogger<MasterReligionImpl> _mstimpl;
        public MasterReligionImpl(DomainModelMsSqlServerContext db, ILogger<MasterReligionImpl> mstimpl)
        {
            _db = db;
            _mstimpl = mstimpl;
        }
        public MasterReligionDTO getdetails()
        {
            MasterReligionDTO rel = new MasterReligionDTO();
            try
            {
                _mstimpl.LogInformation("AdmissionServiceHub.com.vaps.Services/MasterReligionImpl/getdetails");
                List<MasterReligionDMO> religion = new List<MasterReligionDMO>();
                religion = _db.masterReligion.OrderByDescending(d => d.UpdatedDate).ToList();
                rel.religionList = religion.ToArray();
            }
            catch (Exception ex)
            {
                _mstimpl.LogError(ex.Message);
                _mstimpl.LogDebug(ex.Message);
                _mstimpl.LogTrace(ex.Message);
            }
            return rel;
        }
        public MasterReligionDTO saveData(MasterReligionDTO data)
        {
            try
            {
                _mstimpl.LogInformation("AdmissionServiceHub.com.vaps.Services/MasterReligionImpl/saveData");
                MasterReligionDMO MM = Mapper.Map<MasterReligionDMO>(data);
                if (MM.IVRMMR_Id > 0)
                {
                    var isDuplicate = _db.masterReligion.Where(d => d.IVRMMR_Name == data.IVRMMR_Name && d.IVRMMR_Id !=data.IVRMMR_Id).ToList();
                    if (isDuplicate.Count > 0)
                    {
                        data.message = "Master Religion Already Exists";
                        return data;
                    }
                    else
                    {
                        var result = _db.masterReligion.Single(t => t.IVRMMR_Id == MM.IVRMMR_Id);
                        result.IVRMMR_Name = MM.IVRMMR_Name;
                        result.CreatedDate = result.CreatedDate;
                        result.Is_Active = result.Is_Active;
                        result.UpdatedDate = DateTime.Now;
                        _db.Update(result);
                        int flag = _db.SaveChanges();
                        if (flag == 1)
                        {
                            data.returnval = true;
                            data.operation = "Record Updated Successfully.";
                        }
                        else
                        {
                            data.returnval = false;
                            data.operation = "Failed to Update Record.";
                        }
                    }

                }
                else
                {
                    int Count = _db.masterReligion.Where(t => t.IVRMMR_Name.Equals(MM.IVRMMR_Name)).Count();

                    if (Count > 0)
                    {
                        data.message = "Master Religion Already Exists";
                        return data;
                    }
                    else
                    {
                        MM.CreatedDate = DateTime.Now;
                        MM.Is_Active = true;
                        MM.UpdatedDate = DateTime.Now;
                        _db.Add(MM);
                        int flag = _db.SaveChanges();
                        if (flag == 1)
                        {
                            data.returnval = true;
                            data.operation = "Record Saved Successfully.";
                        }
                        else
                        {
                            data.returnval = false;
                            data.operation = "Failed to Save Record.";
                        }

                    }



                }

                List<MasterReligionDMO> religion = new List<MasterReligionDMO>();
                religion = _db.masterReligion.OrderByDescending(d => d.UpdatedDate).ToList();
                data.religionList = religion.ToArray();
            }
            catch (Exception ex)
            {
                _mstimpl.LogError(ex.Message);
                _mstimpl.LogDebug(ex.Message);
                _mstimpl.LogTrace(ex.Message);
            }
            return data;
        }
        public MasterReligionDTO Edit(int id)
        {
            MasterReligionDTO rel = new MasterReligionDTO();
            try
            {
                _mstimpl.LogInformation("AdmissionServiceHub.com.vaps.Services/MasterReligionImpl/Edit");
                List<MasterReligionDMO> relig = new List<MasterReligionDMO>();
                relig = _db.masterReligion.AsNoTracking().Where(t => t.IVRMMR_Id == id).ToList();
                rel.religionList = relig.ToArray();
            }
            catch (Exception ee)
            {
                _mstimpl.LogError(ee.Message);
                _mstimpl.LogDebug(ee.Message);
                _mstimpl.LogTrace(ee.Message);
            }
            return rel;
        }
        public MasterReligionDTO deleterec(int id)
        {
            MasterReligionDTO org = new MasterReligionDTO();
            List<MasterReligionDMO> lorg = new List<MasterReligionDMO>(); // Mapper.Map<Organisation>(org);

            try
            {
                _mstimpl.LogInformation("AdmissionServiceHub.com.vaps.Services/MasterReligionImpl/deleterec");
                lorg = _db.masterReligion.Where(t => t.IVRMMR_Id == id).ToList();

                if (lorg.Any())
                {
                    _db.Remove(lorg.ElementAt(0));
                    var flag = _db.SaveChanges();
                    if (flag == 1)
                    {
                        org.returnval = true;
                    }
                    else
                    {
                        org.returnval = false;
                    }
                }
                List<MasterReligionDMO> allorganisation = new List<MasterReligionDMO>();
                allorganisation = _db.masterReligion.OrderByDescending(d => d.UpdatedDate).ToList();
                org.religionList = allorganisation.ToArray();
            }
            catch (Exception ee)
            {
                org.message = "You Can Not Delete This Record It Is Already Mapped With Student";
                _mstimpl.LogTrace(ee.Message);
                _mstimpl.LogDebug(ee.Message);
                _mstimpl.LogError(ee.Message);
            }

            return org;
        }
        public MasterReligionDTO deactivate(MasterReligionDTO dto)
        {
            try
            {
                MasterReligionDMO enq = Mapper.Map<MasterReligionDMO>(dto);
                _mstimpl.LogInformation("AdmissionServiceHub.com.vaps.Services/MasterReligionImpl/deactivate");

                if (enq.IVRMMR_Id > 0)
                {
                    var check_religinassign = (from a in _db.Adm_M_Student
                                               from b in _db.masterReligion
                                               where (a.IVRMMR_Id == b.IVRMMR_Id && a.IVRMMR_Id == dto.IVRMMR_Id && b.Is_Active == true)
                                               select new MasterReligionDTO
                                               {
                                                   IVRMMR_Id = b.IVRMMR_Id
                                               }
                                                ).ToList();

                    if (check_religinassign.Count > 0)
                    {
                        dto.returnval = true;
                        
                        dto.message = "You Can Not Deactivate This Record It Is Already Mapped With Student";
                    }
                    else
                    {
                        var result = _db.masterReligion.Single(t => t.IVRMMR_Id == enq.IVRMMR_Id);
                        if (result.Is_Active == true)
                        {
                            result.Is_Active = false;
                        }
                        else
                        {
                            result.Is_Active = true;
                        }
                        result.CreatedDate = result.CreatedDate;
                        result.UpdatedDate = DateTime.Now;
                        _db.Update(result);
                        var flag = _db.SaveChanges();
                        if (flag == 1)
                        {
                            dto.returnval = true;

                            if (result.Is_Active == true)
                            {
                                dto.message = "Religion Activated Successfully.";
                            }
                            else if (result.Is_Active == false)
                            {
                                dto.message = "Religion Deactivated Successfully.";
                            }
                        }
                        else
                        {
                            dto.returnval = false;
                        }

                        List<MasterReligionDMO> allorganisation = new List<MasterReligionDMO>();
                        allorganisation = _db.masterReligion.OrderByDescending(d => d.UpdatedDate).ToList();
                        dto.religionList = allorganisation.ToArray();
                    }
                }
            }
            catch (Exception ee)
            {
                _mstimpl.LogTrace(ee.Message);
                _mstimpl.LogDebug(ee.Message);
                _mstimpl.LogError(ee.Message);
            }
            return dto;
        }
        public MasterReligionDTO searchByColumn(MasterReligionDTO dto)
        {
            try
            {
                _mstimpl.LogInformation("AdmissionServiceHub.com.vaps.Services/MasterReligionImpl/searchByColumn");
                if (dto.SearchColumn == "1")
                {
                    List<MasterReligionDMO> allorganisation = new List<MasterReligionDMO>();
                    allorganisation = _db.masterReligion.OrderByDescending(d => d.UpdatedDate).Where(s => s.IVRMMR_Name == dto.EnteredData).ToList();
                    dto.religionList = allorganisation.ToArray();
                }
                else if (dto.SearchColumn == "2")
                {
                    try
                    {
                        DateTime date = DateTime.ParseExact(dto.EnteredData, "dd/MM/yyyy",
                                   CultureInfo.InvariantCulture);
                        List<MasterReligionDMO> allorganisation = new List<MasterReligionDMO>();
                        allorganisation = _db.masterReligion.OrderByDescending(d => d.UpdatedDate).Where(s => s.CreatedDate.Value.Date == Convert.ToDateTime(date.ToString("yyyy-MM-dd"))).ToList();
                        dto.religionList = allorganisation.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        dto.message = "Please Enter date in dd/MM/yyyy format";
                        List<MasterReligionDMO> allorganisation = new List<MasterReligionDMO>();
                        allorganisation = _db.masterReligion.OrderByDescending(d => d.UpdatedDate).ToList();
                        dto.religionList = allorganisation.ToArray();
                    }

                }
                else
                {
                    List<MasterReligionDMO> allorganisation = new List<MasterReligionDMO>();
                    allorganisation = _db.masterReligion.OrderByDescending(d => d.UpdatedDate).ToList();
                    dto.religionList = allorganisation.ToArray();
                }

            }
            catch (Exception e)
            {
                _mstimpl.LogTrace(e.Message);
                _mstimpl.LogDebug(e.Message);
                _mstimpl.LogError(e.Message);
            }
            return dto;
        }
    }
}
