
(function () {
    'use strict';
    angular
.module('app') 
.controller('AttendanceEntryTypeController', AttendanceEntryTypeController)

    AttendanceEntryTypeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function AttendanceEntryTypeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        $scope.currentPage = 1;
        
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.className = {};
        $scope.donce = false;

        $scope.sortKey = 'asaeT_Id';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            }
        }
      

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null) {
            if (admfigsettings.length > 0) {
                var logopath = admfigsettings[0].asC_Logo_Path;
            }
        }
       

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        $scope.itemsPerPage = paginationformasters;


        $scope.BindData = function () {
            apiService.getDATA("AttendanceEntryType/Getdetails").
       then(function (promise) {
           
           $scope.arrlist2 = promise.loadallyear;
           $scope.allAcademicYear = promise.yeardropDown;

           for (var i = 0; i < $scope.arrlist2.length; i++) {
               name = $scope.arrlist2[i].asmaY_Id;
               for (var j = 0; j < $scope.allAcademicYear.length; j++) {
                   if (name == $scope.allAcademicYear[j].asmaY_Id) {
                       $scope.arrlist2[i].Selected = true;
                       $scope.ASMAY_Id = $scope.allAcademicYear[j].asmaY_Id;
                   }
               }
           }
          // $scope.arrlist2 = promise.yeardropDown;
           $scope.arrlist4 = promise.classdropDown;
           if (promise.count > 0) {
               $scope.attendanceTypeList = promise.attendanceEntryTypeList;
               $scope.presentCountgrid = $scope.attendanceTypeList.length;
           }
           else {
               swal("No Records Found");
           }
       })
            //$scope.sort = function (keyname) {
            //    $scope.sortKey = keyname;   //set the sortKey to the param passed
            //    $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            //}
        };




        $scope.Deletedata = function (DeleteRecord, SweetAlert) {
            $scope.deleteId = DeleteRecord.asaeT_Id;
            var MdeleteId = $scope.deleteId;
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
                    apiService.DeleteURI("AttendanceEntryType/AttendanceDeleteEntryTypeDTO", MdeleteId).
                    then(function (promise) {
                        if (promise.returnval == true) {
                            swal("Record Deleted Sucessfully");
                            $state.reload();
                        }
                        else {
                            swal("Failed to  Delete record");
                        }

                    })
                }
                else {
                    swal("Cancelled");
                }
            });
        }

        //$scope.Deletedata = function (DeleteRecord, SweetAlert) {
        //    $scope.deleteId = DeleteRecord.asaeT_Id;
        //    var MdeleteId = $scope.deleteId;
        //    swal({
        //        title: "Are you sure?",
        //        text: "Do you want to delete this item?",
        //        type: "warning",
        //        showCancelButton: true,
        //        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,delete it!",
        //        cancelButtonText: "Cancel..!",
        //        closeOnConfirm: false,
        //        closeOnCancel: false
        //    },
        //    function (isConfirm) {
        //        if (isConfirm) {
        //            apiService.DeleteURI("AttendanceEntryType/AttendanceDeleteEntryTypeDTO", MdeleteId).
        //            then(function (promise) {
        //                swal("Record Deleted Sucessfully");
        //                $state.reload();
        //            })
        //        }
        //        else {
        //                swal("Cancelled");
        //        }
        //    });
        //}

        $scope.isOptionsRequired = function () {
            return !$scope.arrlist4.some(function (options) {
                return options.Selected;
            });
        }


        $scope.checkboxchcked = [];
        $scope.editClassList=[];

        $scope.CheckedClassName = function (data) {

            if ($scope.checkboxchcked.indexOf(data) === -1) {
                $scope.checkboxchcked.push(data);
            }
            else {
                $scope.checkboxchcked.splice($scope.checkboxchcked.indexOf(data), 1);
            }
        }
        $scope.interacted = function (field) {

            return $scope.submitted;
        };
        $scope.Editdata = function (EditRecord) {
            
            $scope.edit = true;
            $scope.EditId = EditRecord.asaeT_Id;
            var MEditId = $scope.EditId;
            apiService.getURI("AttendanceEntryType/GetSelectedRowDetails/", MEditId).
            then(function (promise) {
                $scope.ASMAY_Id = promise.attendanceEntryTypeList[0].asmaY_Id;
                $scope.AttendanceType = promise.attendanceEntryTypeList[0].asaeT_Att_Type;
                $scope.asmcL_Id = promise.attendanceEntryTypeList[0].asmcL_Id;

                if ($scope.ASMAY_Id == promise.yearid) {
                    $scope.donce = false;
                    $scope.btnsaveedit = false;
                }
                else {
                    $scope.donce = true;
                    $scope.btnsaveedit = true;
                }
                for (var i = 0; i < $scope.arrlist4.length; i++)
                {
                    if ($scope.arrlist4[i].asmcL_Id == promise.attendanceEntryTypeList[0].asmcL_Id)
                    {
                        $scope.arrlist4[i].Selected = true; 
                        $scope.editClassList.push( $scope.arrlist4[i]);
                        
                    }
                    else {
                        $scope.arrlist4[i].Selected = false;
                    }
                }
                $("input:checkbox").on('click', function () {
                    var $box = $(this);
                    if ($box.is(":checked")) {                     
                        var group = "input:checkbox[name='" + $box.attr("name") + "']";                       
                        $(group).prop("checked", false);
                        $box.prop("checked", true);
                    } else {
                        $box.prop("checked", false);
                    }
                });
             //   $scope.asmcL_Id = promise.classdropDown[0].amcL_Id;
            })
        };

      
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        
        $scope.submitted = false;
        $scope.saveAttendanceEntryTypedata = function () {
            
            if ($scope.myForm.$valid) {
                var ClassIDs = [];

                if ($scope.checkboxchcked.length > 0) {
                    ClassIDs = $scope.checkboxchcked;
                }
                else if ($scope.edit == true && $scope.editClassList.length > 0) {
                    ClassIDs = $scope.editClassList;
                }
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASAET_Att_Type": $scope.AttendanceType,
                    "SelectedClassDetails": ClassIDs,
                    "ASAET_Id": $scope.EditId
                };
                apiService.create("AttendanceEntryType/", data).
                    then(function (promise) {
                        if (promise.message != "" && promise.message != null) {
                            swal(promise.message);
                            $state.reload();
                            return;
                        }
                        else{
                            if (promise.returnval == true && promise.typechangeflag == "Update") {
                            swal("Record Updated Successfully");
                            $state.reload();
                            }
                            else if (promise.returnval == true && promise.typechangeflag == "Add")
                            {
                                swal("Record Saved Successfully");
                                $state.reload();
                            }
                        else if (promise.typechangeflag == 'NotAbleToChangeTheAttEntryType') {
                            swal("Please Note: you cannot change the Attendance entry type for the current month", "Changing of the Attendance entry type can be done at the start of the month or before entring attendace for that particular month");
                        }
                        else {
                            swal("Failed to Save/Update");
                        }
                    }
                    })
            } else {
                $scope.submitted = true;
            }


        };
       
        $scope.cancel = function () {
            $state.reload();
            $scope.ASMAY_Id = "";
            $scope.Selected = "";
            $scope.AttendanceType = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            
            return (angular.lowercase(obj.instituteName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.className)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.acedemicYear)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.asaeT_Att_Type)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }
    }

})();