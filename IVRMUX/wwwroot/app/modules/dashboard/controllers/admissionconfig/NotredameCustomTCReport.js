(function () {
    'use strict';
    angular.module('app').controller('NotredameCustomTCReportController', NotredameCustomTCReportController)

    NotredameCustomTCReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function NotredameCustomTCReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {
        var data1;
        var frommonth;
        var tomonth;
        $scope.report = false;
        $scope.Print_flag = false;

        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

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

            if ($scope.asmaY_Id !== undefined && $scope.asmaY_Id !== null && $scope.asmaY_Id !== "") {
                $scope.studentDropdown = [];
                $scope.AMST_Id = "";
                $scope.Print_flag = false;
                $scope.report = false;

                var data = {
                    "admnoorname": $scope.admname,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "tctemporper": $scope.tctempper
                };

                apiService.create("BBKVCustomReport/getnameregno", data).then(function (promise) {
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
            }
        };

        $scope.stdnamechange = function () {
            $scope.report = false;
            var data = {
                "AMST_Id": $scope.AMST_Id.amsT_Id,
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
            $scope.report = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "admnoorname": $scope.admname,
                "tctemporper": $scope.tctempper
            };
            apiService.create("BBKVCustomReport/onclicktcperortemo", data).then(function (promise) {
                if (promise.studentlist !== null && promise.studentlist.length > 0) {
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
            $scope.report = false;
            if ($scope.myform.$valid) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMST_Id": $scope.AMST_Id.amsT_Id,
                    "tctemporper": $scope.tctempper
                };

                apiService.create("BBKVCustomReport/getTcdetails", data).then(function (promise) {
                    if (promise.studentTCList !== null && promise.studentTCList.length > 0) {
                        $scope.report = true;
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
                        $scope.classnamejoin = promise.classnamejoin;

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
                        $scope.elective = promise.studentTCList[0].electivestudies;
                        if ($scope.elective !== null && $scope.elective.length > 0) {
                            $scope.elective = ',' + $scope.elective;
                        }

                        $scope.promotionflag = promise.studentTCList[0].promotedflag;
                        $scope.feedue = promise.studentTCList[0].feedue;
                        $scope.feeconcession = promise.studentTCList[0].feeconcession;
                        $scope.totalworkingdays = promise.studentTCList[0].totalworkingdays;
                        $scope.noworkingdays = promise.studentTCList[0].noworkingdays;

                        //angular.forEach($scope.class, function (aa) {
                        //    if (parseInt(aa.asmcL_Id) === parseInt($scope.asmcL_Id)) {
                        //        $scope.promotedclass = aa.asmcL_ClassName;
                        //        $scope.SwitchFuction($scope.promotedclass);
                        //        $scope.promotedclassinfig = data1;
                        //    }
                        //});


                        if ($scope.laststudied == 'X') {
                            $scope.promotedclass = promise.studentTCList[0].qualificlass;
                            $scope.SwitchFuction($scope.promotedclass);
                            $scope.promotedclassinfig = data1;
                        }
                        else {
                            $scope.promotedclass = aa.asmcL_ClassName;
                            $scope.SwitchFuction($scope.promotedclass);
                            $scope.promotedclassinfig = data1;
                        }

                        $scope.lastpaiddate = promise.lastpaiddate;

                        if ($scope.lastpaiddate !== null && $scope.lastpaiddate !== undefined) {
                            $scope.lastpaidmonth = $scope.lastpaiddate;
                        }

                        $scope.studenttcdetails = promise.studenttcdetails;
                        $scope.getadm_m_student_details = promise.getadm_m_student_details;

                        $scope.stsno = $scope.getadm_m_student_details[0].amsT_BPLCardNo;
                        $scope.lblbirthcertificateno = $scope.getadm_m_student_details[0].amsT_BirthCertNO;

                        $scope.nccdetais = $scope.studenttcdetails[0].astC_NCCDetails;
                        $scope.gamesplayed = $scope.studenttcdetails[0].astC_ExtraActivities;
                        $scope.lblapplicationdate = $scope.studenttcdetails[0].astC_TCApplicationDate;
                        $scope.lastexamdetails = $scope.studenttcdetails[0].astC_LastExamDetails;
                        $scope.ASTC_Result = $scope.studenttcdetails[0].astC_Result;
                        $scope.ASTC_Qual_PromotionFlag = $scope.studenttcdetails[0].astC_Qual_PromotionFlag;
                        $scope.subjectsstudied = $scope.studenttcdetails[0].astC_SubjectsStudied;

                        $scope.IMCC_CategoryCode = promise.studentTCList[0].schedulecaste;

                        if ($scope.IMCC_CategoryCode !== "OBC" && $scope.IMCC_CategoryCode !== "SC" && $scope.IMCC_CategoryCode !== "ST") {
                            $scope.IMCC_CategoryCode = "No";
                        }else{$scope.IMCC_CategoryCode="Yes"}

                        $scope.castecategory = promise.studentTCList[0].castecategory;
                        $scope.ASTC_ResultDetails = $scope.studenttcdetails[0].astC_ResultDetails;
                        $scope.ASMC_SectionName = promise.studentTCList[0].asmC_SectionName;

                        $scope.getsubjectsdetails = promise.getsubjectsdetails;

                        $scope.subjectsname = "";

                        if ($scope.getsubjectsdetails !== null && $scope.getsubjectsdetails.length > 0) {

                            if ($scope.getsubjectsdetails[0] !== undefined) {
                                if ($scope.getsubjectsdetails[0].ISMS_SUBJECTNAME !== null && $scope.getsubjectsdetails[0].ISMS_SUBJECTNAME !== undefined
                                    && $scope.getsubjectsdetails[0].ISMS_SUBJECTNAME !== "") {
                                    $scope.subject1 = $scope.getsubjectsdetails[0].ISMS_SUBJECTNAME;
                                    $scope.subjectsname= $scope.subject1;
                                }
                            }
                            if ($scope.getsubjectsdetails[1] !== undefined) {
                                if ($scope.getsubjectsdetails[1].ISMS_SUBJECTNAME !== null && $scope.getsubjectsdetails[1].ISMS_SUBJECTNAME !== undefined
                                    && $scope.getsubjectsdetails[1].ISMS_SUBJECTNAME !== "") {
                                    $scope.subject2 = $scope.getsubjectsdetails[1].ISMS_SUBJECTNAME;
                                    $scope.subjectsname+= "," + $scope.subject2;
                                }
                            }
                            if ($scope.getsubjectsdetails[2] !== undefined) {
                                if ($scope.getsubjectsdetails[2].ISMS_SUBJECTNAME !== null && $scope.getsubjectsdetails[2].ISMS_SUBJECTNAME !== undefined
                                    && $scope.getsubjectsdetails[2].ISMS_SUBJECTNAME !== "") {
                                    $scope.subject3 = $scope.getsubjectsdetails[2].ISMS_SUBJECTNAME;
                                    $scope.subjectsname+= "," + $scope.subject3;
                                }
                            }
                            if ($scope.getsubjectsdetails[3] !== undefined) {
                                if ($scope.getsubjectsdetails[3].ISMS_SUBJECTNAME !== null && $scope.getsubjectsdetails[3].ISMS_SUBJECTNAME !== undefined
                                    && $scope.getsubjectsdetails[3].ISMS_SUBJECTNAME !== "") {
                                    $scope.subject4 = $scope.getsubjectsdetails[3].ISMS_SUBJECTNAME;
                                    $scope.subjectsname+= "," + $scope.subject4;
                                }
                            }
                            if ($scope.getsubjectsdetails[4] !== undefined) {
                                if ($scope.getsubjectsdetails[4].ISMS_SUBJECTNAME !== null && $scope.getsubjectsdetails[4].ISMS_SUBJECTNAME !== undefined
                                    && $scope.getsubjectsdetails[4].ISMS_SUBJECTNAME !== "") {
                                    $scope.subject5 = $scope.getsubjectsdetails[4].ISMS_SUBJECTNAME;
                                    $scope.subjectsname+= "," + $scope.subject5;
                                }
                            }
                            if ($scope.getsubjectsdetails[5] !== undefined) {
                                if ($scope.getsubjectsdetails[5].ISMS_SUBJECTNAME !== null && $scope.getsubjectsdetails[5].ISMS_SUBJECTNAME !== undefined
                                    && $scope.getsubjectsdetails[5].ISMS_SUBJECTNAME !== "") {
                                    $scope.subject6 = $scope.getsubjectsdetails[5].ISMS_SUBJECTNAME;
                                    $scope.subjectsname += "," + $scope.subject6;
                                }
                            }
                        }

                        if ($scope.classnamejoin !== null && $scope.classnamejoin.length > 0) {
                            $scope.classjoined = $scope.classnamejoin[0].classjoinname;
                        }

                        if (promise.getlastpaidfeedetails !== null && promise.getlastpaidfeedetails.length > 0) {
                            $scope.getlastpaidfeedetails = promise.getlastpaidfeedetails;
                            $scope.lastdatepaid = $scope.getlastpaidfeedetails[0].FMTFHDD_ToDate;
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
            var innerContents = document.getElementById('BBKVTC').innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/BBKV/BBKVTC/BBKVTCPdf.css" />' +
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
                case 'XI':
                    data1 = "Eleventh";
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
                    frommonth = "APRIL"
                    break;
                case 4:
                    frommonth = "MAY";
                    break;
                case 5:
                    frommonth = "JUNE";
                    break;
                case 6:
                    frommonth = "JULY"
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
