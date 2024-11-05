(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClassCategoryReportController', ClassCategoryReportController)
    ClassCategoryReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', 'superCache']
    function ClassCategoryReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, superCache) {


        $scope.exportsheet = false;
        $scope.IsHiddenup = true;
        $scope.IsHiddendown = true;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');


        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.doc_flag = true;
        $scope.div_flag = false;
        $scope.searchString = "";
        $scope.ShowHideup = function () {

            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        }


        $scope.ShowHidedown = function () {


            $scope.IsHiddendown = $scope.IsHiddendown ? false : true;
        }


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


        $scope.loadbasicdata = function () {

            apiService.getURI("ClassCategoryReport/getinitialdata", 2).then(function (promise) {
                if (promise !== "") {
                    $scope.yearlist = promise.yearList;

                }
            });
        }

        $scope.onclickgroup = function () {

            if ($scope.chkgroup == true) {
                $scope.chkcustom = false;
                $scope.chkhead = false;
                $scope.chkyearlyhead = false;
                $scope.chkinstallment = false;
                $scope.chkfine = false;
                $scope.chkclass = false;
                $scope.chkprivileges = false;
                $scope.chkterms = false;
                $scope.chkamount = false;
                $scope.academic = true;

                $scope.feeheadnew = false;
                $scope.feeyearlynew = false;
                $scope.feeinstallment = false;
                $scope.feefineslab = false;
                $scope.feeamount = false;
                $scope.feegroupnew = false;
                $scope.feecustomgroup = false;
                $scope.feeclass = false;
                $scope.feeterm = false;
                $scope.feeamount = false;
                $scope.feeprevileges = false;
            }

        }
        $scope.onclickhead = function () {

            if ($scope.chkhead == true) {
                $scope.chkcustom = false;
                $scope.chkgroup = false;
                $scope.chkyearlyhead = false;
                $scope.chkinstallment = false;
                $scope.chkfine = false;
                $scope.chkclass = false;
                $scope.chkprivileges = false;
                $scope.chkterms = false;
                $scope.chkamount = false;
                $scope.academic = false;

                $scope.feeheadnew = false;
                $scope.feegroupnew = false;
                $scope.feeyearlynew = false;
                $scope.feeinstallment = false;
                $scope.feefineslab = false;
                $scope.feeamount = false;
                $scope.feecustomgroup = false;
                $scope.feeclass = false;
                $scope.feeterm = false;
                $scope.feeamount = false;
                $scope.feeprevileges = false;
            }

        }
        $scope.onclickyearlyhead = function () {

            if ($scope.chkyearlyhead == true) {
                $scope.chkcustom = false;
                $scope.chkgroup = false;
                $scope.chkhead = false;
                $scope.chkinstallment = false;
                $scope.chkfine = false;
                $scope.chkclass = false;
                $scope.chkprivileges = false;
                $scope.chkterms = false;
                $scope.chkamount = false;
                $scope.academic = true;

                $scope.feegroupnew = false;
                $scope.feeheadnew = false;
                $scope.feeinstallment = false;
                $scope.feefineslab = false;
                $scope.feeamount = false;
                $scope.feeyearlynew = false;
                $scope.feecustomgroup = false;
                $scope.feecustomgroup = false;
                $scope.feeclass = false;
                $scope.feeterm = false;
                $scope.feeamount = false;
                $scope.feeprevileges = false;
            }

        }
        $scope.onclickinstallment = function () {

            if ($scope.chkinstallment == true) {
                $scope.chkcustom = false;
                $scope.chkgroup = false;
                $scope.chkhead = false;
                $scope.chkyearlyhead = false;
                $scope.chkfine = false;
                $scope.chkclass = false;
                $scope.chkprivileges = false;
                $scope.chkterms = false;
                $scope.chkamount = false;
                $scope.academic = true;
                $scope.feeinstallment = false;
                $scope.feeyearlynew = false;
                $scope.feegroupnew = false;
                $scope.feeheadnew = false;
                $scope.feefineslab = false;
                $scope.feeamount = false;
                $scope.feecustomgroup = false;
                $scope.feeclass = false;
                $scope.feeterm = false;
                $scope.feeamount = false;
                $scope.feeprevileges = false;
            }

        }
        $scope.onclickfine = function () {

            if ($scope.chkfine == true) {
                $scope.chkcustom = false;
                $scope.chkgroup = false;
                $scope.chkhead = false;
                $scope.chkyearlyhead = false;
                $scope.chkinstallment = false;
                $scope.chkclass = false;
                $scope.chkprivileges = false;
                $scope.chkterms = false;
                $scope.chkamount = false;
                $scope.academic = false;
                $scope.feeyearlynew = false;
                $scope.feegroupnew = false;
                $scope.feeheadnew = false;
                $scope.feeinstallment = false;
                $scope.feefineslab = false;
                $scope.feeamount = false;
                $scope.feecustomgroup = false;
                $scope.feeclass = false;
                $scope.feeterm = false;
                $scope.feeamount = false;
                $scope.feeprevileges = false;
            }

        }
        $scope.onclickclass = function () {

            if ($scope.chkclass == true) {
                $scope.chkcustom = false;
                $scope.chkgroup = false;
                $scope.chkhead = false;
                $scope.chkyearlyhead = false;
                $scope.chkinstallment = false;
                $scope.chkfine = false;
                $scope.chkprivileges = false;
                $scope.chkterms = false;
                $scope.chkamount = false;
                $scope.academic = true;
                $scope.feeyearlynew = false;
                $scope.feegroupnew = false;
                $scope.feeheadnew = false;
                $scope.feeinstallment = false;
                $scope.feefineslab = false;
                $scope.feeamount = false;
                $scope.feecustomgroup = false;
                $scope.feeclass = false;
                $scope.feeterm = false;
                $scope.feeamount = false;
                $scope.feeprevileges = false;
            }

        }
        $scope.onclickprivileges = function () {

            if ($scope.chkprivileges == true) {
                $scope.chkcustom = false;
                $scope.chkgroup = false;
                $scope.chkhead = false;
                $scope.chkyearlyhead = false;
                $scope.chkinstallment = false;
                $scope.chkfine = false;
                $scope.chkclass = false;
                $scope.chkterms = false;
                $scope.chkamount = false;
                $scope.academic = true;

                $scope.feeyearlynew = false;
                $scope.feegroupnew = false;
                $scope.feeheadnew = false;
                $scope.feeamount = false;
                $scope.feeinstallment = false;
                $scope.feecustomgroup = false;
                $scope.feeclass = false;
                $scope.feeterm = false;
                $scope.feeamount = false;
                $scope.feeprevileges = false;
            }

        }
        $scope.onclickterms = function () {

            if ($scope.chkterms == true) {
                $scope.chkcustom = false;
                $scope.chkgroup = false;
                $scope.chkhead = false;
                $scope.chkyearlyhead = false;
                $scope.chkinstallment = false;
                $scope.chkfine = false;
                $scope.chkclass = false;
                $scope.chkprivileges = false;
              
                $scope.chkamount = false;
                $scope.academic = false;

                $scope.feeyearlynew = false;
                $scope.feegroupnew = false;
                $scope.feeheadnew = false;
                $scope.feeamount = false;
                $scope.feeinstallment = false;
                $scope.feecustomgroup = false;
                $scope.feeterm = false;
                $scope.feeamount = false;
                $scope.feeprevileges = false;
            }

        }
        $scope.onclickamount = function () {

            if ($scope.chkamount == true) {
                $scope.chkcustom = false;
                $scope.chkgroup = false;
                $scope.chkhead = false;
                $scope.chkyearlyhead = false;
                $scope.chkinstallment = false;
                $scope.chkfine = false;
                $scope.chkprivileges = false;
                $scope.chkterms = false;
                $scope.academic = true;

                $scope.feeyearlynew = false;
                $scope.feegroupnew = false;
                $scope.feeheadnew = false;
                $scope.feeamount = false;
                $scope.feeinstallment = false;
                $scope.feecustomgroup = false;
                $scope.feeclass = false;
                $scope.feeterm = false;
                $scope.feeamount = false;
                $scope.feeprevileges = false;
            }

        }
        $scope.onclickcustom = function () {

            if ($scope.chkcustom == true) {
                $scope.chkgroup = false;
                $scope.chkhead = false;
                $scope.chkyearlyhead = false;
                $scope.chkinstallment = false;
                $scope.chkfine = false;
                $scope.chkclass = false;
                $scope.chkprivileges = false;
                $scope.chkterms = false;
                $scope.chkamount = false;
                $scope.academic = false;
                $scope.feeyearlynew = false;
                $scope.feegroupnew = false;
                $scope.feeheadnew = false;
                $scope.feeinstallment = false;
                $scope.feeamount = false;
                $scope.feecustomgroup = false;
                $scope.feeclass = false;
                $scope.feeterm = false;
                $scope.feeamount = false;
                $scope.feeprevileges = false;
       

            }

        }

        $scope.submitted = false;
        $scope.showreport = function () {
            var type = 0;
            if ($scope.myForm.$valid) {
                if ($scope.chkgroup == true) {
                    type = 1;
                }
                else if ($scope.chkhead == true) {
                    type = 2;

                }
                else if ($scope.chkyearlyhead == true) {
                    type = 3;
                }
                else if ($scope.chkinstallment == true) {
                    type = 4;
                }
                else if ($scope.chkfine == true) {
                    type = 5;

                }
                else if ($scope.chkclass == true) {
                    type = 6;
                }
                else if ($scope.chkprivileges == true) {
                    type = 7;
                }
                else if ($scope.chkterms == true) {
                    type = 8;
                }
                else if ($scope.chkamount == true) {
                    type = 9;

                }
                else if ($scope.chkcustom == true) {
                    type = 10;
                }



                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "type": type

                }
                apiService.create("ClassCategoryReport/", data).then(function (promise) {
                    if (promise.searchdatalist != null && promise.searchdatalist != "") {

                        if ($scope.chkgroup == true) {
                            $scope.feegroup = promise.searchdatalist;
                            $scope.totcountfirstfeegroup = promise.searchdatalist.length;
                            $scope.feegroupnew = true;
                            $scope.exportsheet = true;


                        }
                        else if ($scope.chkhead == true) {
                            $scope.feehead = promise.searchdatalist;
                            $scope.totcountfirstfeehead = promise.searchdatalist.length;
                            $scope.feeheadnew = true;
                            $scope.exportsheet = true;
                        }
                        else if ($scope.chkyearlyhead == true) {
                            $scope.exportsheet = true;

                            if (promise.searchdatalist !== null && promise.searchdatalist.length > 0) {
                                $scope.students = promise.searchdatalist;
                                // $scope.presentCountgrid = $scope.searchdatalist.length;


                                $scope.recivedamtarray1 = [];
                                $scope.recivedamtarray2 = [];
                                $scope.recivedamtarray3 = [];

                                angular.forEach($scope.students, function (t3) {
                                    var al_cnt = 0;
                                    angular.forEach($scope.recivedamtarray1, function (rt) {
                                        if (parseInt(rt.FMG_Id) === parseInt(t3.FMG_Id)) {
                                            al_cnt += 1;
                                        }
                                    });
                                    if (al_cnt === 0) {




                                        $scope.recivedamtarray1.push({

                                            FMG_GroupName: t3.FMG_GroupName,

                                            FMG_Id: t3.FMG_Id,

                                            FMH_Id: t3.FMH_Id,

                                            FMI_Id: t3.FMI_Id,

                                            FMH_FeeName: t3.FMH_FeeName,

                                            FMI_Name: t3.FMI_Name,

                                            FYGHM_FineApplicableFlag: t3.FYGHM_FineApplicableFlag,

                                            FYGHM_Common_AmountFlag: t3.FYGHM_Common_AmountFlag,


                                            FTI_Id: t3.FTI_Id,

                                            FTI_Name: t3.FTI_Name


                                        });
                                    }
                                });
                                angular.forEach($scope.students, function (t3) {
                                    var al_cnt = 0;
                                    angular.forEach($scope.recivedamtarray2, function (rt) {
                                        if (parseInt(rt.FMH_Id) === parseInt(t3.FMH_Id)) {
                                            al_cnt += 1;
                                        }
                                    });
                                    if (al_cnt === 0) {
                                        $scope.recivedamtarray2.push({
                                            FMG_GroupName: t3.FMG_GroupName,

                                            FMG_Id: t3.FMG_Id,

                                            FMH_Id: t3.FMH_Id,

                                            FMI_Id: t3.FMI_Id,

                                            FMH_FeeName: t3.FMH_FeeName,

                                            FMI_Name: t3.FMI_Name,

                                            FYGHM_FineApplicableFlag: t3.FYGHM_FineApplicableFlag,

                                            FYGHM_Common_AmountFlag: t3.FYGHM_Common_AmountFlag,


                                            FTI_Id: t3.FTI_Id,

                                            FTI_Name: t3.FTI_Name
                                        });
                                    }
                                });
                                angular.forEach($scope.students, function (t3) {
                                    var al_cnt = 0;
                                    angular.forEach($scope.recivedamtarray3, function (rt) {
                                        if (parseInt(rt.FTI_Id) === parseInt(t3.FTI_Id)) {
                                            al_cnt += 1;
                                        }
                                    });
                                    if (al_cnt === 0) {
                                        $scope.recivedamtarray3.push({
                                            FMG_GroupName: t3.FMG_GroupName,

                                            FMG_Id: t3.FMG_Id,

                                            FMH_Id: t3.FMH_Id,

                                            FMI_Id: t3.FMI_Id,

                                            FMH_FeeName: t3.FMH_FeeName,

                                            FMI_Name: t3.FMI_Name,

                                            FYGHM_FineApplicableFlag: t3.FYGHM_FineApplicableFlag,

                                            FYGHM_Common_AmountFlag: t3.FYGHM_Common_AmountFlag,


                                            FTI_Id: t3.FTI_Id,

                                            FTI_Name: t3.FTI_Name
                                        });
                                    }
                                });
                                var details_list = [];

                                angular.forEach($scope.recivedamtarray1, function (cat) {
                                    var alrdy_cnt = 0;
                                    angular.forEach(details_list, function (final_t) {
                                        if (parseInt(final_t.FMG_Id) === parseInt(cat.FMG_Id)) {
                                            alrdy_cnt += 1;
                                        }
                                    });

                                    if (alrdy_cnt === 0) {
                                        var cat_grp_list = [];
                                        var temp_cat_grp_list = []
                                        angular.forEach($scope.students, function (t1) {
                                            if (parseInt(t1.FMG_Id) === parseInt(cat.FMG_Id)) {
                                                temp_cat_grp_list.push(t1);
                                            }
                                        });
                                        angular.forEach($scope.recivedamtarray2, function (grp) {
                                            var grp_subjs = [];
                                            var count = 0;
                                            angular.forEach(temp_cat_grp_list, function (t2) {
                                                if (parseInt(t2.FMH_Id) === parseInt(grp.FMH_Id)) {
                                                    count += 1;
                                                    grp_subjs.push(t2);
                                                }
                                            });
                                            if (count > 0) {
                                                cat_grp_list.push({
                                                    FMG_GroupName: grp.FMG_GroupName,

                                                    FMG_Id: grp.FMG_Id,

                                                    FMH_Id: grp.FMH_Id,

                                                    FMI_Id: grp.FMI_Id,

                                                    FMH_FeeName: grp.FMH_FeeName,

                                                    FMI_Name: grp.FMI_Name,

                                                    FYGHM_FineApplicableFlag: grp.FYGHM_FineApplicableFlag,

                                                    FYGHM_Common_AmountFlag: grp.FYGHM_Common_AmountFlag,


                                                    FTI_Id: grp.FTI_Id,

                                                    FTI_Name: grp.FTI_Name,
                                                    grp_subjs: grp_subjs

                                                    // emG_Id: grp.emG_Id, emG_GroupName: grp.emG_GroupName, grp_subjs: grp_subjs
                                                });
                                            }
                                        });
                                        var rowspan = temp_cat_grp_list.length;
                                        details_list.push({
                                            FMG_GroupName: cat.FMG_GroupName,

                                            FMG_Id: cat.FMG_Id,

                                            FMH_Id: cat.FMH_Id,

                                            FMI_Id: cat.FMI_Id,

                                            FMH_FeeName: cat.FMH_FeeName,

                                            FMI_Name: cat.FMI_Name,

                                            FYGHM_FineApplicableFlag: cat.FYGHM_FineApplicableFlag,

                                            FYGHM_Common_AmountFlag: cat.FYGHM_Common_AmountFlag,


                                            FTI_Id: cat.FTI_Id,

                                            FTI_Name: cat.FTI_Name,
                                            cat_grp_list: cat_grp_list,
                                            rowspan: rowspan
                                        });
                                    }

                                });

                                // console.log(details_list);
                                $scope.final_details_list = details_list;

                            }






                            $scope.feeyearlynew = true;

                        }
                        else if ($scope.chkinstallment == true) {
                            $scope.plannerid = [];

                            $scope.exportsheet = true;

                            if (promise.searchdatalist.length > 0) {
                                $scope.get_approvalreport = promise.searchdatalist;
                                angular.forEach($scope.get_approvalreport, function (dev) {
                                    if ($scope.plannerid.length === 0) {

                                        $scope.plannerid.push({


                                            FTI_Id: dev.FTI_Id,
                                            FTI_Name: dev.FTI_Name,

                                            FMI_Id: dev.FMI_Id,
                                            FMI_Name: dev.FMI_Name,
                                            FTIDD_ToDate: new Date(dev.FTIDD_ToDate),
                                            FTIDD_FromDate: new Date(dev.FTIDD_FromDate),
                                            FTIDD_DueDate: new Date(dev.FTIDD_DueDate),
                                            FTIDD_ApplicableDate: new Date(dev.FTIDD_ApplicableDate)
                                        });
                                    } else if ($scope.plannerid.length > 0) {
                                        var intcount = 0;
                                        angular.forEach($scope.plannerid, function (emp) {
                                            if (emp.FMI_Id === dev.FMI_Id) {
                                                intcount += 1;
                                            }
                                        });
                                        if (intcount === 0) {
                                            $scope.plannerid.push({

                                                FTI_Id: dev.FTI_Id,
                                                FTI_Name: dev.FTI_Name,

                                                FMI_Id: dev.FMI_Id,
                                                FMI_Name: dev.FMI_Name,
                                                FTIDD_ToDate: new Date(dev.FTIDD_ToDate),
                                                FTIDD_FromDate: new Date(dev.FTIDD_FromDate),
                                                FTIDD_DueDate: new Date(dev.FTIDD_DueDate),
                                                FTIDD_ApplicableDate: new Date(dev.FTIDD_ApplicableDate)

                                            });
                                        }
                                    }
                                });

                                angular.forEach($scope.plannerid, function (ddd) {
                                    $scope.templist = [];
                                    angular.forEach($scope.get_approvalreport, function (dd) {
                                        if (dd.FMI_Id === ddd.FMI_Id) {
                                            $scope.templist.push({
                                                FTI_Id: dd.FTI_Id,
                                                FTI_Name: dd.FTI_Name,
                                                FTIDD_ToDate: new Date(dd.FTIDD_ToDate),
                                                FTIDD_FromDate: new Date(dd.FTIDD_FromDate),
                                                FTIDD_DueDate: new Date(dd.FTIDD_DueDate),
                                                FTIDD_ApplicableDate: new Date(dd.FTIDD_ApplicableDate)
                                            });
                                        }
                                    });
                                    ddd.feeinstallmentdata = $scope.templist;
                                });
                            }

                            //type = 4;


                            $scope.feeinstallment = true;
                        }
                        else if ($scope.chkfine == true) {
                            $scope.exportsheet = true;

                            $scope.feefineslab = true;
                            $scope.fineslab = promise.searchdatalist;
                            $scope.fineslabtotal = promise.searchdatalist.length;

                        }


                        else if ($scope.chkclass == true) {
                            $scope.exportsheet = true;
                            if (promise.searchdatalist !== null && promise.searchdatalist.length > 0) {
                                $scope.students = promise.searchdatalist;
                                // $scope.presentCountgrid = $scope.searchdatalist.length;


                                $scope.recivedamtarray1 = [];
                                $scope.recivedamtarray2 = [];
                                $scope.recivedamtarray3 = [];

                                angular.forEach($scope.students, function (t3) {
                                    var al_cnt = 0;
                                    angular.forEach($scope.recivedamtarray1, function (rt) {
                                        if (parseInt(rt.FMCC_Id) === parseInt(t3.FMCC_Id)) {
                                            al_cnt += 1;
                                        }
                                    });
                                    if (al_cnt === 0) {




                                        $scope.recivedamtarray1.push({

                                            FMCC_ClassCategoryName: t3.FMCC_ClassCategoryName,

                                            ASMC_SectionName: t3.ASMC_SectionName,

                                            ASMS_Id: t3.ASMS_Id,

                                            ASMCL_ClassName: t3.ASMCL_ClassName,

                                            ASMCL_Id: t3.ASMCL_Id,

                                            FMCC_Id: t3.FMCC_Id,




                                        });
                                    }
                                });
                                angular.forEach($scope.students, function (t3) {
                                    var al_cnt = 0;
                                    angular.forEach($scope.recivedamtarray2, function (rt) {
                                        if (parseInt(rt.ASMCL_Id) === parseInt(t3.ASMCL_Id)) {
                                            al_cnt += 1;
                                        }
                                    });
                                    if (al_cnt === 0) {
                                        $scope.recivedamtarray2.push({
                                            FMCC_ClassCategoryName: t3.FMCC_ClassCategoryName,

                                            ASMC_SectionName: t3.ASMC_SectionName,

                                            ASMS_Id: t3.ASMS_Id,

                                            ASMCL_ClassName: t3.ASMCL_ClassName,

                                            ASMCL_Id: t3.ASMCL_Id,

                                            FMCC_Id: t3.FMCC_Id
                                        });
                                    }
                                });
                                angular.forEach($scope.students, function (t3) {
                                    var al_cnt = 0;
                                    angular.forEach($scope.recivedamtarray3, function (rt) {
                                        if (parseInt(rt.ASMS_Id) === parseInt(t3.ASMS_Id)) {
                                            al_cnt += 1;
                                        }
                                    });
                                    if (al_cnt === 0) {
                                        $scope.recivedamtarray3.push({
                                            FMCC_ClassCategoryName: t3.FMCC_ClassCategoryName,

                                            ASMC_SectionName: t3.ASMC_SectionName,

                                            ASMS_Id: t3.ASMS_Id,

                                            ASMCL_ClassName: t3.ASMCL_ClassName,

                                            ASMCL_Id: t3.ASMCL_Id,

                                            FMCC_Id: t3.FMCC_Id
                                        });
                                    }
                                });
                                var details_list = [];

                                angular.forEach($scope.recivedamtarray1, function (cat) {
                                    var alrdy_cnt = 0;
                                    angular.forEach(details_list, function (final_t) {
                                        if (parseInt(final_t.FMCC_Id) === parseInt(cat.FMCC_Id)) {
                                            alrdy_cnt += 1;
                                        }
                                    });

                                    if (alrdy_cnt === 0) {
                                        var cat_grp_list = [];
                                        var temp_cat_grp_list = []
                                        angular.forEach($scope.students, function (t1) {
                                            if (parseInt(t1.FMCC_Id) === parseInt(cat.FMCC_Id)) {
                                                temp_cat_grp_list.push(t1);
                                            }
                                        });
                                        angular.forEach($scope.recivedamtarray2, function (grp) {
                                            var grp_subjs = [];
                                            var count = 0;
                                            angular.forEach(temp_cat_grp_list, function (t2) {
                                                if (parseInt(t2.ASMCL_Id) === parseInt(grp.ASMCL_Id)) {
                                                    count += 1;
                                                    grp_subjs.push(t2);
                                                }
                                            });
                                            if (count > 0) {
                                                cat_grp_list.push({
                                                    FMCC_ClassCategoryName: grp.FMCC_ClassCategoryName,

                                                    ASMC_SectionName: grp.ASMC_SectionName,

                                                    ASMS_Id: grp.ASMS_Id,

                                                    ASMCL_ClassName: grp.ASMCL_ClassName,

                                                    ASMCL_Id: grp.ASMCL_Id,

                                                    FMCC_Id: grp.FMCC_Id,
                                                    grp_subjs: grp_subjs

                                                    // emG_Id: grp.emG_Id, emG_GroupName: grp.emG_GroupName, grp_subjs: grp_subjs
                                                });
                                            }
                                        });
                                        var rowspan = temp_cat_grp_list.length;
                                        details_list.push({
                                            FMCC_ClassCategoryName: cat.FMCC_ClassCategoryName,

                                            ASMC_SectionName: cat.ASMC_SectionName,

                                            ASMS_Id: cat.ASMS_Id,

                                            ASMCL_ClassName: cat.ASMCL_ClassName,

                                            ASMCL_Id: cat.ASMCL_Id,

                                            FMCC_Id: cat.FMCC_Id,
                                            cat_grp_list: cat_grp_list,
                                            rowspan: rowspan
                                        });
                                    }

                                });

                             
                                $scope.final_details_list = details_list;

                            }

                            $scope.feeclass = true;

                        }
                        else if ($scope.chkprivileges == true) {
                            $scope.exportsheet = true;
                            if (promise.searchdatalist !== null && promise.searchdatalist.length > 0) {
                                $scope.students = promise.searchdatalist;
                            

                                $scope.recivedamtarray1 = [];
                                $scope.recivedamtarray2 = [];
                                $scope.recivedamtarray3 = [];

                                angular.forEach($scope.students, function (t3) {
                                    var al_cnt = 0;
                                    angular.forEach($scope.recivedamtarray1, function (rt) {
                                        if (parseInt(rt.User_Id) === parseInt(t3.User_Id)) {
                                            al_cnt += 1;
                                        }
                                    });
                                    if (al_cnt === 0) {




                                        $scope.recivedamtarray1.push({

                                            NormalizedUserName:t3.NormalizedUserName,
                                            FMG_GroupName : t3.FMG_GroupName,	
                                                FMG_Id : t3.FMG_Id,
                                            FMH_Id : t3.FMH_Id,
                                            FMH_FeeName : t3.FMH_FeeName,
                                            IVRMRT_Role : t3.IVRMRT_Role,
                                            User_Id : t3.User_Id

                                           




                                        });
                                    }
                                });
                                angular.forEach($scope.students, function (t3) {
                                    var al_cnt = 0;
                                    angular.forEach($scope.recivedamtarray2, function (rt) {
                                        if (parseInt(rt.FMG_Id) === parseInt(t3.FMG_Id)) {
                                            al_cnt += 1;
                                        }
                                    });
                                    if (al_cnt === 0) {
                                        $scope.recivedamtarray2.push({
                                            NormalizedUserName:t3.NormalizedUserName,
                                            FMG_GroupName : t3.FMG_GroupName,
                                            FMG_Id : t3.FMG_Id,
                                            FMH_Id : t3.FMH_Id,
                                            FMH_FeeName : t3.FMH_FeeName,
                                            IVRMRT_Role : t3.IVRMRT_Role,
                                            User_Id : t3.User_Id
                                        });
                                    }
                                });
                                angular.forEach($scope.students, function (t3) {
                                    var al_cnt = 0;
                                    angular.forEach($scope.recivedamtarray3, function (rt) {
                                        if (parseInt(rt.FMH_Id) === parseInt(t3.FMH_Id)) {
                                            al_cnt += 1;
                                        }
                                    });
                                    if (al_cnt === 0) {
                                        $scope.recivedamtarray3.push({
                                            NormalizedUserName:t3.NormalizedUserName,
                                            FMG_GroupName : t3.FMG_GroupName,
                                            FMG_Id : t3.FMG_Id,
                                            FMH_Id : t3.FMH_Id,
                                            FMH_FeeName : t3.FMH_FeeName,
                                            IVRMRT_Role : t3.IVRMRT_Role,
                                            User_Id : t3.User_Id
                                        });
                                    }
                                });
                                var details_list = [];

                                angular.forEach($scope.recivedamtarray1, function (cat) {
                                    var alrdy_cnt = 0;
                                    angular.forEach(details_list, function (final_t) {
                                        if (parseInt(final_t.User_Id) === parseInt(cat.User_Id)) {
                                            alrdy_cnt += 1;
                                        }
                                    });

                                    if (alrdy_cnt === 0) {
                                        var cat_grp_list = [];
                                        var temp_cat_grp_list = []
                                        angular.forEach($scope.students, function (t1) {
                                            if (parseInt(t1.User_Id) === parseInt(cat.User_Id)) {
                                                temp_cat_grp_list.push(t1);
                                            }
                                        });
                                        angular.forEach($scope.recivedamtarray2, function (grp) {
                                            var grp_subjs = [];
                                            var count = 0;
                                            angular.forEach(temp_cat_grp_list, function (t2) {
                                                if (parseInt(t2.FMG_Id) === parseInt(grp.FMG_Id)) {
                                                    count += 1;
                                                    grp_subjs.push(t2);
                                                }
                                            });
                                            if (count > 0) {
                                                cat_grp_list.push({
                                                    NormalizedUserName:grp.NormalizedUserName,
                                                    FMG_GroupName : grp.FMG_GroupName,
                                                    FMG_Id : grp.FMG_Id,
                                                    FMH_Id : grp.FMH_Id,
                                                    FMH_FeeName : grp.FMH_FeeName,
                                                    IVRMRT_Role : grp.IVRMRT_Role,
                                                    User_Id : grp.User_Id,
                                                    grp_subjs: grp_subjs

                                                    // emG_Id: grp.emG_Id, emG_GroupName: grp.emG_GroupName, grp_subjs: grp_subjs
                                                });
                                            }
                                        });
                                        var rowspan = temp_cat_grp_list.length;
                                        details_list.push({
                                            NormalizedUserName:cat.NormalizedUserName,
                                            FMG_GroupName : cat.FMG_GroupName,
                                            FMG_Id : cat.FMG_Id,
                                            FMH_Id : cat.FMH_Id,
                                            FMH_FeeName : cat.FMH_FeeName,
                                            IVRMRT_Role : cat.IVRMRT_Role,
                                            User_Id : cat.User_Id,
                                            cat_grp_list: cat_grp_list,
                                            rowspan: rowspan
                                        });
                                    }

                                });


                                $scope.final_details_list = details_list;

                            }

                            $scope.feeprevileges = true;


                        }
                        else if ($scope.chkterms == true) {
                            $scope.exportsheet = true;
                            if (promise.searchdatalist !== null && promise.searchdatalist.length > 0) {
                                $scope.students = promise.searchdatalist;
                                // $scope.presentCountgrid = $scope.searchdatalist.length;


                                $scope.recivedamtarray1 = [];
                                $scope.recivedamtarray2 = [];
                                $scope.recivedamtarray3 = [];

                                angular.forEach($scope.students, function (t3) {
                                    var al_cnt = 0;
                                    angular.forEach($scope.recivedamtarray1, function (rt) {
                                        if (parseInt(rt.FMT_Id) === parseInt(t3.FMT_Id)) {
                                            al_cnt += 1;
                                        }
                                    });
                                    if (al_cnt === 0) {

                                    
                                      


                                        $scope.recivedamtarray1.push({

                                            FMT_Id: t3.FMT_Id,
                                            FMT_Name: t3.FMT_Name,
                                            FMH_Id: t3.FMH_Id,
                                            FMH_FeeName: t3.FMH_FeeName,
                                            FTI_Id: t3.FTI_Id,
                                            FTI_Name: t3.FTI_Name

                                        });
                                    }
                                });
                                angular.forEach($scope.students, function (t3) {
                                    var al_cnt = 0;
                                    angular.forEach($scope.recivedamtarray2, function (rt) {
                                        if (parseInt(rt.FMH_Id) === parseInt(t3.FMH_Id)) {
                                            al_cnt += 1;
                                        }
                                    });
                                    if (al_cnt === 0) {
                                        $scope.recivedamtarray2.push({
                                            FMT_Id: t3.FMT_Id,
                                            FMT_Name: t3.FMT_Name,
                                            FMH_Id: t3.FMH_Id,
                                            FMH_FeeName: t3.FMH_FeeName,
                                            FTI_Id: t3.FTI_Id,
                                            FTI_Name: t3.FTI_Name
                                        });
                                    }
                                });
                                angular.forEach($scope.students, function (t3) {
                                    var al_cnt = 0;
                                    angular.forEach($scope.recivedamtarray3, function (rt) {
                                        if (parseInt(rt.FTI_Id) === parseInt(t3.FTI_Id)) {
                                            al_cnt += 1;
                                        }
                                    });
                                    if (al_cnt === 0) {
                                        $scope.recivedamtarray3.push({
                                            FMT_Id: t3.FMT_Id,
                                            FMT_Name: t3.FMT_Name,
                                            FMH_Id: t3.FMH_Id,
                                            FMH_FeeName: t3.FMH_FeeName,
                                            FTI_Id: t3.FTI_Id,
                                            FTI_Name: t3.FTI_Name
                                        });
                                    }
                                });
                                var details_list = [];

                                angular.forEach($scope.recivedamtarray1, function (cat) {
                                    var alrdy_cnt = 0;
                                    angular.forEach(details_list, function (final_t) {
                                        if (parseInt(final_t.FMT_Id) === parseInt(cat.FMT_Id)) {
                                            alrdy_cnt += 1;
                                        }
                                    });

                                    if (alrdy_cnt === 0) {
                                        var cat_grp_list = [];
                                        var temp_cat_grp_list = []
                                        angular.forEach($scope.students, function (t1) {
                                            if (parseInt(t1.FMT_Id) === parseInt(cat.FMT_Id)) {
                                                temp_cat_grp_list.push(t1);
                                            }
                                        });
                                        angular.forEach($scope.recivedamtarray2, function (grp) {
                                            var grp_subjs = [];
                                            var count = 0;
                                            angular.forEach(temp_cat_grp_list, function (t2) {
                                                if (parseInt(t2.FMH_Id) === parseInt(grp.FMH_Id)) {
                                                    count += 1;
                                                    grp_subjs.push(t2);
                                                }
                                            });
                                            if (count > 0) {
                                                cat_grp_list.push({
                                                    FMT_Id: grp.FMT_Id,
                                                    FMT_Name: grp.FMT_Name,
                                                    FMH_Id: grp.FMH_Id,
                                                    FMH_FeeName: grp.FMH_FeeName,
                                                    FTI_Id: grp.FTI_Id,
                                                    FTI_Name: grp.FTI_Name,
                                                    grp_subjs: grp_subjs

                                                    // emG_Id: grp.emG_Id, emG_GroupName: grp.emG_GroupName, grp_subjs: grp_subjs
                                                });
                                            }
                                        });
                                        var rowspan = temp_cat_grp_list.length;
                                        details_list.push({
                                            FMT_Id: cat.FMT_Id,
                                            FMT_Name: cat.FMT_Name,
                                            FMH_Id: cat.FMH_Id,
                                            FMH_FeeName: cat.FMH_FeeName,
                                            FTI_Id: cat.FTI_Id,
                                            FTI_Name: cat.FTI_Name,
                                            cat_grp_list: cat_grp_list,
                                            rowspan: rowspan
                                        });
                                    }

                                });

                                // console.log(details_list);
                                $scope.final_details_list = details_list;

                            }

                            $scope.feeterm = true;

                        }
                        else if ($scope.chkamount == true) {
                            // type = 9;
                            $scope.exportsheet = true;
                            $scope.amountid = [];

                            $scope.feeinstallment = false;

                            if (promise.searchdatalist.length > 0) {
                                $scope.get_approvalreport = promise.searchdatalist;
                                angular.forEach($scope.get_approvalreport, function (dev) {
                                    if ($scope.amountid.length === 0) {

                                        $scope.amountid.push({

                                            FMG_GroupName: dev.FMG_GroupName,
                                            FTI_Name: dev.FTI_Name,
                                            FTI_Id: dev.FTI_Id,
                                            FMG_Id: dev.FMG_Id,
                                            FMI_Name: dev.FMI_Name,
                                            FMA_Amount: dev.FMA_Amount,
                                            FMA_Id: dev.FMA_Id,
                                            FMH_FeeName: dev.FMH_FeeName,
                                            FMH_Id: dev.FMH_Id,
                                            FMCC_ClassCategoryName: dev.FMCC_ClassCategoryName,
                                            FTFSE_Amount: dev.FTFSE_Amount,
                                            FTDDE_Month: dev.FTDDE_Month

                                        });
                                    } else if ($scope.amountid.length > 0) {
                                        var intcount = 0;
                                        angular.forEach($scope.amountid, function (emp) {
                                            if (emp.FMG_Id === dev.FMG_Id) {
                                                intcount += 1;
                                            }
                                        });
                                        if (intcount === 0) {
                                            $scope.amountid.push({


                                                FMG_GroupName: dev.FMG_GroupName,
                                                FTI_Name: dev.FTI_Name,
                                                FTI_Id: dev.FTI_Id,
                                                FMG_Id: dev.FMG_Id,
                                                FMI_Name: dev.FMI_Name,
                                                FMA_Amount: dev.FMA_Amount,
                                                FMA_Id: dev.FMA_Id,
                                                FMH_FeeName: dev.FMH_FeeName,
                                                FMH_Id: dev.FMH_Id,
                                                FMCC_ClassCategoryName: dev.FMCC_ClassCategoryName,
                                                FTFSE_Amount: dev.FTFSE_Amount,
                                                FTDDE_Month: dev.FTDDE_Month

                                            });
                                        }
                                    }
                                });

                                angular.forEach($scope.amountid, function (ddd) {
                                    $scope.templist = [];
                                    angular.forEach($scope.get_approvalreport, function (dd) {
                                        if (dd.FMG_Id === ddd.FMG_Id) {
                                            $scope.templist.push({
                                                FMG_GroupName: dd.FMG_GroupName,
                                                FTI_Name: dd.FTI_Name,
                                                FTI_Id: dd.FTI_Id,
                                                FMG_Id: dd.FMG_Id,
                                                FMI_Name: dd.FMI_Name,
                                                FMA_Amount: dd.FMA_Amount,
                                                FMA_Id: dd.FMA_Id,
                                                FMH_FeeName: dd.FMH_FeeName,
                                                FMH_Id: dd.FMH_Id,
                                                FMCC_ClassCategoryName: dd.FMCC_ClassCategoryName,
                                                FTFSE_Amount: dd.FTFSE_Amount,
                                                FTDDE_Month: dd.FTDDE_Month
                                            });
                                        }
                                    });
                                    ddd.feeamount = $scope.templist;
                                });
                            }

                            //type = 4;


                            $scope.feeamount = true;




                        }
                        else if ($scope.chkcustom == true) {
                            //    type = 10;
                            $scope.exportsheet = true;
                            $scope.customid = [];


                            if (promise.searchdatalist.length > 0) {
                                $scope.get_approvalreport = promise.searchdatalist;
                                angular.forEach($scope.get_approvalreport, function (dev) {
                                    if ($scope.customid.length === 0) {

                                        $scope.customid.push({

                                            FMG_GroupName: dev.FMG_GroupName,
                                            FMGG_GroupName: dev.FMGG_GroupName,
                                            FMGG_Id: dev.FMGG_Id,
                                            FMG_Id: dev.FMG_Id


                                        });
                                    } else if ($scope.customid.length > 0) {
                                        var intcount = 0;
                                        angular.forEach($scope.customid, function (emp) {
                                            if (emp.FMGG_Id === dev.FMGG_Id) {
                                                intcount += 1;
                                            }
                                        });
                                        if (intcount === 0) {
                                            $scope.customid.push({


                                                FMG_GroupName: dev.FMG_GroupName,
                                                FMGG_GroupName: dev.FMGG_GroupName,
                                                FMGG_Id: dev.FMGG_Id,
                                                FMG_Id: dev.FMG_Id

                                            });
                                        }
                                    }
                                });

                                angular.forEach($scope.customid, function (ddd) {
                                    $scope.templist = [];
                                    angular.forEach($scope.get_approvalreport, function (dd) {
                                        if (dd.FMGG_Id === ddd.FMGG_Id) {
                                            $scope.templist.push({
                                                FMG_GroupName: dd.FMG_GroupName,
                                                FMGG_GroupName: dd.FMGG_GroupName,
                                                FMGG_Id: dd.FMGG_Id,
                                                FMG_Id: dd.FMG_Id
                                            });
                                        }
                                    });
                                    ddd.feecustom = $scope.templist;
                                });
                            }


                            $scope.feecustomgroup = true;
                            $scope.feeinstallment = false;
                        }


                    }

                    else {
                        swal("No Records Found");
                        $scope.exportsheet = false;
                        $scope.div_flag = false;
                        $scope.doc_flag = true;
                        $scope.doc_sel = "";
                        $scope.exportsheet = false;
                    }
                })

            } else {
                $scope.submitted = true;

            }

        }

        $scope.printdatatable = [];
        $scope.exportToExcel = function () {
        
            var excelname = "";
            var exportHref = "";
            var excelsheetname = "Fee";
            var reportname = "";


            if ($scope.chkgroup == true) {
                reportname = "#feegroupexcel";
            }
            else if ($scope.chkhead == true) {
                reportname = "#feeheadexcel";

            }
            else if ($scope.chkyearlyhead == true) {
                reportname = "#yearlygroupexcel";
            }
            else if ($scope.chkinstallment == true) {
                reportname = "#installmentexcel";
            }
            else if ($scope.chkfine == true) {
                reportname = "#fineexcel";

            }
            else if ($scope.chkclass == true) {
                reportname = "#classcategoryexcel";
            }
            else if ($scope.chkprivileges == true) {
                reportname = "#previlegeexcel";
            }
            else if ($scope.chkterms == true) {
                reportname = "#feetermexcel";
            }
            else if ($scope.chkamount == true) {
                reportname = "#feeamountexcel";

            }
            else if ($scope.chkcustom == true) {
                reportname = "#feecustomgroupexcel";
            }


                

                    excelname = excelsheetname + ' REPORT ';
                    excelname = excelname.toUpperCase() + '.xls';
                    exportHref = Excel.tableToExcel(reportname, excelsheetname);
                    $timeout(function () {
                        var a = document.createElement('a');
                        a.href = exportHref;
                        a.download = excelname;
                        document.body.appendChild(a);
                        a.click();
                        a.remove();

                }, 100);
            

        }

        $scope.printData = function (printSectionId) {
            var innerContents = "";
            if ($scope.chkgroup == true) {
                innerContents = document.getElementById("feegroupexcel").innerHTML;
                //reportname = "#feegroupexcel";
            }
            else if ($scope.chkhead == true) {
                innerContents = document.getElementById("feeheadexcel").innerHTML;
              

            }
            else if ($scope.chkyearlyhead == true) {
                innerContents = document.getElementById("yearlygroupexcel").innerHTML;

            
            }
            else if ($scope.chkinstallment == true) {
               
                innerContents = document.getElementById("installmentexcel").innerHTML;

            }
            else if ($scope.chkfine == true) {
                innerContents = document.getElementById("fineexcel").innerHTML;

              

            }
            else if ($scope.chkclass == true) {
                innerContents = document.getElementById("classcategoryexcel").innerHTML;

                reportname = "#classcategoryexcel";
            }
            else if ($scope.chkprivileges == true) {
                innerContents = document.getElementById("previlegeexcel").innerHTML;

                reportname = "#previlegeexcel";
            }
            else if ($scope.chkterms == true) {
                innerContents = document.getElementById("feetermexcel").innerHTML;

                reportname = "#feetermexcel";
            }
            else if ($scope.chkamount == true) {
                innerContents = document.getElementById("feeamountexcel").innerHTML;

                reportname = "#feeamountexcel";

            }
            else if ($scope.chkcustom == true) {
                innerContents = document.getElementById("feecustomgroupexcel").innerHTML;

                reportname = "#feecustomgroupexcel";
            }
   

           // if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
            //var innerContents = document.getElementById(reportname).innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                    '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                    '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
              //  $state.reload();
           // }
          //  else {
          //      swal("Please Select Records to be Printed");
          //  }
        }


        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.Clearid = function () {
            $state.reload();
            $scope.loadbasicdata();
        }

    }

})();







