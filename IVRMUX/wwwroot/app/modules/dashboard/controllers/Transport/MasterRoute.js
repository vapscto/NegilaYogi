
(function () {
    'use strict';
    angular
.module('app')
.controller('MasterRouteController', MasterRouteController)

    MasterRouteController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MasterRouteController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }


        $scope.masterlist = false;
        $scope.sortKey = 'trmR_Id';
        $scope.sortReverse = true;
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;

        $scope.sortKey1 = 'traR_Id';
        $scope.sortReverse1 = true;
        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = paginationformasters;

        $scope.page1 = "page1";
        $scope.page2 = "page2";
        $scope.searchValue = "";
        $scope.searchValue1 = "";

        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("MasterRoute/getdata", pageid).then(function (promise) {
                $scope.getzonearea = promise.getzonearea;
                $scope.rotesdata = promise.routedata;
                if (promise.getroutemater.length > 0) {
                    $scope.masterlist = true;
                    $scope.routelist = promise.getroutemater;
                    $scope.presentCountgrid = $scope.routelist.length;
                    $scope.routearea = promise.routearea;
                    $scope.presentCountgridtabtwo = $scope.routearea.length;
                }
                else {
                    swal("No Records Found");
                }
            })
        }

        $scope.submitted = false;
        $scope.submitted1 = false;

        //$scope.init = function () {
        //    $scope.resetLists();
        //};
        //$scope.init();

        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.areawisedetails) {
                    $scope.areawisedetails[index].trmR_order = Number(index) + 1;

                }


            }
        };
        //--Save Data--//
        $scope.savedata = function () {
            
            if ($scope.myForm.$valid) {
                var data = {
                    "TRMR_Id": $scope.TRMR_Id,
                    "TRMR_RouteName": $scope.TRMR_RouteName,
                    "TRMR_RouteNo": $scope.TRMR_RouteNo,
                    "TRMR_RouteDesc": $scope.TRMR_RouteDesc,
                    // "TRMA_Id": $scope.TRMA_Id,
                    "TRMR_order": $scope.TRMR_order
                }
                apiService.create("MasterRoute/savedata", data).then(function (promise) {
                    if (promise.message == "Add") {
                        if (promise.returnval == true) {
                            swal("Record Saved Successfully");
                        }
                        else {
                            swal("Failed To Save Record");
                        }
                    }
                    else if (promise.message == "Update") {
                        if (promise.returnval == true) {
                            swal("Record Update Successfully");
                        }
                        else {
                            swal("Failed To Update Record");
                        }
                    }
                    else if (promise.message == "Duplicate") {
                        swal("Record Already Exists");
                    }
                    else if (promise.message == "Mapped") {
                        swal("You Can't Update The Record Its Already Mapped");
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //--Clear--//
        $scope.clear = function () {
            $state.reload();
        }

        //--Edit--//
        $scope.edit = function (user) {
            var data = {
                "TRMR_Id": user.trmR_Id
            }
            apiService.create("MasterRoute/edit", data).then(function (promise) {
                if (promise.geteditdata.length > 0) {
                    $scope.TRMR_Id = promise.geteditdata[0].trmR_Id;
                    $scope.TRMR_RouteName = promise.geteditdata[0].trmR_RouteName;
                    $scope.TRMR_RouteNo = promise.geteditdata[0].trmR_RouteNo;
                    $scope.TRMR_RouteDesc = promise.geteditdata[0].trmR_RouteDesc;
                    $scope.TRMA_Id = promise.geteditdata[0].trmA_Id;
                    $scope.TRMR_order = promise.geteditdata[0].trmR_order;
                }
                else {
                }
            })
        }

        //--Active Deactive---//
        $scope.deactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.trmR_ActiveFlg === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";

            }
            else {

                mgs = "Active";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
        function (isConfirm) {
            if (isConfirm) {
                apiService.create("MasterRoute/activedeactive/", user).
                then(function (promise) {
                    if (promise.message != null) {
                        swal(promise.message);
                    }
                    else {
                        if (promise.returnval == true) {
                            swal(confirmmgs + " " + "Successfully");
                            $state.reload();
                        }
                        else {
                            swal(confirmmgs + " " + " Successfully");
                            $state.reload();
                        }
                    }
                })
            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
            $state.reload();
        });
        }

        $scope.toggleAll = function () {
            var toggleStatus = $scope.selectAll;
            angular.forEach($scope.getzonearea, function (role) { role.selected = toggleStatus; });

        }

        $scope.optionToggled = function () {

            $scope.selectAll = $scope.getzonearea.every(function (options) {
                return options.selected;
            });
        }


        $scope.cance = function () {
            $scope.trmR_Id = 0;
            $scope.TRMR_RouteName = "";
            $scope.TRMR_RouteDesc = "";
            $scope.TRMR_RouteNo = "";
            $scope.TRMR_order = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.searchValue = "";
            //var pageid = 1;
            //apiService.getURI("FeeGroup/getalldetails", pageid).
            //    then(function (promise) {

            //        $scope.arrlistchk = promise.activegrpnames;
            //    })
        }

        var name = "";
        $scope.saveroutearea = function () {

            if ($scope.myForm1.$valid) {

                $scope.arealist = [];
                angular.forEach($scope.getzonearea, function (role) {
                    if (!!role.selected) $scope.arealist.push(role);
                })

                if ($scope.arealist.length === 0) {
                    swal('Please Select Area From List Atleast One....!')
                }
                else {

                    var data = {
                        "TRMR_Id": $scope.TRMR_Id,
                         arealistarr: $scope.arealist

                    }
                    apiService.create("MasterRoute/saveroutearea", data).then(function (promise) {
                        if (promise.message == "Add") {
                            if (promise.returnval == true) {
                                swal("Record Saved Successfully");
                                $scope.cance1($scope.arrlistchk);
                                $scope.loaddata();
                            }
                            else {
                                swal("Failed To Save Record");
                                $scope.cance1($scope.arrlistchk);
                                $scope.loaddata();
                            }
                        }
                        else if (promise.message == "Update") {
                            if (promise.returnval == true) {
                                swal("Record Update Successfully");
                                $scope.cance1($scope.arrlistchk);
                                $scope.loaddata();
                            }
                            else {
                                swal("Failed To Update Record");
                                $scope.cance1($scope.arrlistchk);
                                $scope.loaddata();
                            }
                        }
                        else if (promise.message == "Duplicate") {
                            swal("Record Already Exists");
                        }
                        else if (promise.message == "Mapped") {
                            swal("You Can't Update The Record Its Already Mapped");
                        }
                        //$state.reload();
                    })
                }
            }
            else {
                $scope.submitted1 = true;
            }

        };


        $scope.cance1 = function (getzonearea) {
            $scope.TRMR_Id = "";
            for (var i = 0; i < $scope.getzonearea.length; i++) {
                name = $scope.getzonearea[i].selected
                if (name == true) {
                    $scope.getzonearea[i].selected = false;
                }
            }
            $scope.submitted1 = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
            $scope.searchValue1 = "";

        }


        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };


        //--Active Deactive Route Area Mapping---//
        $scope.activedeactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.traR_ActiveFlg === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";

            }
            else {

                mgs = "Active";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("MasterRoute/activedeactiveroutearea/", user).
                            then(function (promise) {
                                if (promise.message != null) {
                                    swal(promise.message);
                                }
                                else {
                                    if (promise.returnval == true) {
                                        swal(confirmmgs + " " + "Successfully");
                                        $state.reload();
                                    }
                                    else {
                                        swal(confirmmgs + " " + " Successfully");
                                        $state.reload();
                                    }
                                }
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                    $state.reload();
                });
        }

        //--Sorting--//     
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
          
            return (angular.lowercase(obj.trmR_RouteName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.trmR_RouteNo)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.trmR_RouteDesc)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.trmA_AreaName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (JSON.stringify(obj.trmR_order)).indexOf($scope.searchValue) >= 0
        }


        $scope.sort1 = function (key) {
            $scope.sortReverse1 = ($scope.sortKey1 == key) ? !$scope.sortReverse1 : $scope.sortReverse1;
            $scope.sortKey1 = key;
        }
       
        $scope.getyearorder = function () {
            var pageid = 2;
            apiService.getURI("MasterRoute/getdata", pageid).then(function (promise) {
                $scope.areawisedetails = {}
                $scope.getzonearea1 = promise.getzonearea;
            })
        }
        $scope.details = false;
       

        $scope.getstudentlistre = function () {
            var data = {
                "TRMA_Id": $scope.yearid1
            }
            apiService.create("MasterRoute/getstudentlistre", data).then(function (promise) {
                if (promise != null) {
                    $scope.areawisedetails = promise.areawisedetails
                    if ($scope.areawisedetails.length > 0) {
                        $scope.areawisedetails = promise.areawisedetails;
                        $scope.details = true;
                    } else {
                        swal("No Records Found");
                        $scope.details = false;
                    }
                } else {
                    swal("No Records Found");
                    $scope.details = false;
                }
            })
        }

        $scope.resetLists = function () {
            $scope.configA = {
                // Changed sorting within list
                onUpdate: function (evt) {
                    var itemEl = evt.item;  // dragged HTMLElement
                    // + indexes from onEnd
                }
            };
        }

        $scope.saveorder = function (newuser2) {
            var data = {
                "temp_masterroute": $scope.areawisedetails
            }
            apiService.create("MasterRoute/saveorder", data).then(function (promise) {
                if (promise != null) {
                    if (promise.returnval == true) {
                        swal("Updated Successfully")
                    }
                    else {
                        swal("Failed To Update")
                    }
                    $state.reload();
                }
            })
        }


        $scope.closere = function () {
            $('#myModalreadmit').modal('hide');
            $state.reload();
        }
    };
})();


