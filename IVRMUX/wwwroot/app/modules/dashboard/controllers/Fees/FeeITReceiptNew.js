

(function () {
    'use strict';
    angular
.module('app')
.controller('FeeITReceiptReportController', FeeITReceiptReportController123)

    FeeITReceiptReportController123.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http','superCache']
    function FeeITReceiptReportController123($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;         
            var pageid = 2;
            apiService.getURI("FeeITReceiptReport/getalldetails123", pageid).
        then(function (promise) {  
            //$scope.studentlst = promise.admsudentslist;
            $scope.acayyearbind = promise.academicyr;
            $scope.ASMAY_Year = promise.academicyr[0].asmaY_Year;
            $scope.date = new Date(); 
        })
        }

        //OnChange
        $scope.selectacademicyear = function () {
            var data = {
                "asmyid": $scope.asmaY_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeITReceiptReport/selectacademicyear", data).
                then(function (promise) {
                    $scope.studentlst = promise.admsudentslist;
                })
        }

       //Report
        $scope.ShowReportdata = function () {                 
            var data = {
                "datedisplay": $scope.date,
                "Amst_Id": $scope.Amst_Id.amst_Id,
                "asmyid": $scope.asmaY_Id,               
            }
            apiService.create("FeeITReceiptReport/getreport", data).
        then(function (promise) {
            $scope.getclientsnames = promise.studentsnames;
            angular.forEach($scope.getclientsnames, function (e) { $scope.ClientName = e.clientname; });
            angular.forEach($scope.getclientsnames, function (e) { $scope.ClientAddress = e.insaddress; });
            angular.forEach($scope.getclientsnames, function (e) { $scope.stuname = e.stuname; });
            angular.forEach($scope.getclientsnames, function (e) { $scope.stufather = e.fathername; });
            angular.forEach($scope.getclientsnames, function (e) { $scope.stumother = e.mothername; });
            angular.forEach($scope.getclientsnames, function (e) { $scope.stucls = e.classname; });
            angular.forEach($scope.getclientsnames, function (e) { $scope.stusec = e.sectionname; });  
            //angular.forEach($scope.getclientsnames, function (e) { $scope.ASMAY_Year = e.ASMAY_Year; });
            angular.forEach($scope.getclientsnames, function (e) { $scope.AMST_AdmNo = e.AMST_AdmNo; });
              
            $scope.students = promise.reportdatelist;

            $scope.TotalDisplay = $scope.getTotal(promise.reportdatelist);

            $scope.words = $scope.amountinwords($scope.TotalDisplay);
        })
        }
       

        $scope.getTotal = function (int) {
            var total = 0;
            angular.forEach($scope.students, function (e) {
                total += e.tot;
            });
            return total;
        };

        //Amount In Words
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
            var words_string = "";
            if (n_length <= 9) {
                var n_array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0);
                var received_n_array = new Array();
                for (var i = 0; i < n_length; i++) {
                    received_n_array[i] = number.substr(i, 1);
                }
                for (var i = 9 - n_length, j = 0; i < 9; i++ , j++) {
                    n_array[i] = received_n_array[j];
                }
                for (var i = 0, j = 1; i < 9; i++ , j++) {
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

        //PrintData
        $scope.printData = function (printmodal) {
            var innerContents = document.getElementById("printmodal").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                '<link href="plugins/bootstrap/css/bootstrap.css" />' +
                '<link href="css/style.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
    }
})();

