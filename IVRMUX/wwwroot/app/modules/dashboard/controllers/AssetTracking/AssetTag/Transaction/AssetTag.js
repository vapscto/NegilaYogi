
(function () {
    'use strict';
    angular
        .module('app')
        .controller('AssetTagController', AssetTagController);
    AssetTagController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function AssetTagController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.obj = {};
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //========================================== PAGE LOAD

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage = 10;
            $scope.itemsPerPage1 = paginationformasters;
            $scope.search = "";          
            var pageid = 2;
            apiService.getURI("AssetTag/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_store = promise.get_store;
                    $scope.get_Assetstag = promise.get_Assetstag;
                    $scope.presentCountgrid1 = $scope.get_Assetstag.length;
                });
        };
        //========================================= STORE CHANGE
        $scope.onstorechange = function () {
            var data = {
                "INVMST_Id": $scope.invmsT_Id
            };
            apiService.create("AssetTag/getdata", data).
                then(function (promise) {
                    $scope.get_tagdata = promise.get_tagdata;
                    $scope.presentCountgrid = $scope.get_tagdata.length;
                });
        };
        //======================================== Check Duplicate        
        $scope.checkduplicate = function (tagdata) {
            $scope.tag_list = [];
            angular.forEach(tagdata, function (tg) {
                if (tg.xyz) {
                    if ($scope.tag_list.length === 0) {
                        $scope.tag_list.push(tg);
                    }
                    else if ($scope.tag_list.length > 0) {
                        angular.forEach($scope.tag_list, function (tl) {                           
                                if (tg.INVMI_Id === tl.INVMI_Id) {
                                    if (tg.invaaT_AssetId === tl.invaaT_AssetId) {
                                        swal("Tag already exist....!!");
                                        tg.invaaT_AssetId = "";
                                    }
                                }                            
                        });
                    }
                    angular.forEach($scope.get_Assetstag, function (at) {                     
                            if (at.invmI_Id === tg.INVMI_Id) {
                                if (at.invaaT_AssetId === tg.invaaT_AssetId) {
                                    swal("Tag already assigned....!!");
                                    tg.invaaT_AssetId = "";
                                }
                            }                       

                    });
                }
            });
        };
        //======================================= Grid Check box Selection
        $scope.toggleAll = function () {
            angular.forEach($scope.get_tagdata, function (subj) {
                subj.xyz = $scope.all;
            });
        };
        $scope.optionToggled = function () {
            $scope.all = $scope.get_tagdata.every(function (itm) { return itm.xyz; });
        };
        //=====================================  SAVE DATA
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                $scope.tagckdArray = [];

                angular.forEach($scope.get_tagdata, function (tag) {
                    if (tag.xyz) {
                        $scope.tagckdArray.push({
                            invmsT_Id: $scope.invmsT_Id, invmI_Id: tag.INVMI_Id, invaaT_AssetId: tag.invaaT_AssetId, invaaT_AssetDescription: tag.invaaT_AssetDescription, invaaT_ModelNo: tag.invaaT_ModelNo,
                            invaaT_SerialNo: tag.invaaT_SerialNo, invaaT_SKU: tag.invaaT_SKU, invaaT_ManufacturedDate: tag.invaaT_ManufacturedDate, invaaT_PurchaseDate: tag.invaaT_PurchaseDate,
                            invaaT_WarantyPeriod: tag.invaaT_WarantyPeriod, invaaT_WarantyExpiryDate: tag.invaaT_WarantyExpiryDate, invaaT_UnderAMCFlg: tag.invaaT_UnderAMCFlg,
                            invaaT_AMCExpiryDate: tag.invaaT_AMCExpiryDate
                        });
                    }
                });
                if ($scope.tagckdArray.length > 0) {
                    var data = {
                        "INVAAT_Id": $scope.invaaT_Id,
                        "tagckdArray": $scope.tagckdArray
                    };
                }
                else {
                    swal("Select Atleast One checkbox....!!");
                }

                apiService.create("AssetTag/savedata", data).then(function (promise) {

                    if (promise.returnval === true) {
                        if (promise.invaaT_Id === 0 || promise.invaaT_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.invaaT_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invaaT_Id === 0 || promise.invaaT_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invaaT_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $state.reload();
                });
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
        };
        $scope.deactive = function (item) {
            $scope.INVAAT_Id = item.invaaT_Id;
            var dystring = "";
            if (item.invaaT_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invaaT_ActiveFlg === false) {
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
                        apiService.create("AssetTag/deactive", item).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        };
        $scope.edit = function (item) {
            $scope.invmsT_Id = item.invmsT_Id;
            $scope.invaaT_Id = item.invaaT_Id;
            $scope.edits = true;
            $scope.get_tagdata = [];
            $scope.taglist = [];
            $scope.taglist.push(item);
            angular.forEach($scope.taglist, function (li) {
                $scope.get_tagdata.push({
                    INVMI_Id: li.invmI_Id, INVMI_ItemName: li.invmI_ItemName, invaaT_AssetId: li.invaaT_AssetId,
                    invaaT_AssetDescription: li.invaaT_AssetDescription, invaaT_ModelNo: li.invaaT_ModelNo, invaaT_SerialNo: li.invaaT_SerialNo,
                    invaaT_SKU: li.invaaT_SKU, invaaT_ManufacturedDate: new Date(li.invaaT_ManufacturedDate), invaaT_PurchaseDate: new Date(li.invaaT_PurchaseDate),
                    invaaT_WarantyPeriod: li.invaaT_WarantyPeriod, invaaT_WarantyExpiryDate: new Date(li.invaaT_WarantyExpiryDate),
                    invaaT_UnderAMCFlg: li.invaaT_UnerAMCFlg, invaaT_AMCExpiryDate: new Date(li.invaaT_AMCExpiryDate), xyz: true
                });

            });

        };

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
        $scope.searchValue1 = '';

     
    }
})();