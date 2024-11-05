(function () {
    'use strict';
    angular
        .module('app')
        .controller('DatewiseattendancereportController', DatewiseattendancereportController)
    DatewiseattendancereportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'superCache', 'Excel', '$timeout', '$filter']
    function DatewiseattendancereportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, superCache, Excel, $timeout, $filter) {

        $scope.grid_flag = false;
        $scope.tadprint = false;
        $scope.items = {};
        $scope.class = false;
        $scope.subject = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.printdatatable = [];
        $scope.excel_flag = true;
        $scope.print_flag = true;
        $scope.printdatatablesubject = [];
        $scope.searchValue = '';
        $scope.searchValue1 = '';

        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.ts = {};
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;

        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            }
        } else {
            paginationformasters = 10;
            copty = '';
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;



        $scope.loadInitialData = function () {
            var page = 2;
            apiService.getDATA("DatewiseattendanceReport/getdata/2").then(function (promise) {

                if (promise != null) {
                    $scope.newuser1 = promise.yearlist;
                }
                else {
                    swal("No Records Found");
                    $state.reload();
                }
            });
        };

        $scope.onchangeyear = function (ts) {

            angular.forEach($scope.newuser1, function (ddd) {
                if (ddd.asmaY_Id == ts.asmaY_Id) {
                    $scope.minDatedof = new Date(ddd.asmaY_From_Date);
                    $scope.maxDatedof = new Date(ddd.asmaY_To_Date);
                }
            })

            var data = {
                "ASMAY_Id": ts.asmaY_Id
            }
            apiService.create("DatewiseattendanceReport/onchangeyear", data).then(function (promise) {
                if (promise != null) {
                    $scope.classList = promise.classlist
                }
            });
        };


        $scope.onchangeclass = function (ts) {
            var data = {
                "ASMAY_Id": ts.asmaY_Id,
                "ASMCL_Id": ts.asmcL_Id
            }
            apiService.create("DatewiseattendanceReport/onchangeclass", data).then(function (promise) {
                if (promise != null) {
                    $scope.sectionList = promise.sectionlist
                }
            });
        };

        $scope.Report = function (ts) {

            $scope.submitted = false;

            if ($scope.myForm.$valid) {

                var sectionid = 0;
                var classid = 0;

                if (ts.optradio == "all") {
                    sectionid = 0;
                    classid = 0;
                } else {
                    sectionid = ts.asmS_Id;
                    classid = ts.asmcL_Id;
                }

                var data = {
                    "ASMAY_Id": ts.asmaY_Id,
                    "ASMCL_Id": classid,
                    "ASMS_Id": sectionid,
                    "flag": ts.optradio,
                    "type": ts.optradiof1,
                    "fromdate": new Date(ts.FromDate).toDateString()
                }
                apiService.create("DatewiseattendanceReport/getreport", data).then(function (promise) {

                    if (promise != null) {

                        if (ts.optradiof1 == 'f1') {
                            $scope.loginPData = promise.reportdata;
                            if ($scope.loginPData.length == 0) {
                                swal("No Records Found");
                            } else {
                                $scope.presentCountgrid = $scope.loginPData.length;
                                $scope.class = true;
                            }
                        } else {
                            $scope.loginPData22 = promise.reportdata;
                            if ($scope.loginPData22.length == 0) {
                                swal("No Records Found");
                            } else {
                                $scope.presentCountgridd = $scope.loginPData22.length;
                                $scope.class = true;
                            }
                        }

                        angular.forEach($scope.yearlist, function (dd) {
                            if (dd.asmaY_Id == ts.asmaY_Id) {
                                $scope.yearname = dd.asmaY_Year;
                            }
                        })

                    } else {
                        swal("No Records Found");
                    }
                })

            } else {
                $scope.submitted = true;
            }
        }



        $scope.submitted = false;

        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.all2 = $scope.loginPData.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
        };

        $scope.toggleAll = function (ts) {
            $scope.printdatatable = [];
            var toggleStatus = ts.all2;
            angular.forEach($scope.filterValue1, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all2 == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
        };

        $scope.optionToggled2 = function (SelectedStudentRecord, index) {

            $scope.all3 = $scope.loginPDatasubject.every(function (itm1) { return itm1.selected; });
            if ($scope.printdatatablesubject.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatablesubject.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatablesubject.splice($scope.printdatatablesubject.indexOf(SelectedStudentRecord), 1);
            }
        };

        $scope.toggleAll2 = function () {

            $scope.printdatatablesubject = [];
            var toggleStatus1 = $scope.all3;
            angular.forEach($scope.filterValue2, function (itm1) {
                itm1.selected = toggleStatus1;
                if ($scope.all3 == true) {
                    if ($scope.printdatatablesubject.indexOf(itm1) === -1) {
                        $scope.printdatatablesubject.push(itm1);
                    }
                }
                else {
                    $scope.printdatatablesubject.splice(itm1);
                }
            });
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.sortBy1 = function (propertyName1) {

            $scope.reverse1 = ($scope.propertyName1 === propertyName1) ? !$scope.reverse1 : false;
            $scope.propertyName1 = propertyName1;
        };


        var abc = "";
        $scope.minall = abc;
        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden = $scope.IsHidden ? false : true;
        };

        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        };

        $scope.IsHidden2 = true;
        $scope.ShowHide2 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden2 = $scope.IsHidden2 ? false : true;
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };


        $scope.radiobtn = function () {
            if ($scope.type === '1') {
                $scope.class = false;
                $scope.subject = false;
                $scope.excel_flag = true;
                $scope.print_flag = true;
            }
            else if ($scope.type === '2') {
                $scope.class = false;
                $scope.subject = false;
                $scope.excel_flag = true;
                $scope.print_flag = true;
            }
            else if ($scope.type == '3') {
                $scope.loginPData = "";
                $scope.presentCountgrid = "";
                $scope.class = false;
                $scope.subject = false;
            }
        };

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.exportToExcel = function (ts) {
            if (ts.optradiof1 == 'f1') {
                var exportHref = Excel.tableToExcel('#table', 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                exportHref = Excel.tableToExcel('#table1', 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
        };

        $scope.print = function (ts) {

            //$scope.date = new Date();

            $scope.attdate = new Date(ts.FromDate).toDateString();

            if (ts.optradiof1 == 'f1') {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );

            } else {
                var innerContents = document.getElementById("printSectionId1").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
            }
            popupWinindow.document.close();
        };
    }
})
    ();