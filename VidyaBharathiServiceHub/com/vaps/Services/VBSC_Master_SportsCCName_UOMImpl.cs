using AutoMapper;
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
    public class VBSC_Master_SportsCCName_UOMImpl : Interfaces.VBSC_Master_SportsCCName_UOMInterface
    {
        public VidyaBharathiContext _VidyaBharathiContext;
        public VBSC_Master_SportsCCName_UOMImpl(VidyaBharathiContext VidyaBharathiContext)
        {
            _VidyaBharathiContext = VidyaBharathiContext;
        }

        public VBSC_Master_SportsCCName_UOMDTO getDetails(VBSC_Master_SportsCCName_UOMDTO data)
        {
            try
            {
                var sportsccName = _VidyaBharathiContext.VBSC_Master_SportsCCNameDMO.ToList();
                if (sportsccName.Count > 0)
                {
                    data.sportsCCNameList = sportsccName.ToArray();
                }

             
                data.uomList = _VidyaBharathiContext.VBSC_Master_UOMDMO.Distinct().ToArray();

                data.save = (from a in _VidyaBharathiContext.VBSC_Master_SportsCCName_UOMDMO
                             from b in _VidyaBharathiContext.VBSC_Master_UOMDMO
                             from c in _VidyaBharathiContext.VBSC_Master_SportsCCNameDMO
                             where ( a.VBSCMSCC_Id == c.VBSCMSCC_Id && a.VBCCMUOM_Id == b.VBCCMUOM_Id)
                              select new VBSC_Master_SportsCCName_UOMDTO
                              {
                                  uom =b.VBCCMUOM_UOMName,
                                  sport = c.VBSCMSCC_SportsCCName,
                                  VBSCMSCCUOM_Id = a.VBSCMSCCUOM_Id,
                                  VBSCMSCCUOM_ActiveFlag=a.VBSCMSCCUOM_ActiveFlag
                              }).Distinct().ToArray();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

      
        public VBSC_Master_SportsCCName_UOMDTO saveRecord(VBSC_Master_SportsCCName_UOMDTO data)
        {
            try
            {

                if (data.VBSCMSCCUOM_Id != 0)
                {
                    var res = _VidyaBharathiContext.VBSC_Master_SportsCCName_UOMDMO.Where(t => t.VBCCMUOM_Id==data.VBCCMUOM_Id && t.VBSCMSCC_Id==data.VBSCMSCC_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnVal = "duplicate";
                    }
                    else
                    {
                        var result = _VidyaBharathiContext.VBSC_Master_SportsCCName_UOMDMO.Single(t => t.VBSCMSCCUOM_Id == data.VBSCMSCCUOM_Id);
                        result.VBSCMSCC_Id = data.VBSCMSCC_Id;
                        result.VBCCMUOM_Id = data.VBCCMUOM_Id;
                        result.VBSCMSCCUOM_ActiveFlag = true;
                        result.VBSCMSCCUOM_CreatedDate = DateTime.Now;
                        result.VBSCMSCCUOM_UpdatedDate = DateTime.Now;
                        _VidyaBharathiContext.Update(result);

                        var contactExists = _VidyaBharathiContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnVal = "updated";
                        }
                        else
                        {
                            data.returnVal = "updateFailed";
                        }
                    }
                }
                else
                {
                    var res = _VidyaBharathiContext.VBSC_Master_SportsCCName_UOMDMO.Where(t => t.VBCCMUOM_Id==data.VBCCMUOM_Id && t.VBSCMSCC_Id==data.VBSCMSCC_Id).ToList();

                    if (res.Count > 0)
                    {
                        data.returnVal = "duplicate";
                    }
                    else
                    {
                        VBSC_Master_SportsCCName_UOMDMO lvl = new VBSC_Master_SportsCCName_UOMDMO();
                        lvl.VBCCMUOM_Id = data.VBCCMUOM_Id;
                        lvl.VBSCMSCC_Id = data.VBSCMSCC_Id;
                        lvl.VBSCMSCCUOM_ActiveFlag = true;
                        lvl.VBSCMSCCUOM_CreatedDate = DateTime.Now;
                        lvl.VBSCMSCCUOM_UpdatedDate = DateTime.Now;
                        _VidyaBharathiContext.Add(lvl);

                        var contactExists = _VidyaBharathiContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnVal = "saved";
                           
                        }
                        else
                        {
                            data.returnVal = "savingFailed";
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



        public VBSC_Master_SportsCCName_UOMDTO EditDetails(int id)
        {
            VBSC_Master_SportsCCName_UOMDTO resp = new VBSC_Master_SportsCCName_UOMDTO();
            try
            {
                resp.editDetails = _VidyaBharathiContext.VBSC_Master_SportsCCName_UOMDMO.Where(d => d.VBSCMSCCUOM_Id == id).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return resp;
        }

        public VBSC_Master_SportsCCName_UOMDTO deactivate(VBSC_Master_SportsCCName_UOMDTO obj)
        {
            try
            {
                var query = _VidyaBharathiContext.VBSC_Master_SportsCCName_UOMDMO.Single(d => d.VBSCMSCCUOM_Id == obj.VBSCMSCCUOM_Id);
                if (query.VBSCMSCCUOM_ActiveFlag == true)
                {
                    query.VBSCMSCCUOM_ActiveFlag = false;
                }
                else if (query.VBSCMSCCUOM_ActiveFlag == false)
                {
                    query.VBSCMSCCUOM_ActiveFlag = true;
                }

                query.VBSCMSCCUOM_UpdatedDate = DateTime.Now;
                _VidyaBharathiContext.Update(query);
                int s = _VidyaBharathiContext.SaveChanges();
                if (s > 0)
                {
                    obj.retval = true;
                }
                else
                {
                    obj.retval = false;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }

    }
}

