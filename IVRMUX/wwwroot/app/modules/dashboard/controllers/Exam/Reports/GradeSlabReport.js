(function () {
    'use strict';
    angular.module('app').controller('GradeSlabReportController', GradeSlabReportController)

    GradeSlabReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter']
    function GradeSlabReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter) {

        $scope.gridflag = false;
        $scope.print_flag = true;
        $scope.excel_flag = true;
        $scope.currentPage = 1;
        //  $scope.itemsPerPage = 10;
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

        $scope.StuAttRptDropdownList = function () {
            apiService.get("GradeSlabReport/getdetails/").then(function (promise) {
                $scope.classDropdown = promise.grlist;
            });
        };        

        $scope.printData = function () {
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
        };

        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            var excelname = "Master Grade Slab.xls";
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.onchangegrade = function () {
            $scope.gridflag = false;
            $scope.print_flag = true;
            $scope.excel_flag = true;
        };

        $scope.submitted = false;

        $scope.savetmpldata = function () {
            $scope.printdatatable = [];
            $scope.searchValue = "";
            if (parseInt($scope.type) === 1) {
                $scope.asmcL_Id = 0;
                $scope.asmC_Id = 0;
            }

            var data = {
                "type": $scope.type,
                "EMGR_Id": $scope.emgR_Id
            };
            if ($scope.myForm.$valid) {
                if (parseInt($scope.type) === 1) {
                    apiService.create("GradeSlabReport/getAttendetails", data).then(function (promise) {                    
                        if (promise.studentAttendanceList.length > 0 && promise.studentAttendanceList !== null) {
                            $scope.students = promise.studentAttendanceList;
                            $scope.masterinstitution = promise.masterinstitution;
                            $scope.presentCountgrid = $scope.students.length;

                            $scope.show_grades = promise.grlist;
                            var details_list = [];
                            angular.forEach($scope.show_grades, function (grd) {
                                var alrdy_cnt = 0;
                                angular.forEach(details_list, function (final_t) {
                                    if (parseInt(final_t.emgR_Id) === parseInt(grd.emgR_Id)) {
                                        alrdy_cnt += 1;
                                    }
                                });

                                if (alrdy_cnt === 0) {
                                    var grd_details = [];
                                    angular.forEach($scope.students, function (t2) {
                                        if (parseInt(t2.emgR_Id) === parseInt(grd.emgR_Id)) {
                                            grd_details.push(t2);
                                        }
                                    });
                                    details_list.push({ emgR_Id: grd.emgR_Id, emgR_GradeName: grd.emgR_GradeName, emgR_MarksPerFlag: grd.emgR_MarksPerFlag, grd_details: grd_details });
                                }
                            });

                            $scope.institutename = $scope.masterinstitution[0].mI_Name;
                            $scope.instituteaddress = $scope.masterinstitution[0].mI_Address1;

                            console.log(details_list);
                            $scope.final_details_list = details_list;
                            $scope.gridflag = true;
                            $scope.print_flag = false;
                            $scope.excel_flag = false;

                        }
                        else {
                            swal("No record Found !");
                            $scope.gridflag = false;
                            $scope.print_flag = true;
                            $scope.excel_flag = true;
                        }
                    });
                }

                if (parseInt($scope.type) === 2) {
                    apiService.create("GradeSlabReport/getAttendetails", data).then(function (promise) {
                        if (promise.studentAttendanceList.length > 0 && promise.studentAttendanceList !== null) {
                            $scope.students = promise.studentAttendanceList;
                            $scope.presentCountgrid = $scope.students.length;
                            var details_list = [];
                            angular.forEach($scope.classDropdown, function (grd) {
                                if (parseInt(grd.emgR_Id) === parseInt($scope.emgR_Id)) {
                                    var grd_details = [];
                                    angular.forEach($scope.students, function (t2) {
                                        if (parseInt(t2.emgR_Id) === parseInt(grd.emgR_Id)) {
                                            grd_details.push(t2);
                                        }
                                    });
                                    details_list.push({ emgR_Id: grd.emgR_Id, emgR_GradeName: grd.emgR_GradeName, emgR_MarksPerFlag: grd.emgR_MarksPerFlag, grd_details: grd_details });
                                }
                            });

                            angular.forEach($scope.classDropdown, function (dd) {
                                if (dd.emgR_Id === parseInt($scope.emgR_Id)) {
                                    $scope.gradename = dd.emgR_GradeName;
                                }
                            });

                            $scope.reportdetails = "Grade Name : " + $scope.gradename;

                            console.log(details_list);
                            $scope.final_details_list = details_list;
                            $scope.gridflag = true;
                            $scope.print_flag = false;
                            $scope.excel_flag = false;
                        }
                        else {
                            swal("No record Found !");
                            $scope.gridflag = false;
                            $scope.print_flag = true;
                            $scope.excel_flag = true;
                        }
                    });
                }
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.getDataByType = function (type) {
            $scope.print_flag = true;
            $scope.excel_flag = true;
            if (parseInt(type) === 1) {
                $scope.classname = false;
                $scope.sectionname = false;
                $scope.gridflag = false;
                $scope.asmaY_Id = "";
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
            }
            if (parseInt(type) === 2) {
                $scope.classname = true;
                $scope.gridflag = false;
                $scope.asmcL_Id = "";
                $scope.asmC_Id = "";
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all2;
            $scope.printdatatable = [];
            angular.forEach($scope.final_details_list, function (itm) {
                itm.selected = toggleStatus;
                if (itm.selected === true) {
                    $scope.printdatatable.push(itm);
                }
            });
        };

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.namme)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.AMST_AdmNo)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.AMST_RegistrationNo)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.ASMCL_ClassName)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.ASMC_SectionName)).indexOf($scope.searchValue) >= 0 ||
                (JSON.stringify(obj.classes)).indexOf($scope.searchValue) >= 0
        };


        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all2 = $scope.final_details_list.every(function (itm) { return itm.selected; });
            $scope.printdatatable = [];
            angular.forEach($scope.final_details_list, function (itm) {
                if (itm.selected === true) {
                    $scope.printdatatable.push(itm);
                }
            });
        };
    }
})();
