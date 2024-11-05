(function () {
    'use strict';
    angular
        .module('app')
        .controller('MastersModuleController', MastersModuleController)

    MastersModuleController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MastersModuleController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


        //================================Tab-1
        $scope.submitted2 = false;
        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.searchValueU = '';
        $scope.currentPageU = 1;
        $scope.itemsPerPageU = paginationformasters;

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        //================================

        //========================Tab-2
        $scope.submitted = false;
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverseV = !$scope.reverseV; //if true make it false and vice versa
        }
        //$scope.currentPageMap = 1;
        //$scope.itemsPerPageMap = paginationformasters;


        //================================

        //=====================Load--data.............
        $scope.Loaddata = function () {

            var pageid = 2;
            apiService.getURI("MastersModule/getdetails", pageid).then(function (promise) {
                $scope.alldata = promise.alldata;
                $scope.deptlist = promise.deptlist;
                $scope.projectlist = promise.projectlist;
                $scope.modulelist = promise.modulelist;
                $scope.emplistHead = promise.emplistHead;
                $scope.emplist = promise.emplist;

                $scope.newuser = promise.masterModulesname;
            })

        }
        //=====================End-----Load--data----//

        $scope.tab1 = false;
        $scope.next = function () {

            //$scope.tab2 = false;
            $scope.myTabIndex = $scope.myTabIndex + 1;

        }
        $scope.previous = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
            //$scope.tab2 = true;
            $scope.tab1 = false;
        }

        //=====================save--record....
        $scope.saverecord = function () {
            $scope.developerlist = [];

            if ($scope.myForm.$valid) {
                $scope.developerheadlist = [];
                $scope.developerlist = [];
                angular.forEach($scope.emplist, function (developer) {
                    var hed = developer.hrmE_Id.toString();
                    if (developer.select === true && developer.hrmE_Id !== $scope.headId) {
                        $scope.developerlist.push(developer);
                    }
                });
                angular.forEach($scope.emplistHead, function (developer) {
                    var hed = developer.headId.toString();
                    if ($scope.headId === hed || $scope.headId === developer.headId) {
                        $scope.developerheadlist.push(developer);
                    }
                });

                var data = {
                    "ISMMMD_Id": $scope.ismmmD_Id,
                    "HRMD_Id": $scope.HRMD_Id,
                    "ISMMPR_Id": $scope.ISMMPR_Id,
                    "IVRMM_Id": $scope.IVRMM_Id,
                    "ISMMMD_ModuleHeadId": $scope.headId,
                    developerlist: $scope.developerlist,
                    developerheadlist: $scope.developerheadlist
                }
                apiService.create("MastersModule/saverecord", data).then(function (promise) {
                    if (promise.returnval != null && promise.duplicate != null) {
                        if (promise.duplicate == false) {
                            if (promise.returnval == true) {
                                if ($scope.ismmmD_Id > 0) {
                                    swal('Record Updated Successfully!!!');
                                    $scope.alldata = promise.alldata;
                                    $scope.Clearid();
                                }
                                else {
                                    swal('Record Saved Successfully!!!');
                                    $scope.alldata = promise.alldata;
                                    $scope.Clearid();
                                }
                            }
                            else {
                                if (promise.returnval == false) {
                                    if ($scope.ismmmD_Id > 0) {
                                        swal('Record Not Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Not Saved Successfully!!!');
                                    }
                                }
                            }
                        }
                        else {
                            swal("Record already exist");
                        }

                    }
                    else {
                        swal("Kindly Contact Administrator!!!");
                    }

                })
            }
            else {
                $scope.submitted = true;
            }
        };
        //=====================End---save--record....


        //=====================Editrecord....
        $scope.EditData = function (user) {

            var data = {
                "ISMMMD_Id": user.ismmmD_Id
            }
            apiService.create("MastersModule/editlist", user).then(function (promise) {
                //$scope.emp_flag = true;
                if (promise.editlist.length > 0) {
                    $scope.ismmmD_Id = promise.editlist[0].ismmmD_Id;
                    $scope.HRMD_Id = promise.editlist[0].hrmD_Id;
                    $scope.ISMMPR_Id = promise.editlist[0].ismmpR_Id;
                    $scope.IVRMM_Id = promise.editlist[0].ivrmM_Id;

                    $scope.emplist = promise.emplist;
                    $scope.emplistHead = promise.emplistHead;
                    $scope.headId = promise.editlist[0].ismmmD_ModuleHeadId;
                    angular.forEach($scope.emplist, function (emp) {
                        angular.forEach(promise.editlist, function (edit) {
                            if (emp.hrmE_Id == edit.ivrmmmddE_ModuleIncharge) {
                                emp.select = true;
                            }
                        })
                    })
                }


            });





        }
        //====================End---edit-record....


        //=================Activation/Deactivation--Record.........
        $scope.deactiveY = function (user, SweetAlert) {

            $scope.ismmmD_Id = user.ismmmD_Id;

            var dystring = "";
            if (user.ismmmD_ActiveFlag == 1) {
                dystring = "Deactivate";
            }
            else if (user.ismmmD_ActiveFlag == 0) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("MastersModule/deactiveY", user).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");

                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");

                                }
                                $state.reload();
                            })

                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }

                });
        }



        $scope.deactiveDevpMappingdata = function (user, SweetAlert) {

            $scope.ismmmddE_Id = user.ismmmddE_Id;

            var dystring = "";
            if (user.ismmmddE_ActiveFlag == 1) {
                dystring = "Deactivate";
            }
            else if (user.ismmmddE_ActiveFlag == 0) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("MastersModule/deactiveDevpMappingdata", user).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                    $scope.developerlistd = promise.developerlistd;
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");

                                }
                                // $state.reload();
                            })

                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }

                });
        }
        //================End----Activation/Deactivation--Record.........




        //===========----Clear Field
        $scope.Clearid = function () {
            $scope.ISMMPR_Id = "";
            $scope.HRMD_Id = "";
            $scope.IVRMM_Id = "";
            $scope.headId = "";
            $scope.searchchkbx = "";
            angular.forEach($scope.emplist, function (tt) {
                return tt.select = false;
            });
            $scope.usercheck = false;
            $scope.ismmmD_Id = 0;
            $scope.submitted = false;
        };

        $scope.searchchkbx = "";
        $scope.all_check = function () {
            var checkStatus = $scope.usercheck;
            angular.forEach($scope.emplist, function (itm) {
                itm.select = checkStatus;
            });
        }

        $scope.togchkbx = function () {
            $scope.usercheck = $scope.emplist.every(function (options) {
                return options.select;
            });
        }

        $scope.isOptionsRequired = function () {
            return !$scope.emplist.some(function (options) {
                return options.select;
            });
        }

        $scope.get_emplist = function () {

            var data = {
                "HRMD_Id": $scope.HRMD_Id
            }
            apiService.create("MastersModule/get_emplist", data).then(function (promise) {
                $scope.emplist = promise.emplist;
                $scope.emplistHead = promise.emplistHead;
            });

        }

        $scope.get_MappedDeveloperlist = function (data) {

            apiService.create("MastersModule/get_MappedDeveloperlist", data).then(function (promise) {

                $scope.developerlistd = promise.developerlistd;
            })
        }

        //=====================================================================Module




        $scope.saveMasterModulesdata = function () {

            $scope.submitted2 = true;

            if ($scope.frmmodule.$valid) {

                var activeflag = $scope.module_ActiveFlag

                activeflag = 1;

                var data = {
                    "IVRMM_ModuleName": $scope.newuser.name,
                    "IVRMM_ModuleDesc": $scope.newuser.description,
                    "Module_ActiveFlag": activeflag,
                    "IVRMM_Id": $scope.newuser.IVRMM_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("MasterModules/", data).
                    then(function (promise) {

                        if (promise.returnval == "Save" && promise.returnduplicatestatus != "Duplicate") {
                            swal('Record Saved Successfully', 'success');
                            $state.reload();
                        }
                        else if (promise.returnval == "NotSave" && promise.returnduplicatestatus != "Duplicate") {
                            swal('Record Not Saved');
                            $state.reload();
                        }
                        else if (promise.returnval == "Update" && promise.returnduplicatestatus != "Duplicate") {
                            swal('Record Updated Successfully');
                            $state.reload();
                        }
                        else if (promise.returnval == "NotUpdate" && promise.returnduplicatestatus != "Duplicate") {
                            swal('Record Not Updated');
                            $state.reload();
                        }
                        else if (promise.returnduplicatestatus == "Duplicate") {
                            swal('Module Name Already Exist');
                        }


                    })
            };
        }


        $scope.EditMasterModulesdata = function (EditRecord) {
            $scope.EditId = EditRecord.ivrmM_Id;
            var MEditId = $scope.EditId;
            apiService.getURI("MasterModules/GetSelectedRowDetails/", MEditId).
                then(function (promise) {

                    $scope.newuser.IVRMM_Id = promise.masterModulesname[0].ivrmM_Id;
                    $scope.newuser.name = promise.masterModulesname[0].ivrmM_ModuleName;
                    $scope.newuser.description = promise.masterModulesname[0].ivrmM_ModuleDesc;


                })
        };

        $scope.DeleteMasterModulesdata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.ivrmM_Id;
            var MdeleteId = $scope.editEmployee;

            var mgs = "";
            var confirmmgs = "";
            if (employee.module_ActiveFlag == 1) {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";

            }

            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.DeleteURI("MasterModules/MasterDeleteModulesDTO", MdeleteId).
                            then(function (promise) {

                                if (promise.returnval == "true") {
                                    swal('Record Successfully ' + confirmmgs);
                                }
                                else {
                                    swal('Record not successfully ' + confirmmgs);
                                }

                                $state.reload();

                            })
                    }
                    else {
                        swal("Record  " + mgs + " Cancelled");
                    }
                });
        }


    }
})();

