(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterAuthorController', MasterAuthorController)

    MasterAuthorController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MasterAuthorController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;

        //=====================Load--data.............
        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("MasterAuthor/getdetails", pageid).then(function (promise) {
                $scope.ccategorylist = promise.authorlist;
            })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }
         //=====================End-----Load--data----//



          //=====================saverecord....
        $scope.savedata = function () {
            debugger;
            if ($scope.myForm.$valid) {

                if ($scope.lmaU_AuthorMiddleName == undefined || $scope.lmaU_AuthorMiddleName == null) {
                    $scope.lmaU_AuthorMiddleName = '';
                }
                if ($scope.lmaU_AuthorLastName == undefined || $scope.lmaU_AuthorLastName == null) {
                    $scope.lmaU_AuthorLastName = '';
                }
                var data = {
                    "LMAU_Id": $scope.lmaU_Id,
                    "LMAU_AuthorFirstName": $scope.lmaU_AuthorFirstName,
                    "LMAU_AuthorMiddleName": $scope.lmaU_AuthorMiddleName,
                    "LMAU_AuthorLastName": $scope.lmaU_AuthorLastName,
                    "LMAU_MobileNo": $scope.lmaU_MobileNo,
                    "LMAU_PhoneNo": $scope.lmaU_PhoneNo,
                    "LMAU_EmailId": $scope.lmaU_EmailId,
                    "LMAU_Address": $scope.lmaU_Address
                }
                apiService.create("MasterAuthor/Savedata", data)
                    .then(function (promise) {
                        if (promise.returnval != null && promise.duplicate != null) {
                            if (promise.duplicate == false) {
                                if (promise.returnval == true) {
                                    if ($scope.lmaU_Id > 0) {
                                        swal('Record Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');
                                    }
                                }
                                else {
                                    if (promise.returnval == false) {
                                        if ($scope.lmaU_Id > 0) {
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
            
            $scope.lmaU_Id = user.lmaU_Id;
            $scope.lmaU_AuthorFirstName = user.lmaU_AuthorFirstName;
            $scope.lmaU_AuthorMiddleName = user.lmaU_AuthorMiddleName;
            $scope.lmaU_AuthorLastName = user.lmaU_AuthorLastName;
            $scope.lmaU_MobileNo = user.lmaU_MobileNo;
            $scope.lmaU_PhoneNo = user.lmaU_PhoneNo;
            $scope.lmaU_EmailId = user.lmaU_EmailId;
            $scope.lmaU_Address = user.lmaU_Address;
        
        }
         //====================End---edit-record....
       

        //=================Activation/Deactivation--Record.........
        $scope.deactiveY = function (user, SweetAlert) {
            debugger;
            $scope.lmaU_Id = user.lmaU_Id;

            var dystring = "";
            if (user.lmaU_ActiveFlg == 1) {
                dystring = "Deactivate";
            }
            else if (user.lmaU_ActiveFlg == 0) {
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
                        apiService.create("MasterAuthor/deactiveY", user).
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
            return $scope.submitted || field.$dirty;
        };

        //===========----Clear Field
        $scope.Clearid = function () {
            $state.reload();
        }



    }
})();

