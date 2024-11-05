using DataAccessMsSqlServerProvider.com.vapstech.Library;
using DomainModel.Model.com.vapstech.Library;
using LibraryServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Services
{
    public class MasterAuthorImpl : Interfaces.MasterAuthorInterface
    {
        public LibraryContext _LibraryContext;
        public MasterAuthorImpl(LibraryContext context)
        {
            _LibraryContext = context;
        }

        public LIB_Master_Author_DTO Savedata(LIB_Master_Author_DTO data)
        {
            try
            {
                if (data.LMAU_Id > 0)
                {
                    var Duplicate = _LibraryContext.LIB_Master_Author_DMO.Where(t => t.LMAU_Id != data.LMAU_Id && t.MI_Id == data.MI_Id && t.LMAU_AuthorFirstName==data.LMAU_AuthorFirstName && t.LMAU_AuthorMiddleName==data.LMAU_AuthorMiddleName && t.LMAU_AuthorLastName==data.LMAU_AuthorLastName).ToList();
                   

                    if (Duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _LibraryContext.LIB_Master_Author_DMO.Where(t => t.LMAU_Id == data.LMAU_Id && t.MI_Id == data.MI_Id).SingleOrDefault();

                        update.LMAU_AuthorFirstName = data.LMAU_AuthorFirstName;
                        if(data.LMAU_AuthorMiddleName=="" || data.LMAU_AuthorMiddleName==null)
                        {
                            update.LMAU_AuthorMiddleName = "";
                        }
                        else
                        {
                            update.LMAU_AuthorMiddleName = data.LMAU_AuthorMiddleName;
                        }                        
                        if (data.LMAU_AuthorLastName == "" || data.LMAU_AuthorLastName == null)
                        {
                            update.LMAU_AuthorLastName = "";
                        }
                        else
                        {
                            update.LMAU_AuthorLastName = data.LMAU_AuthorLastName;
                        }
                       
                        update.LMAU_MobileNo = data.LMAU_MobileNo;
                        update.LMAU_PhoneNo = data.LMAU_PhoneNo;
                        update.LMAU_Address = data.LMAU_Address;
                        update.LMAU_EmailId = data.LMAU_EmailId;
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
                    var Duplicate = _LibraryContext.LIB_Master_Author_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMAU_AuthorFirstName==data.LMAU_AuthorFirstName && t.LMAU_AuthorMiddleName==data.LMAU_AuthorMiddleName && t.LMAU_AuthorLastName==data.LMAU_AuthorLastName).ToList();

                    if (Duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        LIB_Master_Author_DMO obj = new LIB_Master_Author_DMO();

                        obj.MI_Id = data.MI_Id;
                        obj.LMAU_AuthorFirstName = data.LMAU_AuthorFirstName;
                        if (data.LMAU_AuthorMiddleName == "" || data.LMAU_AuthorMiddleName == null)
                        {
                            obj.LMAU_AuthorMiddleName = "";
                        }
                        else
                        {
                            obj.LMAU_AuthorMiddleName = data.LMAU_AuthorMiddleName;
                        }
                        if (data.LMAU_AuthorLastName == "" || data.LMAU_AuthorLastName == null)
                        {
                            obj.LMAU_AuthorLastName = "";
                        }
                        else
                        {
                            obj.LMAU_AuthorLastName = data.LMAU_AuthorLastName;
                        }                       
                        obj.LMAU_MobileNo = data.LMAU_MobileNo;
                        obj.LMAU_PhoneNo = data.LMAU_PhoneNo;
                        obj.LMAU_Address = data.LMAU_Address;
                        obj.LMAU_EmailId = data.LMAU_EmailId;
                        obj.LMAU_ActiveFlg = true;
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

        public LIB_Master_Author_DTO getdetails(LIB_Master_Author_DTO id)
        {
            LIB_Master_Author_DTO data = new LIB_Master_Author_DTO();
            try
            {
                data.authorlist = (from a in _LibraryContext.LIB_Master_Author_DMO
                                   where (a.MI_Id == id.MI_Id)
                                   select new LIB_Master_Author_DTO
                                   {
                                       // LMAU_AuthorFirstName = a.LMAU_AuthorFirstName,
                                       LMAU_Id=a.LMAU_Id,
                                       LMAU_MobileNo = a.LMAU_MobileNo,
                                       LMAU_PhoneNo = a.LMAU_PhoneNo,
                                       LMAU_Address = a.LMAU_Address,
                                       LMAU_EmailId = a.LMAU_EmailId,
                                       LMAU_ActiveFlg = a.LMAU_ActiveFlg,
                                       LMAU_AuthorFirstName = a.LMAU_AuthorFirstName + (string.IsNullOrEmpty(a.LMAU_AuthorMiddleName) ? "" : ' ' + a.LMAU_AuthorMiddleName) + (string.IsNullOrEmpty(a.LMAU_AuthorLastName) ? "" : ' ' + a.LMAU_AuthorLastName),

                                   }).Distinct().OrderBy(t => t.LMAU_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public LIB_Master_Author_DTO deactiveY(LIB_Master_Author_DTO data)
        {
            try
            {
                var result = _LibraryContext.LIB_Master_Author_DMO.Single(t => t.LMAU_Id == data.LMAU_Id && t.MI_Id == data.MI_Id);

                if (result.LMAU_ActiveFlg == true)
                {
                    result.LMAU_ActiveFlg = false;
                }
                else if (result.LMAU_ActiveFlg == false)
                {
                    result.LMAU_ActiveFlg = true;
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
