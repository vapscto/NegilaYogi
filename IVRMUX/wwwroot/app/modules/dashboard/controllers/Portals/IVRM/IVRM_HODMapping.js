(function () {
    'use strict';
    angular
        .module('app')
        .controller('IVRM_HODMappingController', IVRM_HODMappingController);
    IVRM_HODMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', '$sce']
    function IVRM_HODMappingController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce) {

       
        //------------for sorting.........
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.currentPage3 = 1;
        $scope.itemsPerPage3 = 10;
        $scope.search3 = "";
        //------------for sorting.........
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse3 = !$scope.reverse3; //if true make it false and vice versa
        }

        $scope.search = "";
        $scope.search2 = "";
        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = 10;
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse2 = !$scope.reverse2; //if true make it false and vice versa
        }

        //----------load data into page.............
        $scope.loaddata = function () {
            
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            var id = 1;
            apiService.getURI("IVRM_HODMapping/Getdetails", id).
                then(function (promise) {
                    
                    $scope.stafflist = promise.stafflist;
                    $scope.clsslist = promise.clsslist
                    $scope.alldata = promise.alldata;
                //=======================================================//form 2
                    $scope.hodlist = promise.hodlist;
                    $scope.stafflist2 = promise.stafflist2;
                    $scope.gethodstafdata = promise.gethodstafdata;
                })
        }

     
       

        //========selectionist CheckBox Field Validation===========//
        $scope.isOptionsRequired = function () {
            return !$scope.clsslist.some(function (item) {
                return item.selected;
            });
        }


        //=======selection of checkbox....
        $scope.togchkbxC = function () {
            $scope.usercheckC = $scope.clsslist.every(function (role) {
                return role.selected;
            });
        }

        //---------all checkbox Select...
        $scope.all_checkC = function (all) {
            
            $scope.usercheckC = all;
            var toggleStatus = $scope.usercheckC;
            angular.forEach($scope.clsslist, function (role) {
                role.selected = toggleStatus;
            });
        }
        //=======================================

        


        //=====================save data into hod with hod_class table.!
        $scope.saveclsdata = function () {

            $scope.classids = [];
            
            if ($scope.myForm.$valid) {
                angular.forEach($scope.clsslist, function (cls) {
                    if (cls.selected == true) {
                        $scope.classids.push(cls);
                    }
                });
                
                var data = {
                    "IHOD_Id": $scope.ihoD_Id,
                    "HRME_Id": $scope.hrmE_Id,
                    classlst: $scope.classids,
                    
                }
                
                apiService.create("IVRM_HODMapping/saveclsdata", data).
                    then(function (promise) {
                        
                        if (promise.returnval != null && promise.duplicate != null) {
                            if (promise.duplicate == false) {
                                if (promise.returnval == true) {
                                    if ($scope.ihoD_Id > 0) {
                                        swal('Record Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');
                                    }
                                }
                                else {
                                    if (promise.returnval == false) {
                                        if ($scope.ihoD_Id > 0) {
                                            swal('Record Not Update Successfully!!!');
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
                            $state.reload();
                        }
                        else {
                            swal("Kindly Contact Administrator!!!");
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };
        

        //==========================================Edit HOD Data
        $scope.editHoddata = function (id) {
            
            var data = {
                "IHOD_Id": id.ihoD_Id,
            }

            apiService.create("IVRM_HODMapping/editHoddata", data).then(function (promise) {
                
                $scope.edithodlist = promise.edithodlist;
                if ($scope.edithodlist.length > 0) {

                    $scope.stafflist = promise.stafflist2;
                    $scope.clsslist = promise.clsslist;

                    $scope.ihoD_Id = promise.edithodlist[0].ihoD_Id;
                    $scope.hrmE_Id = promise.edithodlist[0].hrmE_Id;
                    $scope.asmcL_Id = promise.edithodlist[0].asmcL_Id;
                }
                
                angular.forEach($scope.clsslist, function (tt) {
                    tt.selected = false;
                    angular.forEach($scope.edithodlist, function (xx) {
                        
                        if (tt.asmcL_Id == xx.asmcL_Id) {
                            
                            tt.selected = true;
                        }
                    })
                })
                $scope.togchkbxC();
            })

        }

        //=========================================Edit HodStaff data
        $scope.editHodStaffdata = function (id) {
            
            var data = {
                "IHODS_Id": id.ihodS_Id,
                "IHOD_Id": id.ihoD_Id,
                
            }

            apiService.create("IVRM_HODMapping/editHodStaffdata", data).then(function (promise) {
                
                $scope.edithodstaflist = promise.edithodstaflist;
                if ($scope.edithodstaflist.length > 0) {
                    $scope.stafflist2=promise.stafflist2;


                    $scope.ihoD_Id = promise.edithodstaflist[0].ihoD_Id;
                    $scope.ihodS_Id = promise.edithodstaflist[0].ihodS_Id;
                    $scope.hrmE_Id = promise.edithodstaflist[0].staff_id;
                }
            })

        }

      

        //====================================Save HOD Staff Data
        $scope.savehodstaf = function () {
            
            if ($scope.myForm2.$valid) {
                var data = {
                    "IHODS_Id": $scope.ihodS_Id,
                    "IHOD_Id": $scope.ihoD_Id,
                    "HRME_Id": $scope.hrmE_Id,
                }
                
                apiService.create("IVRM_HODMapping/savehodstaf", data).
                    then(function (promise) {
                        
                        if (promise.returnval != null && promise.duplicate != null) {
                            if (promise.duplicate == false) {
                                if (promise.returnval == true) {
                                    if ($scope.ihodS_Id > 0) {
                                        swal('Record Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');
                                    }
                                }
                                if ($scope.returnval == false) {
                                    if ($scope.ihodS_Id > 0) {
                                        swal('Record Not Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Not Saved Successfully!!!');
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
                $scope.submitted2 = true;
            }
           
        }



        $scope.onmodelclick = function (id) {
            
            var data = {
                "IHOD_Id": id,
                
            }
            
            apiService.create("IVRM_HODMapping/onmodelclick", data).
                then(function (promise) {
                    $scope.modalclaslist = promise.modalclaslist;
                });
        }
        

        //=====================================================for active and deactive HOD
        $scope.deactivehod = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ihoD_ActiveFlag === true) {
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

                        apiService.create("IVRM_HODMapping/deactivehod", employee).
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


        //===================================================for active and deactive Staff
        $scope.deactivehodstaff = function (employee, SweetAlert) {
            
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ihodS_ActiveFlag === true) {
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

                        apiService.create("IVRM_HODMapping/Deactivatestaf", employee).
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
        

        //===================================================for active and deactive Class
        $scope.Deactivateclass = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ihodC_ActiveFlag === true) {
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

                        apiService.create("IVRM_HODMapping/Deactivateclass", employee).
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


        $scope.clear = function () {
            $state.reload();
        };
    }
})();
