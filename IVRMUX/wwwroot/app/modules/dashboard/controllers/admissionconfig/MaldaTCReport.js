(function () {
    'use strict';
    angular.module('app').controller('MaldaTCReportController', MaldaTCReportController)
    MaldaTCReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function MaldaTCReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {
        var data1;
        var frommonth;
        var tomonth;


        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;



        //$scope.report = false;
        $scope.loaddata = function () {

            apiService.get("BBKVCustomReport/getdetails/2").then(function (promise) {

                $scope.yearDropdown = promise.accyear;
                $scope.class = promise.accclass;
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

            apiService.create("BBKVCustomReport/getnameregno", data).
                then(function (promise) {
                    if (promise.studentlist !== null && promise.studentlist.length > 0) {
                        $scope.studentDropdown = promise.studentlist;
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
                "AMST_Id": $scope.amsT_Id,
                "ASMAY_Id": $scope.asmaY_Id
            };

            apiService.create("BBKVCustomReport/stdnamechange", data).then
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

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "admnoorname": $scope.admname,
                "tctemporper": $scope.tctempper
            };

            apiService.create("BBKVCustomReport/onclicktcperortemo", data).
                then(function (promise) {
                    if (promise.studentlist !== null && promise.studentlist.length > 0) {
                        $scope.studentDropdown = promise.studentlist;
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

            if ($scope.myform.$valid) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMST_Id": $scope.amsT_Id,
                    "tctemporper": $scope.tctempper
                };

                apiService.create("BBKVCustomReport/getTcdetails", data)
                    .then(function (promise) {

                        if (promise.studentTCList.length > 0) {

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
                            // $scope.last_date_attended = promise.studentTCList[0].astC_LastAttendedDate;

                            $scope.lbldoa = promise.studentTCList[0].amsT_Date;
                            $scope.lblprogress = "Satisfactory";
                            $scope.lblconduct = promise.studentTCList[0].astC_Conduct;
                            $scope.lblPromotion = promise.studentTCList[0].qualificlass;
                            $scope.lbldol = promise.studentTCList[0].astC_TCIssueDate;
                            $scope.lblstudyingsince = "";
                            $scope.lblschoolbegins = $scope.frommonth1 + '-' + $scope.fromyear + ' ' + 'to' + $scope.tomonth1 + '-' + $scope.toyear;
                            $scope.lblallsumsfee = "";
                            $scope.lblreasonleaving = promise.studentTCList[0].astC_LeavingReason;
                            $scope.lbldate = promise.studentTCList[0].astC_TCIssueDate;
                            $scope.studcaste = promise.studentTCList[0].caste;
                            $scope.laststudied = promise.studentTCList[0].last_Class_Studied;
                            $scope.SwitchFuction($scope.laststudied);
                            $scope.laststudiedinwords = data1;

                            $scope.ASTC_Remarks = promise.studentTCList[0].astC_Remarks;
                            $scope.subjectsstudied = promise.studentTCList[0].langustudies;
                            $scope.applicationdate = new Date(promise.studentTCList[0].astC_TCApplicationDate);


                            $scope.promotionflag = promise.studentTCList[0].promotedflag;
                            $scope.feedue = promise.studentTCList[0].feedue;
                            $scope.feeconcession = promise.studentTCList[0].feeconcession;
                            $scope.totalworkingdays = promise.studentTCList[0].totalworkingdays;
                            $scope.noworkingdays = promise.studentTCList[0].noworkingdays;
                            $scope.classjoined = promise.studentTCList[0].classjoinname;

                            if (promise.lastpaiddate !== null && promise.lastpaiddate != "") {
                                $scope.lastdatepaid = promise.lastpaiddate;
                            }



                            angular.forEach($scope.class, function (aa) {
                                if (parseInt(aa.asmcL_Id) === parseInt($scope.asmcL_Id)) {
                                    $scope.promotedclass = aa.asmcL_ClassName;
                                    $scope.SwitchFuction($scope.promotedclass);
                                    $scope.promotedclassinfig = data1;
                                }
                            });

                            $scope.elective = promise.studentTCList[0].electivestudies;
                            if ($scope.elective.length > 0) {
                                $scope.elective = ',' + $scope.elective;
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

        $scope.interacted = function (field) {
            return $scope.submitted;
        };



        $scope.printToCart = function () {
            var innerContents = document.getElementById('printSectionIdgirls').innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/StMaryMalda/MaldaStudentTcPdf.css" />' +
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
