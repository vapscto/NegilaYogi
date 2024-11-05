using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.Library;
using PreadmissionDTOs.com.vaps.Library;
using DomainModel.Model.com.vapstech.Library;

namespace LibraryServiceHub.com.vaps.Services
{
    public class MasterPeriodicityImpl : Interfaces.MasterPeriodicityInterface
    {
      public  LibraryContext _LibraryContext;
        public MasterPeriodicityImpl(LibraryContext para)
        {
            _LibraryContext = para;
        }

        public MasterPeriodicityDTO getdetails(int id)
        {
            MasterPeriodicityDTO data = new MasterPeriodicityDTO();
            try
            {
                data.periodlist = _LibraryContext.MasterPeriodicityDMO.Where(t => t.MI_Id == id).Distinct().ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        public MasterPeriodicityDTO Savedata(MasterPeriodicityDTO data)
        {
            try
            {
                if(data.LMPE_Id>0)
                {
                    var Duplicate = _LibraryContext.MasterPeriodicityDMO.Where(t => t.LMPE_Id != data.LMPE_Id && t.LMPE_PeriodicityName == data.LMPE_PeriodicityName && t.MI_Id == data.MI_Id).ToList();
                    if(Duplicate.Count()>0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _LibraryContext.MasterPeriodicityDMO.Single(t => t.MI_Id == data.MI_Id && t.LMPE_Id == data.LMPE_Id);
                        update.MI_Id = data.MI_Id;
                        update.LMPE_Id = data.LMPE_Id;
                        update.LMPE_PeriodicityName = data.LMPE_PeriodicityName;
                      
                        update.UpdatedDate = DateTime.Now;
                        update.UpdatedBy = data.UserId;
                        _LibraryContext.Update(update);
                        int rowAffected=_LibraryContext.SaveChanges();
                        if(rowAffected>0)
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
                    var Duplicate = _LibraryContext.MasterPeriodicityDMO.Where(t => t.LMPE_PeriodicityName == data.LMPE_PeriodicityName && t.MI_Id == data.MI_Id).ToList();
                    if(Duplicate.Count()>0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        MasterPeriodicityDMO obj = new MasterPeriodicityDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.LMPE_PeriodicityName = data.LMPE_PeriodicityName;
                        obj.LMPE_ActiveFlg = true;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        obj.CreatedBy = data.UserId;
                        obj.UpdatedBy = data.UserId;

                        _LibraryContext.Add(obj);
                        int rowAffected=_LibraryContext.SaveChanges();
                        if(rowAffected>0)
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
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MasterPeriodicityDTO deactiveY(MasterPeriodicityDTO data)
        {
            try
            {
                var result = _LibraryContext.MasterPeriodicityDMO.Single(t => t.MI_Id == data.MI_Id && t.LMPE_Id == data.LMPE_Id);

                if (result.LMPE_ActiveFlg == true)
                {
                    result.LMPE_ActiveFlg = false;
                }
                else if (result.LMPE_ActiveFlg == false)
                {
                    result.LMPE_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _LibraryContext.Update(result);
                int rowAffected = _LibraryContext.SaveChanges();
                if (rowAffected > 0)
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
    }
}
