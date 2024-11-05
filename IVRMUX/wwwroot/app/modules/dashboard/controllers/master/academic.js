
(function () {
    'use strict';
    angular
        .module('app')
        .controller('academicController', academicController)

    academicController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'superCache', 'Excel', '$timeout']
    function academicController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, superCache, Excel, $timeout) {


        $scope.searchProspectus = '';
        $scope.sortKey = 'asmaY_Id';
        $scope.sortReverse = true;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.ddate = {};
        $scope.getyear = [];
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        //  $scope.ASMAY_From_Date = new Date();
        $scope.asmaY_Id = "";
        $scope.sortColumn = false;
        $scope.selacdfryr = true;
        $scope.selacdtoyr = true;
        $scope.prestdt = true;
        $scope.preenddate = true;

        // Load Data
        $scope.academicDet = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("Academic/getalldetails", pageid).
                then(function (promise) {
                    if (promise.count === 0) {
                        swal("No Records Found.....!!");
                    }
                    else {
                        $scope.students = promise.academicList;
                        $scope.presentCountgrid = $scope.students.length;
                        $scope.getyear = promise.getyear;

                        $scope.arrearmindate = new Date($scope.getyear[0].asmaY_From_Date);
                        $scope.arrearmaxdate = new Date($scope.getyear[0].asmaY_To_Date);

                        $scope.newuser2 = promise.getallyear;
                        $scope.yearlist = promise.getallyearnew;
                    }
                    $scope.institutionList = promise.institutionList;
                    $scope.MI_Id = $scope.institutionList[0].mI_Id;
                });
        };

        // Adding +1 To Academic From Year 
        $scope.count1 = function () {
            //$scope.ASMAY_to_Year = $scope.ASMAY_from_Year + 1;
            var To_Year = parseInt($scope.ASMAY_from_Year);
            $scope.ASMAY_to_Year = To_Year + 1;
        };

        // Set Start Date
        $scope.setfromdate = function (data) {

            if (data !== null) {

                console.log(data);
                $scope.fromDate = data;
                $scope.frommon = "";
                $scope.fromDay = "";

                // For Academic From Date
                //$scope.minDatef = new Date(
                //      $scope.fromDate,
                //       $scope.frommon,
                //        $scope.fromDay + 1);

                //$scope.maxDatef = new Date(
                //      $scope.fromDate,
                //       $scope.frommon,
                //        $scope.fromDay + 365);

                $scope.fromD = new Date($scope.getyear[0].asmaY_From_Date);
                $scope.toD = new Date($scope.getyear[0].asmaY_To_Date);

                console.log($scope.getyear);
                $scope.minDatef = new Date(
                    $scope.fromDate,
                    $scope.fromD.getMonth(),
                    $scope.fromD.getDate());


                $scope.maxDatef = new Date(
                    $scope.fromDate,
                    $scope.toD.getMonth(),
                    $scope.toD.getDate() + 365);


                //for Preadmission From date
                $scope.minDatefP = new Date(
                    $scope.fromDate - 1,
                    $scope.frommon,
                    $scope.fromDay + 1);


                $scope.maxDatefP = new Date(
                    $scope.fromDate,
                    $scope.frommon,
                    $scope.fromDay + 365);


                $scope.selacdfryr = false;

                $scope.arrearmindate = $scope.fromD;
                $scope.arrearmaxdate = $scope.toD;

                $scope.finacialstartdate = new Date(
                    $scope.fromDate,
                    "03",
                    "01"
                );
                $scope.finacialstartdate1 = $scope.finacialstartdate;
                $scope.ASMAY_RegularFeeFDate = new Date($scope.finacialstartdate);
            }
            else {
                $scope.selacdfryr = true;
            }

            // $scope.ASMAY_from_Year = "";
            $scope.ASMAY_From_Date = "";
            $scope.ASMAY_PreAdm_F_Date = "";
            $scope.ASMAY_Order = "";
            $scope.ASMAY_to_Year = "";
            $scope.ASMAY_To_Date = "";
            $scope.ASMAY_PreAdm_T_Date = "";
            $scope.ASMAY_Cut_Of_Date = "";
            $scope.ASMAY_ActiveFlag = "";
            $scope.ASMAY_Pre_ActiveFlag = "";

        };

        // Checking Academic To Year Is Equal To 1
        $scope.todatevalidation = function () {
            $scope.todate12 = $scope.ASMAY_to_Year;
            var year_from = parseInt($scope.ASMAY_from_Year);
            var year_to = parseInt($scope.ASMAY_to_Year);

            if (year_to < year_from) {
                swal("Academic To Year Should Be Greather Than Academic From Year");
                return;
            }


            var yeardiff = year_to - year_from;
            if (yeardiff !== 1) {
                swal("Difference Should Be One Year");
                var To_Year = parseInt($scope.ASMAY_from_Year);
                $scope.ASMAY_to_Year = To_Year + 1;
            }

            $scope.finacialenddate = new Date(
                $scope.todate12,
                "02",
                "31");

            $scope.finacialenddate1 = new Date(
                $scope.todate12,
                "02",
                "31");

            $scope.ASMAY_RegularFeeTDate = new Date($scope.finacialenddate);
        };

        // Academic Start Date
        $scope.validatetodate = function (data1) {
            $scope.selacdtoyr = false;
            $scope.prestdt = false;
            $scope.toDate = data1;
            //$scope.fromDate = data1;
            //$scope.frommon = "";
            //$scope.fromDay = "";

            // For Academic End Date
            //$scope.minDatet = new Date(
            //      $scope.fromDate.getFullYear() + 1,
            //       $scope.frommon,
            //        $scope.fromDay + 1);

            //$scope.maxDatet = new Date(
            //      $scope.fromDate.getFullYear(),
            //       $scope.fromDate.getMonth(),
            //        $scope.fromDay + 365);
            $scope.minDatet = new Date(
                $scope.toDate.getFullYear(),
                $scope.toDate.getMonth() + 1,
                $scope.toDate.getDate());

            $scope.maxDatet = new Date(
                $scope.toDate.getFullYear() + 1,
                $scope.toDate.getMonth(),
                $scope.toDate.getDate() - 1);

            $scope.maxDatefP = new Date(
                $scope.toDate.getFullYear(),
                $scope.toDate.getMonth(),
                $scope.toDate.getDate() - 1);

        };

        // Academic End Date
        $scope.checkErr = function (FromDate, ToDate) {
            $scope.errMessage = '';
            // $scope.ASMAY_ArrearFeeDate = "";
            $scope.todatee = new Date(ToDate);
            var curDate = new Date();
            if (new Date(FromDate) > new Date(ToDate)) {
                $scope.errMessage = 'To Date should be greater than from date';
                return false;
            }
            //if (new Date(FromDate) < curDate) {
            //    $scope.errMessage = 'from date should not be before today.';
            //    return false;
            //}

            $scope.arrearmindated = new Date(
                $scope.todatee.getFullYear(),
                $scope.todatee.getMonth(),
                $scope.todatee.getDate() + 1
            );

        };

        // Preadmission Start Date
        $scope.validatetodatepre = function (data1) {

            var fromtimee = new Date(data1);
            fromtimee.setHours(0);
            fromtimee.setMinutes(0);
            fromtimee.setSeconds(0);
            $scope.fromtimebind = fromtimee;
            $scope.ScheduleTime_24 = $scope.fromtimebind;

            var totimee = new Date(data1);
            totimee.setHours(23);
            totimee.setMinutes(59);
            totimee.setSeconds(0);
            $scope.totimebind = totimee;
            $scope.ScheduleTimeTo_24 = $scope.totimebind;

            $scope.selacdtoyr = false;
            $scope.preenddate = false;
            $scope.toDate1 = data1;
            $scope.minDatetP = new Date(
                $scope.toDate1.getFullYear(),
                $scope.toDate1.getMonth(),
                $scope.toDate1.getDate() + 1);

            $scope.maxDatetP = new Date(
                $scope.ASMAY_From_Date.getFullYear(),
                $scope.ASMAY_From_Date.getMonth(),
                $scope.ASMAY_From_Date.getDate() - 1);

            //   $scope.maxDatetP = new Date(
            // $scope.toDate1.getFullYear() + 1,
            // $scope.toDate1.getMonth(),
            //$scope.toDate1.getDate());


            //pre cut off
            $scope.minDatetPC = new Date(
                $scope.toDate1.getFullYear(),
                $scope.toDate1.getMonth(),
                $scope.toDate1.getDate());

            $scope.maxDatetPC = new Date(
                $scope.toDate1.getFullYear() + 1,
                $scope.toDate1.getMonth(),
                $scope.toDate1.getDate());
        };

        // Preadmission End Date
        $scope.checkErr1 = function (ASMAY_PreAdm_F_Date, ASMAY_PreAdm_T_Date) {
            $scope.errMessage1 = '';
            var curDate = new Date();
            if (new Date(ASMAY_PreAdm_F_Date) > new Date(ASMAY_PreAdm_T_Date)) {
                $scope.errMessage1 = 'To Date should be greater than from date';
                return false;
            }            
        };

        // Transport 
        $scope.validatetransportstartdate = function () {
            $scope.ASMAY_TransportEDate = "";
        };

        $scope.validatetransportenddate = function () {
            $scope.transportfromdate = new Date($scope.ASMAY_TransportSDate);
            $scope.transporttodate = new Date($scope.ASMAY_TransportEDate);
            if ($scope.transportfromdate > $scope.transporttodate) {
                swal("Transport End Date Should Be Greather Than Start Date");
                $scope.ASMAY_TransportSDate = "";
            }
        };

        // Validation For Finacial Year
        $scope.checkstartfinacial = function () {
            $scope.ASMAY_RegularFeeTDate = "";
            $scope.fincaialstartdate = new Date($scope.ASMAY_RegularFeeFDate);
            $scope.finacialstartdate1 = $scope.fincaialstartdate;
            $scope.finacialenddate1 = new Date(
                $scope.finacialstartdate1.getFullYear() + 1,
                $scope.finacialstartdate1.getMonth(),
                $scope.finacialstartdate1.getDate() - 1
            );
            $scope.ASMAY_RegularFeeTDate = $scope.finacialenddate1;
        };

        $scope.checkendfinacial = function () {

            var today = new Date($scope.ASMAY_RegularFeeTDate);
            var dd = today.getDate();
            var mm = today.getMonth() + 1; //January is 0!
            var yyyy = today.getFullYear();
            if (dd < 10) {
                dd = '0' + dd;
            }
            if (mm < 10) {
                mm = '0' + mm;
            }
            today = yyyy + '/' + mm + '/' + dd;


            var date2 = new Date(today);
            var date1 = new Date($scope.ASMAY_RegularFeeFDate);
            var timeDiff = Math.abs(date2.getTime() - date1.getTime());
            $scope.dayDifference = Math.ceil(timeDiff + 1 / (1000 * 3600 * 24));
            if ($scope.dayDifference !== 365) {
                $scope.finacialenddate1 = new Date(
                    $scope.todate12,
                    "02",
                    "31");
                $scope.ASMAY_RegularFeeTDate = new Date($scope.finacialenddate);

                $scope.finacialstartdate = new Date(
                    $scope.fromDate,
                    "03",
                    "01"
                );

                $scope.ASMAY_RegularFeeFDate = new Date($scope.finacialstartdate);

                swal("Difference Between Financial Year End Date And Financial Year State Date Count Should Be 365");
            }
        };

        // Save 
        $scope.saveorgdata = function () {
            var dateValidation = $scope.checkErr($scope.ASMAY_From_Date, $scope.ASMAY_To_Date);
            var dateValidation1 = $scope.checkErr1($scope.ASMAY_PreAdm_F_Date, $scope.ASMAY_PreAdm_T_Date);
            var dateValidation2 = $scope.checkyr($scope.ASMAY_from_Year, $scope.ASMAY_to_Year);
            $scope.ScheduleTime = $scope.ScheduleTime_24;
            $scope.ScheduleTimeTo = $scope.ScheduleTimeTo_24;
            var ScheduleTime = $filter('date')($scope.ScheduleTime, "HH");
            var ScheduleTimeTo = $filter('date')($scope.ScheduleTimeTo, "HH");
            var ScheduleTimem = $filter('date')($scope.ScheduleTime, "mm");
            var ScheduleTimeTom = $filter('date')($scope.ScheduleTimeTo, "mm");
            var ScheduleTimesec = $filter('date')($scope.ScheduleTime, "ss");
            var ScheduleTimeTosec = $filter('date')($scope.ScheduleTimeTo, "ss");

            if (dateValidation === false || dateValidation1 === false || dateValidation2 === false) {
                $scope.submitted = false;
            }
            else {
                if ($scope.myForm.$valid) {
                    if ($scope.asmaY_Id !== "" && $scope.asmaY_Id !== null) {
                        $scope.Id = $scope.asmaY_Id;
                    }
                    else {
                        $scope.Id = 0;
                    }
                    var ASMAY_ActiveFlag = $scope.ASMAY_ActiveFlag;
                    var ASMAY_Pre_ActiveFlag = $scope.ASMAY_Pre_ActiveFlag;

                    var ASMAY_NewAdmissionFlg = $scope.ASMAY_NewAdmissionFlg;
                    var ASMAY_ReggularFlg = $scope.ASMAY_ReggularFlg;
                    var ASMAY_NewFlg = $scope.ASMAY_NewFlg;

                    if ($scope.ASMAY_ActiveFlag) {
                        ASMAY_ActiveFlag = 1;

                    } else {
                        ASMAY_ActiveFlag = 0;
                    }
                    if ($scope.ASMAY_Pre_ActiveFlag) {
                        ASMAY_Pre_ActiveFlag = 1;

                    } else {
                        ASMAY_Pre_ActiveFlag = 0;
                    }

                    if ($scope.ASMAY_NewAdmissionFlg) {
                        ASMAY_NewAdmissionFlg = 1;

                    } else {
                        ASMAY_NewAdmissionFlg = 0;
                    }

                    if ($scope.ASMAY_ReggularFlg) {
                        ASMAY_ReggularFlg = 1;

                    } else {
                        ASMAY_ReggularFlg = 0;
                    }

                    if ($scope.ASMAY_NewFlg) {
                        ASMAY_NewFlg = 1;

                    } else {
                        ASMAY_NewFlg = 0;
                    }

                    if ($scope.ASMAY_ReferenceDate !== null && $scope.ASMAY_ReferenceDate !== undefined && $scope.ASMAY_ReferenceDate !== "") {
                        $scope.ASMAY_ReferenceDate = new Date($scope.ASMAY_ReferenceDate).toDateString();
                    }
                    if ($scope.ASMAY_TransportSDate !== null && $scope.ASMAY_TransportSDate !== undefined && $scope.ASMAY_TransportSDate !== "") {
                        $scope.ASMAY_TransportSDate = new Date($scope.ASMAY_TransportSDate).toDateString();
                    }
                    if ($scope.ASMAY_TransportEDate !== null && $scope.ASMAY_TransportEDate !== undefined && $scope.ASMAY_TransportEDate !== "") {
                        $scope.ASMAY_TransportEDate = new Date($scope.ASMAY_TransportEDate).toDateString();
                    }
                    if ($scope.ASMAY_AdvanceFeeDate !== null && $scope.ASMAY_AdvanceFeeDate !== undefined && $scope.ASMAY_AdvanceFeeDate !== "") {
                        $scope.ASMAY_AdvanceFeeDate = new Date($scope.ASMAY_AdvanceFeeDate).toDateString();
                    }
                    if ($scope.ASMAY_ArrearFeeDate !== null && $scope.ASMAY_ArrearFeeDate !== undefined && $scope.ASMAY_ArrearFeeDate !== "") {
                        $scope.ASMAY_ArrearFeeDate = new Date($scope.ASMAY_ArrearFeeDate).toDateString();
                    }


                    var data = {
                        "MI_Id": $scope.MI_Id,
                        "ASMAY_From_Year": $scope.ASMAY_from_Year,
                        "ASMAY_To_Year": $scope.ASMAY_to_Year,
                        "ASMAY_From_Date": new Date($scope.ASMAY_From_Date).toDateString(),
                        "ASMAY_To_Date": new Date($scope.ASMAY_To_Date).toDateString(),
                        "ASMAY_PreAdm_F_Date": new Date($scope.ASMAY_PreAdm_F_Date).toDateString(),
                        "ASMAY_PreAdm_T_Date": new Date($scope.ASMAY_PreAdm_T_Date).toDateString(),
                        "ASMAY_Order": $scope.ASMAY_Order,
                        "ASMAY_ActiveFlag": ASMAY_ActiveFlag,
                        "ASMAY_Pre_ActiveFlag": ASMAY_Pre_ActiveFlag,
                        "ASMAY_Cut_Of_Date": new Date($scope.ASMAY_Cut_Of_Date).toDateString(),
                        "ASMAY_Id": $scope.Id,
                        "fhrors": ScheduleTime,
                        "thrors": ScheduleTimeTo,
                        "fminutes": ScheduleTimem,
                        "tminutes": ScheduleTimeTom,
                        "fsec": ScheduleTimesec,
                        "tsec": ScheduleTimeTosec,
                        "ASMAY_NewAdmissionFlg": ASMAY_NewAdmissionFlg,
                        "ASMAY_ReggularFlg": ASMAY_ReggularFlg,
                        "ASMAY_NewFlg": ASMAY_NewFlg,
                        "ASMAY_AcademicYearCode": $scope.ASMAY_AcademicYearCode,
                        "ASMAY_ReferenceDate": $scope.ASMAY_ReferenceDate,
                        "ASMAY_TransportSDate": $scope.ASMAY_TransportSDate,
                        "ASMAY_TransportEDate": $scope.ASMAY_TransportEDate,
                        "ASMAY_RegularFeeFDate": new Date($scope.ASMAY_RegularFeeFDate).toDateString(),
                        "ASMAY_RegularFeeTDate": new Date($scope.ASMAY_RegularFeeTDate).toDateString(),
                        "ASMAY_AdvanceFeeDate": $scope.ASMAY_AdvanceFeeDate,
                        "ASMAY_ArrearFeeDate": $scope.ASMAY_ArrearFeeDate
                    };

                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    };
                    apiService.create("Academic/", data).then(function (promise) {
                        if (promise.count === 0) {
                            swal("No Records Found");
                        }

                        if (promise.message !== "" && promise.message !== null) {
                            swal(promise.message);
                            $state.reload();
                        }
                        else {
                            if (promise.returnval === true) {
                                $scope.students = promise.academicList;
                                $scope.presentCountgrid = $scope.students.length;
                                $scope.clearid();
                                if (promise.messagesaveupdate === "Update") {
                                    swal('Record Updated Successfully');
                                    $state.reload();
                                }
                                else {
                                    swal('Record Saved Successfully');
                                    $state.reload();
                                }
                            }
                            else {
                                if (promise.messagesaveupdate === "Update") {
                                    swal('Failed To Update Record');
                                    $state.reload();
                                }
                                else {
                                    swal('Failed To Save Record');
                                    $state.reload();
                                }
                            }
                        }
                    });
                } else {
                    $scope.submitted = true;
                }
            }
        };

        // Edit
        $scope.edit = function (employee) {
            $scope.asmaY_Id = employee.asmaY_Id;
            var propsId = $scope.asmaY_Id;
            apiService.getURI("Academic/getdetails", propsId).
                then(function (promise) {

                    if (promise.geteditdetails !== null && promise.geteditdetails.length > 0) {
                        $scope.fromD1 = new Date(promise.geteditdetails[0].asmaY_From_Date);
                        $scope.toD1 = new Date(promise.geteditdetails[0].asmaY_To_Date);
                        $scope.arrearmindate = $scope.fromD1;
                        $scope.arrearmaxdate = $scope.toD1;
                    }                   

                    $scope.year = promise.academicList[0].asmaY_Year.split('-');
                    $scope.ASMAY_from_Year = $scope.year[0];
                    $scope.ASMAY_to_Year = $scope.year[1];
                    $scope.MI_Id = promise.academicList[0].mI_Id;
                    $scope.ASMAY_From_Date = new Date(promise.academicList[0].asmaY_From_Date);
                    $scope.ASMAY_To_Date = new Date(promise.academicList[0].asmaY_To_Date);

                    $scope.arrearmindated = new Date(
                        $scope.ASMAY_To_Date.getFullYear,
                        $scope.ASMAY_To_Date.getMonth,
                        $scope.ASMAY_To_Date.getDate + 1
                    );

                    $scope.ASMAY_PreAdm_F_Date = new Date(promise.academicList[0].asmaY_PreAdm_F_Date);
                    $scope.ASMAY_PreAdm_T_Date = new Date(promise.academicList[0].asmaY_PreAdm_T_Date);
                    $scope.ASMAY_Order = promise.academicList[0].asmaY_Order;
                    $scope.ASMAY_Cut_Of_Date = new Date(promise.academicList[0].asmaY_Cut_Of_Date);

                    var activeflag = promise.academicList[0].asmaY_ActiveFlag;
                    var preActive = promise.academicList[0].asmaY_Pre_ActiveFlag;

                    if (promise.academicList[0].asmaY_ActiveFlag === 1) {
                        $scope.ASMAY_ActiveFlag = true;
                    }
                    else {
                        $scope.ASMAY_ActiveFlag = false;
                    }
                    if (promise.academicList[0].asmaY_Pre_ActiveFlag === 1) {
                        $scope.ASMAY_Pre_ActiveFlag = true;
                    }
                    else {
                        $scope.ASMAY_Pre_ActiveFlag = false;
                    }

                    if (promise.academicList[0].asmaY_NewAdmissionFlg === true) {
                        $scope.ASMAY_NewAdmissionFlg = true;
                    }
                    else {
                        $scope.ASMAY_NewAdmissionFlg = false;
                    }

                    if (promise.academicList[0].asmaY_NewFlg === true) {
                        $scope.ASMAY_NewFlg = true;
                    }
                    else {
                        $scope.ASMAY_NewFlg = false;
                    }

                    if (promise.academicList[0].asmaY_ReggularFlg === true) {
                        $scope.ASMAY_ReggularFlg = true;
                    }
                    else {
                        $scope.ASMAY_ReggularFlg = false;
                    }

                    if (promise.academicList[0].asmaY_TransportSDate !== null && promise.academicList[0].asmaY_TransportSDate !== undefined && promise.academicList[0].asmaY_TransportSDate !== "") {
                        $scope.ASMAY_TransportSDate = new Date(promise.academicList[0].asmaY_TransportSDate);
                       // $scope.ASMAY_TransportSDate = promise.academicList[0].asmaY_TransportSDate;
                    }

                    if (promise.academicList[0].asmaY_TransportEDate !== null && promise.academicList[0].asmaY_TransportEDate !== undefined && promise.academicList[0].asmaY_TransportEDate !== "") {
                        $scope.ASMAY_TransportEDate = new Date(promise.academicList[0].asmaY_TransportEDate);
                       // $scope.ASMAY_TransportEDate = promise.academicList[0].asmaY_TransportEDate;
                    }

                    if (promise.academicList[0].asmaY_ReferenceDate !== null && promise.academicList[0].asmaY_ReferenceDate !== undefined && promise.academicList[0].asmaY_ReferenceDate !== "") {
                        $scope.ASMAY_ReferenceDate = promise.academicList[0].asmaY_ReferenceDate;
                    }

                    $scope.ASMAY_AcademicYearCode = promise.academicList[0].asmaY_AcademicYearCode;

                    if (promise.academicList[0].asmaY_RegularFeeFDate !== null) {
                        $scope.ASMAY_RegularFeeFDate = new Date(promise.academicList[0].asmaY_RegularFeeFDate);
                    }
                    else {
                        $scope.finacialstartdate = new Date(
                            $scope.ASMAY_from_Year,
                            "03",
                            "01"
                        );

                        $scope.ASMAY_RegularFeeFDate = new Date($scope.finacialstartdate);
                    }
                    if (promise.academicList[0].asmaY_RegularFeeTDate !== null) {
                        $scope.ASMAY_RegularFeeTDate = new Date(promise.academicList[0].asmaY_RegularFeeTDate);
                    }
                    else {
                        $scope.finacialenddate1 = new Date(
                            $scope.ASMAY_to_Year,
                            "02",
                            "31");
                        $scope.ASMAY_RegularFeeTDate = new Date($scope.finacialenddate1);
                    }
                    if (promise.academicList[0].asmaY_AdvanceFeeDate !== null) {
                        $scope.ASMAY_AdvanceFeeDate = new Date(promise.academicList[0].asmaY_AdvanceFeeDate);
                    }
                    else {
                        //dd
                    }
                    if (promise.academicList[0].asmaY_ArrearFeeDate !== null) {
                        $scope.ASMAY_ArrearFeeDate = new Date(promise.academicList[0].asmaY_ArrearFeeDate);
                    }
                    else {
                        //dd
                    }

                    $scope.totimehr = $filter('date')(promise.academicList[0].asmaY_PreAdm_F_Date, 'HH');
                    $scope.totimemin = $filter('date')(promise.academicList[0].asmaY_PreAdm_F_Date, 'mm');
                    $scope.totimesec = $filter('date')(promise.academicList[0].asmaY_PreAdm_F_Date, 'ss');

                    var fromtime = new Date();
                    fromtime.setHours($scope.totimehr);
                    fromtime.setMinutes($scope.totimemin);
                    fromtime.setSeconds($scope.totimesec);
                    $scope.fromtimebind = fromtime;
                    $scope.ScheduleTime_24 = $scope.fromtimebind;

                    $scope.totimehr = $filter('date')(promise.academicList[0].asmaY_PreAdm_T_Date, 'HH');
                    $scope.totimemin = $filter('date')(promise.academicList[0].asmaY_PreAdm_T_Date, 'mm');
                    $scope.totimesec = $filter('date')(promise.academicList[0].asmaY_PreAdm_T_Date, 'ss');
                    var totime = new Date();
                    totime.setHours($scope.totimehr);
                    totime.setMinutes($scope.totimemin);
                    totime.setSeconds($scope.totimesec);
                    $scope.totimebind = totime;
                    $scope.ScheduleTimeTo_24 = $scope.totimebind;

                });
        };

        // Deactive
        $scope.deactive = function (academicYear, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var mgs = "";
            if (academicYear.is_Active === false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Academic Year?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Academic/deactivate", academicYear).
                            then(function (promise) {

                                if (promise.message !== null) {
                                    swal(promise.message);
                                } else {
                                    $scope.students = promise.academicList;
                                    $scope.presentCountgrid = $scope.students.length;

                                    if (promise.is_Active === true) {
                                        if (promise.returnval === true) {
                                            swal('Academic Year  Deactivated Successfully');
                                        }
                                    }
                                    else if (promise.is_Active === false) {
                                        swal('Academic Year Activated Successfully');
                                    }
                                }

                                $state.reload();
                            });
                    } else {
                        swal("Cancelled");
                        $state.reload();
                    }

                });
        };

        // Delete 
        $scope.delete = function (employee, SweetAlert) {
            $scope.editEmployee = employee.asmaY_Id;
            var orgaid = $scope.editEmployee;
            swal({
                title: "Are you sure?",
                text: "Do you want to delete record !!!!!!!!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.DeleteURI("Academic/deletedetails", orgaid).
                            then(function (promise) {
                                if (promise.count === 0) {
                                    swal("No Records Found");
                                }
                                if (promise.message !== "" && promise.message !== null) {
                                    swal(promise.message);
                                }
                                else {
                                    if (promise.returnval === true) {
                                        $scope.students = promise.academicList;
                                        $scope.presentCountgrid = $scope.students.length;
                                        swal('Record Deleted Successfully');
                                    }
                                    else {
                                        swal('Failed To Delete Record');
                                    }
                                }
                            });
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                    $state.reload();
                });
        };

        // Order Details
        $scope.getyearorder = function () {

            var pageid = 2;
            apiService.getURI("Academic/getalldetails", pageid).
                then(function (promosie) {
                    if (promosie !== null) {
                        $scope.newuser2 = promosie.getallyear;
                    }
                    else {
                        swal("No Records Found");
                    }
                });
        };

        $scope.resetLists = function () {
            $scope.configA = {
                // Changed sorting within list
                onUpdate: function (evt) {
                    var itemEl = evt.item;  // dragged HTMLElement
                    // + indexes from onEnd
                }
            };
        };

        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.table1) {
                    $scope.newuser2[index].order = Number(index) + 1;
                }
            }
        };

        // Saving Order
        $scope.save = function (newuser3) {
            var data = {
                "yearorder": $scope.newuser2
                // "secorder": $scope.newuser3,
            };
            apiService.create("Academic/saveorder/", data).then(
                function (promoise) {
                    if (promoise !== null) {
                        if (promoise.returnval === true) {
                            swal("Records Updated Sucessfully");
                            $state.reload();
                            $('#myModalreadmit').modal('hide');
                        }
                        else {
                            swal("Failed to Update the Record");
                            $state.reload();
                            $('#myModalreadmit').modal('hide');
                        }
                    }
                    else {
                        swal("No Records Updated");
                        $state.reload();
                        $('#myModalreadmit').modal('hide');
                    }
                   // $scope.BindData();
                });
        };

        // Sorting
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        // Filter
        $scope.searchValue = '';

        $scope.filterValue = function (obj) {
            return ($filter('date')(obj.asmaY_From_Date, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) ||
                ($filter('date')(obj.asmaY_To_Date, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) ||
                ($filter('date')(obj.asmaY_Cut_Of_Date, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) ||
                (angular.lowercase(obj.mI_Name)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.asmaY_Year)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (JSON.stringify(obj.asmaY_Order)).indexOf($scope.searchValue) >= 0;
        };

        // Interacted
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        // Clear 
        $scope.clearid = function () {
            $state.reload();
        };

        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden = $scope.IsHidden ? false : true;
        };

        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        };

        // No Use Functions

        $scope.checkyr = function (data) {
            if (data !== null) {
                $scope.selacdtoyr = false;
                $scope.yrerr = "";
                if ($scope.ASMAY_from_Year >= $scope.ASMAY_to_Year) {
                    $scope.yrerr = "To year must be greater than From year";
                    return false;
                }
                $scope.toDate = data;
                $scope.tomon = "";
                $scope.toDay = "";


                $scope.minDatet = new Date(
                    $scope.toDate,
                    $scope.tomon,
                    $scope.toDay + 1);

                $scope.maxDatet = new Date(
                    $scope.toDate,
                    $scope.tomon,
                    $scope.toDay + 365);

                //preadmission To date

                $scope.minDatetP = new Date(
                    $scope.toDate - 1,
                    $scope.tomon,
                    $scope.toDay + 1);

                $scope.maxDatetP = new Date(
                    $scope.toDate,
                    $scope.tomon,
                    $scope.toDay + 365);

                //Preadmission Cutof date

                $scope.minDatetPC = new Date(
                    $scope.toDate - 1,
                    $scope.tomon + 1,
                    $scope.toDay);

                $scope.maxDatetPC = new Date(
                    $scope.toDate,
                    $scope.tomon,
                    $scope.toDay + 365);
            }
            else {
                $scope.selacdtoyr = true;
            }
        };

        $scope.onlyWeekendsPredicate = function (date) {
            var day = date.getDay();
            return day === 0 || day === 6;
        };

        $scope.searchByColumn = function (searchProspectus, searchColumn) {
            if (searchProspectus !== null || searchProspectus !== undefined && searchColumn !== null || searchColumn !== undefined) {
                var data = {
                    "EnteredData": searchProspectus,
                    "SearchColumn": searchColumn,
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("Academic/SearchByColumn", data)
                    .then(function (promise) {
                        if (promise.count === 0) {
                            swal("No Records Found.....!!");
                        }
                        if (promise.message !== "" && promise.message !== null) {
                            swal(promise.message);
                            $scope.students = promise.academicList;
                            $scope.presentCountgrid = $scope.students.length;
                        }
                        else {
                            $scope.searchProspectus = "";
                            $scope.students = promise.academicList;
                            $scope.presentCountgrid = $scope.students.length;
                        }
                    });
            }
            else {
                //dd
            }
        };


        $scope.printData = function () {
            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        };
        $scope.nologo = false;
        $scope.exportToExcel = function (tableId) {
            $scope.nologo = true;
            var excelname = 'Academic Year List Report.xls';
            var exportHref = Excel.tableToExcel(tableId, 'Academic Year List Report');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        //$scope.minDate = new Date(
        //$scope.ASMAY_From_Date.getFullYear(),
        //$scope.ASMAY_From_Date.getMonth() - 2,
        //$scope.ASMAY_From_Date.getDate());

        //$scope.ASMAY_From_Date = new Date(
        //$scope.ASMAY_From_Date.getFullYear(),
        //$scope.ASMAY_From_Date.getMonth() + 2,
        //$scope.ASMAY_From_Date.getDate());

        // $scope.ASMAY_PreAdm_F_Date = new Date();

        //$scope.minDate = new Date(
        //$scope.ASMAY_PreAdm_F_Date.getFullYear(),
        //$scope.ASMAY_PreAdm_F_Date.getMonth() - 2,
        //$scope.ASMAY_PreAdm_F_Date.getDate());

        //$scope.ASMAY_PreAdm_F_Date = new Date(
        //$scope.ASMAY_PreAdm_F_Date.getFullYear(),
        //$scope.ASMAY_PreAdm_F_Date.getMonth() + 2,
        //$scope.ASMAY_PreAdm_F_Date.getDate());

        //  $scope.ASMAY_To_Date = new Date();

        //$scope.minDate = new Date(
        //$scope.ASMAY_To_Date.getFullYear(),
        //$scope.ASMAY_To_Date.getMonth() - 2,
        //$scope.ASMAY_To_Date.getDate());

        //$scope.ASMAY_To_Date = new Date(
        //$scope.ASMAY_To_Date.getFullYear(),
        //$scope.ASMAY_To_Date.getMonth() + 2,
        //$scope.ASMAY_To_Date.getDate());

        //  $scope.ASMAY_PreAdm_T_Date = new Date();

        //$scope.minDate = new Date(
        //$scope.ASMAY_PreAdm_T_Date.getFullYear(),
        //$scope.ASMAY_PreAdm_T_Date.getMonth() - 2,
        //$scope.ASMAY_PreAdm_T_Date.getDate());

        //$scope.ASMAY_PreAdm_T_Date = new Date(
        //$scope.ASMAY_PreAdm_T_Date.getFullYear(),
        //$scope.ASMAY_PreAdm_T_Date.getMonth() + 2,
        //$scope.ASMAY_PreAdm_T_Date.getDate());

        //   $scope.ASMAY_Cut_Of_Date = new Date();

        //$scope.minDate = new Date(
        //$scope.ASMAY_Cut_Of_Date.getFullYear(),
        //$scope.ASMAY_Cut_Of_Date.getMonth() - 2,
        //$scope.ASMAY_Cut_Of_Date.getDate());

        //$scope.ASMAY_Cut_Of_Date = new Date(
        //$scope.ASMAY_Cut_Of_Date.getFullYear(),
        //$scope.ASMAY_Cut_Of_Date.getMonth() + 2,
        //$scope.ASMAY_Cut_Of_Date.getDate());   

        // $scope.submitted = false;

    }
})();
