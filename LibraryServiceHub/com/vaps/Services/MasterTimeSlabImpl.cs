using DataAccessMsSqlServerProvider.com.vapstech.Library;
using DomainModel.Model.com.vapstech.Library;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Services
{
    public class MasterTimeSlabImpl : Interfaces.MasterTimeSlabInterface
    {

        public LibraryContext _LibraryContext;
        public MasterTimeSlabImpl(LibraryContext para)
        {
            _LibraryContext = para;
        }

        public MasterTimeSlabDTO getdetails(int id)
        {
            MasterTimeSlabDTO data = new MasterTimeSlabDTO();

            try
            {
               
                data.alldata = (from a in _LibraryContext.MasterTimeSlabDMO
                                where (a.MI_Id == id)
                                select new MasterTimeSlabDTO
                                {
                                    LFSE_Id = a.LFSE_Id,
                                    MI_Id = a.MI_Id,
                                    LFSE_UserType = a.LFSE_UserType,
                                    LFSE_SlabTypeFlg = a.LFSE_SlabTypeFlg,
                                    LFSE_PerDayFlg = a.LFSE_PerDayFlg,
                                    LFSE_FromDay = a.LFSE_FromDay,
                                    LFSE_ToDay = a.LFSE_ToDay,
                                    LFSE_Amount = a.LFSE_Amount,
                                    LFSE_ActiveFlg = a.LFSE_ActiveFlg,


                                }).Distinct().OrderBy(t=>t.LFSE_Id).ToArray();
            }           
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        public MasterTimeSlabDTO Savedata(MasterTimeSlabDTO data)
        {

            try
            {
                if (data.LFSE_Id > 0)
                {
                    var Duplicate = _LibraryContext.MasterTimeSlabDMO.Where(t => t.LFSE_Id != data.LFSE_Id && t.MI_Id == data.MI_Id && t.LFSE_UserType == data.LFSE_UserType
                        && t.LFSE_SlabTypeFlg == data.LFSE_SlabTypeFlg).ToList();
                    if (Duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _LibraryContext.MasterTimeSlabDMO.Where(t => t.LFSE_Id == data.LFSE_Id && t.MI_Id == data.MI_Id ).SingleOrDefault();

                       // update.LFSE_Id = data.LFSE_Id;
                        //update.MI_Id = data.MI_Id;
                        update.LFSE_UserType = data.LFSE_UserType;
                        update.LFSE_SlabTypeFlg = data.LFSE_SlabTypeFlg;
                        update.LFSE_PerDayFlg = data.LFSE_PerDayFlg;
                        update.LFSE_FromDay = data.LFSE_FromDay;
                        update.LFSE_ToDay = data.LFSE_ToDay;
                        update.LFSE_Amount = data.LFSE_Amount;
                                   
                        update.UpdatedDate = DateTime.Now;
                        _LibraryContext.Update(update);
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
                }
                else
                {
                    var Duplicate = _LibraryContext.MasterTimeSlabDMO.Where(t => t.MI_Id == data.MI_Id && t.LFSE_UserType == data.LFSE_UserType && t.LFSE_SlabTypeFlg == data.LFSE_SlabTypeFlg).ToList();
                    if (Duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        MasterTimeSlabDMO obj = new MasterTimeSlabDMO();

                        //obj.LFSE_Id = data.LFSE_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.LFSE_UserType = data.LFSE_UserType;
                        obj.LFSE_SlabTypeFlg = data.LFSE_SlabTypeFlg;
                        obj.LFSE_PerDayFlg = data.LFSE_PerDayFlg;
                        obj.LFSE_FromDay = data.LFSE_FromDay;
                        obj.LFSE_ToDay = data.LFSE_ToDay;
                        obj.LFSE_Amount = data.LFSE_Amount;

                        obj.LFSE_ActiveFlg = true;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        _LibraryContext.Add(obj);
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
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

          
            return data;
        }

        public MasterTimeSlabDTO deactiveY(MasterTimeSlabDTO data)
        {

            try
            {
                var result = _LibraryContext.MasterTimeSlabDMO.Single(t => t.MI_Id == data.MI_Id && t.LFSE_Id == data.LFSE_Id);

                if (result.LFSE_ActiveFlg == true)
                {
                    result.LFSE_ActiveFlg = false;
                }
                else if (result.LFSE_ActiveFlg == false)
                {
                    result.LFSE_ActiveFlg = true;
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
