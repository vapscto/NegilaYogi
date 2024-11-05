(function () {
    'use strict';
    angular.module('app').controller('ClassTeacherReportAttendanceController', ClassTeacherReportAttendanceController)
    ClassTeacherReportAttendanceController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'superCache', 'Excel', '$timeout', '$filter']
    function ClassTeacherReportAttendanceController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, superCache, Excel, $timeout, $filter) {

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
        $scope.objj = {};

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;

        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        $scope.loadInitialData = function () {
            var page = 2
            apiService.getDATA("ClassTeacherReportAttendance/getdata/2").then(function (promise) {
                if (promise != null) {
                    $scope.newuser1 = promise.getyear;
                    $scope.allAcademicYear1 = promise.getyear1;
                    for (var i = 0; i < $scope.newuser1.length; i++) {
                        name = $scope.newuser1[i].asmaY_Id;
                        for (var j = 0; j < $scope.allAcademicYear1.length; j++) {
                            if (name == $scope.allAcademicYear1[j].asmaY_Id) {
                                $scope.newuser1[i].Selected = true;
                                $scope.asmaY_Id = $scope.allAcademicYear1[j].asmaY_Id;
                            }
                        }
                    }
                    $scope.categoryDropdown = promise.category_list;
                    $scope.categoryflag = promise.categoryflag;
                }
                else {
                    swal("No Records Found");
                    $state.reload();
                }
            })
        }

        $scope.submitted = false;
        $scope.savetmpldata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var AMC_Id = 0
                if ($scope.objj.amC_Id != 0 && $scope.objj.amC_Id > 0) {
                    AMC_Id = $scope.objj.amC_Id
                }
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "Flag": $scope.type,
                    "AMC_Id": AMC_Id
                }
                apiService.create("ClassTeacherReportAttendance/getreport/", data).then(function (promise) {
                    if (promise != null) {
                        if (promise.searchstudentDetails != null && promise.searchstudentDetails.length > 0) {
                            if ($scope.type == '1' || $scope.type == '3') {
                                $scope.class = true;
                                $scope.subject = false;
                                $scope.loginPData = promise.searchstudentDetails;
                                $scope.presentCountgrid = $scope.loginPData.length;
                            } else {

                                $scope.class = false;
                                $scope.subject = true;
                                $scope.loginPDatasubject = promise.searchstudentDetails;
                                $scope.presentCountgrid = $scope.loginPDatasubject.length;
                            }
                            $scope.excel_flag = false;
                            $scope.print_flag = false;
                            angular.forEach($scope.newuser1, function (dd) {
                                if (dd.asmaY_Id == $scope.asmaY_Id) {
                                    $scope.yearname = dd.asmaY_Year;
                                }
                            });
                        }
                        else {
                            swal("No Records Found");
                            $state.reload();
                        }
                    }
                    else {
                        swal("No Records Found");
                        $state.reload();
                    }
                });
            }
            else {
                $scope.excel_flag = true;
                $scope.print_flag = true;
                $scope.class = false;
                $scope.subject = false;
            }
        }

        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.all2 = $scope.loginPData.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
        }

        $scope.toggleAll = function () {
            $scope.printdatatable = [];
            var toggleStatus = $scope.all2;
            angular.forEach($scope.filterValue1, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all2 == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
        }

        $scope.optionToggled2 = function (SelectedStudentRecord, index) {

            $scope.all3 = $scope.loginPDatasubject.every(function (itm1) { return itm1.selected; });
            if ($scope.printdatatablesubject.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatablesubject.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatablesubject.splice($scope.printdatatablesubject.indexOf(SelectedStudentRecord), 1);
            }
        }

        $scope.toggleAll2 = function () {

            //var toggleStatus1 = $scope.all3;
            //angular.forEach($scope.loginPDatasubject, function (itm1) {
            //    itm1.selected = toggleStatus1;

            //});
            //if ($scope.all3 == true) {
            //    $scope.printdatatablesubject.push(itm1);
            //}
            //else {
            //    $scope.printdatatablesubject.splice(itm1);
            //}
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
        }


        //$scope.sortBy = function (propertyName) {
        //    $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
        //    $scope.propertyName = propertyName;
        //};

        //$scope.sortBy1 = function (propertyName) {
        //    $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
        //    $scope.propertyName = propertyName;
        //};

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

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
        }
        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        }
        $scope.IsHidden2 = true;
        $scope.ShowHide2 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden2 = $scope.IsHidden2 ? false : true;
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }


        $scope.radiobtn = function () {
            if ($scope.type == '1') {
                $scope.class = false;
                $scope.subject = false;
                $scope.excel_flag = true;
                $scope.print_flag = true;
            }
            else if ($scope.type == '2') {
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
        }

        $scope.cancel = function () {
            $state.reload();
        }

        $scope.exportToExcel = function () {
            if ($scope.type == '1' || $scope.type == '3') {
                if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                    var exportHref = Excel.tableToExcel('#table', 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    swal("Please Select Records to be Exported");
                }
            }
            else if ($scope.type == '2') {
                if ($scope.printdatatablesubject !== null && $scope.printdatatablesubject.length > 0) {
                    var exportHref = Excel.tableToExcel('#table1', 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    swal("Please Select Records to be Exported");
                }
            }
        }

        $scope.printData = function () {
            $scope.date = new Date();
            if ($scope.type == '1') {
                if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                    var innerContents = document.getElementById("printSectionId2").innerHTML;
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

            else if ($scope.type == '3') {
                if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                    var innerContents = document.getElementById("printSectionId").innerHTML;
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

            else if ($scope.type == '2') {
                if ($scope.printdatatablesubject !== null && $scope.printdatatablesubject.length > 0) {
                    var innerContents = document.getElementById("printSectionId3").innerHTML;
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

        }



        //$("#btnExport").click(function (e) {
        //    window.open('data:application/vnd.ms-excel,' + $('#Table').html());
        //    e.preventDefault();
        //});
    }
})
    ();