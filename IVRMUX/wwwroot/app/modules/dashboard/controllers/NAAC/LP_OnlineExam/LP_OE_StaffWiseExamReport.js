
(function () {
    'use strict';
    angular.module('app').controller('LP_OE_StaffWiseExamReportController', LP_OE_StaffWiseExamReportController)

    LP_OE_StaffWiseExamReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function LP_OE_StaffWiseExamReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.obj = {};
        $scope.obj.all2 = false;
        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        $scope.searchchkbx = "";
        var copty;

        $scope.maxdate = new Date();

        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;
        $scope.reportbtn = true;
        $scope.getclasslist = [];
        $scope.albumNameArraycolumn = [];
        $scope.LoadReport = function () {
            var pageid = 4;
            apiService.getURI("LP_OnlineExam/LoadReport", pageid).then(function (promise) {
                $scope.getclasslist = promise.getclasslist;
                $scope.getyearlist = promise.getyearlist;
                $scope.ASMAY_Id = promise.asmaY_Id;

                angular.forEach($scope.getyearlist, function (dd) {
                    if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                        $scope.maxdate = new Date(dd.asmaY_To_Date);
                        $scope.mindate = new Date(dd.asmaY_From_Date);

                        $scope.FromDate = new Date(dd.asmaY_From_Date);
                        $scope.ToDate = new Date(dd.asmaY_To_Date);
                    }
                });
            });
        };

        $scope.OnChangeYear = function () {
            angular.forEach($scope.getyearlist, function (dd) {
                if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                    $scope.maxdate = new Date(dd.asmaY_To_Date);
                    $scope.mindate = new Date(dd.asmaY_From_Date);

                    $scope.FromDate = new Date(dd.asmaY_From_Date);
                    $scope.ToDate = new Date(dd.asmaY_To_Date);
                }
            });
        };

        $scope.OnChangeRadiobtn = function () {
            $scope.reportbtn = true;
            $scope.GetReportDetails = [];
            $scope.getReport = [];
            $scope.ASMCL_Id = "";

        };

        $scope.OnChangeClass = function () {
            $scope.reportbtn = true;
            $scope.GetReportDetails = [];
            $scope.getReport = [];
        };
        //  $scope.submitted = false;
        $scope.submitted = false;
        $scope.GetReport = function () {
            $scope.submitted = true;
            $scope.GetReportDetails = [];
            $scope.getReport = [];
            $scope.albumNameArraycolumn = [];
            angular.forEach($scope.getclasslist, function (role) {
                if (!!role.selected) $scope.albumNameArraycolumn.push({
                    columnID: role.asmcL_Id,
                    columnName: role.asmcL_ClassName
                });
            });


            if ($scope.myForm.$valid) {
                var data = {
                    "Report_Type": $scope.entry,
                    "classlistarray": $scope.albumNameArraycolumn,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "FromDate": new Date($scope.FromDate).toDateString(),
                    "ToDate": new Date($scope.ToDate).toDateString()
                }

                apiService.create("LP_OnlineExam/GetStaffWiseExamReport", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.getReport !== null && promise.getReport.length > 0) {
                            $scope.GetReportDetails = promise.getReport;
                            $scope.reportbtn = false;

                            var finalamt = 0.00;
                            var totalamt = 0.00;

                            $scope.colspan = $scope.entry === 'SubjectWiseCount' ? 5 : 4;
                            $scope.asmclclassname = "";
                            angular.forEach($scope.GetReportDetails, function (amt) {
                                totalamt += amt.StaffConductedExams;
                            });

                            $scope.finalamt = totalamt.toFixed(2);

                        } else {
                            swal("No Records Found");
                        }
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.asmcL_ClassName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        };


        $scope.cancel = function () {
            $state.reload();
        };

        $scope.sort = function (key) {
            $scope.sortKey = key;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.exportToExcel = function (table1) {
            var reporttype = "";
            reporttype = '#excelSectionId';
            $scope.sheetname = "Online Exam Created Count Report ";

            var exportHref = Excel.tableToExcel(reporttype, $scope.sheetname);
            var excelname = $scope.sheetname + ".xls";
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);

        };

        $scope.printData = function (printSectionId) {
            var reporttype = "";
            reporttype = 'printSectionId';
            var innerContents = document.getElementById(reporttype).innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();

        };

        $scope.all_checkC = function (all) {
            $scope.usercheckS = all;
            var toggleStatus = $scope.usercheckS;
            angular.forEach($scope.getclasslist, function (role) {
                role.selected = toggleStatus;
            });

            $scope.classlistarray = [];
            angular.forEach($scope.getclasslist, function (qq) {
                if (qq.selected == true) {
                    $scope.classlistarray.push({ ASMCL_Id: qq.asmcL_Id })
                }
            });
        };

        $scope.togchkbxC = function () {
            $scope.classlistarray = [];
            angular.forEach($scope.getclasslist, function (item) {
                if (item.selected == true) {
                    $scope.classlistarray.push({ ASMCL_Id: item.asmcL_Id })
                }
            })
        }

        $scope.isOptionsRequired = function () {
            return !$scope.getclasslist.some(function (item) {
                return item.selected;
            });
        };

        $scope.Toggle_header = function () {
            var toggleStatus = $scope.obj.all2;
            angular.forEach($scope.getclasslist, function (itm) {
                itm.selected = toggleStatus;
            });
        };

        $scope.addColumn = function (role) {
            $scope.obj.all2 = $scope.getclasslist.every(function (itm) { return itm.selected; });
        };
    }
})();