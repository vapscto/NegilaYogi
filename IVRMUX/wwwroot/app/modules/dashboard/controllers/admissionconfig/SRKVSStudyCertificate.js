(function () {
    'use strict';
    angular.module('app').controller('SRKVSStudycertificateController', SRKVSStudycertificateController)
    SRKVSStudycertificateController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function SRKVSStudycertificateController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

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
            apiService.getURI("SRKVSStudyCertificate/getdata", pageid).then(function (promise) {
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

            $scope.yearlist = {};
            var data = {
                "AMST_SOL": $scope.ts.optradio
            };
            apiService.create("SRKVSStudyCertificate/getS/", data).then(function (promise) {
                $scope.yearlist = promise.allAcademicYear;
                $scope.ASA_FromDate = new Date();

            });
        };

        $scope.printToCart = function () {
            var data;
            var innerContents = document.getElementById('BBKVTC').innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/SRKVSStudentAddressBook/SRKVSStudy_CertPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 3000);">' + innerContents + '</html>');
            popupWinindow.document.close();

        };

        $scope.onacademicyearchange = function () {
            $scope.bonafide = false;
            $scope.study_cer = false;
            $scope.AMST_Id = "";
            $scope.ASA_FromDate = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMST_SOL": $scope.ts.optradio
            };
            apiService.create("SRKVSStudyCertificate/onacademicyearchange/", data).then(function (promoise) {
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

        $scope.submitted = false;
        $scope.Report = function () {
            if ($scope.myForm.$valid) {
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
                var data = {
                    "ASMAY_Id": acedamicId,
                    "ASMCL_Id": classId,
                    "ASMC_Id": sectionId,
                    "AMST_Id": $scope.AMST_Id.amsT_Id
                };
                apiService.create("SRKVSStudyCertificate/Studdetailsconduct", data).then(function (promise) {
                    //if (promise.principalsign.length > 0 && promise.principalsign !== null) {
                    //    if (promise.principalsign[0].ivrmgC_PrincipalSign !== null && promise.principalsign[0].ivrmgC_PrincipalSign !== "") {
                    //        $scope.imgname = promise.principalsign[0].ivrmgC_PrincipalSign;
                    //        $scope.countimg = 1;
                    //    }
                    //    else {
                    //        $scope.imgname = "";
                    //        $scope.countimg = 0;
                    //    }
                    //}
                    if (promise.studentlist !== null && promise.studentlist.length > 0  ) {
                       // $scope.mi_idd = promise.masterCompany[0].mI_Id;
                        $scope.print_flag = false;
                        $scope.study_cer = true;
                        $scope.bonafide = false;

                        //$scope.grid_flag = true;
                        $scope.students = promise.studentlist;   
                      
                        $scope.admno = promise.studentlist[0].amsT_AdmNo;
                        $scope.studentname = promise.studentlist[0].amsT_FirstName;
                        $scope.fathername = promise.studentlist[0].fathername;
                        $scope.mothername = promise.studentlist[0].mothername;                        
                     

                        $scope.leftyear = promise.studentlist[0].leftyearname;
                        $scope.leftclass = promise.studentlist[0].leftclass;

                        $scope.joinedyear = promise.studentlist[0].joinedyearname;
                        $scope.joinedclass = promise.studentlist[0].joinedclass;

                        $scope.mothertounge = promise.studentlist[0].mothertounge;
                        $scope.gender = promise.studentlist[0].gender;
                        
                        if ($scope.gender === 'Male' || $scope.gender === 'MALE') {
                            $scope.gender = "HIS";
                            $scope.he = "HE";
                            $scope.sd = "son of";
                        }
                        else if ($scope.gender === 'Female' || $scope.gender === 'FEMALE') {
                            $scope.gender = "HER";
                            $scope.he = "SHE";
                            $scope.sd = "D/O";
                        }
                        var datato2 = new Date($scope.ASA_FromDate);
                        var dates1 = datato2.getDate();
                        var months1 = datato2.getMonth();
                        var years1 = datato2.getFullYear();
                        $scope.ordinal_suffix_of1(dates1);
                        $scope.getmontnames(months1);
                        $scope.frommonth223 = frommonth;
                        $scope.dob = promise.studentlist[0].dob;
                        $scope.getdate = $scope.ASA_FromDate;
                        $scope.dobinwords = promise.studentlist[0].dobwords;
                        $scope.photopath = promise.studentlist[0].photopath;
                        $scope.street = promise.studentlist[0].street;
                        $scope.area = promise.studentlist[0].area;
                        $scope.city = promise.studentlist[0].city;
                        if (promise.studentlist[0].pincode != null) {
                            $scope.city = $scope.city + '-' + promise.studentlist[0].pincode;
                        }

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
                apiService.create("SRKVSStudyCertificate/searchfilter", data).then(function (promise) {
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
            if ($scope.ts.allorindii === "A") {
                $scope.allorindiform = false;
            }
            else {
                $scope.allorindiform = true;
            }
        };
    }
})();
