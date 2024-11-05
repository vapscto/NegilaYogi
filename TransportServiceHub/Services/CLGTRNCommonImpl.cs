using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model.com.vapstech.Transport;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Services
{
    public class CLGTRNCommonImpl : Interfaces.CLGTRNCommonInterface
    {
        public TransportContext _trncontext;
        ILogger<CLGTRNCommonImpl> _areaimpl;
        //      public DomainModelMsSqlServerContext _db;


        // parameterized constructor
        public CLGTRNCommonImpl(ILogger<CLGTRNCommonImpl> areaimpl, TransportContext context)
        {

            _areaimpl = areaimpl;
            _trncontext = context;

        }

        #region GET COURSE
        public CLGTRNCommonDTO get_course(CLGTRNCommonDTO data)
        {
            try
            {


                data.courselist = (from a in _trncontext.CLG_Adm_College_AY_CourseDMO
                                   from c in _trncontext.MasterCourseDMO
                                   where a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.MI_Id == c.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == c.AMCO_Id  && a.ACAYC_ActiveFlag == true && c.AMCO_ActiveFlag == true
                                   select c
                                 ).Distinct().ToArray();




            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion 
        #region GET BRANCH
        public CLGTRNCommonDTO getBranch(CLGTRNCommonDTO data)
        {
            try
            {


                data.branchlist = (from a in _trncontext.CLG_Adm_College_AY_CourseDMO
                                   from b in _trncontext.CLG_Adm_College_AY_Course_BranchDMO
                                   from c in _trncontext.ClgMasterBranchDMO
                                   where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.MI_Id == c.MI_Id && a.ACAYC_Id == b.ACAYC_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && b.AMB_Id == c.AMB_Id && a.ACAYC_ActiveFlag == true && b.ACAYCB_ActiveFlag == true
                                   select c
                                 ).Distinct().ToArray();




            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion 
        #region GET semister 
        public CLGTRNCommonDTO get_semister(CLGTRNCommonDTO data)
        {
            try
            {
                data.semisterlist = (from a in _trncontext.CLG_Adm_Master_SemesterDMO
                                     from b in _trncontext.CLG_Adm_College_AY_CourseDMO
                                     from c in _trncontext.CLG_Adm_College_AY_Course_BranchDMO
                                     from d in _trncontext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                     where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag == true && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag == true)
                                     select a).Distinct().ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion  
        #region GET Section 
        public CLGTRNCommonDTO get_section(CLGTRNCommonDTO data)
        {
            try
            {
                data.sectionlist = (from a in _trncontext.CLG_Adm_Master_SemesterDMO
                                     from b in _trncontext.CLG_Adm_College_AY_CourseDMO
                                     from c in _trncontext.CLG_Adm_College_AY_Course_BranchDMO
                                     from d in _trncontext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                     where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
                                     select a).Distinct().ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion   
        #region GET Location BASED ON ROUTE 
        public CLGTRNCommonDTO get_location(CLGTRNCommonDTO data)
        {
            try
            {
                data.locationlist = (from a in _trncontext.Route_Location
                                     from b in _trncontext.MasterLocationDMO
                                     where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && b.TRML_ActiveFlg == true && a.TRML_Id == b.TRML_Id && a.TRMR_Id == data.TRMR_Id && a.TRMRL_ActiveFlag == true)
                                     select b).Distinct().ToArray();
              
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion   
    }
}
