(function () {
    'use strict';
    angular
.module('app')
        .controller('CLGCategoryMappingController', CLGCategoryMappingController)

    CLGCategoryMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function CLGCategoryMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.checkboxchckedcls = [];
        $scope.chckedcategoryids = [];


        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;

            var pageid = 2;
            apiService.getURI("CLGCategoryMapping/getalldetails", pageid).
        then(function (promise) {
            $scope.categorylist = promise.categorylist;
            $scope.courselist = promise.courselist;
            $scope.gridOptions.data = promise.detailslist;
            $scope.acdlist = promise.yearlist;
            $scope.sectionlist = promise.sectionlist;
            $scope.subjectlist = promise.subjectlist;
        })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }

        //Ui Grid view rendering data from data base
        $scope.gridOptions = {
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SlNo', field: 'name', enableFiltering: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'asmaY_Year', displayName: 'Academic Year' },
                { name: 'ttmC_CategoryName', displayName: 'Category Name' },
                { name: 'amcO_CourseName', displayName: 'Course Name' },
                { name: 'amB_BranchName', displayName: 'Branch Name' },
            ]
        };


        $scope.getSelectedclass = function (dataobj1) {
            


            if (dataobj1.asmcL_Id1 === true) {
                $scope.checkboxchckedcls.push(dataobj1);
            }
            else if (dataobj1.asmcL_Id1 === false) {

                angular.forEach($scope.checkboxchckedcls, function (option, index) {
                    if (dataobj1.asmcL_Id == option.asmcL_Id)
                        $scope.checkboxchckedcls.splice($scope.checkboxchckedcls.indexOf(option), 1);
                });
            }


        }


     
      

        $scope.getorgvalue = function (employee) {
           
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "TTMC_Id": $scope.ttmC_Id,
                "AMCO_Id": $scope.AMCO_Id,
            }


            apiService.create("CLGCategoryMapping/getBranch", data).
            then(function (promise) {
                $scope.branchlist = promise.branchlist;
                $scope.savedbranch = promise.savedbranch;

               

                angular.forEach($scope.branchlist, function (bb) {
                    angular.forEach($scope.savedbranch, function (ss) {
                        if (bb.amB_Id == ss.amB_Id) {
                            bb.selected = true;

                        }

                    })

                })

            })
        }
        var name = "";
        var name1 = "";

        $scope.clearfields = function () {
            $scope.ttcC_Id = 0;
            // $state.reload();
            $scope.loaddata();

        }
        $scope.cleredata = function () {
            $scope.AMCO_Id = '';
            $scope.ttmC_Id = '';
            $scope.branchlist = [];

         
        }
        $scope.cleredata1 = function () {
            $scope.AMCO_Id = '';
            $scope.branchlist = [];


        }
       
        $scope.submitted = false;
        $scope.savepages = function () {
            var condition = "";
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.checkboxchckedcls = [];

                angular.forEach($scope.branchlist, function (tt) {
                    if (tt.selected == true) {
                        $scope.checkboxchckedcls.push(tt);
                    }

                })

                if ($scope.ttcC_Id > 0)
                {
                    condition = "Allow";
                }
                else {
                    if ($scope.checkboxchckedcls.length > 0) {
                        condition = "Allow"; 
                    }
                    else {
                        condition = "NotAllow";
                    }

                }
                if (condition == "Allow") {
                    var data = {
                        "ASMAY_Id": $scope.asmaY_Id,
                        "TTMC_Id": $scope.ttmC_Id,
                        "AMCO_Id": $scope.AMCO_Id,
                        "clssids": $scope.checkboxchckedcls
                    }
                    apiService.create("CLGCategoryMapping/savedata", data).
                    then(function (promise) {
                        if (promise.returnMsg != "") {
                            if (promise.returnMsg == "duplicate") {
                                swal('Record Exist Already');
                                return;

                            } else if (promise.returnMsg == "add") {
                                swal('Record Saved Successfully', 'success');
                                $scope.detailslist = promise.detailslist;
                                $state.reload();

                            } else if (promise.returnMsg == "update") {
                                swal('Record Updated Successfully', 'success');
                                $scope.detailslist = promise.detailslist;
                                $state.reload();
                            }
                        } else {
                            swal('Record Not Saved/Updated Successfully', 'Failed');
                        }
                    })
                }
                else {
                    swal('At least select one Branch..!', 'Failed');
                }
            }


        };


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

    }

})();