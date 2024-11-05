using System;
using DataAccessMsSqlServerProvider.com.vapstech.Library;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Library;
using DomainModel.Model.com.vapstech.Library;

namespace LibraryServiceHub.com.vaps.Services
{
    public class MasterPublisherImpl : Interfaces.MasterPublisherInterface
    {
        public LibraryContext _LibraryContext;
        public MasterPublisherImpl(LibraryContext para)
        {
            _LibraryContext = para;
        }

   

        public MasterPublisherDTO getdetails(int id)
        {
            MasterPublisherDTO data = new MasterPublisherDTO();
            try
            {
                data.pulishlist = _LibraryContext.MasterPublisherDMO.Where(t => t.MI_Id == id).Distinct().ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public MasterPublisherDTO Savedata(MasterPublisherDTO data)
        {
            try
            {
                if (data.LMP_Id > 0)
                {

                    if(data.LMP_MobileNo==null || data.LMP_PhoneNo != null && data.LMP_PhoneNo=="" || data.LMP_EMailId != null && data.LMP_EMailId=="")
                    {
                        var update = _LibraryContext.MasterPublisherDMO.Single(t => t.MI_Id == data.MI_Id && t.LMP_Id == data.LMP_Id);

                        update.LMP_PublisherName = data.LMP_PublisherName;
                        update.LMP_Address = data.LMP_Address;
                        update.LMP_EMailId = data.LMP_EMailId;
                        update.LMP_MobileNo = data.LMP_MobileNo;
                        update.LMP_PhoneNo = data.LMP_PhoneNo;
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
                        var Duplicate = _LibraryContext.MasterPublisherDMO.Where(t => t.LMP_Id != data.LMP_Id && t.MI_Id == data.MI_Id && t.LMP_MobileNo == data.LMP_MobileNo | t.LMP_EMailId.Contains(data.LMP_EMailId) | t.LMP_PhoneNo.Contains(data.LMP_PhoneNo)).Distinct().ToList();

                        if (Duplicate.Count() > 0)
                        {
                            data.duplicate = true;
                        }
                        else
                        {
                            var update = _LibraryContext.MasterPublisherDMO.Single(t => t.MI_Id == data.MI_Id && t.LMP_Id == data.LMP_Id);

                            update.LMP_PublisherName = data.LMP_PublisherName;
                            update.LMP_Address = data.LMP_Address;
                            update.LMP_EMailId = data.LMP_EMailId;
                            update.LMP_MobileNo = data.LMP_MobileNo;
                            update.LMP_PhoneNo = data.LMP_PhoneNo;
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
                    if (data.LMP_MobileNo != null || data.LMP_PhoneNo != null && data.LMP_PhoneNo != "" || data.LMP_EMailId != null && data.LMP_EMailId != "")
                    {
                        var Duplicate = _LibraryContext.MasterPublisherDMO.Where(t => t.MI_Id == data.MI_Id && t.LMP_EMailId.Contains(data.LMP_EMailId) | t.LMP_PhoneNo.Contains(data.LMP_PhoneNo) | t.LMP_MobileNo == data.LMP_MobileNo).Distinct().ToList();

                        if (Duplicate.Count() > 0)
                        {
                            data.duplicate = true;
                        }
                        else
                        {
                            MasterPublisherDMO obj = new MasterPublisherDMO();

                            obj.MI_Id = data.MI_Id;
                            obj.LMP_PublisherName = data.LMP_PublisherName;
                            obj.LMP_Address = data.LMP_Address;
                            obj.LMP_EMailId = data.LMP_EMailId;
                            obj.LMP_PhoneNo = data.LMP_PhoneNo;
                            obj.LMP_MobileNo = data.LMP_MobileNo;
                            obj.LMP_ActiveFlg = true;
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
                        var Duplicate = _LibraryContext.MasterPublisherDMO.Where(t => t.MI_Id == data.MI_Id && t.LMP_PublisherName.Contains(data.LMP_PublisherName)).Distinct().ToList();

                        if (Duplicate.Count() > 0)
                        {
                            data.duplicate = true;
                        }
                        else
                        {
                            MasterPublisherDMO obj = new MasterPublisherDMO();

                            obj.MI_Id = data.MI_Id;
                            obj.LMP_PublisherName = data.LMP_PublisherName;
                            obj.LMP_Address = data.LMP_Address;
                            obj.LMP_EMailId = data.LMP_EMailId;
                            obj.LMP_PhoneNo = data.LMP_PhoneNo;
                            obj.LMP_MobileNo = data.LMP_MobileNo;
                            obj.LMP_ActiveFlg = true;
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
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public MasterPublisherDTO deactiveY(MasterPublisherDTO data)
        {
            try
            {
                var result = _LibraryContext.MasterPublisherDMO.Single(t => t.MI_Id == data.MI_Id &&  t.LMP_Id == data.LMP_Id);
                if (result.LMP_ActiveFlg == true)
                {
                    result.LMP_ActiveFlg = false;
                }
                else if (result.LMP_ActiveFlg == false)
                {
                    result.LMP_ActiveFlg = true;
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
