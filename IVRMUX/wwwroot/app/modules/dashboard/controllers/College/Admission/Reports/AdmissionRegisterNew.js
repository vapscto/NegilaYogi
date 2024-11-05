(function () {
    'use strict';
    angular
        .module('app')
        .controller('CollegeAdmissionRegisterNewController', CollegeAdmissionRegisterNewController)

    CollegeAdmissionRegisterNewController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function CollegeAdmissionRegisterNewController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {

        $scope.report = false;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length != 0 && ivrmcofigsettings.length != undefined) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length != 0 && admfigsettings.length != undefined) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        } else {
            var logopath = "";
        }
        $scope.imgname = logopath;

        $scope.exportToExcel = function (export_id) {

            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
                var exportHref = Excel.tableToExcel(export_id, 'studentregisterrepor');
                $timeout(function () {
                    location.href = exportHref;
                }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }
        };


        $scope.BindData = function () {
            apiService.getDATA("CollegeAdmissionRegister/getdetails").
                then(function (promise) {

                    $scope.acdlist = promise.acdlist;
                    $scope.seclist = promise.seclist;
                    $scope.quotalist = promise.quotalist;
                    $scope.checklist = promise.check_list;

                });
        };

        $scope.onselectAcdYear = function (ASMAY_Id) {
            var data = {
                "ASMAY_Id": ASMAY_Id
            };
            apiService.create("CollegeAdmissionRegister/onselectAcdYear", data).
                then(function (promise) {
                    $scope.courselist = promise.courselist;

                });
        };

        $scope.onselectCourse = function (ASMAY_Id, AMCO_Id) {
            var data = {
                "AMCO_Id": AMCO_Id,
                "ASMAY_Id": ASMAY_Id
            };
            apiService.create("CollegeAdmissionRegister/onselectCourse", data).
                then(function (promise) {
                    $scope.branchlist = promise.branchlist;
                });
        };

        $scope.onselectBranch = function (ASMAY_Id, AMCO_Id, AMB_Id) {
            var data = {

                "ASMAY_Id": ASMAY_Id,
                "AMCO_Id": AMCO_Id,
                "AMB_Id": AMB_Id
            };
            apiService.create("CollegeAdmissionRegister/onselectBranch", data).
                then(function (promise) {
                    $scope.semlist = promise.semlist;
                });
        };


        $scope.all_check = function () {

            var toggleStatus = $scope.detail_checked;
            angular.forEach($scope.checklist, function (itm) {
                itm.ivrm_id = toggleStatus;
            });
        };

        $scope.albumNameArraycolumn = [];
        $scope.columnsTest = [];

        $scope.addColumn2 = function (role1) {
            $scope.detail_checked = $scope.checklist.every(function (itm) { return itm.selected; });

            if (role1.selected == true) {

                $scope.albumNameArraycolumn.push(role1);

                var newCol = { id: role1.ivrM_CLGREG_NAME, checked: true, value: role1.ivrM_CLGREG_PAR }

                $scope.columnsTest.push(newCol);
            }
            else {
                var som = $scope.albumNameArraycolumn.indexOf(role1);
                $scope.columnsTest.splice($scope.albumNameArraycolumn.indexOf(role1), 1);
                $scope.albumNameArraycolumn.splice($scope.albumNameArraycolumn.indexOf(role1), 1);

            }
        };


        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.searchValue = '';

        $scope.students = [];
        $scope.catreport = true;
        $scope.submitted = false;

        $scope.onreport = function () {
            $scope.all = false;

            if ($scope.myForm.$valid) {
                $scope.albumNameArraycolumn = [];
                $scope.columntest1 = [];
                $scope.columntest2 = [];
                $scope.columnsTest = [];

                angular.forEach($scope.checklist, function (role) {
                    if (!!role.ivrm_id) $scope.albumNameArraycolumn.push(role);
                });

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                    "TempararyArrayListcoloumn": $scope.albumNameArraycolumn,
                    "gender": $scope.mfa,
                    "ACQ_Id": $scope.ACQ_Id
                };



                apiService.create("CollegeAdmissionRegister/onreportnew", data).
                    then(function (promise) {

                        $scope.columnsTest = $scope.albumNameArraycolumn;

                        $scope.students = promise.studentlist;
                        $scope.presentCountgrid = $scope.students.length;
                        if ($scope.students.length > 0 && $scope.students !== null && $scope.students !== undefined) {


                            angular.forEach($scope.students, function (dd) {

                                if (dd.IMC_CasteName === " & ") {
                                    dd.IMC_CasteName = "";
                                }
                                if (dd.AMCST_mobileno === "  &  ") {
                                    dd.AMCST_mobileno = "";
                                }
                                if (dd.AMCST_MotherMobleNo === "  &  ") {
                                    dd.AMCST_MotherMobleNo = "";
                                }
                                if (dd.AMCST_FatherMobleNo === "  &  ") {
                                    dd.AMCST_FatherMobleNo = "";
                                }

                            });


                            $scope.catreport = false;
                            $scope.exp_excel_flag = false;
                            $scope.print_flag = false;
                            $scope.report = true;

                            angular.forEach($scope.acdlist, function (yy) {
                                if (yy.asmaY_Id == $scope.ASMAY_Id) {
                                    $scope.year = yy.asmaY_Year;
                                }
                            });
                            angular.forEach($scope.courselist, function (yy) {
                                if (yy.amcO_Id == $scope.AMCO_Id) {
                                    $scope.coursename = yy.amcO_CourseName;
                                }
                            });
                            angular.forEach($scope.branchlist, function (yy) {
                                if (yy.amB_Id == $scope.AMB_Id) {
                                    $scope.branchname = yy.amB_BranchName;
                                }
                            });
                            angular.forEach($scope.semlist, function (yy) {
                                if (yy.amsE_Id == $scope.AMSE_Id) {
                                    $scope.semestername = yy.amsE_SEMName;
                                }
                            });

                        }
                        else {
                            swal("No Records Found!");
                            $scope.catreport = true;
                            $scope.report = false;
                            $state.reload();
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };


        $scope.cancel = function () {
            $state.reload();
        };


        $scope.printData = function () {

            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            } else {
                swal("Please Select Records to be Print");
            }


        };

        //$scope.exportToExcel = function (export_id) {
        //    if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
        //        var exportHref = Excel.tableToExcel(export_id, 'Student Register Report');
        //        $timeout(function () { location.href = exportHref; }, 1000);
        //    } else {
        //        swal("Please Select Records to be Exported");
        //    }
        //};

        $scope.exportToExcel = function (export_id) {            var excelnamemain = "Student Register Report";            var printSectionId = export_id;               excelnamemain = excelnamemain + '.xls';            var exportHref = Excel.tableToExcel(printSectionId, 'Student Register Report');            $timeout(function () {                var a = document.createElement('a');                a.href = exportHref;                a.download = excelnamemain;                document.body.appendChild(a);                a.click();                a.remove();            }, 100);            //var exportHref = Excel.tableToExcel("#printgrn", 'sheet name');            //$timeout(function () {            //    location.href = exportHref;            //}, 100);        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.isOptionsRequired = function () {
            return !$scope.checklist.some(function (options) {
                return options.ivrm_id;
            });
        };

        $scope.printstudents = [];


        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.students, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
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

    }

})();