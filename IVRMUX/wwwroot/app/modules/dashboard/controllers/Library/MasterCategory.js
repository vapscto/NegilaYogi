(function () {
    'use strict';
    angular
.module('app')
        .controller('MasterCategoryController', MasterCategoryController)

    MasterCategoryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MasterCategoryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;


          //=====================Loaddata.............
        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";

            var pageid = 2;
            apiService.getURI("MasterCategory/getdetails", pageid).then(function (promise) {
                $scope.ccategorylist = promise.categorylist;
            })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }
         //=====================End-----Loaddata----//


         //=====================saverecord....
        $scope.savedata = function () {
            debugger;
            if ($scope.myForm.$valid) {
                var data = {
                    "LMC_Id": $scope.lmC_Id ,
                    "LMC_CategoryName": $scope.lmC_CategoryName,
                    "LMC_CategoryCode": $scope.lmC_CategoryCode,
                    "LMC_BNBFlg": $scope.lmC_BNBFlg,
                }
                apiService.create("MasterCategory/Savedata", data)
                .then(function (promise) {
                    if (promise.returnval != null && promise.duplicate != null)
                    {
                        if (promise.duplicate == false) {
                            if (promise.returnval == true) {
                                if ($scope.lmC_Id > 0)
                                {
                                    swal('Record Updated Successfully!!!');
                                }
                                else {
                                    swal('Record Saved Successfully!!!');
                                }
                               
                            }
                            else {
                                if (promise.returnval == false)
                                {
                                    if ($scope.lmC_Id > 0)
                                    {
                                        swal('Record Not Update Successfully!!!');
                                    }
                                    else {
                                        swal('Record Not Saved Successfully!!!');
                                    }
                                }                            
                            }
                        }
                        else {
                            swal("Record already exist");
                        }
                        $state.reload();
                    }
                    else {
                        swal("Kindly Contact Administrator!!!");
                    }
                    
                })
                
            }
            else {
                $scope.submitted = true;
            }        
        };
         //=====================End---saverecord....


         //=====================Editrecord....
        $scope.EditData = function (user) {
            debugger;
            $scope.lmC_Id = user.lmC_Id;
            $scope.lmC_CategoryName = user.lmC_CategoryName;
            $scope.lmC_CategoryCode = user.lmC_CategoryCode;
            $scope.lmC_BNBFlg = user.lmC_BNBFlg;
        }
        //====================End---edit--record....


        //=================Activation/Deactivation--Record.........
        $scope.deactiveY = function (user, SweetAlert) {
            debugger;
            $scope.lmC_Id = user.lmC_Id;

            var dystring = "";
            if (user.lmC_ActiveFlag == 1) {
                dystring = "Deactivate";
            }
            else if (user.lmC_ActiveFlag == 0) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.create("MasterCategory/deactiveY", user).
                   then(function (promise) {
                       if (promise.returnval == true) {
                           swal("Record " + dystring + "d Successfully!!!");
                          
                       }
                       else {
                           swal("Record Not " + dystring + "d Successfully!!!");
                           
                       }
                       $state.reload();
                   })
                    
                }
                else {
                    swal("Record " + dystring + " Cancelled");
                }
                
            });
        }
          //================End----Activation/Deactivation--Record.........


        $scope.interacted = function (field) {
            return $scope.submitted;
        };


         //===========----Clear Field
        $scope.Clearid = function () {
            $state.reload();
        }



    }
})();

