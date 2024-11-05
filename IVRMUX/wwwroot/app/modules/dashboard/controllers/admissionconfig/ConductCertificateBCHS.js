(function () {
    'use strict';
    angular
        .module('app')
        .controller('ConductCertificateBCHSController', ConductCertificateBCHSController)

    ConductCertificateBCHSController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function ConductCertificateBCHSController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


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
            //$scope.study = true;
            var pageid = 1;
            apiService.getURI("StudyCertificate/getdata", pageid).
                then(function (promise) {

                    $scope.yearlist = promise.allAcademicYear;
                    $scope.yearlist1 = promise.allAcademicYear;

                    $scope.classlist = promise.allclasslist;
                    $scope.classlist1 = promise.allclasslist;

                    $scope.sectionlist = promise.allsectionlist;
                    $scope.ASA_FromDate = new Date();
                    $scope.ASA_FromDate1 = new Date();
                    if (promise.masterCompany[0].mI_Id == 6) {
                        $scope.fathermothername5 = true;
                    }
                    else {
                        $scope.fathermothername5 = false;
                    }
                });
        };

        $scope.upper_grid = function () {

            $scope.upper_grid_hide = $scope.upper_grid_hide ? false : true;
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        /////////////////////////////////////////////////////////////////////////////

        $scope.fillstudentlist = function () {

            $scope.bonafide = false;
            $scope.study_cer = false;
            $scope.AMST_Id = "";
            $scope.studlist.length = 0;

            $scope.yearlist = {};
            var data = {
                "AMST_SOL": $scope.ts.optradio
            };

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("StudyCertificate/getS/", data).
                then(function (promise) {
                    $scope.yearlist = promise.allAcademicYear;
                    $scope.ASA_FromDate = new Date();
                    $scope.ASA_FromDate1 = new Date();

                });
        };

        $scope.onacademicyearchange = function () {
            $scope.bonafide = false;
            $scope.study_cer = false;
            $scope.AMST_Id = "";
            $scope.ASA_FromDate = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMST_SOL": $scope.ts.optradio
            }
            apiService.create("StudyCertificate/onacademicyearchange/", data).then(
                function (promoise) {
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
        $scope.getEmailsendingConfirmation = function () {

            var answer = confirm("Do you Want To Save The Record?");
            if (answer) {
                $scope.save_flag = "yes";
            }
            else {
                $scope.save_flag = "No";
            }
        };

        $scope.rommanclass = ["I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X"];
        // var frommonth;

        $scope.submitted = false;
        $scope.Reportconduct = function () {

            if ($scope.myForm.$valid) {
                //   $scope.getEmailsendingConfirmation();
                var joinclassname = "";
                var leftclassname = "";
                var totaljoinclassname = "";
                var totalleftclassname = "";
                // var totaljoinclassname = "";

                $scope.tcdategirls = $scope.ASA_FromDate1;

                var acedamicId = $scope.asmaY_Id;
                var sectionId = $scope.asmS_Id;
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

                var data = {
                    "ASMAY_Id": acedamicId,
                    "ASMCL_Id": classId,
                    "ASMC_Id": sectionId,
                    "AMST_Id": $scope.AMST_Id.amsT_Id,
                    "save_flag": $scope.save_flag

                };



                apiService.create("StudyCertificate/Studdetailsconduct", data)
                    .then(function (promise) {


                        if (promise.studentlist !== null && promise.studentlist.length > 0) {

                            if (promise.message !== null) {
                                swal(promise.message);
                            }

                            $scope.print_flag = false;
                            $scope.study_cer = true;
                            $scope.bonafide = false;

                            //$scope.grid_flag = true;
                            $scope.students = promise.studentlist;

                            $scope.admno = promise.studentlist[0].amsT_AdmNo;
                            $scope.studentname = promise.studentlist[0].amsT_FirstName;
                            $scope.fathername = promise.studentlist[0].fathername;
                            $scope.dob = promise.studentlist[0].dob;
                            $scope.dobwords = promise.studentlist[0].dobwords;
                            $scope.uppercase($scope.dobwords);


                            angular.forEach($scope.classlist1, function (xy) {
                                if (parseInt($scope.asmcL_Id1) === parseInt(xy.asmcL_Id)) {
                                    var classname = xy.asmcL_ClassName.toUpperCase();
                                    angular.forEach($scope.rommanclass, function (z) {
                                        if (classname === z) {
                                            joinclassname = "Std " + xy.asmcL_ClassName;
                                        }
                                    });
                                }
                            });
                            if (joinclassname === "") {
                                angular.forEach($scope.classlist1, function (xy) {
                                    var classname = xy.asmcL_Id;
                                    if (classname === parseInt($scope.asmcL_Id1)) {
                                        totaljoinclassname = xy.asmcL_ClassName;
                                    }
                                });
                            } else {
                                totaljoinclassname = joinclassname;
                            }

                            angular.forEach(promise.studentlist, function (xy) {
                                var classname = xy.leftclass.toUpperCase();
                                angular.forEach($scope.rommanclass, function (z) {
                                    if (classname === z) {
                                        leftclassname = "Std " + xy.leftclass;
                                    }
                                });
                            });

                            if (leftclassname === "") {
                                totalleftclassname = promise.studentlist[0].leftclass;
                            }
                            else {
                                totalleftclassname = leftclassname;
                            }

                            $scope.studentclass = totaljoinclassname + ' ' + 'to' + ' ' + totalleftclassname;



                            // var datafrom = new Date(promise.studentlist[0].joinedyear);

                            var datafrom = "";

                            angular.forEach($scope.yearlist, function (xyz) {
                                if (parseInt($scope.asmaY_Id1) === parseInt(xyz.asmaY_Id)) {
                                    datafrom = new Date(xyz.asmaY_From_Date);
                                }
                            });

                            var datatonew = new Date($scope.ASA_FromDate1);

                            var datato = new Date(promise.studentlist[0].leftyear);

                            var n = datafrom.getMonth();
                            $scope.getmontnames1(n);
                            $scope.fromdatee = frommonth;

                            var n1 = datafrom.getFullYear();
                            $scope.fromyear = n1;

                            var m = datatonew.getMonth();
                            $scope.getmontnames1(m);
                            $scope.todatee = frommonth;

                            var m1 = datatonew.getFullYear();
                            $scope.toyear = m1;

                            $scope.studentyear = $scope.fromdatee + '' + $scope.fromyear + '  ' + 'to' + ' ' + $scope.todatee + '' + $scope.toyear;

                            var examdate = $scope.ASA_FromDate;
                            var nn = examdate.getMonth();
                            $scope.getmontnames1(nn);
                            $scope.resultdate = frommonth;

                            var mm = examdate.getFullYear();
                            $scope.examyear = mm;

                            $scope.examheld = $scope.resultdate + ' ' + $scope.examyear;

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

        $scope.uppercase = function (str) {
            var array1 = str.split(' ');
            var newarray1 = [];

            for (var x = 0; x < array1.length; x++) {
                newarray1.push(array1[x].charAt(0).toUpperCase() + array1[x].slice(1).toLowerCase());
            }
            $scope.stuDobwords = newarray1;
            return $scope.stuDobwords = newarray1.join(' ');

        };


        //print
        $scope.printToCart = function () {
            var data;
            //girls school
            data = 'CunductCertificateBGHS';
            var innerContents = document.getElementById(data).innerHTML;
            var popupWinindow = window.open('_blank');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BCEHS/CunductCertificateBCEHSdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 3000);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        //month names
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
            if (objj.search.length >= '2' && radioobj === 'S') {
                $scope.studentlst = "";
                if ($scope.ts.allorindii === "A") {
                    $scope.asmcL_Id = 0;
                    $scope.asmS_Id = 0;
                }
                else {
                    $scope.asmcL_Id = $scope.asmcL_Id;
                    $scope.asmS_Id = $scope.asmS_Id;
                }
                data = {
                    "filterinitialdata": radioobj,
                    "searchfilter": objj.search,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMST_SOL": $scope.ts.optradio,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMC_Id": $scope.asmS_Id,
                    "allorindid": $scope.ts.allorindii
                };

                apiService.create("StudyCertificate/searchfilter", data).
                    then(function (promise) {
                        if (promise.count > 0) {
                            $scope.studlist = promise.fillstudlist;
                        } else {
                            $scope.AMST_Id = "";
                            swal("No students are found for your search");
                        }
                    });
            }

            if (objj.search.length >= '2' && radioobj === 'L') {
                $scope.studentlst = "";
                if ($scope.ts.allorindii === "A") {
                    $scope.asmcL_Id = 0;
                    $scope.asmS_Id = 0;
                }
                else {
                    $scope.asmcL_Id = $scope.asmcL_Id;
                    $scope.asmS_Id = $scope.asmS_Id;
                }
                data = {
                    "filterinitialdata": radioobj,
                    "searchfilter": objj.search,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMST_SOL": $scope.ts.optradio,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMC_Id": $scope.asmS_Id,
                    "allorindid": $scope.ts.allorindii
                };


                apiService.create("StudyCertificate/searchfilter", data).
                    then(function (promise) {
                        if (promise.count > 0) {
                            $scope.studlist = promise.fillstudlist;
                        } else {
                            $scope.AMST_Id = "";
                            swal("No students are found for your search");
                        }
                    });
            }

            else if (objj.search.length >= '2' && radioobj === 'D') {
                $scope.studentlst = "";

                data = {
                    "filterinitialdata": radioobj,
                    "searchfilter": objj.search,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMST_SOL": $scope.ts.optradio,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMC_Id": $scope.asmS_Id,
                    "allorindid": $scope.ts.allorindii
                };


                apiService.create("StudyCertificate/searchfilter", data).
                    then(function (promise) {
                        if (promise.count > 0) {
                            $scope.studlist = promise.fillstudlist;
                        } else {
                            $scope.AMST_Id = "";
                            swal("No students are found for your search");
                        }
                    });
            }
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

        $scope.fillallorindi = function () {
            if ($scope.ts.allorindii == "A") {
                $scope.allorindiform = false;
            }
            else {
                $scope.allorindiform = true;
            }
        };
    }
})();
