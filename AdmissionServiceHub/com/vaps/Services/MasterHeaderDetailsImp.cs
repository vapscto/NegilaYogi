using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class MasterHeaderDetailsImp : Interfaces.MasterHeaderDetailsInterface
    {
        private static ConcurrentDictionary<string, MasterHeaderDetailsDTO> _login =
      new ConcurrentDictionary<string, MasterHeaderDetailsDTO>();

        private readonly DomainModelMsSqlServerContext _MasterPeriodContext;


        public MasterHeaderDetailsImp(DomainModelMsSqlServerContext MasterPeriodContext)
        {
            _MasterPeriodContext = MasterPeriodContext;
        }

        public MasterHeaderDetailsDTO GetMasterHeaderDetailsData(MasterHeaderDetailsDTO data)
        {
            try
            {
                data.institutionModuleList = (from a in _MasterPeriodContext.Institution_Module
                                               from b in _MasterPeriodContext.masterModule
                                               where (a.IVRMM_Id == b.IVRMM_Id && a.MI_Id == data.MI_Id)
                                               select new MasterHeaderDetailsDTO { IVRMIM_Id = a.IVRMIM_Id, IVRMM_ModuleName = b.IVRMM_ModuleName }
                                    ).Distinct().ToArray();


                //to get All Institution  ModulePage Dropdown.
                data.institutionPageList = (from a in _MasterPeriodContext.Institution_Module_Page
                                             from b in _MasterPeriodContext.masterPage
                                             from c  in _MasterPeriodContext.Institution_Module
                                            where a.IVRMIM_Id==c.IVRMIM_Id && b.IVRMP_Id==a.IVRMP_Id 
                                            && c.MI_Id==data.MI_Id 
                                             select new MasterHeaderDetailsDTO { IVRMIMP_Id = a.IVRMIMP_Id, IVRMMP_PageName = b.IVRMMP_PageName }
                                    ).Distinct().ToArray();

                data.parameterlist = (from a in _MasterPeriodContext.SMS_MAIL_PARAMETER_DMO
                                      select a).OrderBy(e=>e.ISMP_NAME).Distinct().ToArray();

                data.headerdata = (from a in _MasterPeriodContext.Institution_Module_Page
                                   from b in _MasterPeriodContext.masterPage
                                   from c in _MasterPeriodContext.Institution_Module
                                   from d in _MasterPeriodContext.smsEmailHeader
                                   from e in _MasterPeriodContext.masterModule
                                   where a.IVRMIM_Id == c.IVRMIM_Id && b.IVRMP_Id == a.IVRMP_Id
                                   && c.MI_Id == data.MI_Id && a.IVRMIMP_Id == d.IVRMIMP_Id && d.IVRMIM_Id == c.IVRMIM_Id && d.MI_Id == data.MI_Id && c.IVRMM_Id == e.IVRMM_Id
                                   select new MasterHeaderDetailsDTO
                                   {
                                       IVRMHE_Id = d.IVRMHE_Id,
                                       IVRMHE_Name = d.IVRMHE_Name,
                                       IVRMIM_Id = a.IVRMIM_Id,
                                       IVRMM_ModuleName = e.IVRMM_ModuleName,
                                       IVRMIMP_Id = a.IVRMIMP_Id,
                                       IVRMMP_PageName = b.IVRMMP_PageName

                                   }
                                 ).Distinct().ToArray();


            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }
        public MasterHeaderDetailsDTO getmodulePage(MasterHeaderDetailsDTO data)
        {
            try
            {

                data.institutionPageList = (from a in _MasterPeriodContext.Institution_Module_Page
                                            from b in _MasterPeriodContext.masterPage
                                            from c in _MasterPeriodContext.Institution_Module
                                            where a.IVRMIM_Id == c.IVRMIM_Id && b.IVRMP_Id == a.IVRMP_Id
                                            && c.MI_Id == data.MI_Id && c.IVRMIM_Id==data.IVRMIM_Id
                                            select new MasterHeaderDetailsDTO { IVRMIMP_Id = a.IVRMIMP_Id, IVRMMP_PageName = b.IVRMMP_PageName }
                                   ).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public MasterHeaderDetailsDTO SaveData(MasterHeaderDetailsDTO data)
        {
            try
            {
                if (data.IVRMHE_Id>0)
                {
                    var checkdup = _MasterPeriodContext.smsEmailHeader.Where(a => a.MI_Id == data.MI_Id && a.IVRMHE_Name.ToLower().Trim() == data.IVRMHE_Name.ToLower().Trim() && a.IVRMHE_Id !=data.IVRMHE_Id).ToList();
                    if (checkdup.Count > 0)
                    {
                        data.messageupdate = "ext";
                        data.returnVal = true;
                    }
                    else
                    {
                        var res = _MasterPeriodContext.smsEmailHeader.Single(r => r.IVRMHE_Id == data.IVRMHE_Id);
                        res.MI_Id = data.MI_Id;
                        res.IVRMIMP_Id = data.IVRMIMP_Id;
                        res.IVRMIM_Id = data.IVRMIM_Id;
                        res.IVRMHE_Name = data.IVRMHE_Name;
                        res.UpdatedDate = DateTime.Now;

                        _MasterPeriodContext.Update(res);

                        var checkparm = _MasterPeriodContext.SmsEmailParameterMappingDMO.Where(r => r.IVRMHE_Id == data.IVRMHE_Id).ToList();

                        if (checkparm.Count>0)
                        {
                            foreach (var item in checkparm)
                            {
                                _MasterPeriodContext.Remove(item);
                            }
                        }


                        if (data.pageids.Length > 0)
                        {
                            foreach (var item in data.pageids)
                            {
                                SmsEmailParameterMappingDMO obj1 = new SmsEmailParameterMappingDMO();

                                obj1.IVRMHE_Id = data.IVRMHE_Id;
                                obj1.ISMP_ID = item.ISMP_ID;
                                obj1.CreatedDate = DateTime.Now;
                                obj1.UpdatedDate = DateTime.Now;
                                _MasterPeriodContext.Add(obj1);

                            }

                        }

                        int res1 = _MasterPeriodContext.SaveChanges();
                        if (res1 > 0)
                        {
                            data.messageupdate = "upd";
                            data.returnVal = true;
                        }
                        else
                        {
                            data.returnVal = false;
                        }

                    }
                }
                else
                {
                    var checkdup = _MasterPeriodContext.smsEmailHeader.Where(a => a.MI_Id == data.MI_Id && a.IVRMHE_Name.ToLower().Trim() == data.IVRMHE_Name.ToLower().Trim()).ToList();
                    if (checkdup.Count>0)
                    {
                        data.messageupdate = "ext";
                        data.returnVal = true;
                    }
                    else
                    {
                        SmsEmailHeader obj = new SmsEmailHeader();
                        obj.MI_Id = data.MI_Id;
                        obj.IVRMIMP_Id = data.IVRMIMP_Id;
                        obj.IVRMIM_Id = data.IVRMIM_Id;
                        obj.IVRMHE_Name = data.IVRMHE_Name;
                        obj.CreatedDate = DateTime.Now; 
                        obj.UpdatedDate = DateTime.Now;

                        _MasterPeriodContext.Add(obj);

                        if (data.pageids.Length>0)
                        {
                            foreach (var item in data.pageids)
                            {
                                SmsEmailParameterMappingDMO obj1 = new SmsEmailParameterMappingDMO();

                                obj1.IVRMHE_Id = obj.IVRMHE_Id;
                                obj1.ISMP_ID = item.ISMP_ID;
                                obj1.CreatedDate = DateTime.Now;
                                obj1.UpdatedDate = DateTime.Now;
                                _MasterPeriodContext.Add(obj1);

                            }
                            
                        }

                        int res= _MasterPeriodContext.SaveChanges();
                        if (res>0)
                        {
                            data.messageupdate = "add";
                            data.returnVal = true;
                        }
                        else
                        {
                            data.returnVal = false;
                        }

                    }
                    
                }
                
            }
            catch (Exception ex )
            {
                Console.WriteLine(ex.Message);
                data.returnVal =false;                
            }
            return data;
        }
        public MasterHeaderDetailsDTO GetSelectedRowDetails(MasterHeaderDetailsDTO data)
        {

            try
            {

                data.GridviewDetails = _MasterPeriodContext.smsEmailHeader.Where(e => e.IVRMHE_Id == data.IVRMHE_Id).ToArray();
                data.parameterlist= _MasterPeriodContext.SmsEmailParameterMappingDMO.Where(e => e.IVRMHE_Id == data.IVRMHE_Id).ToArray();


            }
            catch (Exception ex)
            {

                throw ex;
            }


            return data;
        }

        public MasterHeaderDetailsDTO DeleteEntry(int ID)
        {
            MasterHeaderDetailsDTO data = new MasterHeaderDetailsDTO();
            try
            {

                var dellist = _MasterPeriodContext.smsEmailHeader.Single(e => e.IVRMHE_Id == ID);
                _MasterPeriodContext.Remove(dellist);

                var parlist = _MasterPeriodContext.SmsEmailParameterMappingDMO.Where(e => e.IVRMHE_Id == ID).ToList();
                if (parlist.Count>0)
                {
                    foreach (var item in parlist)
                    {
                        _MasterPeriodContext.Remove(item);
                    }
                }

                int abc = _MasterPeriodContext.SaveChanges();
                if (abc>0)
                {
                    data.returnVal = true;
                }
                else
                {
                    data.returnVal = false;
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }

    }
}
