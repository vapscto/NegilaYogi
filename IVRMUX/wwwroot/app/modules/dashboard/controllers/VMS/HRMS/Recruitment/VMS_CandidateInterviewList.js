(function () {
    'use strict';
    angular
        .module('app')
        .controller('vmsCandidateInterviewListController', vmsCandidateInterviewListController);

    vmsCandidateInterviewListController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'uiCalendarConfig', 'superCache', 'dashboardService', 'uiGridExporterService', 'uiGridExporterConstants'];
    function vmsCandidateInterviewListController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, uiCalendarConfig, superCache, dashboardService, uiGridExporterService, uiGridExporterConstants) {

        $scope.viewby = 5;
        $scope.currentPage = 4;
        $scope.itemsPerPage = $scope.viewby;
        $scope.maxSize = 5;
        $scope.jobedit = {};
        $scope.Editable = false;
        $scope.tempcldrlst = [];
        $scope.EditCalander = false;

        // Datatable display
        $scope.gridOptions = {
            enableFiltering: true,
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 10,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableColumnMenu: false, enableHiding: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'hrcD_FullName', displayName: 'Candidate', enableHiding: false },
                { name: 'hrcisC_InterviewRounds', displayName: 'Interview Type', enableHiding: false },
                { name: 'hrcisC_InterviewDate', displayName: 'Interview Date', enableHiding: false },
                { name: 'hrcisC_Status', displayName: 'Status', enableHiding: false },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);" data-toggle="tooltip" title="Edit"> <i class="fa fa-pencil-square-o"  style="color:blue;" aria-hidden="true">Edit</i></a>' +
                        '<a ng-if="row.entity.hrcisC_ActiveFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);" data-toggle="tooltip" title="Activate"> Activate</a>' +
                        '<span ng-if="row.entity.hrcisC_ActiveFlg === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);" data-toggle="tooltip" title="Deactivate">  Deactivate</a><span>' +
                        '</div>'
                }
            ],
            exporterCsvFilename: 'CandidateList.csv',
            exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };

        $scope.setPage = function (pageNo) {
            $scope.currentPage = pageNo;
        };

        $scope.pageChanged = function () {
            console.log('Page changed to: ' + $scope.currentPage);
        };

        $scope.setItemsPerPage = function (num) {
            $scope.itemsPerPage = num;
            $scope.currentPage = 1;
        };

        $scope.addjob = function () {
            $state.go('app.addcandidatIntvw');
        };

        $scope.onLoadGetData = function () {
            $scope.Editable = false;
            $scope.EditCalander = false;
            var pageid = 2;
            apiService.getURI("CandidateInterviewListVMS/getallwithoutcondtn", pageid).then(function (promise) {
                if (promise.vmsCandidateInterviewList !== null && promise.vmsCandidateInterviewList.length > 0) {
                    $scope.gridOptions.data = promise.vmsCandidateInterviewList;
                    $scope.interviewcandidatelist = promise.vmsCandidateInterviewList;

                    if (promise.candidateDetailsList !== null && promise.candidateDetailsList.length > 0) {
                        $scope.candidateDetailsList = promise.candidateDetailsList;
                    }

                    if (promise.interviewerList !== null && promise.interviewerList.length > 0) {
                        $scope.interviewerList = promise.interviewerList;
                    }
                }

                $scope.calenderlist = promise.calenderlist;
                angular.forEach($scope.calenderlist, function (qwe) {
                    qwe.title = qwe.hrcD_FullName;
                    var xyz = $filter('date')(qwe.hrcisC_InterviewDateTime, "yyyy/MM/dd");
                    qwe.start = new Date(xyz);
                    $scope.tempcldrlst.push({ title: qwe.title, start: qwe.start });
                });
            });
        };

        $scope.EditData = function (jobid) {
            $scope.jobedit = jobid.hrcisC_Id;
            var pageid = $scope.jobedit;
            apiService.getURI("CandidateInterviewListVMS/editRecord", pageid).
                then(function (promise) {
                    $scope.mrfReq = promise.vmsEditValue[0];
                    $scope.Editable = true;
                    if (promise.vmsEditValue[0].hrcisC_InterviewDateTime !== null) {
                        $scope.mrfReq.hrcisC_InterviewDateTime = new Date(promise.vmsEditValue[0].hrcisC_InterviewDateTime);
                    }
                    else {
                        $scope.mrfReq.hrcisC_InterviewDateTime = null;
                    }
                });
        };

        $scope.cancel = function () {
            $scope.Editable = false;
            $scope.onLoadGetData();
        };

        $scope.showcalender = function () {
            $scope.EditCalander = true;
        };

        $scope.savejob = function () {
            if ($scope.myForm.$valid) {
                var data = $scope.mrfReq;
                apiService.create("AddCandidateInterviewVMS/", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {

                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            }

                            else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');

                            }
                            else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");
                            }
                            else if (promise.retrunMsg === "Update") {
                                swal("Record Updated Successfully..");
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                            }

                            $scope.cancel();
                        }
                    });
            }
        };

        var date = new Date();
        var d = date.getDate();
        var m = date.getMonth();
        var y = date.getFullYear();

        $scope.changeTo = 'Hungarian';
        $scope.currentView = 'month';

        $scope.events = $scope.tempcldrlst;
        $scope.eventsF = function (start, end, timezone, callback) {
            var s = new Date(start).getTime() / 1000;
            var m = new Date(start).getMonth();
            var events = [{
                title: 'Feed Me ' + m,
                start: s + (50000),
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
                    // days of week. an array of zero-based day of week integers (0=Sunday)
                    // (Monday-Thursday in this example)
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

        $scope.printvapssalary = function (vapssalary) {
            var innerContents = document.getElementById("interviewlist").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.DeletRecord = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.hrcisC_ActiveFlg == false) {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            else {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            }

            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Record..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.DeleteURI("CandidateInterviewListVMS/ActiveDeactiveRecord", data.hrcisC_Id).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.vmsCandidateInterviewList !== null && promise.vmsCandidateInterviewList.length > 0) {
                                        $scope.gridOptions.data = promise.vmsCandidateInterviewList;
                                        $scope.interviewcandidatelist = promise.vmsCandidateInterviewList;
                                    }
                                    if (promise.retrunMsg === "Activated") {
                                        swal("Record Activated successfully");
                                    }
                                    else if (promise.retrunMsg === "Deactivated") {
                                        swal("Record Deactivated successfully");
                                    }
                                    else {
                                        swal("Record Not Activated/Deactivated", 'Fail');
                                    }
                                }
                            });
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }
            );
        };
    }

})();