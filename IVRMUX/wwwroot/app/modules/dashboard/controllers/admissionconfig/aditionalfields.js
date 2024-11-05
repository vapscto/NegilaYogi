(function () {
    'use strict';
    angular.module('app').controller('aditionalfieldsController', aditionalfieldsController)

    aditionalfieldsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q']
    function aditionalfieldsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q) {
        $scope.newuser = [];
        $scope.field_size_flag = false;
        $scope.field_scale_flag = false;
        $scope.IsHiddenup = true;
        $scope.IsHiddendown = true;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;

        $scope.field_type = function () {
            if($scope.newuser.IPAF_Type=="Bigint" ||$scope.newuser.IPAF_Type=="Bit"||$scope.newuser.IPAF_Type=="DateTime")
            {
                $scope.field_size_flag = true;
                $scope.field_scale_flag = true;
            }
            else if ($scope.newuser.IPAF_Type == "Char" || $scope.newuser.IPAF_Type == "NVarchar")
            {
                $scope.field_scale_flag = true;
                $scope.field_size_flag = false;
            }
            else {
                $scope.field_size_flag = false;
                $scope.field_scale_flag = false;
            }
        }
        
        $scope.pageChanged = function (newPage) {
            if (newPage > 0) {
                $scope.newPage = newPage;


            }
        };

        $scope.ShowHideup = function () {

            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        }



        $scope.ShowHidedown = function () {


            $scope.IsHiddendown = $scope.IsHiddendown ? false : true;
        }

        $scope.propertyName = 'pasE_FirstName';

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };


        $scope.sortBydropdown = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }


        $scope.Clearid = function () {
            $state.reload();
           // loadInitialData();
        }

        $scope.loadData = function  () {
            //swal("hi");
            apiService.getURI("AdditionalFields/getbasicdata", 2).then(function (promise) {
                if (promise.additionalList.length > 0 && promise.additionalList != null) {
                    $scope.pagination = true;
                    $scope.additionalfieldlist = promise.additionalList;
                }
                else {
                    $scope.pagination = false;
                }
            }
            )
        }

        //function loadInitialData() {
        //    swal("hi");
        //    apiService.getURI("AdditionalFields/getbasicdata", 2).then(function (promise) {
        //        $scope.additionalfieldlist = promise.additionalList;
        //    })
        //}

        $scope.submitted = false;

        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "IPAF_Id": $scope.ipaF_Id,
                    "Page_Id": $scope.newuser.Page_Id,
                    "IPAF_Flag": $scope.newuser.IPAF_Active_Flag,      //checkbox
                    "IPAF_Name": $scope.newuser.IPAF_Name,
                    "IPAF_Type": $scope.newuser.IPAF_Type,
                    "IPAF_Size": $scope.newuser.IPAF_Size,
                    "IPAF_Scale": $scope.newuser.IPAF_Scale,
                    "IPAF_Apl_Report": $scope.newuser.IPAF_Apl_Report,
                    "IPAF_Display_Name": $scope.newuser.IPAF_Display_Name,
                    "IPAF_Active_Flag": '1'           //checkbox
                }
                
                apiService.create("AdditionalFields", data).then(function (promise) {
                    $scope.newuser.Page_Id = "";
                    $scope.newuser.IPAF_Name = "";
                    $scope.newuser.IPAF_Type = "";
                    $scope.newuser.IPAF_Size = "";
                    $scope.newuser.IPAF_Scale = "";
                    $scope.newuser.IPAF_Apl_Report = "";
                    $scope.newuser.IPAF_Display_Name = "";
                    $scope.newuser.IPAF_Active_Flag = "";

                    if (promise.message != null || promise.message!="") {                     
                        if (promise.message == "Save") {
                            if(promise.returnval==true){
                                swal("Record Saved Successfully");
                            }
                            else{
                                swal("Failed To Save Record");
                            }   
                        }
                        else if (promise.message == "Update") {
                            if (promise.returnval == true) {
                                swal("Record Updated Successfully");
                            }
                            else {
                                swal("Failed To Update Record");
                            }
                        }
                        else {
                            swal("Record Already Exist");
                        }
                    }

                    //swal('Record Saved/Updated Successfully', 'success');
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;

            }
        };

        $scope.deactive = function (id, flag) {
            apiService.getURI("AdditionalFields/deactivate", id).then(function (promise) {
                if (flag == '1') {
                    swal('Record Deactivated Successfully', 'success');
                }
                else {
                    swal('Record Activated Successfully', 'success');
                }
                $scope.loadData();
            })
        }



        $scope.deactive = function (id, flag) {
            

            var mgs = "";
            if (flag == 0) {

                mgs = "Activate";

            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Record?",
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
                        apiService.getURI("AdditionalFields/deactivate", id).then(function (promise) {
                            if (flag == '1') {
                            swal('Record Deactivated Successfully');
                            }
                            else {
                                swal('Record Activated Successfully');
                            }
                            $scope.loadData();
                        })
                    } else {
                        swal("Cancelled");
                    }
                });

        }









        //$scope.filterValue = function ($event) {
        //    if (isNaN(String.fromCharCode($event.keyCode))) {
        //        $event.preventDefault();
        //    }
        //};

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.edit = function (id) {
            //$scope.editEmployee = employee.IPAF_Id;
            // var templId = $scope.editEmployee;

            apiService.getURI("AdditionalFields/editdata", id).then(function (promise) {
                $scope.newuser.Page_Id = promise.editingList[0].page_Id;
                $scope.newuser.IPAF_Name = promise.editingList[0].ipaF_Name;
                $scope.newuser.IPAF_Type = promise.editingList[0].ipaF_Type;
                $scope.newuser.IPAF_Size = promise.editingList[0].ipaF_Size;
                $scope.newuser.IPAF_Scale = promise.editingList[0].ipaF_Scale;
                $scope.newuser.IPAF_Display_Name = promise.editingList[0].ipaF_Display_Name;
                $scope.ipaF_Id = promise.editingList[0].ipaF_Id;
                $scope.newuser.IPAF_Apl_Report = promise.editingList[0].ipaF_Apl_Report;
                $scope.newuser.IPAF_Active_Flag = promise.editingList[0].ipaF_Active_Flag;
                $scope.loadData();
            })
        }
    }
})();
