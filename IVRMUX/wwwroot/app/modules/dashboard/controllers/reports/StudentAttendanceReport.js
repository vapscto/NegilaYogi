
(function () {
    'use strict';
    angular.module('app').controller('StudentAttendanceReportController', StudentAttendanceReportController)
StudentAttendanceReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache']
    function StudentAttendanceReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache) {
        $scope.currentPage = 1;
        $scope.printdatatable = [];
        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.obj = {};
        $scope.usrname = localStorage.getItem('username');

        $scope.sortKey = 'AMAY_RollNo';
        $scope.sortReverse = false;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
      
        if (ivrmcofigsettings != null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            }
        }
      
        $scope.itemsPerPage = 10;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null) {
            if (admfigsettings.length > 0) {
                var logopath = admfigsettings[0].asC_Logo_Path;
            }
        }
     
        $scope.objj = {};
        $scope.imgname = logopath;

        $scope.obj.fromdate = new Date();
        $scope.obj.todate = new Date();
        $scope.obj.fromdaily = new Date();

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
                $scope.all = true;
                $scope.yearDropdown = promise.academicList;
                $scope.allAcademicYear1 = promise.academicListdefault;
                $scope.classDropdown = promise.classlist;
                $scope.sectionDropdown = promise.sectionList;
                $scope.monthDropdown = promise.monthList;
                $scope.categoryDropdown = promise.category_list;

                $scope.categoryflag = promise.categoryflag;
            });
        };


        $scope.getclass = function () {
            var amcid = 0;
            if ($scope.objj.amC_Id != null && $scope.objj.amC_Id != 0 && $scope.objj.amC_Id != undefined) {
                amcid = $scope.objj.amC_Id;
            }
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMC_Id": amcid
            };
            apiService.create("StudentAttendanceReport/getclass", data).then(function (promise) {
                $scope.classDropdown = promise.fillclass;
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
        };

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
        };

        $scope.optionToggleddate = function (SelectedStudentRecord, index) {
            $scope.all2date = $scope.students1.every(function (itm) { return itm.selected; });
            if ($scope.printdatatabledate.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatabledate.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatabledate.splice($scope.printdatatabledate.indexOf(SelectedStudentRecord), 1);
                $scope.printdatatabledate = [];
            }
        };



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
        };

        $scope.exportToExcel = function () {
            var exportHref = "";
            if ($scope.checkoruncheck === true) {
                if ($scope.printdatatabledate !== null && $scope.printdatatabledate.length > 0) {
                    exportHref = Excel.tableToExcel('#table2ddddd1', 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    swal("Please Select Records to be Exported");
                }
            }
            else {
                if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                    exportHref = Excel.tableToExcel('#table', 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    swal("Please Select Records to be Exported");
                }
            }
        };

        $scope.printData = function () {
            var innerContents = "";
            var popupWinindow = "";
            if ($scope.checkoruncheck === true) {
                if ($scope.printdatatabledate !== null && $scope.printdatatabledate.length > 0) {
                    innerContents = document.getElementById("printSectionId1").innerHTML;
                    popupWinindow = window.open('');
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
                    innerContents = document.getElementById("printSectionId").innerHTML;
                    popupWinindow = window.open('');
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
        };

        $scope.getDataByType = function (type) {
            if (type == 1) {
                $scope.studentname = false;
                $scope.order_flag = false;
                $scope.grid_flag = false;
                $scope.grid_flag = false;
            } else if (type == 2) {
                $scope.AMST_Id = "";
                $scope.studentname = true;
                $scope.order_flag = true;
                $scope.submitted = false;
                $scope.grid_flag = false;
                $scope.stu_name = true;
            }
           
        };

        $scope.betweendate = function () {
            $scope.ftdate = true;
            $scope.monthname = false;
            $scope.ftdaily = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.grid_flag = false;
            $scope.todatef = true;
        };

        $scope.mondate = function () {
            $scope.ftdate = false;
            $scope.monthname = true;
            $scope.ftdaily = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.grid_flag = false;
            $scope.amM_Id = "";
        };

        $scope.daily = function () {
            $scope.ftdate = false;
            $scope.monthname = false;
            $scope.ftdaily = true;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.grid_flag = false;
        };

        $scope.admname = true;

        $scope.studlist = function (iddata) {
            $scope.obj.fromdate = null;
            $scope.obj.todate = null;
            $scope.obj.fromdaily = null;
            for (var k = 0; k < $scope.yearDropdown.length; k++) {
                if ($scope.yearDropdown[k].asmaY_Id == iddata) {
                    var data = $scope.yearDropdown[k].asmaY_Year;
                }
            }
            if (data != null) {
                console.log(data);
                var name, name1;
                for (var i = 0; i < data.length; i++) {
                    if (i < 4) {
                        if (i == 0) {
                            name = data[i];
                        } else {
                            name += data[i];
                        }
                    }
                    if (i > 4) {
                        if (i == 5) {
                            name1 = data[5];
                        } else {
                            name1 += data[i];
                        }
                    }
                }
                $scope.fromDate = name;
                $scope.toDate = name1;
                $scope.frommon = "";
                $scope.tomon = "";
                $scope.fromDay = "";
                $scope.toDay = "";
                // For Academic From Date
                $scope.minDatef = new Date(
                    $scope.fromDate,
                    $scope.frommon,
                    $scope.fromDay + 1);

                $scope.maxDatef = new Date(
                    $scope.toDate,
                    $scope.tomon,
                    $scope.toDay + 365);
            }
        };


        $scope.studlist1 = function (iddata) {
            $scope.obj.fromdate = null;
            $scope.obj.todate = null;
            $scope.obj.fromdaily = null;
            for (var k = 0; k < $scope.yearDropdown.length; k++) {
                if ($scope.yearDropdown[k].asmaY_Id == iddata) {
                    var data = $scope.yearDropdown[k].asmaY_Year;
                }
            }
            if (data != null) {
                console.log(data);
                var name, name1;
                for (var i = 0; i < data.length; i++) {
                    if (i < 4) {
                        if (i == 0) {
                            name = data[i];
                        } else {
                            name += data[i];
                        }
                    }
                    if (i > 4) {
                        if (i == 5) {
                            name1 = data[5];
                        } else {
                            name1 += data[i];
                        }
                    }
                }
                $scope.fromDate = name;
                $scope.toDate = name1;
                $scope.frommon = "";
                $scope.tomon = "";
                $scope.fromDay = "";
                $scope.toDay = "";
                // For Academic From Date
                $scope.minDatef = new Date(
                    $scope.fromDate,
                    $scope.frommon,
                    $scope.fromDay + 1);

                $scope.maxDatef = new Date(
                    $scope.toDate,
                    $scope.tomon,
                    $scope.toDay + 365);
            }
            if ($scope.type1 == "2") {

                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMC_Id": $scope.asmC_Id,
                    "radiotype": $scope.obj.monthly,
                    "type1": "name",
                };
            }
            else if ($scope.type1 == "1") {
                data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMC_Id": $scope.asmC_Id,
                    "radiotype": $scope.obj.monthly,
                    "type1": "admno",
                };
            }
            apiService.create("StudentAttendanceReport/getdatatype", data).then(function (promise) {
                if (promise.studentAttendanceList != null && promise.studentAttendanceList.length > 0) {
                    $scope.studentDropdown = promise.studentAttendanceList;
                    $scope.stu_name = false;
                    $scope.report_btn = false;
                    $scope.admname = false;
                }
                else {
                    swal("Student is Not Available For This Selection,Please Select Different");
                    $scope.stu_name = true;
                    $scope.report_btn = true;
                }
            });
        };



        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //Showing Report
        $scope.submitted = false;

        $scope.savetmpldata = function (obj) {
            if ($scope.obj.monthly == 1) {
                obj.todate = new Date().toDateString();
                obj.fromdate = new Date().toDateString();
            } else if ($scope.obj.monthly == 2) {
                $scope.amM_Id = 0;
            } else if ($scope.obj.monthly == 3) {
                $scope.amM_Id = 0;
                obj.todate = new Date().toDateString();
                obj.fromdate = new Date().toDateString();
            }

            var data = "";
            var checkdate = 0;
            $scope.printdatatable = [];
            $scope.searchValue = "";
            $scope.searchValue1 = "";

            if ($scope.myForm.$valid) {
                var AMC_Id = 0
                if ($scope.objj.amC_Id != 0 && $scope.objj.amC_Id>0) {
                    AMC_Id = $scope.objj.amC_Id
                }
                if ($scope.obj.monthly == 1) {
                    if ($scope.type == 1) {
                        $scope.AMST_Id = 0;
                    }
                    else {
                        $scope.AMST_Id = $scope.AMST_Id;
                    }
                    obj.todate = new Date().toDateString();
                    obj.fromdate = new Date().toDateString();
                    if ($scope.checkoruncheck == true) {
                        checkdate = 1;
                    }
                    else {
                        checkdate = 0;
                    }
     
                    data = {
                        "ASMAY_Id": $scope.asmaY_Id,
                        "ASMCL_Id": $scope.asmcL_Id,
                        "ASMC_Id": $scope.asmC_Id,
                        "radiotype": $scope.obj.monthly,
                        "fromdate": new Date(obj.fromdate).toDateString(),
                        "todate": new Date(obj.todate).toDateString(),
                        "AMM_ID": $scope.amM_Id,
                        "AMST_Id": $scope.AMST_Id,
                        "type": $scope.type,
                        "datewise": checkdate,
                        "AMC_Id": AMC_Id
                    };
                }

                if ($scope.obj.monthly == 3) {
                    if ($scope.type == 1) {
                        $scope.AMST_Id = 0;
                    }
                    else {
                        $scope.AMST_Id = $scope.AMST_Id;
                    }
                    $scope.checkoruncheck = false;
                    checkdate = 0;
                    $scope.amM_Id = 0;
                    obj.todate = new Date();
                    data = {
                        "ASMAY_Id": $scope.asmaY_Id,
                        "ASMCL_Id": $scope.asmcL_Id,
                        "ASMC_Id": $scope.asmC_Id,
                        "radiotype": $scope.obj.monthly,
                        "fromdate": new Date(obj.fromdaily).toDateString(),
                        "todate": obj.todate,
                        "AMM_ID": $scope.amM_Id,
                        "AMST_Id": $scope.AMST_Id,
                        "type": $scope.type,
                        "datewise": checkdate,
                        "AMC_Id": AMC_Id
                    };
                }
                if ($scope.obj.monthly == 2) {
                    $scope.amM_Id = 0;
                    if ($scope.type == 1) {
                        $scope.AMST_Id = 0;
                    }
                    else {
                        $scope.AMST_Id = $scope.AMST_Id;
                    }
                    $scope.checkoruncheck = false;
                    checkdate = 0;
                    data = {
                        "ASMAY_Id": $scope.asmaY_Id,
                        "ASMCL_Id": $scope.asmcL_Id,
                        "ASMC_Id": $scope.asmC_Id,
                        "radiotype": $scope.obj.monthly,
                        "fromdate": new Date(obj.fromdate).toDateString(),
                        "todate": new Date(obj.todate).toDateString(),
                        "AMST_Id": $scope.AMST_Id,
                        "type": $scope.type,
                        "datewise": checkdate,
                        "AMC_Id": AMC_Id
                    };

                }
                apiService.create("StudentAttendanceReport/getAttendetails", data).then(function (promise) {

                    if (promise.studentAttendanceList === null || promise.studentAttendanceList.length === 0) {
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
                        if ($scope.checkoruncheck == true) {
                            var id = parseInt($scope.amM_Id);
                            for (var k = 0; k < $scope.monthDropdown.length; k++) {
                                if (id == $scope.monthDropdown[k].amM_ID) {
                                    $scope.monthname = $scope.monthDropdown[k].amM_NAME;
                                }
                            }
                            $scope.countclass = promise.countclass;
                            $scope.columnsTest = promise.monthList;
                            $scope.students1 = promise.studentAttendanceList;
                            angular.forEach($scope.students1, function (dd) {
                                var total_days = 0;
                                angular.forEach($scope.columnsTest, function (dd1) {
                                    if (dd[dd1.day] === 'P')
                                        total_days += 1;
                                    else if (dd[dd1.day] === 'H') {
                                        total_days += 0.5;
                                    }
                                });
                                dd.total = total_days;
                                dd.total = total_days;
                                var percentage = 0;
                                percentage = (total_days / $scope.countclass) * 100;
                                dd.percentage = percentage;
                            });


                            if (promise.AMC_logo != null) {
                                $scope.imgname = promise.AMC_logo[0].amC_FilePath;
                            }
                            else {
                                $scope.imgname = logopath;
                            }

                            var femalcountd = 0;
                            var malecountd = 0;
                            angular.forEach($scope.students1, function (dd) {
                                if (dd.amst_sex === "F") {
                                    femalcountd += 1;
                                } else {
                                    malecountd += 1;
                                }
                            });

                            $scope.femalcount = femalcountd;
                            $scope.malecount = malecountd;

                            $scope.present = [];
                            $scope.absent = [];

                            angular.forEach($scope.columnsTest, function (dd1) {
                                var presentcount = 0;
                                var absentcount = 0;
                                var holidayne = "";
                                var date = dd1.day;
                                angular.forEach($scope.students1, function (dd) {
                                    if (dd[dd1.day] === 'P')
                                        presentcount += 1;
                                    else if (dd[dd1.day] === 'A') {
                                        absentcount += 1;
                                    } else if (dd[dd1.day] === "HO") {
                                        holidayne = "HO";
                                    } else if (dd[dd1.day] === "NE") {
                                        holidayne = "NE";
                                    } else if (dd[dd1.day] === "S") {
                                        holidayne = "S";
                                    } else {
                                        holidayne = "";
                                    }
                                });

                                if (holidayne === "HO" || holidayne === "NE" || holidayne === "S") {

                                    $scope.absent2 = "";
                                    $scope.present2 = "";
                                } else {
                                    $scope.absent2 = absentcount;
                                    $scope.present2 = presentcount;
                                }

                                $scope.absent.push({ day: dd1.day, absent1: $scope.absent2, hol: holidayne });
                                $scope.present.push({ day: dd1.day, present1: $scope.present2, hol: holidayne });
                            });
                            $scope.totalpercentageatt = [];
                            if (promise.totalpercentageatt != null && promise.totalpercentageatt.length > 0) {
                                $scope.totalpercentageatt = promise.totalpercentageatt;
                                $scope.boysper = $scope.totalpercentageatt[0].boysper;
                                $scope.girlsper = $scope.totalpercentageatt[0].girlsper;
                            }

                          //  $scope.totalpercentageatt = promise.totalpercentageatt;
                          //  $scope.boysper = $scope.totalpercentageatt[0].boysper;
                           // $scope.girlsper = $scope.totalpercentageatt[0].girlsper;

                            $scope.presentCountgrid = $scope.students1.length;
                            $scope.grid_flag = true;
                            $scope.excel_flag = false;
                            $scope.print_flag = false;
                            $scope.datewised = true;
                            $scope.datewisen = false;

                        } else {
                            angular.forEach(promise.studentAttendanceList, function (value1, i) {
                                promise.studentAttendanceList[i].Remarks = "";
                            });
                            for (var i = 0; i < promise.studentAttendanceList.length; i++) {
                                if (promise.studentAttendanceList[i].Percentage <= 25) {
                                    promise.studentAttendanceList[i].Remarks = "Poor Attendance";
                                }
                                else if (promise.studentAttendanceList[i].Percentage >= 25 && promise.studentAttendanceList[i].Percentage <= 50) {
                                    promise.studentAttendanceList[i].Remarks = "Average Attendance";
                                }
                                else if (promise.studentAttendanceList[i].Percentage >= 50 && promise.studentAttendanceList[i].Percentage <= 75) {
                                    promise.studentAttendanceList[i].Remarks = "Good Attendance";
                                }
                                else {
                                    promise.studentAttendanceList[i].Remarks = "Very Good Attendance";
                                }
                            }
                            $scope.datewised = false;
                            $scope.datewisen = true;
                            $scope.students = promise.studentAttendanceList;
                            $scope.presentCountgrid = $scope.students.length;
                            $scope.grid_flag = true;
                            $scope.excel_flag = false;
                            $scope.print_flag = false;
                        }

                        $scope.institutiondetails = promise.institutiondetails;

                        $scope.institutionname = $scope.institutiondetails[0].mI_Name;
                        $scope.institutionaddress = $scope.institutiondetails[0].mI_Address1;

                        angular.forEach($scope.yearDropdown, function (y) {
                            if (y.asmaY_Id == $scope.asmaY_Id) {
                                $scope.yearname = y.asmaY_Year;
                            }
                        });

                        angular.forEach($scope.classDropdown, function (y) {
                            if (y.asmcL_Id == $scope.asmcL_Id) {
                                $scope.classname = y.asmcL_ClassName;
                            }
                        });

                        angular.forEach($scope.sectionDropdown, function (y) {
                            if (y.asmS_Id == $scope.asmC_Id) {
                                $scope.sectionname = y.asmC_SectionName;
                            }
                        });
                    }
                });
            }
            else {

                $scope.submitted = true;
            }
        };

        $scope.Clearid = function () {
            $state.reload();
            $scope.asmaY_Id = "";
            $scope.asmcL_Id = "";
            $scope.asmC_Id = "";
            $scope.fromdate = "";
            $scope.todate = "";
            // $scope.amsT_Id = "";
            $scope.amM_Id = "";
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.setTodate = function (data1) {
            $scope.obj.todate = null;
            $scope.todatef = false;
            $scope.todate1 = data1.fromdate;
            $scope.minDatet = new Date(
                $scope.todate1.getFullYear(),
                $scope.todate1.getMonth(),
                $scope.todate1.getDate() + 1);

            //   $scope.maxDatef = new Date(
            // $scope.todate1.getFullYear(),
            // $scope.todate1.getMonth(),
            //$scope.todate1.getDate() + 1);
        };


        $scope.setTodate1 = function (datato) {

            $scope.todate2 = datato.todate;
            //  $scope.minDatet = new Date(
            // $scope.todate2.getFullYear(),
            // $scope.todate2.getMonth(),
            //$scope.todate2.getDate() + 1);
        };

    }
})();