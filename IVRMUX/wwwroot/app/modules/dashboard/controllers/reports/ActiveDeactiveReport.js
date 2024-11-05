(function () {
    'use strict';
    angular.module('app').controller('ActiveDeactiveReportController', ActiveDeactiveReportController)

    ActiveDeactiveReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter']
    function ActiveDeactiveReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter) {

        $scope.gridflag = false;
        $scope.print_flag = true;
        $scope.excel_flag = true;
        $scope.currentPage = 1;
        $scope.printdatatable = [];
        $scope.studentAttendanceList = {};
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        $scope.ddate = {};
        $scope.ddate = new Date();

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
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        $scope.StuAttRptDropdownList = function () {
            $scope.classname = false;
            $scope.sectionname = false;

            apiService.get("AttendanceReport/getdetails/").then(function (promise) {

                $scope.all = true;
                $scope.yearDropdown = promise.academicList;
                $scope.allAcademicYear1 = promise.academicListdefault;

                for (var i = 0; i < $scope.yearDropdown.length; i++) {
                    name = $scope.yearDropdown[i].asmaY_Id;
                    for (var j = 0; j < $scope.allAcademicYear1.length; j++) {
                        if (name == $scope.allAcademicYear1[j].asmaY_Id) {
                            $scope.yearDropdown[i].Selected = true;
                            $scope.asmaY_Id = $scope.allAcademicYear1[j].asmaY_Id;
                            //$scope.yearId = $scope.allAcademicYear1[j].asmaY_Id;
                        }
                    }
                }
                $scope.imgname = promise.photopathname;
                $scope.classDropdown = promise.classlist;
                $scope.sectionDropdown = promise.sectionList;
                $scope.monthDropdown = promise.monthList;
            });
        }

        $scope.getDataByType = function (type) {

            if (type == 1) {
                $scope.asmcL_Id = "";
                $scope.asmC_Id = "";
                $scope.classname = false;
                $scope.sectionname = false;
                $scope.gridflag = false;
                //$scope.asmaY_Id = "";
                //$scope.submitted = false;
                //$scope.myForm.$setPristine();
                //$scope.myForm.$setUntouched();
            }
            if (type == 2) {
                $scope.classname = true;
                $scope.sectionname = true;
                $scope.gridflag = false;
                //$scope.asmaY_Id = "";
                $scope.asmcL_Id = "";
                $scope.asmC_Id = "";
                //$scope.submitted = false;
                //$scope.myForm.$setPristine();
                //$scope.myForm.$setUntouched();
            }
        }

        $scope.getclass = function () {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
            }
            apiService.create("AttendanceReport/getclass", data).then(function (promise) {
                $scope.classDropdown = promise.classlist;
            })
        }

        $scope.getsection = function () {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
            }
            apiService.create("AttendanceReport/getsection", data).then(function (promise) {
                $scope.sectionDropdown = promise.sectionList;
            })
        }

        $scope.getreport = function () {

            var classid = "";
            var sectionid = "";
            if ($scope.type23 == 1) {
                classid = 0;
                sectionid = 0;
            } else {
                classid = $scope.asmcL_Id;
                sectionid = $scope.asmC_Id;
            }

            if ($scope.myForm.$valid) {

                var data = {
                    "type": $scope.type23,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": classid,
                    "ASMC_Id": sectionid,
                }
                apiService.create("StudentAttendanceReport/getreport", data)
                    .then(function (promise) {
                        if (promise.newarray != null) {
                            if (promise.newarray.length == 0) {
                                swal("No record Found !");
                                $scope.gridflag = false;
                                $scope.print_flag = true;
                                $scope.excel_flag = true;
                            } else {
                                $scope.students = promise.newarray;
                                $scope.presentCountgrid = $scope.students.length;

                                angular.forEach($scope.yearDropdown, function (dd) {
                                    if (dd.asmaY_Id == $scope.asmaY_Id) {

                                        $scope.year = dd.asmaY_Year;
                                    }
                                })

                                $scope.gridflag = true;
                                $scope.print_flag = false;
                                $scope.excel_flag = false;
                            }
                        }
                        else {
                            swal("No record Found !");
                            $scope.gridflag = false;
                            $scope.print_flag = true;
                            $scope.excel_flag = true;
                        }
                    })
            } else {
                $scope.submitted = true;
            }
        }

        $scope.toggleAll = function () {

            var toggleStatus = $scope.all2;
            angular.forEach($scope.students, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all2 == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
        }

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.studentname)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.admno)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.regno)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.classname)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.sectionname)).indexOf($scope.searchValue) >= 0 ||
                ($filter('date')(obj.deactivatdate, 'dd/MM/yyyy').indexOf($scope.searchValue) >= 0) ||
                ($filter('date')(obj.activatedate, 'dd/MM/yyyy').indexOf($scope.searchValue) >= 0) ||
                (angular.lowercase(obj.deactivatereason)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.activatereason)).indexOf($scope.searchValue) >= 0
        }

        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.all2 = $scope.students.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }

        }

        $scope.printData = function () {
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

        $scope.exportToExcel = function (tableId) {

            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }
        }

        $scope.submitted = false;

        $scope.angularData =
            {
                'nameList': []
            };

        $scope.vals = [];

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        $scope.cancel = function () {
            $state.reload();
        };

    }
})();
