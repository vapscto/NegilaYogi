(function () {
    'use strict';
    angular.module('app').controller('StudenttcController', StudenttcController)

    StudenttcController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function StudenttcController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

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
        $scope.reportdetails = false;
        $scope.certificatenameregno = "";
        $scope.examcertificatename = "";

        $scope.onpageload = function () {
            //$scope.study = true;
            var pageid = 2;
            apiService.get("HHSBonafiedCertificate/getdata/2").then(function (promise) {
                $scope.yearlist = promise.allAcademicYear;
                $scope.classlist = promise.allclasslist;
                $scope.sectionlist = promise.allsectionlist;
                $scope.ASA_FromDate = new Date();
            });
        };

      
        $scope.upper_grid = function () {
            $scope.upper_grid_hide = $scope.upper_grid_hide ? false : true;
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.fillstudentlist = function () {
            $scope.bonafide = false;
            $scope.study_cer = false;
            $scope.AMST_Id = "";
            $scope.studlist.length = 0;
            $scope.studlist = [];
            $scope.certificatenameregno = "";
            $scope.examcertificatename = "";
            $scope.reportdetails = false;
            $scope.yearlist = {};
            var data = {
                "AMST_SOL": $scope.ts.optradio
            };

            apiService.create("HHSBonafiedCertificate/getS/", data).then(function (promise) {
                $scope.yearlist = promise.allAcademicYear;
                $scope.ASA_FromDate = new Date();
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

        $scope.onacademicyearchange = function () {
            $scope.bonafide = false;
            $scope.study_cer = false;
            $scope.AMST_Id = "";
            $scope.ASA_FromDate = "";
            $scope.reportdetails = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMST_SOL": $scope.ts.optradio
            };
            apiService.create("HHSBonafiedCertificate/onacademicyearchange/", data).then(function (promoise) {
                $scope.AMST_Id = "";
                if (promoise.fillstudlist.length > 0) {
                    $scope.ASA_FromDate = new Date();
                }
                else {
                    swal("No Records Found");
                    $scope.AMST_Id = "";
                }
            });
        };

        $scope.OnChangeYear = function () {
            $scope.reportdetails = false;
            $scope.certificatenameregno = "";
            $scope.examcertificatename = "";
            $scope.examarray = [];
            $scope.studlist = [];
            $scope.AMST_Id = "";

            angular.forEach($scope.yearlist, function (dd) {
                if (dd.asmaY_Id === Number($scope.asmaY_Id)) {
                    var yearname_temp = dd.asmaY_Year.split('-');

                    angular.forEach(yearname_temp, function (d) {
                        //$scope.examarray.push({ certificatename: 'INDIAN SCHOOL CERTIFICATE EXAMINATIONS, ' + d });
                        $scope.examarray.push({ certificatename: 'INDIAN CERTIFICATE OF SECONDARY EDUCATION (Year 10) Examination held in March ' + d });
                    });
                }
            });
        };

        $scope.OnChangeStudentName = function () {
            $scope.reportdetails = false;
        };


        $scope.submitted = false;

        $scope.Report = function () {
            $scope.reportdetails = false;
            if ($scope.myForm.$valid) {
                $scope.getEmailsendingConfirmation();
                var acedamicId = $scope.asmaY_Id;
                var studId = $scope.AMST_Id;
                var sectionId = 0;
                var classId = 0;

                if ($scope.ts.allorindii === "A") {
                    classId = 0;
                    sectionId = 0;
                }
                else {
                    classId = $scope.asmcL_Id;
                    sectionId = $scope.asmS_Id;
                }

                var data = {
                    "ASMAY_Id": acedamicId,
                    "ASMCL_Id": classId,
                    "ASMC_Id": sectionId,
                    "AMST_Id": $scope.AMST_Id.amsT_Id,
                    "save_flag": $scope.save_flag
                };

                apiService.create("HHSBonafiedCertificate/MigrationCertificateStuddetails", data).then(function (promise) {
                    if (promise.studentlist1 !== null && promise.studentlist1.length > 0) {
                        $scope.studentlist = promise.studentlist1;
                        $scope.studentname = promise.studentlist1[0].amsT_FirstName;
                        $scope.mi_idd = promise.masterCompany[0].mI_Id;
                        $scope.schoolname = promise.masterCompany[0].companyname;
                        $scope.schooladdress = promise.masterCompany[0].address;
                        $scope.milogo = promise.masterCompany[0].milogo;
                        $scope.print_flag = false;
                        $scope.study_cer = true;
                        $scope.bonafide = false;
                        $scope.reportdetails = true;


                        angular.forEach($scope.yearlist, function (x) {
                            if (parseInt($scope.asmaY_Id) === parseInt(x.asmaY_Id)) {
                                $scope.acadamicyear = x.asmaY_Year;
                            }
                        });

                        $scope.date = new Date();
                        if (promise.studentlist[0].AMST_Sex === "FEMALE" || promise.studentlist1[0].AMST_Sex === "Female") {
                            $scope.gender = "kumari";
                            $scope.gender1 = "She";
                            $scope.gender2 = "her";
                            $scope.gender3 = "Daughter";
                            $scope.gender4 = "She";
                            $scope.sonof = "d/o";
                        } else {
                            $scope.gender = "kumar";
                            $scope.gender1 = "He";
                            $scope.gender2 = "his";
                            $scope.gender3 = "Son";
                            $scope.gender4 = "He";
                            $scope.sonof = "s/o";
                        }

                        //$scope.grid_flag = true;
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

                        // $scope.acadamicyear = $scope.frommonth1 + '-' + $scope.fromyear + '-' + $scope.tomonth1 + '-' + $scope.toyear;

                        $scope.admno = promise.studentlist[0].admno;
                        $scope.Mc_No = promise.studentlist[0].Mc_No;
                        $scope.studentnamed = promise.studentlist[0].studentnam;
                        $scope.uppercase($scope.studentnamed);
                        $scope.studentname = $scope.stuDobwordsd;

                        $scope.fathernamed = promise.studentlist[0].fatherName;
                        $scope.uppercase($scope.fathernamed);
                        $scope.fathername = $scope.stuDobwordsd;

                        $scope.certificatename = $scope.examcertificatename;
                        $scope.registernumber = $scope.certificatenameregno;

                        $scope.mothernamed = promise.studentlist[0].mothername;
                        $scope.uppercase($scope.mothernamed);
                        $scope.mothername = $scope.stuDobwordsd;

                        $scope.class = promise.studentlist[0].class;
                        $scope.section = promise.studentlist[0].section;

                        $scope.stuMT = promise.studentlist[0].stuMT;
                        $scope.caste = promise.studentlist[0].caste_name;
                        $scope.CurrentDate = $scope.ASA_FromDate;
                        $scope.countryname = promise.studentlist[0].addressd1;
                        $scope.stuDoa = new Date(promise.studentlist[0].admdate);

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
                        $scope.getyear = years1;

                        $scope.getdate = dates1;

                        $scope.dob = promise.studentlist[0].dob;
                        $scope.dobinwords = promise.studentlist[0].dobwords;
                        $scope.SwitchFuction(promise.studentlist[0].class);
                        $scope.photopath = promise.studentlist[0].photopath;
                        $scope.street = promise.studentlist[0].street;
                        $scope.area = promise.studentlist[0].area;
                        $scope.city = promise.studentlist[0].city;
                        $scope.mothertounge = promise.studentlist[0].stuMT;

                        if (promise.studentlist[0].pincode !== null) {
                            $scope.city = $scope.city + '-' + promise.studentlist[0].pincode;
                        }
                        $scope.classname = data1;
                        if ($scope.classname !== "") {
                            $scope.classname = '[' + data1 + ']';
                            $scope.std = "STD";
                        }
                        else {
                            //d
                        }

                        $scope.studentlist1 = promise.studentlist1;
                        $scope.admittedclass = $scope.studentlist1[0].joinedclass;
                    }
                    else {
                        swal("Record not found");
                        $scope.print_flag = true;
                        $scope.study_cer = false;
                        $scope.bonafide = false;
                        $state.reload();
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        var studclear = [];
        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.HHSStudyCert = function () {
            var innerContents = document.getElementById("printmodalnew").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                //'<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                //'<link type="text/css" media="print" rel="stylesheet" href="css\print\baldwin\BCEHS\BCOESAPPReportPdf.css />' +
                //'<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                //'</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
        
            popupWinindow.document.close();

        };

        //month names
        $scope.getmontnames = function (monthid) {
            frommonth = "";
            switch (monthid) {

                case 0:
                    frommonth = "January";
                    break;
                case 1:
                    frommonth = "February";
                    break;
                case 2:
                    frommonth = "March";
                    break;
                case 3:
                    frommonth = "April";
                    break;
                case 4:
                    frommonth = "May";
                    break;
                case 5:
                    frommonth = "June";
                    break;
                case 6:
                    frommonth = "July";
                    break;
                case 7:
                    frommonth = "August";
                    break;
                case 8:
                    frommonth = "September";
                    break;
                case 9:
                    frommonth = "October";
                    break;
                case 10:
                    frommonth = "November";
                    break;
                case 11:
                    frommonth = "December";
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
            var sectionidnew = 0;
            var classidnew = 0;
            if (objj.search.length >= '3') {
                $scope.studentlst = "";
                if ($scope.ts.allorindii === "A") {
                    classidnew = 0;
                    sectionidnew = 0;
                }
                else {
                    classidnew = $scope.asmcL_Id;
                    sectionidnew = $scope.asmS_Id;
                }
                data = {
                    "filterinitialdata": radioobj,
                    "searchfilter": objj.search,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMST_SOL": $scope.ts.optradio,
                    "ASMCL_Id": classidnew,
                    "ASMC_Id": sectionidnew,
                    "allorindid": $scope.ts.allorindii
                };

                apiService.create("HHSBonafiedCertificate/searchfilter", data).then(function (promise) {
                    if (promise.count > 0) {
                        $scope.studlist = promise.fillstudlist;
                    } else {
                        $scope.AMST_Id = "";
                        $scope.studlist = [];
                        swal("No students are found for your search");
                    }
                });
            }
        };

        //adding sufix to date
        $scope.ordinal_suffix_of = function (datesufix) {
            var j = datesufix % 10,
                k = datesufix % 100;
            if (j === 1 && k !== 11) {
                $scope.datesuf = datesufix + "st";
                $scope.index = "st";
                return $scope.datesuf;
            }
            if (j === 2 && k !== 12) {
                $scope.datesuf = datesufix + "nd";
                $scope.index = "nd";
                return $scope.datesuf;
            }
            if (j === 3 && k !== 13) {
                $scope.datesuf = datesufix + "rd";
                $scope.index = "rd";
                return $scope.datesuf;
            }
            $scope.datesuf = datesufix + "th";
            $scope.index = "th";
            return $scope.datesuf;
        };

        $scope.ordinal_suffix_of1 = function (datesufix1) {
            var j = datesufix1 % 10,
                k = datesufix1 % 100;

            console.log(j, k);
            if (j === 1 && k !== 11) {
                $scope.datesuf1 = datesufix1 + "st";
                $scope.index1 = "st";
                return $scope.datesuf1;
            }
            if (j === 2 && k !== 12) {
                $scope.datesuf1 = datesufix1 + "nd";
                $scope.index1 = "nd";
                return $scope.datesuf1;
            }
            if (j === 3 && k !== 13) {
                console.log("D");
                $scope.datesuf1 = datesufix1 + "rd";
                $scope.index1 = "rd";
                return $scope.datesuf1;
            }
            $scope.datesuf1 = datesufix1 + "th";
            $scope.index1 = "th";
            return $scope.datesuf1;
        };

        $scope.fillallorindi = function () {
            $scope.reportdetails = false;
            if ($scope.ts.allorindii === "A") {
                $scope.allorindiform = false;
            }
            else {
                $scope.allorindiform = true;
            }
        };

        $scope.uppercase = function (str) {
            var array1 = str.split(' ');
            var newarray1 = [];

            for (var x = 0; x < array1.length; x++) {
                newarray1.push(array1[x].charAt(0).toUpperCase() + array1[x].slice(1).toLowerCase());
            }
            $scope.stuDobwordsd = newarray1;
            return $scope.stuDobwordsd = newarray1.join(' ');
        };
    }
})();
