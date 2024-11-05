

//dashboard.controller("loginController", ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash',
//function ($rootScope, $scope, $state, $location, dashboardService, Flash) {
//    $scope.predicate = 'sno';
//    $scope.reverse = false;
//    $scope.currentPage = 1;
//    $scope.order = function (predicate) {
//        $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : false;
//        $scope.predicate = predicate;
//    };
//    $scope.students = [
//      { sno: '1', name: 'Kevin', age: 25, gender: 'boy' },
//      { sno: '2', name: 'John', age: 30, gender: 'girl' },
//      { sno: '3', name: 'Laura', age: 28, gender: 'girl' },
//      { sno: '4', name: 'Joy', age: 15, gender: 'girl' },
//      { sno: '5', name: 'Mary', age: 28, gender: 'girl' },
//      { sno: '6', name: 'Peter', age: 95, gender: 'boy' },
//      { sno: '7', name: 'Bob', age: 50, gender: 'boy' },
//      { sno: '8', name: 'Erika', age: 27, gender: 'girl' },
//      { sno: '9', name: 'Patrick', age: 40, gender: 'boy' },
//      { sno: '10', name: 'Tery', age: 60, gender: 'girl' }
//    ];
//    $scope.totalItems = $scope.students.length;
//    $scope.numPerPage = 5;
//    $scope.paginate = function (value) {
//        var begin, end, index;
//        begin = ($scope.currentPage - 1) * $scope.numPerPage;
//        end = begin + $scope.numPerPage;
//        index = $scope.students.indexOf(value);
//        return (begin <= index && index < end);
//    };


//}]);

(function () {
    'use strict';
    angular
.module('app')
.controller('StaffLoginController', StaffLoginController);

    StaffLoginController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http'];
    function StaffLoginController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


        $scope.pagesrecord = {};
        $scope.adds = {};
        var stuDelRecord = {};

        var selection = {};
        $scope.SelectedStudentRecord = {};
        $scope.deleteselectedrecord = {};


        $scope.modulefill = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 10

            $scope.itemsPerPagethird = 5

            var pageid = 2;
            apiService.getURI("StaffLogin/getalldetails", pageid).
        then(function (promise) {

            $scope.institutionname = promise.fillinstitution;
            $scope.staffname = promise.fillstaff;
            $scope.roletype = promise.fillroletype;
            $scope.modulename = promise.fillmodule;
            $scope.categoryName = promise.fillcategory;

            $scope.gridview1 = promise.showgrid1;
            $scope.thirdgriddata = promise.thirdgriddata;

            $scope.secondgrid = {};

            //$scope.totalItems = $scope.thirdgriddata.length;
            //$scope.numPerPage = 5;
        });

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

        };

        $scope.predicate = 'sno';
        $scope.reverse = false;
        $scope.currentPage = 1;
        $scope.order = function (predicate) {
            $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : false;
            $scope.predicate = predicate;
        };

        $scope.paginate = function (value) {
            var begin, end, index;
            begin = ($scope.currentPage - 1) * $scope.numPerPage;
            end = begin + $scope.numPerPage;
            index = $scope.thirdgriddata.indexOf(value);
            return (begin <= index && index < end);
        };

        $scope.selectrole = function () {
            var moduleid = $scope.IVRMM_Id;
            var roletypeid = $scope.IVRMRT_Id;

            var data = {
                "MI_Id": $scope.IVRMM_Id,
                "IVRMRT_Id": $scope.IVRMRT_Id,
            };

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };


            apiService.create("StaffLogin/getpagedetailsrolemodulewise", data).
        then(function (promise) {
            $scope.gridview1 = true;
            $scope.gridview1 = promise.showgrid1;
        });
        };


        $scope.selectinstitution = function () {
            var institutionid = $scope.MI_Id;

            apiService.getURI("StaffLogin/getmodulerolesinswise", institutionid).
        then(function (promise) {
            $scope.roletype = promise.fillroletype;
            $scope.modulename = promise.fillmodule;
        });
        };

        
        $scope.addtocart = function (SelectedStudentRecord, index) {
            $scope.gridview2 = true;
            $scope.secondgrid[index] = SelectedStudentRecord;
        };

        $scope.currenrow = {};
        $scope.deletesecondgriddata = function (index, stuDelRecord, currenrow) {

            //var currentrowid = currenrow.seconduser.ivrmmP_Id

            $scope.newDataList = {};
            var i = 0;

            angular.forEach(stuDelRecord, function () {
                if (i !== index) {
                    // $scope.newDataList.push(stuDelRecord[i])
                    $scope.newDataList[i] = $scope.secondgrid[i];
                }
                i++;
            });
         
            $scope.secondgrid = $scope.newDataList;

        };


        $scope.checkAll = function () {
            $scope.grid1.roles = angular.copy($scope.roles);
        };

        $scope.filteroption = function () {
            var entereddata = $scope.search;

            var data = {
                "IVRMMP_PageName": $scope.searchthird,
                "ivrmM_ModuleName": $scope.thirdgridview
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("StaffLogin/1", data).
        then(function (promise) {
            $scope.thirdgriddata = promise.thirdgriddata;
        })
        }


        $scope.deletrec = function (employee, SweetAlert) {
            $scope.editEmployee = employee.ivrmstauP_Id;
            var staffloginid = $scope.editEmployee;
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
                    apiService.DeleteURI("StaffLogin/deletemodpages", staffloginid).
                    then(function (promise) {

                        $scope.thirdgriddata = promise.thirdgriddata;

                        if (promise.returnval === true) {
                            swal('Record Deleted Successfully!', 'success');
                        }
                        else {
                            swal('Record Not Deleted Successfully!', 'Failed');
                        }
                    });
                }
                else {
                    swal("Record Deletion Cancelled", "Ok");
                }
            });
        };

        //$scope.myAppObjects = {};
        //$scope.checkedItems = function (myAppObjects) {
        //    var checkedItems = [];
        //    angular.forEach($scope.myAppObjects, function (appObj, arrayIndex) {
        //        angular.forEach(appObj, function (cb, key) {
        //            if (key.substring(0, 2) == "cb" && cb) {
        //                checkedItems.push(appObj.id + '-' + key)
        //            }
        //        })
        //    })
        //    return checkedItems
        //}


        $scope.clickall = function (pagesrecord) {
            console.log("dfgt");
        };

        $scope.savadata = function (pagesrecord) {

            var array = $.map(pagesrecord, function (value, index) {
                return [value];
            });

            //var addarray = $.map(adds, function (value, index) {
            //    return [value];
            //});


            var data = {
                "MI_Id": $scope.MI_Id,
                "IVRMSTAUL_Id": $scope.IVRMSTAUL_Id,
                "User_Name": $scope.User_Name,
                "IVRMRT_Id": $scope.IVRMRT_Id,
                "amc_id": $scope.amc_id,
                // ADDDATA: addarray,
                savetmpdata: array
            };

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };

            apiService.create("StaffLogin/", data).
            then(function (promise) {
                $scope.students = promise.fillpagesdata;
                if (promise.returnval === true) {

                    $scope.thirdgriddata = promise.thirdgriddata;

                    $scope.MI_Id = "";
                    $scope.User_Name = "";
                    $scope.IVRMM_Id = "";
                    $scope.amc_id = "";
                    $scope.IVRMRT_Id = "";
                    $scope.IVRMSTAUL_Id = "";

                    swal('Record Saved/Updated Successfully', 'success');
                }
                else {
                    swal('Record Not Saved/Updated Successfully', 'Failed');
                }
            });

        };
    }

})();