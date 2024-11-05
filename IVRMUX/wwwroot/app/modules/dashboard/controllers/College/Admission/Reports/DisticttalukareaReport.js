(function () {
    'use strict';

    angular
        .module('app')
        .controller('DistricttalukareaReportController', DistricttalukareaReportController);

    DistricttalukareaReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout'];

    function DistricttalukareaReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {

          $scope.currentPage = 1;
       $scope.itemsPerPage = 10;


        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("TeressianCertificate/getalldetails", pageid).
                then(function (promise) {
                    $scope.yearlist = promise.acdlist;
                    $scope.courselist = promise.courselist;
                    $scope.semesterlist = promise.semlist;
                    $scope.sectionlist = promise.seclist;
                    $scope.branchlist = promise.branchlist;
                    $scope.talukalist = promise.talukalist;
                  
                    $scope.districtlist = promise.districtlist;



                })
        }

        $scope.getCourse = function (acmAY_Id) {
            $scope.acmAY_Id = '';
            $scope.courselist = [];
            var data = {
                "ASMAY_Id": acmAY_Id
            }
            apiService.create("TeressianCertificate/getcoursedata", data).then(function (promise) {

                $scope.courselist = promise.courselist;


            })

        }
        $scope.getbranchdata = function (ASMAY_Id, AMCO_Id) {
            var data = {
                "AMCO_Id": AMCO_Id,
                "ASMAY_Id": ASMAY_Id
            }
            apiService.create("TeressianCertificate/getbranchdata", data).
                then(function (promise) {
                    $scope.branchlist = promise.branchlist;
                })
        };

        $scope.getsemisterdata = function (ASMAY_Id, AMCO_Id, AMB_Id) {
            var data = {
                "ASMAY_Id": ASMAY_Id,
                "AMCO_Id": AMCO_Id,
                "AMB_Id": AMB_Id
            }
            apiService.create("TeressianCertificate/getsemisterdata", data).
                then(function (promise) {
                    $scope.semlist = promise.semlist;
                })
        };
        

        $scope.obj = {};
        $scope.get_report = function (obj) {
            $scope.submitted = true;          
            if ($scope.myForm.$valid) {
                var param = '';
                if ($scope.customercertificate === 'taluk') {
                    param = obj.amcsT_Taluk;                 
                }
                else if ($scope.customercertificate === 'district') {
                    param = obj.amcsT_District;   
                }
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMCO_Id": $scope.amcO_Id,
                    "AMSE_ID": $scope.amsE_Id,
                    "ACMS_ID": $scope.acmS_Id,
                    "AMB_Id": $scope.amB_Id,
                    "report_name": $scope.customercertificate,
                    "param": param
                }
                apiService.create("TeressianCertificate/GetCertificate", data).
                    then(function (promise) {

                        $scope.reportpart = true;
                        if ($scope.customercertificate === 'taluk') {
                            $scope.Reportname = 'Taluk ';                       
                            $scope.newuser = promise.getreportdata;                                         
                        }
                        else if ($scope.customercertificate === 'district') {
                            $scope.Reportname = 'District';
                            $scope.newuser = promise.getreportdata;           
                        }
                        
                    })
            }

        };
        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EligibilityCert/EligibilitycertPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        $scope.Clear = function () {
            $state.reload();
        }
        $scope.exportToExcel = function (datatable) {

            var exportHref = Excel.tableToExcel(datatable, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);

        }
     

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

    }
})();
