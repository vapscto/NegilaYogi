
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgStudentDashboardController', ClgStudentDashboardController)

    ClgStudentDashboardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'uiCalendarConfig', 'superCache', '$window']
    function ClgStudentDashboardController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, uiCalendarConfig, superCache, $window) {

        CanvasJS.addColorSet("graphcolor",
            [//colorSet Array
                "#2471A3",
                "#76D7C4",
                "#DAF7A6",
                "#FFC300",
                "#FF5733",
            ]);
        //======================================================
        $('#myCarousel').carousel({
            interval: 10000
        })
        $('#myCarouselGallery').carousel({
            interval: 5000
        })
        $('.carousel .item').each(function () {
            var next = $(this).next();
            if (!next.length) {
                next = $(this).siblings(':first');
            }
            next = next.next();
            if (!next.length) {
                next = $(this).siblings(':first');
            }
        });
        $scope.currentPage5 = 1;
        $scope.itemsPerPage5 = 10;
        $scope.currentPageS = 1;
        $scope.itemsPerPageS = 10;
        $scope.currentPage6 = 1;
        $scope.itemsPerPage6 = 10;

        $scope.tempcldrlst = [];
        $scope.loaddata = function () {
            apiService.getDATA("ClgStudentDashboard/Getdetails").then(function (promise) {
               
                //===================================== STUDENT Details
                $scope.studentdetails = promise.studentdetails;
                $scope.yearlist = promise.yearlist;
                $scope.studentbuspassdetails = promise.studentbuspassdetails;
                

                if ($scope.studentdetails.length > 0) {
                    $scope.studentname = $scope.studentdetails[0].studentname;
                    $scope.AMST_Id_session = $scope.studentdetails[0].AMCST_Id;
                    $scope.course = $scope.studentdetails[0].AMCO_CourseName;
                    $scope.branch = $scope.studentdetails[0].AMB_BranchName;
                    $scope.semester = $scope.studentdetails[0].AMSE_SEMName;
                    $scope.year = $scope.studentdetails[0].ASMAY_Year;
                    $scope.admno = $scope.studentdetails[0].AMCST_AdmNo;
                    $scope.dob = $scope.studentdetails[0].AMCST_DOB;
                    $scope.email = $scope.studentdetails[0].AMCST_emailId;
                    $scope.mobileno = $scope.studentdetails[0].AMCST_MobileNo;
                    $scope.regno = $scope.studentdetails[0].AMCST_RegistrationNo;
                    $scope.photo = $scope.studentdetails[0].AMCST_StudentPhoto;
                    $scope.address = $scope.studentdetails[0].AMCST_PerStreet;
                }
                //===================================== FEE Details
                $scope.feedetails = promise.feedetails;
                $scope.feetotal = [];
                $scope.feepaid = [];
                $scope.feebalance = [];
                $scope.total_Paid = 0.00;
                $scope.total_amount = 0.00;
                $scope.balance = 0.00;
                angular.forEach($scope.feedetails, function (fee) {
                    $scope.total_Paid += fee.paid;
                    $scope.total_amount += fee.Total;
                    $scope.balance += fee.balance;
                    $scope.feetotal.push({ label: "Fee", "y": fee.Total })
                    $scope.feepaid.push({ label: "Fee", "y": fee.paid })
                    $scope.feebalance.push({ label: "Fee", "y": fee.balance })
                })
                var chart = new CanvasJS.Chart("FeeDetailsChart", {
                    animationEnabled: true,
                    animationDuration: 3000,
                    height: 350,
                    colorSet: "graphcolor",
                    axisX: {
                        labelFontSize: 13,
                    },
                    axisY: {
                        labelFontSize: 13,
                    },

                    toolTip: {
                        shared: true
                    },
                    data: [{
                        name: "TOTAL AMOUNT",
                        showInLegend: true,
                        type: "column",
                        dataPoints: $scope.feetotal
                    },
                    {
                        name: "PAID AMOUNT",
                        showInLegend: true,
                        type: "column",
                        dataPoints: $scope.feepaid
                    },
                    {
                        name: "BALANCE",
                        showInLegend: true,
                        type: "column",
                        dataPoints: $scope.feebalance
                    }]
                });
                chart.render();


                //===================================== Notice Board Details
                $scope.noticeboard = promise.noticeboard;
                $scope.mainnotice = [];
                if ($scope.noticeboard.length > 0) {
                    angular.forEach($scope.noticeboard, function (noti) {
                        $scope.curdate = new Date();
                        $scope.noticenddate = new Date(noti.INTB_EndDate);
                        $scope.displaydate = new Date(noti.INTB_DisplayDate);
                        var curntdate = $scope.curdate == null ? "" : $filter('date')($scope.curdate, "yyyy-MM-dd");
                        var enddate = $scope.noticenddate == null ? "" : $filter('date')($scope.noticenddate, "yyyy-MM-dd");
                        var dsplydate = $scope.displaydate == null ? "" : $filter('date')($scope.displaydate, "yyyy-MM-dd");

                        if (curntdate >= dsplydate && curntdate <= enddate) {
                            $scope.mainnotice.push({ INTB_Title: noti.INTB_Title, NTB_TTSylabusFlg: noti.NTB_TTSylabusFlg, INTB_Attachment: noti.INTB_Attachment, INTB_StartDate: noti.INTB_StartDate, INTB_EndDate: noti.INTB_EndDate, INTB_FilePath: noti.INTB_FilePath });
                        }
                        else if (curntdate == enddate) {
                            $scope.mainnotice.push({ INTB_Title: noti.INTB_Title, NTB_TTSylabusFlg: noti.NTB_TTSylabusFlg, INTB_Attachment: noti.INTB_Attachment, INTB_StartDate: noti.INTB_StartDate, INTB_EndDate: noti.INTB_EndDate, INTB_FilePath: noti.INTB_FilePath });
                        }
                    })
                }

                //===================================== COE EVENTS
                $scope.coereportlist = promise.coereportlist;
                $scope.maincalender = [];
                if ($scope.coereportlist.length > 0) {
                    angular.forEach($scope.coereportlist, function (coe) {
                        $scope.curdate = new Date();
                        $scope.endDate = new Date(coe.coeE_EEndDate);
                        $scope.reminderdate = new Date(coe.coeE_ReminderDate);
                        var curntdate = $scope.curdate == null ? "" : $filter('date')($scope.curdate, "yyyy-MM-dd");
                        var eventenddate = $scope.endDate == null ? "" : $filter('date')($scope.endDate, "yyyy-MM-dd");
                        var remiderdate = $scope.reminderdate == null ? "" : $filter('date')($scope.reminderdate, "yyyy-MM-dd");
                        //curntdate > remiderdate &&
                        if (curntdate < eventenddate) {
                            $scope.maincalender.push({
                                coemE_EventName: coe.coemE_EventName, coemE_EventDesc: coe.coemE_EventDesc, coeE_EStartDate: coe.coeE_EStartDate,
                                coeE_EEndDate: coe.coeE_EEndDate
                            });
                        }
                        else if (curntdate == eventenddate) {
                            $scope.maincalender.push({
                                coemE_EventName: coe.coemE_EventName, coemE_EventDesc: coe.coemE_EventDesc, coeE_EStartDate: coe.coeE_EStartDate,
                                coeE_EEndDate: coe.coeE_EEndDate
                            });
                        }
                    })
                }

                //===================================== Attendance Details
                $scope.attendancedetails = promise.attendancedetails;
                $scope.total_ch = 0.00;
                $scope.total_p = 0.00;
                $scope.atten = 0.00;
                $scope.attendanceCH = [];
                $scope.attendanceyTP = [];
                if ($scope.attendancedetails !== null) {
                    angular.forEach($scope.attendancedetails, function (att) {
                        $scope.total_ch += parseFloat(att.CLASS_HELD);
                        $scope.total_p += parseFloat(att.TOTAL_PRESENT);
                        $scope.attendanceCH.push({ label: att.MONTH_NAME, "y": parseInt(att.CLASS_HELD) })
                        $scope.attendanceyTP.push({ label: att.MONTH_NAME, "y": parseInt(att.TOTAL_PRESENT) })
                    })
                    $scope.atten = ($scope.total_p / $scope.total_ch) * 100;

                    var chart = new CanvasJS.Chart("AttendanceDetailsChart", {
                        animationEnabled: true,
                        animationDuration: 3000,
                        height: 350,
                        colorSet: "graphcolor",
                        axisX: {
                            labelFontSize: 13,
                        },
                        axisY: {
                            labelFontSize: 13,
                        },

                        toolTip: {
                            shared: true
                        },
                        data: [{
                            name: "CLASS HELD",
                            showInLegend: true,
                            type: "column",
                            dataPoints: $scope.attendanceCH
                        },
                        {
                            name: "TOTAL PRESENT",
                            showInLegend: true,
                            type: "column",
                            dataPoints: $scope.attendanceyTP
                        }]
                    });
                    chart.render();
                }

                //======================================= LIBRARY 
                $scope.librarydetails = promise.librarydetails;
                $scope.curldate = new Date();
                $scope.curldate = $scope.curldate === null ? "" : $filter('date')($scope.curldate, "yyyy-MM-dd");
                $scope.libraryarray = [];
                if ($scope.librarydetails.length > 0) {
                    angular.forEach($scope.librarydetails, function (lib) {
                        $scope.librarydate = new Date(lib.LBTR_IssuedDate);
                        $scope.librarydate = $scope.librarydate === null ? "" : $filter('date')($scope.librarydate, "yyyy-MM-dd");

                        if ($scope.librarydate < $scope.curldate) {
                            $scope.libraryarray.push({
                                LMBANO_AccessionNo: lib.LMBANO_AccessionNo, LMB_BookTitle: lib.LMB_BookTitle, LMB_BookSubTitle: lib.LMB_BookSubTitle, LBTR_IssuedDate: lib.LBTR_IssuedDate, LMBANO_Id: lib.LMBANO_Id, LBTR_Status: lib.LBTR_Status, LBTR_DueDate: lib.LBTR_DueDate, LBTR_ReturnedDate: lib.LBTR_ReturnedDate,
                                LBTR_RenewedDate: lib.LBTR_RenewedDate, LBTR_TotalFine: lib.LBTR_TotalFine, LBTR_FineCollected: lib.LBTR_FineCollected,
                                LBTR_FineWaived: lib.LBTR_FineWaived, LBTR_Renewalcounter: lib.LBTR_Renewalcounter
                            });
                        }
                        else if ($scope.librarydate === $scope.curldate) {
                            $scope.libraryarray.push({
                                LMBANO_AccessionNo: lib.LMBANO_AccessionNo, LMB_BookTitle: lib.LMB_BookTitle, LMB_BookSubTitle: lib.LMB_BookSubTitle, LBTR_IssuedDate: lib.LBTR_IssuedDate, LMBANO_Id: lib.LMBANO_Id, LBTR_Status: lib.LBTR_Status, LBTR_DueDate: lib.LBTR_DueDate, LBTR_ReturnedDate: lib.LBTR_ReturnedDate,
                                LBTR_RenewedDate: lib.LBTR_RenewedDate, LBTR_TotalFine: lib.LBTR_TotalFine, LBTR_FineCollected: lib.LBTR_FineCollected,
                                LBTR_FineWaived: lib.LBTR_FineWaived, LBTR_Renewalcounter: lib.LBTR_Renewalcounter
                            });
                        }
                    });
                }
              
                // #region PortalCalender
                //=====================================  Calender
                $scope.calenderlist = promise.calenderlist;
                angular.forEach($scope.calenderlist, function (qwe) {
                    qwe.title = qwe.coemE_EventName;
                    var xyz = $filter('date')(qwe.coeE_EStartDate, "yyyy/MM/dd");
                    var abc = $filter('date')(qwe.coeE_EEndDate, "yyyy/MM/dd");
                    qwe.start = new Date(xyz);
                    $scope.tempcldrlst.push({ title: qwe.title, start: qwe.start });
                })

                var date = new Date();
                var d = date.getDate();
                var m = date.getMonth();
                var y = date.getFullYear();

                $scope.changeTo = 'Hungarian';
                $scope.currentView = 'month';

                /* event source that contains custom events on the scope */
                $scope.events = $scope.tempcldrlst;
                /* event source that calls a function on every view switch */
                $scope.eventsF = function (start, end, timezone, callback) {

                    var s = new Date(start).getTime() / 1000;
                    //  var e = new Date(end).getTime() / 1000;
                    var m = new Date(start).getMonth();
                    var events = [{
                        title: 'Feed Me ' + m,
                        start: s + (50000),
                        // end: s + (100000),
                        allDay: false,
                        className: ['customFeed']
                    }];
                    callback(events);
                };
                $scope.calEventsExt = {
                    color: '#f00',
                    textColor: 'yellow',
                    events: []
                };
                $scope.ev = {};
                /* alert on dayClick */
                $scope.alertOnDayClick = function (date) {
                    $scope.alertMessage = (date.toString() + ' was clicked ');
                    $scope.ev = {
                        from: date.format('YYYY-MM-DD'),
                        to: date.format('YYYY-MM-DD'),
                        title: '',
                        allDay: true
                    };
                };
                /* alert on eventClick */
                $scope.alertOnEventClick = function (date, jsEvent, view) {
                    $scope.alertMessage = (date.title + ' was clicked ');
                };
                /* alert on Drop */
                $scope.alertOnDrop = function (event, delta, revertFunc, jsEvent, ui, view) {
                    $scope.alertMessage = ('Event Dropped to make dayDelta ' + delta);
                };
                /* alert on Resize */
                $scope.alertOnResize = function (event, delta, revertFunc, jsEvent, ui, view) {
                    $scope.alertMessage = ('Event Resized to make dayDelta ' + delta);
                };
                /* add and removes an event source of choice */
                $scope.addRemoveEventSource = function (sources, source) {
                    var canAdd = 0;
                    angular.forEach(sources, function (value, key) {
                        if (sources[key] === source) {
                            sources.splice(key, 1);
                            canAdd = 1;
                        }
                    });
                    if (canAdd === 0) {
                        sources.push(source);
                    }
                };
                /* add custom event*/
                $scope.addEvent = function () {
                    $scope.events.push({
                        title: $scope.ev.title,
                        start: moment($scope.ev.from),
                        //   end: moment($scope.ev.to),
                        allDay: true,
                        className: ['openSesame']
                    });
                };
                /* Change View */
                $scope.changeView = function (view, calendar) {
                    uiCalendarConfig.calendars.myCalendar1.fullCalendar('removeEvents');
                    uiCalendarConfig.calendars.myCalendar1.fullCalendar('addEventSource',
                        $scope.tempcldrlst);
                };
                /* Change View */
                $scope.renderCalender = function (calendar) {
                    $timeout(function () {
                        if (uiCalendarConfig.calendars[calendar]) {
                            uiCalendarConfig.calendars[calendar].fullCalendar('render');
                        }
                    });
                };
                /* Render Tooltip */
                $scope.eventRender = function (event, element, view) { };
                /* config object */
                $scope.uiConfig = {
                    calendar: {
                        height: 325,

                        editable: false,
                        viewRender: $scope.changeView,

                        header: {
                            left: 'title',
                            right: 'today prev,next'
                        },
                        dayClick: $scope.alertOnDayClick,
                        eventClick: $scope.alertOnEventClick,
                        eventDrop: $scope.alertOnDrop,
                        eventResize: $scope.alertOnResize,
                        eventRender: $scope.eventRender,
                        businessHours: {
                            start: '12:00', // a start time (10am in this example)
                            //     end: '18:00', // an end time (6pm in this example)
                            dow: [1, 2, 3, 4]
                        }
                    }
                };
                /* event sources array*/
                $scope.eventSources = [$scope.events, $scope.eventsF];
                $scope.eventSources2 = [$scope.calEventsExt, $scope.eventsF, $scope.events];
                $scope.eventRender = function (event, element, view) {
                    element.attr({
                        'tooltip': event.events,
                        'tooltip-append-to-body': true
                    });
                    $compile(element)($scope);
                };
                // #endregion

            });
        }





        $scope.ViewStudentProfile = function () {
            $scope.myTabIndex2 = 0;
            $scope.year_name = "";
            $scope.viewstudentexamsubjectdetails = [];
            $scope.viewstudentwiseexamdetails = [];
            $scope.viewstudentattendancetails = [];
            $scope.viewstudentsubjectdetails = [];
            $scope.viewstudentfeedetails = [];
            $scope.studentdivlist = [];
            $scope.btnshow = false;
            $scope.showfee = true;
            var data = {
                "AMST_Id": $scope.AMST_Id_session,
                "student_staffflag": 'Student'
            };

            apiService.create("ClgStudentDashboard/ViewStudentProfile", data).then(function (promise) {

                if (promise !== null) {
                    $scope.viewstudentjoineddetails = promise.viewstudentjoineddetails;

                    if ($scope.viewstudentjoineddetails !== null && $scope.viewstudentjoineddetails.length > 0) {
                        $scope.studentname_view = $scope.viewstudentjoineddetails[0].studentname;
                        $scope.amstadmno_view = $scope.viewstudentjoineddetails[0].AMCST_AdmNo;
                        $scope.amstregno_view = $scope.viewstudentjoineddetails[0].AMCST_RegistrationNo;
                        $scope.year_view = $scope.viewstudentjoineddetails[0].ASMAY_Year;
                        $scope.class_view = $scope.viewstudentjoineddetails[0].AMCO_CourseName;
                        $scope.photo_view = $scope.viewstudentjoineddetails[0].AMCST_StudentPhoto;
                        $scope.gender_view = $scope.viewstudentjoineddetails[0].AMCST_Sex;
                        $scope.status_view = $scope.viewstudentjoineddetails[0].AMCST_SOL;
                        $scope.doa_view = new Date($scope.viewstudentjoineddetails[0].AMCST_Date);
                        $scope.dob_view = new Date($scope.viewstudentjoineddetails[0].AMCST_DOB);

                        $scope.viewstudentdetails = promise.viewstudentdetails;

                        if ($scope.viewstudentdetails !== null && $scope.viewstudentdetails.length > 0) {
                            $scope.mobile_view = $scope.viewstudentdetails[0].amcsT_MobileNo;
                            $scope.email_view = $scope.viewstudentdetails[0].amcsT_emailId;
                            $scope.stutpin = $scope.viewstudentdetails[0].amcsT_Tpin;
                            //Father Details
                            $scope.FatherName = $scope.viewstudentdetails[0].amcsT_FatherName;
                            $scope.FatherSurName = $scope.viewstudentdetails[0].amcsT_FatherSurname === null
                                || $scope.viewstudentdetails[0].amcsT_FatherSurname === "" ? "" : $scope.viewstudentdetails[0].amsT_FatherSurname;
                            $scope.Father_MobileNo = $scope.viewstudentdetails[0].amcsT_FatherMobleNo;
                            $scope.Father_EmailId = $scope.viewstudentdetails[0].amcsT_FatheremailId;
                            $scope.Father_photo = $scope.viewstudentdetails[0].amcsT_FatherPhoto;


                            //Mother Details
                            $scope.MotherName = $scope.viewstudentdetails[0].amcsT_MotherName;
                            $scope.MotherSurName = $scope.viewstudentdetails[0].amcsT_MotherSurname === null || $scope.viewstudentdetails[0].amcsT_MotherSurname === "" || $scope.viewstudentdetails[0].amcsT_MotherSurname === "0" ? "" : ' ' + $scope.viewstudentdetails[0].amcsT_MotherSurname;
                            $scope.Mother_MobileNo = $scope.viewstudentdetails[0].amcsT_MotherMobleNo;
                            $scope.Mother_EmailId = $scope.viewstudentdetails[0].amcsT_MotheremailId;
                            $scope.Mother_photo = $scope.viewstudentdetails[0].amcsT_MotherPhoto;

                        }

                        if (promise.viewstudentacademicyeardetails !== null && promise.viewstudentacademicyeardetails.length > 0) {
                            $scope.viewstudentacademicyeardetails = promise.viewstudentacademicyeardetails;
                        }

                        if (promise.viewstudentguardiandetails !== null && promise.viewstudentguardiandetails.length > 0) {
                            $scope.viewstudentguardiandetails = promise.viewstudentguardiandetails;
                        }

                        //Over All Attendance Details
                        $scope.att_workingdays = [];
                        $scope.att_presentdays = [];
                        $scope.att_percentage = [];
                        if (promise.viewstudentattendancetails !== null && promise.viewstudentattendancetails.length > 0) {
                            $scope.viewstudentattendancetails = promise.viewstudentattendancetails;

                            angular.forEach($scope.viewstudentattendancetails, function (d) {
                                $scope.att_workingdays.push({ label: d.ASMAY_Year + '-' + d.AMCO_CourseName + '-' + d.AMB_BranchName, "y": d.WORKINGDAYS });
                                $scope.att_presentdays.push({ label: d.ASMAY_Year + '-' + d.AMCO_CourseName + '-' + d.AMB_BranchName, "y": d.PRESENTDAYS });
                                $scope.att_percentage.push({ label: d.ASMAY_Year + '-' + d.AMCO_CourseName + '-' + d.AMB_BranchName, "y": d.PERCENTAGE });
                            });

                            var chart = new CanvasJS.Chart("att_profile_chartContainer", {
                                animationEnabled: true,
                                animationDuration: 3000,
                                height: 350,
                                colorSet: "graphcolor",
                                axisX: {
                                    labelFontSize: 13,
                                },
                                axisY: {
                                    labelFontSize: 13,
                                },

                                toolTip: {
                                    shared: true
                                },
                                data: [{
                                    name: "Class Held",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.att_workingdays
                                },
                                {
                                    name: "Class Attendant",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.att_presentdays
                                },
                                {
                                    name: "Percentage",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.att_percentage
                                }]
                            });
                            chart.render();
                        }

                        // Year Month Wise Attendance Details
                        $scope.viewstudentattendanceMonthdetails = [];
                        $scope.att_Month_workingdays = [];
                        $scope.att_Month_presentdays = [];
                        if (promise.viewstudentattendanceMonthdetails !== null && promise.viewstudentattendanceMonthdetails.length > 0) {
                            $scope.viewstudentattendanceMonthdetails = promise.viewstudentattendanceMonthdetails;

                            angular.forEach($scope.viewstudentattendanceMonthdetails, function (d) {
                                $scope.att_Month_workingdays.push({ label: d.Months + '-' + d.Years, "y": d.WorkingCount });
                                $scope.att_Month_presentdays.push({ label: d.Months + '-' + d.Years, "y": d.PresentCount });
                            });

                            var chart = new CanvasJS.Chart("att_Month_profile_chartContainer", {
                                animationEnabled: true,
                                animationDuration: 3000,
                                height: 350,
                                colorSet: "graphcolor",
                                axisX: {
                                    labelFontSize: 13,
                                },
                                axisY: {
                                    labelFontSize: 13,
                                },

                                toolTip: {
                                    shared: true
                                },
                                data: [{
                                    name: "Class Held",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.att_Month_workingdays
                                },
                                {
                                    name: "Class Attendant",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.att_Month_presentdays
                                }]
                            });
                            chart.render();
                        }


                        //Subject Details For Current Year
                        if (promise.viewstudentsubjectdetails !== null && promise.viewstudentsubjectdetails.length > 0) {
                            $scope.viewstudentsubjectdetails = promise.viewstudentsubjectdetails;
                        }

                        ////Exam Details
                        //if (promise.viewstudentwiseexamdetails !== null && promise.viewstudentwiseexamdetails.length > 0) {
                        //    $scope.viewstudentwiseexamdetails = promise.viewstudentwiseexamdetails;
                        //}


                        //// Fee Details
                        //$scope.fee_yearlycharges = [];
                        //$scope.fee_Concession = [];
                        //$scope.fee_Payable = [];
                        //$scope.fee_PaidAmount = [];
                        //$scope.fee_Outstanding = [];
                        //$scope.class_feeminus = "fa-minus";
                        //if (promise.viewstudentfeedetails !== null && promise.viewstudentfeedetails.length > 0) {
                        //    $scope.viewstudentfeedetails = promise.viewstudentfeedetails;

                        //    angular.forEach($scope.viewstudentfeedetails, function (d) {
                        //        $scope.fee_yearlycharges.push({ label: d.asmay_year + '-' + d.ClassSection, "y": d.YearlyCharges });
                        //        $scope.fee_Concession.push({ label: d.asmay_year + '-' + d.ClassSection, "y": d.Concession });
                        //        $scope.fee_Payable.push({ label: d.asmay_year + '-' + d.ClassSection, "y": d.Payable });
                        //        $scope.fee_PaidAmount.push({ label: d.asmay_year + '-' + d.ClassSection, "y": d.PaidAmount });
                        //        $scope.fee_Outstanding.push({ label: d.asmay_year + '-' + d.ClassSection, "y": d.Outstanding });
                        //    });

                        //    var chart = new CanvasJS.Chart("fee_profile_chartContainer", {
                        //        animationEnabled: true,
                        //        animationDuration: 3000,
                        //        height: 350,
                        //        colorSet: "graphcolor",
                        //        axisX: {
                        //            labelFontSize: 13,
                        //        },
                        //        axisY: {
                        //            labelFontSize: 13,
                        //        },

                        //        toolTip: {
                        //            shared: true
                        //        },
                        //        data: [{
                        //            name: "Yearly Charges",
                        //            showInLegend: true,
                        //            type: "column",
                        //            dataPoints: $scope.fee_yearlycharges
                        //        },
                        //        {
                        //            name: "Concession",
                        //            showInLegend: true,
                        //            type: "column",
                        //            dataPoints: $scope.fee_Concession
                        //        },
                        //        {
                        //            name: "Payable",
                        //            showInLegend: true,
                        //            type: "column",
                        //            dataPoints: $scope.fee_Payable
                        //        },
                        //        {
                        //            name: "Paid Amount",
                        //            showInLegend: true,
                        //            type: "column",
                        //            dataPoints: $scope.fee_PaidAmount
                        //        },
                        //        {
                        //            name: "Outstanding",
                        //            showInLegend: true,
                        //            type: "column",
                        //            dataPoints: $scope.fee_Outstanding
                        //        }]
                        //    });
                        //    chart.render();
                        //}

                        //// Fee Yearly Paid Details
                        //$scope.viewstudenfeeyeardetails = [];
                        //if (promise.viewstudenfeeyeardetails !== null && promise.viewstudenfeeyeardetails.length > 0) {
                        //    $scope.viewstudenfeeyeardetails = promise.viewstudenfeeyeardetails;
                        //}

                        ////Compliants list
                        //if (promise.studentdivlist !== null && promise.studentdivlist.length > 0) {
                        //    $scope.studentdivlist = promise.studentdivlist;

                        //    angular.forEach($scope.studentdivlist, function (dd) {
                        //        if (dd.ASCOMP_FilePath !== null && dd.ASCOMP_FilePath !== "") {
                        //            var img = dd.ASCOMP_FilePath;
                        //            var imagarr = img.split('.');
                        //            var lastelement = imagarr[imagarr.length - 1];
                        //            dd.filetype = lastelement;
                        //            console.log("data.filetype : " + dd.filetype);
                        //            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        //                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.ASCOMP_FilePath;
                        //            }
                        //        }
                        //    });
                        //}

                        //Address
                        if (promise.viewstudentaddressdetails !== null && promise.viewstudentaddressdetails.length > 0) {
                            $scope.viewstudentaddressdetails = promise.viewstudentaddressdetails;

                            if ($scope.viewstudentaddressdetails[0].PermanentAddress !== null && $scope.viewstudentaddressdetails[0].PermanentAddress !== "") {
                                $scope.permanentaddress = $scope.viewstudentaddressdetails[0].PermanentAddress.split(',');
                            }

                            if ($scope.viewstudentaddressdetails[0].ContactAddress !== null && $scope.viewstudentaddressdetails[0].ContactAddress !== "") {
                                $scope.communicationaddress = $scope.viewstudentaddressdetails[0].ContactAddress.split(',');
                            }
                        }

                        $('#mymodalviewdetais').modal('show');
                    }
                }
            });
        };

        //yearly attendance
        $scope.ViewMonthWiseAttendance = function (dd) {

            $scope.viewstudentattendanceMonthdetails = [];
            $scope.att_Month_workingdays = [];
            $scope.att_Month_presentdays = [];
            $scope.year_name_att = dd.ASMAY_Year;
            document.getElementById('att_Month_profile_chartContainer').innerHTML = "";
            var data = {
                "AMST_Id": $scope.AMST_Id_session,
                "ASMAY_Id": dd.ASMAY_Id,
                "student_staffflag": 'Student'
            };

            apiService.create("ClgStudentDashboard/ViewMonthWiseAttendance", data).then(function (promise) {
                if (promise !== null) {

                    if (promise.viewstudentattendanceMonthdetails !== null && promise.viewstudentattendanceMonthdetails.length > 0) {
                        $scope.viewstudentattendanceMonthdetails = promise.viewstudentattendanceMonthdetails;
                        angular.forEach($scope.viewstudentattendanceMonthdetails, function (d) {
                            $scope.att_Month_workingdays.push({ label: d.Months + '-' + d.Years, "y": d.WorkingCount });
                            $scope.att_Month_presentdays.push({ label: d.Months + '-' + d.Years, "y": d.PresentCount });
                        });

                        var chart = new CanvasJS.Chart("att_Month_profile_chartContainer", {
                            animationEnabled: true,
                            animationDuration: 3000,
                            height: 350,
                            colorSet: "graphcolor",
                            axisX: {
                                labelFontSize: 13,
                            },
                            axisY: {
                                labelFontSize: 13,
                            },

                            toolTip: {
                                shared: true
                            },
                            data: [{
                                name: "Working Days",
                                showInLegend: true,
                                type: "column",
                                dataPoints: $scope.att_Month_workingdays
                            },
                            {
                                name: "Present Days",
                                showInLegend: true,
                                type: "column",
                                dataPoints: $scope.att_Month_presentdays
                            }]
                        });
                        chart.render();

                        $('#myModalSP').animate({ scrollTop: 500 }, 'slow');
                    }
                }
            });
        };


        $scope.viewData = function (option) {
            $scope.attachementlist = [];
            var data = {
                "INTB_Id": option.intB_Id

            };
            apiService.create("StudentDashboard/viewnotice", data).then(function (promise) {
                if (promise.attachementlist.length > 0) {
                    //    $scope.attachementlist1 = [];
                    //    angular.forEach(promise.attachementlist, function (qq) {
                    //        for (i = 0; i < $scope.attachementlist.length; i++) {

                    //            if ($scope.attachementlist[i].intbfL_FilePath !== "") {

                    //        $scope.img = qq.intbfL_FilePath;
                    //        if ($scope.img != null) {
                    //            var imagarr = $scope.img.split('.');
                    //            var lastelement = imagarr[imagarr.length - 1];

                    //            $scope.filetype2 = lastelement;
                    //        }

                    //        if ($scope.filetype2 == 'mp4' || $scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp3'
                    //            || $scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png' ||
                    //            $scope.filetype2 == 'pdf' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx'
                    //            || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx' || $scope.filetype2 == 'ppsx'
                    //            || $scope.filetype2 == 'doc' || $scope.filetype2 == 'docx') {
                    //            $scope.attachementlist1.push({
                    //                INTBFL_FileName: qq.intbfL_FileName,
                    //                INTBFL_FilePath: qq.intbfL_FilePath,
                    //                INTB_Attachment: qq.intB_Attachment,
                    //                INTB_Id: qq.intB_Id
                    //            })
                    //        }
                    //        else {
                    //            $scope.attachementlist1.push({

                    //                INTBFL_FileName: qq.intbfL_FileName,
                    //                INTBFL_FilePath: qq.intbfL_FilePath,
                    //                INTB_Attachment: 'HyperLink',
                    //                INTB_Id: qq.intB_Id

                    //            })
                    //        }
                    //    })

                    //    $scope.attachementlist = $scope.attachementlist1;

                    //    $('#myModalCoverview').modal('show');
                    //    $scope.docshowary = true;
                    //    $scope.docshow = false;
                    //}
                    //else {
                    //    swal("No Data Found.")

                    //}





                    $scope.attachementlist = promise.attachementlist;
                    $scope.attachementlist1 = [];
                    //angular.forEach(promise.attachementlist, function (qq1) {
                    for (var i = 0; i < $scope.attachementlist.length; i++) {
                        //$scope.attachementlist[i].intbfL_FilePath !== null || && $scope.attachementlist[i].intB_Attachment !== null || $scope.attachementlist[i].intB_Attachment !== ""
                        if ($scope.attachementlist[i].intbfL_FilePath !== "") {

                            angular.forEach(promise.attachementlist, function (qq) {
                                $scope.img = qq.intbfL_FilePath;
                                if ($scope.img != null) {
                                    var imagarr = $scope.img.split('.');
                                    var lastelement = imagarr[imagarr.length - 1];

                                    $scope.filetype2 = lastelement;
                                }

                                if ($scope.filetype2 == 'mp4' || $scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp3'
                                    || $scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png' ||
                                    $scope.filetype2 == 'pdf' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx'
                                    || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx' || $scope.filetype2 == 'ppsx'
                                    || $scope.filetype2 == 'doc' || $scope.filetype2 == 'docx') {
                                    $scope.attachementlist1.push({
                                        INTBFL_FileName: qq.intbfL_FileName,
                                        INTBFL_FilePath: qq.intbfL_FilePath,
                                        INTB_Attachment: qq.intB_Attachment,
                                        INTB_Id: qq.intB_Id
                                    })
                                }
                                else {
                                    $scope.attachementlist1.push({

                                        INTBFL_FileName: qq.intbfL_FileName,
                                        INTBFL_FilePath: qq.intbfL_FilePath,
                                        INTB_Attachment: 'HyperLink',
                                        INTB_Id: qq.intB_Id

                                    })
                                }
                            })

                            $scope.attachementlist = $scope.attachementlist1;

                            $('#myModalCoverview').modal('show');
                            $scope.docshowary = true;
                            $scope.docshow = false;

                        } else {

                            swal("No Data Found.")
                        }
                    }
                }
                else {
                    swal("No Data Found.")

                }

            });


        };

        $scope.previewimg_new = function (img) {
            $('#myvideoPreview').modal('hide');
            $('#myimagePreview').modal('hide');
            $('#showpdf').modal('hide');
            $scope.imagepreview = img;
            $scope.view_videos = [];
            var img = $scope.imagepreview;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype2 = lastelement;
            }
            if ($scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp4') {

                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myvideoPreview').modal('show');

            }
            else if ($scope.filetype2 == 'mp3') {

                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myaudioPreview').modal('show');

            }
            else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {

                $('#preview').attr('src', $scope.imagepreview);
                $('#myimagePreview').modal('show');

            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreview)

            }

            else if ($scope.filetype2 == 'pdf') {
                $('#showpdf').modal('hide');
                var imagedownload1 = "";
                imagedownload1 = $scope.imagepreview;

                $http.get(imagedownload1, { responseType: 'arraybuffer' })
                    .success(function (response) {
                        var fileURL = "";
                        var file = "";
                        var embed = "";
                        var pdfId = "";
                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        $scope.content = $sce.trustAsResourceUrl(fileURL);
                        var htmlElements = '<embed id="pdfview" src="' + $scope.content + '"  style="width: 100%;" height="1000" type="application/pdf" />';
                        document.getElementById("pdfIdzz").innerHTML = htmlElements;

                        //pdfId = document.getElementById("pdfIdzz");
                        //pdfId.removeChild(pdfId.childNodes[0]);
                        //embed = document.createElement('embed');
                        //embed.setAttribute('src', fileURL);
                        //embed.setAttribute('type', 'application/pdf');
                        //embed.setAttribute('width', '100%');
                        //embed.setAttribute('height', '1000');
                        //pdfId.appendChild(embed);
                        $('#showpdf').modal('show');
                    });
            }
            else {
                $window.open($scope.imagepreview)
            }

        };


        //noticeboard
        $scope.onclick_notice = function (flag) {
            $scope.noticeboard = [];
            var data = {
                "flag": flag,
                "OnClickOrOnChange": 'OnClick',

            };
            apiService.create("ClgStudentDashboard/onclick_notice", data).then(function (promise) {
                if (promise.noticelist !== null && promise.noticelist.length > 0) {
                    $scope.noticelist = promise.noticelist;
                    $scope.noticeboard = [];
                    angular.forEach($scope.noticelist, function (nt) {
                        $scope.noticeboard.push({ INTB_Id: nt.intB_Id, INTB_Title: nt.intB_Title, ntB_TTSylabusFlg: nt.intB_TTSylabusFlg, INTB_Attachment: nt.intB_Attachment, INTB_StartDate: nt.intB_StartDate, INTB_EndDate: nt.intB_EndDate, INTB_FilePath: nt.intB_FilePath, INTB_Description: nt.intB_Description, Filecount: nt.Filecount, AMCO_Id: nt.amcO_Id, INTBCSTDV_ViewFlag: nt.INTBCSTDV_ViewFlag });
                    });
                }
                //else {
                //    swal('No Data Found..!!');
                //}
                $('#myModalNotice').modal('show');
            });
        };


        $scope.OnChangeNoticeboardYear = function (flag) {
            $scope.noticeboard = [];
            var data = {
                "flag": flag,
                "ASMAY_Id": $scope.NoticeBoardYearId,
                "OnClickOrOnChange": 'OnChange'
            };

            apiService.create("ClgStudentDashboard/onclick_notice", data).then(function (promise) {
                if (promise.noticelist !== null && promise.noticelist.length > 0) {
                    $scope.noticelist = promise.noticelist;
                    $scope.noticeboard = [];
                    angular.forEach($scope.noticelist, function (nt) {
                        $scope.noticeboard.push({ INTB_Id: nt.intB_Id, INTB_Title: nt.intB_Title, ntB_TTSylabusFlg: nt.intB_TTSylabusFlg, INTB_Attachment: nt.intB_Attachment, INTB_StartDate: nt.intB_StartDate, INTB_EndDate: nt.intB_EndDate, INTB_FilePath: nt.intB_FilePath, INTB_Description: nt.intB_Description, Filecount: nt.Filecount });
                    });
                }
                //else {
                //    swal('No Data Found..!!');
                //}
            });
        };


        $scope.viewDetail_noticeboard = function (flag) {
            //$scope.homeworklist = [];
            var data = {
                "flag": 'O',
                //"INTB_Id": flag,
                //"OnClickOrOnChange": 'OnClick'

                "INTB_Id": flag.INTB_Id,
                "OnClickOrOnChange": 'OnClick',
                "AMCO_Id": flag.AMCO_Id

            };
            apiService.create("ClgStudentDashboard/onclick_noticeboard_seen", data).then(function (promise) {
                if (promise.noticelist_byid != null && promise.noticelist_byid.length > 0) {
                    $scope.noticelist_byid = promise.noticelist_byid;
                    //$scope.noticeboard = [];
                    //angular.forEach($scope.noticelist, function (nt) {
                    //    $scope.noticeboard.push({ INTB_Id: nt.intB_Id, INTB_Title: nt.intB_Title, ntB_TTSylabusFlg: nt.intB_TTSylabusFlg, INTB_Attachment: nt.intB_Attachment, INTB_StartDate: nt.intB_StartDate, INTB_EndDate: nt.intB_EndDate, INTB_FilePath: nt.intB_FilePath, INTB_Description: nt.intB_Description, Filecount: nt.Filecount });
                    //});
                }
                //else {
                //    swal('No Data Found..!!');
                //}
                $('#myModalNotice_seen').modal('show');
            });
        };
        //
        $scope.OnChangeSyllabusYear = function (flag) {
            $scope.noticeSyllabus = [];
            var data = {
                "flag": flag,
                "OnClickOrOnChange": 'OnChange',
                "ASMAY_Id": $scope.SyllabusYearId
            };
            apiService.create("ClgStudentDashboard/onclick_syllabus", data).then(function (promise) {
                if (promise.noticelist !== null && promise.noticelist.length > 0) {
                    $scope.noticelist = promise.noticelist;
                    $scope.noticeSyllabus = [];
                    angular.forEach($scope.noticelist, function (nt) {
                        $scope.noticeSyllabus.push({ INTB_Id: nt.INTB_Id, INTB_Title: nt.INTB_Title, INTB_TTSylabusFlg: nt.INTB_TTSylabusFlg, INTB_Attachment: nt.INTB_Attachment, INTB_StartDate: nt.INTB_StartDate, INTB_EndDate: nt.INTB_EndDate, INTB_FilePath: nt.INTB_FilePath, INTB_Description: nt.INTB_Description, Filecount: nt.Filecount });
                    });
                }
                //else {
                //    swal('No Data Found..!!');
                //}
            });
        };


        $scope.onlick_buspass = function () {

            if ($scope.studentbuspassdetails.length > 0) {
                $scope.StudentName = $scope.studentbuspassdetails[0].StudentName;
                $scope.RegNo = $scope.studentbuspassdetails[0].RegNo;
                $scope.Department = $scope.studentbuspassdetails[0].Department;
                $scope.Month = $scope.studentbuspassdetails[0].Month;
                $scope.Location = $scope.studentbuspassdetails[0].Location;
                $scope.Photo = $scope.studentbuspassdetails[0].Photo;
                $scope.MI_Logo = $scope.studentbuspassdetails[0].MI_Logo;
                $scope.Route_Nmae = $scope.studentbuspassdetails[0].Route_Nmae;
                $scope.acd = $scope.studentbuspassdetails[0].acd;
                $('#Buspassdetails').modal('show');

            }
            else {
                swal('No Bus Pass Deatils Found..!!');
            }

        }

        $scope.onclick_syllabus = function (flag) {
            $scope.noticeSyllabus = [];
            var data = {
                "flag": flag,
                "OnClickOrOnChange": 'OnClick',
            };
            apiService.create("ClgStudentDashboard/onclick_syllabus", data).then(function (promise) {

                if (promise.noticelist !== null && promise.noticelist.length > 0) {
                    $scope.noticelist = promise.noticelist;
                    $scope.noticeSyllabus = [];
                    angular.forEach($scope.noticelist, function (nt) {
                        $scope.noticeSyllabus.push({ INTB_Id: nt.INTB_Id, INTB_Title: nt.INTB_Title, INTB_TTSylabusFlg: nt.INTB_TTSylabusFlg, INTB_Attachment: nt.INTB_Attachment, INTB_StartDate: nt.INTB_StartDate, INTB_EndDate: nt.INTB_EndDate, INTB_FilePath: nt.INTB_FilePath, INTB_Description: nt.INTB_Description, Filecount: nt.Filecount });
                    });
                }
                //else {
                //    swal('No Data Found..!!');
                //}
                $('#myModalSyllabus').modal('show');
            });
        };



        var HostName = location.host;
        $scope.onfee = function () {
            $window.location.href = 'https://' + HostName + '/#/app/ClgFeeDetails/';
        };
        $scope.onAttendance = function () {
            $window.location.href = 'https://' + HostName + '/#/app/ClgAttendanceDetails/';
        };
        $scope.oncoe = function () {
            $window.location.href = 'https://' + HostName + '/#/app/ClgCOE/';
        };


    };
})();

