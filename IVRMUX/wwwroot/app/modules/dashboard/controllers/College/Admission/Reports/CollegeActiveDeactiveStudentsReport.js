(function () {
    'use strict';
    angular
        .module('app')
        .controller('CollegeActiveDeactiveStudentsReportController', CollegeActiveDeactiveStudentsReportController)

    CollegeActiveDeactiveStudentsReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel','$timeout']
    function CollegeActiveDeactiveStudentsReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {

        $scope.pagesrecord = {};
        $scope.students = [];
        $scope.adtable = true;
        $scope.searchValue = '';

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));

        if (ivrmcofigsettings != null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            } else {
                paginationformasters = 10;
                copty = "";
            }
        } else {
            paginationformasters = 10;
            copty = "";
        }
        $scope.students11 = [];

        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var logopath = "";

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        if (admfigsettings != null) {
            if (admfigsettings.length > 0) {
                logopath = admfigsettings[0].asC_Logo_Path;
            }
        } else {
            logopath = "";
        }

        $scope.imgname = logopath;


        $scope.getpagesname = function () {
            $scope.currentPage = 1;
            var pageid = 1;
            apiService.getURI("CollegeActiveDeactiveStudents/getdata", pageid).
                then(function (promise) {
                    $scope.yearlist = promise.yearlist;
                });
        };



        $scope.radiobtnchange = function () {
            $scope.vals = [];
            $scope.ASMAY_Id = '';
            $scope.AMCO_Id = '';
            $scope.AMB_Id = '';
            $scope.AMSE_Id = '';
            $scope.ACMS_Id = "";
            $scope.submitted = false;
            $scope.adtable = true;
            $scope.students = [];
            $scope.all = '';
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };

        $scope.onacademicyearchange = function () {
            $scope.AMCO_Id = '';
            $scope.AMB_Id = '';
            $scope.AMSE_Id = '';
            $scope.ACMS_Id = "";
            $scope.adtable = true;
            $scope.students = [];
            $scope.students11 = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };

            apiService.create("CollegeActiveDeactiveStudents/onacademicyearchange", data).

                then(function (promise) {
                    $scope.courselist = promise.courselist;
                });
        };

        $scope.oncoursechange = function () {
            $scope.AMB_Id = '';
            $scope.AMSE_Id = '';
            $scope.ACMS_Id = "";
            $scope.adtable = true;
            $scope.students = [];
            $scope.students11 = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id
            };

            apiService.create("CollegeActiveDeactiveStudents/oncoursechange", data).then(function (promise) {
                $scope.branchlist = promise.branchlist;
            });
        };

        $scope.onbranchchange = function () {
            $scope.AMSE_Id = '';
            $scope.ACMS_Id = "";
            $scope.adtable = true;
            $scope.students = [];
            $scope.students11 = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id
            };

            apiService.create("CollegeActiveDeactiveStudents/onbranchchange", data).then(function (promise) {
                $scope.semesterlist = promise.semesterlist;
            });
        };

        $scope.onchangesemester = function () {
            $scope.ACMS_Id = "";
            $scope.adtable = true;
            $scope.students = [];
            $scope.students11 = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id
            };

            apiService.create("CollegeActiveDeactiveStudents/onchangesemester", data).then(function (promise) {
                $scope.sectionlist = promise.sectionlist;
            });
        };

        $scope.onchangesection = function () {
            $scope.adtable = true;
            $scope.students = [];
            $scope.students11 = [];
        };


        $scope.getreport = function () {
            $scope.adtable = true;
            $scope.students = [];
            $scope.students11 = [];

            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                };

                apiService.create("CollegeActiveDeactiveStudents/getreport", data).then(function (promise) {
                    $scope.students11 = promise.getreport;
                    if ($scope.students11.length > 0) {
                        $scope.students = promise.getreport;

                        angular.forEach($scope.yearlist, function (dd) {
                            if (dd.asmaY_Id == $scope.ASMAY_Id) {

                                $scope.year = dd.asmaY_Year;
                            }
                        })
                        angular.forEach($scope.courselist, function (dd) {
                            if (dd.amcO_Id == $scope.AMCO_Id) {

                                $scope.coursename = dd.amcO_CourseName;
                            }
                        })
                        angular.forEach($scope.branchlist, function (dd) {
                            if (dd.amB_Id == $scope.AMB_Id) {

                                $scope.branchname = dd.amB_BranchName;
                            }
                        })
                        angular.forEach($scope.semesterlist, function (dd) {
                            if (dd.amsE_Id == $scope.AMSE_Id) {

                                $scope.semestername = dd.amsE_SEMName;
                            }
                        })
                        angular.forEach($scope.sectionlist, function (dd) {
                            if (dd.acmS_Id == $scope.ACMS_Id) {
                                $scope.sectionname = dd.acmS_SectionName;
                            }
                        })


                        $scope.adtable = false;
                    } else {
                        $scope.adtable = true;
                        swal("No Record Found");
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.submitted = false;

        $scope.Clearid_up = function () {
            $state.reload();
        };


        var studclear = [];

        $scope.Clearid = function () {
            $state.reload();
            $scope.AMST_SOL = 'S';
            $scope.ASMAY_Id = '';
            $scope.ASMCL_Id = '';
            $scope.ASMC_Id = '';
            $scope.submitted = false;
            $scope.adtable = true;
            $scope.all = '';
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };

        $scope.isOptionsRequired = function () {
            return !$scope.students.some(function (options) {
                return options.checkedvalue;
            });
        };

        $scope.students1 = [];
        $scope.submitted = false;

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.propertyName = 'regno';
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.students, function (itm) {
                itm.checkedvalue = toggleStatus;

            });
        };

        $scope.optionToggled = function () {
            $scope.all = $scope.students.every(function (itm) { return itm.checkedvalue; })
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

        }

        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);

        }
    }
})();

