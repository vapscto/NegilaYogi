using DataAccessMsSqlServerProvider.com.vapstech.Library;
using DomainModel.Model.com.vapstech.Library;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Services
{
    public class MasterDonorImpl:Interfaces.MasterDonorInterface
    {
        public LibraryContext _LibraryContext;
        public MasterDonorImpl(LibraryContext para)
        {
            _LibraryContext = para;
        }

        public MasterDonorDTO deactiveY(MasterDonorDTO data)
        {
            try
            {
                var result = _LibraryContext.MasterDonorDMO.Single(t => t.Donor_Id == data.Donor_Id && t.MI_Id == data.MI_Id);
                if (result.Donor_ActiveFlag == true)
                {
                    result.Donor_ActiveFlag = false;
                }
                else if (result.Donor_ActiveFlag == false)
                {
                    result.Donor_ActiveFlag = true;
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

        public MasterDonorDTO getdetails(int id)
        {
            MasterDonorDTO data = new MasterDonorDTO();
            try
            {
                data.donorlist = _LibraryContext.MasterDonorDMO.Where(t => t.MI_Id == id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public MasterDonorDTO Savedata(MasterDonorDTO data)
        {
            try
            {
                if (data.Donor_Id > 0)
                {
                    var Duplicate = _LibraryContext.MasterDonorDMO.Where(t => t.MI_Id == data.MI_Id && t.Donor_Id != data.Donor_Id && t.Donor_Name == data.Donor_Name).Distinct().ToList();
                    if (Duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _LibraryContext.MasterDonorDMO.Single(t => t.Donor_Id == data.Donor_Id && t.MI_Id == data.MI_Id);
                        update.MI_Id = data.MI_Id;
                        update.Donor_Name = data.Donor_Name;
                        update.Donor_Address = data.Donor_Address;
                       
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
                    var Duplicate = _LibraryContext.MasterDonorDMO.Where(t => t.Donor_Name == data.Donor_Name ).ToList();

                    if (Duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        MasterDonorDMO obj = new MasterDonorDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.Donor_Name = data.Donor_Name;
                        obj.Donor_Address = data.Donor_Address;
                        obj.Donor_ActiveFlag = true;
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
