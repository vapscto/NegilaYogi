using DataAccessMsSqlServerProvider.com.vapstech.VidyaBharathi;
using DomainModel.Model.com.vapstech.VidyaBharathi;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaBharathiServiceHub.com.vaps.Services
{
    public class VBSC_MasterCompetition_CategoryIMPL : Interfaces.VBSC_MasterCompetition_CategoryInterface
    {
        public VidyaBharathiContext _VidyaBharathiContext;
        public VBSC_MasterCompetition_CategoryIMPL(VidyaBharathiContext VidyaBharathiContext)
        {
            _VidyaBharathiContext = VidyaBharathiContext;
        }
        public VBSC_MasterCompetition_CategoryDTO loaddata(int id)
        {

            VBSC_MasterCompetition_CategoryDTO dto = new VBSC_MasterCompetition_CategoryDTO();
            try
            {
                var Master_trust = (from a in _VidyaBharathiContext.Organisation
                                    from b in _VidyaBharathiContext.Institute
                                    where (a.MO_Id == b.MO_Id && b.MI_Id == id && a.MO_ActiveFlag == 1)
                                    select new VBSC_MasterCompetition_CategoryDTO
                                    {
                                        MT_Id = a.MO_Id,
                                        MO_Name = a.MO_Name
                                    }
                              ).FirstOrDefault();
                dto.MT_Id = Master_trust.MT_Id;


                dto.Master_trust = (from a in _VidyaBharathiContext.Organisation
                                    from b in _VidyaBharathiContext.Institute
                                    where (a.MO_Id == b.MO_Id && a.MO_ActiveFlag == 1 && b.MI_Id == id)
                                    select new VBSC_MasterCompetition_CategoryDTO
                                    {

                                        MT_Id = a.MO_Id,
                                        MO_Name = a.MO_Name

                                    }
                                 ).Distinct().OrderByDescending(R => R.MT_Id).ToArray();
                using (var cmd = _VidyaBharathiContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ViddyBharthi_Year";
                    cmd.CommandType = CommandType.StoredProcedure;
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
                        dto.Year = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                dto.Month = _VidyaBharathiContext.IVRM_Month_DMO.Distinct().ToArray();
                using (var cmd = _VidyaBharathiContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ViddyBharthi_CateGoryReport";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MT_Id",
                                SqlDbType.BigInt)
                    {
                        Value = 0
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
                        dto.getReport = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                dto.ClassArray = _VidyaBharathiContext.AdmissionClass.Where(R => R.MI_Id == id).Distinct().ToArray();
                using (var cmd = _VidyaBharathiContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Competition_Category_Classes";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MT_Id",
                                SqlDbType.BigInt)
                    {
                        Value = 0
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
                        dto.ClasslistArray = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                dto.returnval = "admin";
            }
            return dto;

        }
        public VBSC_MasterCompetition_CategoryDTO savedata(VBSC_MasterCompetition_CategoryDTO data)
        {
            try
            {
                if (data.VBSCMCC_Id > 0)
                {
                    var duplicate = _VidyaBharathiContext.VBSC_Master_Competition_CategoryDMO.Where(R => R.VBSCMCC_CompetitionCategory == data.VBSCMCC_CompetitionCategory && R.MT_Id == data.MT_Id && R.VBSCMCC_Id != data.VBSCMCC_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "dublicate";
                    }
                    else
                    {
                        var obj = _VidyaBharathiContext.VBSC_Master_Competition_CategoryDMO.Where(R => R.VBSCMCC_Id == data.VBSCMCC_Id).FirstOrDefault();
                        obj.MT_Id = data.MT_Id;
                        obj.VBSCMCC_CompetitionCategory = data.VBSCMCC_CompetitionCategory;
                        obj.VBSCMCC_CCDesc = data.VBSCMCC_CCDesc;
                        obj.VBSCMCC_CCAgeFlag = data.VBSCMCC_CCAgeFlag;
                        obj.VBSCMCC_CCAgeFromYear = data.VBSCMCC_CCAgeFromYear;
                        obj.VBSCMCC_CCAgeFromMonth = data.VBSCMCC_CCAgeFromMonth;
                        obj.VBSCMCC_CCAgeFromDays = data.VBSCMCC_CCAgeFromDays;
                        obj.VBSCMCC_CCAgeToYear = data.VBSCMCC_CCAgeToYear;
                        obj.VBSCMCC_CCAgeToMonth = data.VBSCMCC_CCAgeToMonth;
                        obj.VBSCMCC_CCAgeToDays = data.VBSCMCC_CCAgeToDays;
                        obj.VBSCMCC_CCWeightFlag = data.VBSCMCC_CCWeightFlag;
                        obj.VBSCMCC_CCFromWeight = data.VBSCMCC_CCFromWeight;
                        obj.VBSCMCC_CCToWeight = data.VBSCMCC_CCToWeight;
                        obj.VBSCMCC_CCClassFlg = data.VBSCMCC_CCClassFlg;
                        obj.VBSCMCC_UpdatedBy = data.User_Id;
                        obj.VBSCMCC_UpdatedDate = DateTime.Now;
                        _VidyaBharathiContext.Update(obj);
                        int i = _VidyaBharathiContext.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = "update";
                        }
                        else
                        {
                            data.returnval = "Notupdate";
                        }
                    }
                }
                else
                {
                    var duplicate = _VidyaBharathiContext.VBSC_Master_Competition_CategoryDMO.Where(R => R.VBSCMCC_CompetitionCategory == data.VBSCMCC_CompetitionCategory && R.MT_Id == data.MT_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "dublicate";
                    }
                    else
                    {
                        VBSC_Master_Competition_CategoryDMO obj = new VBSC_Master_Competition_CategoryDMO();
                        obj.MT_Id = data.MT_Id;
                        obj.VBSCMCC_CompetitionCategory = data.VBSCMCC_CompetitionCategory;
                        obj.VBSCMCC_CCDesc = data.VBSCMCC_CCDesc;
                        obj.VBSCMCC_CCAgeFlag = data.VBSCMCC_CCAgeFlag;
                        obj.VBSCMCC_CCAgeFromYear = data.VBSCMCC_CCAgeFromYear;
                        obj.VBSCMCC_CCAgeFromMonth = data.VBSCMCC_CCAgeFromMonth;
                        obj.VBSCMCC_CCAgeFromDays = data.VBSCMCC_CCAgeFromDays;
                        obj.VBSCMCC_CCAgeToYear = data.VBSCMCC_CCAgeToYear;
                        obj.VBSCMCC_CCAgeToMonth = data.VBSCMCC_CCAgeToMonth;
                        obj.VBSCMCC_CCAgeToDays = data.VBSCMCC_CCAgeToDays;
                        obj.VBSCMCC_CCWeightFlag = data.VBSCMCC_CCWeightFlag;
                        obj.VBSCMCC_CCFromWeight = data.VBSCMCC_CCFromWeight;
                        obj.VBSCMCC_CCToWeight = data.VBSCMCC_CCToWeight;
                        obj.VBSCMCC_ActiveFlag = true;
                        obj.VBSCMCC_UpdatedBy = data.User_Id;
                        obj.VBSCMCC_CreatedBy = data.User_Id;
                        obj.VBSCMCC_CreatedDate = DateTime.Now;
                        obj.VBSCMCC_UpdatedDate = DateTime.Now;
                        obj.VBSCMCC_CCClassFlg = data.VBSCMCC_CCClassFlg;
                        _VidyaBharathiContext.Add(obj);
                        int i = _VidyaBharathiContext.SaveChanges();
                        if (i > 0)
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

        //Deactivate
        public VBSC_MasterCompetition_CategoryDTO Deactivate(VBSC_MasterCompetition_CategoryDTO data)
        {
            try
            {
                if (data.VBSCMCC_Id > 0)
                {
                    var obj = _VidyaBharathiContext.VBSC_Master_Competition_CategoryDMO.Where(R => R.VBSCMCC_Id == data.VBSCMCC_Id).FirstOrDefault();
                    if (obj.VBSCMCC_ActiveFlag == true)
                    {
                        obj.VBSCMCC_ActiveFlag = false;
                    }
                    else
                    {
                        obj.VBSCMCC_ActiveFlag = true;
                    }
                    _VidyaBharathiContext.Update(obj);
                    int i = _VidyaBharathiContext.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = "active";
                    }
                    else
                    {
                        data.returnval = "notactive";
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
        //Organsation
        public VBSC_MasterCompetition_CategoryDTO Organsation(VBSC_MasterCompetition_CategoryDTO data)
        {
            try
            {

                using (var cmd = _VidyaBharathiContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ViddyBharthi_CateGoryReport";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MT_Id",
                                SqlDbType.BigInt)
                    {
                        Value = data.MT_Id
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
                        data.getReport = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
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
        public Master_Competition_Category_ClassesDTO savedataCl(Master_Competition_Category_ClassesDTO data)
        {
            try
            {
                int savecount = 0;
                int duplicatecount = 0;

                if (data.VBSCMCCCL_Id > 0)
                {
                    var duplicate = _VidyaBharathiContext.MasterCompetitionCategory_ClassesDMO.Where(R => R.ASMCL_ID == data.ASMCL_ID && R.VBSCMCCCL_Id != data.VBSCMCCCL_Id && R.VBSCMCC_Id == data.VBSCMCC_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "dublicate";
                    }
                    else
                    {
                        var obj = _VidyaBharathiContext.MasterCompetitionCategory_ClassesDMO.Where(R => R.VBSCMCCCL_Id == data.VBSCMCCCL_Id).FirstOrDefault();
                        obj.VBSCMCC_Id = data.VBSCMCC_Id;
                        obj.VBSCMCC_UpdatedBy = data.User_Id;
                        obj.VBSCMCC_UpdatedDate = DateTime.Now;
                        _VidyaBharathiContext.Update(obj);
                        int i = _VidyaBharathiContext.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = "update";
                        }
                        else
                        {
                            data.returnval = "Notupdate";
                        }
                    }
                }
                else
                {
                    foreach (var i in data.clalists)
                    {
                        var duplicate = _VidyaBharathiContext.MasterCompetitionCategory_ClassesDMO.Where(P => P.ASMCL_ID == i.ASMCL_ID && P.VBSCMCC_Id == data.VBSCMCC_Id).ToList();
                        if (duplicate.Count > 0)
                        {
                            duplicatecount += 1;
                        }
                        else
                        {
                            MasterCompetitionCategory_ClassesDMO obj = new MasterCompetitionCategory_ClassesDMO();
                            obj.VBSCMCC_Id = data.VBSCMCC_Id;
                            obj.ASMCL_ID = i.ASMCL_ID;
                            obj.VBSCMCC_ActiveFlag = true;
                            obj.VBSCMCC_UpdatedBy = data.User_Id;
                            obj.VBSCMCC_CreatedBy = data.User_Id;
                            obj.VBSCMCC_CreatedDate = DateTime.Now;
                            obj.VBSCMCC_UpdatedDate = DateTime.Now;

                            _VidyaBharathiContext.Add(obj);

                        }
                    }

                    int R = _VidyaBharathiContext.SaveChanges();
                    if (R > 0)
                    {
                        savecount = R;
                        data.returnval = "save";

                    }
                    else
                    {
                        data.returnval = "Notsave";
                    }
                }
                data.savecount = savecount;
                data.duplicatecount = duplicatecount;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }

        public Master_Competition_Category_ClassesDTO DeactivateCl(Master_Competition_Category_ClassesDTO data)
        {
            try
            {
                if (data.VBSCMCCCL_Id > 0)
                {
                    var obj = _VidyaBharathiContext.MasterCompetitionCategory_ClassesDMO.Where(R => R.VBSCMCCCL_Id == data.VBSCMCCCL_Id).FirstOrDefault();
                    if (obj.VBSCMCC_ActiveFlag == true)
                    {
                        obj.VBSCMCC_ActiveFlag = false;
                    }
                    else
                    {
                        obj.VBSCMCC_ActiveFlag = true;
                    }
                    _VidyaBharathiContext.Update(obj);
                    int i = _VidyaBharathiContext.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = "active";
                    }
                    else
                    {
                        data.returnval = "notactive";
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

        //levels
        public VBSC_Master_Competition_Category_LevelsDTO getdata(int id)
        {

            VBSC_Master_Competition_Category_LevelsDTO dto = new VBSC_Master_Competition_Category_LevelsDTO();
            try
            {
                var Master_trust = (from a in _VidyaBharathiContext.Organisation
                                    from b in _VidyaBharathiContext.Institute
                                    where (a.MO_Id == b.MO_Id && b.MI_Id == id && a.MO_ActiveFlag == 1)
                                    select new VBSC_Master_Competition_Category_LevelsDTO
                                    {
                                        MT_Id = a.MO_Id,
                                        MO_Name = a.MO_Name
                                    }
                             ).FirstOrDefault();
                dto.MT_Id = Master_trust.MT_Id;
                dto.Competetioncategory = _VidyaBharathiContext.VBSC_Master_Competition_CategoryDMO.Where(R => R.MT_Id == dto.MT_Id).Distinct().ToArray();
               
                dto.Competelevelc = _VidyaBharathiContext.VBSC_Master_Competition_LevelDMO.Where(R => R.MT_Id == dto.MT_Id && R.VBSCMCL_ActiveFlag==true).Distinct().ToArray();
                //ViddyBharthi_Year
                using (var cmd = _VidyaBharathiContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ViddyBharthi_Categorylevel";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MT_Id",
                                SqlDbType.BigInt)
                    {
                        Value = dto.MT_Id
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
                        dto.getReport = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                dto.returnval = "admin";
            }
            return dto;

        }
        public VBSC_Master_Competition_Category_LevelsDTO savedataVCl(VBSC_Master_Competition_Category_LevelsDTO data)
        {
            try
            {
                int savecount = 0;
                int duplicatecount = 0;
                if (data.VBSCMCCCLE_Id > 0)
                {
                    var duplicate = _VidyaBharathiContext.VBSC_Master_Competition_Category_LevelsDMO.Where(R => R.VBSCMCC_Id == data.VBSCMCC_Id && R.VBSCMCL_Id == data.VBSCMCL_Id && R.VBSCMCCCLE_Id != data.VBSCMCCCLE_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "dublicate";
                    }
                    else
                    {
                        var obj = _VidyaBharathiContext.VBSC_Master_Competition_Category_LevelsDMO.Where(R => R.VBSCMCCCLE_Id == data.VBSCMCCCLE_Id).FirstOrDefault();
                        obj.VBSCMCC_Id = data.VBSCMCC_Id;
                        obj.VBSCMCL_Id = data.VBSCMCL_Id;
                        obj.VBSCMCCCLE_ActiveFlag = true;
                        obj.VBSCMCCCLE_UpdatedDate = DateTime.Now;
                        obj.VBSCMCCCLE_UpdatedBy = data.User_Id;
                        _VidyaBharathiContext.Update(obj);
                        int i = _VidyaBharathiContext.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = "update";
                        }
                        else
                        {
                            data.returnval = "Notupdate";
                        }
                    }
                }
                else
                {
                    //var Master_trust = (from a in _VidyaBharathiContext.Organisation
                    //                    from b in _VidyaBharathiContext.Institute
                    //                    where (a.MO_Id == b.MO_Id && b.MI_Id == data.MI_Id && a.MO_ActiveFlag == 1)
                    //                    select new VBSC_Master_Competition_Category_LevelsDTO
                    //                    {
                    //                        MT_Id = a.MO_Id,
                    //                        MO_Name = a.MO_Name
                    //                    }
                    //         ).FirstOrDefault();
                    //data.MT_Id = Master_trust.MT_Id;
                    foreach (var p in data.Category_Level)
                    {
                        var duplicate = _VidyaBharathiContext.VBSC_Master_Competition_Category_LevelsDMO.Where(R => R.VBSCMCC_Id == data.VBSCMCC_Id && R.VBSCMCL_Id == data.VBSCMCL_Id).ToList();
                        if (duplicate.Count > 0)
                        {
                            data.returnval = "dublicate";
                            duplicatecount += 1;
                        }
                        else
                        {

                            VBSC_Master_Competition_Category_LevelsDMO obj = new VBSC_Master_Competition_Category_LevelsDMO();
                            obj.VBSCMCC_Id = data.VBSCMCC_Id;
                            obj.VBSCMCL_Id = p.VBSCMCL_Id;
                            obj.VBSCMCCCLE_ActiveFlag = true;
                            obj.VBSCMCCCLE_CreatedDate = DateTime.Now;
                            obj.VBSCMCCCLE_UpdatedDate = DateTime.Now;
                            obj.VBSCMCCCLE_CreatedBy = data.User_Id;
                            obj.VBSCMCCCLE_UpdatedBy = data.User_Id;

                            _VidyaBharathiContext.Add(obj);

                        }
                    }

                    int i = _VidyaBharathiContext.SaveChanges();
                    if (i > 0)
                    {
                        savecount = i;
                        data.returnval = "save";
                    }
                    else
                    {

                        data.returnval = "Notsave";
                    }
                }
                data.savecount = savecount;
                data.duplicatecount = duplicatecount;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }

        //Deactivate
        public VBSC_Master_Competition_Category_LevelsDTO DeactivateVCl(VBSC_Master_Competition_Category_LevelsDTO data)
        {
            try
            {
                if (data.VBSCMCCCLE_Id > 0)
                {
                    var obj = _VidyaBharathiContext.VBSC_Master_Competition_Category_LevelsDMO.Where(R => R.VBSCMCCCLE_Id == data.VBSCMCCCLE_Id).FirstOrDefault();
                    if (obj.VBSCMCCCLE_ActiveFlag == true)
                    {
                        obj.VBSCMCCCLE_ActiveFlag = false;
                    }
                    else
                    {
                        obj.VBSCMCCCLE_ActiveFlag = true;
                    }
                    _VidyaBharathiContext.Update(obj);
                    int i = _VidyaBharathiContext.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = "active";
                    }
                    else
                    {
                        data.returnval = "notactive";
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

    }
}
