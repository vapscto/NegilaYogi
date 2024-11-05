
using System;
using DataAccessMsSqlServerProvider.com.vapstech.Library;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Library;
using DomainModel.Model.com.vapstech.Library;

namespace LibraryServiceHub.com.vaps.Services
{
    public class MasterLanguageImpl:Interfaces.MasterLanguageInterface
    {
        public LibraryContext _LibraryContext;
        public MasterLanguageImpl(LibraryContext para)
        {
            _LibraryContext = para;
        }

      

        public MasterLanguageDTO getdetails(int id)
        {
            MasterLanguageDTO data = new MasterLanguageDTO();
            try
            {
                data.langlist = _LibraryContext.MasterLanguageDMO.Where(t => t.MI_Id == id).ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public MasterLanguageDTO Savedata(MasterLanguageDTO data)
        {
            try
            {
                if(data.LML_Id > 0)
                {
                    var Duplicate = _LibraryContext.MasterLanguageDMO.Where(t => t.MI_Id == data.MI_Id && t.LML_Id != data.LML_Id && t.LML_LanguageName == data.LML_LanguageName ).ToList();

                    if (Duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _LibraryContext.MasterLanguageDMO.Single(t => t.LML_Id == data.LML_Id && t.MI_Id == data.MI_Id);
                        
                        update.LML_LanguageName = data.LML_LanguageName;
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
                    var Duplicate = _LibraryContext.MasterLanguageDMO.Where(t =>t.MI_Id == data.MI_Id && t.LML_LanguageName == data.LML_LanguageName).ToList();

                    if(Duplicate.Count()>0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        MasterLanguageDMO Obj = new MasterLanguageDMO();

                        Obj.MI_Id = data.MI_Id;
                        Obj.LML_LanguageName = data.LML_LanguageName;
                        Obj.LML_ActiveFlg = true;
                        Obj.CreatedDate = DateTime.Now;
                        Obj.UpdatedDate= DateTime.Now;

                        _LibraryContext.Add(Obj);
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


        public MasterLanguageDTO deactiveY(MasterLanguageDTO data)
        {
            try
            {
                var result = _LibraryContext.MasterLanguageDMO.Single(t => t.MI_Id == data.MI_Id && t.LML_Id == data.LML_Id);

                if (result.LML_ActiveFlg == true)
                {
                    result.LML_ActiveFlg = false;
                }
                else if (result.LML_ActiveFlg == false)
                {
                    result.LML_ActiveFlg = true;
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
