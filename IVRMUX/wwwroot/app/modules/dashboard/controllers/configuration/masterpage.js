
(function () {
    'use strict';
    angular
.module('app')
.controller('MasterPageController', MasterPageController)

    MasterPageController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$cookieStore', '$window','superCache']
    function MasterPageController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $cookieStore, $window, superCache) {

        var HostName = location.host;

        $scope.Previous = function () {
            $window.location.href = 'http://' + HostName + '/#/app/module/';
        };

        $scope.Next = function () {
            $window.location.href = 'http://' + HostName + '/#/app/pagemapping/';
        };

        $scope.Finish = function () {
            $window.location.href = 'http://' + HostName + '/#/app/generalSettingWizardComplete/';
        };

        //pagePreviledges();
        //function pagePreviledges() {
        //    var pageid = $stateParams.pageId;
        //    apiService.getURI("Common/getPreviledgs", pageid).then(function (promise) {
        //        $scope.pagepreviledges = promise.pagePreviledgs[0];
        //        //swal($scope.pagepreviledges.ivrmrP_DeleteFlag);
        //    })
        //};

        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;

            var pageid = 2;
            apiService.getURI("MasterPage/getalldetails", pageid).then(function (promise) {
                $scope.pages = promise.pagesdata;

                //$scope.pages = $filter('orderBy')($scope.pages, 'createdDate');

                //$scope.totalItems = $scope.pages.length;
                //$scope.numPerPage = 5;
            })

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
            index = $scope.pages.indexOf(value);
            return (begin <= index && index < end);
        };

        $scope.clk = function () {
            swal("Yahooo!!!!!!!!!!!!");
        }

        $scope.searchsource = function () {
            var entereddata = $scope.search;

            var data = {
                "IVRMMP_PageName": $scope.search,
                "IVRMP_PageDesc": $scope.type,
                "IVRMP_Id": $scope.IVRMP_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("MasterPage/1", data).
        then(function (promise) {
            $scope.pages = promise.pagesdata;
            swal("searched Successfully");
        })
        }

        //$scope.deletedata = function () {
        //    apiService.delete("MasterPage", 2).
        //then(function (promise) {
        //    // swal("Delete Successfully");
        //    swal("deleted Successfully");
        //})
        //}

        $scope.editEmployee = {}
        $scope.deletedata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.ivrmP_Id;
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
                    apiService.DeleteURI("MasterPage/deletepages", pageid).
                    then(function (promise) {

                        if (promise.pagesdata.length > 0)
                        {
                            $scope.pages = promise.pagesdata;
                            swal(promise.returnval)
                        }

                        //if (promise.returnval == true) {
                        //    swal('Record Deleted Successfully!', 'success');
                        //}
                        //else {
                        //    swal('Record Not Deleted Successfully!', 'Failed');
                        //}
                    })
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });
        }

        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.ivrmP_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("MasterPage/getdetails", pageid).
            then(function (promise) {

                $scope.IVRMMP_PageName = promise.pagesdata[0].ivrmmP_PageName;
                $scope.IVRMP_PageDesc = promise.pagesdata[0].ivrmP_PageDesc;
                $scope.IVRMP_PageDisplayName = promise.pagesdata[0].ivrmP_PageDisplayName;
                $scope.IVRMP_Id = promise.pagesdata[0].ivrmP_Id;
                $scope.IVRMP_MandatoryFlag = promise.pagesdata[0].ivrmP_MandatoryFlag;
            })
        }

        $scope.clearfields = function () {
            //$scope.IVRMP_Id = "";
            //$scope.IVRMMP_PageName = "";
            //$scope.IVRMP_PageDesc = "";
            //$scope.submitted = false;
            //$scope.myForm.$setPristine();
            //$scope.myForm.$setUntouched();
            $state.reload();
        }
        $scope.submitted = false;
        $scope.savepages = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "IVRMMP_PageName": $scope.IVRMMP_PageName,
                    "IVRMP_PageDesc": $scope.IVRMP_PageDesc,
                    "IVRMP_PageURL": $scope.IVRMP_PageDesc,
                    "IVRMP_PageDisplayName": $scope.IVRMP_PageDisplayName,
                    "IVRMP_Id": $scope.IVRMP_Id,
                    "IVRMP_MandatoryFlag": $scope.IVRMP_MandatoryFlag
                }


                apiService.create("MasterPage/", data).
                then(function (promise) {

                    if (promise.pagesdata.length > 0) {
                        //$scope.pages = promise.pagesdata;
                        //swal(promise.returnval)
                         if (promise.returnval == "Record Saved Successfully") {
                             swal('Record Saved Successfully');
                             $state.reload();
                         }
                        if (promise.returnval == "Records Updated Successfully") {
                            swal('Records Updated Successfully');
                            $state.reload();
                        }
              
                        else if (promise.returnval == "Duplicate Records") {
                            swal('MasterPage Already Exist');
                        }
                        
                       
                    }


                })
            } else {
                $scope.submitted = true;
            }
            
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
    }

})();