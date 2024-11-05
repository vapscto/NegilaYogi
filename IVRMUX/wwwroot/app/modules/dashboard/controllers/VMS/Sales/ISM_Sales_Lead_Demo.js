(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_Sales_Lead_DemoController', iSM_Sales_Lead_DemoController)

    iSM_Sales_Lead_DemoController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function iSM_Sales_Lead_DemoController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


        $scope.sortKey = 'ISMSLEDMPR_Id';
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



        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }
        //============= + - row===============
        $scope.demonstration = [{ itrS_Id: 'trans1' }];
        if ($scope.demonstration.length === 1) {
            $scope.cnt = 1;
        }
        $scope.addpirows = function () {
            if ($scope.demonstration.length > 1) {
                for (var i = 0; i === $scope.demonstration.length; i++) {
                    var id = $scope.demonstration[i].itrS_Id;
                    var lastChar = id.substr(id.length - 1);
                    $scope.cnt = parseInt(lastChar);
                }
            }
            $scope.cnt = $scope.cnt + 1;
            $scope.tet = 'trans' + $scope.cnt;
            var newItemNo = $scope.cnt;
            $scope.demonstration.push({ 'itrS_Id': 'trans' + newItemNo });

        };
        $scope.removepirows = function (index, data) {
            var newItemNo = $scope.demonstration.length - 1;
            $scope.demonstration.splice(index, 1);

            if (data.amstB_Id > 0) {
                $scope.Deletepirows(data);
            }

        };

        //get data
        $scope.LoadData = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("Sales_Lead/get_load_lead_demo", pageid)
                .then(function (promise) {
                    $scope.lead_list_dd = promise.lead_list_dd;
                    $scope.hrme_list_dd = promise.hrme_list_dd;
                    $scope.product_list_dd = promise.product_list_dd;
                    $scope.demo_list = promise.demo_list;
                    $scope.presentCountgrid = $scope.demo_list.length;
                    $scope.vi =1;

                })
        }



        // edit
        $scope.edibl = {}
        $scope.edibl2 = {}
        $scope.edit = function (bil) {
            $scope.edibl = bil.ismsledM_Id;
            $scope.edib2 = bil.ismslE_Id;
            var data = {
                "ISMSLEDM_Id": $scope.edibl,
                "ISMSLE_Id": $scope.edib2
            }

            apiService.create("Sales_Lead/Edit_details_lead_demo", data).
                then(function (promise) {
                    $scope.ISMSLE_Id = promise.edit_details_lead_demo[0].ismslE_Id;
                    $scope.ISMSLE_LeadName = promise.edit_details_lead_demo[0].ismslE_LeadName;
                    $scope.ISMSLEDM_DemoType = promise.edit_details_lead_demo[0].ismsledM_DemoType;
                    $scope.ISMSLEDM_ContactPerson = promise.edit_details_lead_demo[0].ismsledM_ContactPerson;
                    $scope.HRME_Id = promise.edit_details_lead_demo[0].hrmE_Id;
                    $scope.employeename = promise.edit_details_lead_demo[0].employeename;
                    $scope.ISMSLEDM_DemoDate = new Date(promise.edit_details_lead_demo[0].ismsledM_DemoDate);
                    $scope.ISMSLEDM_DemoAddress = promise.edit_details_lead_demo[0].ismsledM_DemoAddress;
                    $scope.ISMSLEDM_Remarks = promise.edit_details_lead_demo[0].ismsledM_Remarks;
                    $scope.id = promise.edit_details_lead_demo[0].ismsledM_Id;

                    $scope.differentarray = [];
                    $scope.demonstration = [];
                    $scope.differentarray = promise.lead_Demo_Products_list;

                    if ($scope.differentarray.length > 0) {
                        angular.forEach($scope.differentarray, function (tt) {
                            $scope.demonstration.push({
                                ismsmpR_ProductName: tt.ismsmpR_ProductName,
                                ismsmpR_Id: tt.ismsmpR_Id,
                                ISMSLEDMPR_DiscussionPoints: tt.ismsledmpR_DiscussionPoints,

                            });
                        })
                    }
                })
        };
        //get demo response
        $scope.demo_response = function (bil) {
            $scope.edibl = bil.ismsledM_Id;
            $scope.edib2 = bil.ismslE_Id;
            var data = {
                "ISMSLEDM_Id": $scope.edibl,
                "ISMSLE_Id": $scope.edib2
            }
            apiService.create("Sales_Lead/Edit_response_lead_demo", data).then(function (promise) {
                $scope.demo_response_list = promise.demo_response_list;
                $scope.status_demo_master = promise.status_demo_master;
            })
        }
        $scope.all_checkC2 = function () {
            var checklist_temp = $scope.usercheckC2
            angular.forEach($scope.demo_response_list, function (item) {
                item.selectedd2 = checklist_temp
            });
        }
        $scope.usercheckC2 = "";
        $scope.get_evalistt = function () {

            $scope.usercheckC2 = $scope.demo_response_list.every(function (options) {
                return options.selectedd2;
            });
        }
        //update demo response
        $scope.Save_response = function () {
            $scope.empchlist2 = [];


            angular.forEach($scope.demo_response_list, function (emp) {
                if (emp.selectedd2 === true) {
                    $scope.empchlist2.push({ ISMSLEDM_Id: emp.ismsledM_Id, ISMSMPR_Id: emp.ismsmpR_Id, ISMSMST_Id: emp.ISMSMST_Id, ISMSLEDMPR_Remarks: emp.ISMSLEDMPR_Remarks });
                }
                });
   
            var data = {
                product_demo_master_temp2: $scope.empchlist2,
               
            }
            apiService.create("Sales_Lead/Save_response_lead_demo", data).then(function (promise) {
                if (promise.return_status === "Update") {
                    swal('Update Successfully')
                    $state.reload();
                }
                else {
                    swal('Opeartion Failed !')
                    $state.reload();
                }
            });
        }

        //save
        $scope.submitted = false;
        $scope.Savedata_td = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.product_list_tmp = [];
                angular.forEach($scope.demonstration, function (lia) {
                    $scope.product_list_tmp.push({ ISMSMPR_Id: lia.ismsmpR_Id, ISMSLEDMPR_DiscussionPoints: lia.ISMSLEDMPR_DiscussionPoints });
                });

                var data = {
                    "ISMSLEDM_Id": $scope.id,
                    "ISMSLE_Id": $scope.ISMSLE_Id,
                    "ISMSLEDM_DemoType": $scope.ISMSLEDM_DemoType,
                    "ISMSLEDM_ContactPerson": $scope.ISMSLEDM_ContactPerson,
                    "HRME_Id": $scope.HRME_Id,
                    "ISMSLEDM_DemoDate": $scope.ISMSLEDM_DemoDate,
                    "ISMSLEDM_DemoAddress": $scope.ISMSLEDM_DemoAddress,
                    "ISMSLEDM_Remarks": $scope.ISMSLEDM_Remarks,
                    product_demo_master_temp1: $scope.product_list_tmp
                }
                apiService.create("Sales_Lead/SaveEdit_lead_demo", data).
                    then(function (promise) {
                     
                            if (promise.return_status === "Update") {
                                swal('Record Updated Successfully');
                                $state.reload();
                            }
                            else if (promise.return_status === "Add") {
                                swal('Record Saved Successfully');
                                $state.reload();
                            }


                        else {
                            swal('Operation Failed');
                            $state.reload();
                        }
                    })

            }
            else {

                $scope.submitted = true;
            }

        };
        $scope.Clearid = function () {
            $state.reload();
        }
        //Views
        $scope.view = function (ss, vi) {
        
            if (vi == 1) {
                $scope.viewall = 1;
            }
            else {
                $scope.viewall = 0;
            }

            $scope.edibl = ss.ismsledM_Id;
            $scope.edib2 = ss.ismslE_Id;
            var data = {
                "viewall" : $scope.viewall,
                "ISMSLEDM_Id": $scope.edibl,
                "ISMSLE_Id": $scope.edib2
            }
            apiService.create("Sales_Lead/view_lead_demo", data).then(function (promise) {
                $scope.ismslE_LeadName_s = promise.view_lead_demo[0].ismslE_LeadName;
                $scope.ISMSLEDM_ContactPerson_s = promise.view_lead_demo[0].ismsledM_ContactPerson;
                $scope.ISMSLEDM_DemoAddress_s = promise.view_lead_demo[0].ismsledM_DemoAddress;
                $scope.ISMSLEDM_Remarks_s = promise.view_lead_demo[0].ismsledM_Remarks;
                $scope.product_dd_s = promise.product_dd_s;
                if ($scope.viewall == 1) {
                    $scope.demo_response_details = promise.demo_response_details;
                }
                

            });
        }

      
        //deactive
        $scope.deactive = function (bL, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var mgs = "";
            if (bL.ismsmpR_ActiveFlag === false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Product?",
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
                        apiService.create("Sales_Lead/deactivate_pro", bL).
                            then(function (promise) {

                                if (promise.retbool === false) {
                                    swal('Master Product Deactivated Successfully');
                                }
                                else if (promise.retbool === true) {
                                    swal('Master Product Activated Successfully');
                                }


                                $state.reload();
                            });
                    } else {
                        swal("Cancelled");
                        $state.reload();
                    }

                });
        };



        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
         $scope.interacted1 = function (field) {
            return $scope.submitted || field.$dirty;
        };

       
    }

})();



