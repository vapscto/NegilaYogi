(function () {
    'use strict';
    angular.module('app').controller('SummaryreportsController', SummaryreportsController)
    SummaryreportsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function SummaryreportsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;
        $scope.obj = {};
        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("Summaryreports/onloaddata", pageid).then(function (promise) {
                $scope.getloaddetails = promise.getloaddetails;
                $scope.emp_list = promise.employeedetails;

                //inhouse
                $scope.employeelist = promise.internalemployeedetails;
                $scope.programname = promise.programname;

            });
        };


        $scope.monthdays = function () {
            if ($scope.all1 == 1) {
                $scope.days1 = "";
                $scope.days2 = "";
            }
            else if ($scope.all1 == 0) {
                $scope.days = "";
            }
        }
        ////Outhouse
        $scope.ShowReport = function () {
            $scope.sumary1 = [];
            $scope.sumary = [];
            $scope.trainingsumary = [];
            $scope.searchValue = "";
            var data = {
                "startdate": new Date($scope.start_Date).toDateString(),
                "enddate": new Date($scope.end_Date).toDateString(),
                "Rvalue": $scope.all1,
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                apiService.create("Summaryreports/getreport", data).then(function (promise) {
                    if (promise.summaryreport.length > 0) {
                        $scope.sumary = promise.summaryreport;                       
                        $scope.name = $scope.sumary[0].HRME_EmployeeFirstName;
                        $scope.presentCountgrid = $scope.sumary.length;

                        $scope.employeeid = [];
                        angular.forEach($scope.sumary, function (dev) {
                            if ($scope.employeeid.length === 0) {
                                $scope.employeeid.push({
                                    HRME_Id: dev.HRME_Id,
                                    HRMETRTY_Id: dev.HRMETRTY_Id,
                                    HRME_EmployeeFirstName: dev.HRME_EmployeeFirstName,
                                    HRMETRTY_ExternalTrainingType: dev.HRMETRTY_ExternalTrainingType,
                                    HREXTTRN_TrainingTopic: dev.HREXTTRN_TrainingTopic,
                                    HRMETRTY_MinimumTrainingHrs: dev.HRMETRTY_MinimumTrainingHrs,
                                    TotalDuration: dev.TotalDuration
                                });
                            } else if ($scope.employeeid.length > 0) {
                                var intcount = 0;
                                angular.forEach($scope.employeeid, function (emp) {
                                    if (emp.HRME_Id === dev.HRME_Id && emp.HRMETRTY_Id == dev.HRMETRTY_Id) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.employeeid.push({
                                        HRME_Id: dev.HRME_Id,
                                        HRMETRTY_Id: dev.HRMETRTY_Id,
                                        HRME_EmployeeFirstName: dev.HRME_EmployeeFirstName,
                                        HRMETRTY_ExternalTrainingType: dev.HRMETRTY_ExternalTrainingType,
                                        HREXTTRN_TrainingTopic: dev.HREXTTRN_TrainingTopic,
                                        HRMETRTY_MinimumTrainingHrs: dev.HRMETRTY_MinimumTrainingHrs,
                                        TotalDuration: dev.TotalDuration
                                    });
                                }
                            }

                        });
                        angular.forEach($scope.employeeid, function (ddd) {
                            $scope.templist = [];
                            var sum_deviation = 0;
                            angular.forEach($scope.sumary, function (dd) {
                                if (dd.HRME_Id === ddd.HRME_Id && dd.HRMETRTY_Id === ddd.HRMETRTY_Id) {
                                    $scope.templist.push(dd);
                                }
                            });
                            ddd.trainingdetails = $scope.templist;
                        });
                    }                              
                    else {
                            swal("No Records Found");
                            $scope.count = 0;
                    }
                      
                });
            }
        };

        //Inhouse
        $scope.InhouseReport = function () {
            $scope.sumary = [];
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var HRME_Id = 0;
                var HRTCR_Id = 0;
                if ($scope.obj.hrmE_Id.hrmE_Id > 0 && $scope.all1 =="staffwise") {
                    HRME_Id = $scope.obj.hrmE_Id.hrmE_Id;

                }
                else if ($scope.obj.hrtcR_Id.hrtcR_Id > 0 && $scope.all1 == "programwise")
                {
                    HRTCR_Id = $scope.obj.hrtcR_Id.hrtcR_Id;
                }
                     var data = {
                         "HRTCR_Id": HRTCR_Id,
                         "HRME_Id": HRME_Id
                };
          

                apiService.create("Summaryreports/inhouseReportreport", data).
                    then(function (promise) {
                        if (promise.internalreport.length > 0) {
                            $scope.sumary = promise.internalreport;
                            $scope.name = $scope.sumary[0].emplYoeeName;
                            $scope.program = $scope.sumary[0].hrtcR_PrgogramName;
                            $scope.presentCountgrid = $scope.sumary.length;
                        }
                        else {
                            swal("No Records Found");
                            $scope.count = 0;
                        }
                    })
            }
        };


        $scope.viewcomment = function (aaa) {
            $('#myimagePreview').modal('show');
            var data = {
                //"HREXTTRN_Id": $scope.HREXTTRN_Id
                //"HREXTTRN_Id": $scope.aaa,
                "HRME_Id": $scope.obj.hrmE_Id.hrmE_Id
            };
            apiService.create("Summaryreports/trainingcertificate", data).then(function (promise) {

                //$scope.trainingdetails = promise.trainingdetails;

                //$scope.emplYoeeName = $scope.trainingdetails[0].emplYoeeName;

                $scope.certificatedetails = promise.certificatedetails;
                $scope.emplYoeeName = $scope.certificatedetails[0].emplYoeeName;
                $scope.hrtcR_PrgogramName = $scope.certificatedetails[0].hrtcR_PrgogramName;
                $scope.hrtcR_StartDate = $scope.certificatedetails[0].hrtcR_StartDate;


            });
        };


        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("Training").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExcel = function () {
            var divToPrint = document.getElementById("Training");
            var blob = new Blob([divToPrint.outerHTML], {
                type: "application/vnd.ms-excel;charset=utf-8"
            });
            saveAs(blob, "Report.xls");
        };




        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };



        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("printDeviation").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/NDSReportPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        //// to sum a training 
        $scope.sumoftraining = function () {
            var total = 0;
            if ($scope.sumary != null) {
                for (var i = 0; i < $scope.sumary.length; i++) {
                    var add = $scope.sumary[i];
                    total += add.Balancehrs;
                }
            }
            return total;
        };
        $scope.sumTotalHrs = function () {
            var totalHrs = 0;
            if ($scope.sumary != null) {
                for (var i = 0; i < $scope.sumary.length; i++) {
                    var addHrs = $scope.sumary[i];
                    totalHrs += addHrs.HREXTTRN_TotalHrs;
                }
            }
            return totalHrs;
        };
        $scope.sumoftraining11 = function () {
            var total1 = 0;           
            if ($scope.sumary != null) {
                for (var i = 0; i < $scope.sumary.length; i++) {
                    var addDuration = $scope.sumary[i];
                    total1 += addDuration.TotalDuration;
                }
            }     
            return total1;
        }
        $scope.sumofusedDuration = function () {
            var total2 = 0;
            if ($scope.sumary != null) {
                for (var i = 0; i < $scope.sumary.length; i++) {
                    var add1 = $scope.sumary[i];
                    total2 += add1.TotalusedDuration;
                }
            }

            return total2;
        }
        

    }
})();

