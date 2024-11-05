
(function () {
    'use strict';
    angular
.module('app')
        .controller('MasterHeaderDetailsController', MasterHeaderDetailsController)
    MasterHeaderDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function MasterHeaderDetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.BindData = function () {
            apiService.getDATA("MasterHeaderDetails/Getdetails").
       then(function (promise) {
           $scope.institutionModuleList = promise.institutionModuleList;
           $scope.institutionPageLists = promise.institutionPageList;
           $scope.parameterlist = promise.parameterlist;
           $scope.headerdata = promise.headerdata;
         
       })
        };

        $scope.searchchkbx = '';
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

            $scope.EditId = DeleteRecord.ivrmhE_Id;
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
                   apiService.DeleteURI("MasterHeaderDetails/DeleteEntry", MdeleteId).
                   then(function (promise) {
                       swal("Deleted successfully");
                     
                   })
                   $state.reload();
               }
               else {
                   swal("Record Deletion Cancelled", "Failed");
               }
           });
            
        };


        $scope.getModulePage = function (module) {
            $scope.IVRMIMP_Id = '';
            var moduleId = $scope.IVRMIM_Id;
            apiService.getURI("MasterHeaderDetails/getmodulePage", moduleId).
                then(function (promise) {
                    $scope.institutionPageLists = promise.institutionPageList;
                })
        }


        $scope.Editdata = function (EditRecord) {
            
            $scope.edit = true;
          
            var IMPCM_Id = EditRecord.ivrmhE_Id;
           
            var data = {
                "IVRMHE_Id": IMPCM_Id,
                
            }


            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("MasterHeaderDetails/GetSelectedRowDetails/", data).
            then(function (promise) {
                $scope.IVRMHE_Id = promise.gridviewDetails[0].ivrmhE_Id;
                $scope.IVRMIM_Id = promise.gridviewDetails[0].ivrmiM_Id;
                $scope.IVRMIMP_Id = promise.gridviewDetails[0].ivrmimP_Id;
                $scope.IVRMHE_Name = promise.gridviewDetails[0].ivrmhE_Name;

                $scope.editparm = promise.parameterlist

                angular.forEach($scope.parameterlist, function (ee) {
                    angular.forEach($scope.editparm, function (xx) {
                        if (ee.ismP_ID == xx.ismP_ID) {
                            ee.select = true;
                        }

                    })

                })
                
              
            })
        };
        $scope.isOptionsRequired = function () {
            return !$scope.arrlist4.some(function (options) {
                return options.Selected;
            });
        }
        $scope.selectedClasslist = [];
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.submitted = false;
        $scope.savedata = function () {
            $scope.selectedClasslist = [];
           
            if ($scope.myForm.$valid) {
                
                angular.forEach($scope.parameterlist, function (cls) {
                    if (cls.select == true) {
                        $scope.selectedClasslist.push({ ismP_ID: cls.ismP_ID });
                    }
                });
                debugger;
               
                var data = {
                 
                    "IVRMHE_Id": $scope.IVRMHE_Id,
                    "IVRMIM_Id": $scope.IVRMIM_Id,
                    "IVRMIMP_Id": $scope.IVRMIMP_Id,
                    "IVRMHE_Name": $scope.IVRMHE_Name,
                    "pageids": $scope.selectedClasslist
                }
                
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("MasterHeaderDetails/SaveData", data).then(function (promise) {

                        if (promise.messageupdate == "upd") {
                            if (promise.returnVal == true) {
                                swal("Record Updated Successfully");
                            }
                            else {
                                swal("Failed To Update Record");
                            }
                        }
                        else if (promise.messageupdate == "add")
                        {
                        if (promise.returnVal == true) {
                            swal("Record Saved Successfully");
                        }
                        else {
                            swal("Failed To Saved Record");
                        }
                        
                    }
                        else if (promise.messageupdate == "ext") {
                            if (promise.returnVal == true) {
                                swal("Record Exist");
                            }
                            else {
                                swal("Failed To Update Record");
                            }

                        }

                    $state.reload();

                })

                
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

        $scope.filterchkbx23 = function (obj) {
            return (angular.lowercase(obj.ismP_NAME)).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }
    }

})();