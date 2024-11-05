(function () {
    'use strict';
    angular
.module('app')
.controller('BWMCTCCertificateController', BWMCTCCertificateController)

    BWMCTCCertificateController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function BWMCTCCertificateController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {
        var data1;
        var frommonth;
        var tomonth;
        //$scope.report = false;
        $scope.loaddata = function () {
            
            apiService.getDATA("BBKVCustomReport/getdetails/2").then(function (promise) {
                $scope.yearDropdown = promise.accyear;
            });
        }

        $scope.ShowHideup = function () {
            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        }

        $scope.ShowHidedown = function () {
            $scope.IsHiddendown = $scope.IsHiddendown ? false : true;
        }

        $scope.onclickregnoname = function () {
            
            var data = {
                "admnoorname": $scope.admname,
                "ASMAY_Id": $scope.asmaY_Id,
                "tctemporper": $scope.tctempper,
            }

            apiService.create("BBKVCustomReport/getnameregno", data).
       then(function (promise) {
           if (promise.studentlist != null && promise.studentlist.length > 0) {
               $scope.studentDropdown = promise.studentlist;
               $scope.std_name = true;
               $scope.clear_flag = true;
               $scope.report_flag = true;
           }
           else {
               swal("Student is not available for this selection");
               //$scope.admname = "";
               $scope.report = false;
               $scope.std_name = false;
               $scope.submitted = false;
           }

       })
        };

        $scope.stdnamechange = function () {
            var data = {
                "AMST_Id": $scope.amsT_Id,
                "ASMAY_Id": $scope.asmaY_Id,
            }
            
            apiService.create("BBKVCustomReport/stdnamechange", data).then
            (function (promise) {
                if (promise.classsecregno != null || promise.classsecregno.length > 0) {
                    $scope.lblclassname = promise.classsecregno[0].asmcL_ClassName;
                    $scope.lblsectioname = promise.classsecregno[0].asmC_SectionName;
                    $scope.lblregno = promise.classsecregno[0].amsT_RegistrationNo;
                }
                else {
                    swal("No Records Found")
                }
            })
        }

        $scope.onclicktcperortemo = function () {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "admnoorname": $scope.admname,
                "tctemporper": $scope.tctempper,
                "admnoorname": $scope.admname,
            }
            apiService.create("BBKVCustomReport/onclicktcperortemo", data).
            then(function (promise) {
                if (promise.studentlist != null && promise.studentlist.length > 0) {
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
            })
        }




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
                    "tctemporper": $scope.tctempper,
                }

                apiService.create("StudenttcReportcustom/getTcdetailsbwmc", data)
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

                                $scope.stuFN = promise.studentTCList[0].amsT_FirstName;
                                $scope.stuMN = promise.studentTCList[0].amsT_MiddleName;
                                $scope.stuLN = promise.studentTCList[0].amsT_LastName;

                                $scope.lblstdname = $scope.stuFN + ' ' + $scope.stuMN + ' ' + $scope.stuLN;

                                $scope.lblfathername = promise.studentTCList[0].amsT_FatherName;
                                $scope.lblmothername = promise.studentTCList[0].amsT_MotherName;
                                $scope.lblreligioncaste = promise.studentTCList[0].religion;

                                $scope.lblnationality = promise.studentTCList[0].nationality,
                                $scope.lblpobirth = promise.studentTCList[0].amsT_BirthPlace,
                                $scope.lbldob = promise.studentTCList[0].amsT_DOB;
                                $scope.lbldobwords = promise.studentTCList[0].amsT_DOB_Words;
                                $scope.lbllastschool = promise.masterCompany[0].companyname;
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
                                $scope.stuLcs = promise.studentTCList[0].last_Class_Studied;
                                $scope.last_school_attended = promise.studentTCList[0].astC_LastAttendedDate;
                                $scope.AMC_Name = promise.studentTCList[0].amC_Name;
                                $scope.name = promise.studentTCList[0].name;
                                $scope.pervious = $scope.pervious;
                            }
                            else {
                                swal("Select any Student");
                            }
                        })

            }
            else {
                $scope.submitted = true;
            }

        }

        $scope.interacted = function (field) {

            return $scope.submitted;
        };



        $scope.printToCart = function () {

            var innerContents = document.getElementById("printSectionId2").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
         '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
     '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
      '<link type="text/css" media="print" href="css/print/baldwin/BWMC/BGMCTCcertificatePdf.css" rel="stylesheet" />' +
     '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
     );
            popupWinindow.document.close();


        }



        //$scope.printToCart = function () {
        //    var data = {

        //        "ASMAY_Id": $scope.asmaY_Id,
        //        "AMST_Id": $scope.amsT_Id,
        //    }

        //    apiService.create("BBKVCustomReport/getTcdetails", data)
        //            .then(function (promise) {

        //                var innerContents = document.getElementById('BBKVTC').innerHTML;
        //                var popupWinindow = window.open('_blank', 'padding-top=50%;');
        //                popupWinindow.document.open();
        //                popupWinindow.document.write('<html><head>' + '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />'
        //                    + '<link type="text/css" media="print" rel="stylesheet" href="css/print/hutchings/TCCustomReport/TCCustomReportPdf.css" />'
        //                    + '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />'
        //                    + '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">'
        //                    + innerContents + '</html>');
        //                popupWinindow.document.close();
        //            })
        //}

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

    }
})();
