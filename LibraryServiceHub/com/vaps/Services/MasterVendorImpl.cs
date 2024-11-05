using DataAccessMsSqlServerProvider.com.vapstech.Library;
using DomainModel.Model.com.vapstech.Library;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Services
{
    public class MasterVendorImpl:Interfaces.MasterVendorInterface
    {
        public LibraryContext _LibraryContext;
        public MasterVendorImpl(LibraryContext para)
        {
            _LibraryContext = para;
        }

        public MasterVendorDTO deactiveY(MasterVendorDTO data)
        {
            try
            {
                var result = _LibraryContext.MasterVanderDMO.Single(t => t.LMV_Id == data.LMV_Id && t.MI_Id == data.MI_Id);
                if (result.LMV_ActiveFlg == true)
                {
                    result.LMV_ActiveFlg = false;
                }
                else if (result.LMV_ActiveFlg == false)
                {
                    result.LMV_ActiveFlg = true;
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

        public MasterVendorDTO getdetails(int id)
        {
            MasterVendorDTO data = new MasterVendorDTO();
            try
            {
                data.pulishlist = _LibraryContext.MasterVanderDMO.Where(t => t.MI_Id == id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public MasterVendorDTO Savedata(MasterVendorDTO data)
        {
            try
            {
                if (data.LMV_Id > 0)
                {
                    if (data.LMV_MobileNo==null ||  data.LMV_EMailId== null && data.LMV_EMailId == "" || data.LMV_PhoneNo== null && data.LMV_PhoneNo =="")
                    {
                        var update = _LibraryContext.MasterVanderDMO.Single(t => t.LMV_Id == data.LMV_Id && t.MI_Id == data.MI_Id);


                        update.LMV_VendorName = data.LMV_VendorName;
                        update.LMV_Address = data.LMV_Address;
                        update.LMV_EMailId = data.LMV_EMailId;
                        update.LMV_PhoneNo = data.LMV_PhoneNo;
                        update.LMV_MobileNo = data.LMV_MobileNo;
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
                    else
                    {
                        var Duplicate = _LibraryContext.MasterVanderDMO.Where(t => t.MI_Id == data.MI_Id && t.LMV_Id != data.LMV_Id && t.LMV_EMailId.Contains(data.LMV_EMailId) | t.LMV_PhoneNo.Contains(data.LMV_PhoneNo) | t.LMV_MobileNo == data.LMV_MobileNo).Distinct().ToList();
                        if (Duplicate.Count() > 0)
                        {
                            data.duplicate = true;
                        }
                        else
                        {
                            var update = _LibraryContext.MasterVanderDMO.Single(t => t.LMV_Id == data.LMV_Id && t.MI_Id == data.MI_Id);


                            update.LMV_VendorName = data.LMV_VendorName;
                            update.LMV_Address = data.LMV_Address;
                            update.LMV_EMailId = data.LMV_EMailId;
                            update.LMV_PhoneNo = data.LMV_PhoneNo;
                            update.LMV_MobileNo = data.LMV_MobileNo;
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

                    
                }
                else
                {
                    if (data.LMV_MobileNo != null || data.LMV_PhoneNo!= null && data.LMV_PhoneNo != "" || data.LMV_EMailId!= null && data.LMV_EMailId != "")
                    {
                        var Duplicate = _LibraryContext.MasterVanderDMO.Where(t => t.MI_Id == data.MI_Id && t.LMV_EMailId.Contains(data.LMV_EMailId) | t.LMV_PhoneNo.Contains(data.LMV_PhoneNo) | t.LMV_MobileNo == data.LMV_MobileNo).Distinct().ToList();

                        if (Duplicate.Count() > 0)
                        {
                            data.duplicate = true;
                        }
                        else
                        {
                            MasterVanderDMO obj = new MasterVanderDMO();

                            obj.MI_Id = data.MI_Id;
                            obj.LMV_VendorName = data.LMV_VendorName;
                            obj.LMV_Address = data.LMV_Address;
                            obj.LMV_EMailId = data.LMV_EMailId;
                            obj.LMV_PhoneNo = data.LMV_PhoneNo;
                            obj.LMV_MobileNo = data.LMV_MobileNo;
                            obj.LMV_ActiveFlg = true;
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
                    else
                    {
                        var Duplicate = _LibraryContext.MasterVanderDMO.Where(t => t.MI_Id == data.MI_Id && t.LMV_VendorName.Contains(data.LMV_VendorName)).Distinct().ToList();

                        if (Duplicate.Count() > 0)
                        {
                            data.duplicate = true;
                        }
                        else
                        {
                            MasterVanderDMO obj = new MasterVanderDMO();

                            obj.MI_Id = data.MI_Id;
                            obj.LMV_VendorName = data.LMV_VendorName;
                            obj.LMV_Address = data.LMV_Address;
                            obj.LMV_EMailId = data.LMV_EMailId;
                            obj.LMV_PhoneNo = data.LMV_PhoneNo;
                            obj.LMV_MobileNo = data.LMV_MobileNo;
                            obj.LMV_ActiveFlg = true;
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
