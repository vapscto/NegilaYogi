using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.VidyaBharathi;
using DomainModel.Model;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Scholorship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{

    public class IVRM_Master_ViddyBharthiIMPL : Interfaces.IVRM_Master_ViddyBharthiInterface
    {
        public DomainModelMsSqlServerContext _db;
        //public VidyaBharathiContext _Context;
        ILogger<IVRM_Master_ViddyBharthiIMPL> _acdimpl;
        public IVRM_Master_ViddyBharthiIMPL(ILogger<IVRM_Master_ViddyBharthiIMPL> acdimpl, DomainModelMsSqlServerContext db)
        {
            // _Context = context; VidyaBharathiContext context
            _acdimpl = acdimpl;
            _db = db;
        }
        public ScholorshipMasterDTO getallDetails(ScholorshipMasterDTO data)
        {
            try
            {
                data.Country = _db.country.Distinct().OrderByDescending(d => d.IVRMMC_Id).ToArray();

                data.CountryDetails = _db.country.Where(R => R.IVRMMC_ActiveFlag == true).Distinct().OrderByDescending(d => d.IVRMMC_Id).ToArray();
              
                data.statelist = (from a in _db.country
                                  from b in _db.State
                                  where (a.IVRMMC_Id == b.IVRMMC_Id && a.IVRMMC_ActiveFlag == true)
                                  select new ScholorshipMasterDTO
                                  {
                                      IVRMMS_Name = b.IVRMMS_Name,
                                      IVRMMC_CountryName = a.IVRMMC_CountryName,
                                      IVRMMS_Code = b.IVRMMS_Code,
                                      IVRMMC_Id = b.IVRMMC_Id,
                                      IVRMMS_Id = b.IVRMMS_Id,
                                      IVRMMS_ActiveFlag = b.IVRMMS_ActiveFlag,

                                  }
                                 ).Distinct().OrderByDescending(R => R.IVRMMS_Id).ToArray();

                //ScholorshipDitictDTO
                data.distictlist = (from a in _db.DistrictDMO
                                    from b in _db.State
                                    from c in _db.country
                                    where (a.IVRMMS_Id == b.IVRMMS_Id && b.IVRMMS_ActiveFlag == true && b.IVRMMC_Id == c.IVRMMC_Id && c.IVRMMC_ActiveFlag == true)
                                    select new ScholorshipDitictDTO
                                    {
                                        IVRMMS_Name = b.IVRMMS_Name,
                                        IVRMMD_Name = a.IVRMMD_Name,
                                        IVRMMC_CountryName = c.IVRMMC_CountryName,
                                        IVRMMD_Code = a.IVRMMD_Code,
                                        IVRMMS_Id = a.IVRMMS_Id,
                                        IVRMMD_Id = a.IVRMMD_Id,


                                        IVRMMD_ActiveFlag = a.IVRMMD_ActiveFlag,
                                        IVRMMC_Id = c.IVRMMC_Id,


                                    }
                                ).Distinct().OrderByDescending(R => R.IVRMMD_Id).ToArray();
                data.talukalist = (from a in _db.country
                                   from b in _db.State
                                   from c in _db.DistrictDMO
                                   from d in _db.TalukDMO
                                   where (a.IVRMMC_Id == b.IVRMMC_Id && b.IVRMMS_Id == c.IVRMMS_Id && a.IVRMMC_ActiveFlag == true && b.IVRMMS_ActiveFlag == true && d.IVRMMD_Id == c.IVRMMD_Id && c.IVRMMD_ActiveFlag == true)
                                   select new ScholorshipDitictDTO
                                   {
                                       IVRMMS_Name = b.IVRMMS_Name,
                                       IVRMMC_CountryName = a.IVRMMC_CountryName,
                                       IVRMMD_Name = c.IVRMMD_Name,
                                       IVRMMC_Id = b.IVRMMC_Id,
                                       IVRMMS_Id = b.IVRMMS_Id,
                                       IVRMMD_Id = c.IVRMMD_Id,
                                       IVRMMT_Id = d.IVRMMT_Id,
                                       IVRMMT_ActiveFlag = d.IVRMMT_ActiveFlag,
                                       IVRMMT_Name = d.IVRMMT_Name,



                                   }
                         ).Distinct().OrderByDescending(R => R.IVRMMT_Id).ToArray();
               
              
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.returnval = "admin";
            }

            return data;
        }

        public ScholorshipMasterDTO savecountry(ScholorshipMasterDTO data)
        {
            try
            {
                if (data.IVRMMC_Id > 0)
                {
                    var duplicate = _db.country.Where(R => R.IVRMMC_CountryName == data.IVRMMC_CountryName && R.IVRMMC_Id != data.IVRMMC_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        var obj = _db.country.Where(R => R.IVRMMC_Id == data.IVRMMC_Id).FirstOrDefault();
                        obj.IVRMMC_CountryName = data.IVRMMC_CountryName;
                        obj.IVRMMC_CountryCode = data.IVRMMC_CountryCode;
                        obj.IVRMMC_MobileNoLength = data.IVRMMC_MobileNoLength;
                        obj.IVRMMC_Default = 0;
                        obj.IVRMMC_Currency = data.IVRMMC_Currency;
                        obj.UpdatedDate = DateTime.Now;
                        obj.IVRMMC_CountryPhCode = data.IVRMMC_CountryPhCode;
                        obj.IVRMMC_Nationality = data.IVRMMC_Nationality;
                        obj.IVRMMC_UpdatedBy = data.userId;
                        obj.IVRMMC_ActiveFlag = true;
                        _db.Update(obj);
                        int i = _db.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = "update";
                        }
                        else
                        {
                            data.returnval = "notupdate";
                        }
                    }
                }
                else
                {
                    var duplicate = _db.country.Where(R => R.IVRMMC_CountryName == data.IVRMMC_CountryName).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        Country obj = new Country();
                        obj.IVRMMC_CountryName = data.IVRMMC_CountryName;
                        obj.IVRMMC_CountryCode = data.IVRMMC_CountryCode;
                        obj.IVRMMC_MobileNoLength = data.IVRMMC_MobileNoLength;
                        obj.IVRMMC_Default = 0;
                        obj.IVRMMC_Currency = data.IVRMMC_Currency;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        obj.IVRMMC_CountryPhCode = data.IVRMMC_CountryPhCode;
                        obj.IVRMMC_Nationality = data.IVRMMC_Nationality;
                        obj.IVRMMC_CreatedBy = data.userId;
                        obj.IVRMMC_UpdatedBy = data.userId;
                        obj.IVRMMC_ActiveFlag = true;
                        _db.Add(obj);
                        int i = _db.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = "save";
                        }
                        else
                        {
                            data.returnval = "notsave";
                        }
                    }
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.returnval = "admin";
            }

            return data;
        }
        public ScholorshipStateDTO savestate(ScholorshipStateDTO data)
        {
            try
            {
                if (data.IVRMMS_Id > 0)
                {
                    var duplicate = _db.State.Where(R => R.IVRMMS_Name == data.IVRMMS_Name && R.IVRMMC_Id == data.IVRMMC_Id && R.IVRMMS_Id != data.IVRMMS_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        var obj = _db.State.Where(R => R.IVRMMS_Id == data.IVRMMS_Id).FirstOrDefault();
                        obj.IVRMMS_Name = data.IVRMMS_Name;
                        obj.IVRMMS_Code = data.IVRMMS_Code;
                        obj.IVRMMC_Id = data.IVRMMC_Id;
                        obj.IVRMMS_UpdatedBy = data.userId;

                        _db.Update(obj);
                        int i = _db.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = "update";
                        }
                        else
                        {
                            data.returnval = "notupdate";
                        }
                    }
                }
                else
                {
                    var duplicate = _db.State.Where(R => R.IVRMMS_Name == data.IVRMMS_Name && R.IVRMMC_Id == data.IVRMMC_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        State obj = new State();
                        obj.IVRMMS_Name = data.IVRMMS_Name;
                        obj.IVRMMS_Default = 0;
                        obj.IVRMMS_Code = data.IVRMMS_Code;
                        obj.IVRMMC_Id = data.IVRMMC_Id;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        obj.IVRMMS_CreatedBy = data.userId;
                        obj.IVRMMS_UpdatedBy = data.userId;
                        obj.IVRMMS_ActiveFlag = true;

                        _db.Add(obj);
                        int i = _db.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = "save";
                        }
                        else
                        {
                            data.returnval = "notsave";
                        }
                    }

                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.returnval = "admin";
            }

            return data;
        }
        public ScholorshipDitictDTO onchnagestate(ScholorshipDitictDTO data)
        {
            try
            {
                data.statedetails = _db.State.Where(R => R.IVRMMC_Id == data.IVRMMC_Id && R.IVRMMS_ActiveFlag == true).Distinct().ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.returnval = "admin";
            }

            return data;
        }
        public ScholorshipDitictDTO saveDistrict(ScholorshipDitictDTO data)
        {
            try
            {
                if (data.IVRMMD_Id > 0)
                {
                    var duplicate = _db.DistrictDMO.Where(R => R.IVRMMD_Name == data.IVRMMD_Name && R.IVRMMS_Id == data.IVRMMS_Id && R.IVRMMD_Id != data.IVRMMD_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        var obj = _db.DistrictDMO.Where(R => R.IVRMMD_Id == data.IVRMMD_Id).FirstOrDefault();
                        obj.IVRMMD_Name = data.IVRMMD_Name;
                        obj.IVRMMD_Code = data.IVRMMD_Code;
                        obj.IVRMMS_Id = data.IVRMMS_Id;
                        obj.IVRMMD_UpdatedDate = DateTime.Now;
                        obj.IVRMMD_CreatedBy = data.userId;
                        obj.IVRMMD_UpdatedBy = data.userId;

                        //_db.Update(obj);
                        int i = _db.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = "update";
                        }
                        else
                        {
                            data.returnval = "notupdate";
                        }
                    }
                }
                else
                {
                    var duplicate = _db.DistrictDMO.Where(R => R.IVRMMD_Name == data.IVRMMD_Name && R.IVRMMS_Id == data.IVRMMS_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        DistrictDMO obj = new DistrictDMO();
                        obj.IVRMMD_Name = data.IVRMMD_Name;

                        obj.IVRMMD_Code = data.IVRMMD_Code;
                        obj.IVRMMS_Id = data.IVRMMS_Id;
                        obj.IVRMMD_CreatedDate = DateTime.Now;
                        obj.IVRMMD_UpdatedDate = DateTime.Now;
                        obj.IVRMMD_CreatedBy = data.userId;
                        obj.IVRMMD_UpdatedBy = data.userId;
                        obj.IVRMMD_ActiveFlag = true;

                        _db.Add(obj);
                        int i = _db.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = "save";
                        }
                        else
                        {
                            data.returnval = "notsave";
                        }
                    }

                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.returnval = "admin";
            }

            return data;
        }
        public ScholorshipTalukaDTO savetaluka(ScholorshipTalukaDTO data)
        {
            try
            {
                if (data.IVRMMT_Id > 0)
                {
                    var duplicate = _db.TalukDMO.Where(R => R.IVRMMT_Name == data.IVRMMT_Name && R.IVRMMD_Id == data.IVRMMD_Id && R.IVRMMT_Id != data.IVRMMT_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        var obj = _db.TalukDMO.Where(R => R.IVRMMT_Id == data.IVRMMT_Id).FirstOrDefault();
                        obj.IVRMMT_Name = data.IVRMMT_Name;
                        obj.IVRMMD_Id = data.IVRMMD_Id;
                        obj.IVRMMT_UpdatedDate = DateTime.Now;
                        obj.IVRMMT_UpdatedBy = data.userId;
                        _db.Update(obj);
                        int i = _db.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = "update";
                        }
                        else
                        {
                            data.returnval = "notupdate";
                        }
                    }
                }
                else
                {
                    var duplicate = _db.TalukDMO.Where(R => R.IVRMMT_Name == data.IVRMMT_Name && R.IVRMMD_Id == data.IVRMMD_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        TalukDMO obj = new TalukDMO();
                        obj.IVRMMT_Name = data.IVRMMT_Name;
                        obj.IVRMMD_Id = data.IVRMMD_Id;
                        obj.IVRMMT_CreatedDate = DateTime.Now;
                        obj.IVRMMT_UpdatedDate = DateTime.Now;
                        obj.IVRMMT_CreatedBy = data.userId;
                        obj.IVRMMT_UpdatedBy = data.userId;
                        obj.IVRMMT_ActiveFlag = true;

                        _db.Add(obj);
                        int i = _db.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = "save";
                        }
                        else
                        {
                            data.returnval = "notsave";
                        }
                    }

                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.returnval = "admin";
            }

            return data;
        }
        public ScholorshipMasterDTO deactivateCountry(ScholorshipMasterDTO data)
        {
            try
            {
                if (data.IVRMMC_Id > 0)
                {
                    var deactive = _db.country.Where(R => R.IVRMMC_Id == data.IVRMMC_Id).FirstOrDefault();
                    if (deactive.IVRMMC_ActiveFlag == true)
                    {
                        deactive.IVRMMC_ActiveFlag = false;
                    }
                    else
                    {
                        deactive.IVRMMC_ActiveFlag = true;
                    }
                    deactive.UpdatedDate = DateTime.Now;
                    deactive.IVRMMC_UpdatedBy = data.userId;
                    _db.Update(deactive);
                    int r = _db.SaveChanges();
                    if (r > 0)
                    {
                        data.returnval = "activate";
                    }
                    else
                    {
                        data.returnval = "notactivate";
                    }
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.returnval = "admin";
            }

            return data;
        }
        public ScholorshipStateDTO deactivestate(ScholorshipStateDTO data)
        {
            try
            {
                if (data.IVRMMS_Id > 0)
                {
                    var deactive = _db.State.Where(R => R.IVRMMS_Id == data.IVRMMS_Id).FirstOrDefault();
                    if (deactive.IVRMMS_ActiveFlag == true)
                    {
                        deactive.IVRMMS_ActiveFlag = false;
                    }
                    else
                    {
                        deactive.IVRMMS_ActiveFlag = true;
                    }
                    deactive.UpdatedDate = DateTime.Now;
                    deactive.IVRMMS_UpdatedBy = data.userId;
                    _db.Update(deactive);
                    int r = _db.SaveChanges();
                    if (r > 0)
                    {
                        data.returnval = "activate";
                    }
                    else
                    {
                        data.returnval = "notactivate";
                    }
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.returnval = "admin";
            }

            return data;
        }
        public ScholorshipDitictDTO deactivedistict(ScholorshipDitictDTO data)
        {
            try
            {
                if (data.IVRMMD_Id > 0)
                {
                    var deactive = _db.DistrictDMO.Where(R => R.IVRMMD_Id == data.IVRMMD_Id).FirstOrDefault();
                    if (deactive.IVRMMD_ActiveFlag == true)
                    {
                        deactive.IVRMMD_ActiveFlag = false;
                    }
                    else
                    {
                        deactive.IVRMMD_ActiveFlag = true;
                    }
                    deactive.IVRMMD_UpdatedDate = DateTime.Now;
                    deactive.IVRMMD_UpdatedBy = data.userId;
                    _db.Update(deactive);
                    int r = _db.SaveChanges();
                    if (r > 0)
                    {
                        data.returnval = "activate";
                    }
                    else
                    {
                        data.returnval = "notactivate";
                    }
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.returnval = "admin";
            }

            return data;
        }
        //deactivetaluka
        public ScholorshipTalukaDTO deactivetaluka(ScholorshipTalukaDTO data)
        {
            try
            {
                if (data.IVRMMT_Id > 0)
                {
                    var deactive = _db.TalukDMO.Where(R => R.IVRMMT_Id == data.IVRMMT_Id).FirstOrDefault();
                    if (deactive.IVRMMT_ActiveFlag == true)
                    {
                        deactive.IVRMMT_ActiveFlag = false;
                    }
                    else
                    {
                        deactive.IVRMMT_ActiveFlag = true;
                    }
                    deactive.IVRMMT_UpdatedDate = DateTime.Now;
                    deactive.IVRMMT_UpdatedBy = data.userId;


                    _db.Update(deactive);
                    int r = _db.SaveChanges();
                    if (r > 0)
                    {
                        data.returnval = "activate";
                    }
                    else
                    {
                        data.returnval = "notactivate";
                    }
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.returnval = "admin";
            }

            return data;
        }
    }
}
