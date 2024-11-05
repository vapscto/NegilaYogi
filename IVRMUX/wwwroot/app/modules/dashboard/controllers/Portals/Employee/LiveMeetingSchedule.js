(function () {
    'use strict';
    angular.module('app').controller('LiveMeetingScheduleController', LiveMeetingScheduleController)

    LiveMeetingScheduleController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache']
    function LiveMeetingScheduleController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache) {

        $scope.disablestaff = true;
        $scope.disableclass = true;
        $scope.meetingsubjectslist = [];
        $scope.savemgs = "save";
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
        $scope.LoadData = function () {

            apiService.getDATA("LiveMeetingSchedule/getdetails").then(function (promise) {
                $scope.yearlt = promise.academicList;
                $scope.subjlist = promise.subjlist;
                $scope.teacherslist = promise.teacherslist;
                $scope.meetinglist = promise.meetinglist;
                $scope.stafflist = promise.stafflist;
                $scope.allstafflist = promise.allstafflist;
                $scope.asmaY_Id = promise.asmaY_Id;
                $scope.adminstaffflag = promise.adminstaffflag;

                if (promise.adminstaffflag === false) {
                    $scope.disablestaff = false;
                    if ($scope.teacherslist !== null && $scope.teacherslist.length > 0) {
                        $scope.HRME_Id = $scope.teacherslist[0];
                        $scope.OnAcdyear($scope.asmaY_Id);
                    } else {
                        swal("Staff Don't Have Previlages To Create Meeting");
                    }

                }
                $scope.LMSLMEET_PlannedDate = new Date($scope.today);
                $scope.min_LMSLMEET_PlannedDate = new Date($scope.today);


                $scope.meetingclassecsub = [];
                angular.forEach($scope.meetinglist, function (mm) {
                    $scope.meetingclassecsub.push({ lmslmeeT_Id: mm.lmslmeeT_Id, asmcL_ClassName: mm.asmcL_ClassName, asmC_SectionName: mm.asmC_SectionName, ismS_IVRSSubjectName: mm.ismS_IVRSSubjectName });
                });

                var newArray = [];
                angular.forEach($scope.meetinglist, function (value, key) {
                    var exists = false;
                    angular.forEach(newArray, function (val2, key) {
                        if (angular.equals(value.lmslmeeT_Id, val2.lmslmeeT_Id)) { exists = true };
                    });
                    if (exists === false && value.lmslmeeT_Id !== "") { newArray.push(value); }
                });

                $scope.meetinglist = newArray;

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

            });
        };

        $scope.showgrid = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 10;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.searchValue = '';

        $scope.meetingsubjectslisttemp = [];
        $scope.add = function () {
            if ($scope.FOMST_IHalfLoginTime != null) {
                var checkfromdate1 = $scope.LMSLMEET_PlannedDate === null ? "" : $filter('date')($scope.LMSLMEET_PlannedDate, "yyyy-MM-dd");
                $scope.getserverdate();
                $scope.dateof = new Date($scope.today);
               
                var checkfromdate2 = $scope.dateof === null ? "" : $filter('date')($scope.dateof, "yyyy-MM-dd");
                if (checkfromdate1 === checkfromdate2) {                    
                    var alignFillDate = new Date($scope.FOMST_IHalfLoginTime);                    
                    var pickUpDate = new Date($scope.dateof);
                    if (alignFillDate < pickUpDate) {
                        swal("kindly select upcoming time!!");
                        return;
                    }
                }
            }
            if ($scope.myForm.$valid) {
                if ($scope.ASMCL_Id > 0 && $scope.ASMS_Id > 0 && $scope.ismS_Id > 0) {
                    var classname = "";
                    var sectioname = "";
                    var subjectname = "";
                    var classid = "";
                    var sectionid = "";
                    var subjectid = "";
                    var clcode = "";
                    var seccode = "";
                    var subcode = "";
                    $scope.duplciate = false;
                    $scope.disablestaff = false;

                    angular.forEach($scope.classList, function (cat) {
                        if (cat.asmcL_Id === Number($scope.ASMCL_Id)) {
                            classname = cat.asmcL_ClassName;
                            classid = cat.asmcL_Id;
                            clcode = cat.asmcL_ClassCode;
                        }
                    });
                    angular.forEach($scope.sectionList, function (cat) {
                        if (cat.asmS_Id === Number($scope.ASMS_Id)) {
                            sectioname = cat.asmC_SectionName;
                            sectionid = cat.asmS_Id;
                            seccode = cat.asmC_SectionCode;
                        }
                    });
                    angular.forEach($scope.subjlist, function (cat) {
                        if (cat.ismS_Id === Number($scope.ismS_Id)) {
                            subjectname = cat.ismS_SubjectName;
                            subjectid = cat.ismS_Id;
                            subcode = cat.ismS_SubjectCode;
                        }
                    });
                    var count = 0;
                    if ($scope.meetingsubjectslisttemp.length > 0) {
                        angular.forEach($scope.meetingsubjectslisttemp, function (cat) {
                            if (cat.classid === classid) {
                                if (cat.classid === classid && cat.sectionid === sectionid && cat.subjectid === subjectid) {
                                    count += 1;
                                    $scope.duplciate = true;
                                    swal("Record Alreday Added!");
                                }
                                else if (subjectid !== $scope.meetingsubjectslisttemp[0].subjectid) {
                                    $scope.duplciate = true;
                                    count += 1;
                                    swal("Select same subject for the meeting!");
                                }
                            }
                            else {
                                $scope.duplciate = true;
                                count += 1;
                                swal("Select same class for the meeting!");
                            }
                        });
                        if (count === 0) {
                            $scope.disableclass = false;
                            $scope.meetingsubjectslisttemp.push({ classname: classname, sectioname: sectioname, subjectname: subjectname, classid: classid, sectionid: sectionid, subjectid: subjectid, clcode: clcode, seccode: seccode, subcode: subcode });
                        }
                    }
                    else {
                        $scope.disableclass = false;
                        $scope.meetingsubjectslisttemp.push({ classname: classname, sectioname: sectioname, subjectname: subjectname, classid: classid, sectionid: sectionid, subjectid: subjectid, clcode: clcode, seccode: seccode, subcode: subcode });
                    }
                    $scope.saveaarylist = [];
                    angular.forEach($scope.meetingsubjectslisttemp, function (cls) {
                        $scope.saveaarylist.push({ asmcL_Id: cls.classid, asmS_Id: cls.sectionid, ismS_Id: cls.subjectid });
                    });
                    if ($scope.duplciate === false) {
                        var fromdate1 = $scope.LMSLMEET_PlannedDate === null ? "" : $filter('date')($scope.LMSLMEET_PlannedDate, "yyyy-MM-dd");
                        var data = {
                            "HRME_Id": $scope.HRME_Id.hrmE_Id,
                            "ASMAY_Id": $scope.asmaY_Id,
                            "LMSLMEET_PlannedDate": fromdate1,
                            "LMSLMEET_PlannedStartTime": $filter('date')($scope.FOMST_IHalfLoginTime, "HH:mm"),
                            "LMSLMEET_PlannedEndTime": $filter('date')($scope.FOMST_IIHalfLogoutTime, "HH:mm"),
                            saveaarylist: $scope.saveaarylist
                        };
                        apiService.create("LiveMeetingSchedule/checkduplicate", data).then(function (promise) {
                            if (promise.duplicatemeetingemp.length > 0) {
                                $scope.duplicatemeetin = promise.duplicatemeetingemp;

                                $scope.meetingclassecsubdup = [];
                                angular.forEach($scope.duplicatemeetin, function (mm) {
                                    $scope.meetingclassecsubdup.push({ lmslmeeT_Id: mm.lmslmeeT_Id, asmcL_ClassName: mm.asmcL_ClassName, asmC_SectionName: mm.asmC_SectionName, ismS_IVRSSubjectName: mm.ismS_IVRSSubjectName });
                                });
                                var newArraydup = [];
                                angular.forEach($scope.duplicatemeetin, function (value, key) {
                                    var exists = false;
                                    angular.forEach(newArraydup, function (val2, key) {
                                        if (angular.equals(value.lmslmeeT_Id, val2.lmslmeeT_Id)) { exists = true };
                                    });
                                    if (exists === false && value.lmslmeeT_Id !== "") { newArraydup.push(value); }
                                });
                                $scope.duplicatemeetin = newArraydup;

                                angular.forEach($scope.meetingsubjectslisttemp, function (cat) {
                                    $scope.meetingsubjectslisttemp.splice($scope.meetingsubjectslisttemp.length - 1);
                                });
                                angular.forEach($scope.meetingsubjectslist, function (cat) {
                                    $scope.meetingsubjectslist.splice($scope.meetingsubjectslist.length - 1);
                                });

                                $('#myModalViewdup').modal('show');
                                $scope.disablestaff = true;
                                $scope.disableclass = true;
                                swal("Meeting is already scheduled for this employee at same/between time!");
                            }
                            else if (promise.duplicatemeetingclass.length > 0) {
                                $scope.duplicatemeetin = promise.duplicatemeetingclass;

                                $scope.meetingclassecsubdup = [];
                                angular.forEach($scope.duplicatemeetin, function (mm) {
                                    $scope.meetingclassecsubdup.push({ lmslmeeT_Id: mm.lmslmeeT_Id, asmcL_ClassName: mm.asmcL_ClassName, asmC_SectionName: mm.asmC_SectionName, ismS_IVRSSubjectName: mm.ismS_IVRSSubjectName });
                                });

                                var newArraydupp = [];
                                angular.forEach($scope.duplicatemeetin, function (value, key) {
                                    var exists = false;
                                    angular.forEach(newArraydupp, function (val2, key) {
                                        if (angular.equals(value.lmslmeeT_Id, val2.lmslmeeT_Id)) { exists = true };
                                    });
                                    if (exists === false && value.lmslmeeT_Id !== "") { newArraydupp.push(value); }
                                });
                                $scope.duplicatemeetin = newArraydupp;

                                angular.forEach($scope.meetingsubjectslisttemp, function (cat) {
                                    $scope.meetingsubjectslisttemp.splice($scope.meetingsubjectslisttemp.length - 1);
                                });
                                angular.forEach($scope.meetingsubjectslist, function (cat) {
                                    $scope.meetingsubjectslist.splice($scope.meetingsubjectslist.length - 1);
                                });
                                $scope.disablestaff = true;
                                $scope.disableclass = true;
                                $('#myModalViewdup').modal('show');
                                swal("Meeting is already scheduled for this class-section at same/between time!");
                            }
                            else {
                                $scope.meetingsubjectslist = $scope.meetingsubjectslisttemp;
                            }
                            if ($scope.duplicatemeetin.length > 0) {
                                angular.forEach($scope.duplicatemeetin, function (obj) {
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

                                angular.forEach($scope.duplicatemeetin, function (obj) {
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
                            }
                        });
                    }
                }
            }
            else {
                $scope.submitted = true;
            }

        };

        $scope.remove = function (obj, j) {
            angular.forEach($scope.meetingsubjectslist, function (cat, i) {
                if (i === j) {
                    $scope.meetingsubjectslist.splice(i, 1);
                }
            });
            //angular.forEach($scope.meetingsubjectslisttemp, function (cat, i) {
            //    if (i === j) {
            //        $scope.meetingsubjectslisttemp.splice(i, 1);
            //    }
            //});
            $scope.meetingsubjectslisttemp = $scope.meetingsubjectslist;
        };

        $scope.GetReport = function () {
            $scope.saveaarylist = [];
            $scope.selectedClasslist = [];
            $scope.selectedSectionlist = [];
            $scope.selectedsublist = [];
            $scope.selectedstflist = [];
            if ($scope.stf === true || $scope.stud === true) {
                if ($scope.myForm.$valid) {

                    angular.forEach($scope.meetingsubjectslist, function (cls) {
                        $scope.selectedClasslist.push({ asmcL_Id: cls.classid, asmcL_ClassCode: cls.clcode });
                        $scope.selectedSectionlist.push({ asmS_Id: cls.sectionid, asmC_SectionCode: cls.seccode });
                        $scope.selectedsublist.push({ ismS_Id: cls.subjectid, ismS_SubjectCode: cls.subcode });
                        $scope.saveaarylist.push({ asmcL_Id: cls.classid, asmS_Id: cls.sectionid, ismS_Id: cls.subjectid });
                    });

                    angular.forEach($scope.stafflist, function (sect) {
                        if (sect.select == true) {
                            $scope.selectedstflist.push({ UserId: sect.userId, HRME_Id: sect.hrmE_Id });
                        }
                    });

                    var fromdate1 = $scope.LMSLMEET_PlannedDate == null ? "" : $filter('date')($scope.LMSLMEET_PlannedDate, "yyyy-MM-dd");
                    var data = {
                        "LMSLMEET_Id": $scope.LMSLMEET_Id,
                        "HRME_Id": $scope.HRME_Id.hrmE_Id,
                        "stafflag": $scope.stf,
                        "studflag": $scope.stud,
                        "principalflg": $scope.principalflg,
                        "hodflg": $scope.hodflg,
                        "managerflg": $scope.managerflg,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "LMSLMEET_MeetingTopic": $scope.LMSLMEET_MeetingTopic,
                        "LMSLMEET_PlannedDate": fromdate1,
                        "LMSLMEET_PlannedStartTime": $filter('date')($scope.FOMST_IHalfLoginTime, "HH:mm"),
                        "LMSLMEET_PlannedEndTime": $filter('date')($scope.FOMST_IIHalfLogoutTime, "HH:mm"),
                        selectedClasslist: $scope.selectedClasslist,
                        secids: $scope.selectedSectionlist,
                        subids: $scope.selectedsublist,
                        stfids: $scope.selectedstflist,
                        saveaarylist: $scope.saveaarylist
                    };

                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    };
                    swal({
                        title: "Are you sure",
                        text: "Do you want to " + $scope.savemgs + " the Meeting?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + $scope.savemgs + " it!",
                        cancelButtonText: "Cancel",
                        closeOnConfirm: false,
                        closeOnCancel: false
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                apiService.create("LiveMeetingSchedule/savedata", data).then(function (promise) {
                                    if (promise.returnval === true) {
                                        $scope.LoadData();
                                        swal('Meeting Scheduled Successfully');
                                    }
                                    else if (promise.returnvalue === "User") {
                                        swal('Meeting not scheduled kindly check your Teams login details.','Contact administrator!');
                                    }
                                    else if (promise.returnvalue === "Duplicate") {

                                        $scope.gridOptions1.data = promise.filldata;

                                        swal('Record already exist.');
                                    }
                                    else {
                                        swal('Record Not Saved/Updated Successfully');
                                    }

                                    $state.reload();
                                });
                            }
                            else {
                                swal("Meeting " + $scope.savemgs + " Cancelled");
                            }
                        });
                }
                else {
                    $scope.submitted = true;
                }
            }
            else {
                swal('Select Student OR Staff Details');
            }
        };


        $scope.edit = function (data) {
            $scope.savemgs = "update";
            apiService.create("LiveMeetingSchedule/editdata", data).then(function (promise) {
                $scope.editlist = promise.editlist;
                $scope.classList = promise.classlist;
                //$scope.sectionList = promise.sectionList;
                $scope.editedmeetinglist = promise.editedmeetinglist;
                if ($scope.editedmeetinglist != null && $scope.editedmeetinglist.length > 0) {
                    $scope.meetingsubjectslist = $scope.editedmeetinglist;
                    $scope.meetingsubjectslisttemp = $scope.editedmeetinglist;
                }
                $scope.LMSLMEET_Id = promise.editlist[0].lmslmeeT_Id;
                $scope.hrmeid = promise.editlist[0].hrmE_Id;
                $scope.stf = promise.stafflag;
                $scope.stud = promise.studflag;
                $scope.principalflg = true;
                $scope.hodflg = true;
                $scope.managerflg = true;
                $scope.LMSLMEET_MeetingTopic = promise.editlist[0].lmslmeeT_MeetingTopic;
                $scope.LMSLMEET_PlannedDatetemp = new Date(promise.editlist[0].lmslmeeT_PlannedDate);
                $scope.FOMST_IHalfLoginTime = moment(promise.editlist[0].lmslmeeT_PlannedStartTime, 'HH:mm').format();
                $scope.FOMST_IIHalfLogoutTime = moment(promise.editlist[0].lmslmeeT_PlannedEndTime, 'HH:mm').format();

                $scope.getserverdate();
                $scope.dateof = new Date($scope.today);
                var alignFillDate = new Date($scope.LMSLMEET_PlannedDatetemp);
                var pickUpDate = new Date($scope.dateof);
                if (alignFillDate > pickUpDate) {
                    $scope.LMSLMEET_PlannedDate = $scope.LMSLMEET_PlannedDatetemp;
                }
                else {
                    $scope.LMSLMEET_PlannedDate = "";
                    $scope.min_LMSLMEET_PlannedDate = new Date($scope.today);
                }
                //if (promise.emp_punchDetails[0].asmaY_Id > 0) {
                //    $scope.OnAcdyear(promise.emp_punchDetails[0].asmaY_Id);
                //}
                if ($scope.allstafflist.length > 0) {
                    angular.forEach($scope.allstafflist, function (pro) {
                        if (pro.hrmE_Id === $scope.hrmeid) {
                            $scope.$parent.HRME_Id = pro;
                        }
                    });
                }

                if ($scope.stud == true) {
                    $scope.asmaY_Id = promise.emp_punchDetails[0].asmaY_Id;
                    angular.forEach($scope.subjlist, function (ee) {
                        angular.forEach(promise.emp_punchDetails, function (xx) {
                            if (xx.ismS_Id == ee.ismS_Id) {
                                ee.select = true;
                            }
                        });
                    });
                    if ($scope.classList !== undefined) {
                        angular.forEach($scope.classList, function (ee) {
                            angular.forEach(promise.emp_punchDetails, function (xx) {
                                if (xx.asmcL_Id == ee.asmcL_Id) {
                                    ee.select = true;
                                }
                            });
                        });
                    }
                    angular.forEach($scope.sectionList, function (ee) {
                        angular.forEach(promise.emp_punchDetails, function (xx) {
                            if (xx.asmS_Id == ee.asmS_Id) {
                                ee.select = true;
                            }
                        });
                    });
                }
                if ($scope.stf == true) {
                    angular.forEach($scope.stafflist, function (ee) {
                        angular.forEach(promise.empdetails, function (xx) {
                            if (xx.hrmE_Id == ee.hrmE_Id) {
                                ee.select = true;
                            }
                        });
                    });
                }
                $scope.scroll();
            });
        };

        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        }

        $scope.isOptionsRequiredsub = function () {
            if ($scope.stud == true) {
                return !$scope.subjlist.some(function (options) {
                    return options.select;
                });
            }
        };

        $scope.isOptionsRequiredcls = function () {
            if ($scope.stud == true) {
                return !$scope.classList.some(function (options) {
                    return options.select;
                });
            }
        };

        $scope.isOptionsRequiredcls1 = function () {
            if ($scope.stf == true) {
                return !$scope.stafflist.some(function (options) {
                    return options.select;
                });
            }
        };

        $scope.isOptionsRequiredsec = function () {
            if ($scope.stud == true) {
                return !$scope.sectionList.some(function (options) {
                    return options.select;
                });
            }
        };

        $scope.submitted = false;
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.searchfilter = function (studentlst) {
            var studid = studentlst.amst_Id;
            var data = {
                "Amst_Id": studid
            };

            apiService.create("LiveMeetingSchedule/getstudentdetails", data).then(function (promise) {
                $scope.studentlistall = promise.fillstudentalldetails;
                $scope.examdetails = promise.examlist;
                if ($scope.studentlistall != null) {
                    $scope.showStudentD = true;

                } else {
                    $scope.showStudentD = false;

                }
                if ($scope.examdetails != null && $scope.examdetails != 0) {
                    $scope.showExamD = true;

                } else {
                    $scope.showExamD = false;
                    swal("Student did't attend any Exam....!!");
                }
            });
        };

        $scope.section = [];
        $scope.OnAcdyear = function (asmaY_Id) {
            $scope.classList = [];
            $scope.sectionList = [];
            $scope.subjlist = [];
            $scope.meetingsubjectslist = [];
            $scope.meetingsubjectslisttemp = [];
            $scope.ismS_Id = "";
            $scope.ASMS_Id = "";
            $scope.ASMCL_Id = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "HRME_Id": $scope.HRME_Id.hrmE_Id
            };

            apiService.create("LiveMeetingSchedule/getclass", data).then(function (promise) {
                $scope.classList = promise.classlist;
            });
        };

        $scope.selectedClasslist = [];

        $scope.getsection = function (ASMCLId) {
            $scope.sectionList = [];
            $scope.subjlist = [];
            $scope.ismS_Id = "";
            $scope.ASMS_Id = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": ASMCLId,
                "HRME_Id": $scope.HRME_Id.hrmE_Id
            };
            apiService.create("LiveMeetingSchedule/getsection", data).then(function (promise) {
                $scope.sectionList = promise.sectionList;
            });
        };

        $scope.getsubject = function (ASMSId) {

            $scope.subjlist = [];
            $scope.ismS_Id = "";

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": ASMSId,
                "HRME_Id": $scope.HRME_Id.hrmE_Id
            };
            apiService.create("LiveMeetingSchedule/getsubject", data).then(function (promise) {
                $scope.subjlist = promise.subjlist;
            });
        };


        $scope.hmin = new Date($scope.today);
        $scope.hmin.setHours(0);
        $scope.hmin.setMinutes(0);
        $scope.FOMST_HDWHrMin = $scope.hmin;

        var d = new Date($scope.today);
        d.setHours(0);
        d.setMinutes(0);
        $scope.min = d;

        var maxsnfttim = new Date($scope.today);
        maxsnfttim.setHours(18);
        maxsnfttim.setMinutes(0);
        $scope.maxtimme = maxsnfttim;

        var d2 = new Date($scope.today);
        d2.setHours(7);
        d2.setMinutes(0);
        $scope.max = d2;

        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };

        $scope.ismeridian = false;
        $scope.validatemax = function (maxdata) {

            // $scope.FOMST_IHalfLoginTime = maxdata;
            //$scope.FOMST_IIHalfLogoutTime = "";
            var dsttimee = $scope.FOMST_IHalfLoginTime;
            $scope.sresult = $filter('date')(dsttimee, 'HH:mm:ss a');
            $scope.eresult = $filter('date')(maxdata, 'HH:mm:ss a');
            //var startTime = moment(dsttimee, "HH:mm:ss a");
            //  var endTime = moment(maxdata, "HH:mm:ss a");
            var startTime = moment($scope.sresult, "HH:mm:ss a");
            var endTime = moment($scope.eresult, "HH:mm:ss a");
            var duration = moment.duration(endTime.diff(startTime));
            var hours = parseInt(duration.asHours());
            var minutes = parseInt(duration.asMinutes()) - hours * 60;
            var finlrst = hours + ":" + minutes;

            $scope.tmin = new Date($scope.today);
            $scope.tmin.setHours(hours);
            $scope.tmin.setMinutes(minutes);
            $scope.tmax = new Date($scope.today);
            $scope.tmax.setHours(hours);
            $scope.tmax.setMinutes(minutes);

            $scope.ttst = new Date($scope.today);
            $scope.ttst.setHours(hours);
            $scope.ttst.setMinutes(minutes);

            $scope.FOMST_FDWHrMin = $scope.ttst;

            $scope.htmax = new Date($scope.today);
            $scope.htmax.setHours(hours);
            $scope.htmax.setMinutes(minutes);

            if (maxdata >= new Date($scope.FOMST_IHalfLoginTime)) {
                $scope.totimemax = maxdata;
                var hh = $scope.totimemax.getHours();
                var mm = $scope.totimemax.getMinutes();
                $scope.max = maxdata;

                $scope.max.setMinutes(hh);
                $scope.max.setMinutes(mm);
            }
            else {
                $scope.FOMST_IIHalfLogoutTime = "";
            }
        };


        $scope.deactive = function (employee) {
            var flag = employee.lmslmeeT_ActiveFlg;
            var mgs = "";
            var confirmmgs = "";

            var data = {
                "LMSLMEET_Id": employee.lmslmeeT_Id,
            };

            if (flag === true) {
                mgs = "Cancel";
                confirmmgs = "Cancelled";
            }
            else {
                mgs = "Re-Schedule";
                confirmmgs = "Re-Schedule";

            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " the Meeting??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("LiveMeetingSchedule/deactive", data).then(function (promise) {
                            if (promise.returnval == true) {
                                swal("Meeting " + confirmmgs + " " + "successfully");
                            }
                            else {
                                swal("Meeting " + mgs + " Failed");
                            }
                            $state.reload();
                        });
                    }
                    else {
                        swal("Meeting " + mgs + " Cancelled");
                    }
                });
        };

        $scope.cleardata = function () {
            $state.reload();
        };

        $scope.validateTomintime = function (timedata) {

            //$scope.timedis1 = false;
            //$scope.timedis2 = true;
            $scope.FOMST_IIHalfLogoutTime = "";
            $scope.totime = timedata;
            var hh = $scope.totime.getHours();
            var mm = $scope.totime.getMinutes();
            $scope.min = timedata;

            $scope.min.setHours(hh);
            $scope.min.setMinutes(mm);
            $scope.minlnc = timedata;

            $scope.minlnc.setHours(hh);
            $scope.minlnc.setMinutes(mm);
            $scope.FOMST_IHalfLogoutTime = "";
        };
        $scope.searchchkbx23 = '';
        $scope.filterchkbx23 = function (obj) {
            return (angular.lowercase(obj.sectionname)).indexOf(angular.lowercase($scope.searchchkbx23)) >= 0;
        };
        $scope.searchchkbx231 = '';
        $scope.filterchkbx231 = function (obj) {
            return (angular.lowercase(obj.ismS_SubjectName)).indexOf(angular.lowercase($scope.searchchkbx231)) >= 0;
        };
        $scope.searchchkbx = '';
        $scope.filterchkbx = function (obj) {
            return (angular.lowercase(obj.classname)).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        };
        $scope.searchchkbx5 = '';
        $scope.filterchkbx5 = function (obj) {
            return (angular.lowercase(obj.hrmE_EmployeeFirstName)).indexOf(angular.lowercase($scope.searchchkbx5)) >= 0;
        };
        $scope.togchkbx = function () {
            $scope.usercheck = $scope.classList.every(function (options) {
                return options.select;
            });
            $scope.getsection();
        };
    }
})();