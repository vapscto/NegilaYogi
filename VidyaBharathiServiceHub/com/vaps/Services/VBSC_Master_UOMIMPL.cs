using DataAccessMsSqlServerProvider.com.vapstech.VidyaBharathi;
using DomainModel.Model.com.vapstech.VidyaBharathi;
using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VidyaBharathiServiceHub.com.vaps.Services
{
    public class VBSC_Master_UOMIMPL : Interfaces.VBSC_Master_UOMInterface
    {
        //VidyaBharathiContext
        public VidyaBharathiContext _VidyaBharathiContext;
        public VBSC_Master_UOMIMPL(VidyaBharathiContext VidyaBharathiContext)
        {
            _VidyaBharathiContext = VidyaBharathiContext;
        }
        public VBSC_Master_UOMDTO loaddata(int id)
        {

            VBSC_Master_UOMDTO dto = new VBSC_Master_UOMDTO();
            try
            {
                 dto.get_uom = _VidyaBharathiContext.VBSC_Master_UOMDMO.Distinct().ToArray();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
              
            }
            return dto;

        }
        public VBSC_Master_UOMDTO savedetails(VBSC_Master_UOMDTO data)
        {
            try
            {
                if (data.VBCCMUOM_Id != 0)
                {
                    var res = _VidyaBharathiContext.VBSC_Master_UOMDMO.Where(t => t.VBCCMUOM_UOMName == data.VBCCMUOM_UOMName &&
                    t.VBCCMUOM_Id != data.VBCCMUOM_Id).ToList();

                    if (res.Count > 0)
                    {
                      data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _VidyaBharathiContext.VBSC_Master_UOMDMO.Single(t => t.VBCCMUOM_Id == data.VBCCMUOM_Id);
                        
                        result.VBCCMUOM_UOMName = data.VBCCMUOM_UOMName;
                        result.VBCCMUOM_ActiveFlag = true;
                        result.VBCCMUOM_UpdatedDate = DateTime.Now;
                        _VidyaBharathiContext.Update(result);
                        
                        var contactExists = _VidyaBharathiContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                  var res = _VidyaBharathiContext.VBSC_Master_UOMDMO.Where(t => t.VBCCMUOM_UOMName == data.VBCCMUOM_UOMName).ToList();

                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {

                        VBSC_Master_UOMDMO lvl = new VBSC_Master_UOMDMO();
                        
                        lvl.VBCCMUOM_UOMName = data.VBCCMUOM_UOMName;
                        lvl.VBCCMUOM_ActiveFlag = true;
                        lvl.VBCCMUOM_CreatedDate = DateTime.Now;
                        lvl.VBCCMUOM_UpdatedDate = DateTime.Now;
                        _VidyaBharathiContext.Add(lvl);
                        
                        var contactExists = _VidyaBharathiContext.SaveChanges();
                        if (contactExists > 0)
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

        
        public VBSC_Master_UOMDTO deactive(VBSC_Master_UOMDTO data)
        {
            try
            {
                var result = _VidyaBharathiContext.VBSC_Master_UOMDMO.Single(t => t.VBCCMUOM_Id == data.VBCCMUOM_Id);

                if (result.VBCCMUOM_ActiveFlag == true)
                {
                    result.VBCCMUOM_ActiveFlag = false;
                }
                else if (result.VBCCMUOM_ActiveFlag == false)
                {
                    result.VBCCMUOM_ActiveFlag = true;
                }
                result.VBCCMUOM_UpdatedDate = DateTime.Now;
                _VidyaBharathiContext.Update(result);
                int returnval = _VidyaBharathiContext.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

        //competition level

        public VBSC_Master_UOMDTO getloaddatalevel(int id)
        {

            VBSC_Master_UOMDTO dto = new VBSC_Master_UOMDTO();
            try
            {
               
                dto.get_levl = (from a in _VidyaBharathiContext.VBSC_Master_Competition_LevelDMO
                                from b in _VidyaBharathiContext.Organisation
                                where (a.MT_Id == b.MO_Id)
                                select new VBSC_Master_UOMDTO
                                {
                                    MO_Name = b.MO_Name,
                                    VBSCMCL_SportsCCFlg = a.VBSCMCL_SportsCCFlg,
                                    VBSCMCL_LevelFlg = a.VBSCMCL_LevelFlg,
                                    VBSCMCL_CompetitionLevel = a.VBSCMCL_CompetitionLevel,
                                    VBSCMCL_CLDesc = a.VBSCMCL_CLDesc,
                                    VBSCMCL_ActiveFlag = a.VBSCMCL_ActiveFlag,
                                    VBSCMCL_Id = a.VBSCMCL_Id,
                                    MT_Id = a.MT_Id,

                                }).Distinct().OrderByDescending(R => R.MT_Id).ToArray();

                var Master_trust = (from a in _VidyaBharathiContext.Organisation
                                    from b in _VidyaBharathiContext.Institute
                                    where (a.MO_Id == b.MO_Id && b.MI_Id == id && a.MO_ActiveFlag == 1)
                                    select new VBSC_MasterCompetition_CategoryDTO
                                    {
                                        MT_Id = a.MO_Id,
                                        MO_Name = a.MO_Name
                                    }
                                           ).FirstOrDefault();
                dto.MT_Id = Master_trust.MT_Id;


                dto.Master_trust = (from a in _VidyaBharathiContext.Organisation
                                    from b in _VidyaBharathiContext.Institute
                                    where (a.MO_Id == b.MO_Id && a.MO_ActiveFlag == 1 && b.MI_Id == id)
                                    select new VBSC_MasterCompetition_CategoryDTO
                                    {

                                        MT_Id = a.MO_Id,
                                        MO_Name = a.MO_Name

                                    }
                                 ).Distinct().OrderByDescending(R => R.MT_Id).ToArray();

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
               
            }
            return dto;

        }
        
        public VBSC_Master_UOMDTO savedetailslevel(VBSC_Master_UOMDTO data)
        {
            try
            {
                
                if (data.VBSCMCL_Id != 0)
                {
                    var res = _VidyaBharathiContext.VBSC_Master_Competition_LevelDMO.Where(t => t.VBSCMCL_CompetitionLevel == data.VBSCMCL_CompetitionLevel &&
                    t.VBSCMCL_Id != data.VBSCMCL_Id && t.MT_Id== data.MT_Id && t.VBSCMCL_SportsCCFlg==data.VBSCMCL_SportsCCFlg).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _VidyaBharathiContext.VBSC_Master_Competition_LevelDMO.Single(t => t.VBSCMCL_Id == data.VBSCMCL_Id);
                        result.MT_Id = data.MT_Id;
                        result.VBSCMCL_CompetitionLevel = data.VBSCMCL_CompetitionLevel;
                        result.VBSCMCL_CLDesc = data.VBSCMCL_CLDesc;
                        result.VBSCMCL_LevelFlg = data.VBSCMCL_LevelFlg;
                        result.VBSCMCL_LevelOrder = data.VBSCMCL_LevelOrder;
                        result.VBSCMCL_SportsCCFlg = data.VBSCMCL_SportsCCFlg;
                        result.VBSCMCL_ActiveFlag = true;
                        result.VBSCMCL_UpdatedDate = DateTime.Now;
                        _VidyaBharathiContext.Update(result);
                        
                        var contactExists = _VidyaBharathiContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    var res = _VidyaBharathiContext.VBSC_Master_Competition_LevelDMO.Where(t => t.VBSCMCL_CompetitionLevel == data.VBSCMCL_CompetitionLevel && t.MT_Id == data.MT_Id && t.VBSCMCL_SportsCCFlg == data.VBSCMCL_SportsCCFlg).ToList();

                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        VBSC_Master_Competition_LevelDMO lvl = new VBSC_Master_Competition_LevelDMO();
                        lvl.MT_Id = data.MT_Id;
                        lvl.VBSCMCL_CompetitionLevel = data.VBSCMCL_CompetitionLevel;
                        lvl.VBSCMCL_CLDesc = data.VBSCMCL_CLDesc;
                        lvl.VBSCMCL_LevelFlg = data.VBSCMCL_LevelFlg;
                        lvl.VBSCMCL_LevelOrder = data.VBSCMCL_LevelOrder;
                        lvl.VBSCMCL_SportsCCFlg = data.VBSCMCL_SportsCCFlg;
                        lvl.VBSCMCL_ActiveFlag = true;
                        lvl.VBSCMCL_CreatedDate = DateTime.Now;
                        lvl.VBSCMCL_UpdatedDate = DateTime.Now;
                        _VidyaBharathiContext.Add(lvl);
                        
                        var contactExists = _VidyaBharathiContext.SaveChanges();
                        if (contactExists > 0)
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

        public VBSC_Master_UOMDTO deactivelevel(VBSC_Master_UOMDTO data)
        {
            try
            {
                var result = _VidyaBharathiContext.VBSC_Master_Competition_LevelDMO.Single(t => t.VBSCMCL_Id == data.VBSCMCL_Id);

                if (result.VBSCMCL_ActiveFlag == true)
                {
                    result.VBSCMCL_ActiveFlag = false;
                }
                else if (result.VBSCMCL_ActiveFlag == false)
                {
                    result.VBSCMCL_ActiveFlag = true;
                }
                result.VBSCMCL_UpdatedDate = DateTime.Now;
                _VidyaBharathiContext.Update(result);
                int returnval = _VidyaBharathiContext.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        
    }
}
