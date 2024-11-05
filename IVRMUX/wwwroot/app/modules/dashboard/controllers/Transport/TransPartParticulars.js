
(function () {
    'use strict';
    angular
.module('app')
        .controller('TransPartParticularsController', TransPartParticularsController)

    TransPartParticularsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function TransPartParticularsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.masterlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.searchValue = "";
        $scope.TRMV_Id = '';

        $scope.vehicles = [{ id: 'vehicle1' }];
        $scope.addNewVehicle = function () {
            debugger;
            var newItemNo = $scope.vehicles.length + 1;
            if (newItemNo <= 500) {
                $scope.vehicles.push({ 'id': 'vehicle' + newItemNo });
            }
        };
        //removing Vehicle and Driver.
        $scope.delm = [];
        $scope.removeVehicle = function (index) {
            var newItemNo = $scope.vehicles.length - 1;
            if (newItemNo !== 0) {
                $scope.delm = $scope.vehicles.splice(index, 1);
            }
        };
        $scope.additems = false;
        $scope.BindData = function () {
            
            var pageid = 2;
            apiService.getURI("MasterServiceStation/getpartsdata", pageid).then(function (promise) {
                if (promise != null) {

                    $scope.partlist = promise.partlist;
                    $scope.vehicaldetails = promise.vehicaldata;
                    $scope.driverdata = promise.driverdata;
                    $scope.servnamelist = promise.servnamelist;
                    $scope.parttypedropdown = promise.parttypedropdown;

                }
                else {
                    swal("No Records Found")
                }
            })


        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //---Save--//
        $scope.savedata = function () {
            debugger;
            if ($scope.myForm.$valid) {
                var fromdate1 = $scope.TRSE_ServiceDate == null ? "" : $filter('date')($scope.TRSE_ServiceDate, "yyyy-MM-dd");


                var assignedVehicle = $scope.vehicles;
                var newassgn = [];
                angular.forEach(assignedVehicle, function (qqq) {
                    newassgn.push({ id: qqq.id, trseD_ItemsName: qqq.trseD_ItemsName, trseD_Qty: qqq.trseD_Qty, trseD_Remarks: qqq.trseD_Remarks })
                })
                if ($scope.additems == true) {
                    var data = {
                        "TRSE_Id": $scope.TRSE_Id,
                        "TRMV_Id": $scope.TRMV_Id.trmV_Id,
                        "TRMD_Id": $scope.TRMD_Id,
                        "TRMSST_Id": $scope.TRMSST_Id,
                        "TRSE_ServiceDate": fromdate1,
                        "TRSE_ProblemsListed": $scope.TRSE_ProblemsListed,
                        "TRSE_ServiceDetails": $scope.TRSE_ServiceDetails,
                        "TRPAPT_Id": $scope.TRPAPT_Id,
                        "allotteditems": newassgn
                    }
                }
                else {
                    var data = {
                        "TRSE_Id": $scope.TRSE_Id,
                        "TRMV_Id": $scope.TRMV_Id.trmV_Id,
                        "TRMD_Id": $scope.TRMD_Id,
                        "TRMSST_Id": $scope.TRMSST_Id,
                        "TRSE_ServiceDate": fromdate1,
                        "TRSE_ProblemsListed": $scope.TRSE_ProblemsListed,
                        "TRSE_ServiceDetails": $scope.TRSE_ServiceDetails,
                        "TRPAPT_Id": $scope.TRPAPT_Id,
                        //"allotteditems": newassgn
                    }
                }

              
                apiService.create("MasterServiceStation/savepartsdata", data).then(function (promise) {
                    if (promise.returnVal == "Add") {
                        if (promise.retval == true) {
                            swal("Record Saved Successfully");
                        }
                        else {
                            swal("Record Not Saved");
                        }
                    }
                    else if (promise.returnVal == "update") {
                        if (promise.retval == true) {
                            swal("Record Updated Successfully");
                        }
                        else {
                            swal("Record Not Updated");
                        }
                    }
                   
                    else if (promise.returnVal == "duplicate") {
                        swal("Record Already Exists");
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        }
        $scope.requisitionlist = [];
        $scope.requisitionlistold = [];
        $scope.instlist = [];
        $scope.viewreq = function (data) {
            $scope.requisitionlist = [];
            $scope.requisitionlistold = [];
            $scope.instlist = [];
            debugger;
            apiService.create("MasterServiceStation/viewreq", data).then(function (promise) {

                $scope.requisitionlist = promise.requisitionlist;
                $scope.requisitionlistold = promise.requisitionlistold;
                $scope.instlist = promise.instlist;



                $scope.inst_name = $scope.instlist[0].mI_Name;
                $scope.add = $scope.instlist[0].mI_Address1;
                $scope.add1 = $scope.instlist[0].mI_Address2;
                $scope.city = $scope.instlist[0].ivrmmcT_Name;
                $scope.pin = $scope.instlist[0].mI_Pincode;


                $scope.drname = $scope.requisitionlist[0].TRMD_DriverName;
                $scope.srvno = $scope.requisitionlist[0].TRSE_ServiceRefNo;
                $scope.vhno = $scope.requisitionlist[0].TRMV_VehicleNo;
                $scope.details = $scope.requisitionlist[0].TRSE_ProblemsListed;

                if ($scope.requisitionlistold.length>0) {
                    $scope.olddate = new Date($scope.requisitionlistold[0].TRSE_ServiceDate);
                }
                else {
                    $scope.olddate = null

                }

            
                
                })
            
        }



        $scope.rprint = function () {

            var innerContents = document.getElementById("printareaId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/trnRecieptprint.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 500);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();

        }
        //--Sorting--//     
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
       

        //---Edit Data--//
        $scope.viewitems = function (user) {
            //var data = {
            //    "TRPAP_Id": user.trpaP_Id
            //}
            $scope.itemlist = [];
            $scope.srno = user.trsE_ServiceRefNo;
            apiService.getURI("MasterServiceStation/viewitems", user.trsE_Id).then(function (promise) {
                if (promise != null) {

                    if (promise.itemlist.length > 0) {
                        $scope.itemlist = promise.itemlist;

                    }
                    else {

                    }
                }

                //$scope.vehicaldetails = promise.vehicaldata;
                //$scope.driverdata = promise.driverdata;
                //$scope.sessiondetils = promise.sessiondata;


            })
        }


        //---Edit Data--//
        $scope.edit = function (user) {
            //var data = {
            //    "TRPAP_Id": user.trpaP_Id
            //}
            $scope.vehicles = [{ id: 'vehicle1' }];
            apiService.getURI("MasterServiceStation/editpartsdata", user.trsE_Id).then(function (promise) {
                if (promise != null) {
                    debugger;
             $scope.TRMV_Id = promise.editparts[0];
                //  $scope.TRMV_Id = promise.vehicaldetails;
                    $scope.TRMV_Id.trmV_Id = promise.editparts[0].trmV_Id;
                    $scope.TRMD_Id = promise.editparts[0].trmD_Id;
                    $scope.TRMSST_Id = promise.editparts[0].trmssT_Id;
                    $scope.TRSE_ServiceDate = new Date(promise.editparts[0].trsE_ServiceDate);
                    $scope.TRSE_ServiceDetails = promise.editparts[0].trsE_ServiceDetails;
                    $scope.TRSE_ProblemsListed = promise.editparts[0].trsE_ProblemsListed;
                    $scope.TRMSES_Parts = promise.editparts[0].trmseS_Parts;
                    $scope.TRSE_Id = promise.editparts[0].trsE_Id;
                    // $scope.TRPAPT_Id = Promise.editparts[0].trpapT_Id;

                    if (promise.itemlist.length > 0) {
                        $scope.additems = true;
                        $scope.vehicles = promise.itemlist;
                    }
                    else {
                        $scope.additems = false;
                    }
                   

                }
             
                //$scope.vehicaldetails = promise.vehicaldata;
                //$scope.driverdata = promise.driverdata;
                //$scope.sessiondetils = promise.sessiondata;
               
               
            })
        }
        //--Active Deactive--//
        $scope.deactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.trsE_ActiveFlag === true) {
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
                apiService.create("MasterServiceStation/activedeactiveparts/", user).
                then(function (promise) {
                    
                        if (promise.retval == true) {
                            swal(confirmmgs + " " + "Successfully");
                            $state.reload();
                        }
                        else if (promise.retval == false) 
                        {
                            swal(confirmmgs + " " + " Successfully");
                            $state.reload();
                    }
                        else {
                            swal("Error");
                            $state.reload();
                        }
                    
                })
            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
            $state.reload();
        });
        }

        //-Clear-//
        $scope.clearid = function () {
            //$scope.trvD_Id = 0;
            //$scope.trmV_VehicleName = "";
            //$scope.submitted = false;
            //$scope.myForm1.$setPristine();
            //$scope.myForm1.$setUntouched();
            $state.reload();
        };
        //--Sorting--//     
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }


        $scope.searchValue = '';

    };



})();


