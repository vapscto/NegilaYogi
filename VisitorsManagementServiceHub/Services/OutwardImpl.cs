using System;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.VisitorsManagement;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.VisitorsManagement;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitorsManagementServiceHub.Interfaces;
using CommonLibrary;

namespace VisitorsManagementServiceHub.Services
{
    public class OutwardImpl : OutwardInterface
    {
        public VisitorsManagementContext visctxt;
        public DomainModelMsSqlServerContext _db;
        public DomainModelMsSqlServerContext _context;
        public OutwardImpl(VisitorsManagementContext context, DomainModelMsSqlServerContext db, DomainModelMsSqlServerContext contx)
        {
            visctxt = context;
            _db = db;
            _context = contx;
        }

        public OutwardDTO getDetails(OutwardDTO data)
        {
            try
            {
                data.emplist = (from t in visctxt.MasterEmployee
                                where( t.MI_Id == data.MI_Id && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false)
                                select new OutwardDTO {
                                    HRME_EmployeeFirstName = t.HRME_EmployeeFirstName + (string.IsNullOrEmpty(t.HRME_EmployeeMiddleName) ? "" : ' ' + t.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(t.HRME_EmployeeLastName) ? "" : ' ' + t.HRME_EmployeeLastName),
                                    HRME_Id=t.HRME_Id,
                                }).Distinct().OrderBy(T => T.HRME_Id).ToArray();

                data.getdata =(from a in visctxt.FO_Outward_DMO
                               from t in visctxt.MasterEmployee
                               where(a.MI_Id == t.MI_Id && a.FOOUT_DispatachedBy==t.HRME_Id && a.MI_Id==data.MI_Id )
                               select new OutwardDTO {
                                   
                                   HRME_EmployeeFirstName = t.HRME_EmployeeFirstName + (string.IsNullOrEmpty(t.HRME_EmployeeMiddleName) ? "" : ' ' + t.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(t.HRME_EmployeeLastName) ? "" : ' ' + t.HRME_EmployeeLastName),
                                   HRME_Id = t.HRME_Id,
                                   FOOUT_Id=a.FOOUT_Id,
                                   FOOUT_OutwardNo=a.FOOUT_OutwardNo,
                                   FOOUT_DateTime=a.FOOUT_DateTime,
                                   FOOUT_Discription=a.FOOUT_Discription,
                                   FOOUT_From=a.FOOUT_From,
                                   FOOUT_To=a.FOOUT_To,
                                   FOOUT_Address=a.FOOUT_Address,
                                   FOOUT_PhoneNo=a.FOOUT_PhoneNo,
                                   FOOUT_EmailId=a.FOOUT_EmailId,
                                   FOOUT_DispatachedBy = a.FOOUT_DispatachedBy,
                                   FOOUT_DispatchedThrough=a.FOOUT_DispatchedThrough,
                                   FOOUT_DispatchedDeatils=a.FOOUT_DispatchedDeatils,
                                   FOOUT_DispatchedPhNo=a.FOOUT_DispatchedPhNo,
                                   FOOUT_ActiveFlag=a.FOOUT_ActiveFlag,

                               }).Distinct().OrderBy(t => t.FOOUT_Id).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public OutwardDTO EditDetails(OutwardDTO id)
        {
            OutwardDTO resp = new OutwardDTO();
            try
            {
                var editData = (from a in visctxt.FO_Outward_DMO
                                from t in visctxt.MasterEmployee
                                where (a.MI_Id == t.MI_Id && a.FOOUT_DispatachedBy == t.HRME_Id && a.MI_Id == id.MI_Id && a.FOOUT_Id==id.FOOUT_Id)
                                select new OutwardDTO
                                {

                                    HRME_EmployeeFirstName = t.HRME_EmployeeFirstName + (string.IsNullOrEmpty(t.HRME_EmployeeMiddleName) ? "" : ' ' + t.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(t.HRME_EmployeeLastName) ? "" : ' ' + t.HRME_EmployeeLastName),
                                    HRME_Id = t.HRME_Id,
                                    FOOUT_Id = a.FOOUT_Id,
                                    FOOUT_OutwardNo = a.FOOUT_OutwardNo,
                                    FOOUT_DateTime = a.FOOUT_DateTime,
                                    FOOUT_Discription = a.FOOUT_Discription,
                                    FOOUT_From = a.FOOUT_From,
                                    FOOUT_To = a.FOOUT_To,
                                    FOOUT_Address = a.FOOUT_Address,
                                    FOOUT_PhoneNo = a.FOOUT_PhoneNo,
                                    FOOUT_EmailId = a.FOOUT_EmailId,
                                    FOOUT_DispatachedBy = a.FOOUT_DispatachedBy,
                                    FOOUT_DispatchedThrough = a.FOOUT_DispatchedThrough,
                                    FOOUT_DispatchedDeatils = a.FOOUT_DispatchedDeatils,
                                    FOOUT_DispatchedPhNo = a.FOOUT_DispatchedPhNo,
                                    FOOUT_ActiveFlag = a.FOOUT_ActiveFlag,

                                }).ToList();
                resp.editDetails = editData.ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return resp;
        }
        public OutwardDTO deactivate(OutwardDTO obj)
        {
            try
            {
                var result = visctxt.FO_Outward_DMO.Single(t => t.FOOUT_Id == obj.FOOUT_Id && t.MI_Id == obj.MI_Id);

                if (result.FOOUT_ActiveFlag == true)
                {
                    result.FOOUT_ActiveFlag = false;
                }
                else if (result.FOOUT_ActiveFlag == false)
                {
                    result.FOOUT_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                visctxt.Update(result);
                int rowAffected = visctxt.SaveChanges();
                if (rowAffected > 0)
                {
                    obj.returnval2 = true;
                }
                else
                {
                    obj.returnval2 = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }
        public OutwardDTO saveData(OutwardDTO data)
        {
            try
            {
                Master_NumberingDTO check = new Master_NumberingDTO();
                data.transnumbconfigurationsettingsss = check;
                List<Master_Numbering> MM = new List<Master_Numbering>();
                MM = _context.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "VisitorManagement2").ToList();
                if (MM.Count() > 0)
                {
                    data.transnumbconfigurationsettingsss.IMN_AutoManualFlag = MM.FirstOrDefault().IMN_AutoManualFlag;
                    data.transnumbconfigurationsettingsss.IMN_DuplicatesFlag = MM.FirstOrDefault().IMN_DuplicatesFlag;
                    data.transnumbconfigurationsettingsss.IMN_Flag = MM.FirstOrDefault().IMN_Flag;
                    data.transnumbconfigurationsettingsss.IMN_Id = MM.FirstOrDefault().IMN_Id;
                    data.transnumbconfigurationsettingsss.IMN_PrefixAcadYearCode = MM.FirstOrDefault().IMN_PrefixAcadYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixCalYearCode = MM.FirstOrDefault().IMN_PrefixCalYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixFinYearCode = MM.FirstOrDefault().IMN_PrefixFinYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixParticular = MM.FirstOrDefault().IMN_PrefixParticular;
                    data.transnumbconfigurationsettingsss.IMN_RestartNumFlag = MM.FirstOrDefault().IMN_RestartNumFlag;
                    data.transnumbconfigurationsettingsss.IMN_StartingNo = MM.FirstOrDefault().IMN_StartingNo;
                    data.transnumbconfigurationsettingsss.IMN_SuffixAcadYearCode = MM.FirstOrDefault().IMN_SuffixAcadYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixCalYearCode = MM.FirstOrDefault().IMN_SuffixCalYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixFinYearCode = MM.FirstOrDefault().IMN_SuffixFinYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixParticular = MM.FirstOrDefault().IMN_SuffixParticular;
                    data.transnumbconfigurationsettingsss.IMN_WidthNumeric = MM.FirstOrDefault().IMN_WidthNumeric;
                    data.transnumbconfigurationsettingsss.IMN_ZeroPrefixFlag = MM.FirstOrDefault().IMN_ZeroPrefixFlag;
                    data.transnumbconfigurationsettingsss.MI_Id = MM.FirstOrDefault().MI_Id;
                }


                if (data.FOOUT_Id>0)
                {
                    var Duplicate = visctxt.FO_Outward_DMO.Where(t => t.MI_Id == data.MI_Id && t.FOOUT_Id!=data.FOOUT_Id && t.FOOUT_Discription == data.FOOUT_Discription && t.FOOUT_DispatachedBy == data.FOOUT_DispatachedBy && t.FOOUT_DispatchedDeatils == data.FOOUT_DispatchedDeatils && t.FOOUT_DispatchedPhNo == data.FOOUT_DispatchedPhNo && t.FOOUT_DispatchedThrough == data.FOOUT_DispatchedThrough && t.FOOUT_EmailId == data.FOOUT_EmailId && t.FOOUT_PhoneNo == data.FOOUT_PhoneNo && t.FOOUT_From == data.FOOUT_From && t.FOOUT_To == data.FOOUT_To).ToList();
                    if (Duplicate.Count>0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = visctxt.FO_Outward_DMO.Where(t => t.MI_Id == data.MI_Id && t.FOOUT_Id == data.FOOUT_Id).Single();

                       // update.FOOUT_Id = data.FOOUT_Id;                       
                        update.FOOUT_DateTime = data.FOOUT_DateTime;
                        update.FOOUT_Discription = data.FOOUT_Discription;
                        update.FOOUT_From = data.FOOUT_From;
                        update.FOOUT_To = data.FOOUT_To;
                        update.FOOUT_Address = data.FOOUT_Address;
                        update.FOOUT_PhoneNo = data.FOOUT_PhoneNo;
                        update.FOOUT_EmailId = data.FOOUT_EmailId;
                        update.FOOUT_DispatachedBy = data.FOOUT_DispatachedBy;
                        update.FOOUT_DispatchedThrough = data.FOOUT_DispatchedThrough;
                        update.FOOUT_DispatchedDeatils = data.FOOUT_DispatchedDeatils;
                        update.FOOUT_DispatchedPhNo = data.FOOUT_DispatchedPhNo;
                        update.UpdatedDate = DateTime.Now;
                        update.FOOUT_UpdatedBy = data.UserId;

                        visctxt.Update(update);

                        int rowAffected = visctxt.SaveChanges();
                        if (rowAffected > 0)
                        {
                            data.returnval = "Update";
                        }
                        else
                        {
                            data.returnval = "Not Update";
                        }

                    }
                }
                else
                {
                    if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                    {
                        GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                        data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                        data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                        data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);

                    }

                    var Duplicate = visctxt.FO_Outward_DMO.Where(t => t.MI_Id == data.MI_Id && t.FOOUT_Discription==data.FOOUT_Discription && t.FOOUT_DispatachedBy==data.FOOUT_DispatachedBy && t.FOOUT_DispatchedDeatils==data.FOOUT_DispatchedDeatils && t.FOOUT_DispatchedPhNo==data.FOOUT_DispatchedPhNo && t.FOOUT_DispatchedThrough==data.FOOUT_DispatchedThrough && t.FOOUT_EmailId==data.FOOUT_EmailId && t.FOOUT_PhoneNo==data.FOOUT_PhoneNo && t.FOOUT_From ==data.FOOUT_From && t.FOOUT_To==data.FOOUT_To).ToList();
                    if (Duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        FO_Outward_DMO obj = new FO_Outward_DMO();
                        obj.FOOUT_Id=data.FOOUT_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.FOOUT_OutwardNo = data.trans_id;
                        obj.FOOUT_DateTime = data.FOOUT_DateTime;
                        obj.FOOUT_Discription = data.FOOUT_Discription;
                        obj.FOOUT_From = data.FOOUT_From;
                        obj.FOOUT_To = data.FOOUT_To;
                        obj.FOOUT_Address = data.FOOUT_Address;
                        obj.FOOUT_PhoneNo = data.FOOUT_PhoneNo;
                        obj.FOOUT_EmailId = data.FOOUT_EmailId;
                        obj.FOOUT_DispatachedBy = data.FOOUT_DispatachedBy;
                        obj.FOOUT_DispatchedThrough = data.FOOUT_DispatchedThrough;
                        obj.FOOUT_DispatchedDeatils = data.FOOUT_DispatchedDeatils;
                        obj.FOOUT_DispatchedPhNo = data.FOOUT_DispatchedPhNo;
                        obj.FOOUT_ActiveFlag = true;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        obj.FOOUT_CreatedBy = data.UserId;
                        obj.FOOUT_UpdatedBy = data.UserId;

                        visctxt.Add(obj);

                        int rowAffected = visctxt.SaveChanges();

                        if(rowAffected>0)
                        {
                            data.returnval = "Save"; 
                            var outward1 = visctxt.FO_Outward_DMO.OrderByDescending(d => d.FOOUT_Id).First();
                               data.outward = visctxt.FO_Outward_DMO.Where(d => d.FOOUT_Id == outward1.FOOUT_Id).ToArray();

                        }
                        else
                        {
                            data.returnval = "Not Save"; 
                        }
                    }
                }
                data.institution = _db.Institution.Where(i => i.MI_Id == data.MI_Id).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        
    }
}
