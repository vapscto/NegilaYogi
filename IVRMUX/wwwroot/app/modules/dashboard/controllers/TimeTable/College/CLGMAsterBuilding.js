
(function () {
    'use strict';
    angular
        .module('app')
        .controller('CLGMasterBuildingController', CLGMasterBuildingController)

    CLGMasterBuildingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$element']
    function CLGMasterBuildingController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $element) {


        $scope.users = [
            { id: 1, name: 'Bob' },
            { id: 2, name: 'Alice' },
            { id: 3, name: 'Steve' }
        ];
        $scope.selectedUser = { id: 1, name: 'Bob' };
        //Ui Grid view rendering data from data base
        $scope.gridOptions1 = {
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
                { name: 'asmaY_Year', displayName: 'Academic Year' },
                { name: 'ttmB_BuildingName', displayName: 'Building Name' },
                { name: 'amcO_CourseName', displayName: 'Course Name' },
                { name: 'amB_BranchName', displayName: 'Branch Name' },
                { name:'amsE_SEMName',displayName:'Semester Name'},               
                { name: 'acmS_SectionName', displayName: 'Section Name' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        //'<div class="grid-action-cell">' +
                        //'<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue1(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
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
                apiService.create("CLGMasterBuilding/savedetail", data).
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
        //shil
        //$scope.get_course = function () {
        //    var data = {
        //        "TTMB_Id": $scope.ttmB_Id                  
        //        }
        //    apiService.create("CLGMasterBuilding/get_course", data).
        //            then(function (promise) {
        //                $scope.courselist = promise.courselist;
        //                if (promise.courselist == "" || promise.courselist == null) {
        //                    swal("No Course/Branch Are Mapped To Selected Category");
        //                }
        //            })
        //    }        
        $scope.getBranch = function () {
            var data = {
                "TTMB_Id": $scope.ttmB_Id,             
                "AMCO_Id": $scope.AMCO_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
            }
            apiService.create("CLGTTCommon/getBranch", data).
                then(function (promise) {

                    $scope.branchlist = promise.branchlist;

                    if (promise.branchlist == "" || promise.branchlist == null) {
                        swal("No Branch Are Mapped To Selected Category/Course");
                    }
                })
        };
        $scope.get_semister = function () {                    
            var data = {
                "TTMB_Id": $scope.ttmB_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
            }
            apiService.create("CLGTTCommon/get_semister", data).
                then(function (promise) {
                    $scope.semisterlist = promise.semisterlist;
                    if (promise.semisterlist == "" || promise.semisterlist == null) {
                        swal("No Semesters Are Mapped To Selected Course/Branch");
                    }
                })
    };
    //section
    $scope.get_section = function () {
       
        var data = {
            "TTMB_Id": $scope.ttmB_Id,
            "AMCO_Id": $scope.AMCO_Id,
            "AMB_Id": $scope.AMB_Id,
            "AMSE_Id": $scope.AMSE_Id
        }
        apiService.create("CLGMasterBuilding/get_section", data).
            then(function (promise) {

               
                $scope.section = promise.secdrp;
                if (promise.section == "" || promise.section == null) {
                    swal("No sections Are Mapped To Selected Branch/Course");
                }
            })
    };


        $scope.submitted1 = false;
        $scope.saveddata1 = function () {
            $scope.submitted1 = true;
            if ($scope.myForm2.$valid) {
                $scope.albumNameArray1 = [];
                $scope.albumNameArray2 = [];
                //angular.forEach($scope.class, function (role) {
                //    if (role.cls) $scope.albumNameArray1.push(role);
                //})
                angular.forEach($scope.section, function (role) {
                    if (role.section) $scope.albumNameArray2.push(role);
                })
                var data = {
                    "TTMBCS_Id": $scope.TTMBCS_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "TTMB_Id": $scope.ttmB_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    //classarray: $scope.albumNameArray1,
                    sectionarray: $scope.albumNameArray2,
                }
                apiService.create("CLGMasterBuilding/savedetail1", data).
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
            var pageid = 2;
            apiService.getURI("CLGMasterBuilding/getalldetails", pageid).
                then(function (promise) {
                    $scope.currentPage = 1;
                    $scope.itemsPerPage = 5;
                    $scope.bname = promise.masterbuilding;
                    $scope.section = promise.secdrp;
                    $scope.class = promise.clsdrp;
                    $scope.gridOptions1.data = promise.bnamedrp;
                    $scope.gridOptions2.data = promise.csmap;
                    $scope.courselist = promise.courselist;
                    $scope.academic = promise.academic;
                })
        };
        //TO clear  data
        $scope.clearid = function () {
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
            $scope.AMB_Id = "";
            $scope.AMCO_Id = "";
            $scope.AMSE_Id = "";
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
            angular.forEach($scope.section, function (role) {
                role.section = false;
            })
            apiService.getURI("CLGMasterBuilding/getpagedetails1", editid).
                then(function (promise) {
                    $scope.TTMBCS_Id = promise.mastersection[0].ttmbcS_Id;
                    $scope.ttmB_Id = promise.mastersection[0].ttmB_Id;
                    $scope.AMCO_Id = promise.mastersection[0].amcO_Id;
                    $scope.AMB_Id = promise.mastersection[0].amB_Id;
                    $scope.AMSE_Id = promise.mastersection[0].amsE_Id;                   
                    angular.forEach($scope.section, function (role) {
                        if (role.acmS_Id == promise.mastersection[0].acmS_Id)
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
                        apiService.create("CLGMasterBuilding/deactivate1", building).
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