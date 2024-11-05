(function () {
    'use strict';
    angular
        .module('app')
        .controller('TCcustomreportchikkatiController', TCcustomreportchikkatiController)

    TCcustomreportchikkatiController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function TCcustomreportchikkatiController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {
        var data1;
        var frommonth;
        var tomonth;
        $scope.tcpdf = false;



        $scope.loaddata = function () {

            apiService.get("BBKVCustomReport/getdetails/2").then(function (promise) {

                $scope.yearDropdown = promise.accyear;
                //$scope.classlist = promise.accclass;
                //$scope.seclist = promise.accsection;
                // $scope.std_name = false;

            });
        }

        $scope.VCISadmin = false;
        $scope.BIPINadmin = false;
        $scope.BIPINPUadmin = false;

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
                        swal("Student is not available for this selection");
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
                        if (promise.studentTCList != null) {

                            $scope.mi_idd = promise.masterCompany[0].mI_Id;
                            if (parseInt(promise.masterCompany[0].mI_Id) === 6) {
                                $scope.VCISadmin = true;
                                $scope.BCPCAdmin = false;
                                $scope.BCESAdmin = false;
                                $scope.BCPSCAdmin = false;

                            }
                            else if (parseInt(promise.masterCompany[0].mI_Id) === 8) {
                                $scope.VCISadmin = false;
                                $scope.BCPCAdmin = false;
                                $scope.BCPSCAdmin = false;
                                $scope.BCESAdmin = true;

                            }
                            else if (parseInt(promise.masterCompany[0].mI_Id) === 7) {
                                $scope.VCISadmin = false;
                                $scope.BCPCAdmin = true;
                                $scope.BCESAdmin = false;
                                $scope.BCPSCAdmin = false;

                            }
                            else if (parseInt(promise.masterCompany[0].mI_Id) === 9) {
                                $scope.VCISadmin = false;
                                $scope.BCPCAdmin = false;
                                $scope.BCESAdmin = false;
                                $scope.BCPSCAdmin = true;

                            }

                            $scope.tcpdf = true;
                            var datafrom = new Date(promise.academicList1[0].asmaY_From_Date);
                            var datato = new Date(promise.academicList1[0].asmaY_To_Date);
                            $scope.getdate = new Date();
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
                            $scope.tcno = promise.studentTCList[0].astC_TCNO;
                            $scope.admno = promise.studentTCList[0].amsT_AdmNo;
                            $scope.stuFN = promise.studentTCList[0].amsT_FirstName;
                            $scope.stuMN = promise.studentTCList[0].amsT_MiddleName;
                            $scope.stuLN = promise.studentTCList[0].amsT_LastName;
                            $scope.institutionName = promise.studentTCList[0].institutionName;
                            $scope.lblfathername = promise.studentTCList[0].amsT_FatherName;
                            $scope.getdate = promise.studentTCList[0].getdate;


                            $scope.lblmothername = promise.studentTCList[0].amsT_MotherName;
                            $scope.lblreligioncaste = promise.studentTCList[0].religion;
                            $scope.lblnationality = promise.studentTCList[0].nationality,
                                $scope.amC_Name = promise.studentTCList[0].amC_Name,
                                $scope.Languagename = promise.studentTCList[0].Languagename,

                                $scope.lblpobirth = promise.studentTCList[0].amsT_BirthPlace,
                                $scope.lbldob = promise.studentTCList[0].amsT_DOB;
                            $scope.lbldobwords = promise.studentTCList[0].amsT_DOB_Words;
                            $scope.lbllastschool = promise.masterCompany[0].companyname;
                            $scope.last_date_attended = promise.studentTCList[0].astC_LastAttendedDate;
                            $scope.lbldoa = promise.studentTCList[0].amsT_Date;
                            $scope.lblprogress = "Satisfactory";
                            $scope.lblconduct = promise.studentTCList[0].astC_Conduct;
                            $scope.noof_schooldays = promise.studentTCList[0].noof_schooldays;
                            $scope.noof_daysattended = promise.studentTCList[0].noof_daysattended;

                            $scope.lblPromotion = promise.studentTCList[0].qualificlass;
                            $scope.lbldol = promise.studentTCList[0].astC_TCIssueDate;
                            $scope.lblstudyingsince = "";
                            $scope.lblschoolbegins = $scope.frommonth1 + '-' + $scope.fromyear + ' ' + 'to' + $scope.tomonth1 + '-' + $scope.toyear;
                            $scope.lblallsumsfee = "";
                            $scope.lblreasonleaving = promise.studentTCList[0].astC_LeavingReason;
                            $scope.lbldate = promise.studentTCList[0].astC_TCIssueDate;
                            $scope.studcaste = promise.studentTCList[0].caste;
                            $scope.last_Class_Studied = promise.studentTCList[0].last_Class_Studied;
                            $scope.amsT_Sex = promise.studentTCList[0].amsT_Sex;
                            $scope.astC_Qual_PromotionFlag = promise.studentTCList[0].astC_Qual_PromotionFlag;
                            $scope.regno = promise.studentTCList[0].amsT_RegistrationNo;
                            $scope.govtadmno = promise.studentTCList[0].govtadmno;
                            $scope.amsT_Taluk = promise.studentTCList[0].amsT_Taluk;
                            $scope.District = promise.studentTCList[0].District;
                            $scope.states = promise.studentTCList[0].states;
                            $scope.medium = promise.studentTCList[0].mediumOfInstruction;
                            $scope.ASTC_FeeConcession = $scope.ASTC_FeeConcession;
                            $scope.ASTC_Scholarship = $scope.ASTC_Scholarship;
                            $scope.ASTC_FeePaid = $scope.ASTC_FeePaid;
                            $scope.govtno = $scope.govtno;


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


        $scope.printToCart = function () {

            var innerContents = document.getElementById('BBKVTC222').innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/hutchings/TCCustomReport/TCCustomReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();

        };
        //$scope.printToCart = function () {

        //    var innerContents = document.getElementById('BBKVTC222').innerHTML;
        //    var popupWinindow = window.open('');
        //    popupWinindow.document.open();
        //    popupWinindow.document.write('<html><head>' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/hutchings/TCCustomReport/TCCustomReportPdf.css" />' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
        //        '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
        //    popupWinindow.document.close();

        //};
        //$scope.printToCart = function () {

        //    var innerContents = document.getElementById('BBKVTC333').innerHTML;
        //    var popupWinindow = window.open('');
        //    popupWinindow.document.open();
        //    popupWinindow.document.write('<html><head>' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/hutchings/TCCustomReport/TCCustomReportPdf.css" />' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
        //        '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
        //    popupWinindow.document.close();

        //};

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
