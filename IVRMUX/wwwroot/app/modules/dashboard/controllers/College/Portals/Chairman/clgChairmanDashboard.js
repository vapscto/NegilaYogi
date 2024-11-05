
(function () {
    'use strict';
    angular.module('app').controller('clgChairmanDashboardController', clgChairmanDashboardController)
    clgChairmanDashboardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window', 'uiCalendarConfig']
    function clgChairmanDashboardController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window, uiCalendarConfig) {
        $scope.sms = 0;
        $scope.email = 0;
        $scope.hidecoe = false;
        $scope.totalstudent = 0;
        $scope.totalcollected = 0;
        $scope.coedata = [];
        $scope.studentstrenth = [];
        $scope.absentgraph = [];
        $scope.studentstrenth = [];
        $scope.tempcldrlst = [];
        $scope.loadbasicdata = function () {
            $scope.Todaydate = new Date();

            apiService.getDATA("clgChairmanDashboard/Getdetails").then(function (promise) {
                if (promise.smscount != null) {
                    $scope.sms = promise.smscount.length;
                }
                if (promise.emailcount != null) {
                    $scope.email = promise.emailcount.length;
                }

                $scope.studentstrenth = promise.fillstudentstrenth;
                $scope.yearlt = promise.yearlist;
                // $scope.asmaY_Id = promise.yearlist[0].asmaY_Id;
                $scope.absentgraph = promise.fillabsent;
                $scope.asmaY_Id = promise.asmaY_Id;
                $scope.feedetailsgraph = promise.fillfee;
                $scope.year = promise.yearlist[0].asmaY_Year;

                $scope.coedata = promise.coedata;
                if ($scope.coedata != "" && $scope.coedata != null) {
                    if ($scope.coedata.length > 0) {
                        $scope.hidecoe = true;
                    }
                }
                else {
                    $scope.hidecoe = false;
                }

                var total = 0;
                if ($scope.studentstrenth != null) {
                    if ($scope.studentstrenth.length > 0) {

                        for (var i = 0; i < $scope.studentstrenth.length; i++) {
                            total = total + $scope.studentstrenth[i].stud_count;
                        }
                    }
                }

                $scope.totalstudent = total;

                $scope.datagraph = [];
                if ($scope.studentstrenth != null) {

                    for (var i = 0; i < $scope.studentstrenth.length; i++) {
                        $scope.datagraph.push({ label: $scope.studentstrenth[i].AMCO_CourseName, "y": $scope.studentstrenth[i].stud_count })
                    }
                }
                console.log($scope.datagraph);

                $scope.dataabsentgraph = [];
                console.log($scope.absentgraph);
                if ($scope.absentgraph != null) {

                    for (var i = 0; i < $scope.absentgraph.length; i++) {
                        $scope.dataabsentgraph.push({ label: $scope.absentgraph[i].nameOfDesig, "y": $scope.absentgraph[i].absentee })
                    }
                }
                console.log($scope.dataabsentgraph);

                $scope.feegraphseries1 = [];
                if ($scope.feedetailsgraph != null) {

                    for (var i = 0; i < $scope.feedetailsgraph.length; i++) {
                        $scope.feegraphseries1.push({ label: $scope.feedetailsgraph[i].AMCO_CourseName, "y": $scope.feedetailsgraph[i].paid })
                    }
                }
                console.log($scope.feegraphseries1);
                var totalcollected = 0;
                $scope.feegraphseries2 = [];
                if ($scope.feedetailsgraph != null) {

                    for (var i = 0; i < $scope.feedetailsgraph.length; i++) {
                        totalcollected = totalcollected + $scope.feedetailsgraph[i].paid;
                        $scope.feegraphseries2.push({ label: $scope.feedetailsgraph[i].AMCO_CourseName, "y": $scope.feedetailsgraph[i].recived })
                    }
                }
                console.log($scope.feegraphseries2);
                $scope.totalcollected = totalcollected;
                $scope.feegraphseries3 = [];
                if ($scope.feedetailsgraph != null) {

                    for (var i = 0; i < $scope.feedetailsgraph.length; i++) {
                        $scope.feegraphseries3.push({ label: $scope.feedetailsgraph[i].AMCO_CourseName, "y": $scope.feedetailsgraph[i].ballance })
                    }
                }
                console.log($scope.feegraphseries3);


                //columnchart
                var chart = new CanvasJS.Chart("columnchart", {
                    axisX: {
                        labelFontSize: 10,
                        interval: 1,
                        labelAngle: -20,
                        labelFontColor: "black",
                        labelFontWeight: "bold"
                    },
                    axisY: {
                        labelFontSize: 12                     
                    },

                    data: [{
                            type: "column",
                            showInLegend: true,
                            dataPoints: $scope.datagraph
                        }]
                });

                chart.render();

                var chart = new CanvasJS.Chart("areachart",
                    {
                        axisX: {
                            labelFontSize: 8,
                            interval: 1,
                            labelAngle: -20,
                            labelFontColor: "black",
                            labelFontWeight: "bold"
                        },
                        axisY: {
                            labelFontColor: "black",
                            labelFontSize: 12
                        },
                        data: [{
                            type: "column",
                            showInLegend: true,
                            dataPoints: $scope.dataabsentgraph
                        }]
                    });

                chart.render();

                var chart = new CanvasJS.Chart("rangeBarChat");

                chart.options.axisX = {
                    interval: 1, labelFontSize: 10, labelAngle: -20, labelFontColor: "black",
                    labelFontWeight: "bold"
                };
                chart.options.axisY = { labelFontSize: 12 };
                // chart.options.title = { text: "Fruits sold in First & Second Quarter" };

                var series1 = { //dataSeries - first quarter
                    type: "column",
                    name: "Collected",
                    showInLegend: true
                };



                var series2 = { //dataSeries - second quarter
                    type: "column",
                    name: "Receivable",
                    showInLegend: true
                };

                var series3 = { //dataSeries - second quarter
                    type: "column",
                    name: "Balance",
                    showInLegend: true
                };


                chart.options.data = [];
                chart.options.data.push(series2);
                chart.options.data.push(series1);
                chart.options.data.push(series3);

                series2.dataPoints = $scope.feegraphseries2;
                series1.dataPoints = $scope.feegraphseries1;

                series3.dataPoints = $scope.feegraphseries3;

                chart.render();

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

                        if (curntdate >= remiderdate && curntdate <= eventenddate) {
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


                console.log($scope.tempcldrlst);

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

                if (promise.paymentNootificationCollegeChairman === 0) {
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
                        "paymentsubscriptiontype": "PaymentNootificationCollegeChairman"

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
            $window.location.href = 'http://' + HostName + '/#/app/CLGCHStudentStrength/';
        };

        //var HostName = location.host;
        $scope.showStudent1 = function () {
            $window.location.href = 'http://' + HostName + '/#/app/CLGCHOverAllFee/';
        };

        var HostName1= location.host;
        $scope.showStudent3 = function () {
            $window.location.href = 'http://' + HostName1 + '/#/app/CLGCHOverAllFee/';
        };

        var HostName2 = location.host;
        $scope.showStudent4 = function () {
            $window.location.href = 'http://' + HostName2 + '/#/app/CLGCHOverAllFee/';
        };
    }
})();