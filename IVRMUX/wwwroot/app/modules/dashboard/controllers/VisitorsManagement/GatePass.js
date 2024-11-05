(function () {
    'use strict';
    angular
        .module('app')
        .controller('GatePassController', GatePassController)

    GatePassController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache']
    function GatePassController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache) {

        $scope.onselectradio = function () {

            if ($scope.radiotype == 'studentgp') {
                $scope.HRME_Id = "";
                $scope.AGPH_Remark = "";
                $scope.stud = true;
                $scope.remarks = true;
                $scope.emp = false;
                $scope.empp = false;
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
            }
            else if ($scope.radiotype == 'employeegp') {
                $scope.AMST_Id = "";
                $scope.AGPH_Remark = "";
                $scope.stud = false;
                $scope.remarks = true;
                $scope.emp = true;
                $scope.sttud = false;
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
            }

        };


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.loadgrid = function () {
            apiService.getURI("GatePass/getDetails/", 1).then(function (promise) {
                $scope.studentlist = promise.studentlist;
                $scope.employeelist = promise.employeelist;
            });
            //   $scope.cancel();
        };

        // TO Show The Data
        $scope.submitted = false;
        $scope.saveData = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.radiotype == 'studentgp') {
                    $scope.HRME_Id = 0;
                }
                else if ($scope.radiotype == 'employeegp') {
                    $scope.AMST_Id = 0;
                }

                var data = {
                    "AMST_Id": $scope.AMST_Id,
                    "HRME_Id": $scope.HRME_Id,
                    "AGPH_Remark": $scope.AGPH_Remark,
                    "radiotype": $scope.radiotype
                }

                apiService.create("GatePass/saveData", data).
                    then(function (promise) {
                        if ($scope.radiotype == 'studentgp') {
                            $scope.student = promise.student;
                            $scope.sttud = true;
                            $scope.screport = true;
                            $scope.SchoolLogo = promise.institution[0].mI_Logo;
                            $scope.SchollName = promise.institution[0].mI_Name;
                            $scope.SchollAdd = promise.institution[0].mI_Address1;
                        }
                        else if ($scope.radiotype == 'employeegp') {
                            $scope.employ = promise.employ;
                            $scope.empp = true;
                            $scope.screport = true;
                            $scope.SchoolLogo = promise.institution[0].mI_Logo;
                            $scope.SchollName = promise.institution[0].mI_Name;
                            $scope.SchollAdd = promise.institution[0].mI_Address1;
                        }
                    })
            }
        };




        //for print
        $scope.Print = function () {
            var innerContents = '';
            if ($scope.radiotype == 'studentgp') {
                innerContents = document.getElementById("printSectionId").innerHTML;
            }
            if ($scope.radiotype == 'employeegp') {
                innerContents = document.getElementById("printSectionId1").innerHTML;
            }
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

        // end for print

        $scope.cancel = function () {
            $scope.AGPH_Id = 0;
            $scope.AMST_Id = "";
            $scope.AGPH_Remark = "";
            $scope.HRME_Id = "";
            $scope.radiotype = "";
            $scope.Cumureport = false;
            $scope.Cumureport1 = false;
            $scope.stud = false;
            $scope.remarks = false;
            $scope.emp = false;
            $scope.sttud = false;
            $scope.screport = false;
            $scope.empp = false;
            $scope.export = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
            $state.reload();
        };
    }

})();