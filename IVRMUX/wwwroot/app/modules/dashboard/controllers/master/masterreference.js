(function () {
    'use strict';
    angular
.module('app')
.controller('MasterReferenceController', MasterReferenceController)

    MasterReferenceController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MasterReferenceController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.sortKey = 'pamR_Id';
        $scope.sortReverse = true;

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



        $scope.MasterReferenceCl = function ()
        {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("MasterReference/getalldetails", pageid).
        then(function (promise) {
            $scope.MasterRef = promise.refernceData;
            $scope.presentCountgrid = $scope.MasterRef.length;
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


        $scope.searchsource = function ()
        {
            
            var data = {
                "PAMR_ReferenceName": $scope.search,
                "PAMR_ReferenceDesc": $scope.type
            }

            apiService.create("MasterReference/1", data).
        then(function (promise)
        {
            $scope.search = "";
            $scope.type = "";
            if (promise.refernceData != null && promise.refernceData.length > 0)
            {
                
                $scope.MasterRef = promise.refernceData;
                $scope.presentCountgrid = $scope.MasterRef.length;
            }
            else{
                swal("No Records Found");
            }
            
        })
        }

        $scope.DeletRecord = function (employee, SweetAlert) {

            $scope.editEmployee = employee.pamR_Id;
            var orgaid = $scope.editEmployee

            swal({


                title: "Are You Sure?",
                text: "Do You Want to Delete the Record !!!!!!!!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
           function (isConfirm) {
               if (isConfirm) {
                   apiService.getURI("MasterReference/Deletedetails", orgaid).
                   then(function (promise) {

                       $scope.MasterRef = promise.refernceData;
                       $scope.presentCountgrid = $scope.MasterRef.length;
                       if (promise.message != null && promise.message != "")
                       {
                           swal(promise.message);
                           return;
                       }
                       if (promise.returnval == "True") {
                           swal('Record Deleted Successfully');
                       }
                      
                       else {
                           swal('Failed To Delete Record');
                       }

                       // $scope.orgname = promise.organisationname;
                   })
               }
               else {
                   swal("Record Deletion Cancelled");
               }
           });

            //})
        }

        $scope.cance = function ()
        {
            $scope.submitted = false;
            $state.reload();
        }

        $scope.interacted = function (field)
        {
            return $scope.submitted;
        };

        $scope.EditMasterRefvalue = function (employee)
        {
            $scope.editEmployee = employee.pamR_Id;
            var orgid = $scope.editEmployee;
            //orgid = 12;
            apiService.getURI("MasterReference/Editdetails", orgid).
            then(function (promise) {

                $scope.PAMR_Id = promise.refernceData[0].pamR_Id;
                $scope.PAMR_ReferenceName = promise.refernceData[0].pamR_ReferenceName;
                $scope.PAMR_ReferenceDesc = promise.refernceData[0].pamR_ReferenceDesc;

                if (promise.activeflag = 1)
                {

                    $scope.activeflag = true;
                }
                else {

                    $scope.activeflag = false;
                }

            })
        }
        //validation part
        $scope.submitted = false;
        $scope.saveMasterdata = function ()
        {
            $scope.submitted = true;

            if ($scope.myForm.$valid)
            {
                var data = {
                    "PAMR_ReferenceName": $scope.PAMR_ReferenceName,
                    "PAMR_ReferenceDesc": $scope.PAMR_ReferenceDesc,
                    "PAMR_Id": $scope.PAMR_Id
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("MasterReference/", data).
                then(function (promise)
                {
                   

                    if (promise.returnval_save == "True")
                    {
                        swal("Record Saved Successfully");
                    }
                    else if (promise.returnval_save == "False")
                    {
                        swal("Failed To Save The Record");
                    }
                    else if (promise.returnval_update == "True")
                    {
                        swal("Record Updated Successfully");
                    }
                    else if (promise.returnval_update == "False")
                    {
                        swal("Failed To Update The Record");
                    }
                    else if (promise.returnval == "Duplicate")
                    {
                        swal("Record Already Exist");
                    }
                    else if (promise.returnval == "The Reference Name Already Exists")
                    {
                        swal('The Reference Name Already Exist');
                    }

                    $state.reload();
                })
            }
        };


        $scope.order = function (keyname)
        {
            $scope.reverse = !$scope.reverse;
            $scope.sortKey = keyname;
        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
                       
            return (angular.lowercase(obj.pamR_ReferenceName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.pamR_ReferenceDesc)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }
    }

})();