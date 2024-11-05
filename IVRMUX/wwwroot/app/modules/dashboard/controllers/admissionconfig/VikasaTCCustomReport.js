(function () {
    'use strict';
    angular
        .module('app')
        .controller('HHSTCCustomReportController', HHSTCCustomReportController)

    HHSTCCustomReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function HHSTCCustomReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {
        var data1;
        var frommonth;
        var tomonth;
        $scope.class = true;
        $scope.rad = true;
        //$scope.report = false;
        $scope.amsT_Date = new Date();
        $scope.Print_flag = false;

        $scope.loaddata = function () {

            apiService.getURI("HHSTCCustomReport/getdetails/", 2).then(function (promise) {
                $scope.yearDropdown = promise.accyear;
                //$scope.classlist = promise.accclass;
                //$scope.seclist = promise.accsection;
                // $scope.std_name = false;

            });
        };

        $scope.ShowHideup = function () {
            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        };

        $scope.ShowHidedown = function () {
            $scope.IsHiddendown = $scope.IsHiddendown ? false : true;
        };

        $scope.onclickregnoname = function () {

            var data = {
                "admnoorname": $scope.admname,
                "ASMAY_Id": $scope.asmaY_Id,
                "tctemporper": $scope.tctempper
            };

            apiService.create("HHSTCCustomReport/getnameregno", data).
                then(function (promise) {
                    if (promise.studentlist !== null && promise.studentlist.length > 0) {
                        $scope.studentDropdown = promise.studentlist;
                        $scope.class = false;
                        $scope.std_name = true;
                        $scope.clear_flag = true;
                        $scope.report_flag = true;
                    }
                    else {
                        swal("Student is not available for this selection");
                        //$scope.admname = "";
                        $scope.report = false;
                        $scope.std_name = false;
                        $scope.submitted = false;
                        $scope.class = true;
                    }
                });
        };

        $scope.stdnamechange = function () {
            var data = {
                "AMST_Id": $scope.amsT_Id,
                "ASMAY_Id": $scope.asmaY_Id
            };

            apiService.create("HHSTCCustomReport/stdnamechange", data).then
                (function (promise) {
                    if (promise.classsecregno !== null || promise.classsecregno.length > 0) {
                        $scope.lblclassname = promise.classsecregno[0].asmcL_ClassName;
                        $scope.lblsectioname = promise.classsecregno[0].asmC_SectionName;
                        $scope.lblregno = promise.classsecregno[0].amsT_RegistrationNo;
                    }
                    else {
                        swal("No Records Found");
                    }
                });
        };

        $scope.yeardd = function () {
            if ($scope.asmaY_Id !== undefined) {
                $scope.rad = false;
            }
            else {
                $scope.rad = true;
            }
        };


        $scope.onclicktcperortemo = function () {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "admnoorname": $scope.admname,
                "tctemporper": $scope.tctempper
                //"admnoorname": $scope.admname
            };
            apiService.create("HHSTCCustomReport/onclicktcperortemo", data).
                then(function (promise) {
                    if (promise.studentlist !== null && promise.studentlist.length > 0) {
                        $scope.rad = false;

                        $scope.studentDropdown = promise.studentlist;
                        $scope.std_name = true;
                        $scope.clear_flag = true;
                        $scope.report_flag = true;
                    }
                    else {
                        swal("Student is not available for this selection");
                        // $scope.admname = "";
                        $scope.report = false;
                        $scope.std_name = false;
                        $scope.submitted = false;

                    }
                });
        };



        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.submitted = false;
        $scope.savetmpldata = function () {

            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMST_Id": $scope.amsT_Id,
                    "tctemporper": $scope.tctempper,
                    "leftDate": new Date($scope.amsT_Date).toDateString()
                };

                apiService.create("HHSTCCustomReport/Vikasha_getTcdetails", data)
                    .then(function (promise) {

                        if (promise.studentTCList.length > 0) {
                            $scope.Show_report = true;

                            if ($scope.format == "1") {
                                $scope.Show_report1 = true;
                                $scope.Show_report2 = false;
                            }
                            else {
                                $scope.Show_report1 = false;
                                $scope.Show_report2 = true;
                            }

                            var datafrom = new Date(promise.academicList1[0].asmaY_From_Date);
                            var datato = new Date(promise.academicList1[0].asmaY_To_Date);

                            var n = datafrom.getMonth();
                            $scope.getmontnames(n);
                            $scope.frommonth1 = frommonth;

                            var m = datato.getMonth();
                            $scope.getmontnames(m);
                            $scope.tomonth1 = frommonth;

                            var n1 = datafrom.getFullYear();
                            $scope.fromyear = n1;

                            var m1 = datato.getFullYear();
                            $scope.toyear = m1;

                            $scope.Print_flag = true;
                            $scope.IsHiddendown = true;
                            $scope.report = true;
                            $scope.leftdate = promise.studentTCList[0].leftDate;
                            $scope.tcno = promise.studentTCList[0].astC_TCNO;
                            $scope.admno = promise.studentTCList[0].amsT_RegistrationNo;
                            $scope.lblstdname = promise.studentTCList[0].studentname;
                            $scope.lblfathername = promise.studentTCList[0].amsT_FatherName;
                            $scope.lblmothername = promise.studentTCList[0].amsT_MotherName;
                            $scope.lblreligioncaste = promise.studentTCList[0].religion;
                            $scope.lblnationality = promise.studentTCList[0].nationality;
                            $scope.lblpobirth = promise.studentTCList[0].amsT_BirthPlace;
                            $scope.lbldob = promise.studentTCList[0].amsT_DOB;
                            $scope.lbldobwords = promise.studentTCList[0].amsT_DOB_Words;
                            $scope.lbllastschool = promise.masterCompany[0].companyname;
                            $scope.lbldoa = promise.studentTCList[0].amsT_Date;
                            $scope.lblprogress = "Satisfactory";
                            $scope.lblconduct = promise.studentTCList[0].astC_Conduct;
                            $scope.lblPromotion = promise.studentTCList[0].qualificlass;
                            $scope.lbldol = promise.studentTCList[0].astC_TCIssueDate;
                            $scope.lblstudyingsince = "";
                            //$scope.fromdate = $scope.frommonth1 + '  ' + $scope.fromyear + '  ' + 'to' + '  ' + $scope.tomonth1 + '  ' + $scope.toyear;
                            $scope.fromdate = 'JUNE' + '  ' + $scope.fromyear + '  ' + 'to' + '  ' + 'MAY' + '  ' + $scope.toyear;
                            $scope.lblallsumsfee = promise.studentTCList[0].fee_Due_Amnt;
                            $scope.lblreasonleaving = promise.studentTCList[0].astC_LeavingReason;
                            $scope.lbldate = promise.studentTCList[0].astC_TCIssueDate;
                            $scope.lblclassnamed = promise.studentTCList[0].classname;
                            $scope.SwitchFuction($scope.lblclassnamed);
                            $scope.lblclassname = data1;

                            $scope.emisnumber = promise.studentTCList[0].bpl_id;
                            $scope.lblpromoted = $scope.promotion;
                            $scope.stream = $scope.stream;
                            if ($scope.format == 2) {
                                $scope.lblpreviousschool = $scope.lastschool;
                            }

                            if (promise.masterCompany[0].companyname === "The Vikasa  School") {
                                $scope.schoolimage = "css/print/Vikasa/VIKASATCREPORT/vikasalogo.png";
                                $scope.schoolname = "The Vikasa School";
                                $scope.grade = "ICSE/ISC Stream";
                                $scope.streamc = "Stream";
                                $scope.icseisc = "ICSE/ISC";
                            }
                            else {
                                $scope.schoolimage = "css/print/Vikasa/VIKASATCREPORT/Vikasaintlogo.png";
                                $scope.schoolname = "The Vikasa International School";
                                $scope.grade = "CAIE/A LEVEL Curriculum";
                                $scope.streamc = "Curriculum";
                                $scope.icseisc = "CAIE";

                            }
                        }
                        else {
                            swal("Select any Student");
                        }
                    });

            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted;
        };

        //to print
        $scope.printData = function () {
            var data = "";
            if ($scope.format == "1") {
                data = "VIKASATCREPORT";
            }
            else {
                data = "VIKASATCREPORT1";
            }
            var innerContents = document.getElementById(data).innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Vikasa/VIKASATCREPORT/VIKASATCReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };


        //class name from class id
        $scope.SwitchFuction = function (sno) {
            switch (sno) {
                case 'I':
                    data1 = "FIRST";
                    break;
                case 'II':
                    data1 = "SECOND";
                    break;
                case 'III':
                    data1 = "THIRD";
                    break;
                case 'IV':
                    data1 = "FOURTH";
                    break;
                case 'V':
                    data1 = "FIFTH";
                    break;
                case 'VI':
                    data1 = "SIXTH";
                    break;
                case 'VII':
                    data1 = "SEVENTH";
                    break;
                case 'VIII':
                    data1 = "EIGHTH";
                    break;
                case 'IX':
                    data1 = "NINTH";
                    break;
                case 'X':
                    data1 = "TENTH";
                    break;
                default:
                    data1 = sno;
                    break;
            }
            return data1;
        };

        //month names
        $scope.getmontnames = function (monthid) {
            frommonth = "";
            switch (monthid) {

                case 0:
                    frommonth = "JANUARY";
                    break;
                case 1:
                    frommonth = "FEBRUARY";
                    break;
                case 2:
                    frommonth = "MARCH";
                    break;
                case 3:
                    frommonth = "APRIL";
                    break;
                case 4:
                    frommonth = "MAY";
                    break;
                case 5:
                    frommonth = "JUNE";
                    break;
                case 6:
                    frommonth = "JULY";
                    break;
                case 7:
                    frommonth = "AUGUST";
                    break;
                case 8:
                    frommonth = "SEPTEMBER";
                    break;
                case 9:
                    frommonth = "OCTOBER";
                    break;
                case 10:
                    frommonth = "NOVEMBER";
                    break;
                case 11:
                    frommonth = "DECEMBER";
                    break;
                default:
                    frommonth = "";
                    break;
            }
            return frommonth;
        };

    }
})();
