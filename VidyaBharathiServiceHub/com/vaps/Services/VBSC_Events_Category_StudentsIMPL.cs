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
    public class VBSC_Events_Category_StudentsIMPL : Interfaces.VBSC_Events_Category_StudentsInterface
    {
        //VidyaBharathiContext
        public VidyaBharathiContext  _VidyaBharathiContext;
        public VBSC_Events_Category_StudentsIMPL(VidyaBharathiContext VidyaBharathiContext)
        {
            _VidyaBharathiContext = VidyaBharathiContext;
        }
        public VBSC_Events_Category_StudentsDTO loaddata(VBSC_Events_Category_StudentsDTO data)
        { 
            try
            {
                data.geteventCategory = (from a in  _VidyaBharathiContext.VBSC_Master_EventsDMO
                                         from b in  _VidyaBharathiContext.VBSC_Events_CategoryDMO
                                         where (a.VBSCME_Id==b.VBSCME_Id && b.VBSCECT_ActiveFlag)
                                         select new VBSC_Events_Category_StudentsDTO
                                         {
                                             VBSCME_Id=a.VBSCME_Id,
                                             VBSCME_EventName = a.VBSCME_EventName,

                                         }).Distinct().OrderByDescending(m => m.VBSCME_Id).ToArray();

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }


        public VBSC_Events_Category_StudentsDTO savedata(VBSC_Events_Category_StudentsDTO data)
        {
            try
            {
                if (data.VBSCECTSTU_Id != 0)
                {
                    var res = _VidyaBharathiContext.VBSC_Events_Category_StudentsDMO.Where(t => t.VBSCECTSTU_Id != data.VBSCECTSTU_Id ).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _VidyaBharathiContext.VBSC_Events_Category_StudentsDMO.Single(t => t.VBSCECTSTU_Id == data.VBSCECTSTU_Id);

                        result.VBSCECTSTU_Id = data.VBSCECTSTU_Id;
                        result.VBSCECT_Id = data.VBSCECT_Id;
                        result.AMST_ID = data.AMST_ID;
                        result.VBSCECTSTU_Rank = data.VBSCECTSTU_Rank;
                        result.VBSCECTSTU_Points = data.VBSCECTSTU_Points;
                        result.VBSCECTSTU_RecordBrokenFlag = data.VBSCECTSTU_RecordBrokenFlag;
                        result.VBSCECTSTU_Remarks = data.VBSCECTSTU_Remarks;
                        result.VBSCECTSTU_ActiveFlag = true;
                        result.VBSCECTSTU_UpdatedDate = DateTime.Now;
                        result.VBSCECTSTU_UpdatedDate = DateTime.Now;
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
                    var res = _VidyaBharathiContext.VBSC_Events_Category_StudentsDMO.Where(t => t.VBSCECTSTU_Id == data.VBSCECTSTU_Id).ToList();

                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        VBSC_Events_Category_StudentsDMO tax = new VBSC_Events_Category_StudentsDMO();
                        tax.VBSCECTSTU_Id = data.VBSCECTSTU_Id;
                        tax.VBSCECT_Id = data.VBSCECT_Id;
                        tax.AMST_ID = data.AMST_ID;
                        tax.VBSCECTSTU_Rank = data.VBSCECTSTU_Rank;
                        tax.VBSCECTSTU_Points = data.VBSCECTSTU_Points;
                        tax.VBSCECTSTU_RecordBrokenFlag = data.VBSCECTSTU_RecordBrokenFlag;
                        tax.VBSCECTSTU_Remarks = data.VBSCECTSTU_Remarks;
                        tax.VBSCECTSTU_ActiveFlag = true;
                        tax.VBSCECTSTU_UpdatedDate = DateTime.Now;
                        tax.VBSCECTSTU_UpdatedDate = DateTime.Now;
           
                        _VidyaBharathiContext.Add(tax);

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
                data.message = "Error";
           
            }
            return data;
        }


        public VBSC_Events_Category_StudentsDTO deactive(VBSC_Events_Category_StudentsDTO data)
        {
            try
            {
                var result = _VidyaBharathiContext.VBSC_Events_Category_StudentsDMO.Single(t => t.VBSCECTSTU_Id == data.VBSCECTSTU_Id);

                if (result.VBSCECTSTU_ActiveFlag == true)
                {
                    result.VBSCECTSTU_ActiveFlag = false;
                }
                else if (result.VBSCECTSTU_ActiveFlag == false)
                {
                    result.VBSCECTSTU_ActiveFlag = true;
                }
                result.VBSCECTSTU_UpdatedDate = DateTime.Now;
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
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }



    }
}
