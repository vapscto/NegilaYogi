(function () {
    'use strict';
    angular
        .module('app')
        .controller('SchoolStaffperiodtransactionreportcontroller', SchoolStaffperiodtransactionreportcontroller)

    SchoolStaffperiodtransactionreportcontroller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function SchoolStaffperiodtransactionreportcontroller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.showbtn = false;
        $scope.details = false;
        //TO  GEt The Values iN Grid
        $scope.currentPage = 1;
        // $scope.itemsPerPage = 1;
        var logopath = "";

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null) {
            if (admfigsettings.length !== 0 && admfigsettings.length !== null && admfigsettings.length !== undefined) {
                logopath = admfigsettings[0].asC_Logo_Path;
            } else {
                logopath = "";
            }
        } else {
            logopath = "";
        }


        $scope.imgname = logopath;

        $scope.BindData = function () {
            var id = 2;
            apiService.getURI("SchoolStaffperiodtransactionreport/Getdetailstransaction", id).
                then(function (promise) {
                    $scope.masteryear = promise.masteryear;
                });
        };

        $scope.obj = {};

        $scope.onselectAcdYear = function () {
            $scope.ASMCL_Id = "";
            $scope.ASMS_Id = "";
            $scope.HRME_Id = "";
            $scope.getreport1 = [];
            $scope.getreport = [];
            $scope.getreportemployee = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };

            apiService.create("SchoolStaffperiodtransactionreport/onselectAcdYear", data).then(function (promise) {
                if (promise !== null) {
                    $scope.mastercourse = promise.masterclass;
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }
            });
        };

        $scope.onselectclass = function () {
            $scope.ASMS_Id = "";
            $scope.HRME_Id = "";
            $scope.getreport1 = [];
            $scope.getreport = [];
            $scope.getreportemployee = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            };

            apiService.create("SchoolStaffperiodtransactionreport/onselectclass", data).then(function (promise) {
                if (promise !== null) {
                    $scope.sectionlist = promise.mastersection;
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }
            });
        };

        $scope.onchangesection = function () {
            $scope.HRME_Id = "";
            $scope.getreport1 = [];
            $scope.getreport = [];
            $scope.getreportemployee = [];

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            };

            apiService.create("SchoolStaffperiodtransactionreport/onchangesection", data).then(function (promise) {
                if (promise !== null) {
                    $scope.mastersubjects = promise.employeedetails;
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }
            });
        };

        $scope.getreportdetails = function () {

            $scope.submitted = true;

            if ($scope.myForm.$valid) {

                $scope.getreport1 = [];
                $scope.getreport = [];
                $scope.getreportemployee = [];

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "HRME_Id": $scope.HRME_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id

                };

                apiService.create("SchoolStaffperiodtransactionreport/getreport", data).then(function (promise) {
                    if (promise !== null) {

                        $scope.getreport1 = promise.getreport;

                        if ($scope.getreport1 !== null) {

                            if ($scope.getreport1.length > 0) {

                                $scope.getreport = promise.getreport;
                                $scope.getreportemployee = promise.getreportemployee;


                                $scope.tempdata = [];
                                $scope.tempdata1 = [];


                                angular.forEach($scope.getreportemployee, function (dd) {

                                    $scope.tempdata1 = [];

                                    angular.forEach($scope.getreport, function (d) {

                                        if (dd.isms_id === d.isms_id) {
                                            $scope.tempdata1.push({ topicname: d.topicname, allocateddate: d.allocateddate, staffname: d.staffname });
                                        }
                                    });

                                    $scope.tempdata.push({ subjectname: dd.subjectname, staffname: dd.staffname, tempdatad: $scope.tempdata1 });

                                });


                                angular.forEach($scope.masteryear, function (ddd) {
                                    if (ddd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                        $scope.yearname = ddd.asmaY_Year;
                                    }
                                });


                                angular.forEach($scope.mastercourse, function (ddd) {
                                    if (ddd.asmcL_Id === parseInt($scope.ASMCL_Id)) {
                                        $scope.coursename = ddd.asmcL_ClassName;
                                    }
                                });

                                angular.forEach($scope.sectionlist, function (ddd) {
                                    if (ddd.asmS_Id === parseInt($scope.ASMS_Id)) {
                                        $scope.sectionname = ddd.asmC_SectionName;
                                    }
                                });

                                $scope.details = true;
                                console.log($scope.tempdata);

                            } else {
                                swal("No Records Found");
                            }
                        } else {
                            swal("Something Went Wrong Kindly Contact Administrator");
                        }

                        console.log($scope.getalldates);

                        $scope.details = true;
                        $scope.gettopicdetails = promise.gettopicdetails;
                    } else {
                        swal("Something Went Wrong Kindly Contact Administrator");
                    }
                });

            } else {
                $scope.submitted = true;
            }
        };

        $scope.isOptionsRequired = function () {
            return !$scope.topicdetails.some(function (options) {
                return options.Selected;
            });
        };

        // TO Save The Data
        $scope.submitted = false;

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return JSON.stringify(obj.alldates).indexOf($scope.searchValue) >= 0;
        };



        $scope.print = function () {
            var innerContents = document.getElementById("printSectionId1").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
    }

})();