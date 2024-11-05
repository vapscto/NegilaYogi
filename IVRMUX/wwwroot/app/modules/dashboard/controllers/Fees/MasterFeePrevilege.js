


(function () {
    'use strict';
    angular
.module('app')
.controller('FeePrevilegeController', FeePreController)

    FeePreController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'superCache', '$stateParams']
    function FeePreController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, superCache,$stateParams) {

        $scope.username = false;
        $scope.disableroles = false;
        $scope.savedisable = true;
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.disabled = true;
                if (privlgs[i].ivrmirP_AddFlag == true) {
                    $scope.saveflg = true;
                    $scope.savebtn = true;

                }
                else {
                    $scope.saveflg = false;
                    $scope.savebtn = false;
                }
                if (privlgs[i].ivrmirP_UpdateFlag == true) {
                    $scope.editflg = true;
                    $scope.editbtn = true;

                }
                else {
                    $scope.editflg = false;
                    $scope.editbtn = false;
                }
                if (privlgs[i].ivrmirP_DeleteFlag == true) {
                    $scope.deactiveflag = true;
                    $scope.deletebtn = true;
                }
                else {
                    $scope.deactiveflag = false;
                    $scope.deletebtn = false;
                }


            }
        }
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.sortKey = "fgL_Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa
        $scope.searchValue1 = "";
        //$scope.students = {

        //    enableFiltering: true,
        //    paginationPageSizes: [5, 10, 15],
        //    paginationPageSize: 5,
        //    columnDefs: [
        //                {
        //                    name: 'SlNo', field: 'name', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>'
        //                },
        //                 {
        //                     name: 'normalizedUserName', displayName: 'User Name'
        //                 },
        //                {
        //                    name: 'fmG_GroupName', displayName: 'Group'
        //                },
        //                {
        //                    name: 'fmH_FeeName', displayName: 'Head'
        //                },

        //                {
        //                    name: 'ivrmrT_Role', displayName: 'Role'
        //                },

        //                {
        //                    field: 'id', name: '',
        //                    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
        //                    '<div class="grid-action-cell">' +
        //                    '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
        //                    '<a href="javascript:void(0)" ng-click="grid.appScope.deletedata(row.entity);"> <i class="fa fa-trash text-danger"></i></a>'+
        //                    //'<a ng-if="row.entity.fmH_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);"> Activate</a>' +
        //                    //'<span ng-if="row.entity.fmH_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);">  Deactivate</a><span>' +
        //                    '</div>'
        //                }
        //    ],
        //    onRegisterApi: function (gridApi) {
        //        $scope.gridApi = gridApi;
        //    }
        //};

        $scope.academicyear = false;
        $scope.loaddata = function () {

            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = paginationformasters;
            $scope.page2 = "page2";

            var pageid = 2;
            apiService.getURI("FeePrevilege/getalldetails", pageid).
        then(function (promise) {
            $scope.academic = promise.adcyear;

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


        $scope.fillhead = function (id) {

            var data = {
                "FMG_Id": id,
                "ASMAY_Id": $scope.asmaY_Id,
            }

            apiService.create("FeePrevilege/getheads", data).
                then(function (promise) {
                    if (promise.fillheads.length > 0) {
                        $scope.head = promise.fillheads;
                       
                    } else {
                        swal('No Records!');
                        $scope.head = [];
                    }
                })
        }


        $scope.toggleAll = function () {
            var toggleStatus = $scope.selectAll;
            angular.forEach($scope.head, function (option)
            {
                option.hea = toggleStatus;
            });

        }

        $scope.disablegroups = false;
        $scope.optionToggled = function (fmG_Id) {

            if ($scope.disablegroups == false) {
                $scope.selectAll = $scope.head.every(function (option) { return option.hea; })
            }
            else {
                for (var i = 0; i < $scope.head.length; i++) {

                    if (fmG_Id == $scope.head[i].fmG_Id) {
                        $scope.head[i].hea = true;
                    }
                    else {
                        $scope.head[i].hea = false;
                    }
                }
            }
        }



        $scope.get_Role = function (id) {
            
            var roleid= id;
            apiService.getURI("FeePrevilege/getusername", roleid).
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
            
            var del = data.fgL_Id;
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
                   apiService.getURI("FeePrevilege/delete", del).
                   then(function (promise) {

                       if (promise.returnval == true) {
                           swal('Record Deleted Successfully');
                       }
                       //else if (promise.returnval == "false") {
                       else{
                          swal ('Record Not Deleted')
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
                    "FGL_Id":$scope.FGL_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "FMG_ID":$scope.fmG_Id,
                    "IVRMRT_Id":$scope.ivrmrT_Id,
                    grouphead: $scope.albumNameArray1,
                    username: $scope.albumNameArray2,
                }
                apiService.create("FeePrevilege/savedetail", data).
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