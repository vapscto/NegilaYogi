(function () {
    'use strict';
    angular.module('app').controller('StudentYearlyAttendanceController', StudentYearlyAttendanceController)

    StudentYearlyAttendanceController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function StudentYearlyAttendanceController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {

        $scope.searchValue = '';
        $scope.objj = {};
        $scope.AttRptDropdownList = function () {
            $scope.currentPage = 1;
            // $scope.itemsPerPage = 10;
            $scope.printstudents = [];
            apiService.get("StudentYearlyAttendance/getdetails/").then(function (promise) {
                $scope.yearDropdown = promise.academicList;
                $scope.classDropdown = promise.classlist;
                $scope.sectionDropdown = promise.sectionList;
                $scope.categoryDropdown = promise.category_list;

                $scope.categoryflag = promise.categoryflag;
            });
        }
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !=null && ivrmcofigsettings.length > 0) {
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


        $scope.clssec = true;
        $scope.onclickloaddata = function () {
            if ($scope.TC_allorind == "all") {
                $scope.asmaY_Id = "";
                $scope.clssec = true;
                $scope.catreport = true;
                $scope.grid_flag = false;
                $scope.asmC_Id = '0';
                $scope.asmcL_Id = '0';
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
            }
            else if ($scope.TC_allorind === "indi") {
                $scope.asmaY_Id = "";
                $scope.asmcL_Id = "";
                $scope.asmC_Id = "";
                $scope.clssec = false;
                $scope.catreport = true;
                $scope.grid_flag = false;
                $scope.asmcL_Id = $scope.asmcL_Id;
                $scope.asmC_Id = $scope.asmC_Id;
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
            }
        };


        $scope.getclass = function () {
            var acedamicId = $scope.asmaY_Id;

            var amcid = 0;
            if ($scope.objj.amC_Id != null && $scope.objj.amC_Id != 0 && $scope.objj.amC_Id != undefined) {
                amcid = $scope.objj.amC_Id;
            }

            var data = {
                "ASMAY_Id": acedamicId,
                "AMC_Id": amcid
            };
            apiService.create("StudentYearlyAttendance/getclass", data).then(function (promise) {
                $scope.classDropdown = promise.fillclass;
            });
        };

        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }

        $scope.catreport = true;
        $scope.submitted = false;

        $scope.columnTotal = [];
        $scope.angularData =
            {
                'nameList': []
            };

        $scope.vals = [];
        $scope.rptyearwisedata1 = function () {
            $scope.printstudents = [];
            $scope.searchValue = '';

            var AMC_Id = 0
            if ($scope.objj.amC_Id != 'All') {
                AMC_Id = $scope.objj.amC_Id
            }
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMC_Id": $scope.asmC_Id,
                    "allorindiflag": "indi",
                    "AMC_Id": AMC_Id
                };
                apiService.create("StudentYearlyAttendance/getAttendetails", data).then(function (promise) {
                    $scope.columnsTest = promise.monthList;
                    $scope.students = promise.studentAttendanceList;
                    $scope.presentCountgrid = $scope.students.length;
                    for (var i = 0; i < promise.studentAttendanceList.length; i++) {
                        var name_list = promise.studentAttendanceList[i].name;
                        $scope.vals.push(name_list);
                    }
                    if (promise.AMC_logo != null) {
                        $scope.imgname = promise.AMC_logo[0].amC_FilePath;
                    }
                    else {
                        $scope.imgname = logopath;
                    }
                    angular.forEach($scope.vals, function (v, k) {
                        $scope.angularData.nameList.push({
                            'fullname': v
                        });
                    });
                    var j = 0;
                    angular.forEach($scope.students, function (obj) {
                        //Using bracket notation
                        obj["fullname"] = $scope.angularData.nameList[j].fullname;
                        j++;
                    });
                    angular.forEach($scope.students, function (objectt) {
                        if (objectt.fullname.length > 0) {
                            var string = objectt.fullname
                            objectt.namme = string.replace(/  +/g, ' ');
                        }
                    });
                    angular.forEach($scope.columnsTest, function (value1, i) {
                        var clmttl = $scope.columnsTest[i].TOTAL_classheld;
                        $scope.columnTotal.push(clmttl);
                    });
                    angular.forEach($scope.students, function (value1, i) {
                        $scope.students[i].totalval = 0;
                    });

                    $scope.totalList = [];

                    for (var i = 0; i < $scope.students.length; i++) {
                        $scope.total = 0;
                        for (var j = 0; j < $scope.columnsTest.length; j++) {
                            $scope.mnth = $scope.columnsTest[j].MONTH_NAME;
                            if ($scope.mnth === "January") {
                                $scope.val = $scope.students[i].January;
                                if (!isNaN($scope.val) && angular.isNumber(+$scope.val) && $scope.val != null) {
                                    $scope.total = parseFloat($scope.total) + parseFloat($scope.val);
                                }
                                else {
                                    $scope.total = parseFloat($scope.total) + parseFloat(0);
                                }
                            }
                        }

                        $scope.totalList.push($scope.total);
                    }

                    if (promise.studentAttendanceList === null || promise.studentAttendanceList.length === 0) {
                        swal('No Records Found!');
                        $scope.catreport = true;
                    }
                    else {
                        $scope.catreport = false;
                        angular.forEach($scope.totalList, function (value1, i) {
                            $scope.students[i].totalval = $scope.totalList[i];
                        });


                        var perc = 0;
                        for (var k = 0; k < $scope.columnsTest.length; k++) {
                            perc = $filter('number')(parseFloat(perc + $scope.columnsTest[k].TOTAL_classheld));
                            $scope.tot = parseFloat(perc);
                        }
                        angular.forEach($scope.students, function (value1, i) {
                            var perc_number = $filter('number')(((parseFloat($scope.students[i].totalval) / parseFloat(perc)) * 100), 2);
                            $scope.students[i].percentage = parseFloat(perc_number);
                        });
                        console.log($scope.students);
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.filterValue1, function (itm) {
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

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.Clearid = function () {
            $scope.asmaY_Id = "";
            $scope.asmcL_Id = "";
            $scope.asmC_Id = "";
            $scope.submitted = false;
            $scope.catreport = true;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

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

        //$scope.exportToExcel = function (export_id) {
        //    if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
        //        var exportHref = Excel.tableToExcel(export_id, 'WireWorkbenchDataExport');
        //        $timeout(function () {
        //            location.href = exportHref;
        //        }, 100);
        //    }
        //    else {
        //        swal("Please Select Records to be Export Excel");
        //    }
        //};

        $scope.exportToExcel = function (export_id) {            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {                var excelnamemain = "STUDENT YEARLY ATTENDANCE REPORT";                //var printSectionId = '#printgrn';                //$scope.start_Date = new Date();                //$scope.end_Date = new Date();                //$scope.from_dateex = $filter('date')($scope.start_Date, 'yyyy-MM-dd');                //$scope.to_dateex = $filter('date')($scope.end_Date, 'yyyy-MM-dd');                excelnamemain = excelnamemain + '-' + '.xls';                var exportHref = Excel.tableToExcel(export_id, 'sheet name');                $timeout(function () {                    var a = document.createElement('a');                    a.href = exportHref;                    a.download = excelnamemain;                    document.body.appendChild(a);                    a.click();                    a.remove();                }, 100);
            }            else {                swal("Please Select Records to be Export Excel");
            }                   };




        $scope.rptyearwisedata = function () {
            $scope.printstudents = [];
            $scope.searchValue = '';
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMC_Id": $scope.asmC_Id,
                    "allorindiflag": "indi"
                };

                apiService.create("StudentYearlyAttendance/getAttendetails", data).then(function (promise) {

                    if (promise.studentAttendanceList === null || promise.studentAttendanceList.length === 0) {
                        swal('No Records Found!');
                        $scope.catreport = true;
                    }
                    else {
                        $scope.columnsTest = promise.monthList;
                        $scope.reports = promise.studentAttendanceList;
                        $scope.students = [];
                        angular.forEach($scope.reports, function (dd) {
                            if ($scope.students.length === 0) {
                                $scope.students.push({
                                    AMST_Id: dd.AMST_Id, name: dd.name, AMST_AdmNo: dd.AMST_AdmNo,
                                    AMST_RegistrationNo: dd.AMST_RegistrationNo, AMAY_RollNo: dd.AMAY_RollNo
                                });
                            } else if ($scope.students.length > 0){
                                var countstu = 0;
                                angular.forEach($scope.students, function (d) {
                                    if (d.AMST_Id === dd.AMST_Id) {
                                        countstu += 1;
                                    }
                                });

                                if (countstu === 0) {
                                    $scope.students.push({
                                        AMST_Id: dd.AMST_Id, name: dd.name, AMST_AdmNo: dd.AMST_AdmNo,
                                        AMST_RegistrationNo: dd.AMST_RegistrationNo, AMAY_RollNo: dd.AMAY_RollNo
                                    });
                                }
                            }
                        });

                        $scope.monthListdata = [];

                        angular.forEach($scope.students, function (dd) {
                            $scope.monthListdata = [];
                            angular.forEach($scope.reports, function (d) {
                                if (d.AMST_Id === dd.AMST_Id) {
                                    $scope.monthListdata.push(d);
                                }
                            });
                            dd.monthdata = $scope.monthListdata;
                        });


                        var totalclassheld = 0;
                        angular.forEach($scope.columnsTest, function (x) {
                            totalclassheld = totalclassheld + x.total;
                        });
                        $scope.tot = totalclassheld;
                        angular.forEach($scope.students, function (y) {
                            var total_days = 0;
                            angular.forEach(y.monthdata, function (x) {                               
                                total_days += x.TOTAL_PRESENT;                                
                            });
                            
                            y.Total = total_days;
                            y.percentage = (total_days / totalclassheld) * 100;
                        });


                        $scope.presentCountgrid = $scope.students.length;
                        $scope.catreport = false;
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.TotalPresentDays = function (data) {
            var total = 0;
            return total;
        };
    }
})();
