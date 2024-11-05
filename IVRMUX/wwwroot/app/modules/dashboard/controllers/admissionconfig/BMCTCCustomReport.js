(function () {
    'use strict';
    angular
        .module('app')
        .controller('BBKVCustomReportController', BBKVCustomReportController)

    BBKVCustomReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function BBKVCustomReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {
        var data1;
        var frommonth;
        var tomonth;
        $scope.tcpdf = false;
        $scope.cvtc = false;
        $scope.huvama = false;
        $scope.SBIOA = false;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        $scope.loaddata = function () {

            apiService.get("BBKVCustomReport/getdetails/2").then(function (promise) {

                $scope.yearDropdown = promise.accyear;
                //$scope.classlist = promise.accclass;
                //$scope.seclist = promise.accsection;
                // $scope.std_name = false;

            });
        }

        $scope.ShowHideup = function () {
            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        }

        $scope.ShowHidedown = function () {
            $scope.IsHiddendown = $scope.IsHiddendown ? false : true;
        }

        $scope.onclickregnoname = function () {

            var data = {
                "admnoorname": $scope.admname,
                "ASMAY_Id": $scope.asmaY_Id,
                "tctemporper": $scope.tctempper,
            }

            apiService.create("BBKVCustomReport/getnameregno", data).
                then(function (promise) {
                    if (promise.studentlist != null && promise.studentlist.length > 0) {
                        $scope.studentDropdown = promise.studentlist;
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
                    }

                })
        };

        $scope.stdnamechange = function () {
            var data = {
                "AMST_Id": $scope.amsT_Id,
                "ASMAY_Id": $scope.asmaY_Id,
            }

            apiService.create("BBKVCustomReport/stdnamechange", data).then
                (function (promise) {
                    if (promise.classsecregno != null || promise.classsecregno.length > 0) {
                        $scope.lblclassname = promise.classsecregno[0].asmcL_ClassName;
                        $scope.lblsectioname = promise.classsecregno[0].asmC_SectionName;
                        $scope.lblregno = promise.classsecregno[0].amsT_RegistrationNo;
                    }
                    else {
                        swal("No Records Found")
                    }
                })
        }

        $scope.onclicktcperortemo = function () {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "admnoorname": $scope.admname,
                "tctemporper": $scope.tctempper,
                "admnoorname": $scope.admname,
            }
            apiService.create("BBKVCustomReport/onclicktcperortemo", data).
                then(function (promise) {
                    if (promise.studentlist != null && promise.studentlist.length > 0) {
                        $scope.studentDropdown = promise.studentlist;
                        $scope.std_name = true;
                        $scope.clear_flag = true;
                        $scope.report_flag = true;

                    }
                    else {
                        swal("Student is not available for this selection","","cancel");
                        // $scope.admname = "";
                        $scope.report = false;
                        $scope.std_name = false;
                        $scope.submitted = false;

                    }
                })
        }




        $scope.Clearid = function () {
            $state.reload();
            $scope.admname = "";
            $scope.asmaY_Id = "";
            $scope.ASMCL_Id = "";
            $scope.ASMC_Id = "";
            $scope.report = false;
            $scope.amsT_Id = "";
            $scope.print = false;
            $scope.submitted = false;
            $scope.Print_flag = false;
            $scope.IsHiddendown = false;
            $scope.logo_required = "";   //added on 28 feb by sudeep
            $scope.compname = "";        //added on 28 feb by sudeep     
            $scope.myform.$setPristine();
            $scope.myform.$setUntouched();
        };

        $scope.submitted = false;

        $scope.savetmpldata = function () {
            $scope.tcpdf = false;
            if ($scope.myform.$valid) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMST_Id": $scope.amsT_Id,
                    "tctemporper": $scope.tctempper,
                }

                apiService.create("BBKVCustomReport/getTcdetails", data)
                    .then(function (promise) {
                        $scope.mi_idd = promise.academicList1[0].mI_Id;
                        if (promise.studentTCList != null) {

                            var datafrom = new Date(promise.academicList1[0].asmaY_From_Date);
                            var datato = new Date(promise.academicList1[0].asmaY_To_Date);

                            if (promise.academicList1[0].mI_Id == '14') {
                                $scope.cvtc = true;
                                $scope.tcpdf = false;
                                $scope.huvama = false;

                            }

                            else if (promise.academicList1[0].mI_Id == '8') {
                                $scope.cvtc = false;
                                $scope.tcpdf = false;
                                $scope.huvama = true;

                            }
                            else if (promise.academicList1[0].mI_Id == '15') {
                                $scope.cvtc = false;
                                $scope.tcpdf = false;
                                $scope.huvama = false;
                                $scope.SBIOA = true;

                            }
                            else {
                                swal("Tc Not Available ! ");
                                return;

                            }

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

                            $scope.todaydate = new Date();
                            $scope.Print_flag = true;
                            $scope.IsHiddendown = true;
                            $scope.report = true;
                            $scope.tcno = promise.studentTCList[0].astC_TCNO;
                            $scope.admno = promise.studentTCList[0].amsT_AdmNo;
                            $scope.lblstdname = promise.studentTCList[0].StudentName;
                            $scope.lblfathername = promise.studentTCList[0].amsT_FatherName;
                            $scope.lblmothername = promise.studentTCList[0].amsT_MotherName;
                            $scope.lblreligioncaste = promise.studentTCList[0].religion;
                            $scope.lblnationality = promise.studentTCList[0].nationality,
                                $scope.lblpobirth = promise.studentTCList[0].amsT_BirthPlace,
                                $scope.lbldob = promise.studentTCList[0].amsT_DOB;
                            $scope.lbldobwords = promise.studentTCList[0].amsT_DOB_Words;
                            $scope.lbllastschool = promise.masterCompany[0].companyname;
                            $scope.last_date_attended = promise.studentTCList[0].astC_LastAttendedDate;
                            $scope.lbldoa = promise.studentTCList[0].amsT_Date;
                            $scope.lblelectives = promise.studentTCList[0].electivestudied;
                            $scope.lblprogress = "Satisfactory";
                            $scope.lblconduct = promise.studentTCList[0].astC_Conduct;
                            $scope.lblPromotion = promise.studentTCList[0].qualificlass;
                            $scope.lbldol = promise.studentTCList[0].astC_TCIssueDate;
                            $scope.lblstudyingsince = "";
                            $scope.lblschoolbegins = $scope.frommonth1 + '-' + $scope.fromyear + ' ' + 'to' + $scope.tomonth1 + '-' + $scope.toyear;
                            $scope.lblallsumsfee = "";
                            $scope.lblreasonleaving = promise.studentTCList[0].astC_LeavingReason;
                            $scope.lblfeespaid = promise.studentTCList[0].astC_FeePaid;
                            $scope.lbldate = promise.studentTCList[0].astC_TCIssueDate;
                            $scope.studcaste = promise.studentTCList[0].caste;
                            $scope.last_Class_Studied = promise.studentTCList[0].last_Class_Studied;
                            $scope.amsT_Sex = promise.studentTCList[0].amsT_Sex;
                            $scope.astC_Qual_PromotionFlag = promise.studentTCList[0].astC_Qual_PromotionFlag;
                            $scope.regno = promise.studentTCList[0].amsT_RegistrationNo;
                            $scope.firstlang = promise.studentTCList[0].FirstLanguage;
                            $scope.secondlang = promise.studentTCList[0].SecondLanguage;
                            $scope.thirdlang = promise.studentTCList[0].Thirdlanguage;
                            $scope.govtadmno = promise.studentTCList[0].govtadmno;
                            $scope.scholership = promise.studentTCList[0].astC_Scholarship;
                            $scope.noof_daysattended = promise.studentTCList[0].noof_daysattended;
                            $scope.language_studied = promise.studentTCList[0].name;
                            $scope.electivestudied = promise.studentTCList[0].electivestudied;
                            $scope.mediumOfInstruction = promise.studentTCList[0].mediumOfInstruction;
                            $scope.medicallyexamined = promise.studentTCList[0].medicallyexamined;


                            if ($scope.ilang == undefined || $scope.ilang == "") {
                                $scope.langustudies = promise.studentTCList[0].langustudies;
                            }


                            else {
                                $scope.langustudies = $scope.ilang;
                            }
                            if ($scope.iilang == undefined || $scope.iilang == "") {
                                $scope.electivestudies = promise.studentTCList[0].electivestudies;
                            }
                            else {
                                $scope.electivestudies = $scope.iilang;
                            }

                            $scope.feedue = promise.studentTCList[0].feedue;
                            $scope.ASTC_TCApplicationDate = promise.studentTCList[0].astC_TCApplicationDate;
                            $scope.schecasteortribe = $scope.scgcaste;
                            $scope.monthyear = $scope.monthyearf;
                            $scope.exam = $scope.examf;
                        }
                        else {
                            swal("Select any Student");
                        }
                    })

            }
            else {
                $scope.submitted = true;
            }

        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };



        //$scope.printToCart = function () {

        //    var innerContents = document.getElementById('BBKVTC111').innerHTML;
        //    var popupWinindow = window.open('');
        //    popupWinindow.document.open();
        //    popupWinindow.document.write('<html><head>' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
        //           '<link type="text/css" media="print" rel="stylesheet" href="css/print/BBKV/BBKVTC/BPUTCPdf.css" />' +
        //      '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
        //    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
        //    popupWinindow.document.close();
        //}


        //$scope.printToCart = function () {

        //    var innerContents = document.getElementById('BBKVTC111').innerHTML;
        //    var popupWinindow = window.open('');
        //    popupWinindow.document.open();
        //    popupWinindow.document.write('<html><head>' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/hutchings/TCCustomReport/TCCustomReportPdf.css" />' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
        //        '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
        //    popupWinindow.document.close();

        //};
        $scope.printToCart = function () {

            if ($scope.mi_idd == 14) {

                var innerContents = document.getElementById('cvtfffc').innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/hutchings/TCCustomReport/TCCustomReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();

            }
            else if ( $scope.mi_idd == 8){
                var innerContents = document.getElementById('huvamatc').innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/hutchings/TCCustomReport/TCCustomReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
            else if ($scope.mi_idd == 15) {
                var innerContents = document.getElementById('SBIOA').innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    //'<link type="text/css" media="print" rel="stylesheet" href="css/print/hutchings/TCCustomReport/TCCustomReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
            else {
                var innerContents = document.getElementById('BBKVTC111').innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/hutchings/TCCustomReport/TCCustomReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();

            }
            
        }


        //class name from class id
        $scope.SwitchFuction = function (sno) {
            switch (sno) {
                case 'I':
                    data1 = "FIRST"
                    break;
                case 'II':
                    data1 = "SECOND"
                    break;
                case 'III':
                    data1 = "THIRD"
                    break;
                case 'IV':
                    data1 = "FOURTH"
                    break;
                case 'V':
                    data1 = "FIFTH"
                    break;
                case 'VI':
                    data1 = "SIXTH"
                    break;
                case 'VII':
                    data1 = "SEVENTH"
                    break;
                case 'VIII':
                    data1 = "EIGHTH"
                    break;
                case 'IX':
                    data1 = "NINTH"
                    break;
                case 'X':
                    data1 = "TENTH"
                    break;
                default:
                    data1 = ""
                    break;
            }
            return data1;
        };

        //month names
        $scope.getmontnames = function (monthid) {
            frommonth = "";
            switch (monthid) {

                case 0:
                    frommonth = "JANUARY"
                    break;
                case 1:
                    frommonth = "FEBRUARY"
                    break;
                case 2:
                    frommonth = "MARCH"
                    break;
                case 3:
                    frommonth = "APRIL"
                    break;
                case 4:
                    frommonth = "MAY"
                    break;
                case 5:
                    frommonth = "JUNE"
                    break;
                case 6:
                    frommonth = "JULY"
                    break;
                case 7:
                    frommonth = "AUGUST"
                    break;
                case 8:
                    frommonth = "SEPTEMBER"
                    break;
                case 9:
                    frommonth = "OCTOBER"
                    break;
                case 10:
                    frommonth = "NOVEMBER"
                    break;
                case 11:
                    frommonth = "DECEMBER"
                    break;
                default:
                    frommonth = ""
                    break;
            }
            return frommonth;
        }

    }
})();
