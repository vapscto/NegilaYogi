(function () {
    'use strict';
    angular
        .module('app')
        .controller('IVRM_PrincipalMappingController', IVRM_PrincipalMappingController);
    IVRM_PrincipalMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', '$sce']
    function IVRM_PrincipalMappingController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce) {


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
            apiService.getURI("IVRM_PrincipalMapping/Getdetails", id).
                then(function (promise) {
                    
                    $scope.stafflist = promise.stafflist;
                    $scope.clsslist = promise.clsslist
                    $scope.alldata = promise.alldata;
                    //=======================================================//form 2
                    $scope.principlelist = promise.principlelist;
                    $scope.stafflist2 = promise.stafflist2;
                    $scope.getprincplstafdata = promise.getprincplstafdata;
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
                    "IPR_Id": $scope.ipR_Id,
                    "IVRMUL_Id": $scope.ivrmuL_Id,
                    classlst: $scope.classids,

                }
                
                apiService.create("IVRM_PrincipalMapping/saveclsdata", data).
                    then(function (promise) {
                        
                        if (promise.returnval != null && promise.duplicate != null) {
                            if (promise.duplicate == false) {
                                if (promise.returnval == true) {
                                    if ($scope.ipR_Id > 0) {
                                        swal('Record Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');
                                    }
                                }
                                else {
                                    if (promise.returnval == false) {
                                        if ($scope.ipR_Id > 0) {
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


        //----------------edit data.......
        $scope.editprincipledata = function (id) {
            
            var data = {
                "IPR_Id": id.ipR_Id,
            }

            apiService.create("IVRM_PrincipalMapping/editprincipledata", data).then(function (promise) {
                
                $scope.editprincclaslist = promise.editprincclaslist;
                if ($scope.editprincclaslist.length > 0) {

                    $scope.stafflist = promise.stafflist;
                    $scope.clsslist = promise.clsslist

                    $scope.ipR_Id = promise.editprincclaslist[0].ipR_Id;
                    $scope.ivrmuL_Id = promise.editprincclaslist[0].ivrmuL_Id;

                    $scope.irpC_Id = promise.editprincclaslist[0].irpC_Id;
                    //$scope.asmcL_Id = promise.editprincclaslist[0].asmcL_Id;
                }
                
               
                angular.forEach($scope.clsslist, function (tt) {
                    tt.selected = false;
                    angular.forEach($scope.editprincclaslist, function (xx) {
                        
                        if (tt.asmcL_Id == xx.asmcL_Id) {
                            
                            tt.selected = true;
                        }
                    })
                })
               $scope.togchkbxC();
            })

        }



        $scope.editprinciplestaffdata = function (id) {
            
            var data = {
                "IPR_Id": id.ipR_Id,
                "IPRS_Id": id.iprS_Id,
            }

            apiService.create("IVRM_PrincipalMapping/editprinciplestaffdata", data).then(function (promise) {
                
                $scope.editprincstafflist = promise.editprincstafflist;

                if ($scope.editprincstafflist.length > 0) {

                    $scope.stafflist2 = promise.stafflist;

                    $scope.ipR_Id = promise.editprincstafflist[0].ipR_Id;
                    $scope.iprS_Id = promise.editprincstafflist[0].iprS_Id;
                    $scope.hrmE_Id = promise.editprincstafflist[0].hrmE_Id;
                }
            });
        }

        

        $scope.saveprncplstaf = function () {
            
            if ($scope.myForm2.$valid) {
                var data = {
                    "IPRS_Id": $scope.iprS_Id,
                    "IPR_Id": $scope.ipR_Id,
                    "HRME_Id": $scope.hrmE_Id,
                }
                
                apiService.create("IVRM_PrincipalMapping/saveprncplstaf", data).
                    then(function (promise) {
                        
                        if (promise.returnval != null && promise.duplicate != null) {
                            if (promise.duplicate == false) {
                                if (promise.returnval == true) {
                                    if ($scope.iprS_Id > 0) {
                                        swal('Record update Successfully!!!');
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');
                                    }
                                }
                                else {
                                    if (promise.returnval == false) {

                                        if ($scope.iprS_Id > 0) {
                                            swal('Record Not update Successfully!!!');
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
                $scope.submitted2 = true;
            }

        }

        $scope.onmodelclick = function (id) {
            
            var data = {
                "IPR_Id": id,

            }
            
            apiService.create("IVRM_PrincipalMapping/onmodelclick", data).
                then(function (promise) {
                    $scope.modalclaslist = promise.modalclaslist;
                });
        }


        //===============================================Active and deactive For Principal data
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

                        apiService.create("IVRM_PrincipalMapping/deactivehod", employee).
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


        //===============================================Active and deactive For Staff data
        $scope.deactivehodStaff = function (employee, SweetAlert) {
            
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.irpS_ActiveFlag === true) {
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

                        apiService.create("IVRM_PrincipalMapping/Deactivatestaf", employee).
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


        //===============================================Active and deactive For Class data
        $scope.Deactivateclass = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.irpC_ActiveFlag === true) {
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

                        apiService.create("IVRM_PrincipalMapping/Deactivateclass", employee).
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
