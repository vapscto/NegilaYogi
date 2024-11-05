(function () {
    'use strict';
    angular.module('app').controller('CollegeTpinGenreationController', CollegeTpinGenreationController);
    CollegeTpinGenreationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'superCache', '$stateParams'];

    function CollegeTpinGenreationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, superCache, $stateParams) {

        $scope.chckedIndexs = [];
        $scope.unchckedIndexs = [];
        $scope.albumNameArraycolumn = [];
        $scope.columnsTest = [];

        $scope.SaveDis = false;
        $scope.IsHiddenup = true;
        $scope.IsHiddendown = false;
        $scope.currentPage = 1;
        $scope.gridswimming = false;
        $scope.gridslunch = false;
        $scope.gridlibrary = false;
        $scope.grid_flag = false;
        $scope.excel_flag = false;


        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;



        $scope.loaddata = function () {
            var pageid = 1;
            apiService.getURI("CollegeTpinGeneration/loaddata", pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.academicYearList = promise.getyearlist;
                } else {
                    swal("Something Went Wrong Contact Administrator");
                }
            });
        };

        $scope.onchnageyear = function () {
            $scope.grid_flag = false;            
        };

        $scope.search = function (att) {

            if ($scope.myForm.$valid) {
                $scope.report1 = [];
                $scope.printdatatable = [];
                $scope.grid_flag = false;

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id
                };

                apiService.create("CollegeTpinGeneration/search", data).then(function (promise) {
                    if (promise !== null) {
                        angular.forEach($scope.academicYearList, function (dd) {
                            if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                $scope.yearname = dd.asmaY_Year;
                            }
                        });

                        $scope.getstudentlistnotgiven = promise.gettpinnotgeneratedliststudent;
                        $scope.getstudentlistgiven = promise.gettpingeneratedliststudent;


                        $scope.reportlist = [];
                        $scope.reportlist.push({
                            year: $scope.yearname, notgeneratedlist: promise.gettpinnotgeneratedliststudent, genereatedlist: promise.gettpingeneratedliststudent
                        });

                        $scope.gridflag = true;
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };


        $scope.getstudentlistnotgiven = function (report1) {
            $scope.notgivenlist = report1.notgeneratedlist;
        };

        $scope.getstudentlistgiven = function (report1) {
            $scope.givenlist = report1.genereatedlist;
        };  

        $scope.generatetpin = function (att) {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("CollegeTpinGeneration/generatetpin", data).then(function (promise) {
                if (promise.returnval === true) {
                    swal("Tpin Number Generated Successfully");
                } else {
                    swal("Failed Generated Tpin Number");
                }
                $("#mymodalnotgivenlist").modal({ backdrop: false });
                $("#mymodaltgivenlist").modal({ backdrop: false });
                $state.reload();
            });
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.submitted = false;
        $scope.submitted1 = false;

        $scope.sortBy = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //search
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return JSON.stringify(obj.amaY_RollNo).indexOf($scope.searchValue) >= 0 ||
                angular.lowercase(obj.studentname).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (obj.amsT_RegistrationNo).indexOf($scope.searchValue) >= 0 ||
                (obj.amsT_AdmNo).indexOf($scope.searchValue) >= 0;
        };

        //clear data
        $scope.clearData = function () {
            $state.reload();
        };

        $scope.printstudentlist = function () {
            var innerContents = "";
            var popupWinindow = "";
            innerContents = document.getElementById("printSectionIdtotalstudentlist").innerHTML;
            popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                '<link type="text/css" media="print" href="css/print/NaacReportFeedback/FeedbackReportPdf.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();


        };
    }
})();
