

(function () {
    'use strict';
    angular
        .module('app')
        .controller('GenderWisePaidDetailsController', GenderWisePaidDetailsController)

    GenderWisePaidDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function GenderWisePaidDetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {
        $scope.IsHiddenup = true;
        $scope.print_flag = false;
        $scope.route = false;
        $scope.bus = true;
        var fromdate = "";
        $scope.groupwiseshow = false;
        $scope.headwiseshow = false;
        $scope.termwiseshow = false;
        $scope.classwiseshow = false;
        $scope.classcategoryshow = false;
        $scope.grpwisedata = false;
        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
          var institutionid = configsettings[0].mI_Id;
      
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        if (configsettings.length > 0) {
            grouporterm = configsettings[0].fmC_GroupOrTermFlg;
            autoreceipt = configsettings[0].fmC_AutoReceiptFeeGroupFlag;
            receiptformat = configsettings[0].fmC_Receipt_Format;
            mergeinstallment = configsettings[0].fmC_RInstallmentsMergeFlag;//added by kiran
        }
      

       
        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 5
            $scope.bus = true;
            var pageid = 1;
            var data = {
                "reporttype": grouporterm,
            }

            apiService.create("GenderWisePaidDetails/getalldetails123", data).
                then(function (promise) {

                    $scope.arrlist6 = promise.acayear;
                    

                   $scope.asmaY_Id = academicyrlst[0].asmaY_Id;

                  
                    if (promise.grouplist.length != null) {
                        angular.forEach(promise.grouplist, function (tr) {
                            tr.fmG_Id_chk = true;
                        })
                    }
                    if (promise.customlist.length != null) {
                        angular.forEach(promise.customlist, function (tr) {
                            tr.fmgG_Id_chk = true;
                        })
                    }
                    $scope.custom = promise.customlist;

                    $scope.term = promise.fillmastergroup;
                    $scope.group = promise.grouplist;
                    $scope.group = promise.grouplist;
             

                })
        }

      
        $scope.ShowHideup = function () {

            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.isOptionsRequired = function () {

            return !$scope.termlst.some(function (options) {
                return options.trmids;
            });
        }

        $scope.isRequired = function () {

            return !$scope.grplst.some(function (options) {
                return options.grpids;
            });
        }
        $scope.due_date_div = false;
        $scope.due_date_check = function () {
            if ($scope.due_date == "1") {
                $scope.due_date_div = true;
            }
            else {
                $scope.due_date_div = false;
            }
        }

        $scope.onselectclass = function () {

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }


            apiService.create("GenderWisePaidDetails/getsection", data).
                then(function (promise) {
                    $scope.sectioncount = promise.sectionlist;

                })
        }

        $scope.onselectsection = function () {

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "AMSC_Id": $scope.asmC_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }


            apiService.create("GenderWisePaidDetails/getstudent", data).
                then(function (promise) {
                    $scope.studlist = promise.admsudentslist;
                })
        }





        $scope.get_groups = function () {
            var FMGG_Ids = [];
           
                angular.forEach($scope.custom, function (ty) {
                    if (ty.fmgG_Id_chk) {
                        FMGG_Ids.push(ty.fmgG_Id);
                    }
                })
           

            if (FMGG_Ids.length > 0) {
                var data = {
                    "reporttype": grouporterm,
                    FMGG_Ids: FMGG_Ids
                }

                apiService.create("FeeDefaulterReport/get_groups", data).
                    then(function (promise) {

                        if (promise.grouplist.length > 0) {
                            angular.forEach(promise.grouplist, function (tr) {
                                tr.fmG_Id_chk = true;
                            })
                        }
                        
                        $scope.group = promise.grouplist;
                    });
            }
            else if (FMGG_Ids.length == 0) {
             
                $scope.group = [];
            }


        }


        $scope.rndind = "All";
     
        $scope.individual_Name_Regno = false;
        $scope.individual_Student = false;
        $scope.print_flag = true;
        $scope.Grid_view = false;
        $scope.submitted = false;
       
        $scope.showreport = function (asmaY_Id, asmcL_Id, asmC_Id, amst_Id, termlst, due_date, asd) {

            if ($scope.myForm.$valid) {
                var cnt_trm = 0;
                $scope.albumNameArraycolumn = [];
                
                $scope.albumNameArraycolumn1 = [];
                angular.forEach($scope.custom, function (custom1) {
                    if (!!custom1.selected) $scope.albumNameArraycolumn1.push({
                        columnID1: custom1.fmgG_Id

                    });
                })

                $scope.albumNameArraycolumn2 = [];
                angular.forEach($scope.group, function (group) {
                    if (!!group.selected) $scope.albumNameArraycolumn2.push({
                        columnID2: group.fmG_Id

                    });
                })

                $scope.albumNameArraycolumn3 = [];
                angular.forEach($scope.groupcount, function (groupcount) {
                    if (!!groupcount.selected) $scope.albumNameArraycolumn3.push({
                        columnID3: groupcount.fmT_Id

                    });
                })

                angular.forEach($scope.arrlist6, function (ty) {
                    if (ty.asmaY_Id == $scope.asmaY_Id) {
                        $scope.year = ty.asmaY_Year;
                    }
                })



                var FMGG_Ids = [];
                var FMG_Ids = [];
                var FMT_Ids = [];
               
                    angular.forEach($scope.custom, function (ty) {
                        if (ty.fmgG_Id_chk) {
                            FMGG_Ids.push(ty.fmgG_Id);
                        }
                    })


                    angular.forEach($scope.group, function (ty) {
                        if (ty.fmG_Id_chk) {
                            FMG_Ids.push(ty.fmG_Id);
                        }
                    })
                   
              
                angular.forEach($scope.term, function (ty) {
                    if (ty.fmT_Id_chk) {
                        FMT_Ids.push(ty.fmT_Id);
                    }
                })

             
                



                var data = {
                    "asmay_id": $scope.asmaY_Id,
                    
                    FMGG_Ids: FMGG_Ids,
                    FMG_Ids: FMG_Ids,
                    FMT_Ids: FMT_Ids,
                    
                    "type": $scope.type
                }


                apiService.create("GenderWisePaidDetails/getreport", data).then(function (promise) {

                    
                   
                    $scope.groupwisereport = [];
                    $scope.headwisereport = [];
                    $scope.termwisereport = [];
                    $scope.classwisereport = [];
                    $scope.classcategorywisereport = [];
                    if (promise.getreportdata != null) {
                        $scope.grpwisedata = true;
                        if ($scope.type == 'GroupWise') {
                            $scope.groupwisereport = promise.getreportdata;
                            $scope.groupwiseshow = true;
                        }
                        else if ($scope.type == 'HeadWise') {
                            $scope.headwisereport = promise.getreportdata;
                            $scope.headwiseshow = true;
                       $scope.groupwiseshow = false;
                        }
                        else if ($scope.type == 'TermWise') {
                            $scope.termwisereport = promise.getreportdata;
                            $scope.termwiseshow = true;
                        $scope.headwiseshow = false;
                       $scope.groupwiseshow = false;
                        }
                        else if ($scope.type == 'ClassWise') {
                            $scope.classwisereport = promise.getreportdata;
                            $scope.classwiseshow = true;
                            $scope.termwiseshow = false;
                        $scope.headwiseshow = false;
                       $scope.groupwiseshow = false;
                        }
                        else if ($scope.type == 'ClassCategoryWise') {
                            $scope.classcategorywisereport = promise.getreportdata;
                            $scope.classcategoryshow = true;
                         $scope.classwiseshow = false;
                            $scope.termwiseshow = false;
                        $scope.headwiseshow = false;
                       $scope.groupwiseshow = false;

                        }
                    }
                    else {
                        swal("Records Not Found");
                        $state.reload();
                   
                    }
                   
                
                  
     


                })
            }
            else {
                $scope.submitted = true;

            }
        };

        $scope.getTotal126 = function (int) {
            var total = 0;
            angular.forEach($scope.students, function (e) {
                total += e.paidAmt;
            });
            return total;
        };


        $scope.onselectmodeof = function () {

            var VALU;
            if ($scope.BRcheck == "1") {
                VALU = $scope.CMR_Id;
            }
            else {
                VALU = 'Uncheck';
            }
            var data = {
                "filterinitialdata": $scope.filterdata,
                "fillbusroutestudents": VALU,
                "fillseccls": $scope.sectiondrp,
                "fillclasflg": $scope.clsdrp,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("GenderWisePaidDetails/getgroupmappedheads", data).
                then(function (promise) {

                    $scope.studentlst = promise.admsudentslist1;
                    $scope.Amst_Id = "";
                })
        };




        $scope.onclickloaddata = function () {
            if ($scope.rndind == "All") {
              
                $scope.individual_Name_Regno = false;
                 
                $scope.individual_Student = false;
            }
            else if ($scope.rndind == "Individual") {
               
                $scope.individual_Name_Regno = true;
               
                $scope.individual_Student = true;
            }
        };

        $scope.BusRoute_section = true;
        $scope.BusRoute_class = true;
        $scope.busroutedisable = true;
        $scope.Bus_Route = false;

        $scope.Clearid = function () {

            $state.reload();
            $scope.loaddata();
        }



        $scope.onclickloaddataCS = function () {


            if ($scope.BRcheck == "1") {

                $scope.BusRoute_section = false;

                $scope.BusRoute_class = false;
                $scope.busroutedisable = false;
                $scope.Bus_Route = true;

            }
            else {

                $scope.BusRoute_section = true;

                $scope.BusRoute_class = true;
                $scope.busroutedisable = true;
                $scope.Bus_Route = false;
            }

        };




        $scope.printToCart = function () {

            var pdss = "";
            if ($scope.userid == 364) {
                $scope.route = true;
                pdss = 'trpt'
            }
            else if ($scope.userid == 362) {
                $scope.route = false;
                pdss = 'glstrpt'
            }
            else {
                $scope.route = false;
                pdss = 'printrcp'
            }

            var innerContents = document.getElementById(pdss).innerHTML;

            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BArrearReport/BArrearReportPdf.css"/>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }





    }
})();

