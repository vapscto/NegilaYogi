(function () {
    'use strict';
    angular
        .module('app')
        .controller('Sales_Lead_CreationController', sales_Lead_CreationController)
    sales_Lead_CreationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$stateParams', 'Excel', '$timeout', '$q']
    function sales_Lead_CreationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $stateParams, Excel, $timeout, $q) {
        //======================================================
        var paginationformasters;
        $scope.saveupdate = true;
        $scope.emailbtn = false;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }


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
        $scope.all_check = function () {
            var checkStatus2 = $scope.usercheck;
            angular.forEach($scope.product_dd, function (itm) {
                itm.selectedd2 = checkStatus2;
            });
        };

        //=====================================

        $scope.isOptionsRequired = function () {

            return !$scope.product_dd.some(function (sec) {
                return sec.selectedd2;
            });
        }
        $scope.select_state = function (ss) {
            var id = ss;
            apiService.getURI("Sales_Lead/select_state", id).then(function (promise) {
                $scope.state_dd = promise.state_dd;
            });
        };
        //=====================================
        $scope.load_date = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("Sales_Lead/load_all_lead", pageid).then(function (promise) {
                $scope.lead_list = promise.lead_list;
                $scope.presentCountgrid = promise.lead_list.length;
                $scope.category_dd = promise.category_dd;
                $scope.source_dd = promise.source_dd;
                $scope.product_dd = promise.product_dd;
                $scope.status_dd = promise.status_dd;
                $scope.country_dd = promise.country_dd;

            });
        };

        $scope.onchangesource = function (sourceid) {
            apiService.getURI("Sales_Lead/checkemailtemplet", sourceid).then(function (promise) {
                $scope.emailbtn = promise.returnvalue;
            });
        };

        //$scope.saveLead = function () {
        //    swal({
        //        title: "Are you sure?",
        //        text: "Do you want to save the data?",
        //        type: "warning",
        //        showCancelButton: true,
        //        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,Save it",
        //        cancelButtonText: "Cancel",
        //        closeOnConfirm: true,
        //        closeOnCancel: true
        //    },
        //        function (isConfirm) {
        //            if (isConfirm) {
        //                $('#myModalswal').modal('show');
        //            }
        //            else {
        //                swal("Save data Cancelled!!!");
        //            }
        //        });
        //};



        $scope.saveLeadnew = function (trfal) {

            $scope.product_list = [];
            angular.forEach($scope.product_dd, function (cls) {
                if (cls.selectedd2 === true) {
                    $scope.product_list.push(cls);
                }
            });
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    product_list_save: $scope.product_list,
                    "ISMSLE_Id": $scope.id,
                    "ISMSLE_LeadName": $scope.ISMSLE_LeadName,
                    "ISMSLE_LeadCode": $scope.ISMSLE_LeadCode,
                    "ISMSLE_ContactPerson": $scope.ISMSLE_ContactPerson,
                    "ISMSLE_LeadAddress1": $scope.ISMSLE_LeadAddress1,
                    "ISMSLE_LeadAddress2": $scope.ISMSLE_LeadAddress2,
                    "ISMSLE_LeadAddress3": $scope.ISMSLE_LeadAddress3,
                    "ISMSLE_Pincode": $scope.ISMSLE_Pincode,
                    "ISMSLE_ContactNo": $scope.ISMSLE_ContactNo,
                    "IVRMMC_Id": $scope.IVRMMC_Id,
                    "ISMSMST_Id": $scope.ISMSMST_Id,
                    "ISMSLE_EmailId": $scope.ISMSLE_EmailId,
                    "ISMSMCA_Id": $scope.ISMSMCA_Id,
                    "ISMSMSO_Id": $scope.ISMSMSO_Id,
                    "ISMSLE_Reference": $scope.ISMSLE_Reference,
                    "ISMSLE_ContactDesignation": $scope.ISMSLE_ContactDesignation,
                    "IVRMMS_Id": $scope.IVRMMS_Id,
                    "ISMSLE_StudentStrength": $scope.ISMSLE_StudentStrength,
                    "ISMSLE_StaffStrength": $scope.ISMSLE_StaffStrength,
                    "ISMSLE_NoOfInstitutions": $scope.ISMSLE_NoOfInstitutions,
                    "ISMSLE_Remarks": $scope.ISMSLE_Remarks,
                    "ISMSLE_VisitedDate": new Date($scope.ISMSLE_VisitedDate).toDateString(),
                    "sendemail": trfal
                };
                var swaltext = "";
                var btntext = "";
                if ($scope.saveupdate === true) {
                    if (trfal === true) {
                        swaltext = "Do you want to save and send mail?";
                        btntext = "Yes,Save and send mail";
                    }
                    else {
                        swaltext = "Do you want to save without mail?";
                        btntext = "Yes,Save without mail";
                    }
                }
                else {
                    if (trfal === true) {
                        swaltext = "Do you want to update data and send mail?";
                        btntext = "Yes,Update and send mail";
                    }
                    else {
                        swaltext = "Do you want to update data?";
                        btntext = "Yes,Update data";
                    }
                }

                swal({
                    title: "Are you sure?",
                    text: swaltext,
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: btntext,
                    cancelButtonText: "Cancel",
                    closeOnConfirm: true,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            apiService.create("Sales_Lead/Save_Edit_SaleLead", data).then(function (promise) {
                                if (promise.return_value === "Add") {
                                    swal('Record Saved Successfully');
                                    //$('#myModalswal').modal('hide');
                                    //$('.modal-backdrop').remove();
                                    $state.reload();
                                }
                                else if (promise.return_value === "Update") {
                                    swal('Record Update Successfully');
                                    //$('#myModalswal').modal('hide');
                                    //$('.modal-backdrop').remove();
                                    $state.reload();
                                }
                                else {
                                    swal('Operation Failed!');
                                    //$('#myModalswal').modal('hide');
                                    //$('.modal-backdrop').remove();
                                    $state.reload();
                                }
                            });
                            //$('#myModalswal').modal('show');
                        }
                        else {
                            swal("Save data Cancelled!!!");
                        }
                    });
            }
            else {

                $scope.submitted = true;
            }
        };
        $scope.swalcancel = function () {
            //$('#myModalswal').modal('hide');
            //$('.modal-backdrop').remove();
            //swal("Save Data Cancelled!");
            $state.reload();
        };
        $scope.edit = function (ss) {
            $scope.saveupdate = false;
            var pageid = ss.ismslE_Id
            apiService.getURI("Sales_Lead/Sales_lead_edit", pageid).then(function (promise) {
                $scope.ISMSLE_LeadName = promise.sales_lead_edit[0].ismslE_LeadName;
                $scope.ISMSLE_LeadCode = promise.sales_lead_edit[0].ismslE_LeadCode;
                $scope.ISMSLE_ContactPerson = promise.sales_lead_edit[0].ismslE_ContactPerson;
                $scope.ISMSLE_LeadAddress1 = promise.sales_lead_edit[0].ismslE_LeadAddress1;
                $scope.ISMSLE_LeadAddress2 = promise.sales_lead_edit[0].ismslE_LeadAddress2;
                $scope.ISMSLE_LeadAddress3 = promise.sales_lead_edit[0].ismslE_LeadAddress3;
                $scope.ISMSLE_Pincode = promise.sales_lead_edit[0].ismslE_Pincode;
                $scope.ISMSLE_ContactNo = promise.sales_lead_edit[0].ismslE_ContactNo;
                $scope.ismsmsT_StatusName = promise.sales_lead_edit[0].ismsmsT_StatusName;
                $scope.ISMSMST_Id = promise.sales_lead_edit[0].ismsmsT_Id;
                $scope.ISMSLE_EmailId = promise.sales_lead_edit[0].ismslE_EmailId;
                $scope.ISMSMCA_Id = promise.sales_lead_edit[0].ismsmcA_Id;
                $scope.ismsmcA_CategoryName = promise.sales_lead_edit[0].ismsmcA_CategoryName;
                $scope.ISMSMSO_Id = promise.sales_lead_edit[0].ismsmsO_Id;
                $scope.ismsmsO_SourceName = promise.sales_lead_edit[0].ismsmsO_SourceName;
                $scope.ISMSLE_Reference = promise.sales_lead_edit[0].ismslE_Reference;
                $scope.ISMSLE_ContactDesignation = promise.sales_lead_edit[0].ismslE_ContactDesignation;
                $scope.IVRMMS_Id = promise.sales_lead_edit[0].ivrmmS_Id;
                $scope.ivrmmS_Name = promise.sales_lead_edit[0].ivrmmS_Name;
                $scope.ISMSLE_StudentStrength = promise.sales_lead_edit[0].ismslE_StudentStrength;
                $scope.ISMSLE_StaffStrength = promise.sales_lead_edit[0].ismslE_StaffStrength;
                $scope.ISMSLE_NoOfInstitutions = promise.sales_lead_edit[0].ismslE_NoOfInstitutions;
                $scope.ISMSLE_Remarkspromise = promise.sales_lead_edit[0].ismslE_Remarkspromise;
                $scope.ivrmmC_CountryName = promise.sales_lead_edit[0].ivrmmC_CountryName;
                $scope.ISMSLE_Remarks = promise.sales_lead_edit[0].ismslE_Remarks;
                $scope.ISMSLE_VisitedDate = new Date(promise.sales_lead_edit[0].ismslE_VisitedDate);
                //$scope.IVRMMC_Id = promise.sales_lead_edit[0].ivrmmC_Id;
                $scope.id = promise.sales_lead_edit[0].ismslE_Id;
                $scope.product_dd_temp = [];
                $scope.product_dd_temp = promise.sales_lead_edit_product_dd;
                $scope.product_dd = promise.product_dd;
                $scope.state_dd = promise.state_dd;
                $scope.IVRMMC_Id = promise.ivrmmC_Id;
                $scope.IVRMMS_Id = promise.ivrmmS_Id;
                $scope.emailbtn = promise.returnvalue;
                //$scope.country_dd = promise.country_dd;



                if ($scope.product_dd_temp.length > 0) {
                    angular.forEach($scope.product_dd_temp, function (ss) {
                        angular.forEach($scope.product_dd, function (qq) {
                            if (ss.ismsmpR_Id === qq.ismsmpR_Id) {
                                qq.selectedd2 = true;
                            }
                        });
                    });
                }
            });
        };
        $scope.view = function (ss) {
            var pageid = ss.ismslE_Id
            apiService.getURI("Sales_Lead/Sales_lead_edit", pageid).then(function (promise) {

                $scope.ISMSLE_LeadName_s = promise.sales_lead_edit[0].ismslE_LeadName;
                $scope.ISMSLE_LeadName_Code_s = promise.sales_lead_edit[0].ismslE_LeadCode;
                $scope.ISMSLE_ContactPerson_s = promise.sales_lead_edit[0].ismslE_ContactPerson;
                $scope.ISMSLE_LeadAddress_s = promise.sales_lead_edit[0].ismslE_LeadAddress1;
                $scope.ISMSLE_ContactNo_s = promise.sales_lead_edit[0].ismslE_ContactNo;
                $scope.ismsmsT_StatusName_s = promise.sales_lead_edit[0].ismsmsT_StatusName;
                $scope.ISMSMST_Id_s = promise.sales_lead_edit[0].ismsmsT_Id;
                $scope.ISMSLE_EmailId_s = promise.sales_lead_edit[0].ismslE_EmailId;
                $scope.ISMSMCA_Id_s = promise.sales_lead_edit[0].ismsmcA_Id;
                $scope.ismsmcA_CategoryName_s = promise.sales_lead_edit[0].ismsmcA_CategoryName;
                $scope.ISMSMSO_Id_s = promise.sales_lead_edit[0].ismsmsO_Id;
                $scope.ismsmsO_SourceName_s = promise.sales_lead_edit[0].ismsmsO_SourceName;
                $scope.ISMSLE_Reference_s = promise.sales_lead_edit[0].ismslE_Reference;
                $scope.ISMSLE_ContactDesignation_s = promise.sales_lead_edit[0].ismslE_ContactDesignation;
                $scope.IVRMMS_Id_s = promise.sales_lead_edit[0].ivrmmS_Id;
                $scope.ivrmmS_Name_s = promise.sales_lead_edit[0].ivrmmS_Name;
                $scope.ISMSLE_StudentStrength_s = promise.sales_lead_edit[0].ismslE_StudentStrength;
                $scope.ISMSLE_StaffStrength_s = promise.sales_lead_edit[0].ismslE_StaffStrength;
                $scope.ISMSLE_NoOfInstitutions_s = promise.sales_lead_edit[0].ismslE_NoOfInstitutions;
                $scope.ISMSLE_Remarkspromise_s = promise.sales_lead_edit[0].ismslE_Remarkspromise;
                $scope.ivrmmC_CountryName_s = promise.sales_lead_edit[0].ivrmmC_CountryName;
                $scope.ISMSLE_Remarks_s = promise.sales_lead_edit[0].ismslE_Remarks;
                $scope.IVRMMC_Id_s = promise.sales_lead_edit[0].ivrmmC_Id;
                $scope.id_s = promise.sales_lead_edit[0].ismslE_Id;
                $scope.product_dd_s = promise.sales_lead_edit_product_dd_product;

            })
        }
        $scope.deactiv_prde = function (bL, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var mgs = "";
            if (bL.ismslepR_ActiveFlag === false) {
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
                        apiService.create("Sales_Lead/deactiv_prde", bL).
                            then(function (promise) {

                                if (promise.retbool === false) {
                                    swal('Sales Product  Deactivated Successfully');
                                }
                                else if (promise.retbool === true) {
                                    swal('Sales Product Activated Successfully');
                                }


                                $state.reload();
                            });
                    } else {
                        swal("Cancelled");

                    }

                });
        };

        $scope.cancel = function () {
            $state.reload();
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        //======================
    }
})();