(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeedBackSchoolReportController', FeedBackSchoolReportController)

    FeedBackSchoolReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function FeedBackSchoolReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {
        CanvasJS.addColorSet("graphcolor",
            [//colorSet Array
                "#34495E",
                "#85C1E9",
                "#DAF7A6",
                "#FFC300",
                "#FF5733"
            ]);
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length !== 0 && ivrmcofigsettings.length !== null && ivrmcofigsettings.length !== undefined) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null) {
            if (admfigsettings.length > 0) {
                logopath = admfigsettings[0].asC_Logo_Path;
            }
        } else {
            logopath = "";
        }
        $scope.imgname = logopath;
        $scope.searchValue = '';
        $scope.search = "";
        $scope.obj = {};
        $scope.BindData = function () {
            apiService.getDATA("FeedBackSchoolReport/getdetails").then(function (promise) {
                $scope.yearlist = promise.getyear;
                $scope.feedbacktype = promise.feedbacktype;
                $scope.classlist = promise.classlist;

            });
        };
        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
        $scope.submitted = false;
        $scope.onreport = function (obj) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var ASMCL_Id; var ASMS_Id;
                if ($scope.optionflag == "COUNT") {
                    ASMCL_Id = 0;
                    ASMS_Id = 0;
                }
                else {
                    ASMCL_Id = $scope.ASMCL_Id;
                    ASMS_Id = $scope.ASMS_Id;
                }
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "FMTY_Id": $scope.FMTY_Id,
                    "optionflag": $scope.optionflag,
                    "ASMCL_Id": ASMCL_Id,
                    "ASMS_Id": ASMS_Id
                };
                apiService.create("FeedBackSchoolReport/getreport", data).
                    then(function (promise) {
                        if (promise.getReport.length != null && promise.getReport.length > 0) {
                            $scope.studlist = promise.getReport;
                        }
                        else {
                            swal("Record Not Found !");
                        }
                    });
            }
        };
       //onclass
        $scope.onclass = function () {          
            var data = {
                "ASMCL_Id": $scope.obj.ASMCL_Id
            };
            apiService.create("FeedBackSchoolReport/onclass", data).
                then(function (promise) {
                    if (promise.sectionlist.length != null && promise.sectionlist.length > 0) {
                        $scope.sectionlist = promise.sectionlist;
                    }
                    else {
                        swal("Record Not Found !");
                    }
                });
        };
        $scope.onchangeradio = function () {
            $scope.studlist = [];
        };
        //NONC
        $scope.FBNotGivenCount = function (user) {
            $scope.getcount = [];
            $('#viewDeatils').modal('hide');
            var ASMAY_Id = user.ASMAY_Id;
            var FMTY_Id = user.FMTY_Id;
            var ASMCL_Id = user.ASMCL_Id;
            var ASMS_Id = user.ASMS_Id;
            var type = 0;
            var data = {
                "ASMAY_Id": ASMAY_Id,
                "FMTY_Id": FMTY_Id,
                "ASMCL_Id": ASMCL_Id,
                "ASMS_Id": ASMS_Id,
                "type": type
            };
            apiService.create("FeedBackSchoolReport/count", data).
                then(function (promise) {
                    if (promise.getcount.length != null && promise.getcount.length > 0) {
                        $scope.getcount = promise.getcount;
                        $('#viewDeatils').modal('show');
                    }
                    else {
                        swal("Record Not Found !");
                    }
                });
        };
        //FBGivenCount
        $scope.FBGivenCount = function (user) {
            $scope.getcount = [];
            $('#viewDeatils').modal('hide');
            var ASMAY_Id = user.ASMAY_Id;
            var FMTY_Id = user.FMTY_Id;
            var ASMCL_Id = user.ASMCL_Id;
            var ASMS_Id = user.ASMS_Id;
            var type = 1;
            var data = {
                "ASMAY_Id": ASMAY_Id,
                "FMTY_Id": FMTY_Id,
                "ASMCL_Id": ASMCL_Id,
                "ASMS_Id": ASMS_Id,
                "type": type
            };
            apiService.create("FeedBackSchoolReport/count", data).
                then(function (promise) {
                    if (promise.getcount.length != null && promise.getcount.length > 0) {
                        $scope.getcount = promise.getcount;
                        $('#viewDeatils').modal('show');
                    }
                    else {
                        swal("Record Not Found !");
                    }
                });
        };
        $scope.cancel = function () {
            $state.reload();
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        };
        $scope.Print = function () {
            var innerContents = document.getElementById("printable").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
    }
})();