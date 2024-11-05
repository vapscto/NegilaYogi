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
            apiService.getURI("VikasaAdmissionReport/getdata", pageid).then(function (promise) {
                $scope.yearlst = promise.allAcademicYear;
            });
        };

        $scope.getEmailsendingConfirmation = function () {

            var answer = confirm("Do you Want To Save The Record?");
            if (answer) {
                $scope.save_flag = "yes";
            }
            else {
                $scope.save_flag = "No";
            }
        };

        $scope.changerdio = function () {
            $scope.report = false;
        };

        $scope.ShowReport = function (obj) {
            if ($scope.myForm.$valid) {
                $scope.getEmailsendingConfirmation();
                var data = {
                    "AMST_Id": $scope.AMST_Id.amsT_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "radiobutton": $scope.Character,
                    "save_flag": $scope.save_flag,
                };
                apiService.create("VikasaAdmissionReport/ShowReport", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.studentlist.length > 0) {
                            $scope.schoolname = promise.mastercompany[0].mI_Name;

                            if (promise.message !== null && promise.message !== "") {
                                swal(promise.message);
                            }

                            $scope.report = true;
                            if (parseInt($scope.Character) === 0) {
                                $scope.bonfide = true;
                                $scope.cer = "Bonafide Certificate";
                                $scope.dr = $scope.obj.prurpose23;
                                $scope.dr1 = $scope.obj.prurpose23;
                            }
                            else if (parseInt($scope.Character) === 1) {
                                $scope.bonfide = false;
                                $scope.cer = "IT Declaration";
                            }
                            else if (parseInt($scope.Character) === 2) {
                                $scope.bonfide = false;
                                $scope.cer = "Conduct Certificate";
                            }
                            else if (parseInt($scope.Character) === 3) {
                                $scope.bonfide = false;
                                $scope.cer = "Admission Note";
                                $scope.dr = $scope.prurpose1;
                            }
                            else if (parseInt($scope.Character) === 4) {
                                $scope.bonfide = false;
                                $scope.cer = "Student TC";
                            }

                            $scope.todaydate = new Date();
                            $scope.amsT_Sex = promise.studentlist[0].amsT_Sex;
                            if (promise.studentlist[0].amsT_Sex === "Male") {
                                $scope.gen = "Mast.";
                                $scope.gen1 = "S/O";
                                $scope.gen2 = "His";
                                $scope.gen3 = "He";
                            }
                            else {
                                $scope.gen = "Miss.";
                                $scope.gen1 = "D/O";
                                $scope.gen2 = "Her";
                                $scope.gen3 = "She";
                            }
                            $scope.name = promise.studentlist[0].amsT_FirstName;
                            $scope.fname = promise.studentlist[0].amsT_FatherName;
                            $scope.dob = promise.studentlist[0].amsT_DOB;
                            $scope.classname = promise.studentlist[0].classname;
                            $scope.dobw = promise.studentlist[0].amsT_DOB_Words;
                            $scope.AMST_AdmNo = promise.studentlist[0].amsT_AdmNo;
                            $scope.fromdate = promise.studentlist[0].doa;
                            $scope.sectionname = promise.studentlist[0].sectionname;
                            $scope.address = promise.studentlist[0].address;
                            $scope.phone = promise.studentlist[0].mobileno;
                            if (parseInt($scope.Character) === 1) {
                                $scope.paidamount = promise.studentlist[0].paidamount;
                                $scope.amountinwords($scope.paidamount);
                                $scope.words_string = words_string;
                            }
                            if (parseInt($scope.Character) === 5) {
                                $scope.paidamount = promise.studentlist[0].paidamount;
                            }
                            if (parseInt($scope.Character) === 3) {
                                $scope.label = obj.prurpose1;
                            }
                            angular.forEach($scope.yearlst, function (y) {
                                if (y.asmaY_Id === parseInt($scope.asmaY_Id)) {
                                    $scope.year = y.asmaY_Year;
                                    $scope.todate = y.asmaY_To_Date;
                                }
                            });

                            if (parseInt($scope.Character) === 6) {
                                $scope.previousyear = promise.previousyear;
                                if ($scope.previousyear !== null && $scope.previousyear.length > 0) {
                                    $scope.fromdate = new Date($scope.previousyear[0].asmaY_From_Date);
                                }
                            }

                            $scope.tcno = "VIK/ON/" + $scope.year + "/" + promise.count + "";
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
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.printToCart = function () {
            var data = "";
            var innerContents = "";
            var popupWinindow = "";
            if (parseInt($scope.Character) === 0) {
                data = "ADMNReport";
                innerContents = document.getElementById(data).innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" href="css/print/Vikasa/Admission/ADMNReportPdf.css" rel="stylesheet" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +

                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }
            else if (parseInt($scope.Character) === 1) {
                data = "ADMNReport1";
                innerContents = document.getElementById(data).innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" href="css/print/Vikasa/Admission/ADMNReportPdf.css" rel="stylesheet" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +

                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }
            else if (parseInt($scope.Character) === 2) {
                data = "ADMNReport2";
                innerContents = document.getElementById(data).innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" href="css/print/Vikasa/Admission/ADMNReportConductPdf.css" rel="stylesheet" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +

                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }

            else if (parseInt($scope.Character) === 6) {
                data = "ADMNReport21";
                innerContents = document.getElementById(data).innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" href="css/print/Vikasa/Admission/ADMNReportConductPdf.css" rel="stylesheet" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +

                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }
            else if (parseInt($scope.Character) === 3) {
                data = "ADMNReport3";
                innerContents = document.getElementById(data).innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" href="css/print/Vikasa/Admission/ADMNReportNotePdf.css" rel="stylesheet" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +

                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }
            else if (parseInt($scope.Character) === 4) {
                data = "ADMNReport4";
                innerContents = document.getElementById(data).innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" href="css/print/Vikasa/Admission/ADMNReportTCPdf.css" rel="stylesheet" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +

                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }
            else if (parseInt($scope.Character) === 5) {
                data = "ADMNReport5";
                innerContents = document.getElementById(data).innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" href="css/print/Vikasa/Admission/ADMNReportPdf1.css" rel="stylesheet" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +

                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }
        };

        $scope.searchfilter = function (objj, radioobj) {
            if (objj.search.length >= '2') {
                $scope.studentlst = "";
                var data = {
                    "searchfilter": objj.search,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMST_SOL": radioobj
                };

                apiService.create("VikasaAdmissionReport/searchfilter", data).then(function (promise) {
                    if (promise.fillstudlist !== null || promise.fillstudlist.length > 0) {
                        $scope.studlist = promise.fillstudlist;
                    } else {
                        $scope.AMST_Id = "";
                        swal("No students are found for your search");
                    }
                });
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
    }
})();

