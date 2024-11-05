(function () {
    'use strict';
    angular.module('app').controller('CollegeBWMCTCCertificateMethodist', CollegeBWMCTCCertificateMethodist)

    CollegeBWMCTCCertificateMethodist.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function CollegeBWMCTCCertificateMethodist($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {
        var data1;
        var frommonth;
        var tomonth;
        $scope.tcpdf = false;

        $scope.loaddata = function () {
            apiService.get("CollegeStudentTCReport/getalldetails/2").then(function (promise) {
                $scope.yearDropdown = promise.getyear;
            });
        };

        $scope.onchangeyear = function () {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "tctemporper": $scope.tctempper
            };

            apiService.create("CollegeStudentTCReport/onchangeyeartc", data).
                then(function (promise) {
                    if (promise.getstudent !== null && promise.getstudent.length > 0) {
                        $scope.studentDropdown = promise.getstudent;
                        $scope.std_name = true;
                        $scope.clear_flag = true;
                        $scope.report_flag = true;
                    }
                    else {
                        swal("Student is not available for this selection");
                        $scope.report = false;
                        $scope.std_name = false;
                        $scope.submitted = false;
                    }
                });
        };

        $scope.stdnamechange = function () {
            var data = {
                "AMCST_Id": $scope.AMCST_Id.amcsT_Id,
                "ASMAY_Id": $scope.asmaY_Id,
                "tctemporper": $scope.tctempper
            };

            apiService.create("CollegeStudentTCReport/stdnamechange", data).then
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

        $scope.onclicktcperortemo = function () {

            $scope.asmaY_Id = "";
            $scope.AMCST_Id = "";
            $scope.studentDropdown = [];
            $scope.tcpdf = false;

        };

        $scope.ShowHideup = function () {
            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        };

        $scope.ShowHidedown = function () {
            $scope.IsHiddendown = $scope.IsHiddendown ? false : true;
        };

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.submitted = false;

        $scope.savetmpldata = function () {
            $scope.tcpdf = false;
            if ($scope.myform.$valid) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMCST_Id": $scope.AMCST_Id.amcsT_Id,
                    "tctemporper": $scope.tctempper
                };

                apiService.create("CollegeStudentTCReport/getTcdetails", data).then(function (promise) {
                    if (promise.getstudentdetails !== null) {
                        $scope.tcpdf = true;

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

                        $scope.todaydate = new Date();
                        $scope.Print_flag = true;
                        $scope.IsHiddendown = true;
                        $scope.report = true;
                        $scope.tcno = promise.getstudentdetails[0].ACSTC_TCNO;
                        $scope.admno = promise.getstudentdetails[0].AMCST_AdmNo;
                        $scope.lblstdname = promise.getstudentdetails[0].studentname;

                        $scope.lblfathername = promise.getstudentdetails[0].AMCST_FatherName;
                        $scope.lblmothername = promise.getstudentdetails[0].AMCST_MotherName;
                        $scope.lblreligioncaste = promise.getstudentdetails[0].religion;
                        $scope.lblnationality = promise.getstudentdetails[0].nationality;
                        $scope.lblpobirth = promise.getstudentdetails[0].AMCST_BirthPlace;
                        $scope.lbldob = promise.getstudentdetails[0].AMCST_DOB;
                        $scope.lbldobwords = promise.getstudentdetails[0].AMCST_DOBin_words;
                        $scope.lbllastschool = promise.masterCompany[0].companyname;
                        $scope.last_date_attended = promise.getstudentdetails[0].ACSTC_LastAttendedDate;
                        $scope.lbldoa = promise.getstudentdetails[0].AMCST_Date;
                        $scope.last_school_attended = promise.getstudentdetails[0].ACSTC_TCIssueDate;
                        $scope.stuLcs = promise.getstudentdetails[0].AMSE_SEMName;
                        $scope.lblPromotion = promise.getstudentdetails[0].astC_Qual_PromotionFlag;



                        $scope.lblprogress = "Satisfactory";
                        $scope.lblconduct = promise.getstudentdetails[0].ACSTC_Conduct;
                        $scope.ACSTC_Qual_Course = promise.getstudentdetails[0].ACSTC_Qual_Course;
                        $scope.sem = "Sem";
                        $scope.lbldol = promise.getstudentdetails[0].ACSTC_TCIssueDate;
                        $scope.lblstudyingsince = "";
                        // $scope.lblschoolbegins = $scope.frommonth1 + '-' + $scope.fromyear + ' ' + 'to' + $scope.tomonth1 + '-' + $scope.toyear;
                        $scope.lblallsumsfee = "";
                        $scope.lblreasonleaving = promise.getstudentdetails[0].ACSTC_LeavingReason;
                        $scope.lbldate = promise.getstudentdetails[0].ACSTC_TCIssueDate;
                        $scope.studcaste = promise.getstudentdetails[0].caste;
                        $scope.last_Class_Studied = promise.getstudentdetails[0].last_Class_Studied;
                        $scope.amsT_Sex = promise.getstudentdetails[0].AMCST_Sex;
                        $scope.astC_Qual_PromotionFlag = promise.getstudentdetails[0].ACSTC_Qual_PromotionFlag;
                        $scope.regno = promise.getstudentdetails[0].AMCST_RegistrationNo;
                        $scope.govtadmno = promise.getstudentdetails[0].AMCST_RegistrationNo;
                        if ($scope.ilang === undefined || $scope.ilang === "") {
                            $scope.langustudies = promise.getstudentdetails[0].langustudies;
                        }
                        else {
                            $scope.langustudies = $scope.ilang;
                        }
                        if ($scope.iilang === undefined || $scope.iilang === "") {
                            $scope.electivestudies = promise.getstudentdetails[0].electivestudies;
                        }
                        else {
                            $scope.electivestudies = $scope.iilang;
                        }

                        $scope.feedue = promise.getstudentdetails[0].feedue;
                        $scope.ASTC_TCApplicationDate = promise.getstudentdetails[0].ACSTC_TCApplicationDate;
                        $scope.schecasteortribe = $scope.scgcaste;
                        $scope.monthyear = $scope.monthyearf;
                        $scope.exam = $scope.examf;
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

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.printToCart = function () {

            var innerContents = document.getElementById("printSectionId2").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/baldwin/BWMC/BGMCTCcertificatePdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();


        }

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
                    data1 = "";
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
