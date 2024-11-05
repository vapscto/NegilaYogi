
(function () {
    'use strict';
    angular
.module('app')
.controller('MasterBuildingController', MasterBuildingController)

    MasterBuildingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$element']
    function MasterBuildingController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $element) {


        $scope.users = [
     { id: 1, name: 'Bob' },
     { id: 2, name: 'Alice' },
     { id: 3, name: 'Steve' }
        ];
        $scope.selectedUser = { id: 1, name: 'Bob' };


        //Ui Grid view rendering data from data base
        $scope.gridOptions1= {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                   { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
              { name: 'ttmB_BuildingName', displayName: 'Building Name' },
         
               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                       '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.ttmB_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.ttmB_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                '</div>'
               }
            ]          

        };
        //Ui Grid view rendering data from data base
        $scope.gridOptions2 = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                   { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
              { name: 'ttmB_BuildingName', displayName: 'Building Name' },
            { name: 'asmcL_ClassName', displayName: 'Class' },
            { name: 'asmC_SectionName', displayName: 'Section' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                  '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue1(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                 '<a ng-if="row.entity.ttmbcS_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive1(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
               '<span ng-if="row.entity.ttmbcS_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive1(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                 '</div>'
                }
            ]

        };

        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {
                     
            $scope.submitted = true;
            if ($scope.myForm1.$valid) {

                var data = {
                    "TTMB_Id": $scope.TTMB_Id,
                    "TTMB_BuildingName": $scope.TTMB_BuildingName,
                }
                apiService.create("MasterBuilding/savedetail", data).
                    then(function (promise) {
                        if (promise.returnval === true) {
                            swal('Data successfully Saved');
                            $scope.BindData();
                            $scope.clearid();
                        }
                        else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                            swal('Records Already Exist !');
                        }
                        else {
                            swal('Data Not Saved !');
                        }
                    })               
            }

        };

        $scope.submitted1 = false;
        $scope.saveddata1= function () {
            
            $scope.submitted1 = true;

            if ($scope.myForm2.$valid) {
                $scope.albumNameArray1 = [];
                $scope.albumNameArray2 = [];
               
                angular.forEach($scope.class, function (role) {
                    if (role.cls) $scope.albumNameArray1.push(role);
                })
                angular.forEach($scope.section, function (role) {
                    if (role.section) $scope.albumNameArray2.push(role);
                })

                var data = {
                    "TTMBCS_Id":$scope.TTMBCS_Id,
                    "TTMB_Id": $scope.ttmB_Id,
                    classarray: $scope.albumNameArray1,
                    sectionarray: $scope.albumNameArray2,
                }
                apiService.create("MasterBuilding/savedetail1", data).
                    then(function (promise) {
                        if (promise.returnval === true) {
                            swal('Data successfully Saved');
                            $scope.BindData();
                            $scope.clearid1();
                        }
                        else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                            swal('Records Already Exist !');
                        }
                        else {
                            swal('Data Not Saved !');
                        }
                       
                    })
            }

        };

        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            
            apiService.getDATA("MasterBuilding/getalldetails").
       then(function (promise) {
           $scope.currentPage = 1;
           $scope.itemsPerPage = 5;
           $scope.bname = promise.masterbuilding;
           $scope.section = promise.secdrp;
           $scope.class = promise.clsdrp;
           $scope.gridOptions1.data = promise.bnamedrp;
           $scope.gridOptions2.data = promise.csmap;
       })
        };    
        //TO clear  data
        $scope.clearid= function () {      
            $scope.TTMB_Id = 0;
            $scope.TTMB_BuildingName = "";
            $scope.submitted = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();         
        };

        $scope.clearid1 = function () {
            
            $scope.submitted1 = false;
            $scope.ttmB_Id = "";
            $scope.TTMBCS_Id = 0;
            angular.forEach($scope.section, function (role) {
                    role.section = false;
            })
            angular.forEach($scope.class, function (role) {
                role.cls = false;
            })
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
            $scope.BindData();
        };

        $scope.isOptionsRequired = function () {

            return !$scope.class.some(function (options) {
                return options.cls;
            });
        }

        $scope.isOptionsRequired1 = function () {

            return !$scope.section.some(function (options) {
                return options.section;
            });
        }



        //TO  Edit Record
        $scope.getorgvalue = function (employee) {
            
            $scope.Masterbuild = employee.ttmB_Id;
            var editid = $scope.Masterbuild;
            apiService.getURI("MasterBuilding/getpagedetails", editid).
            then(function (promise) {
                $scope.TTMB_BuildingName = promise.masterbuild[0].ttmB_BuildingName;
                $scope.TTMB_Id = promise.masterbuild[0].ttmB_Id;
                
            })
        }


        $scope.getorgvalue1 = function (employee) {
            
            $scope.Mastersection = employee.ttmbcS_Id;
            var editid = $scope.Mastersection;
            angular.forEach($scope.class, function (role) {
                role.cls = false;
            })
            angular.forEach($scope.section, function (role) {
                role.section = false;
            })
            apiService.getURI("MasterBuilding/getpagedetails1", editid).
            then(function (promise) {
                $scope.TTMBCS_Id = promise.mastersection[0].ttmbcS_Id;
                $scope.ttmB_Id = promise.mastersection[0].ttmB_Id;
                angular.forEach($scope.class, function (role) {
                    if (role.asmcL_Id == promise.mastersection[0].asmcL_Id)
                        role.cls = true;
                })
                angular.forEach($scope.section, function (role) {
                    if (role.asmS_Id == promise.mastersection[0].asmS_Id)
                        role.section = true;
                })
               // $scope.asmcL_Id = promise.mastersection[0].asmcL_Id;
               // $scope.asmS_Id = promise.mastersection[0].asmS_Id;
            })
        }

      

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted1 || field.$dirty;
        };
        // To Active or Deactive
        $scope.deactive = function (building) {
            var mgs = "";
            var confirmmgs = "";
            if (building.ttmB_ActiveFlag == true) {
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

                   var config = {
                       headers: {
                           'Content-Type': 'application/json;'
                       }
                   }

                   apiService.create("MasterBuilding/deactivate", building).
                       then(function (promise) {
                           if (promise.returnval === true) {
                               swal(confirmmgs + ' Successfully');
                           }
                           else {
                               swal('Record Not  Activated/Deactivated');
                           }
                           $scope.BindData();
                           $scope.clearid1();
                       })
               }
               else {
                   swal("Record" + mgs + "Cancelled");
               }
           });
        }

        $scope.deactive1 = function (building) {
            var mgs = "";
            var confirmmgs = "";
            if (building.ttmbcS_ActiveFlag == true) {
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

                   var config = {
                       headers: {
                           'Content-Type': 'application/json;'
                       }
                   }

                   apiService.create("MasterBuilding/deactivate1", building).
                       then(function (promise) {
                           if (promise.returnval === true) {
                               swal(confirmmgs + ' Successfully');
                           }
                           else {
                               swal('Record Not  Activated/Deactivated');
                           }
                           $scope.BindData();
                           $scope.clearid();
                       })
               }
               else {
                   swal("Record" + mgs + "Cancelled");
               }
           });
        }
    }

})();