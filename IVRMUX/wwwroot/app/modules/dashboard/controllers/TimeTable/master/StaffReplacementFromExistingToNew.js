
(function () {
    'use strict';
    angular
.module('app')
.controller('StaffReplacementFromExistingToNewController', StaffReplacementFromExistingToNewController)

    StaffReplacementFromExistingToNewController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', 'Excel', '$stateParams', '$filter']
    function StaffReplacementFromExistingToNewController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, Excel, $stateParams, $filter) {
        $scope.editEmployee = {};
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.grid_view = false;

        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };

        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        
        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
               { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
              { name: 'ttmC_CategoryName', displayName: 'Catergory' },
            { name: 'asmcL_ClassName', displayName: 'Class' },
            { name: 'asmC_SectionName', displayName: 'Section' },
            { name: 'hrmE_EmployeeFirstName', displayName: 'Staff' },
            { name: 'ismS_SubjectName', displayName: 'Subject' },
            { name: 'ttmP_PeriodName', displayName: 'Period' },
              // {
              //     field: 'id', name: '',
              //     displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
              //         '<div class="grid-action-cell">' +
              //   '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
              //  '<a ng-if="row.entity.ttmB_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              //'<span ng-if="row.entity.ttmB_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
              //  '</div>'
              // }
            ]

        };

        //TO clear  data
        $scope.clearid = function () {
            
            $state.reload();
        };


        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            
            apiService.getDATA("StaffReplacementFromExistingToNew/getdetails").
       then(function (promise) {
           $scope.subject = promise.subdrp;
           $scope.class = promise.clsdrp;
           $scope.Category = promise.categorylist;
           $scope.section = promise.secdrp;
           $scope.staff1 = promise.staffDrpDwn;
           $scope.staff2 = promise.staffDrpDwn;
           $scope.gridOptions.data = promise.view;

       })
        };
        

        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {
            
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.hrmE_Id1 == $scope.hrmE_Id2)
                {
                    swal('Same staff Name.. Please choose a different staff Name !');
                }
                else 
                {
                    var data = {
                        "ttmC_Id": $scope.ttmC_Id,
                        "asmcL_Id": $scope.asmcL_Id,
                        "asmS_Id": $scope.asmS_Id,
                        "staf_from": $scope.hrmE_Id1,
                        "staf_to": $scope.hrmE_Id2,
                        "ismS_Id": $scope.ismS_Id,
                    }
                    apiService.create("StaffReplacementFromExistingToNew/savedetail", data).
                        then(function (promise) {
                            if (promise.returnval === true) {
                                swal('Staff Name successfully Replaced');
                                $scope.BindData();
                            }
                            else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                                swal('Records Doesnot Exist !');
                            }
                            else {
                                swal('Data Not Saved !');
                            }
                            $scope.BindData();
                        })
                }
            }
            else {
                $scope.submitted = true;
            
            }
           
        };
    }

})();