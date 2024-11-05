
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClassTeacherMappingController', ClassTeacherMappingController)

    ClassTeacherMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams']
    function ClassTeacherMappingController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams) {
        $scope.editEmployee = {};

        $scope.sortKey = 'imcT_Id';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));




        // Time picker starts
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



        //load data
        $scope.BindData = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("ClassTeacherMapping/getdetails", pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.accyear = promise.accyear;
                    $scope.section = promise.accsection;
                    $scope.classname = promise.accclass;
                    $scope.staff = promise.empdetails;
                    $scope.newuser1 = promise.getsavedetails;
                    $scope.presentCountgrid = $scope.newuser1.length;
                    // $scope.imcT_Id = 0;
                }
                else {
                    swal("No Records Found");
                }
            });
        };

        $scope.submitted = false;
        //save
        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id,
                    "HRME_Id": $scope.HRME_Id.hrmE_Id,
                    "IMCT_Id": $scope.imcT_Id
                };
                apiService.create("ClassTeacherMapping/save/", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.message !== null) {
                            swal(promise.message);
                        }
                        else {
                            if (promise.retruval === true) {
                                swal("Record Saved Successfully");
                                $state.reload();
                            }
                            else {
                                swal("Record Failed To Save");
                                $state.reload();
                            }
                        }
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        //validation
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //edit data 
        $scope.Editclassteachermappingdata = function (EditRecord) {

            $scope.EditId = EditRecord.imcT_Id;
            var MEditId = $scope.EditId;
            apiService.getURI("ClassTeacherMapping/GetSelectedRowDetails/", MEditId).then(function (promise) {
                $scope.imcT_Id = promise.geteditdetails[0].imcT_Id;
                $scope.asmaY_Id = promise.geteditdetails[0].asmaY_Id;
                $scope.asmcL_Id = promise.geteditdetails[0].asmcL_Id;
                $scope.asmS_Id = promise.geteditdetails[0].asmS_Id;
                $scope.hrmE_Id = promise.geteditdetails[0].hrmE_Id;
            });
        };

        //exchange data
        $scope.staffload = function () {
            var pageid = 2;
            apiService.getURI("ClassTeacherMapping/getdetails", pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.emp1 = promise.empdetails;
                    $scope.emp12 = promise.empdetails;
                }
                else {
                    swal("No Records Found");
                }
            });
        };

        //on change staff1 and getting the class alloted and section alloted
        $scope.onchangestaff1 = function () {
            var data = {
                "HRME_Id": $scope.staffid1.hrmE_Id
            };
            apiService.create("ClassTeacherMapping/onchangestaff1/", data).then(function (promise) {
                if (promise !== null) {
                    $scope.classalotted = promise.onchangestaff[0].classname;
                    $scope.sectionalloted = promise.onchangestaff[0].secname;
                    $scope.emp12 = promise.empdetails;
                    $scope.staffpk1 = promise.onchangestaff[0].staffpk1;
                }
                else {
                    swal("No Records Found");
                }
            });
        };

        //on change staff2 and getting the class alloted and section alloted
        $scope.onchangestaff2 = function () {
            var data = {
                "HRME_Id": $scope.staffid12.hrmE_Id
            };
            apiService.create("ClassTeacherMapping/onchangestaff2/", data).then(function (promise) {
                if (promise !== null) {
                    $scope.classalotted1 = promise.onchangestaff[0].classname;
                    $scope.sectionalloted1 = promise.onchangestaff[0].secname;
                    $scope.staffpk2 = promise.onchangestaff[0].staffpk2;
                }
                else {
                    swal("No Records Found");
                }
            });
        };

        //cancel button
        $scope.removeall = function () {
            $('#myModalreadmit').modal('hide');
            $scope.classalotted1 = "";
            $scope.sectionalloted1 = "";
            $scope.classalotted = "";
            $scope.sectionalloted = "";
            $scope.staffpk2 = 0;
            $scope.staffpk1 = 0;
            $scope.staffid1 = "";
            $scope.staffid12 = "";
        };

        //saving the ex change data
        $scope.addtocart = function () {
            var data = {
                "HRME_Id1": $scope.staffid1.hrmE_Id,
                "HRME_Id2": $scope.staffid12.hrmE_Id,
                "staffpk1": $scope.staffpk1,
                "staffpk2": $scope.staffpk2
            };
            apiService.create("ClassTeacherMapping/exchangesave/", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.retruval === true) {
                        swal("Data Exchanged Successfully");
                        $state.reload();
                    } else {
                        swal("Data Failed To Exchange");
                        $state.reload();
                    }
                }
                $('#myModalreadmit').modal('hide');
                $scope.classalotted1 = "";
                $scope.sectionalloted1 = "";
                $scope.classalotted = "";
                $scope.sectionalloted = "";
                $scope.staffpk2 = 0;
                $scope.staffpk1 = 0;
                $scope.staffid1 = "";
                $scope.staffid12 = "";
            });
        };

        //delete record
        $scope.Deleteclassteachermappingdata = function (EditRecord) {
            $scope.EditId = EditRecord.imcT_Id;
            var MEditId = $scope.EditId;
            apiService.create("ClassTeacherMapping/deleterecord/", MEditId).then(function (promise) {
                if (promise !== null) {
                    if (promise.message !== null) {
                        swal(promise.message);
                    }
                    else {
                        if (promise.retruval === true) {
                            swal("Record DeActivated Successfully");
                            $state.reload();
                        }
                        else {
                            //dd
                        }
                    }
                }
            });
        };

        $scope.switch = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.imct_activeflag === true) {
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

                        apiService.create("ClassTeacherMapping/deleterecord/", employee).then(function (promise) {
                            if (promise.message !== null) {
                                swal(promise.message);
                            }
                            else {
                                if (promise.returnval === true) {
                                    swal(confirmmgs + " " + "Successfully.");
                                    $state.reload();
                                }
                                else {
                                    swal(confirmmgs + " " + " Successfully");
                                    $state.reload();
                                }
                            }
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                    $state.reload();

                });
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.cance = function () {
            $state.reload();

        };
        $scope.searchValue = "";
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.asmaY_Year)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.asmcL_ClassName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.asmS_SectionName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.hrmE_EmployeeFirstName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.employeecode)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        };

    }

})();