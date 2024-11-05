(function () {
    'use strict';
    angular
        .module('app')
        .controller('CollegeStudentTCReportController', CollegeStudentTCReportController)

    CollegeStudentTCReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache']
    function CollegeStudentTCReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache) {

        $scope.print_flag = true;
        $scope.excel_flag = true;
        $scope.printstudents = [];

        $scope.searchValue = '';
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
        } else {
            paginationformasters = 10;
            copty = "";
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        } else {
            logopath = "";
        }
        $scope.imgname = logopath;

        var _date = new Date();
        $scope.today_date = _date;
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //$scope.userPrivileges = "";
        //var pageid = $stateParams.pageId;  
        $scope.sortKey = 'ASMCL_Order';
        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.submitted = false;
        $scope.albumNameArraycolumn = [];
        $scope.columnsTest = [];

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            var pageid = 1;
            apiService.getURI("CollegeStudentTCReport/getalldetails", pageid).
                then(function (promise) {

                    $scope.yearlst = promise.getyear;
                    $scope.columnsTest = [];
                    $scope.headertest = [{ id: 'AMCST_FirstName', checked: true, value: 'Student Name' },
                    { id: 'AMCST_RegistrationNo', checked: true, value: 'Registration No.' },
                    { id: 'ACSTC_TCNO', checked: true, value: 'TC No.' },
                    { id: 'AMCO_CourseName', checked: true, value: 'Leaving Course' },
                    { id: 'AMB_BranchName', checked: true, value: 'Leaving Branch' },
                    { id: 'AMSE_SEMName', checked: true, value: 'Leaving Semester' },
                    { id: 'ACMS_SectionName', checked: true, value: 'Section' },
                    { id: 'ACSTC_LeavingReason', checked: true, value: 'Leaving Reason' },
                    { id: 'ACSTC_Remarks', checked: true, value: 'Remark' },
                    { id: 'AMCST_AdmNo', checked: true, value: 'Adm.No.' },
                    { id: 'AMCST_Date', checked: true, value: 'Date Of Admission' },
                    { id: 'ACSTC_TCDate', checked: true, value: 'TC Issue Date' },
                    { id: 'AMCST_FatherName', checked: true, value: 'Father Name' },
                    { id: 'AMCST_MotherName', checked: true, value: 'Mother Name' },
                    { id: 'AMCST_MobileNo', checked: true, value: 'Contact No.' },
                    { id: 'AMCST_emailId', checked: true, value: 'Email ID' },
                    { id: 'AMCST_DOB', checked: true, value: 'Date Of Birth' },
                    { id: 'AMCST_PerAdd3', checked: true, value: 'Permanent Address' },
                    { id: 'AMCST_ConStreet', checked: true, value: 'Present Address' },
                    { id: 'AMCST_AadharNo', checked: true, value: 'Government ID' }]
                });
        };

        $scope.onchangeyear = function () {

            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.report123 = false;
            $scope.columnsTest = [];

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("CollegeStudentTCReport/onchangeyear", data).then(function (promise) {
                $scope.getcourse = promise.getcourse;
            });
        };

        $scope.onchangecourse = function () {
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.columnsTest = [];
            $scope.report123 = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id
            };
            apiService.create("CollegeStudentTCReport/onchangecourse", data).then(function (promise) {
                $scope.getbranch = promise.getbranch;
            });
        };

        $scope.onchangebranch = function () {
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.columnsTest = [];
            $scope.report123 = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id
            };
            apiService.create("CollegeStudentTCReport/onchangebranch", data).then(function (promise) {
                $scope.getsemester = promise.getsemester;
            });
        };

        $scope.onchangesemester = function () {
            $scope.ACMS_Id = "";
            $scope.report123 = false;
            $scope.columnsTest = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id
            };
            apiService.create("CollegeStudentTCReport/onchangesemester", data).then(function (promise) {
                $scope.getsection = promise.getsection;
            });
        };

        $scope.ShowReport = function () {
            $scope.students = [];
            $scope.sortKey = 'ASMCL_Order';
            $scope.printstudents = [];
            $scope.searchValue = '';
            if ($scope.myForm.$valid) {
                $scope.albumNameArraycolumn = [];
                angular.forEach($scope.headertest, function (role) {
                    if (!!role.selected) $scope.albumNameArraycolumn.push({
                        columnID: role.id,
                        columnName: role.value
                    });
                });
                var amcoid = 0;
                var ambid = 0;
                var amseid = 0;
                var acmsid = 0;
                if ($scope.TC_allorind === "all") {
                    amcoid = 0;
                    ambid = 0;
                    amseid = 0;
                    acmsid = 0;
                } else {
                    amcoid = $scope.AMCO_Id;
                    ambid = $scope.AMB_Id;
                    amseid = $scope.AMSE_Id;
                    acmsid = $scope.ACMS_Id;
                }

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": amcoid,
                    "AMB_Id": ambid,
                    "AMSE_Id": amseid,
                    "ACMS_Id": acmsid,
                    "tcperortemp": $scope.tctemporperm,
                    "allorindi": $scope.TC_allorind,
                    "TempararyArrayheadList": $scope.albumNameArraycolumn
                };
                apiService.create("CollegeStudentTCReport/Getreportdetails", data).
                    then(function (promise) {

                        if (promise.getreport !== null && promise.getreport.length > 0) {
                            $scope.students = promise.getreport;
                            $scope.sortKey = 'ASMCL_Order';
                            $scope.columnsTest = promise.tempararyArrayheadList;
                            $scope.presentCountgrid = $scope.students.length;
                            $scope.print_flag = false;
                            $scope.excel_flag = false;
                            $scope.report123 = true;

                            angular.forEach($scope.yearlst, function (pdd) {

                                if (pdd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                    $scope.yearname = pdd.asmaY_Year;
                                }

                            });


                        }
                        else {
                            swal(" Record Not Found. !!");
                            $scope.students = [];
                            $scope.report123 = false;
                            $scope.print_flag = true;
                            $scope.excel_flag = true;
                        }
                    });


            } else {
                $scope.submitted = true;
            }
        };


        $scope.report123 = false;
        $scope.clssec = true;
        $scope.clssec = true;
        $scope.albumNameArraycolumn = [];
        $scope.addColumn = function (role) {
            $scope.all_header = $scope.headertest.every(function (itm) { return itm.selected; });

            if (role.selected === true) {
                $scope.albumNameArraycolumn.push(role);
            }
            else {
                var som = $scope.albumNameArraycolumn.indexOf(role);
                $scope.columnsTest.splice($scope.albumNameArraycolumn.indexOf(role), 1);
                $scope.albumNameArraycolumn.splice($scope.albumNameArraycolumn.indexOf(role), 1);
            }
        };

        $scope.Toggle_header = function () {
            var toggleStatus_header = $scope.all_header;
            angular.forEach($scope.headertest, function (itm) {
                itm.selected = toggleStatus_header;
            });
        };


        $scope.toggleAll = function () {

            var toggleStatus = $scope.all;
            angular.forEach($scope.students, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all === true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
                else {
                    $scope.printstudents.splice(itm);
                }
            });
        };


        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all = $scope.students.every(function (itm) { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }
        };

        //Exporting data(table id will be passed)
        $scope.exportToExcel = function (export_id) {
            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
                var exportHref = Excel.tableToExcel(export_id, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }
        };

        //Printing the data (div id will be passed)
        $scope.printData = function (printSectionId) {
            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
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
        };

        $scope.onclickloaddata = function () {

            if ($scope.TC_allorind === "all") {

                $scope.ASMAY_Id = "";
                $scope.AMCO_Id = "";
                $scope.AMB_Id = "";
                $scope.AMSE_Id = "";
                $scope.ACMS_Id = "";
                $scope.report123 = false;
                $scope.columnsTest = [];
                $scope.submitted = false;
                $scope.report123 = false;

                for (var i = 0; i < $scope.headertest.length; i++) {
                    $scope.headertest[i].selected = false;
                }
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
            }
            else if ($scope.TC_allorind === "indi") {
                $scope.ASMAY_Id = "";
                $scope.AMCO_Id = "";
                $scope.AMB_Id = "";
                $scope.AMSE_Id = "";
                $scope.ACMS_Id = "";
                $scope.report123 = false;
                $scope.columnsTest = [];
                $scope.submitted = false;
                $scope.report123 = false;

                for (var i1 = 0; i1 < $scope.headertest.length; i1++) {
                    $scope.headertest[i1].selected = false;
                }
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
            }
        };

        $scope.isOptionsRequired = function () {
            return !$scope.headertest.some(function (options) {
                return options.selected;
            });
        };



        $("#btnExport").click(function (e) {
            window.open('data:application/vnd.ms-excel,' + $('#Table').html());
            e.preventDefault();
        });

        $scope.clear = function () {
            $state.reload();
            $scope.grid_flag = false;
        };
    }
})();

