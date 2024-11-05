(function () {
    'use strict';
    angular.module('app').controller('BGHSStudenttcReportcustom', BGHSStudenttcReportcustom)
    BGHSStudenttcReportcustom.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function BGHSStudenttcReportcustom($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {
        $scope.report = false;
        $scope.mi_id = "";
        $scope.showlist = true;
        $scope.lblyearr = "2018";
        $scope.yearname = "";
        $scope.yearname1 = "";

        var data1;
        var frommonth = "";
        var tomonth;

        $scope.baldwinsboys = false;
        $scope.baldwinsgirls = false;
        $scope.baldwinscoedu = false;
        $scope.Admno_flag = true;
        $scope.IsHiddenup = true;
        $scope.IsHiddendown = false;
        $scope.report_flag = true;
        $scope.stu_name_flag = false;
        $scope.clear_flag = false;
        $scope.Print_flag = false;

        $scope.StuAttRptDropdownList = function () {
            apiService.get("StudenttcReportcustom/getdetails/").then(function (promise) {
                $scope.yearDropdown = promise.academicList;
                $scope.classlist = promise.fillclass;
                $scope.classlist1 = promise.fillclass;
                $scope.seclist = promise.fillsec;
                $scope.mididi = true;
                $scope.mididi1 = true;
                $scope.mididi1girls = true;
            });
        };

        $scope.changeyear = function () {
            angular.forEach($scope.yearDropdown, function (tt) {
                if (parseInt(tt.asmaY_Id) === parseInt($scope.asmaY_Id)) {
                    $scope.yearname = tt.asmaY_Year;
                    $scope.yearname1 = $scope.yearname.split("-");
                    $scope.lblyearr = $scope.yearname1[1];
                }
            });

            $scope.ASMCL_Id = "";
            $scope.ASMC_Id = "";
            $scope.amsT_Id = "";
            $scope.studentDropdown = [];
            $scope.baldwinsgirls = false;
            $scope.Print_flag = false;
            $scope.admname = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            };

            apiService.create("StudenttcReportcustom/getnameregno", data).then(function (promise) {
                if (promise.studentlist !== null && promise.studentlist.length > 0) {
                    $scope.studentDropdown = promise.studentlist;
                    $scope.stu_name_flag = true;
                    $scope.clear_flag = true;
                    $scope.report_flag = true;
                }
                else {
                    swal("Student is not available for this selection");
                    $scope.admname = "";
                    $scope.report = false;
                    $scope.stu_name_flag = false;
                    $scope.submitted = false;
                    $scope.myform.$setPristine();
                    $scope.myform.$setUntouched();
                }
            });
        };

        $scope.changeclass = function () {
            $scope.ASMC_Id = "";
            $scope.AMST_Id = "";
            $scope.studentDropdown = [];
            $scope.baldwinsgirls = false;
            $scope.Print_flag = false;
            $scope.admname = "";
        };

        $scope.changesection = function () {
            $scope.AMST_Id = "";
            $scope.studentDropdown = [];
            $scope.baldwinsgirls = false;
            $scope.Print_flag = false;
            $scope.admname = "";
            if ($scope.ASMC_Id !== undefined) {
                $scope.showlist = false;
            }
            else {
                $scope.showlist = true;
            }
        };

        $scope.ShowHideup = function () {
            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        };

        $scope.ShowHidedown = function () {
            $scope.IsHiddendown = $scope.IsHiddendown ? false : true;
        };

        $scope.reportshow = function () {
            if ($scope.asmaY_Id !== null && $scope.ASMCL_Id !== null && $scope.ASMS_Id !== null && $scope.admname !== null) {
                $scope.clear_flag = true;
                $scope.report_flag = true;
            }
        };

        $scope.onacademicyearchange = function (yearlist) {

            var yearid = $scope.asmaY_Id;
            apiService.getURI("StudenttcReportcustom/getACS", yearid).then(function (promise) {
                $scope.studentDropdown = promise.studentList;
                $scope.report_flag = true;
                $scope.IsHiddendown = true;
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
                    "AMST_Id": $scope.AMST_Id.amsT_Id
                };
                apiService.create("StudenttcReportcustom/getTcdetails", data).then(function (promise) {
                    if (promise.studentTCList.length > 0) {
                        var datafrom = new Date(promise.academicList1[0].asmaY_From_Date);
                        var datato = new Date(promise.academicList1[0].asmaY_To_Date);

                        var n = datafrom.getMonth();
                        $scope.getmontnames(n);
                        if (promise.masterCompany[0].miid == 4) {
                            $scope.getmontnames1(n);
                            $scope.frommonth1 = frommonth;
                        }
                        else {
                            $scope.getmontnames(n);
                            $scope.frommonth1 = frommonth;
                        }

                        var m = datato.getMonth();

                        if (promise.masterCompany[0].miid == 4) {
                            $scope.getmontnames1(m);
                        } else {
                            $scope.getmontnames(m);
                        }
                        var int = promise.studentTCList[0].Last_Class_Studied;
                        if (int == "X") {
                            $scope.tomonth1 = 'May';
                            $scope.monnto = 'May';
                        }
                        else {
                            $scope.tomonth1 = frommonth;
                            $scope.monnto = 'April';
                        }


                        var n1 = datafrom.getFullYear();
                        $scope.fromyear = n1;

                        var m1 = datato.getFullYear();
                        $scope.toyear = m1;
                        $scope.mi_id = promise.masterCompany[0].miid;
                        if (promise.masterCompany[0].miid == 4) {
                            //girls school
                            $scope.baldwinsgirls = true;
                            $scope.baldwinscoedu = false;
                            $scope.baldwinsboys = false;

                        }
                        else if (promise.masterCompany[0].miid == 5) {
                            //co-education
                            $scope.baldwinscoedu = true;
                            $scope.baldwinsboys = false;
                            $scope.baldwinsgirls = false;
                        }
                        else if (promise.masterCompany[0].miid == 6) {
                            //boys school
                            $scope.baldwinsboys = true;
                            $scope.baldwinsgirls = false;
                            $scope.baldwinscoedu = false;
                        }

                        if (promise.studentTCList[0].fee_Due_Amnt === null || promise.studentTCList[0].fee_Due_Amnt === "") {
                            promise.studentTCList[0].Fee_Due_Amnt = 0;
                        }


                        if (promise.studentTCList[0].library_Due_Amnt === null || promise.studentTCList[0].library_Due_Amnt === "") {
                            promise.studentTCList[0].library_Due_Amnt = 0;
                        }

                        if (promise.studentTCList[0].store_Canteen_Due === null || promise.studentTCList[0].store_Canteen_Due === "") {
                            promise.studentTCList[0].store_Canteen_Due = 0;
                        }
                        if (promise.studentTCList[0].pdA_Due === null || promise.studentTCList[0].pdA_Due === "") {
                            promise.studentTCList[0].pdA_Due = 0;
                        }

                        $scope.Print_flag = true;
                        $scope.IsHiddendown = true;
                        $scope.report = true;
                        $scope.stuFN = promise.studentTCList[0].amsT_FirstName;
                        $scope.stuMN = promise.studentTCList[0].amsT_MiddleName;
                        $scope.stuLN = promise.studentTCList[0].amsT_LastName;
                        $scope.stuFatName = promise.studentTCList[0].amsT_FatherName;
                        $scope.stuMotName = promise.studentTCList[0].amsT_MotherName;
                        $scope.stuRelig = promise.studentTCList[0].amsT_MotherName;
                        $scope.last_date_attended = promise.studentTCList[0].astC_LastAttendedDate;
                        $scope.stuDob = promise.studentTCList[0].amsT_DOB;
                        if (promise.masterCompany[0].miid == 4) {
                            $scope.stuDobwords = promise.studentTCList[0].amsT_DOB_Words;
                            $scope.uppercase($scope.stuDobwords);
                        }
                        else {
                            $scope.stuDobwords = promise.studentTCList[0].amsT_DOB_Words;
                        }

                        $scope.stuDoa = promise.studentTCList[0].amsT_Date;
                        $scope.stuCond = promise.studentTCList[0].astC_Conduct;
                        $scope.studRem = promise.studentTCList[0].astC_Remarks;
                        $scope.religion = promise.studentTCList[0].religion;
                        $scope.studcaste = promise.studentTCList[0].caste;
                        $scope.studReg = promise.studentTCList[0].amsT_RegistrationNo;
                        $scope.stuTCno = promise.studentTCList[0].astC_TCNO;
                        $scope.studEmail = promise.studentTCList[0].amsT_emailId;
                        $scope.studPlc = promise.studentTCList[0].amsT_PerCity;
                        $scope.stuAdmno = promise.studentTCList[0].amsT_AdmNo;
                        $scope.stuEmail = promise.studentTCList[0].amsT_emailId;
                        $scope.stuAadh = promise.studentTCList[0].amsT_AadharNo;
                        $scope.stuMob = promise.studentTCList[0].amsT_MobileNo;
                        $scope.stuLeaReas = promise.studentTCList[0].astC_LeavingReason;
                        $scope.stuLcs = promise.studentTCList[0].Last_Class_Studied;
                        if (promise.masterCompany[0].miid == 4) {
                            $scope.SwitchFuction1($scope.stuLcs);
                        } else {
                            $scope.SwitchFuction($scope.stuLcs);
                        }
                        if (data1 !== "") {
                            $scope.data2 = data1;
                            $scope.std1 = "Std";
                            $scope.data2girls = data1;
                        }
                        else {
                            $scope.std1 = "";
                            $scope.data2girls = promise.studentTCList[0].Last_Class_Studied;
                        }
                        $scope.stuIssDat = promise.studentTCList[0].astC_TCIssueDate;
                        $scope.stuPArea = promise.studentTCList[0].amsT_PerArea;
                        $scope.stuPStree = promise.studentTCList[0].amsT_PerStreet;
                        $scope.stuPCity = promise.studentTCList[0].amsT_PerCity;
                        $scope.stuCArea = promise.studentTCList[0].amsT_ConArea;
                        $scope.stuCStree = promise.studentTCList[0].amsT_ConStreet;
                        $scope.stuCCity = promise.studentTCList[0].amsT_ConCity;
                        $scope.stuSect = promise.studentTCList[0].asmC_SectionName;

                        $scope.nationality = promise.studentTCList[0].nationality;
                        $scope.birth_place = promise.studentTCList[0].amsT_BirthPlace;
                        $scope.last_school_attended = promise.studentTCList[0].astC_LastAttendedDate;
                        $scope.leaving_date = promise.studentTCList[0].astC_TCIssueDate;
                        $scope.progress = promise.studentTCList[0].astC_Qual_PromotionFlag;

                        $scope.Fee_Due_Amnt = promise.studentTCList[0].fee_Due_Amnt;
                        $scope.Library_Due_Amnt = promise.studentTCList[0].library_Due_Amnt;
                        $scope.Store_Canteen_Due = promise.studentTCList[0].store_Canteen_Due;
                        $scope.PDA_Due = promise.studentTCList[0].pdA_Due;
                        $scope.total_sum = $scope.Fee_Due_Amnt + $scope.Library_Due_Amnt + $scope.Store_Canteen_Due + $scope.PDA_Due;

                        if (promise.studentTCList[0].amsT_Sex === 'Male') {
                            $scope.sondaughter = 'Son';
                        } else {
                            $scope.sondaughter = 'Daughter';
                        }
                        $scope.getdate = new Date();

                        if (promise.previousschool.length > 0) {
                            $scope.stuschoolname = promise.previousschool[0].amstpS_PrvSchoolName;
                        }
                        else {
                            $scope.stuschoolname = $scope.pervious;
                        }

                        $scope.nextclass = promise.studentTCList[0].qualificlass;
                        $scope.SwitchFuction($scope.nextclass);
                        if (data1 !== "") {
                            $scope.std = "Std";
                            $scope.nextclassname = '[' + data1 + ']';
                        }
                        else {
                            $scope.nextclass = promise.studentTCList[0].qualificlass;
                            $scope.nextclassname = "";
                        }
                        $scope.promotiontype = $scope.prom;
                        //$scope.promotiontype = promise.studentTCList[0].qualificlass;

                        if (promise.masterCompany[0].miid == 4) {
                            angular.forEach($scope.classlist1, function (y) {
                                if (y.asmcL_Id == $scope.ASMCL_Id1) {
                                    $scope.joinclass = y.asmcL_ClassName;
                                }
                            });
                        }
                        else {
                            $scope.joinclass = promise.classnamejoin[0].classjoinname;
                        }

                        $scope.tcdategirls = promise.studentTCList[0].tcdatess;

                    }
                    else {
                        swal("Select any Student");
                        $scope.baldwinsboys = false;
                        $scope.baldwinsgirls = false;
                        $scope.baldwinscoedu = false;
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

        $scope.printdd = function (printSectionId) {

            $scope.printToCart($scope.printSectionId);
        };

        //print
        $scope.printToCart = function () {
            var data;
            //girls school
            data = 'printSectionIdgirls';
            var innerContents = document.getElementById(data).innerHTML;
            var popupWinindow = window.open('_blank', 'padding-top=50%;');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BGHS/StudentTcBBHS/BGHSStudentTcPdf.css" />' +
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

        $scope.SwitchFuction1 = function (sno) {
            switch (sno) {
                case 'I':
                    data1 = "First";
                    break;
                case 'II':
                    data1 = "Second";
                    break;
                case 'III':
                    data1 = "Third";
                    break;
                case 'IV':
                    data1 = "Fourth";
                    break;
                case 'V':
                    data1 = "Fifth";
                    break;
                case 'VI':
                    data1 = "Sixth";
                    break;
                case 'VII':
                    data1 = "Seventh";
                    break;
                case 'VIII':
                    data1 = "Eighth";
                    break;
                case 'IX':
                    data1 = "Ninth";
                    break;
                case 'X':
                    data1 = "Tenth";
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

        $scope.uppercase = function (str) {
            var array1 = str.split(' ');
            var newarray1 = [];

            for (var x = 0; x < array1.length; x++) {
                newarray1.push(array1[x].charAt(0).toUpperCase() + array1[x].slice(1).toLowerCase());
            }
            $scope.stuDobwords = newarray1;
            return $scope.stuDobwords = newarray1.join(' ');
        };
    }
})();
