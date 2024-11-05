
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_QuotationController', INV_QuotationController);
    INV_QuotationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', '$q', 'apiService', '$http', 'superCache']
    function INV_QuotationController($rootScope, $scope, $state, $location, Flash, appSettings, $q, apiService, $http, superCache) {

        var date = new Date();
        $scope.invmpI_Doc_Date = date;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        //====================== Auto Generated Number
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transnumconfigsettings.length; i++) {
            if (transnumconfigsettings.length > 0) {
                if (transnumconfigsettings[i].imN_Flag == "INVQUOTATION") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }
        }

        //====================================== Page Load
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("INV_Quotation/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_piNo = promise.get_piNo;
                    $scope.get_supplier = promise.get_supplier;
                    $scope.get_Quotation = promise.get_Quotation;
                    $scope.presentCountgrid = $scope.get_Quotation.length;
                })
        };

        //===================================== Upload  
        $scope.uploadFile = function (input, document) {
            $scope.UploadFile = input.files;
            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" || input.files[0].type == "image/png" || input.files[0].type == "image/jpg" || input.files[0].type === "application/pdf" ||
                    input.files[0].type === "application/msword" || input.files[0].type === "application/vnd.ms-excel" && input.files[0].size <= 2097152) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#blahD')
                            .attr('src', e.target.result)
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofile();
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        }

        function Uploadprofile() {
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadFile.length; i++) {

                formData.append("File", $scope.UploadFile[i]);
                $scope.file_detail = $scope.UploadFile[0].name;
            }
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_Noticefiles", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    $scope.quotation = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }
        $scope.remove_file = function () {

            $scope.file_detail = "";
            $scope.quotation = "";
        }
        //===================================== Radio Change
        $scope.onradiochange = function () {
            $scope.submitted = false;
            $scope.get_supplier = "";
            $scope.invmS_SupplierName = "";
            $scope.invmsQ_SupplierContactNo = "";
            $scope.invmsQ_SupplierEmailId = "";
            $scope.loaddata();
        }

        //===================================== PI Change
        $scope.onpichange = function (itemid) {
            $scope.get_indentDetail = "";
            var item_id = itemid.invmpI_Id;
            var data = {
                "INVMPI_Id": item_id
            }
            apiService.create("INV_Quotation/getpiDetail", data).
                then(function (promise) {
                    $scope.get_pidetails = promise.get_pidetails;
                    $scope.pi_list = [];
                    angular.forEach($scope.get_pidetails, function (idn) {
                        $scope.pi_list.push({
                            invmpI_Id: idn.invmpI_Id, invtpI_Id: idn.invtpI_Id, invmI_Id: idn.invmI_Id,
                            invmuoM_Id: idn.invmuoM_Id, invmI_ItemName: idn.invmI_ItemName, invmuoM_UOMName: idn.invmuoM_UOMName,
                            invtpI_PIQty: idn.invtpI_PIQty, invtsQ_QuotedRate: idn.invtpI_PIUnitRate, invtsQ_NegotiatedRate: idn.invtpI_PIUnitRate
                        });
                    })
                    var a = 0;
                    var b = 0;
                    angular.forEach($scope.pi_list, function (obj) {
                        a += parseFloat(obj.invtsQ_QuotedRate);
                        b += parseFloat(obj.invtsQ_NegotiatedRate);
                    })

                    var qtamt = a;
                    $scope.invmsQ_TotalQuotedRate = qtamt;
                    $scope.invmsQ_TotalQuotedRate = parseFloat($scope.invmsQ_TotalQuotedRate);
                    $scope.invmsQ_TotalQuotedRate = $scope.invmsQ_TotalQuotedRate.toFixed(2);
                    var ntamt = b;
                    $scope.invmsQ_NegotiatedRate = ntamt;
                    $scope.invmsQ_NegotiatedRate = parseFloat($scope.invmsQ_NegotiatedRate);
                    $scope.invmsQ_NegotiatedRate = $scope.invmsQ_NegotiatedRate.toFixed(2);
                })
        }
        //================== Count Amount
        $scope.countQAmt = function (qobj) {

            var b = 0;
            angular.forEach($scope.pi_list, function (obj) {
                b += parseFloat(obj.invtsQ_NegotiatedRate);
            })
            var ntamt = b;
            $scope.invmsQ_NegotiatedRate = ntamt;
            $scope.invmsQ_NegotiatedRate = parseFloat($scope.invmsQ_NegotiatedRate);
            $scope.invmsQ_NegotiatedRate = $scope.invmsQ_NegotiatedRate.toFixed(2);
        }
        //===================================== SAVE DATA
        $scope.savedata = function () {
            $scope.submitted = true;
            var data = {};
            if ($scope.myForm.$valid) {
                $scope.arrayQuatation = [];
                angular.forEach($scope.pi_list, function (idn) {
                    $scope.arrayQuatation.push({
                        invmpI_Id: idn.invmpI_Id, invmI_Id: idn.invmI_Id, invmuoM_Id: idn.invmuoM_Id, invmI_ItemName: idn.invmI_ItemName, invmuoM_UOMName: idn.invmuoM_UOMName, invtsQ_QuotedRate: idn.invtsQ_QuotedRate, invtsQ_NegotiatedRate: idn.invtsQ_NegotiatedRate
                    });
                })
                var att_file = "";
                var att_file11 = "";
                if ($scope.quotation != undefined || $scope.quotation != "") {

                    $scope.filedoc = [];
                    $scope.filedoc = [$scope.quotation];
                    if ($scope.filedoc.length > 0) {
                        for (var i = 0; i < $scope.filedoc.length; i++) {
                            att_file = $scope.filedoc[0];
                        }
                    }
                    if (att_file != undefined && att_file != '' && att_file != null) {
                        att_file11 = att_file.toString();
                    }
                   
                }
                if (att_file11 != "" && att_file11 != null && att_file11 != undefined) {

             
                if ($scope.supplierflag == "S") {
                    data = {
                        "INVMPI_Id": $scope.obj.invmpI_Id.invmpI_Id,
                        "INVMSQ_SupplierName": $scope.obj.invmS_Id.invmS_SupplierName,
                        "INVMSQ_SupplierContactNo": $scope.obj.invmS_Id.invmS_SupplierConatctNo,
                        "INVMSQ_SupplierEmailId": $scope.obj.invmS_Id.invmS_EmailId,
                        "INVMSQ_Quotation": att_file11,
                        "INVMSQ_TotalQuotedRate": $scope.invmsQ_TotalQuotedRate,
                        "INVMSQ_NegotiatedRate": $scope.invmsQ_NegotiatedRate,
                        "INVMSQ_Remarks": $scope.invmsQ_Remarks,
                        "arrayQuatation": $scope.arrayQuatation,
                        "INVMSQ_Id": $scope.invmsQ_Id,
                        transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
                    }
                }
                else {
                    data = {
                        "INVMPI_Id": $scope.obj.invmpI_Id.invmpI_Id,
                        "INVMSQ_SupplierName": $scope.invmsQ_SupplierName,
                        "INVMSQ_SupplierContactNo": $scope.invmsQ_SupplierContactNo,
                        "INVMSQ_SupplierEmailId": $scope.invmsQ_SupplierEmailId,
                        "INVMSQ_Quotation": att_file11,
                        "INVMSQ_TotalQuotedRate": $scope.invmsQ_TotalQuotedRate,
                        "INVMSQ_NegotiatedRate": $scope.invmsQ_NegotiatedRate,
                        "INVMSQ_Remarks": $scope.invmsQ_Remarks,
                        "arrayQuatation": $scope.arrayQuatation,
                        "INVMSQ_Id": $scope.invmsQ_Id,
                        transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
                    }
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("INV_Quotation/savedetails", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.invmsQ_Id == 0 || promise.invmsQ_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.invmsQ_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invmsQ_Id == 0 || promise.invmsQ_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invmsQ_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $state.reload();
                })
                }
                else {
                    swal('Upload Quotation!!!!....')
                }
                }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        }

        $scope.deactiveM = function (item, SweetAlert) {
            $scope.invmsQ_Id = item.invmsQ_Id;
            var dystring = "";
            if (item.invmsQ_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.invmsQ_ActiveFlg == false) {
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
                        apiService.create("INV_Quotation/deactiveM", item).
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
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }

        $scope.deactive = function (item, SweetAlert) {
            $scope.invtsQ_Id = item.invtsQ_Id;
            var dystring = "";
            if (item.invtsQ_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.invtsQ_ActiveFlg == false) {
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
                        apiService.create("INV_Quotation/deactive", item).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $scope.onformclick(item);
                                $scope.loaddata();

                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }

        $scope.edit = function (item) {
            $scope.pi_list = [];
            $scope.invmsQ_Id = item.invmsQ_Id;
            $scope.edits = true;
            var qt_id = item.invmsQ_Id;
            var data = {
                "INVMSQ_Id": qt_id
            }
            apiService.create("INV_Quotation/getquotationdetails", data).
                then(function (promise) {
                    $scope.editquotation = promise.get_quotationdetails;
                    angular.forEach($scope.editquotation, function (objQ) {
                        $scope.pi_list.push(objQ);
                    })
                })
        }



        $scope.onformclick = function (itemid) {
            $scope.get_quotationdetails = "";
            var item_id = itemid.invmsQ_Id;
            var data = {
                "INVMSQ_Id": item_id
            }
            apiService.create("INV_Quotation/getquotationdetails", data).
                then(function (promise) {
                    $scope.get_quotationdetails = promise.get_quotationdetails;
                    $scope.qtnum = $scope.get_quotationdetails[0].invmsQ_QuotationNo;
                    $scope.supname = $scope.get_quotationdetails[0].invmsQ_SupplierName;
                    $scope.supcontactno = $scope.get_quotationdetails[0].invmsQ_SupplierContactNo;
                    $scope.supemail = $scope.get_quotationdetails[0].invmsQ_SupplierEmailId;
                })
        }

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';

    }
})();