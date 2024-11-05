(function () {
    'use strict';
    angular
        .module('app')
        .controller('StaffMeetingProfileController', StaffMeetingProfileController);
    StaffMeetingProfileController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache', '$window', '$timeout'];
    function StaffMeetingProfileController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache, $window, $timeout) {
        //object

        $scope.ttt = true;
        $scope.hostname = "";
        $scope.roomName = "";
        $scope.meetid = 0;
        $scope.showstaff = true;
        $scope.meetingst = false;
        $scope.clearallsettings = function () {
            $state.reload();
        };
        $scope.getserverdate = function () {
            var xmlHttp;
            function srvTime() {
                try {
                    //FF, Opera, Safari, Chrome
                    xmlHttp = new XMLHttpRequest();
                }
                catch (err1) {
                    //IE
                    try {
                        xmlHttp = new ActiveXObject('Msxml2.XMLHTTP');
                    }
                    catch (err2) {
                        try {
                            xmlHttp = new ActiveXObject('Microsoft.XMLHTTP');
                        }
                        catch (eerr3) {
                            //AJAX not supported, use CPU time.
                            alert("AJAX not supported");
                        }
                    }
                }
                xmlHttp.open('HEAD', window.location.href.toString(), false);
                xmlHttp.setRequestHeader("Content-Type", "text/html");
                xmlHttp.send('');
                return xmlHttp.getResponseHeader("Date");
            }
            $scope.today = srvTime();
        }
        $scope.getserverdate();
        $scope.LMSLMEET_PlannedDate = new Date($scope.today);
        var stopped;
        $scope.countdown = function () {
            stopped = $timeout(function () {
                if ($scope.meetingst === false) {
                    var pageurl = window.location.href;
                    var res = pageurl.split("/");
                    if (res.indexOf("StaffMeetingProfile") !== -1) {
                        $scope.checkmeetingstatus();
                        $scope.countdown();
                    }
                }
            }, 10000);
        };

        //Employeee
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 5;
        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = 10;
        $scope.page1 = "page1";
        $scope.page2 = "page2";
        $scope.page3 = "page3";
        $scope.page4 = "page4";

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getDATA("LiveMeetingSchedule/getempdetails").then(function (promise) {
                $scope.meetinglist = promise.meetinglist;
                $scope.vcflag = promise.vcflag;
                $scope.joinmeetinglist = promise.joinmeetinglist;
                $scope.joinedmeeting = promise.joinedmeeting;
                $scope.recordedmeetinglist = promise.recordedmeetinglist;
                //$scope.hrmE_Photo = promise.stafflist[0].hrmE_Photo;
                //$scope.HRMDES_DesignationName = promise.stafflist[0].hrmdeS_DesignationName;
                //$scope.HRME_EmployeeFirstName = promise.stafflist[0].hrmE_EmployeeFirstName;
                //$scope.HRME_DOJ = promise.stafflist[0].hrmE_DOJ;
                //$scope.HRME_DOB = promise.stafflist[0].hrmE_DOB;

                $scope.meetingclassecsub = [];
                angular.forEach($scope.meetinglist, function (mm) {
                    mm.name = $scope.HRME_EmployeeFirstName;
                    $scope.meetingclassecsub.push({ lmslmeeT_Id: mm.lmslmeeT_Id, asmcL_ClassName: mm.asmcL_ClassName, asmC_SectionName: mm.asmC_SectionName, ismS_IVRSSubjectName: mm.ismS_IVRSSubjectName });
                });

                $scope.joinclassecsub = [];
                angular.forEach($scope.joinmeetinglist, function (mm) {
                    mm.name = $scope.HRME_EmployeeFirstName;
                    $scope.joinclassecsub.push({ lmslmeeT_Id: mm.lmslmeeT_Id, asmcL_ClassName: mm.asmcL_ClassName, asmC_SectionName: mm.asmC_SectionName, ismS_IVRSSubjectName: mm.ismS_IVRSSubjectName });
                });

                angular.forEach($scope.joinmeetinglist, function (mm) {
                    mm.name = mm.hrmE_EmployeeFirstName;
                });

                if ($scope.recordedmeetinglist.length > 0) {
                    angular.forEach($scope.recordedmeetinglist, function (mm) {
                        if (mm.LMSLMEET_RecordId !== null && mm.LMSLMEET_RecordId !== "") {
                            mm.LMSLMEET_RecordId = "https://conference.vapssmartecampus.com/playback/presentation/2.0/playback.html?meetingId=" + mm.LMSLMEET_RecordId;
                        }
                    });
                }
                //if ($scope.joinmeetinglist != null && $scope.joinmeetinglist.length > 0) {
                //    if ($scope.joinedmeeting != null && $scope.joinedmeeting.length > 0) {
                //        angular.forEach($scope.joinmeetinglist, function (mm) {
                //            mm.name = mm.EmpName;
                //            mm.join = 'N';stopstreeming
                //            mm.dis = 'D';
                //            var a = 0;
                //            angular.forEach($scope.joinedmeeting, function (pp) {
                //                if (pp.lmslmeeT_Id == mm.lmslmeeT_Id) {
                //                    mm.dis = 'ND';
                //                    mm.join = 'Y';
                //                    a += 1
                //                }
                //                else {
                //                    mm.dis = 'D';
                //                    mm.join = 'N';
                //                    a += 1
                //                }
                //            });
                //            if (a == 0) {
                //                mm.dis = 'ND';
                //            }
                //        });
                //    }
                //}


                $scope.dublicatemeetings = [];
                angular.forEach($scope.meetinglist, function (mm) {
                    if ($scope.dublicatemeetings.length > 0) {
                        angular.forEach($scope.dublicatemeetings, function (mmm) {
                            if (mm.lmslmeeT_Id !== mmm.lmslmeeT_Id) {
                                $scope.dublicatemeetings.push(mm);
                            }
                        });
                    }
                    else {
                        $scope.dublicatemeetings.push(mm);
                    }
                });

                $scope.meetinglist = [];
                var newArray = [];
                angular.forEach($scope.dublicatemeetings, function (value, key) {
                    var exists = false;
                    angular.forEach(newArray, function (val2, key) {
                        if (angular.equals(value.lmslmeeT_Id, val2.lmslmeeT_Id)) { exists = true };
                    });
                    if (exists === false && value.lmslmeeT_Id !== "") { newArray.push(value); }
                });

                $scope.meetinglist = newArray;

                // MEETING AARAY TIME
                angular.forEach($scope.meetinglist, function (obj) {
                    if (obj.lmslmeeT_PlannedStartTime.substring(0, 2) > 12) {
                        obj.paotS_ScheduleTimeonly = (Number(obj.lmslmeeT_PlannedStartTime.substring(0, 2)) - 12).toString();
                        if (obj.paotS_ScheduleTimeonly.length === 1) {
                            obj.paotS_ScheduleTimeoly = '0' + obj.paotS_ScheduleTimeonly;
                            obj.lmslmeeT_PlannedStartTime = obj.paotS_ScheduleTimeoly + ':' + obj.lmslmeeT_PlannedStartTime.substring(3, 5);
                            obj.lmslmeeT_PlannedStartTime = obj.lmslmeeT_PlannedStartTime + 'PM';
                        }
                        else if (obj.paotS_ScheduleTimeonly.length > 1) {
                            obj.lmslmeeT_PlannedStartTime = obj.paotS_ScheduleTimeonly + ':' + obj.lmslmeeT_PlannedStartTime.substring(3, 5);
                            obj.lmslmeeT_PlannedStartTime = obj.lmslmeeT_PlannedStartTime + 'PM';
                        }
                    }
                    else if (obj.lmslmeeT_PlannedStartTime.substring(0, 2) == 12) {
                        obj.lmslmeeT_PlannedStartTime = obj.lmslmeeT_PlannedStartTime + 'PM';
                    }
                    else {

                        if (obj.lmslmeeT_PlannedStartTime.length == 1) {
                            obj.lmslmeeT_PlannedStartTime = '0' + obj.lmslmeeT_PlannedStartTime;
                            obj.lmslmeeT_PlannedStartTime = obj.lmslmeeT_PlannedStartTime + 'AM';
                        }
                        else {
                            obj.lmslmeeT_PlannedStartTime = obj.lmslmeeT_PlannedStartTime + 'AM';
                        }
                    }
                })

                angular.forEach($scope.meetinglist, function (obj) {
                    if (obj.lmslmeeT_PlannedEndTime.substring(0, 2) > 12) {
                        obj.paotS_ScheduleTimeToonly = (Number(obj.lmslmeeT_PlannedEndTime.substring(0, 2)) - 12).toString();
                        if (obj.paotS_ScheduleTimeToonly.length == 1) {
                            obj.paotS_ScheduleTimeTooly = '0' + obj.paotS_ScheduleTimeToonly;
                            obj.lmslmeeT_PlannedEndTime = obj.paotS_ScheduleTimeTooly + ':' + obj.lmslmeeT_PlannedEndTime.substring(3, 5);
                            obj.lmslmeeT_PlannedEndTime = obj.lmslmeeT_PlannedEndTime + 'PM';
                        }
                        else if (obj.paotS_ScheduleTimeToonly.length > 1) {
                            obj.lmslmeeT_PlannedEndTime = obj.paotS_ScheduleTimeToonly + ':' + obj.lmslmeeT_PlannedEndTime.substring(3, 5);
                            obj.lmslmeeT_PlannedEndTime = obj.lmslmeeT_PlannedEndTime + 'PM';
                        }
                    }
                    else if (obj.lmslmeeT_PlannedEndTime.substring(0, 2) == 12) {
                        obj.lmslmeeT_PlannedEndTime = obj.lmslmeeT_PlannedEndTime + 'PM';
                    }
                    else {

                        if (obj.lmslmeeT_PlannedEndTime.length == 1) {
                            obj.lmslmeeT_PlannedEndTime = '0' + obj.lmslmeeT_PlannedEndTime;
                            obj.lmslmeeT_PlannedEndTime = obj.lmslmeeT_PlannedEndTime + 'AM';
                        }
                        else {
                            obj.lmslmeeT_PlannedEndTime = obj.lmslmeeT_PlannedEndTime + 'AM';
                        }
                    }
                })

                //

                // JOINING ARRAY TIME
                angular.forEach($scope.joinmeetinglist, function (obj) {
                    if (obj.lmslmeeT_PlannedStartTime.substring(0, 2) > 12) {
                        obj.paotS_ScheduleTimeonly = (Number(obj.lmslmeeT_PlannedStartTime.substring(0, 2)) - 12).toString();
                        if (obj.paotS_ScheduleTimeonly.length === 1) {
                            obj.paotS_ScheduleTimeoly = '0' + obj.paotS_ScheduleTimeonly;
                            obj.lmslmeeT_PlannedStartTime = obj.paotS_ScheduleTimeoly + ':' + obj.lmslmeeT_PlannedStartTime.substring(3, 5);
                            obj.lmslmeeT_PlannedStartTime = obj.lmslmeeT_PlannedStartTime + 'PM';
                        }
                        else if (obj.paotS_ScheduleTimeonly.length > 1) {
                            obj.lmslmeeT_PlannedStartTime = obj.paotS_ScheduleTimeonly + ':' + obj.lmslmeeT_PlannedStartTime.substring(3, 5);
                            obj.lmslmeeT_PlannedStartTime = obj.lmslmeeT_PlannedStartTime + 'PM';
                        }
                    }
                    else if (obj.lmslmeeT_PlannedStartTime.substring(0, 2) == 12) {
                        obj.lmslmeeT_PlannedStartTime = obj.lmslmeeT_PlannedStartTime + 'PM';
                    }
                    else {

                        if (obj.lmslmeeT_PlannedStartTime.length == 1) {
                            obj.lmslmeeT_PlannedStartTime = '0' + obj.lmslmeeT_PlannedStartTime;
                            obj.lmslmeeT_PlannedStartTime = obj.lmslmeeT_PlannedStartTime + 'AM';
                        }
                        else {
                            obj.lmslmeeT_PlannedStartTime = obj.lmslmeeT_PlannedStartTime + 'AM';
                        }
                    }
                })

                angular.forEach($scope.joinmeetinglist, function (obj) {
                    if (obj.lmslmeeT_PlannedEndTime.substring(0, 2) > 12) {
                        obj.paotS_ScheduleTimeToonly = (Number(obj.lmslmeeT_PlannedEndTime.substring(0, 2)) - 12).toString();
                        if (obj.paotS_ScheduleTimeToonly.length == 1) {
                            obj.paotS_ScheduleTimeTooly = '0' + obj.paotS_ScheduleTimeToonly;
                            obj.lmslmeeT_PlannedEndTime = obj.paotS_ScheduleTimeTooly + ':' + obj.lmslmeeT_PlannedEndTime.substring(3, 5);
                            obj.lmslmeeT_PlannedEndTime = obj.lmslmeeT_PlannedEndTime + 'PM';
                        }
                        else if (obj.paotS_ScheduleTimeToonly.length > 1) {
                            obj.lmslmeeT_PlannedEndTime = obj.paotS_ScheduleTimeToonly + ':' + obj.lmslmeeT_PlannedEndTime.substring(3, 5);
                            obj.lmslmeeT_PlannedEndTime = obj.lmslmeeT_PlannedEndTime + 'PM';
                        }
                    }
                    else if (obj.lmslmeeT_PlannedEndTime.substring(0, 2) == 12) {
                        obj.lmslmeeT_PlannedEndTime = obj.lmslmeeT_PlannedEndTime + 'PM';
                    }
                    else {

                        if (obj.lmslmeeT_PlannedEndTime.length == 1) {
                            obj.lmslmeeT_PlannedEndTime = '0' + obj.lmslmeeT_PlannedEndTime;
                            obj.lmslmeeT_PlannedEndTime = obj.lmslmeeT_PlannedEndTime + 'AM';
                        }
                        else {
                            obj.lmslmeeT_PlannedEndTime = obj.lmslmeeT_PlannedEndTime + 'AM';
                        }
                    }
                })
                //

                //$scope.html = "";

                //angular.forEach($scope.meetinglist, function (mm) {

                //    $scope.date = $filter('date')(mm.lmslmeeT_PlannedEndTime, "dd/MM/yyyy");

                //    $scope.html += '<div class="row well"><div class="col-sm-12"> <label class="control-label col-sm-12"><b>MEETING ID :</b> <span style="color:red">' + mm.lmslmeeT_MeetingId + '</span></label > <label class="control-label col-sm-12"><b>TOPIC:</b><span style="color:forestgreen;font-size:12px;">' + mm.lmslmeeT_MeetingTopic + '</span></label><label class="control-label col-sm-12"> <b>DATE/TIMTE:</b>' + mm.lmslmeeT_PlannedDate + '/(' + mm.lmslmeeT_PlannedStartTime + ' TO ' + $scope.date + ')</label ><div class="col-sm-12"><button type="button" class="btn btn-info" style="float:right" ng-click="onstartmeeting(' + mm.lmslmeeT_Id + ')" onclick="loadvalues(\'' + mm.lmslmeeT_MeetingId + '\','+mm.lmslmeeT_Id+')">START</button></div></div></div>';

                //    document.getElementById('htmldiv').innerHTML = $scope.html;
                //});
                $scope.countdown();
            });
        };


        $scope.stopstreeming = function (user) {
            debugger;
            var data = {
                "LMSLMEET_Id": user,
                "HRML_LeaveType": $scope.type
            };

            $scope.ttt = false;
            apiService.create("LiveMeetingSchedule/endmainmeeting", data).then(function (promise) {
                //$scope.meetinglist = promise.meetinglist;
                if (promise.returnval == true) {

                    swal('Meeting Completed')
                    $state.reload();
                }

            });
        }
        $scope.type = '';
        $scope.meetingst = false;
        $scope.onstartmeeting = function (user) {
            //debugger;
            //var data = {
            //    "LMSLMEET_Id": user
            //};
            $scope.type = 'M';
            $scope.topic = user.lmslmeeT_MeetingTopic;
            $scope.ttt = false;
            $scope.meetingst = true;
            $scope.meetid = user.lmslmeeT_Id;
            $scope.hrmE_Id = user.hrmE_Id;
            apiService.create("LiveMeetingSchedule/onstartmeeting", user).then(function (promise) {
                if ($scope.vcflag === "BBB") {
                    $scope.murl = promise.moderatorurl;
                    window.open($scope.murl, "_self");
                }
                else if ($scope.vcflag === "Teams") {
                    $scope.murl = promise.lmslmeeT_MeetingURL;
                    window.open($scope.murl);
                };
            });
        };
        $scope.topic = "";
        $scope.joinmeeting = function (user) {
            debugger;
            $scope.ttt = false;
            $scope.meetingst = true;
            $scope.type = 'ST';
            $scope.topic = user.lmslmeeT_MeetingTopic;
            $scope.meetid = user.lmslmeeT_Id
            apiService.create("LiveMeetingSchedule/joinmeeting", user).then(function (promise) {
                if ($scope.vcflag === "BBB" && promise.meetingstatus === true) {
                    if (promise.joined === false) {
                        $scope.murl = promise.moderatorurl;
                        window.open($scope.murl, "_self");
                    } else if (promise.joined === true) {
                        swal("You have already joined meeting from differnt device/browser!");
                        $state.reload();
                    }
                }
                else if ($scope.vcflag === "Teams" || $scope.vcflag === "TEAMS") {
                    $scope.murl = promise.lmslmeeT_MeetingURL;
                    window.open($scope.murl);
                };
                if ($scope.vcflag === "BBB" && promise.meetingstatus === false) {
                    swal("Meeting currently not active/Meeting ended!");
                    $state.reload();
                }
            });
        };

        $scope.ondatechange = function () {
            $scope.meetinglist = [];
            $scope.joinmeetinglist = [];
            var fromdate1 = $scope.LMSLMEET_PlannedDate === null ? "" : $filter('date')($scope.LMSLMEET_PlannedDate, "yyyy-MM-dd");
            var data = {
                "LMSLMEET_PlannedDate": fromdate1
            };
            apiService.create("LiveMeetingSchedule/ondatechange", data).then(function (promise) {
                $scope.meetinglist = promise.meetinglist;
                $scope.joinmeetinglist = promise.joinmeetinglist;
                $scope.joinedmeeting = promise.joinedmeeting;
                $scope.meetingclassecsub = [];
                angular.forEach($scope.meetinglist, function (mm) {
                    $scope.meetingclassecsub.push({ lmslmeeT_Id: mm.lmslmeeT_Id, asmcL_ClassName: mm.asmcL_ClassName, asmC_SectionName: mm.asmC_SectionName, ismS_IVRSSubjectName: mm.ismS_IVRSSubjectName });
                });

                $scope.joinclassecsub = [];
                angular.forEach($scope.joinmeetinglist, function (mm) {
                    mm.name = $scope.HRME_EmployeeFirstName;
                    $scope.joinclassecsub.push({ lmslmeeT_Id: mm.lmslmeeT_Id, asmcL_ClassName: mm.asmcL_ClassName, asmC_SectionName: mm.asmC_SectionName, ismS_IVRSSubjectName: mm.ismS_IVRSSubjectName });
                });

                $scope.dublicatemeetings = [];
                angular.forEach($scope.meetinglist, function (mm) {
                    if ($scope.dublicatemeetings.length > 0) {
                        angular.forEach($scope.dublicatemeetings, function (mmm) {
                            if (mm.lmslmeeT_Id !== mmm.lmslmeeT_Id) {
                                $scope.dublicatemeetings.push(mm);
                            }
                        });
                    }
                    else {
                        $scope.dublicatemeetings.push(mm);
                    }
                });

                //if ($scope.joinmeetinglist != null && $scope.joinmeetinglist.length > 0) {
                //    if ($scope.joinedmeeting != null && $scope.joinedmeeting.length > 0) {
                //        angular.forEach($scope.joinmeetinglist, function (mm) {
                //            mm.name = mm.EmpName;
                //            mm.join = 'N';
                //            mm.dis = 'D';
                //            var a = 0;
                //            angular.forEach($scope.joinedmeeting, function (pp) {
                //                if (pp.lmslmeeT_Id == mm.lmslmeeT_Id) {
                //                    mm.dis = 'ND';
                //                    mm.join = 'Y';
                //                    a += 1
                //                }
                //                else {
                //                    mm.dis = 'D';
                //                    mm.join = 'N';
                //                    a += 1
                //                }
                //            });
                //            if (a == 0) {
                //                mm.dis = 'ND';
                //            }
                //        });
                //    }
                //}

                $scope.meetinglist = [];
                var newArray = [];
                angular.forEach($scope.dublicatemeetings, function (value, key) {
                    var exists = false;
                    angular.forEach(newArray, function (val2, key) {
                        if (angular.equals(value.lmslmeeT_Id, val2.lmslmeeT_Id)) { exists = true };
                    });
                    if (exists === false && value.lmslmeeT_Id !== "") { newArray.push(value); }
                });

                $scope.meetinglist = newArray;

                // MEETING AARAY TIME
                angular.forEach($scope.meetinglist, function (obj) {
                    if (obj.lmslmeeT_PlannedStartTime.substring(0, 2) > 12) {
                        obj.paotS_ScheduleTimeonly = (Number(obj.lmslmeeT_PlannedStartTime.substring(0, 2)) - 12).toString();
                        if (obj.paotS_ScheduleTimeonly.length === 1) {
                            obj.paotS_ScheduleTimeoly = '0' + obj.paotS_ScheduleTimeonly;
                            obj.lmslmeeT_PlannedStartTime = obj.paotS_ScheduleTimeoly + ':' + obj.lmslmeeT_PlannedStartTime.substring(3, 5);
                            obj.lmslmeeT_PlannedStartTime = obj.lmslmeeT_PlannedStartTime + 'PM';
                        }
                        else if (obj.paotS_ScheduleTimeonly.length > 1) {
                            obj.lmslmeeT_PlannedStartTime = obj.paotS_ScheduleTimeonly + ':' + obj.lmslmeeT_PlannedStartTime.substring(3, 5);
                            obj.lmslmeeT_PlannedStartTime = obj.lmslmeeT_PlannedStartTime + 'PM';
                        }
                    }
                    else if (obj.lmslmeeT_PlannedStartTime.substring(0, 2) == 12) {
                        obj.lmslmeeT_PlannedStartTime = obj.lmslmeeT_PlannedStartTime + 'PM';
                    }
                    else {

                        if (obj.lmslmeeT_PlannedStartTime.length == 1) {
                            obj.lmslmeeT_PlannedStartTime = '0' + obj.lmslmeeT_PlannedStartTime;
                            obj.lmslmeeT_PlannedStartTime = obj.lmslmeeT_PlannedStartTime + 'AM';
                        }
                        else {
                            obj.lmslmeeT_PlannedStartTime = obj.lmslmeeT_PlannedStartTime + 'AM';
                        }
                    }
                })

                angular.forEach($scope.meetinglist, function (obj) {
                    if (obj.lmslmeeT_PlannedEndTime.substring(0, 2) > 12) {
                        obj.paotS_ScheduleTimeToonly = (Number(obj.lmslmeeT_PlannedEndTime.substring(0, 2)) - 12).toString();
                        if (obj.paotS_ScheduleTimeToonly.length == 1) {
                            obj.paotS_ScheduleTimeTooly = '0' + obj.paotS_ScheduleTimeToonly;
                            obj.lmslmeeT_PlannedEndTime = obj.paotS_ScheduleTimeTooly + ':' + obj.lmslmeeT_PlannedEndTime.substring(3, 5);
                            obj.lmslmeeT_PlannedEndTime = obj.lmslmeeT_PlannedEndTime + 'PM';
                        }
                        else if (obj.paotS_ScheduleTimeToonly.length > 1) {
                            obj.lmslmeeT_PlannedEndTime = obj.paotS_ScheduleTimeToonly + ':' + obj.lmslmeeT_PlannedEndTime.substring(3, 5);
                            obj.lmslmeeT_PlannedEndTime = obj.lmslmeeT_PlannedEndTime + 'PM';
                        }
                    }
                    else if (obj.lmslmeeT_PlannedEndTime.substring(0, 2) == 12) {
                        obj.lmslmeeT_PlannedEndTime = obj.lmslmeeT_PlannedEndTime + 'PM';
                    }
                    else {

                        if (obj.lmslmeeT_PlannedEndTime.length == 1) {
                            obj.lmslmeeT_PlannedEndTime = '0' + obj.lmslmeeT_PlannedEndTime;
                            obj.lmslmeeT_PlannedEndTime = obj.lmslmeeT_PlannedEndTime + 'AM';
                        }
                        else {
                            obj.lmslmeeT_PlannedEndTime = obj.lmslmeeT_PlannedEndTime + 'AM';
                        }
                    }
                })

                //

                // JOINING ARRAY TIME
                angular.forEach($scope.joinmeetinglist, function (obj) {
                    if (obj.lmslmeeT_PlannedStartTime.substring(0, 2) > 12) {
                        obj.paotS_ScheduleTimeonly = (Number(obj.lmslmeeT_PlannedStartTime.substring(0, 2)) - 12).toString();
                        if (obj.paotS_ScheduleTimeonly.length === 1) {
                            obj.paotS_ScheduleTimeoly = '0' + obj.paotS_ScheduleTimeonly;
                            obj.lmslmeeT_PlannedStartTime = obj.paotS_ScheduleTimeoly + ':' + obj.lmslmeeT_PlannedStartTime.substring(3, 5);
                            obj.lmslmeeT_PlannedStartTime = obj.lmslmeeT_PlannedStartTime + 'PM';
                        }
                        else if (obj.paotS_ScheduleTimeonly.length > 1) {
                            obj.lmslmeeT_PlannedStartTime = obj.paotS_ScheduleTimeonly + ':' + obj.lmslmeeT_PlannedStartTime.substring(3, 5);
                            obj.lmslmeeT_PlannedStartTime = obj.lmslmeeT_PlannedStartTime + 'PM';
                        }
                    }
                    else if (obj.lmslmeeT_PlannedStartTime.substring(0, 2) == 12) {
                        obj.lmslmeeT_PlannedStartTime = obj.lmslmeeT_PlannedStartTime + 'PM';
                    }
                    else {

                        if (obj.lmslmeeT_PlannedStartTime.length == 1) {
                            obj.lmslmeeT_PlannedStartTime = '0' + obj.lmslmeeT_PlannedStartTime;
                            obj.lmslmeeT_PlannedStartTime = obj.lmslmeeT_PlannedStartTime + 'AM';
                        }
                        else {
                            obj.lmslmeeT_PlannedStartTime = obj.lmslmeeT_PlannedStartTime + 'AM';
                        }
                    }
                })

                angular.forEach($scope.joinmeetinglist, function (obj) {
                    if (obj.lmslmeeT_PlannedEndTime.substring(0, 2) > 12) {
                        obj.paotS_ScheduleTimeToonly = (Number(obj.lmslmeeT_PlannedEndTime.substring(0, 2)) - 12).toString();
                        if (obj.paotS_ScheduleTimeToonly.length == 1) {
                            obj.paotS_ScheduleTimeTooly = '0' + obj.paotS_ScheduleTimeToonly;
                            obj.lmslmeeT_PlannedEndTime = obj.paotS_ScheduleTimeTooly + ':' + obj.lmslmeeT_PlannedEndTime.substring(3, 5);
                            obj.lmslmeeT_PlannedEndTime = obj.lmslmeeT_PlannedEndTime + 'PM';
                        }
                        else if (obj.paotS_ScheduleTimeToonly.length > 1) {
                            obj.lmslmeeT_PlannedEndTime = obj.paotS_ScheduleTimeToonly + ':' + obj.lmslmeeT_PlannedEndTime.substring(3, 5);
                            obj.lmslmeeT_PlannedEndTime = obj.lmslmeeT_PlannedEndTime + 'PM';
                        }
                    }
                    else if (obj.lmslmeeT_PlannedEndTime.substring(0, 2) == 12) {
                        obj.lmslmeeT_PlannedEndTime = obj.lmslmeeT_PlannedEndTime + 'PM';
                    }
                    else {

                        if (obj.lmslmeeT_PlannedEndTime.length == 1) {
                            obj.lmslmeeT_PlannedEndTime = '0' + obj.lmslmeeT_PlannedEndTime;
                            obj.lmslmeeT_PlannedEndTime = obj.lmslmeeT_PlannedEndTime + 'AM';
                        }
                        else {
                            obj.lmslmeeT_PlannedEndTime = obj.lmslmeeT_PlannedEndTime + 'AM';
                        }
                    }
                })
                //
            });
        };

        $scope.checkmeetingstatus = function () {
            $scope.meetinglist = [];
            $scope.joinmeetinglist = [];
            var fromdate1 = $scope.LMSLMEET_PlannedDate === null ? "" : $filter('date')($scope.LMSLMEET_PlannedDate, "yyyy-MM-dd");
            var data = {
                "LMSLMEET_PlannedDate": fromdate1
            };
            apiService.create("LiveMeetingSchedule/ondatechange", data).then(function (promise) {
                $scope.meetinglist = promise.meetinglist;
                $scope.joinmeetinglist = promise.joinmeetinglist;
                $scope.joinedmeeting = promise.joinedmeeting;
                $scope.meetingclassecsub = [];
                angular.forEach($scope.meetinglist, function (mm) {
                    $scope.meetingclassecsub.push({ lmslmeeT_Id: mm.lmslmeeT_Id, asmcL_ClassName: mm.asmcL_ClassName, asmC_SectionName: mm.asmC_SectionName, ismS_IVRSSubjectName: mm.ismS_IVRSSubjectName });
                });

                $scope.joinclassecsub = [];
                angular.forEach($scope.joinmeetinglist, function (mm) {
                    mm.name = $scope.HRME_EmployeeFirstName;
                    $scope.joinclassecsub.push({ lmslmeeT_Id: mm.lmslmeeT_Id, asmcL_ClassName: mm.asmcL_ClassName, asmC_SectionName: mm.asmC_SectionName, ismS_IVRSSubjectName: mm.ismS_IVRSSubjectName });
                });

                $scope.dublicatemeetings = [];
                angular.forEach($scope.meetinglist, function (mm) {
                    if ($scope.dublicatemeetings.length > 0) {
                        angular.forEach($scope.dublicatemeetings, function (mmm) {
                            if (mm.lmslmeeT_Id !== mmm.lmslmeeT_Id) {
                                $scope.dublicatemeetings.push(mm);
                            }
                        });
                    }
                    else {
                        $scope.dublicatemeetings.push(mm);
                    }
                });

                //if ($scope.joinmeetinglist != null && $scope.joinmeetinglist.length > 0) {
                //    if ($scope.joinedmeeting != null && $scope.joinedmeeting.length > 0) {
                //        angular.forEach($scope.joinmeetinglist, function (mm) {
                //            mm.name = mm.EmpName;
                //            mm.join = 'N';
                //            mm.dis = 'D';
                //            var a = 0;
                //            angular.forEach($scope.joinedmeeting, function (pp) {
                //                if (pp.lmslmeeT_Id == mm.lmslmeeT_Id) {
                //                    mm.dis = 'ND';
                //                    mm.join = 'Y';
                //                    a += 1
                //                }
                //                else {
                //                    mm.dis = 'D';
                //                    mm.join = 'N';
                //                    a += 1
                //                }
                //            });
                //            if (a == 0) {
                //                mm.dis = 'ND';
                //            }
                //        });
                //    }
                //}

                $scope.meetinglist = [];
                var newArray = [];
                angular.forEach($scope.dublicatemeetings, function (value, key) {
                    var exists = false;
                    angular.forEach(newArray, function (val2, key) {
                        if (angular.equals(value.lmslmeeT_Id, val2.lmslmeeT_Id)) { exists = true };
                    });
                    if (exists === false && value.lmslmeeT_Id !== "") { newArray.push(value); }
                });

                $scope.meetinglist = newArray;

                // MEETING AARAY TIME
                angular.forEach($scope.meetinglist, function (obj) {
                    if (obj.lmslmeeT_PlannedStartTime.substring(0, 2) > 12) {
                        obj.paotS_ScheduleTimeonly = (Number(obj.lmslmeeT_PlannedStartTime.substring(0, 2)) - 12).toString();
                        if (obj.paotS_ScheduleTimeonly.length === 1) {
                            obj.paotS_ScheduleTimeoly = '0' + obj.paotS_ScheduleTimeonly;
                            obj.lmslmeeT_PlannedStartTime = obj.paotS_ScheduleTimeoly + ':' + obj.lmslmeeT_PlannedStartTime.substring(3, 5);
                            obj.lmslmeeT_PlannedStartTime = obj.lmslmeeT_PlannedStartTime + 'PM';
                        }
                        else if (obj.paotS_ScheduleTimeonly.length > 1) {
                            obj.lmslmeeT_PlannedStartTime = obj.paotS_ScheduleTimeonly + ':' + obj.lmslmeeT_PlannedStartTime.substring(3, 5);
                            obj.lmslmeeT_PlannedStartTime = obj.lmslmeeT_PlannedStartTime + 'PM';
                        }
                    }
                    else if (obj.lmslmeeT_PlannedStartTime.substring(0, 2) == 12) {
                        obj.lmslmeeT_PlannedStartTime = obj.lmslmeeT_PlannedStartTime + 'PM';
                    }
                    else {

                        if (obj.lmslmeeT_PlannedStartTime.length == 1) {
                            obj.lmslmeeT_PlannedStartTime = '0' + obj.lmslmeeT_PlannedStartTime;
                            obj.lmslmeeT_PlannedStartTime = obj.lmslmeeT_PlannedStartTime + 'AM';
                        }
                        else {
                            obj.lmslmeeT_PlannedStartTime = obj.lmslmeeT_PlannedStartTime + 'AM';
                        }
                    }
                })

                angular.forEach($scope.meetinglist, function (obj) {
                    if (obj.lmslmeeT_PlannedEndTime.substring(0, 2) > 12) {
                        obj.paotS_ScheduleTimeToonly = (Number(obj.lmslmeeT_PlannedEndTime.substring(0, 2)) - 12).toString();
                        if (obj.paotS_ScheduleTimeToonly.length == 1) {
                            obj.paotS_ScheduleTimeTooly = '0' + obj.paotS_ScheduleTimeToonly;
                            obj.lmslmeeT_PlannedEndTime = obj.paotS_ScheduleTimeTooly + ':' + obj.lmslmeeT_PlannedEndTime.substring(3, 5);
                            obj.lmslmeeT_PlannedEndTime = obj.lmslmeeT_PlannedEndTime + 'PM';
                        }
                        else if (obj.paotS_ScheduleTimeToonly.length > 1) {
                            obj.lmslmeeT_PlannedEndTime = obj.paotS_ScheduleTimeToonly + ':' + obj.lmslmeeT_PlannedEndTime.substring(3, 5);
                            obj.lmslmeeT_PlannedEndTime = obj.lmslmeeT_PlannedEndTime + 'PM';
                        }
                    }
                    else if (obj.lmslmeeT_PlannedEndTime.substring(0, 2) == 12) {
                        obj.lmslmeeT_PlannedEndTime = obj.lmslmeeT_PlannedEndTime + 'PM';
                    }
                    else {

                        if (obj.lmslmeeT_PlannedEndTime.length == 1) {
                            obj.lmslmeeT_PlannedEndTime = '0' + obj.lmslmeeT_PlannedEndTime;
                            obj.lmslmeeT_PlannedEndTime = obj.lmslmeeT_PlannedEndTime + 'AM';
                        }
                        else {
                            obj.lmslmeeT_PlannedEndTime = obj.lmslmeeT_PlannedEndTime + 'AM';
                        }
                    }
                })

                //

                // JOINING ARRAY TIME
                angular.forEach($scope.joinmeetinglist, function (obj) {
                    if (obj.lmslmeeT_PlannedStartTime.substring(0, 2) > 12) {
                        obj.paotS_ScheduleTimeonly = (Number(obj.lmslmeeT_PlannedStartTime.substring(0, 2)) - 12).toString();
                        if (obj.paotS_ScheduleTimeonly.length === 1) {
                            obj.paotS_ScheduleTimeoly = '0' + obj.paotS_ScheduleTimeonly;
                            obj.lmslmeeT_PlannedStartTime = obj.paotS_ScheduleTimeoly + ':' + obj.lmslmeeT_PlannedStartTime.substring(3, 5);
                            obj.lmslmeeT_PlannedStartTime = obj.lmslmeeT_PlannedStartTime + 'PM';
                        }
                        else if (obj.paotS_ScheduleTimeonly.length > 1) {
                            obj.lmslmeeT_PlannedStartTime = obj.paotS_ScheduleTimeonly + ':' + obj.lmslmeeT_PlannedStartTime.substring(3, 5);
                            obj.lmslmeeT_PlannedStartTime = obj.lmslmeeT_PlannedStartTime + 'PM';
                        }
                    }
                    else if (obj.lmslmeeT_PlannedStartTime.substring(0, 2) == 12) {
                        obj.lmslmeeT_PlannedStartTime = obj.lmslmeeT_PlannedStartTime + 'PM';
                    }
                    else {

                        if (obj.lmslmeeT_PlannedStartTime.length == 1) {
                            obj.lmslmeeT_PlannedStartTime = '0' + obj.lmslmeeT_PlannedStartTime;
                            obj.lmslmeeT_PlannedStartTime = obj.lmslmeeT_PlannedStartTime + 'AM';
                        }
                        else {
                            obj.lmslmeeT_PlannedStartTime = obj.lmslmeeT_PlannedStartTime + 'AM';
                        }
                    }
                })

                angular.forEach($scope.joinmeetinglist, function (obj) {
                    if (obj.lmslmeeT_PlannedEndTime.substring(0, 2) > 12) {
                        obj.paotS_ScheduleTimeToonly = (Number(obj.lmslmeeT_PlannedEndTime.substring(0, 2)) - 12).toString();
                        if (obj.paotS_ScheduleTimeToonly.length == 1) {
                            obj.paotS_ScheduleTimeTooly = '0' + obj.paotS_ScheduleTimeToonly;
                            obj.lmslmeeT_PlannedEndTime = obj.paotS_ScheduleTimeTooly + ':' + obj.lmslmeeT_PlannedEndTime.substring(3, 5);
                            obj.lmslmeeT_PlannedEndTime = obj.lmslmeeT_PlannedEndTime + 'PM';
                        }
                        else if (obj.paotS_ScheduleTimeToonly.length > 1) {
                            obj.lmslmeeT_PlannedEndTime = obj.paotS_ScheduleTimeToonly + ':' + obj.lmslmeeT_PlannedEndTime.substring(3, 5);
                            obj.lmslmeeT_PlannedEndTime = obj.lmslmeeT_PlannedEndTime + 'PM';
                        }
                    }
                    else if (obj.lmslmeeT_PlannedEndTime.substring(0, 2) == 12) {
                        obj.lmslmeeT_PlannedEndTime = obj.lmslmeeT_PlannedEndTime + 'PM';
                    }
                    else {

                        if (obj.lmslmeeT_PlannedEndTime.length == 1) {
                            obj.lmslmeeT_PlannedEndTime = '0' + obj.lmslmeeT_PlannedEndTime;
                            obj.lmslmeeT_PlannedEndTime = obj.lmslmeeT_PlannedEndTime + 'AM';
                        }
                        else {
                            obj.lmslmeeT_PlannedEndTime = obj.lmslmeeT_PlannedEndTime + 'AM';
                        }
                    }
                })
                //
            });
        };

        // retun details
        $scope.employeeDetails = [];
        $scope.searchValue = '';


        $scope.currentemployeequalificationDetails = [];
        $scope.employequalify = false;
        $scope.employeedocumentflg = false;
        $scope.employeeclasssubjectflg = false;
        $scope.qualifymsg = false;
        $scope.empdetails = {};
        $scope.empqualifydetails = {};
        //Search employee
        $scope.submitted = false;

        $scope.SearchEmployee = function () {
            $scope.employequalification = [];
            $scope.employeedocument = [];
            $scope.empdetails = {};
            $scope.DesignationName = "";
            $scope.qualifymsg = false;
            $scope.submitted = true;
            $scope.Employee.hrmE_Id = 0;
            var today = moment(new Date());
            $scope.allInOne = [];

            //if ($scope.myForm.$valid) { }
            $scope.institutionDetails = {};
            $scope.empdetails = {};

            $scope.Employee.hrmE_multiId = [];
            angular.forEach($scope.employeedropdown, function (role) {
                if (role.emple) $scope.Employee.hrmE_multiId.push(role.hrmE_Id);
            });

            var data = $scope.Employee;
            apiService.create("LiveMeetingSchedule/getEmployeedetailsBySelection", data).
                then(function (promise) {
                    if (promise.institutionDetails !== null) {
                        $scope.qualifymsg = true;
                        $scope.institutionDetails = promise.institutionDetails;

                        var instuteAddress = "";
                        if ($scope.institutionDetails.mI_Address1 !== null && $scope.institutionDetails.mI_Address1 !== "") {
                            instuteAddress = $scope.institutionDetails.mI_Address1;
                        }
                        if ($scope.institutionDetails.mI_Address2 !== null && $scope.institutionDetails.mI_Address2 !== "") {
                            instuteAddress = instuteAddress + ',' + $scope.institutionDetails.mI_Address2;
                        }
                        if ($scope.institutionDetails.mI_Address3 !== null && $scope.institutionDetails.mI_Address3 !== "") {
                            instuteAddress = instuteAddress + ',' + $scope.institutionDetails.mI_Address3;
                        }
                        $scope.CurrentInstuteAddress = instuteAddress;
                    }

                    angular.forEach(promise.arrayempsList, function (val) {
                        if (val.currentemployeeDetails !== null) {
                            val.empdetails = val.currentemployeeDetails;
                            var eventE = moment(val.empdetails.hrmE_DOB);
                            val.age = moment.duration(today.diff(eventE)).asDays() / 365;
                            //$('#blah').attr('src', val.currentemployeeDetails.hrmE_Photo);
                        }

                        if (val.employequalification !== null && val.employequalification.length > 0) {
                            val.employequalify = true;
                        }
                        else {
                            val.employequalify = false;
                        }

                        if (val.employeedocument !== null && val.employeedocument.length > 0) {
                            val.employeedocumentflg = true;
                        }
                        else {
                            val.employeedocumentflg = false;
                        }

                        if (val.employeeclasssubject !== null && val.employeeclasssubject.length > 0) {
                            val.employeeclasssubjectflg = true;
                        }
                        else {
                            val.employeeclasssubjectflg = false;
                        }
                        $scope.allInOne.push(val);
                    });
                });
            console.log($scope.allInOne);
            //}
        };

        $scope.OnEmployeeChange = function () {
            $scope.qualifymsg = false;
        };

        $scope.all_check_empl = function (empl) {
            var toggleStatus4 = empl;
            angular.forEach($scope.employeedropdown, function (itm) {
                itm.emple = toggleStatus4;
            });
        };

        $scope.searchDiv = true;
        $scope.empDiv = false;


        //Clear data
        $scope.Employee = {};
        $scope.cleardata = function () {
            $scope.Employee = {};
            $scope.employeeDetails = [];
            $scope.employequalification = [];
            $scope.employeedocument = [];
            $scope.empdetails = {};
            $scope.searchDiv = true;
            $scope.empDiv = false;
            $scope.DesignationName = "";
            $scope.groupTypeselectedAll = false;
            $scope.departmentselectedAll = false;
            $scope.designationselectedAll = false;
            $scope.submitted = false;
            $scope.employequalify = false;
            $scope.qualifymsg = false;
            $scope.designationselectedAll = false;

            $scope.UploadEmployeeProfilePic = [];
            $('#blah').removeAttr('src');
            $scope.search = "";

            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.onLoadGetData();

        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        //By group Type
        //By group Type
        $scope.GetEmployeeBygroupTypeAll = function (groupTypeselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.groupTypeselectedAll;

            angular.forEach($scope.groupTypedropdown, function (itm) {
                itm.selected = toggleStatus;
            });

            angular.forEach($scope.designationdropdown, function (itm22) {
                itm22.selected = toggleStatus;
                $scope.designationselectedAll = toggleStatus;
            });

            angular.forEach($scope.departmentdropdown, function (itm232) {
                itm232.selected = toggleStatus;
                $scope.departmentselectedAll = toggleStatus;
            });

            $scope.get_depts();
        };



        //single
        $scope.GetEmployeeBygroupType = function (groupType) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.groupTypeselectedAll = $scope.groupTypedropdown.every(function (itm) {

                return itm.selected;
            });

            $scope.get_depts();
        };

        $scope.get_depts = function () {
            var ids = [];
            angular.forEach($scope.groupTypedropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids.push(grp_t.hrmgT_Id);
                }
            });
            var data = {
                hrmgT_IdList: ids
            };
            apiService.create("LiveMeetingSchedule/get_depts", data).
                then(function (promise) {

                    if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                        $scope.departmentdropdown = promise.departmentdropdown;
                        $scope.departmentselectedAll = true;
                        $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
                    }
                });
        };






        $scope.GetEmployeeByDepartmentAll = function (departmentselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }

            var toggleStatus = $scope.departmentselectedAll;
            angular.forEach($scope.departmentdropdown, function (itm) {
                itm.selected = toggleStatus;
            });
            angular.forEach($scope.designationdropdown, function (itm1) {
                itm1.selected = toggleStatus;
                $scope.designationselectedAll = toggleStatus;

            });

            $scope.get_desig();

        };


        //By Department Single
        $scope.GetEmployeeByDepartment = function (department) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {

                return itm.selected;
            });
            $scope.get_desig();
        };

        $scope.get_desig = function () {
            var ids = [];
            angular.forEach($scope.groupTypedropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids.push(grp_t.hrmgT_Id);
                }
            });
            var ids1 = [];
            angular.forEach($scope.departmentdropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids1.push(grp_t.hrmD_Id);
                }
            });
            var data = {
                hrmgT_IdList: ids,
                hrmD_IdList: ids1
            };
            apiService.create("LiveMeetingSchedule/get_desig", data).
                then(function (promise) {
                    if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                        $scope.designationdropdown = promise.designationdropdown;
                        $scope.designationselectedAll = true;
                        $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);
                    }
                });
        };


        //By Designation
        $scope.GetEmployeeByDesignationAll = function (designationselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.designationselectedAll;
            angular.forEach($scope.designationdropdown, function (itm) {
                itm.selected = toggleStatus;

            });

        };


        //By Designation Single
        $scope.GetEmployeeByDesignation = function (designation) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.designationselectedAll = $scope.designationdropdown.every(function (itm) {

                return itm.selected;
            });
        };




        $scope.SearchEmployeeDetails = function () {

            var groupTypeselected = [];

            angular.forEach($scope.groupTypedropdown, function (itm) {
                if (itm.selected) {
                    groupTypeselected.push(itm.hrmgT_Id);//
                }
            });

            var employeeTypeselected = [];
            angular.forEach($scope.employeeTypedropdown, function (itm) {
                if (itm.selected) {
                    employeeTypeselected.push(itm.hrmeT_Id);
                }

            });

            var departmentselected = [];
            angular.forEach($scope.departmentdropdown, function (itm) {
                if (itm.selected) {
                    departmentselected.push(itm.hrmD_Id);
                }

            });


            var designationselected = [];
            angular.forEach($scope.designationdropdown, function (itm) {
                if (itm.selected) {
                    designationselected.push(itm.hrmdeS_Id);
                }

            });

            if (groupTypeselected.length === 0 && departmentselected.length === 0 && designationselected.length === 0) {
                swal('Kindly select atleast one record');
                return;
            }
            $scope.employeedropdown = [];
            var data = {
                hrmgT_IdList: groupTypeselected,
                hrmD_IdList: departmentselected,
                hrmdeS_IdList: designationselected

            };

            apiService.create("LiveMeetingSchedule/filterEmployeedetailsBySelection", data).
                then(function (promise) {

                    if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                        $scope.employeedropdown = promise.employeedropdown;
                        $scope.searchDiv = false;
                        $scope.empDiv = true;
                    }
                    else {
                        swal('No Record found to display .. !');
                        return;
                    }

                });
        };






        //$scope.printData = function () {
        //    var divToPrint = document.getElementById("Table");
        //        var newWin = window.open();
        //        newWin.document.write(divToPrint.outerHTML);
        //        newWin.print();
        //        newWin.close();
        //        // $state.reload();
        //    }

        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.showmodaldetails = function (data) {
            $('#preview').removeAttr('src');
            var filename = data.hrmedS_DocumentImageName.toString();
            var nameArray = filename.split('.');
            var extention = nameArray[nameArray.length - 1];

            if (extention === "jpg" || extention === "jpeg") {
                $('#preview').attr('src', data.hrmedS_DocumentImageName);
            }
            else if (extention === "doc" || extention === "docx") {
                $('#preview').removeAttr('src');
            }
            else if (extention === "pdf") {
                $scope.content = 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/149125/relativity.pdf';
            }
        };

        $scope.zoomin = function () {
            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth >= 750) {
                swal("Maximum zoom-in level reached.");
            } else {
                myImg.style.width = (currWidth + 50) + "px";
            }
        };

        $scope.zoomout = function () {
            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth <= 400) {
                swal("Maximum zoom-out level reached.");
            } else {
                myImg.style.width = (currWidth - 50) + "px";
            }
        };

    }


})();