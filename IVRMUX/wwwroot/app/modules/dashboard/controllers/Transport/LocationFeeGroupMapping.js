
(function () {
    'use strict';
    angular
.module('app')
        .controller('LocationFeeGroupMappingController', LocationFeeGroupMappingController)

    LocationFeeGroupMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$stateParams']
    function LocationFeeGroupMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $stateParams) {

        var paginationformasters=10;
        //var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        //if (ivrmcofigsettings.length > 0) {
        //    paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        //}
       // paginationformasters = 10;
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 10;
        $scope.masterlist = false;
        $scope.sortKey = 'trlfM_Id';
        $scope.sortReverse = true;
        $scope.masterlist = false;
        $scope.submitted1 = false;
        $scope.savedisable = true;
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.disabled = true;
                if (privlgs[i].ivrmirP_AddFlag == true) {
                    $scope.saveflg = true;
                    $scope.savebtn = true;

                }
                else {
                    $scope.saveflg = false;
                    $scope.savebtn = false;
                }
                if (privlgs[i].ivrmirP_UpdateFlag == true) {
                    $scope.editflag = true;
                    $scope.editbtn = true;

                }
                else {
                    $scope.editflag = false;
                    $scope.editbtn = false;
                }
                if (privlgs[i].ivrmirP_DeleteFlag == true) {
                    $scope.deactiveflag = true;
                    $scope.deletebtn = true;
                }
                else {
                    $scope.deactiveflag = false;
                    $scope.deletebtn = false;
                }


            }
        }
        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("LocationFeeGroupMapping/getdata/", pageid).then(function (promise) {
                if (promise != null) {
                    $scope.yearlist = promise.yearlist;
                    $scope.locationlist = promise.locationlist;
                    $scope.grouplist = promise.grouplist;
                 
                   // $scope.ASMAY_Id1 = promise.asmaY_Id;

                    $scope.arrlistnew = promise.yearlist;
                    $scope.location = promise.locationlist;
                    $scope.students = promise.studentdata;
                    $scope.griddata = promise.griddata;
                    if (promise.griddata != null) {
                        $scope.masterlist = true;
                    }
                  
                    // $scope.locationdetails = promise.locationdetails;
                    $scope.routedetailsarea = promise.routedetailsarea;
                }
                else {
                    swal("No Records Found");
                }
            })
        }
        $scope.searchValue = '';
        $scope.savedata = function () {
            
            var TRLFM_WayFlag = "";
            if ($scope.TRLFM_WayFlag == 'OneWay') {
                TRLFM_WayFlag = 'OneWay';
            }

          else {
                TRLFM_WayFlag = 'TwoWay';
            }

            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id1,
                    "TRML_Id": $scope.TRML_Id,
                    "FMG_Id": $scope.FMG_Id,
                    "TRLFM_Id": $scope.TRLFM_Id,
                    "TRLFM_WayFlag": TRLFM_WayFlag,
                }
                apiService.create("LocationFeeGroupMapping/savedata", data).then(function (promise) {
                    if (promise != null) {
                        if (promise.message == "Add") {
                            if (promise.retrval == true) {
                                swal("Record Saved Successfully");
                            }
                            else {
                                swal("Failed To Save Record");
                            }
                        }
                        else if (promise.message == "Update") {
                            if (promise.retrval == true) {
                                swal("Record Update Successfully");
                            }
                            else {
                                swal("Failed To Update Record");
                            }
                        }
                        else if (promise.message == "Duplicate") {
                            swal("Record Already Exists");
                        }
                    }
                    else {

                    }
                   // $scope.close();
                   $state.reload();
                })
            } else {
                $scope.submitted = true;
            }
        }


      

        $scope.close1 = function () {
       
                $scope.TRMLAMT_OneWayAmount = "";
            $scope.TRMLAMT_TwoWayAmount = "";
            var pageid = 2;
            apiService.getURI("LocationFeeGroupMapping/getdata/", pageid).then(function (promise) {
                if (promise != null) {
                    $scope.yearlist = promise.yearlist;
                    $scope.locationlist = promise.locationlist;
                    $scope.grouplist = promise.grouplist;
                    

                    $scope.arrlistnew = promise.yearlist;
                    $scope.location = promise.locationlist;
                    $scope.students = promise.studentdata;
                    $scope.griddata = promise.griddata;
                    if (promise.griddata != null) {
                        $scope.masterlist = true;
                    }

                    $scope.routedetailsarea = promise.routedetailsarea;
                }
                else {
                    swal("No Records Found");
                }
            })
        }
        $scope.edit = function (user) {
            var data = {
                "TRLFM_Id": user.trlfM_Id
            }
            apiService.create("LocationFeeGroupMapping/geteditdata", data).then(function (Promise) {
                if (Promise != null) {
                    debugger;
                    $scope.ASMAY_Id1 = Promise.editdatadetails[0].asmaY_Id;
                    $scope.TRML_Id = Promise.editdatadetails[0].trmL_Id;
                    $scope.FMG_Id = Promise.editdatadetails[0].fmG_Id;
                    $scope.TRLFM_Id = Promise.editdatadetails[0].trlfM_Id;
                    if (Promise.editdatadetails[0].trlfM_WayFlag == "OneWay") {
                        $scope.TRLFM_WayFlag = 'OneWay';
                    }

                    if (Promise.editdatadetails[0].trlfM_WayFlag == "TwoWay") {
                        $scope.TRLFM_WayFlag = 'TwoWay';
                    }

                }
            })
        }

        $scope.checkboxchcked = [];
        $scope.editClassList = [];

        $scope.CheckedClassName = function (data) {
            if ($scope.checkboxchcked.indexOf(data) === -1) {
                $scope.checkboxchcked.push(data);
            }
            else {
                $scope.checkboxchcked.splice($scope.checkboxchcked.indexOf(data), 1);
            }
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.isOptionsRequired = function () {
            return !$scope.locationdetails.some(function (options) {
                return options.Selected;
            });
        }

       $scope.clear = function () {
            $state.reload();
        }
        $scope.cance1 = function () {
            $state.reload();
        }
        
        $scope.getlocations = function () {
            var data = {
                "TRMR_Id": $scope.TRMR_Id
            }
            apiService.create("RouteLocationMapping/getlocations/", data).then(function (promise) {
                if (promise != null) {
                    if (promise.locationdetails.length > 0) {
                        $scope.locationdetails = promise.locationdetails;
                    }
                    else {
                        swal("All Locations Is Mapped For This Route");
                    }
                }
                else {
                    swal("No Records Found");
                }
            })
        }

        $scope.getlocationsarea = function () {
            var data = {
                "TRMA_Id":$scope.TRMA_Id
            }
            apiService.create("RouteLocationMapping/getlocationsarea/", data).then(function (promise) {
                if (promise != null) {
                    if (promise.routedetails.length > 0) {
                        $scope.routedetails = promise.routedetails;
                    }
                    else {
                        swal("No Reocrds Found");
                    }

                }
                else {
                    swal("No Reocrds Found");
                }
            })
            
        }

        //--Sorting--//     
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            
            return (JSON.stringify(obj.routename)).indexOf($scope.searchValue) >= 0 ||
                (JSON.stringify(obj.locationname)).indexOf($scope.searchValue) >= 0
        }


        $scope.deactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.trlfM_ActiveFlag === true) {
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
                        apiService.create("LocationFeeGroupMapping/activedeactive/", user).
                            then(function (promise) {
                                if (promise.message != null) {
                                    swal(promise.message);
                                }
                                else {
                                    if (promise.returnval == true) {
                                        swal(confirmmgs + " " + "Successfully.");
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

        $scope.resetLists = function () {
            $scope.configA = {
                // Changed sorting within list
                onUpdate: function (evt) {
                    var itemEl = evt.item;  // dragged HTMLElement
                    // + indexes from onEnd
                }
            };
        };

        $scope.getOrder = function (getdetails1) {
            var data = {
                "selectedorder": getdetails1
            }
            apiService.create("RouteLocationMapping/getOrder/", data).then(function (promise) {
                if (promise.returnval == true) {
                    swal("Record Updated Successfully");
                }
                else {
                    swal("Failed To Update Record");
                }
            })
        }


        //location wise amount 
        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.savadataamount = function () {

            $scope.submitted1 = true;

            if ($scope.myForm1.$valid) {
            
                var data = {
                    "TRMLAMT_Id": $scope.TRMLAMT_Id,
                    "ASMAY_Id": $scope.ASMAY_Idnew,
                    "TRML_Id": $scope.TRML_Idnew,
                   "TRMLAMT_OneWayAmount":$scope.TRMLAMT_OneWayAmount,
                    "TRMLAMT_TwoWayAmount": $scope.TRMLAMT_TwoWayAmount
                }
                apiService.create("LocationFeeGroupMapping/savedataamount", data).then(function (promise) {
                    if (promise != null) {
                        if (promise.message == "Add") {
                            if (promise.retrval == true) {
                                swal("Record Saved Successfully");
                            }
                            else {
                                swal("Failed To Save Record");
                            }
                        }
                        else if (promise.message == "Update") {
                            if (promise.retrval == true) {
                                swal("Record Update Successfully");
                            }
                            else {
                                swal("Failed To Update Record");
                            }
                        }
                        else if (promise.message == "Duplicate") {
                            swal("Record Already Exists");
                        }
                    }
                    else {

                    }
                   //$state.reload();
                    $scope.close1();
                })
            } else {
                $scope.submitted1 = true;
            }
        }



        $scope.editamount = function (user) {
            var data = {
                "TRMLAMT_Id": user.TRMLAMT_Id
            }
            apiService.create("LocationFeeGroupMapping/geteditdataamount", data).then(function (Promise) {
                if (Promise != null) {
                    debugger;
                    $scope.ASMAY_Idnew = Promise.editdatadetails[0].asmaY_Id;
                    $scope.TRML_Idnew = Promise.editdatadetails[0].trmL_Id;
                 
                    $scope.TRMLAMT_Id = Promise.editdatadetails[0].trmlamT_Id;
                    $scope.TRMLAMT_OneWayAmount = Promise.editdatadetails[0].trmlamT_OneWayAmount;

                    $scope.TRMLAMT_TwoWayAmount = Promise.editdatadetails[0].trmlamT_TwoWayAmount;

           
                }
            })
        }


        $scope.deactiveamount = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.trlfM_ActiveFlag === true) {
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
                        apiService.create("LocationFeeGroupMapping/activedeactiveamount/", user).
                            then(function (promise) {
                                if (promise.message != null) {
                                    swal(promise.message);
                                }
                                else {
                                    if (promise.returnval == true) {
                                        swal(confirmmgs + " " + "Successfully.");
                                        $state.reload();
                                    }
                                    else {
                                        swal(confirmmgs + " " + " Successfully");
                                        $state.reload();
                                    }
                                    $scope.close1();
                                }
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                    $state.reload();
                });
        }
    };
})();


