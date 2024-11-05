


(function () {
    'use strict';
    angular
.module('app')
.controller('MasterClgFeePrevilegeController', FeePreController)

    FeePreController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'superCache']
    function FeePreController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, superCache) {

        $scope.username = false;
        $scope.disableroles = false;
        
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));
        $scope.cfg = {};

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        //var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        //if (ivrmcofigsettings.length > 0) {
        //    paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        //}
        //else if (ivrmcofigsettings.length == 0 || ivrmcofigsettings == undefined || ivrmcofigsettings == null) {
        //    paginationformasters = 5;
        //}

        $scope.sortKey = "fgL_Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa
        $scope.searchValue1 = "";
       

        $scope.academicyear = false;
        $scope.loaddata = function () {

            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = paginationformasters;
            $scope.page2 = "page2";

            var pageid = 2;
            apiService.getURI("MasterClgFeePrevilege/getalldetails", pageid).
        then(function (promise) {
            $scope.academic = promise.adcyear;
            //$scope.cfg.ASMAY_Id = academicyrlst[0].asmaY_Id;
            $scope.Group = promise.group;
            $scope.head = promise.head;
            $scope.Role = promise.role;
            $scope.students = promise.prelist;

            $scope.totcountfirst = $scope.students.length;

        })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

        }

        $scope.get_Role = function (id) {
            
            var roleid = id;
            apiService.getURI("MasterClgFeePrevilege/getusername", roleid).
               then(function (promise) {
                   if (promise.usnam.length > 0) {
                       $scope.username = true;
                       $scope.uname = promise.usnam;
                   } else {
                       swal('No match found!');
                       $scope.username = false;
                   }
               })
        }

        $scope.EditData = function (data) {
            
            var editid = data.fgL_Id;
            apiService.getURI("FeePrevilege/edit", editid).
               then(function (promise) {

                   $scope.disableroles = true;

                   $scope.academicyear = true;
                   $scope.FGL_Id = promise.editlist[0].fgL_Id;
                   $scope.asmaY_Id = promise.asmaY_Id;
                   $scope.fmG_Id = promise.editlist[0].fmG_ID;
                   $scope.ivrmrT_Id = promise.usnam[0].ivrmrT_Id;
                   if ($scope.ivrmrT_Id > 0) {
                       $scope.username = true;
                   } else {
                       swal('Role not found!');
                       $scope.username = false;
                   }
                   angular.forEach($scope.head, function (role) {
                       role.hea = false;
                       if (role.fmH_Id == promise.editlist[0].fmH_Id) {
                           role.hea = true;
                       }
                   })
                   $scope.uname = promise.usnam;
                   angular.forEach($scope.uname, function (role) {
                       if (role.userId == promise.editlist[0].user_Id) {
                           role.usr = true;
                       }
                   })

               })
        }


        $scope.deletedata = function (data) {

            var data = {
                "FGL_Id": data.fgL_Id
            }
            
            //var del = data.fgL_Id;
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
           function (isConfirm) {
               if (isConfirm) {
                   apiService.create("MasterClgFeePrevilege/Deletedetails", data).
                   then(function (promise) {

                       if (promise.returnval == true) {
                           swal('Record Deleted Successfully');
                       }
                           //else if (promise.returnval == "false") {
                       else {
                           swal('Record Not Deleted')
                       }
                       $state.reload();
                   })
               }
               else {
                   swal("Record Deletion Cancelled");
                   $state.reload();
               }
           });
            //})
        }

        $scope.isOptionsRequired = function () {

            return !$scope.head.some(function (options) {
                return options.hea;
            });
        }

        $scope.isOptionsRequired1 = function () {

            return !$scope.uname.some(function (options) {
                return options.usr;
            });
        }

        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.saveddata = function () {
            
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.albumNameArray1 = [];
                $scope.albumNameArray2 = [];

                angular.forEach($scope.head, function (role) {
                    if (role.hea) $scope.albumNameArray1.push(role);
                })
                angular.forEach($scope.uname, function (role) {
                    if (role.usr) $scope.albumNameArray2.push(role);
                })
                var data = {
                    "FGL_Id": $scope.FGL_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "FMG_ID": $scope.fmG_Id,
                    "IVRMRT_Id": $scope.ivrmrT_Id,
                    grouphead: $scope.albumNameArray1,
                    username: $scope.albumNameArray2,
                }
                apiService.create("MasterClgFeePrevilege/savedetail", data).
                    then(function (promise) {
                        if (promise.returnval == true && promise.fgL_Id > 0) {
                            swal('Record Updated Successfully');
                            $state.reload();
                        }
                        else if (promise.returnval == true && promise.fgL_Id == 0) {
                            swal('Record Saved Successfully');
                            $state.reload();
                        }
                        else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                            swal('Record Already Exist');
                        }
                        else {
                            swal('Record Not Saved Successfully');
                        }
                    })

            }
            else {
                $scope.submitted = true;
            }

        };

        $scope.Clear = function () {
            
            $state.reload();
        };
    }

})();