

(function () {
    'use strict';
    angular
.module('app')
.controller('FeeChallanReportController', FeeChallanReportController123)

    FeeChallanReportController123.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache','Excel', '$timeout', '$filter']
    function FeeChallanReportController123($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter) {
        $scope.IsHiddenup = true;
        $scope.itemterm = [];
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
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
        $scope.search = "";
        $scope.printdatatable = [];
        $scope.toggleAllstd = function () {
            
            $scope.printdatatable.length = 0;
            var toggleStatus = $scope.stdall;
            angular.forEach($scope.feechallanrpt, function (itm) {
                itm.stdselected = toggleStatus;
                if ($scope.stdall == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
            if ($scope.printdatatable.length != null) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            var balstd = 0;
            for (var q = 0; q < $scope.feechallanrpt.length; q++) {
                if ($scope.feechallanrpt[q].stdselected == true) {
                    balstd = Number(balstd) + Number($scope.feechallanrpt[q].FYP_Tot_Amount);
                   // balstd = balstd + $scope.feechallanrpt[q].fyP_Tot_Amount;
                }
            }
            $scope.selectedbalstd = balstd;
        }

        $scope.optionToggledstd = function (SelectedStudentRecord, index) {
            
           
            $scope.stdall = $scope.feechallanrpt.every(function (itm)
            { return itm.stdselected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printdatatable.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            
            var balstd = 0;
            for (var q = 0; q < $scope.feechallanrpt.length; q++) {
                if ($scope.feechallanrpt[q].stdselected == true) {
                    balstd = Number(balstd) + Number($scope.feechallanrpt[q].FYP_Tot_Amount);
                }
            }
            $scope.selectedbalstd = balstd;
        }


        $scope.loaddata = function () {
            
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 2;
            apiService.getURI("FeeChallanReport/getalldetails123", pageid).
        then(function (promise) {
            
            //$scope.acayyearbind = promise.acayear;
            $scope.yearlist = promise.acayear;
            $scope.sectiondrpre = promise.sectionlist;
            $scope.clsdrpdown = promise.classlist;
            $scope.studentlst = promise.admsudentslist;
           // $scope.printdatatable = [];

        })
        }
        

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.ShowHideup = function () {

            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        }
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.individual_drpdis = function () {
            if ($scope.individual == 1) {
                $scope.Challan_report = false;
                $scope.feechallanrpt = "";
            }
            else {
                $scope.Challan_report = false;
                $scope.feechallanrpt = "";
            }
        }



        $scope.clear_fee_challan = function () {

            $state.reload();
        }

        $scope.Challan_report = false;
        $scope.print_flag = true;
        $scope.submitted = false;


        //------get Class Name
        $scope.getcls = function (clsdrp) {
            
            for (var i = 0; i < $scope.clsdrpdown.length; i++) {
                if (clsdrp == $scope.clsdrpdown[i].asmcL_Id) {
                    $scope.clsname = $scope.clsdrpdown[i].asmcL_ClassName;
                }
            }
        }
        //------get Section Name
        $scope.getsection = function (sectiondrp) {
            
            for (var i = 0; i < $scope.sectiondrpre.length; i++) {
                if (sectiondrp == $scope.sectiondrpre[i].asmS_Id) {
                    $scope.sectionname = $scope.sectiondrpre[i].asmC_SectionName;
                }
            }
        }
        //--------------Fee Challan Get Repot
        $scope.showreport = function (asmaY_Id, clsdrp, sectiondrp, From_Date, To_Date) {
            $scope.stdall = false;
            if ($scope.myForm.$valid) {
                $scope.from_date = new Date($scope.From_Date).toDateString();
                $scope.to_date = new Date($scope.To_Date).toDateString();
                var typ = 1;
                var data = {
                    "asmayidpss": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.clsdrp,
                    "ASMS_Id": $scope.sectiondrp,
                    "fromdate": $scope.from_date,
                    "todate": $scope.to_date,
                    "typeofrpt": $scope.individual,
                }
                apiService.create("FeeChallanReport/getreport", data).
            then(function (promise) {
                var temp_final_report = [];
                if (promise.reportdatelist != null && promise.reportdatelist != "") {
                    //$scope.toggleAllstd = [];
                    $scope.Challan_report = true;
                    $scope.print_flag = false;
                    $scope.feechallanrpt = promise.reportdatelist;
                }
                else {
                    swal("No Record Found");
                    $scope.Challan_report = false;
                    $scope.print_flag = true;
                }
            })
            }
            else {
                $scope.submitted = true;
            }
        }


       

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

        };




        $scope.onclickloaddata = function () {
            if ($scope.rndind == "All") {
                //$scope.rbnsforall = true;
                $scope.individual_Name_Regno = false;
                //$scope.rbnsNameforall = true;    
                $scope.individual_Student = false;
            }
            else if ($scope.rndind == "Individual") {
                //$scope.rbnsforall = false;
                $scope.individual_Name_Regno = true;
                // $scope.rbnsNameforall = false;
                $scope.individual_Student = true;
            }
        };

        $scope.BusRoute_section = true;
        $scope.BusRoute_class = true;
        $scope.busroutedisable = true;
        $scope.Bus_Route = false;
        $scope.printdiv = false;
        $scope.print= false;

        $scope.exportToExcel = function () {
           

            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                if ($scope.individual == 1) {
                    var exportHref = Excel.tableToExcel(test2, 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    var exportHref = Excel.tableToExcel(test1, 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
               
            }


            else {
                swal("Please select records to be Exported");
            }
        }

            
      
        $scope.printData = function (printrcp) {
                
                if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                    var id = "";
                    if ($scope.individual == 1) {
                        $scope.printdiv = true;
                        id = 'printrcp';
                        var innerContents = document.getElementById(id).innerHTML;
                        var popupWinindow = window.open('');
                        popupWinindow.document.open();
                        popupWinindow.document.write('<html><head>' +
                            '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                            '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                            '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                            '</head><body onload="window.print()" onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                        popupWinindow.document.close();
                    }
                    else {
                        $scope.print = true;
                        id = 'printch';
                        var innerContents = document.getElementById(id).innerHTML;
                        var popupWinindow = window.open('');
                        popupWinindow.document.open();
                        popupWinindow.document.write('<html><head>' +
                            '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                            '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                            '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                            '</head><body onload="window.print()" onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                        popupWinindow.document.close();
                    }

                   
                }
                else {
                    swal("Please Select Records to be Printed");
                    $scope.printdiv = false;
                    $scope.print = false;
                }               
                $scope.printdiv = false;
                $scope.print = false;
            }
        $scope.printdatatable.length = 0;
    }
})();

