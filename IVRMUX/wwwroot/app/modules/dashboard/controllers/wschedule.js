
(function () {
    'use strict';
    angular
.module('app')
.controller('WrittenTestScheduleController', WrittenTestScheduleController)

    WrittenTestScheduleController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function WrittenTestScheduleController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {

        //Date:23-12-2016 for displaying privileges.
        //$scope.searchValue = '';
        //$scope.filterValue = function (obj) {
        //    return ($filter('date')(obj.pasE_Date, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) || JSON.stringify(obj.pasR_MobileNo).indexOf($scope.searchValue) >= 0 || (angular.lowercase(obj.pasR_FirstName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.pasR_MiddleName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.pasR_LastName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.pasR_RegistrationNo)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.pasR_Sex)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.pasR_emailId)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        //}

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return ($filter('date')(obj.pawtS_ScheduleDate, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) || ($filter('date')(obj.pawtS_ScheduleTime, 'h:mm a').indexOf($scope.searchValue) >= 0) || ($filter('date')(obj.pawtS_ScheduleTimeTo, 'h:mm a').indexOf($scope.searchValue) >= 0) || JSON.stringify(obj.pasR_MobileNo).indexOf($scope.searchValue) >= 0 || (angular.lowercase(obj.pawtS_ScheduleName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.pawtS_Remarks)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }
        $scope.disableedit = false;

        $scope.search = "";
        $scope.IsHiddendown = false;

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
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



        var paginationformasters=10;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !=null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.sortKey = "pasR_Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.sortKey1 = "pasR_Id";   //set the sortKey to the param passed
        $scope.reverse1 = true; //if true make it false and vice versa

        $scope.sortKey2 = "pasR_Id";   //set the sortKey to the param passed
        $scope.reverse2 = true; //if true make it false and vice versa

        $scope.sortKey3 = "pawtS_Id";   //set the sortKey to the param passed
        $scope.reverse3 = true; //if true make it false and vice versa

        $scope.presentCountgrid1 = 0;
        $scope.presentCountgrid2 = 0;
        $scope.presentCountgrid3 = 0;
        $scope.presentCountgrid4 = 0;


        //$scope.ScheduleTime = new Date();
        //$scope.ScheduleTimeTo = new Date();
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.SelectStudentRecord = {};
        $scope.SelectedStudentRecord = {};
        $scope.tempArray = [];
        $scope.stuDelRecord = {};
        $scope.minDate = new Date();

        $scope.secondTableData = [];

        $scope.showBinddata = function () {
            // $scope.myVar = !$scope.myVar;
            $scope.myVar = true;
        };

        $scope.showcart = function () {
            // $scope.myVar = !$scope.myVar;
            $scope.myVar1 = true;
        };

        // time picker starts
        //$scope.timedis = true;
        //$scope.ScheduleTime = new Date();

        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };

        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        //time picker ends



        $scope.SelectStudent = function (SelectStudentRecord) {
            $scope.SelectedId = SelectStudentRecord.pasR_Id;
            var SSelectedId = $scope.SelectedId;
            apiService.getURI("WrittenTestSchedule/GetStudentdetails/", SSelectedId).
       then(function (promise) {
           $scope.SelectedStudent = promise.studentDetails;

           $scope.presentCountgrid3 = promise.studentDetails.length;
       })
        };
        $scope.TempdeleteStudent = function (index, stuDelRecord) {

            stuDelRecord.checked = false;

            console.log($scope.secondTableData.indexOf(stuDelRecord));
            $scope.secondTableData.splice($scope.secondTableData.indexOf(stuDelRecord), 1);

            $scope.presentCountgrid2 = $scope.secondTableData.length;
        };
        $scope.TempSelectStudent = function (record) {

            
            if ($scope.secondTableData.indexOf(record) === -1) {
                $scope.secondTableData.push(record);

            }
            else {
                $scope.secondTableData.splice($scope.secondTableData.indexOf(record), 1);

            }
            $scope.presentCountgrid2 = $scope.secondTableData.length;
        };




        $scope.clearallsettings = function () {
            $state.reload();
        };



        $scope.paginate = "paginate";
        $scope.paginate1 = "paginate1";
        $scope.paginate2 = "paginate2";
        $scope.paginate3 = "paginate3";

        $scope.currentPage = 1;
        $scope.currentPage1 = 1;
        $scope.currentPage2 = 1;
        $scope.currentPage3 = 1;

        $scope.itemsPerPage = paginationformasters;
        $scope.itemsPerPage1 = paginationformasters;
        $scope.itemsPerPage2 = paginationformasters;
        $scope.itemsPerPage3 = paginationformasters;
        $scope.BindData = function () {

            $scope.reverse = true;
            $scope.reverse1 = true;
            $scope.reverse2 = true;
            $scope.reverse3 = true;

            if ($scope.asmcL_Id == null || $scope.asmcL_Id == undefined) {
                $scope.asmcL_Id = 0;
            }

            var Data = {
                "ASMCL_ID":$scope.asmcL_Id
            };

            apiService.create("WrittenTestSchedule/Getdetails", Data).
       then(function (promise) {
           $scope.secondTableData = [];
           $scope.newuser = promise.writtenTestSchedule;
           $scope.schedulecount = promise.schedulecount;
           if (promise.studentDetails.length > 0) {
               $scope.newuser1 = promise.studentDetails;
           }
           else {
               $scope.newuser1 = {};
               swal("No students found to schedule!!");
           }

          
           $scope.presentCountgrid1 = promise.studentDetails.length;
           $scope.presentCountgrid4 = promise.writtenTestSchedule.length;

           $scope.arrlist5 = promise.admissioncatdrp;
           $scope.classlist = promise.admissioncatdrpall;
           if (promise.admissioncatdrpall.length > 0) {
               for (var i = 0; i < $scope.newuser1.length; i++) {
                   for (var j = 0; j < promise.admissioncatdrpall.length; j++) {
                       if ($scope.newuser1[i].asmcL_Id == promise.admissioncatdrpall[j].asmcL_Id) {
                           $scope.newuser1[i].classname = promise.admissioncatdrpall[j].asmcL_ClassName;
                       }
                   }
               }
           }
           $scope.classid = $scope.asmcL_Id;
           $scope.SelectedStudentInCart = {};

       })

            $scope.pageChangeHandler = function (num) {
                console.log('first grid ' + num);
            };

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

            $scope.searchValue = '';
            $scope.filterValue = function (obj) {
                // 
                return ($filter('date')(obj.pawtS_ScheduleTime, 'HH:mm').indexOf($scope.searchValue) >= 0) || ($filter('date')(obj.pawtS_ScheduleDate, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0 || (angular.lowercase(obj.pawtS_ScheduleName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.pawtS_Remarks)).indexOf(angular.lowercase($scope.searchValue)) >= 0);
            }
        };



        $scope.testnoofstu = function (nostu) {
            var sbc = $scope.ScheduleTime;
            var sbxc = $scope.ScheduleTimeTo;
            angular.forEach($scope.newuser1, function (obj) {
                obj.checked = false;
                obj.makedisable = false;
            })
            $scope.makedisable = false;
            $scope.myVar1 = false;
            $scope.secondTableData = [];
            $scope.presentCountgrid2 = [];
          
            if (nostu != undefined) {
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
                $scope.sortKey = "pasR_RegistrationNo";   //set the sortKey to the param passed
                $scope.reverse1 = false; //if true make it false and vice versa
                $scope.showcart()
            }
            
        }


        $scope.BindStudentDataAfterDelete = function () {
            apiService.getDATA("WrittenTestSchedule/GetRemainStudentdetails").
       then(function (promise) {
           $scope.SelectedStudent = promise.selectedStudentDetails;


           $scope.presentCountgrid = promise.selectedStudentDetails.length;
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

        $scope.DeleteWrittenTestScheduleStudentdata = function (DeleteRecord, SweetAlert) {
            $scope.deleteId = DeleteRecord.pasR_Id;
            var MdeleteId = $scope.deleteId;


            swal({
                title: "Are you sure?",
                text: "Do you want to Delete this record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,Delete it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("WrittenTestSchedule/WrittenTestScheduleDeletesStudentData", MdeleteId).
                    then(function (promise) {
                        swal("Record Deleted Successfully");
                        $state.reload();

                    })
                }
                else {
                    swal(" Cancelled", "Ok");
                }
            });
        }


        //Delete Test Schedule Student

        $scope.DeleteWrittenTestScheduleStudentdata = function (DeleteRecord, SweetAlert) {

            $scope.deleteId = DeleteRecord.pasr_id;
            var MdeleteId = $scope.deleteId;
            var PAWTS_Id = $scope.PAWTS_Id;
            var data = {
                "PAWTS_Id": $scope.PAWTS_Id,
                "PASR_Id": MdeleteId
            }

            //$scope.deleteId = DeleteRecord.pasR_Id;
            //var MdeleteId = $scope.deleteId;
            swal({
                title: "Are you sure?",
                text: "Do you want to Delete this record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,delete it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.create("WrittenTestSchedule/WrittenTestScheduleDeletesStudentData", data).
                    then(function (promise) {
                        swal("Record Deleted Successfully");
                        $scope.BindStudentDataAfterDelete();
                    })
                }
                else {
                    swal(" Cancelled", "Ok");
                }
            });
        }

        //Delete Test Schedule

        $scope.DeleteWrittenTestScheduledata = function (DeleteRecord, SweetAlert) {
            $scope.deleteId = DeleteRecord.pawtS_Id;
            var MdeleteId = $scope.deleteId;
            // swal(id);
            swal({
                title: "Are you sure?",
                text: "Do you want to Delete this record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,Delete it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("WrittenTestSchedule/WrittenTestScheduleDeletesData", MdeleteId).
                    then(function (promise) {
                        //////// $scope.$apply();
                        swal("Record Deleted Successfully");
                        $state.reload();
                    })
                }
                else {
                    swal(" Cancelled", "Ok");
                }
            });
        }



      
        $scope.search12 = '';
       
        $scope.filterOnLocation2 = function (obj) {
            // 
            return ($filter('date')(obj.pawtS_ScheduleDate, 'dd-MM-yyyy').indexOf($scope.search12) >= 0 || $filter('date')(obj.pawtS_ScheduleTime, 'h:mm a').indexOf($scope.search12) >= 0 || $filter('date')(obj.pawtS_ScheduleTimeTo, 'h:mm a').indexOf($scope.search12) >= 0 || (angular.lowercase(obj.pawtS_ScheduleName)).indexOf(angular.lowercase($scope.search12)) >= 0);
            //return ($filter('date')(obj.paotS_ScheduleTime, 'HH:mm').indexOf($scope.searchValue) >= 0) || ($filter('date')(obj.paotS_ScheduleDate, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0 || (obj.paotS_ScheduleName).indexOf($scope.searchValue) >= 0 || (obj.paotS_AM_PM).indexOf($scope.searchValue) >= 0 || (obj.paotS_Remarks).indexOf($scope.searchValue) >= 0);
        }



        $scope.EditWrittenTestScheduledata = function (EditRecord) {
            $scope.IsHidden3 = true;
            $scope.myVar1 = true;
            $scope.EditId = EditRecord.pawtS_Id;
            var MEditId = $scope.EditId;
            $scope.disableedit = true;
            apiService.getURI("WrittenTestSchedule/GetSelectedRowDetails/", MEditId).
            then(function (promise) {
                $scope.timing = moment(promise.writtenTestSchedule[0].pawtS_ScheduleTime, 'h:mm a').format();
                $scope.timing1 = moment(promise.writtenTestSchedule[0].pawtS_ScheduleTimeTo, 'h:mm a').format();
                $scope.ScheduleTime = $scope.timing;
                $scope.ScheduleName = promise.writtenTestSchedule[0].pawtS_ScheduleName;
                // $scope.ScheduleTime =  new Date(promise.writtenTestSchedule[0].pawtS_ScheduleTime);
                $scope.ScheduleTime = $scope.timing;
                $scope.ScheduleTimeTo = $scope.timing1;
                $scope.ScheduleDate = new Date(promise.writtenTestSchedule[0].pawtS_ScheduleDate);
                $scope.ScheduleSession = promise.writtenTestSchedule[0].pawtS_AM_PM;
                if (promise.writtenTestSchedule[0].pawtS_Remarks != null || promise.writtenTestSchedule[0].pawtS_Remarks != undefined) {
                    $scope.ScheduleRemark = promise.writtenTestSchedule[0].pawtS_Remarks;
                }
                else {
                    $scope.ScheduleRemark = "";
                }

                if (promise.writtenTestSchedule[0].pawtS_Superviser != null || promise.writtenTestSchedule[0].pawtS_Superviser != undefined) {
                    $scope.Supervisor = promise.writtenTestSchedule[0].pawtS_Superviser;
                }
                else {
                    $scope.Supervisor = "";
                }

                if (promise.writtenTestSchedule[0].pawtS_Skills != null || promise.writtenTestSchedule[0].pawtS_Skills != undefined) {
                    $scope.Skill = promise.writtenTestSchedule[0].pawtS_Skills;
                }
                else {
                    $scope.Skill = "";
                }

                $scope.SelectedStudent = promise.selectedStudentDetails;
                if ($scope.SelectedStudent.length > 0) {
                    $scope.myVar = true;
                }
                else {
                    $scope.myVar = false;
                }

                $scope.presentCountgrid3 = promise.selectedStudentDetails.length;
                $scope.PAWTS_Id = promise.writtenTestSchedule[0].pawtS_Id;
                //$scope.Supervisor = promise.writtenTestSchedule[0].pawtS_Superviser;
                //$scope.Skill = promise.writtenTestSchedule[0].pawtS_Skills;

                $scope.minDatet = new Date();
                if (new Date($scope.ScheduleDate) < new Date($scope.minDatet)) {
                    $scope.IsHiddendown = true;
                    swal("Scheduled date is lesser than current date so you can't update!!");

                    return false;
                }

            })
        };



        $scope.submitted = false;

        $scope.saveWrittenTestScheduledata = function (stuRecord) {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {

                $scope.ScheduleTime = $filter('date')($scope.ScheduleTime, "h:mm a");
                $scope.ScheduleTimeTo = $filter('date')($scope.ScheduleTimeTo, "h:mm a");


                var array = $.map(stuRecord, function (value, index) {
                    return [value];
                });
                var data = {

                    "PAWTS_ScheduleName": $scope.ScheduleName,
                    "PAWTS_ScheduleTime": $scope.ScheduleTime,
                    "PAWTS_ScheduleTimeTo": $scope.ScheduleTimeTo,
                    "PAWTS_ScheduleDate": new Date($scope.ScheduleDate).toDateString(),
                    "PAWTS_AM_PM": $scope.ScheduleSession,
                    "PAWTS_Remarks": $scope.ScheduleRemark,
                    "SelectedStudentData": $scope.secondTableData,
                    "SelectedStudentDataForEdit": $scope.SelectedStudent,
                    "PAWTS_Superviser": $scope.Supervisor,
                    "PAWTS_Skills": $scope.Skill
                };

                apiService.create("WrittenTestSchedule/", data).
              then(function (promise) {

                  if (promise.returnvalue == "false") {
                      swal("Schedule Name/Date Already Exist ");
                      $state.reload();
                  }
                  else {
                      swal("Record Saved Successfully");

                      $state.reload();
                  }

              })

            }
        };
        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };
        $scope.cancel = function () {
            $state.reload();

        };



        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.order3 = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse3 = !$scope.reverse3; //if true make it false and vice versa
        }

    }

})();