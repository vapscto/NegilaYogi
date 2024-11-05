(function () {
    'use strict';
    angular
        .module('app')
        .controller('QuotaCategoryReportController', QuotaCategoryReportController)

    QuotaCategoryReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function QuotaCategoryReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        var paginationformasters;
        var copty;

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        $scope.currentPage = 1;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');


        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("QuotaCategoryReport/getdetails", pageid).
                then(function (promise) {
                    $scope.acdlist = promise.acdlist;
                    $scope.quotalist = promise.quotalist;
                    $scope.seclist = promise.seclist;
                    $scope.catlist = promise.catlist;
                })
        };

        $scope.onselectAcdYear = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
            }
            apiService.create("QuotaCategoryReport/onselectAcdYear", data).then(function (promise) {
                if (promise.courselist != null) {
                    $scope.courselist = promise.courselist;
                } else {
                    swal("No Records Found")
                }
            })
        }

        $scope.onselectCourse = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
            }
            apiService.create("QuotaCategoryReport/onselectCourse", data).then(function (promise) {
                if (promise.branchlist != null) {
                    $scope.branchlist = promise.branchlist;
                } else {
                    swal("No Records Found")
                }
            })
        }

        $scope.onselectBranch = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
            }
            apiService.create("QuotaCategoryReport/onselectBranch", data).then(function (promise) {
                if (promise.semlist != null) {
                    $scope.semlist = promise.semlist;
                } else {
                    swal("No Records Found")
                }
            })
        }

        $scope.submitted = false;

        $scope.onreport = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                    "ACQ_Id": $scope.ACQ_Id,
                    "ACQC_Id": $scope.ACQC_Id,
                    "branchid": $scope.branchlist,
                    "semester": $scope.semlist,
                    "quota": $scope.quotalist,
                    "quotacategry": $scope.catlist,
                }
                apiService.create("QuotaCategoryReport/onreport", data).
                    then(function (promise) {
                        if (promise.datareport != null) {
                            $scope.main_list = promise.datareport;
                            $scope.masterinst = promise.masterinst;
                            $scope.instname = $scope.masterinst[0].mI_Name;
                            $scope.instadd = $scope.masterinst[0].mI_Address1 + ' ' + $scope.masterinst[0].mI_Address2 + ' ' + $scope.masterinst[0].mI_Address3;

                            angular.forEach($scope.acdlist, function (yy) {

                                if (yy.asmaY_Id == $scope.ASMAY_Id) {
                                    $scope.year = yy.asmaY_Year;
                                }
                            })

                        } else {
                            swal("No Records Found")
                        }

                    })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.printData = function () {
            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.cancel = function () {
            $scope.ASMAY_Id = '';
            $scope.AMCO_Id = '';
            $scope.AMB_Id = '';
            $scope.AMSE_Id = '';
            $scope.ACQ_Id = '';
            $scope.ACQC_Id = '';
            $scope.ACMS_Id = '';
            $scope.main_list = [];
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        }

        $scope.printdatatable = [];

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.main_list, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
        }

        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all = $scope.main_list.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
        }

    }

})();