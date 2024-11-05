using AutoMapper;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.VisitorsManagement;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.VisitorsManagement;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitorsManagementServiceHub.Interfaces;

namespace VisitorsManagementServiceHub.Services
{
    public class InwardImpl : InwardInterface
    {
        public VisitorsManagementContext visctxt;
        public DomainModelMsSqlServerContext _db;
        public DomainModelMsSqlServerContext _context;
        public InwardImpl(VisitorsManagementContext context, DomainModelMsSqlServerContext db, DomainModelMsSqlServerContext pad)
        {
            visctxt = context;
            _db = db;
            _context = pad;
        }

        public InwardDTO getDetails(InwardDTO data)
        {
            try
            {
                data.emplist = (from t in visctxt.MasterEmployee
                                where (t.MI_Id == data.MI_Id && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false)
                                select new InwardDTO
                                {
                                    HRME_EmployeeFirstName = t.HRME_EmployeeFirstName + (string.IsNullOrEmpty(t.HRME_EmployeeMiddleName) ? "" : ' ' + t.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(t.HRME_EmployeeLastName) ? "" : ' ' + t.HRME_EmployeeLastName),
                                    HRME_Id = t.HRME_Id,
                                }).Distinct().OrderBy(T => T.HRME_Id).ToArray();

         
                var listdata = (from a in visctxt.FO_Inward_DMO
                                from b in visctxt.MasterEmployee
                                where (a.MI_Id == b.MI_Id && a.FOIN_To == b.HRME_Id && a.MI_Id == data.MI_Id && b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false)
                                select new InwardDTO
                                {
                                    FOIN_Id = a.FOIN_Id,
                                    MI_Id = a.MI_Id,
                                    FOIN_InwardNo = a.FOIN_InwardNo,
                                    FOIN_DateTime = a.FOIN_DateTime,
                                    FOIN_From = a.FOIN_From,
                                    FOIN_Adddress = a.FOIN_Adddress,
                                    FOIN_ContactPerson = a.FOIN_ContactPerson,
                                    FOIN_PhoneNo = a.FOIN_PhoneNo,
                                    FOIN_EmailId = a.FOIN_EmailId,
                                    FOIN_Discription = a.FOIN_Discription,
                                    FOIN_To = a.FOIN_To,
                                    FOIN_ReceivedBy = a.FOIN_ReceivedBy,
                                    FOIN_HandedOverTo = a.FOIN_HandedOverTo,
                                    FOIN_ActiveFlag = a.FOIN_ActiveFlag,
                                    HRME_EmployeeFirstName = b.HRME_EmployeeFirstName + (string.IsNullOrEmpty(b.HRME_EmployeeMiddleName) ? "" : ' ' + b.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(b.HRME_EmployeeLastName) ? "" : ' ' + b.HRME_EmployeeLastName),
                                    HRME_Id = b.HRME_Id,

                                    firstName1 = (visctxt.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == a.FOIN_ReceivedBy).FirstOrDefault().HRME_EmployeeFirstName),
                                    firstName2 = (visctxt.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == a.FOIN_ReceivedBy).FirstOrDefault().HRME_EmployeeMiddleName),
                                    firstName3 = (visctxt.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == a.FOIN_ReceivedBy).FirstOrDefault().HRME_EmployeeLastName),

                                    secnam1 = (visctxt.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == a.FOIN_HandedOverTo).FirstOrDefault().HRME_EmployeeFirstName),
                                    secnam2 = (visctxt.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == a.FOIN_HandedOverTo).FirstOrDefault().HRME_EmployeeMiddleName),
                                    secnam3 = (visctxt.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == a.FOIN_HandedOverTo).FirstOrDefault().HRME_EmployeeLastName),


                                }).Distinct().OrderBy(t => t.FOIN_Id).ToList();

                if (listdata.Count > 0)
                {
                    data.getdataall = listdata.ToArray();
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }

            return data;
        }
        public InwardDTO saveData(InwardDTO data)
        {
            try
            {
                Master_NumberingDTO check = new Master_NumberingDTO();
                data.transnumbconfigurationsettingsss = check;
                List<Master_Numbering> MM = new List<Master_Numbering>();
                MM = _context.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "VisitorManagement").ToList();
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


                if (data.FOIN_Id > 0)
                {
                    var Duplicate = visctxt.FO_Inward_DMO.Where(t => t.FOIN_Id != data.FOIN_Id && t.MI_Id == data.MI_Id && t.FOIN_EmailId == data.FOIN_EmailId && t.FOIN_PhoneNo == data.FOIN_PhoneNo && t.FOIN_ReceivedBy == data.empid2 && t.FOIN_To == data.HRME_Id && t.FOIN_Adddress == data.FOIN_Adddress && t.FOIN_ContactPerson == data.FOIN_ContactPerson && t.FOIN_Discription == data.FOIN_Discription && t.FOIN_HandedOverTo == data.empid3 && t.FOIN_From == data.FOIN_From).ToList();
                    if (Duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = visctxt.FO_Inward_DMO.Single(t => t.MI_Id == data.MI_Id && t.FOIN_Id == data.FOIN_Id);

                        //update.FOIN_Id = data.FOIN_Id;                        
                        update.FOIN_DateTime = data.FOIN_DateTime;
                        update.FOIN_From = data.FOIN_From;
                        update.FOIN_Adddress = data.FOIN_Adddress;
                        update.FOIN_ContactPerson = data.FOIN_ContactPerson;
                        update.FOIN_PhoneNo = data.FOIN_PhoneNo;
                        update.FOIN_EmailId = data.FOIN_EmailId;
                        update.FOIN_Discription = data.FOIN_Discription;
                        update.FOIN_To = data.HRME_Id;
                        update.FOIN_ReceivedBy = data.empid2;
                        update.FOIN_HandedOverTo = data.empid3;
                        update.UpdatedDate = DateTime.Now;
                        update.FOIN_UpdatedBy = data.UserId;

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

                    var Duplicate = visctxt.FO_Inward_DMO.Where(t => t.MI_Id == data.MI_Id && t.FOIN_EmailId == data.FOIN_EmailId && t.FOIN_PhoneNo == data.FOIN_PhoneNo && t.FOIN_ReceivedBy == data.empid2 && t.FOIN_To == data.HRME_Id && t.FOIN_Adddress == data.FOIN_Adddress && t.FOIN_ContactPerson == data.FOIN_ContactPerson && t.FOIN_Discription == data.FOIN_Discription && t.FOIN_HandedOverTo == data.empid3 && t.FOIN_From == data.FOIN_From).ToList();
                    if (Duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        FO_Inward_DMO obj1 = new FO_Inward_DMO();

                        obj1.FOIN_Id = data.FOIN_Id;
                        obj1.MI_Id = data.MI_Id;
                        obj1.FOIN_InwardNo = data.trans_id;
                        obj1.FOIN_DateTime = data.FOIN_DateTime;
                        obj1.FOIN_From = data.FOIN_From;
                        obj1.FOIN_Adddress = data.FOIN_Adddress;
                        obj1.FOIN_ContactPerson = data.FOIN_ContactPerson;
                        obj1.FOIN_PhoneNo = data.FOIN_PhoneNo;
                        obj1.FOIN_EmailId = data.FOIN_EmailId;
                        obj1.FOIN_Discription = data.FOIN_Discription;
                        obj1.FOIN_To = data.HRME_Id;
                        obj1.FOIN_ReceivedBy = data.empid2;
                        obj1.FOIN_HandedOverTo = data.empid3;
                        obj1.FOIN_ActiveFlag = true;
                        obj1.CreatedDate = DateTime.Now;
                        obj1.UpdatedDate = DateTime.Now;
                        obj1.FOIN_CreatedBy = data.UserId;
                        obj1.FOIN_UpdatedBy = data.UserId;

                        visctxt.Add(obj1);
                        int rowAffected = visctxt.SaveChanges();
                        if (rowAffected > 0)
                        {
                            data.returnval = "Save";

                            var inward1 = visctxt.FO_Inward_DMO.OrderByDescending(d => d.FOIN_Id).First();
                            data.curntInwardrec = visctxt.FO_Inward_DMO.Where(d => d.FOIN_Id == inward1.FOIN_Id).ToArray();

                        }
                        else
                        {
                            data.returnval = "Not Save";
                        }
                    }
                }
                data.institution = _db.Institution.Where(i => i.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public InwardDTO EditDetails(InwardDTO id)
        {
           
            try
            {
                var editData = (from a in visctxt.FO_Inward_DMO
                                from t in visctxt.MasterEmployee
                                where (a.MI_Id == t.MI_Id && a.FOIN_To == t.HRME_Id && a.MI_Id == id.MI_Id && a.FOIN_Id == id.FOIN_Id)
                                select new InwardDTO
                                {

                                    HRME_EmployeeFirstName = t.HRME_EmployeeFirstName + (string.IsNullOrEmpty(t.HRME_EmployeeMiddleName) ? "" : ' ' + t.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(t.HRME_EmployeeLastName) ? "" : ' ' + t.HRME_EmployeeLastName),
                                    FOIN_Id = a.FOIN_Id,
                                    MI_Id = a.MI_Id,
                                    FOIN_InwardNo = a.FOIN_InwardNo,
                                    FOIN_DateTime = a.FOIN_DateTime,
                                    FOIN_From = a.FOIN_From,
                                    FOIN_Adddress = a.FOIN_Adddress,
                                    FOIN_ContactPerson = a.FOIN_ContactPerson,
                                    FOIN_PhoneNo = a.FOIN_PhoneNo,
                                    FOIN_EmailId = a.FOIN_EmailId,
                                    FOIN_Discription = a.FOIN_Discription,
                                    FOIN_To = a.FOIN_To,
                                    hrmid1 = a.FOIN_HandedOverTo,
                                    hrmid12 = a.FOIN_ReceivedBy,
                                   
                                    //employeename1 = a.FOIN_HandedOverTo != 0 ? _context.HR_Master_Employee_DMO.Where(c => c.MI_Id == id.MI_Id && c.HRME_Id == a.FOIN_HandedOverTo).FirstOrDefault().HRME_EmployeeFirstName : "",

                                    //employeename12 = a.FOIN_ReceivedBy != 0 ?  _context.HR_Master_Employee_DMO.Where(c => c.MI_Id == id.MI_Id && c.HRME_Id == a.FOIN_ReceivedBy).Select(t => t.HRME_EmployeeFirstName + (string.IsNullOrEmpty(t.HRME_EmployeeMiddleName) ? "" : ' ' + t.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(t.HRME_EmployeeLastName) ? "" : ' ' + t.HRME_EmployeeLastName)).ToString(),
                                    FOIN_ActiveFlag =a.FOIN_ActiveFlag,
                                   
                                }).ToList();
                id.editDetails = editData.ToArray();


                

                id.emplist2 = (from a in _db.HR_Master_Employee_DMO
                                where (a.MI_Id == id.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRME_Id == editData.FirstOrDefault().hrmid12
                                )
                                select new InwardDTO
                                {
                                    employeename1 = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                    hrmid1 = a.HRME_Id,

                                }).Distinct().ToArray();

                id.emplist3 = (from a in _db.HR_Master_Employee_DMO
                               where (a.MI_Id == id.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRME_Id == editData.FirstOrDefault().hrmid1)
                               select new InwardDTO
                               {
                                   employeename12 = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                   hrmid12 = a.HRME_Id,

                               }).Distinct().ToArray();

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return id;
        }
        public InwardDTO deactivate(InwardDTO data)
        {
            try
            {
                var result = visctxt.FO_Inward_DMO.Single(t => t.FOIN_Id == data.FOIN_Id && t.MI_Id == data.MI_Id);

                if (result.FOIN_ActiveFlag == true)
                {
                    result.FOIN_ActiveFlag = false;
                }
                else if (result.FOIN_ActiveFlag == false)
                {
                    result.FOIN_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                visctxt.Update(result);
                int rowAffected = visctxt.SaveChanges();
                if (rowAffected > 0)
                {
                    data.returnval2 = true;
                }
                else
                {
                    data.returnval2 = false;
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public InwardDTO searchfilter(InwardDTO data)
        {
            try
            {
                data.emplist = (from a in _db.HR_Master_Employee_DMO
                                where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRME_EmployeeFirstName.Contains(data.searchfilter))
                                select new InwardDTO
                                {
                                    HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                    HRME_Id = a.HRME_Id,

                                }).Distinct().ToArray();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public InwardDTO get_empdetails(InwardDTO data)
        {
            try
            {
                data.emplist = (from a in _db.HR_Master_Employee_DMO
                                where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRME_Id != data.HRME_Id)
                                select new InwardDTO
                                {
                                    employeename1 = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                    hrmid1 = a.HRME_Id,

                                }).Distinct().ToArray();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public InwardDTO searchfilter2(InwardDTO data)
        {
            try
            {
                data.emplist = (from a in _db.HR_Master_Employee_DMO
                                where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRME_Id != data.HRME_Id && a.HRME_EmployeeFirstName.Contains(data.searchfilter))
                                select new InwardDTO
                                {
                                    HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                    HRME_Id = a.HRME_Id,

                                }).Distinct().ToArray();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public InwardDTO get_empdetails2(InwardDTO data)
        {
            try
            {
                data.emplist = (from a in _db.HR_Master_Employee_DMO
                                where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRME_Id != data.HRME_Id && a.HRME_Id != data.empid2)
                                select new InwardDTO
                                {
                                    employeename12 = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                    hrmid12 = a.HRME_Id,

                                }).Distinct().ToArray();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
      
    }
}
