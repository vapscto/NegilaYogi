
(function () {
    'use strict';
    angular
        .module('app')
        .controller('HouseInchargeController', HouseInchargeController)

    HouseInchargeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
    function HouseInchargeController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.sortReverse = true;
        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("HouseIncharge/Getdetails", pageid).
                then(function (promise) {
                    
                    $scope.yearlist = promise.yearlist;
                    //$scope.emplist = promise.emplist;
                    $scope.filldepartment = promise.filldepartment;
                    if (promise.alldata.length > 0) {
                        $scope.alldatalist = promise.alldata;
                    }

                    $scope.cancel();

                  
                })
        };



        //========classlist CheckBox Field Validation===========//
        $scope.isOptionsRequired = function () {
            return !$scope.emplist.some(function (item) {
                return item.selected;
            });
        }


        //=======selection of checkbox....
        $scope.togchkbxC = function () {

            $scope.usercheckC = $scope.emplist.every(function (role) {
                return role.selected;
            });
        }


        //---------all checkbox Select...
        $scope.all_checkC = function (all) {

            $scope.usercheckC = all;
            var toggleStatus = $scope.usercheckC;
            angular.forEach($scope.emplist, function (role) {
                role.selected = toggleStatus;
            });
        }


        $scope.get_department = function () {
         
            var data = {
                "HRMD_Id": $scope.HRMD_Id,
            }
            apiService.create("HouseIncharge/getdepchange/", data).then(function (promise) {
                    $scope.filldesignation = promise.filldesignation;
                })
        };


        $scope.get_emp = function () {
          
            debugger;
            var data = {
                "HRMDES_Id": $scope.HRMDES_Id,
                "HRMD_Id": $scope.HRMD_Id,
            }

            apiService.create("HouseIncharge/get_staff1", data).then(function (promise) {

                $scope.emplist = promise.emplist;

            })
        }

        // to Edit Data
        $scope.editrecord = function (EditRecord) {
            debugger;
            var data = {
                "SPCCMHS_Id": EditRecord.spccmhS_Id,
                "ASMAY_Id": EditRecord.asmaY_Id,

            }

            apiService.create("HouseIncharge/editrecord/", data).
                then(function (promise) {
                    $scope.emplist = promise.emplist;

                    $scope.SPCCMHS_Id = promise.editdata[0].spccmhS_Id;
                    $scope.SPCCMH_Id = promise.editdata[0].spccmH_Id;
                    $scope.SPCCMHS_Description = promise.editdata[0].spccmhS_Description;
                    $scope.ASMAY_Id = promise.editdata[0].asmaY_Id;
                    $scope.hrmE_Id = promise.editdata[0].hrmE_Id;

                    $scope.get_House();
                    $scope.SPCCMH_Id = promise.editdata[0].spccmH_Id;
                  
                    $scope.HRMD_Id = promise.editdata[0].hrmD_Id;
                    //$scope.get_department();

                   
                    //$scope.HRMDES_Id = promise.editdata[0].hrmdeS_Id;
                    //$scope.get_emp();
                    $scope.filldesignation = promise.filldesignation;
                    $scope.HRMDES_Id = promise.editdata[0].hrmdeS_Id;

                    angular.forEach($scope.emplist, function (ss) {
                        angular.forEach(promise.editdata, function (tt) {
                            if (ss.hrmE_Id == tt.hrmE_Id) {
                                ss.selected = true;
                            }
                        })
                    })
                

                })
        };

        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        // TO Save The Data
        $scope.submitted = false;
        $scope.saverecord = function () {
            $scope.emplistdata = [];
            if ($scope.myForm.$valid) {
        
                    angular.forEach($scope.emplist, function (cls) {
                        if (cls.selected == true) {
                            $scope.emplistdata.push(cls);
                        }
                    });

                var data = {
                    "SPCCMHS_Id": $scope.SPCCMHS_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,             
                    "SPCCMHS_Description": $scope.SPCCMHS_Description,
                    "SPCCMH_Id": $scope.SPCCMH_Id,
                    emplstdata: $scope.emplistdata,
      
                }
                apiService.create("HouseIncharge/saverecord", data).
                    then(function (promise) {

                        if (promise.returnval != null && promise.dulicate != null) {
                            if (promise.dulicate == false) {
                                if (promise.returnval == true) {
                                    if ($scope.spccmhS_Id > 0) {
                                        swal('Record Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');
                                    }

                                }
                                else {
                                    if (promise.returnval == false) {
                                        if ($scope.spccmhS_Id > 0) {
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

        /////====================================deactive and active
        $scope.deactive = function (newuser1, SweetAlertt) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            var mgs = "";
            if (newuser1.spccmhS_ActiveFlag == false) {

                mgs = "Activate";

            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the House Committee?",
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
                        apiService.create("HouseIncharge/deactive", newuser1).
                            then(function (promise) {

                                if (promise.returnval == true) {
                                 
                                    swal("Record " + mgs + "d Successfully!!!");
                                    $state.reload();

                                    }
                                    else {
                                        swal("Record Not " + mgs + "d Successfully!!!");
                                    $state.reload();
                                    }
                            })
                    }
                    else {
                        swal("Cancelled");
                    }
                })
        }


        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        }

        $scope.IsHidden2 = true;
        $scope.ShowHide2 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden2 = $scope.IsHidden2 ? false : true;
        }


        $scope.cancel = function () {
         
            $scope.SPCCMH_Id = "";  
            $scope.SPCCMH_Id = "";            
            $scope.SPCCMHS_Description = "";
            $scope.ASMAY_Id = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();

            $scope.emplist = [];
            $scope.HRMD_Id = "";
            $scope.HRMDES_Id = "";
            angular.forEach($scope.emplist, function (itm1) {
                itm1.selected = false;
            })

            $scope.usercheckC = "";

          
        }

        $scope.searchValue = "";
       

    

        //============================Get House List
        $scope.get_House = function () {
          
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
            }
            apiService.create("HouseIncharge/get_House", data).
                then(function (promise) {

                    $scope.houseList = promise.houseList;

                });
        }






    }

})();