(function () {
    'use strict';
    angular
        .module('app')
        .controller('Hostel_GatePass_Admin_ApplyController', Hostel_GatePass_Admin_ApplyController)
    Hostel_GatePass_Admin_ApplyController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function Hostel_GatePass_Admin_ApplyController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {

        $scope.sortKey = 'HLMF_Id';
        $scope.sortReverse = true;
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));



        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

        //get data
        $scope.getAllDetail = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("Hostel_Student_Gatepass_Process/getGatePassAdminApplyOnload", pageid).then(function (promise) {
                $scope.getstudent = promise.getstudent;
                $scope.gridOptions = promise.admingatepassapplylist;

            })
        };

        $scope.studentdetails = function (item) {
            $scope.Studentname = item.Studentname;
            $scope.mobileNo = item.mobileNo;
            $scope.emailId = item.emailId;
            $scope.Homeadress = item.Homeadress;
            $scope.HLMH_Name = item.HLMH_Name;
            $scope.HRMRM_RoomNo = item.HRMRM_RoomNo;
            $scope.HLMRCA_RoomCategory = item.HLMRCA_RoomCategory;
            $scope.Requesteddate = item.Requesteddate;
        }

        $scope.GetstudentDetails = function (item) {
            $scope.Student_Name = item.Student_Name;
            $scope.HLHSTGP_TypeFlg = item.HLHSTGP_TypeFlg;
            $scope.HLHSTGP_GoingOutDate = item.HLHSTGP_GoingOutDate;
            $scope.HLHSTGP_GoingOutTime = item.HLHSTGP_GoingOutTime;
            $scope.HLHSTGP_Reason = item.HLHSTGP_Reason;        
        }


        //save
        $scope.submitted = false;
        $scope.savetmpldata = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "HLHSTGP_Id": $scope.hlhstgP_Id,
                    "HLHSTGP_TypeFlg": $scope.hlhstgP_TypeFlg,
                    "HLHSTGP_GoingOutDate": $scope.hlhstgP_GoingOutDate,
                    "HLHSTGP_ComingBackDate": $scope.hlhstgP_ComingBackDate,
                    "HLHSTGP_GoingOutTime": $filter('date')($scope.hlhstgP_GoingOutTime, "HH:mm"),
                    "HLHSTGP_Reason": $scope.hlhstgP_Reason,
                    "AMCST_Id": $scope.StudentId.StudentId.StudentId
                }
                apiService.create("Hostel_Student_Gatepass_Process/SaveUpdate", data).
                    then(function (promise) {
                        if (promise.returnvalue !== "") {

                            if (promise.returnvalue === "Update") {
                                swal('Record Updated Successfully');
                                $state.reload();
                                return;
                            }
                            else if (promise.returnvalue === "Add") {
                                swal('Record Saved Successfully');
                                $state.reload();
                                return;
                            }
                            else if (promise.returnvalue === "false") {
                                swal('Record Not Saved/Updated successfully');
                                return;
                            }
                            else if (promise.returnvalue === "Duplicate") {
                                swal('Category Already Exist');
                                $state.reload();
                                return;
                            }
                        }

                        else {
                            swal('Operation Failed');
                            $state.reload();
                        }
                    })

            }
            else {

                $scope.submitted = true;
            }

        };

        $scope.UpdateStatus = function () {

  
                var data = {
                    "HLHSTGP_Id": $scope.HLHSTGP_Id,
                    "HLHSTGP_CameBackDate": $scope.HLHSTGP_CameBackDate,
                    "HLHSTGP_CameBackTime": $filter('date')($scope.HLHSTGP_CameBackTime, "HH:mm"),
                }
                apiService.create("Hostel_Student_Gatepass_Process/UpdateStatus", data).
                    then(function (promise) {
                        if (promise.returnvalue !== "") {

                            if (promise.returnvalue === "Update") {
                                swal('Record Updated Successfully');
                                $state.reload();
                                return;
                            }
                            else if (promise.returnvalue === "Add") {
                                swal('Record Saved Successfully');
                                $state.reload();
                                return;
                            }
                            else if (promise.returnvalue === "false") {
                                swal('Record Not Saved/Updated successfully');
                                return;
                            }
                            else if (promise.returnvalue === "Duplicate") {
                                swal('Category Already Exist');
                                $state.reload();
                                return;
                            }
                        }

                        else {
                            swal('Operation Failed');
                            $state.reload();
                        }
                    })

         

        };

        //edit

        $scope.Edit = function (item) {
            var data = {
                "HLHSTGP_Id": item.HLHSTGP_Id,
            };

            apiService.create("Hostel_Student_Gatepass_Process/Edit", data).then(function (promise) {
                if (promise !== null) {
                    $scope.editdata = promise.editdata;
                    $scope.editflag = true;

                    $scope.hlhstgP_TypeFlg = promise.editdata[0].hlhstgP_TypeFlg;
                    $scope.hlhstgP_Reason = promise.editdata[0].hlhstgP_Reason;
                    $scope.hlhstgP_GoingOutDate = new Date(promise.editdata[0].hlhstgP_GoingOutDate);
                    $scope.hlhstgP_ComingBackDate = new Date(promise.editdata[0].hlhstgP_ComingBackDate);
                    $scope.hlhstgP_GoingOutTime = moment(promise.editdata[0].hlhstgP_GoingOutTime, 'H:mm  ').format();
                    


                  
                    angular.forEach($scope.getstudent, function (item) {
                        if (item.StudentId == promise.editdata[0].amcsT_Id) {                            
                            $scope.Studentname = item.Studentname;
                            $scope.mobileNo = item.mobileNo;
                            $scope.emailId = item.emailId;
                            $scope.Homeadress = item.Homeadress;
                            $scope.HLMH_Name = item.HLMH_Name;
                            $scope.HRMRM_RoomNo = item.HRMRM_RoomNo;
                            $scope.HLMRCA_RoomCategory = item.HLMRCA_RoomCategory;
                            $scope.Requesteddate = item.Requesteddate;
                            $scope.StudentId.StudentId.StudentId = item.StudentId;
                        }
                    })



                }
            })
        }

        $scope.obj = {};

        //deactivate record
        $scope.DeletRecord = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.HLHSTGP_ActiveFlg == false) {
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
                        apiService.create("Hostel_Student_Gatepass_Process/ActiveDeactiveRecord", data).then(function (promise) {
                            if (promise.retrunMsg !== "") {
                                if (promise.retrunMsg === "Activated") {
                                    swal("Record Activated successfully");
                                    $state.reload();
                                }
                                else if (promise.retrunMsg === "Deactivated") {
                                    swal("Record Deactivated successfully");
                                    $state.reload();
                                }
                                else {
                                    swal("Record Not Activated/Deactivated", 'Fail');
                                }
                            }

                        })
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }
            );


            //===================get  Mapped  facility
            $scope.get_Mappedfacility = function (user) {
                apiService.create("Training_Master/get_Mappedfacility", user).then(function (promise) {
                    $scope.get_MappedfacilityforRoom = promise.get_MappedfacilityforRoom;
                    $scope.temparry = promise.get_MappedfacilityforRoom;
                    $scope.mainarray = [];
                    forEach($scope.get_MappedfacilityforRoom, function (tt) {
                        var subarry = [];
                        forEach($scope.temparry, function (ss) {
                            if (tt.hrmrM_Id == ss.hrmrM_Id) {
                                subarry.push(ss);
                            }
                        })
                    })
                });
            }

            $scope.edit = function (record) {
                var data = {
                    "HLMB_Id": record.hlmB_Id,
                    "HLMH_Id": record.hlmH_Id,
                }

                apiService.create("Training_Master/details_F", data).
                    then(function (promise) {
                        $scope.hlmH_Id = promise.floor_lists[0].hlmH_Id;
                        $scope.bedName = promise.floor_lists[0].hlmB_BedName;
                        $scope.hlmF_Id = promise.floor_lists[0].hlmF_Id;
                        $scope.hrmrM_Id = promise.floor_lists[0].hrmrM_Id;
                        $scope.hlmB_MattressFlg = promise.floor_lists[0].hlmB_MattressFlg;
                        $scope.hlmB_BedSheetFlg = promise.floor_lists[0].hlmB_BedSheetFlg;
                        $scope.hlmB_PillowFlg = promise.floor_lists[0].hlmB_PillowFlg;
                        $scope.hlmB_StudyTableFlg = promise.floor_lists[0].hlmB_StudyTableFlg;
                        $scope.hlmB_LampFlg = promise.floor_lists[0].hlmB_LampFlg;
                        $scope.getAllDetail();
                        //getAllDetail
                    })
            };




            $scope.cancel = function () {
                $state.reload();
            }

            //deactive
            //$scope.deactive = function (flr, SweetAlertt) {

            //    var config = {
            //        headers: {
            //            'Content-Type': 'application/json;'
            //        }
            //    };
            //    var mgs = "";
            //    if (flr.hrmF_ActiveFlag === false) {
            //        mgs = "Activate";
            //    } else {
            //        mgs = "Deactivate";
            //    }
            //    swal({
            //        title: "Are you sure?",
            //        text: "Do you want to  " + mgs + " the Academic Year?",
            //        type: "warning",
            //        showCancelButton: true,
            //        confirmButtonColor: "#DD6B55",
            //        confirmButtonText: "Yes, " + mgs + " it!",
            //        cancelButtonText: "Cancel..!",
            //        closeOnConfirm: false,
            //        closeOnCancel: false
            //    },
            //        function (isConfirm) {
            //            if (isConfirm) {
            //                apiService.create("Training_Master/deactivate_F", flr).
            //                    then(function (promise) {

            //                        if (promise.hrmF_ActiveFlag === true) {
            //                            if (promise.returnval === false) {
            //                                swal('Master Floor Deactivated Successfully');
            //                            }
            //                        }
            //                        else if (promise.hrmF_ActiveFlag === false) {
            //                            swal('Master Floor Activated Successfully');
            //                        }
            //                        //   }

            //                        $state.reload();
            //                    });
            //            } else {
            //                swal("Cancelled");
            //                $state.reload();
            //            }

            //        });
            //};


            $scope.deactive_Roomdata = function (data, SweetAlert) {
                var mgs = "";
                var confirmmgs = "";
                if (data.hrmrM_ActiveFlag == false) {
                    mgs = "Activate";
                    confirmmgs = "Activated";
                }
                else {
                    mgs = "Deactivate";
                    confirmmgs = "Deactivated";
                }

                swal({
                    title: "Are you sure?",
                    text: "Do you want to " + mgs + " record..!?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                    cancelButtonText: "Cancel..!",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            apiService.create("Training_Master/deactive_Roomdata", data.hrmrM_Id).
                                then(function (promise) {


                                    if (promise.retrunMsg !== "") {

                                        if (promise.retrunMsg === "Activated") {
                                            swal("Record Activated successfully");
                                        }
                                        else if (promise.retrunMsg === "Deactivated") {
                                            swal("Record Deactivated successfully");
                                        }
                                        else {
                                            swal("Record Not Activated/Deactivated", 'Fail');
                                        }

                                        $scope.getAllDetail();
                                    }

                                })
                        }
                        else {
                            swal(" Cancelled", "Ok");
                        }
                    }

                );
            }




            $scope.interacted = function (field) {
                return $scope.submitted || field.$dirty;
            };

        }
    }

})();






































