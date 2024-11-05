(function () {
    'use strict';
    angular.module('app').controller('masterreligionController', masterreligionController)

    masterreligionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$filter', 'superCache']
    function masterreligionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $filter, superCache) {

        $scope.ivrmmr_Id = 0;
        $scope.flag = "";

        $scope.sortKey = 'ivrmmR_Id';
        $scope.sortReverse = true;

        $scope.submitted = false;
        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden = $scope.IsHidden ? false : true;
        }
        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        }
        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        $scope.searchValue = '';
        $scope.testSub = {};
        $scope.filterValue = function (obj) {
            if (obj.is_Active == true) {
                $scope.testSub = "Active";
            } else if (obj.is_Active == false) {
                $scope.testSub = "Deactive";
            }
            return angular.lowercase(obj.ivrmmR_Name).indexOf(angular.lowercase($scope.searchValue)) >= 0 || ($scope.testSub).indexOf($scope.searchValue) >= 0;
        }
        $scope.loadgrid = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            apiService.getDATA("MasterReligion/").
            then(function (promise) {
                if (promise.religionList == null || promise.religionList.length == 0) {
                    swal('No Records Found');
                }
                else {
                    $scope.religionList = promise.religionList;
                    $scope.presentCountgrid = $scope.religionList.length;
                }

            })

            //$scope.sort = function (keyname) {

            //    $scope.sortKey = keyname;   //set the sortKey to the param passed
            //    $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            //}
        }


        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.saveRecord = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "IVRMMR_Name": $scope.IVRMMR_Name,
                    "IVRMMR_Id": $scope.ivrmmr_Id
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("MasterReligion/", data).
                then(function (promise) {
                    if (promise.message != null && promise.message != "") {
                        swal(promise.message);
                        $state.reload();
                        return;
                    }
                    if (promise.returnval == true) {
                        swal(promise.operation);
                        $state.reload();
                    }
                    else {
                        swal(operation);
                        //$state.reload();
                    }

                })

            }
        }
        $scope.getResult = function (search, searchColumn) {
            if (search != null && search != undefined && searchColumn != null && searchColumn != undefined) {
                var data = {
                    "EnteredData": search,
                    "SearchColumn": searchColumn
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("MasterReligion/SearchByColumn", data).
                then(function (promise) {
                    if (promise.message != "" && promise.message != null) {
                        swal(promise.message);
                    }
                    if (promise.religionList == null || promise.religionList.length == 0) {
                        swal('No Records Found.....!!');
                    }
                    else {
                        $scope.search = "";
                        $scope.searchColumn = "";
                        $scope.religionList = promise.religionList;
                        $scope.presentCountgrid = $scope.religionList.length;
                    }
                })
            }
            else {
                swal('Please select/enter Search criteria');
            }
        }
        $scope.edit = function (ivrmmR_Id) {
            $scope.ivrmmr_Id = ivrmmR_Id;
            apiService.getURI("MasterReligion/Editdetails", ivrmmR_Id).
            then(function (promise) {
                $scope.IVRMMR_Name = promise.religionList[0].ivrmmR_Name;
            })
        }
        $scope.deleteRecord = function (ivrmmR_Id, SweetAlert) {
            swal({
                title: "Are you sure?",
                text: "Do you want to Delete the record !!!!!!!!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
           function (isConfirm) {
               if (isConfirm) {
                   
                   apiService.DeleteURI("MasterReligion/DeleteRecord", ivrmmR_Id).
                   then(function (promise) {
                       if (promise.message != "" && promise.message != null) {
                           swal(promise.message);
                           $state.reload();
                       }
                       if (promise.returnval == true) {
                           swal('Record Deleted Successfully.');
                           $scope.religionList = promise.religionList;
                           $scope.presentCountgrid = $scope.religionList.length;
                       }
                       else {
                           swal('Record Not Deleted.');
                           // $state.reload();
                       }

                       // $scope.orgname = promise.organisationname;
                   })
               }
               else {
                   swal("Record Deletion Cancelled");
                   // $state.reload();
               }
           });
        }
        $scope.deactive = function (religion, SweetAlertt) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            
            var mgs = "";
            if (religion.is_Active == false) {

                mgs = "Activate";

            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Religion?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
             function (isConfirm) {
                 if (isConfirm) {
                     apiService.create("MasterReligion/deactivate", religion).
                     then(function (promise) {
                         
                         if (promise.returnval == true) {
                             if (promise.message != null) {
                                 swal(promise.message);
                                 $scope.loadgrid();
                             }
                         }
                         else {
                             swal('Failed to Activate/Deactivate the Record');
                         }
                     })
                 } else {
                     swal("Cancelled");
                 }
             })
        }

      
    }
})();