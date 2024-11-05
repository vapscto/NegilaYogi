

(function () {
    'use strict';
    angular
.module('app')
.controller('masterSubMenuINSController', masterSubMenuINSController)

    masterSubMenuINSController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window','superCache']
    function masterSubMenuINSController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, superCache) {


        //$http.get('/Angular/AllStudents') // added an '/' along with deleting Controller portion
        // .then(function (response) {
        //     $scope.newuser = response.data
        // })

        var HostName = location.host;

        $scope.Previous = function () {
            $window.location.href = 'http://' + HostName + '/#/app/MasterMainMenuINS/';
        };

        $scope.Next = function () {
            $window.location.href = 'http://' + HostName + '/#/app/MasterMenuPageMappingInstitutionwise/';
        };

        $scope.Finish = function () {
            $window.location.href = 'http://' + HostName + '/#/app/generalSettingWizardComplete/';
        };

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

        $scope.IsHidden2 = true;
        $scope.ShowHide2 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden2 = $scope.IsHidden2 ? false : true;
        }

        $scope.BindData = function () {

            $scope.page4 = "page4";
            $scope.page3 = "page3";

            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;

            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 10;

            apiService.getDATA("InstituteSubMenu/Getdetails").
       then(function (promise) {
           $scope.modulename = promise.masterModulesname;
           $scope.GridDetails = promise.gridDetails;
           $scope.sortKey = "ivrmmmI_Id";
           $scope.reverse = true;
           $scope.MainMenulist = promise.masterMainMenuName;
           //$scope.SubMenulist = promise.masterSubMenuName;
           $scope.institution = promise.fillinstitution
        
       })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

            $scope.sort1 = function (keyname) {
                $scope.sortKey1 = keyname;   //set the sortKey to the param passed
                $scope.reverse1 = !$scope.reverse1; //if true make it false and vice versa
            }

        };

        $scope.institutionchange = function (IVRMM_Id) {

            if (IVRMM_Id === undefined || IVRMM_Id === "") {
                IVRMM_Id = 0;
            }

            var data = {
                "MI_Id": IVRMM_Id
            }


            apiService.create("InstituteSubMenu/getmoduledetails/", data).
        then(function (promise) {
            $scope.modulename = promise.masterModulesname;
            $scope.GridDetails = promise.gridDetails;
        })
        }


        $scope.getMenusName = function (IVRMM_Id) {

            if (IVRMM_Id === undefined || IVRMM_Id === "") {
                IVRMM_Id = 0;
            }

            var data = {
                "IVRMM_Id": IVRMM_Id,
                "MI_Id": $scope.MI_Id,
            }
            apiService.create("InstituteSubMenu/getMenudetailsByModuleId", data).
                then(function (promise) {
          
                    $scope.MainMenulist = promise.masterMainMenuName;
                    $scope.GridDetails = promise.gridDetails;
                    $scope.institutionname = promise.gridDetails[0].mI_Name;
        })
        }

        $scope.getSubMenusName = function (IVRMMM_Id) {

            if (IVRMMM_Id === undefined || IVRMMM_Id === "") {
                IVRMMM_Id = 0;
            }

            var data = {
                "IVRMMM_Id": $scope.IVRMMM_Id,
                "MI_Id": $scope.MI_Id,
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }



            apiService.create("InstituteSubMenu/getSubMenudetailsByMainMenuId", data).
        then(function (promise) {

            if (promise.masterSubMenuName.length > 0) {
                $scope.SubMenulist = promise.masterSubMenuName;
            }
            else {
                swal("All Submenues are mapped!!");
            }
        })
        }

        $scope.editEmployee = {}
        $scope.DeleteMasterMainMenudata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.ivrmmmI_Id;
            var MdeleteId = $scope.editEmployee;

            swal({
                title: "Are you sure?",
                text: "Do you want to Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("InstituteSubMenu/MasterDeleteMainMenuDTO", MdeleteId).
                    then(function (promise) {

                        if (promise.returnval == "true") {
                            swal('Record Deleted Successfully!', 'success');
                            $state.reload();
                        }
                        else {
                            swal('Record Cant Be Deleted','It Is Reffered!! ');
                        }

                        $state.reload();

                    })
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });
        }


        $scope.EditMasterMainMenudata = function (EditRecord) {
            $scope.EditId = EditRecord.ivrmmM_Id;
            var MEditId = $scope.EditId;
            apiService.getURI("InstituteSubMenu/GetSelectedRowDetails/", MEditId).
            then(function (promise) {

                $scope.IVRMMM_Id = promise.masterModulesname[0].ivrmmM_Id;
                $scope.IVRMM_Id = promise.masterModulesname[0].ivrmM_Id;
                $scope.MainMainMenuName = promise.masterModulesname[0].ivrmmM_MenuName;


            })
        };

        $scope.cance = function () {
         //   $scope.IVRMMM_Id = "";
            $state.reload();
        }

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };
        $scope.submitted = false;

        $scope.checkboxchcked = [];

        $scope.Checked = function (data) {

            if ($scope.checkboxchcked.indexOf(data) === -1) {
                $scope.checkboxchcked.push(data);
            }
            else {
                $scope.checkboxchcked.splice($scope.checkboxchcked.indexOf(data), 1);
            }
        }


        $scope.saveMasterMainMenudata = function () {

            $scope.submitted = true;

            var IDs = $scope.checkboxchcked;

            if ($scope.frmmodule.$valid) {


                var data = {
                    "IVRMMMI_Id": $scope.IVRMMMI_Id,
                    "MI_Id": $scope.MI_Id,
                    "IVRMMM_Id": $scope.IVRMMM_Id,
                    "IVRMMMI_MenuName": $scope.MainMainMenuName,
                    "IVRMM_Id": 0,
                    "IVRMMMI_ParentId": 0,
                    "IVRMMMI_PageNonPageFlag": "False",
                    "IVRMMMI_MenuOrder": $scope.Order,
                    "SelectedMasterMenuDetails": IDs
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("InstituteSubMenu/", data).
                        then(function (promise) {

                            if (promise.returnval == "utrue") {
                                swal('Record Updated Successfully', 'success');
                                $state.reload();
                            }
                            else if (promise.returnval == "atrue") {
                                swal('Record Saved Successfully', 'success');
                                $state.reload();
                            }
                            else if (promise.returnval == "ufalse") {
                                swal('Record Not Updated Successfully');
                                $state.reload();
                            }
                            else if (promise.returnval == "afalse") {
                                swal('Record Not Saved Successfully');
                                $state.reload();
                            }
                           
                            else if (promise.returnval == "Duplicate") {
                                swal('Institute Sub Menu Already Record Exists');
                            }
                            else if (promise.returnval == "DuplicateOrder") {
                                swal('Institute Sub Menu Order Already Record Exists');
                            }
                            $state.reload();

                        })
            };
        }



        //fix the order drag
        //ConfigA is an Items
        $scope.resetLists = function () {
            $scope.configA = {
                // Changed sorting within list
                onUpdate: function (evt) {
                    var itemEl = evt.item;  // dragged HTMLElement
                    // + indexes from onEnd
                }
            };
        };
        $scope.init = function () {

            $scope.resetLists();

        };
        $scope.init();
        $scope.getOrder = function (orderarray) {
            angular.forEach(orderarray, function (value, key) {
                if (value.ivrmmmI_Id !== 0) {
                    //orderarray[key].ivrmmmI_MenuOrder = key + 1;
                    orderarray[key].ivrmmmI_MenuOrder = value.ivrmmmI_MenuOrder;
                }
            });
            var data = {
                menuDTO: orderarray,
            }
            apiService.create("InstituteSubMenu/validateordernumber", data).
                then(function (promise) {
                    if (promise.retrunMsg != "" && promise.retrunMsg != undefined && promise.retrunMsg != null) {
                        swal(promise.retrunMsg);
                        $state.reload();

                    }
                });
        }







    }

})();