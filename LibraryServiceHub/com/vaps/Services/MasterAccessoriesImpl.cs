using DataAccessMsSqlServerProvider.com.vapstech.Library;
using DomainModel.Model.com.vapstech.Library;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Services
{
    public class MasterAccessoriesImpl:Interfaces.MasterAccessoriesInterface
    {


        public LibraryContext _LibraryContext;
        public MasterAccessoriesImpl(LibraryContext para)
        {
            _LibraryContext = para;
        }



        public LIB_Master_Accessories_DTO getdetails(int id)
        {
            LIB_Master_Accessories_DTO data = new LIB_Master_Accessories_DTO();
            try
            {
                data.alldata = _LibraryContext.LIB_Master_Accessories_DMO.Where(t => t.MI_Id == id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public LIB_Master_Accessories_DTO Savedata(LIB_Master_Accessories_DTO data)
        {
            try
            {
                if (data.LMAC_Id > 0)
                {
                    var Duplicate = _LibraryContext.LIB_Master_Accessories_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMAC_Id != data.LMAC_Id && t.LMAC_AccessoriesName == data.LMAC_AccessoriesName).ToList();

                    if (Duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _LibraryContext.LIB_Master_Accessories_DMO.Single(t => t.LMAC_Id == data.LMAC_Id && t.MI_Id == data.MI_Id);

                        update.LMAC_AccessoriesName = data.LMAC_AccessoriesName;
                        update.LMAC_AccessoriesDesc = data.LMAC_AccessoriesDesc;
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
                    var Duplicate = _LibraryContext.LIB_Master_Accessories_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMAC_AccessoriesName == data.LMAC_AccessoriesName).ToList();

                    if (Duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        LIB_Master_Accessories_DMO Obj = new LIB_Master_Accessories_DMO();

                        Obj.MI_Id = data.MI_Id;
                        Obj.LMAC_AccessoriesName = data.LMAC_AccessoriesName;
                        Obj.LMAC_AccessoriesDesc = data.LMAC_AccessoriesDesc;
                        Obj.LMAC_ActiveFlg = true;
                        Obj.CreatedDate = DateTime.Now;
                        Obj.UpdatedDate = DateTime.Now;

                        _LibraryContext.Add(Obj);
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


        public LIB_Master_Accessories_DTO deactiveY(LIB_Master_Accessories_DTO data)
        {
            try
            {
                var result = _LibraryContext.LIB_Master_Accessories_DMO.Single(t => t.MI_Id == data.MI_Id && t.LMAC_Id == data.LMAC_Id);

                if (result.LMAC_ActiveFlg == true)
                {
                    result.LMAC_ActiveFlg = false;
                }
                else if (result.LMAC_ActiveFlg == false)
                {
                    result.LMAC_ActiveFlg = true;
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
