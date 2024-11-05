(function () {
    'use strict';
    angular
.module('app')
        .controller('HeadLedgerMappingController', HeadLedgerMappingController)
    HeadLedgerMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function HeadLedgerMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.cfg = {};
        $scope.stuwiseorheadwise = "";
        $scope.formload = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters
            var pageid = 1;
            apiService.create("HeadLedgerMapping/getalldetails", pageid).
                then(function (promise) {

                    $scope.yearlst = promise.academicdrp;

                    $scope.cfg.ASMAY_Id = academicyrlst[0].asmaY_Id;

                    $scope.groupcount = promise.fillmastergroup;
                   

                    if (promise.totaldata.length > 0) {
                        $scope.thirdgrid = promise.totaldata;
                        $scope.totcountfirst = $scope.thirdgrid.length;
                    }
                    else {
                        swal("No Records Found!!!!!");
                    }

                    if (promise.fillconfig.length > 0) {
                        $scope.stuwiseorheadwise = promise.fillconfig[0].fmC_StudentwiseJVFlag
                    } else {
                        $scope.stuwiseorheadwise = "";
                    }

                })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }

        $scope.onselectacademic = function (yearlst) {
            var data = {
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
            }

            apiService.create("HeadLedgerMapping/getgroupdetails", data).
                then(function (promise) {
                    if (promise.fillmastergroup.length > 0) {
                        $scope.groupcount = promise.fillmastergroup;
                    }
                    else {
                        swal("No Records Found!!")
                    }
                })
        }

        $scope.temptermarray = [];
        $scope.onselectgroup = function () {
            $scope.temptermarray = [];
            $scope.totalgrid = [];
            var data = {
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
                "FMG_Id": $scope.FMG_Id
            }
            apiService.create("HeadLedgerMapping/getheaddetails", data).
                then(function (promise) {
                    var headid = 0;
                    if (promise.ftmcoM_companyname != null && promise.ftmcoM_companyname.length > 0) {
                        $scope.feeGroupname = promise.ftmcoM_companyname;
                        if (promise.totaldata.length > 0) {

                            $scope.totalgrid = promise.totaldata;

                            //for (var i = 0; i < promise.fillheaddata.length; i++) {
                            //    headid = promise.fillheaddata[i].fyghM_Id;
                            //    for (var j = 0; j < promise.totaldata.length; j++) {
                            //        if (headid != promise.totaldata[j].fyghM_Id)
                            //            $scope.temptermarray.push(promise.fillheaddata[i]);
                            //    }
                            //}

                            //$scope.totalgrid = promise.totaldata;

                            angular.forEach(promise.savednotsavedlist, function (ty) {
                                $scope.totalgrid.push(ty);
                            });
                        }
                        else {

                            if (promise.fillheaddata.length > 0) {
                                $scope.totalgrid = promise.fillheaddata;
                            }
                            else {
                                swal("No Records Found!!!!!");
                            }
                        }
                    }
                    else {
                        swal("Group wise Company  Name  Not mapping ,First Group name map  !!!!!");
                        $scope.feeGroupname = [];
                        $scope.feeGroupname = promise.ftmcoM_companyname;
                    }
                   

                })
        }

        $scope.cleardata = function () {
            $state.reload();
        }



        $scope.submitted = false;
        $scope.savedata = function (totalgrid) {
            $scope.submitted = true;
            if ($scope.myform.$valid) {
                if ($scope.FTMCOM_Id != null && $scope.FTMCOM_Id > 0) {
                    var data = {
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "FMG_Id": $scope.FMG_Id,
                        "FTMCOM_Id": $scope.FTMCOM_Id,
                        savetmpdata: totalgrid,
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }

                    apiService.create("HeadLedgerMapping/savedata", data).
                        then(function (promise) {

                            if (promise.returnval == "true") {
                                swal("Record Saved Sucessfully");
                            }
                            else {
                                swal("Record Not Saved Sucessfully");
                            }

                            $state.reload();

                        })
                }
                else {
                    swal("Select Company Name");
                }
                
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.DeletRecord = function (FYGHLM_Id) {
            var data = {
                "FYGHLM_Id": FYGHLM_Id,
            }
            apiService.create("HeadLedgerMapping/deletedata", data).
                then(function (promise) {

                    if (promise.returnval == "true") {
                        swal("Record Deleted Sucessfully");
                    }
                    else {
                        swal("Record Not Deleted Sucessfully")
                    }

                    $state.reload();

                })
        }

    }

})();



