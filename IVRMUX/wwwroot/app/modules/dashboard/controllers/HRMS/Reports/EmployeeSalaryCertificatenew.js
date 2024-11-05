(function () {
    'use strict';
    angular
        .module('app')
        .controller('EmployeeSalaryCertificatenewController', EmployeeSalaryCertificateReportController)

    EmployeeSalaryCertificateReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function EmployeeSalaryCertificateReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


        $scope.print_flag = true;
        $scope.grid_flag = false;
        $scope.stud_name_flag = false;
        $scope.report = false;
        $scope.obj = {};
        var words_string = "";

        $scope.loaddata = function () {
            var pageid = 1;
            apiService.getURI("EmployeeSalaryCertificate/getalldetails", pageid).then(function (promise) {
                $scope.employeedetails = promise.employeedetailList;
                $scope.leaveyeardropdown = promise.leaveyeardropdown;

                if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                    $scope.monthdropdown = promise.monthdropdown;
                    $scope.monthselectedAll = true;
                    angular.forEach($scope.monthdropdown, function (itm) {
                        itm.selected = true
                    });

                }
                })
        }

        $scope.getEmailsendingConfirmation = function () {

            var answer = confirm("Do you Want To Save The Record?");
            if (answer) {
                $scope.save_flag = "yes";
            }
            else {
                $scope.save_flag = "No";
            }
        }

        $scope.changerdio = function () {
            $scope.report = false;
        }

        $scope.ShowReport = function (obj) {
            if ($scope.myForm.$valid) {
                $scope.monthselected = [];
                angular.forEach($scope.monthdropdown, function (itm) {
                    if (itm.selected) {
                        $scope.monthselected.push({ IVRM_Month_Id: itm.ivrM_Month_Id, IVRM_Month_Name: itm.ivrM_Month_Name });

                    }

                });
                $scope.noofdays = $scope.monthselected.length;

                $scope.earningdetails = [];
                $scope.deductiondetails = [];
                $scope.noofdays = $scope.noofdays;
                $scope.totalearning = 0;
                $scope.totaldeduction = 0;

                var data = {
                    //"empid": $scope.empid,
                    "HRME_Id": $scope.HRME_Id,
                    
                }
                apiService.create("EmployeeSalaryCertificate/GetEmployeeSalaryCertificate", data).then(function (promise) {
                    if (promise != null) {
                        if (promise.reportData.length > 0) {
                            $scope.report = true;                           
                            var earningdetails = [];
                            var deductiondetails = [];
                            for (var i = 0; i < promise.reportData.length; i++) {
                                if (promise.reportData[i].HREED_Amount>0) {

                                    if (promise.reportData[i].HRMED_EarnDedFlag == "Earning") {

                                        var headwisetotalearning = promise.reportData[i].HREED_Amount * $scope.noofdays
                                        earningdetails.push({ HRMED_Name: promise.reportData[i].HRMED_Name, HREED_Amount: promise.reportData[i].HREED_Amount, headwisetotalearning: headwisetotalearning })

                                        $scope.totalearning = $scope.totalearning + promise.reportData[i].HREED_Amount
                                    }
                                    else if (promise.reportData[i].HRMED_EarnDedFlag == "Deduction") {
                                        var headwisetotaldeduction = promise.reportData[i].HREED_Amount * $scope.noofdays
                                        deductiondetails.push({ HRMED_Name: promise.reportData[i].HRMED_Name, HREED_Amount: promise.reportData[i].HREED_Amount, headwisetotaldeduction: headwisetotaldeduction })
                                        $scope.totaldeduction = $scope.totaldeduction + promise.reportData[i].HREED_Amount
                                    }

                                }
                            }
                            $scope.earningdetails = earningdetails;
                            $scope.deductiondetails = deductiondetails;
                            $scope.todaydate = new Date();
                            $scope.empname = promise.reportData[0].EmployeeName;
                            $scope.doa = promise.reportData[0].doa;
                            $scope.designation = promise.reportData[0].deptname;

                            $scope.NetAmountInwords = toWords($scope.totaldeduction * $scope.noofdays) + 'Rupees only.';
                            //if (promise.reportData[0].amsT_Sex == "FEMALE") {
                            //    $scope.gen = "her";
                            //}
                            //else if (promise.reportData[0] == "MALE") {
                            //    $scope.gen = "his";
                            //}
                           
                        }
                        else {
                            swal("No Record Found");
                            $scope.report = false;
                        }
                    }
                    else {
                        swal("No Record Found");
                        $scope.report = false;
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.Clearid = function () {
            $state.reload();
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.printToCart = function () {


            var innerContents = document.getElementById("ADMNReport").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" href="css/print/Vikasa/Admission/ADMNReportPdf.css" rel="stylesheet" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +

                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();

           




        }


        $scope.searchfilter = function (objj, radioobj) {

            if (objj.search.length >= '2' && radioobj == 'S') {
                $scope.studentlst = "";
                var data = {
                    "searchfilter": objj.search,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMST_SOL": radioobj,
                }

                apiService.create("VikasaAdmissionReport/searchfilter", data).
                    then(function (promise) {
                        if (promise.fillstudlist != null || promise.fillstudlist.length > 0) {
                            $scope.studlist = promise.fillstudlist;
                        } else {
                            $scope.AMST_Id = "";
                            swal("No students are found for your search");
                        }
                    })
            }
        };


        //By group Type
        $scope.GetmonthAll = function (monthselectedAll) {
            var toggleStatus = monthselectedAll;
            angular.forEach($scope.monthdropdown, function (itm) {
                itm.selected = toggleStatus;
            });


        };

        $scope.Getmonthlist = function (designation) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.monthselectedAll = $scope.monthdropdown.every(function (itm) {
                return itm.selected;
            });

        };



        var th = ['', 'thousand', 'million', 'billion', 'trillion'];
        var dg = ['Zero', 'One', 'Two', 'Three', 'Four', 'Five', 'Six', 'Seven', 'Eight', 'Nine'];
        var tn = ['Ten', 'Eleven', 'Twelve', 'Thirteen', 'Fourteen', 'Fifteen', 'Sixteen', 'Seventeen', 'Eighteen', 'Nineteen'];
        var tw = ['Twenty', 'Thirty', 'Forty', 'Fifty', 'Sixty', 'Seventy', 'Eighty', 'Ninety'];


        function toWords(s) {
            s = s.toString();
            s = s.replace(/[\, ]/g, '');
            if (s != parseFloat(s)) return 'not a number';
            var x = s.indexOf('.');
            if (x == -1) x = s.length;
            if (x > 15) return 'too big';
            var n = s.split('');
            var str = '';
            var sk = 0;
            for (var i = 0; i < x; i++) {
                if ((x - i) % 3 == 2) {
                    if (n[i] == '1') {
                        str += tn[Number(n[i + 1])] + ' ';
                        i++;
                        sk = 1;
                    }
                    else if (n[i] != 0) {
                        str += tw[n[i] - 2] + ' ';
                        sk = 1;
                    }
                }
                else if (n[i] != 0) {
                    str += dg[n[i]] + ' ';
                    if ((x - i) % 3 == 0) str += 'hundred ';
                    sk = 1;
                }


                if ((x - i) % 3 == 1) {
                    if (sk) str += th[(x - i - 1) / 3] + ' ';
                    sk = 0;
                }
            }
            if (x != s.length) {
                var y = s.length;
                str += 'point ';
                for (var i = x + 1; i < y; i++) str += dg[n[i]] + ' ';
            }
            return str.replace(/\s+/g, ' ');
        }





    }
})();

