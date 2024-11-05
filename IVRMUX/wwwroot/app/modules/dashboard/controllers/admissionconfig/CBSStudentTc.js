(function () {
    'use strict';
    angular.module('app').controller('CBSStudentTc', CBSStudentTc)
    CBSStudentTc.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function CBSStudentTc($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {
        $scope.tcshow = false;
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

        //$scope.imgname = logopath;

        $scope.loaddata = function () {
            apiService.get("studenttccustomreport/getdetails/2").then(function (promise) {
                $scope.yearDropdown = promise.accyear;
                $scope.MI_ID = promise.accyear[0].mI_Id
                var logopath = "";
                if ($scope.MI_ID == 39) {
                    logopath = "https://calcuttaboy.blob.core.windows.net/files/Logo_Main_Campus.jpg"
                }
                else if ($scope.MI_ID == 40) {
                    logopath = "https://dcampusstrg.blob.core.windows.net/files/17/ISM_Attachments/f6c4aa09-bac8-4ba8-b6e3-88edc7f32f74.jpg"
                }
                else if ($scope.MI_ID == 38) {
                    logopath = "https://dcampusstrg.blob.core.windows.net/files/17/ISM_Attachments/c38c66c3-d64e-4e4c-9056-c5de4d9b51d9.jpg"
                }
                else {
                    logopath = "https://dcampusstrg.blob.core.windows.net/files/17/ISM_Attachments/d551a44b-d3b5-4586-b444-0d87d2064e52.jpg"
                }
                $scope.imgname = logopath;
                $scope.classlist = promise.accclass;
                $scope.seclist = promise.accsection;
            });
        };
        $scope.ShowHideup = function () {
            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        };
        $scope.ShowHidedown = function () {
            $scope.IsHiddendown = $scope.IsHiddendown ? false : true;
        };

        //  ===================ALL and Individual
        $scope.changeyear = function () {
            $scope.studentDropdown = [];
            $scope.AMST_Id = "";
            $scope.Print_flag = false;
            $scope.report = false;
            $scope.ASMS_Id = "";
            $scope.ASMCL_Id = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "tctemporper": $scope.temporper,
                "report_type": $scope.reporttype
            };

            apiService.create("studenttccustomreport/changeyear", data).then(function (promise) {

                if ($scope.reporttype === "All") {
                    if (promise.studentlist !== null && promise.studentlist.length > 0) {
                        $scope.studentDropdown = promise.studentlist;
                        $scope.stu_name_flag = true;
                        $scope.clear_flag = true;
                        $scope.report_flag = true;
                    }
                    else {
                        swal("Student is not available for this selection");
                        $scope.report = false;
                        $scope.std_name = false;
                        $scope.submitted = false;
                    }
                }
                $scope.classlist = promise.accclass;

            });
        };

        $scope.changeclass = function () {
            $scope.studentDropdown = [];
            $scope.AMST_Id = "";
            $scope.Print_flag = false;
            $scope.report = false;
            $scope.ASMS_Id = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "tctemporper": $scope.temporper,
                "report_type": $scope.reporttype,
                "ASMCL_Id": $scope.ASMCL_Id
            };

            apiService.create("studenttccustomreport/changeclass", data).then(function (promise) {
                $scope.seclist = promise.accsection;
            });
        };
        $scope.changesection = function () {
            $scope.studentDropdown = [];
            $scope.AMST_Id = "";
            $scope.Print_flag = false;
            $scope.report = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "tctemporper": $scope.temporper,
                "report_type": $scope.reporttype,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            };

            apiService.create("studenttccustomreport/changesection", data).then(function (promise) {
                if (promise.studentlist !== null && promise.studentlist.length > 0) {
                    $scope.studentDropdown = promise.studentlist;
                    $scope.stu_name_flag = true;
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

        $scope.submitted = false;

        $scope.savetmpldata = function () {
            $scope.tcshow = true;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMST_Id": $scope.AMST_Id.amsT_Id,
                "tctemporper": $scope.temporper
            };
            apiService.create("studenttccustomreport/getTcdetails", data).then(function (promise) {
                if (promise.studentTCList.length > 0) {
                    var datafrom = new Date(promise.academicList1[0].asmaY_From_Date);
                    var datato = new Date(promise.academicList1[0].asmaY_To_Date);

                    angular.forEach($scope.yearDropdown, function (year) {
                        if (year.asmaY_Id == $scope.asmaY_Id) {
                            $scope.currentyear = year.asmaY_Year;
                        }
                    })

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
                    if (promise.studentTCList[0].amsT_Sex = "Male") {
                        $scope.abbrivation = "Master. "
                    }
                    else {
                        $scope.abbrivation = "Mrs."

                    }
                    $scope.lblreligioncaste = promise.studentTCList[0].religion;
                    $scope.lblnationality = promise.studentTCList[0].nationality;
                    $scope.lblpobirth = promise.studentTCList[0].amsT_BirthPlace;
                    $scope.lbldob = promise.studentTCList[0].amsT_DOB;
                    $scope.lbldobwords = promise.studentTCList[0].amsT_DOB_Words;
                    $scope.lbllastschool = promise.masterCompany[0].companyname;
                    $scope.last_date_attended = promise.studentTCList[0].astC_LastAttendedDate;
                    $scope.lblconduct = promise.studentTCList[0].astC_Conduct;
                    $scope.lbldoa = promise.studentTCList[0].amsT_Date;
                    $scope.lblprogress = "Satisfactory";
                    $scope.lblPromotion = promise.studentTCList[0].qualificlass;
                    $scope.todaydate = promise.studentTCList[0].astC_TCIssueDate;
                    $scope.lblstudyingsince = "";
                    $scope.lblschoolbegins = $scope.frommonth1 + '-' + $scope.fromyear + ' ' + 'to' + $scope.tomonth1 + '-' + $scope.toyear;

                    $scope.lblallsumsfee = "";
                    $scope.lblreasonleaving = promise.studentTCList[0].astC_LeavingReason;
                    $scope.lbldate = promise.studentTCList[0].astC_TCIssueDate;
                    $scope.studcaste = promise.studentTCList[0].caste;
                    $scope.astC_FeePaid = promise.studentTCList[0].astC_FeePaid;
                    if ($scope.astC_FeePaid == "No") {
                        $scope.astC_FeePaid = "Not Paid";
                    }
                    else {
                        $scope.astC_FeePaid = "Paid";
                    }
                    $scope.tcdatess = promise.studentTCList[0].tcdatess;
                    $scope.laststudied = promise.studentTCList[0].Last_Class_Studied;
                    $scope.asmC_SectionName = promise.studentTCList[0].asmC_SectionName;
                    $scope.SwitchFuction($scope.laststudied);
                    $scope.laststudiedinwords = data1;
                    $scope.ASTC_Remarks = promise.studentTCList[0].astC_Remarks;
                    $scope.subjectsstudied = promise.studentTCList[0].langustudies;
                    $scope.elective = promise.studentTCList[0].electivestudies;
                    $scope.streamname = promise.studentTCList[0].ASMST_StreamName;
                    $scope.image = promise.studentTCList[0].logo;


                    if ($scope.elective !== null && $scope.elective.length > 0) {
                        $scope.elective = ',' + $scope.elective;
                    }
                    //$scope.lblPromotion = promise.studentTCList[0].ASTC_Qual_PromotionFlag;
                    $scope.feedue = promise.studentTCList[0].feedue;
                    $scope.feeconcession = promise.studentTCList[0].feeconcession;
                    $scope.totalworkingdays = promise.studentTCList[0].totalworkingdays;
                    $scope.noworkingdays = promise.studentTCList[0].noworkingdays;
                    angular.forEach($scope.class, function (aa) {
                        if (parseInt(aa.asmcL_Id) === parseInt($scope.asmcL_Id)) {
                            $scope.promotedclass = aa.asmcL_ClassName;
                            $scope.SwitchFuction($scope.promotedclass);
                            $scope.promotedclassinfig = data1;
                        }
                    });
                    $scope.lastpaiddate = promise.lastpaiddate;
                    if ($scope.lastpaiddate !== null && $scope.lastpaiddate !== undefined) {
                        $scope.lastpaidmonth = $scope.lastpaiddate;
                    }

                    $scope.previousschool = promise.previousschool;
                    if ($scope.previousschool !== null && $scope.previousschool.length > 0) {
                        $scope.previousschoolname = $scope.previousschool[0].amstpS_PrvSchoolName;
                    } else {
                        $scope.previousschoolname = "";
                    }

                    $scope.studenttcdetails = promise.studenttcdetails;

                    $scope.lbldol = $scope.studenttcdetails[0].astC_TCDate;
                }
                else {
                    swal("Select any Student");
                }
            });
        };

        $scope.Clearid = function () {
            $state.reload();
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.printdd = function (printSectionId) {
            $scope.printToCart($scope.printSectionId);
        };
        //=====================class name from class id
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
                case 'Nursery':
                    data1 = "Nursery";
                    break;
                case 'Transition':
                    data1 = "Transition";
                    break;
                default:
                    data1 = "";
                    break;
            }
            return data1;
        };

        $scope.SwitchFuction1 = function (sno) {
            switch (sno) {
                case 'I':
                    data1 = "First";
                    break;
                case 'II':
                    data1 = "Second";
                    break;
                case 'III':
                    data1 = "Third";
                    break;
                case 'IV':
                    data1 = "Fourth";
                    break;
                case 'V':
                    data1 = "Fifth";
                    break;
                case 'VI':
                    data1 = "Sixth";
                    break;
                case 'VII':
                    data1 = "Seventh";
                    break;
                case 'VIII':
                    data1 = "Eighth";
                    break;
                case 'IX':
                    data1 = "Ninth";
                    break;
                case 'X':
                    data1 = "Tenth";
                    break;
                case 'XI':
                    data1 = "Eleventh";
                    break;
                case 'Nursery':
                    data1 = "Nursery";
                    break;
                case 'Transition':
                    data1 = "Transition";
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

        $scope.getmontnames1 = function (monthid) {
            frommonth = "";
            switch (monthid) {

                case 0:
                    frommonth = "January-";
                    break;
                case 1:
                    frommonth = "February-";
                    break;
                case 2:
                    frommonth = "March-";
                    break;
                case 3:
                    frommonth = "April-";
                    break;
                case 4:
                    frommonth = "May-";
                    break;
                case 5:
                    frommonth = "June-";
                    break;
                case 6:
                    frommonth = "July-";
                    break;
                case 7:
                    frommonth = "August-";
                    break;
                case 8:
                    frommonth = "September-";
                    break;
                case 9:
                    frommonth = "October-";
                    break;
                case 10:
                    frommonth = "November-";
                    break;
                case 11:
                    frommonth = "December-";
                    break;
                default:
                    frommonth = "";
                    break;
            }
            return frommonth;
        };

        $scope.uppercase = function (str) {
            var array1 = str.split(' ');
            var newarray1 = [];

            for (var x = 0; x < array1.length; x++) {
                newarray1.push(array1[x].charAt(0).toUpperCase() + array1[x].slice(1).toLowerCase());
            }
            $scope.stuDobwords = newarray1;
            return $scope.stuDobwords = newarray1.join(' ');
        };

        // ============print data
        $scope.printToCart = function () {
            var innerContents = document.getElementById('Stjames').innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            //popupWinindow.document.write('<html><head>' +
            //    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
            //    '<link type="text/css" media="print" rel="stylesheet" href="css/print/StJames/Stjamestcpdf.css" />' +
            //    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
            //    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            //popupWinindow.document.close();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/InvoicePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
    }
})();
