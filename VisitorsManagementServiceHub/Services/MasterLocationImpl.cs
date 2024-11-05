using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.VisitorsManagement;
using DomainModel.Model.com.vapstech.VisitorsManagement;
using PreadmissionDTOs.com.vaps.VisitorsManagement;

namespace VisitorsManagementServiceHub.Services
{
    public class MasterLocationImpl : Interfaces.MasterLocationInterface
    {
        public VisitorsManagementContext _visctxt;
        public DomainModelMsSqlServerContext _db;
        public MasterLocationImpl(VisitorsManagementContext para1, DomainModelMsSqlServerContext para2)
        {
            _visctxt = para1;
            _db = para2;
        }

        public Visitor_Management_Master_Location_DTO getdetails(Visitor_Management_Master_Location_DTO data)
        {
            try
            {
                data.getdata = _visctxt.Visitor_Management_Master_Location_DMO.Where(t => t.MI_Id == data.MI_Id).Distinct().OrderBy(t => t.VMML_Id).ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Visitor_Management_Master_Location_DTO saveRecorddata(Visitor_Management_Master_Location_DTO data)
        {
            try
            {
                if(data.VMML_Id>0)
                {
                    //var Duplicate = _visctxt.Visitor_Management_Master_Location_DMO.Where(t => t.MI_Id == data.MI_Id && t.VMML_Id != data.VMML_Id && t.VMML_Location == data.VMML_Location).ToList();
                  
                    //if(Duplicate.Count>0)
                    //{
                    //    data.duplicate = true;
                    //}
                    //else
                    //{
                        var update = _visctxt.Visitor_Management_Master_Location_DMO.Where(t => t.MI_Id == data.MI_Id && t.VMML_Id == data.VMML_Id).SingleOrDefault();
                       // update.VMML_Id = data.VMML_Id;
                       // update.MI_Id = data.MI_Id;
                        update.VMML_Location = data.VMML_Location;
                        update.VMML_LocationDescription = data.VMML_LocationDescription;
                        update.VMML_LocationFacilities = data.VMML_LocationFacilities;
                      
                        update.UpdatedDate = DateTime.Now;
                        //update.VMML_CreatedBy = data.VMML_CreatedBy;
                        update.VMML_UpdatedBy = data.UserId;

                        _visctxt.Update(update);

                        int rowAffected = _visctxt.SaveChanges();
                        if (rowAffected > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = true;
                        }

                    }

               // }
                else
                {
                    //var Duplicate = _visctxt.Visitor_Management_Master_Location_DMO.Where(t => t.MI_Id == data.MI_Id && t.VMML_Location == data.VMML_Location).ToList();
                    //if (Duplicate.Count > 0)
                    //{
                    //    data.duplicate = true;
                    //}
                    //else
                    //{
                        Visitor_Management_Master_Location_DMO obj = new Visitor_Management_Master_Location_DMO();
                       // obj.VMML_Id = data.VMML_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.VMML_Location = data.VMML_Location;
                        obj.VMML_LocationDescription = data.VMML_LocationDescription;
                        obj.VMML_LocationFacilities = data.VMML_LocationFacilities;
                        obj.VMML_ActiveFlg = true;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        obj.VMML_CreatedBy = data.UserId;
                        obj.VMML_UpdatedBy = data.UserId;

                        _visctxt.Add(obj);

                        int rowAffected = _visctxt.SaveChanges();
                        if(rowAffected>0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = true;
                        }



                    }
               // }                    

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Visitor_Management_Master_Location_DTO editrecord(Visitor_Management_Master_Location_DTO data)
        {
            try
            {
                data.editlist = _visctxt.Visitor_Management_Master_Location_DMO.Where(t => t.MI_Id == data.MI_Id && t.VMML_Id == data.VMML_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Visitor_Management_Master_Location_DTO deactiveY(Visitor_Management_Master_Location_DTO data)
        {
            try
            {
                var result = _visctxt.Visitor_Management_Master_Location_DMO.Single(t => t.VMML_Id == data.VMML_Id && t.MI_Id == data.MI_Id);

                if (result.VMML_ActiveFlg == true)
                {
                    result.VMML_ActiveFlg = false;
                }
                else if (result.VMML_ActiveFlg == false)
                {
                    result.VMML_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _visctxt.Update(result);
                int rowAffected = _visctxt.SaveChanges();
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
