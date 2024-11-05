
(function () {
    'use strict';
    angular
        .module('app')
        .controller('StudentAttendanceReportController1', StudentAttendanceReportController1)

    StudentAttendanceReportController1.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache','$filter']
    function StudentAttendanceReportController1($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {
        $scope.currentPage = 1;
        //   $scope.itemsPerPage = 10;
        $scope.printdatatable = [];
        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.detail_checked_subject = false;

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
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;
        //$scope.itemsPerPage = 10;




        $scope.obj = {};
        $scope.StuAttRptDropdownList = function () {

            $scope.studentname = false;
            $scope.ftdate = false;
            $scope.monthname = true;
            $scope.grid_flag = false;
            $scope.excel_flag = true;
            $scope.print_flag = true;
            $scope.order_flag = false;
            $scope.report_btn = false;


            apiService.get("StudentAttendanceReport/getdetails/").then(function (promise) {
                {

                    $scope.all = true;
                    $scope.yearDropdown = promise.academicList;
                    $scope.allAcademicYear1 = promise.academicListdefault;

                    $scope.classDropdown = promise.classlist;
                    $scope.sectionDropdown = promise.sectionList;
                    $scope.monthDropdown = promise.monthList;
                    $scope.fromdaily = new Date();
                    $scope.fromdate = new Date();
                    $scope.maxDatef = new Date(
                        $scope.fromdate.getFullYear(),
                        $scope.fromdate.getMonth(),
                        $scope.fromdate.getDate());

                    $scope.todate = new Date();
                    $scope.maxDatet = new Date(
                        $scope.todate.getFullYear(),
                        $scope.todate.getMonth(),
                        $scope.todate.getDate());

                    $scope.obj.fromdaily = new Date();
                    console.log($scope.obj.fromdaily);
                    $scope.maxDatedf = new Date(
                        $scope.fromdaily.getFullYear(),
                        $scope.fromdaily.getMonth(),
                        $scope.fromdaily.getDate());
                }
            });
        };


        $scope.onchangeyear = function () {
            $scope.obj.todate = "";
            $scope.obj.fromdate = "";
            $scope.obj.hrmE_Id = "";
            $scope.detail_checked_subject = false;

            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            };
            apiService.create("StudentAttendanceReport/onchangeyear", data).then(function (promise) {

                if (promise != null) {
                    $scope.classDropdown = promise.classlist;
                    $scope.stafflist = promise.stafflist;

                    for (var i = 0; i < $scope.yearDropdown.length; i++) {
                        name = $scope.yearDropdown[i].asmaY_Id;

                        if (name == $scope.asmaY_Id) {


                            $scope.frommon = "";
                            $scope.tomon = "";
                            $scope.fromDay = "";
                            $scope.toDay = "";


                            $scope.minDatef = new Date($scope.yearDropdown[i].asmaY_From_Date);

                            $scope.maxDatef = new Date($scope.yearDropdown[i].asmaY_To_Date);

                            // $scope.obj.minDatef = new Date($scope.yearDropdown[i].asmaY_From_Date);

                            $scope.obj.maxDatet = new Date($scope.yearDropdown[i].asmaY_To_Date);

                        }
                    }
                } else {
                    swal("No Record Found");
                }
            });
        };

        $scope.onclasschange = function (obj) {
            $scope.detail_checked_subject = false;
            $scope.obj.hrmE_Id = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": obj.asmcL_Id
            };

            apiService.create("StudentAttendanceReport/onclasschange", data).then(function (promise) {

                if (promise != null) {
                    $scope.sectionDropdown = promise.sectionList;
                } else {
                    swal("No Record Found");
                }
            });
        };

        $scope.onsectionchange = function (obj) {
            $scope.detail_checked_subject = false;
            $scope.obj.hrmE_Id = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": obj.asmcL_Id,
                "ASMS_Id": obj.asmS_Id
            };

            apiService.create("StudentAttendanceReport/onsectionchange", data).then(function (promise) {

                if (promise != null) {
                    $scope.stafflist = promise.stafflist;
                } else {
                    swal("No Record Found");
                }
            });
        };

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
                    $scope.printdatatable = [];
                }
            });
        }
        $scope.printdatatabledate = [];
        $scope.toggleAlldate = function () {
            $scope.printdatatabledate = [];
            var toggleStatus1 = $scope.all2date;
            angular.forEach($scope.filterValue2, function (itm) {
                itm.selected = toggleStatus1;
                if ($scope.all2date == true) {
                    $scope.printdatatabledate.push(itm);
                }
                else {
                    $scope.printdatatabledate.splice(itm);
                    $scope.printdatatabledate = [];
                }
            });
        }

        $scope.optionToggleddate = function (SelectedStudentRecord, index) {
            $scope.all2date = $scope.students1.every(function (itm) { return itm.selected; });
            if ($scope.printdatatabledate.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatabledate.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatabledate.splice($scope.printdatatabledate.indexOf(SelectedStudentRecord), 1);
                $scope.printdatatabledate = [];
            }
        }



        $scope.propertyName = 'name';
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };


        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all2 = $scope.students.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
                $scope.printdatatable = [];
            }
        }

        $scope.exportToExcel = function () {
            if ($scope.checkoruncheck == true) {
                if ($scope.printdatatabledate !== null && $scope.printdatatabledate.length > 0) {
                    var exportHref = Excel.tableToExcel('#table21', 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    swal("Please Select Records to be Exported");
                }
            }
            else {
                if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                    var exportHref = Excel.tableToExcel('#table', 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    swal("Please Select Records to be Exported");
                }
            }




        }

        $scope.printData = function () {
            if ($scope.checkoruncheck == true) {
                if ($scope.printdatatabledate !== null && $scope.printdatatabledate.length > 0) {
                    var innerContents = document.getElementById("printSectionId1").innerHTML;
                    var popupWinindow = window.open('');
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
            else {
                if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                    var innerContents = document.getElementById("printSectionId").innerHTML;
                    var popupWinindow = window.open('');
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

        $scope.getDataByType = function (type) {
            $scope.detail_checked_subject = false;
            $scope.obj.hrmE_Id = "";
            if (type == 1)      //all
            {
                $scope.studentname = false;
                $scope.order_flag = false
                $scope.grid_flag = false;

                $scope.grid_flag = false;
            }

            else if (type == 2)         //individual
            {
                $scope.AMST_Id = "";
                $scope.studentname = true;
                $scope.order_flag = true;

                $scope.submitted = false;

                $scope.grid_flag = false;
                $scope.stu_name = true;

            }
        }

        $scope.betweendate = function () {
            $scope.ftdate = true;
            $scope.monthname = false;
            $scope.ftdaily = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.grid_flag = false;
            $scope.todatef = true;
            //  $scope.amM_Id = "";
            // $scope.ftdaily = "";
        }

        $scope.mondate = function () {
            $scope.ftdate = false;
            $scope.monthname = true;
            $scope.ftdaily = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.grid_flag = false;
            $scope.amM_Id = "";
        }

        $scope.daily = function () {
            $scope.ftdate = false;
            $scope.monthname = false;
            $scope.ftdaily = true;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.grid_flag = false;
            // $scope.amM_Id = "";
        }

        $scope.type1 = "1";

        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        //Showing Report
        $scope.submitted = false;

        $scope.getreportdiv = function (obj) {
            var data = "";
            var checkdate = 0;
            $scope.printdatatable = [];
            $scope.searchValue = "";
            $scope.searchValue1 = "";
            var hrmeid = 0;
            if ($scope.detail_checked_subject == false) {
                hrmeid = 0;
            } else {
                hrmeid = obj.hrmE_Id.hrmE_Id;
            }

            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": obj.asmcL_Id,
                    "ASMC_Id": obj.asmS_Id,
                    "fromdate": new Date(obj.fromdate).toDateString(),
                    "todate": new Date(obj.todate).toDateString(),
                    "flag": $scope.type,
                    "hrmE_Id": hrmeid
                }
                apiService.create("StudentAttendanceReport/getreportdiv", data)
                    .then(function (promise) {


                        if (promise.studentAttendanceList == null || promise.studentAttendanceList.length == 0) {
                            $scope.students = [];
                            $scope.students1 = [];
                            swal("Record Not Found !");
                            $scope.grid_flag = false;
                            $scope.excel_flag = true;
                            $scope.print_flag = true;
                            $scope.datewised = false;
                            $scope.datewisen = false;
                        }
                        else {

                            angular.forEach($scope.yearDropdown, function (dd) {
                                if ($scope.asmaY_Id == dd.asmaY_Id) {
                                    $scope.year = dd.asmaY_Year;
                                    $scope.fromdated = $filter('date')((new Date(obj.fromdate)), 'dd/MM/yyyy');
                                    $scope.fromdatetd = $filter('date')((new Date(obj.todate)), 'dd/MM/yyyy');

                                    $scope.details = "For Year : " + $scope.year + "  From Date : " + $scope.fromdated + " To Date :" + $scope.fromdatetd;


                                }
                            })


                            $scope.datewised = false;
                            $scope.datewisen = true;
                            $scope.students = promise.studentAttendanceList;
                            $scope.presentCountgrid = $scope.students.length;
                            $scope.grid_flag = true;
                            $scope.excel_flag = false;
                            $scope.print_flag = false;
                        }
                    })
            }
            else {

                $scope.submitted = true;
            }
        }

        $scope.Clearid = function () {
            $state.reload();
            $scope.asmaY_Id = "";
            $scope.asmcL_Id = "";
            $scope.asmC_Id = "";
            $scope.fromdate = "";
            $scope.todate = "";
            // $scope.amsT_Id = "";
            $scope.amM_Id = "";
        }

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.setTodate = function (data1) {
            $scope.obj.todate = "";

            $scope.todatef = false;
            $scope.todate1 = data1.fromdate;

            $scope.minDatet = new Date(
                $scope.todate1.getFullYear(),
                $scope.todate1.getMonth(),
                $scope.todate1.getDate() + 1);
        };


        $scope.setTodate1 = function (datato) {

            $scope.todate2 = datato.todate;
            $scope.minDatet = new Date(
                $scope.todate2.getFullYear(),
                $scope.todate2.getMonth(),
                $scope.todate2.getDate() + 1);
        }

    }
})();