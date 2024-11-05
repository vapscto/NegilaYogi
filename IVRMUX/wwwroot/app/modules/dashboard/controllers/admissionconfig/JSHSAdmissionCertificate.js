(function () {
    'use strict';
    angular.module('app').controller('JSHSAdmissionCertificateController', JSHSAdmissionCertificateController)

    JSHSAdmissionCertificateController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function JSHSAdmissionCertificateController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.mi_idd = {};
        $scope.studlist = [];
        $scope.allorindiform = false;
        $scope.print_flag = true;
        var data1;
        var frommonth;
        var tomonth;
        var datesufix;
        $scope.datesuf = {};
        $scope.datesuf1 = {};

        $scope.onpageload = function () {
            var pageid = 1;
            apiService.getURI("JSHSAdmissionCertificate/getdata", pageid).then(function (promise) {
                $scope.yearlist = promise.allAcademicYear;
                $scope.ASA_FromDate = new Date();
            });
        };

        $scope.upper_grid = function () {
            $scope.upper_grid_hide = $scope.upper_grid_hide ? false : true;
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.fillallorindi = function () {
            if ($scope.ts.allorindii === "A") {
                $scope.allorindiform = false;
            }
            else {
                $scope.allorindiform = true;
            }
            $scope.bonafide = false;
            $scope.study_cer = false;
            $scope.print_flag = true;
            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.refernceno = "";
        };

        $scope.fillstudentlist = function () {
            $scope.bonafide = false;
            $scope.study_cer = false;
            $scope.AMST_Id = "";
            $scope.studlist.length = 0;
            $scope.print_flag = true;
            $scope.refernceno = "";
        };

        $scope.certificatetype = function () {
            $scope.bonafide = false;
            $scope.study_cer = false;
            $scope.print_flag = true;
            $scope.refernceno = "";
        };

        $scope.onchangeyear = function () {
            $scope.bonafide = false;
            $scope.study_cer = false;
            $scope.AMST_Id = "";
            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.print_flag = true;
            $scope.studlist = [];
            $scope.refernceno = "";

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "allorindi": $scope.ts.allorindii,
                "AMST_SOL": $scope.ts.optradio
            };

            apiService.create("JSHSAdmissionCertificate/onchangeyear", data).then(function (Promise) {
                $scope.studlist = Promise.fillstudlist;
                $scope.classlist = Promise.allclasslist;
            });

        };

        $scope.onchangeclass = function () {
            $scope.bonafide = false;
            $scope.study_cer = false;
            $scope.AMST_Id = "";
            $scope.asmS_Id = "";
            $scope.print_flag = true;
            $scope.studlist = [];
            $scope.refernceno = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id
            };
            apiService.create("JSHSAdmissionCertificate/onchangeclass", data).then(function (promise) {
                $scope.sectionlist = promise.allsectionlist;
            });
        };

        $scope.onchangesection = function () {
            $scope.bonafide = false;
            $scope.study_cer = false;
            $scope.AMST_Id = "";
            $scope.print_flag = true;
            $scope.studlist = [];
            $scope.refernceno = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id
            };
            apiService.create("JSHSAdmissionCertificate/onchangesection", data).then(function (promise) {


            });
        };

        $scope.onchangestudent = function () {
            $scope.bonafide = false;
            $scope.study_cer = false;
            $scope.print_flag = true;
            $scope.refernceno = "";
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

        $scope.submitted = false;

        $scope.Report = function () {
            if ($scope.myForm.$valid) {

               // $scope.getEmailsendingConfirmation();

                var acedamicId = $scope.asmaY_Id;
                var sectionId = $scope.asmC_Id;
                var classId = $scope.asmcL_Id;
                var studId = $scope.AMST_Id;

                if (acedamicId === undefined || acedamicId === "") {
                    acedamicId = 0;
                }
                if (classId === undefined || classId === "") {
                    classId = 0;
                }
                if (sectionId === undefined || sectionId === "") {
                    sectionId = 0;
                }
                if ($scope.Character === null || $scope.Character === undefined) {
                    swal("Select The Required Radio Buttons and Fields");
                }
                var data = {
                    "ASMAY_Id": acedamicId,
                    "ASMCL_Id": classId,
                    "ASMC_Id": sectionId,
                    "AMST_Id": $scope.AMST_Id.amsT_Id,
                    "radiobutton": $scope.Character,
                    "save_flag": "yes"
                };

                if (parseInt($scope.Character) === 0) {

                    apiService.create("JSHSAdmissionCertificate/Studdetails", data).then(function (promise) {

                        if (promise.studentlist !== null && promise.studentlist.length > 0) {
                            $scope.mi_idd = promise.masterCompany[0].mI_Id;
                            $scope.count = promise.count;

                            $scope.print_flag = false;
                            $scope.study_cer = true;
                            $scope.bonafide = false;

                            $scope.students = promise.studentlist;

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

                            $scope.admno = promise.studentlist[0].admno;
                            $scope.studentname = promise.studentlist[0].studentnam;

                            $scope.fathername = promise.studentlist[0].fatherName;
                            $scope.mothername = promise.studentlist[0].mothername;

                            $scope.classstudying = promise.studentlist[0].class;
                            $scope.section = promise.studentlist[0].section;
                            $scope.acadamicyear = promise.studentlist[0].acadamicyear;
                            $scope.stuMT = promise.studentlist[0].stuMT;
                            $scope.admNo = promise.studentlist[0].admNo;
                            $scope.caste_name = promise.studentlist[0].caste_name;
                            $scope.CurrentDate = $scope.ASA_FromDate;
                            $scope.countryname = promise.studentlist[0].addressd1;

                            $scope.gen = promise.studentlist[0].gen;
                            $scope.AMST_Sex = promise.AMST_Sex;
                            if ($scope.gen === 'Male' || $scope.gen === 'MALE') {
                                $scope.gender = "HIS";
                                $scope.he = "HE";
                                $scope.sd = "S/O";
                            }
                            else if ($scope.gen === 'Female' || $scope.gen === 'FEMALE') {
                                $scope.gender = "HER";
                                $scope.he = "SHE";
                                $scope.sd = "D/O";
                            }

                            var datato1 = new Date(promise.studentlist[0].dob);
                            var dates = datato1.getDate();
                            var months = datato1.getMonth();
                            var years = datato1.getFullYear();
                            $scope.ordinal_suffix_of(dates);
                            $scope.getmontnames(months);
                            $scope.frommonth123 = frommonth;

                            var datato2 = new Date($scope.ASA_FromDate);
                            var dates1 = datato2.getDate();
                            var months1 = datato2.getMonth();
                            var years1 = datato2.getFullYear();
                            $scope.ordinal_suffix_of1(dates1);
                            $scope.getmontnames(months1);
                            $scope.frommonth223 = frommonth;

                            $scope.dob = promise.studentlist[0].dob;
                            $scope.getdate = $scope.ASA_FromDate;
                            $scope.index = $scope.datesuf1;
                            $scope.getmonth = $scope.frommonth223;
                            $scope.getyear = years1;
                            $scope.getdate1 = dates;
                            $scope.index1 = $scope.datesuf;
                            $scope.getmonth1 = $scope.frommonth123;
                            $scope.getyear1 = years;

                            $scope.dobinwords = promise.studentlist[0].dobwords;
                            $scope.SwitchFuction(promise.studentlist[0].class);
                            $scope.photopath = promise.studentlist[0].photopath;
                            $scope.street = promise.studentlist[0].street;
                            $scope.area = promise.studentlist[0].area;
                            $scope.city = promise.studentlist[0].city;
                            if (promise.studentlist[0].pincode !== null) {
                                $scope.city = $scope.city + '-' + promise.studentlist[0].pincode;
                            }
                            $scope.classname = data1;
                            if ($scope.classname !== "") {
                                $scope.classname = '[' + data1 + ']';
                                $scope.std = "STD";
                            }
                        }
                        else {
                            swal("Record not found");
                            $state.reload();
                        }
                    });
                }

                if (parseInt($scope.Character) === 2) {
                    apiService.create("JSHSAdmissionCertificate/Studdetails", data).then(function (promise) {
                        if (promise.principalsign !== null && promise.principalsign.length > 0) {
                            if (promise.principalsign[0].ivrmgC_PrincipalSign !== null && promise.principalsign[0].ivrmgC_PrincipalSign !== "") {
                                $scope.imgname = promise.principalsign[0].ivrmgC_PrincipalSign;
                                $scope.countimg = 1;
                            }
                            else {
                                $scope.imgname = "";
                                $scope.countimg = 0;
                            }
                        }
                        if (promise.studentlist !== null && promise.studentlist.length > 0) {
                            $scope.bonafide = true;
                            $scope.study_cer = false;
                            $scope.print_flag = false;
                            $scope.count = promise.count;

                            $scope.students = promise.studentlist;
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

                            $scope.studentname = promise.studentlist[0].studentnam;
                            $scope.fatherName = promise.studentlist[0].fatherName;
                            $scope.class = promise.studentlist[0].class;
                            $scope.acadamicyear = promise.studentlist[0].acadamicyear;
                            $scope.stuMT = promise.studentlist[0].stuMT;
                            $scope.admNo = promise.studentlist[0].admNo;
                            $scope.getdate = $scope.ASA_FromDate;
                            $scope.countryname = promise.studentlist[0].countryname;
                            $scope.statename = promise.studentlist[0].statename;
                            $scope.PerCity = promise.studentlist[0].PerCity;
                            $scope.PerArea = promise.studentlist[0].PerArea;
                            $scope.perstreet = promise.studentlist[0].perstreet;
                            $scope.CurrentDate = $scope.ASA_FromDate;

                            $scope.gen = promise.studentlist[0].gen;
                            if ($scope.gen === 'Male' || $scope.gen === 'MALE') {
                                $scope.gender = "HIS";
                                $scope.he = "HE";
                                $scope.sd = "S/O";
                            }
                            else if ($scope.gen === 'Female' || $scope.gen === 'FEMALE') {
                                $scope.gender = "HER";
                                $scope.he = "SHE";
                                $scope.sd = "D/O";
                            }

                            $scope.dob = promise.studentlist[0].dob;

                            $scope.countryname = promise.studentlist[0].addressd1;

                            $scope.AMST_Sex = promise.AMST_Sex;
                            if ($scope.AMST_Sex === 'Male' || $scope.AMST_Sex === 'MALE') {
                                $scope.gender = "HIS";
                                $scope.he = "HE";
                                $scope.sd = "S/O";
                            }
                            else if ($scope.AMST_Sex === 'Female' || $scope.AMST_Sex === 'FEMALE') {
                                $scope.gender = "HER";
                                $scope.he = "SHE";
                                $scope.sd = "D/O";
                            }
                        }
                        else {
                            swal("Record not found");
                            $state.reload();
                        }
                    });
                }
            }
            else {
                $scope.submitted = true;
            }
        };

        var studclear = [];
        $scope.Clearid = function () {
            $state.reload();
        };


        //print
        $scope.printToCart = function () {
            var data = "";
            var innerContents = "";
            var popupWinindow = "";

            if (parseInt($scope.Character) === 0) {

                data = 'printSectionIdgirls';
                innerContents = document.getElementById(data).innerHTML;
                popupWinindow = window.open('_blank');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/JSHSConductCertificatePdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 3000);">' + innerContents + '</html>');
                popupWinindow.document.close();


            }
            else if (parseInt($scope.Character) === 2) {

                data = 'printSectionId';
                innerContents = document.getElementById(data).innerHTML;
                popupWinindow = window.open('_blank');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    ' <link href="css/print/baldwin/BCharacterCertificatePdf.css" media="print" rel="stylesheet" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 3000);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
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

        $scope.searchfilter = function (objj, radioobj) {

            var data = "";
            var classid = 0;
            var sectionid = 0;

            $scope.bonafide = false;
            $scope.study_cer = false;
            $scope.print_flag = true;

            if (objj.search.length >= '2') {
                $scope.studentlst = "";
                if ($scope.ts.allorindii === "A") {
                    classid = 0;
                    sectionid = 0;
                }
                else {
                    classid = $scope.asmcL_Id;
                    sectionid = $scope.asmS_Id;
                }
                data = {
                    "filterinitialdata": radioobj,
                    "searchfilter": objj.search,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMST_SOL": $scope.ts.optradio,
                    "ASMCL_Id": classid,
                    "ASMC_Id": sectionid,
                    "allorindid": $scope.ts.allorindii
                };
                apiService.create("JSHSAdmissionCertificate/searchfilter", data).then(function (promise) {
                    if (promise.count > 0) {
                        $scope.studlist = promise.fillstudlist;
                    } else {
                        $scope.AMST_Id = "";
                        swal("No students are found for your search");
                    }
                });
            }

            //if (objj.search.length >= '2' && radioobj === 'L') {
            //    $scope.studentlst = "";
            //    if ($scope.ts.allorindii === "A") {
            //        classid = 0;
            //        sectionid = 0;
            //    }
            //    else {
            //        classid = $scope.asmcL_Id;
            //        sectionid = $scope.asmS_Id;
            //    }
            //    data = {
            //        "filterinitialdata": radioobj,
            //        "searchfilter": objj.search,
            //        "ASMAY_Id": $scope.asmaY_Id,
            //        "AMST_SOL": $scope.ts.optradio,
            //        "ASMCL_Id": classid,
            //        "ASMC_Id": sectionid,
            //        "allorindid": $scope.ts.allorindii
            //    };

            //    apiService.create("JSHSAdmissionCertificate/searchfilter", data).then(function (promise) {
            //        if (promise.count > 0) {
            //            $scope.studlist = promise.fillstudlist;
            //        } else {
            //            $scope.AMST_Id = "";
            //            swal("No students are found for your search");
            //        }
            //    });
            //}

            //else if (objj.search.length >= '2' && radioobj === 'D') {
            //    $scope.studentlst = "";

            //    data = {
            //        "filterinitialdata": radioobj,
            //        "searchfilter": objj.search,
            //        "ASMAY_Id": $scope.asmaY_Id,
            //        "AMST_SOL": $scope.ts.optradio,
            //        "ASMCL_Id": classid,
            //        "ASMC_Id": sectionid,
            //        "allorindid": $scope.ts.allorindii
            //    };

            //    apiService.create("JSHSAdmissionCertificate/searchfilter", data).then(function (promise) {
            //        if (promise.count > 0) {
            //            $scope.studlist = promise.fillstudlist;
            //        } else {
            //            $scope.AMST_Id = "";
            //            swal("No students are found for your search");
            //        }
            //    });
            //}
        };

        $scope.ordinal_suffix_of = function (datesufix) {
            var j = datesufix % 10,
                k = datesufix % 100;
            if (j == 1 && k != 11) {
                $scope.datesuf = "st";
                return $scope.datesuf;
            }
            if (j == 2 && k != 12) {
                $scope.datesuf = "nd";
                return $scope.datesuf;
            }
            if (j == 3 && k != 13) {
                $scope.datesuf = "rd";
                return $scope.datesuf;
            }
            $scope.datesuf = "th";
            return $scope.datesuf;
        };

        $scope.ordinal_suffix_of1 = function (datesufix1) {
            var j = datesufix1 % 10,
                k = datesufix1 % 100;
            if (j == 1 && k != 11) {
                $scope.datesuf1 = "st";
                return $scope.datesuf1;
            }
            if (j == 2 && k != 12) {
                $scope.datesuf1 = "nd";
                return $scope.datesuf1;
            }
            if (j == 3 && k != 13) {
                $scope.datesuf1 = "rd";
                return $scope.datesuf1;
            }
            $scope.datesuf1 = "th";
            return $scope.datesuf1;
        };


    }
})();