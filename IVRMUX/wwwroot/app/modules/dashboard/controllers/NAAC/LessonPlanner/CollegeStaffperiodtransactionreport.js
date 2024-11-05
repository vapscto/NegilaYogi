(function () {
    'use strict';
    angular
        .module('app')
        .controller('CollegeStaffperiodtransactionreportcontroller', CollegeStaffperiodtransactionreportcontroller)

    CollegeStaffperiodtransactionreportcontroller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function CollegeStaffperiodtransactionreportcontroller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


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
            apiService.getURI("CollegeStaffperiodtransactionreport/Getdetailstransaction", id).
                then(function (promise) {
                    $scope.masteryear = promise.getyear;
                });
        };

        $scope.obj = {};

        $scope.onselectAcdYear = function () {
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.ISMS_Id = "";
            $scope.getreport1 = [];
            $scope.getreport = [];
            $scope.getreportemployee = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };

            apiService.create("CollegeStaffperiodtransactionreport/onselectAcdYear", data).then(function (promise) {
                if (promise !== null) {
                    $scope.mastercourse = promise.getcourse;
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }
            });
        };

        $scope.onselectCourse = function () {
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.ISMS_Id = "";
            $scope.getreport1 = [];
            $scope.getreport = [];
            $scope.getreportemployee = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id
            };

            apiService.create("CollegeStaffperiodtransactionreport/onselectCourse", data).then(function (promise) {
                if (promise !== null) {
                    $scope.masterbranch = promise.getbranch;
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }
            });
        };

        $scope.onselectBranch = function () {
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.ISMS_Id = "";
            $scope.getreport1 = [];
            $scope.getreport = [];
            $scope.getreportemployee = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id
            };

            apiService.create("CollegeStaffperiodtransactionreport/onselectBranch", data).then(function (promise) {
                if (promise !== null) {
                    $scope.mastersemester = promise.getsemester;
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }
            });
        };

        $scope.getsection = function () {

            $scope.getreport1 = [];
            $scope.getreport = [];
            $scope.getreportemployee = [];

            $scope.ACMS_Id = "";
            $scope.ISMS_Id = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id
            };

            apiService.create("CollegeStaffperiodtransactionreport/getsection", data).then(function (promise) {
                if (promise !== null) {
                    $scope.mastersection = promise.getsection;
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }
            });
        };

        $scope.onchangesection = function () {
            $scope.ISMS_Id = "";

            $scope.getreport1 = [];
            $scope.getreport = [];
            $scope.getreportemployee = [];

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ACMS_Id": $scope.ACMS_Id
            };

            apiService.create("CollegeStaffperiodtransactionreport/onchangesection", data).then(function (promise) {
                if (promise !== null) {
                    $scope.mastersubjects = promise.getemployee;
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
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ACMS_Id": $scope.ACMS_Id,

                };

                apiService.create("CollegeStaffperiodtransactionreport/getreport", data).then(function (promise) {
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
                                    if (ddd.amcO_Id === parseInt($scope.AMCO_Id)) {
                                        $scope.coursename = ddd.amcO_CourseName;
                                    }
                                });

                                angular.forEach($scope.masterbranch, function (ddd) {
                                    if (ddd.amB_Id === parseInt($scope.AMB_Id)) {
                                        $scope.branchname = ddd.amB_BranchName;
                                    }
                                });

                                angular.forEach($scope.mastersemester, function (ddd) {
                                    if (ddd.amsE_Id === parseInt($scope.AMSE_Id)) {
                                        $scope.semestername = ddd.amsE_SEMName;
                                    }
                                });

                                angular.forEach($scope.mastersection, function (ddd) {
                                    if (ddd.acmS_Id === parseInt($scope.ACMS_Id)) {
                                        $scope.sectionname = ddd.acmS_SectionName;
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