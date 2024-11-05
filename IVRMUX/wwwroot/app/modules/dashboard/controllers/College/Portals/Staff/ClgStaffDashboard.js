
(function () {
    'use strict';
    angular.module('app').controller('ClgStaffDashboardController', ClgStaffDashboardController);
    ClgStaffDashboardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'uiCalendarConfig', 'superCache', '$window'];
    function ClgStaffDashboardController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, uiCalendarConfig, superCache, $window) {

        $scope.tempcldrlst = [];
        $scope.loaddata = function () {
            apiService.getDATA("ClgStaffDashboard/getloaddata").then(function (promise) {
                if (promise.employeedetails.length > 0 && promise.employeedetails != null) {
                    $scope.empDetails = promise.employeedetails;
                    $scope.HRME_EmployeeFirstName = $scope.empDetails[0].HRME_EmployeeFirstName;
                    $scope.HRME_DOJ = $scope.empDetails[0].HRME_DOJ;
                    $scope.HRMD_DepartmentName = $scope.empDetails[0].HRMD_DepartmentName;
                    $scope.HRMDES_DesignationName = $scope.empDetails[0].HRMDES_DesignationName;
                    $scope.HRME_EmployeeCode = $scope.empDetails[0].HRME_EmployeeCode;
                    $scope.HRME_DOB = $scope.empDetails[0].HRME_DOB;
                    $scope.photo = $scope.empDetails[0].HRME_Photo;
                    $scope.noticedetailsarray = promise.noticelist;
                    $scope.studentcount = promise.studentcount;
                }
                if (promise.empmobileno.length > 0) {
                    $scope.HRME_MobileNo = promise.empmobileno[0].hrmemnO_MobileNo;
                }
                if (promise.empemailid.length > 0) {
                    $scope.HRME_EmailId = promise.empemailid[0].hrmeM_EmailId;
                }

                //===================================== COE EVENTS
                // $scope.coereportlist = promise.coereportlist;
                $scope.coereportlist1 = [];
                if (promise.coereportlist != null && promise.coereportlist != undefined && promise.coereportlist.length > 0) {
                    $scope.coereportlist = promise.coereportlist;
                }
                $scope.coereportlist1 = promise.coereportlist1;
                $scope.maincalender = [];
                if ($scope.coereportlist1.length > 0) {
                    angular.forEach($scope.coereportlist1, function (coe) {
                        $scope.curdate = new Date();
                        $scope.endDate = new Date(coe.coeE_EEndDate);
                        $scope.reminderdate = new Date(coe.coeE_ReminderDate);
                        var curntdate = $scope.curdate === null ? "" : $filter('date')($scope.curdate, "yyyy-MM-dd");
                        var eventenddate = $scope.endDate === null ? "" : $filter('date')($scope.endDate, "yyyy-MM-dd");
                        var remiderdate = $scope.reminderdate === null ? "" : $filter('date')($scope.reminderdate, "yyyy-MM-dd");

                        if (curntdate >= remiderdate && curntdate <= eventenddate) {
                            $scope.maincalender.push({
                                coemE_EventName: coe.coemE_EventName, coemE_EventDesc: coe.coemE_EventDesc, coeE_EStartDate: coe.coeE_EStartDate,
                                coeE_EEndDate: coe.coeE_EEndDate
                            });
                        }
                        else if (curntdate === eventenddate) {
                            $scope.maincalender.push({
                                coemE_EventName: coe.coemE_EventName, coemE_EventDesc: coe.coemE_EventDesc, coeE_EStartDate: coe.coeE_EStartDate,
                                coeE_EEndDate: coe.coeE_EEndDate
                            });
                        }
                    })
                }
                //======================================== PORTAL-CALENDER


                // #region PortalCalender
                //=====================================  Calender
                $scope.calenderlist = promise.calenderlist;
                if ($scope.calenderlist.length > 0) {
                    angular.forEach($scope.calenderlist, function (qwe) {
                        qwe.title = qwe.coemE_EventName;
                        var xyz = $filter('date')(qwe.coeE_EStartDate, "yyyy/MM/dd");
                        var abc = $filter('date')(qwe.coeE_EEndDate, "yyyy/MM/dd");
                        qwe.start = new Date(xyz);
                        $scope.tempcldrlst.push({ title: qwe.title, start: qwe.start });
                    });
                }

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

                if (promise.paymentNootificationCollegeStaff === 0) {
                    if (promise.getpaymentnotificationdetails !== null && promise.getpaymentnotificationdetails.length > 0) {
                        var ISMCLTPRP_InstallmentAmt = promise.getpaymentnotificationdetails[0].ISMCLTPRP_InstallmentAmt;
                        var ISMCLTPRP_InstallmentName = promise.getpaymentnotificationdetails[0].ISMCLTPRP_InstallmentName;
                        var ISMCLTPRP_PaymentDate = new Date(promise.getpaymentnotificationdetails[0].ISMCLTPRP_PaymentDate);
                        var dated = $filter('date')(new Date(ISMCLTPRP_PaymentDate), 'dd/MM/yyyy');

                        var stringdisplay = "Dear Sir/Madam,\n Digital Campus Project Bill Payment is overdue as on " + dated + " Please pay for uninterrupted service.\n If already paid kindly ignore.";

                        $scope.DeletRecord(stringdisplay);
                    }
                }
            });
        };

        $scope.DeletRecord = function (stringdisplay, SweetAlert) {
            swal({
                //html: true,
                title: "Payment Subscription !",
                //text: stringdisplay,
                text: stringdisplay,
                type: "input",
                showCancelButton: false,
                closeOnConfirm: false,
                inputPlaceholder: "Enter Remarks",
                confirmButtonText: "OK"
            }, function (inputValue) {
                if (inputValue === false) return false;
                if (inputValue === "") {
                    swal.showInputError("You need to write something!");
                    return false;
                }
                if (inputValue !== "") {
                    var data = {
                        "subscriptionremarks": inputValue,
                        "paymentsubscriptiontype": "PaymentNootificationCollegeStaff"

                    };
                    apiService.create("InstitutionUserMapping/savepaymentremarks", data).then(function (promise) {
                        if (promise !== null) {
                            swal("Remarks Captured");
                        }
                    });
                }
            });
        };

        var HostName = location.host;
        $scope.showStudent = function () {
            $window.location.href = 'https://' + HostName + '/#/app/ClgStudentSearch/11';
        };
    }
})();

