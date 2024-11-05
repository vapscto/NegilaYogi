(function () {
    'use strict';
    angular
.module('app')
        .controller('MasterFeeGroupwiseAutoReceiptController', MasterFeeGroupwiseAutoReceiptController)

    MasterFeeGroupwiseAutoReceiptController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MasterFeeGroupwiseAutoReceiptController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));
        $scope.cfg = {};

        $scope.searchValue = "";
        $scope.showdet = true;
        $scope.showbuttons = true;

        $scope.prefixshow = true;
        $scope.suffixshow = true;

        //==============Get Data
        $scope.onload = function () {
            
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 2;
            var data = {
                "MI_Id": $scope.Id,
            }
            apiService.create("MasterFeeGroupwiseAutoReceipt/getdetails", data).
            then(function (promise) {
                $scope.arrlist6 = promise.acayear;
                $scope.cfg.ASMAY_Id = academicyrlst[0].asmaY_Id;
                $scope.arrlistchk = promise.fillgroup;
                
                if (promise.filldata.length > 0) {
                    $scope.students = promise.filldata;
                    $scope.presentCountgrid = promise.filldata.length;
                }
                else {
                    swal("No Records Found");
                    $scope.presentCountgrid = 0;
                }

            })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        };
        //=====================End=================//

        //===============Delete Data==============//
        $scope.delete = function (det, SweetAlert) {
            var data = {
                "FGARG_Id": det.fgarG_Id
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to delete record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.create("MasterFeeGroupwiseAutoReceipt/delete/", data).
                    then(function (promise) {
                        
                        if (promise.returnvalue == "failed") {
                            swal("Sorry.....You can not delete this record.Because it is already mapped");
                            return;
                        }
                        else if (promise.returnvalue === "deleted") {
                            swal('Record Deleted Successfully');
                            $state.reload();
                        }
                        else {
                            swal('Record Not Deleted');
                        }
                    })
                }
                else {
                    swal("Cancelled");
                }
            });
        }
        //=====================End=================//


        //===========For Edit==============//
        $scope.edit = function (det) {
            
            var data = {
                "FGAR_Id": det.fgaR_Id,
                "FGARG_Id": det.fgarG_Id
            }
            $scope.prefixnme = true;
            $scope.suffixnme = true;
            $scope.academicnme = true;
            //  $scope.Id = det.fgarG_Id;
            apiService.create("MasterFeeGroupwiseAutoReceipt/edit/", data).
            then(function (promise) {
                $scope.ASMAY_Id = promise.filldata[0].asmaY_Id;
                if (promise.filldata[0].fgaR_PrefixFlag == 1) {
                    $scope.FGAR_PrefixFlag = true;
                    $scope.FGAR_PrefixName = promise.filldata[0].fgaR_PrefixName;
                    $scope.predisable = false;
                    $scope.prefixnme = false;
                    $scope.FGAR_Template_Name = promise.filldata[0].fgaR_Template_Name;

                }
                else {
                    $scope.FGAR_PrefixFlag = false;
                    $scope.FGAR_PrefixName = "";
                    $scope.predisable = false;
                    $scope.prefixnme = false;
                }
                if (promise.filldata[0].fgaR_SuffixFlag == 1) {
                    $scope.FGAR_SuffixFlag = true;
                    $scope.FGAR_SuffixName = promise.filldata[0].fgaR_SuffixName;
                    $scope.sufdisable = false;
                    $scope.suffixnme = false;
                }
                else {
                    $scope.FGAR_SuffixFlag = false;
                    $scope.FGAR_SuffixName = "";
                    $scope.sufdisable = false;
                    $scope.suffixnme = false;
                }
                $scope.FGAR_Starting_No = promise.filldata[0].fgaR_Starting_No;
                $scope.FGAR_Name = promise.filldata[0].fgaR_Name;
                $scope.FGAR_Address = promise.filldata[0].fgaR_Address;
                $scope.FMG_Id = promise.filldata[0].fmG_Id;
                $scope.fgaR_Id = promise.filldata[0].fgaR_Id;

            })
        }
        //======================End=====================//


        //==============For Save=========================//
        $scope.submitted = false;
        $scope.save = function (selected, arrlistchk) {
            
            if ($scope.myForm.$valid) {

                if (arrlistchk.length > 0) {
                    $scope.albumNameArray = [];
                    for (var j = 0; j < selected.length; j++) {
                        for (var i = 0; i < arrlistchk.length; i++) {
                            if (selected[j] == arrlistchk[i].fmG_Id) {
                                $scope.albumNameArray.push(arrlistchk[i]);
                            }
                        }
                    }
                    var data = {
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "FGAR_Id": $scope.fgaR_Id,
                        "FGAR_PrefixFlag": $scope.FGAR_PrefixFlag,
                        "FGAR_PrefixName": $scope.FGAR_PrefixName,
                        "FGAR_SuffixFlag": $scope.FGAR_SuffixFlag,
                        "FGAR_SuffixName": $scope.FGAR_SuffixName,
                        "FGAR_Name": $scope.FGAR_Name,
                        "FGAR_Address": $scope.FGAR_Address,
                        "savegroup": $scope.albumNameArray,
                        "FGAR_Starting_No": $scope.FGAR_Starting_No,
                        "FGAR_Template_Name": $scope.FGAR_Template_Name

                    }
                    apiService.create("MasterFeeGroupwiseAutoReceipt/savedata/", data)

                    .then(function (promise) {

                        if (promise.returnvalue == "true") {
                            swal('Record Saved Successfully', "save");
                            $state.reload();
                        }
                        else if (promise.returnvalue == "false") {
                            swal('Record Saving Failed', "Failed");
                            $state.reload();
                        }
                        else if (promise.returnvalue == "updated") {
                            swal('Record Updated Successfully', "Update");
                            $state.reload();
                        }
                        else if (promise.returnvalue == "updatefailed") {
                            swal('Record Update Failed', "Failed");
                            $state.reload();
                        }
                        else if (promise.returnvalue == "duplicate") {
                            swal('Record Already Exists');
                        }
                        else {
                            swal('Operation Failed');
                            $state.reload();
                        }
                    })
                }
                else {
                    swal("Kindly select any one group!!!")
                }

            }
            else {
                $scope.submitted = true;
            }
        };
        //======================End=====================//

        $scope.clearid = function () {
            $state.reload();
        }

        $scope.interacted = function (field) {
            return $scope.submitted
        };

        $scope.sufdisable = true;
        $scope.predisable = true;

        $scope.changeprefix = function (FGAR_PrefixFlag) {
            if (FGAR_PrefixFlag == false) {
                $scope.predisable = true;
                $scope.FGAR_PrefixName = "";
            }
            else if (FGAR_PrefixFlag == true) {
                $scope.predisable = false;
                $scope.FGAR_PrefixName = "";
            }
        };


        $scope.changesuffix = function (FGAR_SuffixFlag) {
            if (FGAR_SuffixFlag == false) {
                $scope.sufdisable = true;
                $scope.FGAR_SuffixName = "";
            }
            else if (FGAR_SuffixFlag == true) {
                $scope.sufdisable = false;
                $scope.FGAR_SuffixName = "";
            }
        };

    }
})();
