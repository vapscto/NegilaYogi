using DataAccessMsSqlServerProvider.com.vapstech.Library;
using DomainModel.Model.com.vapstech.Library;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Services
{
    public class MasterClassCategoryImpl
    {
        public MasterClassCategoryImpl()
        {

        }
        //public Task<LIB_Master_ClassCategory_DTO> getdetails(LIB_Master_ClassCategory_DTO data)
        //{
        public LIB_Master_ClassCategory_DTO getdetails(LIB_Master_ClassCategory_DTO data)
        {
            try
            {
                #region for gettting Library staff details
                //using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "LIB_Staff_Details";
                //    cmd.CommandType = CommandType.StoredProcedure;

                //    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                //SqlDbType.BigInt)
                //    {
                //        Value = data.MI_Id
                //    });

                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();

                //    var retObject = new List<dynamic>();
                //    try
                //    {
                //        using (var dataReader = await cmd.ExecuteReaderAsync())
                //        {
                //            while (await dataReader.ReadAsync())
                //            {
                //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                {
                //                    dataRow.Add(
                //                        dataReader.GetName(iFiled),
                //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                    );
                //                }

                //                retObject.Add((ExpandoObject)dataRow);
                //            }
                //        }
                //        data.stafflist = retObject.ToArray();
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public LIB_Master_ClassCategory_DTO Savedata(LIB_Master_ClassCategory_DTO data)
        {
            //try
            //{
            //    if (data.LMCC_Id > 0)
            //    {
            //        var Duplicate = _LibraryContext.LIB_Master_ClassCategory_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMCC_Id != data.LMCC_Id && t.LMCC_CategoryName == data.LMCC_CategoryName).ToList();

            //        if (Duplicate.Count() > 0)
            //        {
            //            data.duplicate = true;
            //        }
            //        else
            //        {
            //            var update = _LibraryContext.LIB_Master_ClassCategory_DMO.Single(t => t.LMCC_Id == data.LMCC_Id && t.MI_Id == data.MI_Id);

            //            update.LMCC_CategoryName = data.LMCC_CategoryName;
            //            update.UpdatedDate = DateTime.Now;
            //            _LibraryContext.Update(update);

            //            var update2 = _LibraryContext.LIB_User_ClassCategory_DMO.Where(t => t.LMCC_Id == data.LMCC_Id).SingleOrDefault();

            //            update2.IVRMUL_Id = data.IVRMUL_Id;
            //            update2.LMCC_Id = update.LMCC_Id;
            //            update2.UpdatedDate = DateTime.Now;

            //            _LibraryContext.Update(update2);
                        

            //            int rowAffected = _LibraryContext.SaveChanges();
            //            if (rowAffected > 0)
            //            {
            //                data.returnval = true;
            //            }
            //            else
            //            {
            //                data.returnval = false;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        var Duplicate = _LibraryContext.LIB_Master_ClassCategory_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMCC_CategoryName == data.LMCC_CategoryName).ToList();

            //        if (Duplicate.Count() > 0)
            //        {
            //            data.duplicate = true;
            //        }
            //        else
            //        {
            //                LIB_Master_ClassCategory_DMO Obj = new LIB_Master_ClassCategory_DMO();

            //                Obj.MI_Id = data.MI_Id;
            //                Obj.LMCC_CategoryName = data.LMCC_CategoryName;
            //                Obj.LMCC_ActiveFlag = true;
            //                Obj.CreatedDate = DateTime.Now;
            //                Obj.UpdatedDate = DateTime.Now;

            //                _LibraryContext.Add(Obj);

            //                LIB_User_ClassCategory_DMO obj2 = new LIB_User_ClassCategory_DMO();

            //                obj2.MI_Id = data.MI_Id;
            //                obj2.LMCC_Id = Obj.LMCC_Id;
            //                obj2.IVRMUL_Id = data.IVRMUL_Id;
            //                obj2.LUCC_ActiveFlg = true;
            //                obj2.CreatedDate = DateTime.Now;
            //                obj2.UpdatedDate = DateTime.Now;

            //                _LibraryContext.Add(obj2);

            //                int rowAffected = _LibraryContext.SaveChanges();
            //                if (rowAffected > 0)
            //                {
            //                    data.returnval = true;
            //                }
            //                else
            //                {
            //                    data.returnval = false;
            //                }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            return data;
        }


        public LIB_Master_ClassCategory_DTO deactiveY(LIB_Master_ClassCategory_DTO data)
        {
            //try
            //{
            //    var result = _LibraryContext.LIB_Master_ClassCategory_DMO.Single(t => t.MI_Id == data.MI_Id && t.LMCC_Id == data.LMCC_Id);

            //    if (result.LMCC_ActiveFlag == true)
            //    {
            //        result.LMCC_ActiveFlag = false;
            //    }
            //    else if (result.LMCC_ActiveFlag == false)
            //    {
            //        result.LMCC_ActiveFlag = true;
            //    }
            //    result.UpdatedDate = DateTime.Now;
            //    _LibraryContext.Update(result);
            //    int rowAffected = _LibraryContext.SaveChanges();
            //    if (rowAffected > 0)
            //    {
            //        data.returnval = true;
            //    }
            //    else
            //    {
            //        data.returnval = false;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            return data;
        }
    }
}
