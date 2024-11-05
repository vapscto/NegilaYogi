(function () {
    'use strict';
    angular
        .module('app')
        .controller('CLGTTCoursewiseConsolidatedReport', CLGTTCoursewiseConsolidatedReport)

    CLGTTCoursewiseConsolidatedReport.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', 'Excel', '$timeout']
    function CLGTTCoursewiseConsolidatedReport($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, Excel, $timeout) {
        $scope.reportdata = true;
        $scope.loaddata = function () {        
            var pageid = 2;
            apiService.getURI("CLGTTCollegewiseConsolidatedReport/loaddata", pageid).then(function (promise) {             
                $scope.yearlist = promise.yearlist;
                $scope.categorylst = promise.categorylist;
                $scope.sectionlist = promise.sectionlist;
            })
        }


        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.getcourse = function () {
            $scope.reportdata = true;
            if ($scope.asmay_Id == "" && $scope.asmay_Id == undefined) {
                swal("Please Select The Academic Year !");
            }
            else if ($scope.asmay_Id != "" && $scope.asmay_Id != undefined && $scope.TTMC_Id != "") {
                var data = {
                    "TTMC_Id": $scope.TTMC_Id,
                    "ASMAY_Id": $scope.asmay_Id,
                }

                apiService.create("CLGTTCommon/getcourse_catg", data).
                    then(function (promise) {
                     
                        $scope.courselist = promise.courselist;

                        if (promise.courselist == "" || promise.courselist == null) {
                            swal("No Course/Branch Are Mapped To Selected Category");
                        }
                    })
            }
        }

        $scope.getbranch_catg = function () {
            $scope.reportdata = true;
            $scope.semisterlist = [];
            $scope.amB_Id = '';
            $scope.semisterlist = [];
            $scope.usercheck = false;
            $scope.branchlist = [];
            if ($scope.asmay_Id == "" && $scope.asmay_Id == undefined) {
                swal("Please Select The Academic Year !");
            }
            else if ($scope.asmay_Id != "" && $scope.asmay_Id != undefined && $scope.TTMC_Id != "") {
                var data = {
                    "TTMC_Id": $scope.TTMC_Id,
                    "ASMAY_Id": $scope.asmay_Id,
                    "AMCO_Id": $scope.amcO_Id,
                }
                apiService.create("CLGTTCommon/getbranch_catg", data).
                    then(function (promise) {
                  
                        $scope.branchlist = promise.branchlist;

                        if (promise.branchlist == "" || promise.branchlist == null) {
                            swal("No Branches Are Mapped To Selected Category/Course");
                        }
                    })
            }
        };


     
        $scope.get_semister = function () {
            $scope.reportdata = true;
            $scope.semisterlist = [];
            if ($scope.asmay_Id == "" && $scope.asmay_Id == undefined) {
                swal("Please Select The Academic Year !");
            }
            else if ($scope.asmay_Id != "" && $scope.asmay_Id != undefined && $scope.TTMC_Id != "") {
                var data = {
                    "TTMC_Id": $scope.TTMC_Id,
                    "ASMAY_Id": $scope.asmay_Id,
                    "AMCO_Id": $scope.amcO_Id,
                    "AMB_Id": $scope.amB_Id,
                }
                apiService.create("CLGTTCommon/get_semister", data).
                    then(function (promise) {
                      
                        $scope.semisterlist = promise.semisterlist;

                        if (promise.semisterlist == "" || promise.semisterlist == null) {
                            swal("No Semesters Are Mapped To Selected Category/Course");
                        }
                    })
            }
        };


        $scope.report = function () {
            $scope.submitted = true;
            $scope.reportdata = true;
            if ($scope.myForm.$valid) {
                var data = {

                    "TTMC_Id": $scope.TTMC_Id,
                    "ASMAY_Id": $scope.asmay_Id,
                    "AMCO_Id": $scope.amcO_Id,
                    "AMB_Id": $scope.amB_Id,
                    "AMSE_Id": $scope.amsE_Id,
                    "ACMS_Id": $scope.acmS_Id
                }
                apiService.create("CLGTTCollegewiseConsolidatedReport/report", data).then(function (promise) {











                    $scope.reportdata = false;
                    $scope.subject = promise.subject;
                    $scope.day = promise.day;
                    $scope.getreportdata = promise.getreportdata;
                    $scope.mainarray = [];
                    angular.forEach($scope.subject, function (y) {

                        var temp = [];
                        var count = 0;
                        angular.forEach($scope.getreportdata, function (m) {

                            if (y.ismS_Id == m.ISMS_Id) {
                                temp.push(m);
                                count += m.PCOUNT;
                            }

                        })
                        $scope.mainarray.push({
                            subjectname: y.ismS_SubjectName, ismS_Id: y.ismS_Id, ttlist: temp, total: count

                        })

                    })

                    console.log($scope.mainarray);

                })
            }
        }
        


        $scope.exportToExcel = function () {

            var table = '';

            table = '#A';


            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);

        }






        var paginationformasters = '';
        var ivrmcofigsettings = [];


        var copty;
        ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = [];

        $scope.coptyright = copty;
        $scope.ddate = new Date();
        admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;











        $scope.printData = function () {

            var innerContents = '';

            innerContents = document.getElementById("printareaId").innerHTML;



            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); },900);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();

        }




        $scope.clearid = function () {
            $state.reload();
        }

    };
})();