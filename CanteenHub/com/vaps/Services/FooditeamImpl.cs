using DataAccessMsSqlServerProvider.com.vapstech.Canteen;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Canteen;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Canteen;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CanteenHub.com.vaps.Services
{
    public class FooditeamImpl : Interfaces.FooditeamInterface
    {
        public Canteencontext _CmsContext;
        public FooditeamImpl(Canteencontext cmsContext)
        {
            _CmsContext = cmsContext;
        }
        public FooditeamDTO loaddata(FooditeamDTO data)
        {
            try
            {
                data.categeorylist = _CmsContext.FoodMasterCategoryDMO.Where(A => A.CMMCA_ActiveFlag == true).ToArray();

                data.studentpinlist = _CmsContext.Adm_M_Student_WalletPINDMO.Where(A => A.MI_Id==data.MI_Id && A.Amst_Id==data.AMST_Id).ToArray();


                data.Fooditeam = (from a in _CmsContext.FooditeamDMO
                                  from b in _CmsContext.FoodMasterCategoryDMO
                                  where (a.CMMCA_Id == b.CMMCA_Id && a.CMMFI_ActiveFlg == true)
                                  select new FooditeamDTO
                                  {
                                      CMMFI_FoodItemName = a.CMMFI_FoodItemName,
                                      CMMFI_FoodItemDescription = a.CMMFI_FoodItemDescription,
                                      CMMFI_UnitRate = a.CMMFI_UnitRate,
                                      CMMFI_OutofStockFlg = a.CMMFI_OutofStockFlg,
                                      CMMFI_PathURL = a.CMMFI_PathURL,
                                      CMMFI_Id = a.CMMFI_Id,
                                      CMMCA_CategoryName = b.CMMCA_CategoryName,
                                      CMMCA_Id = b.CMMCA_Id,
                                      CMMFI_ActiveFlg = a.CMMFI_ActiveFlg,
                                      CMMFI_FoodItemFlag=a.CMMFI_FoodItemFlag,
                                      //ICAI_Attachment = c.ICAI_Attachment,
                                      MI_Id = b.MI_Id

                                  }).ToArray();

                data.FooditeamDeatils = (from a in _CmsContext.FooditeamDMO
                                  from b in _CmsContext.FoodMasterCategoryDMO
                                  where (a.CMMCA_Id == b.CMMCA_Id)
                                  select new FooditeamDTO
                                  {
                                      CMMFI_FoodItemName = a.CMMFI_FoodItemName,
                                      CMMFI_FoodItemDescription = a.CMMFI_FoodItemDescription,
                                      CMMFI_UnitRate = a.CMMFI_UnitRate,
                                      CMMFI_OutofStockFlg = a.CMMFI_OutofStockFlg,
                                      CMMFI_PathURL = a.CMMFI_PathURL,
                                      CMMFI_Id = a.CMMFI_Id,
                                      CMMCA_CategoryName = b.CMMCA_CategoryName,
                                      CMMCA_Id = b.CMMCA_Id,
                                      CMMFI_ActiveFlg = a.CMMFI_ActiveFlg,
                                      CMMFI_FoodItemFlag = a.CMMFI_FoodItemFlag,
                                      //ICAI_Attachment = c.ICAI_Attachment,
                                      MI_Id = b.MI_Id

                                  }).ToArray();

                using (var cmd = _CmsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Pdaamount";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.AMST_Id
                    });
                    

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.padamountdeatils = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public FooditeamDTO savedata(FooditeamDTO data)
        {
            try
            {
                if (data.CMMFI_Id != 0)
                {
                    var result = _CmsContext.FooditeamDMO.Where(R => R.CMMFI_Id != data.CMMFI_Id && R.CMMFI_FoodItemName == data.CMMFI_FoodItemName).ToList();
                    if (result.Count > 0)
                    {
                        data.returnval = "exit";
                    }
                    else
                    {
                        var resultwo = _CmsContext.FooditeamDMO
                            .Where(R => R.CMMFI_Id == data.CMMFI_Id).FirstOrDefault();
                        if (resultwo.CMMFI_Id > 0)
                        {
                            resultwo.CMMFI_FoodItemName = data.CMMFI_FoodItemName;
                            resultwo.CMMFI_FoodItemDescription = data.CMMFI_FoodItemDescription;
                            resultwo.CMMFI_UnitRate = data.CMMFI_UnitRate;
                            resultwo.CMMFI_OutofStockFlg = data.CMMFI_OutofStockFlg;
                            resultwo.CMMFI_FoodItemFlag = data.CMMFI_FoodItemFlag;
                            resultwo.CMMFI_UpdatedBy = data.UserId;
                            resultwo.CMMFI_Updateddate = DateTime.Now;
                            _CmsContext.Update(resultwo);


                            if (data.CMMFI_Id_FilePath_Array != null && data.CMMFI_Id_FilePath_Array.Length > 0)
                            {
                                var results = _CmsContext.FooditemimageDMO.Where(a => a.CMMFI_Id == data.CMMFI_Id).ToList();
                                if (results.Count > 0)
                                {
                                    foreach (var res in results)
                                    {
                                        var result2 = _CmsContext.FooditemimageDMO.Single(a => a.CMMFI_Id == data.CMMFI_Id && a.ICAI_Id == res.ICAI_Id);
                                        _CmsContext.Remove(result2);

                                    }
                                }
                                if (data.CMMFI_Id_FilePath_Array != null && data.CMMFI_Id_FilePath_Array.Length > 0)
                                {
                                    foreach (var itm in data.CMMFI_Id_FilePath_Array)
                                    {
                                        FooditemimageDMO had = new FooditemimageDMO();
                                        had.CMMFI_Id = data.CMMFI_Id;
                                        had.ICAI_Attachment = itm.IHW_FilePath;
                                        had.ICAI_FileName = itm.FileName;
                                        had.ICAI_ActiveFlag = true;
                                        had.CreatedDate = DateTime.Now;
                                        _CmsContext.Add(had);
                                    }
                                }

                            }
                            
                            var contactExists = _CmsContext.SaveChanges();
                            if (contactExists > 0)

                            {
                                data.returnval = "update";

                            }
                            else
                            {
                                data.returnval = "Notupdate";
                            }

                        }
                    }
                }
                else
                {
                    var result = _CmsContext.FooditeamDMO.Where(R => R.CMMFI_FoodItemName == data.CMMFI_FoodItemName && R.CMMFI_UnitRate == data.CMMFI_UnitRate && R.CMMCA_Id == data.CMMCA_Id).ToList();
                    if (result.Count > 0)
                    {
                        data.returnval = "exit";
                    }
                    else
                    {
                        FooditeamDMO obj = new FooditeamDMO();
                        obj.CMMCA_Id = data.CMMCA_Id;
                        obj.CMMFI_FoodItemName = data.CMMFI_FoodItemName;
                        obj.CMMFI_FoodItemDescription = data.CMMFI_FoodItemDescription;
                        obj.CMMFI_UnitRate = data.CMMFI_UnitRate;
                        if (data.CMMFI_Id_FilePath_Array.Length > 0)
                        {
                            obj.CMMFI_PathURL = data.CMMFI_Id_FilePath_Array[0].IHW_FilePath;
                        }
                        obj.CMMFI_OutofStockFlg = data.CMMFI_OutofStockFlg;
                        obj.CMMFI_FoodItemFlag = data.CMMFI_FoodItemFlag;
                        obj.CMMFI_ActiveFlg = true;
                        obj.CMMFI_CreatedDate = DateTime.Now;
                        obj.CMMFI_CreatedBy = data.UserId;
                        obj.CMMFI_UpdatedBy = data.UserId;
                        obj.CMMFI_Updateddate = DateTime.Now;
                        _CmsContext.Add(obj);

                        if (data.CMMFI_Id_FilePath_Array != null && data.CMMFI_Id_FilePath_Array.Length > 0)
                        {
                            foreach (var itm in data.CMMFI_Id_FilePath_Array)
                            {
                                FooditemimageDMO had = new FooditemimageDMO();
                                had.CMMFI_Id = obj.CMMFI_Id;
                                had.ICAI_Attachment = itm.IHW_FilePath;
                                had.ICAI_FileName = itm.FileName;
                                had.ICAI_ActiveFlag = true;
                                had.CreatedDate = DateTime.Now;
                                _CmsContext.Add(had);
                            }
                        }
                        var contactExists = _CmsContext.SaveChanges();

                        if (contactExists > 0)
                        {
                            data.returnval = "save";

                        }
                        else
                        {
                            data.returnval = "Notsave";
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }


        public FooditeamDTO Getimagedata(FooditeamDTO data)
        {

            try
            {

                var ImageDetails = (from a in _CmsContext.FooditeamDMO
                                    from c in _CmsContext.FooditemimageDMO
                                    where (a.CMMFI_Id == c.CMMFI_Id && c.CMMFI_Id == data.CMMFI_Id && a.CMMFI_ActiveFlg == true)
                                    select new FooditeamDTO
                                    {
                                        ICAI_Attachment = c.ICAI_Attachment

                                    }).Distinct().ToList();

                data.ImageDetails = ImageDetails.ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        public FooditeamDTO GetEditdata(FooditeamDTO data)
        {

            try
            {

                var GridviewDetails = (from a in _CmsContext.FooditeamDMO
                                       from b in _CmsContext.FoodMasterCategoryDMO
                                       from c in _CmsContext.FooditemimageDMO
                                       where (a.CMMCA_Id == b.CMMCA_Id && a.CMMFI_Id == c.CMMFI_Id)
                                       select new FooditeamDTO
                                       {

                                           CMMCA_Id = a.CMMCA_Id,
                                           CMMFI_Id = a.CMMFI_Id,
                                           CMMFI_FoodItemName = a.CMMFI_FoodItemName,
                                           CMMFI_FoodItemDescription = a.CMMFI_FoodItemDescription,
                                           CMMFI_UnitRate = a.CMMFI_UnitRate,
                                           CMMFI_FoodItemFlag = a.CMMFI_FoodItemFlag,
                                           CMMCA_CategoryName = b.CMMCA_CategoryName,
                                           ICAI_Attachment = c.ICAI_Attachment

                                       }).Distinct().ToList();

                data.GridviewDetails = GridviewDetails.ToArray();

                var img = (from a in _CmsContext.FooditeamDMO
                           from b in _CmsContext.FooditemimageDMO
                           where a.CMMFI_Id == data.CMMFI_Id && a.CMMFI_Id == b.CMMFI_Id
                           select new FooditeamDTO
                           {
                               ICAI_Id = b.ICAI_Id,
                               ICAI_Attachment = b.ICAI_Attachment,
                               ICAI_FileName = b.ICAI_FileName,
                  

                           }).ToArray();
                if (img.Length > 0)
                {
                    data.attachementlist = img;
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        public FooditeamDTO deactivate(FooditeamDTO acd)
        {
            try
            {
                if (acd.CMMFI_Id > 0)
                {
                    var result = _CmsContext.FooditeamDMO.Single(t => t.CMMFI_Id == acd.CMMFI_Id);

                    if (acd.CMMFI_ActiveFlg == true)
                    {
                        result.CMMFI_ActiveFlg = false;
                    }
                    else if (acd.CMMFI_ActiveFlg == false)
                    {
                        result.CMMFI_ActiveFlg = true;
                    }

                    result.CMMFI_Updateddate = DateTime.Now;

                    _CmsContext.Update(result);
                    var flag = _CmsContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.CMMFI_ActiveFlg == true)
                        {

                            acd.returnval = "Fooditeam Activated Successfully.";
                        }
                        else
                        {
                            acd.returnval = "Fooditeam Deactivated Successfully.";
                        }
                    }
                    else
                    {
                        acd.returnval = "Fooditeam Not Activated/Deactivated";
                    }


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }

        

        public FooditeamDTO changepassword(FooditeamDTO data)
        {
            try
            {
                if (data.AMCSTW_Id != 0)
                {

                    var res = _CmsContext.Adm_M_Student_WalletPINDMO.Where(t => t.Amst_Id == data.AMST_Id && t.MI_Id == data.MI_Id && t.AMCSTW_WalletPIN == data.AMCSTW_WalletPIN && t.AMCSTW_Id != data.AMCSTW_Id).ToList();

                    if (res.Count > 0)
                    {
                        data.returnval = "Duplicate";
                    }
                    else
                    {
                        var result = _CmsContext.Adm_M_Student_WalletPINDMO.Single(t => t.AMCSTW_Id == data.AMCSTW_Id);
                        result.AMCSTW_WalletPIN = data.AMCSTW_WalletPIN;
                        result.AMCSTW_UpdatedDate = DateTime.Now;
                        _CmsContext.Update(result);

                        var contactExists = _CmsContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = "update";
                            data.studentpinlist = _CmsContext.Adm_M_Student_WalletPINDMO.ToArray();
                        }
                        else
                        {
                            data.returnval = "not update";
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



        public FooditeamDTO Createpin(FooditeamDTO data)
        {
            try
            {
                if (data.AMCSTW_Id != 0)
                {

                    var res = _CmsContext.Adm_M_Student_WalletPINDMO.Where(t => t.Amst_Id == data.AMST_Id && t.MI_Id == data.MI_Id && t.AMCSTW_WalletPIN == data.AMCSTW_WalletPIN && t.AMCSTW_Id != data.AMCSTW_Id).ToList();

                    if (res.Count > 0)
                    {
                        data.returnval = "Duplicate";
                    }
                    else
                    {
                        var result = _CmsContext.Adm_M_Student_WalletPINDMO.Single(t => t.AMCSTW_Id == data.AMCSTW_Id);
                        result.AMCSTW_WalletPIN = data.AMCSTW_WalletPIN;
                        result.AMCSTW_UpdatedDate = DateTime.Now;
                        _CmsContext.Update(result);

                        var contactExists = _CmsContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = "update";
                            data.studentpinlist = _CmsContext.Adm_M_Student_WalletPINDMO.ToArray();
                        }
                        else
                        {
                            data.returnval = "not update";
                        }



                    }
                }
                else
                {

                    var res = _CmsContext.Adm_M_Student_WalletPINDMO.Where(t => t.Amst_Id == data.AMST_Id && t.MI_Id == data.MI_Id).ToList();

                    if (res.Count > 0)
                    {
                        data.returnval = "alredy Exist";
                    }
                    else
                    {
                        Adm_M_Student_WalletPINDMO result = new Adm_M_Student_WalletPINDMO();
                        result.MI_Id = data.MI_Id;
                        result.Amst_Id = data.AMST_Id;
                        result.AMCSTW_WalletPIN = data.AMCSTW_WalletPIN;
                        result.AMCSTW_CreatedDate = DateTime.Now;
                        result.AMCSTW_CreatedBy = data.UserId;
                        _CmsContext.Add(result);
                        var contactExists = _CmsContext.SaveChanges();


                        if (contactExists > 0)
                        {
                            data.returnval = "save";
                            data.studentpinlist = _CmsContext.Adm_M_Student_WalletPINDMO.ToArray();

                        }
                        else
                        {
                            data.returnval = "Notsave";
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

        public FooditeamDTO Forgotpin(FooditeamDTO data)
        {
            try
            {
                if (data.AMCSTW_Id != 0 && data.message=="Match")
                {

                    var vduplicate = _CmsContext.AdmissionStudentDMO.Where(R => R.MI_Id == data.MI_Id && R.AMST_Id == data.AMST_Id && R.AMST_emailId == data.amst_emailid).ToList();

                    if (vduplicate.Count > 0)
                    {
                        data.returnval = "Matched";
                    }
                    else
                    {
                        data.returnval = "Not Matched";
                    }
                }
                
                if (data.AMCSTW_Id != 0 && data.message == "sucess")
                {

                    var vduplicate = _CmsContext.AdmissionStudentDMO.Where(R => R.MI_Id == data.MI_Id && R.AMST_Id == data.AMST_Id && R.AMST_emailId == data.amst_emailid).ToList();


                    if (vduplicate.Count > 0)
                    {
                        var result = _CmsContext.Adm_M_Student_WalletPINDMO.Single(t => t.AMCSTW_Id == data.AMCSTW_Id);
                        result.AMCSTW_WalletPIN = data.AMCSTW_WalletPIN;
                        result.AMCSTW_UpdatedDate = DateTime.Now;
                        _CmsContext.Update(result);

                        var contactExists = _CmsContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = "Reset sucessfully";
                            data.studentpinlist = _CmsContext.Adm_M_Student_WalletPINDMO.ToArray();
                        }
                        else
                        {
                            data.returnval = "not update";
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
