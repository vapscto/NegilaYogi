(function () {
    'use strict';
    angular.module('app').controller('MasterCycleYearMappingController', MasterCycleYearMappingController)
    MasterCycleYearMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter']
    function MasterCycleYearMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter) {

        $scope.searchValue1 = "";
        
        $scope.selacdfryr = false;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 10;
        }

        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;

        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = paginationformasters;

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        var logopath = "";
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        } else {
            logopath = "";
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        //------------------------------------------------------------------------------
        $scope.clgQuotaload = function () {
            var pageid = 2;
            apiService.getURI("MasterCycleYearMapping/getalldetails", pageid).then(function (promise) {

                $scope.getdetailsmastercycle = [];
                $scope.grouptypeListOrder = [];
                $scope.getyearlist = [];
                $scope.getdetails1 = [];
                $scope.getmastercyclemapping = [];

                $scope.gridOptions.data = promise.getmastercycle;
                $scope.getdetailsmastercycle = promise.getmastercycle;
               
                $scope.grouptypeListOrder = promise.getmastercycleorder;
                $scope.getyearlist = promise.getyearlist;
                $scope.getdetails1 = promise.getmastercyclemappingdetails;
                $scope.gridOptions2.data = promise.getmastercyclemappingdetails;
                $scope.getmastercyclemapping = promise.getmastercyclemapping;
            });
        };

        //------------------------------MASTER CYCLE--------------------------- //


        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'ncmacY_NAACCycle', displayName: 'Cycle' },
                { name: 'ncmacY_FromDate', displayName: 'Start Date', type: 'date', cellFilter: 'date:"dd-MM-yyyy"' },
                { name: 'ncmacY_TODate', displayName: 'End Date', type: 'date', cellFilter: 'date:"dd-MM-yyyy"' },
                { name: 'ncmacY_Order', displayName: 'Order' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.editQuota(row.entity);"><md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.ncmacY_ActiveFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.activedeactiveQuota(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.ncmacY_ActiveFlg === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.activedeactiveQuota(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;                
            }
        };


        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "NCMACY_NAACCycle": $scope.NCMACY_NAACCycle,
                    "NCMACY_FromDate": new Date($scope.NCMACY_FromDate).toDateString(),
                    "NCMACY_TODate": new Date($scope.NCMACY_TODate).toDateString(),
                    "NCMACY_Id": $scope.NCMACY_Id
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };

                apiService.create("MasterCycleYearMapping/savedetails", data).then(function (promise) {
                    if (promise.message === "Add") {
                        if (promise.returnval === true) {
                            swal("Record Saved Successfully");
                        }
                        else {
                            swal("Failed To Save Record");
                        }
                    }
                    else if (promise.message === "Update") {
                        if (promise.returnval === true) {
                            swal("Record Updated Successfully");
                        }
                        else {
                            swal("Failed To Update Record");
                        }
                    }
                    else if (promise.message === "Duplicate") {
                        swal("Record Already Exists");
                    }
                    else {
                        swal("Something Went Wrong please contact administrator");
                    }
                    $state.reload();
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.activedeactiveQuota = function (data, SweetAlert) {
            var mgs = "";
            if (data.ncmacY_ActiveFlg === false) {
                mgs = "Activate";
            }
            else {
                mgs = "De-Activate";
            }
            swal({
                title: "Are You Sure?",
                text: "Do You Want to " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("MasterCycleYearMapping/activedeactivedetails", data).then(function (promise) {
                            if (promise.message === "Mapped") {
                                swal("You Can Not Deactive This Record Its Already Mapped");
                            } else if (promise.message === "Error") {
                                swal("Something Went Wrong please contact administrator");
                            } else {
                                if (promise.returnval === true) {
                                    swal("Cycle " + mgs + "Successfully");
                                } else {
                                    swal("Failed To " + mgs + "Cycle");
                                }
                            }
                            $state.reload();
                        });
                    }
                    else {
                        swal("Cancelled");
                        $state.reload();
                    }
                });
        };

        $scope.editQuota = function (id) {
            var data = id;
            apiService.create("MasterCycleYearMapping/editdetails", data).then(function (promise) {
                if (promise.geteditdetails.length > 0) {
                    $scope.selacdfryr = true;
                    $scope.NCMACY_NAACCycle = promise.geteditdetails[0].ncmacY_NAACCycle;
                    $scope.NCMACY_FromDate = new Date(promise.geteditdetails[0].ncmacY_FromDate);
                    $scope.NCMACY_TODate = new Date(promise.geteditdetails[0].ncmacY_TODate);
                    $scope.NCMACY_Id = promise.geteditdetails[0].ncmacY_Id;
                }
            });
        };

        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.grouptypeListOrder) {
                    $scope.grouptypeListOrder[index].ncmacY_Order = Number(index) + 1;
                }
            }
        };

        $scope.getOrder = function (orderarray) {
            var data = {
                order_temp: orderarray
            };

            apiService.create("MasterCycleYearMapping/getOrder", data).then(function (promise) {
                if (promise.returnval === true) {
                    swal("Order Updated Successfully");
                }
                else {
                    swal(" Failed To Update Order");
                }
                $state.reload();
            });
        };       

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
        };      

        //------------------------------ Master Cycle Year Mapping -------------------------- //

        $scope.gridOptions2 = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'ncmacY_NAACCycle', displayName: 'Cycle' },
                { name: 'ncmacY_FromDate', displayName: 'Start Date', type: 'date', cellFilter: 'date:"dd-MM-yyyy"' },
                { name: 'ncmacY_TODate', displayName: 'End Date', type: 'date', cellFilter: 'date:"dd-MM-yyyy"' },               
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.viewdetails(row.entity);" data-toggle="modal" data-target="#myModalmapping"><md-tooltip md-direction="down">View</md-tooltip> <i class="fa fa-eye" ></i></a>' +                        
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };



        $scope.onchangecycle = function () {
            var data = {
                "NCMACY_Id": $scope.NCMACY_IdNew
            };
            apiService.create("MasterCycleYearMapping/onchangecycle", data).then(function (promise) {

                if (promise !== null) {
                    $scope.getyearlist = promise.getyearlist;
                    if ($scope.getyearlist !== null && $scope.getyearlist.length > 0) {
                        $scope.getyearlist = promise.getyearlist;
                    } else {
                        swal("No Records Found");
                    }
                } else {
                    swal("No Records Found");
                }
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.getyearlist.some(function (options) {
                return options.asmayid;
            });
        };

        $scope.submitted1 = false;
        $scope.savedata1 = function () {

            if ($scope.myForm1.$valid) {
                $scope.tempyear = [];
                angular.forEach($scope.getyearlist, function (dd) {
                    if (dd.asmayid) {
                        $scope.tempyear.push({ ASMAY_Id: dd.asmaY_Id });
                    }
                });

                var data = {
                    "NCMACY_Id": $scope.NCMACY_IdNew,
                    "temp_yeardto": $scope.tempyear
                };

                apiService.create("MasterCycleYearMapping/savedetails1", data).then(function (promise) {
                    if (promise.message === "Add") {
                        if (promise.returnval === true) {
                            swal("Record Saved Successfully");
                        }
                        else {
                            swal("Failed To Save Record");
                        }
                    }
                    else if (promise.message === "Update") {
                        if (promise.returnval === true) {
                            swal("Record Updated Successfully");
                        }
                        else {
                            swal("Failed To Update Record");
                        }
                    }
                    else if (promise.message === "Duplicate") {
                        swal("Record Already Exists");
                    }
                    else {
                        swal("Something Went Wrong please contact administrator");
                    }

                    $scope.clgQuotaload();
                    $scope.myForm1.$setPristine();
                    $scope.myForm1.$setUntouched();
                });
            }
            else {
                $scope.submitted1 = true;
            }
        };

        $scope.viewdetails = function (id) {
            var data = id;
            apiService.create("MasterCycleYearMapping/viewdetails", data).then(function (promise) {
                if (promise.getviewdetails.length > 0) {
                    $scope.getviewdetails = promise.getviewdetails;
                }
            });
        };

        $scope.deactivesem = function (id) {
            var data = id;
            var mgs = "";
            if (data.ncmacyyR_ActiveFlg === false) {
                mgs = "Activate";
            }
            else {
                mgs = "De-Activate";
            }
            swal({
                title: "Are You Sure?",
                text: "Do You Want to " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("MasterCycleYearMapping/deactivesem", data).then(function (promise) {
                            if (promise.message === "Mapped") {
                                swal("You Can Not Deactive This Record Its Already Mapped");
                            } else if (promise.message === "Error") {
                                swal("Something Went Wrong please contact administrator");
                            } else {
                                if (promise.returnval === true) {
                                    swal("Record " + mgs + "Successfully");
                                } else {
                                    swal("Failed To " + mgs + " Record");
                                }
                            }
                            $state.reload();
                        });
                    }
                    else {
                        swal("Cancelled");
                        $state.reload();
                    }
                });

        };
        $scope.delete = function (id) {

            var mgs = "Delete";
            var data = id;

            swal({
                title: "Are You Sure?",
                text: "Do You Want to " + mgs + " The Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("MasterCycleYearMapping/delete", data).then(function (promise) {
                            if (promise.message === "Mapped") {
                                swal("You Can Not Deactive This Record Its Already Mapped");
                            } else if (promise.message === "Error") {
                                swal("Something Went Wrong please contact administrator");
                            } else {
                                if (promise.returnval === true) {
                                    swal("Record Deleted Successfully");
                                } else {
                                    swal("Failed To Delete Record");
                                }
                            }
                            $state.reload();
                        });
                    }
                    else {
                        swal("Cancelled");
                        $state.reload();
                    }
                });
        };

        $scope.cancel1 = function () {
            $scope.clgQuotaload();
        };




        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        };

        $scope.search_box1 = function () {
            if ($scope.searchValue1 !== "" || $scope.searchValue1 !== null) {
                $scope.searc_button1 = false;
            }
            else {
                $scope.searc_button1 = true;
            }
        };
        $scope.filterValue01 = function (obj) {

            return angular.lowercase(obj.ncmacY_NAACCycle).indexOf(angular.lowercase($scope.searchValue1)) >= 0 ||
                $filter('date')(obj.ncmacY_FromDate, 'dd-MM-yyyy').indexOf($scope.searchValue1) >= 0 ||
                $filter('date')(obj.ncmacY_TODate, 'dd-MM-yyyy').indexOf($scope.searchValue1) >= 0;


        };
    }
})();