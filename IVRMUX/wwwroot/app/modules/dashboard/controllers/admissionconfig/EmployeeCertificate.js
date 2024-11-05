(function () {
    'use strict';
    angular
.module('app')
        .controller('VikasaAdmissionReportController', VikasaAdmissionReportController)

    VikasaAdmissionReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function VikasaAdmissionReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


        $scope.print_flag = true;
        $scope.grid_flag = false;
        $scope.stud_name_flag = false;
        $scope.report = false;
        $scope.obj = {};
        var words_string = "";

        $scope.loaddata = function () {
            var pageid = 1;
            apiService.getURI("VikasaAdmissionReport/getdata", pageid).
        then(function (promise) {
            $scope.employeedetails = promise.employeedetails;
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
                var data = {
                    "empid": $scope.empid,
                }
                apiService.create("VikasaAdmissionReport/ShowReport1", data).then(function (promise) {
                    if (promise != null) {
                        if (promise.studentlist.length > 0) {
                            $scope.report = true;
                            if ($scope.ts.optradio == 'S') {
                                $scope.bonfide = true;
                                $scope.bonfide1 = false;
                            }
                            else if ($scope.ts.optradio == 'L') {
                                $scope.bonfide = false;
                                $scope.bonfide1 = true;
                            }

                            if (promise.studentlist[0].amsT_Sex == "FEMALE") {
                                $scope.gen = "her";
                            }
                            else if (promise.studentlist[0] == "MALE") {
                                $scope.gen = "his";
                            }
                            $scope.todaydate = new Date();
                            $scope.empname = promise.studentlist[0].empname;
                            $scope.doa = promise.studentlist[0].doa;
                            $scope.designation = promise.studentlist[0].deptname;
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
            var data = "";
            if ($scope.ts.optradio == 'L') {

                var innerContents = document.getElementById("ADMNReport1").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.write('<html><head>' +
             '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                  '<link type="text/css" media="print" href="css/print/Vikasa/Admission/ADMNReportPdf.css" rel="stylesheet" />' +
         '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +

         '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
         );
                popupWinindow.document.close();

            } else {

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


        $scope.amountinwords = function convertNumberToWords(atotalc) {
            var words = new Array();
            words[0] = '';
            words[1] = 'One';
            words[2] = 'Two';
            words[3] = 'Three';
            words[4] = 'Four';
            words[5] = 'Five';
            words[6] = 'Six';
            words[7] = 'Seven';
            words[8] = 'Eight';
            words[9] = 'Nine';
            words[10] = 'Ten';
            words[11] = 'Eleven';
            words[12] = 'Twelve';
            words[13] = 'Thirteen';
            words[14] = 'Fourteen';
            words[15] = 'Fifteen';
            words[16] = 'Sixteen';
            words[17] = 'Seventeen';
            words[18] = 'Eighteen';
            words[19] = 'Nineteen';
            words[20] = 'Twenty';
            words[30] = 'Thirty';
            words[40] = 'Forty';
            words[50] = 'Fifty';
            words[60] = 'Sixty';
            words[70] = 'Seventy';
            words[80] = 'Eighty';
            words[90] = 'Ninety';
            atotalc = atotalc.toString();
            var atemp = atotalc.split(".");
            var number = atemp[0].split(",").join("");
            var n_length = number.length;

            if (n_length <= 9) {
                var n_array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0);
                var received_n_array = new Array();
                for (var i = 0; i < n_length; i++) {
                    received_n_array[i] = number.substr(i, 1);
                }
                for (var i = 9 - n_length, j = 0; i < 9; i++, j++) {
                    n_array[i] = received_n_array[j];
                }
                for (var i = 0, j = 1; i < 9; i++, j++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        if (n_array[i] == 1) {
                            n_array[j] = 10 + parseInt(n_array[j]);
                            n_array[i] = 0;
                        }
                    }
                }
                atotalc = "";
                for (var i = 0; i < 9; i++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        atotalc = n_array[i] * 10;
                    } else {
                        atotalc = n_array[i];
                    }
                    if (atotalc != 0) {
                        words_string += words[atotalc] + " ";
                    }
                    if ((i == 1 && atotalc != 0) || (i == 0 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Crores ";
                    }
                    if ((i == 3 && atotalc != 0) || (i == 2 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Lakhs ";
                    }
                    if ((i == 5 && atotalc != 0) || (i == 4 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Thousand ";
                    }
                    if (i == 6 && atotalc != 0 && (n_array[i + 1] != 0 && n_array[i + 2] != 0)) {
                        words_string += "Hundred and ";
                    } else if (i == 6 && atotalc != 0) {
                        words_string += "Hundred ";
                    }
                }
                words_string = words_string.split("  ").join(" ");
            }
            return words_string;
        }


    }
})();

