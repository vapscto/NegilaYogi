﻿(function () {
    'use strict';
    angular
.module('app')
        .controller('HHSBonafiedCertificateController', HHSBonafiedCertificateController)

    HHSBonafiedCertificateController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function HHSBonafiedCertificateController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.mi_idd = {};
        $scope.studlist = [];
        $scope.allorindiform = false;
        $scope.print_flag = true;
        $scope.report = false;
        var data1;
        var frommonth;
        var tomonth;
        var datesufix;
        $scope.datesuf = {};
        $scope.datesuf1 = {};

        $scope.onpageload = function () {
            //$scope.study = true;
            var pageid = 2;

            apiService.get("HHSBonafiedCertificate/getdata/2").then(function (promise) {

                $scope.yearlist = promise.allAcademicYear;
                $scope.classlist = promise.allclasslist;
                $scope.sectionlist = promise.allsectionlist;
                $scope.ASA_FromDate = new Date();
            })
        }

        $scope.upper_grid = function () {

            $scope.upper_grid_hide = $scope.upper_grid_hide ? false : true;
        }
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
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("HHSBonafiedCertificate/getS/", data).
        then(function (promise) {
            $scope.yearlist = promise.allAcademicYear;
            $scope.ASA_FromDate = new Date();

        })


        }

        $scope.onacademicyearchange = function () {
            $scope.bonafide = false;
            $scope.study_cer = false;
            $scope.AMST_Id = "";
            $scope.ASA_FromDate = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMST_SOL": $scope.ts.optradio
            }
            apiService.create("HHSBonafiedCertificate/onacademicyearchange/", data).then(
                function (promoise) {
                    $scope.AMST_Id = "";
                    if (promoise.fillstudlist.length > 0) {
                        $scope.ASA_FromDate = new Date();
                    }
                    else {
                        swal("No Records Found");
                        $scope.AMST_Id = "";
                    }
                })
        }



        $scope.submitted = false;
        $scope.Report = function () {
            
            if ($scope.myForm.$valid) {
                var acedamicId = $scope.asmaY_Id;
                var sectionId = $scope.asmC_Id;
                var classId = $scope.asmcL_Id;
                var studId = $scope.AMST_Id;

                if (acedamicId == undefined || acedamicId == "") {
                    acedamicId = 0;
                }
                if (classId == undefined || classId == "") {
                    classId = 0;
                }
                if (sectionId == undefined || sectionId == "") {
                    sectionId = 0;
                }
                //if ($scope.Character == null || $scope.Character == undefined) {
                //    swal("Select The Required Radio Buttons and Fields");
                //}


                var data = {
                    "ASMAY_Id": acedamicId,
                    "ASMCL_Id": classId,
                    "ASMC_Id": sectionId,
                    "AMST_Id": $scope.AMST_Id.amsT_Id,
                }
                
                apiService.create("HHSBonafiedCertificate/Studdetails", data)
              .then(function (promise) {
                  if (promise.studentlist.length > 0 && promise.studentlist != null) {
                      $scope.report = true;
                      $scope.mi_idd = promise.masterCompany[0].mI_Id;
                      $scope.schoolname = promise.masterCompany[0].companyname;
                      $scope.schooladdress = promise.masterCompany[0].address;
                      $scope.milogo = promise.masterCompany[0].milogo;
                      $scope.print_flag = false;
                      $scope.study_cer = true;
                      $scope.bonafide = false;

                      angular.forEach($scope.yearlist, function (x) {
                          if ($scope.asmaY_Id == x.asmaY_Id) {
                              $scope.acadamicyear = x.asmaY_Year;
                          }
                      })

                      $scope.date = new Date();
                      if (promise.studentlist[0].AMST_Sex == "FEMALE" || promise.studentlist[0].AMST_Sex == "Female") {
                          $scope.gender = "kumari";
                          $scope.gender1 = "She";
                          $scope.gender2 = "She";
                          $scope.gender3 = "D/o";
                          $scope.gender4 = "Her";
                      } else {
                          $scope.gender = "kumar";
                          $scope.gender1 = "He";
                          $scope.gender2 = "He";
                          $scope.gender3 = "S/o";
                          $scope.gender4 = "His";
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
                      $scope.studentname = promise.studentlist[0].studentnam;
                      $scope.fathername = promise.studentlist[0].fatherName;
                      $scope.mothername = promise.studentlist[0].mothername;

                      $scope.stugndfth = " " + $scope.studentname + " " + $scope.gender3 + " " + $scope.fathername;
                      $scope.class = promise.studentlist[0].class;
                      $scope.section = promise.studentlist[0].section;

                      $scope.stuMT = promise.studentlist[0].stuMT;
                      //    $scope.admno = promise.studentlist[0].admNo;
                      $scope.caste = promise.studentlist[0].caste_name;
                      $scope.CurrentDate = $scope.ASA_FromDate;
                      $scope.countryname = promise.studentlist[0].addressd1;

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
                      $scope.getdate = $scope.ASA_FromDate;
                      $scope.dob = promise.studentlist[0].dob;
                      $scope.dobinwords = promise.studentlist[0].dobwords;
                      $scope.SwitchFuction(promise.studentlist[0].class);
                      $scope.photopath = promise.studentlist[0].photopath;
                      $scope.street = promise.studentlist[0].street;
                      $scope.area = promise.studentlist[0].area;
                      $scope.city = promise.studentlist[0].city;
                      $scope.mothertounge = promise.studentlist[0].stuMT;
                      $scope.caste = promise.studentlist[0].caste;

                      if (promise.studentlist[0].pincode != null) {
                          $scope.city = $scope.city + '-' + promise.studentlist[0].pincode;
                      }
                      $scope.classname = data1
                      if ($scope.classname != "") {
                          $scope.classname = '[' + data1 + ']';
                          $scope.std = "STD";
                      }
                      else {

                      }
                  }
                  else {
                      $scope.report = false;
                      swal("Record not found");
                      $scope.print_flag = true;
                      $scope.study_cer = false;
                      $scope.bonafide = false;
                      $state.reload();
                  }
              })
            }
            else {
                $scope.submitted = true;
            }
        }

        var studclear = [];
        $scope.Clearid = function () {
            $state.reload();
        }


        $scope.HHSStudyCert = function () {
            var innerContents = document.getElementById("BBKV_SC").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                  '<link type="text/css" media="print" rel="stylesheet" href="css/print/BBKV/BBKVStudy/BBKVStudy_CertPdf.css" />' +
              '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
            '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }



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

        $scope.searchfilter = function (objj, radioobj) {
            
            if (objj.search.length >= '2' && radioobj == 'S') {
                $scope.studentlst = "";
                if ($scope.ts.allorindii == "A") {
                    $scope.asmcL_Id = 0;
                    $scope.asmS_Id = 0;
                }
                else {
                    $scope.asmcL_Id = $scope.asmcL_Id;
                    $scope.asmS_Id = $scope.asmS_Id;
                }
                var data = {
                    "filterinitialdata": radioobj,
                    "searchfilter": objj.search,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMST_SOL": $scope.ts.optradio,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMC_Id": $scope.asmS_Id,
                    "allorindid": $scope.ts.allorindii
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("HHSBonafiedCertificate/searchfilter", data).
            then(function (promise) {
                if (promise.count > 0) {
                    $scope.studlist = promise.fillstudlist;
                } else {
                    $scope.AMST_Id = "";
                    swal("No students are found for your search");
                }
            })
            }

            if (objj.search.length >= '2' && radioobj == 'L') {
                $scope.studentlst = "";
                if ($scope.ts.allorindii == "A") {
                    $scope.asmcL_Id = 0;
                    $scope.asmS_Id = 0;
                }
                else {
                    $scope.asmcL_Id = $scope.asmcL_Id;
                    $scope.asmS_Id = $scope.asmS_Id;
                }
                var data = {
                    "filterinitialdata": radioobj,
                    "searchfilter": objj.search,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMST_SOL": $scope.ts.optradio,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMC_Id": $scope.asmS_Id,
                    "allorindid": $scope.ts.allorindii
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("HHSBonafiedCertificate/searchfilter", data).
            then(function (promise) {
                if (promise.count > 0) {
                    $scope.studlist = promise.fillstudlist;
                } else {
                    $scope.AMST_Id = "";
                    swal("No students are found for your search");
                }
            })
            }

            else if (objj.search.length >= '2' && radioobj == 'D') {
                $scope.studentlst = "";

                var data = {
                    "filterinitialdata": radioobj,
                    "searchfilter": objj.search,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMST_SOL": $scope.ts.optradio,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMC_Id": $scope.asmS_Id,
                    "allorindid": $scope.ts.allorindii
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("HHSBonafiedCertificate/searchfilter", data).
            then(function (promise) {
                if (promise.count > 0) {
                    $scope.studlist = promise.fillstudlist;
                } else {
                    $scope.AMST_Id = "";
                    swal("No students are found for your search");
                }
            })
            }
        };

        //adding sufix to date
        $scope.ordinal_suffix_of = function (datesufix) {
            var j = datesufix % 10,
                k = datesufix % 100;
            if (j == 1 && k != 11) {
                $scope.datesuf = datesufix + "st";
                return $scope.datesuf;
            }
            if (j == 2 && k != 12) {
                $scope.datesuf = datesufix + "nd";
                return $scope.datesuf;
            }
            if (j == 3 && k != 13) {
                $scope.datesuf = datesufix + "rd";
                return $scope.datesuf;
            }
            $scope.datesuf = datesufix + "th";
            return $scope.datesuf;
        }

        $scope.ordinal_suffix_of1 = function (datesufix1) {
            var j = datesufix1 % 10,
                k = datesufix1 % 100;
            if (j == 1 && k != 11) {
                $scope.datesuf1 = datesufix1 + "st";
                return $scope.datesuf1;
            }
            if (j == 2 && k != 12) {
                $scope.datesuf1 = datesufix1 + "nd";
                return $scope.datesuf1;
            }
            if (j == 3 && k != 13) {
                $scope.datesuf1 = datesufix1 + "rd";
                return $scope.datesuf1;
            }
            $scope.datesuf1 = datesufix1 + "th";
            return $scope.datesuf1;
        }

        $scope.fillallorindi = function () {
            if ($scope.ts.allorindii == "A") {
                $scope.allorindiform = false;
            }
            else {
                $scope.allorindiform = true;
            }
        }
    }
})();
