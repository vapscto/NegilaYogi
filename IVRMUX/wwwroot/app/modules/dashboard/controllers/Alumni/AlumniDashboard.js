(function () {
    'use strict';
    angular
        .module('app')
        .controller('AlumniDashboardController', AlumniDashboardController)

    AlumniDashboardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'uiCalendarConfig', 'superCache', '$window']

    function AlumniDashboardController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, uiCalendarConfig, superCache, $window) {

        $scope.tempcldrlst = [];


        //$('.carousel .item').each(function () {
        //    var next = $(this).next();
        //    if (!next.length) {
        //        next = $(this).siblings(':first');
        //    }
        //    next = next.next();
        //    if (!next.length) {
        //        next = $(this).siblings(':first');

        //    }
        //});
        $scope.currentPage = 1;
        $scope.currentPage1 = 1;
        $scope.currentPage2 = 1;
        $scope.currentPage3 = 1;
        $scope.currentPage5 = 1;
        $scope.currentPage6 = 1;

        $scope.itemsPerPage = 10;
        $scope.itemsPerPage1 = 10;
        $scope.itemsPerPage2 = 10;
        $scope.itemsPerPage3 = 10;
        $scope.itemsPerPage5 = 10;
        $scope.itemsPerPage6 = 10;








        $scope.LoadData = function () {
            var sal_list = [];
            var TT_list = [];
            apiService.getDATA("AlumniDashboard/getalldetails").then(function (promise) {

                if (promise.friendrequestlist !== null && promise.friendrequestlist.length > 0) {
                    $scope.ttcount = promise.friendrequestlist.length;
                }
                $scope.coedata = [];

                //$scope.Month_list = promise.monthName;
                if (promise.calenderlist != null && promise.calenderlist.length > 0) {
                    $scope.coedata = promise.calenderlist;

                }
                $scope.rolename = promise.rolename;
                if ($scope.rolename !== 'Alumni') {
                    $scope.show = true;
                }
                else {
                    $scope.show = false;
                }

                $scope.batch = promise.batch;
                if ($scope.batch != null && $scope.batch.length > 0) {
                    $scope.hidecoebaatch = true;
                }
                else {
                    $scope.hidecoebaatch = false;
                }

                $scope.alumnilist = promise.alumnilist;
                $scope.presentCountgrid = promise.alumnilist.length;
                if ($scope.alumnilist != null && $scope.alumnilist.length > 0) {
                    $scope.hidecoelist = true;
                }
                else {
                    $scope.hidecoelist = false;
                }

                $scope.alumnibirthday = promise.alumnibirthday;

                if ($scope.alumnibirthday !== null && $scope.alumnibirthday.length > 0) {
                    $scope.hidecoebirthday = true;


                }
                else {
                    $scope.hidecoebirthday = false;
                }

                if ($scope.coedata !== null && $scope.coedata.length > 0) {
                    $scope.hidecoe = true;
                }
                else {
                    $scope.hidecoe = false;
                }
                //birthday marqee
                var bdaylist = "";
                if (promise.birthdaylist !== null && promise.birthdaylist.length > 0) {
                    angular.forEach(promise.birthdaylist, function (bd) {
                        if (bd.almsT_FirstName === null) bd.almsT_MiddleName = "";
                        if (bd.almsT_LastName === null) bd.almsT_LastName = "";
                        if (bdaylist === "") { bdaylist = bd.almsT_FirstName + " " + bd.almsT_MiddleName + " " + bd.almsT_LastName; }
                        else {
                            bdaylist = bdaylist + ", " + bd.almsT_FirstName + " " + bd.almsT_MiddleName + " " + bd.almsT_LastName;
                        }
                    });
                }

                if (bdaylist === "") {
                    $scope.bdaystring = "";
                }
                else {
                    $scope.bdaystring = "Happy Birthday " + bdaylist;
                }

                //----------------------------------Calender
                $scope.calenderlist = promise.calenderlist;
                angular.forEach($scope.calenderlist, function (qwe) {
                    qwe.title = qwe.coemE_EventName;
                    var xyz = $filter('date')(qwe.coeE_EStartDate, "yyyy/MM/dd");
                    var abc = $filter('date')(qwe.coeE_EEndDate, "yyyy/MM/dd");
                    qwe.start = new Date(xyz);
                    $scope.tempcldrlst.push({ title: qwe.title, start: qwe.start });
                })
                if (promise.totaldonation !== null || promise.totaldonation > 0) {
                    $scope.totamount = promise.totaldonation[0].ALDON_Amount;
                }

                var chart = new CanvasJS.Chart("columnchart", {

                    axisX: {
                        labelFontSize: 12,
                    },
                    axisY: {
                        labelFontSize: 12,
                    },

                    data: [
                        {
                            type: "column",
                            showInLegend: true,
                            dataPoints: sal_list
                        }
                    ]
                });
                chart.render();
                //rangeBarChat
                var chart1 = new CanvasJS.Chart1("areachart", {

                    axisX: {
                        labelFontSize: 12,
                    },
                    axisY: {
                        labelFontSize: 12,
                    },

                    data: [
                        {
                            type: "column",
                            showInLegend: true,
                            dataPoints: TT_list,

                        }
                    ]
                });

                //chart.render();

                chart1.render();


            })
        };

        //-------------------------------------------------------PORTAL-CALENDER

        // #region PortalCalender

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
            //$scope.alertMessage = (date.title + ' was clicked ');
            swal({
                title: date.title,
                text: "Day Event!",
                imageUrl: 'https://jshsstorage.blob.core.windows.net/files/events-icon-4.jpg'
            });
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
        /* remove event */
        /*$scope.remove = function (index) {
            $scope.events.splice(index, 1);
        };*/
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
                //customButtons: {
                //    myCustomButton: {
                //        text: 'custom!',
                //        click: function () {
                //            alert('clicked the custom button!');
                //        }
                //    }
                //},
                header: {
                    left: 'title',
                    // center: 'myCustomButton',
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

        $scope.saveakpkfile = function () {
            var data = {
                "AMSY_ID": $scope.mI_Id,
            }
            apiService.create("AlumniDashboard/saveakpkfile", data).
                then(function (promise) {
                    if (promise.returnval == true) {
                        swal('File Download!');
                    }
                });
        }

        $scope.yearwiselistfunction = function (asmay) {
            var data = {
                "ASMAY_Id": asmay,
            }
            apiService.create("AlumniDashboard/yearwiselist", data).
                then(function (promise) {
                    if (promise.yearwiselist != null && promise.yearwiselist.length > 0) {
                        $scope.yearwiselist = promise.yearwiselist;
                    }
                    else {
                        $scope.yearwiselist = {};
                    }
                });
        }

        $scope.classwiselistfunction = function (asmay, classid) {
            var data = {
                "ASMAY_Id": asmay,
                "ASMCL_Id": classid
            }
            apiService.create("AlumniDashboard/classwisestudent", data).
                then(function (promise) {
                    if (promise.classwiselist != null && promise.classwiselist.length > 0) {
                        $scope.classwiselist = promise.classwiselist;
                    }
                    else {
                        $scope.classwiselist = {};
                    }
                });
        }

        $scope.gallery = function () {
            apiService.getDATA("AlumniDashboard/getgallery")
                .then(function (promise) {
                    if (promise.alumnigallerygrid != null && promise.alumnigallerygrid.length > 0) {
                        $scope.Alumnigallerygrid = promise.alumnigallerygrid;
                        $('#myModalgallery').modal('show');
                    }
                    else {
                        swal('No data Found..!!');
                    }


                });


        }
        $scope.previewimgnew = function (ww) {
            $scope.slides = [];
            $scope.viewgallery1 = [];
            $scope.viewgallery1 = $scope.viewgallery;

            $scope.slides = $scope.viewgallery1;
            $('#viewimageslide').modal('show');
        }
        $scope.direction = 'left';
        $scope.currentIndex = 0;

        $scope.setCurrentSlideIndex = function (index) {
            $scope.direction = (index > $scope.currentIndex) ? 'left' : 'right';
            $scope.currentIndex = index;
        };

        $scope.isCurrentSlideIndex = function (index) {
            return $scope.currentIndex === index;
        };

        $scope.prevSlide = function () {
            $scope.direction = 'left';
            $scope.currentIndex = ($scope.currentIndex < $scope.slides.length - 1) ? ++$scope.currentIndex : 0;
        };

        $scope.nextSlide = function () {
            $scope.direction = 'right';
            $scope.currentIndex = ($scope.currentIndex > 0) ? --$scope.currentIndex : $scope.slides.length - 1;
        };

        //==============view data=========



        $scope.viewData = function (option) {
            $scope.attachementlist = [];
            var data = {
                "ALGA_Id": option.algA_Id

            };
            apiService.create("AlumniDashboard/viewgallery", data).then(function (promise) {
                $scope.viewgallery = promise.viewgallery;
                $('#myModalCover').modal('show');

            });
        };
        $scope.noticeboard = function () {

            var id = 1;
            apiService.getURI("AlumniDashboard/alumninotice", id).
                then(function (promise) {
                    if (promise.alumninoticeboardlist.length > 0 && promise.alumninoticeboardlist != null) {

                        $scope.alumninoticeboardlist = promise.alumninoticeboardlist;
                        $('#myModalNotice').modal('show');
                    }
                    else {
                        swal('No Data Found.');
                    }
                });
        };

        $scope.viewnotice = function (option) {
            $scope.attachementlist = [];
            var data = {
                "ALNTB_Id": option.ALNTB_Id

            };
            apiService.create("AlumniDashboard/viewnotice", data)
                .then(function (promise) {

                    if (promise.attachementlist.length > 0) {

                        $scope.attachementlist = promise.attachementlist;

                        $('#myModalCoverview').modal('show');

                    }
                    else {
                        swal("No Data Found.");

                    }

                });
        };

        $scope.previewimg_new = function (img1) {
            $scope.imagepreview = img1;
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
            else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {

                $('#preview').attr('src', $scope.imagepreview);
                $('#myimagePreview').modal('show');

            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreview)
            }
            else if ($scope.filetype2 == 'mp3') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myaudioPreview').modal('show');

            }
            else if ($scope.filetype2 == 'pdf') {

                ///=====================show pdf, img

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

                        pdfId = document.getElementById("pdfIdzz");
                        pdfId.removeChild(pdfId.childNodes[0]);
                        embed = document.createElement('embed');
                        embed.setAttribute('src', fileURL);
                        embed.setAttribute('type', 'application/pdf');
                        embed.setAttribute('width', '100%');
                        embed.setAttribute('height', '1000');
                        pdfId.appendChild(embed);
                        $('#showpdf').modal('show');



                    });
            }
            else {
                $window.open($scope.imagepreview)
            }

        };
        //==========================
        var HostName = location.host;

        $scope.showStudent = function () {

            $window.location.href = 'https://' + HostName + '/#/app/employeeStudentSearch';

        };
        $scope.showTT = function () {

            $window.location.href = 'https://' + HostName + '/#/app/employeeTimetable';

        };
        $scope.showSalary = function () {

            $window.location.href = 'https://' + HostName + '/#/app/employeeSalaryDetails';

        };

        $scope.myprofile = function () {

            $window.location.href = 'http://' + HostName + '/#/app/AlumniHomepage/';

        };

        $scope.donation = function () {

            $window.location.href = 'http://' + HostName + '/#/app/AlumniDonation/';

        };

        $scope.birthday = function () {

            $window.location.href = 'http://' + HostName + '/#/app/AlumniBirthday/';

        };
        $scope.friendrequest = function () {

            $window.location.href = 'http://' + HostName + '/#/app/Alumni_FriendRequestAccept/';

        };

        //$scope.gallery = function () {
        //} 
        $scope.notice = function () {
            $('#myModalNotice').modal('show');
        }
    };
    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });
})();