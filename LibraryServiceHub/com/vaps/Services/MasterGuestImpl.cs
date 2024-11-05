using DataAccessMsSqlServerProvider.com.vapstech.Library;
using DomainModel.Model.com.vapstech.Library;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Services
{
    public class MasterGuestImpl:Interfaces.MasterGuestInterface
    {
        public LibraryContext _LibraryContext;
        public MasterGuestImpl(LibraryContext para)
        {
            _LibraryContext = para;
        }

        public MasterGuestDTO deactiveY(MasterGuestDTO data)
        {
            try
            {
                var result = _LibraryContext.MasterGuestDMO.Single(t => t.Guest_Id == data.Guest_Id && t.MI_Id == data.MI_Id);
                if (result.Guest_ActiveFlag == true)
                {
                    result.Guest_ActiveFlag = false;
                }
                else if (result.Guest_ActiveFlag == false)
                {
                    result.Guest_ActiveFlag = true;
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

        public MasterGuestDTO getdetails(int id)
        {
            MasterGuestDTO data = new MasterGuestDTO();
            try
            {
                data.pulishlist = _LibraryContext.MasterGuestDMO.Where(t => t.MI_Id == id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public MasterGuestDTO Savedata(MasterGuestDTO data)
        {
            try
            {
                if (data.Guest_Id > 0)
                {
                    var Duplicate = _LibraryContext.MasterGuestDMO.Where(t => t.MI_Id == data.MI_Id && t.Guest_Id != data.Guest_Id && t.Guest_Name == data.Guest_Name || t.Guest_Email_Id.Contains(data.Guest_Email_Id)).Distinct().ToList();
                    if (Duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _LibraryContext.MasterGuestDMO.Single(t => t.Guest_Id == data.Guest_Id && t.MI_Id == data.MI_Id);
                        update.MI_Id = data.MI_Id;
                        update.Guest_Name = data.Guest_Name;
                        update.Guest_address = data.Guest_address;
                        update.Guest_Email_Id = data.Guest_Email_Id;
                        update.Guest_Phone_No = data.Guest_Phone_No;
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
                    var Duplicate = _LibraryContext.MasterGuestDMO.Where(t => t.Guest_Name == data.Guest_Name || t.Guest_Email_Id.Contains(data.Guest_Email_Id)  ).ToList();

                    if (Duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        MasterGuestDMO obj = new MasterGuestDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.Guest_Name = data.Guest_Name;
                        obj.Guest_address = data.Guest_address;
                        obj.Guest_Email_Id = data.Guest_Email_Id;
                        obj.Guest_Phone_No = data.Guest_Phone_No;
                        obj.Guest_ActiveFlag = true;
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
    }
}
