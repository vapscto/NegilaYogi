(function () {
    'use strict';
    angular
        .module('app')
        .controller('CollegeBWMCStudyCertificateformethodist', CollegeBWMCStudyCertificateformethodist)

    CollegeBWMCStudyCertificateformethodist.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function CollegeBWMCStudyCertificateformethodist($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        // $scope.upper_grid = true;
        // $scope.study = true;

        //$scope.fathermothername5 = false;
        // $scope.Selected =0;
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
        $scope.bon = false;
        $scope.cor = false;
        $scope.exmname = '';
        $scope.exmmonth = '';
        $scope.stdreg = '';

        $scope.onpageload = function () {
            //$scope.study = true;
            var pageid = 1;
            apiService.getURI("CollegeStudyCertificateReport/getdata", pageid).
                then(function (promise) {

                    $scope.yearlist = promise.getyear;
                    $scope.classlist = promise.allclasslist;
                    $scope.sectionlist = promise.allsectionlist;
                    $scope.ASA_FromDate = new Date();
                    if (promise.masterCompany[0].mI_Id === 6) {
                        $scope.fathermothername5 = true;
                    }
                    else {
                        $scope.fathermothername5 = false;
                    }
                });
        };

        $scope.onchangeyear = function () {
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.AMCST_Id = "";
            $scope.studlist = [];
            $scope.exmname = "";
            $scope.stdreg = "";
            $scope.exmmonth = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("CollegeStudyCertificateReport/onchangeyear", data).then(function (promise) {
                $scope.courselist = promise.getcourse;
            });
        };

        $scope.onchangecourse = function () {
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.AMCST_Id = "";
            $scope.studlist = [];
            $scope.exmname = "";
            $scope.stdreg = "";
            $scope.exmmonth = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id
            };
            apiService.create("CollegeStudyCertificateReport/onchangecourse", data).then(function (promise) {
                $scope.branchlist = promise.getbranch;
            });
        };
        $scope.onchangebranch = function () {
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.AMCST_Id = "";
            $scope.studlist = [];
            $scope.exmname = "";
            $scope.stdreg = "";
            $scope.exmmonth = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id
            };
            apiService.create("CollegeStudyCertificateReport/onchangebranch", data).then(function (promise) {
                $scope.semesterlist = promise.getsemester;
            });
        };

        $scope.onchangesemester = function () {
            $scope.ACMS_Id = "";
            $scope.AMCST_Id = "";
            $scope.studlist = [];
            $scope.exmname = "";
            $scope.stdreg = "";
            $scope.exmmonth = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id
            };
            apiService.create("CollegeStudyCertificateReport/onchangesemester", data).then(function (promise) {
                $scope.sectionlist = promise.getsection;
            });
        };

        $scope.searchfilter = function (objj, radioobj) {

            var courseid = 0;
            var branchid = 0;
            var semesterid = 0;
            var sectionid = 0;

            if (objj.search.length > 2) {
                $scope.studentlst = "";
                if ($scope.ts.allorindii === "A") {
                    courseid = 0;
                    branchid = 0;
                    sectionid = 0;
                    semesterid = 0;
                }
                else {
                    courseid = $scope.AMCO_Id;
                    branchid = $scope.AMB_Id;
                    sectionid = $scope.ACMS_Id;
                    semesterid = $scope.AMSE_Id;
                }
                var data = {
                    "filterinitialdata": radioobj,
                    "searchfilter": objj.search,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCST_SOL": $scope.ts.optradio,
                    "AMCO_Id": courseid,
                    "AMB_Id": branchid,
                    "AMSE_Id": semesterid,
                    "ACMS_Id": sectionid,
                    "allorindid": $scope.ts.allorindii
                };
                apiService.create("CollegeStudyCertificateReport/searchfilter", data).
                    then(function (promise) {
                        if (promise.count > 0) {
                            $scope.studlist = promise.getstudentlist;
                        } else {
                            $scope.AMCST_Id = "";
                            swal("No students are found for your search");
                        }
                    });
            }

        };

        $scope.upper_grid = function () {
            $scope.upper_grid_hide = $scope.upper_grid_hide ? false : true;
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.fillstudentlist = function () {
            $scope.AMCST_Id = "";
            $scope.studlist = [];
        };

        $scope.submitted = false;



        $scope.Report = function () {

            $scope.bon = false;
            $scope.cor = false;


            if ($scope.myForm.$valid) {

                var courseid = 0;
                var branchid = 0;
                var semesterid = 0;
                var sectionid = 0;

                $scope.studentlst = "";
                if ($scope.ts.allorindii === "A") {
                    courseid = 0;
                    branchid = 0;
                    sectionid = 0;
                    semesterid = 0;
                }
                else {
                    courseid = $scope.AMCO_Id;
                    branchid = $scope.AMB_Id;
                    sectionid = $scope.ACMS_Id;
                    semesterid = $scope.AMSE_Id;
                }

                //if ($scope.Character === null || $scope.Character === undefined) {
                //    swal("Select The Required Radio Buttons and Fields");
                //}
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCST_SOL": $scope.ts.optradio,
                    "AMCO_Id": courseid,
                    "AMB_Id": branchid,
                    "AMSE_Id": semesterid,
                    "ACMS_Id": sectionid,
                    "AMCST_Id": $scope.AMCST_Id.amcsT_Id,
                    "allorindid": $scope.ts.allorindii
                };

                apiService.create("CollegeStudyCertificateReport/onclickreport", data)
                    .then(function (promise) {

                        if (promise.getstudentdetailslist.length > 0 && promise.getstudentdetailslist !== null) {
                            $scope.bon = true;
                            //if ($scope.Character === '1') {
                            //    $scope.bon = true;
                            //}
                            //if ($scope.Character === '0') {
                            //    $scope.cor = true;

                            //    $scope.exmname1 = $scope.exmname;
                            //    $scope.exmmonth1 = $scope.exmmonth;
                            //    $scope.stdreg1 = $scope.stdreg;
                            //}


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

                            $scope.print_flag = false;
                            $scope.study_cer = true;
                            $scope.bonafide = false;
                            $scope.todaydate = new Date();
                            $scope.students = promise.getstudentdetailslist;

                            $scope.admno = promise.getstudentdetailslist[0].admno;
                            $scope.studentname = promise.getstudentdetailslist[0].studentname;
                            $scope.fathername = promise.getstudentdetailslist[0].fathername;
                            $scope.acadamicyear = promise.getstudentdetailslist[0].yearname;
                         
                            var str1 = $scope.acadamicyear;
                            var str2 = str1.split('-');
                            $scope.aa = str2[0];
                            $scope.ba = str2[1];


                            $scope.classstudying = promise.getstudentdetailslist[0].semestername;
                            $scope.dob = promise.getstudentdetailslist[0].dob;
                            $scope.religion = promise.getstudentdetailslist[0].religion;
                            $scope.caste_name = promise.getstudentdetailslist[0].castecat;


                            $scope.getdate = $scope.ASA_FromDate;

                            //$scope.section = promise.studentgetstudentdetailslistlist[0].section;                            
                            //$scope.stuMT = promise.getstudentdetailslist[0].stuMT;                          
                            //$scope.caste_name = promise.getstudentdetailslist[0].caste_name;
                            //$scope.CurrentDate = $scope.ASA_FromDate;
                            //$scope.countryname = promise.getstudentdetailslist[0].addressd1;
                            //$scope.rollno = promise.getstudentdetailslist[0].rollno;

                            //$scope.dobinwords = promise.getstudentdetailslist[0].dobwords;
                            //$scope.classname = promise.getstudentdetailslist[0].class;
                            //$scope.photopath = promise.getstudentdetailslist[0].photopath;
                            //$scope.street = promise.getstudentdetailslist[0].street;
                            //$scope.area = promise.getstudentdetailslist[0].area;
                            //$scope.city = promise.getstudentdetailslist[0].city;
                            //$scope.fathername = promise.getstudentdetailslist[0].fatherName;
                            //$scope.mothername = promise.getstudentdetailslist[0].mothername;

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


        //print
        var popupWinindow = "";
        var innerContents = "";

        $scope.printToCart = function () {
            var data;
            data = 'printSectionIdgirls1';
            var innerContents = document.getElementById(data).innerHTML;
            var popupWinindow = window.open('_blank');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                ' <link href="css/print/baldwin/BWMC/BGMCStudycertificatePdf.css" media="print" rel="stylesheet" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 3000);">' + innerContents + '</html>');
            popupWinindow.document.close();

        }
        $scope.printToCart1 = function () {
            var data;
            if ($scope.Character === '1') {
                data = 'StudyCertificateBMC';
                innerContents = document.getElementById(data).innerHTML;
                popupWinindow = window.open('_blank');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BMC/CunductCertificateBMC/StudyCertificateBMCPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 3000);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
            if ($scope.Character === '0') {
                data = 'COURSECertificateBMC';
                innerContents = document.getElementById(data).innerHTML;
                popupWinindow = window.open('_blank');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BMC/CunductCertificateBMC/COURSECertificateBMCPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 3000);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }


        }
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



        $scope.ordinal_suffix_of = function (datesufix) {
            var j = datesufix % 10,
                k = datesufix % 100;
            if (j === 1 && k !== 11) {
                $scope.datesuf = "st";
                return $scope.datesuf;
            }
            if (j === 2 && k !== 12) {
                $scope.datesuf = "nd";
                return $scope.datesuf;
            }
            if (j === 3 && k !== 13) {
                $scope.datesuf = "rd";
                return $scope.datesuf;
            }
            $scope.datesuf = "th";
            return $scope.datesuf;
        };

        $scope.ordinal_suffix_of1 = function (datesufix1) {
            var j = datesufix1 % 10,
                k = datesufix1 % 100;
            if (j === 1 && k !== 11) {
                $scope.datesuf1 = "st";
                return $scope.datesuf1;
            }
            if (j === 2 && k !== 12) {
                $scope.datesuf1 = "nd";
                return $scope.datesuf1;
            }
            if (j === 3 && k !== 13) {
                $scope.datesuf1 = "rd";
                return $scope.datesuf1;
            }
            $scope.datesuf1 = "th";
            return $scope.datesuf1;
        };

        $scope.fillallorindi = function () {

            // $scope.ASMAY_Id = "";
            $scope.bon = false;
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.AMCST_Id = "";
            $scope.studlist = [];
            $scope.exmname = "";
            $scope.stdreg = "";
            $scope.exmmonth = "";
        };
    }
})();
