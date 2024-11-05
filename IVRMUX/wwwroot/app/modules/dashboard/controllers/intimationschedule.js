


(function () {
    'use strict';
    angular
.module('app')
.controller('intimationscheduleController', intimationscheduleController)

    intimationscheduleController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache']
    function intimationscheduleController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache) {

        //Date:23-12-2016 for displaying privileges.
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }
        
        $scope.testnoofstu = function (nostu, NoOfMin, NoOfHr) {
            
            angular.forEach($scope.newuser1, function (obj) {
                obj.checked = false;
                obj.makedisable = false;
            })
            $scope.makedisable = false;
            $scope.myVar1 = false;
            $scope.secondTableData = [];
            $scope.presentCountgrid2 = [];
            // $scope.validate_time_noofstudent(nostu, NoOfMin, NoOfHr);
            if (nostu != undefined && $scope.autostudent == true) {
                for (var k = 0; k < $scope.newuser1.length; k++) {
                    if (nostu < (k + 1)) {
                        break;
                    } else {
                        $scope.newuser1[k].checked = true;
                        $scope.secondTableData.push($scope.newuser1[k]);
                        angular.forEach($scope.newuser1, function (obj) {
                            angular.forEach($scope.secondTableData, function (obj1) {
                                if (obj.pasr_id == obj1.pasr_id) {
                                    obj.makedisable = true;

                                }
                            });

                        });
                    }
                    $scope.presentCountgrid2 = $scope.secondTableData.length;
                }
                //  $scope.makedisable = true;


                $scope.sortKey = "pasR_RegistrationNo";   //set the sortKey to the param passed
                $scope.reverse = false; //if true make it false and vice versa
                $scope.showcart()

            }



            // $scope.order

        }
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


        }
        $scope.noofhrs = function (e) {
            if (e >= 25) {
                swal("Hour cant be more than 24");
                $scope.NoOfHr = "";
            }
        }



        $scope.validate_time_noofstudent = function (stu, min, hr) {
            if ($scope.hrs == '12hr') {
                $scope.ScheduleTime = $scope.ScheduleTime_12;
                $scope.ScheduleTimeTo = $scope.ScheduleTimeTo_12;
            }
            else if ($scope.hrs == '24hr') {
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
            // alert( hours + ' hour and ' + minutes + ' minutes.' + e + 'no. student');
            // alert('hour to min' + hrinmin + 'hour and min in minutes' + hrtomin);
            //var nomin = $scope.noofmins;
            var tot = stu * min;
            var tot1 = stu * hr;
            var tot1tomin = tot1 * 60;
            //alert('total time for ' + e + 'no of student ' + tot + 'For total time alocated time in minutes ' + hrtomin);
            if (tot > 0) {
                if (hrtomin < tot) {

                    swal('Schedule time and mapping time of student is not possible');

                    $scope.NoOfStud = null;
                    angular.forEach($scope.newuser1, function (obj) {
                        obj.checked = false;
                        obj.makedisable = false;
                    })

                    $scope.myVar1 = false;
                }
            } else {
                if (hrtomin < tot1tomin) {

                    swal('Schedule time and mapping time of student is not possible');

                    $scope.NoOfStud = null;
                    angular.forEach($scope.newuser1, function (obj) {
                        obj.checked = false;
                        obj.makedisable = false;
                    })
                    $scope.myVar1 = false;

                }
            }


        }

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

        $scope.sortKey1 = "pasR_Id";   //set the sortKey to the param passed
        $scope.reverse1 = true; //if true make it false and vice versa

        $scope.sortKey2 = "pasR_Id";   //set the sortKey to the param passed
        $scope.reverse2 = true; //if true make it false and vice versa

        $scope.sortKey3 = "paotS_Id";   //set the sortKey to the param passed
        $scope.reverse3 = true; //if true make it false and vice versa

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
            $scope.SelectedId = SelectStudentRecord.pasR_Id;
            var SSelectedId = $scope.SelectedId;
            apiService.getURI("intimationschedule/GetStudentdetails/", SSelectedId).
       then(function (promise) {
           $scope.SelectedStudent = promise.studentDetails;

           $scope.presentCountgrid3 = promise.studentDetails.length;
       })
        };


        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
        }


        //$scope.IsHidden1 = true;
        //$scope.ShowHide1 = function () {
        //    $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        //}

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

                if (obj.pasr_id == record.pasr_id) {
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

                    if (obj.pasr_id == record.pasr_id) {
                        obj.makedisable = false;
                    }

                });
                swal("Cannot Select More Than " + $scope.NoOfStud + " No. Of Students");
            }
        };

        $scope.TempdeleteStudent = function (stuDelRecord, index) {

            stuDelRecord.checked = false;
            angular.forEach($scope.newuser1, function (obj) {

                if (obj.pasr_id == stuDelRecord.pasr_id) {
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
        $scope.currentPage1 = 1;
        $scope.currentPage2 = 1;
        $scope.currentPage3 = 1;
        $scope.currentPage4 = 1;

        $scope.itemsPerPage1 = paginationformasters;
        $scope.itemsPerPage2 = paginationformasters;
        $scope.itemsPerPage3 = paginationformasters;
        $scope.itemsPerPage4 = paginationformasters;

        $scope.BindData = function () {
            $scope.reverse = true;
            $scope.reverse1 = true;
            $scope.reverse2 = true;
            $scope.reverse3 = true;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            apiService.getDATA("intimationschedule/Getdetails").
       then(function (promise) {
           $scope.newuser = promise.oralTestSchedule;
           $scope.academiclist = promise.academicList;
           $scope.classlist = promise.classlist;
           $scope.statuslist = promise.statuslist;

           angular.forEach($scope.newuser, function (obj) {
               obj.fromtime = obj.paotS_ScheduleTime;
               obj.totime = obj.paotS_ScheduleTimeTo;
           })

           angular.forEach($scope.newuser, function (obj) {
               if (obj.paotS_ScheduleTime.substring(0, 2) > 12) {
                   obj.paotS_ScheduleTimeonly = (Number(obj.paotS_ScheduleTime.substring(0, 2)) - 12).toString();
                   if (obj.paotS_ScheduleTimeonly.length == 1) {
                       obj.paotS_ScheduleTimeoly = '0' + obj.paotS_ScheduleTimeonly;
                       obj.paotS_ScheduleTime = obj.paotS_ScheduleTimeoly + ':' + obj.paotS_ScheduleTime.substring(3, 5);
                       obj.paotS_ScheduleTime = obj.paotS_ScheduleTime + 'PM';
                   }
                   else if (obj.paotS_ScheduleTimeonly.length > 1) {
                       obj.paotS_ScheduleTime = obj.paotS_ScheduleTimeonly + ':' + obj.paotS_ScheduleTime.substring(3, 5);
                       obj.paotS_ScheduleTime = obj.paotS_ScheduleTime + 'PM';
                   }
               }
               else if (obj.paotS_ScheduleTime.substring(0, 2) == 12) {
                   obj.paotS_ScheduleTime = obj.paotS_ScheduleTime + 'PM';
               }
               else {

                   if (obj.paotS_ScheduleTime.length == 1) {
                       obj.paotS_ScheduleTime = '0' + obj.paotS_ScheduleTime;
                       obj.paotS_ScheduleTime = obj.paotS_ScheduleTime + 'AM';
                   }
                   else {
                       obj.paotS_ScheduleTime = obj.paotS_ScheduleTime + 'AM';
                   }
               }
           })

           angular.forEach($scope.newuser, function (obj) {
               if (obj.paotS_ScheduleTimeTo.substring(0, 2) > 12) {
                   obj.paotS_ScheduleTimeToonly = (Number(obj.paotS_ScheduleTimeTo.substring(0, 2)) - 12).toString();
                   if (obj.paotS_ScheduleTimeToonly.length == 1) {
                       obj.paotS_ScheduleTimeTooly = '0' + obj.paotS_ScheduleTimeToonly;
                       obj.paotS_ScheduleTimeTo = obj.paotS_ScheduleTimeTooly + ':' + obj.paotS_ScheduleTimeTo.substring(3, 5);
                       obj.paotS_ScheduleTimeTo = obj.paotS_ScheduleTimeTo + 'PM';
                   }
                   else if (obj.paotS_ScheduleTimeToonly.length > 1) {
                       obj.paotS_ScheduleTimeTo = obj.paotS_ScheduleTimeToonly + ':' + obj.paotS_ScheduleTimeTo.substring(3, 5);
                       obj.paotS_ScheduleTimeTo = obj.paotS_ScheduleTimeTo + 'PM';
                   }
               }
               else if (obj.paotS_ScheduleTimeTo.substring(0, 2) == 12) {
                   obj.paotS_ScheduleTimeTo = obj.paotS_ScheduleTimeTo + 'PM';
               }
               else {

                   if (obj.paotS_ScheduleTimeTo.length == 1) {
                       obj.paotS_ScheduleTimeTo = '0' + obj.paotS_ScheduleTimeTo;
                       obj.paotS_ScheduleTimeTo = obj.paotS_ScheduleTimeTo + 'AM';
                   }
                   else {
                       obj.paotS_ScheduleTimeTo = obj.paotS_ScheduleTimeTo + 'AM';
                   }
               }
           })

           $scope.newuser1 = promise.studentDetails;

           $scope.presentCountgrid1 = promise.studentDetails.length;
           $scope.presentCountgrid4 = promise.oralTestSchedule.length;

           $scope.SelectedStudentInCart = {};

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
                return ($filter('date')(obj.paotS_ScheduleDate, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0 || $filter('date')(obj.paotS_ScheduleTime, 'h:mm a').indexOf($scope.searchValue) >= 0 || $filter('date')(obj.paotS_ScheduleTimeTo, 'h:mm a').indexOf($scope.searchValue) >= 0 || (angular.lowercase(obj.paotS_ScheduleName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.paotS_Remarks)).indexOf(angular.lowercase($scope.searchValue)) >= 0);
                //return ($filter('date')(obj.paotS_ScheduleTime, 'HH:mm').indexOf($scope.searchValue) >= 0) || ($filter('date')(obj.paotS_ScheduleDate, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0 || (obj.paotS_ScheduleName).indexOf($scope.searchValue) >= 0 || (obj.paotS_AM_PM).indexOf($scope.searchValue) >= 0 || (obj.paotS_Remarks).indexOf($scope.searchValue) >= 0);
            }
        };

        //Academic Year Change
        $scope.yearchange = function () {
            
            $scope.ScheduleTime_12 = null;
            $scope.ScheduleTime_24 = null;
            $scope.ScheduleTimeTo_12 = null;
            $scope.ScheduleTimeTo_24 = null;
            $scope.NoOfMin = null;
            $scope.NoOfHr = null;
            $scope.NoOfStud = null;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        }


        //Class Change
        $scope.clschange = function () {
            
            $scope.ScheduleTime_12 = null;
            $scope.ScheduleTime_24 = null;
            $scope.ScheduleTimeTo_12 = null;
            $scope.ScheduleTimeTo_24 = null;
            $scope.NoOfMin = null;
            $scope.NoOfHr = null;
            $scope.NoOfStud = null;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        }

        //Status Change
        $scope.getstudetails = function (pamsT_Id) {
            
            $scope.page = "pag";
            $scope.currentPage = 1;
            $scope.ScheduleTime_12 = null;
            $scope.ScheduleTime_24 = null;
            $scope.ScheduleTimeTo_12 = null;
            $scope.ScheduleTimeTo_24 = null;
            $scope.NoOfMin = null;
            $scope.NoOfHr = null;
            $scope.NoOfStud = null;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "PAMST_Id": $scope.pamsT_Id,
            }

            apiService.create("intimationschedule/SearchData", data).then(function (promise) {
                
                if (promise.getstudentDetails != null || promise.getstudentDetails != undefined) {
                    $scope.newuser1 = promise.getstudentDetails;
                    $scope.presentCountgrid = promise.getstudentDetails.length;

                    //$scope.abc = "1";                    
                    //$scope.abc = $scope.pamsT_Id;                      
                    $scope.IsHidden1 = true;
                    $scope.Student_status = 786;
                    $scope.set_student_staus();
                }
                else {
                    $scope.IsHidden1 = false;
                    swal("No records Found");
                }
            });
        }


        $scope.BindStudentDataAfterDelete = function () {
            apiService.getDATA("intimationschedule/GetRemainStudentdetails").
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
                    apiService.create("intimationschedule/OralTestScheduleDeletesStudentData", data).

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
                    apiService.DeleteURI("intimationschedule/OralTestScheduleDeletesData", MdeleteId).
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

        $scope.EditWrittenTestScheduledata = function (EditRecord) {
            
            $scope.EditId = EditRecord.paotS_Id;
            $scope.myVar1 = true;
            var MEditId = $scope.EditId;
            apiService.getURI("intimationschedule/GetSelectedRowDetails/", MEditId).
            then(function (promise) {
                $scope.timing = moment(promise.oralTestSchedule[0].paotS_ScheduleTime, 'h:mm a').format();
                $scope.timing1 = moment(promise.oralTestSchedule[0].paotS_ScheduleTimeTo, 'h:mm a').format();
                // $scope.paotS_Id = promise.writtenTestSchedule[0].paotS_Id;
                $scope.ScheduleName = promise.oralTestSchedule[0].paotS_ScheduleName;
                // $scope.ScheduleTime = new Date(promise.oralTestSchedule[0].paotS_ScheduleTime);
                $scope.ScheduleTime_12 = $scope.timing;
                $scope.ScheduleTime_24 = $scope.timing;
                $scope.ScheduleTimeTo_12 = $scope.timing1;
                $scope.ScheduleTimeTo_24 = $scope.timing1;
                $scope.ScheduleDate = new Date(promise.oralTestSchedule[0].paotS_ScheduleDate);
                $scope.ScheduleSession = promise.oralTestSchedule[0].paotS_AM_PM;
                $scope.ScheduleRemark = promise.oralTestSchedule[0].paotS_Remarks;
                $scope.SelectedStudent = promise.selectedStudentDetails;

                // $scope.presentCountgrid3 = promise.SelectedStudent.length;

                $scope.PAOTS_Id = promise.oralTestSchedule[0].paotS_Id;
                $scope.Supervisor = promise.oralTestSchedule[0].paotS_Superviser;
                $scope.Skill = promise.oralTestSchedule[0].paotS_Skills;

            })
        };
        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };
        $scope.stuRecord = {};



        $scope.saveWrittenTestScheduledata = function (stuRecord) {
            $scope.submitted = true;
            var stud = $scope.NoOfStud;
            var stud1 = $scope.presentCountgrid2;
            $scope.ScheduleTime = $filter('date')($scope.ScheduleTime, "HH:mm");
            $scope.ScheduleTimeTo = $filter('date')($scope.ScheduleTimeTo, "HH:mm");


            //var startTime = moment($scope.ScheduleTime, "HH:mm");
            //var endTime = moment($scope.ScheduleTimeTo, "HH:mm");
            //var duration = moment.duration(endTime.diff(startTime));
            //var hours = parseInt(duration.asHours());
            //var minutes = parseInt(duration.asMinutes()) - hours * 60;
            //alert(hours + ' hour and ' + minutes + ' minutes.');
            //   if (stud == stud1) {

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
                    "PAOTS_ScheduleName": $scope.ScheduleName,
                    "PAOTS_ScheduleTime": $scope.ScheduleTime,
                    "PAOTS_ScheduleTimeTo": $scope.ScheduleTimeTo,
                    "PAOTS_TimePeriod": time,
                    "PAOTS_TPFlag": $scope.PAOTS_TPFlag,
                    "PAOTS_Strength": $scope.NoOfStud,
                    "pamsT_Id": $scope.pamsT_Id,
                    //  "PAOTS_ScheduleDate": $scope.ScheduleDate,
                    "PAOTS_ScheduleDate": new Date($scope.ScheduleDate).toDateString(),

                    //"PAOTS_AM_PM": $scope.ScheduleSession,
                    "PAOTS_Remarks": $scope.ScheduleRemark,
                    "SelectedStudentData": $scope.secondTableData,
                    "SelectedStudentDataForEdit": $scope.SelectedStudent,
                    "PAOTS_Superviser": $scope.Supervisor,
                    "PAOTS_Skills": $scope.Skill



                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }


                apiService.create("intimationschedule/", data).

                   then(function (promise) {
                       if (promise.returnvalue == "false") {
                           swal("Record Already Exist With Same Schedule Name/Date");
                           $state.reload();
                       }
                       else {
                           swal("Interview Scheduled Successfully");

                           $state.reload();
                       }

                   })
            }

            // }
            //else {
            //    swal("No. of student and student list is not equal")
            //}
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
            $scope.PAOTS_TPFlag = false;

            angular.forEach($scope.newuser1, function (obj) {
                obj.checked = false;
                obj.makedisable = false;
                // obj.newuser1 = "";
            })
            $scope.makedisable = false;
            $scope.myVar1 = false;
            $scope.Student_status = 786;
            $scope.set_student_staus();
            $scope.secondTableData = [];
            $scope.presentCountgrid2 = [];

            $scope.asmaY_Id = "";
            $scope.asmcL_Id = "";
            $scope.pamsT_Id = "";
            $scope.IsHidden1 = false;
            // $scope.statuslist = "";

            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();

        };
        

        $scope.set_student_staus = function () {
            if ($scope.Student_status != "786") {
                angular.forEach($scope.newuser1, function (opq) {
                    opq.pamsT_Id = $scope.Student_status;
                    $scope.abc = opq.pamsT_Id;
                })
            }

        }

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
    }

})();

