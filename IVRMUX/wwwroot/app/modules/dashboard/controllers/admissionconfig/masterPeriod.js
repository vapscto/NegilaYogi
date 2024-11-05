//dashboard.controller("attendanceEntryTypeController", ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash',
//function ($rootScope, $scope, $state, $location, dashboardService, Flash) {



//}]);



(function () {
    'use strict';
    angular
.module('app')
.controller('masterPeriodController', masterPeriodController)



    masterPeriodController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function masterPeriodController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.BindData = function () {
            apiService.getDATA("MasterPeriod/Getdetails").
       then(function (promise) {
           $scope.arrlist2 = promise.yeardropDown;
           $scope.arrlist4 = promise.categorydropDown;
           $scope.gridviewDetails = promise.gridviewDetails;
       })
        };


        $scope.checkboxchcked = [];
        $scope.editcatdrpdwn = [];

        $scope.CheckedCategoryName = function (data) {
            

            if ($scope.checkboxchcked.indexOf(data) === -1) {
                $scope.checkboxchcked.push(data);
            }
            else {
                $scope.checkboxchcked.splice($scope.checkboxchcked.indexOf(data), 1);
            }
        }

        $scope.Deletedata = function (DeleteRecord) {

            $scope.EditId = DeleteRecord.imP_Id;
            var MdeleteId = $scope.EditId

            swal({

                title: "Are you sure?",
                text: "Do you want to delete record !!!!!!!!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: true
            },
           function (isConfirm) {
               if (isConfirm) {
                   apiService.getURI("MasterPeriod/DeleteEntry", MdeleteId).
                   then(function (promise) {
                       swal("Deleted successfully");
                     
                   })
                   $state.reload();
               }
               else {
                   swal("Record Deletion Cancelled", "Failed");
               }
           });


            //var confirmPopup = swal('Are you sure you want to delete this item?');
            //if (confirmPopup === true) {
            //    $scope.deleteId = DeleteRecord.imP_Id;
            //    var MdeleteId = $scope.deleteId;
            //    apiService.DeleteURI("MasterPeriod/DeleteEntry", MdeleteId)

            //    $scope.$apply();

            //    swal("Record Deleted Successfully");
            //    $scope.saved = "Record Deleted Successfully";

            //    $scope.BindData();

            //}

        };

        $scope.Editdata = function (EditRecord) {
            
            $scope.edit = true;
            $scope.EditId = EditRecord.imP_Id;
            var IMPCM_Id = EditRecord.impcM_Id;
            var MEditId = $scope.EditId;
            var data = {
                "IMP_Id": MEditId,
                "IMPCM_Id":IMPCM_Id,
            }


            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("MasterPeriod/GetSelectedRowDetails/", data).
            then(function (promise) {
                
                for (var i = 0; i < $scope.arrlist4.length; i++) {
                    if ($scope.arrlist4[i].amC_Id == promise.categorydropDown[0].amC_Id) {
                        $scope.arrlist4[i].Selected = true;
                        $scope.editcatdrpdwn.push($scope.arrlist4[i]);
                    }
                    else {
                        $scope.arrlist4[i].Selected = false;
                    }
                }

                //for (var i = 0; i < arrlist4.length; i++) {

                //    name = arrlist4[i].amC_Id;
                //    if (name == promise.gridviewDetails[0].amC_Id) {
                //        arrlist4[i].AMC_Id = true
                //    }
                //    else {
                //        arrlist4[i].AMC_Id = false;
                //    }

                //}
                
                $scope.Period = promise.gridviewDetails[0].imP_PeriodName;
                $scope.PeriodOrder = promise.gridviewDetails[0].imP_PeriodOrder;
                $scope.Half = promise.gridviewDetails[0].half;
               // $scope.AMC_Id = promise.gridviewDetails[0].amC_Id;
            })
        };
        $scope.isOptionsRequired = function () {
            return !$scope.arrlist4.some(function (options) {
                return options.Selected;
            });
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.submitted = false;
        $scope.savedata = function () {
            
           
            if ($scope.myForm.$valid) {
                //var CategoryIDs = $scope.checkboxchcked;
                //$scope.albumNameArray = [];
                //angular.forEach($scope.checkboxchcked, function (CategoryIDs) {
                //    if (!!CategoryIDs.AMC_Id) $scope.albumNameArray.push(CategoryIDs);
                //})
                if ($scope.checkboxchcked.length > 0)
                {
                    for (var i = 0; i < $scope.checkboxchcked.length; i++) {
                        delete $scope.checkboxchcked[i].AMC_Id;
                    }
                }
                else {
                    $scope.checkboxchcked = $scope.editcatdrpdwn;
                }
               
               
                var data = {
                 
                    "IMP_Id":  $scope.EditId,
                    "IMP_PeriodName": $scope.Period,
                    "IMP_PeriodOrder": $scope.PeriodOrder,
                    "Half": $scope.Half,
                    "SelectedCategoryDetails": $scope.checkboxchcked
                }


                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("MasterPeriod/", data).then(function (promise) {

                    if (promise.message != null && promise.message != "") {
                        swal(promise.message);
                        $state.reload();
                        return;
                    }

                    else {
                        if (promise.messageupdate == "Update") {
                            if (promise.returnVal == true) {
                                swal("Record Updated Successfully");
                            }
                            else {
                                swal("Failed To Update Record");
                            }
                        }
                        else {
                            swal("Record Saved Successfully");
                        }
                        
                        $state.reload();
                        
                    }

                })



                // stuRecord.push(data);
                //    apiService.create("MasterPeriod/", data)

                //    if (promise.message != null && promise.message != "") {
                //        swal(promise.message);
                //        return;
                //    }
                //    else {
                //        swal('Record Saved Successfully')
                //        $scope.saved = "Record Saved Successfully";
                //        $state.reload();
                //    }


                //    // $scope.BindData();
                //} else {
                //    $scope.submitted = true;
                //}
            }
            else {
                   $scope.submitted = true;
                }
        };
       
        $scope.cleardata = function () {

            $state.reload();

            $scope.Period = "";
            $scope.PeriodOrder = "";
            $scope.AMC_Id = "";
            $scope.Half = "";
            $scope.submitted = false;
            $scope.AMC_Id= "";
         
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();

        }
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
    }

})();