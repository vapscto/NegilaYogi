﻿(function () {
    'use strict';
    angular.module('app').controller('EndorsementReportSRKVSController', EndorsementReportSRKVSController)

    EndorsementReportSRKVSController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function EndorsementReportSRKVSController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {

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
        $scope.obj = {};
        $scope.print_flagg = false;
        $scope.ASA_FromDate = new Date();
        $scope.onpageload = function () {
            //$scope.study = true;
            var pageid = 1;
            apiService.getURI("StudyCertificate/getdata", pageid).then(function (promise) {
                $scope.yearlist = promise.allAcademicYear;
                $scope.classlist = promise.allclasslist;
                $scope.sectionlist = promise.allsectionlist;
                $scope.ASA_FromDate = new Date();
                if (parseInt(promise.masterCompany[0].mI_Id) === 6) {
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

        $scope.fillstudentlist = function () {
            $scope.Transfer = false;
            $scope.study_cer = false;
            $scope.AMST_Id = "";
            $scope.studlist.length = 0;
        };

        $scope.onchangeyear = function () {
            $scope.Transfer = false;
            $scope.study_cer = false;
            $scope.AMST_Id = "";
            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.print_flag = true;
            $scope.studlist = [];
        };

        $scope.onchangeclass = function () {
            $scope.Transfer = false;
            $scope.study_cer = false;
            $scope.AMST_Id = "";
            $scope.asmS_Id = "";
            $scope.print_flag = true;
            $scope.studlist = [];
        };

        $scope.onchangesection = function () {
            $scope.Transfer = false;
            $scope.study_cer = false;
            $scope.AMST_Id = "";
            $scope.print_flag = true;
            $scope.studlist = [];
        };


        $scope.submitted = false;
        $scope.student_id =0;
        $scope.Report = function () {
            if ($scope.myForm.$valid) {
                //$scope.feebalaa = $scope.obj.feebal;
                var acedamicId = $scope.asmaY_Id;
                var sectionId = $scope.asmC_Id;
                var classId = $scope.asmcL_Id;
                var studId = $scope.AMST_Id;
                $scope.student_id = $scope.AMST_Id;

                if (acedamicId === undefined || acedamicId === "") {
                    acedamicId = 0;
                }
                if (classId === undefined || classId === "") {
                    classId = 0;
                }
                if (sectionId === undefined || sectionId === "") {
                    sectionId = 0;
                }
                //if ($scope.Character === null || $scope.Character === undefined) {
                //    swal("Select The Required Radio Buttons and Fields");
                //}
                var data = {
                    "ASMAY_Id": acedamicId,
                    "ASMCL_Id": classId,
                    //"ASMC_Id": sectionId,
                    "AMST_Id": $scope.AMST_Id.amsT_Id,
                    "mailflag": 0
                };


               
              //  if (parseInt($scope.Character) === 0) {

                    apiService.create("StudyCertificate/StuddetailsSRKVS", data).then(function (promise) {
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
                            $scope.studetails = promise.studentlist;
                            $scope.mi_idd = promise.masterCompany[0].mI_Id;
                            if (parseInt(promise.masterCompany[0].mI_Id) === 30) {
                                $scope.TMAESeducation = true;
                                //$scope.TMAEStransfer = false;
                                //$scope.TMAESboys = false;
                                //$scope.hide = false;
                            }
                            //else if (parseInt(promise.masterCompany[0].mI_Id) === 4) {
                            //    $scope.TMAEStransfer = true;
                            //    //$scope.TMAESeducation = false;
                            //    //$scope.TMAESboys = false;
                            //    //$scope.hide = false;
                            //}
                            //else if (parseInt(promise.masterCompany[0].mI_Id) === 6) {
                            //    $scope.TMAESboys = true;
                            //    $scope.TMAESeducation = false;
                            //    $scope.TMAEStransfer = false;
                            //    $scope.hide = false;
                            //}

                            $scope.print_flag = false;
                            $scope.study_cer = true;
                            $scope.Transfer = false;

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

                            $scope.admno = promise.studentlist[0].admno;
                            $scope.studentname = promise.studentlist[0].studentnam;

                            if (parseInt(promise.masterCompany[0].mI_Id) === 6) {
                                if ($scope.paretsnamefather === '0') {
                                    $scope.fathername = 'Mr.' + promise.studentlist[0].fatherName;
                                }
                                else if ($scope.paretsnamefather === '1') {
                                    $scope.fathername = 'Mrs.' + promise.studentlist[0].mothername;
                                }
                                else if ($scope.paretsnamefather === '2') {
                                    $scope.fathername = 'Mr.' + promise.studentlist[0].fatherName + ' ' + 'And' + ' ' + 'Mrs.' + promise.studentlist[0].mothername;
                                }
                            }
                            else {
                                $scope.fathername = promise.studentlist[0].fatherName;
                                $scope.mothername = promise.studentlist[0].mothername;
                            }
                            $scope.classstudying = promise.studentlist[0].class;
                            $scope.section = promise.studentlist[0].section;
                            $scope.acadamicyear = promise.studentlist[0].acadamicyear;
                            $scope.stuMT = promise.studentlist[0].stuMT;
                            $scope.admNo = promise.studentlist[0].admNo;
                            $scope.caste_name = promise.studentlist[0].caste_name;
                            $scope.CurrentDate = $scope.ASA_FromDate;
                            $scope.countryname = promise.studentlist[0].addressd1;
                            $scope.prospectusNo = promise.studentlist[0].PASP_ProspectusNo;
                            $scope.amst_emailid = promise.studentlist[0].AMST_emailId;

                         

                            var fromTime = $scope.obj.fromTime;
                            var toTime = $scope.obj.toTime;
                            $scope.obj.fromTimebindr  = $filter('date')(fromTime, 'HH:mm');
                            $scope.obj.toTimebindr = $filter('date')(toTime, 'HH:mm');
                            $scope.obj.fromTimebind = moment($scope.obj.fromTimebindr, 'HH:mm a').format();


                            var paidtime = $scope.obj.beforeTime;
                            $scope.obj.beforeTimebindr = $filter('date')(paidtime, 'HH:mm');
                        

                            //$scope.obj.fromTimebind = $filter('date')($scope.obj.fromTime, "HH:mm");

                            //$scope.obj.toTimebind = $filter('date')($scope.obj.toTime, "HH:mm");
                           // $scope.obj.beforeTimebind = $filter('date')($scope.obj.beforeTime, "HH:mm");

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


                            //$scope.dob = promise.studentlist[0].dob;
                            if (parseInt(promise.masterCompany[0].mI_Id) === 5) {
                                $scope.dob = promise.studentlist[0].dob;
                                $scope.getdate = $scope.ASA_FromDate;
                                $scope.getday = (new Date()).getDay();
                            }
                            else {
                                $scope.dob = promise.studentlist[0].dob;
                                $scope.getdate = $scope.ASA_FromDate;
                                $scope.day1 = new Date().getDay();

                                var arr = [];
                                $scope.getday = arr[2];
                                //  $scope.getday = (new Date()).getDay();
                                $scope.index = $scope.datesuf1;
                                $scope.getmonth = $scope.frommonth223;
                                $scope.getyear = years1;
                                $scope.getdate1 = dates;
                                $scope.index1 = $scope.datesuf;
                                $scope.getmonth1 = $scope.frommonth123;
                                $scope.getyear1 = years;
                            }

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
               // }


                if (parseInt($scope.Character) === 1) {

                    apiService.create("StudyCertificate/Studdetails", data).then(function (promise) {
                        //if (promise.principalsign !== null && promise.principalsign.length > 0) {
                        //    if (promise.principalsign[0].ivrmgC_PrincipalSign !== null && promise.principalsign[0].ivrmgC_PrincipalSign !== "") {
                        //        $scope.imgname = promise.principalsign[0].ivrmgC_PrincipalSign;
                        //        $scope.countimg = 1;
                        //    }
                        //    else {
                        //        $scope.imgname = "";
                        //        $scope.countimg = 0;
                        //    }
                        //}
                        if (promise.studentlist !== null && promise.studentlist.length > 0) {
                          
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
                            $scope.mothername = promise.studentlist[0].mothername;
                            $scope.fatherName = promise.studentlist[0].fatherName;
                            $scope.class = promise.studentlist[0].class;
                            $scope.acadamicyear = promise.studentlist[0].acadamicyear;
                            $scope.stuMT = promise.studentlist[0].stuMT;
                            $scope.admNo = promise.studentlist[0].admNo;
                            $scope.gender = promise.studentlist[0].AMST_Sex;
                            $scope.getdate = $scope.ASA_FromDate;
                            $scope.countryname = promise.studentlist[0].countryname;
                            $scope.statename = promise.studentlist[0].statename;
                            $scope.PerCity = promise.studentlist[0].PerCity;
                            $scope.PerArea = promise.studentlist[0].PerArea;
                            $scope.perstreet = promise.studentlist[0].perstreet;
                            $scope.CurrentDate = $scope.ASA_FromDate;

                            $scope.districtname = promise.studentlist[0].AMST_Distirct;
                            $scope.talukname = promise.studentlist[0].AMST_Taluk;
                            $scope.religionname = promise.studentlist[0].religion;
                            $scope.statename = promise.studentlist[0].IVRMMS_Name;
                            if (promise.studentlist[0].placeofbirth != null || promise.studentlist[0].placeofbirth != '0') {
                                $scope.placeofbirth = promise.studentlist[0].placeofbirth;
                            }
                            $scope.stucaste = promise.studentlist[0].IMC_CasteName;
                            $scope.stusubcaste = promise.studentlist[0].IMCC_CategoryName;
                            $scope.dobwords = promise.studentlist[0].dobwords;
                            $scope.dob = promise.studentlist[0].AMST_DOB;

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
        $scope.validatemax = function (maxdata) {

        var studclear = [];
        $scope.Clearid = function () {
            $state.reload();
        };


        $scope.SendEmail = function () {
            var acedamicId = $scope.asmaY_Id;
            var sectionId = $scope.asmC_Id;
            var classId = $scope.asmcL_Id;
            var studId = $scope.student_id

            

            if (acedamicId === undefined || acedamicId === "") {
                acedamicId = 0;
            }
            if (classId === undefined || classId === "") {
                classId = 0;
            }
            if (sectionId === undefined || sectionId === "") {
                sectionId = 0;
            }
            $scope.mailparameters = [];
           // if (obj.fintsalfee != null && obj.fintsalfee != undefined) {
                    
              //  $scope.mailparametrs.push({ id: , value: obj.fintsalfee })
            $scope.mailparameters.push({
                first_installemnt: $scope.obj.fintsalfee, demand_amt: $scope.obj.demandAmount, before_time: $scope.obj.beforeTime, from_btw_time: $scope.obj.fromTime,
                to_btw_time: $scope.obj.toTime, paid_onor_before: $scope.obj.paidonbefore, brought_admission: $scope.obj.broughtadmission, caution_amt: $scope.obj.cautiondeposit, selected_date: $scope.ASA_FromDate
            });
             //   }

            $scope.tmplt = [];
            var Template = document.getElementById("empmail").innerHTML;
            $scope.tmplt.push({ hrmE_EmployeeCode: "1", TemplateString: Template });
           
           var data = {
               
                "mailflag": 1,
                "mailParameters": $scope.mailparameters,
                 "ASMAY_Id": acedamicId,
                "ASMCL_Id": classId,
               //"ASMC_Id": sectionId,
               "AMST_Id": studId.amsT_Id,
               "AMST_EmailId": $scope.amst_emailid,
               Template: Template
              
            };
            apiService.create("StudyCertificate/StuddetailsSRKVS", data).then(function (promise) {
                if (promise.count > 0) {
                    $scope.studlist = promise.fillstudlist;
                } else {
                    $scope.AMST_Id = "";
                    swal("No students are found for your search");
                }
            });
        };


        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };


        //print

        $scope.printToCart = function () {
            var data = "";
            var innerContents = "";
            var popupWinindow = "";

        //    if (parseInt($scope.Character) === 1) {
                $scope.print_flagg = true;
                data = 'Endorsementcert';
                innerContents = document.getElementById(data).innerHTML;
                popupWinindow = window.open('_blank');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    ' <link href="css/print/baldwin/BCharacterCertificatePdf.css" media="print" rel="stylesheet" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 3000);">' + innerContents + '</html>');
                popupWinindow.document.close();

            //}
            //else if (parseInt($scope.Character) === 0) {

            //    data = 'TMAESStudy';
            //    innerContents = document.getElementById(data).innerHTML;
            //    popupWinindow = window.open('_blank');
            //    popupWinindow.document.open();
            //    popupWinindow.document.write('<html><head>' +
            //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
            //        ' <link href="css/print/baldwin/BCharacterCertificatePdf.css" media="print" rel="stylesheet" />' +
            //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
            //        '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 3000);">' + innerContents + '</html>');
            //    popupWinindow.document.close();
            //}
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

        $scope.searchfilter = function (objj) {
            var data = "";
            var classid = 0;
            var sectionid = 0;

            if (objj.search.length >= '2' ) {
                $scope.studentlst = "";
                //if ($scope.ts.allorindii === "A") {
                //    classid = 0;
                //    $scope.asmS_Id = 0;
                //}
             //   else {
                    classid = $scope.asmcL_Id;
                 //   sectionid = $scope.asmS_Id;
//}
                data = {
                   // "filterinitialdata": radioobj,
                    "searchfilter": objj.search,
                    "ASMAY_Id": $scope.asmaY_Id,
                  //  "AMST_SOL": $scope.ts.optradio,
                    "ASMCL_Id": classid,
                  //  "ASMC_Id": sectionid,
                   // "allorindid": $scope.ts.allorindii
                };
                apiService.create("StudyCertificate/searchfilterSRKVS", data).then(function (promise) {
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

            //    apiService.create("StudyCertificate/searchfilter", data).then(function (promise) {
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

            //    apiService.create("StudyCertificate/searchfilter", data).then(function (promise) {
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