(function () {
    'use strict';
    angular
        .module('app')
        .controller('PS_MonthWiseReportStjamesController', PS_MonthWiseReportStjamesController)

    PS_MonthWiseReportStjamesController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function PS_MonthWiseReportStjamesController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {
        var data1;
        var frommonth;
        var tomonth;
        $scope.tcpdf = true;



        // Get form Details at onload 
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("PS7andPS8FormReport/getalldetails", pageid).then(function (promise) {

                if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {

                    $scope.leaveyeardropdown = promise.leaveyeardropdown;
                }



                if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                    $scope.employeedropdown = promise.employeedropdown;
                }

            })
        }


        $scope.employeeDetails = [];
        $scope.EmployeeDis = false;
        //Search employee
        $scope.submitted = false;
        $scope.SearchEmployee = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "HRES_Year": $scope.hreS_Year,
                    "Flag": "MonthWise"
                }

                apiService.create("PS7andPS8FormReport/getdataps8", data).then(function (promise) {

                    if (promise.pfreport.length > 0 && promise.pfreport != null) {
                        //$scope.employeeDetails = promise.employeeDetails;

                        //$scope.hrmE_EmployeeFirstName = $scope.employeeDetails[0].hrmE_EmployeeFirstName;
                        //$scope.hrmE_FatherName = $scope.employeeDetails[0].hrmE_FatherName;
                        //$scope.mI_Name = $scope.employeeDetails[0].mI_Name;
                        //$scope.mI_Address1 = $scope.employeeDetails[0].mI_Address1;
                        $scope.plannerdetails = [];
                        $scope.nextyear = $scope.hreS_Year;
                        $scope.nextyear++;

                        $scope.pfreport = promise.pfreport;
                        $scope.Totalamountofwages = 0;
                        $scope.Totalpensionamount = 0;
                        $scope.temp = [];
                        angular.forEach($scope.pfreport, function (itm) {
                            $scope.Totalamountofwages = $scope.Totalamountofwages + itm.sumcondition;
                            $scope.Totalpensionamount = $scope.Totalpensionamount + itm.pensionamout;
                            // itm.Monthlow="100023";
                        });

                        $scope.temp.push({
                            Certified: "Certified that the difference between the Figures of total Pension Fubnd Contribution remitted during the currency period and the total pension Fund Contribution shown under Col.5 is solely due to the rounding of amounts to the nearest Rupee under the rules.",
                            Certifiedtwo: "(i)Total Number of contribution cards enclosed (Form 7).",
                            Certifiedthree: " (ii) Certified that Forms 7 (PS) completed of all the members listed in this statement are enclosed except those already sent during the course of the currency period for the final settlement of the concerned member's account vide 'Remarks' furnished against the names of the respective members above.",
                            Month_Idone: 1
                        })

                    }

                })
            }

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

                    "Flag": MonthWise
                }

                apiService.create("PS7andPS8FormReport/getTcdetails", data)
                    .then(function (promise) {

                        if (promise.studentTCList != null) {
                            $scope.tcpdf = true;
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
                            $scope.admno = promise.studentTCList[0].amsT_AdmNo;
                            $scope.lblstdname = promise.studentTCList[0].studentname;
                            $scope.lblfathername = promise.studentTCList[0].amsT_FatherName;
                            $scope.lblmothername = promise.studentTCList[0].amsT_MotherName;
                            $scope.lblreligioncaste = promise.studentTCList[0].religion;
                            $scope.lblnationality = promise.studentTCList[0].nationality,
                                $scope.lblpobirth = promise.studentTCList[0].amsT_BirthPlace,
                                $scope.lbldob = promise.studentTCList[0].amsT_DOB;
                            $scope.lbldobwords = promise.studentTCList[0].amsT_DOB_Words;
                            $scope.lbllastschool = promise.masterCompany[0].companyname;
                            $scope.last_date_attended = promise.studentTCList[0].astC_LastAttendedDate;
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
                            $scope.last_Class_Studied = promise.studentTCList[0].last_Class_Studied;
                            $scope.amsT_Sex = promise.studentTCList[0].amsT_Sex;
                            $scope.astC_Qual_PromotionFlag = promise.studentTCList[0].astC_Qual_PromotionFlag;
                            $scope.regno = promise.studentTCList[0].amsT_RegistrationNo;
                            $scope.govtadmno = promise.studentTCList[0].govtadmno;
                            if ($scope.ilang == undefined || $scope.ilang == "") {
                                $scope.langustudies = promise.studentTCList[0].langustudies;
                            }
                            else {
                                $scope.langustudies = $scope.ilang;
                            }
                            if ($scope.iilang == undefined || $scope.iilang == "") {
                                $scope.electivestudies = promise.studentTCList[0].electivestudies;
                            }
                            else {
                                $scope.electivestudies = $scope.iilang;
                            }

                            $scope.feedue = promise.studentTCList[0].feedue;
                            $scope.ASTC_TCApplicationDate = promise.studentTCList[0].astC_TCApplicationDate;
                            $scope.schecasteortribe = $scope.scgcaste;
                            $scope.monthyear = $scope.monthyearf;
                            $scope.exam = $scope.examf;
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

            var innerContents = document.getElementById('BBKVTC111').innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/BBKV/BBKVTC/BPUTCPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
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




        $scope.exportToExcel = function () {
            var divToPrint = document.getElementById("Baldwin");
            var blob = new Blob([divToPrint.outerHTML], {
                type: "application/vnd.ms-excel;charset=utf-8"
            });
            saveAs(blob, "Report.xls");
        };

    }
})();
