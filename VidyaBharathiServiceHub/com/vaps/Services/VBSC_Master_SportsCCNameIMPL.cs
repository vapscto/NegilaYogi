using DataAccessMsSqlServerProvider.com.vapstech.VidyaBharathi;
using DomainModel.Model.com.vapstech.VidyaBharathi;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaBharathiServiceHub.com.vaps.Services
{
    public class VBSC_Master_SportsCCNameIMPL : Interfaces.VBSC_Master_SportsCCNameInterface
    {
        public VidyaBharathiContext _VidyaBharathiContext;
       
        public VBSC_Master_SportsCCNameIMPL(VidyaBharathiContext VidyaBharathiContext)
        {
            _VidyaBharathiContext = VidyaBharathiContext;

        }

        public VBSC_Master_SportsCCNameDTO getloaddata(VBSC_Master_SportsCCNameDTO data)
        {
            try
            {

                var Master_trust = (from a in _VidyaBharathiContext.Organisation
                                    from b in _VidyaBharathiContext.Institute
                                    where (a.MO_Id == b.MO_Id && b.MI_Id == data.MI_Id && a.MO_ActiveFlag == 1)
                                    select new VBSC_Master_SportsCCGroupNameDTO
                                    {
                                        MT_Id = a.MO_Id,
                                        MO_Name = a.MO_Name
                                    }
                             ).FirstOrDefault();

                data.MT_Id = Master_trust.MT_Id;

                data.Master_trust = (from a in _VidyaBharathiContext.Organisation
                                     from b in _VidyaBharathiContext.Institute
                                     where (a.MO_Id == b.MO_Id && b.MI_Id==data.MI_Id && a.MO_ActiveFlag == 1)
                                     select new VBSC_Master_SportsCCNameDTO
                                     {
                                         MT_Id = a.MO_Id,
                                         MO_Name = a.MO_Name
                                     }
                                ).Distinct().OrderByDescending(R => R.MT_Id).ToArray();



                using (var cmd = _VidyaBharathiContext.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "Getreport_sportsnames";                    cmd.CommandType = CommandType.StoredProcedure;                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }                        data.get_customer = retObject.ToArray();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                }




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.message = "admin";
            }
            return data;
        }
        
        public VBSC_Master_SportsCCNameDTO getInstitute(VBSC_Master_SportsCCNameDTO data)
        {
            try
            {
                data.getGroupName = _VidyaBharathiContext.VBSC_Master_SportsCCGroupNameDMO.Where(a => a.VBSCMSCCG_ActiveFlag == true && a.VBSCMSCCG_SCCFlag == data.VBSCMSCCG_SCCFlag).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        
        public VBSC_Master_SportsCCNameDTO savedetails(VBSC_Master_SportsCCNameDTO data)
        {
            try
            {

                if (data.VBSCMSCC_Id != 0)
                {
                    var res = _VidyaBharathiContext.VBSC_Master_SportsCCNameDMO.Where(t => t.MT_Id == data.MT_Id && t.VBSCMSCC_SportsCCName == data.VBSCMSCC_SportsCCName && t.VBSCMSCC_SGFlag == data.VBSCMSCC_SGFlag && t.VBSCMSCC_Id != data.VBSCMSCC_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _VidyaBharathiContext.VBSC_Master_SportsCCNameDMO.Single(t => t.VBSCMSCC_Id == data.VBSCMSCC_Id);
                        result.MT_Id = data.MT_Id;
                        result.VBSCMSCCG_Id = data.VBSCMSCCG_Id;
                        result.VBSCMSCC_SportsCCName = data.VBSCMSCC_SportsCCName;
                        result.VBSCMSCC_SportsCCDesc = data.VBSCMSCC_SportsCCDesc;
                        result.VBSCMSCC_SGFlag = data.VBSCMSCC_SGFlag;
                        result.VBSCMSCC_NoOfMembers = data.VBSCMSCC_NoOfMembers;
                        result.VBSCMSCC_RecHighLowFlag = data.VBSCMSCC_RecHighLowFlag;
                        result.VBSCMSCC_RecInfo = data.VBSCMSCC_RecInfo;
                        result.VBSCMSCC_GenderFlg = data.VBSCMSCC_GenderFlg;
                        result.VBSCMSCC_ActiveFlag = true;
                        result.VBSCMSCC_UpdatedDate = DateTime.Now;
                        result.VBSCMSCC_CreatedDate = DateTime.Now;
                        _VidyaBharathiContext.Update(result);
                        
                        var contactExists = _VidyaBharathiContext.SaveChanges();
                        if (contactExists > 0)
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
                    var Master_trust = (from a in _VidyaBharathiContext.Organisation
                                        from b in _VidyaBharathiContext.Institute
                                        where (a.MO_Id == b.MO_Id && b.MI_Id == data.MI_Id && a.MO_ActiveFlag == 1)
                                        select new VBSC_Master_SportsCCGroupNameDTO
                                        {
                                            MT_Id = a.MO_Id,
                                            MO_Name = a.MO_Name
                                        }
                           ).FirstOrDefault();
                    data.MT_Id = Master_trust.MT_Id;

                    var res = _VidyaBharathiContext.VBSC_Master_SportsCCNameDMO.Where(t => t.MT_Id == data.MT_Id && t.VBSCMSCC_SportsCCName == data.VBSCMSCC_SportsCCName && t.VBSCMSCC_SGFlag == data.VBSCMSCC_SGFlag).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        VBSC_Master_SportsCCNameDMO customer = new VBSC_Master_SportsCCNameDMO();
                        customer.MT_Id = data.MT_Id;
                        customer.VBSCMSCCG_Id = data.VBSCMSCCG_Id;
                        customer.VBSCMSCC_SportsCCName = data.VBSCMSCC_SportsCCName;
                        customer.VBSCMSCC_SportsCCDesc = data.VBSCMSCC_SportsCCDesc;
                        customer.VBSCMSCC_SGFlag = data.VBSCMSCC_SGFlag;
                        customer.VBSCMSCC_NoOfMembers = data.VBSCMSCC_NoOfMembers;
                        customer.VBSCMSCC_RecHighLowFlag = data.VBSCMSCC_RecHighLowFlag;
                        customer.VBSCMSCC_RecInfo = data.VBSCMSCC_RecInfo;
                        customer.VBSCMSCC_GenderFlg = data.VBSCMSCC_GenderFlg;
                        customer.VBSCMSCC_ActiveFlag = true;
                        customer.VBSCMSCC_UpdatedDate = DateTime.Now;
                        customer.VBSCMSCC_CreatedDate = DateTime.Now;
                        _VidyaBharathiContext.Add(customer);

                        var contactExists = _VidyaBharathiContext.SaveChanges();
                        if (contactExists > 0)
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
                data.message = "admin";
            }
            return data;
        }

        public VBSC_Master_SportsCCNameDTO deactive(VBSC_Master_SportsCCNameDTO data)
        {
            try
            {
                var result = _VidyaBharathiContext.VBSC_Master_SportsCCNameDMO.Single(t => t.VBSCMSCC_Id == data.VBSCMSCC_Id);

                if (result.VBSCMSCC_ActiveFlag == true)
                {
                    result.VBSCMSCC_ActiveFlag = false;
                }
                else if (result.VBSCMSCC_ActiveFlag == false)
                {
                    result.VBSCMSCC_ActiveFlag = true;
                }
                result.VBSCMSCC_UpdatedDate = DateTime.Now;
                _VidyaBharathiContext.Update(result);
                int returnval = _VidyaBharathiContext.SaveChanges();
                if (returnval > 0)
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
                data.message = "admin";
            }
            return data;
        }


    }
}
