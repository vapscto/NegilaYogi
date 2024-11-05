(function () {
    'use strict';
    angular.module('app').controller('HHSTCCustomReportController', HHSTCCustomReportController)

    HHSTCCustomReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function HHSTCCustomReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {
        var data1;
        var frommonth;
        var tomonth;
        $scope.classstudyingsincewhen = "";
        $scope.schoolyearbeginfrom = "";

        //$scope.report = false;
        $scope.loaddata = function () {
            apiService.get("HHSTCCustomReport/getdetails/2").then(function (promise) {
                $scope.yearDropdown = promise.accyear;
                $scope.classdropdown = promise.accclass;
            });
        };

        $scope.ShowHideup = function () {
            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        };

        $scope.ShowHidedown = function () {
            $scope.IsHiddendown = $scope.IsHiddendown ? false : true;
        };

        $scope.onclickregnoname = function () {
            var data = {
                "admnoorname": $scope.admname,
                "ASMAY_Id": $scope.asmaY_Id,
                "tctemporper": $scope.tctempper
            };
            apiService.create("HHSTCCustomReport/getnameregno", data).then(function (promise) {
                if (promise.studentlist !== null && promise.studentlist.length > 0) {
                    $scope.studentDropdown = promise.studentlist;
                    $scope.std_name = true;
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

        $scope.stdnamechange = function () {
            var data = {
                "AMST_Id": $scope.amsT_Id,
                "ASMAY_Id": $scope.asmaY_Id
            };

            apiService.create("HHSTCCustomReport/stdnamechange", data).then(function (promise) {
                if (promise.classsecregno !== null || promise.classsecregno.length > 0) {
                    $scope.lblclassname = promise.classsecregno[0].asmcL_ClassName;
                    $scope.lblsectioname = promise.classsecregno[0].asmC_SectionName;
                    $scope.lblregno = promise.classsecregno[0].amsT_RegistrationNo;
                }
                else {
                    swal("No Records Found");
                }
            });
        };

        $scope.onclicktcperortemo = function () {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "admnoorname": $scope.admname,
                "tctemporper": $scope.tctempper
            };
            apiService.create("HHSTCCustomReport/onclicktcperortemo", data).then(function (promise) {
                if (promise.studentlist !== null && promise.studentlist.length > 0) {
                    $scope.studentDropdown = promise.studentlist;
                    $scope.std_name = true;
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

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.submitted = false;
        $scope.savetmpldata = function () {

            if ($scope.myform.$valid) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMST_Id": $scope.amsT_Id,
                    "tctemporper": $scope.tctempper
                };

                apiService.create("HHSTCCustomReport/getTcdetails", data).then(function (promise) {

                    if (promise.studentTCList.length > 0) {

                        var datafrom = new Date(promise.academicList1[0].asmaY_From_Date);
                        var datato = new Date(promise.academicList1[0].asmaY_To_Date);

                        var n = datafrom.getMonth();
                        $scope.getmontnames(n);
                        $scope.frommonth1 = frommonth;
                        //$scope.frommonth1 = "June";

                        var m = datato.getMonth();
                        $scope.getmontnames(m);
                        $scope.tomonth1 = frommonth;
                        //$scope.tomonth1 = "May";

                        var n1 = datafrom.getFullYear();
                        $scope.fromyear = n1;

                        var m1 = datato.getFullYear();
                        $scope.toyear = m1;

                        $scope.Print_flag = true;
                        $scope.IsHiddendown = true;
                        $scope.report = true;
                        $scope.tcno = promise.studentTCList[0].astC_TCNO;
                        $scope.admno = promise.studentTCList[0].amsT_RegistrationNo;

                        $scope.uppercase(promise.studentTCList[0].studentname);
                        $scope.lblstdname = $scope.stuDobwords;

                        $scope.uppercase(promise.studentTCList[0].amsT_FatherName);
                        $scope.lblfathername = $scope.stuDobwords;

                        $scope.uppercase(promise.studentTCList[0].amsT_MotherName);
                        $scope.lblmothername = $scope.stuDobwords;

                        $scope.uppercase(promise.studentTCList[0].religion);
                        $scope.lblreligioncaste = $scope.stuDobwords;

                        $scope.uppercase(promise.studentTCList[0].nationality);
                        $scope.lblnationality = $scope.stuDobwords;

                        //$scope.uppercase(promise.studentTCList[0].amsT_BirthPlace);
                        $scope.lblpobirth = promise.studentTCList[0].amsT_BirthPlace;

                        $scope.lbldob = promise.studentTCList[0].amsT_DOB;

                        $scope.uppercase(promise.studentTCList[0].amsT_DOB_Words);
                        $scope.lbldobwords = $scope.stuDobwords;

                        $scope.uppercase(promise.masterCompany[0].companyname);
                        $scope.lbllastschool = $scope.stuDobwords;

                        $scope.classnamestd = $scope.lblclassname;
                        $scope.last_date_attended = promise.studentTCList[0].astC_LastAttendedDate;
                        $scope.lbldoa = promise.studentTCList[0].amsT_Date;
                        $scope.lblprogress = "Satisfactory";

                        $scope.uppercase(promise.studentTCList[0].astC_Conduct);
                        $scope.lblconduct = $scope.stuDobwords;

                        $scope.uppercase(promise.studentTCList[0].qualificlass);
                        //$scope.lblPromotion = $scope.stuDobwords;
                        $scope.lblPromotion = promise.studentTCList[0].qualificlass;

                        $scope.lbldol = promise.studentTCList[0].astC_TCIssueDate;

                        //Auto Generated
                        $scope.lblstudyingsince = promise.studentTCList[0].classname + ' ' + 'Since' + ' ' + $scope.frommonth1 + '-' + $scope.fromyear;
                        $scope.lblschoolbegins = $scope.frommonth1 + '-' + $scope.fromyear + ' ' + 'to' + ' ' + $scope.tomonth1 + '-' + $scope.toyear;

                        //Manual Entered
                        if ($scope.schoolyearbeginfrom !== null && $scope.schoolyearbeginfrom !== "") {
                            $scope.lblschoolbegins = $scope.schoolyearbeginfrom;
                        }

                        if ($scope.classstudyingsincewhen !== null && $scope.classstudyingsincewhen !== "") {
                            $scope.lblstudyingsince = promise.studentTCList[0].classname + ' ' + 'Since' + ' ' + $scope.classstudyingsincewhen;
                        }


                        $scope.uppercase(promise.studentTCList[0].feepaid);
                        $scope.lblallsumsfee = $scope.stuDobwords;

                        //$scope.uppercase(promise.studentTCList[0].astC_LeavingReason);
                        $scope.lblreasonleaving = promise.studentTCList[0].astC_LeavingReason;

                        $scope.lbldate = promise.studentTCList[0].astC_TCIssueDate;

                        $scope.uppercase(promise.studentTCList[0].caste);
                        $scope.studcaste = $scope.stuDobwords;

                        $scope.castecategory = promise.studentTCList[0].category;

                        $scope.uppercase(promise.studentTCList[0].mothertounge);
                        $scope.lblmothertongue = $scope.stuDobwords;

                        $scope.bpl_id = promise.studentTCList[0].bpl_id;

                        $scope.religion_caste_castercategory = "";
                        if ($scope.lblreligioncaste === $scope.studcaste) {
                            $scope.religion_caste_castercategory = $scope.lblreligioncaste + "/" + $scope.castecategory;
                        } else {
                            $scope.religion_caste_castercategory = $scope.lblreligioncaste + "/" + $scope.studcaste + "/" + $scope.castecategory;
                        }

                        angular.forEach($scope.classdropdown, function (xdd) {
                            if (parseInt(xdd.asmcL_Id) === parseInt($scope.asmcL_Id)) {
                                $scope.lastclass = xdd.asmcL_ClassName;
                            }
                        });
                    }
                    else {
                        swal("Select Any Student");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {

            return $scope.submitted;
        };


        //print
        $scope.printToCart = function () {

            var innerContents = document.getElementById('HHSTCCustomReport').innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/hutchings/TCCustomReport/TCCustomReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();

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


        $scope.uppercase = function (str) {
            if (str !== null && str !== undefined && str !== "") {
                var array1 = str.split(' ');
                var newarray1 = [];

                for (var x = 0; x < array1.length; x++) {
                    newarray1.push(array1[x].charAt(0).toUpperCase() + array1[x].slice(1).toLowerCase());
                }

                $scope.stuDobwords = newarray1;
                return $scope.stuDobwords = newarray1.join(' ');
            } else {
                return $scope.stuDobwords = "";
            }
        };
    }
})();