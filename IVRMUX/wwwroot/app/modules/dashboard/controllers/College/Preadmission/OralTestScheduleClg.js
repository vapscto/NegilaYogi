


(function () {
    'use strict';
    angular
        .module('app')
        .controller('OralTestScheduleClgController', OralTestScheduleClgController)

    OralTestScheduleClgController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', 'Excel', '$timeout']
    function OralTestScheduleClgController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, Excel, $timeout) {

        $('.modal').on('hide.bs.modal', function (e) {
            e.stopPropagation();
            $('body').css('padding-right', '');
        });
        $('body').on('hidden.bs.modal', function () {
            if ($('.modal.in').length > 0) {
                $('body').addClass('modal-open');
            }
        });

        //Date:23-12-2016 for displaying privileges.
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $('.modal').on('hide.bs.modal', function (e) {
            e.stopPropagation();
            $('body').css('padding-right', '');
        });

        $scope.imgname = logopath;
        $scope.deleteshow = true;
        $scope.stf = false;
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
        $scope.printdatatable = [];
        $scope.printdatatablenew = [];
        $scope.newuser1 = [];
        $scope.lengthof = $scope.newuser1.length;
        $scope.coursechnage = function (courseid) {
            var data = {
                "AMCO_Id": courseid
            }
            apiService.create("OralTestScheduleClg/coursewisestudent", data).then(function (promise) {
                if (promise.oralTestScheduleclg != null && promise.oralTestScheduleclg.length > 0) {
                    $scope.newuser = promise.oralTestScheduleclg;
                    $scope.oralTestSchedulecount = promise.OralTestScheduleClgcount;
                    $scope.overallOralTestSchedulecount = promise.overallOralTestSchedulecountClg;
                    $scope.vcorallist = promise.VcOralTestScheduleClg;
                    if ($scope.vcorallist != null && $scope.vcorallist.length > 0 && $scope.newuser != null && $scope.newuser.length > 0) {
                        angular.forEach($scope.newuser, function (a) {
                            angular.forEach($scope.vcorallist, function (b) {
                                if (a.paotsC_Id === b.PAOTSC_Id) {
                                    a.vc = true;
                                }
                            })
                        })
                    };
                    if ($scope.oralTestSchedulecount != null && $scope.oralTestSchedulecount.length > 0 && $scope.newuser != null && $scope.newuser.length > 0) {
                        angular.forEach($scope.newuser, function (a) {
                            angular.forEach($scope.oralTestSchedulecount, function (b) {
                                if (a.paotsC_Id === b.PAOTSC_Id) {
                                    a.ocount = b.ocount;
                                }
                            })
                        })
                    };
                    if ($scope.oralTestSchedulecount != null && $scope.oralTestSchedulecount.length > 0) {
                        $scope.studentcount = 0;
                        angular.forEach($scope.oralTestSchedulecount, function (b) {
                            $scope.studentcount = $scope.studentcount + b.ocount;
                        })
                    };
                    angular.forEach($scope.newuser, function (obj) {
                        obj.delete = true;
                        obj.fromtime = obj.paotsC_ScheduleFromTime;
                        obj.totime = obj.paotsC_ScheduleToTime;
                        $scope.getserverdate();
                        $scope.dateof = new Date($scope.today);
                        //var alignFillDate = new Date(obj.paotS_ScheduleDate);
                        //var pickUpDate = new Date($scope.dateof);
                        //if (alignFillDate < pickUpDate) {
                        //    obj.delete = false;
                        //}

                        var alignFillDate = new Date(obj.paotsC_ScheduleDate);
                        var nameArray = obj.fromtime.split(':');
                        if (nameArray != null && nameArray.length > 0) {
                            alignFillDate.setHours(alignFillDate.getHours() + nameArray[0]);
                            alignFillDate.setMinutes(alignFillDate.getMinutes() + nameArray[1]);
                        }
                        //fromtime
                        var pickUpDate = new Date($scope.dateof);
                        if (alignFillDate < pickUpDate) {
                            obj.delete = false;
                        }
                    })
                    angular.forEach($scope.newuser, function (obj) {
                        if (obj.paotsC_ScheduleFromTime.substring(0, 2) > 12) {
                            obj.paotS_ScheduleTimeonly = (Number(obj.paotsC_ScheduleFromTime.substring(0, 2)) - 12).toString();
                            if (obj.paotS_ScheduleTimeonly.length == 1) {
                                obj.paotS_ScheduleTimeoly = '0' + obj.paotS_ScheduleTimeonly;
                                obj.paotsC_ScheduleFromTime = obj.paotS_ScheduleTimeoly + ':' + obj.paotsC_ScheduleFromTime.substring(3, 5);
                                obj.paotsC_ScheduleFromTime = obj.paotsC_ScheduleFromTime + 'PM';
                            }
                            else if (obj.paotS_ScheduleTimeonly.length > 1) {
                                obj.paotsC_ScheduleFromTime = obj.paotS_ScheduleTimeonly + ':' + obj.paotsC_ScheduleFromTime.substring(3, 5);
                                obj.paotsC_ScheduleFromTime = obj.paotsC_ScheduleFromTime + 'PM';
                            }
                        }
                        else if (obj.paotsC_ScheduleFromTime.substring(0, 2) == 12) {
                            obj.paotsC_ScheduleFromTime = obj.paotsC_ScheduleFromTime + 'PM';
                        }
                        else {

                            if (obj.paotsC_ScheduleFromTime.length == 1) {
                                obj.paotsC_ScheduleFromTime = '0' + obj.paotsC_ScheduleFromTime;
                                obj.paotsC_ScheduleFromTime = obj.paotsC_ScheduleFromTime + 'AM';
                            }
                            else {
                                obj.paotsC_ScheduleFromTime = obj.paotsC_ScheduleFromTime + 'AM';
                            }
                        }
                    })
                    angular.forEach($scope.newuser, function (obj) {
                        if (obj.paotsC_ScheduleToTime.substring(0, 2) > 12) {
                            obj.paotS_ScheduleTimeToonly = (Number(obj.paotsC_ScheduleToTime.substring(0, 2)) - 12).toString();
                            if (obj.paotS_ScheduleTimeToonly.length == 1) {
                                obj.paotS_ScheduleTimeTooly = '0' + obj.paotS_ScheduleTimeToonly;
                                obj.paotsC_ScheduleToTime = obj.paotS_ScheduleTimeTooly + ':' + obj.paotsC_ScheduleToTime.substring(3, 5);
                                obj.paotsC_ScheduleToTime = obj.paotsC_ScheduleToTime + 'PM';
                            }
                            else if (obj.paotS_ScheduleTimeToonly.length > 1) {
                                obj.paotsC_ScheduleToTime = obj.paotS_ScheduleTimeToonly + ':' + obj.paotsC_ScheduleToTime.substring(3, 5);
                                obj.paotsC_ScheduleToTime = obj.paotsC_ScheduleToTime + 'PM';
                            }
                        }
                        else if (obj.paotsC_ScheduleToTime.substring(0, 2) == 12) {
                            obj.paotsC_ScheduleToTime = obj.paotsC_ScheduleToTime + 'PM';
                        }
                        else {

                            if (obj.paotsC_ScheduleToTime.length == 1) {
                                obj.paotsC_ScheduleToTime = '0' + obj.paotsC_ScheduleToTime;
                                obj.paotsC_ScheduleToTime = obj.paotsC_ScheduleToTime + 'AM';
                            }
                            else {
                                obj.paotsC_ScheduleToTime = obj.paotsC_ScheduleToTime + 'AM';
                            }
                        }
                    })
                    if ($scope.overallOralTestSchedulecount != null && $scope.overallOralTestSchedulecount.length > 0) {
                        angular.forEach($scope.overallOralTestSchedulecount, function (obj) {
                            if (obj.ScheduleTime.substring(0, 2) > 12) {
                                obj.ScheduleTimeonly = (Number(obj.ScheduleTime.substring(0, 2)) - 12).toString();
                                if (obj.ScheduleTimeonly.length == 1) {
                                    obj.ScheduleTimeoly = '0' + obj.ScheduleTimeonly;
                                    obj.ScheduleTime = obj.ScheduleTimeoly + ':' + obj.ScheduleTime.substring(3, 5);
                                    obj.ScheduleTime = obj.ScheduleTime + 'PM';
                                }
                                else if (obj.ScheduleTimeonly.length > 1) {
                                    obj.ScheduleTime = obj.ScheduleTimeonly + ':' + obj.ScheduleTime.substring(3, 5);
                                    obj.ScheduleTime = obj.ScheduleTime + 'PM';
                                }
                            }
                            else if (obj.ScheduleTime.substring(0, 2) == 12) {
                                obj.ScheduleTime = obj.ScheduleTime + 'PM';
                            }
                            else {
                                if (obj.ScheduleTime.length == 1) {
                                    obj.ScheduleTime = '0' + obj.ScheduleTime;
                                    obj.ScheduleTime = obj.ScheduleTime + 'AM';
                                }
                                else {
                                    obj.ScheduleTime = obj.ScheduleTime + 'AM';
                                }
                            }
                        })
                        angular.forEach($scope.overallOralTestSchedulecount, function (obj) {
                            if (obj.ScheduleTimeTo.substring(0, 2) > 12) {
                                obj.ScheduleTimeToonly = (Number(obj.ScheduleTimeTo.substring(0, 2)) - 12).toString();
                                if (obj.ScheduleTimeToonly.length == 1) {
                                    obj.ScheduleTimeTooly = '0' + obj.ScheduleTimeToonly;
                                    obj.ScheduleTimeTo = obj.ScheduleTimeTooly + ':' + obj.ScheduleTimeTo.substring(3, 5);
                                    obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'PM';
                                }
                                else if (obj.ScheduleTimeToonly.length > 1) {
                                    obj.ScheduleTimeTo = obj.ScheduleTimeToonly + ':' + obj.ScheduleTimeTo.substring(3, 5);
                                    obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'PM';
                                }
                            }
                            else if (obj.ScheduleTimeTo.substring(0, 2) == 12) {
                                obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'PM';
                            }
                            else {

                                if (obj.ScheduleTimeTo.length == 1) {
                                    obj.ScheduleTimeTo = '0' + obj.ScheduleTimeTo;
                                    obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'AM';
                                }
                                else {
                                    obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'AM';
                                }
                            }
                        })

                        if ($scope.overallOralTestSchedulecount != null && $scope.overallOralTestSchedulecount.length > 0 && $scope.newuser != null && $scope.newuser.length > 0) {
                            angular.forEach($scope.newuser, function (a) {
                                a.childarray = [];
                                angular.forEach($scope.overallOralTestSchedulecount, function (b) {
                                    if (a.paotsC_Id === b.PAOTSC_Id) {
                                        a.childarray.push(b)
                                    }
                                })
                            })
                        };
                    }
                };
                if (promise.studentDetails != null && promise.studentDetails.length > 0) {
                    angular.forEach($scope.studentDetails, function (opqr) {
                        if (opqr1.pacA_MiddleName == null) {
                            opqr1.pacA_MiddleName = "Not Avilable";
                        }
                    })
                    $scope.albumNameArray1 = [];
                    for (var i = 0; i < promise.studentDetails.length; i++) {
                        if (promise.studentDetails[i].pacA_FirstName != '') {
                            if (promise.studentDetails[i].pacA_MiddleName != null && promise.studentDetails[i].pacA_MiddleName != '' && promise.studentDetails[i].pacA_MiddleName != "") {
                                if (promise.studentDetails[i].pacA_LastName != null && promise.studentDetails[i].pacA_LastName != '' && promise.studentDetails[i].pacA_LastName != "") {

                                    $scope.albumNameArray1.push({ name: promise.studentDetails[i].pacA_FirstName + " " + promise.studentDetails[i].pacA_MiddleName + " " + promise.studentDetails[i].pacA_LastName, pacA_Id: promise.studentDetails[i].pacA_Id });
                                }
                                else {
                                    $scope.albumNameArray1.push({ name: promise.studentDetails[i].pacA_FirstName + '' + promise.studentDetails[i].pacA_MiddleName, pasR_Id: promise.studentDetails[i].pacA_Id });
                                }
                            }
                            else {
                                if (promise.studentDetails[i].pacA_LastName != null && promise.studentDetails[i].pacA_LastName != '' && promise.studentDetails[i].pacA_LastName != "") {
                                    $scope.albumNameArray1.push({ name: promise.studentDetails[i].pacA_FirstName + " " + promise.studentDetails[i].pacA_LastName, pacA_Id: promise.studentDetails[i].pacA_Id });
                                }
                                else {
                                    $scope.albumNameArray1.push({ name: promise.studentDetails[i].pacA_FirstName, pacA_Id: promise.studentDetails[i].pacA_Id });
                                }
                            }

                            promise.studentDetails[i].name = $scope.albumNameArray1[i].name;
                        }
                    }
                    if (promise.studentDetails.length > 0) {
                        $scope.newuser1 = promise.studentDetails;
                        angular.forEach($scope.newuser1, function (obj) {
                            if ($scope.vidc == true) {
                                obj.vcmeeting = true;
                            }
                            else {
                                obj.vcmeeting = false;
                            }
                        });
                    }
                    else {
                        swal("No students found to schedule!!");
                    }
                    $scope.arrlist58 = promise.admissioncatdrpall;
                    if ($scope.arrlist58.length > 0) {
                        for (var i = 0; i < $scope.newuser1.length; i++) {
                            for (var j = 0; j < $scope.arrlist58.length; j++) {
                                if ($scope.newuser1[i].amcO_Id == $scope.arrlist58[j].amcO_Id) {
                                    $scope.newuser1[i].coursename = $scope.arrlist58[j].amcO_CourseName;
                                }
                            }
                        }
                    }
                    $scope.presentCountgrid1 = promise.studentDetails.length;
                    $scope.presentCountgrid4 = promise.oralTestSchedule.length;
                    $scope.SelectedStudentInCart = {};
                    $scope.minDate = new Date();
                    $scope.PAOTSC_Id = "";
                }
                else {
                    $scope.newuser1 = [];
                    swal('Records not found..!!');
                }
            });
        };

        $scope.commonschedulechange = function () {
            angular.forEach($scope.newuser1, function (obj) {
                obj.checked = false;
                obj.makedisable = false;
            });
            $scope.makedisable = false;
            $scope.myVar1 = false;
            $scope.secondTableData = [];
            $scope.presentCountgrid2 = [];
            $scope.NoOfStud = null;
            $scope.NoOfMin = null;
        };

        $scope.videoconferencecheck = function () {
            angular.forEach($scope.newuser1, function (obj) {
                if ($scope.vidc == true) {
                    obj.vcmeeting = true;
                }
                else {
                    obj.vcmeeting = false;
                }
            });
        };

        $scope.testnoofstu = function (nostu, NoOfMin, NoOfHr) {
            $scope.startTime = true;
            $scope.endtime = true;
            if ($scope.hrs === "12hr") {
                if ($scope.ScheduleTime_12 === "" || $scope.ScheduleTime_12 === undefined) {
                    $scope.startTime = false;
                }
                if ($scope.ScheduleTimeTo_12 === "" || $scope.ScheduleTimeTo_12 === undefined) {
                    $scope.endtime = false;
                }
            }
            if ($scope.hrs === "24hr") {
                if ($scope.ScheduleTime_24 === "" || $scope.ScheduleTime_24 === undefined) {
                    $scope.startTime = false;
                }
                if ($scope.ScheduleTimeTo_24 === "" || $scope.ScheduleTimeTo_24 === undefined) {
                    $scope.endtime = false;
                }
            }

            if ($scope.ScheduleName === "" || $scope.ScheduleName === undefined) {
                $scope.NoOfStud = null;
                swal("Enter Schedule Name");
                return;
            }
            else if ($scope.ScheduleDate === "" || $scope.ScheduleDate === undefined) {
                $scope.NoOfStud = null;
                swal("Select Schedule Date");
                return;
            }
            else if ($scope.startTime === false) {
                $scope.NoOfStud = null;
                swal("Select Schedule Time From");
                return;
            }
            else if ($scope.endtime === false) {
                $scope.NoOfStud = null;
                swal("Select Schedule Time To");
                return;
            }
            else {
                if ($scope.autoschedule === false || $scope.autoschedule === undefined) {
                    if ($scope.NoOfMin === null || $scope.NoOfMin === "" || $scope.NoOfMin === undefined) {
                        $scope.NoOfStud = null;
                        swal("Enter Individual Student Schedule time");
                        return;
                    }
                    else {
                        if ($scope.autoschedule === false) {

                            if ($scope.hrs === '12hr') {
                                $scope.ScheduleTime = $scope.ScheduleTime_12;
                                $scope.ScheduleTimeTo = $scope.ScheduleTimeTo_12;
                            }
                            else if ($scope.hrs === '24hr') {
                                $scope.ScheduleTime = $scope.ScheduleTime_24;
                                $scope.ScheduleTimeTo = $scope.ScheduleTimeTo_24;
                            }
                            var ScheduleTime = $filter('date')($scope.ScheduleTime, "HH:mm");
                            var ScheduleTimeTo = $filter('date')($scope.ScheduleTimeTo, "HH:mm");
                            var startTime = moment(ScheduleTime, "H:mm");
                            var endTime = moment(ScheduleTimeTo, "H:mm");
                            var duration = moment.duration(endTime.diff(startTime));
                            var hours = parseInt(duration.asHours());
                            var minutes = parseInt(duration.asMinutes()) - hours * 60;
                            var hrinmin = hours * 60;
                            var hrtomin = hrinmin + minutes;
                            var tot = $scope.NoOfStud * $scope.NoOfMin;
                            var tot1 = $scope.NoOfStud * $scope.NoOfHr;
                            var tot1tomin = tot1 * 60;
                            if (tot > 0) {
                                if (hrtomin < tot) {
                                    swal('Schedule time and mapping time of student is not possible');
                                    $scope.NoOfStud = null;
                                    angular.forEach($scope.newuser1, function (obj) {
                                        obj.checked = false;
                                        obj.makedisable = false;
                                    });
                                    $scope.myVar1 = false;
                                    $scope.secondTableData = [];
                                    $scope.presentCountgrid2 = [];
                                }
                                else {
                                    angular.forEach($scope.newuser1, function (obj) {
                                        obj.checked = false;
                                        obj.makedisable = false;
                                    });
                                    $scope.makedisable = false;
                                    $scope.myVar1 = false;
                                    $scope.secondTableData = [];
                                    $scope.presentCountgrid2 = [];
                                    if (nostu !== undefined && $scope.autostudent === true) {
                                        for (var k = 0; k < $scope.newuser1.length; k++) {
                                            if (nostu < (k + 1)) {
                                                break;
                                            } else {
                                                $scope.newuser1[k].checked = true;
                                                $scope.secondTableData.push($scope.newuser1[k]);
                                                angular.forEach($scope.newuser1, function (obj) {
                                                    angular.forEach($scope.secondTableData, function (obj1) {
                                                        if (obj.pacA_Id === obj1.pacA_Id) {
                                                            obj.makedisable = true;
                                                        }
                                                    });
                                                });
                                            }
                                            $scope.presentCountgrid2 = $scope.secondTableData.length;
                                        }
                                        $scope.sortKey = "pacA_RegistrationNo";   //set the sortKey to the param passed
                                        $scope.reverse = false; //if true make it false and vice versa
                                        $scope.showcart();
                                    }
                                }
                            } else {
                                if (hrtomin < tot1tomin) {
                                    swal('Schedule time and mapping time of student is not possible');
                                    $scope.NoOfStud = null;
                                    angular.forEach($scope.newuser1, function (obj) {
                                        obj.checked = false;
                                        obj.makedisable = false;
                                    });
                                    $scope.secondTableData = [];
                                    $scope.presentCountgrid2 = [];
                                    $scope.myVar1 = false;
                                }
                                else {
                                    angular.forEach($scope.newuser1, function (obj) {
                                        obj.checked = false;
                                        obj.makedisable = false;
                                    });
                                    $scope.makedisable = false;
                                    $scope.myVar1 = false;
                                    $scope.secondTableData = [];
                                    $scope.presentCountgrid2 = [];
                                    if (nostu !== undefined && $scope.autostudent === true) {
                                        for (var k = 0; k < $scope.newuser1.length; k++) {
                                            if (nostu < (k + 1)) {
                                                break;
                                            } else {
                                                $scope.newuser1[k].checked = true;
                                                $scope.secondTableData.push($scope.newuser1[k]);
                                                angular.forEach($scope.newuser1, function (obj) {
                                                    angular.forEach($scope.secondTableData, function (obj1) {
                                                        if (obj.pacA_Id === obj1.pacA_Id) {
                                                            obj.makedisable = true;
                                                        }
                                                    });
                                                });
                                            }
                                            $scope.presentCountgrid2 = $scope.secondTableData.length;
                                        }
                                        $scope.sortKey = "pacA_RegistrationNo";   //set the sortKey to the param passed
                                        $scope.reverse = false; //if true make it false and vice versa
                                        $scope.showcart();
                                    }
                                }

                            }
                        }
                    }
                }
                else {
                    if ($scope.hrs === '12hr') {
                        $scope.ScheduleTime = $scope.ScheduleTime_12;
                        $scope.ScheduleTimeTo = $scope.ScheduleTimeTo_12;
                    }
                    else if ($scope.hrs === '24hr') {
                        $scope.ScheduleTime = $scope.ScheduleTime_24;
                        $scope.ScheduleTimeTo = $scope.ScheduleTimeTo_24;
                    }
                    ScheduleTime = $filter('date')($scope.ScheduleTime, "HH:mm");
                    ScheduleTimeTo = $filter('date')($scope.ScheduleTimeTo, "HH:mm");
                    startTime = moment(ScheduleTime, "H:mm");
                    endTime = moment(ScheduleTimeTo, "H:mm");
                    duration = moment.duration(endTime.diff(startTime));

                    angular.forEach($scope.newuser1, function (obj) {
                        obj.checked = false;
                        obj.makedisable = false;
                    });
                    $scope.makedisable = false;
                    $scope.myVar1 = false;
                    $scope.secondTableData = [];
                    $scope.presentCountgrid2 = [];
                    if (nostu !== undefined && $scope.autostudent === true) {
                        for (var k = 0; k < $scope.newuser1.length; k++) {
                            if (nostu < (k + 1)) {
                                break;
                            } else {
                                $scope.newuser1[k].checked = true;
                                $scope.secondTableData.push($scope.newuser1[k]);
                                angular.forEach($scope.newuser1, function (obj) {
                                    angular.forEach($scope.secondTableData, function (obj1) {
                                        if (obj.pacA_Id == obj1.pacA_Id) {
                                            obj.makedisable = true;
                                        }
                                    });
                                });
                            }
                            $scope.presentCountgrid2 = $scope.secondTableData.length;
                        }
                        $scope.sortKey = "pasR_RegistrationNo";   //set the sortKey to the param passed
                        $scope.reverse = false; //if true make it false and vice versa
                        $scope.showcart();
                    }
                }
            }
        };
        $scope.validateTomintime = function (timedata) {
            $scope.ScheduleTimeTo = "";
            $scope.totime = timedata;
            var hh = $scope.totime.getHours();
            var mm = $scope.totime.getMinutes();
            $scope.min = timedata;

            $scope.min.setMinutes(hh);
            $scope.min.setMinutes(mm);
        }

        $scope.validateTomintime_12 = function (timedata) {

            $scope.ScheduleTimeTo_12 = "";
            $scope.totime = timedata;
            var hh = $scope.totime.getHours();
            var mm = $scope.totime.getMinutes();
            $scope.min = timedata;

            $scope.min.setMinutes(hh);
            $scope.min.setMinutes(mm);
        }
        $scope.validateTomintime_24 = function (timedata) {

            $scope.ScheduleTimeTo_24 = "";
            $scope.totime = timedata;
            var hh = $scope.totime.getHours();
            var mm = $scope.totime.getMinutes();
            $scope.min = timedata;

            $scope.min.setMinutes(hh);
            $scope.min.setMinutes(mm);
        }
        $scope.validatefromtime = function () {
            //
            //$scope.ScheduleTime = $filter('date')($scope.ScheduleTime, "HH:mm");
            //$scope.ScheduleTimeTo = $filter('date')($scope.ScheduleTimeTo, "HH:mm");


            //var startTime = moment($scope.ScheduleTime, "HH:mm");
            //var endTime = moment($scope.ScheduleTimeTo, "HH:mm");
            //var duration = moment.duration(endTime.diff(startTime));
            //var hours = parseInt(duration.asHours());
            //var minutes = parseInt(duration.asMinutes()) - hours * 60;

        }
        //$scope.ScheduleTime = new Date();
        //$scope.ScheduleTimeTo = new Date();
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.SelectStudentRecord = {};
        $scope.SelectedStudentRecord = {};
        $scope.tempArray = [];
        $scope.stuDelRecord = {};
        $scope.SelectedStudentInCart = [];
        $scope.secondTableData = [];
        $scope.showBinddata = function () {
            $scope.myVar = true;
        };

        $scope.noofmins = function (e) {
            if (e >= 61) {
                swal("minutes cant be more than 60");
                $scope.NoOfMin = "";
            }
        };
        $scope.noofhrs = function (e) {
            if (e >= 25) {
                swal("Hour cant be more than 24");
                $scope.NoOfHr = "";
            }
        };



        $scope.validate_time_noofstudent = function (stu, min, hr) {
            if ($scope.autoschedule === false) {
                if ($scope.hrs === '12hr') {
                    $scope.ScheduleTime = $scope.ScheduleTime_12;
                    $scope.ScheduleTimeTo = $scope.ScheduleTimeTo_12;
                }
                else if ($scope.hrs === '24hr') {
                    $scope.ScheduleTime = $scope.ScheduleTime_24;
                    $scope.ScheduleTimeTo = $scope.ScheduleTimeTo_24;
                }
                var ScheduleTime = $filter('date')($scope.ScheduleTime, "HH:mm");
                var ScheduleTimeTo = $filter('date')($scope.ScheduleTimeTo, "HH:mm");
                var startTime = moment(ScheduleTime, "H:mm");
                var endTime = moment(ScheduleTimeTo, "H:mm");
                var duration = moment.duration(endTime.diff(startTime));
                var hours = parseInt(duration.asHours());
                var minutes = parseInt(duration.asMinutes()) - hours * 60;
                var hrinmin = hours * 60;
                var hrtomin = hrinmin + minutes;
                var tot = stu * min;
                var tot1 = stu * hr;
                var tot1tomin = tot1 * 60;
                if (tot > 0) {
                    if (hrtomin < tot) {
                        swal('Schedule time and mapping time of student is not possible');
                        $scope.NoOfStud = null;
                        angular.forEach($scope.newuser1, function (obj) {
                            obj.checked = false;
                            obj.makedisable = false;
                        });
                        $scope.myVar1 = false;
                        $scope.secondTableData = [];
                        $scope.presentCountgrid2 = [];
                    }
                } else {
                    if (hrtomin < tot1tomin) {
                        swal('Schedule time and mapping time of student is not possible');
                        $scope.NoOfStud = null;
                        angular.forEach($scope.newuser1, function (obj) {
                            obj.checked = false;
                            obj.makedisable = false;
                        });
                        $scope.secondTableData = [];
                        $scope.presentCountgrid2 = [];
                        $scope.myVar1 = false;
                    }
                }
            }
        };

        $scope.showcart = function () {
            $scope.myVar1 = true;
        };

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        //$scope.sortKey = "paotS_Id";   //set the sortKey to the param passed
        //$scope.reverse = true; //if true make it false and vice versa

        $scope.sortKey1 = "pacA_Id";   //set the sortKey to the param passed
        $scope.reverse1 = true; //if true make it false and vice versa

        $scope.sortKey2 = "pacA_Id";   //set the sortKey to the param passed
        $scope.reverse2 = true; //if true make it false and vice versa

        //$scope.sortKey3 = "paotS_ScheduleDate";   //set the sortKey to the param passed
        //$scope.reverse3 = true; //if true make it false and vice versa

        $scope.presentCountgrid1 = 0;
        $scope.presentCountgrid2 = 0;
        $scope.presentCountgrid3 = 0;
        $scope.presentCountgrid4 = 0;

        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };

        //$scope.ismeridian = true;
        //$scope.toggleMode = function () {
        //    $scope.ismeridian = !$scope.ismeridian;
        //};

        //time picker ends



        $scope.SelectStudent = function (SelectStudentRecord) {
            $scope.SelectedId = SelectStudentRecord.pacA_Id;
            var SSelectedId = $scope.SelectedId;
            apiService.getURI("OralTestScheduleClg/GetStudentdetails/", SSelectedId).
                then(function (promise) {
                    $scope.SelectedStudent = promise.studentDetails;

                    $scope.presentCountgrid3 = promise.studentDetails.length;
                })
        };


        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
        }


        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        }

        $scope.IsHidden2 = true;
        $scope.ShowHide2 = function () {
            $scope.IsHidden2 = $scope.IsHidden2 ? false : true;
        }
        $scope.IsHidden3 = true;
        $scope.ShowHide3 = function () {
            $scope.IsHidden3 = $scope.IsHidden3 ? false : true;
        }
        $scope.IsHidden4 = true;
        $scope.ShowHide4 = function () {
            $scope.IsHidden4 = $scope.IsHidden4 ? false : true;
        }




        $scope.TempSelectStudent = function (record) {

            angular.forEach($scope.newuser1, function (obj) {

                if (obj.pacA_Id == record.pacA_Id) {
                    obj.makedisable = true;
                }

            });

            if ($scope.presentCountgrid2 < Number($scope.NoOfStud)) {

                if ($scope.secondTableData.indexOf(record) === -1) {
                    $scope.secondTableData.push(record);
                }
                else {
                    $scope.secondTableData.splice($scope.secondTableData.indexOf(record), 1);
                }
                console.log($scope.secondTableData);

                $scope.presentCountgrid2 = $scope.secondTableData.length;

            }
            else {
                record.checked = false;
                //angular.forEach($scope.newuser1, function (obj) {
                //    obj.checked = false;
                //})
                angular.forEach($scope.newuser1, function (obj) {

                    if (obj.pacA_Id == record.pacA_Id) {
                        obj.makedisable = false;
                    }

                });
                swal("Cannot Select More Than " + $scope.NoOfStud + " No. Of Students");
            }
        };

        $scope.TempdeleteStudent = function (stuDelRecord, index) {

            stuDelRecord.checked = false;
            angular.forEach($scope.newuser1, function (obj) {

                if (obj.pacA_id == stuDelRecord.pacA_id) {
                    obj.makedisable = false;
                }

            });

            console.log($scope.secondTableData.indexOf(stuDelRecord));
            $scope.secondTableData.splice($scope.secondTableData.indexOf(stuDelRecord), 1);

            $scope.presentCountgrid2 = $scope.secondTableData.length;
            if ($scope.secondTableData == 0) {
                $scope.myVar1 = false;
            }

        };
        $scope.page1 = "pag1";
        $scope.page2 = "pag2";
        $scope.page3 = "pag3";
        $scope.page4 = "pag4";
        $scope.page5 = "pag5";
        $scope.page6 = "pag6";
        $scope.page7 = "pag7";
        $scope.currentPage1 = 1;
        $scope.currentPage2 = 1;
        $scope.currentPage3 = 1;
        $scope.currentPage4 = 1;
        $scope.currentPage5 = 1;
        $scope.currentPage6 = 1;
        $scope.currentPage7 = 1;

        $scope.itemsPerPage1 = paginationformasters;
        $scope.itemsPerPage2 = paginationformasters;
        $scope.itemsPerPage3 = paginationformasters;
        $scope.itemsPerPage4 = paginationformasters;
        $scope.itemsPerPage5 = paginationformasters;
        $scope.itemsPerPage6 = paginationformasters;
        $scope.itemsPerPage7 = 5;

        $scope.BindData = function () {
            $scope.reverse = true;
            $scope.reverse1 = true;
            $scope.reverse2 = true;
            $scope.reverse3 = true;

            apiService.getDATA("OralTestScheduleClg/Getdetails").
                then(function (promise) {
                    $scope.courselist = promise.allcourse;
                    $scope.newuser = promise.oralTestScheduleclg;
                    $scope.stafflist = promise.stafflist;
                    $scope.oralTestSchedulecount = promise.OralTestScheduleClgcount;
                    $scope.overallOralTestSchedulecount = promise.overallOralTestSchedulecount;
                    $scope.vcorallist = promise.vcOralTestScheduleClg;
                    if ($scope.vcorallist != null && $scope.vcorallist.length > 0 && $scope.newuser != null && $scope.newuser.length > 0) {
                        angular.forEach($scope.newuser, function (a) {
                            angular.forEach($scope.vcorallist, function (b) {
                                if (a.paotsC_Id === b.PAOTSC_Id) {
                                    a.vc = true;
                                }
                            })
                        })
                    };
                    if ($scope.OralTestScheduleClgcount != null && $scope.OralTestScheduleClgcount.length > 0 && $scope.newuser != null && $scope.newuser.length > 0) {
                        angular.forEach($scope.newuser, function (a) {
                            angular.forEach($scope.OralTestScheduleClgcount, function (b) {
                                if (a.paotsC_Id === b.PAOTSC_Id) {
                                    a.ocount = b.ocount;
                                }
                            })
                        })
                    };
                    if ($scope.OralTestScheduleClgcount != null && $scope.OralTestScheduleClgcount.length > 0) {
                        $scope.studentcount = 0;
                        angular.forEach($scope.OralTestScheduleClgcount, function (b) {
                            $scope.studentcount = $scope.studentcount + b.ocount;
                        })
                    };

                    angular.forEach($scope.newuser, function (obj) {
                        obj.delete = true;
                        obj.fromtime = obj.paotsC_ScheduleFromTime;
                        obj.totime = obj.paotsC_ScheduleToTime;
                        $scope.getserverdate();
                        $scope.dateof = new Date($scope.today);
                        var alignFillDate = new Date(obj.paotsC_ScheduleDate);
                        var nameArray = obj.fromtime.split(':');
                        if (nameArray != null && nameArray.length > 0) {
                            alignFillDate.setHours(alignFillDate.getHours() + nameArray[0]);
                            alignFillDate.setMinutes(alignFillDate.getMinutes() + nameArray[1]);
                        }
                        //fromtime
                        var pickUpDate = new Date($scope.dateof);
                        if (alignFillDate < pickUpDate) {
                            obj.delete = false;
                        }
                    })

                    angular.forEach($scope.newuser, function (obj) {
                        if (obj.paotsC_ScheduleFromTime.substring(0, 2) > 12) {
                            obj.paotsC_ScheduleTimeonly = (Number(obj.paotsC_ScheduleFromTime.substring(0, 2)) - 12).toString();
                            if (obj.paotsC_ScheduleTimeonly.length == 1) {
                                obj.paotsC_ScheduleTimeoly = '0' + obj.paotsC_ScheduleTimeonly;
                                obj.paotsC_ScheduleFromTime = obj.paotsC_ScheduleTimeoly + ':' + obj.paotsC_ScheduleFromTime.substring(3, 5);
                                obj.paotsC_ScheduleFromTime = obj.paotsC_ScheduleTime + 'PM';
                            }
                            else if (obj.paotS_ScheduleTimeonly.length > 1) {
                                obj.paotsC_ScheduleFromTime = obj.paotsC_ScheduleTimeonly + ':' + obj.paotsC_ScheduleFromTime.substring(3, 5);
                                obj.paotsC_ScheduleFromTime = obj.paotsC_ScheduleTime + 'PM';
                            }
                        }
                        else if (obj.paotsC_ScheduleFromTime.substring(0, 2) == 12) {
                            obj.paotsC_ScheduleFromTime = obj.paotsC_ScheduleFromTime + 'PM';
                        }
                        else {

                            if (obj.paotsC_ScheduleFromTime.length == 1) {
                                obj.paotsC_ScheduleFromTime = '0' + obj.paotsC_ScheduleTime;
                                obj.paotsC_ScheduleFromTime = obj.paotsC_ScheduleFromTime + 'AM';
                            }
                            else {
                                obj.paotsC_ScheduleFromTime = obj.paotsC_ScheduleFromTime + 'AM';
                            }
                        }
                    })

                    angular.forEach($scope.newuser, function (obj) {
                        if (obj.paotsC_ScheduleToTime.substring(0, 2) > 12) {
                            obj.paotsC_ScheduleTimeToonly = (Number(obj.paotsC_ScheduleToTime.substring(0, 2)) - 12).toString();
                            if (obj.paotsC_ScheduleTimeToonly.length == 1) {
                                obj.paotsC_ScheduleTimeTooly = '0' + obj.paotsC_ScheduleTimeToonly;
                                obj.paotsC_ScheduleToTime = obj.paotsC_ScheduleTimeTooly + ':' + obj.paotsC_ScheduleToTime.substring(3, 5);
                                obj.paotsC_ScheduleToTime = obj.paotsC_ScheduleToTime + 'PM';
                            }
                            else if (obj.paotsC_ScheduleTimeToonly.length > 1) {
                                obj.paotsC_ScheduleToTime = obj.paotsC_ScheduleTimeToonly + ':' + obj.paotsC_ScheduleToTime.substring(3, 5);
                                obj.paotsC_ScheduleToTime = obj.paotsC_ScheduleToTime + 'PM';
                            }
                        }
                        else if (obj.paotsC_ScheduleToTime.substring(0, 2) == 12) {
                            obj.paotsC_ScheduleToTime = obj.paotsC_ScheduleToTime + 'PM';
                        }
                        else {

                            if (obj.paotsC_ScheduleToTime.length == 1) {
                                obj.paotsC_ScheduleToTime = '0' + obj.paotsC_ScheduleToTime;
                                obj.paotsC_ScheduleToTime = obj.paotsC_ScheduleToTime + 'AM';
                            }
                            else {
                                obj.paotcS_ScheduleTimeTo = obj.paotcS_ScheduleTimeTo + 'AM';
                            }
                        }
                    })

                    if ($scope.overallOralTestSchedulecount != null && $scope.overallOralTestSchedulecount.length > 0) {
                        angular.forEach($scope.overallOralTestSchedulecount, function (obj) {
                            if (obj.ScheduleTime.substring(0, 2) > 12) {
                                obj.ScheduleTimeonly = (Number(obj.ScheduleTime.substring(0, 2)) - 12).toString();
                                if (obj.ScheduleTimeonly.length == 1) {
                                    obj.ScheduleTimeoly = '0' + obj.ScheduleTimeonly;
                                    obj.ScheduleTime = obj.ScheduleTimeoly + ':' + obj.ScheduleTime.substring(3, 5);
                                    obj.ScheduleTime = obj.ScheduleTime + 'PM';
                                }
                                else if (obj.ScheduleTimeonly.length > 1) {
                                    obj.ScheduleTime = obj.ScheduleTimeonly + ':' + obj.ScheduleTime.substring(3, 5);
                                    obj.ScheduleTime = obj.ScheduleTime + 'PM';
                                }
                            }
                            else if (obj.ScheduleTime.substring(0, 2) == 12) {
                                obj.ScheduleTime = obj.ScheduleTime + 'PM';
                            }
                            else {
                                if (obj.ScheduleTime.length == 1) {
                                    obj.ScheduleTime = '0' + obj.ScheduleTime;
                                    obj.ScheduleTime = obj.ScheduleTime + 'AM';
                                }
                                else {
                                    obj.ScheduleTime = obj.ScheduleTime + 'AM';
                                }
                            }
                        })
                        angular.forEach($scope.overallOralTestSchedulecount, function (obj) {
                            if (obj.ScheduleTimeTo.substring(0, 2) > 12) {
                                obj.ScheduleTimeToonly = (Number(obj.ScheduleTimeTo.substring(0, 2)) - 12).toString();
                                if (obj.ScheduleTimeToonly.length == 1) {
                                    obj.ScheduleTimeTooly = '0' + obj.ScheduleTimeToonly;
                                    obj.ScheduleTimeTo = obj.ScheduleTimeTooly + ':' + obj.ScheduleTimeTo.substring(3, 5);
                                    obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'PM';
                                }
                                else if (obj.ScheduleTimeToonly.length > 1) {
                                    obj.ScheduleTimeTo = obj.ScheduleTimeToonly + ':' + obj.ScheduleTimeTo.substring(3, 5);
                                    obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'PM';
                                }
                            }
                            else if (obj.ScheduleTimeTo.substring(0, 2) == 12) {
                                obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'PM';
                            }
                            else {

                                if (obj.ScheduleTimeTo.length == 1) {
                                    obj.ScheduleTimeTo = '0' + obj.ScheduleTimeTo;
                                    obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'AM';
                                }
                                else {
                                    obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'AM';
                                }
                            }
                        })

                        if ($scope.overallOralTestSchedulecount != null && $scope.overallOralTestSchedulecount.length > 0 && $scope.newuser != null && $scope.newuser.length > 0) {
                            angular.forEach($scope.newuser, function (a) {
                                a.childarray = [];
                                angular.forEach($scope.overallOralTestSchedulecount, function (b) {
                                    if (a.paotS_Id === b.PAOTS_Id) {
                                        a.childarray.push(b)
                                    }
                                })
                            })
                        };
                    }

                    $scope.minDate = new Date();
                    //   $scope.minDate = $scope.Dat
                    $scope.PAOTS_Id = "";
                })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

            $scope.searchValue = '';
            $scope.filterValue = function (obj) {
                // 
             //   return ($filter('date')(obj.paosC_ScheduleDate, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0 || $filter('date')(obj.paotsC_ScheduleFromTime, 'h:mm a').indexOf($scope.searchValue) >= 0 || $filter('date')(obj.paotsC_ScheduleTimeTo, 'h:mm a').indexOf($scope.searchValue) >= 0 || (angular.lowercase(obj.paotsC_ScheduleName)).indexOf(angular.lowercase($scope.searchValue)) >= 0);
                //return ($filter('date')(obj.paotS_ScheduleTime, 'HH:mm').indexOf($scope.searchValue) >= 0) || ($filter('date')(obj.paotS_ScheduleDate, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0 || (obj.paotS_ScheduleName).indexOf($scope.searchValue) >= 0 || (obj.paotS_AM_PM).indexOf($scope.searchValue) >= 0 || (obj.paotS_Remarks).indexOf($scope.searchValue) >= 0);
            }
        };
        $scope.pagebrakevalue = "page-break-after:always;";
        $scope.pagebrake = function () {
            if ($scope.pagebr === true) {
                $scope.pagebrakevalue = "page-break-after:always;";
            }
            else {
                $scope.pagebrakevalue = "";
            }
        };

        $scope.searchValueDept = '';
        $scope.filterValue4 = function (obj) {
            return angular.lowercase(obj.asmcL_ClassName).indexOf(angular.lowercase($scope.searchValueDept)) >= 0;
        };

        $scope.togchkbxdept = function (groupType) {
            $scope.asmcL_Id = $scope.courselist.every(function (itm) {
                return itm.selected;
            });
        };
        $scope.all_checkdept = function (departmentselectedAll) {
            var toggleStatus = departmentselectedAll;
            angular.forEach($scope.filterValue5, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.coursechnage();
        };

        $scope.toggleAll = function () {
            $scope.printdatatable = [];
            var toggleStatus = $scope.all;
            angular.forEach($scope.schedulelist, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    if ($scope.printdatatable.indexOf(itm) === -1) {
                        $scope.printdatatable.push(itm);
                    }
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
        };

        $scope.selected = function (SelectedStudentRecord, index) {
            $scope.all = $scope.schedulelist.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
        };

        $scope.search = '';
        $scope.filterOnLocation = function (user) {
            //  (angular.lowercase(obj.User_Name)).indexOf(angular.lowercase($scope.searchValue))
            return (angular.lowercase(user.name)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                //    (angular.lowercase(user.pasR_MiddleName)).indexOf(angular.lowercase($scope.search)) >= 0  ||
                (angular.lowercase(user.pacA_Sex)).indexOf(angular.lowercase($scope.search)) >= 0
                ||
                (angular.lowercase(user.pacA_RegistrationNo)).indexOf(angular.lowercase($scope.search)) >= 0
                ||
                (JSON.stringify(user.pacA_MobileNo)).indexOf(($scope.search)) >= 0
                ||
                (JSON.stringify(user.coursename)).indexOf(($scope.search)) >= 0
                ||
                (JSON.stringify(user.pacA_Sex)).indexOf(($scope.search)) >= 0
                ||
                (angular.lowercase(user.pacA_emailId)).indexOf(angular.lowercase($scope.search)) >= 0;
        };

        $scope.search1 = '';
        $scope.filterOnLocation2 = function (user) {
            //  (angular.lowercase(obj.User_Name)).indexOf(angular.lowercase($scope.searchValue))
            return (angular.lowercase(user.name)).indexOf(angular.lowercase($scope.search1)) >= 0 ||
                //    (angular.lowercase(user.pasR_MiddleName)).indexOf(angular.lowercase($scope.search)) >= 0  ||
                (angular.lowercase(user.pacA_Sex)).indexOf(angular.lowercase($scope.search1)) >= 0
                ||
                (angular.lowercase(user.pacA_RegistrationNo)).indexOf(angular.lowercase($scope.search1)) >= 0
                ||
                (JSON.stringify(user.pacA_MobileNo)).indexOf(($scope.search1)) >= 0
                ||
                (angular.lowercase(user.pacA_emailId)).indexOf(angular.lowercase($scope.search1)) >= 0;
        };


        $scope.BindStudentDataAfterDelete = function () {
            apiService.getDATA("OralTestSchedule/GetRemainStudentdetails").
                then(function (promise) {
                    $scope.SelectedStudent = promise.selectedStudentDetails;

                    $scope.presentCountgrid3 = promise.selectedStudentDetails.length;
                })
        };


        //Delete Test Schedule Student

        $scope.DeleteWrittenTestScheduleStudentdata = function (DeleteRecord, SweetAlert) {

            $scope.deleteId = DeleteRecord.pasr_id;
            var MdeleteId = $scope.deleteId;
            var PAOTS_Id = $scope.PAOTS_Id;
            var data = {
                "PAOTS_Id": PAOTS_Id,
                "PASR_Id": MdeleteId
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to delete this item?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,delete it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("OralTestScheduleClg/OralTestScheduleDeletesStudentData", data).

                            then(function (promise) {
                                swal("Record Deleted Successfully");

                                $scope.BindData();
                            })
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                });
        }
        $scope.sname = "";
        $scope.sdate = "";
        $scope.sstime = "";
        $scope.sttime = "";
        $scope.showlist = function (DeleteRecord, dele, sname, sdate, sstime, sttime) {
            $scope.schedulelist = [];
            $scope.printdatatable = [];
            $scope.deleteshow = dele;
            $scope.sname = sname;
            $scope.all = false;
            //$scope.snameprint = sname.replace(".", "");
          
            //$scope.sdate = $filter('date')(sdate, 'dd-MM-yyyy');
            //$scope.sstime = sstime.replace(":", "-");
            //$scope.sttime = sttime.replace(":", "-");
            $scope.PAOTSId = DeleteRecord;
            var data = {
                "PAOTSC_Id": DeleteRecord
            }
            apiService.create("OralTestScheduleClg/scheduleGetreportdetails", data).
                then(function (promise) {
                    if (promise.schedulelist != null && promise.schedulelist.length > 0) {
                        $scope.schedulelist = promise.schedulelist;
                        $scope.printdatatablenew = $scope.schedulelist;
                        angular.forEach($scope.schedulelist, function (obj) {
                            obj.starttimeof = obj.ScheduleTime;
                            if (obj.ScheduleTime.substring(0, 2) > 12) {
                                obj.ScheduleTimeonly = (Number(obj.ScheduleTime.substring(0, 2)) - 12).toString();
                                if (obj.ScheduleTimeonly.length == 1) {
                                    obj.ScheduleTimeoly = '0' + obj.ScheduleTimeonly;
                                    obj.ScheduleTime = obj.ScheduleTimeoly + ':' + obj.ScheduleTime.substring(3, 5);
                                    obj.ScheduleTime = obj.ScheduleTime + 'PM';
                                }
                                else if (obj.ScheduleTimeonly.length > 1) {
                                    obj.ScheduleTime = obj.ScheduleTimeonly + ':' + obj.ScheduleTime.substring(3, 5);
                                    obj.ScheduleTime = obj.ScheduleTime + 'PM';
                                }
                            }
                            else if (obj.ScheduleTime.substring(0, 2) == 12) {
                                obj.ScheduleTime = obj.ScheduleTime + 'PM';
                            }
                            else {
                                if (obj.ScheduleTime.length == 1) {
                                    obj.ScheduleTime = '0' + obj.ScheduleTime;
                                    obj.ScheduleTime = obj.ScheduleTime + 'AM';
                                }
                                else {
                                    obj.ScheduleTime = obj.ScheduleTime + 'AM';
                                }
                            }
                        })
                        angular.forEach($scope.schedulelist, function (obj) {
                            if (obj.ScheduleTimeTo.substring(0, 2) > 12) {
                                obj.ScheduleTimeToonly = (Number(obj.ScheduleTimeTo.substring(0, 2)) - 12).toString();
                                if (obj.ScheduleTimeToonly.length == 1) {
                                    obj.ScheduleTimeTooly = '0' + obj.ScheduleTimeToonly;
                                    obj.ScheduleTimeTo = obj.ScheduleTimeTooly + ':' + obj.ScheduleTimeTo.substring(3, 5);
                                    obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'PM';
                                }
                                else if (obj.ScheduleTimeToonly.length > 1) {
                                    obj.ScheduleTimeTo = obj.ScheduleTimeToonly + ':' + obj.ScheduleTimeTo.substring(3, 5);
                                    obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'PM';
                                }
                            }
                            else if (obj.ScheduleTimeTo.substring(0, 2) == 12) {
                                obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'PM';
                            }
                            else {

                                if (obj.ScheduleTimeTo.length == 1) {
                                    obj.ScheduleTimeTo = '0' + obj.ScheduleTimeTo;
                                    obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'AM';
                                }
                                else {
                                    obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'AM';
                                }
                            }
                        })

                        angular.forEach($scope.schedulelist, function (obj) {
                            obj.deletestudent = false;
                            $scope.getserverdate();
                            $scope.dateof = new Date($scope.today);
                            var alignFillDate = new Date(obj.ScheduleDate);
                            var nameArray = obj.starttimeof.split(':');
                            if (nameArray != null && nameArray.length > 0) {
                                alignFillDate.setHours(alignFillDate.getHours() + nameArray[0]);
                                alignFillDate.setMinutes(alignFillDate.getMinutes() + nameArray[1]);
                            }
                            //fromtime
                            var pickUpDate = new Date($scope.dateof);
                            if (alignFillDate < pickUpDate) {
                                obj.deletestudent = true;
                            }
                        })

                        $('#myModal1').modal('show');
                    }
                    else {
                        $('#myModal1').modal('hide');
                        $('.modal-backdrop').remove();
                        $('.modal').on('hide.bs.modal', function (e) {
                            e.stopPropagation();
                            $('body').css('padding-right', '');
                        });
                        swal("No records found!");
                    }
                })
        };

        $scope.showlistremarks = function (DeleteRecord, dele, sname, sdate, sstime, sttime) {
            $scope.schedulelist = [];
            $scope.printdatatable = [];
            $scope.deleteshow = dele;
            $scope.sname = sname;
            //$scope.snameprint = sname.replace(".", "");
            ////$scope.snameprint = $scope.snameprint.replace(" ", "-");
            //$scope.sdate = $filter('date')(sdate, 'dd-MM-yyyy');
            //$scope.sstime = sstime.replace(":", "-");
            //$scope.sttime = sttime.replace(":", "-");
            $scope.PAOTSId = DeleteRecord;
            var data = {
                "PAOTSC_Id": DeleteRecord
            }
            apiService.create("OralTestScheduleClg/scheduleGetreportdetails", data).
                then(function (promise) {
                    if (promise.schedulelist != null && promise.schedulelist.length > 0) {
                        $scope.schedulelist = promise.schedulelist;
                        $scope.printdatatablenew = $scope.schedulelist;
                        angular.forEach($scope.schedulelist, function (obj) {
                            if (obj.ScheduleTime.substring(0, 2) > 12) {
                                obj.ScheduleTimeonly = (Number(obj.ScheduleTime.substring(0, 2)) - 12).toString();
                                if (obj.ScheduleTimeonly.length == 1) {
                                    obj.ScheduleTimeoly = '0' + obj.ScheduleTimeonly;
                                    obj.ScheduleTime = obj.ScheduleTimeoly + ':' + obj.ScheduleTime.substring(3, 5);
                                    obj.ScheduleTime = obj.ScheduleTime + 'PM';
                                }
                                else if (obj.ScheduleTimeonly.length > 1) {
                                    obj.ScheduleTime = obj.ScheduleTimeonly + ':' + obj.ScheduleTime.substring(3, 5);
                                    obj.ScheduleTime = obj.ScheduleTime + 'PM';
                                }
                            }
                            else if (obj.ScheduleTime.substring(0, 2) == 12) {
                                obj.ScheduleTime = obj.ScheduleTime + 'PM';
                            }
                            else {
                                if (obj.ScheduleTime.length == 1) {
                                    obj.ScheduleTime = '0' + obj.ScheduleTime;
                                    obj.ScheduleTime = obj.ScheduleTime + 'AM';
                                }
                                else {
                                    obj.ScheduleTime = obj.ScheduleTime + 'AM';
                                }
                            }
                        })
                        angular.forEach($scope.schedulelist, function (obj) {
                            if (obj.ScheduleTimeTo.substring(0, 2) > 12) {
                                obj.ScheduleTimeToonly = (Number(obj.ScheduleTimeTo.substring(0, 2)) - 12).toString();
                                if (obj.ScheduleTimeToonly.length == 1) {
                                    obj.ScheduleTimeTooly = '0' + obj.ScheduleTimeToonly;
                                    obj.ScheduleTimeTo = obj.ScheduleTimeTooly + ':' + obj.ScheduleTimeTo.substring(3, 5);
                                    obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'PM';
                                }
                                else if (obj.ScheduleTimeToonly.length > 1) {
                                    obj.ScheduleTimeTo = obj.ScheduleTimeToonly + ':' + obj.ScheduleTimeTo.substring(3, 5);
                                    obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'PM';
                                }
                            }
                            else if (obj.ScheduleTimeTo.substring(0, 2) == 12) {
                                obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'PM';
                            }
                            else {

                                if (obj.ScheduleTimeTo.length == 1) {
                                    obj.ScheduleTimeTo = '0' + obj.ScheduleTimeTo;
                                    obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'AM';
                                }
                                else {
                                    obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'AM';
                                }
                            }
                        })

                        $scope.remarkwisereport();
                    }
                    else {
                        $('#myModal1').modal('hide');
                        $('#myModalRemark').modal('hide');
                        $('.modal-backdrop').remove();
                        $('.modal').on('hide.bs.modal', function (e) {
                            e.stopPropagation();
                            $('body').css('padding-right', '');
                        });
                        swal("No records found!");
                    }
                })
        };

        $scope.showschedulepart = function (DeleteRecord, dele, sname, sdate, sstime, sttime) {
            $scope.scheduledstaffs = [];
            $scope.sdeleteshow = dele;
            $scope.ssname = sname;
            $scope.ssnameprint = sname.replace(".", "");
            $scope.ssdate = $filter('date')(sdate, 'dd-MM-yyyy');
            $scope.ssstime = sstime.replace(":", "-");
            $scope.ssttime = sttime.replace(":", "-");
            $scope.sPAOTSId = DeleteRecord;
            var data = {
                "PAOTSC_Id": DeleteRecord,
                "checkadd": "CK"
            }
            apiService.create("OralTestScheduleClg/checkaddparticipants", data).
                then(function (promise) {
                    $scope.scprincipalflg = promise.principalflg;
                    $scope.scmanagerflg = promise.managerflg;
                    $scope.scstf = promise.stafflag;
                    $scope.sanybodystart = promise.anybodystart;
                    $scope.mappedstafflist = promise.mappedstafflist;
                    $scope.scstafflist = promise.stafflist;
                    if ($scope.scstafflist != null && $scope.scstafflist.length > 0) {
                        if ($scope.mappedstafflist != null && $scope.mappedstafflist.length > 0) {
                            angular.forEach($scope.scstafflist, function (a) {
                                angular.forEach($scope.mappedstafflist, function (b) {
                                    if (a.userid === b.user_Id) {
                                        a.select = true;
                                        $scope.scheduledstaffs.push(b);
                                    }
                                })
                            })
                        }
                    }
                    $('#myModalparticipants').modal('show');
                })
        };

        $scope.updateparticipants = function () {
            $scope.selectedstflist = [];
            $scope.sstaffselect = false;
            if ($scope.scstf === true) {
                angular.forEach($scope.scstafflist, function (obj) {
                    if (obj.select === true) {
                        $scope.sstaffselect = true;
                        return;
                    }
                })
                if ($scope.sstaffselect === false) {
                    swal("Kindly select any one participant!!");
                    return;
                }
                else {
                    angular.forEach($scope.scstafflist, function (sect) {
                        if (sect.select == true) {
                            $scope.selectedstflist.push({ userid: sect.userid, HRME_Id: sect.hrmE_Id });
                        }
                    });
                }
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to update participants details?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,update it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        var data = {
                            "PAOTS_Id": $scope.sPAOTSId,
                            "checkadd": "SV",
                            "principalflg": $scope.scprincipalflg,
                            "managerflg": $scope.scmanagerflg,
                            "stafflag": $scope.scstf,
                            "anybodystart": $scope.sanybodystart,
                            stfids: $scope.selectedstflist
                        }
                        apiService.create("OralTestScheduleClg/checkaddparticipants", data).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    $('#myModalparticipants').modal('hide');
                                    swal("Record Updted successfully");
                                }
                                else {
                                    swal("Record updte failed");
                                }

                            })
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                });
        };

        $scope.viewstaff = function () {
            if ($scope.scheduledstaffs != null && $scope.scheduledstaffs.length > 0) {
                $('#myModalstaff').modal('show');
            }
        };

        $scope.remarkwisereport = function () {
            $scope.remarkschedulelist = [];
            $scope.remarkprintdatatable = [];
            var data = {
                "PAOTSC_Id": $scope.PAOTSId
            }
            apiService.create("OralTestScheduleClg/remarksGetreportdetails", data).
                then(function (promise) {
                    if (promise.remarkschedulelist != null && promise.remarkschedulelist.length > 0) {
                        $scope.remarkschedulelist = promise.remarkschedulelist;
                        $('#myModal1').modal('hide');
                        $('#myModalRemark').modal('show');
                        $('.modal').on('hide.bs.modal', function (e) {
                            e.stopPropagation();
                            $('body').css('padding-right', '');
                        });
                    }
                    else {
                        $('#myModal1').modal('hide');
                        $('.modal-backdrop').remove();
                        $('.modal').on('hide.bs.modal', function (e) {
                            e.stopPropagation();
                            $('body').css('padding-right', '');
                        });
                        swal("No records found!");
                    }
                })
        };


        $scope.export = function (tid, DeleteRecord, sname, sdate, sstime, sttime) {
            $scope.schedulelist = [];
            $scope.printdatatable = [];
            var data = {
                "PAOTS_Id": DeleteRecord
            }
            apiService.create("ScheduleReport/scheduleGetreportdetails", data).
                then(function (promise) {
                    if (promise.schedulelist != null && promise.schedulelist.length > 0) {
                        $scope.schedulelist = promise.schedulelist;

                        angular.forEach($scope.schedulelist, function (obj) {
                            if (obj.ScheduleTime.substring(0, 2) > 12) {
                                obj.ScheduleTimeonly = (Number(obj.ScheduleTime.substring(0, 2)) - 12).toString();
                                if (obj.ScheduleTimeonly.length == 1) {
                                    obj.ScheduleTimeoly = '0' + obj.ScheduleTimeonly;
                                    obj.ScheduleTime = obj.ScheduleTimeoly + ':' + obj.ScheduleTime.substring(3, 5);
                                    obj.ScheduleTime = obj.ScheduleTime + 'PM';
                                }
                                else if (obj.ScheduleTimeonly.length > 1) {
                                    obj.ScheduleTime = obj.ScheduleTimeonly + ':' + obj.ScheduleTime.substring(3, 5);
                                    obj.ScheduleTime = obj.ScheduleTime + 'PM';
                                }
                            }
                            else if (obj.ScheduleTime.substring(0, 2) == 12) {
                                obj.ScheduleTime = obj.ScheduleTime + 'PM';
                            }
                            else {
                                if (obj.ScheduleTime.length == 1) {
                                    obj.ScheduleTime = '0' + obj.ScheduleTime;
                                    obj.ScheduleTime = obj.ScheduleTime + 'AM';
                                }
                                else {
                                    obj.ScheduleTime = obj.ScheduleTime + 'AM';
                                }
                            }
                        })
                        angular.forEach($scope.schedulelist, function (obj) {
                            if (obj.ScheduleTimeTo.substring(0, 2) > 12) {
                                obj.ScheduleTimeToonly = (Number(obj.ScheduleTimeTo.substring(0, 2)) - 12).toString();
                                if (obj.ScheduleTimeToonly.length == 1) {
                                    obj.ScheduleTimeTooly = '0' + obj.ScheduleTimeToonly;
                                    obj.ScheduleTimeTo = obj.ScheduleTimeTooly + ':' + obj.ScheduleTimeTo.substring(3, 5);
                                    obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'PM';
                                }
                                else if (obj.ScheduleTimeToonly.length > 1) {
                                    obj.ScheduleTimeTo = obj.ScheduleTimeToonly + ':' + obj.ScheduleTimeTo.substring(3, 5);
                                    obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'PM';
                                }
                            }
                            else if (obj.ScheduleTimeTo.substring(0, 2) == 12) {
                                obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'PM';
                            }
                            else {

                                if (obj.ScheduleTimeTo.length == 1) {
                                    obj.ScheduleTimeTo = '0' + obj.ScheduleTimeTo;
                                    obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'AM';
                                }
                                else {
                                    obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'AM';
                                }
                            }
                        })

                        //$('#myModal1').modal('show');
                        $scope.printdatatable = $scope.schedulelist;
                        //$scope.printData();
                        //$scope.exportToExcel(tid,sname, sdate, sstime, sttime);
                    }
                    //else {
                    //    $('#myModal1').modal('hide');
                    //    $('.modal-backdrop').remove();
                    //    swal("No records found!");
                    //}
                })
        };

        $scope.isOptionsRequiredcls1 = function () {
            if ($scope.stf === true && $scope.pavidc === true) {
                return !$scope.stafflist.some(function (options) {
                    return options.select;
                });
            }
        };

        //Delete Test Schedule

        $scope.DeleteWrittenTestScheduledata = function (DeleteRecord, SweetAlert) {
            $scope.deleteId = DeleteRecord.paotS_Id;
            var MdeleteId = $scope.deleteId;
            // swal(id);
            swal({
                title: "Are you sure?",
                text: "Do you want to delete this item?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,delete it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.DeleteURI("OralTestSchedule/OralTestScheduleDeletesData", MdeleteId).
                            then(function (promise) {
                                swal("Record Deleted successfully");
                                $scope.BindData();
                            })
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                });
        }

        $scope.deletestudent = function () {
            if ($scope.printdatatable.length > 0) {
                swal({
                    title: "Are you sure?",
                    text: "Do you want to delete records ..!?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                    cancelButtonText: "Cancel!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            var data = {
                                "DeleteStudentData": $scope.printdatatable
                            }
                            apiService.create("OralTestSchedule/removestudents", data).
                                then(function (promise) {
                                    $('#myModal1').modal('hide');
                                    $('.modal-backdrop').remove();
                                    $('.modal').on('hide.bs.modal', function (e) {
                                        e.stopPropagation();
                                        $('body').css('padding-right', '');
                                    });
                                    $scope.BindData();
                                    swal("Records removed successfully");
                                });
                        }
                    });
            }
            else {
                swal("No record selected to delete!!");
            }
        };

        //$scope.reminderstudent = function () {
        //    if ($scope.printdatatable.length > 0) {
        //        swal({
        //            title: "Are you sure?",
        //            text: "Do you want to send reminder..!?",
        //            type: "warning",
        //            showCancelButton: true,
        //            confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
        //            cancelButtonText: "Cancel!",
        //            closeOnConfirm: false,
        //            closeOnCancel: true
        //        },
        //            function (isConfirm) {
        //                if (isConfirm) {
        //                    var data = {
        //                        "SelectedStudentData": $scope.printdatatable
        //                    }
        //                    apiService.create("OralTestSchedule/ReseOralTestScheduleData", data).
        //                        then(function (promise) {
        //                            $('#myModal1').modal('hide');
        //                            $('.modal-backdrop').remove();
        //                            $('.modal').on('hide.bs.modal', function (e) {
        //                                e.stopPropagation();
        //                                $('body').css('padding-right', '');
        //                            });
        //                            $scope.BindData();
        //                            swal("Reminder sent successfully");
        //                        });
        //                }
        //            });
        //    }
        //    else {
        //        swal("No record selected to send reminder!!");
        //    }
        //};

        //$scope.EditWrittenTestScheduledata = function (EditRecord) {

        //    $scope.EditId = EditRecord.paotS_Id;
        //    $scope.myVar1 = true;
        //    var MEditId = $scope.EditId;
        //    apiService.getURI("OralTestSchedule/GetSelectedRowDetails/", MEditId).
        //        then(function (promise) {
        //            $scope.timing = moment(promise.oralTestSchedule[0].paotS_ScheduleTime, 'h:mm a').format();
        //            $scope.timing1 = moment(promise.oralTestSchedule[0].paotS_ScheduleTimeTo, 'h:mm a').format();
        //            // $scope.paotS_Id = promise.writtenTestSchedule[0].paotS_Id;
        //            $scope.ScheduleName = promise.oralTestSchedule[0].paotS_ScheduleName;
        //            // $scope.ScheduleTime = new Date(promise.oralTestSchedule[0].paotS_ScheduleTime);
        //            $scope.ScheduleTime_12 = $scope.timing;
        //            $scope.ScheduleTime_24 = $scope.timing;
        //            $scope.ScheduleTimeTo_12 = $scope.timing1;
        //            $scope.ScheduleTimeTo_24 = $scope.timing1;
        //            $scope.ScheduleDate = new Date(promise.oralTestSchedule[0].paotS_ScheduleDate);
        //            $scope.ScheduleSession = promise.oralTestSchedule[0].paotS_AM_PM;
        //            $scope.ScheduleRemark = promise.oralTestSchedule[0].paotS_Remarks;
        //            $scope.SelectedStudent = promise.selectedStudentDetails;

        //            // $scope.presentCountgrid3 = promise.SelectedStudent.length;

        //            $scope.PAOTS_Id = promise.oralTestSchedule[0].paotS_Id;
        //            $scope.Supervisor = promise.oralTestSchedule[0].paotS_Superviser;
        //            $scope.Skill = promise.oralTestSchedule[0].paotS_Skills;

        //        })
        //};
        $scope.interacted = function (field) {

            return $scope.submitted;
        };
        $scope.stuRecord = {};
        $scope.searchchkbx5 = '';
        $scope.filterchkbx5 = function (obj) {
            return (angular.lowercase(obj.hrmE_EmployeeFirstName)).indexOf(angular.lowercase($scope.searchchkbx5)) >= 0;
        };

        $scope.scsearchchkbx5 = '';
        $scope.scfilterchkbx5 = function (obj) {
            return (angular.lowercase(obj.hrmE_EmployeeFirstName)).indexOf(angular.lowercase($scope.scsearchchkbx5)) >= 0;
        };


        $scope.saveWrittenTestScheduledata = function (stuRecord) {
            $scope.submitted = true;
            $scope.staffselect = false;
            var stud = $scope.NoOfStud;
            var stud1 = $scope.presentCountgrid2;
            $scope.selectedstflist = [];
            $scope.ScheduleTime = $filter('date')($scope.ScheduleTime, "HH:mm");
            $scope.ScheduleTimeTo = $filter('date')($scope.ScheduleTimeTo, "HH:mm");
            if ($scope.pavidc === true && $scope.vidc === true) {
                if ($scope.stf === true) {
                    angular.forEach($scope.stafflist, function (obj) {
                        if (obj.select === true) {
                            $scope.staffselect = true;
                            return;
                        }
                    })

                    if ($scope.staffselect === false) {
                        swal("Kindly select any one participant!!");
                    }
                    else {
                        angular.forEach($scope.stafflist, function (sect) {
                            if (sect.select == true) {
                                $scope.selectedstflist.push({ userid: sect.userid, HRME_Id: sect.hrmE_Id });
                            }
                        });
                    }
                }
            }
            else {
                $scope.anybodystart = false;
            }

            //var startTime = moment($scope.ScheduleTime, "HH:mm");
            //var endTime = moment($scope.ScheduleTimeTo, "HH:mm");
            //var duration = moment.duration(endTime.diff(startTime));
            //var hours = parseInt(duration.asHours());
            //var minutes = parseInt(duration.asMinutes()) - hours * 60;
            //alert(hours + ' hour and ' + minutes + ' minutes.');
            //   if (stud == stud1) {
            if ($scope.myForm.$valid) {
                swal({
                    title: "Are you sure?",
                    text: "Do you want to Schedule Interview ..!?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                    cancelButtonText: "Cancel!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            if ($scope.myForm.$valid) {
                                $scope.ScheduleTime = $filter('date')($scope.ScheduleTime, "h:mm");
                                $scope.ScheduleTimeTo = $filter('date')($scope.ScheduleTimeTo, "h:mm");
                                var array = $.map(stuRecord, function (value, index) {
                                    return [value];
                                });
                                if ($scope.PAOTS_Id === "") {
                                    $scope.PAOTS_Id = 0;
                                }
                                if ($scope.NoOfMin > 0) {
                                    var time = $scope.NoOfMin;
                                } else {
                                    var time = $scope.NoOfHr;
                                }
                                var data = {
                                    "PAOTS_Id": $scope.PAOTS_Id,
                                    "PAOTSC_ScheduleName": $scope.ScheduleName,
                                    "PAOTSC_ScheduleFromTime": $scope.ScheduleTime,
                                    "PAOTSC_ScheduleToTime": $scope.ScheduleTimeTo,
                                    "PAOTSC_TimePeriod": time,
                                    "PAOTSC_TPFlag": $scope.PAOTS_TPFlag,
                                    "PAOTSC_Strength": $scope.NoOfStud,
                                    //  "PAOTS_ScheduleDate": $scope.ScheduleDate,
                                    "PAOTSC_ScheduleDate": new Date($scope.ScheduleDate).toDateString(),
                                    //"PAOTS_AM_PM": $scope.ScheduleSession,
                                    "PAOTSC_Remarks": $scope.ScheduleRemark,
                                    "SelectedStudentData": $scope.secondTableData,
                                    "SelectedStudentDataForEdit": $scope.SelectedStudent,
                                    //"PAOTS_Superviser": $scope.Supervisor,
                                    //"PAOTS_Skills": $scope.Skill,
                                    "autoschedule": $scope.autoschedule,
                                    "meetingvc": $scope.vidc,
                                    "pavidc": $scope.pavidc,
                                    "stafflag": $scope.stf,
                                    "principalflg": $scope.principalflg,
                                    "managerflg": $scope.managerflg,
                                    "anybodystart": $scope.anybodystart,
                                    stfids: $scope.selectedstflist,
                                }
                                var config = {
                                    headers: {
                                        'Content-Type': 'application/json;'
                                    }
                                }


                                apiService.create("OralTestScheduleClg/", data).

                                    then(function (promise) {
                                        if (promise.returnvalue == "false") {
                                            swal("Record Already Exist With Same Schedule Name/Date");
                                            $state.reload();
                                        }
                                        else {
                                            swal("Interview Scheduled Successfully");

                                            $state.reload();
                                        }
                                    });
                            }
                        }
                    });
            }
        };

        $scope.cancel = function () {
            $scope.submitted = false;
            $scope.ScheduleName = "";
            $scope.ScheduleName = "";
            $scope.ScheduleDate = "";
            $scope.ScheduleRemark = "";
            $scope.Supervisor = "";
            $scope.Skill = "";
            $scope.ScheduleTime_12 = null;
            $scope.ScheduleTime_24 = null;
            $scope.ScheduleTimeTo_12 = null;
            $scope.ScheduleTimeTo_24 = null;
            $scope.NoOfMin = null;
            $scope.NoOfHr = null;
            $scope.NoOfStud = null;
            $scope.submitted = false;
            $scope.PAOTS_TPFlag = false;

            $scope.albumNameArray1 = [];
            for (var i = 0; i < $scope.newuser1.length; i++) {
                if ($scope.newuser1[i].pacA_FirstName != '') {
                    if ($scope.newuser1[i].pacA_MiddleName != null && $scope.newuser1[i].pacA_MiddleName != '' && $scope.newuser1[i].pacA_MiddleName != "") {
                        if ($scope.newuser1[i].pacA_LastName != null && $scope.newuser1[i].pacA_LastName != '' && $scope.newuser1[i].pacA_LastName != "") {

                            $scope.albumNameArray1.push({ name: $scope.newuser1[i].pacA_FirstName + " " + $scope.newuser1[i].pacA_MiddleName + " " + $scope.newuser1[i].pacA_LastName, pacA_Id: $scope.newuser1[i].pacA_Id });
                        }
                        else {
                            $scope.albumNameArray1.push({ name: $scope.newuser1[i].pacA_FirstName + '' + $scope.newuser1[i].pacA_MiddleName, pacA_Id: $scope.newuser1[i].pacA_Id });
                        }
                    }
                    else {
                        if ($scope.newuser1[i].pacA_LastName != null && $scope.newuser1[i].pacA_LastName != '' && $scope.newuser1[i].pacA_LastName != "") {
                            $scope.albumNameArray1.push({ name: $scope.newuser1[i].pacA_FirstName + " " + $scope.newuser1[i].pacA_LastName, pacA_Id: $scope.newuser1[i].pacA_Id });
                        }
                        else {
                            $scope.albumNameArray1.push({ name: $scope.newuser1[i].pacA_FirstName, pacA_Id: $scope.newuser1[i].pacA_Id });
                        }
                    }

                    $scope.newuser1[i].name = $scope.albumNameArray1[i].name;
                }
            }

            angular.forEach($scope.newuser1, function (obj) {
                obj.checked = false;
                obj.makedisable = false;
            })
            $scope.makedisable = false;
            $scope.myVar1 = false;
            $scope.secondTableData = [];
            $scope.presentCountgrid2 = [];
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();

        };

        $scope.clearallsettings = function () {
            $state.reload();
        }

        //$scope.clearallsettings = function () {
        //    $scope.submitted = false;
        //    $scope.ScheduleName = "";
        //    $scope.ScheduleName = "";
        //    $scope.ScheduleDate = "";
        //    $scope.ScheduleRemark = "";
        //    $scope.Supervisor = "";
        //    $scope.Skill = "";
        //    $scope.ScheduleTime_12 = null;
        //    $scope.ScheduleTime_24 = null;
        //    $scope.ScheduleTimeTo_12 = null;
        //    $scope.ScheduleTimeTo_24 = null;
        //    $scope.NoOfMin = null;
        //    $scope.NoOfHr = null;
        //    $scope.NoOfStud = null;
        //    $scope.PAOTS_TPFlag = false;
        //    $scope.search = "";
        //    $scope.search1 = "";
        //    $scope.searchValue = "";
        //    $scope.albumNameArray1 = [];
        //    for (var i = 0; i < $scope.newuser1.length; i++) {
        //        if ($scope.newuser1[i].pasR_FirstName != '') {
        //            if ($scope.newuser1[i].pasR_MiddleName != null && $scope.newuser1[i].pasR_MiddleName != '' && $scope.newuser1[i].pasR_MiddleName != "") {
        //                if ($scope.newuser1[i].pasR_LastName != null && $scope.newuser1[i].pasR_LastName != '' && $scope.newuser1[i].pasR_LastName != "") {

        //                    $scope.albumNameArray1.push({ name: $scope.newuser1[i].pasR_FirstName + " " + $scope.newuser1[i].pasR_MiddleName + " " + $scope.newuser1[i].pasR_LastName, pasR_Id: $scope.newuser1[i].pasR_Id });
        //                }
        //                else {
        //                    $scope.albumNameArray1.push({ name: $scope.newuser1[i].pasR_FirstName + '' + $scope.newuser1[i].pasR_MiddleName, pasR_Id: $scope.newuser1[i].pasR_Id });
        //                }
        //            }
        //            else {
        //                if ($scope.newuser1[i].pasR_LastName != null && $scope.newuser1[i].pasR_LastName != '' && $scope.newuser1[i].pasR_LastName != "") {
        //                    $scope.albumNameArray1.push({ name: $scope.newuser1[i].pasR_FirstName + " " + $scope.newuser1[i].pasR_LastName, pasR_Id: $scope.newuser1[i].pasR_Id });
        //                }
        //                else {
        //                    $scope.albumNameArray1.push({ name: $scope.newuser1[i].pasR_FirstName, pasR_Id: $scope.newuser1[i].pasR_Id });
        //                }
        //            }

        //            $scope.newuser1[i].name = $scope.albumNameArray1[i].name;
        //        }
        //    }


        //    angular.forEach($scope.newuser1, function (obj) {
        //        obj.checked = false;
        //        obj.makedisable = false;
        //    })
        //    $scope.makedisable = false;
        //    $scope.myVar1 = false;
        //    $scope.secondTableData = [];
        //    $scope.presentCountgrid2 = [];
        //    $scope.myForm.$setPristine();
        //    $scope.myForm.$setUntouched();


        //};




        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.order1 = function (keyname) {
            $scope.sortKey1 = keyname;   //set the sortKey to the param passed
            $scope.reverse1 = !$scope.reverse1; //if true make it false and vice versa
        }
        $scope.order2 = function (keyname) {
            $scope.sortKey2 = keyname;   //set the sortKey to the param passed
            $scope.reverse2 = !$scope.reverse2; //if true make it false and vice versa
        }
        $scope.order3 = function (keyname) {
            $scope.sortKey3 = keyname;   //set the sortKey to the param passed
            $scope.reverse3 = !$scope.reverse3; //if true make it false and vice versa
        }

        //for Export excel


        $scope.exportToExcel = function (tableId) {
            var excelname = $scope.snameprint + "-" + $scope.sdate + "-" + $scope.sstime + "-" + $scope.sttime;

            if ($scope.printdatatablenew !== null && $scope.printdatatablenew.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'Schedule Report');
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = excelname;
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);
            }
            //else {
            //    swal("Please select records to be Exported");
            //}

        };

        $scope.exportToExcelremarks = function (tableId) {
            var excelname = $scope.snameprint + "-" + $scope.sdate + "-" + $scope.sstime + "-" + $scope.sttime;

            if ($scope.printdatatablenew !== null && $scope.printdatatablenew.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'Schedule Report-Remarkwise');
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = excelname;
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);
            }
            //else {
            //    swal("Please select records to be Exported");
            //}

        };

        $scope.exportToExceloral = function (tableId) {
            var excelname = "Scheduled List";

            if ($scope.newuser !== null && $scope.newuser.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'Schedule Report');
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = excelname;
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);
            }
            //else {
            //    swal("Please select records to be Exported");
            //}

        };
        //for print
        $scope.printData = function () {
            if ($scope.printdatatablenew !== null && $scope.printdatatablenew.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }
            //else {
            //    swal("Please Select Records to be Printed");
            //}
        }
        // end for print

        $scope.printDataremark = function () {
            if ($scope.printdatatablenew !== null && $scope.printdatatablenew.length > 0) {
                var innerContents = document.getElementById("printSectionIdre").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }
            //else {
            //    swal("Please Select Records to be Printed");
            //}
        }

        $scope.printDataoral = function () {
            if ($scope.newuser !== null && $scope.newuser.length > 0) {
                var innerContents = document.getElementById("printSectionIdo").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }
            //else {
            //    swal("Please Select Records to be Printed");
            //}
        }
    }

})();

