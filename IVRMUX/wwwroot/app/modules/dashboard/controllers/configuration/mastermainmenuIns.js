

(function () {
    'use strict';
    angular
.module('app')
.controller('masterMenuINSController', masterMenuINSController)

    masterMenuINSController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache']
    function masterMenuINSController(rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache) {

        //$http.get('/Angular/AllStudents') // added an '/' along with deleting Controller portion
        // .then(function (response) {
        //     $scope.newuser = response.data
        // })

     

        var HostName = location.host;
       
        $scope.Previous = function () {
            $window.location.href = 'http://' + HostName + '/#/app/institutionmodulepage/';
        };

        $scope.Next = function () {
            $window.location.href = 'http://' + HostName + '/#/app/MasterSubMenuINS/';
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

            apiService.getDATA("InstituteMainMenu/Getdetails").
       then(function (promise) {
           
           $scope.modulename = promise.masterModulesname;
           $scope.GridDetails = promise.gridDetails;
           $scope.sortKey = "ivrmmmI_Id";
           $scope.reverse = true;
           $scope.MainMenulist = promise.masterMainMenuName;
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

        $scope.IconList = [{ id: "Graduation-Cap", value: "graduation-cap" }, { id: "Users+", value: "user-plus" }, { id: "users", value: "users" }, { id: "CreditCard", value: "credit-card-alt " }, { id: "Desktop", value: "desktop" }, { id: "Map", value: "map" },
            { id: "Server", value: "server" }, { id: "Calendar", value: "calendar-plus-o " }, { id: "Cake", value: "birthday-cake" }, { id: "Envelope", value: "envelope" },
            { id: "Bus ", value: "bus " }, { id: "Add Cart", value: "cart-plus" }, { id: "Book", value: "book" },
            { id: "Reddit ", value: "reddit " }, { id: "Tie", value: "black-tie" }, { id: "Fee-Money", value: "money" }, { id: "Chart-Bar", value: "bar-chart" },
            { id: "Edit-Pencil", value: "pencil-square-o  " }, { id: "University", value: "university" }, { id: "Soccer Ball", value: "soccer-ball-o" }, { id: "Area Chart", value: "area-chart" }, { id: "Download", value: "cloud-download" }, { id: "Upload", value: "cloud-upload" }, { id: "Comments", value: "comments" }, { id: "Dashboard", value: "tachometer" }, { id: "Database", value: "database" }, { id: "Line-chart", value: "line-chart" }, { id: "Mastercard", value: "cc-mastercard" }, { id: "Pie-chart", value: "pie-chart" }
          ];

        $scope.ColorList = [{  id:"color1", value: "color1" }, {  id:"color2", value:"color2" }, {  id:"color3", value: "color3" }, {  id:"color4", value: "color4" },
           {  id:"color5", value: "color5" }, {  id:"color6", value: "color6" },{  id:"color7", value: "color7" },
            { id: "color8", value: "color8" }, { id: "color9", value: "color9" }, { id: "color10", value: "color10" }, { id: "color11", value: "color11" }, { id: "color12", value: "color12" },
            { id: "color13", value: "color13" }, { id: "color14", value: "color14" }, { id: "color15", value: "color15" }, { id: "color16", value: "color16" },
            { id: "color17", value: "color17" }, { id: "color18", value: "color18" }, { id: "color19", value: "color19" }, { id: "color20", value: "color20" }, { id: "color21", value: "color21" },
            { id: "color22", value: "color22" }, { id: "color23", value: "color23" }, { id: "color24", value: "color24" }, { id: "color25", value: "color25" }, { id: "color26", value: "color26" }, { id: "color27", value: "color27" }, { id: "color28", value: "color28" }, { id: "color29", value: "color29" }, { id: "color30", value: "color30" }
        ];


        $scope.institutionchange = function (IVRMM_Id) {

            if (IVRMM_Id === undefined || IVRMM_Id === "") {
                IVRMM_Id = 0;
            }

            var data = {
                "MI_Id": IVRMM_Id
            }
            

            apiService.create("InstituteMainMenu/getmoduledetails/", data).
        then(function (promise) {
            $scope.modulename = promise.masterModulesname;
            $scope.GridDetails = promise.gridDetails;
            $scope.institutionname = promise.gridDetails[0].mI_Name;
        })
        }



        $scope.getMenusName = function (IVRMM_Id) {

            var mi_id = $scope.MI_Id


            if (IVRMM_Id === undefined || IVRMM_Id === "")
            {
                IVRMM_Id = 0;
            }

            var data = {
                
                "MI_Id": mi_id,
              
                "IVRMM_Id": IVRMM_Id,
              
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("InstituteMainMenu/getMenudetailsByModuleId", data).
           // apiService.getURI("InstituteMainMenu/getMenudetailsByModuleId", IVRMM_Id).
                then(function (promise) {
                    if (promise.masterMainMenuName.length > 0) {
                        $scope.MainMenulist = promise.masterMainMenuName;
                    }
                    else {
                        $scope.MainMenulist = [];
                        swal("No Records found..!!");
                    }
           
        })
        }

        $scope.editEmployee = {}
        $scope.DeleteMasterMainMenudata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.ivrmmmI_Id;
            var MdeleteId = $scope.editEmployee;

            swal({
                title: "Are you sure?",
                text: "Do you want to delete record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("InstituteMainMenu/MasterDeleteMainMenuDTO", MdeleteId).
                    then(function (promise) {

                        if (promise.returnval === "true") {
                            swal('Record Deleted Successfully!', 'success');
                            $state.reload();
                        }
                        else {
                            swal('Record Cant Be Deleted!!');
                        }

                        $state.reload();
                       
                    })
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });
        }


        $scope.sortableOptions = {
            handle: '> .handle'
        }

        $scope.EditMasterMainMenudata = function (EditRecord) {
            $scope.EditId = EditRecord.ivrmmM_Id;
            var MEditId = $scope.EditId;
            apiService.getURI("InstituteMainMenu/GetSelectedRowDetails/", MEditId).
            then(function (promise) {

                $scope.IVRMMM_Id = promise.masterModulesname[0].ivrmmM_Id;
                $scope.IVRMM_Id = promise.masterModulesname[0].ivrmM_Id;
                $scope.MainMainMenuName = promise.masterModulesname[0].ivrmmM_MenuName;


            })
        };

        $scope.cance = function () {
          //  $scope.IVRMMM_Id = "";
            $state.reload();
        }

        $scope.interacted = function (field) {

            return $scope.submitted;
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


        $scope.Iconchange = function (icon) {
            $scope.icondesignn = icon;
        }

        $scope.Colorchange = function (icon) {
            $scope.iconcolorr = icon;
        }

        $scope.saveMasterMainMenudata = function (menuorder,ivrmmmI_MenuName) {

            $scope.submitted = true;

            var IDs = $scope.checkboxchcked;

            if ($scope.frmmodule.$valid) {


                var data = {
                    "IVRMMMI_Id": $scope.IVRMMMI_Id,
                    "MI_Id": $scope.MI_Id,
                    "IVRMMM_Id": $scope.IVRMMM_Id,
                    "IVRMMMI_MenuName": $scope.ivrmmmI_MenuName,
                    "IVRMMMI_Color": $scope.iconcolorr,
                    "IVRMMMI_Icon": $scope.icondesignn,
                    "IVRMM_Id": 0,
                    "IVRMMMI_ParentId": 0,
                    "IVRMMMI_PageNonPageFlag": "False",
                    "IVRMMMI_MenuOrder": menuorder,
                    "SelectedMasterMenuDetails": IDs
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("InstituteMainMenu/", data).
                        then(function (promise) {
                           

                            if (promise.returnval === "af") {
                                swal('Record Saved Successfully', 'success');

                            }
                            else if (promise.returnval === "at") {
                                swal('Record Not Saved Successfully');
                            }
                            else if (promise.returnval === "ut") {
                                swal('Record Not Updated Successfully');
                            }
                            else if (promise.returnval === "uf") {
                                swal('Record Not Updated Successfully');
                            }
                            $state.reload();

                        })
            };
        }


        $scope.getOrder = function (orderarray) {
            $scope.nulldisply = false;
            angular.forEach(orderarray, function (value, key) {
                if (value.ivrmmmI_Id !== 0) {
                    //orderarray[key].ivrmmmI_MenuOrder = key + 1;till sort not working
                    orderarray[key].ivrmmmI_MenuOrder = value.ivrmmmI_MenuOrder;
                }
                if (value.ivrmmmI_MenuName == null || value.ivrmmmI_MenuName == undefined) {
                    $scope.nulldisply = true;
                }
                //else {
                //    $scope.nulldisply = false;
                //}
            });

            if ($scope.nulldisply == false) {
            var data = {
                menuDTO: orderarray,
            }
            apiService.create("InstituteMainMenu/validateordernumber", data).
                then(function (promise) {
                    if (promise.retrunMsg != "" && promise.retrunMsg != undefined && promise.retrunMsg != null) {
                        swal(promise.retrunMsg);
                        $state.reload();

                    }
                });
        }
            else {
            swal("Enter display name!! ");
        }
        }
    }

})();