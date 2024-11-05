(function () {
    'use strict';
    angular
.module('app')
.controller('categoryclassmappController', categoryclassmappController)

    categoryclassmappController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function categoryclassmappController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.checkboxchckedcls = [];
        $scope.chckedcategoryids = [];


        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;

            var pageid = 2;
            apiService.getURI("CategoryClassMapp/getalldetails", pageid).
        then(function (promise) {
            $scope.categorylist = promise.categorylist;
            $scope.classlist = promise.classlist;
            $scope.gridOptions.data = promise.detailslist;
            $scope.acdlist = promise.acdlist;
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
                   { name: 'academicName', displayName: 'Academic Year' },
            { name: 'categoryName', displayName: 'Category Name' },
             { name: 'className', displayName: 'Class Name' }
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


        $scope.editEmployee = {}
        $scope.deletedata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.ttcC_Id;
            var pageid = $scope.editEmployee;
            swal({
                title: "Are you sure?",
                text: "Do you want to delete record !!!!!!!!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("CategoryClassMapp/deletedetails", pageid).
                    then(function (promise) {

                        if (promise.returnval === true) {
                            swal('Record Deleted Successfully!', 'success');
                            $scope.detailslist = promise.detailslist;
                            $state.reload();
                        }
                        else {
                            swal('Record Not Deleted Successfully!', 'Failed');
                        }
                    })
                }
                else {
                    swal("Record Deletion Cancelled", "Failed");
                }
            });
        }

        $scope.getorgvalue = function (employee) {
            
            $scope.editEmployee = employee;
            var pageid = $scope.editEmployee;

            //$scope.ttcC_Id = employee;

            for (var i = 0; i < $scope.classlist.length; i++) {
                name1 = $scope.classlist[i].asmcL_Id1
                if (name1 == true) {
                    $scope.classlist[i].asmcL_Id1 = '';
                }
            }

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "TTMC_Id": pageid
            }


            apiService.create("CategoryClassMapp/getdetails", data).
            then(function (promise) {
                $scope.ttcC_Id = promise.ttcC_Id;
                $scope.classlist = promise.classlist;

                $scope.checkboxchckedcls = [];

                for (var i = 0; i < $scope.classlist.length; i++) {
                    name1 = $scope.classlist[i].asmcL_Id
                    for (var j = 0; j < promise.binddetails.length; j++) {
                        if (name1 == promise.binddetails[j].asmcL_Id) {
                            $scope.classlist[i].asmcL_Id1 = true;
                            $scope.checkboxchckedcls.push(promise.binddetails[j]);
                        }
                    }
                }

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
            $scope.ttcC_Id = 0;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;

            var pageid = 2;
            apiService.getURI("CategoryClassMapp/getalldetails", pageid).
        then(function (promise) {
            $scope.categorylist = promise.categorylist;
            $scope.classlist = promise.classlist;
            $scope.gridOptions.data = promise.detailslist;
            $scope.sectionlist = promise.sectionlist;
            $scope.subjectlist = promise.subjectlist;
        })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

        }

        $scope.submitted = false;
        $scope.savepages = function () {
            var condition = "";
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
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
                        "clssids": $scope.checkboxchckedcls
                    }
                    apiService.create("CategoryClassMapp/", data).
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
                    swal('At least select one class..!', 'Failed');
                }
            }


        };


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

    }

})();