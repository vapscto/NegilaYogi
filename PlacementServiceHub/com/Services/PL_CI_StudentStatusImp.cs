using DataAccessMsSqlServerProvider.com.vapstech.Placement;
using DomainModel.Model.com.vapstech.Placement;
using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlacementServiceHub.com.Services
{
    public class PL_CI_StudentStatusImp : Interfaces.PL_CI_StudentStatusInterface
    {
        public PlacementContext _context;
        public PL_CI_StudentStatusImp(PlacementContext _cont)
        {
            _context = _cont;
        }
        public PL_CI_StudentStatusDTO loaddata(PL_CI_StudentStatusDTO data)
        {
            try
            {
                data.studentname = (from a in _context.Adm_Master_College_StudentDMO
                                    from b in _context.PL_CI_Schedule_Company_JobTitle_StudentsDMO
                                    where (a.MI_Id == data.MI_Id && b.AMCST_Id == a.AMCST_Id)
                                    select new PL_CI_StudentStatusDTO
                                    {
                                        sname = a.AMCST_MiddleName,
                                        studentid = a.AMCST_Id,
                                        PLCISCHCOMJTST_Id = b.PLCISCHCOMJTST_Id
                                    }).Distinct().ToArray();


                data.tablegrid = (from a in _context.Adm_Master_College_StudentDMO
                                  from b in _context.PL_CI_Schedule_Company_JobTitle_StudentsDMO
                                  from c in _context.PL_CI_Schedule_Company_JobTitle_Students_StatusDMO
                                  where (a.MI_Id == data.MI_Id && b.AMCST_Id == a.AMCST_Id && c.PLCISCHCOMJTST_Id == b.PLCISCHCOMJTST_Id)
                                  select new PL_CI_StudentStatusDTO
                                  {
                                      PLCISCHCOMJTSTS_Id = c.PLCISCHCOMJTSTS_Id,
                                      PLCISCHCOMJTSTS_ActiveFlag = c.PLCISCHCOMJTSTS_ActiveFlag,
                                      PLCISCHCOMJTSTS_SelectedFlg = c.PLCISCHCOMJTSTS_SelectedFlg,
                                      sname = a.AMCST_MiddleName,
                                      studentid = a.AMCST_Id,
                                      PLCISCHCOMJTSTS_InterviewRound = c.PLCISCHCOMJTSTS_InterviewRound,
                                      PLCISCHCOMJTSTS_Marks = c.PLCISCHCOMJTSTS_Marks,
                                      PLCISCHCOMJTSTS_TestType = c.PLCISCHCOMJTSTS_TestType,
                                      PLCISCHCOMJTSTS_Remarks = c.PLCISCHCOMJTSTS_Remarks

                                  }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public PL_CI_StudentStatusDTO savedetails(PL_CI_StudentStatusDTO data)
        {
            try
            {
                if (data.PLCISCHCOMJTSTS_Id != 0)
                {

                    var result = _context.PL_CI_Schedule_Company_JobTitle_Students_StatusDMO.Single(t => t.PLCISCHCOMJTSTS_Id == data.PLCISCHCOMJTSTS_Id);


                    result.PLCISCHCOMJTSTS_Id = data.PLCISCHCOMJTSTS_Id;
                    result.PLCISCHCOMJTST_Id = data.PLCISCHCOMJTST_Id;
                    result.PLCISCHCOMJTSTS_InterviewRound = data.PLCISCHCOMJTSTS_InterviewRound;
                    result.PLCISCHCOMJTSTS_Marks = data.PLCISCHCOMJTSTS_Marks;
                    result.PLCISCHCOMJTSTS_TestType = data.PLCISCHCOMJTSTS_TestType;
                    result.PLCISCHCOMJTSTS_Remarks = data.PLCISCHCOMJTSTS_Remarks;
                    result.PLCISCHCOMJTSTS_SelectedFlg = data.PLCISCHCOMJTSTS_SelectedFlg;
                    result.PLCISCHCOMJTSTS_ActiveFlag = true;
                    result.PLCISCHCOMJTSTS_CreatedDate = DateTime.Now;
                    result.PLCISCHCOMJTSTS_CreatedBy = data.UserId;
                    result.PLCISCHCOMJTSTS_UpdatedDate = DateTime.Now;
                    result.PLCISCHCOMJTSTS_UpdatedBy = data.UserId;



                    _context.Update(result);

                    var contactExists = _context.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }

                else
                {
                    var res = _context.PL_CI_Schedule_Company_JobTitle_Students_StatusDMO.Where(t => t.PLCISCHCOMJTSTS_Id == data.PLCISCHCOMJTSTS_Id).ToList();

                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        PL_CI_Schedule_Company_JobTitle_Students_StatusDMO tax = new PL_CI_Schedule_Company_JobTitle_Students_StatusDMO();

                        tax.PLCISCHCOMJTST_Id = data.PLCISCHCOMJTST_Id;
                        tax.PLCISCHCOMJTSTS_InterviewRound = data.PLCISCHCOMJTSTS_InterviewRound;
                        tax.PLCISCHCOMJTSTS_Marks = data.PLCISCHCOMJTSTS_Marks;
                        tax.PLCISCHCOMJTSTS_TestType = data.PLCISCHCOMJTSTS_TestType;
                        tax.PLCISCHCOMJTSTS_Remarks = data.PLCISCHCOMJTSTS_Remarks;
                        tax.PLCISCHCOMJTSTS_SelectedFlg = data.PLCISCHCOMJTSTS_SelectedFlg;
                        tax.PLCISCHCOMJTSTS_ActiveFlag = true;
                        tax.PLCISCHCOMJTSTS_CreatedDate = DateTime.Now;
                        tax.PLCISCHCOMJTSTS_CreatedBy = data.UserId;
                        tax.PLCISCHCOMJTSTS_UpdatedDate = DateTime.Now;
                        tax.PLCISCHCOMJTSTS_UpdatedBy = data.UserId;


                        _context.Add(tax);

                        var contactExists = _context.SaveChanges();
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
            }
            return data;
        }


        public PL_CI_StudentStatusDTO editdetails(PL_CI_StudentStatusDTO data)
        {
            try
            {
                data.editdata = _context.PL_CI_Schedule_Company_JobTitle_Students_StatusDMO.Where(t => t.PLCISCHCOMJTSTS_Id == data.PLCISCHCOMJTSTS_Id).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public PL_CI_StudentStatusDTO deactive(PL_CI_StudentStatusDTO data)
        {
            try
            {
                var result = _context.PL_CI_Schedule_Company_JobTitle_Students_StatusDMO.Single(t => t.PLCISCHCOMJTSTS_Id == data.PLCISCHCOMJTSTS_Id);

                if (result.PLCISCHCOMJTSTS_ActiveFlag == true)
                {
                    result.PLCISCHCOMJTSTS_ActiveFlag = false;
                }
                else if (result.PLCISCHCOMJTSTS_ActiveFlag == false)
                {
                    result.PLCISCHCOMJTSTS_ActiveFlag = true;
                }
                result.PLCISCHCOMJTSTS_UpdatedDate = DateTime.Now;
                _context.Update(result);
                int returnval = _context.SaveChanges();
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
            }
            return data;
        }

    }
}
