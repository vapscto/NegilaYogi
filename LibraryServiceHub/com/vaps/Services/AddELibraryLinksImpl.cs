using DataAccessMsSqlServerProvider.com.vapstech.Library;
using System;
using DomainModel.Model.com.vapstech.Library;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Library;


namespace LibraryServiceHub.com.vaps.Services
{
    public class  AddELibraryLinksImpl:Interfaces. AddELibraryLinksInterface
    {
        public LibraryContext _LibraryContext;
        public  AddELibraryLinksImpl(LibraryContext para)
        {
            _LibraryContext = para;
        }

        public  AddELibraryLinksDTO Savedata( AddELibraryLinksDTO data)
        {
            try
            {
                if (data.LELS_Id > 0)
                {
                    var Duplicate = _LibraryContext.AddELibraryLinksDMO.Where(t => t.LELS_Name.Trim() == data.LELS_Name.Trim() && t.LELS_Url.Trim() == data.LELS_Url.Trim() && t.LELS_Id !=data.LELS_Id).ToList
                        ();
                    if (Duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _LibraryContext.AddELibraryLinksDMO.Single(t => t.LELS_Id == data.LELS_Id);
                        update.LELS_Name = data.LELS_Name;
                        update.LELS_BookType = data.LELS_BookType;
                        update.LELS_Genre = data.LELS_Genre;
                        update.LELS_PriceRange = data.LELS_PriceRange;
                        update.LELS_FilePath = data.LELS_FilePath;
                        update.LELS_Url = data.LELS_Url;
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
                        data.duplicate = false;
                    }
                }
                else
                {
                    var Duplicate = _LibraryContext.AddELibraryLinksDMO.Where(t => t.LELS_Name.Trim() == data.LELS_Name.Trim() && t.LELS_Url.Trim() == data.LELS_Url.Trim()).ToList();
                      
                    if (Duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        LIB_ELibraryLinksDMO obj = new LIB_ELibraryLinksDMO();
                        obj.LELS_Name = data.LELS_Name;
                        obj.LELS_BookType = data.LELS_BookType;
                        obj.LELS_Genre = data.LELS_Genre;
                        obj.LELS_PriceRange = data.LELS_PriceRange;
                        obj.LELS_FilePath = data.LELS_FilePath;
                        obj.LELS_Url = data.LELS_Url;
                        obj.LELS_ActiveFlag = true;
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
                        data.duplicate = false;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public  AddELibraryLinksDTO getdetails(int id)
        {
             AddELibraryLinksDTO data = new  AddELibraryLinksDTO();
            try
            {
                data.linklist = _LibraryContext. AddELibraryLinksDMO.Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public  AddELibraryLinksDTO GetELibrary(int id)
        {
             AddELibraryLinksDTO data = new  AddELibraryLinksDTO();
            try
            {
                data.linklist = _LibraryContext. AddELibraryLinksDMO.Where(r=>r.LELS_ActiveFlag==true).Distinct().OrderBy(s=>s.LELS_Name).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public  AddELibraryLinksDTO deactiveY( AddELibraryLinksDTO data)
        {
            try
            {
                var result = _LibraryContext.AddELibraryLinksDMO.Single(t => t.LELS_Id == data.LELS_Id);

                if (result.LELS_ActiveFlag == true)
                {
                    result.LELS_ActiveFlag = false;
                }
                else if (result.LELS_ActiveFlag == false)
                {
                    result.LELS_ActiveFlag = true;
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
        public  AddELibraryLinksDTO geteditdata( AddELibraryLinksDTO data)
        {
            try
            {
                data.editlist = _LibraryContext.AddELibraryLinksDMO.Where(t => t.LELS_Id == data.LELS_Id).Distinct().ToArray();
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
