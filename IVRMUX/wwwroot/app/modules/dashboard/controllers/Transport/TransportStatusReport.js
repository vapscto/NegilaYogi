(function () {
    'use strict';
    angular
.module('app')
.controller('TransportStatusReportController', TransportStatusReportController)

    TransportStatusReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function TransportStatusReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {
        $scope.sortKey = "astA_Id";
        $scope.columnsTest = [];
        $scope.obj = {};
        $scope.tadprint = false;
        $scope.exp_excel_flag = true;
        $scope.print_flag = true;
        $scope.printstudents = [];
        $scope.count12 = false;
        $scope.editcount = false;
        $scope.qwee = false;
        $scope.qwee1 = false;
        $scope.printstudents = [];
        $scope.class = true;
        $scope.alldisable = false;
        $scope.updated = false;

        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }

        $scope.itemsPerPage = paginationformasters;
        if ($scope.itemsPerPage == undefined || $scope.itemsPerPage == null) {
            $scope.itemsPerPage = 5;
        }
        $scope.currentPage = 1;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        $scope.reporsmart = false;

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.BindData = function () {
            apiService.getDATA("TransportStatusReport/getdata").then(function (promise) {
                if (promise != null) {
                    $scope.YearList = promise.yearList;
                    $scope.routendetails = promise.routendetails;
                    $scope.masterclass = promise.masterclass;
                }
                else {
                    swal("No Records Found")
                }
            })
        }
       
        $scope.edit = false;
        $scope.count12 == false;

        $scope.cnt12 = function ()
        {
            $scope.allandfalse = false;
            $scope.counttrue = false;
            $scope.printstudents = [];
            if ($scope.count12 == true) {
                $scope.qwee = true;
                $scope.qwee1 = true;
                $scope.edit = true;

            } else {
                $scope.qwee = false;
                $scope.qwee1 = false;
                $scope.edit = false;
            }
        }

        $scope.checkupdated = function () {
            if ($scope.updated == true)
            {
                $scope.alldisable = true;
                $scope.qwee = true;
                $scope.edit = true;
                $scope.class = false;
                $scope.allorindiv = 'All';
            }
            else {
                $scope.qwee = false;
                $scope.edit = false;
                $scope.class = true;
                $scope.alldisable = false;
            }
            
        }

        $scope.onclickloaddata = function () {


            if ($scope.allorindiv == "All") {

                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.regnonamestd = true;
                $scope.reporsmart = false;

            }
            else if ($scope.allorindiv === "indi") {
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.regnonamestd = false;
                $scope.reporsmart = false;

            }
            $scope.students = [];
        };

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            $scope.printstudents = [];
            angular.forEach($scope.students, function (itm) {
                itm.selected = toggleStatus;
                if (itm.selected == true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }               
            });
           
        }

        $scope.toggleAll1 = function () {
            var toggleStatus1 = $scope.all1;
            $scope.printstudents = [];
            angular.forEach($scope.countsts, function (itm) {
                itm.selected1 = toggleStatus1;
                if (itm.selected1 == true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }                
            });          
        }

        $scope.searchValue = '';

        $scope.filterValue = function (obj) {
            return angular.lowercase(obj.amsT_FirstName).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                 angular.lowercase(obj.trmR_PickRouteName).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                 angular.lowercase(obj.trmL_PickLocationName).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                 angular.lowercase(obj.trmR_DropRouteName).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                 angular.lowercase(obj.trmL_DropLocationName).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.cancel = function () {
            $state.reload();
        }

        $scope.cnt11 = function () {
            if ($scope.date12 == "apd") {
                $scope.edit = false;
                $scope.payment = "payyesno";

            } else {
                $scope.payment = "payyes";
                $scope.edit = true;
            }
        }

        $scope.getreport = function (obj) {
            $scope.all = "";
            $scope.searchValue = '';
            $scope.printstudents = [];
            // $scope.toggleAll();
            // $scope.students = [];
            if ($scope.myForm.$valid) {

                if ($scope.allorindiv == "All") {
                    $scope.sts1 = '';
                }
                var data =
                    {
                        "onclickloaddata": $scope.allorindiv,
                        "asmaY_Id": $scope.asmaY_Id,
                        "ASTA_ApplStatus": $scope.sts1,
                        "cnt12": $scope.count12,
                        "cnt11": $scope.date12,
                        "FMCB_toDATE": new Date($scope.FMCB_toDATE).toDateString(),
                        "FMCB_fromDATE": new Date($scope.FMCB_fromDATE).toDateString(),
                        "regorname_map": $scope.regorname_map,
                        "paymentoption": $scope.payment,
                        "ASMCL_Id": $scope.asmcL_Id,
                        "TRMR_Id": $scope.trmR_Id,
                        "updatedstu": $scope.updated,
                        "updatedtype": $scope.checktype

                    }
                apiService.create("TransportStatusReport/Getreportdetails", data).
                 then(function (promise) {
                     if ($scope.count12 == true) {
                         if (promise.messagelist.length == 0) {
                             swal("Record Not Found !!");
                             $state.reload();
                         }
                         else {
                             
                             $scope.countsts = promise.messagelist;
                             $scope.ApplicationFilled = $scope.countsts[0].ApplicationFilled;
                             $scope.ApplicationPaid = $scope.countsts[0].ApplicationPaid;
                             $scope.ApplicationNotPaid = $scope.countsts[0].ApplicationNotPaid;
                             $scope.Waiting = $scope.countsts[0].Waiting;
                             $scope.Approved = $scope.countsts[0].Approved;
                             $scope.Rejected = $scope.countsts[0].Rejected;

                             $scope.printstudents = $scope.countsts;

                          // $scope.presentCountgrid1 = $scope.countsts.length;
                             $scope.exp_excel_flag = false;
                             $scope.print_flag = false;
                             $scope.counttrue = true;
                             $scope.allandfalse = false;
                         }

                     } else {
                         if (promise.messagelist.length == 0) {
                             $scope.reporsmart = false;
                             swal("Record Not Found !!");
                             $state.reload();
                         }
                         else {
                             
                             $scope.allandfalse = true;
                             $scope.counttrue = false;
                             $scope.students = promise.messagelist;
                             $scope.presentCountgrid = $scope.students.length;
                             $scope.exp_excel_flag = false;
                             $scope.print_flag = false;

                             angular.forEach($scope.students, function (rr) {
                                 if (rr.ApplicationNo != null && rr.ApplicationNo != "" && rr.ApplicationNo != undefined) {
                                     rr.ApplicationNo = parseInt(rr.ApplicationNo);
                                 }

                             })
                         }
                     }
                 })
            }
            else {
                $scope.submitted = true;
            }
        }
       
        $scope.printData = function () {
            if ($scope.printstudents !== null && $scope.students.length > 0 || $scope.countsts.length > 0) {
                var innerContents = "";

                if ($scope.count12==true) {
                    innerContents = document.getElementById("printareaId2").innerHTML;
                }
                else {
                    innerContents = document.getElementById("printareaId").innerHTML;
                }
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
              '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
          '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
           '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
          '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
          );
                popupWinindow.document.close();
            }

            else {
                swal("Please Select Records to be Printed");
            }
        }

        $scope.exportToExcel = function () {
            $scope.fromdateforexcel = new Date($scope.FMCB_fromDATE).toDateString();
            $scope.todateforexcel = new Date($scope.FMCB_toDATE).toDateString()
            var printSectionId = '';
            if ($scope.count12 == true) {
                printSectionId = '#table2';
            }
            else {
                printSectionId = '#table1';
            }
            var excelname = 'Transport Status Report.xls';

            if ($scope.updated == true)
            {
                excelname = "Transport Status Report Updated Datewise.xls";
            }
            else if ($scope.date12 == 'apd') {
                excelname = "Transport Status Report Application Datewise.xls";
            }
            else if ($scope.date12 == 'aprd') {
                excelname = "Transport Status Report Appprove Datewise.xls";
            }
             

            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
                var exportHref = Excel.tableToExcel(printSectionId, 'Transport Report');
                //$timeout(function () { location.href = exportHref; }, 100);
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = excelname;
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);
                //$state.reload();
            }
            else {
                swal("Please Select Records to be Exported");
            }

        }

        $scope.validreport = function () {
            $scope.students = [];
        }


        $scope.optionToggled = function (SelectedStudentRecord, index) {
            
            $scope.all = $scope.students.every(function (itm)
            { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }
        }


        $scope.optionToggled1 = function (SelectedStudentRecord, index) {
            $scope.all = $scope.countsts.every(function (itm)
            { return itm.selected1; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }
        }
    };

})();