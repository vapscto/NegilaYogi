(function () {
    'use strict';
    angular
        .module('app')
        .controller('ReligionCategory_Mapping', ReligionCategory_Mapping)
    ReligionCategory_Mapping.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache']
    function ReligionCategory_Mapping($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache) {
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";
       // $scope.edit_c = true;
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        // ============================ng_click function for cast (ALL)========================//
        $scope.searchchkbx1 = "";
        $scope.searchchkbx = "";
        $scope.all_checkC = function () {
            var checkStatus = $scope.usercheckC;
            var count = 0;
            angular.forEach($scope.caste_list, function (itm) {
                itm.selected = checkStatus;
                if (itm.selected == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        }
        //===ng_click function for cast (ALL voice versa)
        $scope.togchkbxC = function () {
            $scope.usercheck = $scope.caste_list.every(function (options) {
                return options.selected;
            });
        }
        //=============================================clear function========================================//
        $scope.submitted = false;
        $scope.Clearid = function () {        
           
            angular.forEach($scope.caste_list, function (itm) {
                if (itm.selected) {
                }
                $scope.usercheck = '';            
                $scope.searchchkbx1 = '';
                $scope.IVRMMR_Id = '';
                $scope.selected = '';
                $scope.searchchkbx1 = '';
                $scope.religion_list = [];
                $scope.caste_list = [];
                $scope.submitted = false;
                $scope.edit_c = false;
            });
            $state.reload();
        };
        $scope.isOptionsRequired = function () {
            return !$scope.caste_list.some(function (options) {
                return options.selected;
            });
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //============================================load data=====================================================//
        $scope.submitted = false;
        $scope.loaddata = function () {
         
            var pageid = 2;
            apiService.getURI("ReligionCategory_Mapping/loaddata", pageid).then(function (promise) {
               
                $scope.religion_list = promise.religion_list;
                $scope.caste_list = promise.caste_list;
                $scope.get_masterlist = promise.get_masterlist;
                $scope.get_master = $scope.get_masterlist.length;
            });
        }        
        //==============================save data=================================//
        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
                $scope.casts = [];
                angular.forEach($scope.caste_list, function (rr) {
                    if (rr.selected == true) {
                        $scope.casts.push(rr);
                    }
                });
                var data = {
                    "IRCC_Id": $scope.ircC_Id,                    
                    "IVRMMR_Id": $scope.IVRMMR_Id,
                    "castid": $scope.casts,
                }
                apiService.create("ReligionCategory_Mapping/savedata", data).then(function (promise) {
                    if (promise.duplicate == true) {
                        swal("Data is already Existing");
                    }
                    else if (promise.msg == 'Saved') {
                        swal("Data is saved");
                        $state.reload();
                    }
                    else if (promise.msg == 'Failed') {
                        swal("Data is not Saved");
                    }
                    else if (promise.msg == 'Updated') {
                        swal("Data Successfully Updated");
                        $state.reload();
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        }
        //=================Edit
        $scope.Editdata = function (user) {
            var data = {
                "IRCC_Id": user.ircC_Id
            }
            apiService.create("ReligionCategory_Mapping/Editdata", data).then(function (promise) {
                if (promise.editlist.length > 0) {
                    $scope.edit_c = "";
                    $scope.edit_c === true;
                    $scope.ircC_Id = promise.editlist[0].ircC_Id;
                    $scope.IVRMMR_Id = promise.editlist[0].ivrmmR_Id;
                   // $scope.caste_list = promise.caste_list;                   
                    angular.forEach($scope.caste_list, function (yy) {
                        angular.forEach(promise.editlist, function (uu) {
                            if (yy.imcC_Id == uu.imcC_Id) {
                                yy.selected = true;
                            }
                        })
                    })

                }
            })
        }
        //==================================================active/deactive===========================================//
        $scope.masterDecative = function (usersem, SweetAlert) {          
            var dystring = "";
            if (usersem.ircC_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (usersem.ircC_ActiveFlg == false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To" + dystring + "Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + "it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("ReligionCategory_Mapping/masterDecative", usersem).then(function (promise) {                            
                            if (promise.returnval == true) {
                                swal("Record " + dystring + "d" + " Successfully!!!");
                                $state.reload();
                            }
                            else {
                                swal("Record Not " + dystring + "d" + " Successfull!!!");
                                $state.reload();
                            }
                        })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        }        
    }
})();