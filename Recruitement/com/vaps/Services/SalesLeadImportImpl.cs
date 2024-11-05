using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.VMS.Sales;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.Sales;
using Recruitment.com.vaps.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommonLibrary;

namespace Recruitment.com.vaps.Services
{
    public class SalesLeadImportImpl : Interfaces.SalesLeadImportInterface
    {
        public VMSContext _vmsconte;
        public HRMSContext _hrmscon;
        private DomainModelMsSqlServerContext _db;
        public SalesLeadImportImpl(VMSContext vmsContext, HRMSContext hrms, DomainModelMsSqlServerContext db)
        {
            _vmsconte = vmsContext;
            _hrmscon = hrms;
            _db = db;
        }
        public SalesLeadImportDTO saveadvance(SalesLeadImportDTO data)
        {
            try
            {


                if (data.advimppln.Length > 0)
                {
                    data.message = "";
                    var rowno = 1;
                    foreach (var item in data.advimppln)
                    {
                        ///check the validation for start day and end day
                        ///
                        rowno += 1;

                        item.VisitedDay = item.VisitedDay.Trim();
                        item.VisitedMonth = item.VisitedMonth.Trim();
                        item.VisitedYear = item.VisitedYear.Trim();
                        item.VisitedYear = item.VisitedYear.Replace(" ", string.Empty);

                        item.VisitedDay = item.VisitedDay.Replace(" ", string.Empty);
                        item.VisitedMonth = item.VisitedMonth.Replace(" ", string.Empty);
                       
                        item.Pincode =  item.Pincode.Trim().Replace(" ", string.Empty);
                        item.Pincode =  item.Pincode.Trim().Replace(",", string.Empty);
                        item.Pincode =  item.Pincode.Trim().Replace(".", string.Empty);
                        item.Pincode =  item.Pincode.Trim().Replace(":", string.Empty);
                        item.Pincode = item.Pincode.Trim().Replace("-", string.Empty);
                        item.Pincode = item.Pincode.Trim().Replace("(", string.Empty);
                        item.Pincode = item.Pincode.Trim().Replace(")", string.Empty);
                        item.ContactNo = item.ContactNo.Trim().Replace(" ", string.Empty);
                        item.ContactNo = item.ContactNo.Trim().Replace("+", string.Empty);
                        item.ContactNo = item.ContactNo.Trim().Replace(",", string.Empty);
                        item.ContactNo = item.ContactNo.Trim().Replace(".", string.Empty);
                        item.ContactNo = item.ContactNo.Trim().Replace(":", string.Empty);
                        item.ContactNo = item.ContactNo.Trim().Replace("-", string.Empty);
                        item.ContactNo = item.ContactNo.Trim().Replace("(", string.Empty);
                        item.ContactNo = item.ContactNo.Trim().Replace(")", string.Empty);

                        if (item.VisitedDay!="NULL" && item.VisitedMonth != "NULL" && item.VisitedYear != "NULL")
                        {
                            for (int i = 0; i < item.VisitedDay.Trim().Length; i++)
                            {
                                if (char.IsLetter(item.VisitedDay.Trim()[i]))
                                {
                                    data.message = "VisitedDay Column Field should not contain Any characters or special characters : Row No:" + rowno;
                                    return data;
                                }
                                if (!char.IsLetterOrDigit(item.VisitedDay.Trim()[i]))
                                {
                                    data.message = "VisitedDay Column Field should not contain Any characters or special characters: Row No:" + rowno;
                                    return data;
                                }

                            }
                            if (item.VisitedDay.Length > 2)
                            {
                                data.message = "VisitedDay Column Field should  contain 1 to 31 based on Month: Row No:" + rowno;
                                return data;
                            }

                            if (item.VisitedMonth.Length > 2)
                            {
                                data.message = "VisitedMonth Column Field should  contain 1 to 12 : Row No:" + rowno;
                                return data;
                            }

                            for (int i = 0; i < item.VisitedMonth.Trim().Length; i++)
                            {
                                if (char.IsLetter(item.VisitedMonth.Trim()[i]))
                                {
                                    data.message = "StartMonth Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                    return data;
                                }
                                if (!char.IsLetterOrDigit(item.VisitedMonth.Trim()[i]))
                                {
                                    data.message = "StartMonth Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                    return data;
                                }

                            }


                            for (int i = 0; i < item.VisitedYear.Trim().Length; i++)
                            {
                                if (char.IsLetter(item.VisitedYear.Trim()[i]))
                                {
                                    data.message = "VisitedYear Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                    return data;
                                }
                                if (!char.IsLetterOrDigit(item.VisitedYear.Trim()[i]))
                                {
                                    data.message = "VisitedYear Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                    return data;
                                }

                            }
                            if (item.VisitedYear.Length != 4)
                            {
                                data.message = "VisitedYear Column Field should be correct format Row " + rowno + "   " + "EXAMPLE: 2019";
                                return data;
                            }

                            long smonth = Convert.ToInt64(item.VisitedMonth);


                            long monthday = _vmsconte.month.Single(w => w.IVRM_Month_Id == smonth).IVRM_Month_Max_Days;

                            long sdate = Convert.ToInt64(item.VisitedDay);

                            if (sdate > monthday)
                            {
                                data.message = "VisitedDay is exceeding Month End Day at the Row " + rowno;
                                return data;
                            }

                            if (sdate <= 0)
                            {
                                data.message = "VisitedDay should be greater than ZERO at the Row " + rowno;
                                return data;
                            }
                            
                            if (item.VisitedDay.Length == 1)
                            {
                                item.VisitedDay = "0" + item.VisitedDay;
                            }
                            if (item.VisitedMonth.Length == 1)
                            {
                                item.VisitedMonth = "0" + item.VisitedMonth;
                            }


                        }

                        if (item.ContactNo.Trim() == "NULL")
                        {
                            item.ContactNo = "0";
                        }
                        for (int i = 0; i < item.ContactNo.Trim().Length; i++)
                        {
                            if (char.IsLetter(item.ContactNo.Trim()[i]))
                            {
                                data.message = "ContactNo Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                return data;
                            }
                            if (!char.IsLetterOrDigit(item.ContactNo.Trim()[i]))
                            {
                                data.message = "ContactNo Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                return data;
                            }

                        }


                        if (item.StudentStrength.Trim()=="NULL")
                        {
                            item.StudentStrength = "0";
                        }
                        
                        for (int i = 0; i < item.StudentStrength.Trim().Length; i++)
                        {
                            if (char.IsLetter(item.StudentStrength.Trim()[i]))
                            {
                                data.message = "StudentStrength Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                return data;
                            }
                            if (!char.IsLetterOrDigit(item.StudentStrength.Trim()[i]))
                            {
                                data.message = "StudentStrength Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                return data;
                            }

                        }

                        if (item.Pincode.Trim() == "NULL")
                        {
                            item.Pincode = "0";
                        }

                        for (int i = 0; i < item.Pincode.Trim().Length; i++)
                        {
                            if (char.IsLetter(item.Pincode.Trim()[i]))
                            {
                                data.message = "Pincode Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                return data;
                            }
                            if (!char.IsLetterOrDigit(item.Pincode.Trim()[i]))
                            {
                                data.message = "Pincode Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                return data;
                            }

                        }

                        if (item.StaffStrength.Trim() == "NULL")
                        {
                            item.StaffStrength = "0";
                        }

                        for (int i = 0; i < item.StaffStrength.Trim().Length; i++)
                        {
                            if (char.IsLetter(item.StaffStrength.Trim()[i]))
                            {
                                data.message = "StaffStrength Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                return data;
                            }
                            if (!char.IsLetterOrDigit(item.StaffStrength.Trim()[i]))
                            {
                                data.message = "StaffStrength Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                return data;
                            }

                        }



                        if (item.TotalInstitutions.Trim() == "NULL")
                        {
                            item.TotalInstitutions = "0";
                        }
                        

                        for (int i = 0; i < item.TotalInstitutions.Trim().Length; i++)
                        {
                            if (char.IsLetter(item.TotalInstitutions.Trim()[i]))
                            {
                                data.message = "TotalInstitutions Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                return data;
                            }
                            if (!char.IsLetterOrDigit(item.TotalInstitutions.Trim()[i]))
                            {
                                data.message = "TotalInstitutions Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                return data;
                            }

                        }


                        if (item.LeadName.Trim().ToLower()=="null")
                        {
                            data.message = "LeadName Column should not be NULL: Row No:" + rowno;
                            return data;
                        }
                        if (item.ContactPerson.Trim().ToLower() == "null")
                        {
                            data.message = "ContactPerson Column should not be NULL: Row No:" + rowno;
                            return data;
                        }

                        if (item.Category.Trim().ToLower() == "null")
                        {
                            data.message = "Category Column should not be NULL: Row No:" + rowno;
                            return data;
                        }
                        else
                        {
                            var catelist=_vmsconte.ISM_Sales_Master_Category_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.ISMSMCA_ActiveFlag == true&& a.ISMSMCA_CategoryName.ToLower().Trim()== item.Category.Trim().ToLower()).Distinct().ToList();
                            if (catelist.Count==0)
                            {
                                data.message = "Category Name "+ " " + item.Category + "  " + "Not Exist ,Please Enter Category in Master : Row No:" + rowno;
                                return data;
                            }

                        }

                        if (item.Source.Trim().ToLower() == "null")
                        {
                            data.message = "Source Column should not be NULL: Row No:" + rowno;
                            return data;
                        }
                        else
                        {
                            var srclist = _vmsconte.ISM_Sales_Master_Source_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.ISMSMSO_ActiveFlag == true && a.ISMSMSO_SourceName.ToLower().Trim() == item.Source.Trim().ToLower()).Distinct().ToList();
                            if (srclist.Count == 0)
                            {
                                data.message = "Source Name " + " " + item.Source + "  " + "Not Exist ,Please Enter Source in Master : Row No:" + rowno;
                                return data;
                            }

                        }

                        if (item.Health.Trim().ToLower() == "null")
                        {
                            data.message = "Health/Status Column should not be NULL: Row No:" + rowno;
                            return data;
                        }
                        else
                        {
                            var statuslist = _vmsconte.ISM_Sales_Master_Status_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.ISMSMST_ActiveFlag == true && a.ISMSMST_StatusName.ToLower().Trim() == item.Health.Trim().ToLower()).Distinct().ToList();
                            if (statuslist.Count == 0)
                            {
                                data.message = "Health/Status " + " " + item.Health + "  " + "Not Exist ,Please Enter Health/Status in Master. : Row No:" + rowno;
                                return data;
                            }

                        }

                        if (item.Product.Trim().ToLower() == "null"|| item.Product.Trim().ToLower() == " ")
                        {
                            data.message = "Product Column should not be NULL: Row No:" + rowno;
                            return data;
                        }
                        else
                        {

                            List<string> mobilevv = new List<string>(item.Product.Split(','));
                            mobilevv.Reverse();




                            var prodlistdr = mobilevv.ToList();
                            if (prodlistdr.Count>0)
                            {
                                foreach (var pp in prodlistdr)
                                {
                                    var productlist = _vmsconte.ISM_Sales_Master_Product_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.ISMSMPR_ActiveFlag == true && a.ISMSMPR_ProductName.ToLower().Trim() == pp.Trim().ToLower()).Distinct().ToList();
                                    if (productlist.Count == 0)
                                    {
                                        data.message = "Product Name " + " " + pp + "  " + "Not Exist ,Please Enter Product Name  in Master. : Row No:" + rowno;
                                        return data;
                                    }
                                }

                                
                            }

                           

                        }



                        if (item.Country.Trim().ToLower() == "null" && item.State.Trim().ToLower() == "null")
                        {
                            data.message = "Country/State Column should not be NULL: Row No:" + rowno;
                            return data;
                        }
                        else
                        {
                            var countrylist = _vmsconte.IVRM_Master_Country.Where(a => a.IVRMMC_CountryName.ToLower().Trim() == item.Country.Trim().ToLower()).Distinct().ToList();
                            if (countrylist.Count == 0)
                            {
                                data.message = "Country Name " + " " + item.Country + "  " + "Not Exist ,Please select Country from Master : Row No:" + rowno;
                                return data;
                            }
                            else
                            {
                                var statelist = _vmsconte.IVRM_Master_State.Where(a => a.IVRMMS_Name.ToLower().Trim().Replace(" ", string.Empty) == item.State.Trim().ToLower().Replace(" ", string.Empty)).Distinct().ToList();
                                if (statelist.Count == 0)
                                {
                                    data.message = "State Name " + " " + item.State + "  " + "Not Exist OR Not Belongs to Country"+" "+ item.Country + " ,Please select State from Master : Row No:" + rowno;
                                    return data;
                                }

                            }

                        }


                        if (item.Address1.Trim().ToLower() == "null" || item.Address1.Trim().ToLower() == " ")
                        {
                            data.message = "Address1 Column should not be NULL: Row No:" + rowno;
                            return data;
                        }

                      


                    }


                }






                if (data.advimppln.Length > 0)
                {
                    foreach (var item in data.advimppln)
                    {

                        ISM_Sales_Lead_DMO task = new ISM_Sales_Lead_DMO();
                       
                        task.MI_Id = data.MI_Id;

                        if (item.Category != "NULL" && item.Category != "" && item.Category != null)
                        {
                            var catelist = _vmsconte.ISM_Sales_Master_Category_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.ISMSMCA_ActiveFlag == true && a.ISMSMCA_CategoryName.ToLower().Trim() == item.Category.Trim().ToLower()).Distinct().ToList();

                            if (catelist.Count > 0)
                            {
                                task.ISMSMCA_Id = catelist.FirstOrDefault().ISMSMCA_Id;
                            }
                         

                        }

                        if (item.Source.Trim() != "NULL" && item.Source.Trim() != "" && item.Source.Trim() != null)
                        {
                            var srclist = _vmsconte.ISM_Sales_Master_Source_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.ISMSMSO_ActiveFlag == true && a.ISMSMSO_SourceName.ToLower().Trim() == item.Source.Trim().ToLower()).Distinct().ToList();
                            if (srclist.Count > 0)
                            {
                                task.ISMSMSO_Id = srclist.FirstOrDefault().ISMSMSO_Id;
                            }
                           
                        }

                       
                      
                        if (item.Health.Trim() != "NULL" && item.Health.Trim() != "" && item.Health.Trim() != null)
                        {
                            var statuslist = _vmsconte.ISM_Sales_Master_Status_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.ISMSMST_ActiveFlag == true && a.ISMSMST_StatusName.ToLower().Trim() == item.Health.Trim().ToLower()).Distinct().ToList();

                            if (statuslist.Count > 0)
                            {
                                task.ISMSMST_Id = statuslist.FirstOrDefault().ISMSMST_Id;
                            }
                        

                        }

                        if (item.Country.Trim()!="NULL" && item.Country.Trim() != "" && item.Country.Trim() != null)
                        {
                            var countrylist = _vmsconte.IVRM_Master_Country.Where(a => a.IVRMMC_CountryName.ToLower().Trim() == item.Country.Trim().ToLower()).Distinct().ToList();
                            if (countrylist.Count>0)
                            {
                                task.IVRMMC_Id = countrylist.FirstOrDefault().IVRMMC_Id;
                            }
                        }


                        if (item.State.Trim() != "NULL" && item.State.Trim() != "" && item.State.Trim() != null)
                        {
                            var statelist = _vmsconte.IVRM_Master_State.Where(a => a.IVRMMS_Name.ToLower().Trim().Replace(" ", string.Empty) == item.State.Trim().ToLower().Replace(" ", string.Empty)).Distinct().ToList();
                            if (statelist.Count > 0)
                            {
                                task.IVRMMS_Id = statelist.FirstOrDefault().IVRMMS_Id;
                            }
                        }


                        


                        task.CreatedDate = DateTime.Now;
                        if (item.Remark.Trim()!="NULL")
                        {
                            task.ISMSLE_Remarks = item.Remark.Trim();
                        }
                        if (item.Email.Trim() != "NULL")
                        {
                            task.ISMSLE_EmailId = item.Email.Trim();
                        }

                        if (item.Reference.Trim() != "NULL")
                        {
                            task.ISMSLE_Reference = item.Reference.Trim();
                        }
                        if (item.Pincode.Trim() != "NULL")
                        {
                            task.ISMSLE_Pincode = Convert.ToInt64(item.Pincode.Trim());
                        }

                        task.ISMSLE_LeadName = item.LeadName.Trim();
                        if (item.Designation.Trim()!="NULL")
                        {
                            task.ISMSLE_ContactDesignation = item.Designation.Trim();
                        }
                        if (item.ContactPerson.Trim() != "NULL")
                        {
                            task.ISMSLE_ContactPerson = item.ContactPerson.Trim();
                        }

                        if (item.ContactNo.Trim() != "NULL")
                        {
                            task.ISMSLE_ContactNo = Convert.ToInt64(item.ContactNo.Trim());
                        }

                        if (item.StudentStrength.Trim() != "NULL")
                        {
                            task.ISMSLE_StudentStrength = Convert.ToInt64(item.StudentStrength.Trim());
                        }
                        if (item.StaffStrength.Trim() != "NULL")
                        {
                            task.ISMSLE_StaffStrength = Convert.ToInt64(item.StaffStrength.Trim());
                        }
                        if (item.TotalInstitutions.Trim() != "NULL")
                        {
                            task.ISMSLE_NoOfInstitutions = Convert.ToInt64(item.TotalInstitutions.Trim());
                        }

                        if (item.Address1.Trim() != "NULL")
                        {
                            task.ISMSLE_LeadAddress1 = item.Address1.Trim();
                        }
                        if (item.Address2.Trim() != "NULL")
                        {
                            task.ISMSLE_LeadAddress2 = item.Address2.Trim();
                        }
                        if (item.Address3.Trim() != "NULL")
                        {
                            task.ISMSLE_LeadAddress3 = item.Address3.Trim();
                        }

                        if (item.VisitedDay.Trim()!="NULL" && item.VisitedMonth.Trim() != "NULL" && item.VisitedYear.Trim() != "NULL")
                        {
                            string startdate1 = item.VisitedDay.Trim() + "/" + item.VisitedMonth.Trim() + "/" + item.VisitedYear.Trim();
                            startdate1 = startdate1.Trim();
                            if (startdate1 != null && startdate1 != "" && startdate1 != "NULL")
                            {
                                    DateTime DT = DateTime.ParseExact(startdate1.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    task.ISMSLE_VisitedDate = DT;
                              
                                
                            }
                           
                        }
                        




                        task.ISMSLE_ActiveFlag = true;

                       
                        task.ISMSLE_CreatedBy = data.UserId;
                        task.ISMSLE_UpdatedBy = data.UserId;
                        task.CreatedDate = DateTime.Now;
                        task.UpdatedDate = DateTime.Now;
                        _vmsconte.Add(task);

                        if (item.Product!="" && item.Product != "NULL" && item.Product != null)
                        {

                            List<string> mobilevv = new List<string>(item.Product.Split(','));
                            mobilevv.Reverse();




                            var prodlistdr = mobilevv.ToList();
                            if (prodlistdr.Count > 0)
                            {
                                foreach (var pp in prodlistdr)
                                {
                                    var productlist = _vmsconte.ISM_Sales_Master_Product_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.ISMSMPR_ActiveFlag == true && a.ISMSMPR_ProductName.ToLower().Trim() == pp.Trim().ToLower()).Distinct().ToList();
                                    if (productlist.Count > 0)
                                    {

                                        ISM_Sales_Lead_Products_DMO ld = new ISM_Sales_Lead_Products_DMO();
                                        ld.ISMSMPR_Id = productlist[0].ISMSMPR_Id;
                                        ld.ISMSLE_Id = task.ISMSLE_Id;
                                        ld.MI_Id = data.MI_Id;
                                        ld.ISMSLEPR_ActiveFlag = true;
                                        ld.CreatedDate = DateTime.Now;
                                        ld.ISMSLEPR_CreatedBy = data.UserId;
                                        ld.UpdatedDate = DateTime.Now;
                                        ld.ISMSLEPR_UpdatedBy = data.UserId;
                                        _vmsconte.Add(ld);

                                    }
                                }
                                
                            }
                        }

                        int rowAffected = _vmsconte.SaveChanges();

                        if (rowAffected > 0)
                        {
                            data.suscnt += 1;
                           
                        }
                        else
                        {
                            data.failcnt += 1;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            data.returnval = true;
            return data;
        }

    }
}

