
(function () {
    'use strict';

    angular
        .module('app')
        .controller('IVRM_PushNotificationController', IVRM_PushNotificationController);

    IVRM_PushNotificationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', '$sce']

    function IVRM_PushNotificationController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce) {

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));



        //------------for sorting.........
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.itemsPerPage = 10;
        $scope.search = "";
        $scope.currentPage = 1;
        //============================
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse2 = !$scope.reverse2;
        }
        $scope.itemsPerPage2 = 10;
        $scope.search2 = "";
        $scope.currentPage2 = 1;

        //============================
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse3 = !$scope.reverse3;
        }
        $scope.itemsPerPage3 = 10;
        $scope.search3 = "";
        $scope.currentPage3 = 1;
        //==========================


        $scope.submitted = false;

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        //-----------load data
        var id = 1;
        $scope.loaddata = function () {
            
            apiService.getURI("IVRM_PushNotification/Getdetails", id).
                then(function (promise) {
                    
                    $scope.flag_Type = promise.flag_Type;
                    $scope.studentlist = promise.studentlist;
                    $scope.stafflist = promise.stafflist;

                    
                    if ($scope.flag_Type == "Student") {
                        $scope.staf_flag = true;
                        if (promise.empdata.length > 0) {
                            $scope.empinfo = promise.empdata;
                        }
                    }
                    else if ($scope.flag_Type == "Staff") {
                        $scope.student_flag = true;
                        if (promise.studentdata.length > 0) {
                            $scope.studentinfo = promise.studentdata;
                        }
                    }
                })
        }


        //=======selection of checkbox....
        $scope.togchkbxC = function () {
            
            $scope.usercheckC = $scope.studentlist.every(function (role) {
                return role.selected;
            });
        }

        $scope.isOptionsRequired = function () {
            if ($scope.flag_Type == "Student") {
                return !$scope.studentlist.some(function (item) {
                    return item.selected;
                });
            }
        }

        //all select
        $scope.all_checkC1 = function (data) {
            
            $scope.usercheckC = data;
            var toggleStatus = $scope.usercheckC;
            angular.forEach($scope.studentlist, function (role) {
                role.selected = toggleStatus;
            });
        }

        //---------save data..
        $scope.studntlst = [];
        $scope.submitted = false;
        $scope.savedata = function () {
            
            if ($scope.myForm.$valid) {
                if ($scope.flag_Type == "Staff") {
                    $scope.studntlst = [];
                    angular.forEach($scope.studentlist, function (cls) {
                        if (cls.selected == true) {
                            $scope.studntlst.push(cls);
                        }
                    });


                }
                if (($scope.flag_Type == "Staff" && $scope.studntlst.length > 0) || $scope.flag_Type == "Student") {
                    var pushdate = $scope.ipN_Date == null ? "" : $filter('date')($scope.ipN_Date, "yyyy-MM-dd");

                    if ($scope.flag_Type == "Student") {

                        var data = {
                            "IPN_Id": $scope.ipN_Id,
                            "IPN_No": $scope.ipN_No,
                            "IPN_Date": pushdate,
                            "IPN_PushNotification": $scope.ipN_PushNotification,
                            "IPN_StuStaffFlg": $scope.flag_Type,
                            "HRME_Id": $scope.hrmE_Id
                        }
                    }
                    else if ($scope.flag_Type == "Staff") {
                        var data = {
                            "IPN_Id": $scope.ipN_Id,
                            "IPN_No": $scope.ipN_No,
                            "IPN_Date": pushdate,
                            "IPN_PushNotification": $scope.ipN_PushNotification,
                            "IPN_StuStaffFlg": $scope.flag_Type,
                            "stud_ids": $scope.studntlst
                        }
                    }

                    apiService.create("IVRM_PushNotification/savedetail", data).
                        then(function (promise) {
                            if (promise.returnval != null && promise.duplicate != null) {
                                if (promise.duplicate != true) {
                                    if (promise.returnval != false) {
                                        if ($scope.ipN_Id > 0) {
                                            swal("Record Updated Successfully!!!");
                                        }
                                        else {
                                            swal("Record Saved Successfully!!!");
                                        }
                                    }
                                    else {
                                        if ($scope.ipN_Id > 0) {
                                            swal("Record Not Updated Successfully!!!");
                                        }
                                        else {
                                            swal("Record Not Saved Successfully!!!");
                                        }
                                    }
                                }
                                else {
                                    swal("Record already exist");
                                }
                                $state.reload();
                            }
                            else {
                                swal("Kindly Contact Administrator!!!");
                            }
                        });
                }
                else {
                    swal('Please Select Any One Student Name!!');
                }

            }
            else {
                $scope.submitted = true;
            }

        };


        //--------------For edit data for staff
        $scope.editnoticestf = function (id) {

            
            var data = {
                "IPN_Id": id.ipN_Id,
            }

            apiService.create("IVRM_PushNotification/editnoticestf", data).then(function (promise) {
                
                if (promise.editstaflist.length > 0) {
                    $scope.editstaflist = promise.editstaflist;

                    $scope.ipN_Id = promise.editstaflist[0].ipN_Id;
                    $scope.ipN_Date = new Date(promise.editstaflist[0].ipN_Date);
                    $scope.ipN_No = promise.editstaflist[0].ipN_No;
                    $scope.ipN_PushNotification = promise.editstaflist[0].ipN_PushNotification;
                    $scope.hrmE_Id = promise.editstaflist[0].hrmE_Id;

                }
            });
        };


        //--------------For edit data for student
        $scope.editnoticestud = function (id) {

            
            var data = {
                "IPN_Id": id.ipN_Id,
            }

            apiService.create("IVRM_PushNotification/editnoticestud", data).then(function (promise) {
                
                $scope.editstudlist = [];
                $scope.editstudlist = promise.editstudlist;
                angular.forEach($scope.studentlist, function (std) {
                    std.selected = false;
                    angular.forEach($scope.editstudlist, function (edstd) {
                        
                        if (std.amsT_Id == edstd.amsT_Id) {
                            
                            std.selected = true;
                        }
                    })
                })
                $scope.togchkbxC = function () {
                    $scope.usercheckC = $scope.studentlist.every(function (role) {
                        return role.selected;
                    });
                }

                if (promise.editstudlist.length > 0) {
                    $scope.ipN_Id = promise.editstudlist[0].ipN_Id;
                    $scope.ipN_Date = new Date(promise.editstudlist[0].ipN_Date);
                    $scope.ipN_No = promise.editstudlist[0].ipN_No;
                    $scope.ipN_PushNotification = promise.editstudlist[0].ipN_PushNotification;
                    $scope.amsT_Id = promise.editstaflist[0].amsT_Id;
                }



            });
        };

        $scope.clear_Id = function () {
            $state.reload();
        };



        //============activate/decativate data main table with staff
        $scope.Deactivate = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ipN_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        apiService.create("IVRM_PushNotification/deactivate", employee).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal(confirmmgs + " " + "successfully.");
                                }
                                else {
                                    swal(confirmmgs + " " + " successfully");
                                }
                                $state.reload();
                            })

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }



        //============activate/decativate data main table 
        $scope.Deactivatemain = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ipN_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        apiService.create("IVRM_PushNotification/Deactivatemain", employee).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal(confirmmgs + " " + "successfully.");
                                }
                                else {
                                    swal(confirmmgs + " " + " successfully");
                                }
                                $state.reload();
                            })

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }


        //============Model click
        $scope.onmodelclick = function (id) {
            var data = {
                "IPN_Id": id,
            }

            apiService.create("IVRM_PushNotification/get_modeldata", data).then(function (promise) {
                $scope.modalstudlist = promise.modalstudlist;
            });
        };


        //============activate/decativate data main table 
        $scope.Deactivatestud = function (employee, SweetAlert) {
            
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ipnS_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },

                function (isConfirm) {
                    if (isConfirm) {

                        apiService.create("IVRM_PushNotification/Deactivatestud", employee).
                            then(function (promise) {
                                
                                if (promise.returnval == true) {
                                    swal(confirmmgs + " " + "successfully.");
                                }
                                else {
                                    swal(confirmmgs + " " + " successfully");
                                }
                                $state.reload();

                            })

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }
    }
})();
