
(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeePDCController', fee1)

    fee1.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$stateParams']
    function fee1($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $stateParams) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        //  $scope.sortKey = "fmH_Id";   //set the sortKey to the param passed
        $scope.sortKey = "fmH_Order";   //set the sortKey to the param passed

        $scope.reverse = false;
        $scope.gridrecord = false;

        $scope.feeorder = false;
        $scope.feetext = true;
        $scope.searchValue = "";
        $scope.filterValue = function (obj) {
            if (obj.fmH_ActiveFlag == true) {
                $scope.test = "Active";
            } else if (obj.fmH_ActiveFlag == false) {
                $scope.test = "Deactive";
            }
            return angular.lowercase(obj.fmH_FeeName).indexOf(angular.lowercase($scope.searchValue)) >= 0 || JSON.stringify(obj.fmH_Order).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.fmH_SpecialFeeFlag).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.fmH_PDAFlag).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.fmH_RefundFlag).indexOf($scope.searchValue) >= 0 || angular.lowercase(obj.fmH_Flag).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase($scope.test).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }

        $scope.savedisable = true;

        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.disabled = true;
                if (privlgs[i].ivrmirP_AddFlag == true) {
                    $scope.save = true;
                    $scope.savebtn = true;

                }
                else {
                    $scope.save = false;
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

        $scope.resetLists = function () {
            $scope.configA = {
                // Changed sorting within list
                onUpdate: function (evt) {
                    var itemEl = evt.item;  // dragged HTMLElement
                    // + indexes from onEnd
                }
            };
        };
        $scope.init = function () {

            $scope.resetLists();

        };
        $scope.init();


        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("FeePDC/getalldetails", pageid).
                then(function (promise) {

                    $scope.arrlist6 = promise.academicdrp;
                    $scope.termname = promise.termlist;

                    if (promise.groupHeadData.length > 0 || promise.groupHeadData != null) {
                        $scope.totcountfirst = promise.groupHeadData.length;

                        $scope.students = promise.groupHeadData;
                    }

          

                    $scope.classlst = promise.fillclass;
                    $scope.sectionlst = promise.fillsection;
                    $scope.bankname = promise.fillBank;

                    if ($scope.students.length > 0) {
                        $scope.gridrecord = true;

                    }

                    $scope.students.data = promise.groupHeadData;
                    $scope.studentListOrder = promise.groupHeadData;
                })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

        }

        $scope.onselectclass = function () {


            $scope.ASMS_Id = "";
            $scope.AMST_Id = "";

            if ($scope.ASMAY_Id != "" && $scope.ASMCL_Id != "") {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeAdjustment/getsection", data).
                    then(function (promise) {
                        $scope.grigview1 = false;
                        $scope.grigview2 = false;
                        $scope.sectionlst = promise.fillsection;
                    });
            }
        }
      
        $scope.onselectsection = function () {
            if ($scope.ASMAY_Id !== "") {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeWaivedOff/getstudent", data).
                    then(function (promise) {
                        $scope.gridview1 = false;
                        $scope.AMST_Id = "";
                        $scope.studentlst = promise.fillstudent;

                        if (promise.filldata.length > 0) {

                            $scope.thirdgrid = promise.filldata;
                            $scope.totcountfirst = promise.filldata.length;
                        }
                        else {
                            //swal("No Records Found");
                        }

                    });
            }

        }


        $scope.submitted = false;
        $scope.saveGroupdata = function () {

            if ($scope.myFormhead.$valid) {


                var data = {
                    "FPDC_Id": $scope.FPDC_Id,
                    "FPDC_ChequeDate": new Date($scope.FPDC_ChequeDate).toDateString(),
                    "FPDC_ChequeNo": $scope.FPDC_ChequeNo,
                    "FCSPDC_Amount": $scope.FCSPDC_Amount,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "FMT_Id": $scope.FMT_Id,
                    "FPDC_Currency": $scope.FPDC_Currency,
                    "FPDC_Narration": $scope.FPDC_Narration,
                    "FMBANK_Id": $scope.FMBANK_Id,
                    "AMST_Id": $scope.AMST_Id,


                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("FeePDC/", data).
                    then(function (promise) {
                        if (promise.returnval === true) {
                            $scope.students = promise.groupHeadData;
                            $scope.loaddata();
                            if (promise.returnval === true) {
                                if (promise.message != null) {
                                    swal('Record Updated Successfully', 'success');
                                    $state.reload();
                                }
                                else {
                                    swal('Record Saved Successfully', 'success');
                                    $state.reload();
                                }
                                if ($scope.students.length > 0) {
                                    $scope.gridrecord = true;

                                }


                            }
                            //swal('Record Saved/Updated Successfully');
                        }
                        else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                            swal('Record Already Exist');
                        }
                        else {
                            if (promise.message != null) {
                                swal('Record Not Updated', 'success');
                            }
                            else {
                                swal('Record Not Saved', 'success');
                            }
                        }
                    })
                //$scope.cance();

            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.interacted1 = function (field) {

            return $scope.submitted || field.$dirty;
        };


        $scope.cance = function () {
            $scope.FMT_Id = "",
                $scope.ASMAY_Id = "";
            $scope.FPDC_ChequeDate = "";
            $scope.FPDC_ChequeNo = "";
            $scope.FCSPDC_Amount = "";


        }
        $scope.editEmployee = {}
        $scope.deletedata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.fmH_Id;
            var pageid = $scope.editEmployee;
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.DeleteURI("FeePDC/deletepages", pageid).
                            then(function (promise) {
                                $scope.students = promise.groupHeadData;
                                if (promise.returnval === true) {
                                    swal('Record Deleted Successfully');
                                }
                                else if (promise.dupr === false && promise.returnval != true) {
                                    swal('Record Not Deleted', 'Fee Head Already Mapped Yearly Group');
                                }
                                else {
                                    swal('Record Not Deleted');
                                }
                                $scope.loaddata();
                            })
                        $scope.loaddata();
                    }
                    else {
                        swal("Record Deletion Cancelled");
                        $scope.loaddata();
                    }
                });
        }
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.fpdC_Id;
            var pageid = $scope.editEmployee;
            //apiService.getURI("FeePDC/getdetails", pageid).
            //    then(function (promise) {

                    for (var i = 0; i < $scope.students.length; i++) {

                        if ($scope.students[i].fpdC_Id == $scope.editEmployee) {


                            $scope.FPDC_ChequeDate = new Date($scope.students[i].fpdC_ChequeDate);
                            $scope.FPDC_ChequeNo = $scope.students[i].fpdC_ChequeNo;
                            $scope.FCSPDC_Amount = $scope.students[i].fcspdC_Amount;
                            $scope.ASMAY_Id = $scope.students[i].asmaY_Id;
                            $scope.ASMS_Id = $scope.students[i].asmS_Id;
                            $scope.ASMCL_Id = $scope.students[i].asmcL_Id;
                            $scope.FMT_Id = $scope.students[i].fmT_Id;
                            $scope.FPDC_Currency = $scope.students[i].fpdC_Currency;
                            $scope.FPDC_Narration = $scope.students[i].fpdC_Narration;
                            $scope.FMBANK_Id = $scope.students[i].fmbanK_Id;
                            $scope.onselectsection();
                            $scope.AMST_Id = $scope.students[i].amsT_Id;
                        
                            //$scope.AMST_Id = $scope.students[i].amsT_Id;



                            $scope.FPDC_Id = $scope.students[i].fpdC_Id;

                        }
                    }

              //  })
        }

        $scope.deactive = function (groupHeadData, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {


                        apiService.create("FeePDC/deactivate", groupHeadData).
                            then(function (promise) {

                                if (promise.returnval == true) {
                                    swal("Record Deleted Successfully");
                                }

                                else {
                                    swal("Record Not Deleted Successfully");
                                }

                                $state.reload();
                            })
                    }
                    else {
                        swal("Request Failed");
                    }

                    $state.reload();
                })
        }
    };
})();


