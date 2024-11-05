(function () {
    'use strict';
    angular
        .module('app')
        .controller('SendCallLetterController', SendCallLetterController);

    SendCallLetterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter'];
    function SendCallLetterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter) {

        $scope.EarningDet = {};
        $scope.addjob = {};
        $scope.addjob.hrjD_Id = 0;
        $scope.submitted = true;
        $scope.currentdate = new Date();

        $scope.onLoadGetData = function () {
            var pageid = 2;
            //apiService.getURI("AddtoHRMS/getalldetails", pageid).then(function (promise) {
            //});
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.submitted = false;
        $scope.interacted1 = function (field) {
            return $scope.submitted;
        };

        $scope.AddtoTemplate = function () {
            if ($scope.myForm1.$valid) {
                $scope.EmployeeDis = true;
                $scope.interviewtimeformat = $filter('date')($scope.interviewtime, "H:mm  ");
                var data = {

                };
                apiService.create("Appointment/", data).
                    then(function (promise) {
                        
                    });
            }
            else {
                $scope.submitted = true;
            }
            
        };

        $scope.printData = function () {
            var innerContents = document.getElementById("calltemplate").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.SendMail = function () {

            $scope.tmplt = [];
            var Template = document.getElementById("empmail").innerHTML;
            $scope.tmplt.push({ hrmE_EmployeeCode: "1", TemplateString: Template });

            var data =
            {
                "HRCD_EmailId": $scope.hrcD_EmailId,
                Template: Template
            };

            apiService.create("AddCandidateVMS/sendCallLettermail", data).
                then(function (promise) {
                    if (promise.retrunMsg == "success") {
                        swal("Email Sent..!!!");
                    }
                    else if (promise.retrunMsg == "notFound") {
                        swal("Email Not sent..!!!", 'Default Email-Id is Not Found.. !!!');
                    }
                    else if (promise.retrunMsg == "Error") {
                        swal("Something went wrong", 'Try After some time..!!');
                    } else {
                        swal("Something went wrong", 'Try After some time..!!');
                    }
                });
        };
    }
})();